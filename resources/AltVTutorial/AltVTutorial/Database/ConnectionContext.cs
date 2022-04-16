using AltVTutorial.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace AltVTutorial.Database
{
    internal class ConnectionContext : DbContext
    {
        public DbSet<PasswordReset> PasswordReset { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Vehicle> Vehicle { get; set; }
        public DbSet<Whitelist> Whitelist { get; set; }


        private string GetConnectionString()
        {
            Config SQLConfig = new Config();
            return $"server={SQLConfig.Host};" +
                $"port={SQLConfig.Port};" +
                $"database={SQLConfig.Database};" +
                $"user={SQLConfig.Username};" +
                $"password={SQLConfig.Password}";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ServerVersion sv = ServerVersion.AutoDetect(GetConnectionString());
            optionsBuilder.UseMySql(GetConnectionString(), sv);
        }
    }
}
