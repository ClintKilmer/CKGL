/*
 * OpenGL and OpenGL ES Bindings
 * Requires minimum OpenGL 2.1 or OpenGL ES 3.0
 * Loads a common function set from OpenGL 2.1 and OpenGL ES 3.0, with additional OpenGL 3.2+ functions
 * 
 * Partially derived from:
 * OpenGL.cs from Rise: https://github.com/ChevyRay/Rise
 * OpenGLDevice_GL.cs from FNA: https://github.com/FNA-XNA/FNA
 */

using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using static CKGL.OpenGLBindings.Enums;
using GLint = System.Int32;
using GLuint = System.UInt32;
using GLuint64 = System.UInt64;

namespace CKGL.OpenGLBindings
{
	#region Enums
	#region Raw Enums
	internal static class Enums
	{
		public const GLuint GL_BACK = 0x0405;
		public const GLuint GL_FRONT = 0x0404;
		public const GLuint GL_FRONT_AND_BACK = 0x0408;
		public const GLuint GL_CCW = 0x0901;
		public const GLuint GL_CW = 0x0900;
		public const GLuint GL_ALLOW_DRAW_FRG_HINT_PGI = 0x1A210;
		public const GLuint GL_ALLOW_DRAW_MEM_HINT_PGI = 0x1A211;
		public const GLuint GL_ALLOW_DRAW_OBJ_HINT_PGI = 0x1A20E;
		public const GLuint GL_ALLOW_DRAW_WIN_HINT_PGI = 0x1A20F;
		public const GLuint GL_ALWAYS_FAST_HINT_PGI = 0x1A20C;
		public const GLuint GL_ALWAYS_SOFT_HINT_PGI = 0x1A20D;
		public const GLuint GL_BACK_NORMALS_HINT_PGI = 0x1A223;
		public const GLuint GL_BINNING_CONTROL_HINT_QCOM = 0x8FB0;
		public const GLuint GL_CLIP_FAR_HINT_PGI = 0x1A221;
		public const GLuint GL_CLIP_NEAR_HINT_PGI = 0x1A220;
		public const GLuint GL_CLIP_VOLUME_CLIPPING_HINT_EXT = 0x80F0;
		public const GLuint GL_CONSERVE_MEMORY_HINT_PGI = 0x1A1FD;
		public const GLuint GL_CONVOLUTION_HINT_SGIX = 0x8316;
		public const GLuint GL_FRAGMENT_SHADER_DERIVATIVE_HINT = 0x8B8B;
		public const GLuint GL_FRAGMENT_SHADER_DERIVATIVE_HINT_ARB = 0x8B8B;
		public const GLuint GL_FRAGMENT_SHADER_DERIVATIVE_HINT_OES = 0x8B8B;
		public const GLuint GL_FULL_STIPPLE_HINT_PGI = 0x1A219;
		public const GLuint GL_GENERATE_MIPMAP_HINT_SGIS = 0x8192;
		public const GLuint GL_LINE_QUALITY_HINT_SGIX = 0x835B;
		public const GLuint GL_LINE_SMOOTH_HINT = 0x0C52;
		public const GLuint GL_MATERIAL_SIDE_HINT_PGI = 0x1A22C;
		public const GLuint GL_MAX_VERTEX_HINT_PGI = 0x1A22D;
		public const GLuint GL_MULTISAMPLE_FILTER_HINT_NV = 0x8534;
		public const GLuint GL_NATIVE_GRAPHICS_BEGIN_HINT_PGI = 0x1A203;
		public const GLuint GL_NATIVE_GRAPHICS_END_HINT_PGI = 0x1A204;
		public const GLuint GL_PACK_CMYK_HINT_EXT = 0x800E;
		public const GLuint GL_PHONG_HINT_WIN = 0x80EB;
		public const GLuint GL_POLYGON_SMOOTH_HINT = 0x0C53;
		public const GLuint GL_PREFER_DOUBLEBUFFER_HINT_PGI = 0x1A1F8;
		public const GLuint GL_PROGRAM_BINARY_RETRIEVABLE_HINT = 0x8257;
		public const GLuint GL_RECLAIM_MEMORY_HINT_PGI = 0x1A1FE;
		public const GLuint GL_SCALEBIAS_HINT_SGIX = 0x8322;
		public const GLuint GL_STRICT_DEPTHFUNC_HINT_PGI = 0x1A216;
		public const GLuint GL_STRICT_LIGHTING_HINT_PGI = 0x1A217;
		public const GLuint GL_STRICT_SCISSOR_HINT_PGI = 0x1A218;
		public const GLuint GL_TEXTURE_COMPRESSION_HINT = 0x84EF;
		public const GLuint GL_TEXTURE_COMPRESSION_HINT_ARB = 0x84EF;
		public const GLuint GL_TEXTURE_MULTI_BUFFER_HINT_SGIX = 0x812E;
		public const GLuint GL_TEXTURE_STORAGE_HINT_APPLE = 0x85BC;
		public const GLuint GL_TRANSFORM_HINT_APPLE = 0x85B1;
		public const GLuint GL_UNPACK_CMYK_HINT_EXT = 0x800F;
		public const GLuint GL_VERTEX_ARRAY_STORAGE_HINT_APPLE = 0x851F;
		public const GLuint GL_VERTEX_CONSISTENT_HINT_PGI = 0x1A22B;
		public const GLuint GL_VERTEX_DATA_HINT_PGI = 0x1A22A;
		public const GLuint GL_VERTEX_PRECLIP_HINT_SGIX = 0x83EF;
		public const GLuint GL_VERTEX_PRECLIP_SGIX = 0x83EE;
		public const GLuint GL_WIDE_LINE_HINT_PGI = 0x1A222;
		public const GLuint GL_DONT_CARE = 0x1100;
		public const GLuint GL_FASTEST = 0x1101;
		public const GLuint GL_NICEST = 0x1102;
		public const GLuint GL_FILL = 0x1B02;
		public const GLuint GL_LINE = 0x1B01;
		public const GLuint GL_POINT = 0x1B00;
		public const GLuint GL_DETAIL_TEXTURE_2D_SGIS = 0x8095;
		public const GLuint GL_PROXY_TEXTURE_1D = 0x8063;
		public const GLuint GL_PROXY_TEXTURE_1D_EXT = 0x8063;
		public const GLuint GL_PROXY_TEXTURE_2D = 0x8064;
		public const GLuint GL_PROXY_TEXTURE_2D_EXT = 0x8064;
		public const GLuint GL_PROXY_TEXTURE_3D = 0x8070;
		public const GLuint GL_PROXY_TEXTURE_3D_EXT = 0x8070;
		public const GLuint GL_PROXY_TEXTURE_4D_SGIS = 0x8135;
		public const GLuint GL_TEXTURE_1D = 0x0DE0;
		public const GLuint GL_TEXTURE_2D = 0x0DE1;
		public const GLuint GL_TEXTURE_3D = 0x806F;
		public const GLuint GL_TEXTURE_3D_EXT = 0x806F;
		public const GLuint GL_TEXTURE_3D_OES = 0x806F;
		public const GLuint GL_TEXTURE_4D_SGIS = 0x8134;
		public const GLuint GL_TEXTURE_BASE_LEVEL = 0x813C;
		public const GLuint GL_TEXTURE_BASE_LEVEL_SGIS = 0x813C;
		public const GLuint GL_TEXTURE_MAX_LEVEL = 0x813D;
		public const GLuint GL_TEXTURE_MAX_LEVEL_SGIS = 0x813D;
		public const GLuint GL_TEXTURE_MAX_LOD = 0x813B;
		public const GLuint GL_TEXTURE_MAX_LOD_SGIS = 0x813B;
		public const GLuint GL_TEXTURE_MIN_LOD = 0x813A;
		public const GLuint GL_TEXTURE_MIN_LOD_SGIS = 0x813A;
		public const GLuint GL_DETAIL_TEXTURE_LEVEL_SGIS = 0x809A;
		public const GLuint GL_DETAIL_TEXTURE_MODE_SGIS = 0x809B;
		public const GLuint GL_DUAL_TEXTURE_SELECT_SGIS = 0x8124;
		public const GLuint GL_GENERATE_MIPMAP_SGIS = 0x8191;
		public const GLuint GL_POST_TEXTURE_FILTER_BIAS_SGIX = 0x8179;
		public const GLuint GL_POST_TEXTURE_FILTER_SCALE_SGIX = 0x817A;
		public const GLuint GL_QUAD_TEXTURE_SELECT_SGIS = 0x8125;
		public const GLuint GL_SHADOW_AMBIENT_SGIX = 0x80BF;
		public const GLuint GL_TEXTURE_BORDER_COLOR = 0x1004;
		public const GLuint GL_TEXTURE_CLIPMAP_CENTER_SGIX = 0x8171;
		public const GLuint GL_TEXTURE_CLIPMAP_DEPTH_SGIX = 0x8176;
		public const GLuint GL_TEXTURE_CLIPMAP_FRAME_SGIX = 0x8172;
		public const GLuint GL_TEXTURE_CLIPMAP_LOD_OFFSET_SGIX = 0x8175;
		public const GLuint GL_TEXTURE_CLIPMAP_OFFSET_SGIX = 0x8173;
		public const GLuint GL_TEXTURE_CLIPMAP_VIRTUAL_DEPTH_SGIX = 0x8174;
		public const GLuint GL_TEXTURE_COMPARE_SGIX = 0x819A;
		public const GLuint GL_TEXTURE_LOD_BIAS_R_SGIX = 0x8190;
		public const GLuint GL_TEXTURE_LOD_BIAS_S_SGIX = 0x818E;
		public const GLuint GL_TEXTURE_LOD_BIAS_T_SGIX = 0x818F;
		public const GLuint GL_TEXTURE_MAG_FILTER = 0x2800;
		public const GLuint GL_TEXTURE_MAX_CLAMP_R_SGIX = 0x836B;
		public const GLuint GL_TEXTURE_MAX_CLAMP_S_SGIX = 0x8369;
		public const GLuint GL_TEXTURE_MAX_CLAMP_T_SGIX = 0x836A;
		public const GLuint GL_TEXTURE_MIN_FILTER = 0x2801;
		public const GLuint GL_TEXTURE_PRIORITY_EXT = 0x8066;
		public const GLuint GL_TEXTURE_WRAP_Q_SGIS = 0x8137;
		public const GLuint GL_TEXTURE_WRAP_R = 0x8072;
		public const GLuint GL_TEXTURE_WRAP_R_EXT = 0x8072;
		public const GLuint GL_TEXTURE_WRAP_R_OES = 0x8072;
		public const GLuint GL_TEXTURE_WRAP_S = 0x2802;
		public const GLuint GL_TEXTURE_WRAP_T = 0x2803;
		public const GLuint GL_ABGR_EXT = 0x8000;
		public const GLuint GL_ALPHA = 0x1906;
		public const GLuint GL_BLUE = 0x1905;
		public const GLuint GL_CMYKA_EXT = 0x800D;
		public const GLuint GL_CMYK_EXT = 0x800C;
		public const GLuint GL_DEPTH_COMPONENT = 0x1902;
		public const GLuint GL_GREEN = 0x1904;
		public const GLuint GL_RED = 0x1903;
		public const GLuint GL_RED_EXT = 0x1903;
		public const GLuint GL_RGB = 0x1907;
		public const GLuint GL_RGBA = 0x1908;
		public const GLuint GL_STENCIL_INDEX = 0x1901;
		public const GLuint GL_UNSIGNED_INT = 0x1405;
		public const GLuint GL_UNSIGNED_SHORT = 0x1403;
		public const GLuint GL_YCRCB_422_SGIX = 0x81BB;
		public const GLuint GL_YCRCB_444_SGIX = 0x81BC;
		public const GLuint GL_BYTE = 0x1400;
		public const GLuint GL_FLOAT = 0x1406;
		public const GLuint GL_INT = 0x1404;
		public const GLuint GL_SHORT = 0x1402;
		public const GLuint GL_UNSIGNED_BYTE = 0x1401;
		public const GLuint GL_UNSIGNED_BYTE_3_3_2 = 0x8032;
		public const GLuint GL_UNSIGNED_BYTE_3_3_2_EXT = 0x8032;
		public const GLuint GL_UNSIGNED_INT_10_10_10_2 = 0x8036;
		public const GLuint GL_UNSIGNED_INT_10_10_10_2_EXT = 0x8036;
		public const GLuint GL_UNSIGNED_INT_8_8_8_8 = 0x8035;
		public const GLuint GL_UNSIGNED_INT_8_8_8_8_EXT = 0x8035;
		public const GLuint GL_UNSIGNED_SHORT_4_4_4_4 = 0x8033;
		public const GLuint GL_UNSIGNED_SHORT_4_4_4_4_EXT = 0x8033;
		public const GLuint GL_UNSIGNED_SHORT_5_5_5_1 = 0x8034;
		public const GLuint GL_UNSIGNED_SHORT_5_5_5_1_EXT = 0x8034;
		public const GLuint GL_BACK_LEFT = 0x0402;
		public const GLuint GL_BACK_RIGHT = 0x0403;
		public const GLuint GL_FRONT_LEFT = 0x0400;
		public const GLuint GL_FRONT_RIGHT = 0x0401;
		public const GLuint GL_LEFT = 0x0406;
		public const GLuint GL_NONE = 0;
		public const GLuint GL_NONE_OES = 0;
		public const GLuint GL_RIGHT = 0x0407;
		public const GLuint GL_COLOR_BUFFER_BIT = 0x00004000;
		public const GLuint GL_COVERAGE_BUFFER_BIT_NV = 0x00008000;
		public const GLuint GL_DEPTH_BUFFER_BIT = 0x00000100;
		public const GLuint GL_STENCIL_BUFFER_BIT = 0x00000400;
		public const GLuint GL_FALSE = 0;
		public const GLuint GL_TRUE = 1;
		public const GLuint GL_ASYNC_DRAW_PIXELS_SGIX = 0x835D;
		public const GLuint GL_ASYNC_HISTOGRAM_SGIX = 0x832C;
		public const GLuint GL_ASYNC_READ_PIXELS_SGIX = 0x835E;
		public const GLuint GL_ASYNC_TEX_IMAGE_SGIX = 0x835C;
		public const GLuint GL_BLEND = 0x0BE2;
		public const GLuint GL_CALLIGRAPHIC_FRAGMENT_SGIX = 0x8183;
		public const GLuint GL_COLOR_LOGIC_OP = 0x0BF2;
		public const GLuint GL_COLOR_TABLE_SGI = 0x80D0;
		public const GLuint GL_CONVOLUTION_1D_EXT = 0x8010;
		public const GLuint GL_CONVOLUTION_2D_EXT = 0x8011;
		public const GLuint GL_CULL_FACE = 0x0B44;
		public const GLuint GL_DEPTH_TEST = 0x0B71;
		public const GLuint GL_DITHER = 0x0BD0;
		public const GLuint GL_FOG_OFFSET_SGIX = 0x8198;
		public const GLuint GL_FRAGMENT_COLOR_MATERIAL_SGIX = 0x8401;
		public const GLuint GL_FRAGMENT_LIGHT0_SGIX = 0x840C;
		public const GLuint GL_FRAGMENT_LIGHT1_SGIX = 0x840D;
		public const GLuint GL_FRAGMENT_LIGHT2_SGIX = 0x840E;
		public const GLuint GL_FRAGMENT_LIGHT3_SGIX = 0x840F;
		public const GLuint GL_FRAGMENT_LIGHT4_SGIX = 0x8410;
		public const GLuint GL_FRAGMENT_LIGHT5_SGIX = 0x8411;
		public const GLuint GL_FRAGMENT_LIGHT6_SGIX = 0x8412;
		public const GLuint GL_FRAGMENT_LIGHT7_SGIX = 0x8413;
		public const GLuint GL_FRAGMENT_LIGHTING_SGIX = 0x8400;
		public const GLuint GL_FRAMEZOOM_SGIX = 0x818B;
		public const GLuint GL_HISTOGRAM_EXT = 0x8024;
		public const GLuint GL_INTERLACE_SGIX = 0x8094;
		public const GLuint GL_IR_INSTRUMENT1_SGIX = 0x817F;
		public const GLuint GL_LINE_SMOOTH = 0x0B20;
		public const GLuint GL_MINMAX_EXT = 0x802E;
		public const GLuint GL_MULTISAMPLE_SGIS = 0x809D;
		public const GLuint GL_PIXEL_TEXTURE_SGIS = 0x8353;
		public const GLuint GL_PIXEL_TEX_GEN_SGIX = 0x8139;
		public const GLuint GL_POLYGON_OFFSET_FILL = 0x8037;
		public const GLuint GL_POLYGON_OFFSET_LINE = 0x2A02;
		public const GLuint GL_POLYGON_OFFSET_POINT = 0x2A01;
		public const GLuint GL_POLYGON_SMOOTH = 0x0B41;
		public const GLuint GL_POST_COLOR_MATRIX_COLOR_TABLE_SGI = 0x80D2;
		public const GLuint GL_POST_CONVOLUTION_COLOR_TABLE_SGI = 0x80D1;
		public const GLuint GL_REFERENCE_PLANE_SGIX = 0x817D;
		public const GLuint GL_RESCALE_NORMAL_EXT = 0x803A;
		public const GLuint GL_SAMPLE_ALPHA_TO_MASK_SGIS = 0x809E;
		public const GLuint GL_SAMPLE_ALPHA_TO_ONE_SGIS = 0x809F;
		public const GLuint GL_SAMPLE_MASK_SGIS = 0x80A0;
		public const GLuint GL_SCISSOR_TEST = 0x0C11;
		public const GLuint GL_SEPARABLE_2D_EXT = 0x8012;
		public const GLuint GL_SHARED_TEXTURE_PALETTE_EXT = 0x81FB;
		public const GLuint GL_SPRITE_SGIX = 0x8148;
		public const GLuint GL_STENCIL_TEST = 0x0B90;
		public const GLuint GL_TEXTURE_COLOR_TABLE_SGI = 0x80BC;
		public const GLuint GL_CONSTANT_ALPHA_EXT = 0x8003;
		public const GLuint GL_CONSTANT_COLOR_EXT = 0x8001;
		public const GLuint GL_DST_ALPHA = 0x0304;
		public const GLuint GL_DST_COLOR = 0x0306;
		public const GLuint GL_ONE = 1;
		public const GLuint GL_ONE_MINUS_CONSTANT_ALPHA_EXT = 0x8004;
		public const GLuint GL_ONE_MINUS_CONSTANT_COLOR_EXT = 0x8002;
		public const GLuint GL_ONE_MINUS_DST_ALPHA = 0x0305;
		public const GLuint GL_ONE_MINUS_DST_COLOR = 0x0307;
		public const GLuint GL_ONE_MINUS_SRC_ALPHA = 0x0303;
		public const GLuint GL_SRC_ALPHA = 0x0302;
		public const GLuint GL_SRC_ALPHA_SATURATE = 0x0308;
		public const GLuint GL_ZERO = 0;
		public const GLuint GL_ONE_MINUS_SRC_COLOR = 0x0301;
		public const GLuint GL_SRC_COLOR = 0x0300;
		public const GLuint GL_AND = 0x1501;
		public const GLuint GL_AND_INVERTED = 0x1504;
		public const GLuint GL_AND_REVERSE = 0x1502;
		public const GLuint GL_CLEAR = 0x1500;
		public const GLuint GL_COPY = 0x1503;
		public const GLuint GL_COPY_INVERTED = 0x150C;
		public const GLuint GL_EQUIV = 0x1509;
		public const GLuint GL_INVERT = 0x150A;
		public const GLuint GL_NAND = 0x150E;
		public const GLuint GL_NOOP = 0x1505;
		public const GLuint GL_NOR = 0x1508;
		public const GLuint GL_OR = 0x1507;
		public const GLuint GL_OR_INVERTED = 0x150D;
		public const GLuint GL_OR_REVERSE = 0x150B;
		public const GLuint GL_SET = 0x150F;
		public const GLuint GL_XOR = 0x1506;
		public const GLuint GL_ALWAYS = 0x0207;
		public const GLuint GL_EQUAL = 0x0202;
		public const GLuint GL_GEQUAL = 0x0206;
		public const GLuint GL_GREATER = 0x0204;
		public const GLuint GL_LEQUAL = 0x0203;
		public const GLuint GL_LESS = 0x0201;
		public const GLuint GL_NEVER = 0x0200;
		public const GLuint GL_NOTEQUAL = 0x0205;
		public const GLuint GL_DECR = 0x1E03;
		public const GLuint GL_INCR = 0x1E02;
		public const GLuint GL_KEEP = 0x1E00;
		public const GLuint GL_REPLACE = 0x1E01;
		public const GLuint GL_PACK_ALIGNMENT = 0x0D05;
		public const GLuint GL_PACK_IMAGE_DEPTH_SGIS = 0x8131;
		public const GLuint GL_PACK_IMAGE_HEIGHT = 0x806C;
		public const GLuint GL_PACK_IMAGE_HEIGHT_EXT = 0x806C;
		public const GLuint GL_PACK_LSB_FIRST = 0x0D01;
		public const GLuint GL_PACK_RESAMPLE_OML = 0x8984;
		public const GLuint GL_PACK_RESAMPLE_SGIX = 0x842C;
		public const GLuint GL_PACK_ROW_LENGTH = 0x0D02;
		public const GLuint GL_PACK_SKIP_IMAGES = 0x806B;
		public const GLuint GL_PACK_SKIP_IMAGES_EXT = 0x806B;
		public const GLuint GL_PACK_SKIP_PIXELS = 0x0D04;
		public const GLuint GL_PACK_SKIP_ROWS = 0x0D03;
		public const GLuint GL_PACK_SKIP_VOLUMES_SGIS = 0x8130;
		public const GLuint GL_PACK_SUBSAMPLE_RATE_SGIX = 0x85A0;
		public const GLuint GL_PACK_SWAP_BYTES = 0x0D00;
		public const GLuint GL_PIXEL_TILE_CACHE_SIZE_SGIX = 0x8145;
		public const GLuint GL_PIXEL_TILE_GRID_DEPTH_SGIX = 0x8144;
		public const GLuint GL_PIXEL_TILE_GRID_HEIGHT_SGIX = 0x8143;
		public const GLuint GL_PIXEL_TILE_GRID_WIDTH_SGIX = 0x8142;
		public const GLuint GL_PIXEL_TILE_HEIGHT_SGIX = 0x8141;
		public const GLuint GL_PIXEL_TILE_WIDTH_SGIX = 0x8140;
		public const GLuint GL_UNPACK_ALIGNMENT = 0x0CF5;
		public const GLuint GL_UNPACK_IMAGE_DEPTH_SGIS = 0x8133;
		public const GLuint GL_UNPACK_IMAGE_HEIGHT = 0x806E;
		public const GLuint GL_UNPACK_IMAGE_HEIGHT_EXT = 0x806E;
		public const GLuint GL_UNPACK_LSB_FIRST = 0x0CF1;
		public const GLuint GL_UNPACK_RESAMPLE_OML = 0x8985;
		public const GLuint GL_UNPACK_RESAMPLE_SGIX = 0x842D;
		public const GLuint GL_UNPACK_ROW_LENGTH = 0x0CF2;
		public const GLuint GL_UNPACK_ROW_LENGTH_EXT = 0x0CF2;
		public const GLuint GL_UNPACK_SKIP_IMAGES = 0x806D;
		public const GLuint GL_UNPACK_SKIP_IMAGES_EXT = 0x806D;
		public const GLuint GL_UNPACK_SKIP_PIXELS = 0x0CF4;
		public const GLuint GL_UNPACK_SKIP_PIXELS_EXT = 0x0CF4;
		public const GLuint GL_UNPACK_SKIP_ROWS = 0x0CF3;
		public const GLuint GL_UNPACK_SKIP_ROWS_EXT = 0x0CF3;
		public const GLuint GL_UNPACK_SKIP_VOLUMES_SGIS = 0x8132;
		public const GLuint GL_UNPACK_SUBSAMPLE_RATE_SGIX = 0x85A1;
		public const GLuint GL_UNPACK_SWAP_BYTES = 0x0CF0;
		public const GLuint GL_ALIASED_LINE_WIDTH_RANGE = 0x846E;
		public const GLuint GL_ALPHA_TEST_FUNC_QCOM = 0x0BC1;
		public const GLuint GL_ALPHA_TEST_QCOM = 0x0BC0;
		public const GLuint GL_ALPHA_TEST_REF_QCOM = 0x0BC2;
		public const GLuint GL_ASYNC_MARKER_SGIX = 0x8329;
		public const GLuint GL_BLEND_COLOR_EXT = 0x8005;
		public const GLuint GL_BLEND_DST = 0x0BE0;
		public const GLuint GL_BLEND_EQUATION_EXT = 0x8009;
		public const GLuint GL_BLEND_SRC = 0x0BE1;
		public const GLuint GL_COLOR_ARRAY_COUNT_EXT = 0x8084;
		public const GLuint GL_COLOR_CLEAR_VALUE = 0x0C22;
		public const GLuint GL_COLOR_MATRIX_SGI = 0x80B1;
		public const GLuint GL_COLOR_MATRIX_STACK_DEPTH_SGI = 0x80B2;
		public const GLuint GL_COLOR_WRITEMASK = 0x0C23;
		public const GLuint GL_CULL_FACE_MODE = 0x0B45;
		public const GLuint GL_DEFORMATIONS_MASK_SGIX = 0x8196;
		public const GLuint GL_DEPTH_CLEAR_VALUE = 0x0B73;
		public const GLuint GL_DEPTH_FUNC = 0x0B74;
		public const GLuint GL_DEPTH_RANGE = 0x0B70;
		public const GLuint GL_DEPTH_WRITEMASK = 0x0B72;
		public const GLuint GL_DETAIL_TEXTURE_2D_BINDING_SGIS = 0x8096;
		public const GLuint GL_DISTANCE_ATTENUATION_SGIS = 0x8129;
		public const GLuint GL_DOUBLEBUFFER = 0x0C32;
		public const GLuint GL_DRAW_BUFFER = 0x0C01;
		public const GLuint GL_DRAW_BUFFER_EXT = 0x0C01;
		public const GLuint GL_EDGE_FLAG_ARRAY_COUNT_EXT = 0x808D;
		public const GLuint GL_FOG_FUNC_POINTS_SGIS = 0x812B;
		public const GLuint GL_FOG_OFFSET_VALUE_SGIX = 0x8199;
		public const GLuint GL_FRAGMENT_COLOR_MATERIAL_FACE_SGIX = 0x8402;
		public const GLuint GL_FRAGMENT_COLOR_MATERIAL_PARAMETER_SGIX = 0x8403;
		public const GLuint GL_FRAGMENT_LIGHT_MODEL_AMBIENT_SGIX = 0x840A;
		public const GLuint GL_FRAGMENT_LIGHT_MODEL_LOCAL_VIEWER_SGIX = 0x8408;
		public const GLuint GL_FRAGMENT_LIGHT_MODEL_NORMAL_INTERPOLATION_SGIX = 0x840B;
		public const GLuint GL_FRAGMENT_LIGHT_MODEL_TWO_SIDE_SGIX = 0x8409;
		public const GLuint GL_FRAMEZOOM_FACTOR_SGIX = 0x818C;
		public const GLuint GL_FRONT_FACE = 0x0B46;
		public const GLuint GL_INDEX_ARRAY_COUNT_EXT = 0x8087;
		public const GLuint GL_INSTRUMENT_MEASUREMENTS_SGIX = 0x8181;
		public const GLuint GL_LIGHT_ENV_MODE_SGIX = 0x8407;
		public const GLuint GL_LINE_WIDTH = 0x0B21;
		public const GLuint GL_LINE_WIDTH_GRANULARITY = 0x0B23;
		public const GLuint GL_LINE_WIDTH_RANGE = 0x0B22;
		public const GLuint GL_LOGIC_OP_MODE = 0x0BF0;
		public const GLuint GL_MAX_3D_TEXTURE_SIZE_EXT = 0x8073;
		public const GLuint GL_MAX_4D_TEXTURE_SIZE_SGIS = 0x8138;
		public const GLuint GL_MAX_ACTIVE_LIGHTS_SGIX = 0x8405;
		public const GLuint GL_MAX_ASYNC_DRAW_PIXELS_SGIX = 0x8360;
		public const GLuint GL_MAX_ASYNC_HISTOGRAM_SGIX = 0x832D;
		public const GLuint GL_MAX_ASYNC_READ_PIXELS_SGIX = 0x8361;
		public const GLuint GL_MAX_ASYNC_TEX_IMAGE_SGIX = 0x835F;
		public const GLuint GL_MAX_CLIPMAP_DEPTH_SGIX = 0x8177;
		public const GLuint GL_MAX_CLIPMAP_VIRTUAL_DEPTH_SGIX = 0x8178;
		public const GLuint GL_MAX_CLIP_DISTANCES = 0x0D32;
		public const GLuint GL_MAX_COLOR_MATRIX_STACK_DEPTH_SGI = 0x80B3;
		public const GLuint GL_MAX_FOG_FUNC_POINTS_SGIS = 0x812C;
		public const GLuint GL_MAX_FRAGMENT_LIGHTS_SGIX = 0x8404;
		public const GLuint GL_MAX_FRAMEZOOM_FACTOR_SGIX = 0x818D;
		public const GLuint GL_MAX_TEXTURE_SIZE = 0x0D33;
		public const GLuint GL_MAX_VIEWPORT_DIMS = 0x0D3A;
		public const GLuint GL_MODELVIEW0_MATRIX_EXT = 0x0BA6;
		public const GLuint GL_MODELVIEW0_STACK_DEPTH_EXT = 0x0BA3;
		public const GLuint GL_NORMAL_ARRAY_COUNT_EXT = 0x8080;
		public const GLuint GL_PIXEL_TEX_GEN_MODE_SGIX = 0x832B;
		public const GLuint GL_PIXEL_TILE_BEST_ALIGNMENT_SGIX = 0x813E;
		public const GLuint GL_PIXEL_TILE_CACHE_INCREMENT_SGIX = 0x813F;
		public const GLuint GL_POINT_FADE_THRESHOLD_SIZE_SGIS = 0x8128;
		public const GLuint GL_POINT_SIZE = 0x0B11;
		public const GLuint GL_POINT_SIZE_GRANULARITY = 0x0B13;
		public const GLuint GL_POINT_SIZE_MAX_SGIS = 0x8127;
		public const GLuint GL_POINT_SIZE_MIN_SGIS = 0x8126;
		public const GLuint GL_POINT_SIZE_RANGE = 0x0B12;
		public const GLuint GL_POLYGON_MODE = 0x0B40;
		public const GLuint GL_POLYGON_OFFSET_BIAS_EXT = 0x8039;
		public const GLuint GL_POLYGON_OFFSET_FACTOR = 0x8038;
		public const GLuint GL_POLYGON_OFFSET_UNITS = 0x2A00;
		public const GLuint GL_POST_COLOR_MATRIX_ALPHA_BIAS_SGI = 0x80BB;
		public const GLuint GL_POST_COLOR_MATRIX_ALPHA_SCALE_SGI = 0x80B7;
		public const GLuint GL_POST_COLOR_MATRIX_BLUE_BIAS_SGI = 0x80BA;
		public const GLuint GL_POST_COLOR_MATRIX_BLUE_SCALE_SGI = 0x80B6;
		public const GLuint GL_POST_COLOR_MATRIX_GREEN_BIAS_SGI = 0x80B9;
		public const GLuint GL_POST_COLOR_MATRIX_GREEN_SCALE_SGI = 0x80B5;
		public const GLuint GL_POST_COLOR_MATRIX_RED_BIAS_SGI = 0x80B8;
		public const GLuint GL_POST_COLOR_MATRIX_RED_SCALE_SGI = 0x80B4;
		public const GLuint GL_POST_CONVOLUTION_ALPHA_BIAS_EXT = 0x8023;
		public const GLuint GL_POST_CONVOLUTION_ALPHA_SCALE_EXT = 0x801F;
		public const GLuint GL_POST_CONVOLUTION_BLUE_BIAS_EXT = 0x8022;
		public const GLuint GL_POST_CONVOLUTION_BLUE_SCALE_EXT = 0x801E;
		public const GLuint GL_POST_CONVOLUTION_GREEN_BIAS_EXT = 0x8021;
		public const GLuint GL_POST_CONVOLUTION_GREEN_SCALE_EXT = 0x801D;
		public const GLuint GL_POST_CONVOLUTION_RED_BIAS_EXT = 0x8020;
		public const GLuint GL_POST_CONVOLUTION_RED_SCALE_EXT = 0x801C;
		public const GLuint GL_POST_TEXTURE_FILTER_BIAS_RANGE_SGIX = 0x817B;
		public const GLuint GL_POST_TEXTURE_FILTER_SCALE_RANGE_SGIX = 0x817C;
		public const GLuint GL_READ_BUFFER = 0x0C02;
		public const GLuint GL_READ_BUFFER_EXT = 0x0C02;
		public const GLuint GL_READ_BUFFER_NV = 0x0C02;
		public const GLuint GL_REFERENCE_PLANE_EQUATION_SGIX = 0x817E;
		public const GLuint GL_SAMPLES_SGIS = 0x80A9;
		public const GLuint GL_SAMPLE_BUFFERS_SGIS = 0x80A8;
		public const GLuint GL_SAMPLE_MASK_INVERT_SGIS = 0x80AB;
		public const GLuint GL_SAMPLE_MASK_VALUE_SGIS = 0x80AA;
		public const GLuint GL_SAMPLE_PATTERN_SGIS = 0x80AC;
		public const GLuint GL_SCISSOR_BOX = 0x0C10;
		public const GLuint GL_SMOOTH_LINE_WIDTH_GRANULARITY = 0x0B23;
		public const GLuint GL_SMOOTH_LINE_WIDTH_RANGE = 0x0B22;
		public const GLuint GL_SMOOTH_POINT_SIZE_GRANULARITY = 0x0B13;
		public const GLuint GL_SMOOTH_POINT_SIZE_RANGE = 0x0B12;
		public const GLuint GL_SPRITE_AXIS_SGIX = 0x814A;
		public const GLuint GL_SPRITE_MODE_SGIX = 0x8149;
		public const GLuint GL_SPRITE_TRANSLATION_SGIX = 0x814B;
		public const GLuint GL_STENCIL_CLEAR_VALUE = 0x0B91;
		public const GLuint GL_STENCIL_FAIL = 0x0B94;
		public const GLuint GL_STENCIL_FUNC = 0x0B92;
		public const GLuint GL_STENCIL_PASS_DEPTH_FAIL = 0x0B95;
		public const GLuint GL_STENCIL_PASS_DEPTH_PASS = 0x0B96;
		public const GLuint GL_STENCIL_REF = 0x0B97;
		public const GLuint GL_STENCIL_VALUE_MASK = 0x0B93;
		public const GLuint GL_STENCIL_WRITEMASK = 0x0B98;
		public const GLuint GL_STEREO = 0x0C33;
		public const GLuint GL_SUBPIXEL_BITS = 0x0D50;
		public const GLuint GL_TEXTURE_3D_BINDING_EXT = 0x806A;
		public const GLuint GL_TEXTURE_4D_BINDING_SGIS = 0x814F;
		public const GLuint GL_TEXTURE_BINDING_1D = 0x8068;
		public const GLuint GL_TEXTURE_BINDING_2D = 0x8069;
		public const GLuint GL_TEXTURE_BINDING_3D = 0x806A;
		public const GLuint GL_TEXTURE_COORD_ARRAY_COUNT_EXT = 0x808B;
		public const GLuint GL_VERTEX_ARRAY_COUNT_EXT = 0x807D;
		public const GLuint GL_VIEWPORT = 0x0BA2;
		public const GLuint GL_EXTENSIONS = 0x1F03;
		public const GLuint GL_RENDERER = 0x1F01;
		public const GLuint GL_VENDOR = 0x1F00;
		public const GLuint GL_VERSION = 0x1F02;
		public const GLuint GL_DETAIL_TEXTURE_FUNC_POINTS_SGIS = 0x809C;
		public const GLuint GL_SHARPEN_TEXTURE_FUNC_POINTS_SGIS = 0x80B0;
		public const GLuint GL_TEXTURE_4DSIZE_SGIS = 0x8136;
		public const GLuint GL_TEXTURE_ALPHA_SIZE = 0x805F;
		public const GLuint GL_TEXTURE_BLUE_SIZE = 0x805E;
		public const GLuint GL_TEXTURE_BORDER_COLOR_NV = 0x1004;
		public const GLuint GL_TEXTURE_COMPARE_OPERATOR_SGIX = 0x819B;
		public const GLuint GL_TEXTURE_DEPTH_EXT = 0x8071;
		public const GLuint GL_TEXTURE_FILTER4_SIZE_SGIS = 0x8147;
		public const GLuint GL_TEXTURE_GEQUAL_R_SGIX = 0x819D;
		public const GLuint GL_TEXTURE_GREEN_SIZE = 0x805D;
		public const GLuint GL_TEXTURE_HEIGHT = 0x1001;
		public const GLuint GL_TEXTURE_INTERNAL_FORMAT = 0x1003;
		public const GLuint GL_TEXTURE_LEQUAL_R_SGIX = 0x819C;
		public const GLuint GL_TEXTURE_RED_SIZE = 0x805C;
		public const GLuint GL_TEXTURE_WIDTH = 0x1000;
		public const GLuint GL_LINES = 0x0001;
		public const GLuint GL_LINES_ADJACENCY = 0x000A;
		public const GLuint GL_LINES_ADJACENCY_ARB = 0x000A;
		public const GLuint GL_LINES_ADJACENCY_EXT = 0x000A;
		public const GLuint GL_LINE_LOOP = 0x0002;
		public const GLuint GL_LINE_STRIP = 0x0003;
		public const GLuint GL_LINE_STRIP_ADJACENCY = 0x000B;
		public const GLuint GL_LINE_STRIP_ADJACENCY_ARB = 0x000B;
		public const GLuint GL_LINE_STRIP_ADJACENCY_EXT = 0x000B;
		public const GLuint GL_PATCHES = 0x000E;
		public const GLuint GL_POINTS = 0x0000;
		public const GLuint GL_TRIANGLES = 0x0004;
		public const GLuint GL_TRIANGLES_ADJACENCY = 0x000C;
		public const GLuint GL_TRIANGLES_ADJACENCY_ARB = 0x000C;
		public const GLuint GL_TRIANGLES_ADJACENCY_EXT = 0x000C;
		public const GLuint GL_TRIANGLE_FAN = 0x0006;
		public const GLuint GL_TRIANGLE_STRIP = 0x0005;
		public const GLuint GL_TRIANGLE_STRIP_ADJACENCY = 0x000D;
		public const GLuint GL_TRIANGLE_STRIP_ADJACENCY_ARB = 0x000D;
		public const GLuint GL_TRIANGLE_STRIP_ADJACENCY_EXT = 0x000D;
		public const GLuint GL_CLIP_DISTANCE0 = 0x3000;
		public const GLuint GL_CLIP_DISTANCE1 = 0x3001;
		public const GLuint GL_CLIP_DISTANCE2 = 0x3002;
		public const GLuint GL_CLIP_DISTANCE3 = 0x3003;
		public const GLuint GL_CLIP_DISTANCE4 = 0x3004;
		public const GLuint GL_CLIP_DISTANCE5 = 0x3005;
		public const GLuint GL_CLIP_DISTANCE6 = 0x3006;
		public const GLuint GL_CLIP_DISTANCE7 = 0x3007;
		public const GLuint GL_LIGHT_MODEL_COLOR_CONTROL_EXT = 0x81F8;
		public const GLuint GL_EYE_LINE_SGIS = 0x81F6;
		public const GLuint GL_EYE_POINT_SGIS = 0x81F4;
		public const GLuint GL_OBJECT_LINE_SGIS = 0x81F7;
		public const GLuint GL_OBJECT_POINT_SGIS = 0x81F5;
		public const GLuint GL_MULTISAMPLE_BIT_3DFX = 0x20000000;
		public const GLuint GL_MULTISAMPLE_BIT_ARB = 0x20000000;
		public const GLuint GL_MULTISAMPLE_BIT_EXT = 0x20000000;
		public const GLuint GL_GEOMETRY_DEFORMATION_SGIX = 0x8194;
		public const GLuint GL_TEXTURE_DEFORMATION_SGIX = 0x8195;
		public const GLuint GL_POST_COLOR_MATRIX_ALPHA_BIAS = 0x80BB;
		public const GLuint GL_POST_COLOR_MATRIX_ALPHA_SCALE = 0x80B7;
		public const GLuint GL_POST_COLOR_MATRIX_BLUE_BIAS = 0x80BA;
		public const GLuint GL_POST_COLOR_MATRIX_BLUE_SCALE = 0x80B6;
		public const GLuint GL_POST_COLOR_MATRIX_GREEN_BIAS = 0x80B9;
		public const GLuint GL_POST_COLOR_MATRIX_GREEN_SCALE = 0x80B5;
		public const GLuint GL_POST_COLOR_MATRIX_RED_BIAS = 0x80B8;
		public const GLuint GL_POST_COLOR_MATRIX_RED_SCALE = 0x80B4;
		public const GLuint GL_POST_CONVOLUTION_ALPHA_BIAS = 0x8023;
		public const GLuint GL_POST_CONVOLUTION_ALPHA_SCALE = 0x801F;
		public const GLuint GL_POST_CONVOLUTION_BLUE_BIAS = 0x8022;
		public const GLuint GL_POST_CONVOLUTION_BLUE_SCALE = 0x801E;
		public const GLuint GL_POST_CONVOLUTION_GREEN_BIAS = 0x8021;
		public const GLuint GL_POST_CONVOLUTION_GREEN_SCALE = 0x801D;
		public const GLuint GL_POST_CONVOLUTION_RED_BIAS = 0x8020;
		public const GLuint GL_POST_CONVOLUTION_RED_SCALE = 0x801C;
		public const GLuint GL_COLOR = 0x1800;
		public const GLuint GL_COLOR_EXT = 0x1800;
		public const GLuint GL_DEPTH = 0x1801;
		public const GLuint GL_DEPTH_EXT = 0x1801;
		public const GLuint GL_STENCIL = 0x1802;
		public const GLuint GL_STENCIL_EXT = 0x1802;
		public const GLuint GL_MODELVIEW0_EXT = 0x1700;
		public const GLuint GL_TEXTURE = 0x1702;
		public const GLuint GL_NO_ERROR = 0;
		public const GLuint GL_INVALID_ENUM = 0x0500;
		public const GLuint GL_INVALID_VALUE = 0x0501;
		public const GLuint GL_INVALID_OPERATION = 0x0502;
		public const GLuint GL_OUT_OF_MEMORY = 0x0505;
		public const GLuint GL_DOUBLE = 0x140A;
		public const GLuint GL_NEAREST = 0x2600;
		public const GLuint GL_LINEAR = 0x2601;
		public const GLuint GL_NEAREST_MIPMAP_NEAREST = 0x2700;
		public const GLuint GL_LINEAR_MIPMAP_NEAREST = 0x2701;
		public const GLuint GL_NEAREST_MIPMAP_LINEAR = 0x2702;
		public const GLuint GL_LINEAR_MIPMAP_LINEAR = 0x2703;
		public const GLuint GL_REPEAT = 0x2901;
		public const GLuint GL_R3_G3_B2 = 0x2A10;
		public const GLuint GL_RGB4 = 0x804F;
		public const GLuint GL_RGB5 = 0x8050;
		public const GLuint GL_RGB565 = 0x8D62; // Added
		public const GLuint GL_RGB8 = 0x8051;
		public const GLuint GL_RGB10 = 0x8052;
		public const GLuint GL_RGB12 = 0x8053;
		public const GLuint GL_RGB16 = 0x8054;
		public const GLuint GL_RGBA2 = 0x8055;
		public const GLuint GL_RGBA4 = 0x8056;
		public const GLuint GL_RGB5_A1 = 0x8057;
		public const GLuint GL_RGBA8 = 0x8058;
		public const GLuint GL_RGB10_A2 = 0x8059;
		public const GLuint GL_RGBA12 = 0x805A;
		public const GLuint GL_RGBA16 = 0x805B;
		public const GLuint GL_TEXTURE_DEPTH = 0x8071;
		public const GLuint GL_MAX_3D_TEXTURE_SIZE = 0x8073;
		public const GLuint GL_UNSIGNED_BYTE_2_3_3_REV = 0x8362;
		public const GLuint GL_UNSIGNED_SHORT_5_6_5 = 0x8363;
		public const GLuint GL_UNSIGNED_SHORT_5_6_5_REV = 0x8364;
		public const GLuint GL_UNSIGNED_SHORT_4_4_4_4_REV = 0x8365;
		public const GLuint GL_UNSIGNED_SHORT_1_5_5_5_REV = 0x8366;
		public const GLuint GL_UNSIGNED_INT_8_8_8_8_REV = 0x8367;
		public const GLuint GL_UNSIGNED_INT_2_10_10_10_REV = 0x8368;
		public const GLuint GL_BGR = 0x80E0;
		public const GLuint GL_BGRA = 0x80E1;
		public const GLuint GL_MAX_ELEMENTS_VERTICES = 0x80E8;
		public const GLuint GL_MAX_ELEMENTS_INDICES = 0x80E9;
		public const GLuint GL_CLAMP_TO_EDGE = 0x812F;
		public const GLuint GL_TEXTURE0 = 0x84C0;
		public const GLuint GL_TEXTURE1 = 0x84C1;
		public const GLuint GL_TEXTURE2 = 0x84C2;
		public const GLuint GL_TEXTURE3 = 0x84C3;
		public const GLuint GL_TEXTURE4 = 0x84C4;
		public const GLuint GL_TEXTURE5 = 0x84C5;
		public const GLuint GL_TEXTURE6 = 0x84C6;
		public const GLuint GL_TEXTURE7 = 0x84C7;
		public const GLuint GL_TEXTURE8 = 0x84C8;
		public const GLuint GL_TEXTURE9 = 0x84C9;
		public const GLuint GL_TEXTURE10 = 0x84CA;
		public const GLuint GL_TEXTURE11 = 0x84CB;
		public const GLuint GL_TEXTURE12 = 0x84CC;
		public const GLuint GL_TEXTURE13 = 0x84CD;
		public const GLuint GL_TEXTURE14 = 0x84CE;
		public const GLuint GL_TEXTURE15 = 0x84CF;
		public const GLuint GL_TEXTURE16 = 0x84D0;
		public const GLuint GL_TEXTURE17 = 0x84D1;
		public const GLuint GL_TEXTURE18 = 0x84D2;
		public const GLuint GL_TEXTURE19 = 0x84D3;
		public const GLuint GL_TEXTURE20 = 0x84D4;
		public const GLuint GL_TEXTURE21 = 0x84D5;
		public const GLuint GL_TEXTURE22 = 0x84D6;
		public const GLuint GL_TEXTURE23 = 0x84D7;
		public const GLuint GL_TEXTURE24 = 0x84D8;
		public const GLuint GL_TEXTURE25 = 0x84D9;
		public const GLuint GL_TEXTURE26 = 0x84DA;
		public const GLuint GL_TEXTURE27 = 0x84DB;
		public const GLuint GL_TEXTURE28 = 0x84DC;
		public const GLuint GL_TEXTURE29 = 0x84DD;
		public const GLuint GL_TEXTURE30 = 0x84DE;
		public const GLuint GL_TEXTURE31 = 0x84DF;
		public const GLuint GL_ACTIVE_TEXTURE = 0x84E0;
		public const GLuint GL_MULTISAMPLE = 0x809D;
		public const GLuint GL_SAMPLE_ALPHA_TO_COVERAGE = 0x809E;
		public const GLuint GL_SAMPLE_ALPHA_TO_ONE = 0x809F;
		public const GLuint GL_SAMPLE_COVERAGE = 0x80A0;
		public const GLuint GL_SAMPLE_BUFFERS = 0x80A8;
		public const GLuint GL_SAMPLES = 0x80A9;
		public const GLuint GL_SAMPLE_COVERAGE_VALUE = 0x80AA;
		public const GLuint GL_SAMPLE_COVERAGE_INVERT = 0x80AB;
		public const GLuint GL_TEXTURE_CUBE_MAP = 0x8513;
		public const GLuint GL_TEXTURE_BINDING_CUBE_MAP = 0x8514;
		public const GLuint GL_TEXTURE_CUBE_MAP_POSITIVE_X = 0x8515;
		public const GLuint GL_TEXTURE_CUBE_MAP_NEGATIVE_X = 0x8516;
		public const GLuint GL_TEXTURE_CUBE_MAP_POSITIVE_Y = 0x8517;
		public const GLuint GL_TEXTURE_CUBE_MAP_NEGATIVE_Y = 0x8518;
		public const GLuint GL_TEXTURE_CUBE_MAP_POSITIVE_Z = 0x8519;
		public const GLuint GL_TEXTURE_CUBE_MAP_NEGATIVE_Z = 0x851A;
		public const GLuint GL_PROXY_TEXTURE_CUBE_MAP = 0x851B;
		public const GLuint GL_MAX_CUBE_MAP_TEXTURE_SIZE = 0x851C;
		public const GLuint GL_COMPRESSED_RGB = 0x84ED;
		public const GLuint GL_COMPRESSED_RGBA = 0x84EE;
		public const GLuint GL_TEXTURE_COMPRESSED_IMAGE_SIZE = 0x86A0;
		public const GLuint GL_TEXTURE_COMPRESSED = 0x86A1;
		public const GLuint GL_NUM_COMPRESSED_TEXTURE_FORMATS = 0x86A2;
		public const GLuint GL_COMPRESSED_TEXTURE_FORMATS = 0x86A3;
		public const GLuint GL_CLAMP_TO_BORDER = 0x812D;
		public const GLuint GL_BLEND_DST_RGB = 0x80C8;
		public const GLuint GL_BLEND_SRC_RGB = 0x80C9;
		public const GLuint GL_BLEND_DST_ALPHA = 0x80CA;
		public const GLuint GL_BLEND_SRC_ALPHA = 0x80CB;
		public const GLuint GL_POINT_FADE_THRESHOLD_SIZE = 0x8128;
		public const GLuint GL_DEPTH_COMPONENT16 = 0x81A5;
		public const GLuint GL_DEPTH_COMPONENT24 = 0x81A6;
		public const GLuint GL_DEPTH_COMPONENT32 = 0x81A7;
		public const GLuint GL_MIRRORED_REPEAT = 0x8370;
		public const GLuint GL_MAX_TEXTURE_LOD_BIAS = 0x84FD;
		public const GLuint GL_TEXTURE_LOD_BIAS = 0x8501;
		public const GLuint GL_INCR_WRAP = 0x8507;
		public const GLuint GL_DECR_WRAP = 0x8508;
		public const GLuint GL_TEXTURE_DEPTH_SIZE = 0x884A;
		public const GLuint GL_TEXTURE_COMPARE_MODE = 0x884C;
		public const GLuint GL_TEXTURE_COMPARE_FUNC = 0x884D;
		public const GLuint GL_FUNC_ADD = 0x8006;
		public const GLuint GL_FUNC_SUBTRACT = 0x800A;
		public const GLuint GL_FUNC_REVERSE_SUBTRACT = 0x800B;
		public const GLuint GL_MIN = 0x8007;
		public const GLuint GL_MAX = 0x8008;
		public const GLuint GL_CONSTANT_COLOR = 0x8001;
		public const GLuint GL_ONE_MINUS_CONSTANT_COLOR = 0x8002;
		public const GLuint GL_CONSTANT_ALPHA = 0x8003;
		public const GLuint GL_ONE_MINUS_CONSTANT_ALPHA = 0x8004;
		public const GLuint GL_BUFFER_SIZE = 0x8764;
		public const GLuint GL_BUFFER_USAGE = 0x8765;
		public const GLuint GL_QUERY_COUNTER_BITS = 0x8864;
		public const GLuint GL_CURRENT_QUERY = 0x8865;
		public const GLuint GL_QUERY_RESULT = 0x8866;
		public const GLuint GL_QUERY_RESULT_AVAILABLE = 0x8867;
		public const GLuint GL_ARRAY_BUFFER = 0x8892;
		public const GLuint GL_ELEMENT_ARRAY_BUFFER = 0x8893;
		public const GLuint GL_ARRAY_BUFFER_BINDING = 0x8894;
		public const GLuint GL_ELEMENT_ARRAY_BUFFER_BINDING = 0x8895;
		public const GLuint GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING = 0x889F;
		public const GLuint GL_READ_ONLY = 0x88B8;
		public const GLuint GL_WRITE_ONLY = 0x88B9;
		public const GLuint GL_READ_WRITE = 0x88BA;
		public const GLuint GL_BUFFER_ACCESS = 0x88BB;
		public const GLuint GL_BUFFER_MAPPED = 0x88BC;
		public const GLuint GL_BUFFER_MAP_POINTER = 0x88BD;
		public const GLuint GL_STREAM_DRAW = 0x88E0;
		public const GLuint GL_STREAM_READ = 0x88E1;
		public const GLuint GL_STREAM_COPY = 0x88E2;
		public const GLuint GL_STATIC_DRAW = 0x88E4;
		public const GLuint GL_STATIC_READ = 0x88E5;
		public const GLuint GL_STATIC_COPY = 0x88E6;
		public const GLuint GL_DYNAMIC_DRAW = 0x88E8;
		public const GLuint GL_DYNAMIC_READ = 0x88E9;
		public const GLuint GL_DYNAMIC_COPY = 0x88EA;
		public const GLuint GL_SAMPLES_PASSED = 0x8914;
		public const GLuint GL_SRC1_ALPHA = 0x8589;
		public const GLuint GL_BLEND_EQUATION_RGB = 0x8009;
		public const GLuint GL_VERTEX_ATTRIB_ARRAY_ENABLED = 0x8622;
		public const GLuint GL_VERTEX_ATTRIB_ARRAY_SIZE = 0x8623;
		public const GLuint GL_VERTEX_ATTRIB_ARRAY_STRIDE = 0x8624;
		public const GLuint GL_VERTEX_ATTRIB_ARRAY_TYPE = 0x8625;
		public const GLuint GL_CURRENT_VERTEX_ATTRIB = 0x8626;
		public const GLuint GL_VERTEX_PROGRAM_POINT_SIZE = 0x8642;
		public const GLuint GL_VERTEX_ATTRIB_ARRAY_POINTER = 0x8645;
		public const GLuint GL_STENCIL_BACK_FUNC = 0x8800;
		public const GLuint GL_STENCIL_BACK_FAIL = 0x8801;
		public const GLuint GL_STENCIL_BACK_PASS_DEPTH_FAIL = 0x8802;
		public const GLuint GL_STENCIL_BACK_PASS_DEPTH_PASS = 0x8803;
		public const GLuint GL_MAX_DRAW_BUFFERS = 0x8824;
		public const GLuint GL_DRAW_BUFFER0 = 0x8825;
		public const GLuint GL_DRAW_BUFFER1 = 0x8826;
		public const GLuint GL_DRAW_BUFFER2 = 0x8827;
		public const GLuint GL_DRAW_BUFFER3 = 0x8828;
		public const GLuint GL_DRAW_BUFFER4 = 0x8829;
		public const GLuint GL_DRAW_BUFFER5 = 0x882A;
		public const GLuint GL_DRAW_BUFFER6 = 0x882B;
		public const GLuint GL_DRAW_BUFFER7 = 0x882C;
		public const GLuint GL_DRAW_BUFFER8 = 0x882D;
		public const GLuint GL_DRAW_BUFFER9 = 0x882E;
		public const GLuint GL_DRAW_BUFFER10 = 0x882F;
		public const GLuint GL_DRAW_BUFFER11 = 0x8830;
		public const GLuint GL_DRAW_BUFFER12 = 0x8831;
		public const GLuint GL_DRAW_BUFFER13 = 0x8832;
		public const GLuint GL_DRAW_BUFFER14 = 0x8833;
		public const GLuint GL_DRAW_BUFFER15 = 0x8834;
		public const GLuint GL_BLEND_EQUATION_ALPHA = 0x883D;
		public const GLuint GL_MAX_VERTEX_ATTRIBS = 0x8869;
		public const GLuint GL_VERTEX_ATTRIB_ARRAY_NORMALIZED = 0x886A;
		public const GLuint GL_MAX_TEXTURE_IMAGE_UNITS = 0x8872;
		public const GLuint GL_FRAGMENT_SHADER = 0x8B30;
		public const GLuint GL_VERTEX_SHADER = 0x8B31;
		public const GLuint GL_MAX_FRAGMENT_UNIFORM_COMPONENTS = 0x8B49;
		public const GLuint GL_MAX_VERTEX_UNIFORM_COMPONENTS = 0x8B4A;
		public const GLuint GL_MAX_VARYING_FLOATS = 0x8B4B;
		public const GLuint GL_MAX_VERTEX_TEXTURE_IMAGE_UNITS = 0x8B4C;
		public const GLuint GL_MAX_COMBINED_TEXTURE_IMAGE_UNITS = 0x8B4D;
		public const GLuint GL_SHADER_TYPE = 0x8B4F;
		public const GLuint GL_FLOAT_VEC2 = 0x8B50;
		public const GLuint GL_FLOAT_VEC3 = 0x8B51;
		public const GLuint GL_FLOAT_VEC4 = 0x8B52;
		public const GLuint GL_INT_VEC2 = 0x8B53;
		public const GLuint GL_INT_VEC3 = 0x8B54;
		public const GLuint GL_INT_VEC4 = 0x8B55;
		public const GLuint GL_BOOL = 0x8B56;
		public const GLuint GL_BOOL_VEC2 = 0x8B57;
		public const GLuint GL_BOOL_VEC3 = 0x8B58;
		public const GLuint GL_BOOL_VEC4 = 0x8B59;
		public const GLuint GL_FLOAT_MAT2 = 0x8B5A;
		public const GLuint GL_FLOAT_MAT3 = 0x8B5B;
		public const GLuint GL_FLOAT_MAT4 = 0x8B5C;
		public const GLuint GL_SAMPLER_1D = 0x8B5D;
		public const GLuint GL_SAMPLER_2D = 0x8B5E;
		public const GLuint GL_SAMPLER_3D = 0x8B5F;
		public const GLuint GL_SAMPLER_CUBE = 0x8B60;
		public const GLuint GL_SAMPLER_1D_SHADOW = 0x8B61;
		public const GLuint GL_SAMPLER_2D_SHADOW = 0x8B62;
		public const GLuint GL_DELETE_STATUS = 0x8B80;
		public const GLuint GL_COMPILE_STATUS = 0x8B81;
		public const GLuint GL_LINK_STATUS = 0x8B82;
		public const GLuint GL_VALIDATE_STATUS = 0x8B83;
		public const GLuint GL_INFO_LOG_LENGTH = 0x8B84;
		public const GLuint GL_ATTACHED_SHADERS = 0x8B85;
		public const GLuint GL_ACTIVE_UNIFORMS = 0x8B86;
		public const GLuint GL_ACTIVE_UNIFORM_MAX_LENGTH = 0x8B87;
		public const GLuint GL_SHADER_SOURCE_LENGTH = 0x8B88;
		public const GLuint GL_ACTIVE_ATTRIBUTES = 0x8B89;
		public const GLuint GL_ACTIVE_ATTRIBUTE_MAX_LENGTH = 0x8B8A;
		public const GLuint GL_SHADING_LANGUAGE_VERSION = 0x8B8C;
		public const GLuint GL_CURRENT_PROGRAM = 0x8B8D;
		public const GLuint GL_POINT_SPRITE_COORD_ORIGIN = 0x8CA0;
		public const GLuint GL_LOWER_LEFT = 0x8CA1;
		public const GLuint GL_UPPER_LEFT = 0x8CA2;
		public const GLuint GL_STENCIL_BACK_REF = 0x8CA3;
		public const GLuint GL_STENCIL_BACK_VALUE_MASK = 0x8CA4;
		public const GLuint GL_STENCIL_BACK_WRITEMASK = 0x8CA5;
		public const GLuint GL_PIXEL_PACK_BUFFER = 0x88EB;
		public const GLuint GL_PIXEL_UNPACK_BUFFER = 0x88EC;
		public const GLuint GL_PIXEL_PACK_BUFFER_BINDING = 0x88ED;
		public const GLuint GL_PIXEL_UNPACK_BUFFER_BINDING = 0x88EF;
		public const GLuint GL_FLOAT_MAT2x3 = 0x8B65;
		public const GLuint GL_FLOAT_MAT2x4 = 0x8B66;
		public const GLuint GL_FLOAT_MAT3x2 = 0x8B67;
		public const GLuint GL_FLOAT_MAT3x4 = 0x8B68;
		public const GLuint GL_FLOAT_MAT4x2 = 0x8B69;
		public const GLuint GL_FLOAT_MAT4x3 = 0x8B6A;
		public const GLuint GL_SRGB = 0x8C40;
		public const GLuint GL_SRGB8 = 0x8C41;
		public const GLuint GL_SRGB_ALPHA = 0x8C42;
		public const GLuint GL_SRGB8_ALPHA8 = 0x8C43;
		public const GLuint GL_COMPRESSED_SRGB = 0x8C48;
		public const GLuint GL_COMPRESSED_SRGB_ALPHA = 0x8C49;
		public const GLuint GL_COMPARE_REF_TO_TEXTURE = 0x884E;
		public const GLuint GL_MAJOR_VERSION = 0x821B;
		public const GLuint GL_MINOR_VERSION = 0x821C;
		public const GLuint GL_NUM_EXTENSIONS = 0x821D;
		public const GLuint GL_CONTEXT_FLAGS = 0x821E;
		public const GLuint GL_COMPRESSED_RED = 0x8225;
		public const GLuint GL_COMPRESSED_RG = 0x8226;
		public const GLuint GL_CONTEXT_FLAG_FORWARD_COMPATIBLE_BIT = 0x00000001;
		public const GLuint GL_RGBA32F = 0x8814;
		public const GLuint GL_RGB32F = 0x8815;
		public const GLuint GL_RGBA16F = 0x881A;
		public const GLuint GL_RGB16F = 0x881B;
		public const GLuint GL_VERTEX_ATTRIB_ARRAY_INTEGER = 0x88FD;
		public const GLuint GL_MAX_ARRAY_TEXTURE_LAYERS = 0x88FF;
		public const GLuint GL_MIN_PROGRAM_TEXEL_OFFSET = 0x8904;
		public const GLuint GL_MAX_PROGRAM_TEXEL_OFFSET = 0x8905;
		public const GLuint GL_CLAMP_READ_COLOR = 0x891C;
		public const GLuint GL_FIXED_ONLY = 0x891D;
		public const GLuint GL_MAX_VARYING_COMPONENTS = 0x8B4B;
		public const GLuint GL_TEXTURE_1D_ARRAY = 0x8C18;
		public const GLuint GL_PROXY_TEXTURE_1D_ARRAY = 0x8C19;
		public const GLuint GL_TEXTURE_2D_ARRAY = 0x8C1A;
		public const GLuint GL_PROXY_TEXTURE_2D_ARRAY = 0x8C1B;
		public const GLuint GL_TEXTURE_BINDING_1D_ARRAY = 0x8C1C;
		public const GLuint GL_TEXTURE_BINDING_2D_ARRAY = 0x8C1D;
		public const GLuint GL_R11F_G11F_B10F = 0x8C3A;
		public const GLuint GL_UNSIGNED_INT_10F_11F_11F_REV = 0x8C3B;
		public const GLuint GL_RGB9_E5 = 0x8C3D;
		public const GLuint GL_UNSIGNED_INT_5_9_9_9_REV = 0x8C3E;
		public const GLuint GL_TEXTURE_SHARED_SIZE = 0x8C3F;
		public const GLuint GL_TRANSFORM_FEEDBACK_VARYING_MAX_LENGTH = 0x8C76;
		public const GLuint GL_TRANSFORM_FEEDBACK_BUFFER_MODE = 0x8C7F;
		public const GLuint GL_MAX_TRANSFORM_FEEDBACK_SEPARATE_COMPONENTS = 0x8C80;
		public const GLuint GL_TRANSFORM_FEEDBACK_VARYINGS = 0x8C83;
		public const GLuint GL_TRANSFORM_FEEDBACK_BUFFER_START = 0x8C84;
		public const GLuint GL_TRANSFORM_FEEDBACK_BUFFER_SIZE = 0x8C85;
		public const GLuint GL_PRIMITIVES_GENERATED = 0x8C87;
		public const GLuint GL_TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN = 0x8C88;
		public const GLuint GL_RASTERIZER_DISCARD = 0x8C89;
		public const GLuint GL_MAX_TRANSFORM_FEEDBACK_INTERLEAVED_COMPONENTS = 0x8C8A;
		public const GLuint GL_MAX_TRANSFORM_FEEDBACK_SEPARATE_ATTRIBS = 0x8C8B;
		public const GLuint GL_INTERLEAVED_ATTRIBS = 0x8C8C;
		public const GLuint GL_SEPARATE_ATTRIBS = 0x8C8D;
		public const GLuint GL_TRANSFORM_FEEDBACK_BUFFER = 0x8C8E;
		public const GLuint GL_TRANSFORM_FEEDBACK_BUFFER_BINDING = 0x8C8F;
		public const GLuint GL_RGBA32UI = 0x8D70;
		public const GLuint GL_RGB32UI = 0x8D71;
		public const GLuint GL_RGBA16UI = 0x8D76;
		public const GLuint GL_RGB16UI = 0x8D77;
		public const GLuint GL_RGBA8UI = 0x8D7C;
		public const GLuint GL_RGB8UI = 0x8D7D;
		public const GLuint GL_RGBA32I = 0x8D82;
		public const GLuint GL_RGB32I = 0x8D83;
		public const GLuint GL_RGBA16I = 0x8D88;
		public const GLuint GL_RGB16I = 0x8D89;
		public const GLuint GL_RGBA8I = 0x8D8E;
		public const GLuint GL_RGB8I = 0x8D8F;
		public const GLuint GL_RED_INTEGER = 0x8D94;
		public const GLuint GL_GREEN_INTEGER = 0x8D95;
		public const GLuint GL_BLUE_INTEGER = 0x8D96;
		public const GLuint GL_RGB_INTEGER = 0x8D98;
		public const GLuint GL_RGBA_INTEGER = 0x8D99;
		public const GLuint GL_BGR_INTEGER = 0x8D9A;
		public const GLuint GL_BGRA_INTEGER = 0x8D9B;
		public const GLuint GL_SAMPLER_1D_ARRAY = 0x8DC0;
		public const GLuint GL_SAMPLER_2D_ARRAY = 0x8DC1;
		public const GLuint GL_SAMPLER_1D_ARRAY_SHADOW = 0x8DC3;
		public const GLuint GL_SAMPLER_2D_ARRAY_SHADOW = 0x8DC4;
		public const GLuint GL_SAMPLER_CUBE_SHADOW = 0x8DC5;
		public const GLuint GL_UNSIGNED_INT_VEC2 = 0x8DC6;
		public const GLuint GL_UNSIGNED_INT_VEC3 = 0x8DC7;
		public const GLuint GL_UNSIGNED_INT_VEC4 = 0x8DC8;
		public const GLuint GL_INT_SAMPLER_1D = 0x8DC9;
		public const GLuint GL_INT_SAMPLER_2D = 0x8DCA;
		public const GLuint GL_INT_SAMPLER_3D = 0x8DCB;
		public const GLuint GL_INT_SAMPLER_CUBE = 0x8DCC;
		public const GLuint GL_INT_SAMPLER_1D_ARRAY = 0x8DCE;
		public const GLuint GL_INT_SAMPLER_2D_ARRAY = 0x8DCF;
		public const GLuint GL_UNSIGNED_INT_SAMPLER_1D = 0x8DD1;
		public const GLuint GL_UNSIGNED_INT_SAMPLER_2D = 0x8DD2;
		public const GLuint GL_UNSIGNED_INT_SAMPLER_3D = 0x8DD3;
		public const GLuint GL_UNSIGNED_INT_SAMPLER_CUBE = 0x8DD4;
		public const GLuint GL_UNSIGNED_INT_SAMPLER_1D_ARRAY = 0x8DD6;
		public const GLuint GL_UNSIGNED_INT_SAMPLER_2D_ARRAY = 0x8DD7;
		public const GLuint GL_QUERY_WAIT = 0x8E13;
		public const GLuint GL_QUERY_NO_WAIT = 0x8E14;
		public const GLuint GL_QUERY_BY_REGION_WAIT = 0x8E15;
		public const GLuint GL_QUERY_BY_REGION_NO_WAIT = 0x8E16;
		public const GLuint GL_BUFFER_ACCESS_FLAGS = 0x911F;
		public const GLuint GL_BUFFER_MAP_LENGTH = 0x9120;
		public const GLuint GL_BUFFER_MAP_OFFSET = 0x9121;
		public const GLuint GL_DEPTH_COMPONENT32F = 0x8CAC;
		public const GLuint GL_DEPTH32F_STENCIL8 = 0x8CAD;
		public const GLuint GL_FLOAT_32_UNSIGNED_INT_24_8_REV = 0x8DAD;
		public const GLuint GL_INVALID_FRAMEBUFFER_OPERATION = 0x0506;
		public const GLuint GL_FRAMEBUFFER_ATTACHMENT_COLOR_ENCODING = 0x8210;
		public const GLuint GL_FRAMEBUFFER_ATTACHMENT_COMPONENT_TYPE = 0x8211;
		public const GLuint GL_FRAMEBUFFER_ATTACHMENT_RED_SIZE = 0x8212;
		public const GLuint GL_FRAMEBUFFER_ATTACHMENT_GREEN_SIZE = 0x8213;
		public const GLuint GL_FRAMEBUFFER_ATTACHMENT_BLUE_SIZE = 0x8214;
		public const GLuint GL_FRAMEBUFFER_ATTACHMENT_ALPHA_SIZE = 0x8215;
		public const GLuint GL_FRAMEBUFFER_ATTACHMENT_DEPTH_SIZE = 0x8216;
		public const GLuint GL_FRAMEBUFFER_ATTACHMENT_STENCIL_SIZE = 0x8217;
		public const GLuint GL_FRAMEBUFFER_DEFAULT = 0x8218;
		public const GLuint GL_FRAMEBUFFER_UNDEFINED = 0x8219;
		public const GLuint GL_DEPTH_STENCIL_ATTACHMENT = 0x821A;
		public const GLuint GL_MAX_RENDERBUFFER_SIZE = 0x84E8;
		public const GLuint GL_DEPTH_STENCIL = 0x84F9;
		public const GLuint GL_UNSIGNED_INT_24_8 = 0x84FA;
		public const GLuint GL_DEPTH24_STENCIL8 = 0x88F0;
		public const GLuint GL_TEXTURE_STENCIL_SIZE = 0x88F1;
		public const GLuint GL_TEXTURE_RED_TYPE = 0x8C10;
		public const GLuint GL_TEXTURE_GREEN_TYPE = 0x8C11;
		public const GLuint GL_TEXTURE_BLUE_TYPE = 0x8C12;
		public const GLuint GL_TEXTURE_ALPHA_TYPE = 0x8C13;
		public const GLuint GL_TEXTURE_DEPTH_TYPE = 0x8C16;
		public const GLuint GL_UNSIGNED_NORMALIZED = 0x8C17;
		public const GLuint GL_FRAMEBUFFER_BINDING = 0x8CA6;
		public const GLuint GL_DRAW_FRAMEBUFFER_BINDING = 0x8CA6;
		public const GLuint GL_RENDERBUFFER_BINDING = 0x8CA7;
		public const GLuint GL_READ_FRAMEBUFFER = 0x8CA8;
		public const GLuint GL_DRAW_FRAMEBUFFER = 0x8CA9;
		public const GLuint GL_READ_FRAMEBUFFER_BINDING = 0x8CAA;
		public const GLuint GL_RENDERBUFFER_SAMPLES = 0x8CAB;
		public const GLuint GL_FRAMEBUFFER_ATTACHMENT_OBJECT_TYPE = 0x8CD0;
		public const GLuint GL_FRAMEBUFFER_ATTACHMENT_OBJECT_NAME = 0x8CD1;
		public const GLuint GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_LEVEL = 0x8CD2;
		public const GLuint GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_CUBE_MAP_FACE = 0x8CD3;
		public const GLuint GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_LAYER = 0x8CD4;
		public const GLuint GL_FRAMEBUFFER_COMPLETE = 0x8CD5;
		public const GLuint GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT = 0x8CD6;
		public const GLuint GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT = 0x8CD7;
		public const GLuint GL_FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER = 0x8CDB;
		public const GLuint GL_FRAMEBUFFER_INCOMPLETE_READ_BUFFER = 0x8CDC;
		public const GLuint GL_FRAMEBUFFER_UNSUPPORTED = 0x8CDD;
		public const GLuint GL_MAX_COLOR_ATTACHMENTS = 0x8CDF;
		public const GLuint GL_COLOR_ATTACHMENT0 = 0x8CE0;
		public const GLuint GL_COLOR_ATTACHMENT1 = 0x8CE1;
		public const GLuint GL_COLOR_ATTACHMENT2 = 0x8CE2;
		public const GLuint GL_COLOR_ATTACHMENT3 = 0x8CE3;
		public const GLuint GL_COLOR_ATTACHMENT4 = 0x8CE4;
		public const GLuint GL_COLOR_ATTACHMENT5 = 0x8CE5;
		public const GLuint GL_COLOR_ATTACHMENT6 = 0x8CE6;
		public const GLuint GL_COLOR_ATTACHMENT7 = 0x8CE7;
		public const GLuint GL_COLOR_ATTACHMENT8 = 0x8CE8;
		public const GLuint GL_COLOR_ATTACHMENT9 = 0x8CE9;
		public const GLuint GL_COLOR_ATTACHMENT10 = 0x8CEA;
		public const GLuint GL_COLOR_ATTACHMENT11 = 0x8CEB;
		public const GLuint GL_COLOR_ATTACHMENT12 = 0x8CEC;
		public const GLuint GL_COLOR_ATTACHMENT13 = 0x8CED;
		public const GLuint GL_COLOR_ATTACHMENT14 = 0x8CEE;
		public const GLuint GL_COLOR_ATTACHMENT15 = 0x8CEF;
		public const GLuint GL_DEPTH_ATTACHMENT = 0x8D00;
		public const GLuint GL_STENCIL_ATTACHMENT = 0x8D20;
		public const GLuint GL_FRAMEBUFFER = 0x8D40;
		public const GLuint GL_RENDERBUFFER = 0x8D41;
		public const GLuint GL_RENDERBUFFER_WIDTH = 0x8D42;
		public const GLuint GL_RENDERBUFFER_HEIGHT = 0x8D43;
		public const GLuint GL_RENDERBUFFER_INTERNAL_FORMAT = 0x8D44;
		public const GLuint GL_STENCIL_INDEX1 = 0x8D46;
		public const GLuint GL_STENCIL_INDEX4 = 0x8D47;
		public const GLuint GL_STENCIL_INDEX8 = 0x8D48;
		public const GLuint GL_STENCIL_INDEX16 = 0x8D49;
		public const GLuint GL_RENDERBUFFER_RED_SIZE = 0x8D50;
		public const GLuint GL_RENDERBUFFER_GREEN_SIZE = 0x8D51;
		public const GLuint GL_RENDERBUFFER_BLUE_SIZE = 0x8D52;
		public const GLuint GL_RENDERBUFFER_ALPHA_SIZE = 0x8D53;
		public const GLuint GL_RENDERBUFFER_DEPTH_SIZE = 0x8D54;
		public const GLuint GL_RENDERBUFFER_STENCIL_SIZE = 0x8D55;
		public const GLuint GL_FRAMEBUFFER_INCOMPLETE_MULTISAMPLE = 0x8D56;
		public const GLuint GL_MAX_SAMPLES = 0x8D57;
		public const GLuint GL_INDEX = 0x8222;
		public const GLuint GL_FRAMEBUFFER_SRGB = 0x8DB9;
		public const GLuint GL_HALF_FLOAT = 0x140B;
		public const GLuint GL_MAP_READ_BIT = 0x0001;
		public const GLuint GL_MAP_WRITE_BIT = 0x0002;
		public const GLuint GL_MAP_INVALIDATE_RANGE_BIT = 0x0004;
		public const GLuint GL_MAP_INVALIDATE_BUFFER_BIT = 0x0008;
		public const GLuint GL_MAP_FLUSH_EXPLICIT_BIT = 0x0010;
		public const GLuint GL_MAP_UNSYNCHRONIZED_BIT = 0x0020;
		public const GLuint GL_COMPRESSED_RED_RGTC1 = 0x8DBB;
		public const GLuint GL_COMPRESSED_SIGNED_RED_RGTC1 = 0x8DBC;
		public const GLuint GL_COMPRESSED_RG_RGTC2 = 0x8DBD;
		public const GLuint GL_COMPRESSED_SIGNED_RG_RGTC2 = 0x8DBE;
		public const GLuint GL_RG = 0x8227;
		public const GLuint GL_RG_INTEGER = 0x8228;
		public const GLuint GL_R8 = 0x8229;
		public const GLuint GL_R16 = 0x822A;
		public const GLuint GL_RG8 = 0x822B;
		public const GLuint GL_RG16 = 0x822C;
		public const GLuint GL_R16F = 0x822D;
		public const GLuint GL_R32F = 0x822E;
		public const GLuint GL_RG16F = 0x822F;
		public const GLuint GL_RG32F = 0x8230;
		public const GLuint GL_R8I = 0x8231;
		public const GLuint GL_R8UI = 0x8232;
		public const GLuint GL_R16I = 0x8233;
		public const GLuint GL_R16UI = 0x8234;
		public const GLuint GL_R32I = 0x8235;
		public const GLuint GL_R32UI = 0x8236;
		public const GLuint GL_RG8I = 0x8237;
		public const GLuint GL_RG8UI = 0x8238;
		public const GLuint GL_RG16I = 0x8239;
		public const GLuint GL_RG16UI = 0x823A;
		public const GLuint GL_RG32I = 0x823B;
		public const GLuint GL_RG32UI = 0x823C;
		public const GLuint GL_VERTEX_ARRAY_BINDING = 0x85B5;
		public const GLuint GL_SAMPLER_2D_RECT = 0x8B63;
		public const GLuint GL_SAMPLER_2D_RECT_SHADOW = 0x8B64;
		public const GLuint GL_SAMPLER_BUFFER = 0x8DC2;
		public const GLuint GL_INT_SAMPLER_2D_RECT = 0x8DCD;
		public const GLuint GL_INT_SAMPLER_BUFFER = 0x8DD0;
		public const GLuint GL_UNSIGNED_INT_SAMPLER_2D_RECT = 0x8DD5;
		public const GLuint GL_UNSIGNED_INT_SAMPLER_BUFFER = 0x8DD8;
		public const GLuint GL_TEXTURE_BUFFER = 0x8C2A;
		public const GLuint GL_MAX_TEXTURE_BUFFER_SIZE = 0x8C2B;
		public const GLuint GL_TEXTURE_BINDING_BUFFER = 0x8C2C;
		public const GLuint GL_TEXTURE_BUFFER_DATA_STORE_BINDING = 0x8C2D;
		public const GLuint GL_TEXTURE_RECTANGLE = 0x84F5;
		public const GLuint GL_TEXTURE_BINDING_RECTANGLE = 0x84F6;
		public const GLuint GL_PROXY_TEXTURE_RECTANGLE = 0x84F7;
		public const GLuint GL_MAX_RECTANGLE_TEXTURE_SIZE = 0x84F8;
		public const GLuint GL_R8_SNORM = 0x8F94;
		public const GLuint GL_RG8_SNORM = 0x8F95;
		public const GLuint GL_RGB8_SNORM = 0x8F96;
		public const GLuint GL_RGBA8_SNORM = 0x8F97;
		public const GLuint GL_R16_SNORM = 0x8F98;
		public const GLuint GL_RG16_SNORM = 0x8F99;
		public const GLuint GL_RGB16_SNORM = 0x8F9A;
		public const GLuint GL_RGBA16_SNORM = 0x8F9B;
		public const GLuint GL_SIGNED_NORMALIZED = 0x8F9C;
		public const GLuint GL_PRIMITIVE_RESTART = 0x8F9D;
		public const GLuint GL_PRIMITIVE_RESTART_INDEX = 0x8F9E;
		public const GLuint GL_COPY_READ_BUFFER = 0x8F36;
		public const GLuint GL_COPY_WRITE_BUFFER = 0x8F37;
		public const GLuint GL_UNIFORM_BUFFER = 0x8A11;
		public const GLuint GL_UNIFORM_BUFFER_BINDING = 0x8A28;
		public const GLuint GL_UNIFORM_BUFFER_START = 0x8A29;
		public const GLuint GL_UNIFORM_BUFFER_SIZE = 0x8A2A;
		public const GLuint GL_MAX_VERTEX_UNIFORM_BLOCKS = 0x8A2B;
		public const GLuint GL_MAX_FRAGMENT_UNIFORM_BLOCKS = 0x8A2D;
		public const GLuint GL_MAX_COMBINED_UNIFORM_BLOCKS = 0x8A2E;
		public const GLuint GL_MAX_UNIFORM_BUFFER_BINDINGS = 0x8A2F;
		public const GLuint GL_MAX_UNIFORM_BLOCK_SIZE = 0x8A30;
		public const GLuint GL_MAX_COMBINED_VERTEX_UNIFORM_COMPONENTS = 0x8A31;
		public const GLuint GL_MAX_COMBINED_FRAGMENT_UNIFORM_COMPONENTS = 0x8A33;
		public const GLuint GL_UNIFORM_BUFFER_OFFSET_ALIGNMENT = 0x8A34;
		public const GLuint GL_ACTIVE_UNIFORM_BLOCK_MAX_NAME_LENGTH = 0x8A35;
		public const GLuint GL_ACTIVE_UNIFORM_BLOCKS = 0x8A36;
		public const GLuint GL_UNIFORM_TYPE = 0x8A37;
		public const GLuint GL_UNIFORM_SIZE = 0x8A38;
		public const GLuint GL_UNIFORM_NAME_LENGTH = 0x8A39;
		public const GLuint GL_UNIFORM_BLOCK_INDEX = 0x8A3A;
		public const GLuint GL_UNIFORM_OFFSET = 0x8A3B;
		public const GLuint GL_UNIFORM_ARRAY_STRIDE = 0x8A3C;
		public const GLuint GL_UNIFORM_MATRIX_STRIDE = 0x8A3D;
		public const GLuint GL_UNIFORM_IS_ROW_MAJOR = 0x8A3E;
		public const GLuint GL_UNIFORM_BLOCK_BINDING = 0x8A3F;
		public const GLuint GL_UNIFORM_BLOCK_DATA_SIZE = 0x8A40;
		public const GLuint GL_UNIFORM_BLOCK_NAME_LENGTH = 0x8A41;
		public const GLuint GL_UNIFORM_BLOCK_ACTIVE_UNIFORMS = 0x8A42;
		public const GLuint GL_UNIFORM_BLOCK_ACTIVE_UNIFORM_INDICES = 0x8A43;
		public const GLuint GL_UNIFORM_BLOCK_REFERENCED_BY_VERTEX_SHADER = 0x8A44;
		public const GLuint GL_UNIFORM_BLOCK_REFERENCED_BY_FRAGMENT_SHADER = 0x8A46;
		public const GLuint GL_INVALID_INDEX = 0xFFFFFFFF;
		public const GLuint GL_CONTEXT_CORE_PROFILE_BIT = 0x00000001;
		public const GLuint GL_CONTEXT_COMPATIBILITY_PROFILE_BIT = 0x00000002;
		public const GLuint GL_PROGRAM_POINT_SIZE = 0x8642;
		public const GLuint GL_MAX_GEOMETRY_TEXTURE_IMAGE_UNITS = 0x8C29;
		public const GLuint GL_FRAMEBUFFER_ATTACHMENT_LAYERED = 0x8DA7;
		public const GLuint GL_FRAMEBUFFER_INCOMPLETE_LAYER_TARGETS = 0x8DA8;
		public const GLuint GL_GEOMETRY_SHADER = 0x8DD9;
		public const GLuint GL_GEOMETRY_VERTICES_OUT = 0x8916;
		public const GLuint GL_GEOMETRY_INPUT_TYPE = 0x8917;
		public const GLuint GL_GEOMETRY_OUTPUT_TYPE = 0x8918;
		public const GLuint GL_MAX_GEOMETRY_UNIFORM_COMPONENTS = 0x8DDF;
		public const GLuint GL_MAX_GEOMETRY_OUTPUT_VERTICES = 0x8DE0;
		public const GLuint GL_MAX_GEOMETRY_TOTAL_OUTPUT_COMPONENTS = 0x8DE1;
		public const GLuint GL_MAX_VERTEX_OUTPUT_COMPONENTS = 0x9122;
		public const GLuint GL_MAX_GEOMETRY_INPUT_COMPONENTS = 0x9123;
		public const GLuint GL_MAX_GEOMETRY_OUTPUT_COMPONENTS = 0x9124;
		public const GLuint GL_MAX_FRAGMENT_INPUT_COMPONENTS = 0x9125;
		public const GLuint GL_CONTEXT_PROFILE_MASK = 0x9126;
		public const GLuint GL_DEPTH_CLAMP = 0x864F;
		public const GLuint GL_QUADS_FOLLOW_PROVOKING_VERTEX_CONVENTION = 0x8E4C;
		public const GLuint GL_FIRST_VERTEX_CONVENTION = 0x8E4D;
		public const GLuint GL_LAST_VERTEX_CONVENTION = 0x8E4E;
		public const GLuint GL_PROVOKING_VERTEX = 0x8E4F;
		public const GLuint GL_TEXTURE_CUBE_MAP_SEAMLESS = 0x884F;
		public const GLuint GL_MAX_SERVER_WAIT_TIMEOUT = 0x9111;
		public const GLuint GL_OBJECT_TYPE = 0x9112;
		public const GLuint GL_SYNC_CONDITION = 0x9113;
		public const GLuint GL_SYNC_STATUS = 0x9114;
		public const GLuint GL_SYNC_FLAGS = 0x9115;
		public const GLuint GL_SYNC_FENCE = 0x9116;
		public const GLuint GL_SYNC_GPU_COMMANDS_COMPLETE = 0x9117;
		public const GLuint GL_UNSIGNALED = 0x9118;
		public const GLuint GL_SIGNALED = 0x9119;
		public const GLuint GL_ALREADY_SIGNALED = 0x911A;
		public const GLuint GL_TIMEOUT_EXPIRED = 0x911B;
		public const GLuint GL_CONDITION_SATISFIED = 0x911C;
		public const GLuint GL_WAIT_FAILED = 0x911D;
		public const GLuint64 GL_TIMEOUT_IGNORED = 0xFFFFFFFFFFFFFFFF;
		public const GLuint GL_SYNC_FLUSH_COMMANDS_BIT = 0x00000001;
		public const GLuint GL_SAMPLE_POSITION = 0x8E50;
		public const GLuint GL_SAMPLE_MASK = 0x8E51;
		public const GLuint GL_SAMPLE_MASK_VALUE = 0x8E52;
		public const GLuint GL_MAX_SAMPLE_MASK_WORDS = 0x8E59;
		public const GLuint GL_TEXTURE_2D_MULTISAMPLE = 0x9100;
		public const GLuint GL_PROXY_TEXTURE_2D_MULTISAMPLE = 0x9101;
		public const GLuint GL_TEXTURE_2D_MULTISAMPLE_ARRAY = 0x9102;
		public const GLuint GL_PROXY_TEXTURE_2D_MULTISAMPLE_ARRAY = 0x9103;
		public const GLuint GL_TEXTURE_BINDING_2D_MULTISAMPLE = 0x9104;
		public const GLuint GL_TEXTURE_BINDING_2D_MULTISAMPLE_ARRAY = 0x9105;
		public const GLuint GL_TEXTURE_SAMPLES = 0x9106;
		public const GLuint GL_TEXTURE_FIXED_SAMPLE_LOCATIONS = 0x9107;
		public const GLuint GL_SAMPLER_2D_MULTISAMPLE = 0x9108;
		public const GLuint GL_INT_SAMPLER_2D_MULTISAMPLE = 0x9109;
		public const GLuint GL_UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE = 0x910A;
		public const GLuint GL_SAMPLER_2D_MULTISAMPLE_ARRAY = 0x910B;
		public const GLuint GL_INT_SAMPLER_2D_MULTISAMPLE_ARRAY = 0x910C;
		public const GLuint GL_UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE_ARRAY = 0x910D;
		public const GLuint GL_MAX_COLOR_TEXTURE_SAMPLES = 0x910E;
		public const GLuint GL_MAX_DEPTH_TEXTURE_SAMPLES = 0x910F;
		public const GLuint GL_MAX_INTEGER_SAMPLES = 0x9110;
		public const GLuint GL_VERTEX_ATTRIB_ARRAY_DIVISOR = 0x88FE;
		public const GLuint GL_SRC1_COLOR = 0x88F9;
		public const GLuint GL_ONE_MINUS_SRC1_COLOR = 0x88FA;
		public const GLuint GL_ONE_MINUS_SRC1_ALPHA = 0x88FB;
		public const GLuint GL_MAX_DUAL_SOURCE_DRAW_BUFFERS = 0x88FC;
		public const GLuint GL_ANY_SAMPLES_PASSED = 0x8C2F;
		public const GLuint GL_SAMPLER_BINDING = 0x8919;
		public const GLuint GL_RGB10_A2UI = 0x906F;
		public const GLuint GL_TEXTURE_SWIZZLE_R = 0x8E42;
		public const GLuint GL_TEXTURE_SWIZZLE_G = 0x8E43;
		public const GLuint GL_TEXTURE_SWIZZLE_B = 0x8E44;
		public const GLuint GL_TEXTURE_SWIZZLE_A = 0x8E45;
		public const GLuint GL_TEXTURE_SWIZZLE_RGBA = 0x8E46;
		public const GLuint GL_TIME_ELAPSED = 0x88BF;
		public const GLuint GL_TIMESTAMP = 0x8E28;
		public const GLuint GL_INT_2_10_10_10_REV = 0x8D9F;
		// Source Enum Values
		public const GLuint GL_DEBUG_SOURCE_API = 0x8246;
		public const GLuint GL_DEBUG_SOURCE_WINDOW_SYSTEM = 0x8247;
		public const GLuint GL_DEBUG_SOURCE_SHADER_COMPILER = 0x8248;
		public const GLuint GL_DEBUG_SOURCE_THIRD_PARTY = 0x8249;
		public const GLuint GL_DEBUG_SOURCE_APPLICATION = 0x824A;
		public const GLuint GL_DEBUG_SOURCE_OTHER = 0x824B;
		// Type Enum Values
		public const GLuint GL_DEBUG_TYPE_ERROR = 0x824C;
		public const GLuint GL_DEBUG_TYPE_DEPRECATED_BEHAVIOR = 0x824D;
		public const GLuint GL_DEBUG_TYPE_UNDEFINED_BEHAVIOR = 0x824E;
		public const GLuint GL_DEBUG_TYPE_PORTABILITY = 0x824F;
		public const GLuint GL_DEBUG_TYPE_PERFORMANCE = 0x8250;
		public const GLuint GL_DEBUG_TYPE_OTHER = 0x8251;
		// Severity Enum Values
		public const GLuint GL_DEBUG_SEVERITY_HIGH = 0x9146;
		public const GLuint GL_DEBUG_SEVERITY_MEDIUM = 0x9147;
		public const GLuint GL_DEBUG_SEVERITY_LOW = 0x9148;
		public const GLuint GL_DEBUG_SEVERITY_NOTIFICATION = 0x826B;
	}
	#endregion

