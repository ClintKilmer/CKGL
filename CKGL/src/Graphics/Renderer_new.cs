using System;
using System.Runtime.InteropServices;

namespace CKGL
{
	public static class Renderer
	{
		//	  code crab...
		//		  ^ ^
		//		>( . )<

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct Vertex
		{
			public Vector3 Position;
			public Colour Colour;
			public Vector2 UV;
			public bool Textured;

			public Vertex(Vector3 position, Colour colour, Vector2 uv, bool textured)
			{
				Position = position;
				Colour = colour;
				UV = uv;
				Textured = textured;
			}

			public static int FloatStride = 10;

			public static float[] GetVBO(Vertex[] vertices)
			{
				float[] vbo = new float[FloatStride * bufferSize];

				for (int i = 0; i < vertexCount; i++)
				{
					vbo[i * FloatStride + 0] = vertices[i].Position.X;
					vbo[i * FloatStride + 1] = vertices[i].Position.Y;
					vbo[i * FloatStride + 2] = vertices[i].Position.Z;
					vbo[i * FloatStride + 3] = vertices[i].Colour.R;
					vbo[i * FloatStride + 4] = vertices[i].Colour.G;
					vbo[i * FloatStride + 5] = vertices[i].Colour.B;
					vbo[i * FloatStride + 6] = vertices[i].Colour.A;
					vbo[i * FloatStride + 7] = vertices[i].UV.X;
					vbo[i * FloatStride + 8] = vertices[i].UV.Y;
					vbo[i * FloatStride + 9] = vertices[i].Textured ? 1.0f : 0.0f;
				}

				return vbo;
			}
		}

		private static VertexArray vao;
		private static VertexBuffer vbo;
		private static VertexBufferLayout vboLayout;
		private static DrawMode currentDrawMode = DrawMode.TriangleList;
		private static Shader DefaultShader { get; } = InternalShaders.Renderer; // TODO - Default Renderer Shader
		private const int bufferSize = 1998; // Divisible by 3 and 2 for no vertex wrapping per batch
		private static Vertex[] vertices = new Vertex[bufferSize];
		private static int vertexCount = 0;

		public static void Init()
		{
			vao = new VertexArray();
			vbo = new VertexBuffer();
			vboLayout = new VertexBufferLayout();
			vboLayout.Push<float>(3); // position
			vboLayout.Push<float>(4); // colour
			vboLayout.Push<float>(2); // uvs
			vboLayout.Push<float>(1); // textured
			vao.AddBuffer(vbo, vboLayout);

			Graphics.State.OnStateChanging += () => { Flush(); };

			// Debug
			Output.WriteLine($"Renderer Initialized");
		}

		public static void Destroy()
		{
			vao.Destroy();
			vbo.Destroy();

			vao = null;
			vbo = null;
			vboLayout = null;
		}

		// TODO - Move all state stuff to Graphics.State
		#region State
		public static void SetTexture(Texture texture)
		{
			if (!texture.IsBound())
			{
				Flush();
				texture.Bind();
			}
		}
		#endregion

		public static void Flush()
		{
			if (vertexCount > 0)
			{
				OpenGL.GL.Enable(OpenGL.EnableCap.ScissorTest);
				OpenGL.GL.Scissor(0, 0, RenderTarget.Current?.Width ?? Window.Width, RenderTarget.Current?.Height ?? Window.Height);

				vao.Bind();
				vbo.LoadData(Vertex.GetVBO(vertices), BufferUsage.DynamicDraw);

				Graphics.DrawVertexArrays(currentDrawMode, 0, vertexCount);
			}

			// Reset vertexCount so we don't lose any vertex data
			int remainder = 0;
			switch (currentDrawMode)
			{
				case (DrawMode.TriangleList):
					remainder = vertexCount % 3;
					break;
				case (DrawMode.LineList):
					remainder = vertexCount % 2;
					break;
				default:
					remainder = 0;
					break;
			}
			for (int i = 0; i < remainder; i++)
			{
				vertices[i] = vertices[vertexCount - remainder + i];
			}
			vertexCount = remainder;
		}

		private static void AddVertex(DrawMode type, Vector3 position, Colour colour, Vector2? uv, Matrix2D? matrix) => AddVertex(type, position * matrix ?? position, colour, uv);
		private static void AddVertex(DrawMode type, Vector3 position, Colour colour, Vector2? uv, Matrix? matrix) => AddVertex(type, position * matrix ?? position, colour, uv);
		private static void AddVertex(DrawMode type, Vector3 position, Colour colour, Vector2? uv)
		{
			if (currentDrawMode != type)
			{
				Flush();
				currentDrawMode = type;
				// We can lose vertices here, but it's ok as we're switching primitive types anyways
				vertexCount = 0;
			}

			if (vertexCount >= bufferSize)
				Flush();

			vertices[vertexCount].Position = position;
			vertices[vertexCount].Colour = colour;
			vertices[vertexCount].UV = uv ?? Vector2.Zero;
			vertices[vertexCount].Textured = uv != null;
			vertexCount++;
		}

