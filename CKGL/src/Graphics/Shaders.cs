namespace CKGL
{
	public static class ShaderIncludes
	{
		#region Common
		private static readonly string OpenGL = "#version 330 core";
		private static readonly string OpenGLES = @"#version 300 es
precision mediump float;

//#extension GL_EXT_geometry_shader: require
//#extension GL_OES_geometry_shader: require";

		private static readonly string Common = (Platform.GraphicsBackend == GraphicsBackend.OpenGLES || Platform.GraphicsBackend == GraphicsBackend.WebGL ? OpenGLES : OpenGL) + @"

float fog_linear(const float dist, const float start, const float end)
{
	return 1.0 - clamp((end - dist) / (end - start), 0.0, 1.0);
}";
		#endregion

		#region Vertex
		public static readonly string Vertex = Common + @"";
		#endregion

		#region Geometry
		public static readonly string Geometry = Common + @"";
		#endregion

		#region Fragment
		public static readonly string Fragment = Common + @"
float fog_exp(const float dist, const float density)
{
	return 1.0 - clamp(exp(-density * dist), 0.0, 1.0);
}

float fog_exp2(const float dist, const float density)
{
	const float LOG2 = -1.442695;
	float d = density * dist;
	return 1.0 - clamp(exp2(d * d * LOG2), 0.0, 1.0);
}

float LinearizeDepth(const float depth, const float zNear, const float zFar)
{
    return 1.0 - (2.0 * zNear) / (zFar + zNear - depth * (zFar - zNear));
}";
		#endregion
	}

	public abstract class ShaderWrapper
	{
		public Shader Shader;

		public ShaderWrapper(string source) => Shader = Shader.Create(source);

		public ShaderWrapper(bool fromFile, string file) => Shader = Shader.CreateFromFile(file);

		public void Destroy() => Shader.Destroy();
		public void Bind() => Shader.Bind();

		public void SetUniform(string name, bool value) => Shader.SetUniform(name, value);
		public void SetUniform(string name, int value) => Shader.SetUniform(name, value);
		public void SetUniform(string name, float value) => Shader.SetUniform(name, value);
		public void SetUniform(string name, float x, float y) => Shader.SetUniform(name, x, y);
		public void SetUniform(string name, float x, float y, float z) => Shader.SetUniform(name, x, y, z);
		public void SetUniform(string name, float x, float y, float z, float w) => Shader.SetUniform(name, x, y, z, w);
		public void SetUniform(string name, Vector2 value) => Shader.SetUniform(name, value);
		public void SetUniform(string name, Vector3 value) => Shader.SetUniform(name, value);
		public void SetUniform(string name, Vector4 value) => Shader.SetUniform(name, value);
		public void SetUniform(string name, Colour value) => Shader.SetUniform(name, value);
		public void SetUniform(string name, Matrix2D value) => Shader.SetUniform(name, value);
		public void SetUniform(string name, Matrix3x3 value) => Shader.SetUniform(name, value);
		public void SetUniform(string name, Matrix value) => Shader.SetUniform(name, value);
		public void SetUniform(string name, Texture value, uint textureSlot) => Shader.SetUniform(name, value, textureSlot);
		//public void SetUniform(string name, UniformSampler2D value) => Shader.SetUniform(name, value);
		//public void SetUniform(string name, UniformSamplerCube value) => Shader.SetUniform(name, value);
	}

	public static class InternalShaders
	{
		#region Renderer
		public class RendererShader : ShaderWrapper
		{
			public RendererShader() : base(glsl) { }

			public Matrix MVP { set { SetUniform("MVP", value); } }

			#region GLSL
			private static string glsl = @"
#vertex

layout(location = 0) in vec3 position;
layout(location = 1) in vec4 colour;
layout(location = 2) in vec2 uv;
layout(location = 3) in float textured;

uniform mat4 MVP;

out vec4 vColour;
out vec2 vUV;
out float vTextured;

void main()
{
	gl_Position = vec4(position.xyz, 1.0) * MVP;
	vColour = colour;
	vUV = uv;
	vTextured = textured;
}


#fragment

layout(location = 0) out vec4 colour;

uniform sampler2D Texture;

in vec4 vColour;
in vec2 vUV;
in float vTextured;

void main()
{
	colour = mix(vColour, texture(Texture, vUV) * vColour, vTextured);
}";
			#endregion
		}
		#endregion

		#region RendererFog
		public enum FogType
		{
			LinearVertex,
			Linear,
			Exponential,
			Exponential2
		}

		public class RendererFogShader : ShaderWrapper
		{
			public RendererFogShader() : base(glsl) { }

			public Matrix MVP { set { SetUniform("MVP", value); } }
			public Matrix MV { set { SetUniform("MV", value); } }
			public FogType FogType { set { SetUniform("FogType", (int)value); } }
			public Colour FogColour { set { SetUniform("FogColour", value); } }
			public float FogDensity { set { SetUniform("FogDensity", value); } }
			public float FogStart { set { SetUniform("FogStart", value); } }
			public float FogEnd { set { SetUniform("FogEnd", value); } }

			#region GLSL
			private static string glsl = @"
#vertex

layout(location = 0) in vec3 position;
layout(location = 1) in vec4 colour;
layout(location = 2) in vec2 uv;
layout(location = 3) in float textured;

uniform mat4 MVP;
uniform mat4 MV;
uniform float FogStart;// = 20.0;
uniform float FogEnd;// = 50.0;

out vec4 vColour;
out vec2 vUV;
out float vTextured;
out vec4 vViewSpace;
out float vLinearFogAmount;

void main()
{
	gl_Position = vec4(position.xyz, 1.0) * MVP;
	vColour = colour;
	vUV = uv;
	vTextured = textured;
	vViewSpace = vec4(position.xyz, 1.0) * MV;
	vLinearFogAmount = fog_linear(length(vViewSpace), FogStart, FogEnd);
}


#fragment

layout(location = 0) out vec4 colour;

uniform sampler2D Texture;
uniform int FogType;// = 0;
uniform vec4 FogColour;// = vec4(0.0, 0.3, 0.5, 1.0);
uniform float FogDensity;// = 0.03;
uniform float FogStart;// = 20.0;
uniform float FogEnd;// = 50.0;

in vec4 vColour;
in vec2 vUV;
in float vTextured;
in vec4 vViewSpace;
in float vLinearFogAmount;

void main()
{
	colour = mix(vColour, texture(Texture, vUV) * vColour, vTextured);
	
	if(FogType == 0) // Fog - Linear Vertex
		colour.rgb = mix(colour.rgb, FogColour.rgb, vLinearFogAmount);
	else if(FogType == 1) // Fog - Linear
		colour.rgb = mix(colour.rgb, FogColour.rgb, fog_linear(length(vViewSpace), FogStart, FogEnd));
	else if(FogType == 2) // Fog - Exponential
		colour.rgb = mix(colour.rgb, FogColour.rgb, fog_exp(length(vViewSpace), FogDensity));
	else if(FogType == 3) // Fog - Exponential2
		colour.rgb = mix(colour.rgb, FogColour.rgb, fog_exp2(length(vViewSpace), FogDensity));
}";
			#endregion
		}
		#endregion

		#region LinearizeDepth
		public class LinearizeDepthShader : ShaderWrapper
		{
			public LinearizeDepthShader() : base(glsl) { }

			public Matrix MVP { set { SetUniform("MVP", value); } }
			public float zNear { set { SetUniform("zNear", value); } }
			public float zFar { set { SetUniform("zFar", value); } }

			#region GLSL
			private static string glsl = @"
#vertex

layout(location = 0) in vec3 position;
layout(location = 1) in vec4 colour;
layout(location = 2) in vec2 uv;

uniform mat4 MVP;

out vec4 vColour;
out vec2 vUV;

void main()
{
	gl_Position = vec4(position.xyz, 1.0) * MVP;
	vColour = colour;
	vUV = uv;
}


#fragment

layout(location = 0) out vec4 colour;

uniform sampler2D Texture;
uniform float zNear;// = 0.5;
uniform float zFar;// = 1000.0;

in vec4 vColour;
in vec2 vUV;

void main()
{
	float c = LinearizeDepth(texture(Texture, vUV).x, zNear, zFar);
	colour = vec4(c, c, c, 1.0) * vColour;
}";
			#endregion
		}
		#endregion
	}
}