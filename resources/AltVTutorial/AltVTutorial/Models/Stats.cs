using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltVTutorial.Models
{
    public class Stats
    {
        public string name { get; set; }
        public int level { get; set; }
        public int money { get; set; }

        public Stats(string name, int level, int money)
        {
            this.name = name;
            this.level = level;
            this.money = money;
        }
    }
}
