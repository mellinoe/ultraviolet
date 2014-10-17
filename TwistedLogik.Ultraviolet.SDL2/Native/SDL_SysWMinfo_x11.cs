﻿using System;
using System.Runtime.InteropServices;

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_SysWMinfo_x11
    {
        public IntPtr display;
        public IntPtr window;
    }
}