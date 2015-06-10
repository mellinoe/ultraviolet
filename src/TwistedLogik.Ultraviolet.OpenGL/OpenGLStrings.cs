﻿using System.Reflection;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.OpenGL
{
    /// <summary>
    /// Contains the implementation's string resources.
    /// </summary>
    public static class OpenGLStrings
    {
        /// <summary>
        /// Initializes the OpenGLStrings type.
        /// </summary>
        static OpenGLStrings()
        {
#if NETCORE
            var asm = typeof(OpenGLStrings).GetTypeInfo().Assembly;
#else
            var asm = Assembly.GetExecutingAssembly();
#endif
            using (var stream = asm.GetManifestResourceStream("TwistedLogik.Ultraviolet.OpenGL.Resources.Strings.xml"))
            {
                StringDatabase.LoadFromStream(stream);
            }
        }

        private static readonly LocalizationDatabase StringDatabase = new LocalizationDatabase();

#pragma warning disable 1591
        public static readonly StringResource UnsupportedGraphicsDevice             = new StringResource(StringDatabase, "UNSUPPORTED_GRAPHICS_DEVICE");
        public static readonly StringResource UnsupportedShaderType                 = new StringResource(StringDatabase, "UNSUPPORTED_SHADER_TYPE");
        public static readonly StringResource UnsupportedDataType                   = new StringResource(StringDatabase, "UNSUPPORTED_DATA_TYPE");
        public static readonly StringResource UnsupportedElementType                = new StringResource(StringDatabase, "UNSUPPORTED_ELEMENT_TYPE");
        public static readonly StringResource UnsupportedImageType                  = new StringResource(StringDatabase, "UNSUPPORTED_IMAGE_TYPE");
        public static readonly StringResource UnsupportedVertexFormat               = new StringResource(StringDatabase, "UNSUPPORTED_VERTEX_FORMAT");
        public static readonly StringResource UnsupportedIndexFormat                = new StringResource(StringDatabase, "UNSUPPORTED_INDEX_FORMAT");
        public static readonly StringResource UnsupportedPrimitiveType              = new StringResource(StringDatabase, "UNSUPPORTED_PRIMITIVE_TYPE");
        public static readonly StringResource DataTooLargeForBuffer                 = new StringResource(StringDatabase, "DATA_TOO_LARGE_FOR_BUFFER");
        public static readonly StringResource NoGeometryStream                      = new StringResource(StringDatabase, "NO_GEOMETRY_STREAM");
        public static readonly StringResource InvalidGeometryStream                 = new StringResource(StringDatabase, "INVALID_GEOMETRY_STREAM");
        public static readonly StringResource NoEffect                              = new StringResource(StringDatabase, "NO_EFFECT");
        public static readonly StringResource InvalidSpriteFontTexture              = new StringResource(StringDatabase, "INVALID_SPRITEFONT_TEXTURE");
        public static readonly StringResource InvalidSpriteFontKerningPair          = new StringResource(StringDatabase, "INVALID_SPRITEFONT_KERNING_PAIR");
        public static readonly StringResource BufferIsTooSmall                      = new StringResource(StringDatabase, "BUFFER_IS_TOO_SMALL");
        public static readonly StringResource RenderTargetNeedsBuffers              = new StringResource(StringDatabase, "RENDER_TARGET_NEEDS_BUFFERS");
        public static readonly StringResource RenderTargetFramebufferIsNotComplete  = new StringResource(StringDatabase, "RENDER_TARGET_FRAMEBUFFER_IS_NOT_COMPLETE");
        public static readonly StringResource RenderTargetCannotBeUsedAsTexture     = new StringResource(StringDatabase, "RENDER_TARGET_CANNOT_BE_USED_AS_TEXTURE");
        public static readonly StringResource RenderBufferIsWrongSize               = new StringResource(StringDatabase, "RENDER_BUFFER_IS_WRONG_SIZE");
        public static readonly StringResource RenderBufferExceedsTargetCapacity     = new StringResource(StringDatabase, "RENDER_BUFFER_EXCEEDS_TARGET_CAPACITY");
        public static readonly StringResource ResourceAlreadyBound                  = new StringResource(StringDatabase, "RESOURCE_ALREADY_BOUND");
        public static readonly StringResource ResourceNotBound                      = new StringResource(StringDatabase, "RESOURCE_NOT_BOUND");
        public static readonly StringResource StaleOpenGLCache                      = new StringResource(StringDatabase, "STALE_OPENGL_CACHE");
        public static readonly StringResource EffectUniformTypeMismatch             = new StringResource(StringDatabase, "EFFECT_UNIFORM_TYPE_MISMATCH");
        public static readonly StringResource ImmutableValueAlreadySet              = new StringResource(StringDatabase, "IMMUTABLE_VALUE_ALREADY_SET");
        public static readonly StringResource InvalidEffectPass                     = new StringResource(StringDatabase, "INVALID_EFFECT_PASS");
        public static readonly StringResource EffectMustHaveVertexAndFragmentShader = new StringResource(StringDatabase, "EFFECT_MUST_HAVE_VERTEX_AND_FRAGMENT_SHADER");
        public static readonly StringResource EffectMustHaveTechniques              = new StringResource(StringDatabase, "EFFECT_MUST_HAVE_TECHNIQUES");
        public static readonly StringResource EffectTechniqueMustHavePasses         = new StringResource(StringDatabase, "EFFECT_TECHNIQUE_MUST_HAVE_PASSES");
        public static readonly StringResource InvalidAudioAssembly                  = new StringResource(StringDatabase, "INVALID_AUDIO_ASSEMBLY");
        public static readonly StringResource TechniqueBelongsToDifferentEffect     = new StringResource(StringDatabase, "TECHNIQUE_BELONGS_TO_DIFFERENT_EFFECT");
        public static readonly StringResource DebugOutputNotSupported               = new StringResource(StringDatabase, "DEBUG_OUTPUT_NOT_SUPPORTED");
        public static readonly StringResource ShaderUniformHasNoSource              = new StringResource(StringDatabase, "SHADER_UNIFORM_HAS_NO_SOURCE");
        public static readonly StringResource UnsupportedFillModeGLES               = new StringResource(StringDatabase, "UNSUPPORTED_FILLMODE_GLES");
        public static readonly StringResource UnsupportedLODBiasGLES                = new StringResource(StringDatabase, "UNSUPPORTED_LOD_BIAS_GLES");
        public static readonly StringResource CannotCreateHeadlessContextOnAndroid  = new StringResource(StringDatabase, "CANNOT_CREATE_HEADLESS_CONTEXT_ON_ANDROID");
#pragma warning restore 1591
    }
}
