using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebOsadnici.Data
{
    public class ApplicationDbContext : IdentityDbContext<Hrac, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Hra>().HasMany(d => d.hraci).WithMany();
        }

        public DbSet<Hrac> hraci { get; set; }
        public DbSet<Hra> hry { get; set; }
        public DbSet<Mapka> mapky { get; set; }
    }
}