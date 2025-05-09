using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OilDrillingSimulationApp.Models
{
    public interface ILoaderService
    {
        Task LoadOilAsync(ObservableCollection<OilBarrel> barrels);
    }
}