	public enum ErrorCode : GLuint
	{
		NoError = GL_NO_ERROR,
		InvalidEnum = GL_INVALID_ENUM,
		InvalidValue = GL_INVALID_VALUE,
		InvalidOperation = GL_INVALID_OPERATION,
		InvalidFramebufferOperation = GL_INVALID_FRAMEBUFFER_OPERATION,
		OutOfMemory = GL_OUT_OF_MEMORY
	}

	public enum Strings : GLuint
	{
		Vendor = GL_VENDOR,
		Renderer = GL_RENDERER,
		Version = GL_VERSION,
		GLSLVersion = GL_SHADING_LANGUAGE_VERSION,
		Extensions = GL_EXTENSIONS
	}

	public enum Integers : GLuint
	{
		MajorVersion = GL_MAJOR_VERSION,
		MinorVersion = GL_MINOR_VERSION,
		MaxColourAttachments = GL_MAX_COLOR_ATTACHMENTS,
		MaxCubeMapTextureSize = GL_MAX_CUBE_MAP_TEXTURE_SIZE,
		MaxDrawBuffers = GL_MAX_DRAW_BUFFERS,
		MaxElementIndices = GL_MAX_ELEMENTS_INDICES,
		MaxElementVertices = GL_MAX_ELEMENTS_VERTICES,
		MaxRenderbufferSize = GL_MAX_RENDERBUFFER_SIZE,
		MaxSamples = GL_MAX_SAMPLES,
		MaxTextureImageUnits = GL_MAX_TEXTURE_IMAGE_UNITS,
		MaxTextureSize = GL_MAX_TEXTURE_SIZE
	}

