using System;
using System.Runtime.InteropServices;

namespace CKGL
{
	public static class Renderer
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct Vertex : IVertex
		{
			public Vector3 Position;
			public Colour Colour;
			public UV UV;
			public byte Textured;

			public Vertex(Vector3 position, Colour colour, UV uv, bool textured)
			{
				Position = position;
				Colour = colour;
				UV = uv;
				Textured = (byte)(textured ? 1 : 0);
			}

			public readonly static VertexAttributeLayout AttributeLayout = new VertexAttributeLayout
			(
				new VertexAttribute(VertexType.Float, 3, false),        // Position
				new VertexAttribute(VertexType.UnsignedByte, 4, true),  // Colour
				new VertexAttribute(VertexType.UnsignedShort, 2, true), // UV
				new VertexAttribute(VertexType.UnsignedByte, 1, false)  // Textured
			);

			VertexAttributeLayout IVertex.AttributeLayout
			{
				get { return AttributeLayout; }
			}
		}

		private static VertexArray vao;
		private static VertexBuffer vbo;
		private static DrawMode currentDrawMode = DrawMode.TriangleList;
		// Adaptive vertex buffer size
		//private static int bufferSize = 16384; // Performant on Laptop
		private static int bufferSize = 1024;
		private const int maxBufferSize = 128000;
		//private const int bufferSize = 1998; // Divisible by 3 and 2 for no vertex wrapping per batch
		private static Vertex[] vertices = new Vertex[bufferSize];
		private static int vertexCount = 0;

		public static void Init()
		{
			vao = new VertexArray();
			vbo = new VertexBuffer();
			vao.AddBuffer(vbo, Vertex.AttributeLayout);

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
		}

