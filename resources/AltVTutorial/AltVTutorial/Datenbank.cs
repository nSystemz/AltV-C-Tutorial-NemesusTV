﻿using AltV.Net;
using MySql.Data.MySqlClient;
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
            this.Username = "altv";
            this.Password = "12345";
            this.Database = "altv";
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
                Alt.Log("MYSQL Verbindung erfolgreich aufgebaut!");
            } catch(Exception e)
            {
                DatenbankVerbindung = false;
                Alt.Log("MYSQL Verbindung konnte nicht aufgebaut werden");
                Alt.Log(e.ToString());
                System.Threading.Thread.Sleep(5000);
                Environment.Exit(0);
            }
        }

        public static void AccountSpeichern(TPlayer.TPlayer tplayer)
        {
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "UPDATE users SET adminlevel=@adminlevel, geld=@geld, fraktion=@fraktion, rang=@rang, payday=@payday, posx=@posx, posy=@posy, posz=@posz, posa=@posa WHERE id=@id";

            command.Parameters.AddWithValue("@adminlevel", tplayer.AdminLevel);
            command.Parameters.AddWithValue("@geld", tplayer.Money);
            command.Parameters.AddWithValue("@fraktion", tplayer.Fraktion);
            command.Parameters.AddWithValue("@rang", tplayer.Rank);
            command.Parameters.AddWithValue("@payday", tplayer.Payday);
            command.Parameters.AddWithValue("@posx", tplayer.Position.X);
            command.Parameters.AddWithValue("@posy", tplayer.Position.Y);
            command.Parameters.AddWithValue("@posz", tplayer.Position.Z);
            command.Parameters.AddWithValue("@posa", tplayer.Rotation.Yaw);
            command.Parameters.AddWithValue("@id", tplayer.DBID);

            command.ExecuteNonQuery();
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
    }
}
