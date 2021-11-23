using AltV.Net;
using AltV.Net.Elements.Entities;
using AltVTutorial.TPlayer;
using AltVTutorial.TVehicle;
using System.Threading;

namespace AltVTutorial
{
    class Server : Resource
    {
        public override void OnStart()
        {
            Alt.Log("Server wurde gestartet!");
            Utils.adminLog("Server wurde gestartet", "TutorialServer");
            //MYSQL
            Datenbank.InitConnection();
            Datenbank.FahrzeugeLaden();
            //Timer
            Timer paydayTimer = new Timer(OnPaydayTimer, null, 60000, 60000);
            Timer fuelTimer = new Timer(OnFuelTimer, null, 60000, 60000);
        }

        public static void OnPaydayTimer(object state)
        {
            foreach(TPlayer.TPlayer tplayer in Alt.GetAllPlayers())
            {
                tplayer.Payday--;
                if(tplayer.Payday <= 0)
                {
                    tplayer.Geld += 500;
                    Utils.sendNotification(tplayer, "info", "Du hast einen Payday in Höhe von 500$ erhalten!");
                    tplayer.Payday = 60;
                }
            }
        }

        public static void OnFuelTimer(object state)
        {
            foreach(TVehicle.TVehicle tvehicle in Alt.GetAllVehicles())
            {
                if(tvehicle.Fuel > 0 && tvehicle.EngineOn == true)
                {
                    tvehicle.Fuel--;
                    if(tvehicle.Fuel <= 0)
                    {
                        tvehicle.EngineOn = false;
                    }
                }
            }
        }

        public override void OnStop()
        {
            Alt.Log("Server wurde beendet!");
        }

        public override IEntityFactory<IPlayer> GetPlayerFactory()
        {
            return new TPlayerFactory();
        }

        public override IEntityFactory<IVehicle> GetVehicleFactory()
        {
            return new TVehicleFactory();
        }
    }
}