	public enum EnableCap : GLuint
	{
		Blend = GL_BLEND,
		CullFace = GL_CULL_FACE,
		DepthTest = GL_DEPTH_TEST,
		Dither = GL_DITHER,
		PolygonOffsetFill = GL_POLYGON_OFFSET_FILL,
		RasterizerDiscard = GL_RASTERIZER_DISCARD,
		Multisample = GL_MULTISAMPLE,
		SampleAlphaToCoverage = GL_SAMPLE_ALPHA_TO_COVERAGE,
		SampleCoverage = GL_SAMPLE_COVERAGE,
		ScissorTest = GL_SCISSOR_TEST,
		StencilTest = GL_STENCIL_TEST,
		LineSmooth = GL_LINE_SMOOTH
	}

	public enum BlendEquation : GLuint
	{
		Add = GL_FUNC_ADD,
		Subtract = GL_FUNC_SUBTRACT,
		ReverseSubtract = GL_FUNC_REVERSE_SUBTRACT,
		Min = GL_MIN,
		Max = GL_MAX
	}

	public enum BlendFactor : GLuint
	{
		Zero = GL_ZERO,
		One = GL_ONE,
		SrcColour = GL_SRC_COLOR,
		OneMinusSrcColour = GL_ONE_MINUS_SRC_COLOR,
		SrcAlpha = GL_SRC_ALPHA,
		OneMinusSrcAlpha = GL_ONE_MINUS_SRC_ALPHA,
		DstAlpha = GL_DST_ALPHA,
		OneMinusDstAlpha = GL_ONE_MINUS_DST_ALPHA,
		DstColour = GL_DST_COLOR,
		OneMinusDstcolour = GL_ONE_MINUS_DST_COLOR,
		SrcAlphaSaturate = GL_SRC_ALPHA_SATURATE,
		ConstantColour = GL_CONSTANT_COLOR,
		OneMinusConstantColour = GL_ONE_MINUS_CONSTANT_COLOR,
		ConstantAlpha = GL_CONSTANT_ALPHA,
		OneMinusConstantAlpha = GL_ONE_MINUS_CONSTANT_ALPHA
	}