		public static class Draw
		{
			public static void Triangle(Vector2 v1, Vector2 v2, Vector2 v3, Colour colour) => Triangle(v1, v2, v3, colour, colour, colour, null, null, null, null);
			public static void Triangle(Vector2 v1, Vector2 v2, Vector2 v3, Colour colour, Matrix2D matrix) => Triangle(v1, v2, v3, colour, colour, colour, null, null, null, matrix);
			public static void Triangle(Vector2 v1, Vector2 v2, Vector2 v3, Colour c1, Colour c2, Colour c3) => Triangle(v1, v2, v3, c1, c2, c3, null, null, null, null);
			public static void Triangle(Vector2 v1, Vector2 v2, Vector2 v3, Colour c1, Colour c2, Colour c3, Matrix2D matrix) => Triangle(v1, v2, v3, c1, c2, c3, null, null, null, matrix);
			public static void Triangle(Vector2 v1, Vector2 v2, Vector2 v3, Colour c1, Colour c2, Colour c3, Vector2? uv1, Vector2? uv2, Vector2? uv3) => Triangle(v1, v2, v3, c1, c2, c3, null, null, null, null);
			public static void Triangle(Vector2 v1, Vector2 v2, Vector2 v3, Colour c1, Colour c2, Colour c3, Vector2? uv1, Vector2? uv2, Vector2? uv3, Matrix2D? matrix)
			{
				AddVertex(DrawMode.TriangleList, v1, c1, uv1, matrix);
				AddVertex(DrawMode.TriangleList, v2, c2, uv2, matrix);
				AddVertex(DrawMode.TriangleList, v3, c3, uv3, matrix);
			}

