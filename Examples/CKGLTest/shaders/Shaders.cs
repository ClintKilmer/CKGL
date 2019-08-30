using CKGL;

namespace CKGLTest
{
	#region GeometryTestShader
	public class GeometryTestShader : ShaderWrapper
	{
		public GeometryTestShader() : base(glsl) { }

		public Matrix MVP { set { SetUniform("MVP", value); } }

		#region GLSL
		private static string glsl = @"
#vertex

layout(location = 0) in vec3 position;
layout(location = 1) in vec4 colour;
layout(location = 2) in vec2 uv;
layout(location = 3) in float textured;

out DATA
{
	vec4 colour;
	vec2 uv;
	float textured;
} o;

void main()
{
	gl_Position = vec4(position.xyz, 1.0);
	o.colour = colour;
	o.uv = uv;
	o.textured = textured;
}


#geometry

layout (points) in;
layout (triangle_strip, max_vertices = 6) out;

uniform mat4 MVP;

in DATA
{
	vec4 colour;
	vec2 uv;
	float textured;
} vertices[];

out DATA
{
	vec4 colour;
	vec2 uv;
	float textured;
} o;   

void main(void)
{
	int i;
	for(i = 0; i < gl_in.length(); i++) // gl_in.length() = 1
	{
		o.colour = vertices[i].colour;
		o.uv = vertices[i].uv;
		o.textured = vertices[i].textured;
		gl_Position = (gl_in[i].gl_Position + vec4(-0.5, -0.5, 0, 0)) * MVP;
		EmitVertex();

		o.colour = vertices[i].colour;
		o.uv = vertices[i].uv;
		o.textured = vertices[i].textured;
		gl_Position = (gl_in[i].gl_Position + vec4(0.5, -0.5, 0, 0)) * MVP;
		EmitVertex();

		o.colour = vertices[i].colour;
		o.uv = vertices[i].uv;
		o.textured = vertices[i].textured;
		gl_Position = (gl_in[i].gl_Position + vec4(0.5, 0.5, 0, 0)) * MVP;
		EmitVertex();
	}
	EndPrimitive();
	for(i = 0; i < gl_in.length(); i++) // gl_in.length() = 1
	{
		o.colour = vertices[i].colour;
		o.uv = vertices[i].uv;
		o.textured = vertices[i].textured;
		gl_Position = (gl_in[i].gl_Position + vec4(0.5, 0.5, 0, 0)) * MVP;
		EmitVertex();

		o.colour = vertices[i].colour;
		o.uv = vertices[i].uv;
		o.textured = vertices[i].textured;
		gl_Position = (gl_in[i].gl_Position + vec4(-0.5, 0.5, 0, 0)) * MVP;
		EmitVertex();

		o.colour = vertices[i].colour;
		o.uv = vertices[i].uv;
		o.textured = vertices[i].textured;
		gl_Position = (gl_in[i].gl_Position + vec4(-0.5, -0.5, 0, 0)) * MVP;
		EmitVertex();
	}
	EndPrimitive();
}


#fragment

layout(location = 0) out vec4 colour;

uniform sampler2D Texture;

in DATA
{
	vec4 colour;
	vec2 uv;
	float textured;
} i;

void main()
{
	colour = mix(i.colour, texture(Texture, i.uv) * i.colour, i.textured);
}";
		#endregion
	}
	#endregion
}