	public enum DepthFunc : GLuint
	{
		Never = GL_NEVER,
		Less = GL_LESS,
		Equal = GL_EQUAL,
		LessEqual = GL_LEQUAL,
		Greater = GL_GREATER,
		NotEqual = GL_NOTEQUAL,
		GreaterEqual = GL_GEQUAL,
		Always = GL_ALWAYS
	}

	public enum StencilFunc : GLuint
	{
		Never = GL_NEVER,
		Less = GL_LESS,
		Equal = GL_EQUAL,
		LessEqual = GL_LEQUAL,
		Greater = GL_GREATER,
		NotEqual = GL_NOTEQUAL,
		GreaterEqual = GL_GEQUAL,
		Always = GL_ALWAYS
	}

	public enum StencilOp : GLuint
	{
		Keep = GL_KEEP,
		Zero = GL_ZERO,
		Replace = GL_REPLACE,
		Increment = GL_INCR,
		IncrementWrap = GL_INCR_WRAP,
		Decrement = GL_DECR,
		DecrementWrap = GL_DECR_WRAP,
		Invert = GL_INVERT
	}

	public enum Face : GLuint
	{
		Front = GL_FRONT,
		Back = GL_BACK,
		FrontAndBack = GL_FRONT_AND_BACK
	}

	public enum FrontFace : GLuint
	{
		Clockwise = GL_CW,
		CounterClockwise = GL_CCW
	}

