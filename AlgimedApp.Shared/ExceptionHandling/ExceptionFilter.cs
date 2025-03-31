using System;
using System.Threading.Tasks;

namespace AlgimedApp.Shared.ExceptionHandling
{
    public static class GlobalExceptionHandler
    {
        private static Action<string, Exception>? _onException;

        public static void Register(Action<string, Exception> onException)
        {
            _onException = onException;

            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        }

        public static void HandleUiException(Exception ex)
        {
            _onException?.Invoke("UI Thread Exception", ex);
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
                _onException?.Invoke("AppDomain Unhandled Exception", ex);
        }

        private static void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            _onException?.Invoke("Unobserved Task Exception", e.Exception);
            e.SetObserved();
        }
    }
}