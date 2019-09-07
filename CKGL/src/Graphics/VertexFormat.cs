namespace CKGL
{
	public struct VertexAttribute
	{
		public readonly DataType Type;
		public readonly int Count;
		public readonly int Size;
		public int Offset;
		public readonly bool Normalized;
		public readonly uint Divisor;

		public VertexAttribute(DataType type, int count, bool normalized) : this(type, count, normalized, 0) { }
		public VertexAttribute(DataType type, int count, bool normalized, uint divisor)
		{
			Type = type;
			Count = count;
			Size = count * type.Size();
			Offset = 0; // Calculated in VertexFormat constructor
			Normalized = normalized;
			Divisor = divisor;
		}

		internal void SetOffset(int offset)
		{
			Offset = offset;
		}

		#region Overrides
		public override string ToString()
		{
			return $"VertexAttribute: [Type: {Type}, Count: {Count}, Size: {Size}, Offset: {Offset}, Normalized: {Normalized}, Divisor: {Divisor}]";
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
				hash = hash * 23 + Divisor.GetHashCode();
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
		public VertexFormat(params VertexAttribute[] attributes) : this(4, attributes) { }
		public VertexFormat(int pack = 4, params VertexAttribute[] attributes)
		{
			if (attributes == null || attributes.Length == 0)
				throw new CKGLException("At least 1 VertexAttribute is required for a VertexFormat.");

			Attributes = attributes;
			Stride = 0;

			for (int i = 0; i < attributes.Length; i++)
			{
				attributes[i].Offset = Stride;
				Stride += attributes[i].Size;

				// Pad bytes to pack length
				if (Stride % pack > 0)
					Stride += pack - Stride % pack;
			}
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