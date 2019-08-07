using System.Collections.Generic;

namespace CKGL
{
	public struct Geometry
	{
		public struct Vertex
		{
			public Vector3 Position;
			public Vector3 Normal;
			public Vector3 Tangent;
			public Vector3 Bitangent;
			public Colour Colour;
			public UV UV;

			public Vertex(Vector3 position, Vector3 normal, Colour colour, UV uv) : this(position, normal, Vector3.Zero, Vector3.Zero, colour, uv) { }
			public Vertex(Vector3 position, Vector3 normal, Vector3 tangent, Vector3 bitangent, Colour colour, UV uv)
			{
				Position = position;
				Normal = normal;
				Tangent = tangent;
				Bitangent = bitangent;
				Colour = colour;
				UV = uv;
			}
		}

		public Vertex[] Vertices;
		public ushort[] Indices16;
		public uint[] Indices32;

		public Geometry(Vertex[] vertices, ushort[] indices16, uint[] indices32)
		{
			Vertices = vertices;
			Indices16 = indices16;
			Indices32 = indices32;
		}

		private struct TriangleIndices
		{
			public readonly int i1;
			public readonly int i2;
			public readonly int i3;

			public TriangleIndices(int i1, int i2, int i3)
			{
				this.i1 = i1;
				this.i2 = i2;
				this.i3 = i3;
			}
		}

		#region Cube
		public static Geometry Cube(float unitLength = 1f)
		{
			float t = unitLength * 0.5f;

			Vertex[] vertices = new Vertex[] {
				// Front
				new Vertex(new Vector3(-t,  t, -t), Vector3.Backward, Vector3.Right, Vector3.Up, Colour.White, UV.TopLeft),
				new Vertex(new Vector3( t,  t, -t), Vector3.Backward, Vector3.Right, Vector3.Up, Colour.White, UV.TopRight),
				new Vertex(new Vector3(-t, -t, -t), Vector3.Backward, Vector3.Right, Vector3.Up, Colour.White, UV.BottomLeft),
				new Vertex(new Vector3( t, -t, -t), Vector3.Backward, Vector3.Right, Vector3.Up, Colour.White, UV.BottomRight),
				// Back
				new Vertex(new Vector3( t,  t,  t), Vector3.Forward, Vector3.Left, Vector3.Up, Colour.White, UV.TopLeft),
				new Vertex(new Vector3(-t,  t,  t), Vector3.Forward, Vector3.Left, Vector3.Up, Colour.White, UV.TopRight),
				new Vertex(new Vector3( t, -t,  t), Vector3.Forward, Vector3.Left, Vector3.Up, Colour.White, UV.BottomLeft),
				new Vertex(new Vector3(-t, -t,  t), Vector3.Forward, Vector3.Left, Vector3.Up, Colour.White, UV.BottomRight),
				// Top
				new Vertex(new Vector3(-t,  t,  t), Vector3.Up, Vector3.Right, Vector3.Forward, Colour.White, UV.TopLeft),
				new Vertex(new Vector3( t,  t,  t), Vector3.Up, Vector3.Right, Vector3.Forward, Colour.White, UV.TopRight),
				new Vertex(new Vector3(-t,  t, -t), Vector3.Up, Vector3.Right, Vector3.Forward, Colour.White, UV.BottomLeft),
				new Vertex(new Vector3( t,  t, -t), Vector3.Up, Vector3.Right, Vector3.Forward, Colour.White, UV.BottomRight),
				// Bottom
				new Vertex(new Vector3( t, -t,  t), Vector3.Down, Vector3.Right, Vector3.Backward, Colour.White, UV.BottomRight),
				new Vertex(new Vector3(-t, -t,  t), Vector3.Down, Vector3.Right, Vector3.Backward, Colour.White, UV.BottomLeft),
				new Vertex(new Vector3( t, -t, -t), Vector3.Down, Vector3.Right, Vector3.Backward, Colour.White, UV.TopRight),
				new Vertex(new Vector3(-t, -t, -t), Vector3.Down, Vector3.Right, Vector3.Backward, Colour.White, UV.TopLeft),
				// Left
				new Vertex(new Vector3(-t,  t,  t), Vector3.Left, Vector3.Backward, Vector3.Up, Colour.White, UV.TopLeft),
				new Vertex(new Vector3(-t,  t, -t), Vector3.Left, Vector3.Backward, Vector3.Up, Colour.White, UV.TopRight),
				new Vertex(new Vector3(-t, -t,  t), Vector3.Left, Vector3.Backward, Vector3.Up, Colour.White, UV.BottomLeft),
				new Vertex(new Vector3(-t, -t, -t), Vector3.Left, Vector3.Backward, Vector3.Up, Colour.White, UV.BottomRight),
				// Right
				new Vertex(new Vector3( t,  t, -t), Vector3.Right, Vector3.Forward, Vector3.Up, Colour.White, UV.TopLeft),
				new Vertex(new Vector3( t,  t,  t), Vector3.Right, Vector3.Forward, Vector3.Up, Colour.White, UV.TopRight),
				new Vertex(new Vector3( t, -t, -t), Vector3.Right, Vector3.Forward, Vector3.Up, Colour.White, UV.BottomLeft),
				new Vertex(new Vector3( t, -t,  t), Vector3.Right, Vector3.Forward, Vector3.Up, Colour.White, UV.BottomRight)
			};

			ushort[] indices16 = new ushort[] {
				// Front
				0, 2, 1,
				2, 3, 1,
				// Back
				0 + 4, 2 + 4, 1 + 4,
				2 + 4, 3 + 4, 1 + 4,
				// Top
				0 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,
				2 + 4 * 2, 3 + 4 * 2, 1 + 4 * 2,
				// Bottom
				0 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,
				2 + 4 * 3, 3 + 4 * 3, 1 + 4 * 3,
				// Left
				0 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,
				2 + 4 * 4, 3 + 4 * 4, 1 + 4 * 4,
				// Right
				0 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,
				2 + 4 * 5, 3 + 4 * 5, 1 + 4 * 5
			};

			uint[] indices32 = new uint[indices16.Length];
			for (int i = 0; i < indices32.Length; i++)
				indices32[i] = indices16[i];

			return new Geometry(vertices, indices16, indices32);
		}
		#endregion

