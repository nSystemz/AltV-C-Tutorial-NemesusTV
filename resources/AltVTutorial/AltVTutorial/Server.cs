using AltV.Net;
using AltV.Net.Elements.Entities;
using AltVTutorial.TPlayer;
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
            //Timer
            Timer paydayTimer = new Timer(OnPaydayTimer, null, 10000, 10000);
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

        public override void OnStop()
        {
            Alt.Log("Server wurde beendet!");
        }

        public override IEntityFactory<IPlayer> GetPlayerFactory()
        {
            return new TPlayerFactory();
        }
    }
}
