using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Resources.Chat.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltVTutorial
{
    public class Commands : IScript
    {
        [CommandEvent(CommandEventType.CommandNotFound)]
        public void OnCommandNotFound(IPlayer iplayer, string command)
        {
            iplayer.SendChatMessage("{FF0000}Befehl " + command + " nicht gefunden!");
            return;
        }

        [Command("car")]
        public void CMD_car(IPlayer iplayer, string VehicleName, int R = 0, int G = 0, int B = 0)
        {
            IVehicle veh = Alt.CreateVehicle(Alt.Hash(VehicleName), new AltV.Net.Data.Position(iplayer.Position.X, iplayer.Position.Y + 1.0f, iplayer.Position.Z), iplayer.Rotation);
            if(veh != null)
            {
                veh.PrimaryColorRgb = new AltV.Net.Data.Rgba((byte)R, (byte)G, (byte)B, 255);
                iplayer.SendChatMessage("{04B404} Das Fahrzeug wurde erfolgreich gespawned!");
            }
            else
            {
                iplayer.SendChatMessage("{FF0000} Das Fahrzeug konnte nicht gespawned werden!");
            }
        }

        [Command("freezeme")]
        public void CMD_freezeme(IPlayer iplayer, bool freeze)
        {
            iplayer.Emit("freezePlayer", freeze);
            iplayer.SendChatMessage("{04B404}Der freeze Befehl wurde ausgeführt!");
        }
    }
}
