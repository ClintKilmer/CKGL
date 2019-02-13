/*
 * Original from:
 * OpenGL.cs in the Rise Library: https://github.com/ChevyRay/Rise
 * Modified for use in CKGL
 */

using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using GLint = System.Int32;
using GLuint = System.UInt32;

namespace CKGL.OpenGLBindings
{
	internal static class GL
	{
		// Strings
		public static string Version { get; private set; }
		public static string GLSLVersion { get; private set; }
		public static string Vendor { get; private set; }
		public static string Renderer { get; private set; }
		public static string Extensions { get; private set; }

		// Integers
		public static GLint MajorVersion { get; private set; }
		public static GLint MinorVersion { get; private set; }
		public static GLint MaxColourAttachments { get; private set; }
		public static GLint MaxCubeMapTextureSize { get; private set; }
		public static GLint MaxDrawBuffers { get; private set; }
		public static GLint MaxElementIndices { get; private set; }
		public static GLint MaxElementVertices { get; private set; }
		public static GLint MaxRenderbufferSize { get; private set; }
		public static GLint MaxSamples { get; private set; }
		public static GLint MaxTextureImageUnits { get; private set; }
		public static GLint MaxTextureSize { get; private set; }

		[AttributeUsage(AttributeTargets.Field)]
		public class Shared : Attribute { }

		[AttributeUsage(AttributeTargets.Field)]
		public class OpenGL : Attribute { }

		[AttributeUsage(AttributeTargets.Field)]
		public class OpenGLES : Attribute { }

		[AttributeUsage(AttributeTargets.Field)]
		public class DesktopES : Attribute { }

		private static Delegate GetProcAddress(string name, Type type)
		{
			IntPtr addr = Platform.GetProcAddress(name);

			if (addr == IntPtr.Zero)
				throw new Exception($"OpenGL function not available: {name}");

			return Marshal.GetDelegateForFunctionPointer(addr, type);
		}

		private static Delegate GetProcAddressEXT(string name, Type type, string ext = "EXT")
		{
			IntPtr addr = Platform.GetProcAddress(name);

			if (addr == IntPtr.Zero)
				addr = Platform.GetProcAddress(name + ext);

			if (addr == IntPtr.Zero)
				throw new Exception($"OpenGL function not available: {name}{ext}");

			return Marshal.GetDelegateForFunctionPointer(addr, type);
		}

		public static void Init()
		{
			bool es = Platform.GraphicsBackend == GraphicsBackend.OpenGLES;
			IntPtr addr;

			string baseError;
			if (es)
				baseError = "OpenGL ES 3.0 support is required.";
			else
				baseError = "OpenGL 2.1 support is required.";

			#region Attribute Reflection Functions
			try
			{
				foreach (var field in typeof(GL).GetFields(BindingFlags.NonPublic | BindingFlags.Static).Where(e => e.IsDefined(typeof(Shared))))
				{
					field.SetValue(null, GetProcAddress(field.Name, field.FieldType));
					// Debug
					Output.WriteLine($"Shared - {field.Name}");
				}

				if (!es)
				{
					foreach (var field in typeof(GL).GetFields(BindingFlags.NonPublic | BindingFlags.Static).Where(e => e.IsDefined(typeof(OpenGL))))
					{
						field.SetValue(null, GetProcAddress(field.Name, field.FieldType));
						// Debug
						Output.WriteLine($"OpenGL - {field.Name}");
					}
				}

				if (es)
				{
					foreach (var field in typeof(GL).GetFields(BindingFlags.NonPublic | BindingFlags.Static).Where(e => e.IsDefined(typeof(OpenGLES))))
					{
						field.SetValue(null, GetProcAddress(field.Name, field.FieldType));
						// Debug
						Output.WriteLine($"OpenGL ES - {field.Name}");
					}

					// NVIDIA / Desktop ES Functions
					//  Not normally supported in ES, but possibly supported on
					//  NVIDIA or Desktop ES, so try to aquire but fail silently.
					foreach (var field in typeof(GL).GetFields(BindingFlags.NonPublic | BindingFlags.Static).Where(e => e.IsDefined(typeof(DesktopES))))
					{
						try
						{
							field.SetValue(null, GetProcAddress(field.Name, field.FieldType));
							// Debug
							Output.WriteLine($"Desktop ES - {field.Name}");
						}
						catch { }
					}
				}
			}
			catch (Exception e)
			{
				throw new Exception($"{baseError}\nEntry Point: {e.Message}");
			}
			#endregion

			#region OpenGL ES float -> GL double Functions
			addr = Platform.GetProcAddress("glDepthRange");
			if (addr != IntPtr.Zero)
			{
				glDepthRange = (_glDepthRange)Marshal.GetDelegateForFunctionPointer(addr, typeof(_glDepthRange));
			}
			else
			{
				try
				{
					glDepthRangef = (_glDepthRangef)GetProcAddress("glDepthRangef", typeof(_glDepthRangef));
				}
				catch (Exception e)
				{
					throw new Exception($"{baseError}\nEntry Point: {e.Message}");
				}
				glDepthRange = (min, max) => glDepthRangef((float)min, (float)max);
			}
			addr = Platform.GetProcAddress("glClearDepth");
			if (addr != IntPtr.Zero)
			{
				glClearDepth = (_glClearDepth)Marshal.GetDelegateForFunctionPointer(addr, typeof(_glClearDepth));
			}
			else
			{
				try
				{
					glClearDepthf = (_glClearDepthf)GetProcAddress("glClearDepthf", typeof(_glClearDepthf));
				}
				catch (Exception e)
				{
					throw new Exception($"{baseError}\nEntry Point: {e.Message}");
				}
				glClearDepth = (depth) => glClearDepthf((float)depth);
			}
			#endregion

			// Strings
			Version = GetString(Strings.Version);
			GLSLVersion = GetString(Strings.GLSLVersion);
			Vendor = GetString(Strings.Vendor);
			Renderer = GetString(Strings.Renderer);
			Extensions = GetString(Strings.Extensions);

			// Integers
			MajorVersion = GetIntegerv(Integers.MajorVersion);
			MinorVersion = GetIntegerv(Integers.MinorVersion);
			MaxColourAttachments = GetIntegerv(Integers.MaxColourAttachments);
			MaxCubeMapTextureSize = GetIntegerv(Integers.MaxCubeMapTextureSize);
			MaxDrawBuffers = GetIntegerv(Integers.MaxDrawBuffers);
			MaxElementIndices = GetIntegerv(Integers.MaxElementIndices);
			MaxElementVertices = GetIntegerv(Integers.MaxElementVertices);
			MaxRenderbufferSize = GetIntegerv(Integers.MaxRenderbufferSize);
			MaxSamples = GetIntegerv(Integers.MaxSamples);
			MaxTextureImageUnits = GetIntegerv(Integers.MaxTextureImageUnits);
			MaxTextureSize = GetIntegerv(Integers.MaxTextureSize);

			// Debug
			Output.WriteLine($"OpenGL Initialized");
			Output.WriteLine($"OpenGL Version: {MajorVersion}.{MinorVersion}");
			Output.WriteLine($"OpenGL Version: {Version}");
			Output.WriteLine($"OpenGL GLSL Version: {GLSLVersion}");
			Output.WriteLine($"OpenGL Vendor: {Vendor}");
			Output.WriteLine($"OpenGL Renderer: {Renderer}");
			Output.WriteLine($"OpenGL MaxColourAttachments: {MaxColourAttachments}");
			Output.WriteLine($"OpenGL MaxCubeMapTextureSize: {MaxCubeMapTextureSize}");
			Output.WriteLine($"OpenGL MaxDrawBuffers: {MaxDrawBuffers}");
			Output.WriteLine($"OpenGL MaxElementIndices: {MaxElementIndices}");
			Output.WriteLine($"OpenGL MaxElementVertices: {MaxElementVertices}");
			Output.WriteLine($"OpenGL MaxRenderbufferSize: {MaxRenderbufferSize}");
			Output.WriteLine($"OpenGL MaxSamples: {MaxSamples}");
			Output.WriteLine($"OpenGL MaxTextureImageUnits: {MaxTextureImageUnits}");
			Output.WriteLine($"OpenGL MaxTextureSize: {MaxTextureSize}");
			//Output.WriteLine($"OpenGL - Extensions: {GetString(Strings.Extensions)}");
		}

#pragma warning disable CS0649
#pragma warning disable IDE0044

