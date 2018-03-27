using System;
using System.IO;
using System.Collections.Generic;

namespace CKGL
{
	public class Shader
	{
		private uint id;
		//private List<Uniform> uniforms = new List<Uniform>();
		private Dictionary<string, Uniform> uniforms = new Dictionary<string, Uniform>(StringComparer.Ordinal);

		#region Default Shaders
		public static readonly string Basic2D = @"
			#version 330
			uniform mat4 Matrix;
			layout(location = 0) in vec2 vertPos;
			layout(location = 1) in vec2 vertUV;
			layout(location = 2) in vec4 vertCol;
			layout(location = 3) in float vertMult;
			layout(location = 4) in float vertWash;
			layout(location = 5) in float vertVeto;
			out vec2 fragUV;
			out vec4 fragCol;
			out float fragMult;
			out float fragWash;
			out float fragVeto;
			void main(void)
			{
				gl_Position = Matrix * vec4(vertPos, 0.0, 1.0);
				fragUV = vertUV;
				fragCol = vertCol;
				fragMult = vertMult;
				fragWash = vertWash;
				fragVeto = vertVeto;
			}
			...
			#version 330
			uniform sampler2D Texture;
			in vec2 fragUV;
			in vec4 fragCol;
			in float fragMult;
			in float fragWash;
			in float fragVeto;
			layout(location = 0) out vec4 outColor;
			void main(void)
			{
				vec4 color = texture(Texture, fragUV);
				outColor = 
					fragMult * color * fragCol + 
					fragWash * color.a * fragCol + 
					fragVeto * fragCol;
			}
		";
		#endregion

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
			string source = File.ReadAllText(file);
			return new Shader(ref source);
		}

		public void Destroy()
		{
			GL.DeleteShader(id);
		}

		//protected override void Dispose()
		//{
		//	Destroy();
		//}

		public void Use()
		{
			GL.UseProgram(id);
		}

		#region Compile
		private void CompileShader(uint shaderID, string source)
		{
			//Populate the shader and compile it
			GL.ShaderSource(shaderID, source);
			GL.CompileShader(shaderID);

			//Check for shader compile errors
			GL.GetShader(shaderID, ShaderParam.CompileStatus, out int status);
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
			uint vertID = GL.CreateShader(ShaderType.Vertex);
			uint fragID = GL.CreateShader(ShaderType.Fragment);
			CompileShader(vertID, vertSource);
			CompileShader(fragID, fragSource);

			//Create the program and attach the shaders to it
			id = GL.CreateProgram();
			GL.AttachShader(id, vertID);
			GL.AttachShader(id, fragID);

			//Link the program and check for errors
			GL.LinkProgram(id);
			GL.GetProgram(id, ProgramParam.LinkStatus, out int status);
			if (status == 0)
				throw new Exception("Program link error: " + GL.GetProgramInfoLog(id));

			//Once linked, we can detach and delete the shaders
			GL.DetachShader(id, vertID);
			GL.DetachShader(id, fragID);
			GL.DeleteShader(vertID);
			GL.DeleteShader(fragID);

			//Get all the uniforms the shader has and store their information
			GL.GetProgram(id, ProgramParam.ActiveUniforms, out int numUniforms);
			for (int i = 0; i < numUniforms; ++i)
			{
				GL.GetActiveUniform(id, (uint)i, out int count, out UniformType type, out string name);
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
							//Console.WriteLine("index:{0} name:{1} type:{2} loc:{3} count:{4}", i, arrName, type, loc, count);
							var uniform = new Uniform(i, arrName, type, loc);
							//uniforms.Add(uniform);
							uniforms.Add(arrName, uniform);
						}
					}
					else
					{
						int loc = GL.GetUniformLocation(id, name);
						//Console.WriteLine("index:{0} name:{1} type:{2} loc:{3} count:{4}", i, name, type, loc, count);
						var uniform = new Uniform(i, name, type, loc);
						//uniforms.Add(uniform);
						uniforms.Add(name, uniform);
					}
				}
			}
		}
		#endregion

		#region Uniforms
		private class Uniform
		{
			public int Index;
			public string Name;
			public UniformType Type;
			public int Location;

			public Uniform(int index, string name, UniformType type, int location)
			{
				Index = index;
				Name = name;
				Type = type;
				Location = location;
			}

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
			//public void SetUniform(Vector2 value)
			//{
			//	GL.Uniform2F(Location, value.X, value.Y);
			//}
			//public void SetUniform(Vector3 value)
			//{
			//	GL.Uniform3F(Location, value.X, value.Y, value.Z);
			//}
			//public void SetUniform(Vector4 value)
			//{
			//	GL.Uniform4F(Location, value.X, value.Y, value.Z, value.W);
			//}
			//public void SetUniform(Matrix3x2 value)
			//{
			//	fixed (float* m = &value.M0)
			//	{
			//		GL.UniformMatrix3x2FV(Location, 1, false, m);
			//	}
			//}
			//public void SetUniform(Matrix4x4 value)
			//{
			//	fixed (float* m = &Val.M11)
			//	{
			//		GL.UniformMatrix4FV(Location, 1, false, m);
			//	}
			//}
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
		}

		private Uniform GetUniform(string name)
		{
			if (uniforms.TryGetValue(name, out Uniform uniform))
			{
				return uniform;
			}
			else
			{
				throw new Exception($"No uniform with name: {name}");
			}
		}

		public void SetUniform(string name, bool value)
		{
			GetUniform(name).SetUniform(value);
		}
		public void SetUniform(string name, int value)
		{
			GetUniform(name).SetUniform(value);
		}
		public void SetUniform(string name, float value)
		{
			GetUniform(name).SetUniform(value);
		}
		public void SetUniform(string name, float x, float y)
		{
			GetUniform(name).SetUniform(x, y);
		}
		public void SetUniform(string name, float x, float y, float z)
		{
			GetUniform(name).SetUniform(x, y, z);
		}
		public void SetUniform(string name, float x, float y, float z, float w)
		{
			GetUniform(name).SetUniform(x, y, z, w);
		}
		//public void SetUniform(string name, Vector2 value)
		//{
		//	GetUniform(name).SetUniform(value);
		//}
		//public void SetUniform(string name, Vector3 value)
		//{
		//	GetUniform(name).SetUniform(value);
		//}
		//public void SetUniform(string name, Vector4 value)
		//{
		//	GetUniform(name).SetUniform(value);
		//}
		//public void SetUniform(string name, Matrix3x2 value)
		//{
		//	GetUniform(name).SetUniform(value);
		//}
		//public void SetUniform(string name, Matrix4x4 value)
		//{
		//	GetUniform(name).SetUniform(value);
		//}
		//public void SetUniform(string name, UniformSampler2D value)
		//{
		//	GetUniform(name).SetUniform(value);
		//}
		//public void SetUniform(string name, UniformSamplerCube value)
		//{
		//	GetUniform(name).SetUniform(value);
		//}
		#endregion
	}
}