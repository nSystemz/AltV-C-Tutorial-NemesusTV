﻿using AltV.Net;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AltVTutorial.Garagen
{
    public class Garagen
    {
        public static List<Garagen> garageList = new List<Garagen>();
        public int id { get; set; }
        public String name { get; set; }
        public int maxcount { get; set; }
        public int count { get; set; }
        public float posx { get; set; }
        public float posy { get; set; }
        public float posz { get; set; }
        public float posa { get; set; }

        public Garagen()
        {
            id = 0;
            name = "n/A";
            maxcount = 0;
            count = 0;
            posx = 0.0f;
            posy = 0.0f;
            posz = 0.0f;
            posa = 0.0f;
        }

        public static void GarageErstellen(Garagen garage)
        {
            MySqlCommand command = Datenbank.Connection.CreateCommand();
            command.CommandText = "INSERT INTO garagen (name, maxcount, count, pos_x, pos_y, pos_z, pos_a) VALUES (@name, @maxcount, @count, @pos_x, @pos_y, @pos_z, @pos_a)";
            command.Parameters.AddWithValue("name", garage.name);
            command.Parameters.AddWithValue("maxcount", garage.maxcount);
            command.Parameters.AddWithValue("count", garage.count);
            command.Parameters.AddWithValue("pos_x", garage.posx);
            command.Parameters.AddWithValue("pos_y", garage.posy);
            command.Parameters.AddWithValue("pos_z", garage.posz);
            command.Parameters.AddWithValue("pos_a", garage.posa);

            command.ExecuteNonQuery();

            garage.id = (int)command.LastInsertedId;

            garageList.Add(garage);
        }

        public static void GarageSpeichern(Garagen garage)
        {
            if(garage == null) return;

            MySqlCommand command = Datenbank.Connection.CreateCommand();
            command.CommandText = "UPDATE garagen SET count = @count WHERE id = @id";
            command.Parameters.AddWithValue("count", garage.count);
            command.Parameters.AddWithValue("id", garage.id);

            command.ExecuteNonQuery();
        }

        public static void GarageDelete(Garagen garage)
        {
            if (garage == null) return;
            MySqlCommand command = Datenbank.Connection.CreateCommand();
            command.CommandText = "DELETE FROM garagen WHERE id = @id LIMIT 1";
            command.Parameters.AddWithValue("@id", garage.id);

            command.ExecuteNonQuery();

            garageList.Remove(garage);
        }

        public static void GarageLoad()
        {
            MySqlCommand command = Datenbank.Connection.CreateCommand();
            command.CommandText = "SELECT * FROM garagen";
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Garagen garagen = new Garagen();
                    garagen.id = reader.GetInt32("id");
                    garagen.name = reader.GetString("name");
                    garagen.maxcount = reader.GetInt32("maxcount");
                    garagen.count = reader.GetInt32("count");
                    garagen.posx = reader.GetFloat("pos_x");
                    garagen.posy = reader.GetFloat("pos_y");
                    garagen.posz = reader.GetFloat("pos_z");
                    garagen.posa = reader.GetFloat("pos_a");

                    garageList.Add(garagen);
                }
            }
        }
    }
}
