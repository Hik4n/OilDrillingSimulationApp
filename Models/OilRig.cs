using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OilDrillingSimulationApp.Models
{
    public class OilRig
    {
        public string Name { get; }
        public ObservableCollection<OilBarrel> Barrels { get; } = new ObservableCollection<OilBarrel>();
        public ObservableCollection<Mechanic> Mechanics { get; } = new();
        public ILoaderService? LoaderService { get; set; }
        public bool IsOnFire { get; private set; }
        public bool IsRunning { get; private set; }
        private CancellationTokenSource? _cts;
        private readonly Random _random = new();

        public event Action<string>? OilExtracted;
        public event Action<string>? FireStarted;
        public event Action<string>? FireExtinguished;
        public event Action<string>? LoaderArrived;
        public event Action<string>? LoaderDeparted;
        public event Action<string>? MechanicStatusChanged;

        public OilRig(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public void StartDrilling()
        {
            if (IsRunning) return;
            
            IsRunning = true;
            _cts = new CancellationTokenSource();
            Task.Run(() => DrillingProcess(_cts.Token));
            Mechanics.Add(new Mechanic("Mechanic 1"));
            Mechanics.Add(new Mechanic("Mechanic 2"));
        }

        public void StopDrilling()
        {
            IsRunning = false;
            _cts?.Cancel();
        }

        private async Task DrillingProcess(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(5000);
                var amount = _random.Next(1, 5);
                Barrels.Add(new OilBarrel(amount));
                OilExtracted?.Invoke($"Extracted {amount} barrels of oil");

                if (_random.Next(0, 100) < 30 && Mechanics.Any())
                {
                    var mechanic = Mechanics[_random.Next(0, Mechanics.Count)];
                    _ = mechanic.PerformMaintenanceAsync();
                    OilExtracted?.Invoke($"{mechanic.Name} is performing maintenance");
                }

                if (_random.Next(0, 100) < 5)
                {
                    IsOnFire = true;
                    FireStarted?.Invoke("Fire detected on the oil rig!");
                    
                    foreach (var mechanic in Mechanics)
                    {
                        MechanicStatusChanged?.Invoke($"{mechanic.Name} is extinguishing fire");
                        await mechanic.ExtinguishFire(this);
                    }
                    
                    IsOnFire = false;
                    FireExtinguished?.Invoke("Fire extinguished");
                }

                if (Barrels.Sum(b => b.Amount) >= 10)
                {
                    await SendOilToLoader();
                }
            }
        }

        private async Task SendOilToLoader()
        {
            if (LoaderService == null) return;
            LoaderArrived?.Invoke("Loader arrived to collect oil");
            await LoaderService.LoadOilAsync(Barrels);
            Barrels.Clear();
            LoaderDeparted?.Invoke("Loader departed with oil shipment");
        }
    }
}