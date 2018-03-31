using System;
using System.Collections.Generic;

using OpenGL;
using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL
{
	public struct VertexBufferElement
	{
		public VertexType VertexType;
		public GLint Count;
		public bool Normalized;

		public GLint Size
		{
			get { return GetSizeOfType(VertexType); }
		}

		public VertexBufferElement(VertexType vertexType, GLint count, bool normalized)
		{
			VertexType = vertexType;
			Count = count;
			Normalized = normalized;
		}

		public static GLint GetSizeOfType(VertexType vertexType)
		{
			switch (vertexType)
			{
				case VertexType.Float:
					return sizeof(float);
				default:
					throw new NotImplementedException();
			}
		}
	};

	public class VertexBufferLayout
	{
		private List<VertexBufferElement> elements = new List<VertexBufferElement>();
		private GLint stride = 0;

		public VertexBufferLayout() { }

		public void Push<T>(GLint count)
		{
			if (typeof(T) == typeof(float))
				Push(VertexType.Float, count, false);
			else
				throw new NotImplementedException();
		}

		public void Push(VertexType vertexType, GLint count, bool normalized)
		{
			elements.Add(new VertexBufferElement(vertexType, count, normalized));
			stride += count * VertexBufferElement.GetSizeOfType(vertexType);
		}

		public void SetLayout(VertexBuffer vertexBuffer)
		{
			vertexBuffer.Bind();

			GLint offset = 0;
			for (GLuint i = 0; i < elements.Count; i++)
			{
				GL.EnableVertexAttribArray(i);
				GL.VertexAttribPointer(i, elements[(int)i].Count, elements[(int)i].VertexType, elements[(int)i].Normalized, stride, offset);
				offset += elements[(int)i].Count * elements[(int)i].Size;
			}
		}
	}
}