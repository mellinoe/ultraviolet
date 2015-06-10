﻿using System.Reflection;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.BASS
{
    /// <summary>
    /// Contains the implementation's string resources.
    /// </summary>
    public static class BASSStrings
    {
        /// <summary>
        /// Initializes the BASSStrings type.
        /// </summary>
        static BASSStrings()
        {
#if NETCORE
            var asm = typeof(BASSStrings).GetTypeInfo().Assembly;
#else
            var asm = Assembly.GetExecutingAssembly();
#endif
            using (var stream = asm.GetManifestResourceStream("TwistedLogik.Ultraviolet.BASS.Resources.Strings.xml"))
            {
                StringDatabase.LoadFromStream(stream);
            }
        }

        private static readonly LocalizationDatabase StringDatabase = new LocalizationDatabase();

#pragma warning disable 1591
        public static readonly StringResource StreamsNotSupported = new StringResource(StringDatabase, "STREAMS_NOT_SUPPORTED");
        public static readonly StringResource NotCurrentlyValid   = new StringResource(StringDatabase, "NOT_CURRENTLY_VALID");
#pragma warning restore 1591
    }
}
