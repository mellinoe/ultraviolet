﻿using System;
using System.Runtime.InteropServices;

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_CommonEvent
    {
        public UInt32 type;
        public UInt32 timestamp;
    }
}