			public static void Rectangle(float x, float y, float width, float height, Colour colour) => Rectangle(new Vector2(x, y), new Vector2(x + width, y), new Vector2(x, y + height), new Vector2(x + width, y + height), colour, colour, colour, colour, null, null, null, null, null);
			public static void Rectangle(float x, float y, float width, float height, Colour colour, Matrix2D? matrix) => Rectangle(new Vector2(x, y), new Vector2(x + width, y), new Vector2(x, y + height), new Vector2(x + width, y + height), colour, colour, colour, colour, null, null, null, null, matrix);
			public static void Rectangle(float x, float y, float width, float height, Colour colour, Vector2? uv1, Vector2? uv2, Vector2? uv3, Vector2? uv4) => Rectangle(new Vector2(x, y), new Vector2(x + width, y), new Vector2(x, y + height), new Vector2(x + width, y + height), colour, colour, colour, colour, uv1, uv2, uv3, uv4, null);
			public static void Rectangle(float x, float y, float width, float height, Colour colour, Vector2? uv1, Vector2? uv2, Vector2? uv3, Vector2? uv4, Matrix2D? matrix) => Rectangle(new Vector2(x, y), new Vector2(x + width, y), new Vector2(x, y + height), new Vector2(x + width, y + height), colour, colour, colour, colour, uv1, uv2, uv3, uv4, matrix);
			public static void Rectangle(float x, float y, float width, float height, Colour c1, Colour c2, Colour c3, Colour c4) => Rectangle(new Vector2(x, y), new Vector2(x + width, y), new Vector2(x, y + height), new Vector2(x + width, y + height), c1, c2, c3, c4, null, null, null, null, null);
			public static void Rectangle(float x, float y, float width, float height, Colour c1, Colour c2, Colour c3, Colour c4, Matrix2D? matrix) => Rectangle(new Vector2(x, y), new Vector2(x + width, y), new Vector2(x, y + height), new Vector2(x + width, y + height), c1, c2, c3, c4, null, null, null, null, matrix);
			public static void Rectangle(float x, float y, float width, float height, Colour c1, Colour c2, Colour c3, Colour c4, Vector2? uv1, Vector2? uv2, Vector2? uv3, Vector2? uv4) => Rectangle(new Vector2(x, y), new Vector2(x + width, y), new Vector2(x, y + height), new Vector2(x + width, y + height), c1, c2, c3, c4, uv1, uv2, uv3, uv4, null);
			public static void Rectangle(float x, float y, float width, float height, Colour c1, Colour c2, Colour c3, Colour c4, Vector2? uv1, Vector2? uv2, Vector2? uv3, Vector2? uv4, Matrix2D? matrix) => Rectangle(new Vector2(x, y), new Vector2(x + width, y), new Vector2(x, y + height), new Vector2(x + width, y + height), c1, c2, c3, c4, uv1, uv2, uv3, uv4, matrix);
			//
			public static void Rectangle(Vector2 v1, Vector2 v2, Colour colour) => Rectangle(new Vector2(v1.X, v1.Y), new Vector2(v1.X + v2.X, v1.Y), new Vector2(v1.X, v1.Y + v2.Y), new Vector2(v1.X + v2.X, v1.Y + v2.Y), colour, colour, colour, colour, null, null, null, null, null);
			public static void Rectangle(Vector2 v1, Vector2 v2, Colour colour, Matrix2D? matrix) => Rectangle(new Vector2(v1.X, v1.Y), new Vector2(v1.X + v2.X, v1.Y), new Vector2(v1.X, v1.Y + v2.Y), new Vector2(v1.X + v2.X, v1.Y + v2.Y), colour, colour, colour, colour, null, null, null, null, matrix);
			public static void Rectangle(Vector2 v1, Vector2 v2, Colour colour, Vector2? uv1, Vector2? uv2, Vector2? uv3, Vector2? uv4) => Rectangle(new Vector2(v1.X, v1.Y), new Vector2(v1.X + v2.X, v1.Y), new Vector2(v1.X, v1.Y + v2.Y), new Vector2(v1.X + v2.X, v1.Y + v2.Y), colour, colour, colour, colour, uv1, uv2, uv3, uv4, null);
			public static void Rectangle(Vector2 v1, Vector2 v2, Colour colour, Vector2? uv1, Vector2? uv2, Vector2? uv3, Vector2? uv4, Matrix2D? matrix) => Rectangle(new Vector2(v1.X, v1.Y), new Vector2(v1.X + v2.X, v1.Y), new Vector2(v1.X, v1.Y + v2.Y), new Vector2(v1.X + v2.X, v1.Y + v2.Y), colour, colour, colour, colour, uv1, uv2, uv3, uv4, matrix);
			public static void Rectangle(Vector2 v1, Vector2 v2, Colour c1, Colour c2, Colour c3, Colour c4) => Rectangle(new Vector2(v1.X, v1.Y), new Vector2(v1.X + v2.X, v1.Y), new Vector2(v1.X, v1.Y + v2.Y), new Vector2(v1.X + v2.X, v1.Y + v2.Y), c1, c2, c3, c4, null, null, null, null, null);
			public static void Rectangle(Vector2 v1, Vector2 v2, Colour c1, Colour c2, Colour c3, Colour c4, Matrix2D? matrix) => Rectangle(new Vector2(v1.X, v1.Y), new Vector2(v1.X + v2.X, v1.Y), new Vector2(v1.X, v1.Y + v2.Y), new Vector2(v1.X + v2.X, v1.Y + v2.Y), c1, c2, c3, c4, null, null, null, null, matrix);
			public static void Rectangle(Vector2 v1, Vector2 v2, Colour c1, Colour c2, Colour c3, Colour c4, Vector2? uv1, Vector2? uv2, Vector2? uv3, Vector2? uv4) => Rectangle(new Vector2(v1.X, v1.Y), new Vector2(v1.X + v2.X, v1.Y), new Vector2(v1.X, v1.Y + v2.Y), new Vector2(v1.X + v2.X, v1.Y + v2.Y), c1, c2, c3, c4, uv1, uv2, uv3, uv4, null);
			public static void Rectangle(Vector2 v1, Vector2 v2, Colour c1, Colour c2, Colour c3, Colour c4, Vector2? uv1, Vector2? uv2, Vector2? uv3, Vector2? uv4, Matrix2D? matrix) => Rectangle(new Vector2(v1.X, v1.Y), new Vector2(v1.X + v2.X, v1.Y), new Vector2(v1.X, v1.Y + v2.Y), new Vector2(v1.X + v2.X, v1.Y + v2.Y), c1, c2, c3, c4, uv1, uv2, uv3, uv4, matrix);
			//
			public static void Rectangle(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Colour colour) => Rectangle(v1, v2, v3, v4, colour, colour, colour, colour, null, null, null, null, null);
			public static void Rectangle(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Colour colour, Matrix2D? matrix) => Rectangle(v1, v2, v3, v4, colour, colour, colour, colour, null, null, null, null, matrix);
			public static void Rectangle(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Colour colour, Vector2? uv1, Vector2? uv2, Vector2? uv3, Vector2? uv4) => Rectangle(v1, v2, v3, v4, colour, colour, colour, colour, uv1, uv2, uv3, uv4, null);
			public static void Rectangle(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Colour colour, Vector2? uv1, Vector2? uv2, Vector2? uv3, Vector2? uv4, Matrix2D? matrix) => Rectangle(v1, v2, v3, v4, colour, colour, colour, colour, uv1, uv2, uv3, uv4, matrix);
			public static void Rectangle(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Colour c1, Colour c2, Colour c3, Colour c4) => Rectangle(v1, v2, v3, v4, c1, c2, c3, c4, null, null, null, null, null);
			public static void Rectangle(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Colour c1, Colour c2, Colour c3, Colour c4, Matrix2D? matrix) => Rectangle(v1, v2, v3, v4, c1, c2, c3, c4, null, null, null, null, matrix);
			public static void Rectangle(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Colour c1, Colour c2, Colour c3, Colour c4, Vector2? uv1, Vector2? uv2, Vector2? uv3, Vector2? uv4) => Rectangle(v1, v2, v3, v4, c1, c2, c3, c4, uv1, uv2, uv3, uv4, null);
			public static void Rectangle(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Colour c1, Colour c2, Colour c3, Colour c4, Vector2? uv1, Vector2? uv2, Vector2? uv3, Vector2? uv4, Matrix2D? matrix)
			{
				AddVertex(DrawMode.TriangleList, v1, c1, uv1, matrix);
				AddVertex(DrawMode.TriangleList, v2, c2, uv2, matrix);
				AddVertex(DrawMode.TriangleList, v3, c3, uv3, matrix);
				AddVertex(DrawMode.TriangleList, v3, c3, uv3, matrix);
				AddVertex(DrawMode.TriangleList, v2, c2, uv2, matrix);
				AddVertex(DrawMode.TriangleList, v4, c4, uv4, matrix);
			}