		#region Get Error/String/Integer Functions
		[Shared]
		private static _glGetError glGetError;
		private delegate ErrorCode _glGetError();
		[Conditional("DEBUG")]
		private static void CheckError()
		{
			var err = glGetError();
			Debug.Assert(err == ErrorCode.NoError, $"OpenGL Error {(int)err:X}: {err.ToString()}");
		}

		[Shared]
		private static _glGetString glGetString;
		private delegate IntPtr _glGetString(Strings name);
		public unsafe static string GetString(Strings name)
		{
			string s = new string((sbyte*)glGetString(name));
			CheckError();
			return s;
		}

		[Shared]
		private static _glGetIntegerv glGetIntegerv;
		private unsafe delegate void _glGetIntegerv(Integers name, GLint* data);
		private unsafe static void GetIntegerv(Integers name, out GLint val)
		{
			fixed (GLint* p = &val)
			{
				glGetIntegerv(name, p);
				val = *p;
			}
			CheckError();
		}
		public static GLint GetIntegerv(Integers name)
		{
			GetIntegerv(name, out GLint val);
			return val;
		}
		#endregion

		#region Enable/Disable Functions
		[Shared]
		private static _glEnable glEnable;
		private delegate void _glEnable(EnableCap mode);
		public static void Enable(EnableCap mode)
		{
			glEnable(mode);
			CheckError();
		}

		[Shared]
		private static _glDisable glDisable;
		private delegate void _glDisable(EnableCap mode);
		public static void Disable(EnableCap mode)
		{
			glDisable(mode);
			CheckError();
		}
		#endregion

		#region Viewport/Scissor Functions
		[Shared]
		private static _glViewport glViewport;
		private delegate void _glViewport(GLint x, GLint y, GLint width, GLint height);
		public static void Viewport(int x, int y, int width, int height)
		{
			glViewport(x, y, width, height);
			CheckError();
		}

		[Shared]
		private static _glScissor glScissor;
		private delegate void _glScissor(GLint x, GLint y, GLint width, GLint height);
		public static void Scissor(int x, int y, int width, int height)
		{
			glScissor(x, y, width, height);
			CheckError();
		}
		#endregion

		#region Colour Mask Functions
		[Shared]
		private static _glColorMask glColorMask;
		private delegate void _glColorMask(bool red​, bool green​, bool blue​, bool alpha);
		public static void ColourMask(bool red​, bool green​, bool blue​, bool alpha​)
		{
			glColorMask(red​, green​, blue​, alpha​);
			CheckError();
		}

		[OpenGL]
		private static _glColorMaski glColorMaski;
		private delegate void _glColorMaski(GLuint buf, bool red​, bool green​, bool blue​, bool alpha​);
		public static void ColourMask(uint buf, bool red​, bool green​, bool blue​, bool alpha​)
		{
			glColorMaski(buf, red​, green​, blue​, alpha​);
			CheckError();
		}
		#endregion

		#region Blend State Functions
		[Shared]
		private static _glBlendColor glBlendColor;
		private delegate void _glBlendColor(float red, float green, float blue, float alpha);
		public static void BlendColour(float red, float green, float blue, float alpha)
		{
			glBlendColor(red, green, blue, alpha);
			CheckError();
		}

		[Shared]
		private static _glBlendEquation glBlendEquation;
		private delegate void _glBlendEquation(BlendEquation eq);
		public static void BlendEquation(BlendEquation eq)
		{
			glBlendEquation(eq);
			CheckError();
		}

		[Shared]
		private static _glBlendEquationSeparate glBlendEquationSeparate;
		private delegate void _glBlendEquationSeparate(BlendEquation modeRGB, BlendEquation modeAlpha);
		public static void BlendEquationSeparate(BlendEquation modeRGB, BlendEquation modeAlpha)
		{
			glBlendEquationSeparate(modeRGB, modeAlpha);
			CheckError();
		}

