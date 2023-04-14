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
        public int vehicleID { get; set; }
        public int SpielerID { get; set; }
        public int VehicleLock { get; set; }
        public String vehicleName { get; set; }
        public float Fuel { get; set; }
        public int Garage { get; set; }

        public TVehicle(ICore core, IntPtr nativePointer, ushort id) : base(core, nativePointer, id)
        {
            vehicleID = 0;
            SpielerID = 0;
            VehicleLock = 1;
            vehicleName = "";
            ManualEngineControl = true;
            Fuel = 100f;
            Garage = -1;
        }
    }
}
