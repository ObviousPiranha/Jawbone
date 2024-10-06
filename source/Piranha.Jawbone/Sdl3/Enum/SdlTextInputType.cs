namespace Piranha.Jawbone.Sdl3;

public enum SdlTextInputType // SDL_TextInputType
{
    Text = 0, // SDL_TEXTINPUT_TYPE_TEXT
    TextName = 1, // SDL_TEXTINPUT_TYPE_TEXT_NAME
    TextEmail = 2, // SDL_TEXTINPUT_TYPE_TEXT_EMAIL
    TextUsername = 3, // SDL_TEXTINPUT_TYPE_TEXT_USERNAME
    TextPasswordHidden = 4, // SDL_TEXTINPUT_TYPE_TEXT_PASSWORD_HIDDEN
    TextPasswordVisible = 5, // SDL_TEXTINPUT_TYPE_TEXT_PASSWORD_VISIBLE
    Number = 6, // SDL_TEXTINPUT_TYPE_NUMBER
    NumberPasswordHidden = 7, // SDL_TEXTINPUT_TYPE_NUMBER_PASSWORD_HIDDEN
    NumberPasswordVisible = 8, // SDL_TEXTINPUT_TYPE_NUMBER_PASSWORD_VISIBLE
}
