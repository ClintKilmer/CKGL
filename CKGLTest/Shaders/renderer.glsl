#version 330 core
uniform mat4 matrix;
layout(location = 0) in vec3 vertexPosition;
layout(location = 1) in vec4 vertexColour;
layout(location = 2) in vec2 vertexUV;
layout(location = 3) in float vertexTextured;
out vec4 fragColour;
out vec2 fragUV;
out float fragTextured;
void main()
{
	gl_Position = vec4(vertexPosition.xyz, 1.0);// * matrix;
	fragColour = vertexColour;
	fragUV = vertexUV;
	fragTextured = vertexTextured;
}
...
#version 330 core
uniform sampler2D Texture;
in vec4 fragColour;
in vec2 fragUV;
in float fragTextured;
layout(location = 0) out vec4 colour;
void main()
{
    if (fragTextured > 0.0)
		colour = texture(Texture, fragUV) * fragColour;
    else
        colour = fragColour;
}