﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using SDL2;
using static SDL2.SDL;

namespace CKGL
{
	public static class Platform
	{
		#region Events
		public delegate void KeyEvent(KeyCode keyCode, ScanCode scanCode, bool repeat);
		public delegate void MouseMoveEvent(int x, int y);
		public delegate void MouseButtonEvent(int buttonID);
		public delegate void MouseScrollEvent(int x, int y);
		public delegate void JoyDeviceEvent(int deviceID);
		public delegate void JoyButtonEvent(int deviceID, int buttonID);
		public delegate void JoyAxisEvent(int deviceID, int axisID, float value);
		public delegate void OtherEvent(int eventID);

		public static Action OnQuit;
		public static KeyEvent OnKeyDown;
		public static KeyEvent OnKeyRepeat;
		public static KeyEvent OnKeyUp;
		public static MouseMoveEvent OnMouseMove;
		public static MouseButtonEvent OnMouseButtonDown;
		public static MouseButtonEvent OnMouseButtonUp;
		public static MouseScrollEvent OnMouseScroll;
		public static JoyDeviceEvent OnJoyDeviceAdd;
		public static JoyDeviceEvent OnJoyDeviceRemove;
		public static JoyButtonEvent OnJoyButtonDown;
		public static JoyButtonEvent OnJoyButtonUp;
		public static JoyAxisEvent OnJoyAxisMove;
		public static Action OnWinClose;
		public static Action OnWinShown;
		public static Action OnWinHidden;
		public static Action OnWinExposed;
		public static Action OnWinMoved;
		public static Action OnWinResized;
		public static Action OnWinMinimized;
		public static Action OnWinMaximized;
		public static Action OnWinRestored;
		public static Action OnWinEnter;
		public static Action OnWinLeave;
		public static Action OnWinFocusGained;
		public static Action OnWinFocusLost;
		public static OtherEvent OnWinOtherEvent;
		public static OtherEvent OnOtherEvent;
		#endregion

		public static bool Running { get; private set; } = false;

		private static SDL_Event Event;

		public static string OS
		{
			get { return SDL_GetPlatform(); }
		}

		public static uint TotalMilliseconds
		{
			get { return SDL_GetTicks(); }
		}

		public static ulong PerformanceCounter
		{
			get { return SDL_GetPerformanceCounter(); }
		}

		public static ulong PerformanceFrequency
		{
			get { return SDL_GetPerformanceFrequency(); }
		}

		public static bool ShowCursor // Default true
		{
			get { return SDL_ShowCursor(SDL_QUERY) == 1; }
			set { SDL_ShowCursor(value ? SDL_ENABLE : SDL_DISABLE); }
		}

		public static string Clipboard
		{
			get { return SDL_GetClipboardText(); }
			set { SDL_SetClipboardText(value); }
		}

		public static bool ScreensaverAllowed // Default false
		{
			get { return SDL_IsScreenSaverEnabled() == SDL_bool.SDL_TRUE; }
			set { if (value) SDL_EnableScreenSaver(); else SDL_DisableScreenSaver(); }
		}

