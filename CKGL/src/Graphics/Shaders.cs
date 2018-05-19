namespace CKGL
{
	public static class InternalShaders
	{
		#region Renderer
		#region Original - Commented out
		//		public static Shader Renderer = new Shader(@"
		//#version 330 core
		//uniform mat4 MVP;
		//layout(location = 0) in vec3 position;
		//layout(location = 1) in vec4 colour;
		//layout(location = 2) in vec2 texCoord;
		//layout(location = 3) in float textured;
		//out vec4 v_colour;
		//out vec2 v_texCoord;
		//out float v_textured;
		//void main()
		//{
		//	gl_Position = vec4(position.xyz, 1.0) * MVP;
		//	v_colour = colour;
		//	v_texCoord = texCoord;
		//	v_textured = textured;
		//}
		//...
		//#version 330 core
		//uniform sampler2D Texture;
		//in vec4 v_colour;
		//in vec2 v_texCoord;
		//in float v_textured;
		//layout(location = 0) out vec4 colour;
		//void main()
		//{
		//	if (v_textured > 0.0)
		//		colour = texture(Texture, v_texCoord) * v_colour;
		//	else
		//		colour = v_colour;
		//}");
		#endregion

		public static RendererShader Renderer = new RendererShader();
		public class RendererShader : Shader
		{
			#region GLSL
			private static string glsl = @"
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
}";
			#endregion

			private Matrix mvp;
			public Matrix MVP
			{
				get { return mvp; }
				set
				{
					if (mvp != value)
					{
						mvp = value;
						SetUniform("MVP", value);
					}
				}
			}

			public RendererShader() : base(glsl)
			{
				MVP = Matrix.Identity;
			}
		}
		#endregion

		#region LinearizeDepth
		public static LinearizeDepthShader LinearizeDepth = new LinearizeDepthShader();
		public class LinearizeDepthShader : Shader
		{
			#region GLSL
			private static string glsl = @"
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
uniform float zNear;
uniform float zFar;
in vec4 v_colour;
in vec2 v_texCoord;
in float v_textured;
layout(location = 0) out vec4 colour;
float LinearizeDepth(in float depth, in float zNear, in float zFar)
{
    return (2.0 * zNear) / (zFar + zNear - depth * (zFar - zNear));
}
void main()
{
	float c = LinearizeDepth(texture(Texture, v_texCoord).x, zNear, zFar);
	colour = vec4(c, c, c, 1.0) * v_colour;
}";
			#endregion

			private Matrix mvp;
			public Matrix MVP
			{
				get { return mvp; }
				set
				{
					if (mvp != value)
					{
						mvp = value;
						SetUniform("MVP", value);
					}
				}
			}

			private float znear;
			public float zNear
			{
				get { return znear; }
				set
				{
					if (znear != value)
					{
						znear = value;
						SetUniform("zNear", value);
					}
				}
			}

			private float zfar;
			public float zFar
			{
				get { return zfar; }
				set
				{
					if (zfar != value)
					{
						zfar = value;
						SetUniform("zFar", value);
					}
				}
			}

			public LinearizeDepthShader() : base(glsl)
			{
				MVP = Matrix.Identity;
			}
		}
		#endregion
	}
}