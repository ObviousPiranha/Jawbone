#version 150

uniform mat4 theMatrix;
in vec2 position;
in vec2 textureCoordinates;
out vec2 _textureCoordinates;

void main()
{
    _textureCoordinates = textureCoordinates;
    gl_Position = theMatrix * vec4(position, 0.0, 1.0);
}