		public static void Flush()
		{
			if (
				(currentDrawMode == DrawMode.TriangleList && vertexCount >= 3) ||
				(currentDrawMode == DrawMode.TriangleStrip && vertexCount >= 3) ||
				(currentDrawMode == DrawMode.TriangleFan && vertexCount >= 3) ||
				(currentDrawMode == DrawMode.LineList && vertexCount >= 2) ||
				(currentDrawMode == DrawMode.LineStrip && vertexCount >= 2) ||
				(currentDrawMode == DrawMode.LineLoop && vertexCount >= 2) ||
				(currentDrawMode == DrawMode.PointList && vertexCount >= 1)
			)
			{
				vao.Bind();
				vbo.LoadData(Vertex.AttributeLayout, ref vertices, vertexCount, BufferUsage.DynamicDraw);
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

		private static void AddVertex(DrawMode type, Vector3 position, Colour? colour, UV? uv)
		{
			if (currentDrawMode != type)
			{
				Flush();
				currentDrawMode = type;
				// We can lose vertices here, but it's ok as we're switching primitive types anyways
				vertexCount = 0;
			}

			if (vertexCount >= bufferSize)
			{
				Flush();

				// Adaptive vertex buffer size
				if (bufferSize < maxBufferSize)
				{
					bufferSize *= 2;
					if (bufferSize > maxBufferSize)
						bufferSize = maxBufferSize;

					Array.Resize(ref vertices, bufferSize);

					Output.WriteLine($"Renderer - VertexBuffer size: {bufferSize:n0}");
				}
			}

			vertices[vertexCount] = new Vertex(position, colour ?? Colour.White, uv ?? UV.Zero, uv != null);
			vertexCount++;
		}

		#region Draw
		public static class Draw
		{
			#region Depth
			private static float depth = 0f;

			public static void SetDepth(float depth)
			{
				Draw.depth = depth;
			}

			public static void ResetDepth()
			{
				depth = 0f;
			}
			#endregion

			#region Transform
			private static Transform2D transform = null;

			public static void SetTransform(Transform2D transform)
			{
				Draw.transform = transform;
			}

			public static void ResetTransform()
			{
				transform = null;
			}
			#endregion

			#region AddVertex
			public static void AddVertex(DrawMode type, Vector2 position) => AddVertex(type, position, null, null);
			public static void AddVertex(DrawMode type, Vector2 position, Colour? colour) => AddVertex(type, position, colour, null);
			public static void AddVertex(DrawMode type, Vector2 position, Colour? colour, UV? uv)
			{
				Vector3 positionDepth = new Vector3(position.X, position.Y, depth);
				Renderer.AddVertex(type, positionDepth * transform?.Matrix ?? positionDepth, colour, uv);
			}
			public static void AddVertex(DrawMode type, Vector2 position, Colour? colour, UV? uv, float rotation, Vector2? origin)
			{
				if (rotation != 0f)
					AddVertex(type, position * (Matrix2D.CreateTranslation(-origin ?? Vector2.Zero) * Matrix2D.CreateRotationZ(rotation) * Matrix2D.CreateTranslation(origin ?? Vector2.Zero)), colour, uv);
				else
					AddVertex(type, position, colour, uv);
			}
			#endregion

			#region Triangle
			public static void Triangle(Vector2 v1, Vector2 v2, Vector2 v3) => Triangle(v1, v2, v3, null, null, null, null, null, null);
			public static void Triangle(Vector2 v1, Vector2 v2, Vector2 v3, Colour? colour) => Triangle(v1, v2, v3, colour, colour, colour, null, null, null);
			public static void Triangle(Vector2 v1, Vector2 v2, Vector2 v3, Colour? c1, Colour? c2, Colour? c3) => Triangle(v1, v2, v3, c1, c2, c3, null, null, null);
			public static void Triangle(Vector2 v1, Vector2 v2, Vector2 v3, Colour? c1, Colour? c2, Colour? c3, UV? uv1, UV? uv2, UV? uv3)
			{
				AddVertex(DrawMode.TriangleList, v1, c1, uv1);
				AddVertex(DrawMode.TriangleList, v2, c2, uv2);
				AddVertex(DrawMode.TriangleList, v3, c3, uv3);
			}
			public static void Triangle(Vector2 v1, Vector2 v2, Vector2 v3, Colour? c1, Colour? c2, Colour? c3, UV? uv1, UV? uv2, UV? uv3, float rotation, Vector2? origin)
			{
				if (rotation != 0f)
				{
					AddVertex(DrawMode.TriangleList, v1, c1, uv1, rotation, origin);
					AddVertex(DrawMode.TriangleList, v2, c2, uv2, rotation, origin);
					AddVertex(DrawMode.TriangleList, v3, c3, uv3, rotation, origin);
				}
				else
				{
					Triangle(v1, v2, v3, c1, c2, c3, uv1, uv2, uv3);
				}
			}
			#endregion

			#region Rectangle
			public static void Rectangle(float x, float y, float width, float height) => Rectangle(x, y, width, height, null, null, null, null, null, null, null, null);
			public static void Rectangle(float x, float y, float width, float height, Colour? colour) => Rectangle(x, y, width, height, colour, colour, colour, colour, null, null, null, null);
			public static void Rectangle(float x, float y, float width, float height, Colour? c1, Colour? c2, Colour? c3, Colour? c4) => Rectangle(x, y, width, height, c1, c2, c3, c4, null, null, null, null);
			public static void Rectangle(float x, float y, float width, float height, Colour? c1, Colour? c2, Colour? c3, Colour? c4, UV? uv1, UV? uv2, UV? uv3, UV? uv4) => Rectangle(new Vector2(x, y), new Vector2(x + width, y), new Vector2(x, y + height), new Vector2(x + width, y + height), c1, c2, c3, c4, uv1, uv2, uv3, uv4);

			public static void Rectangle(float x, float y, float width, float height, float rotation, Vector2? origin) => Rectangle(x, y, width, height, null, null, null, null, null, null, null, null, rotation, origin);
			public static void Rectangle(float x, float y, float width, float height, Colour? colour, float rotation, Vector2? origin) => Rectangle(x, y, width, height, colour, colour, colour, colour, null, null, null, null, rotation, origin);
			public static void Rectangle(float x, float y, float width, float height, Colour? c1, Colour? c2, Colour? c3, Colour? c4, float rotation, Vector2? origin) => Rectangle(x, y, width, height, c1, c2, c3, c4, null, null, null, null, rotation, origin);
			public static void Rectangle(float x, float y, float width, float height, Colour? c1, Colour? c2, Colour? c3, Colour? c4, UV? uv1, UV? uv2, UV? uv3, UV? uv4, float rotation, Vector2? origin) => Rectangle(new Vector2(x, y), new Vector2(x + width, y), new Vector2(x, y + height), new Vector2(x + width, y + height), c1, c2, c3, c4, uv1, uv2, uv3, uv4, rotation, origin);

			public static void Rectangle(Vector2 v1, Vector2 v2) => Rectangle(v1, v2, null, null, null, null, null, null, null, null);
			public static void Rectangle(Vector2 v1, Vector2 v2, Colour? colour) => Rectangle(v1, v2, colour, colour, colour, colour, null, null, null, null);
			public static void Rectangle(Vector2 v1, Vector2 v2, Colour? c1, Colour? c2, Colour? c3, Colour? c4) => Rectangle(v1, v2, c1, c2, c3, c4, null, null, null, null);
			public static void Rectangle(Vector2 v1, Vector2 v2, Colour? c1, Colour? c2, Colour? c3, Colour? c4, UV? uv1, UV? uv2, UV? uv3, UV? uv4) => Rectangle(new Vector2(v1.X, v1.Y), new Vector2(v2.X, v1.Y), new Vector2(v1.X, v2.Y), new Vector2(v2.X, v2.Y), c1, c2, c3, c4, uv1, uv2, uv3, uv4);
			public static void Rectangle(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Colour? c1, Colour? c2, Colour? c3, Colour? c4, UV? uv1, UV? uv2, UV? uv3, UV? uv4)
			{
				AddVertex(DrawMode.TriangleList, v1, c1, uv1);
				AddVertex(DrawMode.TriangleList, v2, c2, uv2);
				AddVertex(DrawMode.TriangleList, v3, c3, uv3);
				AddVertex(DrawMode.TriangleList, v3, c3, uv3);
				AddVertex(DrawMode.TriangleList, v2, c2, uv2);
				AddVertex(DrawMode.TriangleList, v4, c4, uv4);
			}

			public static void Rectangle(Vector2 v1, Vector2 v2, float rotation, Vector2 origin) => Rectangle(v1, v2, null, null, null, null, null, null, null, null, rotation, origin);
			public static void Rectangle(Vector2 v1, Vector2 v2, Colour? colour, float rotation, Vector2 origin) => Rectangle(v1, v2, colour, colour, colour, colour, null, null, null, null, rotation, origin);
			public static void Rectangle(Vector2 v1, Vector2 v2, Colour? c1, Colour? c2, Colour? c3, Colour? c4, float rotation, Vector2 origin) => Rectangle(v1, v2, c1, c2, c3, c4, null, null, null, null, rotation, origin);
			public static void Rectangle(Vector2 v1, Vector2 v2, Colour? c1, Colour? c2, Colour? c3, Colour? c4, UV? uv1, UV? uv2, UV? uv3, UV? uv4, float rotation, Vector2 origin) => Rectangle(new Vector2(v1.X, v1.Y), new Vector2(v2.X, v1.Y), new Vector2(v1.X, v2.Y), new Vector2(v2.X, v2.Y), c1, c2, c3, c4, uv1, uv2, uv3, uv4, rotation, origin);
			public static void Rectangle(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Colour? c1, Colour? c2, Colour? c3, Colour? c4, UV? uv1, UV? uv2, UV? uv3, UV? uv4, float rotation, Vector2? origin)
			{
				if (rotation != 0f)
				{
					AddVertex(DrawMode.TriangleList, v1, c1, uv1, rotation, origin);
					AddVertex(DrawMode.TriangleList, v2, c2, uv2, rotation, origin);
					AddVertex(DrawMode.TriangleList, v3, c3, uv3, rotation, origin);
					AddVertex(DrawMode.TriangleList, v3, c3, uv3, rotation, origin);
					AddVertex(DrawMode.TriangleList, v2, c2, uv2, rotation, origin);
					AddVertex(DrawMode.TriangleList, v4, c4, uv4, rotation, origin);
				}
				else
				{
					Rectangle(v1, v2, v3, v4, c1, c2, c3, c4, uv1, uv2, uv3, uv4);
				}
			}
			#endregion

			#region Sprite
			public static void Sprite(Sprite sprite, Vector2 position) => Sprite(sprite, position, null, null);
			public static void Sprite(Sprite sprite, Vector2 position, Vector2? scale) => Sprite(sprite, position, scale, null);
			public static void Sprite(Sprite sprite, Vector2 position, Colour? colour) => Sprite(sprite, position, null, colour);
			public static void Sprite(Sprite sprite, Vector2 position, Vector2? scale, Colour? colour)
			{
				sprite.SpriteSheet.Texture.Bind();
				if (scale.HasValue)
					Rectangle(position.X, position.Y, sprite.Width * scale.Value.X, sprite.Height * scale.Value.Y, colour, colour, colour, colour, sprite.UV_BL, sprite.UV_BR, sprite.UV_TL, sprite.UV_TR);
				else
					Rectangle(position.X, position.Y, sprite.Width, sprite.Height, colour, colour, colour, colour, sprite.UV_BL, sprite.UV_BR, sprite.UV_TL, sprite.UV_TR);
			}
			public static void Sprite(Sprite sprite, Vector2 position, float rotation, Vector2? origin) => Sprite(sprite, position, null, null, rotation, origin);
			public static void Sprite(Sprite sprite, Vector2 position, Vector2? scale, float rotation, Vector2? origin) => Sprite(sprite, position, scale, null, rotation, origin);
			public static void Sprite(Sprite sprite, Vector2 position, Colour? colour, float rotation, Vector2? origin) => Sprite(sprite, position, null, colour, rotation, origin);
			public static void Sprite(Sprite sprite, Vector2 position, Vector2? scale, Colour? colour, float rotation, Vector2? origin)
			{
				sprite.SpriteSheet.Texture.Bind();
				if (scale.HasValue)
					Rectangle(position.X, position.Y, sprite.Width * scale.Value.X, sprite.Height * scale.Value.Y, colour, colour, colour, colour, sprite.UV_BL, sprite.UV_BR, sprite.UV_TL, sprite.UV_TR, rotation, origin);
				else
					Rectangle(position.X, position.Y, sprite.Width, sprite.Height, colour, colour, colour, colour, sprite.UV_BL, sprite.UV_BR, sprite.UV_TL, sprite.UV_TR, rotation, origin);
			}
			#endregion

			#region Text
			public static void Text(SpriteFont font, string text, Vector2 position, Vector2 scale, Colour colour, HAlign hAlign = HAlign.Left, VAlign vAlign = VAlign.Top)
			{
				font.SpriteSheet.Texture.Bind();

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

				float lineHeight = font.LineHeight * scale.Y;
				float totalHeight = lines.Length * lineHeight;

				bool mod = false;
				string modData = "";
				// Shadow
				bool modShadow = false;
				Vector2 modShadowOffset = Vector2.Zero;
				float modShadowDepth = 0f;
				Colour modShadowColour = Colour.White;
				// Outline
				bool modOutline = false;
				bool modOutlineCorners = false;
				float modOutlineDepth = 0f;
				Colour modOutlineColour = Colour.White;

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
								lineWidth += font.SpaceWidth * scale.X;
							else if (c == '\a')
								mod = true;
							else
								lineWidth += (font.Glyph(c).Width + font.CharSpacing) * scale.X;
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

										if (value.Length == 7)
										{
											try
											{
												modShadow = true;
												modShadowOffset = new Vector2(float.Parse(value[0]), float.Parse(value[1])) * scale;
												modShadowDepth = float.Parse(value[2]);
												modShadowColour = new Colour(float.Parse(value[3]), float.Parse(value[4]), float.Parse(value[5]), float.Parse(value[6]));
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
									else if (tag[0] == "outline")
									{
										string[] value = tag[1].Split(',');

										if (value.Length == 6)
										{
											try
											{
												modOutline = true;
												modOutlineCorners = float.Parse(value[0]) != 0;
												modOutlineDepth = float.Parse(value[1]);
												modOutlineColour = new Colour(float.Parse(value[2]), float.Parse(value[3]), float.Parse(value[4]), float.Parse(value[5]));
											}
											catch
											{
												throw new Exception("Text modifier value parse error.");
											}
										}
										else if (value.Length == 1 && (value[0] == "disable" || value[0] == "off"))
										{
											modOutline = false;
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
								offsetX += font.SpaceWidth;
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
									float currentDepth = depth;
									SetDepth(currentDepth + modShadowDepth);
									Rectangle(position.X + modShadowOffset.X + offsetX - lineWidth * offsetHAlign,
											  position.Y + modShadowOffset.Y + offsetY + (totalHeight - lineHeight) - totalHeight * offsetVAlign,
											  sprite.Width * scale.X,
											  sprite.Height * scale.Y,
											  modShadowColour, modShadowColour, modShadowColour, modShadowColour,
											  sprite.UV_BL, sprite.UV_BR, sprite.UV_TL, sprite.UV_TR);
									SetDepth(currentDepth);
								}

								if (modOutline)
								{
									float currentDepth = depth;
									SetDepth(currentDepth + modOutlineDepth);
									Rectangle(position.X - scale.X + offsetX - lineWidth * offsetHAlign,
											  position.Y + offsetY + (totalHeight - lineHeight) - totalHeight * offsetVAlign,
											  sprite.Width * scale.X,
											  sprite.Height * scale.Y,
											  modOutlineColour, modOutlineColour, modOutlineColour, modOutlineColour,
											  sprite.UV_BL, sprite.UV_BR, sprite.UV_TL, sprite.UV_TR);
									Rectangle(position.X + scale.X + offsetX - lineWidth * offsetHAlign,
											  position.Y + offsetY + (totalHeight - lineHeight) - totalHeight * offsetVAlign,
											  sprite.Width * scale.X,
											  sprite.Height * scale.Y,
											  modOutlineColour, modOutlineColour, modOutlineColour, modOutlineColour,
											  sprite.UV_BL, sprite.UV_BR, sprite.UV_TL, sprite.UV_TR);
									Rectangle(position.X + offsetX - lineWidth * offsetHAlign,
											  position.Y - scale.Y + offsetY + (totalHeight - lineHeight) - totalHeight * offsetVAlign,
											  sprite.Width * scale.X,
											  sprite.Height * scale.Y,
											  modOutlineColour, modOutlineColour, modOutlineColour, modOutlineColour,
											  sprite.UV_BL, sprite.UV_BR, sprite.UV_TL, sprite.UV_TR);
									Rectangle(position.X + offsetX - lineWidth * offsetHAlign,
											  position.Y + scale.Y + offsetY + (totalHeight - lineHeight) - totalHeight * offsetVAlign,
											  sprite.Width * scale.X,
											  sprite.Height * scale.Y,
											  modOutlineColour, modOutlineColour, modOutlineColour, modOutlineColour,
											  sprite.UV_BL, sprite.UV_BR, sprite.UV_TL, sprite.UV_TR);
									if (modOutlineCorners)
									{
										Rectangle(position.X - scale.X + offsetX - lineWidth * offsetHAlign,
												  position.Y - scale.Y + offsetY + (totalHeight - lineHeight) - totalHeight * offsetVAlign,
												  sprite.Width * scale.X,
												  sprite.Height * scale.Y,
												  modOutlineColour, modOutlineColour, modOutlineColour, modOutlineColour,
												  sprite.UV_BL, sprite.UV_BR, sprite.UV_TL, sprite.UV_TR);
										Rectangle(position.X + scale.X + offsetX - lineWidth * offsetHAlign,
												  position.Y - scale.Y + offsetY + (totalHeight - lineHeight) - totalHeight * offsetVAlign,
												  sprite.Width * scale.X,
												  sprite.Height * scale.Y,
												  modOutlineColour, modOutlineColour, modOutlineColour, modOutlineColour,
												  sprite.UV_BL, sprite.UV_BR, sprite.UV_TL, sprite.UV_TR);
										Rectangle(position.X - scale.X + offsetX - lineWidth * offsetHAlign,
												  position.Y + scale.Y + offsetY + (totalHeight - lineHeight) - totalHeight * offsetVAlign,
												  sprite.Width * scale.X,
												  sprite.Height * scale.Y,
												  modOutlineColour, modOutlineColour, modOutlineColour, modOutlineColour,
												  sprite.UV_BL, sprite.UV_BR, sprite.UV_TL, sprite.UV_TR);
										Rectangle(position.X + scale.X + offsetX - lineWidth * offsetHAlign,
												  position.Y + scale.Y + offsetY + (totalHeight - lineHeight) - totalHeight * offsetVAlign,
												  sprite.Width * scale.X,
												  sprite.Height * scale.Y,
												  modOutlineColour, modOutlineColour, modOutlineColour, modOutlineColour,
												  sprite.UV_BL, sprite.UV_BR, sprite.UV_TL, sprite.UV_TR);
									}
									SetDepth(currentDepth);
								}

								Rectangle(position.X + offsetX - lineWidth * offsetHAlign,
										  position.Y + offsetY + (totalHeight - lineHeight) - totalHeight * offsetVAlign,
										  sprite.Width * scale.X,
										  sprite.Height * scale.Y,
										  colour, colour, colour, colour,
										  sprite.UV_BL, sprite.UV_BR, sprite.UV_TL, sprite.UV_TR);

								offsetX += (sprite.Width + font.CharSpacing) * scale.X;
							}
						}
					}

					offsetX = 0;
					offsetY -= lineHeight;
				}
			}
			#endregion

			// TODO - Fix RenderTarget method cascading
			#region RenderTarget
			private static void RenderTargetBindTexture(RenderTarget renderTarget, RenderTarget.TextureSlot textureSlot)
			{
				if (renderTarget == null)
					throw new Exception("Can't bind default framebuffer textures, use a RenderTarget instead.");

				renderTarget.GetTexture(textureSlot).Bind();
			}
			public static void RenderTarget(RenderTarget renderTarget, RenderTarget.TextureSlot textureSlot, float x, float y, Colour colour)
			{
				RenderTargetBindTexture(renderTarget, textureSlot);
				Rectangle(x, y, (renderTarget ?? CKGL.RenderTarget.Default).Width, (renderTarget ?? CKGL.RenderTarget.Default).Height, colour, colour, colour, colour, UV.BottomLeft, UV.BottomRight, UV.TopLeft, UV.TopRight);
			}
			public static void RenderTarget(RenderTarget renderTarget, RenderTarget.TextureSlot textureSlot, float x, float y, float rotation, Vector2 origin, Colour colour)
			{
				RenderTargetBindTexture(renderTarget, textureSlot);
				Rectangle(x, y, (renderTarget ?? CKGL.RenderTarget.Default).Width, (renderTarget ?? CKGL.RenderTarget.Default).Height, colour, colour, colour, colour, UV.BottomLeft, UV.BottomRight, UV.TopLeft, UV.TopRight, rotation, origin);
			}
			public static void RenderTarget(RenderTarget renderTarget, RenderTarget.TextureSlot textureSlot, float x, float y, float scale, Colour colour)
			{
				RenderTargetBindTexture(renderTarget, textureSlot);
				Rectangle(x, y, (renderTarget ?? CKGL.RenderTarget.Default).Width * scale, (renderTarget ?? CKGL.RenderTarget.Default).Height * scale, colour, colour, colour, colour, UV.BottomLeft, UV.BottomRight, UV.TopLeft, UV.TopRight);
			}
			public static void RenderTarget(RenderTarget renderTarget, RenderTarget.TextureSlot textureSlot, float x, float y, float scale, float rotation, Vector2 origin, Colour colour)
			{
				RenderTargetBindTexture(renderTarget, textureSlot);
				Rectangle(x, y, (renderTarget ?? CKGL.RenderTarget.Default).Width * scale, (renderTarget ?? CKGL.RenderTarget.Default).Height * scale, colour, colour, colour, colour, UV.BottomLeft, UV.BottomRight, UV.TopLeft, UV.TopRight, rotation, origin);
			}
			public static void RenderTarget(RenderTarget renderTarget, RenderTarget.TextureSlot textureSlot, float x, float y, float width, float height, Colour colour)
			{
				RenderTargetBindTexture(renderTarget, textureSlot);
				Rectangle(x, y, width, height, colour, colour, colour, colour, UV.BottomLeft, UV.BottomRight, UV.TopLeft, UV.TopRight);
			}
			public static void RenderTarget(RenderTarget renderTarget, RenderTarget.TextureSlot textureSlot, Vector2 v1, Vector2 v2, Colour colour)
			{
				RenderTargetBindTexture(renderTarget, textureSlot);
				Rectangle(new Vector2(v1.X, v1.Y), new Vector2(v2.X, v2.Y), colour, colour, colour, colour, UV.BottomLeft, UV.BottomRight, UV.TopLeft, UV.TopRight);
			}
			#endregion

			#region Circle
			public static void Circle(Vector2 center, float radius, int circleSegments = 16) => Circle(center, radius, null, null, circleSegments);
			public static void Circle(Vector2 center, float radius, Colour? colour, int circleSegments = 16) => Circle(center, radius, colour, colour, circleSegments);
			public static void Circle(Vector2 center, float radius, Colour? c1, Colour? c2, int circleSegments = 16)
			{
				var increment = Math.PI * 2.0f / circleSegments;
				var theta = 0.0f;

				var v0 = center;
				theta += increment;

				for (int i = 0; i < circleSegments; i++)
				{
					var v1 = center + radius * new Vector2(Math.Cos(theta), Math.Sin(theta));
					var v2 = center + radius * new Vector2(Math.Cos(theta + increment), Math.Sin(theta + increment));

					AddVertex(DrawMode.TriangleList, v0, c1);
					AddVertex(DrawMode.TriangleList, v1, c2);
					AddVertex(DrawMode.TriangleList, v2, c2);

					theta += increment;
				}
			}
			#endregion

			#region Point
			public static void Point(float x, float y) => Point(x, y, null);
			public static void Point(float x, float y, Colour? c) => Point(new Vector2(x, y), c);
			public static void Point(Vector2 v) => Point(v, null);
			public static void Point(Vector2 v, Colour? c)
			{
				AddVertex(DrawMode.PointList, v, c);
			}
			#endregion

			#region PolyPoint
			public static void PolyPoint(int x, int y, Colour colour)
			{
				Rectangle(new Vector2(x, y), new Vector2(x + 1, y + 1), colour);
			}
			public static void PolyPoint(float x, float y, Colour colour)
			{
				Rectangle(new Vector2(x, y), new Vector2(x + 1, y + 1), colour);
			}
			public static void PolyPoint(Vector2 vector, Colour colour)
			{
				Rectangle(vector, vector + Vector2.One, colour);
			}
			#endregion

			#region Line
			public static void Line(float x1, float y1, float x2, float y2) => Line(new Vector2(x1, y1), new Vector2(x2, y2), null, null, null, null);
			public static void Line(float x1, float y1, float x2, float y2, Colour? colour) => Line(new Vector2(x1, y1), new Vector2(x2, y2), colour, colour, null, null);
			public static void Line(float x1, float y1, float x2, float y2, Colour? c1, Colour? c2) => Line(new Vector2(x1, y1), new Vector2(x2, y2), c1, c2, null, null);
			public static void Line(float x1, float y1, float x2, float y2, Colour? c1, Colour? c2, UV? uv1, UV? uv2) => Line(new Vector2(x1, y1), new Vector2(x2, y2), c1, c2, uv1, uv2);

			public static void Line(Vector2 v1, Vector2 v2) => Line(v1, v2, null, null, null, null);
			public static void Line(Vector2 v1, Vector2 v2, Colour? colour) => Line(v1, v2, colour, colour, null, null);
			public static void Line(Vector2 v1, Vector2 v2, Colour? c1, Colour? c2) => Line(v1, v2, c1, c2, null, null);
			public static void Line(Vector2 v1, Vector2 v2, Colour? c1, Colour? c2, UV? uv1, UV? uv2)
			{
				AddVertex(DrawMode.LineList, v1, c1, uv1);
				AddVertex(DrawMode.LineList, v2, c2, uv2);
			}
			#endregion

			// TODO - Fix PolyLine method cascading
			#region PolyLine
			public static void PolyLine(float x1, float y1, float x2, float y2, Colour colour)
			{
				PolyLine(x1, y1, x2, y2, colour, colour, 1f);
			}
			public static void PolyLine(float x1, float y1, float x2, float y2, Colour colour, float width)
			{
				PolyLine(x1, y1, x2, y2, colour, colour, width);
			}

			public static void PolyLine(float x1, float y1, float x2, float y2, Colour c1, Colour c2)
			{
				PolyLine(x1, y1, x2, y2, c1, c2, 1f);
			}
			public static void PolyLine(float x1, float y1, float x2, float y2, Colour c1, Colour c2, float width)
			{
				PolyLine(new Vector2(x1, y1), new Vector2(x2, y2), c1, c2, width);
			}

			public static void PolyLine(Vector2 v1, Vector2 v2, Colour colour)
			{
				PolyLine(v1, v2, colour, colour, 1f);
			}
			public static void PolyLine(Vector2 v1, Vector2 v2, Colour colour, float width)
			{
				PolyLine(v1, v2, colour, colour, width);
			}

			public static void PolyLine(Vector2 v1, Vector2 v2, Colour c1, Colour c2)
			{
				PolyLine(v1, v2, c1, c2, 1f);
			}
			public static void PolyLine(Vector2 v1, Vector2 v2, Colour c1, Colour c2, float width)
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
			#endregion

			#region LineStrip
			public static class LineStrip
			{
				public static void AddVertex(Vector2 position)
				{
					Draw.AddVertex(DrawMode.LineStrip, position);
				}
				public static void AddVertex(Vector2 position, Colour? colour)
				{
					Draw.AddVertex(DrawMode.LineStrip, position, colour);
				}
				public static void AddVertex(Vector2 position, Colour? colour, UV? uv)
				{
					Draw.AddVertex(DrawMode.LineStrip, position, colour, uv);
				}
				public static void AddVertex(Vector2 position, Colour? colour, UV? uv, float rotation, Vector2? origin)
				{
					Draw.AddVertex(DrawMode.LineStrip, position, colour, uv, rotation, origin);
				}
			}
			#endregion

			// TODO - LineListStrip
			//public static class LineListStrip
			//{
			//	private static bool working = false;
			//	private static Vertex? lastVertex = null;

			//	public static void Begin()
			//	{
			//		if (working)
			//			throw new Exception("End must be called before Begin can be called again.");

			//		working = true;
			//		lastVertex = null;
			//	}

			//	public static void End()
			//	{
			//		if (!working)
			//			throw new Exception("Begin must be called before End can be called.");

			//		working = false;
			//		lastVertex = null;
			//	}

			//	public static void AddVertex(Vector2 position, Colour colour)
			//	{
			//		AddVertex(position, colour);
			//	}
			//	public static void AddVertex(Vector2 position, Colour colour)
			//	{
			//		AddVertex(position, colour, Vector2.Zero);
			//	}
			//	public static void AddVertex(Vector2 position, Colour colour, UV uv)
			//	{
			//		if (!working)
			//			throw new Exception("Begin must be called before AddVertex can be called.");

			//		if (lastVertex.HasValue)
			//		{
			//			Draw.AddVertex(DrawMode.LineList,
			//							   new Vector2(lastVertex.Value.Position.X, lastVertex.Value.Position.Y),
			//							   lastVertex.Value.Colour,
			//							   lastVertex.Value.Textured,
			//							   lastVertex.Value.UV);
			//			Draw.AddVertex(DrawMode.LineList, position, colour, uv);
			//		}

			//		lastVertex = new Vertex(new Vector3(position.X, position.Y, 0f), colour, uv);
			//	}
			//}

			#region TriangleStrip
			public static class TriangleStrip
			{
				public static void AddVertex(Vector2 position)
				{
					Draw.AddVertex(DrawMode.TriangleStrip, position);
				}
				public static void AddVertex(Vector2 position, Colour? colour)
				{
					Draw.AddVertex(DrawMode.TriangleStrip, position, colour);
				}
				public static void AddVertex(Vector2 position, Colour? colour, UV? uv)
				{
					Draw.AddVertex(DrawMode.TriangleStrip, position, colour, uv);
				}
				public static void AddVertex(Vector2 position, Colour? colour, UV? uv, float rotation, Vector2? origin)
				{
					Draw.AddVertex(DrawMode.TriangleStrip, position, colour, uv, rotation, origin);
				}
			}
			#endregion

			// TODO - TriangleListStrip
			//public static class TriangleListStrip
			//{
			//	private static bool working = false;
			//	private static Vertex? lastVertex = null;
			//	private static Vertex? lastLastVertex = null;

			//	public static void Begin()
			//	{
			//		if (working)
			//			throw new Exception("End must be called before Begin can be called again.");

			//		working = true;
			//		lastVertex = null;
			//		lastLastVertex = null;
			//	}

			//	public static void End()
			//	{
			//		if (!working)
			//			throw new Exception("Begin must be called before End can be called.");

			//		working = false;
			//		lastVertex = null;
			//		lastLastVertex = null;
			//	}

			//	public static void AddVertex(Vector2 position, Colour colour)
			//	{
			//		AddVertex(position, colour);
			//	}
			//	public static void AddVertex(Vector2 position, Colour colour)
			//	{
			//		AddVertex(position, colour, Vector2.Zero);
			//	}
			//	public static void AddVertex(Vector2 position, Colour colour, UV uv)
			//	{
			//		if (!working)
			//			throw new Exception("Begin must be called before AddVertex can be called.");

			//		if (lastLastVertex.HasValue && lastVertex.HasValue)
			//		{
			//			Draw.AddVertex(DrawMode.TriangleList,
			//							   new Vector2(lastLastVertex.Value.Position.X, lastLastVertex.Value.Position.Y),
			//							   lastLastVertex.Value.Colour,
			//							   lastLastVertex.Value.Textured,
			//							   lastLastVertex.Value.UV);
			//			Draw.AddVertex(DrawMode.TriangleList,
			//							   new Vector2(lastVertex.Value.Position.X, lastVertex.Value.Position.Y),
			//							   lastVertex.Value.Colour,
			//							   lastVertex.Value.Textured,
			//							   lastVertex.Value.UV);
			//			Draw.AddVertex(DrawMode.TriangleList, position, colour, uv);
			//		}

			//		lastLastVertex = lastVertex;
			//		lastVertex = new Vertex(new Vector3(position.X, position.Y, 0), colour, uv);
			//	}
			//}
		}

		#region Draw3D
		public static class Draw3D
		{
			#region Transform
			private static Transform transform = null;

			public static void SetTransform(Transform transform)
			{
				Draw3D.transform = transform;
			}

			public static void ResetTransform()
			{
				transform = null;
			}
			#endregion

			#region AddVertex
			public static void AddVertex(DrawMode type, Vector3 position) => AddVertex(type, position, null, null);
			public static void AddVertex(DrawMode type, Vector3 position, Colour? colour) => AddVertex(type, position, colour, null);
			public static void AddVertex(DrawMode type, Vector3 position, Colour? colour, UV? uv)
			{
				Renderer.AddVertex(type, position * transform?.Matrix ?? position, colour, uv);
			}
			#endregion

			#region Triangle
			public static void Triangle(Vector3 v1, Vector3 v2, Vector3 v3) => Triangle(v1, v2, v3, null, null, null, null, null, null);
			public static void Triangle(Vector3 v1, Vector3 v2, Vector3 v3, Colour? colour) => Triangle(v1, v2, v3, colour, colour, colour, null, null, null);
			public static void Triangle(Vector3 v1, Vector3 v2, Vector3 v3, Colour? c1, Colour? c2, Colour? c3) => Triangle(v1, v2, v3, c1, c2, c3, null, null, null);
			public static void Triangle(Vector3 v1, Vector3 v2, Vector3 v3, Colour? c1, Colour? c2, Colour? c3, UV? uv1, UV? uv2, UV? uv3)
			{
				AddVertex(DrawMode.TriangleList, v1, c1, uv1);
				AddVertex(DrawMode.TriangleList, v2, c2, uv2);
				AddVertex(DrawMode.TriangleList, v3, c3, uv3);
			}
			#endregion

			#region Rectangle
			public static void Rectangle(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) => Rectangle(v1, v2, v3, v4, null, null, null, null, null, null, null, null);
			public static void Rectangle(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Colour? colour) => Rectangle(v1, v2, v3, v4, colour, colour, colour, colour, null, null, null, null);
			public static void Rectangle(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Colour? colour, UV? uv1, UV? uv2, UV? uv3, UV? uv4) => Rectangle(v1, v2, v3, v4, colour, colour, colour, colour, uv1, uv2, uv3, uv4);
			public static void Rectangle(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Colour? c1, Colour? c2, Colour? c3, Colour? c4) => Rectangle(v1, v2, v3, v4, c1, c2, c3, c4, null, null, null, null);
			public static void Rectangle(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Colour? c1, Colour? c2, Colour? c3, Colour? c4, UV? uv1, UV? uv2, UV? uv3, UV? uv4)
			{
				AddVertex(DrawMode.TriangleList, v1, c1, uv1);
				AddVertex(DrawMode.TriangleList, v2, c2, uv2);
				AddVertex(DrawMode.TriangleList, v3, c3, uv3);
				AddVertex(DrawMode.TriangleList, v3, c3, uv3);
				AddVertex(DrawMode.TriangleList, v2, c2, uv2);
				AddVertex(DrawMode.TriangleList, v4, c4, uv4);
			}
			#endregion

			#region Point
			public static void Point(float x, float y, float z) => Point(x, y, z, null);
			public static void Point(float x, float y, float z, Colour? c) => Point(new Vector3(x, y, z), c);
			public static void Point(Vector3 v) => Point(v, null);
			public static void Point(Vector3 v, Colour? c)
			{
				AddVertex(DrawMode.PointList, v, c);
			}
			#endregion

			#region Line
			public static void Line(Vector3 v1, Vector3 v2) => Line(v1, v2, null, null, null, null);
			public static void Line(Vector3 v1, Vector3 v2, Colour? colour) => Line(v1, v2, colour, colour, null, null);
			public static void Line(Vector3 v1, Vector3 v2, Colour? c1, Colour? c2) => Line(v1, v2, c1, c2, null, null);
			public static void Line(Vector3 v1, Vector3 v2, Colour? c1, Colour? c2, UV? uv1, UV? uv2)
			{
				AddVertex(DrawMode.LineList, v1, c1, uv1);
				AddVertex(DrawMode.LineList, v2, c2, uv2);
			}
			#endregion

			#region Cube
			public static void Cube(Colour? colour) => Cube(colour, colour, colour, colour, colour, colour);
			public static void Cube(Colour? l, Colour? r, Colour? d, Colour? u, Colour? b, Colour? f)
			{
				Vector3 ldb = new Vector3(-1f, -1f, -1f);
				Vector3 rdb = new Vector3(1f, -1f, -1f);
				Vector3 lub = new Vector3(-1f, 1f, -1f);
				Vector3 rub = new Vector3(1f, 1f, -1f);
				Vector3 ldf = new Vector3(-1f, -1f, 1f);
				Vector3 rdf = new Vector3(1f, -1f, 1f);
				Vector3 luf = new Vector3(-1f, 1f, 1f);
				Vector3 ruf = new Vector3(1f, 1f, 1f);

				Rectangle(ldf, ldb, luf, lub, l, l, l, l); // Left
				Rectangle(rdb, rdf, rub, ruf, r, r, r, r); // Right
				Rectangle(rdb, ldb, rdf, ldf, d, d, d, d); // Down
				Rectangle(lub, rub, luf, ruf, u, u, u, u); // Up
				Rectangle(ldb, rdb, lub, rub, b, b, b, b); // Back
				Rectangle(rdf, ldf, ruf, luf, f, f, f, f); // Front
			}

			//public static void Cube(Colour? colour) => Cube(colour, colour, colour, colour, colour, colour, colour, colour);
			public static void Cube(Colour? c_ldb, Colour? c_rdb, Colour? c_lub, Colour? c_rub, Colour? c_ldf, Colour? c_rdf, Colour? c_luf, Colour? c_ruf)
			{
				Vector3 ldb = new Vector3(-1f, -1f, -1f);
				Vector3 rdb = new Vector3(1f, -1f, -1f);
				Vector3 lub = new Vector3(-1f, 1f, -1f);
				Vector3 rub = new Vector3(1f, 1f, -1f);
				Vector3 ldf = new Vector3(-1f, -1f, 1f);
				Vector3 rdf = new Vector3(1f, -1f, 1f);
				Vector3 luf = new Vector3(-1f, 1f, 1f);
				Vector3 ruf = new Vector3(1f, 1f, 1f);

				Rectangle(ldf, ldb, luf, lub, c_ldf, c_ldb, c_luf, c_lub); // Left
				Rectangle(rdb, rdf, rub, ruf, c_rdb, c_rdf, c_rub, c_ruf); // Right
				Rectangle(rdb, ldb, rdf, ldf, c_rdb, c_ldb, c_rdf, c_ldf); // Bottom
				Rectangle(lub, rub, luf, ruf, c_lub, c_rub, c_luf, c_ruf); // Top
				Rectangle(ldb, rdb, lub, rub, c_ldb, c_rdb, c_lub, c_rub); // Back
				Rectangle(rdf, ldf, ruf, luf, c_rdf, c_ldf, c_ruf, c_luf); // Front
			}

			public static void CubeWireframe(Colour? colour) => CubeWireframe(colour, colour, colour, colour, colour, colour, colour, colour);
			public static void CubeWireframe(Colour? lbb, Colour? rbb, Colour? ltb, Colour? rtb, Colour? lbf, Colour? rbf, Colour? ltf, Colour? rtf)
			{
				Line(new Vector3(-1f, -1f, -1f), new Vector3(1f, -1f, -1f), lbb, rbb);
				Line(new Vector3(1f, -1f, -1f), new Vector3(1f, 1f, -1f), rbb, rtb);
				Line(new Vector3(1f, 1f, -1f), new Vector3(-1f, 1f, -1f), rtb, ltb);
				Line(new Vector3(-1f, 1f, -1f), new Vector3(-1f, -1f, -1f), ltb, lbb);

				Line(new Vector3(-1f, -1f, 1f), new Vector3(1f, -1f, 1f), lbf, rbf);
				Line(new Vector3(1f, -1f, 1f), new Vector3(1f, 1f, 1f), rbf, rtf);
				Line(new Vector3(1f, 1f, 1f), new Vector3(-1f, 1f, 1f), rtf, ltf);
				Line(new Vector3(-1f, 1f, 1f), new Vector3(-1f, -1f, 1f), ltf, lbf);

				Line(new Vector3(-1f, -1f, -1f), new Vector3(-1f, -1f, 1f), lbb, lbf);
				Line(new Vector3(1f, -1f, -1f), new Vector3(1f, -1f, 1f), rbb, rbf);
				Line(new Vector3(1f, 1f, -1f), new Vector3(1f, 1f, 1f), rtb, rtf);
				Line(new Vector3(-1f, 1f, -1f), new Vector3(-1f, 1f, 1f), ltb, ltf);
			}
			#endregion
		}
		#endregion
	}
	#endregion
}