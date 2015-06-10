﻿using System;
using System.IO;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.OpenGL;
using TwistedLogik.Ultraviolet.UI.Presentation;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;
using UltravioletSample.Input;
using UltravioletSample.UI;
using UltravioletSample.UI.Screens;
using System.Reflection;

namespace UltravioletSample
{
#if ANDROID
    [Android.App.Activity(Label = "Ultraviolet Sample 12", MainLauncher = true, ConfigurationChanges = 
        Android.Content.PM.ConfigChanges.Orientation | 
        Android.Content.PM.ConfigChanges.ScreenSize | 
        Android.Content.PM.ConfigChanges.KeyboardHidden)]
    public class Game : UltravioletActivity
#else
    public class Game : UltravioletApplication
#endif
    {
        public Game()
            : base("TwistedLogik", "Ultraviolet Sample 12")
        {

        }

        public static void Main(string[] args)
        {
            Console.WriteLine(typeof(ExampleViewModel).AssemblyQualifiedName);
            Console.WriteLine(typeof(UltravioletApplication).GetTypeInfo().Assembly.FullName);
            using (var game = new Game())
            {
                game.Run();
            }
        }

        protected override UltravioletContext OnCreatingUltravioletContext()
        {
            var configuration = new OpenGLUltravioletConfiguration();
            PresentationFoundation.Configure(configuration);

            return new OpenGLUltravioletContext(this, configuration);
        }

        protected override void OnLoadingContent()
        {
            this.content = ContentManager.Create("Content");

            LoadInputBindings();
            LoadContentManifests();
            LoadPresentation();

            GC.Collect(2);

            var screenService = new UIScreenService(content);
            var screen = screenService.Get<ExampleScreen>();

            Ultraviolet.GetUI().GetScreens().Open(screen);

            base.OnLoadingContent();
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            if (Ultraviolet.GetInput().GetActions().ExitApplication.IsPressed())
            {
                Exit();
            }
            base.OnUpdating(time);
        }

        protected override void OnDrawing(UltravioletTime time)
        {
            base.OnDrawing(time);
        }

        protected override void OnShutdown()
        {
            SaveInputBindings();

            base.OnShutdown();
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.DisposeRef(ref content);
            }
            base.Dispose(disposing);
        }

        private String GetInputBindingsPath()
        {
            return Path.Combine(GetRoamingApplicationSettingsDirectory(), "InputBindings.xml");
        }

        private void LoadInputBindings()
        {
            Ultraviolet.GetInput().GetActions().Load(GetInputBindingsPath(), throwIfNotFound: false);
        }

        private void SaveInputBindings()
        {
            Ultraviolet.GetInput().GetActions().Save(GetInputBindingsPath());
        }

        private void LoadPresentation()
        {
            var globalStylesheet = content.Load<UvssDocument>("UI/DefaultUIStyles");
            Ultraviolet.GetUI().GetPresentationFoundation().SetGlobalStyleSheet(globalStylesheet);
        }

        private void LoadContentManifests()
        {
            var uvContent = Ultraviolet.GetContent();

            var contentManifestFiles = this.content.GetAssetFilePathsInDirectory("Manifests");
            uvContent.Manifests.Load(contentManifestFiles);
        }

        private ContentManager content;
    }
}
