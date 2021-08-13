using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Resources.Chat.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AltVTutorial.TPlayer;

namespace AltVTutorial
{
    public class Commands : IScript
    {
        [CommandEvent(CommandEventType.CommandNotFound)]
        public void OnCommandNotFound(TPlayer.TPlayer tplayer, string command)
        {
            tplayer.SendChatMessage("{FF0000}Befehl " + command + " nicht gefunden!");
            return;
        }

        [Command("car")]
        public void CMD_car(TPlayer.TPlayer tplayer, string VehicleName, int R = 0, int G = 0, int B = 0)
        {
            IVehicle veh = Alt.CreateVehicle(Alt.Hash(VehicleName), new AltV.Net.Data.Position(tplayer.Position.X, tplayer.Position.Y + 1.0f, tplayer.Position.Z), tplayer.Rotation);
            if(veh != null)
            {
                veh.PrimaryColorRgb = new AltV.Net.Data.Rgba((byte)R, (byte)G, (byte)B, 255);
                tplayer.SendChatMessage("{04B404} Das Fahrzeug wurde erfolgreich gespawned!");
            }
            else
            {
                tplayer.SendChatMessage("{FF0000} Das Fahrzeug konnte nicht gespawned werden!");
            }
        }

        [Command("freezeme")]
        public void CMD_freezeme(TPlayer.TPlayer tplayer, bool freeze)
        {
            tplayer.Emit("freezePlayer", freeze);
            tplayer.SendChatMessage("{04B404}Der freeze Befehl wurde ausgeführt!");
        }
    }
}
