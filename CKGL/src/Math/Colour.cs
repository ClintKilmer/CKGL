using System;
using System.Runtime.InteropServices;

namespace CKGL
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct Colour
	{
		private byte r;
		private byte g;
		private byte b;
		private byte a;

		#region Properties
		public float R { get { return r / 255f; } set { r = (byte)(value * 255); } }
		public float G { get { return g / 255f; } set { g = (byte)(value * 255); } }
		public float B { get { return b / 255f; } set { b = (byte)(value * 255); } }
		public float A { get { return a / 255f; } set { a = (byte)(value * 255); } } 
		#endregion

		#region Constructors
		public Colour(byte r, byte g, byte b, byte a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}
		public Colour(UInt32 val)
		{
			r = (byte)((val >> 24) & 0xff);
			g = (byte)((val >> 16) & 0xff);
			b = (byte)((val >> 8) & 0xff);
			a = (byte)(val & 0xff);
		}
		#endregion

		#region Static Constructors
		public static readonly Colour Transparent = 0;
		public static readonly Colour Black = 0x000000FF;
		public static readonly Colour White = 0xFFFFFFFF;
		public static readonly Colour Red = 0xFF0000FF;
		public static readonly Colour Green = 0x00FF00FF;
		public static readonly Colour Blue = 0x0000FFFF;
		public static readonly Colour Yellow = 0xFFFF00FF;
		public static readonly Colour Cyan = 0x00FFFFFF;
		public static readonly Colour Magenta = 0xFF00FFFF;
		public static readonly Colour Grey = 0x808080FF;

		#region XNA Colours
		public static class XNA
		{
			//public Colour XNAColour(UInt32 val)
			//{
			//	r = (byte)((val >> 24) & 0xff);
			//	g = (byte)((val >> 16) & 0xff);
			//	b = (byte)((val >> 8) & 0xff);
			//	a = (byte)(val & 0xff);
			//}

			public static Colour Transparent = new Colour(0);
			public static Colour AliceBlue = new Colour(0xfffff8f0);
			public static Colour AntiqueWhite = new Colour(0xffd7ebfa);
			public static Colour Aqua = new Colour(0xffffff00);
			public static Colour Aquamarine = new Colour(0xffd4ff7f);
			public static Colour Azure = new Colour(0xfffffff0);
			public static Colour Beige = new Colour(0xffdcf5f5);
			public static Colour Bisque = new Colour(0xffc4e4ff);
			public static Colour Black = new Colour(0xff000000);
			public static Colour BlanchedAlmond = new Colour(0xffcdebff);
			public static Colour Blue = new Colour(0xffff0000);
			public static Colour BlueViolet = new Colour(0xffe22b8a);
			public static Colour Brown = new Colour(0xff2a2aa5);
			public static Colour BurlyWood = new Colour(0xff87b8de);
			public static Colour CadetBlue = new Colour(0xffa09e5f);
			public static Colour Chartreuse = new Colour(0xff00ff7f);
			public static Colour Chocolate = new Colour(0xff1e69d2);
			public static Colour Coral = new Colour(0xff507fff);
			public static Colour CornflowerBlue = new Colour(0xffed9564);
			public static Colour Cornsilk = new Colour(0xffdcf8ff);
			public static Colour Crimson = new Colour(0xff3c14dc);
			public static Colour Cyan = new Colour(0xffffff00);
			public static Colour DarkBlue = new Colour(0xff8b0000);
			public static Colour DarkCyan = new Colour(0xff8b8b00);
			public static Colour DarkGoldenrod = new Colour(0xff0b86b8);
			public static Colour DarkGrey = new Colour(0xffa9a9a9);
			public static Colour DarkGreen = new Colour(0xff006400);
			public static Colour DarkKhaki = new Colour(0xff6bb7bd);
			public static Colour DarkMagenta = new Colour(0xff8b008b);
			public static Colour DarkOliveGreen = new Colour(0xff2f6b55);
			public static Colour DarkOrange = new Colour(0xff008cff);
			public static Colour DarkOrchid = new Colour(0xffcc3299);
			public static Colour DarkRed = new Colour(0xff00008b);
			public static Colour DarkSalmon = new Colour(0xff7a96e9);
			public static Colour DarkSeaGreen = new Colour(0xff8bbc8f);
			public static Colour DarkSlateBlue = new Colour(0xff8b3d48);
			public static Colour DarkSlateGrey = new Colour(0xff4f4f2f);
			public static Colour DarkTurquoise = new Colour(0xffd1ce00);
			public static Colour DarkViolet = new Colour(0xffd30094);
			public static Colour DeepPink = new Colour(0xff9314ff);
			public static Colour DeepSkyBlue = new Colour(0xffffbf00);
			public static Colour DimGrey = new Colour(0xff696969);
			public static Colour DodgerBlue = new Colour(0xffff901e);
			public static Colour Firebrick = new Colour(0xff2222b2);
			public static Colour FloralWhite = new Colour(0xfff0faff);
			public static Colour ForestGreen = new Colour(0xff228b22);
			public static Colour Fuchsia = new Colour(0xffff00ff);
			public static Colour Gainsboro = new Colour(0xffdcdcdc);
			public static Colour GhostWhite = new Colour(0xfffff8f8);
			public static Colour Gold = new Colour(0xff00d7ff);
			public static Colour Goldenrod = new Colour(0xff20a5da);
			public static Colour Grey = new Colour(0xff808080);
			public static Colour Green = new Colour(0xff008000);
			public static Colour GreenYellow = new Colour(0xff2fffad);
			public static Colour Honeydew = new Colour(0xfff0fff0);
			public static Colour HotPink = new Colour(0xffb469ff);
			public static Colour IndianRed = new Colour(0xff5c5ccd);
			public static Colour Indigo = new Colour(0xff82004b);
			public static Colour Ivory = new Colour(0xfff0ffff);
			public static Colour Khaki = new Colour(0xff8ce6f0);
			public static Colour Lavender = new Colour(0xfffae6e6);
			public static Colour LavenderBlush = new Colour(0xfff5f0ff);
			public static Colour LawnGreen = new Colour(0xff00fc7c);
			public static Colour LemonChiffon = new Colour(0xffcdfaff);
			public static Colour LightBlue = new Colour(0xffe6d8ad);
			public static Colour LightCoral = new Colour(0xff8080f0);
			public static Colour LightCyan = new Colour(0xffffffe0);
			public static Colour LightGoldenrodYellow = new Colour(0xffd2fafa);
			public static Colour LightGrey = new Colour(0xffd3d3d3);
			public static Colour LightGreen = new Colour(0xff90ee90);
			public static Colour LightPink = new Colour(0xffc1b6ff);
			public static Colour LightSalmon = new Colour(0xff7aa0ff);
			public static Colour LightSeaGreen = new Colour(0xffaab220);
			public static Colour LightSkyBlue = new Colour(0xffface87);
			public static Colour LightSlateGrey = new Colour(0xff998877);
			public static Colour LightSteelBlue = new Colour(0xffdec4b0);
			public static Colour LightYellow = new Colour(0xffe0ffff);
			public static Colour Lime = new Colour(0xff00ff00);
			public static Colour LimeGreen = new Colour(0xff32cd32);
			public static Colour Linen = new Colour(0xffe6f0fa);
			public static Colour Magenta = new Colour(0xffff00ff);
			public static Colour Maroon = new Colour(0xff000080);
			public static Colour MediumAquamarine = new Colour(0xffaacd66);
			public static Colour MediumBlue = new Colour(0xffcd0000);
			public static Colour MediumOrchid = new Colour(0xffd355ba);
			public static Colour MediumPurple = new Colour(0xffdb7093);
			public static Colour MediumSeaGreen = new Colour(0xff71b33c);
			public static Colour MediumSlateBlue = new Colour(0xffee687b);
			public static Colour MediumSpringGreen = new Colour(0xff9afa00);
			public static Colour MediumTurquoise = new Colour(0xffccd148);
			public static Colour MediumVioletRed = new Colour(0xff8515c7);
			public static Colour MidnightBlue = new Colour(0xff701919);
			public static Colour MintCream = new Colour(0xfffafff5);
			public static Colour MistyRose = new Colour(0xffe1e4ff);
			public static Colour Moccasin = new Colour(0xffb5e4ff);
			public static Colour NavajoWhite = new Colour(0xffaddeff);
			public static Colour Navy = new Colour(0xff800000);
			public static Colour OldLace = new Colour(0xffe6f5fd);
			public static Colour Olive = new Colour(0xff008080);
			public static Colour OliveDrab = new Colour(0xff238e6b);
			public static Colour Orange = new Colour(0xff00a5ff);
			public static Colour OrangeRed = new Colour(0xff0045ff);
			public static Colour Orchid = new Colour(0xffd670da);
			public static Colour PaleGoldenrod = new Colour(0xffaae8ee);
			public static Colour PaleGreen = new Colour(0xff98fb98);
			public static Colour PaleTurquoise = new Colour(0xffeeeeaf);
			public static Colour PaleVioletRed = new Colour(0xff9370db);
			public static Colour PapayaWhip = new Colour(0xffd5efff);
			public static Colour PeachPuff = new Colour(0xffb9daff);
			public static Colour Peru = new Colour(0xff3f85cd);
			public static Colour Pink = new Colour(0xffcbc0ff);
			public static Colour Plum = new Colour(0xffdda0dd);
			public static Colour PowderBlue = new Colour(0xffe6e0b0);
			public static Colour Purple = new Colour(0xff800080);
			public static Colour Red = new Colour(0xff0000ff);
			public static Colour RosyBrown = new Colour(0xff8f8fbc);
			public static Colour RoyalBlue = new Colour(0xffe16941);
			public static Colour SaddleBrown = new Colour(0xff13458b);
			public static Colour Salmon = new Colour(0xff7280fa);
			public static Colour SandyBrown = new Colour(0xff60a4f4);
			public static Colour SeaGreen = new Colour(0xff578b2e);
			public static Colour SeaShell = new Colour(0xffeef5ff);
			public static Colour Sienna = new Colour(0xff2d52a0);
			public static Colour Silver = new Colour(0xffc0c0c0);
			public static Colour SkyBlue = new Colour(0xffebce87);
			public static Colour SlateBlue = new Colour(0xffcd5a6a);
			public static Colour SlateGrey = new Colour(0xff908070);
			public static Colour Snow = new Colour(0xfffafaff);
			public static Colour SpringGreen = new Colour(0xff7fff00);
			public static Colour SteelBlue = new Colour(0xffb48246);
			public static Colour Tan = new Colour(0xff8cb4d2);
			public static Colour Teal = new Colour(0xff808000);
			public static Colour Thistle = new Colour(0xffd8bfd8);
			public static Colour Tomato = new Colour(0xff4763ff);
			public static Colour Turquoise = new Colour(0xffd0e040);
			public static Colour Violet = new Colour(0xffee82ee);
			public static Colour Wheat = new Colour(0xffb3def5);
			public static Colour White = new Colour(uint.MaxValue);
			public static Colour WhiteSmoke = new Colour(0xfff5f5f5);
			public static Colour Yellow = new Colour(0xff00ffff);
			public static Colour YellowGreen = new Colour(0xff32cd9a);
		}
		#endregion
		#endregion

		#region Methods
		public Colour Alpha(float alpha)
		{
			// nonpremultiplied
			A = alpha;

			// premultiplied
			//return c * alpha;

			return this;
		}
		#endregion

		#region Overrides
		public override string ToString()
		{
			return $"{R}, {G}, {B}, {A}";
		}
		#endregion

		#region Operators
		public static bool operator ==(Colour c1, Colour c2) => c1.r == c2.r && c1.g == c2.g && c1.b == c2.b && c1.a == c2.a;

		public static bool operator !=(Colour c1, Colour c2) => c1.r != c2.r || c1.g != c2.g || c1.b != c2.b || c1.a != c2.a;

		public static Colour operator *(Colour c1, Colour c2)
		{
			c1.r *= c2.r;
			c1.g *= c2.g;
			c1.b *= c2.b;
			c1.a *= c2.a;
			return c1;
		}

		public static Colour operator *(Colour c, float n)
		{
			c.r = (byte)(c.r * n);
			c.g = (byte)(c.g * n);
			c.b = (byte)(c.b * n);
			c.a = (byte)(c.a * n);
			return c;
		}

		public static Colour operator *(float n, Colour c)
		{
			c.r = (byte)(c.r * n);
			c.g = (byte)(c.g * n);
			c.b = (byte)(c.b * n);
			c.a = (byte)(c.a * n);
			return c;
		}

		public static Colour operator /(Colour c, float n)
		{
			c.r = (byte)(c.r / n);
			c.g = (byte)(c.g / n);
			c.b = (byte)(c.b / n);
			c.a = (byte)(c.a / n);
			return c;
		}
		#endregion

		#region Implicit Conversion Operators
		//public override bool Equals(object obj)
		//{
		//	return obj is Colour && Equals((Colour)obj);
		//}
		//public bool Equals(Colour other)
		//{
		//	return r == other.r && g == other.g && b == other.b && a == other.a;
		//}

		//public override int GetHashCode()
		//{
		//	unchecked
		//	{
		//		int hash = 17;
		//		hash = hash * 23 + r;
		//		hash = hash * 23 + g;
		//		hash = hash * 23 + b;
		//		hash = hash * 23 + a;
		//		return hash;
		//	}
		//}

		public static implicit operator UInt32(Colour val)
		{
			return ((UInt32)val.r << 24) | ((UInt32)val.g << 16) | ((UInt32)val.b << 8) | val.a;
		}
		public static implicit operator Colour(UInt32 val)
		{
			return new Colour(
				(byte)((val >> 24) & 0xff),
				(byte)((val >> 16) & 0xff),
				(byte)((val >> 8) & 0xff),
				(byte)(val & 0xff)
			);
		}
		#endregion
	}
}