		[Shared]
		private static _glBlendFunc glBlendFunc;
		private delegate void _glBlendFunc(BlendFactor sfactor, BlendFactor dfactor);
		public static void BlendFunc(BlendFactor sFactor, BlendFactor dFactor)
		{
			glBlendFunc(sFactor, dFactor);
			CheckError();
		}

		[Shared]
		private static _glBlendFuncSeparate glBlendFuncSeparate;
		private delegate void _glBlendFuncSeparate(BlendFactor srcRGB, BlendFactor dstRGB, BlendFactor srcAlpha, BlendFactor dstAlpha);
		public static void BlendFuncSeparate(BlendFactor srcRGB, BlendFactor dstRGB, BlendFactor srcAlpha, BlendFactor dstAlpha)
		{
			glBlendFuncSeparate(srcRGB, dstRGB, srcAlpha, dstAlpha);
			CheckError();
		}
		#endregion

		#region Depth State Functions
		[Shared]
		private static _glDepthMask glDepthMask;
		private delegate void _glDepthMask(bool flag);
		public static void DepthMask(bool flag)
		{
			glDepthMask(flag);
			CheckError();
		}

		[Shared]
		private static _glDepthFunc glDepthFunc;
		private delegate void _glDepthFunc(DepthFunc func);
		public static void DepthFunc(DepthFunc func)
		{
			glDepthFunc(func);
			CheckError();
		}

		private static _glDepthRange glDepthRange;
		private delegate void _glDepthRange(double nearVal, double farVal);
		private static _glDepthRangef glDepthRangef;
		private delegate void _glDepthRangef(float n, float f);
		public static void DepthRange(double min, double max)
		{
			glDepthRange(min, max);
			CheckError();
		}
		#endregion

		#region Stencil State Functions
		[Shared]
		private static _glStencilMask glStencilMask;
		private delegate void _glStencilMask(GLuint mask);
		public static void StencilMask(uint mask)
		{
			glStencilMask(mask);
			CheckError();
		}

		[Shared]
		private static _glStencilMaskSeparate glStencilMaskSeparate;
		private delegate void _glStencilMaskSeparate(Face face, GLuint mask);
		public static void StencilMaskSeparate(Face face, uint mask)
		{
			glStencilMaskSeparate(face, mask);
			CheckError();
		}

		[Shared]
		private static _glStencilFunc glStencilFunc;
		private delegate void _glStencilFunc(StencilFunc func, GLint reference, GLuint mask);
		public static void StencilFunc(StencilFunc func, int reference, uint mask)
		{
			glStencilFunc(func, reference, mask);
			CheckError();
		}

		[Shared]
		private static _glStencilFuncSeparate glStencilFuncSeparate;
		private delegate void _glStencilFuncSeparate(Face face, StencilFunc func, GLint reference, GLuint mask);
		public static void StencilFuncSeparate(Face face, StencilFunc func, int reference, uint mask)
		{
			glStencilFuncSeparate(face, func, reference, mask);
			CheckError();
		}

		[Shared]
		private static _glStencilOp glStencilOp;
		private delegate void _glStencilOp(StencilOp sfail, StencilOp dpfail, StencilOp dppass);
		public static void StencilOp(StencilOp sfail, StencilOp dpfail, StencilOp dppass)
		{
			glStencilOp(sfail, dpfail, dppass);
			CheckError();
		}

		[Shared]
		private static _glStencilOpSeparate glStencilOpSeparate;
		private delegate void _glStencilOpSeparate(Face face, StencilOp sfail, StencilOp dpfail, StencilOp dppass);
		public static void StencilOpSeparate(Face face, StencilOp sfail, StencilOp dpfail, StencilOp dppass)
		{
			glStencilOpSeparate(face, sfail, dpfail, dppass);
			CheckError();
		}
		#endregion

		#region Rasterizer State Functions
		[Shared]
		private static _glFrontFace glFrontFace;
		private delegate void _glFrontFace(FrontFace mode);
		public static void FrontFace(FrontFace face)
		{
			glFrontFace(face);
			CheckError();
		}

		[Shared]
		private static _glCullFace glCullFace;
		private delegate void _glCullFace(Face mode);
		public static void CullFace(Face face)
		{
			glCullFace(face);
			CheckError();
		}

		[Shared]
		private static _glPolygonOffset glPolygonOffset;
		private delegate void _glPolygonOffset(float factor, float units);
		public static void PolygonOffset(float factor, float units)
		{
			glPolygonOffset(factor, units);
			CheckError();
		}

		[OpenGL, DesktopES]
		private static _glPolygonMode glPolygonMode;
		private delegate void _glPolygonMode(Face face, PolygonMode mode);
		public static void PolygonMode(PolygonMode mode)
		{
			if (glPolygonMode != null)
			{
				// OpenGL 3.2 deprecated separate front/back states for PolygonMode, so hardcode the only Face enum allowed
				glPolygonMode(Face.FrontAndBack, mode);
				CheckError();
			}
		}
		#endregion

		#region Clear Functions
		[Shared]
		private static _glClear glClear;
		private delegate void _glClear(BufferBit mask);
		public static void Clear(BufferBit mask)
		{
			glClear(mask);
			CheckError();
		}

		[Shared]
		private static _glClearColor glClearColor;
		private delegate void _glClearColor(float red, float green, float blue, float alpha);
		public static void ClearColour(float red, float green, float blue, float alpha)
		{
			glClearColor(red, green, blue, alpha);
			CheckError();
		}
		public static void ClearColour(Colour colour)
		{
			ClearColour(colour.R, colour.G, colour.B, colour.A);
		}

		private static _glClearDepth glClearDepth;
		private delegate void _glClearDepth(double depth);
		private static _glClearDepthf glClearDepthf;
		private delegate void _glClearDepthf(float depth);
		public static void ClearDepth(double depth)
		{
			glClearDepth(depth);
			CheckError();
		}
		#endregion

		#region Texture Functions
		#endregion

		#region Buffer Functions
		//[OpenGL]
		//[DesktopES]
		//glGetBufferSubData
		//if (glGetBufferSubData != null)
		//{
		//}
		#endregion

		#region Framebuffer Functions
		#endregion

		#region Vertex Attribute Functions
		#endregion

		#region Drawing Functions
		#endregion

