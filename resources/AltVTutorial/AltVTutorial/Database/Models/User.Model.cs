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
        public int Money { get; set; } = 5000;
        public int AdminLevel { get; set; } = 0;
        public int Fraktion { get; set; } = 0;
        public int Rank { get; set; } = 0;
        public float PosX { get; set; } = -425f;
        public float PosY { get; set; } = 1123f;
        public float PosZ { get; set; } = 325f;
        public float PosR { get; set; } = 0f;
        public int Payday { get; set; } = 60;

    }
}
