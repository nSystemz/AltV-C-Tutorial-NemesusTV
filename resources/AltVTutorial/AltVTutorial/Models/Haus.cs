using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltVTutorial.Models
{
    public class Haus
    {
        public int id { get; set; }
        public string strasse { get; set; }
        [NotMapped]
        public int hausnummer { get; set; }
    }
}