		#region Query Functions
		#endregion

		#region 3.2 Core Functions
		#endregion

		#region Debug Output Functions
		#endregion

		//////////////////////////////////////////////////////////

		private unsafe delegate void _glGenTextures(GLint n, uint* textures);
		private static _glGenTextures glGenTextures;
		public unsafe static void GenTextures(GLint n, uint[] textures)
		{
			fixed (uint* ptr = textures) { glGenTextures(n, ptr); }
			CheckError();
		}
		public unsafe static uint GenTexture()
		{
			uint texture = 0;
			glGenTextures(1, &texture);
			CheckError();
			return texture;
		}

		private unsafe delegate void _glDeleteTextures(GLint n, uint* textures);
		private static _glDeleteTextures glDeleteTextures;
		public unsafe static void DeleteTextures(GLint n, uint[] textures)
		{
			fixed (uint* ptr = textures) { glDeleteTextures(n, ptr); }
			CheckError();
		}
		public unsafe static void DeleteTexture(uint texture)
		{
			glDeleteTextures(1, &texture);
			CheckError();
		}

		private delegate void _glActiveTexture(GLuint textureImageUnit);
		private static _glActiveTexture glActiveTexture;
		public static void ActiveTexture(GLuint textureImageUnit)
		{
			if (textureImageUnit >= MaxTextureImageUnits)
				throw new Exception("ActiveTexture textureImageUnit must be between 0 and " + (MaxTextureImageUnits - 1));

			// TextureUnit0 = 0x84C0
			glActiveTexture(0x84C0 + textureImageUnit);
			CheckError();
		}

		private delegate void _glBindTexture(TextureTarget target, uint texture);
		private static _glBindTexture glBindTexture;
		public static void BindTexture(TextureTarget target, uint texture)
		{
			glBindTexture(target, texture);
			CheckError();
		}

		private delegate void _glTexParameteri(TextureTarget target, TextureParam name, int param);
		private static _glTexParameteri glTexParameteri;
		public static void TexParameterI(TextureTarget target, TextureParam name, int param)
		{
			glTexParameteri(target, name, param);
			CheckError();
		}

		private delegate void _glGetTexParameteriv(TextureTarget target, TextureParam name, out int result);
		private static _glGetTexParameteriv glGetTexParameteriv;
		public static void GetTexParameterI(TextureTarget target, TextureParam name, out int result)
		{
			glGetTexParameteriv(target, name, out result);
			CheckError();
		}

		private delegate void _glTexImage2D(TextureTarget target, int level, int internalFormat, GLint width, GLint height, int border, PixelFormat format, PixelType type, IntPtr data);
		private static _glTexImage2D glTexImage2D;
		public static void TexImage2D(TextureTarget target, int level, TextureFormat internalFormat, GLint width, GLint height, int border, PixelFormat format, PixelType type, IntPtr data)
		{
			glTexImage2D(target, level, (int)internalFormat, width, height, border, format, type, data);
			CheckError();
		}

		[OpenGL]
		[DesktopES]
		private static _glGetTexImage glGetTexImage;
		private delegate void _glGetTexImage(TextureTarget target, int level, PixelFormat format, PixelType type, IntPtr data);
		public static void GetTexImage(TextureTarget target, int level, PixelFormat format, PixelType type, IntPtr data)
		{
			if (glGetTexImage != null)
			{
				glGetTexImage(target, level, format, type, data);
				CheckError();
			}
		}

		private delegate uint _glCreateShader(ShaderType type);
		private static _glCreateShader glCreateShader;
		public static uint CreateShader(ShaderType type)
		{
			var shader = glCreateShader(type);
			CheckError();
			return shader;
		}

		private delegate void _glDeleteShader(uint shader);
		private static _glDeleteShader glDeleteShader;
		public static void DeleteShader(uint shader)
		{
			glDeleteShader(shader);
			CheckError();
		}

		private delegate void _glAttachShader(uint program, uint shader);
		private static _glAttachShader glAttachShader;
		public static void AttachShader(uint program, uint shader)
		{
			glAttachShader(program, shader);
			CheckError();
		}

		private delegate void _glDetachShader(uint program, uint shader);
		private static _glDetachShader glDetachShader;
		public static void DetachShader(uint program, uint shader)
		{
			glDetachShader(program, shader);
			CheckError();
		}

		private delegate void _glShaderSource(uint shader, GLint count, string[] source, int[] length);
		private static _glShaderSource glShaderSource;
		public static void ShaderSource(uint shader, string source)
		{
			var sourceArr = new string[] { source };
			var lengthArr = new int[] { source.Length };
			glShaderSource(shader, 1, sourceArr, lengthArr);
			CheckError();
		}

		private delegate void _glCompileShader(uint shader);
		private static _glCompileShader glCompileShader;
		public static void CompileShader(uint shader)
		{
			glCompileShader(shader);
			CheckError();
		}

		private delegate void _glGetShaderiv(uint shader, ShaderParam pname, out int result);
		private static _glGetShaderiv glGetShaderiv;
		public static void GetShader(uint shader, ShaderParam pname, out int result)
		{
			glGetShaderiv(shader, pname, out result);
			CheckError();
		}

		private delegate void _glGetShaderInfoLog(uint shader, GLint maxLength, out GLint length, byte[] infoLog);
		private static _glGetShaderInfoLog glGetShaderInfoLog;
		public static string GetShaderInfoLog(uint shader)
		{
			GetShader(shader, ShaderParam.InfoLogLength, out int len);
			var bytes = new byte[len];
			glGetShaderInfoLog(shader, len, out len, bytes);
			CheckError();
			return Encoding.UTF8.GetString(bytes);
		}

		private delegate uint _glCreateProgram();
		private static _glCreateProgram glCreateProgram;
		public static uint CreateProgram()
		{
			var program = glCreateProgram();
			CheckError();
			return program;
		}

		private delegate void _glDeleteProgram(uint program);
		private static _glDeleteProgram glDeleteProgram;
		public static void DeleteProgram(uint program)
		{
			glDeleteProgram(program);
			CheckError();
		}

		private delegate void _glLinkProgram(uint program);
		private static _glLinkProgram glLinkProgram;
		public static void LinkProgram(uint program)
		{
			glLinkProgram(program);
			CheckError();
		}

