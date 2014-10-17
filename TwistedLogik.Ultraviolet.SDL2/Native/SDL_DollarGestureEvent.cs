﻿using System;
using System.Runtime.InteropServices;
using SDL_GestureID = System.Int64;
using SDL_TouchID = System.Int64;

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SDL_DollarGestureEvent
    {
        public UInt32 type;
        public UInt32 timestamp;
        public SDL_TouchID touchId;
        public SDL_GestureID gestureId;
        public UInt32 numFingers;
        public Single error;
        public Single x;
        public Single y;
    }
}