using System.Linq;
using static CKGL.WebGL.WebGLGraphics; // WebGL Context Methods
using static Retyped.dom; // DOM / WebGL Types
using static Retyped.es5; // JS TypedArrays

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
				GL.bindBuffer(GL.ARRAY_BUFFER, buffer);
				currentlyBoundVertexBuffer = buffer;
			}
		}

		public override void LoadData(byte[] data)
		{
			Bind();
			GL.bufferData(GL.ARRAY_BUFFER, new Uint8Array(data), BufferUsage.ToWebGL());
		}

		public override void LoadData(ArrayBuffer data)
		{
			Bind();
			GL.bufferData(GL.ARRAY_BUFFER, data, BufferUsage.ToWebGL());
		}

		public override void LoadData<T>(T[] data, VertexFormat vertexFormat)// where T : struct // TODO - add this back in .NET Core 3.0
		{
			Bind();
			GL.bufferData(GL.ARRAY_BUFFER, ConvertVertices(data, vertexFormat), BufferUsage.ToWebGL());
		}

		[Bridge.Rules(Integer = Bridge.IntegerRule.Plain)]
		private ArrayBuffer ConvertVertices<T>(T[] data, VertexFormat vertexFormat)
		{
			ArrayBuffer buffer = new ArrayBuffer(data.Length * vertexFormat.Stride);
			Int8Array byteView = new Int8Array(buffer);
			Uint8Array ubyteView = new Uint8Array(buffer);
			Int16Array shortView = new Int16Array(buffer);
			Uint16Array ushortView = new Uint16Array(buffer);
			Int32Array intView = new Int32Array(buffer);
			Uint32Array uintView = new Uint32Array(buffer);
			Float32Array floatView = new Float32Array(buffer);
			//Float64Array doubleView = new Float64Array(buffer);

			var attributeNames = GetOwnPropertyNames(data[0]).Where(p => p[0] != '$').ToArray();

			var attributeNameIndex = 0;
			foreach (var vertexAttribute in vertexFormat.Attributes)
			{
				var attributeName = attributeNames[attributeNameIndex++];
				var attributeElementNames = GetOwnPropertyNames(data[0][attributeName]).Where(p => p[0] != '$').ToArray();
				//Output.WriteLine($"attributeName: {attributeName} | attributeElementNames: {string.Join(", ", attributeElementNames)}"); // Debug

				for (uint i = 0; i < data.Length; i++)
				{
					uint offset = (uint)(vertexAttribute.Offset + i * vertexFormat.Stride);
					for (uint j = 0; j < vertexAttribute.Count; j++)
					{
						switch (vertexAttribute.Type)
						{
							case DataType.Byte:
								byteView[offset / (uint)vertexAttribute.Type.Size() + j] = (sbyte)(attributeElementNames.Length == 0 ? data[i][attributeName] : data[i][attributeName][attributeElementNames[j]]);
								break;
							case DataType.UnsignedByte:
								ubyteView[offset / (uint)vertexAttribute.Type.Size() + j] = (byte)(attributeElementNames.Length == 0 ? data[i][attributeName] : data[i][attributeName][attributeElementNames[j]]);
								break;
							case DataType.Short:
								shortView[offset / (uint)vertexAttribute.Type.Size() + j] = (short)(attributeElementNames.Length == 0 ? data[i][attributeName] : data[i][attributeName][attributeElementNames[j]]);
								break;
							case DataType.UnsignedShort:
								ushortView[offset / (uint)vertexAttribute.Type.Size() + j] = (ushort)(attributeElementNames.Length == 0 ? data[i][attributeName] : data[i][attributeName][attributeElementNames[j]]);
								break;
							case DataType.Int:
								intView[offset / (uint)vertexAttribute.Type.Size() + j] = (int)(attributeElementNames.Length == 0 ? data[i][attributeName] : data[i][attributeName][attributeElementNames[j]]);
								break;
							case DataType.UnsignedInt:
								uintView[offset / (uint)vertexAttribute.Type.Size() + j] = (uint)(attributeElementNames.Length == 0 ? data[i][attributeName] : data[i][attributeName][attributeElementNames[j]]);
								break;
							case DataType.Float:
								floatView[offset / (uint)vertexAttribute.Type.Size() + j] = (float)(attributeElementNames.Length == 0 ? data[i][attributeName] : data[i][attributeName][attributeElementNames[j]]);
								break;
							//case DataType.Double:
							//	doubleView[offset / (uint)vertexAttribute.Type.Size() + j] = (double)(attributeElementNames.Length == 0 ? data[i][attributeName] : data[i][attributeName][attributeElementNames[j]]);
							//	break;
							default:
								throw new IllegalValueException(typeof(DataType), vertexAttribute.Type);
						}
					}
				}
			}

			return buffer;
		}
	}
}