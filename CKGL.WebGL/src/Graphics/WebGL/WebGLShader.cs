using System;
using System.Collections.Generic;
using static CKGL.WebGL.WebGLGraphics; // WebGL Context Methods
using static Retyped.dom; // DOM / WebGL Types

namespace CKGL.WebGL
{
	public class WebGLShader : Shader
	{
		private static WebGLProgram currentlyBoundShader;

		private WebGLProgram shader;
		private Dictionary<string, Uniform> uniforms = new Dictionary<string, Uniform>(StringComparer.Ordinal);

		internal WebGLShader(string source)
		{
			Compile(source);
		}

		#region Convert
		private static string ConvertVertex(string source)
		{
			source = source.Replace("#version 300 es", "");

			for (int i = 0; i < 32; i++)
				source = source.Replace($"layout(location = {i}) ", "");

			source = source.Replace("in ", "attribute ");

			source = source.Replace("out ", "varying ");

			return source;
		}

		//private static string ConvertGeometry(string source)
		//{
		//	return source;
		//}

		private static string ConvertFragment(string source)
		{
			source = source.Replace("#version 300 es", "");

			source = source.Replace("in ", "varying ");

			source = source.Replace("layout(location = 0) out vec4 colour;", "");
			source = source.Replace("colour = vColour;", "gl_FragColor = vColour;");

			return source;
		}
		#endregion

		#region Compile
		private void CompileShader(Retyped.dom.WebGLShader shader, string source)
		{
			// Populate the shader and compile it
			GL.shaderSource(shader, source);
			GL.compileShader(shader);

			// Check for shader compile errors
			bool status = (bool)GL.getShaderParameter(shader, GL.COMPILE_STATUS);
			if (!status)
				throw new CKGLException("Shader compile error: " + GL.getShaderInfoLog(shader));
		}

