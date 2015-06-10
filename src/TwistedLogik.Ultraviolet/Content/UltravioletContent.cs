using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Content
{
    /// <summary>
    /// Represents the core implementation of the Ultraviolet content subsystem.
    /// </summary>
    public sealed class UltravioletContent : UltravioletResource, IUltravioletContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletContent"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public UltravioletContent(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <inheritdoc/>
        public void RegisterImportersAndProcessors(IEnumerable<Assembly> additionalAssemblies)
        {
            Contract.EnsureNot(registered, UltravioletStrings.ContentHandlersAlreadyRegistered);

            var asmUltravioletCore = typeof(UltravioletContext).GetTypeInfo().Assembly;
            var asmUltravioletImpl = Ultraviolet.GetType().GetTypeInfo().Assembly;

            var asmUltravioletPlatform = Ultraviolet.GetPlatform().GetType().GetTypeInfo().Assembly;
            var asmUltravioletContent  = Ultraviolet.GetContent().GetType().GetTypeInfo().Assembly;
            var asmUltravioletGraphics = Ultraviolet.GetGraphics().GetType().GetTypeInfo().Assembly;
            var asmUltravioletAudio    = Ultraviolet.GetAudio().GetType().GetTypeInfo().Assembly;
            var asmUltravioletInput    = Ultraviolet.GetInput().GetType().GetTypeInfo().Assembly;
            var asmUltravioletUI       = Ultraviolet.GetUI().GetType().GetTypeInfo().Assembly;

            var assemblies = new[] { 
                asmUltravioletCore, 
                asmUltravioletImpl, 
                asmUltravioletPlatform,
                asmUltravioletContent,
                asmUltravioletGraphics,
                asmUltravioletAudio,
                asmUltravioletInput,
                asmUltravioletUI }.Union(additionalAssemblies ?? Enumerable.Empty<Assembly>()).Where(x => x != null).Distinct();

            foreach (var asm in assemblies)
            {
                importers.RegisterAssembly(asm);
                processors.RegisterAssembly(asm);
            }

            registered = true;
        }

        /// <inheritdoc/>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);

            OnUpdating(time);
        }

        /// <inheritdoc/>
        public ContentManifestRegistry Manifests
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return manifests;
            }
        }

        /// <inheritdoc/>
        public ContentImporterRegistry Importers
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);
            
                return importers; 
            }
        }

        /// <inheritdoc/>
        public ContentProcessorRegistry Processors
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);
                
                return processors;
            }
        }

        /// <inheritdoc/>
        public event UltravioletSubsystemUpdateEventHandler Updating;

        /// <summary>
        /// Raises the <see cref="Updating"/> event.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        private void OnUpdating(UltravioletTime time)
        {
            var temp = Updating;
            if (temp != null)
            {
                temp(this, time);
            }
        }

        // Registered content importers and processors.
        private Boolean registered;
        private readonly ContentManifestRegistry manifests = new ContentManifestRegistry();
        private readonly ContentImporterRegistry importers = new ContentImporterRegistry();
        private readonly ContentProcessorRegistry processors = new ContentProcessorRegistry();
    }
}
