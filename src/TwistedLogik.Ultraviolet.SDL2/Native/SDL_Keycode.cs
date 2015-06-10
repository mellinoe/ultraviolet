﻿#pragma warning disable 1591

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    public enum SDL_Keycode
    {
        UNKNOWN = 0,
        RETURN = '\r',
        ESCAPE = '\x1B',
        BACKSPACE = '\b',
        TAB = '\t',
        SPACE = ' ',
        EXCLAIM = '!',
        QUOTEDBL = '"',
        HASH = '#',
        PERCENT = '%',
        DOLLAR = '$',
        AMPERSAND = '&',
        QUOTE = '\'',
        LEFTPAREN = '(',
        RIGHTPAREN = ')',
        ASTERISK = '*',
        PLUS = '+',
        COMMA = ',',
        MINUS = '-',
        PERIOD = '.',
        SLASH = '/',
        D0 = '0',
        D1 = '1',
        D2 = '2',
        D3 = '3',
        D4 = '4',
        D5 = '5',
        D6 = '6',
        D7 = '7',
        D8 = '8',
        D9 = '9',
        COLON = ':',
        SEMICOLON = ';',
        LESS = '<',
        EQUALS = '=',
        GREATER = '>',
        QUESTION = '?',
        AT = '@',
        LEFTBRACKET = '[',
        BACKSLASH = '\\',
        RIGHTBRACKET = ']',
        CARET = '^',
        UNDERSCORE = '_',
        BACKQUOTE = '`',
        A = 'a',
        B = 'b',
        C = 'c',
        D = 'd',
        E = 'e',
        F = 'f',
        G = 'g',
        H = 'h',
        I = 'i',
        J = 'j',
        K = 'k',
        L = 'l',
        M = 'm',
        N = 'n',
        O = 'o',
        P = 'p',
        Q = 'q',
        R = 'r',
        S = 's',
        T = 't',
        U = 'u',
        V = 'v',
        W = 'w',
        X = 'x',
        Y = 'y',
        Z = 'z',
        CAPSLOCK = SDL_Scancode.CAPSLOCK | 0x40000000,
        F1 = SDL_Scancode.F1 | 0x40000000,
        F2 = SDL_Scancode.F2 | 0x40000000,
        F3 = SDL_Scancode.F3 | 0x40000000,
        F4 = SDL_Scancode.F4 | 0x40000000,
        F5 = SDL_Scancode.F5 | 0x40000000,
        F6 = SDL_Scancode.F6 | 0x40000000,
        F7 = SDL_Scancode.F7 | 0x40000000,
        F8 = SDL_Scancode.F8 | 0x40000000,
        F9 = SDL_Scancode.F9 | 0x40000000,
        F10 = SDL_Scancode.F10 | 0x40000000,
        F11 = SDL_Scancode.F11 | 0x40000000,
        F12 = SDL_Scancode.F12 | 0x40000000,
        PRINTSCREEN = SDL_Scancode.PRINTSCREEN | 0x40000000,
        SCROLLLOCK = SDL_Scancode.SCROLLLOCK | 0x40000000,
        PAUSE = SDL_Scancode.PAUSE | 0x40000000,
        INSERT = SDL_Scancode.INSERT | 0x40000000,
        HOME = SDL_Scancode.HOME | 0x40000000,
        PAGEUP = SDL_Scancode.PAGEUP | 0x40000000,
        DELETE = '\x7F',
        END = SDL_Scancode.END | 0x40000000,
        PAGEDOWN = SDL_Scancode.PAGEDOWN | 0x40000000,
        RIGHT = SDL_Scancode.RIGHT | 0x40000000,
        LEFT = SDL_Scancode.LEFT | 0x40000000,
        DOWN = SDL_Scancode.DOWN | 0x40000000,
        UP = SDL_Scancode.UP | 0x40000000,
        NUMLOCKCLEAR = SDL_Scancode.NUMLOCKCLEAR | 0x40000000,
        KP_DIVIDE = SDL_Scancode.KP_DIVIDE | 0x40000000,
        KP_MULTIPLY = SDL_Scancode.KP_MULTIPLY | 0x40000000,
        KP_MINUS = SDL_Scancode.KP_MINUS | 0x40000000,
        KP_PLUS = SDL_Scancode.KP_PLUS | 0x40000000,
        KP_ENTER = SDL_Scancode.KP_ENTER | 0x40000000,
        KP_D1 = SDL_Scancode.KP_D1 | 0x40000000,
        KP_D2 = SDL_Scancode.KP_D2 | 0x40000000,
        KP_D3 = SDL_Scancode.KP_D3 | 0x40000000,
        KP_D4 = SDL_Scancode.KP_D4 | 0x40000000,
        KP_D5 = SDL_Scancode.KP_D5 | 0x40000000,
        KP_D6 = SDL_Scancode.KP_D6 | 0x40000000,
        KP_D7 = SDL_Scancode.KP_D7 | 0x40000000,
        KP_D8 = SDL_Scancode.KP_D8 | 0x40000000,
        KP_D9 = SDL_Scancode.KP_D9 | 0x40000000,
        KP_D0 = SDL_Scancode.KP_D0 | 0x40000000,
        KP_PERIOD = SDL_Scancode.KP_PERIOD | 0x40000000,
        APPLICATION = SDL_Scancode.APPLICATION | 0x40000000,
        POWER = SDL_Scancode.POWER | 0x40000000,
        KP_EQUALS = SDL_Scancode.KP_EQUALS | 0x40000000,
        F13 = SDL_Scancode.F13 | 0x40000000,
        F14 = SDL_Scancode.F14 | 0x40000000,
        F15 = SDL_Scancode.F15 | 0x40000000,
        F16 = SDL_Scancode.F16 | 0x40000000,
        F17 = SDL_Scancode.F17 | 0x40000000,
        F18 = SDL_Scancode.F18 | 0x40000000,
        F19 = SDL_Scancode.F19 | 0x40000000,
        F20 = SDL_Scancode.F20 | 0x40000000,
        F21 = SDL_Scancode.F21 | 0x40000000,
        F22 = SDL_Scancode.F22 | 0x40000000,
        F23 = SDL_Scancode.F23 | 0x40000000,
        F24 = SDL_Scancode.F24 | 0x40000000,
        EXECUTE = SDL_Scancode.EXECUTE | 0x40000000,
        HELP = SDL_Scancode.HELP | 0x40000000,
        MENU = SDL_Scancode.MENU | 0x40000000,
        SELECT = SDL_Scancode.SELECT | 0x40000000,
        STOP = SDL_Scancode.STOP | 0x40000000,
        AGAIN = SDL_Scancode.AGAIN | 0x40000000,
        UNDO = SDL_Scancode.UNDO | 0x40000000,
        CUT = SDL_Scancode.CUT | 0x40000000,
        COPY = SDL_Scancode.COPY | 0x40000000,
        PASTE = SDL_Scancode.PASTE | 0x40000000,
        FIND = SDL_Scancode.FIND | 0x40000000,
        MUTE = SDL_Scancode.MUTE | 0x40000000,
        VOLUMEUP = SDL_Scancode.VOLUMEUP | 0x40000000,
        VOLUMEDOWN = SDL_Scancode.VOLUMEDOWN | 0x40000000,
        KP_COMMA = SDL_Scancode.KP_COMMA | 0x40000000,
        KP_EQUALSAS400 = SDL_Scancode.KP_EQUALSAS400 | 0x40000000,
        ALTERASE = SDL_Scancode.ALTERASE | 0x40000000,
        SYSREQ = SDL_Scancode.SYSREQ | 0x40000000,
        CANCEL = SDL_Scancode.CANCEL | 0x40000000,
        CLEAR = SDL_Scancode.CLEAR | 0x40000000,
        PRIOR = SDL_Scancode.PRIOR | 0x40000000,
        RETURN2 = SDL_Scancode.RETURN2 | 0x40000000,
        SEPARATOR = SDL_Scancode.SEPARATOR | 0x40000000,
        OUT = SDL_Scancode.OUT | 0x40000000,
        OPER = SDL_Scancode.OPER | 0x40000000,
        CLEARAGAIN = SDL_Scancode.CLEARAGAIN | 0x40000000,
        CRSEL = SDL_Scancode.CRSEL | 0x40000000,
        EXSEL = SDL_Scancode.EXSEL | 0x40000000,
        KP_00 = SDL_Scancode.KP_00 | 0x40000000,
        KP_000 = SDL_Scancode.KP_000 | 0x40000000,
        THOUSANDSSEPARATOR = SDL_Scancode.THOUSANDSSEPARATOR | 0x40000000,
        DECIMALSEPARATOR = SDL_Scancode.DECIMALSEPARATOR | 0x40000000,
        CURRENCYUNIT = SDL_Scancode.CURRENCYUNIT | 0x40000000,
        CURRENCYSUBUNIT = SDL_Scancode.CURRENCYSUBUNIT | 0x40000000,
        KP_LEFTPAREN = SDL_Scancode.KP_LEFTPAREN | 0x40000000,
        KP_RIGHTPAREN = SDL_Scancode.KP_RIGHTPAREN | 0x40000000,
        KP_LEFTBRACE = SDL_Scancode.KP_LEFTBRACE | 0x40000000,
        KP_RIGHTBRACE = SDL_Scancode.KP_RIGHTBRACE | 0x40000000,
        KP_TAB = SDL_Scancode.KP_TAB | 0x40000000,
        KP_BACKSPACE = SDL_Scancode.KP_BACKSPACE | 0x40000000,
        KP_A = SDL_Scancode.KP_A | 0x40000000,
        KP_B = SDL_Scancode.KP_B | 0x40000000,
        KP_C = SDL_Scancode.KP_C | 0x40000000,
        KP_D = SDL_Scancode.KP_D | 0x40000000,
        KP_E = SDL_Scancode.KP_E | 0x40000000,
        KP_F = SDL_Scancode.KP_F | 0x40000000,
        KP_XOR = SDL_Scancode.KP_XOR | 0x40000000,
        KP_POWER = SDL_Scancode.KP_POWER | 0x40000000,
        KP_PERCENT = SDL_Scancode.KP_PERCENT | 0x40000000,
        KP_LESS = SDL_Scancode.KP_LESS | 0x40000000,
        KP_GREATER = SDL_Scancode.KP_GREATER | 0x40000000,
        KP_AMPERSAND = SDL_Scancode.KP_AMPERSAND | 0x40000000,
        KP_DBLAMPERSAND = SDL_Scancode.KP_DBLAMPERSAND | 0x40000000,
        KP_VERTICALBAR = SDL_Scancode.KP_VERTICALBAR | 0x40000000,
        KP_DBLVERTICALBAR = SDL_Scancode.KP_DBLVERTICALBAR | 0x40000000,
        KP_COLON = SDL_Scancode.KP_COLON | 0x40000000,
        KP_HASH = SDL_Scancode.KP_HASH | 0x40000000,
        KP_SPACE = SDL_Scancode.KP_SPACE | 0x40000000,
        KP_AT = SDL_Scancode.KP_AT | 0x40000000,
        KP_EXCLAM = SDL_Scancode.KP_EXCLAM | 0x40000000,
        KP_MEMSTORE = SDL_Scancode.KP_MEMSTORE | 0x40000000,
        KP_MEMRECALL = SDL_Scancode.KP_MEMRECALL | 0x40000000,
        KP_MEMCLEAR = SDL_Scancode.KP_MEMCLEAR | 0x40000000,
        KP_MEMADD = SDL_Scancode.KP_MEMADD | 0x40000000,
        KP_MEMSUBTRACT = SDL_Scancode.KP_MEMSUBTRACT | 0x40000000,
        KP_MEMMULTIPLY = SDL_Scancode.KP_MEMMULTIPLY | 0x40000000,
        KP_MEMDIVIDE = SDL_Scancode.KP_MEMDIVIDE | 0x40000000,
        KP_PLUSMINUS = SDL_Scancode.KP_PLUSMINUS | 0x40000000,
        KP_CLEAR = SDL_Scancode.KP_CLEAR | 0x40000000,
        KP_CLEARENTRY = SDL_Scancode.KP_CLEARENTRY | 0x40000000,
        KP_BINARY = SDL_Scancode.KP_BINARY | 0x40000000,
        KP_OCTAL = SDL_Scancode.KP_OCTAL | 0x40000000,
        KP_DECIMAL = SDL_Scancode.KP_DECIMAL | 0x40000000,
        KP_HEXADECIMAL = SDL_Scancode.KP_HEXADECIMAL | 0x40000000,
        LCTRL = SDL_Scancode.LCTRL | 0x40000000,
        LSHIFT = SDL_Scancode.LSHIFT | 0x40000000,
        LALT = SDL_Scancode.LALT | 0x40000000,
        LGUI = SDL_Scancode.LGUI | 0x40000000,
        RCTRL = SDL_Scancode.RCTRL | 0x40000000,
        RSHIFT = SDL_Scancode.RSHIFT | 0x40000000,
        RALT = SDL_Scancode.RALT | 0x40000000,
        RGUI = SDL_Scancode.RGUI | 0x40000000,
        MODE = SDL_Scancode.MODE | 0x40000000,
        AUDIONEXT = SDL_Scancode.AUDIONEXT | 0x40000000,
        AUDIOPREV = SDL_Scancode.AUDIOPREV | 0x40000000,
        AUDIOSTOP = SDL_Scancode.AUDIOSTOP | 0x40000000,
        AUDIOPLAY = SDL_Scancode.AUDIOPLAY | 0x40000000,
        AUDIOMUTE = SDL_Scancode.AUDIOMUTE | 0x40000000,
        MEDIASELECT = SDL_Scancode.MEDIASELECT | 0x40000000,
        WWW = SDL_Scancode.WWW | 0x40000000,
        MAIL = SDL_Scancode.MAIL | 0x40000000,
        CALCULATOR = SDL_Scancode.CALCULATOR | 0x40000000,
        COMPUTER = SDL_Scancode.COMPUTER | 0x40000000,
        AC_SEARCH = SDL_Scancode.AC_SEARCH | 0x40000000,
        AC_HOME = SDL_Scancode.AC_HOME | 0x40000000,
        AC_BACK = SDL_Scancode.AC_BACK | 0x40000000,
        AC_FORWARD = SDL_Scancode.AC_FORWARD | 0x40000000,
        AC_STOP = SDL_Scancode.AC_STOP | 0x40000000,
        AC_REFRESH = SDL_Scancode.AC_REFRESH | 0x40000000,
        AC_BOOKMARKS = SDL_Scancode.AC_BOOKMARKS | 0x40000000,
        BRIGHTNESSDOWN = SDL_Scancode.BRIGHTNESSDOWN | 0x40000000,
        BRIGHTNESSUP = SDL_Scancode.BRIGHTNESSUP | 0x40000000,
        DISPLAYSWITCH = SDL_Scancode.DISPLAYSWITCH | 0x40000000,
        KBDILLUMTOGGLE = SDL_Scancode.KBDILLUMTOGGLE | 0x40000000,
        KBDILLUMDOWN = SDL_Scancode.KBDILLUMDOWN | 0x40000000,
        KBDILLUMUP = SDL_Scancode.KBDILLUMUP | 0x40000000,
        EJECT = SDL_Scancode.EJECT | 0x40000000,
        SLEEP = SDL_Scancode.SLEEP | 0x40000000
    }
}