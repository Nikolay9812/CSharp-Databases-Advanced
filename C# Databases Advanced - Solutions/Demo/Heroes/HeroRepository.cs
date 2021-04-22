using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Heroes
{
    public class HeroRepository
    {
        private List<Hero> data;

        public HeroRepository()
        {
            this.data = new List<Hero>();
        }

        public int Count { get { return this.data.Count; } }

        public void Add(Hero hero)
        {
            this.data.Add(hero);
        }

        public bool Remove(string name)
        {
            Hero hero = this.data.FirstOrDefault(n => n.Name == name);

            if (hero != null)
            {
                this.data.Remove(hero);
                return true;
            }

            return false;
        }

        public Hero GetHeroWithHighestStrength()
        {
            Hero hero = this.data
                .OrderByDescending(h => h.Item.Strength)
                .FirstOrDefault();
            return hero;
        }

        public Hero GetHeroWithHighestAbility()
        {
            Hero hero = this.data
                .OrderByDescending(h => h.Item.Ability)
                .FirstOrDefault();
            return hero;
        }

        public Hero GetHeroWithHighestIntelligence()
        {
            Hero hero = this.data
                .OrderByDescending(h => h.Item.Intelligence)
                .FirstOrDefault();
            return hero;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var hero in this.data)
            {
                sb.AppendLine(hero.ToString());
            }

            return sb.ToString().TrimEnd();
        }
    }
}
