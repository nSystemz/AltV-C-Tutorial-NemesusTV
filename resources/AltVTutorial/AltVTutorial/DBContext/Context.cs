using AltVTutorial.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltVTutorial.DBContext
{
    public class Context : DbContext
    {

        public DbSet<Haus> Häuser { get; set; }

        public DbSet<Garagen.Garagen> Garagen { set; get; }

        public DbSet<TPlayer.TPlayer> Accounts { set; get; }

        static readonly string connectionString = Datenbank.GetConnectionString();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
    }
}
