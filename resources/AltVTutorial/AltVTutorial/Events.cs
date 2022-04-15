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
        }
    }
}