	public enum PolygonMode : GLuint
	{
		Fill = GL_FILL,
		Line = GL_LINE,
		Point = GL_POINT
	}

	[Flags]
	public enum BufferBit : GLuint
	{
		Colour = GL_COLOR_BUFFER_BIT,
		Depth = GL_DEPTH_BUFFER_BIT,
		Stencil = GL_STENCIL_BUFFER_BIT
	}

	public enum TextureTarget : GLuint
	{
		Texture1D = GL_TEXTURE_1D, // Not available in ES
		Texture2D = GL_TEXTURE_2D,
		Texture2DMultisample = GL_TEXTURE_2D_MULTISAMPLE,
		Texture3D = GL_TEXTURE_3D,
		Texture1DArray = GL_TEXTURE_1D_ARRAY, // Not available in ES
		Texture2DArray = GL_TEXTURE_2D_ARRAY,
		TextureCubeMap = GL_TEXTURE_CUBE_MAP,
		TextureCubeMapPosX = GL_TEXTURE_CUBE_MAP_POSITIVE_X,
		TextureCubeMapNegX = GL_TEXTURE_CUBE_MAP_NEGATIVE_X,
		TextureCubeMapPosY = GL_TEXTURE_CUBE_MAP_POSITIVE_Y,
		TextureCubeMapNegY = GL_TEXTURE_CUBE_MAP_NEGATIVE_Y,
		TextureCubeMapPosZ = GL_TEXTURE_CUBE_MAP_POSITIVE_Z,
		TextureCubeMapNegZ = GL_TEXTURE_CUBE_MAP_NEGATIVE_Z
	}

	public enum TextureFormat : GLuint
	{
		// Red textures
		R = GL_RED,
		R8 = GL_R8,
		R8SNorm = GL_R8_SNORM,
		R16F = GL_R16F,
		R32F = GL_R32F,
		R8I = GL_R8I,
		R8UI = GL_R8UI,
		R16I = GL_R16I,
		R16UI = GL_R16UI,
		R32I = GL_R32I,
		R32UI = GL_R32UI,

		// RG textures
		RG = GL_RG,
		RG8 = GL_RG8,
		RG8SNorm = GL_RG8_SNORM,
		RG16F = GL_RG16F,
		RG32F = GL_RG32F,
		RG8I = GL_RG8I,
		RG8UI = GL_RG8UI,
		RG16I = GL_RG16I,
		RG16UI = GL_RG16UI,
		RG32I = GL_RG32I,
		RG32UI = GL_RG32UI,

		// RGB textures
		RGB = GL_RGB,
		RGB8 = GL_RGB8,
		RGB8SNorm = GL_RGB8_SNORM,
		RGB16F = GL_RGB16F,
		RGB32F = GL_RGB32F,
		RGB8I = GL_RGB8I,
		RGB8UI = GL_RGB8UI,
		RGB16I = GL_RGB16I,
		RGB16UI = GL_RGB16UI,
		RGB32I = GL_RGB32I,
		RGB32UI = GL_RGB32UI,
		R3G3B2 = GL_R3_G3_B2,
		R5G6B5 = GL_RGB565,
		R11G11B10F = GL_R11F_G11F_B10F,

		// RGBA textures
		RGBA = GL_RGBA,
		RGBA8 = GL_RGBA8,
		RGBA8SNorm = GL_RGBA8_SNORM,
		RGBA16F = GL_RGBA16F,
		RGBA32F = GL_RGBA32F,
		RGBA8I = GL_RGBA8I,
		RGBA8UI = GL_RGBA8UI,
		RGBA16I = GL_RGBA16I,
		RGBA16UI = GL_RGBA16UI,
		RGBA32I = GL_RGBA32I,
		RGBA32UI = GL_RGBA32UI,
		RGBA2 = GL_RGBA2,
		RGBA4 = GL_RGBA4,
		RGB5A1 = GL_RGB5_A1,
		RGB10A2 = GL_RGB10_A2,
		RGB10A2UI = GL_RGB10_A2UI,

		// Depth textures
		Depth = GL_DEPTH_COMPONENT,
		Depth16 = GL_DEPTH_COMPONENT16,
		Depth24 = GL_DEPTH_COMPONENT24,
		Depth32 = GL_DEPTH_COMPONENT32,
		Depth32F = GL_DEPTH_COMPONENT32F,

		// Depth/Stencil textures
		DepthStencil = GL_DEPTH_STENCIL,
		Depth24Stencil8 = GL_DEPTH24_STENCIL8,
		Depth32FStencil8 = GL_DEPTH32F_STENCIL8
	}
	#region TextureFormat Extensions
	public static class TextureFormatExt
	{
		public static PixelFormat PixelFormat(this TextureFormat textureFormat)
		{
			switch (textureFormat)
			{
				//case TextureFormat.R:
				case TextureFormat.R8:
					//case TextureFormat.R8SNorm:
					//case TextureFormat.R8I:
					//case TextureFormat.R8UI:
					//case TextureFormat.R16I:
					//case TextureFormat.R16UI:
					//case TextureFormat.R16F:
					//case TextureFormat.R32I:
					//case TextureFormat.R32UI:
					//case TextureFormat.R32F:
					return OpenGLBindings.PixelFormat.R;
				//case TextureFormat.RG:
				case TextureFormat.RG8:
					//case TextureFormat.RG8SNorm:
					//case TextureFormat.RG8I:
					//case TextureFormat.RG8UI:
					//case TextureFormat.RG16I:
					//case TextureFormat.RG16UI:
					//case TextureFormat.RG16F:
					//case TextureFormat.RG32I:
					//case TextureFormat.RG32UI:
					//case TextureFormat.RG32F:
					return OpenGLBindings.PixelFormat.RG;
				//case TextureFormat.RGB:
				case TextureFormat.RGB8:
					//case TextureFormat.RGB8SNorm:
					//case TextureFormat.RGB8I:
					//case TextureFormat.RGB8UI:
					//case TextureFormat.RGB16I:
					//case TextureFormat.RGB16UI:
					//case TextureFormat.RGB16F:
					//case TextureFormat.RGB32I:
					//case TextureFormat.RGB32UI:
					//case TextureFormat.RGB32F:
					//case TextureFormat.R3G3B2:
					//case TextureFormat.R5G6B5:
					//case TextureFormat.R11G11B10F:
					return OpenGLBindings.PixelFormat.RGB;
				//case TextureFormat.RGBA:
				case TextureFormat.RGBA8:
					//case TextureFormat.RGBA8SNorm:
					//case TextureFormat.RGBA16F:
					//case TextureFormat.RGBA32F:
					//case TextureFormat.RGBA8I:
					//case TextureFormat.RGBA8UI:
					//case TextureFormat.RGBA16I:
					//case TextureFormat.RGBA16UI:
					//case TextureFormat.RGBA32I:
					//case TextureFormat.RGBA32UI:
					//case TextureFormat.RGBA2:
					//case TextureFormat.RGBA4:
					//case TextureFormat.RGB5A1:
					//case TextureFormat.RGB10A2:
					//case TextureFormat.RGB10A2UI:
					return OpenGLBindings.PixelFormat.RGBA;
				//case TextureFormat.Depth:
				case TextureFormat.Depth16:
				case TextureFormat.Depth24:
				//case TextureFormat.Depth32:
				case TextureFormat.Depth32F:
					return OpenGLBindings.PixelFormat.Depth;
				//case TextureFormat.DepthStencil:
				case TextureFormat.Depth24Stencil8:
				case TextureFormat.Depth32FStencil8:
					return OpenGLBindings.PixelFormat.DepthStencil;
				default:
					throw new IllegalValueException(typeof(TextureFormat), textureFormat);
			}
		}

		public static PixelType PixelType(this TextureFormat textureFormat)
		{
			switch (textureFormat)
			{
				case TextureFormat.R8:
					return OpenGLBindings.PixelType.UnsignedByte;
				case TextureFormat.RG8:
					return OpenGLBindings.PixelType.UnsignedByte;
				case TextureFormat.RGB8:
					return OpenGLBindings.PixelType.UnsignedByte;
				case TextureFormat.RGBA8:
					return OpenGLBindings.PixelType.UnsignedByte;
				case TextureFormat.Depth16:
					return OpenGLBindings.PixelType.UnsignedShort;
				case TextureFormat.Depth24:
					return OpenGLBindings.PixelType.UnsignedInt;
				case TextureFormat.Depth32F:
					return OpenGLBindings.PixelType.Float;
				case TextureFormat.Depth24Stencil8:
					return OpenGLBindings.PixelType.UnsignedInt_24_8;
				case TextureFormat.Depth32FStencil8:
					return OpenGLBindings.PixelType.Float_32_UnsignedInt_24_8_REV;
				default:
					throw new IllegalValueException(typeof(TextureFormat), textureFormat);
			}
		}

		public static TextureAttachment TextureAttachment(this TextureFormat textureFormat)
		{
			switch (textureFormat)
			{
				//case TextureFormat.Depth:
				case TextureFormat.Depth16:
				case TextureFormat.Depth24:
				//case TextureFormat.Depth32:
				case TextureFormat.Depth32F:
					return OpenGLBindings.TextureAttachment.Depth;
				//case TextureFormat.DepthStencil:
				case TextureFormat.Depth24Stencil8:
				case TextureFormat.Depth32FStencil8:
					return OpenGLBindings.TextureAttachment.DepthStencil;
				default:
					throw new IllegalValueException(typeof(TextureFormat), textureFormat);
			}
		}
	}
	#endregion