		#region Windows - Set Dll Directory - x64 | x86 - https://github.com/FNA-XNA/FNA/wiki/4:-FNA-and-Windows-API#64-bit-support
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetDllDirectory(string lpPathName);
		private static void SetDllDirectory()
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
				SetDllDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Environment.Is64BitProcess ? "x64" : "x86"));
		}
		#endregion

		#region Init/Exit Methods
		public static void Init(string windowTitle, int windowWidth, int windowHeight, bool windowVSync, bool windowFullscreen, bool windowResizable, bool windowBorderless)
		{
			SetDllDirectory();

			#region Flibit - shims
			// Missing libs?
			try
			{
				Console.WriteLine($"Platform: {OS}");
			}
			catch (Exception e)
			{
				Console.WriteLine("SDL2 was not found! Do you have fnalibs?");
				throw e;
			}

			/* SDL2 might complain if an OS that uses SDL_main has not actually
			 * used SDL_main by the time you initialize SDL2.
			 * The only platform that is affected is Windows, but we can skip
			 * their WinMain. This was only added to prevent iOS from exploding.
			 * -flibit
			 */
			SDL_SetMainReady();

			// Also, Windows is an idiot. -flibit
			if (OS.Equals("Windows") || OS.Equals("WinRT"))
			{
				// Visual Studio is an idiot.
				if (System.Diagnostics.Debugger.IsAttached)
				{
					SDL_SetHint(
						SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING,
						"1"
					);
				}

				// TODO - Win32 WM_PAINT Interop
				/* Windows has terrible event pumping and doesn't give us
				 * WM_PAINT events correctly. So we get to do this!
				 * -flibit
				 */
				//SDL_SetEventFilter(win32OnPaint, IntPtr.Zero);
			}
			#endregion

			// If available, load the SDL_GameControllerDB
			//string mappingsDB = Path.Combine(
			//	TitleLocation.Path,
			//	"gamecontrollerdb.txt"
			//);
			//if (File.Exists(mappingsDB))
			//{
			//	SDL_GameControllerAddMappingsFromFile(
			//		mappingsDB
			//	);
			//}

			if (SDL_Init(SDL_INIT_VIDEO | SDL_INIT_TIMER | SDL_INIT_JOYSTICK | SDL_INIT_GAMECONTROLLER | SDL_INIT_HAPTIC) < 0)
			{
				Destroy();
				throw new Exception(SDL_GetError());
			}

			//// We want to initialize the controllers ASAP!
			//SDL_Event[] evt = new SDL_Event[1];
			//SDL_PumpEvents();
			//while (SDL_PeepEvents(
			//	evt,
			//	1,
			//	SDL_eventaction.SDL_GETEVENT,
			//	SDL_EventType.SDL_CONTROLLERDEVICEADDED,
			//	SDL_EventType.SDL_CONTROLLERDEVICEADDED
			//) == 1)
			//{
			//	INTERNAL_AddInstance(evt[0].cdevice.which);
			//}

			//OpenGL attributes
			SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_MAJOR_VERSION, 3);
			SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_MINOR_VERSION, 1);
			SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_PROFILE_MASK, (int)SDL_GLprofile.SDL_GL_CONTEXT_PROFILE_CORE);
			//SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_CONTEXT_FLAGS, (int)SDL_GLcontext.SDL_GL_CONTEXT_FORWARD_COMPATIBLE_FLAG);
			SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_DOUBLEBUFFER, 1);
			//SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_DEPTH_SIZE, 24);
			//SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_STENCIL_SIZE, 8);
			//SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_MULTISAMPLEBUFFERS, 1);
			//SDL_GL_SetAttribute(SDL_GLattr.SDL_GL_MULTISAMPLESAMPLES, 3);

			// Create Window
			Window.Create(windowTitle, windowWidth, windowHeight, windowVSync, windowFullscreen, windowResizable, windowBorderless);

			Running = true;
		}

		public static void Destroy()
		{
			Window.Destroy();
			SDL_Quit();
		}
		#endregion

		public static void PollEvents()
		{
			while (SDL_PollEvent(out Event) != 0)
			{
				//Console.WriteLine(Event.type.ToString());
				switch (Event.type)
				{
					case SDL_EventType.SDL_QUIT:
						OnQuit?.Invoke();
						Quit();
						break;
					case SDL_EventType.SDL_KEYDOWN:
						OnKeyDown?.Invoke((KeyCode)Event.key.keysym.sym, (ScanCode)Event.key.keysym.scancode, Event.key.repeat != 0);
						break;
					case SDL_EventType.SDL_KEYUP:
						OnKeyUp?.Invoke((KeyCode)Event.key.keysym.sym, (ScanCode)Event.key.keysym.scancode, Event.key.repeat != 0);
						break;
					case SDL_EventType.SDL_MOUSEMOTION:
						OnMouseMove?.Invoke(Event.motion.x, Event.motion.y);
						break;
					case SDL_EventType.SDL_MOUSEBUTTONDOWN:
						OnMouseButtonDown?.Invoke(Event.button.button);
						break;
					case SDL_EventType.SDL_MOUSEBUTTONUP:
						OnMouseButtonUp?.Invoke(Event.button.button);
						break;
					case SDL_EventType.SDL_MOUSEWHEEL:
						OnMouseScroll?.Invoke(Event.wheel.x, Event.wheel.y);
						break;
					//case SDL_EventType.SDL_JOYDEVICEADDED:
					//	OnJoyDeviceAdd?.Invoke(Event.JDevice.Which);
					//	break;
					//case SDL_EventType.SDL_JOYDEVICEREMOVED:
					//	OnJoyDeviceRemove?.Invoke(Event.JDevice.Which);
					//	break;
					//case SDL_EventType.SDL_JOYBUTTONDOWN:
					//	OnJoyButtonDown?.Invoke(Event.JButton.Which, Event.JButton.Button);
					//	break;
					//case SDL_EventType.SDL_JOYBUTTONUP:
					//	OnJoyButtonDown?.Invoke(Event.JButton.Which, Event.JButton.Button);
					//	break;
					//case SDL_EventType.SDL_JOYAXISMOTION:
					//	OnJoyAxisMove?.Invoke(Event.JAxis.Which, Event.JAxis.Axis, Event.JAxis.Value / (float)short.MaxValue);
					//	break;
					//case SDL_EventType.SDL_JOYHATMOTION:
					//	OnJoyAxisMove?.Invoke(Event.JAxis.Which, Event.JAxis.Axis, Event.JAxis.Value / (float)short.MaxValue);
					//	break;
					//case SDL_EventType.SDL_JOYBALLMOTION:
					//	OnJoyAxisMove?.Invoke(Event.JAxis.Which, Event.JAxis.Axis, Event.JAxis.Value / (float)short.MaxValue);
					//	break;
					case SDL_EventType.SDL_WINDOWEVENT:
						if (Event.window.windowID == Window.ID)
						{
							//Console.WriteLine(Event.window.windowEvent.ToString());
							switch (Event.window.windowEvent)
							{
								case SDL_WindowEventID.SDL_WINDOWEVENT_CLOSE:
									OnWinClose?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_SHOWN:
									OnWinShown?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_HIDDEN:
									OnWinHidden?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_EXPOSED:
									OnWinExposed?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_MOVED:
									OnWinMoved?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_RESIZED:
									OnWinResized?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_MINIMIZED:
									OnWinMinimized?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_MAXIMIZED:
									OnWinMaximized?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_RESTORED:
									OnWinRestored?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_ENTER:
									OnWinEnter?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_LEAVE:
									OnWinLeave?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED:
									OnWinFocusGained?.Invoke();
									break;
								case SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST:
									OnWinFocusLost?.Invoke();
									break;
								default:
									OnWinOtherEvent?.Invoke((int)Event.window.windowEvent);
									break;
							}
						}
						break;
				}
			}
		}

		public static void Quit()
		{
			Running = false;
		}

		public static void GetGlobalMousePosition(out int x, out int y)
		{
			SDL_GetGlobalMouseState(out x, out y);
		}

		public static IntPtr GetProcAddress(string proc)
		{
			return SDL_GL_GetProcAddress(proc);
		}

		public static void Delay(uint ms)
		{
			SDL_Delay(ms); //release the thread
		}

		#region Image I/O Methods | From FNA SDL2_FNAPlatform.cs https://github.com/FNA-XNA/FNA
		public static void TextureDataFromStream(
			Stream stream,
			out int width,
			out int height,
			out byte[] pixels,
			int reqWidth = -1,
			int reqHeight = -1,
			bool zoom = false
		)
		{
			// Load the SDL_Surface* from RWops, get the image data
			FakeRWops reader = new FakeRWops(stream);
			IntPtr surface = SDL_image.IMG_Load_RW(reader.rwops, 0);
			reader.Free();
			if (surface == IntPtr.Zero)
			{
				// File not found, supported, etc.
				Console.WriteLine($"TextureDataFromStream: {SDL_GetError()}");
				width = 0;
				height = 0;
				pixels = null;
				return;
			}
			surface = INTERNAL_convertSurfaceFormat(surface);

			// Image scaling, if applicable
			if (reqWidth != -1 && reqHeight != -1)
			{
				// Get the file surface dimensions now...
				int rw;
				int rh;
				unsafe
				{
					SDL_Surface* surPtr = (SDL_Surface*)surface;
					rw = surPtr->w;
					rh = surPtr->h;
				}

				// Calculate the image scale factor
				bool scaleWidth;
				if (zoom)
				{
					scaleWidth = rw < rh;
				}
				else
				{
					scaleWidth = rw > rh;
				}
				float scale;
				if (scaleWidth)
				{
					scale = reqWidth / (float)rw;
				}
				else
				{
					scale = reqHeight / (float)rh;
				}

				// Calculate the scaled image size, crop if zoomed
				int resultWidth;
				int resultHeight;
				SDL_Rect crop = new SDL_Rect();
				if (zoom)
				{
					resultWidth = reqWidth;
					resultHeight = reqHeight;
					if (scaleWidth)
					{
						crop.x = 0;
						crop.w = rw;
						crop.y = (int)(rh / 2 - (reqHeight / scale) / 2);
						crop.h = (int)(reqHeight / scale);
					}
					else
					{
						crop.y = 0;
						crop.h = rh;
						crop.x = (int)(rw / 2 - (reqWidth / scale) / 2);
						crop.w = (int)(reqWidth / scale);
					}
				}
				else
				{
					resultWidth = (int)(rw * scale);
					resultHeight = (int)(rh * scale);
				}

				// Alloc surface, blit!
				IntPtr newSurface = SDL_CreateRGBSurface(
					0,
					resultWidth,
					resultHeight,
					32,
					0x000000FF,
					0x0000FF00,
					0x00FF0000,
					0xFF000000
				);
				SDL_SetSurfaceBlendMode(
					surface,
					SDL_BlendMode.SDL_BLENDMODE_NONE
				);
				if (zoom)
				{
					SDL_BlitScaled(
						surface,
						ref crop,
						newSurface,
						IntPtr.Zero
					);
				}
				else
				{
					SDL_BlitScaled(
						surface,
						IntPtr.Zero,
						newSurface,
						IntPtr.Zero
					);
				}
				SDL_FreeSurface(surface);
				surface = newSurface;
			}

			// Copy surface data to output managed byte array
			unsafe
			{
				SDL_Surface* surPtr = (SDL_Surface*)surface;
				width = surPtr->w;
				height = surPtr->h;
				pixels = new byte[width * height * 4]; // MUST be SurfaceFormat.Color!
				Marshal.Copy(surPtr->pixels, pixels, 0, pixels.Length);
			}
			SDL_FreeSurface(surface);

			/* Ensure that the alpha pixels are... well, actual alpha.
			 * You think this looks stupid, but be assured: Your paint program is
			 * almost certainly even stupider.
			 * -flibit
			 */
			for (int i = 0; i < pixels.Length; i += 4)
			{
				if (pixels[i + 3] == 0)
				{
					pixels[i] = 0;
					pixels[i + 1] = 0;
					pixels[i + 2] = 0;
				}
			}
		}

		public static void SavePNG(
			Stream stream,
			int width,
			int height,
			int imgWidth,
			int imgHeight,
			byte[] data
		)
		{
			IntPtr surface = INTERNAL_getScaledSurface(
				data,
				imgWidth,
				imgHeight,
				width,
				height
			);
			FakeRWops writer = new FakeRWops(stream);
			SDL_image.IMG_SavePNG_RW(surface, writer.rwops, 0);
			writer.Free();
			SDL_FreeSurface(surface);
		}

		public static void SaveJPG(
			Stream stream,
			int width,
			int height,
			int imgWidth,
			int imgHeight,
			byte[] data
		)
		{
			// FIXME: What does XNA pick for this? -flibit
			const int quality = 100;

			IntPtr surface = INTERNAL_getScaledSurface(
				data,
				imgWidth,
				imgHeight,
				width,
				height
			);

			// FIXME: Hack for Bugzilla #3972
			IntPtr temp = SDL_ConvertSurfaceFormat(
				surface,
				SDL_PIXELFORMAT_RGB24,
				0
			);
			SDL_FreeSurface(surface);
			surface = temp;

			FakeRWops writer = new FakeRWops(stream);
			SDL_image.IMG_SaveJPG_RW(surface, writer.rwops, 0, quality);
			writer.Free();
			SDL_FreeSurface(surface);
		}

		public static IntPtr INTERNAL_getScaledSurface(
			byte[] data,
			int srcW,
			int srcH,
			int dstW,
			int dstH
		)
		{
			// Create an SDL_Surface*, write the pixel data
			IntPtr surface = SDL_CreateRGBSurface(
				0,
				srcW,
				srcH,
				32,
				0x000000FF,
				0x0000FF00,
				0x00FF0000,
				0xFF000000
			);
			SDL_LockSurface(surface);
			unsafe
			{
				SDL_Surface* surPtr = (SDL_Surface*)surface;
				Marshal.Copy(
					data,
					0,
					surPtr->pixels,
					data.Length
				);
			}
			SDL_UnlockSurface(surface);

			// Blit to a scaled surface of the size we want, if needed.
			if (srcW != dstW || srcH != dstH)
			{
				IntPtr scaledSurface = SDL_CreateRGBSurface(
					0,
					dstW,
					dstH,
					32,
					0x000000FF,
					0x0000FF00,
					0x00FF0000,
					0xFF000000
				);
				SDL_SetSurfaceBlendMode(
					surface,
					SDL_BlendMode.SDL_BLENDMODE_NONE
				);
				SDL_BlitScaled(
					surface,
					IntPtr.Zero,
					scaledSurface,
					IntPtr.Zero
				);
				SDL_FreeSurface(surface);
				surface = scaledSurface;
			}

			return surface;
		}

		private static unsafe IntPtr INTERNAL_convertSurfaceFormat(IntPtr surface)
		{
			IntPtr result = surface;
			unsafe
			{
				SDL_Surface* surPtr = (SDL_Surface*)surface;
				SDL_PixelFormat* pixelFormatPtr = (SDL_PixelFormat*)surPtr->format;

				// SurfaceFormat.Color is SDL_PIXELFORMAT_ABGR8888
				if (pixelFormatPtr->format != SDL_PIXELFORMAT_ABGR8888)
				{
					// Create a properly formatted copy, free the old surface
					result = SDL_ConvertSurfaceFormat(surface, SDL_PIXELFORMAT_ABGR8888, 0);
					SDL_FreeSurface(surface);
				}
			}
			return result;
		}

		private class FakeRWops
		{
			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			private delegate long SizeFunc(IntPtr context);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			private delegate long SeekFunc(
				IntPtr context,
				long offset,
				int whence
			);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			private delegate IntPtr ReadFunc(
				IntPtr context,
				IntPtr ptr,
				IntPtr size,
				IntPtr maxnum
			);

			[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
			private delegate IntPtr WriteFunc(
				IntPtr context,
				IntPtr ptr,
				IntPtr size,
				IntPtr num
			);

			[StructLayout(LayoutKind.Sequential)]
			private struct PartialRWops
			{
				public IntPtr size;
				public IntPtr seek;
				public IntPtr read;
				public IntPtr write;
			}

			[DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
			private static extern IntPtr SDL_AllocRW();

			[DllImport("SDL2.dll", CallingConvention = CallingConvention.Cdecl)]
			private static extern void SDL_FreeRW(IntPtr area);

			public readonly IntPtr rwops;
			private Stream stream;
			private byte[] temp;

			private SizeFunc sizeFunc;
			private SeekFunc seekFunc;
			private ReadFunc readFunc;
			private WriteFunc writeFunc;

			public FakeRWops(Stream stream)
			{
				this.stream = stream;
				rwops = SDL_AllocRW();
				temp = new byte[8192]; // Based on PNG_ZBUF_SIZE default

				sizeFunc = size;
				seekFunc = seek;
				readFunc = read;
				writeFunc = write;
				unsafe
				{
					PartialRWops* p = (PartialRWops*)rwops;
					p->size = Marshal.GetFunctionPointerForDelegate(sizeFunc);
					p->seek = Marshal.GetFunctionPointerForDelegate(seekFunc);
					p->read = Marshal.GetFunctionPointerForDelegate(readFunc);
					p->write = Marshal.GetFunctionPointerForDelegate(writeFunc);
				}
			}

			public void Free()
			{
				SDL_FreeRW(rwops);
				stream = null;
				temp = null;
			}

			private byte[] GetTemp(int len)
			{
				if (len > temp.Length)
				{
					temp = new byte[len];
				}
				return temp;
			}

			private long size(IntPtr context)
			{
				return -1;
			}

			private long seek(IntPtr context, long offset, int whence)
			{
				stream.Seek(offset, (SeekOrigin)whence);
				return stream.Position;
			}

			private IntPtr read(
				IntPtr context,
				IntPtr ptr,
				IntPtr size,
				IntPtr maxnum
			)
			{
				int len = size.ToInt32() * maxnum.ToInt32();
				len = stream.Read(
					GetTemp(len),
					0,
					len
				);
				Marshal.Copy(temp, 0, ptr, len);
				return (IntPtr)len;
			}

			private IntPtr write(
				IntPtr context,
				IntPtr ptr,
				IntPtr size,
				IntPtr num
			)
			{
				int len = size.ToInt32() * num.ToInt32();
				Marshal.Copy(
					ptr,
					GetTemp(len),
					0,
					len
				);
				stream.Write(temp, 0, len);
				return (IntPtr)len;
			}
		}
		#endregion

		// TODO - Win32 WM_PAINT Interop
		#region Private Static Win32 WM_PAINT Interop
		//private static SDL_EventFilter win32OnPaint = Win32OnPaint;
		//private static unsafe int Win32OnPaint(IntPtr func, IntPtr evtPtr)
		//{
		//	SDL_Event* evt = (SDL_Event*)evtPtr;
		//	if (evt->type == SDL_EventType.SDL_WINDOWEVENT &&
		//		evt->window.windowEvent == SDL_WindowEventID.SDL_WINDOWEVENT_EXPOSED)
		//	{
		//		if (evt->window.windowID == SDL_GetWindowID(Window.IntPtr))
		//		{
		//			game.RedrawWindow();
		//			return 0;
		//		}
		//	}
		//	return 1;
		//}
		#endregion
	}
}