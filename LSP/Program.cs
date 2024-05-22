using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IEnginePowered car = new Car();
            car.StartEngine(); // Outputs "Starting the car engine."
            Vehicle electricCar = new ElectricCar();
            //electricCar.StartEngine(); // No exception; no action is performed.
        }
    }

    public abstract class Vehicle
    {
        // Common vehicle behavior and properties.
    }
    public interface IEnginePowered
    {
        void StartEngine();
        void StopEngine();
    }
    public class Car : Vehicle, IEnginePowered
    {
        public void StartEngine()
        {
            Console.WriteLine("Starting the car engine.");
            // Code to start the car engine.
        }
        public void StopEngine()
        {
            Console.WriteLine("Stopping the car engine.");
            // Code to stop the car engine.
        }
    }
    public class ElectricCar : Vehicle
    {
        // Specific behavior for electric cars.
    }
}
