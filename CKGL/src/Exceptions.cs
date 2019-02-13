using System;

namespace CKGL
{
	internal class CKGLException : Exception
	{
		public CKGLException()
		{
		}

		public CKGLException(string message) : base(message)
		{
		}

		public CKGLException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}

	internal class IllegalValueException : CKGLException
	{
		public IllegalValueException(Type type, object value) : base($"Illegal Value:\nType: [{type}], Value: {value.ToString()}")
		{
		}
	}
}