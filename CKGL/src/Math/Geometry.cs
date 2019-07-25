using System.Collections.Generic;

namespace CKGL
{
	public struct Geometry
	{
		public readonly Vector3[] Vertices;
		public readonly ushort[] Indices16;
		public readonly uint[] Indices32;
		public readonly Vector3[] Normals;
		public readonly UV[] UVs;

		internal Geometry(Vector3[] vertices, ushort[] indices16, uint[] indices32, Vector3[] normals, UV[] uvs)
		{
			Vertices = vertices;
			Indices16 = indices16;
			Indices32 = indices32;
			Normals = normals;
			UVs = uvs;
		}

		private struct TriangleIndices
		{
			public int v1;
			public int v2;
			public int v3;

			public TriangleIndices(int v1, int v2, int v3)
			{
				this.v1 = v1;
				this.v2 = v2;
				this.v3 = v3;
			}
		}

		public static Geometry Icosphere(float radius = 1f, uint subdivisions = 1)
		{
			List<Vector3> vertices = new List<Vector3>();
			List<TriangleIndices> faces = new List<TriangleIndices>();

			int generateSubdividedVertex(int index1, int index2)
			{
				vertices.Add((((index1 < index2 ? vertices[index1] : vertices[index2]) + (index1 < index2 ? vertices[index2] : vertices[index1])) * 0.5f).Normalized * radius);
				//vertices.Add(new Vector3(
				//	(vertices[index1].X + vertices[index2].X) / 2f,
				//	(vertices[index1].Y + vertices[index2].Y) / 2f,
				//	(vertices[index1].Z + vertices[index2].Z) / 2f).Normalized * radius);

				return vertices.Count;
			}

			// Icosahedron vertices
			float t = (1f + Math.Sqrt(5f)) / 2f;
			// XY plane
			vertices.Add(new Vector3(-1f,  t, 0f).Normalized * radius);
			vertices.Add(new Vector3( 1f,  t, 0f).Normalized * radius);
			vertices.Add(new Vector3(-1f, -t, 0f).Normalized * radius);
			vertices.Add(new Vector3( 1f, -t, 0f).Normalized * radius);
			// YZ plane
			vertices.Add(new Vector3(0f, -1f,  t).Normalized * radius);
			vertices.Add(new Vector3(0f,  1f,  t).Normalized * radius);
			vertices.Add(new Vector3(0f, -1f, -t).Normalized * radius);
			vertices.Add(new Vector3(0f,  1f, -t).Normalized * radius);
			// XZ plane
			vertices.Add(new Vector3( t, 0f, -1f).Normalized * radius);
			vertices.Add(new Vector3( t, 0f,  1f).Normalized * radius);
			vertices.Add(new Vector3(-t, 0f, -1f).Normalized * radius);
			vertices.Add(new Vector3(-t, 0f,  1f).Normalized * radius);

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
			faces.Add(new TriangleIndices (9,  1,  8));

			// Subdivide faces
			for (int i = 0; i < subdivisions; i++)
			{
				List<TriangleIndices> subdividedFaces = new List<TriangleIndices>();
				foreach (TriangleIndices tri in faces)
				{
					// Subdivide each face with 4 new faces
					int a = generateSubdividedVertex(tri.v1, tri.v2);
					int b = generateSubdividedVertex(tri.v2, tri.v3);
					int c = generateSubdividedVertex(tri.v3, tri.v1);

					//subdividedFaces.Add(new TriangleIndices(tri.v1, c, a));
					//subdividedFaces.Add(new TriangleIndices(tri.v2, a, b));
					//subdividedFaces.Add(new TriangleIndices(tri.v3, b, c));
					//subdividedFaces.Add(new TriangleIndices(a, b, c));
					subdividedFaces.Add(new TriangleIndices(tri.v1, tri.v2, a));
				}
				faces = subdividedFaces;
			}

			// Calculate geometry data
			ushort[] indices16 = new ushort[faces.Count * 3];
			uint[] indices32 = new uint[faces.Count * 3];
			for (int i = 0; i < faces.Count; i++)
			{
				indices16[3 * i] = (ushort)faces[i].v1;
				indices16[3 * i + 1] = (ushort)faces[i].v2;
				indices16[3 * i + 2] = (ushort)faces[i].v3;
				indices32[3 * i] = (uint)faces[i].v1;
				indices32[3 * i + 1] = (uint)faces[i].v2;
				indices32[3 * i + 2] = (uint)faces[i].v3;
			}

			Vector3[] normals = new Vector3[vertices.Count];
			for (int i = 0; i < normals.Length; i++)
				normals[i] = vertices[i].Normalized;

			UV[] uvs = new UV[vertices.Count];

			return new Geometry(vertices.ToArray(), indices16, indices32, normals, uvs);
		}
	}
}