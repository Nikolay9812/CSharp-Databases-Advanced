using SantaWorkshop.Models.Presents.Contracts;
using SantaWorkshop.Utilities.Messages;
using System;

namespace SantaWorkshop.Models.Presents
{
    public class Present : IPresent
    {
        private const int Units = 10;

        private string name;
        private int energyRequired;

        public Present(string name, int energyRequired)
        {
            this.Name = name;
            this.EnergyRequired = energyRequired;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            private set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.InvalidPresentName);
                }
                this.name = value;
            }
        }

        public int EnergyRequired
        {
            get
            {
                return this.energyRequired;
            }
            private set
            {
                this.energyRequired = value > 0 ? value : 0;
            }
        }

        public void GetCrafted()
        {
            this.EnergyRequired -= Units;
        }

        public bool IsDone()
        {
            return this.EnergyRequired == 0;
        }
    }
}
