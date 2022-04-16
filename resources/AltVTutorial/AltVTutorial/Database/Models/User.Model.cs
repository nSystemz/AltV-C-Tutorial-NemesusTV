using System;

namespace AltVTutorial.Database.Models
{
    public class User : Base
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string EMail { get; set; }
        public DateTime VerifiedAt { get; set; }
        public string Token { get; set; }
        public int Money { get; set; }
        public int AdminLevel { get; set; }
        public int Fraktion { get; set; }
        public int Rank { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }
        public float PosR { get; set; }
        public int Payday { get; set; }

    }
}
