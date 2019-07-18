using System;

namespace CKGL
{
	public struct VertexAttribute
	{
		public readonly DataType Type;
		public readonly int Count;
		public readonly int Size;
		public readonly bool Normalized;

		public VertexAttribute(DataType type, int count, bool normalized)
		{
			Type = type;
			Count = count;
			Normalized = normalized;

			switch (Type)
			{
				case DataType.UnsignedByte:
					Size = count * sizeof(byte);
					break;
				case DataType.Byte:
					Size = count * sizeof(sbyte);
					break;
				case DataType.UnsignedShort:
					Size = count * sizeof(ushort);
					break;
				case DataType.Short:
					Size = count * sizeof(short);
					break;
				case DataType.UnsignedInt:
					Size = count * sizeof(uint);
					break;
				case DataType.Int:
					Size = count * sizeof(int);
					break;
				case DataType.Float:
					Size = count * sizeof(float);
					break;
				default:
					throw new NotImplementedException("Unknown VertexType");
			}
		}

		#region Overrides
		public override string ToString()
		{
			return $"VertexAttribute: [Type: {Type}, Count: {Count}, Size: {Size}, Normalized: {Normalized}]";
		}

		public override bool Equals(object obj)
		{
			return obj is VertexAttribute && Equals((VertexAttribute)obj);
		}
		public bool Equals(VertexAttribute vertexAttribute)
		{
			return this == vertexAttribute;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + Type.GetHashCode();
				hash = hash * 23 + Count.GetHashCode();
				hash = hash * 23 + Normalized.GetHashCode();
				return hash;
			}
		}
		#endregion

		#region Operators
		public static bool operator ==(VertexAttribute a, VertexAttribute b)
		{
			return a.Type == b.Type &&
				   a.Count == b.Count &&
				   a.Normalized == b.Normalized;
		}

		public static bool operator !=(VertexAttribute a, VertexAttribute b)
		{
			return a.Type != b.Type ||
				   a.Count != b.Count ||
				   a.Normalized != b.Normalized;
		}
		#endregion
	}

	public struct VertexFormat
	{
		public readonly VertexAttribute[] Attributes;
		public readonly int Stride;

		#region Constructors
		public VertexFormat(params VertexAttribute[] attributes) : this(0, attributes) { }
		public VertexFormat(int stride, params VertexAttribute[] attributes)
		{
			if (attributes == null || attributes.Length == 0)
				throw new ArgumentNullException("attributes", "At least 1 attribute is required");

			Attributes = attributes;

			Stride = 0;
			if (stride > 0)
				Stride = stride;
			else
				foreach (VertexAttribute attribute in attributes)
					Stride += attribute.Size;
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"VertexFormat: [Attributes: {Attributes.Length}, Stride: {Stride}]";
		}

		public override bool Equals(object obj)
		{
			return obj is VertexFormat && Equals((VertexFormat)obj);
		}
		public bool Equals(VertexFormat vertexFormat)
		{
			return this == vertexFormat;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				foreach (VertexAttribute attribute in Attributes)
					hash = hash * 23 + attribute.GetHashCode();
				return hash;
			}
		}
		#endregion

		#region Operators
		public static bool operator ==(VertexFormat a, VertexFormat b)
		{
			if (a.Stride == b.Stride)
			{
				if (a.Attributes.Length == b.Attributes.Length)
				{
					for (int i = 0; i < a.Attributes.Length; i++)
					{
						if (a.Attributes[i] != b.Attributes[i])
							return false;
					}

					return true;
				}
			}

			return false;
		}

		public static bool operator !=(VertexFormat a, VertexFormat b)
		{
			return !(a == b);
		}
		#endregion
	}
}