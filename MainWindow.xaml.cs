using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;

namespace EduCorePro;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        try
        {
            InitializeComponent();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddWpfBlazorWebView();

            serviceCollection.AddMudServices(); 

            serviceCollection.AddSingleton<Services.SettingsService>();

            serviceCollection.AddSingleton<Services.AiService>();

            Resources.Add("services", serviceCollection.BuildServiceProvider());
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Произошла ошибка при запуске:\n\n{ex.Message}\n\n{ex.StackTrace}", 
                            "Ошибка EduCore Pro", 
                            MessageBoxButton.OK, 
                            MessageBoxImage.Error);
        }
    }
}