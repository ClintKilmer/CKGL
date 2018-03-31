using System.Collections.Generic;

using OpenGL;
using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL
{
	public struct VertexBufferElement
	{
		VertexType VertexType;
		GLuint Count;
		bool Normalized;

		public VertexBufferElement(VertexType vertexType, GLuint count, bool normalized)
		{
			VertexType = vertexType;
			Count = count;
			Normalized = normalized;
		}
	};

	public class VertexBufferLayout
	{
		private List<VertexBufferElement> elements = new List<VertexBufferElement>();
		private GLuint stride = 0;

		public VertexBufferLayout() { }

		public void Push<T>(int count)
		{
			throw new System.Exception("Not Implemented Yet.");
		}

		public void Push(VertexType vertexType, GLuint count, bool normalized)
		{
			switch (vertexType)
			{
				case VertexType.Float:
					stride += 4;
					break;
				default:
					throw new System.Exception("Not Implemented Yet.");
			}
			elements.Add(new VertexBufferElement(vertexType, count, normalized));
		}
	}
}