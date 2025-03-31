using AlgimedApp.Data;
using AlgimedApp.Service.Services;
using AlgimedApp.Service.Services.Implementations;
using AlgimedApp.Service.Services.Interfaces;
using AlgimedApp.Shared.ExceptionHandling;
using AlgimedApp.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using System.Windows.Threading;




namespace AlgimedApp
{
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }

        public App()
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<AlgimedDbContext>(options =>
                        options.UseSqlite("Data Source=algimed.db"));

                    services.AddScoped<IAuthService, AuthService>();
                    services.AddScoped<IModeService, ModeService>();
                    services.AddScoped<IStepService, StepService>();
                    services.AddScoped<IExcelImportService, ExcelImportService>();
                    services.AddAutoMapper(cfg =>
                    {
                        cfg.AddProfile<AlgimedApp.Shared.AutoMapper.MappingProfile>();
                    });
                })
                .Build();

            using var scope = AppHost.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AlgimedDbContext>();
            db.Database.Migrate();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var loginWindow = new LoginWindow();
            var result = loginWindow.ShowDialog();

            if (result == true)
            {
                var mainWindow = new MainWindow();
                MainWindow = mainWindow;
                mainWindow.Show();
            }
            else
            {
                Shutdown();
            }
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost.StopAsync();
            AppHost.Dispose();
            base.OnExit(e);
        }

        private void ShowGlobalError(string context, Exception ex)
        {
            MessageBox.Show($"[{context}]\n{ex.Message}", "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
