using System.IO;

namespace CKGL
{
	public abstract class Shader
	{
		public static int Swaps { get; protected set; }
		public static int UniformSwaps { get; protected set; }

		public static Shader Create(string source)
		{
			return Graphics.CreateShader(source);
		}
		public static Shader CreateFromFile(string file)
		{
			if (!File.Exists(file))
				throw new FileNotFoundException("Shader file not found.", file);
			return Graphics.CreateShader(File.ReadAllText(file));
		}

		public static void PreDraw()
		{
			Swaps = 0;
			UniformSwaps = 0;
		}

		public abstract void Destroy();

		public abstract void Bind();

		#region Uniform Methods
		public abstract void SetUniform(string name, bool value);
		public abstract void SetUniform(string name, int value);
		public abstract void SetUniform(string name, float value);
		public abstract void SetUniform(string name, float x, float y);
		public abstract void SetUniform(string name, float x, float y, float z);
		public abstract void SetUniform(string name, float x, float y, float z, float w);
		public abstract void SetUniform(string name, Vector2 value);
		public abstract void SetUniform(string name, Vector3 value);
		public abstract void SetUniform(string name, Vector4 value);
		public abstract void SetUniform(string name, Colour value);
		public abstract void SetUniform(string name, Matrix2D value);
		public abstract void SetUniform(string name, Matrix3x3 value);
		public abstract void SetUniform(string name, Matrix value);
		public abstract void SetUniform(string name, Texture value, uint textureSlot);
		//public abstract void SetUniform(string name, UniformSampler2D value);
		//public abstract void SetUniform(string name, UniformSamplerCube value);
		#endregion

		#region Overrides
		public abstract override string ToString();

		public abstract override bool Equals(object obj);
		public abstract bool Equals(Shader shader);

		public abstract override int GetHashCode();
		#endregion
	}
}