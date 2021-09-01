using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Resources.Chat.Api;

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
            if(!tplayer.IsSpielerAdmin((int)TPlayer.TPlayer.AdminRanks.Supporter))
            {
                tplayer.SendChatMessage("{FF0000}Dein Adminlevel ist zu niedrig!");
                return;
            }
            IVehicle veh = Alt.CreateVehicle(Alt.Hash(VehicleName), new AltV.Net.Data.Position(tplayer.Position.X, tplayer.Position.Y + 2.25f, tplayer.Position.Z), tplayer.Rotation);
            if(veh != null)
            {
                veh.PrimaryColorRgb = new AltV.Net.Data.Rgba((byte)R, (byte)G, (byte)B, 255);
                tplayer.SendChatMessage("{04B404} Das Fahrzeug wurde erfolgreich gespawned!");
                Utils.adminLog($"Der Spieler {tplayer.SpielerName} hat ein {VehicleName} gespawned!", "TutorialServer");
                Utils.sendNotification(tplayer, "info", "Fahrzeug wurde erfolgreich gespawned!");
            }
            else
            {
                tplayer.SendChatMessage("{FF0000} Das Fahrzeug konnte nicht gespawned werden!");
                Utils.sendNotification(tplayer, "error", "Das Fahrzeug konnte nicht erstellt werden!");
            }
        }

        [Command("freezeme")]
        public void CMD_freezeme(TPlayer.TPlayer tplayer, bool freeze)
        {
            if(!tplayer.IsSpielerAdmin((int)TPlayer.TPlayer.AdminRanks.Moderator))
            {
                tplayer.SendChatMessage("{FF0000}Dein Adminlevel ist zu niedrig!");
                return;
            }
            tplayer.Emit("freezePlayer", freeze);
            tplayer.SendChatMessage("{04B404}Der freeze Befehl wurde ausgeführt!");
        }

        [Command("telexyz")]
        public void CMD_telexyz(TPlayer.TPlayer tplayer, float x, float y, float z)
        {
            if (!tplayer.IsSpielerAdmin((int)TPlayer.TPlayer.AdminRanks.Administrator))
            {
                tplayer.SendChatMessage("{FF0000}Dein Adminlevel ist zu niedrig!");
                return;
            }
            AltV.Net.Data.Position positon = new AltV.Net.Data.Position(x, y, z+0.2f);
            tplayer.Position = positon;
            tplayer.SendChatMessage("{04B404}Du hast dich erfolgreich teleportiert!");
            return;
        }

        [Command("fraktionsinfo")]
        public void CMD_fraktionsinfo(TPlayer.TPlayer tplayer)
        {
            tplayer.SendChatMessage($"Du bist in der Fraktion {tplayer.HoleFraktionsName()} und hast den Rang {tplayer.HoleRangName()}!");
            return;
        }
    }
}
