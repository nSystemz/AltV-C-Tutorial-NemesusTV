using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using AltV.Net.Resources.Chat.Api;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AltVTutorial
{

    public class Events : IScript
    {
        public enum GtaWeather
        {
            ExtraSunny = 0,
            Clear = 1,
            Clouds = 2,
            Smog = 3,
            Foggy = 4,
            Overcast = 5,
            Rain = 6,
            Thunder = 7,
            LightRain = 8,
            SmoggyLightRain = 9,
            VeryLightSnow = 10,
            WindyLightSnow = 11,
            LightSnow = 12,
            Christmas = 13,
            Halloween = 14
        }

        public static String weatherDataTemp = "";
        public static String weatherData = "clear sky";
        public static int weatherId = 1;

        [ScriptEvent(ScriptEventType.PlayerConnect)]
        public void OnPlayerConnect(TPlayer.TPlayer tplayer, string reason)
        {
            Utils.ConsoleLog("info", $"Der Spieler {tplayer.Name} hat den Server betreten!");
            tplayer.Spawn(new AltV.Net.Data.Position(-427, 1115, 326), 0);
            tplayer.Model = (uint)PedModel.FreemodeMale01;

            tplayer.Emit("createGaragen", JsonConvert.SerializeObject(Garagen.Garagen.garageList));

            foreach(var target in Alt.GetAllPlayers().Where(p => p != tplayer))
            {
                target.Emit("addToNametag", tplayer.id, tplayer.name);
                tplayer.Emit("addToNametag", target.Id, target.Name);
            }
        }

        [ScriptEvent(ScriptEventType.PlayerDisconnect)]
        public void OnPlayerDisconnect(TPlayer.TPlayer tplayer, string reason)
        {
            //Spieler speichern
            Datenbank.AccountSpeichern(tplayer);
            //Fahrzeuge speichern
            foreach (TVehicle.TVehicle tvehicle in Alt.GetAllVehicles())
            {
                if (tvehicle.vehicleID > 0 && tvehicle.SpielerID == tplayer.Id)
                {
                    Datenbank.SpeichereFahrzeuge(tvehicle);
                }
            }
            foreach (var target in Alt.GetAllPlayers())
            {
                target.Emit("removeFromNametag", tplayer.id);
            }
            //Logcatch
            Utils.ConsoleLog("info", $"Spieler {tplayer.Name} hat den Server verlassen!");
        }

        [ScriptEvent(ScriptEventType.PlayerEnterVehicle)]
        public static void OnPlayerEnterVehicle(TVehicle.TVehicle vehicle, TPlayer.TPlayer tplayer, byte seat)
        {
            int checkfrak = 0;
            bool v = vehicle.GetData<int>("VEHICLE_FRAKTION", out checkfrak);
            if (v && checkfrak > 0)
            {
                if (checkfrak != tplayer.fraktion)
                {
                    tplayer.SendChatMessage("{FF0000}Dieses Fahrzeug gehört nicht zu deiner Fraktion!");
                }
            }
        }

        [ScriptEvent(ScriptEventType.ColShape)]
        public static void OnPlayerEnterExitColshape(IColShape shape, IEntity entity, bool state)
        {
            String status = state ? "betreten" : "verlassen";

            switch(entity)
            {
                    case TPlayer.TPlayer tplayer:
                    {
                        if (shape == Server.testShape)
                        {
                            tplayer.SendChatMessage(status);
                            if(state)
                            {
                                tplayer.colshapeid = shape.Id;
                            }
                            else
                            {
                                tplayer.colshapeid = 9999;
                            }
                        }
                        break;
                    }
            }
        }

        [ClientEvent("Event.OAuth2Request")]
        public void OnOAuth2Request(TPlayer.TPlayer tplayer, string token) 
        {
            try
            {
                var request = WebRequest.Create("https://discordapp.com/api/users/@me");
                request.Method = "GET";
                request.Headers["Content-Type"] = "application/x-www-form-urlencoded";
                request.Headers["Authorization"] = $"Bearer {token}";
                var content = string.Empty;
                using(var response = request.GetResponse()) 
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            content = sr.ReadToEnd();
                            Utils.ConsoleLog("info", content);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Alt.LogError(e.ToString());
            }
        }

        [ClientEvent("Event.Register")]
        public void OnPlayerRegister(TPlayer.TPlayer tplayer, String name, String password)
        {
            if (!Datenbank.IstAccountBereitsVorhanden(name))
            {
                if (!tplayer.eingeloggt && name.Length > 3 && password.Length > 5)
                {
                    tplayer.name = name;
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
                    Utils.UpdateMoneyHud(tplayer, tplayer.geld);
                    tplayer.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Healthbar, 1.0);
                    tplayer.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Hungerbar, 0.5);
                    tplayer.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Thirstbar, 0.3);
                    tplayer.eingeloggt = true;
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
                if (!tplayer.eingeloggt && name.Length > 3 && password.Length > 5)
                {
                    if (Datenbank.PasswortCheck(name, password))
                    {
                        tplayer.name = name;
                        Datenbank.AccountLaden(tplayer);
                        if (tplayer.posx != 0.0 && tplayer.posy != 0.0 && tplayer.posz != 0.0)
                        {
                            tplayer.Spawn(new AltV.Net.Data.Position(tplayer.posx, tplayer.posy, tplayer.posz), 0);
                            tplayer.Rotation = new AltV.Net.Data.Rotation(0, 0, tplayer.posa);
                        }
                        else
                        {
                            if (tplayer.einreise == 0)
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
                        Utils.UpdateMoneyHud(tplayer, tplayer.geld);
                        tplayer.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Healthbar, 1.0);
                        tplayer.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Hungerbar, 0.5);
                        tplayer.Emit("updatePB", (int)TPlayer.TPlayer.ProgressBars.Thirstbar, 0.3);
                        if (!Utils.IsPlayerWhitelisted(tplayer))
                        {
                            tplayer.SendChatMessage("{#eb0e27}Du stehst nicht auf der Whitelist!");
                            tplayer.Kick("Du stehst nicht auf der Whitelist");
                            return;
                        }
                        tplayer.eingeloggt = true;
                        tplayer.SendChatMessage("{00c900}Erfolgreich eingeloggt!");
                        tplayer.SetWeather((uint)Events.weatherId);
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

        [ClientEvent("Event.SpawnVehicle")]
        public void OnSpawnVehicle(TPlayer.TPlayer tplayer, string VehicleName)
        {
            if (VehicleName == "Kein Fahrzeug") return;
            TVehicle.TVehicle veh = (TVehicle.TVehicle)Alt.CreateVehicle(Alt.Hash(VehicleName), new AltV.Net.Data.Position(tplayer.Position.X, tplayer.Position.Y + 4.5f, tplayer.Position.Z), tplayer.Rotation);
            if (veh != null)
            {
                tplayer.SendChatMessage("{04B404} Das Fahrzeug wurde erfolgreich gespawned!");
                Utils.adminLog($"Der Spieler {tplayer.name} hat ein {VehicleName} gespawned!", "TutorialServer");
                Utils.sendNotification(tplayer, "info", "Fahrzeug wurde erfolgreich gespawned!");
            }
            else
            {
                tplayer.SendChatMessage("{FF0000} Das Fahrzeug konnte nicht gespawned werden!");
                Utils.sendNotification(tplayer, "error", "Das Fahrzeug konnte nicht erstellt werden!");
            }
        }

        /*[ClientEvent("Event.successLockpickingServer")]
        public void OnSuccessLockpickingServer(TPlayer.TPlayer tplayer)
        {
            TVehicle.TVehicle veh = Utils.GetClosestVehicle(tplayer);
            if (veh != null)
            {
                veh.LockState = (VehicleLockState)1;
                Utils.sendNotification(tplayer, "info", "Du hast das Fahrzeug erfolgreich geknackt!");
            }
        }*/

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
                if (tplayer.fraktion == 1)
                {
                    Alt.Emit("showMDC");
                }
            }
        }

        public static void OnWeatherChange(object state)
        {
            try
            {
                var request = WebRequest.Create("https://nemesus.de/wetterapi/data.php");
                request.Method = "GET";
                var content = string.Empty;
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            content = sr.ReadToEnd();
                            weatherDataTemp = content.Split(",")[0];
                            weatherData = content.Split(",")[1];
                            switch (weatherData.ToLower())
                            {
                                default:
                                    {
                                        weatherId = (int)GtaWeather.Clear;
                                        break;
                                    }
                                case "clear sky":
                                    {
                                        weatherId = (int)GtaWeather.Clear;
                                        break;
                                    }
                                case "few clouds":
                                    {
                                        weatherId = (int)GtaWeather.Clouds;
                                        break;
                                    }
                                case "scattered clouds":
                                    {
                                        weatherId = (int)GtaWeather.Overcast;
                                        break;
                                    }
                                case "broken clouds":
                                    {
                                        weatherId = (int)GtaWeather.Overcast;
                                        break;
                                    }
                                case "shower rain":
                                    {
                                        weatherId = (int)GtaWeather.Rain;
                                        break;
                                    }
                                case "light rain":
                                    {
                                        weatherId = (int)GtaWeather.LightRain;
                                        break;
                                    }
                                case "rain":
                                    {
                                        weatherId = (int)GtaWeather.Rain;
                                        break;
                                    }
                                case "thunderstorm":
                                    {
                                        weatherId = (int)GtaWeather.Thunder;
                                        break;
                                    }
                                case "snow":
                                    {
                                        weatherId = (int)GtaWeather.LightSnow;
                                        break;
                                    }
                                case "mist":
                                    {
                                        weatherId = (int)GtaWeather.Foggy;
                                        break;
                                    }
                            }
                            foreach(TPlayer.TPlayer tplayer in Alt.GetAllPlayers())
                            {
                                if(tplayer.eingeloggt)
                                {
                                    tplayer.SetWeather((uint)weatherId);
                                }
                            }
                            Utils.ConsoleLog("info", $"Wetter API aufgerufen: {weatherDataTemp} - {weatherData}");
                        }
                    }
                }
            }
            catch (Exception) { }
        }
    }
}