	public enum PixelFormat : GLuint
	{
		R = GL_RED,
		RG = GL_RG,
		RGB = GL_RGB,
		RGBA = GL_RGBA,
		Depth = GL_DEPTH_COMPONENT,
		DepthStencil = GL_DEPTH_STENCIL
	}
	#region PixelFormat Extensions
	public static class PixelFormatExt
	{
		public static GLint Components(this PixelFormat pixelFormat)
		{
			switch (pixelFormat)
			{
				case PixelFormat.R:
					return 1;
				case PixelFormat.RG:
					return 2;
				case PixelFormat.RGB:
					return 3;
				case PixelFormat.RGBA:
					return 4;
				case PixelFormat.Depth:
					return 1;
				case PixelFormat.DepthStencil:
					return 2;
				default:
					throw new IllegalValueException(typeof(PixelFormat), pixelFormat);
			}
		}
	}
	#endregion

	public enum PixelType : GLuint
	{
		//Byte = GL_BYTE,
		UnsignedByte = GL_UNSIGNED_BYTE,
		//Short = GL_SHORT,
		UnsignedShort = GL_UNSIGNED_SHORT,
		//Int = GL_INT,
		UnsignedInt = GL_UNSIGNED_INT,
		//HalfFloat = GL_HALF_FLOAT,
		Float = GL_FLOAT,
		//UnsignedShort_5_6_5 = GL_UNSIGNED_SHORT_5_6_5,
		//UnsignedShort_4_4_4_4 = GL_UNSIGNED_SHORT_4_4_4_4,
		//UnsignedShort_5_5_5_1 = GL_UNSIGNED_SHORT_5_5_5_1,
		//UnsignedInt_2_10_10_10_REV = GL_UNSIGNED_INT_2_10_10_10_REV,
		//UnsignedInt_10F_11F_11F_REV = GL_UNSIGNED_INT_10F_11F_11F_REV,
		//UnsignedInt_5_9_9_9_REV = GL_UNSIGNED_INT_5_9_9_9_REV,
		UnsignedInt_24_8 = GL_UNSIGNED_INT_24_8,
		Float_32_UnsignedInt_24_8_REV = GL_FLOAT_32_UNSIGNED_INT_24_8_REV
	}
	#region PixelType Extensions
	public static class PixelTypeExt
	{
		public static int Size(this PixelType PixelType)
		{
			switch (PixelType)
			{
				case PixelType.UnsignedByte:
					return 1;
				case PixelType.UnsignedShort:
					return 2;
				case PixelType.UnsignedInt:
					return 4;
				case PixelType.Float:
					return 4;
				case PixelType.UnsignedInt_24_8:
					return 4;
				case PixelType.Float_32_UnsignedInt_24_8_REV:
					return 5;
				default:
					throw new IllegalValueException(typeof(PixelType), PixelType);
			}
		}
	}
	#endregion

	public enum DataType : GLuint
	{
		Byte = GL_BYTE,
		UnsignedByte = GL_UNSIGNED_BYTE,
		Short = GL_SHORT,
		UnsignedShort = GL_UNSIGNED_SHORT,
		Int = GL_INT,
		UnsignedInt = GL_UNSIGNED_INT,
		Float = GL_FLOAT,
		Double = GL_DOUBLE,
		HalfFloat = GL_HALF_FLOAT,
		UnsignedInt_2_10_10_10_Rev = GL_UNSIGNED_INT_2_10_10_10_REV,
		Int_2_10_10_10_Rev = GL_INT_2_10_10_10_REV
	}

	public enum TextureParam : GLuint
	{
		BaseLevel = GL_TEXTURE_BASE_LEVEL,
		CompareFunc = GL_TEXTURE_COMPARE_FUNC,
		CompareMode = GL_TEXTURE_COMPARE_MODE,
		MinFilter = GL_TEXTURE_MIN_FILTER,
		MagFilter = GL_TEXTURE_MAG_FILTER,
		MinLOD = GL_TEXTURE_MIN_LOD,
		MaxLOD = GL_TEXTURE_MAX_LOD,
		MaxLevel = GL_TEXTURE_MAX_LEVEL,
		SwizzleR = GL_TEXTURE_SWIZZLE_R,
		SwizzleG = GL_TEXTURE_SWIZZLE_G,
		SwizzleB = GL_TEXTURE_SWIZZLE_B,
		SwizzleA = GL_TEXTURE_SWIZZLE_A,
		WrapS = GL_TEXTURE_WRAP_S,
		WrapT = GL_TEXTURE_WRAP_T,
		WrapR = GL_TEXTURE_WRAP_R
		//DepthTextureMode = 0x884B // Unavailable in OpenGL 3.x, Available in ES 3.0
	}

	public enum TextureFilter : GLuint
	{
		Nearest = GL_NEAREST, // Both GL_TEXTURE_MIN_FILTER and GL_TEXTURE_MAX_FILTER
		Linear = GL_LINEAR, // Both GL_TEXTURE_MIN_FILTER and GL_TEXTURE_MAX_FILTER
		NearestMipmapNearest = GL_NEAREST_MIPMAP_NEAREST, // GL_TEXTURE_MIN_FILTER only
		LinearMipmapNearest = GL_LINEAR_MIPMAP_NEAREST, // GL_TEXTURE_MIN_FILTER only
		NearestMipmapLinear = GL_NEAREST_MIPMAP_LINEAR, // GL_TEXTURE_MIN_FILTER only
		LinearMipmapLinear = GL_LINEAR_MIPMAP_LINEAR // GL_TEXTURE_MIN_FILTER only
	}

	public enum TextureWrap : GLuint
	{
		Clamp = GL_CLAMP_TO_EDGE,
		Repeat = GL_REPEAT,
		MirroredRepeat = GL_MIRRORED_REPEAT
	}

	public enum TextureAttachment : GLuint
	{
		Depth = GL_DEPTH_ATTACHMENT,
		DepthStencil = GL_DEPTH_STENCIL_ATTACHMENT,
		Colour0 = GL_COLOR_ATTACHMENT0,
		Colour1 = GL_COLOR_ATTACHMENT1,
		Colour2 = GL_COLOR_ATTACHMENT2,
		Colour3 = GL_COLOR_ATTACHMENT3,
		Colour4 = GL_COLOR_ATTACHMENT4,
		Colour5 = GL_COLOR_ATTACHMENT5,
		Colour6 = GL_COLOR_ATTACHMENT6,
		Colour7 = GL_COLOR_ATTACHMENT7,
		Colour8 = GL_COLOR_ATTACHMENT8,
		Colour9 = GL_COLOR_ATTACHMENT9,
		Colour10 = GL_COLOR_ATTACHMENT10,
		Colour11 = GL_COLOR_ATTACHMENT11,
		Colour12 = GL_COLOR_ATTACHMENT12,
		Colour13 = GL_COLOR_ATTACHMENT13,
		Colour14 = GL_COLOR_ATTACHMENT14,
		Colour15 = GL_COLOR_ATTACHMENT15
	}

	public enum BufferTarget : GLuint
	{
		Array = GL_ARRAY_BUFFER,
		ElementArray = GL_ELEMENT_ARRAY_BUFFER,
		CopyRead = GL_COPY_READ_BUFFER,
		CopyWrite = GL_COPY_WRITE_BUFFER,
		PixelPack = GL_PIXEL_PACK_BUFFER,
		PixelUnpack = GL_PIXEL_UNPACK_BUFFER,
		TransformFeedback = GL_TRANSFORM_FEEDBACK_BUFFER,
		Uniform = GL_UNIFORM_BUFFER
	}

	public enum BufferUsage : GLuint
	{
		StreamDraw = GL_STREAM_DRAW,
		StreamRead = GL_STREAM_READ,
		StreamCopy = GL_STREAM_COPY,
		StaticDraw = GL_STATIC_DRAW,
		StaticRead = GL_STATIC_READ,
		StaticCopy = GL_STATIC_COPY,
		DynamicDraw = GL_DYNAMIC_DRAW,
		DynamicRead = GL_DYNAMIC_READ,
		DynamicCopy = GL_DYNAMIC_COPY
	}

	public enum PrimitiveMode : GLuint
	{
		Points = GL_POINTS,
		Lines = GL_LINES,
		LineLoop = GL_LINE_LOOP,
		LineStrip = GL_LINE_STRIP,
		Triangles = GL_TRIANGLES,
		TriangleStrip = GL_TRIANGLE_STRIP,
		TriangleFan = GL_TRIANGLE_FAN
	}

	public enum IndexType : GLuint
	{
		UnsignedByte = GL_UNSIGNED_BYTE,
		UnsignedShort = GL_UNSIGNED_SHORT,
		UnsignedInt = GL_UNSIGNED_INT
	}

	public enum FramebufferTarget : GLuint
	{
		Framebuffer = GL_FRAMEBUFFER,
		Draw = GL_DRAW_FRAMEBUFFER,
		Read = GL_READ_FRAMEBUFFER
	}

	public enum FramebufferStatus : GLuint
	{
		Complete = GL_FRAMEBUFFER_COMPLETE,
		Undefined = GL_FRAMEBUFFER_UNDEFINED,
		IncompleteAttachment = GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT,
		IncompleteMissingAttachment = GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT,
		IncompleteDrawBuffer = GL_FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER,
		IncompleteReadBuffer = GL_FRAMEBUFFER_INCOMPLETE_READ_BUFFER,
		Unsupported = GL_FRAMEBUFFER_UNSUPPORTED,
		IncompleteMultisample = GL_FRAMEBUFFER_INCOMPLETE_MULTISAMPLE,
		IncompleteLayerTargets = GL_FRAMEBUFFER_INCOMPLETE_LAYER_TARGETS
	}

	public enum DrawBuffer : GLuint
	{
		None = GL_NONE,
		Back = GL_BACK,
		Colour0 = GL_COLOR_ATTACHMENT0,
		Colour1 = GL_COLOR_ATTACHMENT1,
		Colour2 = GL_COLOR_ATTACHMENT2,
		Colour3 = GL_COLOR_ATTACHMENT3,
		Colour4 = GL_COLOR_ATTACHMENT4,
		Colour5 = GL_COLOR_ATTACHMENT5,
		Colour6 = GL_COLOR_ATTACHMENT6,
		Colour7 = GL_COLOR_ATTACHMENT7,
		Colour8 = GL_COLOR_ATTACHMENT8,
		Colour9 = GL_COLOR_ATTACHMENT9,
		Colour10 = GL_COLOR_ATTACHMENT10,
		Colour11 = GL_COLOR_ATTACHMENT11,
		Colour12 = GL_COLOR_ATTACHMENT12,
		Colour13 = GL_COLOR_ATTACHMENT13,
		Colour14 = GL_COLOR_ATTACHMENT14,
		Colour15 = GL_COLOR_ATTACHMENT15
	}

	public enum ReadBuffer : GLuint
	{
		None = GL_NONE,
		Back = GL_BACK,
		Colour0 = GL_COLOR_ATTACHMENT0,
		Colour1 = GL_COLOR_ATTACHMENT1,
		Colour2 = GL_COLOR_ATTACHMENT2,
		Colour3 = GL_COLOR_ATTACHMENT3,
		Colour4 = GL_COLOR_ATTACHMENT4,
		Colour5 = GL_COLOR_ATTACHMENT5,
		Colour6 = GL_COLOR_ATTACHMENT6,
		Colour7 = GL_COLOR_ATTACHMENT7,
		Colour8 = GL_COLOR_ATTACHMENT8,
		Colour9 = GL_COLOR_ATTACHMENT9,
		Colour10 = GL_COLOR_ATTACHMENT10,
		Colour11 = GL_COLOR_ATTACHMENT11,
		Colour12 = GL_COLOR_ATTACHMENT12,
		Colour13 = GL_COLOR_ATTACHMENT13,
		Colour14 = GL_COLOR_ATTACHMENT14,
		Colour15 = GL_COLOR_ATTACHMENT15,
		Depth = GL_DEPTH_ATTACHMENT, // TODO: Not supported?
		DepthStencil = GL_DEPTH_STENCIL_ATTACHMENT // TODO: Not supported?
	}

	public enum BlitFilter : GLuint
	{
		Nearest = GL_NEAREST,
		Linear = GL_LINEAR
	}

	public enum ShaderType : GLuint
	{
		Vertex = GL_VERTEX_SHADER,
		Geometry = GL_GEOMETRY_SHADER,
		Fragment = GL_FRAGMENT_SHADER
	}

	public enum ShaderParam : GLuint
	{
		ShaderType = GL_SHADER_TYPE,
		DeleteStatus = GL_DELETE_STATUS,
		CompileStatus = GL_COMPILE_STATUS,
		InfoLogLength = GL_INFO_LOG_LENGTH,
		ShaderSourceLength = GL_SHADER_SOURCE_LENGTH
	}

	public enum ProgramParam : GLuint
	{
		DeleteStatus = GL_DELETE_STATUS,
		LinkStatus = GL_LINK_STATUS,
		ValidateStatus = GL_VALIDATE_STATUS,
		InfoLogLength = GL_INFO_LOG_LENGTH,
		AttachedShaders = GL_ATTACHED_SHADERS,
		ActiveAttributes = GL_ACTIVE_ATTRIBUTES,
		ActiveAttributeMaxLength = GL_ACTIVE_ATTRIBUTE_MAX_LENGTH,
		ActiveUniforms = GL_ACTIVE_UNIFORMS,
		ActiveUniformMaxLength = GL_ACTIVE_UNIFORM_MAX_LENGTH
	}

	public enum UniformType : GLuint
	{
		Bool = GL_BOOL,
		Int = GL_INT,
		Float = GL_FLOAT,
		Vec2 = GL_FLOAT_VEC2,
		Vec3 = GL_FLOAT_VEC3,
		Vec4 = GL_FLOAT_VEC4,
		Mat3x2 = GL_FLOAT_MAT3x2,
		Mat3 = GL_FLOAT_MAT3,
		Mat4 = GL_FLOAT_MAT4,
		Sampler2D = GL_SAMPLER_2D,
		SamplerCube = GL_SAMPLER_CUBE
	}

	public enum DebugSource : GLuint
	{
		DontCare = GL_DONT_CARE, API = GL_DEBUG_SOURCE_API,
		WindowSystem = GL_DEBUG_SOURCE_WINDOW_SYSTEM,
		ShaderCompiler = GL_DEBUG_SOURCE_SHADER_COMPILER,
		ThirdParty = GL_DEBUG_SOURCE_THIRD_PARTY,
		Application = GL_DEBUG_SOURCE_APPLICATION,
		Other = GL_DEBUG_SOURCE_OTHER
	}

	public enum DebugType : GLuint
	{
		DontCare = GL_DONT_CARE,
		Error = GL_DEBUG_TYPE_ERROR,
		DeprecatedBehavior = GL_DEBUG_TYPE_DEPRECATED_BEHAVIOR,
		UndefinedBehavior = GL_DEBUG_TYPE_UNDEFINED_BEHAVIOR,
		Portability = GL_DEBUG_TYPE_PORTABILITY,
		Performance = GL_DEBUG_TYPE_PERFORMANCE,
		Other = GL_DEBUG_TYPE_OTHER
	}

	public enum DebugSeverity : GLuint
	{
		DontCare = GL_DONT_CARE,
		High = GL_DEBUG_SEVERITY_HIGH,
		Medium = GL_DEBUG_SEVERITY_MEDIUM,
		Low = GL_DEBUG_SEVERITY_LOW,
		Notification = GL_DEBUG_SEVERITY_NOTIFICATION
	}
	#endregion

	internal static class GL
	{
		// Errors
		private static bool supportsDebugMessageCallback = true;
		private static IntPtr messageCallback;
		private static IntPtr messageControl;

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
				foreach (var field in typeof(GL).GetFields(BindingFlags.NonPublic | BindingFlags.Static).Where(field => field.IsDefined(typeof(Shared))))
				{
					field.SetValue(null, GetProcAddress(field.Name, field.FieldType));
					// Debug
					//Output.WriteLine($"Shared - {field.Name}");
				}

				if (!es)
				{
					foreach (var field in typeof(GL).GetFields(BindingFlags.NonPublic | BindingFlags.Static).Where(field => field.IsDefined(typeof(OpenGL))))
					{
						field.SetValue(null, GetProcAddress(field.Name, field.FieldType));
						// Debug
						//Output.WriteLine($"OpenGL - {field.Name}");
					}
				}