		#region Plane
		public enum Orientation
		{
			XY,
			XZ,
			ZY
		};
		public static Geometry Plane(float unitLength = 1f, Orientation orientation = Orientation.XZ, uint cellsFirstDimension = 10, uint cellsSecondDimension = 10, bool flat = false)
		{
			float t = unitLength;
			int tx = (int)cellsFirstDimension;
			int tz = (int)cellsSecondDimension;
			Vector3 origin = new Vector3(-tx * t * 0.5f, 0f, -tz * t * 0.5f);

			List<Vector3> vertices = new List<Vector3>();
			List<UV> uvs = new List<UV>();
			List<TriangleIndices> faces = new List<TriangleIndices>();
			Vector3 normal = Vector3.Up;
			Vector3 tangent = Vector3.Right;
			Vector3 bitangent = Vector3.Forward;

			// Vertices
			for (int z = 0; z < tz + 1; z++)
			{
				for (int x = 0; x < tx + 1; x++)
				{
					vertices.Add(origin + new Vector3(t * x, 0f, t * z));
					uvs.Add(new UV((float)x / (float)tx, (float)z / (float)tz));
				}
			}

			if (orientation == Orientation.XY)
			{
				for (int i = 0; i < vertices.Count; i++)
					vertices[i] = new Vector3(vertices[i].X, vertices[i].Z, 0f);
				normal = Vector3.Backward;
				tangent = Vector3.Right;
				bitangent = Vector3.Up;
			}
			else if (orientation == Orientation.ZY)
			{
				for (int i = 0; i < vertices.Count; i++)
					vertices[i] = new Vector3(0f, vertices[i].Z, vertices[i].X);
				normal = Vector3.Right;
				tangent = Vector3.Forward;
				bitangent = Vector3.Up;
			}

			// Faces
			for (int z = 0; z < tz; z++)
			{
				for (int x = 0; x < tx; x++)
				{
					int bl = (tx + 1) * z + x;
					int br = bl + 1;
					int tl = bl + (tx + 1);
					int tr = tl + 1;
					faces.Add(new TriangleIndices(bl, br, tr));
					faces.Add(new TriangleIndices(bl, tr, tl));
				}
			}

			// Calculate geometry data
			if (flat)
			{
				Vertex[] assembledVertices = new Vertex[faces.Count * 3];
				ushort[] indices16 = new ushort[faces.Count * 3];
				uint[] indices32 = new uint[faces.Count * 3];
				for (int i = 0; i < faces.Count; i++)
				{
					assembledVertices[3 * i] = new Vertex(vertices[faces[i].i1], normal, tangent, bitangent, Colour.White, uvs[faces[i].i1]);
					assembledVertices[3 * i + 1] = new Vertex(vertices[faces[i].i2], normal, tangent, bitangent, Colour.White, uvs[faces[i].i2]);
					assembledVertices[3 * i + 2] = new Vertex(vertices[faces[i].i3], normal, tangent, bitangent, Colour.White, uvs[faces[i].i3]);

					indices16[3 * i] = (ushort)(3 * i);
					indices16[3 * i + 1] = (ushort)(3 * i + 1);
					indices16[3 * i + 2] = (ushort)(3 * i + 2);

					indices32[3 * i] = (uint)(3 * i);
					indices32[3 * i + 1] = (uint)(3 * i + 1);
					indices32[3 * i + 2] = (uint)(3 * i + 2);
				}

				return new Geometry(assembledVertices, indices16, indices32);
			}
			else
			{
				Vertex[] assembledVertices = new Vertex[vertices.Count];
				ushort[] indices16 = new ushort[faces.Count * 3];
				uint[] indices32 = new uint[faces.Count * 3];
				for (int i = 0; i < faces.Count; i++)
				{
					indices16[3 * i] = (ushort)faces[i].i1;
					indices16[3 * i + 1] = (ushort)faces[i].i2;
					indices16[3 * i + 2] = (ushort)faces[i].i3;

					indices32[3 * i] = (uint)faces[i].i1;
					indices32[3 * i + 1] = (uint)faces[i].i2;
					indices32[3 * i + 2] = (uint)faces[i].i3;
				}

				for (int i = 0; i < assembledVertices.Length; i++)
					assembledVertices[i] = new Vertex(vertices[i], normal, tangent, bitangent, Colour.White, uvs[i]);

				return new Geometry(assembledVertices, indices16, indices32);
			}
		}
		#endregion

