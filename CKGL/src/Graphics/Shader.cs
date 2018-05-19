using System;
using System.Collections.Generic;
using System.IO;
using OpenGL;
using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL
{
	public class Shader
	{
		public static Action OnBinding;
		public static Action OnBound;
		public static Action OnUniformChanging;
		public static Action OnUniformChanged;

		private static GLuint currentlyBoundShader;

		public static int Swaps { get; private set; }
		public static int UniformSwaps { get; private set; }

		private GLuint id;
		private Dictionary<string, Uniform> uniforms = new Dictionary<string, Uniform>(StringComparer.Ordinal);

		public Shader(ref string source)
		{
			Compile(ref source);
		}
		public Shader(string source)
		{
			Compile(ref source);
		}
		public static Shader FromFile(string file)
		{
			if (!File.Exists(file))
				throw new FileNotFoundException("Shader file not found.", file);
			return new Shader(File.ReadAllText(file));
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
				throw new Exception("Shader compile error: " + GL.GetShaderInfoLog(shaderID));
		}

		private void Compile(ref string source)
		{
			int vertex = source.IndexOf("#vertex", StringComparison.Ordinal);
			int fragment = source.IndexOf("#fragment", StringComparison.Ordinal);

			if (vertex == -1)
				throw new Exception("Shader source must contain a vertex shader definition.");

			bool hasFragmentDefinition = fragment != -1;

			string vertSource = hasFragmentDefinition ? source.Substring(vertex, fragment - vertex) : source.Substring(vertex);
			string fragSource = hasFragmentDefinition ? source.Substring(fragment) : "";

			// Create the shaders and compile them
			GLuint fragID = 0;
			GLuint vertID = GL.CreateShader(ShaderType.Vertex);
			//Output.WriteLine(vertSource.Replace("#vertex", ShaderIncludes.Vertex));
			CompileShader(vertID, vertSource.Replace("#vertex", ShaderIncludes.Vertex));
			if (hasFragmentDefinition)
			{
				fragID = GL.CreateShader(ShaderType.Fragment);
				//Output.WriteLine(fragSource.Replace("#fragment", ShaderIncludes.Fragment));
				CompileShader(fragID, fragSource.Replace("#fragment", ShaderIncludes.Fragment));
			}

			// Create the program and attach the shaders to it
			id = GL.CreateProgram();
			GL.AttachShader(id, vertID);
			if (hasFragmentDefinition)
				GL.AttachShader(id, fragID);

			// Link the program and check for errors
			GL.LinkProgram(id);
			GL.GetProgram(id, ProgramParam.LinkStatus, out GLint status);
			if (status == 0)
				throw new Exception("Program link error: " + GL.GetProgramInfoLog(id));

			// Once linked, we can detach and delete the shaders
			GL.DetachShader(id, vertID);
			GL.DeleteShader(vertID);
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
							//Output.WriteLine("index:{0} name:{1} type:{2} loc:{3} count:{4}", i, arrName, type, loc, count);
							var uniform = new Uniform(i, arrName, type, loc);
							uniforms.Add(arrName, uniform);
						}
					}
					else
					{
						GLint loc = GL.GetUniformLocation(id, name);
						//Output.WriteLine("index:{0} name:{1} type:{2} loc:{3} count:{4}", i, name, type, loc, count);
						var uniform = new Uniform(i, name, type, loc);
						uniforms.Add(name, uniform);
					}
				}
			}
		}
		#endregion

		public static void PreDraw()
		{
			Swaps = 0;
			UniformSwaps = 0;
		}

		public void Destroy()
		{
			if (id != default(GLuint))
			{
				GL.DeleteProgram(id);
				id = default(GLuint);
			}
		}

		public void Bind()
		{
			if (id != currentlyBoundShader)
			{
				OnBinding?.Invoke();
				GL.UseProgram(id);
				Swaps++;
				currentlyBoundShader = id;
				OnBound?.Invoke();
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
					OnUniformChanging.Invoke();
					GL.Uniform1I(Location, value ? 1 : 0);
					UniformSwaps++;
					boolValue = value;
					OnUniformChanged.Invoke();
				}
			}
			public void SetUniform(int value)
			{
				if (intValue != value)
				{
					OnUniformChanging.Invoke();
					GL.Uniform1I(Location, value);
					UniformSwaps++;
					intValue = value;
					OnUniformChanged.Invoke();
				}
			}
			public void SetUniform(float value)
			{
				if (floatValue != value)
				{
					OnUniformChanging.Invoke();
					GL.Uniform1F(Location, value);
					UniformSwaps++;
					floatValue = value;
					OnUniformChanged.Invoke();
				}
			}
			public void SetUniform(float x, float y)
			{
				if (floatValueX != x && floatValueY != y)
				{
					OnUniformChanging.Invoke();
					GL.Uniform2F(Location, x, y);
					UniformSwaps++;
					floatValueX = x;
					floatValueY = y;
					OnUniformChanged.Invoke();
				}
			}
			public void SetUniform(float x, float y, float z)
			{
				if (floatValueX != x && floatValueY != y && floatValueZ != z)
				{
					OnUniformChanging.Invoke();
					GL.Uniform3F(Location, x, y, z);
					UniformSwaps++;
					floatValueX = x;
					floatValueY = y;
					floatValueZ = z;
					OnUniformChanged.Invoke();
				}
			}
			public void SetUniform(float x, float y, float z, float w)
			{
				if (floatValueX != x && floatValueY != y && floatValueZ != z && floatValueW != w)
				{
					OnUniformChanging.Invoke();
					GL.Uniform4F(Location, x, y, z, w);
					UniformSwaps++;
					floatValueX = x;
					floatValueY = y;
					floatValueZ = z;
					floatValueW = w;
					OnUniformChanged.Invoke();
				}
			}
			public void SetUniform(Vector2 value)
			{
				if (Vector2Value != value)
				{
					OnUniformChanging.Invoke();
					GL.Uniform2F(Location, value.X, value.Y);
					UniformSwaps++;
					Vector2Value = value;
					OnUniformChanged.Invoke();
				}
			}
			public void SetUniform(Vector3 value)
			{
				if (Vector3Value != value)
				{
					OnUniformChanging.Invoke();
					GL.Uniform3F(Location, value.X, value.Y, value.Z);
					UniformSwaps++;
					Vector3Value = value;
					OnUniformChanged.Invoke();
				}
			}
			public void SetUniform(Vector4 value)
			{
				if (Vector4Value != value)
				{
					OnUniformChanging.Invoke();
					GL.Uniform4F(Location, value.X, value.Y, value.Z, value.W);
					UniformSwaps++;
					Vector4Value = value;
					OnUniformChanged.Invoke();
				}
			}
			public void SetUniform(Colour value)
			{
				if (ColourValue != value)
				{
					OnUniformChanging.Invoke();
					GL.Uniform4F(Location, value.R, value.G, value.B, value.A);
					UniformSwaps++;
					ColourValue = value;
					OnUniformChanged.Invoke();
				}
			}
			public void SetUniform(Matrix2D value)
			{
				if (Matrix2DValue != value)
				{
					OnUniformChanging.Invoke();
					GL.UniformMatrix3x2FV(Location, 1, false, value.ToArrayColumnMajor());
					UniformSwaps++;
					Matrix2DValue = value;
					OnUniformChanged.Invoke();
				}
			}
			public void SetUniform(Matrix value)
			{
				if (MatrixValue != value)
				{
					OnUniformChanging.Invoke();
					GL.UniformMatrix4FV(Location, 1, false, value.ToArrayColumnMajor());
					UniformSwaps++;
					MatrixValue = value;
					OnUniformChanged.Invoke();
				}
			}
			public void SetUniform(Texture value, GLuint textureSlot)
			{
				if ((TextureValue == null || TextureValue != value) && GLuintValue != textureSlot)
				{
					OnUniformChanging.Invoke();
					value.Bind(textureSlot);
					GL.Uniform1I(Location, (GLint)textureSlot);
					UniformSwaps++;
					TextureValue = value;
					GLuintValue = textureSlot;
					OnUniformChanged.Invoke();
				}
			}
			//public void SetUniform(UniformSampler2D value)
			//{
			//	if (UniformSampler2DValue == null || UniformSampler2DValue != value)
			//	{
			//		OnUniformChanging.Invoke();
			//		int slot = Texture.Bind(value.ID, value.BindTarget);
			//		GL.Uniform1I(Location, slot);
			//		UniformSwaps++;
			//		UniformSampler2DValue = value;
			//		OnUniformChanged.Invoke();
			//	}
			//}
			//public void SetUniform(UniformSamplerCube value)
			//{
			//	if (UniformSamplerCubeValue == null || UniformSamplerCubeValue != value)
			//	{
			//		OnUniformChanging.Invoke();
			//		int slot = Texture.Bind(value.ID, TextureTarget.TextureCubeMap);
			//		GL.Uniform1I(Location, slot);
			//		UniformSwaps++;
			//		UniformSamplerCubeValue = value;
			//		OnUniformChanged.Invoke();
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
				throw new Exception($"No uniform with name: {name}");
		}

		public void SetUniform(string name, bool value)
		{
			Bind();
			GetUniform(name).SetUniform(value);
		}
		public void SetUniform(string name, int value)
		{
			Bind();
			GetUniform(name).SetUniform(value);
		}
		public void SetUniform(string name, float value)
		{
			Bind();
			GetUniform(name).SetUniform(value);
		}
		public void SetUniform(string name, float x, float y)
		{
			Bind();
			GetUniform(name).SetUniform(x, y);
		}
		public void SetUniform(string name, float x, float y, float z)
		{
			Bind();
			GetUniform(name).SetUniform(x, y, z);
		}
		public void SetUniform(string name, float x, float y, float z, float w)
		{
			Bind();
			GetUniform(name).SetUniform(x, y, z, w);
		}
		public void SetUniform(string name, Vector2 value)
		{
			Bind();
			GetUniform(name).SetUniform(value);
		}
		public void SetUniform(string name, Vector3 value)
		{
			Bind();
			GetUniform(name).SetUniform(value);
		}
		public void SetUniform(string name, Vector4 value)
		{
			Bind();
			GetUniform(name).SetUniform(value);
		}
		public void SetUniform(string name, Colour value)
		{
			Bind();
			GetUniform(name).SetUniform(value);
		}
		public void SetUniform(string name, Matrix2D value)
		{
			Bind();
			GetUniform(name).SetUniform(value);
		}
		public void SetUniform(string name, Matrix value)
		{
			Bind();
			GetUniform(name).SetUniform(value);
		}
		public void SetUniform(string name, Texture value, GLuint textureSlot)
		{
			Bind();
			GetUniform(name).SetUniform(value, textureSlot);
		}
		//public void SetUniform(string name, UniformSampler2D value)
		//{
		//	Bind();
		//	GetUniform(name).SetUniform(value);
		//}
		//public void SetUniform(string name, UniformSamplerCube value)
		//{
		//	Bind();
		//	GetUniform(name).SetUniform(value);
		//}
		#endregion

		#region Operators
		public static bool operator ==(Shader a, Shader b)
		{
			return a.id == b.id;
		}
		public static bool operator !=(Shader a, Shader b)
		{
			return a.id != b.id;
		}
		#endregion
	}
}