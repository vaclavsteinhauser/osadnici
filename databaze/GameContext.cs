using Microsoft.EntityFrameworkCore;

namespace databaze;

public class GameContext:DbContext
{

    private const string connectionString = "server=localhost;port=3306;database=osadnici;user=osadnici;password=osadnici;";
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(connectionString);
        }

        public DbSet<player> players { get; set; }
}