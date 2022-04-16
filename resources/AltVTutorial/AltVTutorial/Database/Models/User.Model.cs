using System;
using System.ComponentModel;

namespace AltVTutorial.Database.Models
{
    public class User : Base
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string EMail { get; set; }
        public DateTime VerifiedAt { get; set; }
        public string Token { get; set; }
        [DefaultValue(5000)]
        public int Money { get; set; }
        [DefaultValue(0)]
        public int AdminLevel { get; set; }
        [DefaultValue(0)]
        public int Fraktion { get; set; }
        [DefaultValue(0)]
        public int Rank { get; set; }
        [DefaultValue(-425f)]
        public float PosX { get; set; }
        [DefaultValue(1123f)]
        public float PosY { get; set; }
        [DefaultValue(325f)]
        public float PosZ { get; set; }
        [DefaultValue(0f)]
        public float PosR { get; set; }
        [DefaultValue(60f)]
        public int Payday { get; set; }

    }
}