		private void Compile(string source)
		{
			int vertex = source.IndexOf("#vertex", StringComparison.Ordinal);
			int geometry = source.IndexOf("#geometry", StringComparison.Ordinal);
			int fragment = source.IndexOf("#fragment", StringComparison.Ordinal);

			if (vertex == -1)
				throw new CKGLException("Shader source must contain a vertex shader definition.");

			bool hasGeometryDefinition = geometry != -1;
			bool hasFragmentDefinition = fragment != -1;

			if (geometry != -1)
				throw new CKGLException("WebGL 2.0 does not support Geometry Shaders.");

			string vertSource = hasGeometryDefinition
								? source.Substring(vertex, geometry - vertex)
								: hasFragmentDefinition
									? source.Substring(vertex, fragment - vertex)
									: source.Substring(vertex);
			//string geomSource = hasGeometryDefinition
			//					? hasFragmentDefinition
			//						? source.Substring(geometry, fragment - geometry)
			//						: source.Substring(geometry)
			//					: "";
			string fragSource = hasFragmentDefinition
								? source.Substring(fragment)
								: "";

			// Debug
			//Output.WriteLine($"\nVertex Shader:\n{vertSource}");
			//Output.WriteLine($"\nGeometry Shader:\n{geomSource}");
			//Output.WriteLine($"\nFragment Shader:\n{fragSource}");

			// Create the shaders and compile them
			Retyped.dom.WebGLShader vertID = GL.createShader(GL.VERTEX_SHADER);
			//Output.WriteLine(vertSource.Replace("#vertex", ShaderIncludes.Vertex));
			CompileShader(vertID, ConvertVertex(vertSource.Replace("#vertex", ShaderIncludes.Vertex)));

			//Retyped.dom.WebGLShader geomID = null;
			//if (hasGeometryDefinition)
			//{
			//	geomID = GL.createShader(GL.GEOMETRY_SHADER);
			//	//Output.WriteLine(geomSource.Replace("#geometry", ShaderIncludes.Geometry));
			//	CompileShader(geomID, ConvertGeometry(geomSource.Replace("#geometry", ShaderIncludes.Geometry)));
			//}

			Retyped.dom.WebGLShader fragID = null;
			if (hasFragmentDefinition)
			{
				fragID = GL.createShader(GL.FRAGMENT_SHADER);
				//Output.WriteLine(fragSource.Replace("#fragment", ShaderIncludes.Fragment));
				CompileShader(fragID, ConvertFragment(fragSource.Replace("#fragment", ShaderIncludes.Fragment)));
			}

			// Create the program and attach the shaders to it
			shader = GL.createProgram();
			GL.attachShader(shader, vertID);
			//if (hasGeometryDefinition)
			//	GL.attachShader(shader, geomID);
			if (hasFragmentDefinition)
				GL.attachShader(shader, fragID);

			// WebGL 1.0 only - Automatically bind attributes based on layout qualifiers
			try
			{
				string[] lines = vertSource.Split('\n');

				for (int i = 0; i < lines.Length; i++)
				{
					if (lines[i].Contains("layout(location") && (lines[i].Contains("in ") || lines[i].Contains("attribute ")))
					{
						string[] line = lines[i].Replace("layout(location =", "").Replace("layout(location=", "").Replace(";", " ").Trim().Split(' ');
						int attribID = int.Parse(line[0].Substring(0, line[0].Length - 1).Trim());
						string name = line[3].Trim();
						//Output.WriteLine($"Shader - Bind Attribute | id: {attribID}, {name} ({line[2]})"); // Debug
						GL.bindAttribLocation(shader, attribID, name);
					}
				}
			}
			catch (Exception e)
			{
				Output.WriteLine($"WebGL 1.0 - Automatic Shader Attribute binding failed: {e.Message}");
			}

			// Link the program and check for errors
			GL.linkProgram(shader);
			bool linkStatus = (bool)GL.getProgramParameter(shader, GL.LINK_STATUS);
			if (!linkStatus)
				throw new CKGLException("Program link error: " + GL.getProgramInfoLog(shader));

			// Validate the program and check for errors
			GL.validateProgram(shader);
			bool validateStatus = (bool)GL.getProgramParameter(shader, GL.VALIDATE_STATUS);
			if (!validateStatus)
				throw new CKGLException("Program validate error: " + GL.getProgramInfoLog(shader));

			// Once linked, we can detach and delete the shaders
			GL.detachShader(shader, vertID);
			GL.deleteShader(vertID);
			//if (hasGeometryDefinition)
			//{
			//	GL.detachShader(shader, geomID);
			//	GL.deleteShader(geomID);
			//}
			if (hasFragmentDefinition)
			{
				GL.detachShader(shader, fragID);
				GL.deleteShader(fragID);
			}

			// Get all the uniforms the shader has and store their information
			int numUniforms = (int)GL.getProgramParameter(shader, GL.ACTIVE_UNIFORMS);
			for (int i = 0; i < numUniforms; ++i)
			{
				var uniformInfo = GL.getActiveUniform(shader, i);
				string name = uniformInfo.name;
				uint type = uniformInfo.type;
				int count = uniformInfo.size;
				if (count > 0 && name != null)
				{
					if (count > 1)
					{
						name = name.Substring(0, name.LastIndexOf('['));
						string arrName;
						for (int n = 0; n < count; ++n)
						{
							arrName = $"{name}[{n}]";
							WebGLUniformLocation loc = GL.getUniformLocation(shader, arrName);
							//Output.WriteLine($"index:{i} name:{arrName} type:{type} loc:{loc} count:{count}");
							var uniform = new Uniform(i, arrName, type, loc);
							uniforms.Add(arrName, uniform);
						}
					}
					else
					{
						WebGLUniformLocation loc = GL.getUniformLocation(shader, name);
						//Output.WriteLine($"index:{i} name:{name} type:{type} loc:{loc} count:{count}");
						var uniform = new Uniform(i, name, type, loc);
						uniforms.Add(name, uniform);
					}
				}
			}
		}
		#endregion

		public override void Destroy()
		{
			if (shader != null)
			{
				GL.deleteProgram(shader);
				shader = null;
			}
		}

		public override void Bind()
		{
			if (shader != currentlyBoundShader)
			{
				Graphics.State.OnStateChanging?.Invoke();
				GL.useProgram(shader);
				Swaps++;
				currentlyBoundShader = shader;
				Graphics.State.OnStateChanged?.Invoke();
			}
		}

		#region Uniform
		private class Uniform
		{
			public int Index;
			public string Name;
			public uint Type;
			public WebGLUniformLocation Location;

			private bool boolValue;
			private int intValue;
			private float floatValue;
			private float floatValueX;
			private float floatValueY;
			private float floatValueZ;
			private float floatValueW;
			private Vector2 Vector2Value;
			private Vector3 Vector3Value;
			private Vector4 Vector4Value;
			private Colour ColourValue;
			private Matrix2D Matrix2DValue;
			private Matrix3x3 Matrix3x3Value;
			private Matrix MatrixValue;
			private Texture TextureValue;
			private uint uintValue;
			//private UniformSampler2D UniformSampler2DValue;
			//private UniformSamplerCube UniformSamplerCubeValue;

			public Uniform(int index, string name, uint type, WebGLUniformLocation location)
			{
				Index = index;
				Name = name;
				Type = type;
				Location = location;
			}

