namespace AltVTutorial.Database.Models
{
    public class PasswordReset : Base
    {
        public string Token { get; set; }
        public User Owner { get; set; }
    }
}
