using System;

namespace CKGL
{
	#region VertexDeclaration
	public interface IVertexType
	{
		VertexDeclaration VertexDeclaration { get; }
	}
	public class VertexDeclaration
	{
		public int Stride { get; private set; } = 0;
		private VertexElement[] elements;

		public VertexDeclaration(params VertexElement[] elements)
		{
			if ((elements == null) || (elements.Length == 0))
				throw new ArgumentNullException("elements", "Elements cannot be empty");

			this.elements = (VertexElement[])elements.Clone();

			int totalOffset = 0;
			for (int i = 0; i < elements.Length; i += 1)
			{
				// Set VertexElement size
				elements[i].Offset = totalOffset;

				// Set Vertex Stride
				Stride.Max(totalOffset += GetTypeSize(elements[i].VertexElementFormat));
			}
		}

		private static int GetTypeSize(VertexElementFormat elementFormat)
		{
			switch (elementFormat)
			{
				case VertexElementFormat.Single:
					return 4;
				case VertexElementFormat.Vector2:
					return 8;
				case VertexElementFormat.Vector3:
					return 12;
				case VertexElementFormat.Vector4:
					return 16;
				case VertexElementFormat.Colour:
					return 4;
				case VertexElementFormat.Byte4:
					return 4;
				case VertexElementFormat.Short2:
					return 4;
				case VertexElementFormat.Short4:
					return 8;
				case VertexElementFormat.NormalizedShort2:
					return 4;
				case VertexElementFormat.NormalizedShort4:
					return 8;
				case VertexElementFormat.HalfVector2:
					return 4;
				case VertexElementFormat.HalfVector4:
					return 8;
				default:
					throw new Exception("VertexElementFormat invalid");
			}
		}
	}
	#endregion

	#region VertexElement
	public struct VertexElement
	{
		public int Offset { get; set; }
		public VertexElementFormat VertexElementFormat { get; set; }
		public VertexElementUsage VertexElementUsage { get; set; }
		public int UsageIndex { get; set; }

		public VertexElement(VertexElementFormat elementFormat, VertexElementUsage elementUsage, int usageIndex) : this()
		{
			UsageIndex = usageIndex;
			VertexElementFormat = elementFormat;
			VertexElementUsage = elementUsage;
		}
	}
	#endregion

	#region VertexElementFormat
	public enum VertexElementFormat
	{
		/// <summary>
		/// Single 32-bit floating point number.
		/// </summary>
		Single,
		/// <summary>
		/// Two component 32-bit floating point number.
		/// </summary>
		Vector2,
		/// <summary>
		/// Three component 32-bit floating point number.
		/// </summary>
		Vector3,
		/// <summary>
		/// Four component 32-bit floating point number.
		/// </summary>
		Vector4,
		/// <summary>
		/// Four component, packed unsigned byte, mapped to 0 to 1 range.
		/// </summary>
		Colour,
		/// <summary>
		/// Four component unsigned byte.
		/// </summary>
		Byte4,
		/// <summary>
		/// Two component signed 16-bit integer.
		/// </summary>
		Short2,
		/// <summary>
		/// Four component signed 16-bit integer.
		/// </summary>
		Short4,
		/// <summary>
		/// Normalized, two component signed 16-bit integer.
		/// </summary>
		NormalizedShort2,
		/// <summary>
		/// Normalized, four component signed 16-bit integer.
		/// </summary>
		NormalizedShort4,
		/// <summary>
		/// Two component 16-bit floating point number.
		/// </summary>
		HalfVector2,
		/// <summary>
		/// Four component 16-bit floating point number.
		/// </summary>
		HalfVector4
	}
	#endregion

	#region VertexElementUsage
	public enum VertexElementUsage
	{
		/// <summary>
		/// Position data.
		/// </summary>
		Position,
		/// <summary>
		/// Colour data.
		/// </summary>
		Colour,
		/// <summary>
		/// Texture coordinate data or can be used for user-defined data.
		/// </summary>
		TextureCoordinate,
		/// <summary>
		/// Normal data.
		/// </summary>
		Normal,
		/// <summary>
		/// Binormal data.
		/// </summary>
		Binormal,
		/// <summary>
		/// Tangent data.
		/// </summary>
		Tangent,
		/// <summary>
		/// Blending indices data.
		/// </summary>
		BlendIndices,
		/// <summary>
		/// Blending weight data.
		/// </summary>
		BlendWeight,
		/// <summary>
		/// Depth data.
		/// </summary>
		Depth,
		/// <summary>
		/// Fog data.
		/// </summary>
		Fog,
		/// <summary>
		/// Point size data. Usable for drawing point sprites.
		/// </summary>
		PointSize,
		/// <summary>
		/// Sampler data for specifies the displacement value to look up.
		/// </summary>
		Sample,
		/// <summary>
		/// Single, positive float value, specifies a tessellation factor used in the tessellation unit to control the rate of tessellation.
		/// </summary>
		TessellateFactor
	}
	#endregion
}