			// Sprites
			//public static void Sprite(Sprite sprite, Vector2 position, Vector2 scale) => Sprite(sprite, position, scale, Colour.White, null);
			public static void Sprite(Sprite sprite, Vector2 position, Vector2 scale, Matrix2D? matrix = null) => Sprite(sprite, position, scale, Colour.White, matrix);
			public static void Sprite(Sprite sprite, Vector2 position, Vector2 scale, Colour colour) => Sprite(sprite, position, scale, colour, null);
			public static void Sprite(Sprite sprite, Vector2 position, Vector2 scale, Colour colour, Matrix2D? matrix)
			{
				SetTexture(sprite.SpriteSheet.Texture);
				Rectangle(position.X, position.Y, sprite.Width * scale.X, sprite.Height * scale.Y, colour, colour, colour, colour, sprite.UV_BL, sprite.UV_BR, sprite.UV_TL, sprite.UV_TR, matrix);
			}

			// Strings
			public static void Text(SpriteFont font, string text, Vector2 position, Vector2 scale, Colour colour, HAlign hAlign = HAlign.Left, VAlign vAlign = VAlign.Top)
			{
				SetTexture(font.SpriteSheet.Texture);

				float offsetX = 0;
				float offsetY = 0;

				float offsetHAlign = 0;
				if (hAlign == HAlign.Center)
					offsetHAlign = 0.5f;
				else if (hAlign == HAlign.Right)
					offsetHAlign = 1f;

				float offsetVAlign = 0;
				if (vAlign == VAlign.Middle)
					offsetVAlign = 0.5f;
				else if (vAlign == VAlign.Top)
					offsetVAlign = 1f;

				string[] lines = text.Replace("|:", "\a").Replace(":|", "\a").Split('\n');

				float totalHeight = lines.Length * font.LineHeight;

				bool mod = false;
				string modData = "";
				bool modShadow = false;
				Vector2 modShadowOffset = Vector2.Zero;
				Colour modShadowColour = Colour.White;

				foreach (string line in lines)
				{
					float lineWidth = 0;

					foreach (char c in line)
					{
						if (mod)
						{
							if (c == '\a')
								mod = false;
						}
						else
						{
							if (c == ' ')
								lineWidth += font.SpaceWidth;
							else if (c == '\a')
								mod = true;
							else
								lineWidth += font.Glyph(c).Width + font.CharSpacing;
						}
					}

					foreach (char c in line)
					{
						if (mod)
						{
							if (c == '\a')
							{
								string[] tag = modData.Split('=');

								if (tag.Length == 2)
								{
									if (tag[0] == "colour" || tag[0] == "colour")
									{
										string[] value = tag[1].Split(',');

										if (value.Length == 4)
										{
											try
											{
												colour = new Colour(float.Parse(value[0]), float.Parse(value[1]), float.Parse(value[2]), float.Parse(value[3]));
											}
											catch
											{
												throw new Exception("Text modifier value parse error.");
											}
										}
										else
										{
											throw new Exception("Unrecognized text modifier value.");
										}
									}
									else if (tag[0] == "shadow")
									{
										string[] value = tag[1].Split(',');

										if (value.Length == 6)
										{
											try
											{
												modShadow = true;
												modShadowOffset = new Vector2(float.Parse(value[0]), float.Parse(value[1])) * scale;
												modShadowColour = new Colour(float.Parse(value[2]), float.Parse(value[3]), float.Parse(value[4]), float.Parse(value[5]));
											}
											catch
											{
												throw new Exception("Text modifier value parse error.");
											}
										}
										else if (value.Length == 1 && (value[0] == "disable" || value[0] == "off"))
										{
											modShadow = false;
										}
										else
										{
											throw new Exception("Unrecognized text modifier value.");
										}
									}
									else
									{
										throw new Exception("Unrecognized text modifier key.");
									}
								}
								else
								{
									throw new Exception("Text modifier parse error.");
								}

								mod = false;
								modData = "";
							}
							else
							{
								modData += c;
							}
						}
						else
						{
							if (c == ' ')
							{
								offsetX += font.SpaceWidth * scale.X;
							}
							else if (c == '\a')
							{
								mod = true;
							}
							else
							{
								Sprite sprite = font.Glyph(c);

								if (modShadow)
								{
									Rectangle(position.X + offsetX * scale.X - lineWidth * scale.X * offsetHAlign + modShadowOffset.X,
											  position.Y + offsetY * scale.Y - totalHeight * scale.Y * offsetVAlign + modShadowOffset.Y,
											  sprite.Width * scale.X,
											  sprite.Height * scale.Y,
											  modShadowColour, modShadowColour, modShadowColour, modShadowColour,
											  sprite.UV_BL, sprite.UV_BR, sprite.UV_TL, sprite.UV_TR);
								}

								Rectangle(position.X + offsetX * scale.X - lineWidth * scale.X * offsetHAlign,
										  position.Y + offsetY * scale.Y - totalHeight * scale.Y * offsetVAlign,
										  sprite.Width * scale.X,
										  sprite.Height * scale.Y,
										  colour, colour, colour, colour,
										  sprite.UV_BL, sprite.UV_BR, sprite.UV_TL, sprite.UV_TR);

								offsetX += sprite.Width + font.CharSpacing;
							}
						}
					}

					offsetX = 0;
					offsetY -= font.LineHeight;
				}
			}

