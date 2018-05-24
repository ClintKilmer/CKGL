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
				case VertexType.UnsignedByte:
					return sizeof(byte);
				case VertexType.Byte:
					return sizeof(sbyte);
				case VertexType.UnsignedShort:
					return sizeof(ushort);
				case VertexType.Short:
					return sizeof(short);
				case VertexType.UnsignedInt:
					return sizeof(uint);
				case VertexType.Int:
					return sizeof(int);
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
		public GLint Stride { get; private set; } = 0;

		public VertexBufferLayout() { }

		public void Push<T>(GLint count, bool normalized)
		{
			if (typeof(T) == typeof(byte))
				Push(VertexType.UnsignedByte, count, normalized);
			else if (typeof(T) == typeof(sbyte))
				Push(VertexType.Byte, count, normalized);
			else if (typeof(T) == typeof(ushort))
				Push(VertexType.UnsignedShort, count, normalized);
			else if (typeof(T) == typeof(short))
				Push(VertexType.Short, count, normalized);
			else if (typeof(T) == typeof(uint))
				Push(VertexType.UnsignedInt, count, normalized);
			else if (typeof(T) == typeof(int))
				Push(VertexType.Int, count, normalized);
			else if (typeof(T) == typeof(float))
				Push(VertexType.Float, count, normalized);
			else
				throw new NotImplementedException();
		}

		public void Push(VertexType vertexType, GLint count, bool normalized)
		{
			elements.Add(new VertexBufferElement(vertexType, count, normalized));
			Stride += count * VertexBufferElement.GetSizeOfType(vertexType);
		}

		public void SetLayout(VertexBuffer vertexBuffer)
		{
			vertexBuffer.Bind();

			GLint offset = 0;
			for (GLuint i = 0; i < elements.Count; i++)
			{
				GL.EnableVertexAttribArray(i);
				GL.VertexAttribPointer(i, elements[(int)i].Count, elements[(int)i].VertexType, elements[(int)i].Normalized, Stride, offset);
				offset += elements[(int)i].Count * elements[(int)i].Size;
			}
		}
	}
}