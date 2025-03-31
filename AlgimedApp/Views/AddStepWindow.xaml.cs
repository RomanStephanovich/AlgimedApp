using AlgimedApp.Data.Models;
using AlgimedApp.Service.Services.Interfaces;
using AlgimedApp.Shared.Dtos;
using AlgimedApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace AlgimedApp.Views
{
    public partial class AddStepWindow : Window
    {
        public AddStepWindow(int modeId)
        {
            InitializeComponent();
            DataContext = new StepViewModel(modeId); 
        }

        public AddStepWindow(int modeId, StepDto dto)
        {
            InitializeComponent();
            DataContext = new StepViewModel(modeId, dto);
        }
    }
}