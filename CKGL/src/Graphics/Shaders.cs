namespace CKGL
{
	public static class Shaders
	{
		#region Renderer
		public static Shader Renderer = new Shader(@"
#version 330 core
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
...
#version 330 core
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
}");
	}
	#endregion
}