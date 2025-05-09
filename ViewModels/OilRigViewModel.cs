using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using OilDrillingSimulationApp.Models;

namespace OilDrillingSimulationApp.ViewModels
{
    public class OilRigViewModel : INotifyPropertyChanged
    {
        private readonly OilRig? _oilRig;
        private string _status = "Not running";
        private string _lastEvent = string.Empty;

        public string Name => _oilRig?.Name ?? string.Empty;
        public ObservableCollection<OilBarrel>? Barrels => _oilRig?.Barrels;
        public ObservableCollection<Mechanic>? Mechanics => _oilRig?.Mechanics;
        public bool IsOnFire => _oilRig?.IsOnFire ?? false;
        public int TotalBarrelsAmount => Barrels?.Sum(b => b.Amount) ?? 0;
        public ObservableCollection<string> EventLog { get; } = new();
        public ICommand RequestRemoveCommand { get; }

        public OilRigViewModel(Action<OilRigViewModel> removeAction)
        {
            RequestRemoveCommand = new RelayCommand(() => removeAction(this));
        }

        public OilRigViewModel(OilRig oilRig, ILoaderService loaderService)
        {
            _oilRig = oilRig ?? throw new ArgumentNullException(nameof(oilRig));
            _oilRig.Barrels.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(Barrels));
                OnPropertyChanged(nameof(TotalBarrelsAmount));
            };
            _oilRig.LoaderService = loaderService ?? throw new ArgumentNullException(nameof(loaderService));
            RequestRemoveCommand = new RelayCommand(() => { });

            _oilRig.OilExtracted += OnOilExtracted;
            _oilRig.FireStarted += OnFireStarted;
            _oilRig.FireExtinguished += OnFireExtinguished;
            _oilRig.LoaderArrived += OnLoaderArrived;
            _oilRig.LoaderDeparted += OnLoaderDeparted;
            _oilRig.MechanicStatusChanged += OnMechanicStatusChanged;
        }

        public string Status
        {
            get => _status;
            private set
            {
                _status = value;
                OnPropertyChanged();
            }
        }

        public string LastEvent
        {
            get => _lastEvent;
            private set
            {
                _lastEvent = value;
                OnPropertyChanged();
            }
        }

        public void StartDrilling()
        {
            if (_oilRig != null)
            {
                _oilRig.StartDrilling();
                Status = "Drilling";
                AddEvent("Drilling started");
            }
        }

        public void StopDrilling()
        {
            if (_oilRig != null)
            {
                _oilRig.StopDrilling();
                Status = "Stopped";
                AddEvent("Drilling stopped");
            }
        }

        private void OnMechanicStatusChanged(string message)
        {
            AddEvent(message);
            LastEvent = message;
            OnPropertyChanged(nameof(Mechanics));
        }

        private void OnOilExtracted(string message)
        {
            AddEvent(message);
            LastEvent = message;
            OnPropertyChanged(nameof(Barrels));
        }

        private void OnFireStarted(string message)
        {
            Status = "Fire";
            AddEvent(message);
            LastEvent = message;
            OnPropertyChanged(nameof(IsOnFire));
        }

        private void OnFireExtinguished(string message)
        {
            Status = "Drilling";
            AddEvent(message);
            LastEvent = message;
            OnPropertyChanged(nameof(IsOnFire));
        }

        private void OnLoaderArrived(string message)
        {
            AddEvent(message);
            LastEvent = message;
        }

        private void OnLoaderDeparted(string message)
        {
            AddEvent(message);
            LastEvent = message;
        }

        private void AddEvent(string message)
        {
            var timestamp = DateTime.Now.ToString("HH:mm:ss");
            EventLog.Insert(0, $"[{timestamp}] {message}");

            if (EventLog.Count > 50)
            {
                EventLog.RemoveAt(EventLog.Count - 1);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}