using Microsoft.EntityFrameworkCore;

namespace databaze;

public class GameContext:DbContext
{

    private const string connectionString = "server=localhost;port=3306;database=osadnici;user=osadnici;password=osadnici;";
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Hra>().HasMany(d => d.hraci).WithMany();
        }

        public DbSet<StavHrace> hraci { get; set; }
        public DbSet<Hra> hry { get; set; }
        public DbSet<Mapka> mapky { get; set; }
}