		private delegate void _glValidateProgram(uint program);
		private static _glValidateProgram glValidateProgram;
		public static void ValidateProgram(uint program)
		{
			glValidateProgram(program);
			CheckError();
		}

		private delegate void _glGetProgramiv(uint program, ProgramParam pname, out int result);
		private static _glGetProgramiv glGetProgramiv;
		public static void GetProgram(uint program, ProgramParam pname, out int result)
		{
			glGetProgramiv(program, pname, out result);
			CheckError();
		}

		private delegate void _glGetProgramInfoLog(uint program, GLint maxLength, out GLint length, byte[] infoLog);
		private static _glGetProgramInfoLog glGetProgramInfoLog;
		public static string GetProgramInfoLog(uint program)
		{
			GetProgram(program, ProgramParam.InfoLogLength, out int len);
			var bytes = new byte[len];
			glGetProgramInfoLog(program, len, out len, bytes);
			CheckError();
			return Encoding.UTF8.GetString(bytes);
		}

		private static byte[] uniformName = new byte[32];
		private static UniformType[] validUniformTypes;

		private unsafe delegate void _glGetActiveUniform(uint program, uint index, GLint bufSize, out GLint length, out int size, out UniformType type, byte* name);
		private static _glGetActiveUniform glGetActiveUniform;
		public unsafe static void GetActiveUniform(uint program, uint index, out int size, out UniformType type, out string name)
		{
			if (validUniformTypes == null)
				validUniformTypes = (UniformType[])Enum.GetValues(typeof(UniformType));
			fixed (byte* ptr = uniformName)
			{
				glGetActiveUniform(program, index, uniformName.Length, out GLint length, out size, out type, ptr);
				name = length > 0 ? Encoding.UTF8.GetString(ptr, length) : null;
				if (!validUniformTypes.Contains(type))
					size = 0;
			}
			CheckError();
		}

		private delegate void _glUseProgram(uint program);
		private static _glUseProgram glUseProgram;
		public static void UseProgram(uint program)
		{
			glUseProgram(program);
			CheckError();
		}

		private delegate void _glGetAttribLocation(uint program, string name);
		private static _glGetAttribLocation glGetAttribLocation;
		public static void GetAttribLocation(uint program, string name)
		{
			glGetAttribLocation(program, name);
			CheckError();
		}

		private delegate void _glBindAttribLocation(uint program, uint index, string name);
		private static _glBindAttribLocation glBindAttribLocation;
		public static void BindAttribLocation(uint program, uint index, string name)
		{
			glBindAttribLocation(program, index, name);
			CheckError();
		}

		private delegate int _glGetUniformLocation(uint program, string name);
		private static _glGetUniformLocation glGetUniformLocation;
		public static int GetUniformLocation(uint program, string name)
		{
			var loc = glGetUniformLocation(program, name);
			CheckError();
			return loc;
		}

		private delegate void _glVertexAttribPointer(uint index, int size, VertexType type, bool normalized, GLint stride, IntPtr pointer);
		private static _glVertexAttribPointer glVertexAttribPointer;
		public static void VertexAttribPointer(uint index, int size, VertexType type, bool normalized, GLint stride, IntPtr pointer)
		{
			glVertexAttribPointer(index, size, type, normalized, stride, pointer);
			CheckError();
		}
		public static void VertexAttribPointer(uint index, int size, VertexType type, bool normalized, GLint stride, int pointer)
		{
			VertexAttribPointer(index, size, type, normalized, stride, new System.IntPtr(pointer));
		}

		private delegate void _glEnableVertexAttribArray(uint index);
		private static _glEnableVertexAttribArray glEnableVertexAttribArray;
		public static void EnableVertexAttribArray(uint index)
		{
			glEnableVertexAttribArray(index);
			CheckError();
		}

		private delegate void _glDisableVertexAttribArray(uint index);
		private static _glDisableVertexAttribArray glDisableVertexAttribArray;
		public static void DisableVertexAttribArray(uint index)
		{
			glDisableVertexAttribArray(index);
			CheckError();
		}

		private unsafe delegate void _glGenBuffers(GLint n, uint* buffers);
		private static _glGenBuffers glGenBuffers;
		public unsafe static void GenBuffers(GLint n, uint[] buffers)
		{
			fixed (uint* ptr = buffers) { glGenBuffers(n, ptr); }
			CheckError();
		}
		public static void GenBuffers(uint[] buffers)
		{
			GenBuffers(buffers.Length, buffers);
		}
		public unsafe static uint GenBuffer()
		{
			uint buffer = 0;
			glGenBuffers(1, &buffer);
			CheckError();
			return buffer;
		}

		private unsafe delegate void _glDeleteBuffers(GLint n, uint* buffers);
		private static _glDeleteBuffers glDeleteBuffers;
		public unsafe static void DeleteBuffers(GLint n, uint[] buffers)
		{
			fixed (uint* ptr = buffers) { glDeleteBuffers(n, ptr); }
			CheckError();
		}
		public static void DeleteBuffers(uint[] buffers)
		{
			DeleteBuffers(buffers.Length, buffers);
		}
		public unsafe static void DeleteBuffer(uint buffer)
		{
			glDeleteBuffers(1, &buffer);
			CheckError();
		}

		private delegate void _glBindBuffer(BufferTarget target, uint buffer);
		private static _glBindBuffer glBindBuffer;
		public static void BindBuffer(BufferTarget target, uint buffer)
		{
			glBindBuffer(target, buffer);
			CheckError();
		}

		private unsafe delegate void _glGenVertexArrays(GLint n, uint* arrays);
		private static _glGenVertexArrays glGenVertexArrays;
		public unsafe static void GenVertexArrays(GLint n, uint[] arrays)
		{
			fixed (uint* ptr = arrays) { glGenVertexArrays(n, ptr); }
			CheckError();
		}
		public unsafe static uint GenVertexArray()
		{
			uint arr = 0;
			glGenVertexArrays(1, &arr);
			CheckError();
			return arr;
		}

