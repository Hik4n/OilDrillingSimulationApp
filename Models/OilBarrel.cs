using System;

namespace OilDrillingSimulationApp.Models
{
    public class OilBarrel
    {
        public int Amount { get; }
        public DateTime ExtractionTime { get; }

        public OilBarrel(int amount)
        {
            Amount = amount;
            ExtractionTime = DateTime.Now;
        }

        public override string ToString() => $"{Amount} barrels (extracted at {ExtractionTime:HH:mm:ss})";
    }
}