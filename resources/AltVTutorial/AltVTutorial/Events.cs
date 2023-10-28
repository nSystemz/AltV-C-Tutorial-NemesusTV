using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using AltV.Net.Resources.Chat.Api;
using Newtonsoft.Json;
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

            tplayer.Emit("createGaragen", JsonConvert.SerializeObject(Garagen.Garagen.garageList));
        }

        [ScriptEvent(ScriptEventType.PlayerDisconnect)]
        public void OnPlayerDisconnect(TPlayer.TPlayer tplayer, string reason)
        {
            //Spieler speichern
            Datenbank.AccountSpeichern(tplayer);
            //Fahrzeuge speichern
            foreach(TVehicle.TVehicle tvehicle in Alt.GetAllVehicles())
            {
                if(tvehicle.vehicleID > 0 && tvehicle.SpielerID == tplayer.Id)
                {
                    Datenbank.SpeichereFahrzeuge(tvehicle);
                }
            }
            //Log
            Alt.Log($"Spieler {tplayer.Name} hat den Server verlassen!");
        }

        [ScriptEvent(ScriptEventType.PlayerEnterVehicle)]
        public static void OnPlayerEnterVehicle(TVehicle.TVehicle vehicle, TPlayer.TPlayer tplayer, byte seat)
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
                    tplayer.Model = (uint)PedModel.FreemodeMale01;
                    tplayer.Dimension = 0;
                    tplayer.Emit("CloseLoginHud");
                    if (!Utils.IsPlayerWhitelisted(tplayer))
                    {
                        tplayer.SendChatMessage("{#eb0e27}Du stehst nicht auf der Whitelist!");
                        tplayer.Kick($"Du stehst nicht auf der Whitelist - (Socialclubid: {tplayer.SocialClubId})!");
                        return;
                    }
                    Utils.UpdateMoneyHud(tplayer, tplayer.Geld);
                    Utils.UpdateStatsHud(tplayer);
                    tplayer.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Healthbar, 1.0);
                    tplayer.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Hungerbar, 0.5);
                    tplayer.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Thirstbar, 0.3);
                    tplayer.Eingeloggt = true;
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
                        if(tplayer.positions[0] != 0.0 && tplayer.positions[1] != 0.0 && tplayer.positions[2] != 0.0)
                        {
                            tplayer.Spawn(new AltV.Net.Data.Position(tplayer.positions[0], tplayer.positions[1], tplayer.positions[2]), 0);
                            tplayer.Rotation = new AltV.Net.Data.Rotation(0, 0, tplayer.positions[3]);
                        }
                        else
                        {
                            if(tplayer.Einreise == 0)
                            {
                                tplayer.Spawn(new AltV.Net.Data.Position(405, -993, -99), 0);
                                tplayer.SendChatMessage("{00c900}Warte auf Einreise ...");
                            }
                            else
                            {
                                tplayer.Spawn(new AltV.Net.Data.Position(-425, 1123, 325), 0);
                            }
                        }
                        tplayer.Model = (uint)PedModel.FreemodeMale01;
                        tplayer.Emit("CloseLoginHud");
                        tplayer.Health = 200;
                        Utils.UpdateMoneyHud(tplayer, tplayer.Geld);
                        Utils.UpdateStatsHud(tplayer);
                        tplayer.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Healthbar, 1.0);
                        tplayer.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Hungerbar, 0.5);
                        tplayer.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Thirstbar, 0.3);
                        if(!Utils.IsPlayerWhitelisted(tplayer))
                        {
                            tplayer.SendChatMessage("{#eb0e27}Du stehst nicht auf der Whitelist!");
                            tplayer.Kick("Du stehst nicht auf der Whitelist");
                            return;
                        }
                        tplayer.Eingeloggt = true;
                        tplayer.SendChatMessage("{00c900}Erfolgreich eingeloggt!");
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
            TVehicle.TVehicle veh = (TVehicle.TVehicle)Alt.CreateVehicle(Alt.Hash(VehicleName), new AltV.Net.Data.Position(tplayer.Position.X, tplayer.Position.Y + 4.5f, tplayer.Position.Z), tplayer.Rotation);
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
            TVehicle.TVehicle veh = Utils.GetClosestVehicle(tplayer);
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

        [ClientEvent("Event.startStopEngine")]
        public void OnStartStopEngine(TPlayer.TPlayer tplayer)
        {
            if (tplayer.IsInVehicle)
            {
                Commands.CMD_engine(tplayer);
            }
            else
            {
                if(tplayer.Fraktion == 1)
                {
                    Alt.Emit("showMDC");
                }
            }
        }
    }
}
