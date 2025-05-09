using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using OilDrillingSimulationApp.Models;

namespace OilDrillingSimulationApp.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly ILoaderService _loaderService = new LoaderService();
        private int _oilRigCounter = 1;
        private OilRigViewModel? _selectedOilRig;

        public ObservableCollection<OilRigViewModel> OilRigs { get; } = new();
        public ICommand AddOilRigCommand { get; }
        public ICommand RemoveOilRigCommand { get; }

        public OilRigViewModel? SelectedOilRig
        {
            get => _selectedOilRig;
            set
            {
                _selectedOilRig = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            AddOilRigCommand = new RelayCommand(AddOilRig);
            RemoveOilRigCommand = new RelayCommand(RemoveSelectedOilRig);
        }

        private void AddOilRig()
        {
            var name = $"Oil Rig {_oilRigCounter++}";
            var oilRig = new OilRig(name);
            
            var viewModel = new OilRigViewModel(oilRig, _loaderService);
            OilRigs.Add(viewModel);
            viewModel.StartDrilling();
            
            SelectedOilRig = viewModel;
        }

        private void RemoveSelectedOilRig()
        {
            if (SelectedOilRig != null)
            {
                SelectedOilRig.StopDrilling();
                OilRigs.Remove(SelectedOilRig);
                SelectedOilRig = null;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}