using AlgimedApp.ViewModels;
using System.Windows;

namespace AlgimedApp.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            var viewModel = new LoginViewModel();
            viewModel.LoginSucceeded += () =>
            {
                
                DialogResult = true;
            };

            DataContext = viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = PasswordBox.Password;
            }
        }
    }
}
