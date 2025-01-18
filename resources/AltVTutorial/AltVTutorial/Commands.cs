using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using AltV.Net.Resources.Chat.Api;
using AltVTutorial.TPlayer;
using AltVTutorial.TVehicle;
using Google.Protobuf.WellKnownTypes;
using MySqlConnector;
using Org.BouncyCastle.Asn1.X509;
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

        [Command("aduty")]
        public void CMD_aduty(TPlayer.TPlayer tplayer, string password) 
        {
            if(!tplayer.IsSpielerAdmin((int)TPlayer.TPlayer.AdminRanks.Moderator))
            {
                tplayer.SendChatMessage("{FF0000}Dein Adminlevel ist zu niedrig!");
                return;
            }
            if(password == "test" || tplayer.aduty == true)
            {
                if(tplayer.aduty == true)
                {
                    tplayer.aduty = false;
                    tplayer.SendChatMessage("{04B404} Aduty-Modus beendet!");
                }
                else
                {
                    tplayer.aduty = true;
                    tplayer.SendChatMessage("{04B404} Aduty-Modus akttiviert!");
                }
            }
            else
            {
                tplayer.SendChatMessage("{FF0000}Falsches Adminpasswort!");
            }
        }

        [Command("followme")]
        public void CMD_followme(TPlayer.TPlayer tplayer, string password)
        {
            tplayer.Emit("followMe");
        }

        [Command("car")]
        public void CMD_car(TPlayer.TPlayer tplayer, string VehicleName, string PlayerName = null, int R = 0, int G = 0, int B = 0)
        {
            if (!tplayer.IsSpielerAdmin((int)TPlayer.TPlayer.AdminRanks.Supporter))
            {
                tplayer.SendChatMessage("{FF0000}Dein Adminlevel ist zu niedrig!");
                return;
            }
            TVehicle.TVehicle veh = (TVehicle.TVehicle)Alt.CreateVehicle(Alt.Hash(VehicleName), new AltV.Net.Data.Position(tplayer.Position.X, tplayer.Position.Y + 2.5f, tplayer.Position.Z), tplayer.Rotation);
            if (veh != null)
            {
                veh.LockState = (VehicleLockState)1;
                veh.PrimaryColorRgb = new AltV.Net.Data.Rgba((byte)R, (byte)G, (byte)B, 255);
                if (PlayerName == null)
                {
                    tplayer.SendChatMessage("{04B404} Das Fahrzeug wurde erfolgreich gespawned!");
                    Utils.adminLog($"Der Spieler {tplayer.name} hat ein {VehicleName} gespawned!", "TutorialServer");
                    Utils.sendNotification(tplayer, "info", "Fahrzeug wurde erfolgreich gespawned!");
                }
                else
                {
                    TPlayer.TPlayer target = Utils.GetPlayerByName(PlayerName);
                    if (target == null)
                    {
                        Utils.sendNotification(tplayer, "error", "Ungültiger Spieler!");
                        return;
                    }
                    tplayer.SendChatMessage("{04B404} Du hast erfolgreich ein Fahrzeug gespawnt und dieses einem Spieler zugewiesen!");
                    Utils.adminLog($"Der Spieler {tplayer.name} hat ein {VehicleName} für {target.Name} erstellt!", "TutorialServer");
                    Utils.sendNotification(tplayer, "info", "Fahrzeug wurde erfolgreich für einen Spieler erstellt!");
                    veh.SpielerID = target.id;
                    veh.VehicleLock = (int)veh.LockState;
                    veh.vehicleName = VehicleName;
                    veh.NumberplateText = VehicleName;
                    Datenbank.FahrzeugErstellen(veh);
                }
                tplayer.Emit("freezePlayer", false);
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
            if (!tplayer.IsSpielerAdmin((int)TPlayer.TPlayer.AdminRanks.Moderator))
            {
                tplayer.SendChatMessage("{FF0000}Dein Adminlevel ist zu niedrig!");
                return;
            }
            tplayer.Emit("freezePlayer", freeze);
            tplayer.SendChatMessage("{04B404}Der freeze Befehl wurde ausgeführt!");
        }

        [Command("einreise")]
        public void CMD_einreise(TPlayer.TPlayer tplayer, string playerName)
        {
            if (!tplayer.IsSpielerAdmin((int)TPlayer.TPlayer.AdminRanks.Moderator))
            {
                tplayer.SendChatMessage("{FF0000}Dein Adminlevel ist zu niedrig!");
                return;
            }
            TPlayer.TPlayer target = Utils.GetPlayerByName(playerName);
            if (target == null)
            {
                Utils.sendNotification(tplayer, "error", "Ungültiger Spieler!");
                return;
            }
            if (target.einreise == 0)
            {
                target.einreise = 1;
                tplayer.Spawn(new AltV.Net.Data.Position(-425, 1123, 325), 0);
                Utils.sendNotification(target, "success", "Einreise erfolgreich!");
                Utils.sendNotification(tplayer, "success", "Einreise erfolgreich!");
                Datenbank.AccountSpeichern(tplayer);
            }
            else
            {
                Utils.sendNotification(tplayer, "error", "Dieser Spieler muss nicht mehr einreisen!");
            }
        }

        [Command("pet")]
        public void CMD_pet(TPlayer.TPlayer tplayer)
        {
            if (!tplayer.IsSpielerAdmin((int)TPlayer.TPlayer.AdminRanks.Moderator))
            {
                tplayer.SendChatMessage("{FF0000}Dein Adminlevel ist zu niedrig!");
                return;
            }
            if (tplayer.pet == null)
            {
                tplayer.pet = (Ped)Alt.CreatePed(PedModel.Husky, new AltV.Net.Data.Position((float)(tplayer.Position.X + 2.5), tplayer.Position.Y, tplayer.Position.Z), tplayer.Rotation);
                Utils.sendNotification(tplayer, "success", "Das Tier wurde erstellt!");
                Alt.Emit("PetFollowPlayer", tplayer, tplayer.pet);
            }
            else
            {
                tplayer.pet.Destroy();
                tplayer.pet = null;
                Utils.sendNotification(tplayer, "success", "Das Tier wurde gelöscht!");
            }
        }



        [Command("testeupoutfit", greedyArg: true)]
        public void CMD_testeupoutfit(TPlayer.TPlayer tplayer, String outfitname)
        {

            try
            {
                String json1 = "";
                String json2 = "";
                if (outfitname.Length < 5 || outfitname.Length > 35)
                {
                    tplayer.SendChatMessage("Ungültiger Outfitname!");
                    return;
                }

                MySqlCommand command = Datenbank.Connection.CreateCommand();
                command.CommandText = "SELECT json1,json2 FROM eupoutfits WHERE owner='EUP' AND name=@name LIMIT 1";
                command.Parameters.AddWithValue("name", outfitname);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    if (reader.HasRows)
                    {
                        json1 = reader.GetString("json1");
                        json2 = reader.GetString("json2");
                    }
                    reader.Close();
                }

                string[] json1Array = new string[14];
                string[] json2Array = new string[14];

                json1 = json1.Substring(1, json1.Length - 2);
                json2 = json2.Substring(1, json2.Length - 2);

                json1Array = json1.Split(",");
                json2Array = json2.Split(",");

                tplayer.ClearProps(0);
                tplayer.ClearProps(1);
                tplayer.ClearProps(2);
                tplayer.ClearProps(6);
                tplayer.ClearProps(7);
                tplayer.SetClothes(10, 0, 0, 0);
                tplayer.SetClothes(5, 0, 0, 0);
                tplayer.SetClothes(7, 0, 0, 0);
                tplayer.SetClothes(1, 0, 0, 0);
                tplayer.SetClothes(9, 0, 0, 0);

                //Top
                tplayer.SetClothes(11, (ushort)(Convert.ToInt32(json1Array[5]) - 1), (byte)(Convert.ToInt32(json2Array[5]) - 1), 0);
                //Torso
                tplayer.SetClothes(3, (ushort)(Convert.ToInt32(json1Array[6]) - 1), (byte)(Convert.ToInt32(json2Array[6]) - 1), 0);
                //Legs
                tplayer.SetClothes(4, (ushort)(Convert.ToInt32(json1Array[9]) - 1), (byte)(Convert.ToInt32(json2Array[9]) - 1), 0);
                //Shoes
                tplayer.SetClothes(6, (ushort)(Convert.ToInt32(json1Array[10]) - 1), (byte)(Convert.ToInt32(json2Array[10]) - 1), 0);
                //Undershirt
                tplayer.SetClothes(8, (ushort)(Convert.ToInt32(json1Array[8]) - 1), (byte)(Convert.ToInt32(json2Array[8]) - 1), 0);
                //Bag
                tplayer.SetClothes(5, (ushort)(Convert.ToInt32(json1Array[13]) - 1), (byte)(Convert.ToInt32(json2Array[13]) - 1), 0);
                //Glasses
                tplayer.SetProps(1, (ushort)(Convert.ToInt32(json1Array[1]) - 1), (byte)(Convert.ToInt32(json2Array[1]) - 1));
                //Hat
                tplayer.SetProps(0, (ushort)(Convert.ToInt32(json1Array[0]) - 1), (byte)(Convert.ToInt32(json2Array[0]) - 1));
                //Mask
                tplayer.SetClothes(1, (ushort)(Convert.ToInt32(json1Array[4]) - 1), (byte)(Convert.ToInt32(json2Array[4]) - 1), 0);
                //Ears
                tplayer.SetProps(2, (ushort)(Convert.ToInt32(json1Array[2]) - 1), (byte)(Convert.ToInt32(json2Array[2]) - 1));
                //Watches
                tplayer.SetProps(6, (ushort)(Convert.ToInt32(json1Array[3]) - 1), (byte)(Convert.ToInt32(json2Array[3]) - 1));
                //Bracelets
                tplayer.SetProps(7, (ushort)(Convert.ToInt32(json1Array[7]) - 1), (byte)(Convert.ToInt32(json2Array[7]) - 1));
                //Accessories
                tplayer.SetClothes(7, (ushort)(Convert.ToUInt16(json1Array[11]) - 1), (byte)(Convert.ToInt32(json2Array[11]) - 1), 0);
                //Armor
                tplayer.SetClothes(9, (ushort)(Convert.ToInt32(json1Array[12]) - 1), (byte)(Convert.ToInt32(json2Array[12]) - 1), 0);
                tplayer.SendChatMessage("Testoutfit (EUP) gesetzt!");
            }
            catch (Exception e)
            {
                Alt.Log($"[CMD_testeupoutfit]: " + e.ToString());
            }
            return;
        }

        [Command("parken")]
        public void CMD_parken(TPlayer.TPlayer tplayer)
        {
            if (tplayer.Vehicle == null)
            {
                tplayer.SendChatMessage("{FF0000}Du sitzt in keinem Fahrzeug!");
                return;
            }
            TVehicle.TVehicle veh = (TVehicle.TVehicle)tplayer.Vehicle;
            if (veh != null && veh.SpielerID == tplayer.id)
            {
                Datenbank.SpeichereFahrzeuge(veh);
                tplayer.SendChatMessage("{04B404}Du hast das Fahrzeug erfolgreich hier geparkt!");
            }
        }

        [Command("buyvehicle")]
        public void CMD_buyvehicle(TPlayer.TPlayer tplayer)
        {
            foreach (Models.Cardealer cardealer in Cardealer.CardealerController.cardealerList)
            {
                if (tplayer.Vehicle == cardealer.vehicle)
                {
                    if (tplayer.geld >= cardealer.price)
                    {
                        tplayer.geld -= cardealer.price;
                        TVehicle.TVehicle veh = (TVehicle.TVehicle)Alt.CreateVehicle(Alt.Hash(cardealer.modelname), new AltV.Net.Data.Position(tplayer.Position.X + 2.5f, tplayer.Position.Y + 2.5f, tplayer.Position.Z), tplayer.Rotation);
                        if (veh != null)
                        {
                            veh.LockState = (VehicleLockState)1;
                            veh.SpielerID = tplayer.id;
                            veh.VehicleLock = (int)veh.LockState;
                            veh.vehicleName = cardealer.modelname;
                            veh.NumberplateText = tplayer.name;
                            Datenbank.FahrzeugErstellen(veh);
                            tplayer.SendChatMessage("{04B404}Du hast dir das Fahrzeug erfolgreich erworben!");
                        }
                    }
                    else
                    {
                        tplayer.SendChatMessage("{FF0000}Du hast nicht genügend Geld dabei!");
                    }
                    break;
                }
            }
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

        [Command("whitelist")]
        public void CMD_whitelist(TPlayer.TPlayer tplayer, ulong socialclubid)
        {
            bool found = false;
            if (!tplayer.IsSpielerAdmin((int)TPlayer.TPlayer.AdminRanks.Administrator))
            {
                tplayer.SendChatMessage("{FF0000}Dein Adminlevel ist zu niedrig!");
                return;
            }
            if (socialclubid < 10000)
            {
                tplayer.SendChatMessage("{FF0000}Ungültige Socialclubid!");
                return;
            }
            MySqlCommand command = Datenbank.Connection.CreateCommand();
            command.CommandText = "SELECT id from whitelist WHERE socialclubid=@socialclubid LIMIT 1";
            command.Parameters.AddWithValue("socialclubid", socialclubid);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    found = true;
                }
            }
            if (found == true)
            {
                MySqlCommand command2 = Datenbank.Connection.CreateCommand();
                command2.CommandText = "DELETE FROM whitelist WHERE socialclubid=@socialclubid LIMIT 1";
                command2.Parameters.AddWithValue("socialclubid", socialclubid);
                command2.ExecuteNonQuery();
                tplayer.SendChatMessage("{04B404}Whitelisteintrag entfernt!");
                Utils.sendNotification(tplayer, "info", "Whitelisteintrag entfernt!");
            }
            else
            {
                MySqlCommand command3 = Datenbank.Connection.CreateCommand();
                command3.CommandText = "INSERT INTO whitelist (socialclubid) VALUES (@socialclubid)";
                command3.Parameters.AddWithValue("@socialclubid", socialclubid);
                command3.ExecuteNonQuery();
                tplayer.SendChatMessage("{04B404}Whitelisteintrag hinzugefügt!");
                Utils.sendNotification(tplayer, "info", "Whitelisteintrag hinzugefügt!");
            }
        }

        [Command("colshapetest")]
        public void CMD_colshapetest(TPlayer.TPlayer tplayer)
        {
            if (tplayer.colshapeid == Server.testShape.Id)
            {
                tplayer.SendChatMessage("Du bist im richtigen Colshape!");
            }
            else
            {
                tplayer.SendChatMessage("Du bist im falschen Colshape!");
            }
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
            if (!tplayer.IsSpielerAdmin((int)TPlayer.TPlayer.AdminRanks.Moderator))
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

            using (StreamWriter file = new StreamWriter(@"./savedpositions.txt", true))
            {
                file.WriteLine(message);
            }
        }

        [Command("creategarage")]
        public void CMD_creategarage(TPlayer.TPlayer tplayer, int maxcount, string name)
        {
            if (!tplayer.IsSpielerAdmin((int)TPlayer.TPlayer.AdminRanks.Supporter))
            {
                tplayer.SendChatMessage("{FF0000}Dein Adminlevel ist zu niedrig!");
                return;
            }
            Garagen.Garagen garagen = new Garagen.Garagen();
            garagen.name = name;
            garagen.maxcount = maxcount;
            garagen.count = 0;
            garagen.pos_x = tplayer.Position.X;
            garagen.pos_y = tplayer.Position.Y;
            garagen.pos_z = tplayer.Position.Z;
            garagen.pos_a = tplayer.Rotation.Yaw;

            Garagen.Garagen.GarageErstellen(garagen);

            Utils.sendNotification(tplayer, "success", "Garage wurde erstellt!");
        }

        [Command("deletegarage")]
        public void CMD_deletegarage(TPlayer.TPlayer tplayer, int id)
        {
            if (!tplayer.IsSpielerAdmin((int)TPlayer.TPlayer.AdminRanks.Supporter))
            {
                tplayer.SendChatMessage("{FF0000}Dein Adminlevel ist zu niedrig!");
                return;
            }
            Garagen.Garagen garageDelete = null;
            foreach (Garagen.Garagen garage in Garagen.Garagen.garageList)
            {
                if (garage.id == id)
                {
                    garageDelete = garage;
                    break;
                }
            }
            if (garageDelete != null)
            {
                Garagen.Garagen.GarageDelete(garageDelete);
                Utils.sendNotification(tplayer, "success", "Garage wurde gelöscht!");
            }
            else
            {
                tplayer.SendChatMessage("{FF0000}Ungültige Garage!");
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
            if (target == null)
            {
                tplayer.SendChatMessage("{FF0000}Ungültiger Spieler!");
                return;
            }
            if (frak < 0 || frak > TPlayer.TPlayer.Fraktionen.Length)
            {
                tplayer.SendChatMessage("{FF0000}Ungültige Fraktion!");
                return;
            }
            target.fraktion = frak;
            target.rang = 6;
            tplayer.SendChatMessage($"Du hast {target.Name} zum Chef der Fraktion {TPlayer.TPlayer.Fraktionen[frak]}!");
            target.SendChatMessage($"Du wurdest von {tplayer.Name} zum Chef der Fraktion {TPlayer.TPlayer.Fraktionen[frak]} gemacht!");
        }

        [Command("invite")]
        public void CMD_invite(TPlayer.TPlayer tplayer, string playertarget)
        {
            if (tplayer.fraktion == 0 || tplayer.rang < 6)
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
            target.fraktion = tplayer.fraktion;
            target.rang = 1;
            tplayer.SendChatMessage($"Du hast {target.Name} zur Fraktion {TPlayer.TPlayer.Fraktionen[tplayer.fraktion]} eingeladen!");
            target.SendChatMessage($"Du wurdest von {tplayer.Name} zur Fraktion {TPlayer.TPlayer.Fraktionen[tplayer.fraktion]} eingeladen!");
        }

        [Command("moneytest")]
        public void CMD_moneytest(TPlayer.TPlayer tplayer)
        {
            Utils.UpdateMoneyHud(tplayer, 500);
        }

        [Command("pistole")]
        public void CMD_pistole(TPlayer.TPlayer tplayer)
        {
            if (tplayer.fraktion != 1)
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
            if (veh != null)
            {
                tplayer.Emit("showLockpicking");
            }
            else
            {
                Utils.sendNotification(tplayer, "error", "Du bist nicht in der Nähe von einem Fahrzeug!");
            }
        }

        [Command("funmodus")]
        public void CMD_funmodus(TPlayer.TPlayer tplayer)
        {
            tplayer.Emit("setFunmodus");
            Utils.sendNotification(tplayer, "success", "Funmodus aktiviert/deaktiviert!");
        }

        [Command("fpsboost")]
        public void CMD_fpsboost(TPlayer.TPlayer tplayer)
        {
            tplayer.Emit("fpsBoost");
        }

        [Command("charcreator")]
        public void CMD_charcreator(TPlayer.TPlayer tplayer)
        {
            tplayer.Spawn(new AltV.Net.Data.Position((float)-347.93405, (float)1277.4725, (float)333.80664), 0);
            tplayer.Rotation = new AltV.Net.Data.Rotation((float)0, (float)0, (float)1.6821126);
            tplayer.SetClothes(11, 15, 0, 0);
            tplayer.SetClothes(3, 15, 0, 0);
            tplayer.SetClothes(8, 15, 0, 0);
            tplayer.SetClothes(6, 1, 0, 0);
            tplayer.Emit("showCharCreator");
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
                if (tvehicle.Fuel > 0)
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

        [Command("tuning")]
        public static void CMD_tuning(TPlayer.TPlayer tplayer, string kategorie, byte komponente)
        {
            TVehicle.TVehicle tvehicle = (TVehicle.TVehicle)tplayer.Vehicle;
            if (tvehicle == null)
            {
                Utils.sendNotification(tplayer, "error", "Du sitzt in keinem Fahrzeug!");
                return;
            }
            tvehicle.ModKit = 2;
            foreach (VehicleModType mod in System.Enum.GetValues(typeof(VehicleModType)))
            {
                if (System.Enum.GetName(typeof(VehicleModType), mod).Equals(kategorie))
                {
                    if (komponente < 0 || komponente > tvehicle.GetModsCount((byte)mod))
                    {
                        Utils.sendNotification(tplayer, "error", "Ungültiges Tuning!");
                        return;
                    }
                    tvehicle.SetMod((byte)mod, komponente);
                    Utils.sendNotification(tplayer, "success", "Das Tuning wurde erfolgreich gesetzt!");
                    return;
                }
            }
        }

        [Command("mods")]
        public static void CMD_mods(TPlayer.TPlayer tplayer)
        {
            String tuning = "";
            TVehicle.TVehicle tvehicle = (TVehicle.TVehicle)tplayer.Vehicle;
            if (tvehicle == null)
            {
                Utils.sendNotification(tplayer, "error", "Du sitzt in keinem Fahrzeug!");
                return;
            }
            foreach (VehicleModType mod in System.Enum.GetValues(typeof(VehicleModType)))
            {
                tuning += $"{System.Enum.GetName(typeof(VehicleModType), mod)},";
            }
            tplayer.SendChatMessage($"Verfügbares Tuning: {tuning}");
        }
    }
}
