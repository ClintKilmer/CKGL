using System;
using System.IO;
using System.Collections.Generic;

using OpenGL;

using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL
{
	public class Shader
	{
		public static Action OnBinding;
		public static Action OnBound;

		private static GLuint currentlyBoundShader;

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
		public Shader(ref string vertSource, ref string fragSource)
		{
			Compile(ref vertSource, ref fragSource);
		}
		public Shader(string vertSource, string fragSource)
		{
			Compile(ref vertSource, ref fragSource);
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
			//Populate the shader and compile it
			GL.ShaderSource(shaderID, source);
			GL.CompileShader(shaderID);

			//Check for shader compile errors
			GL.GetShader(shaderID, ShaderParam.CompileStatus, out GLint status);
			if (status == 0)
				throw new Exception("Shader compile error: " + GL.GetShaderInfoLog(shaderID));
		}

		private void Compile(ref string source)
		{
			int i = source.IndexOf("...", StringComparison.Ordinal);
			if (i < 0)
				throw new Exception("Shader source text must separate vertex and fragment shaders with \"...\"");

			string vertSource = source.Substring(0, i);
			string fragSource = source.Substring(i + 3);

			Compile(ref vertSource, ref fragSource);
		}
		private void Compile(ref string vertSource, ref string fragSource)
		{
			//Create the shaders and compile them
			GLuint vertID = GL.CreateShader(ShaderType.Vertex);
			GLuint fragID = GL.CreateShader(ShaderType.Fragment);
			CompileShader(vertID, vertSource);
			CompileShader(fragID, fragSource);

			//Create the program and attach the shaders to it
			id = GL.CreateProgram();
			GL.AttachShader(id, vertID);
			GL.AttachShader(id, fragID);

			//Link the program and check for errors
			GL.LinkProgram(id);
			GL.GetProgram(id, ProgramParam.LinkStatus, out GLint status);
			if (status == 0)
				throw new Exception("Program link error: " + GL.GetProgramInfoLog(id));

			//Once linked, we can detach and delete the shaders
			GL.DetachShader(id, vertID);
			GL.DetachShader(id, fragID);
			GL.DeleteShader(vertID);
			GL.DeleteShader(fragID);

			//Get all the uniforms the shader has and store their information
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
				currentlyBoundShader = id;
				OnBound?.Invoke();
			}
		}

		#region Uniform
		private struct Uniform
		{
			public int Index;
			public string Name;
			public UniformType Type;
			public int Location;

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
				GL.Uniform1I(Location, value ? 1 : 0);
			}
			public void SetUniform(int value)
			{
				GL.Uniform1I(Location, value);
			}
			public void SetUniform(float value)
			{
				GL.Uniform1F(Location, value);
			}
			public void SetUniform(float x, float y)
			{
				GL.Uniform2F(Location, x, y);
			}
			public void SetUniform(float x, float y, float z)
			{
				GL.Uniform3F(Location, x, y, z);
			}
			public void SetUniform(float x, float y, float z, float w)
			{
				GL.Uniform4F(Location, x, y, z, w);
			}
			public void SetUniform(Vector2 value)
			{
				GL.Uniform2F(Location, value.X, value.Y);
			}
			public void SetUniform(Vector3 value)
			{
				GL.Uniform3F(Location, value.X, value.Y, value.Z);
			}
			public void SetUniform(Vector4 value)
			{
				GL.Uniform4F(Location, value.X, value.Y, value.Z, value.W);
			}
			public void SetUniform(Matrix2D value)
			{
				GL.UniformMatrix3x2FV(Location, 1, false, value.ToArrayColumnMajor());
			}
			public void SetUniform(Matrix value)
			{
				GL.UniformMatrix4FV(Location, 1, false, value.ToArrayColumnMajor());
			}
			public void SetUniform(Texture value, GLuint textureSlot)
			{
				value.Bind(textureSlot);
				GL.Uniform1I(Location, (GLint)textureSlot);
			}
			//public void SetUniform(UniformSampler2D value)
			//{
			//	int slot = Texture.Bind(value.ID, value.BindTarget);
			//	GL.Uniform1I(Location, slot);
			//}
			//public void SetUniform(UniformSamplerCube value)
			//{
			//	int slot = Texture.Bind(value.ID, TextureTarget.TextureCubeMap);
			//	GL.Uniform1I(Location, slot);
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