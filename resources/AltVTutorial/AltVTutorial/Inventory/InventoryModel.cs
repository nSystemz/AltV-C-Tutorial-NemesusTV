using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltVTutorial.Inventory
{
    class InventoryModel
    {
        public int id { get; set; }
        public string hash { get; set; }
        public string description { get; set; }
        public int type { get; set; }
        public int amount { get; set; }
    }
}
