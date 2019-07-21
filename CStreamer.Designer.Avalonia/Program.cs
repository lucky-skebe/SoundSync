using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Logging.Serilog;
using CStreamer.Designer.Avalonia.ViewModels;
using CStreamer.Designer.Avalonia.Views;

namespace CStreamer.Designer.Avalonia
{
    class Program
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
            var viewModel = new MainWindowViewModel(PipeElementFactory.GetFactoryTypes());

            var pipeline = new PipeLine();

            var element = PipeElementFactory.Make("Multiply", null);

            viewModel.Pipeline.Items.Add(new ElementViewModel(100, 100, element));
            
            var window = new MainWindow
            {
                DataContext = viewModel,
            };

            app.Run(window);
        }
    }
}