		private unsafe delegate void _glDeleteVertexArrays(GLint n, uint* arrays);
		private static _glDeleteVertexArrays glDeleteVertexArrays;
		public unsafe static void DeleteVertexArrays(GLint n, uint[] arrays)
		{
			fixed (uint* ptr = arrays) { glDeleteVertexArrays(n, ptr); }
			CheckError();
		}
		public unsafe static void DeleteVertexArray(uint array)
		{
			glDeleteVertexArrays(1, &array);
			CheckError();
		}

		private delegate void _glBindVertexArray(uint array);
		private static _glBindVertexArray glBindVertexArray;
		public static void BindVertexArray(uint array)
		{
			glBindVertexArray(array);
			CheckError();
		}

		private delegate void _glBufferData(BufferTarget target, IntPtr size, IntPtr data, BufferUsage usage);
		private static _glBufferData glBufferData;
		public static void BufferData(BufferTarget target, int size, IntPtr data, BufferUsage usage)
		{
			glBufferData(target, new IntPtr(size), data, usage);
			CheckError();
		}
		public static void BufferData<T>(BufferTarget target, int size, T[] data, BufferUsage usage) where T : struct
		{
			var dataPtr = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);
			BufferData(target, size, dataPtr, usage);
		}

		private delegate void _glBufferSubData(BufferTarget target, IntPtr offset, IntPtr size, IntPtr data);
		private static _glBufferSubData glBufferSubData;
		public static void BufferSubData(BufferTarget target, int offset, int size, IntPtr data)
		{
			glBufferSubData(target, new IntPtr(offset), new IntPtr(size), data);
			CheckError();
		}

		private unsafe delegate void _glGenFramebuffers(GLint n, uint* framebuffers);
		private static _glGenFramebuffers glGenFramebuffers;
		public unsafe static void GenFramebuffers(GLint n, uint[] framebuffers)
		{
			fixed (uint* ptr = framebuffers) { glGenFramebuffers(n, ptr); }
			CheckError();
		}
		public unsafe static uint GenFramebuffer()
		{
			uint fbo = 0;
			glGenFramebuffers(1, &fbo);
			CheckError();
			return fbo;
		}

		private unsafe delegate void _glDeleteFramebuffers(GLint n, uint* framebuffers);
		private static _glDeleteFramebuffers glDeleteFramebuffers;
		public unsafe static void DeleteFramebuffers(GLint n, uint[] framebuffers)
		{
			fixed (uint* ptr = framebuffers) { glDeleteFramebuffers(n, ptr); }
			CheckError();
		}
		public unsafe static void DeleteFramebuffer(uint framebuffer)
		{
			glDeleteFramebuffers(1, &framebuffer);
			CheckError();
		}

		private delegate void _glBindFramebuffer(FramebufferTarget target, uint framebuffer);
		private static _glBindFramebuffer glBindFramebuffer;
		public static void BindFramebuffer(FramebufferTarget target, uint framebuffer)
		{
			glBindFramebuffer(target, framebuffer);
			CheckError();
		}

		private delegate void _glFramebufferTexture2D(FramebufferTarget target, TextureAttachment attachment, TextureTarget textarget, uint texture, int level);
		private static _glFramebufferTexture2D glFramebufferTexture2D;
		public static void FramebufferTexture2D(FramebufferTarget target, TextureAttachment attachment, TextureTarget textarget, uint texture, int level)
		{
			if (attachment != TextureAttachment.Depth)
			{
				uint texn = (uint)attachment - (uint)TextureAttachment.Colour0;
				if (texn >= MaxColourAttachments)
					throw new Exception("Exceeding max colour attachments: " + MaxColourAttachments);
			}
			glFramebufferTexture2D(target, attachment, textarget, texture, level);
			CheckError();
		}

		private unsafe delegate void _glDrawBuffers(GLint n, DrawBuffer* bufs);
		private static _glDrawBuffers glDrawBuffers;
		public unsafe static void DrawBuffers(GLint n, DrawBuffer[] bufs)
		{
			if (n > bufs.Length)
				throw new Exception("Not enough buffers in array.");
			if (n > MaxDrawBuffers)
				throw new Exception("Exceeded maximum number of draw buffers: " + MaxDrawBuffers);

			fixed (DrawBuffer* ptr = bufs) { glDrawBuffers(n, ptr); }
			CheckError();
		}

		private delegate void _glReadBuffer(ReadBuffer buffer);
		private static _glReadBuffer glReadBuffer;
		public static void ReadBuffer(ReadBuffer buffer)
		{
			glReadBuffer(buffer);
			CheckError();
		}

		private unsafe delegate void _glReadPixels(int x, int y, GLint w, GLint h, PixelFormat format, PixelType type, byte* data);
		private static _glReadPixels glReadPixels;
		public unsafe static void ReadPixels(RectangleI rect, byte[] data)
		{
			if (data.Length < rect.Area)
				throw new Exception("Data array is not large enough.");
			fixed (byte* ptr = data) { glReadPixels(rect.X, rect.Y, rect.W, rect.H, PixelFormat.RGBA, PixelType.UnsignedByte, ptr); }
			CheckError();
		}

		private unsafe delegate void _glGenRenderbuffers(GLint n, uint* renderbuffers);
		private static _glGenRenderbuffers glGenRenderbuffers;
		public unsafe static void GenRenderbuffers(GLint n, uint[] renderbuffers)
		{
			fixed (uint* ptr = renderbuffers) { glGenFramebuffers(n, ptr); }
			CheckError();
		}
		public unsafe static uint GenRenderbuffer()
		{
			uint rbo = 0;
			glGenRenderbuffers(1, &rbo);
			CheckError();
			return rbo;
		}

		private const GLuint GL_RENDERBUFFER = 0x8D41;

		private delegate void _glBindRenderbuffer(GLuint target, uint buffer);
		private static _glBindRenderbuffer glBindRenderbuffer;
		public static void BindRenderbuffer(uint buffer)
		{
			glBindRenderbuffer(GL_RENDERBUFFER, buffer);
			CheckError();
		}

		private delegate void _glRenderbufferStorage(GLuint target, TextureFormat format, int width, int height);
		private static _glRenderbufferStorage glRenderbufferStorage;
		public static void RenderbufferStorage(TextureFormat format, int width, int height)
		{
			glRenderbufferStorage(GL_RENDERBUFFER, format, width, height);
			CheckError();
		}