			#region GL Set Calls
			public void SetUniform(bool value)
			{
				if (boolValue != value)
				{
					Graphics.State.OnStateChanging.Invoke();
					GL.uniform1i(Location, value ? 1 : 0);
					UniformSwaps++;
					boolValue = value;
					Graphics.State.OnStateChanged.Invoke();
				}
			}
			public void SetUniform(int value)
			{
				if (intValue != value)
				{
					Graphics.State.OnStateChanging.Invoke();
					GL.uniform1i(Location, value);
					UniformSwaps++;
					intValue = value;
					Graphics.State.OnStateChanged.Invoke();
				}
			}
			public void SetUniform(float value)
			{
				if (floatValue != value)
				{
					Graphics.State.OnStateChanging.Invoke();
					GL.uniform1f(Location, value);
					UniformSwaps++;
					floatValue = value;
					Graphics.State.OnStateChanged.Invoke();
				}
			}
			public void SetUniform(float x, float y)
			{
				if (floatValueX != x && floatValueY != y)
				{
					Graphics.State.OnStateChanging.Invoke();
					GL.uniform2f(Location, x, y);
					UniformSwaps++;
					floatValueX = x;
					floatValueY = y;
					Graphics.State.OnStateChanged.Invoke();
				}
			}
			public void SetUniform(float x, float y, float z)
			{
				if (floatValueX != x && floatValueY != y && floatValueZ != z)
				{
					Graphics.State.OnStateChanging.Invoke();
					GL.uniform3f(Location, x, y, z);
					UniformSwaps++;
					floatValueX = x;
					floatValueY = y;
					floatValueZ = z;
					Graphics.State.OnStateChanged.Invoke();
				}
			}
			public void SetUniform(float x, float y, float z, float w)
			{
				if (floatValueX != x && floatValueY != y && floatValueZ != z && floatValueW != w)
				{
					Graphics.State.OnStateChanging.Invoke();
					GL.uniform4f(Location, x, y, z, w);
					UniformSwaps++;
					floatValueX = x;
					floatValueY = y;
					floatValueZ = z;
					floatValueW = w;
					Graphics.State.OnStateChanged.Invoke();
				}
			}
			public void SetUniform(Vector2 value)
			{
				if (Vector2Value != value)
				{
					Graphics.State.OnStateChanging.Invoke();
					GL.uniform2f(Location, value.X, value.Y);
					UniformSwaps++;
					Vector2Value = value;
					Graphics.State.OnStateChanged.Invoke();
				}
			}
			public void SetUniform(Vector3 value)
			{
				if (Vector3Value != value)
				{
					Graphics.State.OnStateChanging.Invoke();
					GL.uniform3f(Location, value.X, value.Y, value.Z);
					UniformSwaps++;
					Vector3Value = value;
					Graphics.State.OnStateChanged.Invoke();
				}
			}
			public void SetUniform(Vector4 value)
			{
				if (Vector4Value != value)
				{
					Graphics.State.OnStateChanging.Invoke();
					GL.uniform4f(Location, value.X, value.Y, value.Z, value.W);
					UniformSwaps++;
					Vector4Value = value;
					Graphics.State.OnStateChanged.Invoke();
				}
			}
			public void SetUniform(Colour value)
			{
				if (ColourValue != value)
				{
					Graphics.State.OnStateChanging.Invoke();
					GL.uniform4f(Location, value.R, value.G, value.B, value.A);
					UniformSwaps++;
					ColourValue = value;
					Graphics.State.OnStateChanged.Invoke();
				}
			}
			public void SetUniform(Matrix2D value)
			{
				if (Matrix2DValue != value)
				{
					Graphics.State.OnStateChanging.Invoke();
					GL.uniformMatrix3fv(Location, false, value.ToArrayColumnMajor3x3());
					UniformSwaps++;
					Matrix2DValue = value;
					Graphics.State.OnStateChanged.Invoke();
				}
			}
			public void SetUniform(Matrix3x3 value)
			{
				if (Matrix3x3Value != value)
				{
					Graphics.State.OnStateChanging.Invoke();
					GL.uniformMatrix3fv(Location, false, value.ToArrayColumnMajor());
					UniformSwaps++;
					Matrix3x3Value = value;
					Graphics.State.OnStateChanged.Invoke();
				}
			}
			public void SetUniform(Matrix value)
			{
				if (MatrixValue != value)
				{
					Graphics.State.OnStateChanging.Invoke();
					GL.uniformMatrix4fv(Location, false, value.ToArrayColumnMajor());
					UniformSwaps++;
					MatrixValue = value;
					Graphics.State.OnStateChanged.Invoke();
				}
			}
			public void SetUniform(Texture value, uint textureSlot)
			{
				if ((TextureValue == null || TextureValue != value) && uintValue != textureSlot)
				{
					Graphics.State.OnStateChanging.Invoke();
					value.Bind(textureSlot);
					GL.uniform1i(Location, (int)textureSlot);
					UniformSwaps++;
					TextureValue = value;
					uintValue = textureSlot;
					Graphics.State.OnStateChanged.Invoke();
				}
			}
			//public void SetUniform(UniformSampler2D value)
			//{
			//	if (UniformSampler2DValue == null || UniformSampler2DValue != value)
			//	{
			//		Graphics.State.OnStateChanging.Invoke();
			//		int slot = Texture.Bind(value.ID, value.BindTarget);
			//		GL.uniform1i(Location, slot);
			//		UniformSwaps++;
			//		UniformSampler2DValue = value;
			//		Graphics.State.OnStateChanged.Invoke();
			//	}
			//}
			//public void SetUniform(UniformSamplerCube value)
			//{
			//	if (UniformSamplerCubeValue == null || UniformSamplerCubeValue != value)
			//	{
			//		Graphics.State.OnStateChanging.Invoke();
			//		int slot = Texture.Bind(value.ID, TextureTarget.TextureCubeMap);
			//		GL.uniform1i(Location, slot);
			//		UniformSwaps++;
			//		UniformSamplerCubeValue = value;
			//		Graphics.State.OnStateChanged.Invoke();
			//	}
			//}
			#endregion
		}
		#endregion

