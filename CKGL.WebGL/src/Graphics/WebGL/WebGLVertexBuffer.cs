using System.Linq;
using static CKGL.WebGL.WebGLGraphics; // WebGL Context Methods
using static Retyped.dom; // WebGL Types
using static Retyped.es5; // JS TypedArrays
using static Retyped.webgl2.WebGL2RenderingContext; // WebGL Enums

namespace CKGL.WebGL
{
	internal class WebGLVertexBuffer : VertexBuffer
	{
		private static WebGLBuffer currentlyBoundVertexBuffer;

		private WebGLBuffer buffer;

		internal WebGLVertexBuffer(BufferUsage bufferUsage)
		{
			buffer = GL.createBuffer();
			BufferUsage = bufferUsage;
		}

		public override void Destroy()
		{
			if (buffer != null)
			{
				GL.deleteBuffer(buffer);
				buffer = null;
			}
		}

		internal override void Bind()
		{
			if (buffer != currentlyBoundVertexBuffer)
			{
				GL.bindBuffer(ARRAY_BUFFER, buffer);
				currentlyBoundVertexBuffer = buffer;
			}
		}

		public override void LoadData(byte[] data)
		{
			Bind();
			GL.bufferData(ARRAY_BUFFER, new Uint8Array(data), BufferUsage.ToWebGL());
		}

		public override void LoadData<T>(T[] data, VertexFormat vertexFormat)// where T : struct // TODO - add this back in .NET Core 3.0
		{
			Bind();
			GL.bufferData(ARRAY_BUFFER, ConvertVertices(data, vertexFormat), BufferUsage.ToWebGL());
		}

		private ArrayBuffer ConvertVertices<T>(T[] data, VertexFormat vertexFormat)
		{
			ArrayBuffer buffer = new ArrayBuffer(data.Length * vertexFormat.Stride);

			var attributeNames = GetOwnPropertyNames(data[0]).Where(p => p[0] != '$').ToArray();

			var attributeNameIndex = 0;
			foreach (var vertexAttribute in vertexFormat.Attributes)
			{
				var attributeName = attributeNames[attributeNameIndex++];
				var attributeElementNames = GetOwnPropertyNames(data[0][attributeName]).Where(p => p[0] != '$').ToArray();
				//Output.WriteLine($"attributeName: {attributeName} | attributeElementNames: {string.Join(", ", attributeElementNames)}"); // Debug

				switch (vertexAttribute.Type)
				{
					case DataType.UnsignedByte:
						for (var i = 0; i < data.Length; i++)
						{
							var dataView = new Uint8Array(buffer, (uint)(vertexAttribute.Offset + i * vertexFormat.Stride));
							for (uint j = 0; j < vertexAttribute.Count; j++)
								dataView[j] = (byte)data[i][attributeName][attributeElementNames[j]];
						}
						break;
					case DataType.Byte:
						for (var i = 0; i < data.Length; i++)
						{
							var dataView = new Int8Array(buffer, (uint)(vertexAttribute.Offset + i * vertexFormat.Stride));
							for (uint j = 0; j < vertexAttribute.Count; j++)
								dataView[j] = (sbyte)data[i][attributeName][attributeElementNames[j]];
						}
						break;
					case DataType.UnsignedShort:
						for (var i = 0; i < data.Length; i++)
						{
							var dataView = new Uint16Array(buffer, (uint)(vertexAttribute.Offset + i * vertexFormat.Stride));
							for (uint j = 0; j < vertexAttribute.Count; j++)
								dataView[j] = (ushort)data[i][attributeName][attributeElementNames[j]];
						}
						break;
					case DataType.Short:
						for (var i = 0; i < data.Length; i++)
						{
							var dataView = new Int16Array(buffer, (uint)(vertexAttribute.Offset + i * vertexFormat.Stride));
							for (uint j = 0; j < vertexAttribute.Count; j++)
								dataView[j] = (short)data[i][attributeName][attributeElementNames[j]];
						}
						break;
					case DataType.UnsignedInt:
						for (var i = 0; i < data.Length; i++)
						{
							var dataView = new Uint32Array(buffer, (uint)(vertexAttribute.Offset + i * vertexFormat.Stride));
							for (uint j = 0; j < vertexAttribute.Count; j++)
								dataView[j] = (uint)data[i][attributeName][attributeElementNames[j]];
						}
						break;
					case DataType.Int:
						for (var i = 0; i < data.Length; i++)
						{
							var dataView = new Int32Array(buffer, (uint)(vertexAttribute.Offset + i * vertexFormat.Stride));
							for (uint j = 0; j < vertexAttribute.Count; j++)
								dataView[j] = (int)data[i][attributeName][attributeElementNames[j]];
						}
						break;
					case DataType.Float:
						for (var i = 0; i < data.Length; i++)
						{
							var dataView = new Float32Array(buffer, (uint)(vertexAttribute.Offset + i * vertexFormat.Stride));
							for (uint j = 0; j < vertexAttribute.Count; j++)
								dataView[j] = (float)data[i][attributeName][attributeElementNames[j]];
						}
						break;
					//case DataType.Double:
					//	for (var i = 0; i < data.Length; i++)
					//	{
					//		var dataView = new Float64Array(buffer, (uint)(vertexAttribute.Offset + i * vertexFormat.Stride));
					//		for (uint j = 0; j < vertexAttribute.Count; j++)
					//			dataView[j] = (double)data[i][attributeName][attributeElementNames[j]];
					//	}
					//	break;
					default:
						throw new IllegalValueException(typeof(DataType), vertexAttribute.Type);
				}
			}

			return buffer;
		}
	}
}