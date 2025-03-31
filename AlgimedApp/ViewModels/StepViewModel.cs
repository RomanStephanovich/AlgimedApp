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
    public class StepViewModel : BaseViewModel
    {
        private readonly IStepService _stepService;

        public int Timer { get => _timer; set => SetProperty(ref _timer, value); }
        public string Destination { get => _destination; set => SetProperty(ref _destination, value); }
        public int Speed { get => _speed; set => SetProperty(ref _speed, value); }
        public string Type { get => _type; set => SetProperty(ref _type, value); }
        public int Volume { get => _volume; set => SetProperty(ref _volume, value); }

        private int _timer;
        private string _destination = "";
        private int _speed;
        private string _type = "";
        private int _volume;

        public ICommand SaveCommand { get; }

        public bool IsEditMode { get; } = false;
        public int ModeId { get; }
        public int? EditingId { get; }

        public StepViewModel(int modeId)
        {
            ModeId = modeId;
            _stepService = App.AppHost.Services.GetRequiredService<IStepService>();
            SaveCommand = new RelayCommand(_ => Save());
        }

        public StepViewModel(int modeId, StepDto dto) : this(modeId)
        {
            IsEditMode = true;
            EditingId = dto.Id;

            Timer = dto.Timer;
            Destination = dto.Destination;
            Speed = dto.Speed;
            Type = dto.Type;
            Volume = dto.Volume;
        }

        private async void Save()
        {
            if (string.IsNullOrWhiteSpace(Destination) ||
                string.IsNullOrWhiteSpace(Type) ||
                Timer < 0 || Speed < 0 || Volume < 0)
            {
                MessageBox.Show("Please check your input values.");
                return;
            }

            var dto = new StepDto
            {
                ModeId = ModeId,
                Timer = Timer,
                Destination = Destination,
                Speed = Speed,
                Type = Type,
                Volume = Volume
            };

            if (IsEditMode && EditingId.HasValue)
            {
                await _stepService.UpdateStepAsync(EditingId.Value, dto);
                MessageBox.Show("Step updated.");
            }
            else
            {
                await _stepService.AddStepAsync(dto);
                MessageBox.Show("Step added.");
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
