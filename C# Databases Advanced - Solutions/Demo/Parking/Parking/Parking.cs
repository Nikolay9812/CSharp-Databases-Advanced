using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parking
{
    public class Parking
    {
        private List<Car> data;

        private Parking()
        {
            this.data = new List<Car>();
        }

        public Parking(string type, int capacity)
            : this()
        {
            this.Type = type;
            this.Capacity = capacity;
        }

        public string Type { get; set; }

        public int Capacity { get; set; }

        public int Count { get { return this.data.Count; } }

        public void Add(Car car)
        {
            if (this.data.Count + 1 <= this.Capacity)
            {
                this.data.Add(car);
            }
        }

        public bool Remove(string manufacturer, string model)
        {
            Car car = this.data
                .FirstOrDefault(c => c.Manufacturer == manufacturer && c.Model == model);

            if (car != null)
            {
                this.data.Remove(car);
                return true;
            }

            return false;
        }

        public Car GetLatestCar()
        {
            Car car = this.data
                .OrderByDescending(c => c.Year)
                .FirstOrDefault();

            return car;
        }

        public Car GetCar(string manufacturer, string model)
        {
            Car car = this.data
                .FirstOrDefault(c => c.Manufacturer == manufacturer && c.Model == model);

            return car;
        }

        public string GetStatistics()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"The cars are parked in {this.Type}:");
            foreach (var car in this.data)
            {
                sb.AppendLine(car.ToString());
            }
            return sb.ToString().TrimEnd();
        }
    }
}
