using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OilDrillingSimulationApp.Models
{
    public class LoaderService : ILoaderService
    {
        public event Action<string>? LoadingStarted;
        public event Action<string>? LoadingCompleted;
        private readonly Random _random = new();

        public async Task LoadOilAsync(ObservableCollection<OilBarrel> barrels)
        {
            var totalBarrels = barrels.Count;
            LoadingStarted?.Invoke($"Starting to load {totalBarrels} barrels");
            
            await Task.Delay(_random.Next(2000, 5000));
            
            LoadingCompleted?.Invoke($"Successfully loaded {totalBarrels} barrels");
        }
    }
}