		#region Icosahedron
		public static Geometry Icosahedron(float radius = 0.5f, bool flat = false) => Icosphere(radius, 0, flat);
		#endregion

		#region Icosphere
		// Adapted from: http://blog.andreaskahler.com/2009/06/creating-icosphere-mesh-in-code.html
		public static Geometry Icosphere(float radius = 0.5f, uint subdivisions = 0, bool flat = false)
		{
			List<Vector3> vertices = new List<Vector3>();
			List<TriangleIndices> faces = new List<TriangleIndices>();

			// Icosahedron vertices
			float t = (1f + Math.Sqrt(5f)) / 2f;
			// XY plane
			vertices.Add(new Vector3(-1f,  t, 0f).Normalized);
			vertices.Add(new Vector3( 1f,  t, 0f).Normalized);
			vertices.Add(new Vector3(-1f, -t, 0f).Normalized);
			vertices.Add(new Vector3( 1f, -t, 0f).Normalized);
			// YZ plane
			vertices.Add(new Vector3(0f, -1f,  t).Normalized);
			vertices.Add(new Vector3(0f,  1f,  t).Normalized);
			vertices.Add(new Vector3(0f, -1f, -t).Normalized);
			vertices.Add(new Vector3(0f,  1f, -t).Normalized);
			// XZ plane
			vertices.Add(new Vector3( t, 0f, -1f).Normalized);
			vertices.Add(new Vector3( t, 0f,  1f).Normalized);
			vertices.Add(new Vector3(-t, 0f, -1f).Normalized);
			vertices.Add(new Vector3(-t, 0f,  1f).Normalized);

			// Icosahedron faces
			// 5 faces around point 0
			faces.Add(new TriangleIndices( 0,  5, 11));
			faces.Add(new TriangleIndices( 0,  1,  5));
			faces.Add(new TriangleIndices( 0,  7,  1));
			faces.Add(new TriangleIndices( 0, 10,  7));
			faces.Add(new TriangleIndices( 0, 11, 10));
			// 5 adjacent faces
			faces.Add(new TriangleIndices( 1,  9,  5));
			faces.Add(new TriangleIndices( 5,  4, 11));
			faces.Add(new TriangleIndices(11,  2, 10));
			faces.Add(new TriangleIndices(10,  6,  7));
			faces.Add(new TriangleIndices( 7,  8,  1));
			// 5 faces around point 3
			faces.Add(new TriangleIndices( 3,  4,  9));
			faces.Add(new TriangleIndices( 3,  2,  4));
			faces.Add(new TriangleIndices( 3,  6,  2));
			faces.Add(new TriangleIndices( 3,  8,  6));
			faces.Add(new TriangleIndices( 3,  9,  8));
			// 5 adjacent faces
			faces.Add(new TriangleIndices( 4,  5,  9));
			faces.Add(new TriangleIndices( 2, 11,  4));
			faces.Add(new TriangleIndices( 6, 10,  2));
			faces.Add(new TriangleIndices( 8,  7,  6));
			faces.Add(new TriangleIndices( 9,  1,  8));

			// Subdivide faces
			int generateSubdividedVertex(int index1, int index2)
			{
				vertices.Add(((vertices[index1] + vertices[index2]) * 0.5f).Normalized);

				return vertices.Count - 1;
			}
			for (int i = 0; i < subdivisions; i++)
			{
				List<TriangleIndices> subdividedFaces = new List<TriangleIndices>();
				foreach (TriangleIndices face in faces)
				{
					// Generate midpoints between vertices
					int a = generateSubdividedVertex(face.i1, face.i2);
					int b = generateSubdividedVertex(face.i2, face.i3);
					int c = generateSubdividedVertex(face.i3, face.i1);

					// Subdivide each face with 4 new faces
					subdividedFaces.Add(new TriangleIndices(face.i1, a, c));
					subdividedFaces.Add(new TriangleIndices(face.i2, b, a));
					subdividedFaces.Add(new TriangleIndices(face.i3, c, b));
					subdividedFaces.Add(new TriangleIndices(a, b, c));
				}
				faces = subdividedFaces;
			}

			// Scale by radius
			for (int i = 0; i < vertices.Count; i++)
				vertices[i] *= radius;

			// Calculate geometry data
			if (flat)
			{
				Vertex[] assembledVertices = new Vertex[faces.Count * 3];
				ushort[] indices16 = new ushort[faces.Count * 3];
				uint[] indices32 = new uint[faces.Count * 3];
				for (int i = 0; i < faces.Count; i++)
				{
					Vector3 v1 = vertices[faces[i].i1];
					Vector3 v2 = vertices[faces[i].i2];
					Vector3 v3 = vertices[faces[i].i3];
					Vector3 normal = Vector3.Cross(v2 - v1, v2 - v3).Normalized;

					assembledVertices[3 * i] = new Vertex(v1, normal, Colour.White, UV.Zero);
					assembledVertices[3 * i + 1] = new Vertex(v2, normal, Colour.White, UV.Zero);
					assembledVertices[3 * i + 2] = new Vertex(v3, normal, Colour.White, UV.Zero);

					indices16[3 * i] = (ushort)(3 * i);
					indices16[3 * i + 1] = (ushort)(3 * i + 1);
					indices16[3 * i + 2] = (ushort)(3 * i + 2);

					indices32[3 * i] = (uint)(3 * i);
					indices32[3 * i + 1] = (uint)(3 * i + 1);
					indices32[3 * i + 2] = (uint)(3 * i + 2);
				}

				return new Geometry(assembledVertices, indices16, indices32);
			}
			else
			{
				Vertex[] assembledVertices = new Vertex[vertices.Count];
				ushort[] indices16 = new ushort[faces.Count * 3];
				uint[] indices32 = new uint[faces.Count * 3];
				for (int i = 0; i < faces.Count; i++)
				{
					indices16[3 * i] = (ushort)faces[i].i1;
					indices16[3 * i + 1] = (ushort)faces[i].i2;
					indices16[3 * i + 2] = (ushort)faces[i].i3;

					indices32[3 * i] = (uint)faces[i].i1;
					indices32[3 * i + 1] = (uint)faces[i].i2;
					indices32[3 * i + 2] = (uint)faces[i].i3;
				}

				for (int i = 0; i < assembledVertices.Length; i++)
					assembledVertices[i] = new Vertex(vertices[i], vertices[i].Normalized, Colour.White, UV.Zero);

				return new Geometry(assembledVertices, indices16, indices32);
			}
		}
		#endregion

