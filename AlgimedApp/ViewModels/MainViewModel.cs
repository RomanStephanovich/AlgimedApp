using AlgimedApp.Service.Services.Interfaces;
using AlgimedApp.Shared.Dtos;
using AlgimedApp.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace AlgimedApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly IModeService _modeService;
        private readonly IStepService _stepService;
        private readonly IExcelImportService _excelService;

        public ObservableCollection<ModeDto> Modes { get; } = new();
        public ObservableCollection<StepDto> Steps { get; } = new();

        private ModeDto? _selectedMode;
        public ModeDto? SelectedMode
        {
            get => _selectedMode;
            set
            {
                if (SetProperty(ref _selectedMode, value))
                {
                    LoadStepsAsync();
                }
            }
        }

        private StepDto? _selectedStep;
        public StepDto? SelectedStep
        {
            get => _selectedStep;
            set => SetProperty(ref _selectedStep, value);
        }

        // Команды
        public ICommand AddModeCommand { get; }
        public ICommand EditModeCommand { get; }
        public ICommand DeleteModeCommand { get; }
        public ICommand ImportModesCommand { get; }

        public ICommand AddStepCommand { get; }
        public ICommand EditStepCommand { get; }
        public ICommand DeleteStepCommand { get; }
        public ICommand ImportStepsCommand { get; }

        public ICommand RefreshCommand { get; }

        public MainViewModel()
        {
            _modeService = App.AppHost.Services.GetRequiredService<IModeService>();
            _stepService = App.AppHost.Services.GetRequiredService<IStepService>();
            _excelService = App.AppHost.Services.GetRequiredService<IExcelImportService>();

            AddModeCommand = new RelayCommand(_ => AddMode());
            EditModeCommand = new RelayCommand(_ => EditMode());
            DeleteModeCommand = new RelayCommand(async _ => await DeleteModeAsync());
            ImportModesCommand = new RelayCommand(async _ => await ImportModesAsync());

            AddStepCommand = new RelayCommand(_ => AddStep());
            EditStepCommand = new RelayCommand(_ => EditStep());
            DeleteStepCommand = new RelayCommand(async _ => await DeleteStepAsync());
            ImportStepsCommand = new RelayCommand(async _ => await ImportStepsAsync());

            RefreshCommand = new RelayCommand(async _ => await LoadModesAsync());

            _ = LoadModesAsync();
        }

        public async Task LoadModesAsync()
        {
            var list = await _modeService.GetAllModesAsync();
            Application.Current.Dispatcher.Invoke(() =>
            {
                Modes.Clear();
                foreach (var m in list)
                    Modes.Add(m);
            });

            Steps.Clear();
            SelectedMode = null;
        }

        public async Task LoadStepsAsync()
        {
            Steps.Clear();
            SelectedStep = null;

            if (SelectedMode == null) return;

            var list = await _stepService.GetStepsByModeIdAsync(SelectedMode.Id);
            Application.Current.Dispatcher.Invoke(() =>
            {
                foreach (var s in list)
                    Steps.Add(s);
            });
        }

        private void AddMode()
        {
            var win = new AddModeWindow();
            if (win.ShowDialog() == true)
                _ = LoadModesAsync();
        }

        private void EditMode()
        {
            if (SelectedMode == null)
            {
                MessageBox.Show("Select a mode to edit.");
                return;
            }

            var win = new AddModeWindow(SelectedMode);
            if (win.ShowDialog() == true)
                _ = LoadModesAsync();
        }

        private async Task DeleteModeAsync()
        {
            if (SelectedMode == null)
            {
                MessageBox.Show("Select a mode to delete.");
                return;
            }

            var confirm = MessageBox.Show("Delete this mode and its steps?", "Confirm", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                await _modeService.DeleteModeAsync(SelectedMode.Id);
                await LoadModesAsync();
            }
        }

        private async Task ImportModesAsync()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog { Filter = "Excel files (*.xlsx)|*.xlsx" };
            if (dialog.ShowDialog() == true)
            {
                await _excelService.ImportModesFromExcelAsync(dialog.FileName);
                await LoadModesAsync();
            }
        }

        private void AddStep()
        {
            if (SelectedMode == null)
            {
                MessageBox.Show("Select a mode first.");
                return;
            }

            var win = new AddStepWindow(SelectedMode.Id);
            if (win.ShowDialog() == true)
                _ = LoadStepsAsync();
        }

        private void EditStep()
        {
            if (SelectedMode == null || SelectedStep == null)
            {
                MessageBox.Show("Select a step to edit.");
                return;
            }

            var win = new AddStepWindow(SelectedMode.Id, SelectedStep);
            if (win.ShowDialog() == true)
                _ = LoadStepsAsync();
        }

        private async Task DeleteStepAsync()
        {
            if (SelectedStep == null)
            {
                MessageBox.Show("Select a step to delete.");
                return;
            }

            var confirm = MessageBox.Show("Delete this step?", "Confirm", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                await _stepService.DeleteStepAsync(SelectedStep.Id);
                await LoadStepsAsync();
            }
        }

        private async Task ImportStepsAsync()
        {
            if (SelectedMode == null)
            {
                MessageBox.Show("Select a mode first.");
                return;
            }

            var dialog = new Microsoft.Win32.OpenFileDialog { Filter = "Excel files (*.xlsx)|*.xlsx" };
            if (dialog.ShowDialog() == true)
            {
                await _excelService.ImportStepsFromExcelAsync(dialog.FileName);
                await LoadStepsAsync();
            }
        }
    }
}