			// TODO - Fix RenderTargets cascading with Matrix
			// RenderTargets
			private static Vector2 uvFull1 = new Vector2(0f, 0f);
			private static Vector2 uvFull2 = new Vector2(1f, 0f);
			private static Vector2 uvFull3 = new Vector2(0f, 1f);
			private static Vector2 uvFull4 = new Vector2(1f, 1f);
			public static void RenderTarget(RenderTarget renderTarget, int texture, float x, float y, Colour colour)
			{
				SetTexture(renderTarget.textures[texture]);
				Rectangle(x, y, renderTarget.Width, renderTarget.Height, colour, colour, colour, colour, uvFull1, uvFull2, uvFull3, uvFull4);
			}
			public static void RenderTarget(RenderTarget renderTarget, int texture, float x, float y, float rotation, Vector2 origin, Colour colour)
			{
				SetTexture(renderTarget.textures[texture]);
				Rectangle(x, y, renderTarget.Width, renderTarget.Height, colour, colour, colour, colour, uvFull1, uvFull2, uvFull3, uvFull4, Matrix2D.CreateRotationZ(rotation.RotationsToRadians()));
			}
			public static void RenderTarget(RenderTarget renderTarget, int texture, float x, float y, float scale, Colour colour)
			{
				SetTexture(renderTarget.textures[texture]);
				Rectangle(x, y, renderTarget.Width * scale, renderTarget.Height * scale, colour, colour, colour, colour, uvFull1, uvFull2, uvFull3, uvFull4);
			}
			public static void RenderTarget(RenderTarget renderTarget, int texture, float x, float y, float scale, float rotation, Vector2 origin, Colour colour)
			{
				SetTexture(renderTarget.textures[texture]);
				Rectangle(x, y, renderTarget.Width * scale, renderTarget.Height * scale, colour, colour, colour, colour, uvFull1, uvFull2, uvFull3, uvFull4, Matrix2D.CreateRotationZ(rotation.RotationsToRadians()));
			}
			public static void RenderTarget(RenderTarget renderTarget, int texture, float x, float y, float width, float height, Colour colour)
			{
				SetTexture(renderTarget.textures[texture]);
				Rectangle(x, y, width, height, colour, colour, colour, colour, uvFull1, uvFull2, uvFull3, uvFull4);
			}
			public static void RenderTarget(RenderTarget renderTarget, int texture, Vector2 v1, Vector2 v2, Colour colour)
			{
				SetTexture(renderTarget.textures[texture]);
				Rectangle(new Vector2(v1.X, v1.Y), new Vector2(v2.X, v2.Y), colour, colour, colour, colour, uvFull1, uvFull2, uvFull3, uvFull4);
			}

