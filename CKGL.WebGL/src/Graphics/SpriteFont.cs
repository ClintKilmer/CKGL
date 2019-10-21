using static Retyped.dom; // DOM / WebGL Types

namespace CKGL
{
	#region Font Alignment enums
	public enum HAlign
	{
		Left,
		Center,
		Right
	}

	public enum VAlign
	{
		Top,
		Middle,
		Bottom
	}
	#endregion

	public class SpriteFont
	{
		public SpriteSheet SpriteSheet { get; private set; }
		public float CharSpacing;
		public float SpaceWidth;
		public float LineHeight;

		private readonly Sprite[] glyphs;
		private readonly char start;
		private readonly char end;

		private bool loaded = false;
		private Sprite defaultSprite;

		public SpriteFont(SpriteSheet spriteSheet, string file, int glyphWidth, int glyphHeight, char startASCII, char endASCII, float charSpacing, float spaceWidth, float lineHeight, bool xtrim)
		{
			SpriteSheet = spriteSheet;
			CharSpacing = charSpacing;
			SpaceWidth = spaceWidth;
			LineHeight = lineHeight;

			start = startASCII;
			end = endASCII;

			glyphs = new Sprite[end + 1];

			defaultSprite = new Sprite(SpriteSheet, 0, 0, 1, 1);

			// Load image async
			HTMLImageElement image = new HTMLImageElement();
			image.crossOrigin = "";
			image.onload = (ev) => { _SpriteFont(image, glyphWidth, glyphHeight, xtrim); };
			image.src = file;
		}
		private void _SpriteFont(HTMLImageElement image, int glyphWidth, int glyphHeight, bool xtrim)
		{
			HTMLCanvasElement canvas = new HTMLCanvasElement();
			canvas.width = (uint)glyphWidth;
			canvas.height = (uint)glyphHeight;
			CanvasRenderingContext2D context = canvas.getContext(Literals._2d);

			for (int i = start; i <= end; i++)
			{
				context.clearRect(0, 0, canvas.width, canvas.height);
				context.drawImage(image, glyphWidth * (i - start), 0, glyphWidth, glyphHeight, 0, 0, glyphWidth, glyphHeight);

				// Make sure loaded image is large enough
				// If image is not large enough, generate error glyphs
				if (image.width >= glyphWidth * (i - start) + glyphWidth)
					glyphs[i] = SpriteSheet.AddSpriteFontGlyph(canvas, context, xtrim);
				else
					glyphs[i] = SpriteSheet.AddSpriteFontGlyph(canvas, context, false);
			}

			canvas.remove();

			loaded = true;
		}

		public Sprite Glyph(char glyph)
		{
			if (!loaded)
				return defaultSprite;

			if (glyph < start || glyph > end)
				return glyphs[start];

			return glyphs[glyph];
		}
	}
}

// Typical ASCII Range : 33(!)-126(~) excluding 32(space)
// < 32 = Not Used
// 32 = Space
// 33 = !
// 126 = ~
// > 126 = Not Used

//
// ASCII Table
//
//Decimal ASCII     Hex
//0         control   00
//1         control   01
//2         control   02
//3         control   03
//4         control   04
//5         control   05
//6         control   06
//7         control   07
//8         control   08
//9         \t        09
//10        \n        0A
//11        \v        0B
//12        \f        0C
//13        \r        0D
//14        control   0E
//15        control   0F
//16        control   10
//17        control   11
//18        control   12
//19        control   13
//20        control   14
//21        control   15
//22        control   16
//23        control   17
//24        control   18
//25        control   19
//26        control   1A
//27        control   1B
//28        control   1C
//29        control   1D
//30        control   1E
//31        control   1F
//32        space     20
//33        !         21
//34        "         22
//35        #         23
//36        $         24
//37        %         25
//38        &         26
//39        '         27
//40        (         28
//41        )         29
//42        *         2A
//43        +         2B
//44        ,         2C
//45        -         2D
//46        .         2E
//47        /         2F
//48        0         30
//49        1         31
//50        2         32
//51        3         33
//52        4         34
//53        5         35
//54        6         36
//55        7         37
//56        8         38
//57        9         39
//58        :         3A
//59        ;         3B
//60        <         3C
//61        =         3D
//62        >         3E
//63        ?         3F
//64        @         40
//65        A         41
//66        B         42
//67        C         43
//68        D         44
//69        E         45
//70        F         46
//71        G         47
//72        H         48
//73        I         49
//74        J         4A
//75        K         4B
//76        L         4C
//77        M         4D
//78        N         4E
//79        O         4F
//80        P         50
//81        Q         51
//82        R         52
//83        S         53
//84        T         54
//85        U         55
//86        V         56
//87        W         57
//88        X         58
//89        Y         59
//90        Z         5A
//91        [         5B
//92        \         5C
//93        ]         5D
//94        ^         5E
//95        _         5F
//96        `         60
//97        a         61
//98        b         62
//99        c         63
//100       d         64
//101       e         65
//102       f         66
//103       g         67
//104       h         68
//105       i         69
//106       j         6A
//107       k         6B
//108       l         6C
//109       m         6D
//110       n         6E
//111       o         6F
//112       p         70
//113       q         71
//114       r         72
//115       s         73
//116       t         74
//117       u         75
//118       v         76
//119       w         77
//120       x         78
//121       y         79
//122       z         7A
//123       {         7B
//124       |         7C
//125       }         7D
//126       ~         7E
//127       control   7F