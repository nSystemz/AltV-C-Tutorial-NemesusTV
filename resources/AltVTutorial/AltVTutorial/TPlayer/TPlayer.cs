using AltV.Net;
using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
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

        public int SpielerID { get; set; }
        public String SpielerName { get; set; }
        public long Geld { get; set; }
        public int Adminlevel { get; set; }
        public int Fraktion { get; set; }
        public int Rang { get; set; }
        public int Payday { get; set; }
        public bool Eingeloggt { get; set; }
        public float[] positions = new float[4];
        public int Einreise { get; set; }

        public TPlayer(ICore core, IntPtr nativePointer, uint id) : base(core, nativePointer, id)
        {
            Geld = 5000;
            Adminlevel = (int)AdminRanks.Administrator;
            Eingeloggt = false;
            Fraktion = 0;
            Rang = 0;
            Payday = 60;
            Einreise = 0;
        }

        public bool IsSpielerAdmin(int alvl)
        {
            return Adminlevel >= alvl;
        }

        public bool IstSpielerInFraktion(int frak)
        {
            return Fraktion == frak;
        }

        public string HoleFraktionsName()
        {
            return Fraktionen[Fraktion];
        }

        public int HoleRangLevel()
        {
            return Rang;
        }

        public String HoleRangName()
        {
            return RangNamen[Rang];
        }
    }
}
