namespace AltVTutorial.Database.Models
{
    public class Vehicle : Base
    {
        public string Name { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double PosZ { get; set; }
        public double PosR { get; set; }
        public bool Locked { get; set; }
        public float Fuel { get; set; }
        public bool Engine { get; set; }
        public User Owner { get; set; }
    }
}
