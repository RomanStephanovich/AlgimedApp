using AlgimedApp.Shared.Dtos;
using AlgimedApp.ViewModels;
using System.Windows;

namespace AlgimedApp.Views
{
    public partial class AddModeWindow : Window
    {
        public AddModeWindow()
        {
            InitializeComponent();
            DataContext = new ModeViewModel(); 
        }

        public AddModeWindow(ModeDto dto)
        {
            InitializeComponent();
            DataContext = new ModeViewModel(dto); 
        }
    }
}