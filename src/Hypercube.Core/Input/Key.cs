namespace Hypercube.Core.Input;

// Source: https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
[PublicAPI]
public enum Key : short
{
    Any = -1,
    
    #region Letters
    
    A = 65, // A
    B = 66, // B
    C = 67, // C
    D = 68, // D
    E = 69, // E
    F = 70, // F
    G = 71, // G
    H = 72, // H
    I = 73, // I
    J = 74, // J
    K = 75, // K
    L = 76, // L
    M = 77, // M
    N = 78, // N
    O = 79, // O
    P = 80, // P
    Q = 81, // Q
    R = 82, // R
    S = 83, // S
    T = 84, // T
    U = 85, // U
    V = 86, // V
    W = 87, // W
    X = 88, // X
    Y = 89, // Y
    Z = 90, // Z
    
    #endregion

    #region Digits
    
    Digit0 = 48, // 0
    Digit1 = 49, // 1
    Digit2 = 50, // 2
    Digit3 = 51, // 3
    Digit4 = 52, // 4
    Digit5 = 53, // 5
    Digit6 = 54, // 6
    Digit7 = 55, // 7
    Digit8 = 56, // 8
    Digit9 = 57, // 9
    
    #endregion

    #region Special
    
    Apostrophe = 39,   // '
    Quote = 34,        // "
    Comma = 44,        // ,
    Minus = 45,        // -
    Period = 46,       // .
    Slash = 47,        // /
    Semicolon = 59,    // ;
    Equal = 61,        // =
    LeftBracket = 91,  // [
    Backslash = 92,    // \
    RightBracket = 93, // ]
    GraveAccent = 96,  // `
    Space = 32,
    
    #endregion

    #region Navigation / Control
    
    Escape = 256,
    Enter = 257,
    Tab = 258,
    Backspace = 259,
    Insert = 260,
    Delete = 261,
    Right = 262,
    Left = 263,
    Down = 264,
    Up = 265,
    PageUp = 266,
    PageDown = 267,
    Home = 268,
    End = 269,
    CapsLock = 280,
    ScrollLock = 281,
    NumLock = 282,
    PrintScreen = 283,
    Pause = 284,
    
    #endregion

    #region Function
    
    F1 = 290,
    F2 = 291,
    F3 = 292,
    F4 = 293,
    F5 = 294,
    F6 = 295,
    F7 = 296,
    F8 = 297,
    F9 = 298,
    F10 = 299,
    F11 = 300,
    F12 = 301,
    F13 = 302,
    F14 = 303,
    F15 = 304,
    F16 = 305,
    F17 = 306,
    F18 = 307,
    F19 = 308,
    F20 = 309,
    F21 = 310,
    F22 = 311,
    F23 = 312,
    F24 = 313,
    F25 = 314,
    
    #endregion

    #region Numpad
    
    Numpad0 = 320,       // 0
    Numpad1 = 321,       // 1
    Numpad2 = 322,       // 2
    Numpad3 = 323,       // 3
    Numpad4 = 324,       // 4
    Numpad5 = 325,       // 5
    Numpad6 = 326,       // 6
    Numpad7 = 327,       // 7
    Numpad8 = 328,       // 8
    Numpad9 = 329,       // 9

    #endregion

    #region Keypad (Numpad operations)
    
    KeyPadDecimal = 330,   // .
    KeyPadDivide = 331,    // /
    KeyPadMultiply = 332,  // *
    KeyPadSubtract = 333,  // -
    KeyPadAdd = 334,       // +
    KeyPadEnter = 335,     // Enter
    KeyPadEqual = 336,     // =
    
    #endregion

    #region Modifier / Special keys
    
    LeftShift = 340,
    LeftControl = 341,
    LeftAlt = 342,
    LeftSuper = 343,    // "Win" (Windows), "Command" (MacOS)
    RightShift = 344,
    RightControl = 345,
    RightAlt = 346,
    RightSuper = 347,   // "Win" (Windows), "Command" (MacOS)
    Menu = 348,
    Fn = 349,
    FnLock = 350,
    
    #endregion
    
}
