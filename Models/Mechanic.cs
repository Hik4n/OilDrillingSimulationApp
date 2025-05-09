using System.ComponentModel;
using System.Threading.Tasks;

namespace OilDrillingSimulationApp.Models
{
    public class Mechanic : INotifyPropertyChanged
    {
        private string _status = "Idle";
        public string Name { get; }

        public string Status
        {
            get => _status;
            private set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public Mechanic(string name)
        {
            Name = name;
        }

        public async Task ExtinguishFire(OilRig oilRig)
        {
            Status = "Extinguishing fire";
            await Task.Delay(3000);
            Status = "Idle";
        }

        public async Task PerformMaintenanceAsync()
        {
            Status = "Performing maintenance";
            await Task.Delay(3000);
            Status = "Idle";
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}