				if (es)
				{
					foreach (var field in typeof(GL).GetFields(BindingFlags.NonPublic | BindingFlags.Static).Where(field => field.IsDefined(typeof(OpenGLES))))
					{
						field.SetValue(null, GetProcAddress(field.Name, field.FieldType));
						// Debug
						//Output.WriteLine($"OpenGL ES - {field.Name}");
					}

					// NVIDIA / Desktop ES Functions
					//  Not normally supported in ES, but possibly supported on
					//  NVIDIA or Desktop ES, so try to aquire but fail silently.
					foreach (var field in typeof(GL).GetFields(BindingFlags.NonPublic | BindingFlags.Static).Where(field => field.IsDefined(typeof(DesktopES))))
					{
						try
						{
							field.SetValue(null, GetProcAddress(field.Name, field.FieldType));
							// Debug
							//Output.WriteLine($"Desktop ES - {field.Name}");
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

			#region glDebugMessageCallback
#if DEBUG
			// OpenGL requires no suffix whereas OpenGL ES requires the "KHR" suffix. https://www.khronos.org/registry/OpenGL/extensions/KHR/KHR_debug.txt
			// Try None/KHR first
			if (es)
			{
				messageCallback = Platform.GetProcAddress("glDebugMessageCallbackKHR");
				messageControl = Platform.GetProcAddress("glDebugMessageControlKHR");
			}
			else
			{
				messageCallback = Platform.GetProcAddress("glDebugMessageCallback");
				messageControl = Platform.GetProcAddress("glDebugMessageControl");
			}
			// Then try ARB_debug_output
			if (messageCallback == IntPtr.Zero || messageControl == IntPtr.Zero)
			{
				messageCallback = Platform.GetProcAddress("glDebugMessageCallbackARB");
				messageControl = Platform.GetProcAddress("glDebugMessageControlARB");
			}
			if (messageCallback == IntPtr.Zero || messageControl == IntPtr.Zero)
			{
				supportsDebugMessageCallback = false;
			}

			// Check for stub functions (android drivers)
			if (es)
			{
				if (Platform.OpenGLExtensionSupported("GL_KHR_debug") && Platform.OpenGLExtensionSupported("GL_ARB_debug_output"))
				{
					supportsDebugMessageCallback = false;
				}
			}

			// Assign the callback
			if (supportsDebugMessageCallback)
			{
				glDebugMessageCallback = (_glDebugMessageCallback)Marshal.GetDelegateForFunctionPointer(messageCallback, typeof(_glDebugMessageCallback));
				glDebugMessageControl = (_glDebugMessageControl)Marshal.GetDelegateForFunctionPointer(messageControl, typeof(_glDebugMessageControl));
				glDebugMessageControl(DebugSource.DontCare, DebugType.DontCare, DebugSeverity.DontCare, 0, IntPtr.Zero, true);
				glDebugMessageControl(DebugSource.DontCare, DebugType.Other, DebugSeverity.Low, 0, IntPtr.Zero, false);
				glDebugMessageControl(DebugSource.DontCare, DebugType.Other, DebugSeverity.Notification, 0, IntPtr.Zero, false);
				glDebugMessageCallback(Marshal.GetFunctionPointerForDelegate(DebugCall), IntPtr.Zero);
				Output.WriteLine("glDebugMessageCallback Supported");
			}
			else
			{
				Output.WriteLine("glDebugMessageCallback Unsupported, using glCheckError instead");
			}
#endif
			#endregion

			// Strings
			Version = GetString(Strings.Version);
			GLSLVersion = GetString(Strings.GLSLVersion);
			Vendor = GetString(Strings.Vendor);
			Renderer = GetString(Strings.Renderer);
			if (es)
				Extensions = GetString(Strings.Extensions); // Requires OpenGL 3.2 glGetStringi
			else
				Extensions = "Unsupported in OpenGL, only supported on OpenGL ES";

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
			Output.WriteLine($"OpenGL{(es ? " ES" : "")} Initialized");
			Output.WriteLine($"OpenGL{(es ? " ES" : "")} Version: {MajorVersion}.{MinorVersion}");
			Output.WriteLine($"OpenGL{(es ? " ES" : "")} Version: {Version}");
			Output.WriteLine($"OpenGL{(es ? " ES" : "")} GLSL Version: {GLSLVersion}");
			Output.WriteLine($"OpenGL{(es ? " ES" : "")} Vendor: {Vendor}");
			Output.WriteLine($"OpenGL{(es ? " ES" : "")} Renderer: {Renderer}");
			Output.WriteLine($"OpenGL{(es ? " ES" : "")} MaxColourAttachments: {MaxColourAttachments}");
			Output.WriteLine($"OpenGL{(es ? " ES" : "")} MaxCubeMapTextureSize: {MaxCubeMapTextureSize}");
			Output.WriteLine($"OpenGL{(es ? " ES" : "")} MaxDrawBuffers: {MaxDrawBuffers}");
			Output.WriteLine($"OpenGL{(es ? " ES" : "")} MaxElementIndices: {MaxElementIndices}");
			Output.WriteLine($"OpenGL{(es ? " ES" : "")} MaxElementVertices: {MaxElementVertices}");
			Output.WriteLine($"OpenGL{(es ? " ES" : "")} MaxRenderbufferSize: {MaxRenderbufferSize}");
			Output.WriteLine($"OpenGL{(es ? " ES" : "")} MaxSamples: {MaxSamples}");
			Output.WriteLine($"OpenGL{(es ? " ES" : "")} MaxTextureImageUnits: {MaxTextureImageUnits}");
			Output.WriteLine($"OpenGL{(es ? " ES" : "")} MaxTextureSize: {MaxTextureSize}");
			//Output.WriteLine($"OpenGL{(es ? " ES" : "")} - Extensions: {GetString(Strings.Extensions)}");
		}

#pragma warning disable CS0649
#pragma warning disable IDE0044

		#region Error/Debug Functions
		[Shared]
		private static _glGetError glGetError;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate ErrorCode _glGetError();
		[Conditional("DEBUG")]
		private static void CheckError()
		{
			if (!supportsDebugMessageCallback)
			{
				var err = glGetError();
				if (err != ErrorCode.NoError)
					throw new InvalidOperationException($"OpenGL Error {(int)err:X}: {err.ToString()}");
				//Debug.Assert(err == ErrorCode.NoError, $"OpenGL Error {(int)err:X}: {err.ToString()}");
			}
		}

		private static _glDebugMessageCallback glDebugMessageCallback;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glDebugMessageCallback(IntPtr debugCallback, IntPtr userParam);

		private static _glDebugMessageControl glDebugMessageControl;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glDebugMessageControl(DebugSource source, DebugType type, DebugSeverity severity, GLint count, IntPtr ids, bool enabled);

		// ARB_debug_output/KHR_debug callback
		private static DebugProc DebugCall = DebugCallback;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void DebugProc(DebugSource source, DebugType type, GLuint id, DebugSeverity severity, GLint length, IntPtr message, IntPtr userParam);
		private static void DebugCallback(DebugSource source, DebugType type, uint id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
		{
			string error = $"{Marshal.PtrToStringAnsi(message)}\n\tSource: {source.ToString()}\n\tType: {type.ToString()}\n\tSeverity: {severity.ToString()}";
			if (type == DebugType.Error)
			{
				Output.WriteLine($"OpenGL Error: {error}");
				throw new InvalidOperationException(error);
			}
			Output.WriteLine($"OpenGL Warning: {error}");
		}
		#endregion

		#region Get String/Integer Functions
		[Shared]
		private static _glGetString glGetString;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate IntPtr _glGetString(Strings name);
		public static unsafe string GetString(Strings name)
		{
			string s = new string((sbyte*)glGetString(name));
			CheckError();
			return s;
		}

		[Shared]
		private static _glGetIntegerv glGetIntegerv;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private unsafe delegate void _glGetIntegerv(Integers name, GLint* data);
		private static unsafe void GetIntegerv(Integers name, out int val)
		{
			fixed (int* p = &val)
			{
				glGetIntegerv(name, p);
				val = *p;
			}
			CheckError();
		}
		public static int GetIntegerv(Integers name)
		{
			GetIntegerv(name, out int val);
			return val;
		}
		#endregion

		#region Enable/Disable Functions
		[Shared]
		private static _glEnable glEnable;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glEnable(EnableCap mode);
		public static void Enable(EnableCap mode)
		{
			glEnable(mode);
			CheckError();
		}

		[Shared]
		private static _glDisable glDisable;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
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
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glViewport(GLint x, GLint y, GLint width, GLint height);
		public static void Viewport(int x, int y, int width, int height)
		{
			glViewport(x, y, width, height);
			CheckError();
		}

		[Shared]
		private static _glScissor glScissor;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
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
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glColorMask(bool red​, bool green​, bool blue​, bool alpha);
		public static void ColourMask(bool red​, bool green​, bool blue​, bool alpha​)
		{
			glColorMask(red​, green​, blue​, alpha​);
			CheckError();
		}

		[OpenGL]
		private static _glColorMaski glColorMaski;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
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
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glBlendColor(float red, float green, float blue, float alpha);
		public static void BlendColour(float red, float green, float blue, float alpha)
		{
			glBlendColor(red, green, blue, alpha);
			CheckError();
		}

		[Shared]
		private static _glBlendEquation glBlendEquation;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glBlendEquation(BlendEquation eq);
		public static void BlendEquation(BlendEquation eq)
		{
			glBlendEquation(eq);
			CheckError();
		}

		[Shared]
		private static _glBlendEquationSeparate glBlendEquationSeparate;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glBlendEquationSeparate(BlendEquation modeRGB, BlendEquation modeAlpha);
		public static void BlendEquationSeparate(BlendEquation modeRGB, BlendEquation modeAlpha)
		{
			glBlendEquationSeparate(modeRGB, modeAlpha);
			CheckError();
		}

		[Shared]
		private static _glBlendFunc glBlendFunc;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glBlendFunc(BlendFactor sfactor, BlendFactor dfactor);
		public static void BlendFunc(BlendFactor sFactor, BlendFactor dFactor)
		{
			glBlendFunc(sFactor, dFactor);
			CheckError();
		}

		[Shared]
		private static _glBlendFuncSeparate glBlendFuncSeparate;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
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
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glDepthMask(bool flag);
		public static void DepthMask(bool flag)
		{
			glDepthMask(flag);
			CheckError();
		}

		[Shared]
		private static _glDepthFunc glDepthFunc;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glDepthFunc(DepthFunc func);
		public static void DepthFunc(DepthFunc func)
		{
			glDepthFunc(func);
			CheckError();
		}

		private static _glDepthRange glDepthRange;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glDepthRange(double nearVal, double farVal);
		private static _glDepthRangef glDepthRangef;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
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
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glStencilMask(GLuint mask);
		public static void StencilMask(uint mask)
		{
			glStencilMask(mask);
			CheckError();
		}

		[Shared]
		private static _glStencilMaskSeparate glStencilMaskSeparate;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glStencilMaskSeparate(Face face, GLuint mask);
		public static void StencilMaskSeparate(Face face, uint mask)
		{
			glStencilMaskSeparate(face, mask);
			CheckError();
		}

		[Shared]
		private static _glStencilFunc glStencilFunc;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glStencilFunc(StencilFunc func, GLint reference, GLuint mask);
		public static void StencilFunc(StencilFunc func, int reference, uint mask)
		{
			glStencilFunc(func, reference, mask);
			CheckError();
		}

		[Shared]
		private static _glStencilFuncSeparate glStencilFuncSeparate;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glStencilFuncSeparate(Face face, StencilFunc func, GLint reference, GLuint mask);
		public static void StencilFuncSeparate(Face face, StencilFunc func, int reference, uint mask)
		{
			glStencilFuncSeparate(face, func, reference, mask);
			CheckError();
		}

		[Shared]
		private static _glStencilOp glStencilOp;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glStencilOp(StencilOp sfail, StencilOp dpfail, StencilOp dppass);
		public static void StencilOp(StencilOp sfail, StencilOp dpfail, StencilOp dppass)
		{
			glStencilOp(sfail, dpfail, dppass);
			CheckError();
		}

		[Shared]
		private static _glStencilOpSeparate glStencilOpSeparate;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
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
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glFrontFace(FrontFace mode);
		public static void FrontFace(FrontFace face)
		{
			glFrontFace(face);
			CheckError();
		}

		[Shared]
		private static _glCullFace glCullFace;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glCullFace(Face mode);
		public static void CullFace(Face face)
		{
			glCullFace(face);
			CheckError();
		}

		[Shared]
		private static _glPolygonOffset glPolygonOffset;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glPolygonOffset(float factor, float units);
		public static void PolygonOffset(float factor, float units)
		{
			glPolygonOffset(factor, units);
			CheckError();
		}

		[OpenGL, DesktopES]
		private static _glPolygonMode glPolygonMode;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glPolygonMode(Face face, PolygonMode mode);
		public static void PolygonMode(PolygonMode mode)
		{
			if (glPolygonMode != null)
			{
				// OpenGL 3.2 deprecated separate front/back states for PolygonMode, so hardcode the only Face enum allowed
				glPolygonMode(Face.FrontAndBack, mode);
				CheckError();
			}
			else
			{
				Output.WriteLine("glPolygonMode is not available in OpenGL ES.");
				//throw new NotSupportedException("glPolygonMode is not available in OpenGL ES.");
			}
		}
		#endregion

		#region Clear Functions
		[Shared]
		private static _glClear glClear;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glClear(BufferBit mask);
		public static void Clear(BufferBit mask)
		{
			glClear(mask);
			CheckError();
		}

		[Shared]
		private static _glClearColor glClearColor;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
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
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glClearDepth(double depth);
		private static _glClearDepthf glClearDepthf;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glClearDepthf(float depth);
		public static void ClearDepth(double depth)
		{
			glClearDepth(depth);
			CheckError();
		}
		#endregion

		#region Texture Functions
		[Shared]
		private static _glGenTextures glGenTextures;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private unsafe delegate void _glGenTextures(GLint n, GLuint* textures);
		public static unsafe void GenTextures(int n, uint[] textures)
		{
			fixed (uint* ptr = textures) { glGenTextures(n, ptr); }
			CheckError();
		}
		public static unsafe uint GenTexture()
		{
			uint texture = 0;
			glGenTextures(1, &texture);
			CheckError();
			return texture;
		}

		[Shared]
		private static _glDeleteTextures glDeleteTextures;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private unsafe delegate void _glDeleteTextures(GLint n, GLuint* textures);
		public static unsafe void DeleteTextures(int n, uint[] textures)
		{
			fixed (uint* ptr = textures) { glDeleteTextures(n, ptr); }
			CheckError();
		}
		public static unsafe void DeleteTexture(uint texture)
		{
			glDeleteTextures(1, &texture);
			CheckError();
		}

		[Shared]
		private static _glActiveTexture glActiveTexture;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glActiveTexture(GLuint textureImageUnit);
		public static void ActiveTexture(uint textureImageUnit)
		{
			if (textureImageUnit >= MaxTextureImageUnits)
				throw new Exception("ActiveTexture textureImageUnit must be between 0 and " + (MaxTextureImageUnits - 1));

			glActiveTexture(GL_TEXTURE0 + textureImageUnit);
			CheckError();
		}

		[Shared]
		private static _glBindTexture glBindTexture;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glBindTexture(TextureTarget target, GLuint texture);
		public static void BindTexture(TextureTarget target, uint texture)
		{
			glBindTexture(target, texture);
			CheckError();
		}

		[Shared]
		private static _glTexImage2D glTexImage2D;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private unsafe delegate void _glTexImage2D(TextureTarget target, GLint level, GLint internalFormat, GLint width, GLint height, GLint border, PixelFormat format, PixelType type, void* data);
		public static unsafe void TexImage2D(TextureTarget target, int level, TextureFormat internalFormat, int width, int height, int border, PixelFormat format, PixelType type, void* data)
		{
			glTexImage2D(target, level, (int)internalFormat, width, height, border, format, type, data);
			CheckError();
		}

		[Shared]
		private static _glTexParameteri glTexParameteri;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glTexParameteri(TextureTarget target, TextureParam pname, GLint param);
		public static void TexParameterI(TextureTarget target, TextureParam pname, int param)
		{
			glTexParameteri(target, pname, param);
			CheckError();
		}

		[Shared]
		private static _glGetTexParameteriv glGetTexParameteriv;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glGetTexParameteriv(TextureTarget target, TextureParam pname, out GLint result);
		public static void GetTexParameterI(TextureTarget target, TextureParam pname, out int result)
		{
			glGetTexParameteriv(target, pname, out result);
			CheckError();
		}

		[OpenGL]
		[DesktopES]
		private static _glGetTexImage glGetTexImage;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private unsafe delegate void _glGetTexImage(TextureTarget target, GLint level, PixelFormat format, PixelType type, void* data);
		public static unsafe void GetTexImage(TextureTarget target, int level, PixelFormat format, PixelType type, void* data)
		{
			if (glGetTexImage != null)
			{
				glGetTexImage(target, level, format, type, data);
				CheckError();
			}
			else
			{
				Output.WriteLine("glGetTexImage is not available in OpenGL ES.");
				//throw new NotSupportedException("glGetTexImage is not available in OpenGL ES.");
			}
		}
		#endregion

		#region Buffer Functions
		[Shared]
		private static _glGenBuffers glGenBuffers;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private unsafe delegate void _glGenBuffers(GLint n, GLuint* buffers);
		public static unsafe void GenBuffers(int n, uint[] buffers)
		{
			fixed (uint* ptr = buffers) { glGenBuffers(n, ptr); }
			CheckError();
		}
		public static void GenBuffers(uint[] buffers)
		{
			GenBuffers(buffers.Length, buffers);
		}
		public static unsafe uint GenBuffer()
		{
			uint buffer = 0;
			glGenBuffers(1, &buffer);
			CheckError();
			return buffer;
		}

		[Shared]
		private static _glDeleteBuffers glDeleteBuffers;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private unsafe delegate void _glDeleteBuffers(GLint n, GLuint* buffers);
		public static unsafe void DeleteBuffers(int n, uint[] buffers)
		{
			fixed (uint* ptr = buffers) { glDeleteBuffers(n, ptr); }
			CheckError();
		}
		public static void DeleteBuffers(uint[] buffers)
		{
			DeleteBuffers(buffers.Length, buffers);
		}
		public static unsafe void DeleteBuffer(uint buffer)
		{
			glDeleteBuffers(1, &buffer);
			CheckError();
		}

		[Shared]
		private static _glBindBuffer glBindBuffer;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glBindBuffer(BufferTarget target, GLuint buffer);
		public static void BindBuffer(BufferTarget target, uint buffer)
		{
			glBindBuffer(target, buffer);
			CheckError();
		}

		[Shared]
		private static _glBufferData glBufferData;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glBufferData(BufferTarget target, IntPtr size, IntPtr data, BufferUsage usage);
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

		[Shared]
		private static _glBufferSubData glBufferSubData;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glBufferSubData(BufferTarget target, IntPtr offset, IntPtr size, IntPtr data);
		public static void BufferSubData(BufferTarget target, int offset, int size, IntPtr data)
		{
			glBufferSubData(target, new IntPtr(offset), new IntPtr(size), data);
			CheckError();
		}

		[OpenGL]
		[DesktopES]
		private static _glGetBufferSubData glGetBufferSubData;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glGetBufferSubData(BufferTarget target, IntPtr offset, IntPtr size, IntPtr data);
		public static void GetBufferSubData(BufferTarget target, int offset, int size, IntPtr data)
		{
			if (glGetBufferSubData != null)
			{
				glGetBufferSubData(target, new IntPtr(offset), new IntPtr(size), data);
				CheckError();
			}
			else
			{
				Output.WriteLine("glGetBufferSubData is not available in OpenGL ES.");
				//throw new NotSupportedException("glGetBufferSubData is not available in OpenGL ES.");
			}
		}
		#endregion

		#region Framebuffer Functions
		[Shared]
		private static _glGenFramebuffers glGenFramebuffers;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private unsafe delegate void _glGenFramebuffers(GLint n, GLuint* framebuffers);
		public static unsafe void GenFramebuffers(int n, uint[] framebuffers)
		{
			fixed (uint* ptr = framebuffers) { glGenFramebuffers(n, ptr); }
			CheckError();
		}
		public static unsafe uint GenFramebuffer()
		{
			uint fbo = 0;
			glGenFramebuffers(1, &fbo);
			CheckError();
			return fbo;
		}

		[Shared]
		private static _glDeleteFramebuffers glDeleteFramebuffers;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private unsafe delegate void _glDeleteFramebuffers(GLint n, GLuint* framebuffers);
		public static unsafe void DeleteFramebuffers(int n, uint[] framebuffers)
		{
			fixed (uint* ptr = framebuffers) { glDeleteFramebuffers(n, ptr); }
			CheckError();
		}
		public static unsafe void DeleteFramebuffer(uint framebuffer)
		{
			glDeleteFramebuffers(1, &framebuffer);
			CheckError();
		}

		[Shared]
		private static _glBindFramebuffer glBindFramebuffer;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glBindFramebuffer(FramebufferTarget target, GLuint framebuffer);
		public static void BindFramebuffer(FramebufferTarget target, uint framebuffer)
		{
			glBindFramebuffer(target, framebuffer);
			CheckError();
		}

		[Shared]
		private static _glFramebufferTexture2D glFramebufferTexture2D;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glFramebufferTexture2D(FramebufferTarget target, TextureAttachment attachment, TextureTarget textarget, GLuint texture, GLint level);
		public static void FramebufferTexture2D(FramebufferTarget target, TextureAttachment attachment, TextureTarget textarget, uint texture, int level)
		{
			if (attachment != TextureAttachment.Depth && attachment != TextureAttachment.DepthStencil)
			{
				uint texn = (uint)attachment - (uint)TextureAttachment.Colour0;
				if (texn >= MaxColourAttachments)
					throw new Exception("Exceeding max colour attachments: " + MaxColourAttachments);
			}
			glFramebufferTexture2D(target, attachment, textarget, texture, level);
			CheckError();
		}

		[Shared]
		private static _glCheckFramebufferStatus glCheckFramebufferStatus;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate FramebufferStatus _glCheckFramebufferStatus(FramebufferTarget target);
		public static FramebufferStatus CheckFramebufferStatus(FramebufferTarget target)
		{
			var status = glCheckFramebufferStatus(target);
			CheckError();
			return status;
		}

		[Shared]
		private static _glDrawBuffers glDrawBuffers;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private unsafe delegate void _glDrawBuffers(GLint n, DrawBuffer* bufs);
		public static unsafe void DrawBuffers(int n, DrawBuffer[] bufs)
		{
			if (n > bufs.Length)
				throw new Exception("Not enough buffers in array.");
			if (n > MaxDrawBuffers)
				throw new Exception("Exceeded maximum number of draw buffers: " + MaxDrawBuffers);

			fixed (DrawBuffer* ptr = bufs) { glDrawBuffers(n, ptr); }
			CheckError();
		}

		[Shared]
		private static _glReadBuffer glReadBuffer;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glReadBuffer(ReadBuffer buffer);
		public static void ReadBuffer(ReadBuffer buffer)
		{
			glReadBuffer(buffer);
			CheckError();
		}

		[Shared]
		private static _glBlitFramebuffer glBlitFramebuffer;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glBlitFramebuffer(GLint srcX0, GLint srcY0, GLint srcX1, GLint srcY1, GLint dstX0, GLint dstY0, GLint dstX1, GLint dstY1, BufferBit mask, BlitFilter filter);
		public static void BlitFramebuffer(RectangleI src, RectangleI dst, BufferBit mask, BlitFilter filter)
		{
			glBlitFramebuffer(src.X, src.Y, src.MaxX, src.MaxY, dst.X, dst.Y, dst.MaxX, dst.MaxY, mask, filter);
			CheckError();
		}

		[Shared]
		private static _glReadPixels glReadPixels;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private unsafe delegate void _glReadPixels(GLint x, GLint y, GLint w, GLint h, PixelFormat format, DataType type, void* data);
		public static unsafe byte[] ReadPixels(RectangleI rect, PixelFormat format)
		{
			byte[] data = new byte[rect.Area * format.Components()];
			fixed (byte* ptr = data)
				glReadPixels(rect.X, rect.Y, rect.W, rect.H, format, DataType.UnsignedByte, ptr);
			CheckError();
			return data;
		}
		public static unsafe Colour[] ReadPixelsAsColourArray(RectangleI rect, PixelFormat format)
		{
			Colour[] data = new Colour[rect.Area];
			fixed (Colour* ptr = data)
				glReadPixels(rect.X, rect.Y, rect.W, rect.H, format, DataType.UnsignedByte, ptr);
			CheckError();
			return data;
		}
		#endregion

		#region Renderbuffer Functions
		[Shared]
		private static _glGenRenderbuffers glGenRenderbuffers;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private unsafe delegate void _glGenRenderbuffers(GLint n, GLuint* renderbuffers);
		public static unsafe void GenRenderbuffers(int n, uint[] renderbuffers)
		{
			fixed (uint* ptr = renderbuffers) { glGenFramebuffers(n, ptr); }
			CheckError();
		}
		public static unsafe uint GenRenderbuffer()
		{
			uint rbo = 0;
			glGenRenderbuffers(1, &rbo);
			CheckError();
			return rbo;
		}

		[Shared]
		private static _glDeleteRenderbuffers glDeleteRenderbuffers;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private unsafe delegate void _glDeleteRenderbuffers(GLint n, GLuint* renderbuffers);
		public static unsafe void DeleteRenderbuffers(int n, uint[] renderbuffers)
		{
			fixed (uint* ptr = renderbuffers) { glDeleteRenderbuffers(n, ptr); }
			CheckError();
		}
		public static unsafe void DeleteRenderbuffer(uint renderbuffer)
		{
			glDeleteRenderbuffers(1, &renderbuffer);
			CheckError();
		}

		[Shared]
		private static _glBindRenderbuffer glBindRenderbuffer;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glBindRenderbuffer(GLuint target, GLuint buffer);
		public static void BindRenderbuffer(uint buffer)
		{
			glBindRenderbuffer(GL_RENDERBUFFER, buffer);
			CheckError();
		}

		[Shared]
		private static _glRenderbufferStorage glRenderbufferStorage;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glRenderbufferStorage(GLuint target, TextureFormat internalformat, GLint width, GLint height);
		public static void RenderbufferStorage(TextureFormat internalformat, int width, int height)
		{
			glRenderbufferStorage(GL_RENDERBUFFER, internalformat, width, height);
			CheckError();
		}

		[Shared]
		private static _glFramebufferRenderbuffer glFramebufferRenderbuffer;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glFramebufferRenderbuffer(FramebufferTarget target, TextureAttachment attachment, GLuint renderbufferTarget, GLuint renderbuffer);
		public static void FramebufferRenderbuffer(FramebufferTarget target, TextureAttachment attachment, uint renderbuffer)
		{
			glFramebufferRenderbuffer(target, attachment, GL_RENDERBUFFER, renderbuffer);
			CheckError();
		}
		#endregion

		#region Vertex Array Functions
		[Shared]
		private static _glGenVertexArrays glGenVertexArrays;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private unsafe delegate void _glGenVertexArrays(GLint n, GLuint* arrays);
		public static unsafe void GenVertexArrays(int n, uint[] arrays)
		{
			fixed (uint* ptr = arrays) { glGenVertexArrays(n, ptr); }
			CheckError();
		}
		public static unsafe uint GenVertexArray()
		{
			uint vao = 0;
			glGenVertexArrays(1, &vao);
			CheckError();
			return vao;
		}

		[Shared]
		private static _glDeleteVertexArrays glDeleteVertexArrays;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private unsafe delegate void _glDeleteVertexArrays(GLint n, GLuint* arrays);
		public static unsafe void DeleteVertexArrays(int n, uint[] arrays)
		{
			fixed (uint* ptr = arrays) { glDeleteVertexArrays(n, ptr); }
			CheckError();
		}
		public static unsafe void DeleteVertexArray(uint array)
		{
			glDeleteVertexArrays(1, &array);
			CheckError();
		}

		[Shared]
		private static _glBindVertexArray glBindVertexArray;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glBindVertexArray(uint array);
		public static void BindVertexArray(uint array)
		{
			glBindVertexArray(array);
			CheckError();
		}
		#endregion

		#region Vertex Attribute Functions
		[Shared]
		private static _glVertexAttribPointer glVertexAttribPointer;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glVertexAttribPointer(GLuint index, GLint size, DataType type, bool normalized, GLint stride, IntPtr pointer);
		public static void VertexAttribPointer(uint index, int size, DataType type, bool normalized, int stride, IntPtr pointer)
		{
			glVertexAttribPointer(index, size, type, normalized, stride, pointer);
			CheckError();
		}
		public static void VertexAttribPointer(uint index, int size, DataType type, bool normalized, int stride, int pointer)
		{
			VertexAttribPointer(index, size, type, normalized, stride, new System.IntPtr(pointer));
		}

		[Shared]
		private static _glVertexAttribDivisor glVertexAttribDivisor;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glVertexAttribDivisor(GLuint index, GLuint divisor);
		public static void VertexAttribDivisor(uint index, uint divisor)
		{
			glVertexAttribDivisor(index, divisor);
			CheckError();
		}

		[Shared]
		private static _glEnableVertexAttribArray glEnableVertexAttribArray;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glEnableVertexAttribArray(GLuint index);
		public static void EnableVertexAttribArray(uint index)
		{
			glEnableVertexAttribArray(index);
			CheckError();
		}

		[Shared]
		private static _glDisableVertexAttribArray glDisableVertexAttribArray;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glDisableVertexAttribArray(GLuint index);
		public static void DisableVertexAttribArray(uint index)
		{
			glDisableVertexAttribArray(index);
			CheckError();
		}
		#endregion

		#region Shader Functions
		[Shared]
		private static _glCreateShader glCreateShader;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate GLuint _glCreateShader(ShaderType type);
		public static uint CreateShader(ShaderType type)
		{
			var shader = glCreateShader(type);
			CheckError();
			return shader;
		}

		[Shared]
		private static _glDeleteShader glDeleteShader;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glDeleteShader(GLuint shader);
		public static void DeleteShader(uint shader)
		{
			glDeleteShader(shader);
			CheckError();
		}

		[Shared]
		private static _glAttachShader glAttachShader;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glAttachShader(GLuint program, GLuint shader);
		public static void AttachShader(uint program, uint shader)
		{
			glAttachShader(program, shader);
			CheckError();
		}

		[Shared]
		private static _glDetachShader glDetachShader;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glDetachShader(GLuint program, GLuint shader);
		public static void DetachShader(uint program, uint shader)
		{
			glDetachShader(program, shader);
			CheckError();
		}

		[Shared]
		private static _glShaderSource glShaderSource;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glShaderSource(GLuint shader, GLint count, string[] source, GLint[] length);
		public static void ShaderSource(uint shader, string source)
		{
			glShaderSource(shader, 1, new string[] { source }, new int[] { source.Length });
			CheckError();
		}

		[Shared]
		private static _glCompileShader glCompileShader;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glCompileShader(GLuint shader);
		public static void CompileShader(uint shader)
		{
			glCompileShader(shader);
			CheckError();
		}

		[Shared]
		private static _glGetShaderiv glGetShaderiv;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glGetShaderiv(GLuint shader, ShaderParam pname, out GLint result);
		public static void GetShader(uint shader, ShaderParam pname, out int result)
		{
			glGetShaderiv(shader, pname, out result);
			CheckError();
		}

		[Shared]
		private static _glGetShaderInfoLog glGetShaderInfoLog;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glGetShaderInfoLog(GLuint shader, GLint maxLength, out GLint length, byte[] infoLog);
		public static string GetShaderInfoLog(uint shader)
		{
			GetShader(shader, ShaderParam.InfoLogLength, out int len);
			var bytes = new byte[len];
			glGetShaderInfoLog(shader, len, out len, bytes);
			CheckError();
			return Encoding.UTF8.GetString(bytes);
		}

		[Shared]
		private static _glCreateProgram glCreateProgram;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate GLuint _glCreateProgram();
		public static uint CreateProgram()
		{
			var program = glCreateProgram();
			CheckError();
			return program;
		}

		[Shared]
		private static _glDeleteProgram glDeleteProgram;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glDeleteProgram(GLuint program);
		public static void DeleteProgram(uint program)
		{
			glDeleteProgram(program);
			CheckError();
		}

		[Shared]
		private static _glLinkProgram glLinkProgram;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glLinkProgram(GLuint program);
		public static void LinkProgram(uint program)
		{
			glLinkProgram(program);
			CheckError();
		}

		[Shared]
		private static _glValidateProgram glValidateProgram;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glValidateProgram(GLuint program);
		public static void ValidateProgram(uint program)
		{
			glValidateProgram(program);
			CheckError();
		}

		[Shared]
		private static _glGetProgramiv glGetProgramiv;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glGetProgramiv(GLuint program, ProgramParam pname, out GLint result);
		public static void GetProgram(uint program, ProgramParam pname, out int result)
		{
			glGetProgramiv(program, pname, out result);
			CheckError();
		}

		[Shared]
		private static _glGetProgramInfoLog glGetProgramInfoLog;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glGetProgramInfoLog(GLuint program, GLint maxLength, out GLint length, byte[] infoLog);
		public static string GetProgramInfoLog(uint program)
		{
			GetProgram(program, ProgramParam.InfoLogLength, out int len);
			var bytes = new byte[len];
			glGetProgramInfoLog(program, len, out len, bytes);
			CheckError();
			return Encoding.UTF8.GetString(bytes);
		}

		[Shared]
		private static _glUseProgram glUseProgram;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUseProgram(GLuint program);
		public static void UseProgram(uint program)
		{
			glUseProgram(program);
			CheckError();
		}

		[Shared]
		private static _glGetAttribLocation glGetAttribLocation;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glGetAttribLocation(GLuint program, string name);
		public static void GetAttribLocation(uint program, string name)
		{
			glGetAttribLocation(program, name);
			CheckError();
		}

		[Shared]
		private static _glBindAttribLocation glBindAttribLocation;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glBindAttribLocation(GLuint program, GLuint index, string name);
		public static void BindAttribLocation(uint program, uint index, string name)
		{
			glBindAttribLocation(program, index, name);
			CheckError();
		}
		#endregion

		#region Shader Uniform Functions
		[Shared]
		private static _glGetActiveUniform glGetActiveUniform;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private unsafe delegate void _glGetActiveUniform(GLuint program, GLuint index, GLint bufSize, out GLint length, out GLint size, out UniformType type, byte* name);
		private static UniformType[] validUniformTypes = (UniformType[])Enum.GetValues(typeof(UniformType));
		private static byte[] uniformName = new byte[32];
		public static unsafe void GetActiveUniform(uint program, uint index, out int size, out UniformType type, out string name)
		{
			fixed (byte* ptr = uniformName)
			{
				glGetActiveUniform(program, index, uniformName.Length, out int length, out size, out type, ptr);
				name = length > 0 ? Encoding.UTF8.GetString(ptr, length) : null;
				if (!validUniformTypes.Contains(type))
					size = 0;
			}
			CheckError();
		}

		[Shared]
		private static _glGetUniformLocation glGetUniformLocation;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate GLint _glGetUniformLocation(GLuint program, string name);
		public static int GetUniformLocation(uint program, string name)
		{
			int location = glGetUniformLocation(program, name);
			CheckError();
			return location;
		}

		[Shared] private static _glUniform1f glUniform1f;
		[Shared] private static _glUniform2f glUniform2f;
		[Shared] private static _glUniform3f glUniform3f;
		[Shared] private static _glUniform4f glUniform4f;
		[Shared] private static _glUniform1fv glUniform1fv;
		[Shared] private static _glUniform2fv glUniform2fv;
		[Shared] private static _glUniform3fv glUniform3fv;
		[Shared] private static _glUniform4fv glUniform4fv;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform1f(GLint location, float v0);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform2f(GLint location, float v0, float v1);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform3f(GLint location, float v0, float v1, float v2);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform4f(GLint location, float v0, float v1, float v2, float v3);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform1fv(GLint location, GLint count, float[] value);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform2fv(GLint location, GLint count, float[] value);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform3fv(GLint location, GLint count, float[] value);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform4fv(GLint location, GLint count, float[] value);
		public static void Uniform1f(int location, float v0)
		{
			glUniform1f(location, v0);
			CheckError();
		}
		public static void Uniform2f(int location, float v0, float v1)
		{
			glUniform2f(location, v0, v1);
			CheckError();
		}
		public static void Uniform3f(int location, float v0, float v1, float v2)
		{
			glUniform3f(location, v0, v1, v2);
			CheckError();
		}
		public static void Uniform4f(int location, float v0, float v1, float v2, float v3)
		{
			glUniform4f(location, v0, v1, v2, v3);
			CheckError();
		}
		public static void Uniform1fv(int location, int count, float[] value)
		{
			glUniform1fv(location, count, value);
			CheckError();
		}
		public static void Uniform2fv(int location, int count, float[] value)
		{
			glUniform2fv(location, count, value);
			CheckError();
		}
		public static void Uniform3fv(int location, int count, float[] value)
		{
			glUniform3fv(location, count, value);
			CheckError();
		}
		public static void Uniform4fv(int location, int count, float[] value)
		{
			glUniform4fv(location, count, value);
			CheckError();
		}

		[Shared] private static _glUniform1i glUniform1i;
		[Shared] private static _glUniform2i glUniform2i;
		[Shared] private static _glUniform3i glUniform3i;
		[Shared] private static _glUniform4i glUniform4i;
		[Shared] private static _glUniform1iv glUniform1iv;
		[Shared] private static _glUniform2iv glUniform2iv;
		[Shared] private static _glUniform3iv glUniform3iv;
		[Shared] private static _glUniform4iv glUniform4iv;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform1i(GLint location, GLint v0);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform2i(GLint location, GLint v0, GLint v1);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform3i(GLint location, GLint v0, GLint v1, GLint v2);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform4i(GLint location, GLint v0, GLint v1, GLint v2, GLint v3);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform1iv(GLint location, GLint count, GLint[] value);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform2iv(GLint location, GLint count, GLint[] value);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform3iv(GLint location, GLint count, GLint[] value);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform4iv(GLint location, GLint count, GLint[] value);
		public static void Uniform1i(int location, int v0)
		{
			glUniform1i(location, v0);
			CheckError();
		}
		public static void Uniform2i(int location, int v0, int v1)
		{
			glUniform2i(location, v0, v1);
			CheckError();
		}
		public static void Uniform3i(int location, int v0, int v1, int v2)
		{
			glUniform3i(location, v0, v1, v2);
			CheckError();
		}
		public static void Uniform4i(int location, int v0, int v1, int v2, int v3)
		{
			glUniform4i(location, v0, v1, v2, v3);
			CheckError();
		}
		public static void Uniform1iv(int location, int count, int[] value)
		{
			glUniform1iv(location, count, value);
			CheckError();
		}
		public static void Uniform2iv(int location, int count, int[] value)
		{
			glUniform2iv(location, count, value);
			CheckError();
		}
		public static void Uniform3iv(int location, int count, int[] value)
		{
			glUniform3iv(location, count, value);
			CheckError();
		}
		public static void Uniform4iv(int location, int count, int[] value)
		{
			glUniform4iv(location, count, value);
			CheckError();
		}

		[Shared] private static _glUniform1ui glUniform1ui;
		[Shared] private static _glUniform2ui glUniform2ui;
		[Shared] private static _glUniform3ui glUniform3ui;
		[Shared] private static _glUniform4ui glUniform4ui;
		[Shared] private static _glUniform1uiv glUniform1uiv;
		[Shared] private static _glUniform2uiv glUniform2uiv;
		[Shared] private static _glUniform3uiv glUniform3uiv;
		[Shared] private static _glUniform4uiv glUniform4uiv;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform1ui(GLint location, GLuint v0);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform2ui(GLint location, GLuint v0, GLuint v1);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform3ui(GLint location, GLuint v0, GLuint v1, GLuint v2);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform4ui(GLint location, GLuint v0, GLuint v1, GLuint v2, GLuint v3);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform1uiv(GLint location, GLint count, GLuint[] value);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform2uiv(GLint location, GLint count, GLuint[] value);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform3uiv(GLint location, GLint count, GLuint[] value);
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniform4uiv(GLint location, GLint count, GLuint[] value);
		public static void Uniform1ui(int location, uint v0)
		{
			glUniform1ui(location, v0);
			CheckError();
		}
		public static void Uniform2ui(int location, uint v0, uint v1)
		{
			glUniform2ui(location, v0, v1);
			CheckError();
		}
		public static void Uniform3ui(int location, uint v0, uint v1, uint v2)
		{
			glUniform3ui(location, v0, v1, v2);
			CheckError();
		}
		public static void Uniform4ui(int location, uint v0, uint v1, uint v2, uint v3)
		{
			glUniform4ui(location, v0, v1, v2, v3);
			CheckError();
		}
		public static void Uniform1uiv(int location, int count, uint[] value)
		{
			glUniform1uiv(location, count, value);
			CheckError();
		}
		public static void Uniform2uiv(int location, int count, uint[] value)
		{
			glUniform2uiv(location, count, value);
			CheckError();
		}
		public static void Uniform3uiv(int location, int count, uint[] value)
		{
			glUniform3uiv(location, count, value);
			CheckError();
		}
		public static void Uniform4uiv(int location, int count, uint[] value)
		{
			glUniform4uiv(location, count, value);
			CheckError();
		}

		[Shared]
		private static _glUniformMatrix2fv glUniformMatrix2fv;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniformMatrix2fv(GLint location, GLint count, bool transpose, float[] value);
		public static void UniformMatrix2fv(int location, int count, bool transpose, float[] value)
		{
			glUniformMatrix2fv(location, count, transpose, value);
			CheckError();
		}

		[Shared]
		private static _glUniformMatrix3fv glUniformMatrix3fv;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniformMatrix3fv(GLint location, GLint count, bool transpose, float[] value);
		public static void UniformMatrix3fv(int location, int count, bool transpose, float[] value)
		{
			glUniformMatrix3fv(location, count, transpose, value);
			CheckError();
		}

		[Shared]
		private static _glUniformMatrix4fv glUniformMatrix4fv;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private unsafe delegate void _glUniformMatrix4fv(GLint location, GLint count, bool transpose, float[] value);
		public static void UniformMatrix4fv(int location, int count, bool transpose, float[] value)
		{
			glUniformMatrix4fv(location, count, transpose, value);
			CheckError();
		}
		//[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		//private unsafe delegate void _glUniformMatrix4fv(GLint location, GLint count, bool transpose, float* value);
		//public unsafe static void UniformMatrix4fv(int location, int count, bool transpose, float* value)
		//{
		//	glUniformMatrix4fv(location, count, transpose, value);
		//	CheckError();
		//}
		//public unsafe static void UniformMatrix4fv(int location, int count, bool transpose, float[] value)
		//{
		//	fixed (float* ptr = value) { glUniformMatrix4fv(location, count, transpose, ptr); }
		//	CheckError();
		//}

		[Shared]
		private static _glUniformMatrix2x3fv glUniformMatrix2x3fv;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniformMatrix2x3fv(GLint location, GLint count, bool transpose, float[] value);
		public static void UniformMatrix2x3fv(int location, int count, bool transpose, float[] value)
		{
			glUniformMatrix2x3fv(location, count, transpose, value);
			CheckError();
		}

		[Shared]
		private static _glUniformMatrix3x2fv glUniformMatrix3x2fv;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private unsafe delegate void _glUniformMatrix3x2fv(GLint location, GLint count, bool transpose, float[] value);
		public static void UniformMatrix3x2fv(int location, int count, bool transpose, float[] value)
		{
			glUniformMatrix3x2fv(location, count, transpose, value);
			CheckError();
		}
		//[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		//private unsafe delegate void _glUniformMatrix3x2fv(GLint location, GLint count, bool transpose, float* value);
		//public unsafe static void UniformMatrix3x2fv(int location, int count, bool transpose, float* value)
		//{
		//	glUniformMatrix3x2fv(location, count, transpose, value);
		//	CheckError();
		//}
		//public unsafe static void UniformMatrix3x2fv(int location, int count, bool transpose, float[] value)
		//{
		//	fixed (float* ptr = value) { glUniformMatrix3x2fv(location, count, transpose, ptr); }
		//	CheckError();
		//}

		[Shared]
		private static _glUniformMatrix2x4fv glUniformMatrix2x4fv;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniformMatrix2x4fv(GLint location, GLint count, bool transpose, float[] value);
		public static void UniformMatrix2x4fv(int location, int count, bool transpose, float[] value)
		{
			glUniformMatrix2x4fv(location, count, transpose, value);
			CheckError();
		}

		[Shared]
		private static _glUniformMatrix4x2fv glUniformMatrix4x2fv;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniformMatrix4x2fv(GLint location, GLint count, bool transpose, float[] value);
		public static void UniformMatrix4x2fv(int location, int count, bool transpose, float[] value)
		{
			glUniformMatrix4x2fv(location, count, transpose, value);
			CheckError();
		}

		[Shared]
		private static _glUniformMatrix3x4fv glUniformMatrix3x4fv;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniformMatrix3x4fv(GLint location, GLint count, bool transpose, float[] value);
		public static void UniformMatrix3x4fv(int location, int count, bool transpose, float[] value)
		{
			glUniformMatrix3x4fv(location, count, transpose, value);
			CheckError();
		}

		[Shared]
		private static _glUniformMatrix4x3fv glUniformMatrix4x3fv;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glUniformMatrix4x3fv(GLint location, GLint count, bool transpose, float[] value);
		public static void UniformMatrix4x3fv(int location, int count, bool transpose, float[] value)
		{
			glUniformMatrix4x3fv(location, count, transpose, value);
			CheckError();
		}
		#endregion

		#region Drawing Functions
		[Shared]
		private static _glDrawArrays glDrawArrays;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glDrawArrays(PrimitiveMode mode, GLint first, GLint count);
		public static void DrawArrays(PrimitiveMode mode, int start, int count)
		{
			glDrawArrays(mode, start, count);
			CheckError();
		}

		[Shared]
		private static _glDrawElements glDrawElements;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glDrawElements(PrimitiveMode mode, GLint count, IndexType type, IntPtr indices);
		public static void DrawElements(PrimitiveMode mode, int count, IndexType type, IntPtr offset)
		{
			glDrawElements(mode, count, type, offset);
			CheckError();
		}
		public static void DrawElements(PrimitiveMode mode, int count, IndexType type, int offset)
		{
			glDrawElements(mode, count, type, new IntPtr(offset));
			CheckError();
		}

		[Shared]
		private static _glDrawArraysInstanced glDrawArraysInstanced;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glDrawArraysInstanced(PrimitiveMode mode, GLint first, GLint count, GLint primcount);
		public static void DrawArraysInstanced(PrimitiveMode mode, int start, int count, int primcount)
		{
			glDrawArraysInstanced(mode, start, count, primcount);
			CheckError();
		}

		[Shared]
		private static _glDrawElementsInstanced glDrawElementsInstanced;
		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		private delegate void _glDrawElementsInstanced(PrimitiveMode mode, GLint count, IndexType type, IntPtr indices, GLint primcount);
		public static void DrawElementsInstanced(PrimitiveMode mode, int count, IndexType type, IntPtr offset, int primcount)
		{
			glDrawElementsInstanced(mode, count, type, offset, primcount);
			CheckError();
		}
		public static void DrawElementsInstanced(PrimitiveMode mode, int count, IndexType type, int offset, int primcount)
		{
			glDrawElementsInstanced(mode, count, type, new IntPtr(offset), primcount);
			CheckError();
		}
		#endregion

		#region Query Functions
		#endregion

		#region 3.2 Core Functions
		#endregion

#pragma warning restore IDE0044
#pragma warning restore CS0649
	}
}