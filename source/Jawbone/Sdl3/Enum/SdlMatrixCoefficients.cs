namespace Jawbone.Sdl3;

public enum SdlMatrixCoefficients // SDL_MatrixCoefficients
{
    Identity = 0, // SDL_MATRIX_COEFFICIENTS_IDENTITY
    Bt709 = 1, // SDL_MATRIX_COEFFICIENTS_BT709
    Unspecified = 2, // SDL_MATRIX_COEFFICIENTS_UNSPECIFIED
    Fcc = 4, // SDL_MATRIX_COEFFICIENTS_FCC
    Bt470Bg = 5, // SDL_MATRIX_COEFFICIENTS_BT470BG
    Bt601 = 6, // SDL_MATRIX_COEFFICIENTS_BT601
    Smpte240 = 7, // SDL_MATRIX_COEFFICIENTS_SMPTE240
    Ycgco = 8, // SDL_MATRIX_COEFFICIENTS_YCGCO
    Bt2020Ncl = 9, // SDL_MATRIX_COEFFICIENTS_BT2020_NCL
    Bt2020Cl = 10, // SDL_MATRIX_COEFFICIENTS_BT2020_CL
    Smpte2085 = 11, // SDL_MATRIX_COEFFICIENTS_SMPTE2085
    ChromaDerivedNcl = 12, // SDL_MATRIX_COEFFICIENTS_CHROMA_DERIVED_NCL
    ChromaDerivedCl = 13, // SDL_MATRIX_COEFFICIENTS_CHROMA_DERIVED_CL
    Ictcp = 14, // SDL_MATRIX_COEFFICIENTS_ICTCP
    Custom = 31, // SDL_MATRIX_COEFFICIENTS_CUSTOM
}
