using OpenGL;

using GLint = System.Int32;
using GLuint = System.UInt32;

using System;
using System.IO;
using System.Collections.Generic;

namespace CKGL
{
	public class Shader
	{
		private static GLuint currentlyBoundShader;

		private GLuint id;
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
			layout(location = 0) out vec4 outColour;
			void main(void)
			{
				vec4 colour = texture(Texture, fragUV);
				outColour = 
					fragMult * colour * fragCol + 
					fragWash * colour.a * fragCol + 
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
							//Console.WriteLine("index:{0} name:{1} type:{2} loc:{3} count:{4}", i, arrName, type, loc, count);
							var uniform = new Uniform(i, arrName, type, loc);
							uniforms.Add(arrName, uniform);
						}
					}
					else
					{
						GLint loc = GL.GetUniformLocation(id, name);
						//Console.WriteLine("index:{0} name:{1} type:{2} loc:{3} count:{4}", i, name, type, loc, count);
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

		//protected override void Dispose()
		//{
		//	Destroy();
		//}

		public void Bind()
		{
			if (id != currentlyBoundShader)
			{
				GL.UseProgram(id);
				currentlyBoundShader = id;

				BindUniforms();
			}
		}

		private void BindUniforms()
		{
			foreach (KeyValuePair<string, Uniform> entry in uniforms)
			{
				if (entry.Value.Dirty)
					entry.Value.Upload();
			}
		}

		#region Uniform
		private struct Uniform
		{
			public int Index;
			public string Name;
			public UniformType Type;
			public int Location;
			public object Value;
			public bool Dirty;

			public Uniform(int index, string name, UniformType type, GLint location)
			{
				Index = index;
				Name = name;
				Type = type;
				Location = location;
				Value = "";
				Dirty = false;
			}

			public void Set(object value, bool setNow)
			{
				bool valueDifferent = false;
				//bool valueDifferent = Value != value;
				if (Value == (object)"")
				{
					valueDifferent = true;
				}
				else
				{
					switch (Type)
					{
						case UniformType.Bool:
							valueDifferent = ((bool)Value) != ((bool)value);
							break;
						case UniformType.Int:
							valueDifferent = ((int)Value) != ((int)value);
							break;
						case UniformType.Float:
							valueDifferent = ((float)Value) != ((float)value);
							break;
						case UniformType.Vec2:
							valueDifferent = ((Vector2)Value) != ((Vector2)value);
							break;
						case UniformType.Vec3:
							valueDifferent = ((Vector3)Value) != ((Vector3)value);
							break;
						case UniformType.Vec4:
							valueDifferent = ((Vector4)Value) != ((Vector4)value);
							break;
						case UniformType.Mat3x2:
							valueDifferent = ((Matrix2D)Value) != ((Matrix2D)value);
							break;
						case UniformType.Mat4:
							valueDifferent = ((Matrix)Value) != ((Matrix)value);
							break;
						//case UniformType.Sampler2D:
						//	valueDifferent = (UniformSampler2D)Value != (UniformSampler2D)value;
						//	break;
						//case UniformType.SamplerCube:
						//	valueDifferent = (UniformSamplerCube)Value != (UniformSamplerCube)value;
						//	break;
						default:
							throw new NotImplementedException();
					}
				}

				if (valueDifferent)
				{
					Value = value;
					Dirty = true;

					if (setNow)
						Upload();
				}
			}

			public void Upload()
			{
				if (Dirty)
				{
					switch (Type)
					{
						case UniformType.Bool:
							GL.Uniform1I(Location, (bool)Value ? 1 : 0);
							break;
						case UniformType.Int:
							GL.Uniform1I(Location, (int)Value);
							break;
						case UniformType.Float:
							GL.Uniform1F(Location, (float)Value);
							break;
						case UniformType.Vec2:
							Vector2 vector2 = (Vector2)Value;
							GL.Uniform2F(Location, vector2.X, vector2.Y);
							break;
						case UniformType.Vec3:
							Vector3 vector3 = (Vector3)Value;
							GL.Uniform3F(Location, vector3.X, vector3.Y, vector3.Z);
							break;
						case UniformType.Vec4:
							Vector4 vector4 = (Vector4)Value;
							GL.Uniform4F(Location, vector4.X, vector4.Y, vector4.Z, vector4.W);
							break;
						case UniformType.Mat3x2:
							GL.UniformMatrix3x2FV(Location, 1, false, ((Matrix2D)Value).ToFloatArray());
							break;
						case UniformType.Mat4:
							GL.UniformMatrix4FV(Location, 1, false, ((Matrix)Value).ToFloatArray());
							Console.WriteLine(Value);
							break;
						//case UniformType.Sampler2D:
						//	UniformSampler2D uniformSampler2D = (UniformSampler2D)Value;
						//	int slot = Texture.Bind(uniformSampler2D.ID, uniformSampler2D.BindTarget);
						//	GL.Uniform1I(Location, slot);
						//	break;
						//case UniformType.SamplerCube:
						//	UniformSamplerCube uniformSamplerCube = (UniformSamplerCube)Value;
						//	int slot = Texture.Bind(uniformSamplerCube.ID, TextureTarget.TextureCubeMap);
						//	GL.Uniform1I(Location, slot);
						//	break;
						default:
							throw new NotImplementedException();
					}

					Dirty = false;
				}
			}

			#region Direct
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
				GL.UniformMatrix3x2FV(Location, 1, false, value.ToFloatArray());
			}
			public void SetUniform(Matrix value)
			{
				GL.UniformMatrix4FV(Location, 1, false, value.ToFloatArray());
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

		public void SetUniform(string name, object value)
		{
			GetUniform(name).Set(value, id == currentlyBoundShader);
		}
		public void SetUniform(string name, float x, float y)
		{
			SetUniform(name, new Vector2(x, y));
		}
		public void SetUniform(string name, float x, float y, float z)
		{
			SetUniform(name, new Vector3(x, y, z));
		}
		public void SetUniform(string name, float x, float y, float z, float w)
		{
			SetUniform(name, new Vector4(x, y, z, w));
		}

		#region Direct
		//public void SetUniform(string name, bool value)
		//{
		//	Bind();
		//	GetUniform(name).SetUniform(value);
		//}
		//public void SetUniform(string name, int value)
		//{
		//	Bind();
		//	GetUniform(name).SetUniform(value);
		//}
		//public void SetUniform(string name, float value)
		//{
		//	Bind();
		//	GetUniform(name).SetUniform(value);
		//}
		//public void SetUniform(string name, float x, float y)
		//{
		//	Bind();
		//	GetUniform(name).SetUniform(x, y);
		//}
		//public void SetUniform(string name, float x, float y, float z)
		//{
		//	Bind();
		//	GetUniform(name).SetUniform(x, y, z);
		//}
		//public void SetUniform(string name, float x, float y, float z, float w)
		//{
		//	Bind();
		//	GetUniform(name).SetUniform(x, y, z, w);
		//}
		//public void SetUniform(string name, Vector2 value)
		//{
		//	Bind();
		//	GetUniform(name).SetUniform(value);
		//}
		//public void SetUniform(string name, Vector3 value)
		//{
		//	Bind();
		//	GetUniform(name).SetUniform(value);
		//}
		//public void SetUniform(string name, Vector4 value)
		//{
		//	Bind();
		//	GetUniform(name).SetUniform(value);
		//}
		//public void SetUniform(string name, Matrix2D value)
		//{
		//	Bind();
		//	GetUniform(name).SetUniform(value);
		//}
		//public void SetUniform(string name, Matrix value)
		//{
		//	Bind();
		//	GetUniform(name).SetUniform(value);
		//}
		////public void SetUniform(string name, UniformSampler2D value)
		////{
		////	Bind();
		////	GetUniform(name).SetUniform(value);
		////}
		////public void SetUniform(string name, UniformSamplerCube value)
		////{
		////	Bind();
		////	GetUniform(name).SetUniform(value);
		////} 
		#endregion
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