		private delegate void _glFramebufferRenderbuffer(FramebufferTarget target, TextureAttachment attachment, GLuint renderbufferTarget, uint renderbuffer);
		private static _glFramebufferRenderbuffer glFramebufferRenderbuffer;
		public static void FramebufferRenderbuffer(FramebufferTarget target, TextureAttachment attachment, uint renderbuffer)
		{
			glFramebufferRenderbuffer(target, attachment, GL_RENDERBUFFER, renderbuffer);
			CheckError();
		}

		private delegate void _glDrawArrays(DrawMode mode, GLint start, GLint count);
		private static _glDrawArrays glDrawArrays;
		public static void DrawArrays(DrawMode mode, GLint start, GLint count)
		{
			glDrawArrays(mode, start, count);
			CheckError();
		}

		private delegate void _glDrawElements(DrawMode mode, GLint count, IndexType type, IntPtr offset);
		private static _glDrawElements glDrawElements;
		public static void DrawElements(DrawMode mode, GLint count, IndexType type, IntPtr offset)
		{
			glDrawElements(mode, count, type, offset);
			CheckError();
		}
		public static void DrawElements(DrawMode mode, GLint count, IndexType type, GLint offset)
		{
			DrawElements(mode, count, type, new IntPtr(offset));
		}

		private delegate void _glBlitFramebuffer(int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, BufferBit mask, BlitFilter filter);
		private static _glBlitFramebuffer glBlitFramebuffer;
		public static void BlitFramebuffer(RectangleI src, RectangleI dst, BufferBit mask, BlitFilter filter)
		{
			glBlitFramebuffer(src.X, src.Y, src.MaxX, src.MaxY, dst.X, dst.Y, dst.MaxX, dst.MaxY, mask, filter);
			CheckError();
		}

		private delegate FramebufferStatus _glCheckFramebufferStatus(FramebufferTarget target);
		private static _glCheckFramebufferStatus glCheckFramebufferStatus;
		public static FramebufferStatus CheckFramebufferStatus(FramebufferTarget target)
		{
			var status = glCheckFramebufferStatus(target);
			CheckError();
			return status;
		}

		private delegate void _glUniform1f(int location, float v0);
		private delegate void _glUniform2f(int location, float v0, float v1);
		private delegate void _glUniform3f(int location, float v0, float v1, float v2);
		private delegate void _glUniform4f(int location, float v0, float v1, float v2, float v3);
		private delegate void _glUniform1fv(int location, GLint count, float[] value);
		private delegate void _glUniform2fv(int location, GLint count, float[] value);
		private delegate void _glUniform3fv(int location, GLint count, float[] value);
		private delegate void _glUniform4fv(int location, GLint count, float[] value);
		private static _glUniform1f glUniform1f;
		private static _glUniform2f glUniform2f;
		private static _glUniform3f glUniform3f;
		private static _glUniform4f glUniform4f;
		private static _glUniform1fv glUniform1fv;
		private static _glUniform2fv glUniform2fv;
		private static _glUniform3fv glUniform3fv;
		private static _glUniform4fv glUniform4fv;
		public static void Uniform1F(int location, float v0)
		{
			glUniform1f(location, v0);
			CheckError();
		}
		public static void Uniform2F(int location, float v0, float v1)
		{
			glUniform2f(location, v0, v1);
			CheckError();
		}
		public static void Uniform3F(int location, float v0, float v1, float v2)
		{
			glUniform3f(location, v0, v1, v2);
			CheckError();
		}
		public static void Uniform4F(int location, float v0, float v1, float v2, float v3)
		{
			glUniform4f(location, v0, v1, v2, v3);
			CheckError();
		}
		public static void Uniform1FV(int location, GLint count, float[] value)
		{
			glUniform1fv(location, count, value);
			CheckError();
		}
		public static void Uniform2FV(int location, GLint count, float[] value)
		{
			glUniform2fv(location, count, value);
			CheckError();
		}
		public static void Uniform3FV(int location, GLint count, float[] value)
		{
			glUniform3fv(location, count, value);
			CheckError();
		}
		public static void Uniform4FV(int location, GLint count, float[] value)
		{
			glUniform4fv(location, count, value);
			CheckError();
		}

		private delegate void _glUniform1i(int location, int v0);
		private delegate void _glUniform2i(int location, int v0, int v1);
		private delegate void _glUniform3i(int location, int v0, int v1, int v2);
		private delegate void _glUniform4i(int location, int v0, int v1, int v2, int v3);
		private delegate void _glUniform1iv(int location, GLint count, int[] value);
		private delegate void _glUniform2iv(int location, GLint count, int[] value);
		private delegate void _glUniform3iv(int location, GLint count, int[] value);
		private delegate void _glUniform4iv(int location, GLint count, int[] value);
		private static _glUniform1i glUniform1i;
		private static _glUniform2i glUniform2i;
		private static _glUniform3i glUniform3i;
		private static _glUniform4i glUniform4i;
		private static _glUniform1iv glUniform1iv;
		private static _glUniform2iv glUniform2iv;
		private static _glUniform3iv glUniform3iv;
		private static _glUniform4iv glUniform4iv;
		public static void Uniform1I(int location, int v0)
		{
			glUniform1i(location, v0);
			CheckError();
		}
		public static void Uniform2I(int location, int v0, int v1)
		{
			glUniform2i(location, v0, v1);
			CheckError();
		}
		public static void Uniform3I(int location, int v0, int v1, int v2)
		{
			glUniform3i(location, v0, v1, v2);
			CheckError();
		}
		public static void Uniform4I(int location, int v0, int v1, int v2, int v3)
		{
			glUniform4i(location, v0, v1, v2, v3);
			CheckError();
		}
		public static void Uniform1IV(int location, GLint count, int[] value)
		{
			glUniform1iv(location, count, value);
			CheckError();
		}
		public static void Uniform2IV(int location, GLint count, int[] value)
		{
			glUniform2iv(location, count, value);
			CheckError();
		}
		public static void Uniform3IV(int location, GLint count, int[] value)
		{
			glUniform3iv(location, count, value);
			CheckError();
		}
		public static void Uniform4IV(int location, GLint count, int[] value)
		{
			glUniform4iv(location, count, value);
			CheckError();
		}

