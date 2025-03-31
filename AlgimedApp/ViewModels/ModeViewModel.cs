using AlgimedApp.Service.Services.Interfaces;
using AlgimedApp.Shared.Dtos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace AlgimedApp.ViewModels
{
    public class ModeViewModel : BaseViewModel
    {
        private readonly IModeService _modeService;

        public string Name { get => _name; set => SetProperty(ref _name, value); }
        public int MaxBottleNumber { get => _bottle; set => SetProperty(ref _bottle, value); }
        public int MaxUsedTips { get => _tips; set => SetProperty(ref _tips, value); }

        private string _name = "";
        private int _bottle;
        private int _tips;

        public ICommand SaveCommand { get; }

        public bool IsEditMode { get; } = false;
        public int? EditingId { get; } = null;

        public ModeViewModel()
        {
            _modeService = App.AppHost.Services.GetRequiredService<IModeService>();
            SaveCommand = new RelayCommand(_ => Save());
        }

        public ModeViewModel(ModeDto dto) : this()
        {
            IsEditMode = true;
            EditingId = dto.Id;

            Name = dto.Name;
            MaxBottleNumber = dto.MaxBottleNumber;
            MaxUsedTips = dto.MaxUsedTips;
        }

        private async void Save()
        {
            if (string.IsNullOrWhiteSpace(Name) || MaxBottleNumber < 0 || MaxUsedTips < 0)
            {
                MessageBox.Show("Please check your input values.");
                return;
            }

            var dto = new ModeDto
            {
                Name = Name,
                MaxBottleNumber = MaxBottleNumber,
                MaxUsedTips = MaxUsedTips
            };

            if (IsEditMode && EditingId.HasValue)
            {
                await _modeService.UpdateModeAsync(EditingId.Value, dto);
                MessageBox.Show("Mode updated.");
            }
            else
            {
                await _modeService.AddModeAsync(dto);
                MessageBox.Show("Mode added.");
            }

            CloseWindow();
        }

        private void CloseWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.DialogResult = true;
                    window.Close();
                    break;
                }
            }
        }
    }
}