			// Pixels
			public static void Pixel(int x, int y, Colour colour)
			{
				Rectangle(x, y, x + 1, y + 1, colour);
			}
			public static void Pixel(float x, float y, Colour colour)
			{
				Rectangle(x, y, x + 1, y + 1, colour);
			}
			public static void Pixel(Vector2 vector, Colour colour)
			{
				Rectangle(vector, vector + Vector2.One, colour);
			}

			// Circles
			public static void Circle(Vector2 center, float radius, Colour colour, int circleSegments = 16) => Circle(center, radius, colour, colour, circleSegments);
			public static void Circle(Vector2 center, float radius, Colour c1, Colour c2, int circleSegments = 16)
			{
				var increment = Math.PI * 2.0f / circleSegments;
				var theta = 0.0f;

				var v0 = center;
				theta += increment;

				for (int i = 0; i < circleSegments; i++)
				{
					var v1 = center + radius * new Vector2(Math.Cos(theta), Math.Sin(theta));
					var v2 = center + radius * new Vector2(Math.Cos(theta + increment), Math.Sin(theta + increment));

					AddVertex(DrawMode.TriangleList, v0, c1, null);
					AddVertex(DrawMode.TriangleList, v1, c2, null);
					AddVertex(DrawMode.TriangleList, v2, c2, null);

					theta += increment;
				}
			}

			// PolyLines
			public static class PolyLines
			{
				public static void Line(float x1, float y1, float x2, float y2, Colour colour)
				{
					Line(x1, y1, x2, y2, colour, colour, 1f);
				}
				public static void Line(float x1, float y1, float x2, float y2, Colour colour, float width)
				{
					Line(x1, y1, x2, y2, colour, colour, width);
				}

				public static void Line(float x1, float y1, float x2, float y2, Colour c1, Colour c2)
				{
					Line(x1, y1, x2, y2, c1, c2, 1f);
				}
				public static void Line(float x1, float y1, float x2, float y2, Colour c1, Colour c2, float width)
				{
					Line(new Vector2(x1, y1), new Vector2(x2, y2), c1, c2, width);
				}

				public static void Line(Vector2 v1, Vector2 v2, Colour colour)
				{
					Line(v1, v2, colour, colour, 1f);
				}
				public static void Line(Vector2 v1, Vector2 v2, Colour colour, float width)
				{
					Line(v1, v2, colour, colour, width);
				}