		private delegate void _glUniform1ui(int location, uint v0);
		private delegate void _glUniform2ui(int location, uint v0, uint v1);
		private delegate void _glUniform3ui(int location, uint v0, uint v1, uint v2);
		private delegate void _glUniform4ui(int location, uint v0, uint v1, uint v2, uint v3);
		private delegate void _glUniform1uiv(int location, GLint count, uint[] value);
		private delegate void _glUniform2uiv(int location, GLint count, uint[] value);
		private delegate void _glUniform3uiv(int location, GLint count, uint[] value);
		private delegate void _glUniform4uiv(int location, GLint count, uint[] value);
		private static _glUniform1ui glUniform1ui;
		private static _glUniform2ui glUniform2ui;
		private static _glUniform3ui glUniform3ui;
		private static _glUniform4ui glUniform4ui;
		private static _glUniform1uiv glUniform1uiv;
		private static _glUniform2uiv glUniform2uiv;
		private static _glUniform3uiv glUniform3uiv;
		private static _glUniform4uiv glUniform4uiv;
		public static void Uniform1UI(int location, uint v0)
		{
			glUniform1ui(location, v0);
			CheckError();
		}
		public static void Uniform2UI(int location, uint v0, uint v1)
		{
			glUniform2ui(location, v0, v1);
			CheckError();
		}
		public static void Uniform3UI(int location, uint v0, uint v1, uint v2)
		{
			glUniform3ui(location, v0, v1, v2);
			CheckError();
		}
		public static void Uniform4UI(int location, uint v0, uint v1, uint v2, uint v3)
		{
			glUniform4ui(location, v0, v1, v2, v3);
			CheckError();
		}
		public static void Uniform1UIV(int location, GLint count, uint[] value)
		{
			glUniform1uiv(location, count, value);
			CheckError();
		}
		public static void Uniform2UIV(int location, GLint count, uint[] value)
		{
			glUniform2uiv(location, count, value);
			CheckError();
		}
		public static void Uniform3UIV(int location, GLint count, uint[] value)
		{
			glUniform3uiv(location, count, value);
			CheckError();
		}
		public static void Uniform4UIV(int location, GLint count, uint[] value)
		{
			glUniform4uiv(location, count, value);
			CheckError();
		}

		private delegate void _glUniformMatrix2fv(int location, GLint count, bool transpose, float[] value);
		private static _glUniformMatrix2fv glUniformMatrix2fv;
		public static void UniformMatrix2FV(int location, GLint count, bool transpose, float[] value)
		{
			glUniformMatrix2fv(location, count, transpose, value);
			CheckError();
		}

		private delegate void _glUniformMatrix3fv(int location, GLint count, bool transpose, float[] value);
		private static _glUniformMatrix3fv glUniformMatrix3fv;
		public static void UniformMatrix3FV(int location, GLint count, bool transpose, float[] value)
		{
			glUniformMatrix3fv(location, count, transpose, value);
			CheckError();
		}

		private unsafe delegate void _glUniformMatrix4fv(int location, GLint count, bool transpose, float* value);
		private static _glUniformMatrix4fv glUniformMatrix4fv;
		public unsafe static void UniformMatrix4FV(int location, GLint count, bool transpose, float* value)
		{
			glUniformMatrix4fv(location, count, transpose, value);
			CheckError();
		}
		public unsafe static void UniformMatrix4FV(int location, GLint count, bool transpose, float[] value)
		{
			fixed (float* ptr = value) { glUniformMatrix4fv(location, count, transpose, ptr); }
			CheckError();
		}

		private delegate void _glUniformMatrix2x3fv(int location, GLint count, bool transpose, float[] value);
		private static _glUniformMatrix2x3fv glUniformMatrix2x3fv;
		public static void UniformMatrix2x3FV(int location, GLint count, bool transpose, float[] value)
		{
			glUniformMatrix2x3fv(location, count, transpose, value);
			CheckError();
		}

		private unsafe delegate void _glUniformMatrix3x2fv(int location, GLint count, bool transpose, float* value);
		private static _glUniformMatrix3x2fv glUniformMatrix3x2fv;
		public unsafe static void UniformMatrix3x2FV(int location, GLint count, bool transpose, float* value)
		{
			glUniformMatrix3x2fv(location, count, transpose, value);
			CheckError();
		}
		public unsafe static void UniformMatrix3x2FV(int location, GLint count, bool transpose, float[] value)
		{
			fixed (float* ptr = value) { glUniformMatrix3x2fv(location, count, transpose, ptr); }
			CheckError();
		}

		private delegate void _glUniformMatrix2x4fv(int location, GLint count, bool transpose, float[] value);
		private static _glUniformMatrix2x4fv glUniformMatrix2x4fv;
		public static void UniformMatrix2x4FV(int location, GLint count, bool transpose, float[] value)
		{
			glUniformMatrix2x4fv(location, count, transpose, value);
			CheckError();
		}

		private delegate void _glUniformMatrix4x2fv(int location, GLint count, bool transpose, float[] value);
		private static _glUniformMatrix4x2fv glUniformMatrix4x2fv;
		public static void UniformMatrix4x2FV(int location, GLint count, bool transpose, float[] value)
		{
			glUniformMatrix4x2fv(location, count, transpose, value);
			CheckError();
		}

		private delegate void _glUniformMatrix3x4fv(int location, GLint count, bool transpose, float[] value);
		private static _glUniformMatrix3x4fv glUniformMatrix3x4fv;
		public static void UniformMatrix3x4FV(int location, GLint count, bool transpose, float[] value)
		{
			glUniformMatrix3x4fv(location, count, transpose, value);
			CheckError();
		}

		private delegate void _glUniformMatrix4x3fv(int location, GLint count, bool transpose, float[] value);
		private static _glUniformMatrix4x3fv glUniformMatrix4x3fv;
		public static void UniformMatrix4x3FV(int location, GLint count, bool transpose, float[] value)
		{
			glUniformMatrix4x3fv(location, count, transpose, value);
			CheckError();
		}

#pragma warning restore IDE0044
#pragma warning restore CS0649
	}
}