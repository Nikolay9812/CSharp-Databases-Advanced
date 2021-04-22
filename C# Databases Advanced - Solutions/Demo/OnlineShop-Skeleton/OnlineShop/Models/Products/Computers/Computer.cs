using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineShop.Common.Constants;
using OnlineShop.Models.Products.Components;
using OnlineShop.Models.Products.Peripherals;

namespace OnlineShop.Models.Products.Computers
{
    public abstract class Computer : Product, IComputer
    {
        //ToDo: ICollection???
        private readonly ICollection<IComponent> components;
        private readonly ICollection<IPeripheral> peripherals;


        protected Computer(int id, string manufacturer, string model, decimal price, double overallPerformance)
            : base(id, manufacturer, model, price, overallPerformance)
        {
            this.components = new List<IComponent>();
            this.peripherals = new List<IPeripheral>();
        }

        public override double OverallPerformance
        {
            //ToDO: Check logic. Overall performance only to components?
            get
            {
                if (this.Components.Count == 0)
                {
                    return base.OverallPerformance;
                }
                else
                {
                    return base.OverallPerformance + this.Components.Average(c => c.OverallPerformance);
                }
            }



        }

        public override decimal Price
        {
            //ToDo: Check logic
            get
            {
                decimal componentsPrice = this.Components.Sum(c => c.Price);
                decimal peripheralsPrice = this.Peripherals.Sum(p => p.Price);

                return base.Price + componentsPrice + peripheralsPrice;

            }

        }
        public IReadOnlyCollection<IComponent> Components => (IReadOnlyCollection<IComponent>)this.components;
        public IReadOnlyCollection<IPeripheral> Peripherals => (IReadOnlyCollection<IPeripheral>)this.peripherals;

        public void AddComponent(IComponent component)
        {
            //ToDo: Try LINQ

            foreach (var currentComponent in this.Components)
            {
                if (currentComponent.GetType().Name == component.GetType().Name)
                {
                    throw new ArgumentException(
                        string.Format(ExceptionMessages.ExistingComponent, component.GetType().Name,
                            this.GetType().Name, this.Id));
                }


            }

            this.components.Add(component);
        }

        public IComponent RemoveComponent(string componentType)
        {
            // bool isExisting = false;

            //ToDo: Try LINQ
            //foreach (var currentComponent in this.Components)
            //{
            //    if (currentComponent.GetType().Name == componentType.GetType().Name)
            //    {
            //        isExisting = true;
            //        break;
            //    }

            //}


            IComponent component = this.Components.FirstOrDefault(c => c.GetType().Name == componentType);

            if (component == null || this.Components.Count == 0)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.NotExistingComponent, componentType,
                    this.GetType().Name, this.Id));

            }

            this.components.Remove(component);

            return component;
        }

        public void AddPeripheral(IPeripheral peripheral)
        {
            foreach (var currentPeripheral in this.Peripherals)
            {
                if (currentPeripheral.GetType().Name == peripheral.GetType().Name)
                {
                    throw new ArgumentException(
                        string.Format(ExceptionMessages.ExistingPeripheral, peripheral.GetType().Name,
                            this.GetType().Name, this.Id));
                }


            }

            this.peripherals.Add(peripheral);
        }

        public IPeripheral RemovePeripheral(string peripheralType)
        {

            IPeripheral peripheral = this.Peripherals.FirstOrDefault(p => p.GetType().Name == peripheralType);

            if (peripheral == null || this.Peripherals.Count == 0)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.NotExistingPeripheral, peripheralType,
                    this.GetType().Name, this.Id));

            }

            this.peripherals.Remove(peripheral);

            return peripheral;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            sb.AppendLine($" Components ({this.Components.Count}):");

            //ToDo: If empty collection??

            foreach (var component in this.Components)
            {
                sb.AppendLine($"  {component}");
            }

            if (this.Peripherals.Count == 0)
            {
                sb.AppendLine($" Peripherals ({this.Peripherals.Count}); Average Overall Performance (0.00):");
            }
            else
            {
                sb.AppendLine($" Peripherals ({this.Peripherals.Count}); Average Overall Performance ({this.Peripherals.Average(p => p.OverallPerformance):F2}):");
            }

            //ToDo: If empty collection??

            foreach (var peripheral in this.Peripherals)
            {
                sb.AppendLine(peripheral.ToString());
            }

            return sb.ToString().Trim();
        }
    }
}
