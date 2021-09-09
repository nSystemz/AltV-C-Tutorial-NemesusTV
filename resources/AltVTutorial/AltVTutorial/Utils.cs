using AltV.Net;
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
            //Bitte benutzt hier euren eigenen Webhook!
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
    }
}
