using AltV.Net;using AltV.Net.Elements.Entities;
using AltVTutorial.Models;
using AltVTutorial.TPlayer;
using AltVTutorial.TVehicle;
using System;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Xml.Linq;

namespace AltVTutorial
{
    class Server : Resource
    {
        public static IColShape testShape;

        public override void OnStart()
        {
            Discord.initDiscordBot();
            Utils.ConsoleLog("warning", "Tutorial Gamemode von NemesusTV erfolgreich geladen - test1234!");
            Utils.ConsoleLog("warning", "--> https://nemesus.de <--");
            Utils.adminLog("Server wurde gestartet", "TutorialServer");
            //MYSQL
            Datenbank.InitConnection();
            Datenbank.FahrzeugeLaden();
            Garagen.Garagen.GarageLoad();
            //Timer
            Timer paydayTimer = new Timer(OnPaydayTimer, null, 60000, 60000);
            Timer fuelTimer = new Timer(OnFuelTimer, null, 60000, 60000);
            Timer weatherTimer = new Timer(Events.OnWeatherChange, null, 60000 * 60, 60000 * 60);
            //Wettersystem
            Events.OnWeatherChange(null);
            //Colshapes
            testShape = Alt.CreateColShapeCircle(new Vector3(0, 0, 0), 3.0f);
            //Cardealer
            Datenbank.CardealerLoad();
        }

        public static void OnPaydayTimer(object state)
        {
            foreach(TPlayer.TPlayer tplayer in Alt.GetAllPlayers())
            {
                tplayer.payday--;
                if(tplayer.payday <= 0)
                {
                    tplayer.geld += 500;
                    Utils.sendNotification(tplayer, "info", "Du hast einen Payday in Höhe von 500$ erhalten!");
                    tplayer.payday = 60;
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
            Utils.ConsoleLog("warning", "Server wurde beendet!");
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
