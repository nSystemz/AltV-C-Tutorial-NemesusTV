using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using AltV.Net.Resources.Chat.Api;
using System;
using System.Globalization;
using System.IO;
using System.Numerics;

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
        public void CMD_car(TPlayer.TPlayer tplayer, string VehicleName, string PlayerName = null, int R = 0, int G = 0, int B = 0)
        {
            if(!tplayer.IsSpielerAdmin((int)TPlayer.TPlayer.AdminRanks.Supporter))
            {
                tplayer.SendChatMessage("{FF0000}Dein Adminlevel ist zu niedrig!");
                return;
            }
            TVehicle.TVehicle veh = (TVehicle.TVehicle)Alt.CreateVehicle(Alt.Hash(VehicleName), new AltV.Net.Data.Position(tplayer.Position.X, tplayer.Position.Y + 2.5f, tplayer.Position.Z), tplayer.Rotation);
            if(veh != null)
            {
                veh.LockState = (VehicleLockState)1;
                veh.PrimaryColorRgb = new AltV.Net.Data.Rgba((byte)R, (byte)G, (byte)B, 255);
                if (PlayerName == null)
                {
                    tplayer.SendChatMessage("{04B404} Das Fahrzeug wurde erfolgreich gespawned!");
                    Utils.adminLog($"Der Spieler {tplayer.SpielerName} hat ein {VehicleName} gespawned!", "TutorialServer");
                    Utils.sendNotification(tplayer, "info", "Fahrzeug wurde erfolgreich gespawned!");
                }
                else
                {
                    TPlayer.TPlayer target = Utils.GetPlayerByName(PlayerName);
                    if(target == null)
                    {
                        Utils.sendNotification(tplayer, "error", "Ungültiger Spieler!");
                        return;
                    }
                    tplayer.SendChatMessage("{04B404} Du hast erfolgreich ein Fahrzeug gespawnt und dieses einem Spieler zugewiesen!");
                    Utils.adminLog($"Der Spieler {tplayer.SpielerName} hat ein {VehicleName} für {target.Name} erstellt!", "TutorialServer");
                    Utils.sendNotification(tplayer, "info", "Fahrzeug wurde erfolgreich für einen Spieler erstellt!");
                    veh.SpielerID = target.SpielerID;
                    veh.VehicleLock = (int)veh.LockState;
                    veh.vehicleName = VehicleName;
                    veh.NumberplateText = VehicleName;
                    Datenbank.FahrzeugErstellen(veh);
                }
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
        public void CMD_telexyz(TPlayer.TPlayer tplayer, double x, double y, double z)
        {
            if (!tplayer.IsSpielerAdmin((int)TPlayer.TPlayer.AdminRanks.Administrator))
            {
                tplayer.SendChatMessage("{FF0000}Dein Adminlevel ist zu niedrig!");
                return;
            }
            AltV.Net.Data.Position positon = new AltV.Net.Data.Position(-421.34506f, 1122.1318f, 325.85352f);
            tplayer.Position = positon;
            tplayer.Dimension = 0;
            tplayer.SendChatMessage("{04B404}Du hast dich erfolgreich teleportiert!");
            return;
        }

        [Command("fraktionsinfo")]
        public void CMD_fraktionsinfo(TPlayer.TPlayer tplayer)
        {
            tplayer.SendChatMessage($"Du bist in der Fraktion {tplayer.HoleFraktionsName()} und hast den Rang {tplayer.HoleRangName()}!");
            return;
        }

        [Command("save", greedyArg: true)]
        public void CMD_save(TPlayer.TPlayer tplayer, string position)
        {
            if(!tplayer.IsSpielerAdmin((int)TPlayer.TPlayer.AdminRanks.Moderator))
            {
                tplayer.SendChatMessage("{FF0000}Dein Adminlevel ist zu niedrig!");
                return;
            }

            string status = (tplayer.IsInVehicle) ? "Im Fahrzeug" : "Zu Fuß";
            Vector3 pos = (tplayer.IsInVehicle) ? tplayer.Vehicle.Position : tplayer.Position;
            Vector3 rot = (tplayer.IsInVehicle) ? tplayer.Vehicle.Rotation : tplayer.Rotation;

            string message =
            $"{status} -> {position}: {pos.X.ToString(new CultureInfo("en-US")):N3}, {pos.Y.ToString(new CultureInfo("en-US")):N3}, {pos.Z.ToString(new CultureInfo("en-US")):N3}, {rot.X.ToString(new CultureInfo("en-US")):N3}, {rot.Y.ToString(new CultureInfo("en-US")):N3}, {rot.Z.ToString(new CultureInfo("en-US")):N3}";

            tplayer.SendChatMessage(message);

            using(StreamWriter file = new StreamWriter(@"./savedpositions.txt", true))
            {
                file.WriteLine(message);
            }
        }

        [Command("makeleader")]
        public void CMD_makeleader(TPlayer.TPlayer tplayer, string playertarget, int frak)
        {
            if (!tplayer.IsSpielerAdmin((int)TPlayer.TPlayer.AdminRanks.Administrator))
            {
                tplayer.SendChatMessage("{FF0000}Dein Adminlevel ist zu niedrig!");
                return;
            }
            TPlayer.TPlayer target = Utils.GetPlayerByName(playertarget);
            if(target == null)
            {
                tplayer.SendChatMessage("{FF0000}Ungültiger Spieler!");
                return;
            }
            if(frak < 0 || frak > TPlayer.TPlayer.Fraktionen.Length)
            {
                tplayer.SendChatMessage("{FF0000}Ungültige Fraktion!");
                return;
            }
            target.Fraktion = frak;
            target.Rang = 6;
            tplayer.SendChatMessage($"Du hast {target.Name} zum Chef der Fraktion {TPlayer.TPlayer.Fraktionen[frak]}!");
            target.SendChatMessage($"Du wurdest von {tplayer.Name} zum Chef der Fraktion {TPlayer.TPlayer.Fraktionen[frak]} gemacht!");
        }

        [Command("invite")]
        public void CMD_invite(TPlayer.TPlayer tplayer, string playertarget)
        {
            if(tplayer.Fraktion == 0 || tplayer.Rang < 6)
            {
                tplayer.SendChatMessage("{FF0000}Du bist in keiner Fraktion oder dein Rang ist zu niedrig!");
                return;
            }
            TPlayer.TPlayer target = Utils.GetPlayerByName(playertarget);
            if (target == null)
            {
                tplayer.SendChatMessage("{FF0000}Ungültiger Spieler!");
                return;
            }
            target.Fraktion = tplayer.Fraktion;
            target.Rang = 1;
            tplayer.SendChatMessage($"Du hast {target.Name} zur Fraktion {TPlayer.TPlayer.Fraktionen[tplayer.Fraktion]} eingeladen!");
            target.SendChatMessage($"Du wurdest von {tplayer.Name} zur Fraktion {TPlayer.TPlayer.Fraktionen[tplayer.Fraktion]} eingeladen!");
        }

        [Command("pistole")]
        public void CMD_pistole(TPlayer.TPlayer tplayer)
        {
            if(tplayer.Fraktion != 1)
            {
                string msg = "{FF0000}Du bist nicht im PD!";
                tplayer.SendChatMessage(msg);
                return;
            }
            tplayer.GiveWeapon(AltV.Net.Enums.WeaponModel.Pistol, 500, true);
            tplayer.SendChatMessage("{04B404}Du hast dir eine Pistole gegeben!");
        }

        [Command("nativeuitest")]
        public void CMD_nativeuitest(TPlayer.TPlayer tplayer)
        {
            tplayer.Emit("nativeUITest");
        }

        [Command("lockpicking")]
        public void CMD_lockpocking(TPlayer.TPlayer tplayer)
        {
            TVehicle.TVehicle veh = Utils.GetClosestVehicle(tplayer);
            if(veh != null)
            {
                tplayer.Emit("showLockpicking");
            }
            else
            {
                Utils.sendNotification(tplayer, "error", "Du bist nicht in der Nähe von einem Fahrzeug!");
            }
        }

        [Command("fuel")]
        public void CMD_fuel(TPlayer.TPlayer tplayer)
        {
            TVehicle.TVehicle tvehicle = null;
            tvehicle = (TVehicle.TVehicle)tplayer.Vehicle;
            if (tvehicle == null)
            {
                Utils.sendNotification(tplayer, "error", "Ungültiges Fahrzeug!");
                return;
            }
            Utils.sendNotification(tplayer, "info", "Tankstatus: " + Math.Round(tvehicle.Fuel, 2) + "l");
        }

        [Command("engine")]
        public static void CMD_engine(TPlayer.TPlayer tplayer)
        {
            TVehicle.TVehicle tvehicle = null;
            tvehicle = (TVehicle.TVehicle)tplayer.Vehicle;
            tvehicle.ManualEngineControl = true;
            if (tvehicle == null)
            {
                Utils.sendNotification(tplayer, "error", "Ungültiges Fahrzeug!");
                return;
            }
            if (tvehicle.EngineOn == false)
            {
                if(tvehicle.Fuel > 0)
                {
                    Utils.sendNotification(tplayer, "success", "Motor erfolgreich gestartet!");
                    tvehicle.EngineOn = true;
                }
                else
                {
                    Utils.sendNotification(tplayer, "error", "Der Tank ist leer!");
                }
            }
            else
            {
                Utils.sendNotification(tplayer, "success", "Motor erfolgreich abgeschaltet!");
                tvehicle.EngineOn = false;
            }
        }
    }
}
