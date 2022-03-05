using AltV.Net;
using AltV.Net.Elements.Entities;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltVTutorial
{
    public class Utils : IScript
    {
        public static void adminLog(string text, string username)
        {
            return;
            //Bitte benutzt hier euren eigenen Webhook + dann das return entfernen!
            HTTP.Post("https://discord.com/api/webhooks/881829631907487754/aLWht7jKZjTxDUFox4l9INcv6d8LzTQeMY8ZQ_qMK-EeGXqHrPzXd0lY9aTd_tnhNSGl", new System.Collections.Specialized.NameValueCollection()
            {
                {
                    "username",
                    username
                },
                {
                    "content",
                    text
                }
            });
        }

        public static void sendNotification(TPlayer.TPlayer tplayer, string status, string text)
        {
            tplayer.Emit("sendNotification", status, text);
        }

        public static void UpdateMoneyHud(TPlayer.TPlayer tplayer, long money)
        {
            tplayer.Emit("updateMoneyHud", money);
        }

        public static TPlayer.TPlayer GetPlayerByName(string name)
        {
            foreach(TPlayer.TPlayer p in Alt.GetAllPlayers())
            {
                if(p.Name.ToLower().Contains(name.ToLower()))
                {
                    return p;
                }
            }
            return null;
        }

        public static bool IsPlayerWhitelisted(TPlayer.TPlayer tplayer)
        {
            MySqlCommand command = Datenbank.Connection.CreateCommand();
            command.CommandText = "SELECT id FROM whitelist WHERE socialclubid=@socialclubid LIMIT 1";
            command.Parameters.AddWithValue("socialclubid", tplayer.SocialClubId);
            using(MySqlDataReader reader = command.ExecuteReader())
            {
                if(reader.HasRows)
                {
                    return true;
                }
            }
            return false;
        }

        public static TVehicle.TVehicle GetClosestVehicle(TPlayer.TPlayer tplayer, float distance = 2.75f)
        {
            TVehicle.TVehicle vehicle = null;
            foreach(TVehicle.TVehicle veh in Alt.GetAllVehicles())
            {
                AltV.Net.Data.Position vehPos = veh.Position;
                float distanceVehicleToPlayer = tplayer.Position.Distance(vehPos);

                if(distanceVehicleToPlayer < distance && tplayer.Dimension == veh.Dimension)
                {
                    distance = distanceVehicleToPlayer;
                    vehicle = veh;
                }
            }
            return vehicle;
        }
    }
}
