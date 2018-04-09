using System;
using System.Runtime.InteropServices;

namespace CKGL
{
	// TODO - Move these to SpriteFont.cs
	#region Font enums
	public enum HAlign
	{
		Left,
		Center,
		Right
	}

	public enum VAlign
	{
		Top,
		Middle,
		Bottom
	}
	#endregion

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

		private static bool working = false;
		private static VertexArray vao;
		private static VertexBuffer vbo;
		private static VertexBufferLayout vboLayout;
		private static DrawMode currentDrawMode = DrawMode.TriangleList;
		private static Shader DefaultShader { get; } = Shaders.Renderer;
		private static Shader currentShader = DefaultShader;
		private const int bufferSize = 1998; // Divisible by 3 and 2 for no vertex wrapping per batch
		private static Vertex[] vertices = new Vertex[bufferSize];
		private static int vertexCount = 0;

		// TODO - Remove these uv defaults
		private static Vector2 uvFull1 = new Vector2(0f, 0f);
		private static Vector2 uvFull2 = new Vector2(1f, 0f);
		private static Vector2 uvFull3 = new Vector2(0f, 1f);
		private static Vector2 uvFull4 = new Vector2(1f, 1f);

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
		}

		public static void Destroy()
		{
			vao.Destroy();
			vbo.Destroy();

			vao = null;
			vbo = null;
			vboLayout = null;
		}

		// TODO - Render States
		//		private static RasterizerState DefaultRasterizerState { get; } = new RasterizerState
		//		{
		//			CullMode = CullMode.None,
		//			FillMode = FillMode.Solid,
		//			MultiSampleAntiAlias = false,
		//			ScissorTestEnable = false,
		//			SlopeScaleDepthBias = 0,
		//#if LINUX && !FNA
		//			DepthClipEnable = false
		//#elif WINDOWS
		//			DepthClipEnable = true
		//#endif
		//		};

		// TODO - RenderTarget
		//		public static void ResetRenderTarget()
		//		{
		//			SetRenderTarget(null);
		//		}

		// TODO - RenderTarget
		//public static void SetRenderTarget(RenderTarget2D renderTarget2D)
		//{
		//	if ((
		//			renderTarget2D != null &&
		//			Engine.GraphicsDevice.GetRenderTargets().Length == 0
		//		)
		//		||
		//		(
		//			Engine.GraphicsDevice.GetRenderTargets().Length > 0 &&
		//			Engine.GraphicsDevice.GetRenderTargets()[0].RenderTarget != renderTarget2D)
		//		)
		//	{
		//		Flush();
		//		Engine.GraphicsDevice.SetRenderTarget(renderTarget2D);
		//	}
		//}

		#region Clear
		public static void Clear(Colour colour, float depth)
		{
			Graphics.Clear(colour, depth);
		}
		public static void Clear(Colour colour)
		{
			Graphics.Clear(colour);
		}
		public static void Clear(float depth)
		{
			Graphics.Clear(depth);
		}
		#endregion

		#region State
		public static void SetShader(Shader shader)
		{
			if (currentShader != shader)
			{
				Flush();
				shader.Bind();
				currentShader = shader;
			}
		}

		public static void ResetShader()
		{
			SetShader(DefaultShader);
		}

		public static void SetFrontFaceState(FrontFaceState frontFaceState)
		{
			if (Graphics.FrontFaceState != frontFaceState)
			{
				Flush();
				Graphics.FrontFaceState = frontFaceState;
			}
		}

		public static void ResetFrontFaceState()
		{
			SetFrontFaceState(FrontFaceState.Default);
		}

		public static void SetCullState(CullState cullState)
		{
			if (Graphics.CullState != cullState)
			{
				Flush();
				Graphics.CullState = cullState;
			}
		}

		public static void ResetCullState()
		{
			SetCullState(CullState.Default);
		}

		public static void SetBlendState(BlendState blendState)
		{
			if (Graphics.BlendState != blendState)
			{
				Flush();
				Graphics.BlendState = blendState;
			}
		}

		public static void ResetBlendState()
		{
			SetBlendState(BlendState.Default);
		}

		public static void SetDepthState(DepthState depthState)
		{
			if (Graphics.DepthState != depthState)
			{
				Flush();
				Graphics.DepthState = depthState;
			}
		}

		public static void ResetDepthState()
		{
			SetDepthState(DepthState.Default);
		}

		// TODO - Sprites - Make SetTexture private
		public static void SetTexture(Texture2D texture)
		{
			if (!texture.IsBound())
			{
				Flush();
				texture.Bind();
			}
		}
		#endregion

		// TODO - Remove Renderer.Start(), useless method
		public static void Start()
		{
			if (working)
				throw new Exception("End must be called before Begin can be called again.");

			working = true;

			vertexCount = 0;
		}

		public static void End()
		{
			if (!working)
				throw new Exception("Start must be called before End can be called.");

			Flush();

			working = false;
		}

		private static void Flush()
		{
			if (!working)
				throw new Exception("Start must be called before Flush can be called.");

			if (vertexCount > 0)
			{
				currentShader.Bind();

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

		private static void AddVertex(DrawMode type, Vector2 position, Colour colour)
		{
			AddVertex(type, position, colour, false);
		}
		private static void AddVertex(DrawMode type, Vector2 position, Colour colour, bool textured)
		{
			AddVertex(type, position, colour, textured, Vector2.Zero);
		}
		private static void AddVertex(DrawMode type, Vector2 position, Colour colour, bool textured, Vector2 uv)
		{
			if (!working)
				throw new Exception("Start must be called before AddVertex can be called.");

			if (currentDrawMode != type)
			{
				Flush();
				currentDrawMode = type;
				// We can lose vertices here, but it's ok as we're switching primitive types anyways
				vertexCount = 0;
			}

			if (vertexCount >= bufferSize)
				Flush();

			vertices[vertexCount].Position = new Vector3(position.X, position.Y, 0f);
			vertices[vertexCount].Colour = colour;
			vertices[vertexCount].UV = uv;
			vertices[vertexCount].Textured = textured;
			vertexCount++;
		}
		private static void AddVertex(DrawMode type, Vector2 position, Colour colour, bool textured, Vector2 uv, float rotation, Vector2 origin)
		{
			if (rotation != 0f)
				AddVertex(type, position * (Matrix2D.CreateTranslation(-origin) * Matrix2D.CreateRotationZ(Math.RotationsToRadians(rotation)) * Matrix2D.CreateTranslation(origin)), colour, textured, uv);
			else
				AddVertex(type, position, colour, textured, uv);
		}

		public static class Draw
		{
			public static void Triangle(Vector2 v1, Vector2 v2, Vector2 v3, Colour colour)
			{
				Triangle(v1, v2, v3, colour, colour, colour);
			}
			public static void Triangle(Vector2 v1, Vector2 v2, Vector2 v3, Colour c1, Colour c2, Colour c3)
			{
				Triangle(v1, v2, v3, c1, c2, c3, false);
			}
			public static void Triangle(Vector2 v1, Vector2 v2, Vector2 v3, Colour c1, Colour c2, Colour c3, bool textured)
			{
				Triangle(v1, v2, v3, c1, c2, c3, textured, Vector2.Zero, Vector2.Zero, Vector2.Zero);
			}
			public static void Triangle(Vector2 v1, Vector2 v2, Vector2 v3, Colour c1, Colour c2, Colour c3, bool textured, Vector2 uv1, Vector2 uv2, Vector2 uv3)
			{
				AddVertex(DrawMode.TriangleList, v1, c1, textured, uv1);
				AddVertex(DrawMode.TriangleList, v2, c2, textured, uv2);
				AddVertex(DrawMode.TriangleList, v3, c3, textured, uv3);
			}
			public static void Triangle(Vector2 v1, Vector2 v2, Vector2 v3, Colour c1, Colour c2, Colour c3, bool textured, Vector2 uv1, Vector2 uv2, Vector2 uv3, float rotation, Vector2 origin)
			{
				if (rotation != 0f)
				{
					AddVertex(DrawMode.TriangleList, v1, c1, textured, uv1, rotation, origin);
					AddVertex(DrawMode.TriangleList, v2, c2, textured, uv2, rotation, origin);
					AddVertex(DrawMode.TriangleList, v3, c3, textured, uv3, rotation, origin);
				}
				else
				{
					Triangle(v1, v2, v3, c1, c2, c3, textured, uv1, uv2, uv3);
				}
			}

			public static void Rectangle(float x, float y, float width, float height, Colour colour)
			{
				Rectangle(x, y, width, height, colour, colour, colour, colour);
			}
			public static void Rectangle(float x, float y, float width, float height, Colour c1, Colour c2, Colour c3, Colour c4)
			{
				Rectangle(x, y, width, height, c1, c2, c3, c4, false);
			}
			public static void Rectangle(float x, float y, float width, float height, Colour c1, Colour c2, Colour c3, Colour c4, bool textured)
			{
				Rectangle(x, y, width, height, c1, c2, c3, c4, textured, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero);
			}
			public static void Rectangle(float x, float y, float width, float height, Colour c1, Colour c2, Colour c3, Colour c4, bool textured, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4)
			{
				Rectangle(new Vector2(x, y), new Vector2(x + width, y), new Vector2(x, y + height), new Vector2(x + width, y + height), c1, c2, c3, c4, textured, uv1, uv2, uv3, uv4);
			}
			public static void Rectangle(float x, float y, float width, float height, Colour c1, Colour c2, Colour c3, Colour c4, bool textured, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4, float rotation, Vector2 origin)
			{
				Rectangle(new Vector2(x, y), new Vector2(x + width, y), new Vector2(x, y + height), new Vector2(x + width, y + height), c1, c2, c3, c4, textured, uv1, uv2, uv3, uv4, rotation, origin);
			}
			//public static void Rectangle(float x, float y, float width, float height, Colour c1, Colour c2, Colour c3, Colour c4, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4, bool outline = false, float outlineWidth = 1f)
			//{
			//	if (outline)
			//	{
			//		Vector2 tl = new Vector2(x, y);
			//		Vector2 tr = new Vector2(x + width - 1, y);
			//		Vector2 bl = new Vector2(x, y + height - 1);
			//		Vector2 br = new Vector2(x + width - 1, y + height - 1);

			//		Line(tl + new Vector2(1, 0), tr, c1, outlineWidth);
			//		Line(tr + new Vector2(0, 1), br, c2, outlineWidth);
			//		Line(br + new Vector2(-1, 0), bl, c3, outlineWidth);
			//		Line(bl + new Vector2(0, -1), tl, c4, outlineWidth);
			//	}
			//	else
			//	{
			//		Rectangle(new Vector2(x, y), new Vector2(x + width, y), new Vector2(x, y + height), new Vector2(x + width, y + height), c1, c2, c3, c4, uv1, uv2, uv3, uv4);
			//	}
			//}

			public static void Rectangle(Vector2 v1, Vector2 v2, Colour colour)
			{
				Rectangle(v1, v2, colour, colour, colour, colour);
			}
			public static void Rectangle(Vector2 v1, Vector2 v2, Colour c1, Colour c2, Colour c3, Colour c4)
			{
				Rectangle(v1, v2, c1, c2, c3, c4, false);
			}
			public static void Rectangle(Vector2 v1, Vector2 v2, Colour c1, Colour c2, Colour c3, Colour c4, bool textured)
			{
				Rectangle(v1, v2, c1, c2, c3, c4, textured, Vector2.Zero, Vector2.Zero, Vector2.Zero, Vector2.Zero);
			}
			public static void Rectangle(Vector2 v1, Vector2 v2, Colour c1, Colour c2, Colour c3, Colour c4, bool textured, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4)
			{
				Rectangle(new Vector2(v1.X, v1.Y),
						  new Vector2(v2.X, v1.Y),
						  new Vector2(v1.X, v2.Y),
						  new Vector2(v2.X, v2.Y),
						  c1, c2, c3, c4,
						  textured, uv1, uv2, uv3, uv4);
			}
			public static void Rectangle(Vector2 v1, Vector2 v2, Colour c1, Colour c2, Colour c3, Colour c4, bool textured, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4, float rotation, Vector2 origin)
			{
				Rectangle(new Vector2(v1.X, v1.Y),
						  new Vector2(v2.X, v1.Y),
						  new Vector2(v1.X, v2.Y),
						  new Vector2(v2.X, v2.Y),
						  c1, c2, c3, c4,
						  textured, uv1, uv2, uv3, uv4,
						  rotation, origin);
			}
			private static void Rectangle(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Colour c1, Colour c2, Colour c3, Colour c4, bool textured, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4)
			{
				AddVertex(DrawMode.TriangleList, v1, c1, textured, uv1);
				AddVertex(DrawMode.TriangleList, v2, c2, textured, uv2);
				AddVertex(DrawMode.TriangleList, v3, c3, textured, uv3);
				AddVertex(DrawMode.TriangleList, v3, c3, textured, uv3);
				AddVertex(DrawMode.TriangleList, v2, c2, textured, uv2);
				AddVertex(DrawMode.TriangleList, v4, c4, textured, uv4);
			}
			private static void Rectangle(Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, Colour c1, Colour c2, Colour c3, Colour c4, bool textured, Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4, float rotation, Vector2 origin)
			{
				if (rotation != 0f)
				{
					AddVertex(DrawMode.TriangleList, v1, c1, textured, uv1, rotation, origin);
					AddVertex(DrawMode.TriangleList, v2, c2, textured, uv2, rotation, origin);
					AddVertex(DrawMode.TriangleList, v3, c3, textured, uv3, rotation, origin);
					AddVertex(DrawMode.TriangleList, v3, c3, textured, uv3, rotation, origin);
					AddVertex(DrawMode.TriangleList, v2, c2, textured, uv2, rotation, origin);
					AddVertex(DrawMode.TriangleList, v4, c4, textured, uv4, rotation, origin);
				}
				else
				{
					Rectangle(v1, v2, v3, v4, c1, c2, c3, c4, textured, uv1, uv2, uv3, uv4);
				}
			}

			// TODO - Sprites
			// Sprites
			//public static void Sprite(Sprite sprite, Vector2 position, Colour colour)
			//{
			//	SetTexture(sprite.SpriteSheet.Texture);
			//	Rectangle(position.X, position.Y, sprite.Width, sprite.Height, colour, colour, colour, colour, true, sprite.UVs[0], sprite.UVs[1], sprite.UVs[2], sprite.UVs[3]);
			//}
			//public static void Sprite(Sprite sprite, Vector2 position, Vector2 scale, Colour colour)
			//{
			//	SetTexture(sprite.SpriteSheet.Texture);
			//	Rectangle(position.X, position.Y, sprite.Width * scale.X, sprite.Height * scale.Y, colour, colour, colour, colour, true, sprite.UVs[0], sprite.UVs[1], sprite.UVs[2], sprite.UVs[3]);
			//}
			//public static void Sprite(Sprite sprite, Vector2 position, Colour colour, float rotation, Vector2 origin)
			//{
			//	SetTexture(sprite.SpriteSheet.Texture);
			//	Rectangle(position.X, position.Y, sprite.Width, sprite.Height, colour, colour, colour, colour, true, sprite.UVs[0], sprite.UVs[1], sprite.UVs[2], sprite.UVs[3], rotation, origin);
			//}
			//public static void Sprite(Sprite sprite, Vector2 position, Vector2 scale, Colour colour, float rotation, Vector2 origin)
			//{
			//	SetTexture(sprite.SpriteSheet.Texture);
			//	Rectangle(position.X, position.Y, sprite.Width * scale.X, sprite.Height * scale.Y, colour, colour, colour, colour, true, sprite.UVs[0], sprite.UVs[1], sprite.UVs[2], sprite.UVs[3], rotation, origin);
			//}

			//// Strings
			//public static void Text(SpriteFont font, string text, Vector2 position, Vector2 scale, Colour colour, HAlign hAlign = HAlign.Left, VAlign vAlign = VAlign.Top)
			//{
			//	SetTexture(font.SpriteSheet.Texture);

			//	float offsetX = 0;
			//	float offsetY = 0;

			//	float offsetHAlign = 0;
			//	if (hAlign == HAlign.Center)
			//		offsetHAlign = 0.5f;
			//	else if (hAlign == HAlign.Right)
			//		offsetHAlign = 1f;

			//	float offsetVAlign = 0;
			//	if (vAlign == VAlign.Middle)
			//		offsetVAlign = 0.5f;
			//	else if (vAlign == VAlign.Bottom)
			//		offsetVAlign = 1f;

			//	string[] lines = text.Replace("|:", "\a").Replace(":|", "\a").Split('\n');

			//	float totalHeight = font.LineHeight * lines.Length;

			//	bool mod = false;
			//	string modData = "";
			//	bool modShadow = false;
			//	Vector2 modShadowOffset = Vector2.Zero;
			//	Colour modShadowColour = Colour.White;

			//	foreach (string line in lines)
			//	{
			//		float lineWidth = 0;

			//		foreach (char c in line)
			//		{
			//			if (mod)
			//			{
			//				if (c == '\a')
			//					mod = false;
			//			}
			//			else
			//			{
			//				if (c == ' ')
			//					lineWidth += font.SpaceWidth;
			//				else if (c == '\a')
			//					mod = true;
			//				else
			//					lineWidth += font.Glyph(c).Width * scale.X + font.CharSpacing;
			//			}
			//		}

			//		foreach (char c in line)
			//		{
			//			if (mod)
			//			{
			//				if (c == '\a')
			//				{
			//					string[] tag = modData.Split('=');

			//					if (tag.Length == 2)
			//					{
			//						if (tag[0] == "colour" || tag[0] == "colour")
			//						{
			//							string[] value = tag[1].Split(',');

			//							if (value.Length == 4)
			//							{
			//								try
			//								{
			//									colour = new Colour(float.Parse(value[0]), float.Parse(value[1]), float.Parse(value[2]), float.Parse(value[3]));
			//								}
			//								catch
			//								{
			//									throw new Exception("Text modifier value parse error.");
			//								}
			//							}
			//							else
			//							{
			//								throw new Exception("Unrecognized text modifier value.");
			//							}
			//						}
			//						else if (tag[0] == "shadow")
			//						{
			//							string[] value = tag[1].Split(',');

			//							if (value.Length == 6)
			//							{
			//								try
			//								{
			//									modShadow = true;
			//									modShadowOffset = new Vector2(float.Parse(value[0]), float.Parse(value[1]));
			//									modShadowColour = new Colour(float.Parse(value[2]), float.Parse(value[3]), float.Parse(value[4]), float.Parse(value[5]));
			//								}
			//								catch
			//								{
			//									throw new Exception("Text modifier value parse error.");
			//								}
			//							}
			//							else if (value.Length == 1 && (value[0] == "disable" || value[0] == "off"))
			//							{
			//								modShadow = false;
			//							}
			//							else
			//							{
			//								throw new Exception("Unrecognized text modifier value.");
			//							}
			//						}
			//						else
			//						{
			//							throw new Exception("Unrecognized text modifier key.");
			//						}
			//					}
			//					else
			//					{
			//						throw new Exception("Text modifier parse error.");
			//					}

			//					mod = false;
			//					modData = "";
			//				}
			//				else
			//				{
			//					modData += c;
			//				}
			//			}
			//			else
			//			{
			//				if (c == ' ')
			//				{
			//					offsetX += font.SpaceWidth;
			//				}
			//				else if (c == '\a')
			//				{
			//					mod = true;
			//				}
			//				else
			//				{
			//					Sprite sprite = font.Glyph(c);

			//					if (modShadow)
			//					{
			//						Rectangle(position.X + offsetX - lineWidth * offsetHAlign + modShadowOffset.X,
			//								  position.Y + offsetY - totalHeight * offsetVAlign + modShadowOffset.Y,
			//								  sprite.Width * scale.X,
			//								  sprite.Height * scale.Y,
			//								  modShadowColour, modShadowColour, modShadowColour, modShadowColour,
			//								  true, sprite.UVs[0], sprite.UVs[1], sprite.UVs[2], sprite.UVs[3]);
			//					}

			//					Rectangle(position.X + offsetX - lineWidth * offsetHAlign,
			//							  position.Y + offsetY - totalHeight * offsetVAlign,
			//							  sprite.Width * scale.X,
			//							  sprite.Height * scale.Y,
			//							  colour, colour, colour, colour,
			//							  true, sprite.UVs[0], sprite.UVs[1], sprite.UVs[2], sprite.UVs[3]);

			//					offsetX += sprite.Width * scale.X + font.CharSpacing;
			//				}
			//			}
			//		}

			//		offsetX = 0;
			//		offsetY += font.LineHeight;
			//	}
			//}

			// TODO - RenderTargets
			// RenderTargets
			//public static void RenderTarget2D(RenderTarget2D renderTarget2D, float x, float y, Colour colour)
			//{
			//	SetTexture(renderTarget2D);
			//	Rectangle(x, y, renderTarget2D.Width, renderTarget2D.Height, colour, colour, colour, colour, true, uvFull1, uvFull2, uvFull3, uvFull4);
			//}
			//public static void RenderTarget2D(RenderTarget2D renderTarget2D, float x, float y, float rotation, Vector2 origin, Colour colour)
			//{
			//	SetTexture(renderTarget2D);
			//	Rectangle(x, y, renderTarget2D.Width, renderTarget2D.Height, colour, colour, colour, colour, true, uvFull1, uvFull2, uvFull3, uvFull4, rotation, origin);
			//}
			//public static void RenderTarget2D(RenderTarget2D renderTarget2D, float x, float y, float scale, Colour colour)
			//{
			//	SetTexture(renderTarget2D);
			//	Rectangle(x, y, renderTarget2D.Width * scale, renderTarget2D.Height * scale, colour, colour, colour, colour, true, uvFull1, uvFull2, uvFull3, uvFull4);
			//}
			//public static void RenderTarget2D(RenderTarget2D renderTarget2D, float x, float y, float scale, float rotation, Vector2 origin, Colour colour)
			//{
			//	SetTexture(renderTarget2D);
			//	Rectangle(x, y, renderTarget2D.Width * scale, renderTarget2D.Height * scale, colour, colour, colour, colour, true, uvFull1, uvFull2, uvFull3, uvFull4, rotation, origin);
			//}
			//public static void RenderTarget2D(RenderTarget2D renderTarget2D, float x, float y, float width, float height, Colour colour)
			//{
			//	SetTexture(renderTarget2D);
			//	Rectangle(x, y, width, height, colour, colour, colour, colour, true, uvFull1, uvFull2, uvFull3, uvFull4);
			//}
			//public static void RenderTarget2D(RenderTarget2D renderTarget2D, Vector2 v1, Vector2 v2, Colour colour)
			//{
			//	SetTexture(renderTarget2D);
			//	Rectangle(new Vector2(v1.X, v1.Y), new Vector2(v2.X, v2.Y), colour, colour, colour, colour, true, uvFull1, uvFull2, uvFull3, uvFull4);
			//}

			// Pixels
			public static void Pixel(int x, int y, Colour colour)
			{
				Rectangle(new Vector2(x, y), new Vector2(x + 1, y + 1), colour);
			}
			public static void Pixel(float x, float y, Colour colour)
			{
				Rectangle(new Vector2(x, y), new Vector2(x + 1, y + 1), colour);
			}
			public static void Pixel(Vector2 vector, Colour colour)
			{
				Rectangle(vector, vector + Vector2.One, colour);
			}

			//
			// Circles
			//public static void Circle(Vector2 center, float radius, Colour colour, int circleSegments = 16)
			//{
			//	Circle(center, radius, colour, colour, circleSegments);
			//}
			//public static void Circle(Vector2 center, float radius, Colour c1, Colour c2, int circleSegments = 16)
			//{
			//	var increment = MathHelper.Pi * 2.0f / circleSegments;
			//	var theta = 0.0f;

			//	var v0 = center;
			//	theta += increment;

			//	for (int i = 0; i < circleSegments; i++)
			//	{
			//		var v1 = center + radius * new Vector2(Mathf.cos(theta), Mathf.sin(theta));
			//		var v2 = center + radius * new Vector2(Mathf.cos(theta + increment), Mathf.sin(theta + increment));

			//		AddVertex(DrawMode.TriangleList, v0, c1);
			//		AddVertex(DrawMode.TriangleList, v1, c2);
			//		AddVertex(DrawMode.TriangleList, v2, c2);

			//		theta += increment;
			//	}
			//}

			// Lines
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

			public static class Lines
			{
				public static void Line(float x1, float y1, float x2, float y2, Colour colour)
				{
					Line(x1, y1, x2, y2, colour, colour);
				}
				public static void Line(float x1, float y1, float x2, float y2, Colour c1, Colour c2)
				{
					Line(new Vector2(x1, y1), new Vector2(x2, y2), c1, c2);
				}
				public static void Line(Vector2 v1, Vector2 v2, Colour colour)
				{
					Line(v1, v2, colour, colour);
				}
				public static void Line(Vector2 v1, Vector2 v2, Colour c1, Colour c2)
				{
					Line(v1, v2, c1, c2, false);
				}
				public static void Line(Vector2 v1, Vector2 v2, Colour c1, Colour c2, bool textured)
				{
					Line(v1, v2, c1, c2, textured, Vector2.Zero, Vector2.Zero);
				}
				public static void Line(Vector2 v1, Vector2 v2, Colour c1, Colour c2, bool textured, Vector2 uv1, Vector2 uv2)
				{
					AddVertex(DrawMode.LineList, v1, c1, textured, uv1);
					AddVertex(DrawMode.LineList, v2, c2, textured, uv2);
				}
			}

			public static class LineStrip
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

				public static void AddVertex(Vector2 position, Colour colour)
				{
					AddVertex(position, colour, false);
				}
				public static void AddVertex(Vector2 position, Colour colour, bool textured)
				{
					AddVertex(position, colour, textured, Vector2.Zero);
				}
				public static void AddVertex(Vector2 position, Colour colour, bool textured, Vector2 uv)
				{
					if (!working)
						throw new Exception("Begin must be called before AddVertex can be called.");

					if (lastVertex.HasValue)
					{
						Renderer.AddVertex(DrawMode.LineList,
										   new Vector2(lastVertex.Value.Position.X, lastVertex.Value.Position.Y),
										   lastVertex.Value.Colour,
										   lastVertex.Value.Textured,
										   lastVertex.Value.UV);
						Renderer.AddVertex(DrawMode.LineList, position, colour, textured, uv);
					}

					lastVertex = new Vertex(new Vector3(position.X, position.Y, 0f), colour, uv, textured);
				}
			}

			public static class TriangleStrip
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

				public static void AddVertex(Vector2 position, Colour colour)
				{
					AddVertex(position, colour, false);
				}
				public static void AddVertex(Vector2 position, Colour colour, bool textured)
				{
					AddVertex(position, colour, textured, Vector2.Zero);
				}
				public static void AddVertex(Vector2 position, Colour colour, bool textured, Vector2 uv)
				{
					if (!working)
						throw new Exception("Begin must be called before AddVertex can be called.");

					if (lastLastVertex.HasValue && lastVertex.HasValue)
					{
						Renderer.AddVertex(DrawMode.TriangleList,
										   new Vector2(lastLastVertex.Value.Position.X, lastLastVertex.Value.Position.Y),
										   lastLastVertex.Value.Colour,
										   lastLastVertex.Value.Textured,
										   lastLastVertex.Value.UV);
						Renderer.AddVertex(DrawMode.TriangleList,
										   new Vector2(lastVertex.Value.Position.X, lastVertex.Value.Position.Y),
										   lastVertex.Value.Colour,
										   lastVertex.Value.Textured,
										   lastVertex.Value.UV);
						Renderer.AddVertex(DrawMode.TriangleList, position, colour, textured, uv);
					}

					lastLastVertex = lastVertex;
					lastVertex = new Vertex(new Vector3(position.X, position.Y, 0), colour, uv, textured);
				}
			}

			//public static class LineStrip
			//{
			//	public static void AddVertex(Vector2 position, Colour colour)
			//	{
			//		Renderer.AddVertex(DrawMode.LineStrip, position, colour);
			//	}
			//	public static void AddVertex(Vector2 position, Colour colour, bool textured)
			//	{
			//		Renderer.AddVertex(DrawMode.LineStrip, position, colour, textured);
			//	}
			//	public static void AddVertex(Vector2 position, Colour colour, bool textured, Vector2 uv)
			//	{
			//		Renderer.AddVertex(DrawMode.LineStrip, position, colour, textured, uv);
			//	}
			//	public static void AddVertex(Vector2 position, Colour colour, bool textured, Vector2 uv, float rotation, Vector2 origin)
			//	{
			//		Renderer.AddVertex(DrawMode.LineStrip, position, colour, textured, uv, rotation, origin);
			//	}
			//}

			//public static class TriangleStrip
			//{
			//	public static void AddVertex(Vector2 position, Colour colour)
			//	{
			//		Renderer.AddVertex(DrawMode.TriangleStrip, position, colour);
			//	}
			//	public static void AddVertex(Vector2 position, Colour colour, bool textured)
			//	{
			//		Renderer.AddVertex(DrawMode.TriangleStrip, position, colour, textured);
			//	}
			//	public static void AddVertex(Vector2 position, Colour colour, bool textured, Vector2 uv)
			//	{
			//		Renderer.AddVertex(DrawMode.TriangleStrip, position, colour, textured, uv);
			//	}
			//	public static void AddVertex(Vector2 position, Colour colour, bool textured, Vector2 uv, float rotation, Vector2 origin)
			//	{
			//		Renderer.AddVertex(DrawMode.TriangleStrip, position, colour, textured, uv, rotation, origin);
			//	}
			//}
		}
	}
}