// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="LuckySkebe (fmann12345@gmail.com)">
//     Copyright (c) LuckySkebe (fmann12345@gmail.com). All rights reserved.
//     Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace CStreamer.Designer.Avalonia
{
    using CStreamer.Designer.Avalonia.ViewModels;
    using CStreamer.Designer.Avalonia.Views;
    using global::Avalonia;
    using global::Avalonia.Logging.Serilog;

    internal static class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static void Main(string[] args) => BuildAvaloniaApp().Start(AppMain, args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToDebug()
                .UseReactiveUI();

        // Your application's entry point. Here you can initialize your MVVM framework, DI
        // container, etc.
        private static void AppMain(Application app, string[] args)
        {
            var pipeline = new PipeLine();
            var viewModel = new MainWindowViewModel(pipeline, PipeElementFactory.GetFactoryTypes());

            var window = new MainWindow
            {
                DataContext = viewModel,
            };

            app.Run(window);
        }
    }
}