				public static void Line(Vector2 v1, Vector2 v2, Colour c1, Colour c2)
				{
					Line(v1, v2, c1, c2, 1f);
				}
				public static void Line(Vector2 v1, Vector2 v2, Colour c1, Colour c2, float width)
				{
					float a = 0.5f - width * 0.5f;
					float w = 0.5f + width * 0.5f;

					Vector2 v1_tl = new Vector2(v1.X + a, v1.Y + a);
					Vector2 v1_tr = new Vector2(v1.X + w, v1.Y + a);
					Vector2 v1_bl = new Vector2(v1.X + a, v1.Y + w);
					Vector2 v1_br = new Vector2(v1.X + w, v1.Y + w);
					Vector2 v2_tl = new Vector2(v2.X + a, v2.Y + a);
					Vector2 v2_tr = new Vector2(v2.X + w, v2.Y + a);
					Vector2 v2_bl = new Vector2(v2.X + a, v2.Y + w);
					Vector2 v2_br = new Vector2(v2.X + w, v2.Y + w);

					// Cross Side to Side
					if (v1.X < v2.X && v1.Y >= v2.Y)
					{
						//if(v1.Y > v2.Y)
						//{
						//	v1_tl.Y -= width;
						//	v1_tr.Y -= width;
						//	v1_bl.Y -= width;
						//	v1_br.Y -= width;
						//}
						//v2_tl.X -= width;
						//v2_tr.X -= width;
						//v2_bl.X -= width;
						//v2_br.X -= width;
						LineQuad(v1_bl, v1_br, v2_tr, v2_br, c1, c2);
						LineQuad(v1_tl, v1_bl, v2_tl, v2_tr, c1, c2);
					}
					else if (v1.X >= v2.X && v1.Y < v2.Y)
					{
						//v1_tl.X -= width;
						//v1_tr.X -= width;
						//v1_bl.X -= width;
						//v1_br.X -= width;
						//v2_tl.Y -= width;
						//v2_tr.Y -= width;
						//v2_bl.Y -= width;
						//v2_br.Y -= width;
						LineQuad(v1_tr, v1_br, v2_bl, v2_br, c1, c2);
						LineQuad(v1_tl, v1_tr, v2_tl, v2_bl, c1, c2);
					}
					else if (v1.X < v2.X && v1.Y < v2.Y)
					{
						//v2_tl -= new Vector2(width, width);
						//v2_tr -= new Vector2(width, width);
						//v2_bl -= new Vector2(width, width);
						//v2_br -= new Vector2(width, width);
						LineQuad(v1_tl, v1_tr, v2_br, v2_tr, c1, c2);
						LineQuad(v1_tl, v1_bl, v2_br, v2_bl, c1, c2);
					}
					else if (v1.X >= v2.X && v1.Y >= v2.Y)
					{
						//v1_tl -= new Vector2(width, width);
						//v1_tr -= new Vector2(width, width);
						//v1_bl -= new Vector2(width, width);
						//v1_br -= new Vector2(width, width);
						LineQuad(v1_tr, v1_br, v2_tr, v2_tl, c1, c2);
						LineQuad(v1_bl, v1_br, v2_bl, v2_tl, c1, c2);
					}

					// Side to Side
					//LineQuad(v1_tl, v1_tr, v2_tl, v2_tr, c1, c2);
					//LineQuad(v1_bl, v1_br, v2_bl, v2_br, c1, c2);
					//LineQuad(v1_tl, v1_bl, v2_tl, v2_bl, c1, c2);
					//LineQuad(v1_tr, v1_br, v2_tr, v2_br, c1, c2);

					// Cross
					//if ((v1.X >= v2.X && v1.Y < v2.Y) || (v1.X < v2.X && v1.Y >= v2.Y))
					//	LineQuad(v1_tl, v1_br, v2_tl, v2_br, c1, c2);
					//if ((v1.X >= v2.X && v1.Y >= v2.Y) || (v1.X < v2.X && v1.Y < v2.Y))
					//	LineQuad(v1_bl, v1_tr, v2_bl, v2_tr, c1, c2);
				}
				private static void LineQuad(Vector2 v1a, Vector2 v1b, Vector2 v2a, Vector2 v2b, Colour c1, Colour c2)
				{
					Triangle(v1a, v1b, v2a, c1, c1, c2);
					Triangle(v2a, v1b, v2b, c2, c1, c2);
				}
			}

			public static class Points
			{
				public static void Point(float x, float y, Colour c)
				{
					Point(new Vector2(x, y), c);
				}
				public static void Point(float x, float y, float z, Colour c)
				{
					Point(new Vector3(x, y, z), c);
				}
				public static void Point(Vector3 v, Colour c)
				{
					AddVertex(DrawMode.PointList, v, c, null);
				}
			}

			public static class Lines
			{
				public static void Line(float x1, float y1, float x2, float y2, Colour colour) => Line(new Vector2(x1, y1), new Vector2(x2, y2), colour, colour, null, null);
				public static void Line(float x1, float y1, float x2, float y2, Colour c1, Colour c2) => Line(new Vector2(x1, y1), new Vector2(x2, y2), c1, c2, null, null);
				public static void Line(Vector2 v1, Vector2 v2, Colour colour) => Line(v1, v2, colour, colour, null, null);
				public static void Line(Vector2 v1, Vector2 v2, Colour c1, Colour c2) => Line(v1, v2, c1, c2, null, null);
				public static void Line(Vector2 v1, Vector2 v2, Colour c1, Colour c2, Vector2? uv1, Vector2? uv2)
				{
					AddVertex(DrawMode.LineList, v1, c1, uv1);
					AddVertex(DrawMode.LineList, v2, c2, uv2);
				}

				public static void Line3D(Vector3 v1, Vector3 v2, Colour c1, Colour c2)
					=> Line3D(v1, v2, c1, c2, null, null);
				public static void Line3D(Vector3 v1, Vector3 v2, Colour c1, Colour c2, Vector2? uv1, Vector2? uv2)
				{
					AddVertex(DrawMode.LineList, v1, c1, uv1);
					AddVertex(DrawMode.LineList, v2, c2, uv2);
				}
			}

