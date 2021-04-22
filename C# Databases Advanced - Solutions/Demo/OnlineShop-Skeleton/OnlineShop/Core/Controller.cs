using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using OnlineShop.Common.Constants;
using OnlineShop.Models.Products.Components;
using OnlineShop.Models.Products.Computers;
using OnlineShop.Models.Products.Peripherals;

namespace OnlineShop.Core
{
    public class Controller : IController
    {
        private readonly ICollection<IComputer> computers;

        //ToDO: namespace IComponent (2 namespaces of IComponent)
        //change to IComponent?
        private readonly ICollection<IComponent> components;
        private readonly ICollection<IPeripheral> peripherals;


        public Controller()
        {
            this.computers = new List<IComputer>();
            this.components = new List<IComponent>();
            this.peripherals = new List<IPeripheral>();


        }



        public IReadOnlyCollection<IComputer> Computers => (IReadOnlyCollection<IComputer>)this.computers;
        public IReadOnlyCollection<IComponent> Components => (IReadOnlyCollection<IComponent>)this.components;
        public IReadOnlyCollection<IPeripheral> Peripherals => (IReadOnlyCollection<IPeripheral>)this.peripherals;

        public string AddComputer(string computerType, int id, string manufacturer, string model, decimal price)
        {
            if (this.Computers.Any(c => c.Id == id))
            {
                throw new ArgumentException(ExceptionMessages.ExistingComputerId);
            }


            IComputer computer = null;

            if (computerType == nameof(DesktopComputer))
            {
                computer = new DesktopComputer(id, manufacturer, model, price);
            }
            else if (computerType == nameof(Laptop))
            {
                computer = new Laptop(id, manufacturer, model, price);
            }
            else
            {
                throw new ArgumentException(ExceptionMessages.InvalidComputerType);
            }

            this.computers.Add(computer);

            return string.Format(SuccessMessages.AddedComputer, computer.Id);

        }

        public string AddPeripheral(int computerId, int id, string peripheralType, string manufacturer, string model, decimal price,
            double overallPerformance, string connectionType)
        {
            IComputer computer = GetComputer(computerId);
            IPeripheral peripheral = this.Peripherals.FirstOrDefault(c => c.Id == id);

            if (peripheral != null)
            {
                throw new ArgumentException(ExceptionMessages.ExistingPeripheralId);
            }

            IPeripheral newPeripheral = null;

            if (peripheralType == nameof(Headset))
            {
                newPeripheral =
                    new Headset(id, manufacturer, model, price, overallPerformance, connectionType);
            }
            else if (peripheralType == nameof(Keyboard))
            {
                newPeripheral =
                    new Keyboard(id, manufacturer, model, price, overallPerformance, connectionType);
            }
            else if (peripheralType == nameof(Monitor))
            {
                newPeripheral =
                    new Monitor(id, manufacturer, model, price, overallPerformance, connectionType);
            }
            else if (peripheralType == nameof(Mouse))
            {
                newPeripheral =
                    new Mouse(id, manufacturer, model, price, overallPerformance, connectionType);
            }
            else
            {
                throw new ArgumentException(ExceptionMessages.InvalidPeripheralType);
            }

            computer.AddPeripheral(newPeripheral);
            this.peripherals.Add(newPeripheral);

            return string.Format(SuccessMessages.AddedPeripheral, newPeripheral.GetType().Name, newPeripheral.Id,
                computer.Id);
        }

        public string RemovePeripheral(string peripheralType, int computerId)
        {
            IComputer computer = this.Computers.FirstOrDefault(c => c.Id == computerId);

            IPeripheral peripheral = computer.Peripherals
                .FirstOrDefault(p => p.GetType().Name == peripheralType);

            computer.RemovePeripheral(peripheralType);
            this.peripherals.Remove(peripheral);
            return $"Successfully removed {peripheralType} with id {peripheral.Id}.";
        }

        public string AddComponent(int computerId, int id, string componentType, string manufacturer, string model, decimal price,
            double overallPerformance, int generation)
        {
            IComputer computer = GetComputer(computerId);
            IComponent component = this.Components.FirstOrDefault(c => c.Id == id);

            if (component != null)
            {
                throw new ArgumentException(ExceptionMessages.ExistingComponentId);
            }

            IComponent newComponent = null;

            if (componentType == nameof(CentralProcessingUnit))
            {
                newComponent =
                    new CentralProcessingUnit(id, manufacturer, model, price, overallPerformance, generation);
            }
            else if (componentType == nameof(Motherboard))
            {
                newComponent =
                    new Motherboard(id, manufacturer, model, price, overallPerformance, generation);
            }
            else if (componentType == nameof(PowerSupply))
            {
                newComponent =
                    new PowerSupply(id, manufacturer, model, price, overallPerformance, generation);
            }
            else if (componentType == nameof(RandomAccessMemory))
            {
                newComponent =
                    new RandomAccessMemory(id, manufacturer, model, price, overallPerformance, generation);
            }
            else if (componentType == nameof(SolidStateDrive))
            {
                newComponent =
                    new SolidStateDrive(id, manufacturer, model, price, overallPerformance, generation);
            }
            else if (componentType == nameof(VideoCard))
            {
                newComponent =
                    new VideoCard(id, manufacturer, model, price, overallPerformance, generation);
            }
            else
            {
                throw new ArgumentException(ExceptionMessages.InvalidComponentType);
            }

            computer.AddComponent(newComponent);
            this.components.Add(newComponent);

            return string.Format(SuccessMessages.AddedComponent, newComponent.GetType().Name, newComponent.Id,
                computer.Id);

        }

        public string RemoveComponent(string componentType, int computerId)
        {
            IComputer computer = GetComputer(computerId);

            IComponent component = computer.Components
                .FirstOrDefault(c => c.GetType().Name == componentType);

            computer.RemoveComponent(componentType);
            this.components.Remove(component);
            return $"Successfully removed {componentType} with id {component.Id}.";
        }

        public string BuyComputer(int id)
        {
            IComputer computer = GetComputer(id);

            bool isBought = this.computers.Remove(computer);

            return computer.ToString();

        }

        public string BuyBest(decimal budget)
        {
            if (this.Computers.Count == 0)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.CanNotBuyComputer, budget));
            }

            var lowestPrice = this.Computers.OrderBy(c => c.Price).FirstOrDefault().Price;

            if (budget < lowestPrice)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.CanNotBuyComputer, budget));

            }

            IComputer computer = this.Computers
                .OrderByDescending(c => c.OverallPerformance)
                .FirstOrDefault(c => c.Price <= budget);

            string message = computer.ToString();

            this.computers.Remove(computer);

            return message;

        }

        public string GetComputerData(int id)
        {
            IComputer computer = GetComputer(id);

            return computer.ToString();
        }

        private IComputer GetComputer(int id)
        {
            IComputer computer = this.Computers.FirstOrDefault(c => c.Id == id);
            if (computer == null)
            {
                throw new ArgumentException(ExceptionMessages.NotExistingComputerId);
            }

            return computer;
        }
    }
}
