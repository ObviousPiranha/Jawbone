#version 150

uniform sampler2D theTexture;
in vec2 _textureCoordinates;
out vec4 _result;

void main()
{
    _result = texture(theTexture, _textureCoordinates);
}
