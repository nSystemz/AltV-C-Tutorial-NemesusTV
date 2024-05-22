using AltV.Net;
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
        public float pos_x { get; set; }
        public float pos_y { get; set; }
        public float pos_z { get; set; }
        public float pos_a { get; set; }

        public Garagen()
        {
            id = 0;
            name = "n/A";
            maxcount = 0;
            count = 0;
            pos_x = 0.0f;
            pos_y = 0.0f;
            pos_z = 0.0f;
            pos_a = 0.0f;
        }

        public static void GarageErstellen(Garagen garage)
        {
            if (garage == null) return;

            using (var context = new DBContext.Context())
            {
                context.Garagen.Add(garage);
                context.SaveChanges();
            }
            garageList.Add(garage);
        }

        public static void GarageSpeichern(Garagen garage)
        {
            if (garage == null) return;

            using (var context = new DBContext.Context())
            {
                context.Garagen.Update(garage);
                context.SaveChanges();
            }
        }

        public static void GarageDelete(Garagen garage)
        {
            if (garage == null) return;
            using (var context = new DBContext.Context())
            {
                context.Garagen.Remove(garage);
                context.SaveChanges();
            }
        }

        public static void GarageLoad()
        {
            using(var context = new DBContext.Context())
            {
                var garagen = context.Garagen;
                foreach(var garage in garagen)
                {
                    garageList.Add(garage);
                }
            }
            Utils.ConsoleLog("success", $"Es wurden {garageList.Count} Garage/n geladen!");
        }
    }
}
