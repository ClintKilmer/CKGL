using static SDL2.SDL_image;

using OpenGL;

using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL
{
	public class Texture
	{
		private static GLuint currentlyBoundTexture;

		private GLuint ID;

		public string File { get; private set; } = "";
		public int Width { get; private set; } = 0;
		public int Height { get; private set; } = 0;
		public int BitsPerPixel { get; private set; } = 0;

		private GLuint localBuffer = 0;

		public Texture()
		{
			Bind();
		}

		private void Generate()
		{
			if (ID == default(GLuint))
				ID = GL.GenTexture();
		}

		public void Destroy()
		{
			if (ID != default(GLuint))
			{
				GL.DeleteTexture(ID);
				ID = default(GLuint);
			}
		}

		public void Bind()
		{
			Generate();

			if (ID != currentlyBoundTexture)
			{
				GL.BindTexture(TextureTarget.Texture2D, ID);
				currentlyBoundTexture = ID;
			}
		}

		//public void UnBind()
		//{
		//	GL.BindTexture(TextureTarget.Texture2D, 0);
		//	currentlyBoundTexture = 0;
		//}

		//public void LoadImage()
		//{
		//	Bind();
		//	GL.BufferData(BufferTarget.ElementArray, sizeof(GLint) * indices.Length, indices, bufferUsage);
		//	Count = indices.Length;
		//}
	}
}