		#region Octahedron - Original
		//public static Geometry Octahedron(float radius = 0.5f, uint subdivisions = 0, bool flat = false)
		//{
		//	List<Vector3> vertices = new List<Vector3>();
		//	List<TriangleIndices> faces = new List<TriangleIndices>();

		//	int generateSubdividedVertex(int index1, int index2)
		//	{
		//		vertices.Add(((vertices[index1] + vertices[index2]) * 0.5f).Normalized);

		//		return vertices.Count - 1;
		//	}

		//	// Octahedron vertices
		//	vertices.Add(Vector3.Down);
		//	vertices.Add(Vector3.Forward);
		//	vertices.Add(Vector3.Left);
		//	vertices.Add(Vector3.Backward);
		//	vertices.Add(Vector3.Right);
		//	vertices.Add(Vector3.Up);

		//	// Octahedron faces
		//	faces.Add(new TriangleIndices(0, 2, 1));
		//	faces.Add(new TriangleIndices(0, 3, 2));
		//	faces.Add(new TriangleIndices(0, 4, 3));
		//	faces.Add(new TriangleIndices(0, 1, 4));
		//	faces.Add(new TriangleIndices(5, 1, 2));
		//	faces.Add(new TriangleIndices(5, 2, 3));
		//	faces.Add(new TriangleIndices(5, 3, 4));
		//	faces.Add(new TriangleIndices(5, 4, 1));

