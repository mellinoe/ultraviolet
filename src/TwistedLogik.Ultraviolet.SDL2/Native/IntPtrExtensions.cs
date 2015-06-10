﻿using System;

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    /// <summary>
    /// Contains extension methods for the IntPtr structure.
    /// </summary>
    public static class IntPtrExtensions
    {
        /// <summary>
        /// Converts the pointer to a string representation of the hexadecimal value of the memory address which it contains.
        /// </summary>
        /// <param name="ptr">The pointer to convert.</param>
        /// <returns>A string representation of the hexadecimal value of the memory address which the pointer contains.</returns>
        public static String ToStringHex(this IntPtr ptr)
        {
            if (EnvironmentEx.Is64BitProcess)
            {
                return String.Format("{0:x8}", ptr.ToInt64());
            }
            return String.Format("{0:x4}", ptr.ToInt32());
        }
    }
}
