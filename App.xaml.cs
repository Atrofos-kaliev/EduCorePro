using System;
using System.Windows;

namespace EduCorePro;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        this.DispatcherUnhandledException += (sender, args) =>
        {
            MessageBox.Show($"Ошибка UI:\n{args.Exception.Message}\n\n{args.Exception.StackTrace}", "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            args.Handled = true;
        };

        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            var ex = (Exception)args.ExceptionObject;
            MessageBox.Show($"Фатальная ошибка:\n{ex.Message}\n\n{ex.StackTrace}", "Фатальная ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        };

        base.OnStartup(e);
    }
}