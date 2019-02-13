namespace CKGL
{
	public enum TextureAttachment : byte
	{
		Depth,
		DepthStencil,
		Colour0,
		Colour1,
		Colour2,
		Colour3,
		Colour4,
		Colour5,
		Colour6,
		Colour7,
		Colour8,
		Colour9,
		Colour10,
		Colour11,
		Colour12,
		Colour13,
		Colour14,
		Colour15
	}

	internal static class TextureAttachmentExt
	{
		internal static OpenGLBindings.TextureAttachment ToOpenGL(this TextureAttachment textureAttachment)
		{
			switch (textureAttachment)
			{
				case TextureAttachment.Depth:
					return OpenGLBindings.TextureAttachment.Depth;
				case TextureAttachment.DepthStencil:
					return OpenGLBindings.TextureAttachment.DepthStencil;
				case TextureAttachment.Colour0:
					return OpenGLBindings.TextureAttachment.Colour0;
				case TextureAttachment.Colour1:
					return OpenGLBindings.TextureAttachment.Colour1;
				case TextureAttachment.Colour2:
					return OpenGLBindings.TextureAttachment.Colour2;
				case TextureAttachment.Colour3:
					return OpenGLBindings.TextureAttachment.Colour3;
				case TextureAttachment.Colour4:
					return OpenGLBindings.TextureAttachment.Colour4;
				case TextureAttachment.Colour5:
					return OpenGLBindings.TextureAttachment.Colour5;
				case TextureAttachment.Colour6:
					return OpenGLBindings.TextureAttachment.Colour6;
				case TextureAttachment.Colour7:
					return OpenGLBindings.TextureAttachment.Colour7;
				case TextureAttachment.Colour8:
					return OpenGLBindings.TextureAttachment.Colour8;
				case TextureAttachment.Colour9:
					return OpenGLBindings.TextureAttachment.Colour9;
				case TextureAttachment.Colour10:
					return OpenGLBindings.TextureAttachment.Colour10;
				case TextureAttachment.Colour11:
					return OpenGLBindings.TextureAttachment.Colour11;
				case TextureAttachment.Colour12:
					return OpenGLBindings.TextureAttachment.Colour12;
				case TextureAttachment.Colour13:
					return OpenGLBindings.TextureAttachment.Colour13;
				case TextureAttachment.Colour14:
					return OpenGLBindings.TextureAttachment.Colour14;
				case TextureAttachment.Colour15:
					return OpenGLBindings.TextureAttachment.Colour15;
				default:
					throw new IllegalValueException(typeof(TextureAttachment), textureAttachment);
			}
		}
	}
}