		#region Uniform Methods
		private Uniform GetUniform(string name)
		{
			if (uniforms.TryGetValue(name, out Uniform uniform))
				return uniform;
			else
				throw new CKGLException($"No uniform with name: {name}");
		}

		public override void SetUniform(string name, bool value)
		{
			Bind();
			GetUniform(name).SetUniform(value);
		}
		public override void SetUniform(string name, int value)
		{
			Bind();
			GetUniform(name).SetUniform(value);
		}
		public override void SetUniform(string name, float value)
		{
			Bind();
			GetUniform(name).SetUniform(value);
		}
		public override void SetUniform(string name, float x, float y)
		{
			Bind();
			GetUniform(name).SetUniform(x, y);
		}
		public override void SetUniform(string name, float x, float y, float z)
		{
			Bind();
			GetUniform(name).SetUniform(x, y, z);
		}
		public override void SetUniform(string name, float x, float y, float z, float w)
		{
			Bind();
			GetUniform(name).SetUniform(x, y, z, w);
		}
		public override void SetUniform(string name, Vector2 value)
		{
			Bind();
			GetUniform(name).SetUniform(value);
		}
		public override void SetUniform(string name, Vector3 value)
		{
			Bind();
			GetUniform(name).SetUniform(value);
		}
		public override void SetUniform(string name, Vector4 value)
		{
			Bind();
			GetUniform(name).SetUniform(value);
		}
		public override void SetUniform(string name, Colour value)
		{
			Bind();
			GetUniform(name).SetUniform(value);
		}
		public override void SetUniform(string name, Matrix2D value)
		{
			Bind();
			GetUniform(name).SetUniform(value);
		}
		public override void SetUniform(string name, Matrix3x3 value)
		{
			Bind();
			GetUniform(name).SetUniform(value);
		}
		public override void SetUniform(string name, Matrix value)
		{
			Bind();
			GetUniform(name).SetUniform(value);
		}
		public override void SetUniform(string name, Texture value, uint textureSlot)
		{
			Bind();
			GetUniform(name).SetUniform(value, textureSlot);
		}
		//public override void SetUniform(string name, UniformSampler2D value)
		//{
		//	Bind();
		//	GetUniform(name).SetUniform(value);
		//}
		//public override void SetUniform(string name, UniformSamplerCube value)
		//{
		//	Bind();
		//	GetUniform(name).SetUniform(value);
		//}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"Shader: [id: {shader}]";
		}

		public override bool Equals(object obj)
		{
			return obj is WebGLShader && Equals((WebGLShader)obj);
		}
		public override bool Equals(Shader shader)
		{
			return this == (WebGLShader)shader;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + shader.GetHashCode();
				return hash;
			}
		}
		#endregion

		#region Operators
		public static bool operator ==(WebGLShader a, WebGLShader b)
		{
			return (a?.shader ?? null) == (b?.shader ?? null);
		}

		public static bool operator !=(WebGLShader a, WebGLShader b)
		{
			return (a?.shader ?? null) != (b?.shader ?? null);
		}
		#endregion
	}
}