		//	// Subdivide faces
		//	for (int i = 0; i < subdivisions; i++)
		//	{
		//		List<TriangleIndices> subdividedFaces = new List<TriangleIndices>();
		//		foreach (TriangleIndices face in faces)
		//		{
		//			// Generate midpoints between vertices
		//			int a = generateSubdividedVertex(face.i1, face.i2);
		//			int b = generateSubdividedVertex(face.i2, face.i3);
		//			int c = generateSubdividedVertex(face.i3, face.i1);

		//			// Subdivide each face with 4 new faces
		//			subdividedFaces.Add(new TriangleIndices(face.i1, a, c));
		//			subdividedFaces.Add(new TriangleIndices(face.i2, b, a));
		//			subdividedFaces.Add(new TriangleIndices(face.i3, c, b));
		//			subdividedFaces.Add(new TriangleIndices(a, b, c));
		//		}
		//		faces = subdividedFaces;
		//	}

		//	// Calculate geometry data
		//	if (flat)
		//	{
		//		Vertex[] assembledVertices = new Vertex[faces.Count * 3];
		//		ushort[] indices16 = new ushort[faces.Count * 3];
		//		uint[] indices32 = new uint[faces.Count * 3];
		//		for (int i = 0; i < faces.Count; i++)
		//		{
		//			Vector3 v1 = vertices[faces[i].i1] * radius;
		//			Vector3 v2 = vertices[faces[i].i2] * radius;
		//			Vector3 v3 = vertices[faces[i].i3] * radius;
		//			Vector3 normal = ((v1 + v2 + v3) / 3f).Normalized;
		//			assembledVertices[3 * i] = new Vertex(v1, normal, Colour.White, UV.Zero);
		//			assembledVertices[3 * i + 1] = new Vertex(v2, normal, Colour.White, UV.Zero);
		//			assembledVertices[3 * i + 2] = new Vertex(v3, normal, Colour.White, UV.Zero);
		//			indices16[3 * i] = (ushort)(3 * i);
		//			indices16[3 * i + 1] = (ushort)(3 * i + 1);
		//			indices16[3 * i + 2] = (ushort)(3 * i + 2);
		//			indices32[3 * i] = (uint)(3 * i);
		//			indices32[3 * i + 1] = (uint)(3 * i + 1);
		//			indices32[3 * i + 2] = (uint)(3 * i + 2);
		//		}

