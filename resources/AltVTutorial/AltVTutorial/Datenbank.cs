using AltV.Net;
using AltVTutorial.Cardealer;
using MySqlConnector;
using System;

namespace AltVTutorial
{
    class Datenbank : Server
    {
        public static bool DatenbankVerbindung = false;
        public static MySqlConnection Connection;
        public string Host { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public String Database { get; set; }

        public Datenbank ()
        {
            this.Host = "localhost";
            this.Username = "altv2";
            this.Password = "altv2";
            this.Database = "altv2";
        }

        public static String GetConnectionString()
        {
            Datenbank sql = new Datenbank();
            String SQLConnection = $"SERVER={sql.Host}; DATABASE={sql.Database}; UID={sql.Username}; Password={sql.Password}";
            return SQLConnection;
        }

        public static void InitConnection()
        {
            String SQLConnection = GetConnectionString();
            Connection = new MySqlConnection(SQLConnection);
            try
            {
                Connection.Open();
                DatenbankVerbindung = true;
                Utils.ConsoleLog("warning", "MYSQL Verbindung erfolgreich aufgebaut!");
            } catch(Exception e)
            {
                DatenbankVerbindung = false;
                Utils.ConsoleLog("error", "MYSQL Verbindung konnte nicht aufgebaut werden");
                Utils.ConsoleLog("error", e.ToString());
                System.Threading.Thread.Sleep(5000);
                Environment.Exit(0);
            }
        }

        public static bool IstAccountBereitsVorhanden(string name)
        {
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT * FROM users WHERE name=@name LIMIT 1";
            command.Parameters.AddWithValue("@name", name);
            using(MySqlDataReader reader = command.ExecuteReader())
            {
                if(reader.HasRows)
                {
                    return true;
                }
            }
            return false;
        }

        public static int NeuenAccountErstellen(String name, string password)
        {
            string saltedPw = BCrypt.Net.BCrypt.HashPassword(password);

            try
            {
                MySqlCommand command = Connection.CreateCommand();
                command.CommandText = "INSERT INTO users (password, name, email) VALUES (@password, @name, @email)";

                command.Parameters.AddWithValue("@password", saltedPw);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@email", $"{name}@email.de");
                command.ExecuteNonQuery();

                return (int)command.LastInsertedId;
            }
            catch(Exception e)
            {
                Utils.ConsoleLog("error", "Fehler bei NeuenAccountErstellen: " + e.ToString());
                return -1;
            }
        }

        public static void AccountLaden(TPlayer.TPlayer tplayer)
        {
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT * FROM users WHERE name=@name LIMIT 1";

            command.Parameters.AddWithValue("@name", tplayer.name);

            using(MySqlDataReader reader = command.ExecuteReader())
            {
                if(reader.HasRows)
                {
                    reader.Read();
                    tplayer.id = reader.GetInt32("id");
                    tplayer.adminlevel = reader.GetInt16("adminlevel");
                    tplayer.geld = reader.GetInt32("geld");
                    tplayer.fraktion = reader.GetInt16("fraktion");
                    tplayer.rang = reader.GetInt16("rang");
                    tplayer.payday = reader.GetInt16("payday");
                    tplayer.einreise = reader.GetInt16("einreise");
                }
            }
        }

        public static void AccountSpeichern(TPlayer.TPlayer tplayer)
        {
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "UPDATE users SET adminlevel=@adminlevel, geld=@geld, fraktion=@fraktion, rang=@rang, payday=@payday, posx=@posx, posy=@posy, posz=@posz, posa=@posa, einreise=@einreise WHERE id=@id";

            command.Parameters.AddWithValue("@adminlevel", tplayer.adminlevel);
            command.Parameters.AddWithValue("@geld", tplayer.geld);
            command.Parameters.AddWithValue("@fraktion", tplayer.fraktion);
            command.Parameters.AddWithValue("@rang", tplayer.rang);
            command.Parameters.AddWithValue("@payday", tplayer.payday);
            command.Parameters.AddWithValue("@posx", tplayer.Position.X);
            command.Parameters.AddWithValue("@posy", tplayer.Position.Y);
            command.Parameters.AddWithValue("@posz", tplayer.Position.Z);
            command.Parameters.AddWithValue("@posa", tplayer.Rotation.Yaw);
            command.Parameters.AddWithValue("@einreise", tplayer.einreise);
            command.Parameters.AddWithValue("@id", tplayer.id);

            command.ExecuteNonQuery();
        }

        public static bool PasswortCheck(string name, string passwordinput)
        {
            string password = "";
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT password FROM users where name=@name LIMIT 1";
            command.Parameters.AddWithValue("@name", name);

            using(MySqlDataReader reader = command.ExecuteReader())
            {
                if(reader.HasRows)
                {
                    reader.Read();
                    password = reader.GetString("password");
                }
            }
            if (BCrypt.Net.BCrypt.Verify(passwordinput, password)) return true;
            return false;
        }

        public static void FahrzeugErstellen(TVehicle.TVehicle veh)
        {
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "INSERT INTO fahrzeuge (owner, vehicleName, posX, posY, posZ, posA, vehicleLock, fuel, engine) VALUES (@owner, @vehicleName, @posX, @posY, @posZ, @posA, @vehicleLock, @fuel, @engine)";

            command.Parameters.AddWithValue("@owner", veh.SpielerID);
            command.Parameters.AddWithValue("@vehicleName", veh.vehicleName);
            command.Parameters.AddWithValue("@posX", veh.Position.X);
            command.Parameters.AddWithValue("@posY", veh.Position.Y);
            command.Parameters.AddWithValue("@posZ", veh.Position.Z);
            command.Parameters.AddWithValue("@posA", veh.Rotation.Yaw);
            command.Parameters.AddWithValue("@vehicleLock", veh.VehicleLock);
            command.Parameters.AddWithValue("@fuel", veh.Fuel);
            command.Parameters.AddWithValue("@engine", veh.EngineOn);
            command.ExecuteNonQuery();

            veh.vehicleID = (int)command.LastInsertedId;
        }

        public static void SpeichereFahrzeuge(TVehicle.TVehicle tvehicle)
        {
            if (tvehicle == null || tvehicle.vehicleID <= 0) return;

            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "UPDATE fahrzeuge SET posX=@posX,posY=@posY,posZ=@posZ,posA=@posA,vehicleLock=@vehicleLock,fuel=@fuel,engine=@engine where id=@id";
            command.Parameters.AddWithValue("posX", tvehicle.Position.X);
            command.Parameters.AddWithValue("posY", tvehicle.Position.Y);
            command.Parameters.AddWithValue("posZ", tvehicle.Position.Z);
            command.Parameters.AddWithValue("posA", tvehicle.Rotation.Yaw);
            command.Parameters.AddWithValue("vehicleLock", tvehicle.VehicleLock);
            command.Parameters.AddWithValue("fuel", tvehicle.Fuel);
            command.Parameters.AddWithValue("engine", tvehicle.EngineOn);
            command.Parameters.AddWithValue("id", tvehicle.vehicleID);
            command.ExecuteNonQuery();
        }

        public static void FahrzeugeLaden()
        {
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT * from fahrzeuge";

            float[] position = new float[4];
            string vehicleName = "";

            using(MySqlDataReader reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    position[0] = reader.GetFloat("posX");
                    position[1] = reader.GetFloat("posY");
                    position[2] = reader.GetFloat("posZ");
                    position[3] = reader.GetFloat("posA");
                    vehicleName = reader.GetString("vehicleName");
                    TVehicle.TVehicle veh = (TVehicle.TVehicle)Alt.CreateVehicle(Alt.Hash(vehicleName), new AltV.Net.Data.Position(position[0], position[1], position[2]), new AltV.Net.Data.Rotation(0, 0, position[3]));
                    veh.vehicleID = reader.GetInt32("id");
                    veh.VehicleLock = reader.GetInt16("vehicleLock");
                    veh.LockState = (AltV.Net.Enums.VehicleLockState)veh.VehicleLock;
                    veh.SpielerID = reader.GetInt32("owner");
                    veh.vehicleName = reader.GetString("vehicleName");
                    veh.NumberplateText = veh.vehicleName;
                    veh.Fuel = reader.GetFloat("fuel");
                    veh.EngineOn = Convert.ToBoolean(reader.GetInt16("engine"));
                }
            }
        }

        public static void CardealerLoad()
        {
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT * FROM cardealer";
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    Models.Cardealer cardealer = new Models.Cardealer();
                    cardealer.id = reader.GetInt32("id");
                    cardealer.modelname = reader.GetString("modelname");
                    cardealer.posx = reader.GetFloat("posx");
                    cardealer.posy = reader.GetFloat("posy");
                    cardealer.posz = reader.GetFloat("posz");
                    cardealer.posa = reader.GetFloat("posa");
                    cardealer.price = reader.GetInt32("price");
                    cardealer.vehicle = (TVehicle.TVehicle)Alt.CreateVehicle(Alt.Hash(cardealer.modelname), new AltV.Net.Data.Position(cardealer.posx, cardealer.posy, cardealer.posz), new AltV.Net.Data.Rotation(0, 0, cardealer.posa));
                    Random rnd = new Random();
                    cardealer.vehicle.PrimaryColor = (byte)rnd.Next(256);
                    cardealer.vehicle.SecondaryColor = (byte)rnd.Next(256);
                    CardealerController.cardealerList.Add(cardealer);
                }
            }
        }
    }
}
