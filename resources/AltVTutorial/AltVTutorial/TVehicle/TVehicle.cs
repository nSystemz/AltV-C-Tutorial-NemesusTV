using AltV.Net;
using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltVTutorial.TVehicle
{
    public class TVehicle : Vehicle
    {

        public int SpielerID { get; set; }
        public int VehicleLock { get; set; }
        public String vehicleName { get; set; }
        public float Fuel { get; set; }

        public TVehicle(IServer server, IntPtr nativePointer, ushort id) : base(server, nativePointer, id)
        {
            SpielerID = 0;
            VehicleLock = 1;
            vehicleName = "";
            ManualEngineControl = true;
            Fuel = 100f;
        }
    }
}
