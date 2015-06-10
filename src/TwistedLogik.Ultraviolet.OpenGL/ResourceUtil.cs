﻿using System;
using System.IO;
using System.Reflection;
using TwistedLogik.Gluon;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.OpenGL
{
    /// <summary>
    /// Contains utility methods for accessing the library's resource files.
    /// </summary>
    internal static class ResourceUtil
    {
        /// <summary>
        /// Reads a resource file as a string of text.
        /// </summary>
        /// <param name="name">The name of the file to read.</param>
        /// <returns>A string of text that contains the file data.</returns>
        public static String ReadResourceString(String name)
        {
            Contract.RequireNotEmpty(name, "name");

#if NETCORE
            var asm = typeof(ResourceUtil).GetTypeInfo().Assembly;
#else
            var asm = Assembly.GetExecutingAssembly();
#endif

            using (var stream = asm.GetManifestResourceStream("TwistedLogik.Ultraviolet.OpenGL.Resources." + name))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Reads a resource file containing a shader program's source code.
        /// </summary>
        /// <param name="name">The name of the file to read.</param>
        /// <returns>A string of text that contains the file data.</returns>
        public static String ReadShaderResourceString(String name)
        {
            Contract.RequireNotEmpty(name, "name");

            if (gl.IsGLES)
            {
                var ext = Path.GetExtension(name);
                name = Path.ChangeExtension(Path.GetFileNameWithoutExtension(name) + "ES", ext);
            }

            return ReadResourceString(name);
        }
    }
}
