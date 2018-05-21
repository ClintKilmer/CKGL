namespace CKGL
{
	public static class ShaderIncludes
	{
		#region Common
		private static string Common = @"#version 330 core

float fog_linear(const float dist, const float start, const float end)
{
	return 1.0 - clamp((end - dist) / (end - start), 0.0, 1.0);
}";
		#endregion

		#region Vertex
		public static string Vertex = Common + @"";
		#endregion

		#region Fragment
		public static string Fragment = Common + @"
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
    return (2.0 * zNear) / (zFar + zNear - depth * (zFar - zNear));
}";
		#endregion
	}

	public static class InternalShaders
	{
		#region Renderer
		public static RendererShader Renderer = new RendererShader();
		public class RendererShader : Shader
		{
			#region GLSL
			private static string glsl = @"
#vertex

uniform mat4 MVP;

layout(location = 0) in vec3 position;
layout(location = 1) in vec4 colour;
layout(location = 2) in vec2 texCoord;
layout(location = 3) in float textured;

out vec4 v_colour;
out vec2 v_texCoord;
out float v_textured;

void main()
{
	gl_Position = vec4(position.xyz, 1.0) * MVP;
	v_colour = colour;
	v_texCoord = texCoord;
	v_textured = textured;
}


#fragment

uniform sampler2D Texture;

in vec4 v_colour;
in vec2 v_texCoord;
in float v_textured;

layout(location = 0) out vec4 colour;

void main()
{
    if (v_textured > 0.0)
		colour = texture(Texture, v_texCoord) * v_colour;
    else
        colour = v_colour;
}";
			#endregion

			public Matrix MVP { set { SetUniform("MVP", value); } }

			public RendererShader() : base(glsl) { }
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
		public static RendererFogShader RendererFog = new RendererFogShader();
		public class RendererFogShader : Shader
		{
			#region GLSL
			private static string glsl = @"
#vertex

uniform mat4 MVP;
uniform mat4 MV;
uniform float FogStart = 20.0;
uniform float FogEnd = 50.0;

layout(location = 0) in vec3 position;
layout(location = 1) in vec4 colour;
layout(location = 2) in vec2 texCoord;
layout(location = 3) in float textured;

out vec4 v_colour;
out vec2 v_texCoord;
out float v_textured;
out float v_linearFogAmount;
out vec4 v_viewSpace;

void main()
{
	gl_Position = vec4(position.xyz, 1.0) * MVP;
	v_colour = colour;
	v_texCoord = texCoord;
	v_textured = textured;
	v_viewSpace = vec4(position.xyz, 1.0) * MV;
	v_linearFogAmount = fog_linear(length(v_viewSpace), FogStart, FogEnd);
}


#fragment

uniform sampler2D Texture;
uniform int FogType = 0;
uniform vec4 FogColour = vec4(0.0, 0.3, 0.5, 1.0);
uniform float FogDensity = 0.03;
uniform float FogStart = 20.0;
uniform float FogEnd = 50.0;

in vec4 v_colour;
in vec2 v_texCoord;
in float v_textured;
in vec4 v_viewSpace;
in float v_linearFogAmount;

layout(location = 0) out vec4 colour;

void main()
{
    if (v_textured > 0.0)
		colour = texture(Texture, v_texCoord) * v_colour;
    else
        colour = v_colour;
	
	if(FogType == 0) // Fog - Linear Vertex
		colour.rgb = mix(colour.rgb, FogColour.rgb, v_linearFogAmount);
	else if(FogType == 1) // Fog - Linear
		colour.rgb = mix(colour.rgb, FogColour.rgb, fog_linear(length(v_viewSpace), FogStart, FogEnd));
	else if(FogType == 2) // Fog - Exponential
		colour.rgb = mix(colour.rgb, FogColour.rgb, fog_exp(length(v_viewSpace), FogDensity));
	else if(FogType == 3) // Fog - Exponential2
		colour.rgb = mix(colour.rgb, FogColour.rgb, fog_exp2(length(v_viewSpace), FogDensity));
}";
			#endregion

			public Matrix MVP { set { SetUniform("MVP", value); } }
			public Matrix MV { set { SetUniform("MV", value); } }
			public FogType FogType { set { SetUniform("FogType", (int)value); } }
			public Colour FogColour { set { SetUniform("FogColour", value); } }
			public float FogDensity { set { SetUniform("FogDensity", value); } }
			public float FogStart { set { SetUniform("FogStart", value); } }
			public float FogEnd { set { SetUniform("FogEnd", value); } }

			public RendererFogShader() : base(glsl) { }
		}
		#endregion

		#region LinearizeDepth
		public static LinearizeDepthShader LinearizeDepth = new LinearizeDepthShader();
		public class LinearizeDepthShader : Shader
		{
			#region GLSL
			private static string glsl = @"
#vertex

uniform mat4 MVP;

layout(location = 0) in vec3 position;
layout(location = 1) in vec4 colour;
layout(location = 2) in vec2 texCoord;
layout(location = 3) in float textured;

out vec4 v_colour;
out vec2 v_texCoord;
out float v_textured;

void main()
{
	gl_Position = vec4(position.xyz, 1.0) * MVP;
	v_colour = colour;
	v_texCoord = texCoord;
	v_textured = textured;
}


#fragment

uniform sampler2D Texture;
uniform float zNear;
uniform float zFar;

in vec4 v_colour;
in vec2 v_texCoord;
in float v_textured;

layout(location = 0) out vec4 colour;

void main()
{
	float c = LinearizeDepth(texture(Texture, v_texCoord).x, zNear, zFar);
	colour = vec4(c, c, c, 1.0) * v_colour;
}";
			#endregion

			public Matrix MVP { set { SetUniform("MVP", value); } }
			public float zNear { set { SetUniform("zNear", value); } }
			public float zFar { set { SetUniform("zFar", value); } }

			public LinearizeDepthShader() : base(glsl) { }
		}
		#endregion
	}
}