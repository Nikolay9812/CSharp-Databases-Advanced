using SantaWorkshop.Models.Presents.Contracts;
using SantaWorkshop.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SantaWorkshop.Repositories
{
    class PresentRepository : IRepository<IPresent>
    {
        private readonly ICollection<IPresent> models;

        public PresentRepository()
        {
            this.models = new List<IPresent>();
        }

        public IReadOnlyCollection<IPresent> Models => (IReadOnlyCollection<IPresent>)this.models;

        public void Add(IPresent model)
        {
            this.models.Add(model);
        }

        public IPresent FindByName(string name)
        {
            return this.models.FirstOrDefault(m => m.Name == name);
        }

        public bool Remove(IPresent model)
        {
            return this.models.Remove(model);
        }
    }
}
