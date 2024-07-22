#version 460

out vec4 FragColor;

in vec4 Color;
in vec2 TexCoord;

uniform sampler2D texture0;

void main()
{
    FragColor = Color;
}