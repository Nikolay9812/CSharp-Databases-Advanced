using SantaWorkshop.Models.Dwarfs.Contracts;
using SantaWorkshop.Models.Instruments.Contracts;
using SantaWorkshop.Models.Presents.Contracts;
using SantaWorkshop.Models.Workshops.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SantaWorkshop.Models.Workshops
{
    public class Workshop : IWorkshop
    {
        public void Craft(IPresent present, IDwarf dwarf)
        {
            while (dwarf.Energy > 0 && dwarf.Instruments.Any())
            {
                IInstrument curInstrument = dwarf.Instruments.First();

                while (!present.IsDone() && dwarf.Energy > 0 && !curInstrument.IsBroken())
                {
                    dwarf.Work();
                    present.GetCrafted();
                    curInstrument.Use();
                }

                if (curInstrument.IsBroken())
                {
                    dwarf.Instruments.Remove(curInstrument);
                }

                if (present.IsDone())
                {
                    break;
                }
            }
        }
    }
}
