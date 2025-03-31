using AlgimedApp.Data.Models;
using AlgimedApp.Service.Services.Interfaces;
using AlgimedApp.Shared.Dtos;
using AlgimedApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace AlgimedApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}