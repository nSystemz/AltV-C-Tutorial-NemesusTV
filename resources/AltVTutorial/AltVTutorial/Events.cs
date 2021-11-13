using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using AltV.Net.Resources.Chat.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltVTutorial
{
    public class Events : IScript
    {
        [ScriptEvent(ScriptEventType.PlayerConnect)]
        public void OnPlayerConnect(TPlayer.TPlayer tplayer, string reason)
        {
            Alt.Log($"Der Spieler {tplayer.Name} hat den Server betreten!");
            tplayer.Spawn(new AltV.Net.Data.Position(-427, 1115, 326), 0);
            tplayer.Model = (uint)PedModel.Business01AMM;
        }

        [ScriptEvent(ScriptEventType.PlayerDisconnect)]
        public void OnPlayerDisconnect(TPlayer.TPlayer tplayer, string reason)
        {
            Alt.Log($"Spieler {tplayer.Name} hat den Server verlassen - Grund: {reason}!");
            //Spieler speichern
            Datenbank.AccountSpeichern(tplayer);
        }

        [ScriptEvent(ScriptEventType.PlayerEnterVehicle)]
        public static void OnPlayerEnterVehicle(IVehicle vehicle, TPlayer.TPlayer tplayer, byte seat)
        {
            int checkfrak = 0;
            bool v = vehicle.GetData<int>("VEHICLE_FRAKTION", out checkfrak);
            if (v && checkfrak > 0)
            {
                if (checkfrak != tplayer.Fraktion)
                {
                    tplayer.SendChatMessage("{FF0000}Dieses Fahrzeug gehört nicht zu deiner Fraktion!");
                }
            }
        }

        [ClientEvent("Event.Register")]
        public void OnPlayerRegister(TPlayer.TPlayer tplayer, String name, String password)
        {
            if (!Datenbank.IstAccountBereitsVorhanden(name))
            {
                if (!tplayer.Eingeloggt && name.Length > 3 && password.Length > 5)
                {
                    tplayer.SpielerName = name;
                    Datenbank.NeuenAccountErstellen(name, password);
                    tplayer.Spawn(new AltV.Net.Data.Position(-425, 1123, 325), 0);
                    tplayer.Model = (uint)PedModel.Business01AMM;
                    tplayer.Dimension = 0;
                    tplayer.Eingeloggt = true;
                    tplayer.Emit("CloseLoginHud");
                    tplayer.SendChatMessage("{00c900}Erfolgreich registriert!");
                    Alt.Emit("SaltyChat:EnablePlayer", tplayer);
                }
            }
            else
            {
                tplayer.Emit("SendErrorMessage", "Es existiert bereits ein Account mit dem eingegebenen Namen!");
            }
        }

        [ClientEvent("Event.Login")]
        public void OnPlayerLogin(TPlayer.TPlayer tplayer, String name, String password)
        {
            if (Datenbank.IstAccountBereitsVorhanden(name))
            {
                if (!tplayer.Eingeloggt && name.Length > 3 && password.Length > 5)
                {
                    if (Datenbank.PasswortCheck(name, password))
                    {
                        tplayer.SpielerName = name;
                        Datenbank.AccountLaden(tplayer);
                        tplayer.Spawn(new AltV.Net.Data.Position(-425, 1123, 325), 0);
                        tplayer.Model = (uint)PedModel.Business01AMM;
                        tplayer.Eingeloggt = true;
                        tplayer.Emit("CloseLoginHud");
                        tplayer.SendChatMessage("{00c900}Erfolgreich eingeloggt!");
                        tplayer.Health = 200;
                        tplayer.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Healthbar, 1.0);
                        tplayer.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Hungerbar, 0.5);
                        tplayer.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Thirstbar, 0.3);
                        Alt.Emit("SaltyChat:EnablePlayer", tplayer);
                    }
                    else
                    {
                        tplayer.Emit("SendErrorMessage", "Falsches Passwort eingeben oder ungültige Eingabe!");
                    }
                }
                else
                {
                    tplayer.Emit("SendErrorMessage", "Ungültige Eingaben, bitte korregieren!");
                }
            }
            else
            {
                tplayer.Emit("SendErrorMessage", "Es wurde kein Account mit dem Namen gefunden!");
            }
        }

        [ClientEvent("Event.SpawnVehicle")]
        public void OnSpawnVehicle(TPlayer.TPlayer tplayer, string VehicleName)
        {
            if (VehicleName == "Kein Fahrzeug") return;
            IVehicle veh = Alt.CreateVehicle(Alt.Hash(VehicleName), new AltV.Net.Data.Position(tplayer.Position.X, tplayer.Position.Y + 4.5f, tplayer.Position.Z), tplayer.Rotation);
            if (veh != null)
            {
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

        [ClientEvent("Event.successLockpickingServer")]
        public void OnSuccessLockpickingServer(TPlayer.TPlayer tplayer)
        {
            IVehicle veh = Utils.GetClosestVehicle(tplayer);
            if (veh != null)
            {
                veh.LockState = (VehicleLockState)1;
                Utils.sendNotification(tplayer, "info", "Du hast das Fahrzeug erfolgreich geknackt!");
            }
        }

        [ClientEvent("Event.failedLockpickingServer")]
        public void OnFailedLockpickingServer(TPlayer.TPlayer tplayer)
        {
            Utils.sendNotification(tplayer, "error", "Das Fahrzeug konnte nicht aufgeknackt werden!");
        }
    }
}
