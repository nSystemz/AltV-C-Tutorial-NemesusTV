using System;

namespace AltVTutorial.Database.Models
{
    public class User : Base
    {
        public string Username { get; set; }
        public string EMail { get; set; }
        public DateTime VerifiedAt { get; set; }
        public string Token { get; set; }
        public int Money { get; set; }
        public int AdminLevel { get; set; }
        public int Franktion { get; set; }
        public int Rank { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double PosZ { get; set; }
        public double PosR { get; set; }
        public int Payday { get; set; }

    }
}
