#version 460

layout (location = 0) in vec3 aPos;
layout (location = 1) in vec4 aColor;
layout (location = 2) in vec2 aTexCoord;

out vec4 Color;
out vec2 TexCoord;

void main()
{
  gl_Position = vec4(aPos, 1.0);

  Color = aColor;
  TexCoord = aTexCoord;
}