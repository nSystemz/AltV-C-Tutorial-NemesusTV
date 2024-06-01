using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltVTutorial.Models
{
    public class Cardealer
    {
        public int id {  get; set; }
        public string modelname { get; set; }
        public float posx { get; set; } 
        public float posy { get; set; }
        public float posz { get; set; }
        public float posa { get; set; }
        public int price { get; set; }
        public TVehicle.TVehicle vehicle { get; set; }
    }
}
