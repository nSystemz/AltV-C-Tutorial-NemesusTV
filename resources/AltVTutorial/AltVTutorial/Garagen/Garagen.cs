using AltV.Net;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltVTutorial.Garagen
{
    public class Garagen
    {
        public int id { get; set; }
        public String name { get; set; }
        public int maxcount { get; set; }
        public int count { get; set; }
        public float posx { get; set; }
        public float posy { get; set; }
        public float posz { get; set; }

        public Garagen()
        {
            id = 0;
            name = "n/A";
            maxcount = 0;
            count = 0;
            posx = 0.0f;
            posy = 0.0f;
            posz = 0.0f;
        }

        public static void GarageErstellen(Garagen garage)
        {
            MySqlCommand command = Datenbank.Connection.CreateCommand();
            command.CommandText = "INSERT INTO garagen (name, maxcount, count, posx, posy, posz) VALUES (@name, @maxcount, @count, @posx, @posy, @posz)";
            command.Parameters.AddWithValue("name", garage.name);
            command.Parameters.AddWithValue("maxcount", garage.maxcount);
            command.Parameters.AddWithValue("count", garage.count);
            command.Parameters.AddWithValue("posx", garage.posx);
            command.Parameters.AddWithValue("posy", garage.posy);
            command.Parameters.AddWithValue("posz", garage.posz);

            command.ExecuteNonQuery();

            garage.id = (int)command.LastInsertedId;
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
    }
}
