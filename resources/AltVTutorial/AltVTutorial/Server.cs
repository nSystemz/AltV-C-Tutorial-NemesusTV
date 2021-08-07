using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltVTutorial
{
    class Server : Resource
    {
        public override void OnStart()
        {
            Alt.Log("Server wurde gestartet!");
            //MYSQL
            Datenbank.InitConnection();
        }

        public override void OnStop()
        {
            Alt.Log("Server wurde beendet!");
        }
    }
}
