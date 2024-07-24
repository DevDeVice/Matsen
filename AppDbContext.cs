using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Matsen.Models;
using Microsoft.Extensions.Configuration;

namespace Matsen
{
    public class AppDbContext : DbContext
    {
        /*TODO - wrocic bo dla testu ustawiony json i usunac
         Microsoft.Extensions.Configuration
         Microsoft.Extensions.Configuration.Json
         usunac appsettings.json i kod z app.xaml.cs
        
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<WorkEntry> WorkEntries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            optionsBuilder.UseSqlServer(connectionString);
        }
        */
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Pobierz connection string z konfiguracji
            var connectionString = App.Configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
