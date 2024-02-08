using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

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
            
            modelBuilder.Entity<Cesta>()
                .ToTable("Cesty")
                .HasKey(c => c.Id);
            modelBuilder.Entity<Cesta>()
                .Property(c => c.poziceX)
                .IsRequired();
            modelBuilder.Entity<Cesta>()
                .Property(c => c.poziceY)
                .IsRequired();
            modelBuilder.Entity<Cesta>()
                .Property(c => c.natoceni)
                .IsRequired();
            modelBuilder.Entity<Cesta>()
                .HasMany<Rozcesti>(c => c.konce);
            modelBuilder.Entity<Cesta>()
                .HasOne<Hrac>(c => c.hrac);
            
            modelBuilder.Entity<Hrac>()
                .ToTable("Hraci")
                .HasKey(h => h.Id);

            modelBuilder.Entity<Hra>()
                .ToTable("Hry")
                .HasKey(h => h.Id);
            modelBuilder.Entity<Hra>()
                .HasMany(d => d.hraci)
                .WithMany();
            modelBuilder.Entity<Hra>()
                .HasOne(h => h.mapka)
                .WithOne(m=> m.hra)
                .HasForeignKey<Mapka>(m=>m.hraId);
            modelBuilder.Entity<Hra>()
                .Property(h=>h.hracNaTahu)
                .IsRequired();
            modelBuilder.Entity<Hra>()
                .HasMany(d => d.stavy)
                .WithOne(d=> d.hra);

            modelBuilder.Entity<AkcniKarta>()
                .ToTable("AkcniKarty")
                .HasKey(h => h.Id);
            modelBuilder.Entity<AkcniKarta>()
                .Property(d => d.Nazev)
                .IsRequired();
            modelBuilder.Entity<AkcniKarta>()
                .Property(d=>d.pocet)
                .IsRequired();

            modelBuilder.Entity<SurovinaKarta>()
                .ToTable("SurovinaKarty")
                .HasKey(d => d.Id);
            modelBuilder.Entity<SurovinaKarta>()
                .HasOne<Surovina>(d=>d.surovina);
            modelBuilder.Entity<SurovinaKarta>()
                .Property(d=>d.pocet)
                .IsRequired();

            modelBuilder.Entity<Mapka>()
                .ToTable("Mapky")
                .HasKey(h => h.Id);
            modelBuilder.Entity<Mapka>()
                .HasMany(m => m.policka)
                .WithOne(p=>p.mapka);
            modelBuilder.Entity<Mapka>()
                .HasMany(m => m.cesty)
                .WithOne();
            modelBuilder.Entity<Mapka>()
                .HasMany(m => m.rozcesti)
                .WithOne();
            
            modelBuilder.Entity<Pole>()
                .ToTable("Policka")
                .HasKey(h => h.Id);
            modelBuilder.Entity<Pole>()
                .Property(p => p.poziceX)
                .IsRequired(); 
            modelBuilder.Entity<Pole>()
                .Property(p => p.poziceY)
                .IsRequired();
            modelBuilder.Entity<Pole>()
                .HasOne(p => p.surovina);
            modelBuilder.Entity<Pole>()
                .Property(p => p.cislo)
                .IsRequired();
            modelBuilder.Entity<Pole>()
                .Property(p => p.blokovane)
                .IsRequired();
            modelBuilder.Entity<Pole>()
                .HasMany(p => p.rozcesti);

            modelBuilder.Entity<Rozcesti>()
                .ToTable("Rozcesti")
                .HasKey(h => h.Id);
            modelBuilder.Entity<Rozcesti>()
                .Property(r => r.poziceX)
                .IsRequired();
            modelBuilder.Entity<Rozcesti>()
                .Property(r => r.poziceY)
                .IsRequired();
            modelBuilder.Entity<Rozcesti>()
                .HasOne(r => r.hrac);
            modelBuilder.Entity<Rozcesti>()
                .Property(r => r.blokovane)
                .IsRequired();
            modelBuilder.Entity<Rozcesti>()
                .HasOne(r => r.stavba);

            modelBuilder.Entity<Stavba>()
                .ToTable("Stavby")
                .HasKey(h => h.Id);
            modelBuilder.Entity<Stavba>()
                .Property(s=>s.Nazev)
                .IsRequired();
            modelBuilder.Entity<Stavba>()
                .Property(s => s.zisk);

            modelBuilder.Entity<StavHrace>()
                .ToTable("StavyHracu")
                .HasKey(h => h.Id);
            modelBuilder.Entity<StavHrace>()
                .HasOne(s => s.hra)
                .WithMany(h => h.stavy);
            modelBuilder.Entity<StavHrace>()
                .HasOne(s => s.hrac);
            modelBuilder.Entity<StavHrace>()
                .Property(s => s.barva)
                .HasConversion(
                color => color.Name,
                name => Color.FromName(name)
                );
            
            modelBuilder.Entity<StavHrace>()
                .HasMany<SurovinaKarta>(s => s.SurovinaKarty)
                .WithOne();
            modelBuilder.Entity<StavHrace>()
                .HasMany<AkcniKarta>(s => s.AkcniKarty)
                .WithOne();

            modelBuilder.Entity<Surovina>()
                .ToTable("Suroviny")
                .HasKey(h => h.Id);
            modelBuilder.Entity<Surovina>()
                .Property(s => s.Nazev)
                .IsRequired();
            modelBuilder.Entity<Surovina>()
                .Property (s => s.ImageUrl)
                .IsRequired();
            modelBuilder.Entity<Surovina>()
                .Property(s=> s.BackColor) 
                .IsRequired();
        }
        public DbSet<Cesta> cesty { get; set; }
        public DbSet<Hrac> hraci { get; set; }
        public DbSet<Hra> hry { get; set; }
        public DbSet<AkcniKarta> akcnikarty { get; set; }
        public DbSet<SurovinaKarta> surovinakarty { get; set; }
        public DbSet<Mapka> mapky { get; set; }
        public DbSet<Pole> policka { get; set; }
        public DbSet<Rozcesti> rozcesti { get; set; }
        public DbSet<Stavba> stavby { get; set; }
        public DbSet<StavHrace> stavy { get; set; }
        public DbSet<Surovina> suroviny { get; set; }
        
    }
}