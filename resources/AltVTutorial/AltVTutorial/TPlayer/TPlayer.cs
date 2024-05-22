using AltV.Net;
using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltVTutorial.TPlayer
{
    public class TPlayer : Player
    {
        public static String[] Fraktionen = new String[3] { "Keine Fraktion", "Los Santos Police Department", "Newsfirma" };
        public static String[] RangNamen = new String[7] { "Kein Rang", "Praktikant", "Auszubildener", "Angestellter", "Abteilungsleiter", "Ausbilder", "Chef" };

        public enum ProgressBars { Healthbar = 1, Hungerbar, Thirstbar }

        public enum AdminRanks {Spieler,Moderator,Supporter,Administrator};

        public int id { get; set; }
        public String name { get; set; }
        public String password { get; set; }
        public long geld { get; set; }
        public int adminlevel { get; set; }
        public int fraktion { get; set; }
        public int rang { get; set; }
        public int payday { get; set; }
        [NotMapped]
        public bool eingeloggt { get; set; }
        public float posx { get; set; }
        public float posy { get; set; }
        public float posz { get; set; }
        public float posa { get; set; }
        public int einreise { get; set; }
        [NotMapped]
        public Ped pet { get; set; }
        [NotMapped]
        public uint colshapeid;

        public TPlayer(ICore core, IntPtr nativePointer, uint id) : base(core, nativePointer, id)
        {
            geld = 5000;
            adminlevel = (int)AdminRanks.Administrator;
            eingeloggt = false;
            posx = 0.0f;
            posy = 0.0f;
            posz = 0.0f;
            posa = 0.0f;
            fraktion = 0;
            rang = 0;
            payday = 60;
            einreise = 0;
            pet = null;
        }

        public bool IsSpielerAdmin(int alvl)
        {
            return adminlevel >= alvl;
        }

        public bool IstSpielerInFraktion(int frak)
        {
            return fraktion == frak;
        }

        public string HoleFraktionsName()
        {
            return Fraktionen[fraktion];
        }

        public int HoleRangLevel()
        {
            return rang;
        }

        public String HoleRangName()
        {
            return RangNamen[rang];
        }
    }
}
