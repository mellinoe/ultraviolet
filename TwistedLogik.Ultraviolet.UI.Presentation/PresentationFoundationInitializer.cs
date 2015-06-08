﻿using System;

namespace TwistedLogik.Ultraviolet.UI.Presentation
{
    /// <summary>
    /// Contains methods for initializing the Presentation Foundation.
    /// </summary>
    internal sealed class PresentationFoundationInitializer : UIViewProviderInitializer
    {
        /// <inheritdoc/>
        public override void Initialize(UltravioletContext uv, Object configuration)
        {
            var upf       = uv.GetUI().GetPresentationFoundation();
            var upfconfig = (configuration as PresentationFoundationConfiguration) ?? new PresentationFoundationConfiguration();

            upf.OutOfBandRenderer.RenderTargetSize = upfconfig.OutOfBandRenderTargetSize;
        }
    }
}