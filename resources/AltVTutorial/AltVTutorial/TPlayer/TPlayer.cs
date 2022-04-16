using AltV.Net;
using AltV.Net.Elements.Entities;
using System;

namespace AltVTutorial.TPlayer
{
    public class TPlayer : Player
    {
        public static String[] Fraktionen = new String[3] { "Keine Fraktion", "Los Santos Police Department", "Newsfirma" };
        public static String[] RangNamen = new String[7] { "Kein Rang", "Praktikant", "Auszubildener", "Angestellter", "Abteilungsleiter", "Ausbilder", "Chef" };

        public enum ProgressBars { Healthbar = 1, Hungerbar, Thirstbar }

        public enum AdminRanks { Spieler, Moderator, Supporter, Administrator };

        public int DBID { get; set; }
        public string Username { get; set; }
        public long Money { get; set; }
        public int AdminLevel { get; set; }
        public int Fraktion { get; set; }
        public int Rank { get; set; }
        public int Payday { get; set; }

        public bool LoggedIn { get; set; }

        public float[] positions = new float[4];

        public TPlayer(IServer server, IntPtr nativePointer, ushort id) : base(server, nativePointer, id)
        {
            Money = 5000;
            AdminLevel = 0;
            LoggedIn = false;
            Fraktion = 0;
            Rank = 0;
            Payday = 60;
        }

        public bool IsSpielerAdmin(int alvl)
        {
            return AdminLevel >= alvl;
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
            return Rank;
        }

        public String HoleRangName()
        {
            return RangNamen[Rank];
        }
    }
}
