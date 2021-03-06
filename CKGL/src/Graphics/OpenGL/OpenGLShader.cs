using System;
using System.Collections.Generic;
using CKGL.OpenGLBindings;
using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL.OpenGL
{
	public class OpenGLShader : Shader
	{
		private static GLuint currentlyBoundShader;

		private GLuint id;
		private Dictionary<string, Uniform> uniforms = new Dictionary<string, Uniform>(StringComparer.Ordinal);

		internal OpenGLShader(string source)
		{
			Compile(source);
		}

		#region Compile
		private void CompileShader(GLuint shaderID, string source)
		{
			// Populate the shader and compile it
			GL.ShaderSource(shaderID, source);
			GL.CompileShader(shaderID);

			// Check for shader compile errors
			GL.GetShader(shaderID, ShaderParam.CompileStatus, out GLint status);
			if (status == 0)
				throw new CKGLException("Shader compile error: " + GL.GetShaderInfoLog(shaderID));
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

			string vertSource = hasGeometryDefinition
								? source.Substring(vertex, geometry - vertex)
								: hasFragmentDefinition
									? source.Substring(vertex, fragment - vertex)
									: source.Substring(vertex);
			string geomSource = hasGeometryDefinition
								? hasFragmentDefinition
									? source.Substring(geometry, fragment - geometry)
									: source.Substring(geometry)
								: "";
			string fragSource = hasFragmentDefinition
								? source.Substring(fragment)
								: "";

			// Debug
			//Output.WriteLine($"\nVertex Shader:\n{vertSource}");
			//Output.WriteLine($"\nGeometry Shader:\n{geomSource}");
			//Output.WriteLine($"\nFragment Shader:\n{fragSource}");

			// Create the shaders and compile them
			GLuint vertID = GL.CreateShader(ShaderType.Vertex);
			//Output.WriteLine(vertSource.Replace("#vertex", ShaderIncludes.Vertex));
			CompileShader(vertID, vertSource.Replace("#vertex", ShaderIncludes.Vertex));

			GLuint geomID = 0;
			if (hasGeometryDefinition)
			{
				geomID = GL.CreateShader(ShaderType.Geometry);
				//Output.WriteLine(geomSource.Replace("#geometry", ShaderIncludes.Geometry));
				CompileShader(geomID, geomSource.Replace("#geometry", ShaderIncludes.Geometry));
			}

			GLuint fragID = 0;
			if (hasFragmentDefinition)
			{
				fragID = GL.CreateShader(ShaderType.Fragment);
				//Output.WriteLine(fragSource.Replace("#fragment", ShaderIncludes.Fragment));
				CompileShader(fragID, fragSource.Replace("#fragment", ShaderIncludes.Fragment));
			}

			// Create the program and attach the shaders to it
			id = GL.CreateProgram();
			GL.AttachShader(id, vertID);
			if (hasGeometryDefinition)
				GL.AttachShader(id, geomID);
			if (hasFragmentDefinition)
				GL.AttachShader(id, fragID);

			// Link the program and check for errors
			GL.LinkProgram(id);
			GL.GetProgram(id, ProgramParam.LinkStatus, out GLint linkStatus);
			if (linkStatus == 0)
				throw new CKGLException("Program link error: " + GL.GetProgramInfoLog(id));

			// Validate the program and check for errors
			GL.ValidateProgram(id);
			GL.GetProgram(id, ProgramParam.ValidateStatus, out GLint validateStatus);
			if (validateStatus == 0)
				throw new CKGLException("Program validate error: " + GL.GetProgramInfoLog(id));

			// Once linked, we can detach and delete the shaders
			GL.DetachShader(id, vertID);
			GL.DeleteShader(vertID);
			if (hasGeometryDefinition)
			{
				GL.DetachShader(id, geomID);
				GL.DeleteShader(geomID);
			}
			if (hasFragmentDefinition)
			{
				GL.DetachShader(id, fragID);
				GL.DeleteShader(fragID);
			}

			// Get all the uniforms the shader has and store their information
			GL.GetProgram(id, ProgramParam.ActiveUniforms, out GLint numUniforms);
			for (int i = 0; i < numUniforms; ++i)
			{
				GL.GetActiveUniform(id, (GLuint)i, out GLint count, out UniformType type, out string name);
				if (count > 0 && name != null)
				{
					if (count > 1)
					{
						name = name.Substring(0, name.LastIndexOf('['));
						string arrName;
						for (int n = 0; n < count; ++n)
						{
							arrName = $"{name}[{n}]";
							int loc = GL.GetUniformLocation(id, arrName);
							//Output.WriteLine($"index:{i} name:{arrName} type:{type} loc:{loc} count:{count}");
							var uniform = new Uniform(i, arrName, type, loc);
							uniforms.Add(arrName, uniform);
						}
					}
					else
					{
						GLint loc = GL.GetUniformLocation(id, name);
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
			if (id != default)
			{
				GL.DeleteProgram(id);
				id = default;
			}
		}

		public override void Bind()
		{
			if (id != currentlyBoundShader)
			{
				Graphics.State.OnStateChanging?.Invoke();
				GL.UseProgram(id);
				Swaps++;
				currentlyBoundShader = id;
				Graphics.State.OnStateChanged?.Invoke();
			}
		}

		#region Uniform
		private class Uniform
		{
			public int Index;
			public string Name;
			public UniformType Type;
			public int Location;

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
			private GLuint GLuintValue;
			//private UniformSampler2D UniformSampler2DValue;
			//private UniformSamplerCube UniformSamplerCubeValue;

			public Uniform(int index, string name, UniformType type, GLint location)
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
					GL.Uniform1i(Location, value ? 1 : 0);
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
					GL.Uniform1i(Location, value);
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
					GL.Uniform1f(Location, value);
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
					GL.Uniform2f(Location, x, y);
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
					GL.Uniform3f(Location, x, y, z);
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
					GL.Uniform4f(Location, x, y, z, w);
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
					GL.Uniform2f(Location, value.X, value.Y);
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
					GL.Uniform3f(Location, value.X, value.Y, value.Z);
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
					GL.Uniform4f(Location, value.X, value.Y, value.Z, value.W);
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
					GL.Uniform4f(Location, value.R, value.G, value.B, value.A);
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
					GL.UniformMatrix3x2fv(Location, 1, false, value.ToArrayColumnMajor());
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
					GL.UniformMatrix3fv(Location, 1, false, value.ToArrayColumnMajor());
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
					GL.UniformMatrix4fv(Location, 1, false, value.ToArrayColumnMajor());
					UniformSwaps++;
					MatrixValue = value;
					Graphics.State.OnStateChanged.Invoke();
				}
			}
			public void SetUniform(Texture value, GLuint textureSlot)
			{
				if ((TextureValue == null || TextureValue != value) && GLuintValue != textureSlot)
				{
					Graphics.State.OnStateChanging.Invoke();
					value.Bind(textureSlot);
					GL.Uniform1i(Location, (GLint)textureSlot);
					UniformSwaps++;
					TextureValue = value;
					GLuintValue = textureSlot;
					Graphics.State.OnStateChanged.Invoke();
				}
			}
			//public void SetUniform(UniformSampler2D value)
			//{
			//	if (UniformSampler2DValue == null || UniformSampler2DValue != value)
			//	{
			//		Graphics.State.OnStateChanging.Invoke();
			//		int slot = Texture.Bind(value.ID, value.BindTarget);
			//		GL.Uniform1i(Location, slot);
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
			//		GL.Uniform1i(Location, slot);
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
		public override void SetUniform(string name, Texture value, GLuint textureSlot)
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
			return $"Shader: [id: {id}]";
		}

		public override bool Equals(object obj)
		{
			return obj is OpenGLShader && Equals((OpenGLShader)obj);
		}
		public override bool Equals(Shader shader)
		{
			return this == (OpenGLShader)shader;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + id.GetHashCode();
				return hash;
			}
		}
		#endregion

		#region Operators
		public static bool operator ==(OpenGLShader a, OpenGLShader b)
		{
			return (a?.id ?? null) == (b?.id ?? null);
		}

		public static bool operator !=(OpenGLShader a, OpenGLShader b)
		{
			return (a?.id ?? null) != (b?.id ?? null);
		}
		#endregion
	}
}