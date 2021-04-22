using System;
using System.Collections.Generic;
using System.Text;

namespace SantaWorkshop.Models.Dwarfs
{
    public class SleepyDwarf : Dwarf
    {
        private const int InitialEnergy = 50;
        private const int DecreasEnergy = 5;

        public SleepyDwarf(string name)
            : base(name, InitialEnergy)
        {
        }

        public override void Work()
        {
            base.Work();
            this.Energy -= DecreasEnergy;
        }
    }
}
