namespace AltVTutorial.Database.Models
{
    internal class Whitelist : Base
    {
        public int SocialclubId { get; set; }
        public User Owner { get; set; }
    }
}
