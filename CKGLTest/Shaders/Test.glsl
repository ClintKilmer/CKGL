#version 330 core
uniform vec2 offset;
layout(location = 0) in vec3 position;
layout(location = 1) in vec4 colour;
out vec4 vertexColour;
void main()
{
	vertexColour = colour;
	gl_Position = vec4(position.x + offset.x, position.y + offset.y, position.z, 1.0);
}
...
#version 330 core
in vec4 vertexColour;
out vec4 color;
void main()
{
	color = vertexColour;
}