		//		return new Geometry(assembledVertices, indices16, indices32);
		//	}
		//	else
		//	{
		//		ushort[] indices16 = new ushort[faces.Count * 3];
		//		uint[] indices32 = new uint[faces.Count * 3];
		//		for (int i = 0; i < faces.Count; i++)
		//		{
		//			indices16[3 * i] = (ushort)faces[i].i1;
		//			indices16[3 * i + 1] = (ushort)faces[i].i2;
		//			indices16[3 * i + 2] = (ushort)faces[i].i3;
		//			indices32[3 * i] = (uint)faces[i].i1;
		//			indices32[3 * i + 1] = (uint)faces[i].i2;
		//			indices32[3 * i + 2] = (uint)faces[i].i3;
		//		}

		//		Vertex[] assembledVertices = new Vertex[vertices.Count];
		//		for (int i = 0; i < assembledVertices.Length; i++)
		//			assembledVertices[i] = new Vertex(vertices[i] * radius, vertices[i], Colour.White, UV.Zero);

		//		return new Geometry(assembledVertices, indices16, indices32);
		//	}
		//}
		#endregion

		#region Octahedron
		// Adapted from: https://catlikecoding.com/unity/tutorials/octahedron-sphere/
		public static Geometry Octahedron(float radius = 0.5f, uint subdivisions = 0, bool flat = false)
		{
			List<Vector3> vertices = new List<Vector3>();
			List<UV> uvs = new List<UV>();
			List<TriangleIndices> faces = new List<TriangleIndices>();

			// Octahedron vertices
			vertices.Add(Vector3.Down);
			vertices.Add(Vector3.Down);
			vertices.Add(Vector3.Down);
			vertices.Add(Vector3.Down);
			vertices.Add(Vector3.Forward);
			vertices.Add(Vector3.Left);
			vertices.Add(Vector3.Backward);
			vertices.Add(Vector3.Right);
			vertices.Add(Vector3.Forward);
			vertices.Add(Vector3.Up);
			vertices.Add(Vector3.Up);
			vertices.Add(Vector3.Up);
			vertices.Add(Vector3.Up);

			// Octahedron faces
			faces.Add(new TriangleIndices( 0, 5, 4));
			faces.Add(new TriangleIndices( 1, 6, 5));
			faces.Add(new TriangleIndices( 2, 7, 6));
			faces.Add(new TriangleIndices( 3, 8, 7));
			faces.Add(new TriangleIndices( 9, 4, 5));
			faces.Add(new TriangleIndices(10, 5, 6));
			faces.Add(new TriangleIndices(11, 6, 7));
			faces.Add(new TriangleIndices(12, 7, 8));

			// Subdivide faces
			int generateSubdividedVertex(int index1, int index2)
			{
				vertices.Add(((vertices[index1] + vertices[index2]) * 0.5f).Normalized);

				return vertices.Count - 1;
			}
			for (int i = 0; i < subdivisions; i++)
			{
				List<TriangleIndices> subdividedFaces = new List<TriangleIndices>();
				foreach (TriangleIndices face in faces)
				{
					// Generate midpoints between vertices
					int a = generateSubdividedVertex(face.i1, face.i2);
					int b = generateSubdividedVertex(face.i2, face.i3);
					int c = generateSubdividedVertex(face.i3, face.i1);

					// Subdivide each face with 4 new faces
					subdividedFaces.Add(new TriangleIndices(face.i1, a, c));
					subdividedFaces.Add(new TriangleIndices(face.i2, b, a));
					subdividedFaces.Add(new TriangleIndices(face.i3, c, b));
					subdividedFaces.Add(new TriangleIndices(a, b, c));
				}
				faces = subdividedFaces;
			}

			// Reserve memory for enough UVs based on faces
			for (int i = 0; i < vertices.Count; i++)
				uvs.Add(UV.Zero);

			// Calculate spherical UV mapping based on inverse trigonometry
			for (int i = 0; i < vertices.Count; i++)
			{
				UV uv = uvs[i];
				uv.U = Math.Atan2(vertices[i].X, vertices[i].Z) / (-2f * Math.PI);
				if (uv.U < 0f)
					uv.U += 1f;
				uv.V = Math.Asin(vertices[i].Y) / Math.PI + 0.5f;
				uvs[i] = uv;
			}

			// Manually set duplicated poles for their quadrants
			uvs[0] = new UV(0.125f, uvs[0].V);
			uvs[1] = new UV(0.375f, uvs[1].V);
			uvs[2] = new UV(0.625f, uvs[2].V);
			uvs[3] = new UV(0.875f, uvs[3].V);
			uvs[9] = new UV(0.125f, uvs[9].V);
			uvs[10] = new UV(0.375f, uvs[10].V);
			uvs[11] = new UV(0.625f, uvs[11].V);
			uvs[12] = new UV(0.875f, uvs[12].V);

			// Remove UV seams
			float threshold = 0.5f;
			foreach (TriangleIndices face in faces)
			{
				if (uvs[face.i1].U - uvs[face.i2].U > threshold)
					uvs[face.i2] = new UV(1f, uvs[face.i2].V);

				if (uvs[face.i1].U - uvs[face.i3].U > threshold)
					uvs[face.i3] = new UV(1f, uvs[face.i3].V);


				if (uvs[face.i2].U - uvs[face.i1].U > threshold)
					uvs[face.i1] = new UV(1f, uvs[face.i1].V);

				if (uvs[face.i2].U - uvs[face.i3].U > threshold)
					uvs[face.i3] = new UV(1f, uvs[face.i3].V);


				if (uvs[face.i3].U - uvs[face.i1].U > threshold)
					uvs[face.i1] = new UV(1f, uvs[face.i1].V);

				if (uvs[face.i3].U - uvs[face.i2].U > threshold)
					uvs[face.i2] = new UV(1f, uvs[face.i2].V);
			}

			// Scale by radius
			for (int i = 0; i < vertices.Count; i++)
				vertices[i] *= radius;

			// Calculate geometry data
			if (flat)
			{
				Vertex[] assembledVertices = new Vertex[faces.Count * 3];
				ushort[] indices16 = new ushort[faces.Count * 3];
				uint[] indices32 = new uint[faces.Count * 3];
				for (int i = 0; i < faces.Count; i++)
				{
					Vector3 v1 = vertices[faces[i].i1];
					Vector3 v2 = vertices[faces[i].i2];
					Vector3 v3 = vertices[faces[i].i3];
					Vector3 normal = Vector3.Cross(v2 - v1, v2 - v3).Normalized;

					assembledVertices[3 * i] = new Vertex(v1, normal, Colour.White, uvs[faces[i].i1]);
					assembledVertices[3 * i + 1] = new Vertex(v2, normal, Colour.White, uvs[faces[i].i2]);
					assembledVertices[3 * i + 2] = new Vertex(v3, normal, Colour.White, uvs[faces[i].i3]);

					indices16[3 * i] = (ushort)(3 * i);
					indices16[3 * i + 1] = (ushort)(3 * i + 1);
					indices16[3 * i + 2] = (ushort)(3 * i + 2);

					indices32[3 * i] = (uint)(3 * i);
					indices32[3 * i + 1] = (uint)(3 * i + 1);
					indices32[3 * i + 2] = (uint)(3 * i + 2);
				}

				return new Geometry(assembledVertices, indices16, indices32);
			}
			else
			{
				Vertex[] assembledVertices = new Vertex[vertices.Count];
				ushort[] indices16 = new ushort[faces.Count * 3];
				uint[] indices32 = new uint[faces.Count * 3];
				for (int i = 0; i < faces.Count; i++)
				{
					indices16[3 * i] = (ushort)faces[i].i1;
					indices16[3 * i + 1] = (ushort)faces[i].i2;
					indices16[3 * i + 2] = (ushort)faces[i].i3;

					indices32[3 * i] = (uint)faces[i].i1;
					indices32[3 * i + 1] = (uint)faces[i].i2;
					indices32[3 * i + 2] = (uint)faces[i].i3;
				}

				for (int i = 0; i < assembledVertices.Length; i++)
					assembledVertices[i] = new Vertex(vertices[i], vertices[i].Normalized, Colour.White, uvs[i]);

				return new Geometry(assembledVertices, indices16, indices32);
			}
		}
		#endregion
	}
}