			public static class LineListStrip
			{
				private static bool working = false;
				private static Vertex? lastVertex = null;

				public static void Begin()
				{
					if (working)
						throw new Exception("End must be called before Begin can be called again.");

					working = true;
					lastVertex = null;
				}

				public static void End()
				{
					if (!working)
						throw new Exception("Begin must be called before End can be called.");

					working = false;
					lastVertex = null;
				}

				public static void AddVertex(Vector2 position, Colour colour) => AddVertex(position, colour, null, null);
				public static void AddVertex(Vector2 position, Colour colour, Matrix2D? matrix) => AddVertex(position, colour, null, matrix);
				public static void AddVertex(Vector2 position, Colour colour, Vector2? uv) => AddVertex(position, colour, uv, null);
				public static void AddVertex(Vector2 position, Colour colour, Vector2? uv, Matrix2D? matrix)
				{
					if (!working)
						throw new Exception("Begin must be called before AddVertex can be called.");

					if (lastVertex.HasValue)
					{
						Renderer.AddVertex(DrawMode.LineList,
										   lastVertex.Value.Position,
										   lastVertex.Value.Colour,
										   lastVertex.Value.UV);
						Renderer.AddVertex(DrawMode.LineList, position, colour, uv, matrix);
					}

					lastVertex = new Vertex(position * matrix ?? position, colour, uv ?? Vector2.Zero, uv != null);
				}
			}

			public static class LineStrip
			{
				public static void AddVertex(Vector2 position, Colour colour) => AddVertex(position, colour, null, null);
				public static void AddVertex(Vector2 position, Colour colour, Matrix2D? matrix) => AddVertex(position, colour, null, matrix);
				public static void AddVertex(Vector2 position, Colour colour, Vector2? uv) => AddVertex(position, colour, uv, null);
				public static void AddVertex(Vector2 position, Colour colour, Vector2? uv, Matrix2D? matrix)
				{
					Renderer.AddVertex(DrawMode.LineStrip, position, colour, uv, matrix);
				}
			}

			public static class TriangleListStrip
			{
				private static bool working = false;
				private static Vertex? lastVertex = null;
				private static Vertex? lastLastVertex = null;

				public static void Begin()
				{
					if (working)
						throw new Exception("End must be called before Begin can be called again.");

					working = true;
					lastVertex = null;
					lastLastVertex = null;
				}

				public static void End()
				{
					if (!working)
						throw new Exception("Begin must be called before End can be called.");

					working = false;
					lastVertex = null;
					lastLastVertex = null;
				}

				public static void AddVertex(Vector2 position, Colour colour) => AddVertex(position, colour, null, null);
				public static void AddVertex(Vector2 position, Colour colour, Matrix2D? matrix) => AddVertex(position, colour, null, matrix);
				public static void AddVertex(Vector2 position, Colour colour, Vector2? uv) => AddVertex(position, colour, uv, null);
				public static void AddVertex(Vector2 position, Colour colour, Vector2? uv, Matrix2D? matrix)
				{
					if (!working)
						throw new Exception("Begin must be called before AddVertex can be called.");

					if (lastLastVertex.HasValue && lastVertex.HasValue)
					{
						Renderer.AddVertex(DrawMode.TriangleList,
										   lastLastVertex.Value.Position,
										   lastLastVertex.Value.Colour,
										   lastLastVertex.Value.UV);
						Renderer.AddVertex(DrawMode.TriangleList,
										   lastVertex.Value.Position,
										   lastVertex.Value.Colour,
										   lastVertex.Value.UV);
						Renderer.AddVertex(DrawMode.TriangleList, position, colour, uv, matrix);
					}

					lastLastVertex = lastVertex;
					lastVertex = new Vertex(position * matrix ?? position, colour, uv ?? Vector2.Zero, uv != null);
				}
			}

			public static class TriangleStrip
			{
				public static void AddVertex(Vector2 position, Colour colour) => AddVertex(position, colour, null, null);
				public static void AddVertex(Vector2 position, Colour colour, Matrix2D? matrix) => AddVertex(position, colour, null, matrix);
				public static void AddVertex(Vector2 position, Colour colour, Vector2? uv) => AddVertex(position, colour, uv, null);
				public static void AddVertex(Vector2 position, Colour colour, Vector2? uv, Matrix2D? matrix)
				{
					Renderer.AddVertex(DrawMode.TriangleStrip, position, colour, uv, matrix);
				}
			}
		}
	}
}