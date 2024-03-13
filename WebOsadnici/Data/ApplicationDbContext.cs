using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WebOsadnici.Models.HerniTridy;

namespace WebOsadnici.Data
{
    public class ApplicationDbContext : IdentityDbContext<Hrac, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Konfigurace připojení k databázi
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(); // Povolení logování citlivých dat
            optionsBuilder.UseLazyLoadingProxies(); // Povolení lenivého načítání vazebných objektů
            optionsBuilder.UseChangeTrackingProxies(); // Povolení sledování změn v objektech
        }

        // Konfigurace modelu databáze
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfigurace entit
            // Aktivita
            modelBuilder.Entity<Aktivita>().ToTable("Aktivity");
            modelBuilder.Entity<Aktivita>().HasKey(a => a.Id);
            modelBuilder.Entity<Aktivita>().Property(a => a.Akce).IsRequired();
            modelBuilder.Entity<Aktivita>().Property(a => a.CisloAktivity).IsRequired();
            modelBuilder.Entity<Aktivita>().HasOne<Hrac>(a => a.Hrac).WithMany();

            // Cesta
            modelBuilder.Entity<Cesta>().ToTable("Cesty");
            modelBuilder.Entity<Cesta>().HasKey(c => c.Id);
            modelBuilder.Entity<Cesta>().Property(c => c.poziceX).IsRequired();
            modelBuilder.Entity<Cesta>().Property(c => c.poziceY).IsRequired();
            modelBuilder.Entity<Cesta>().Property(c => c.natoceni).IsRequired();
            modelBuilder.Entity<Cesta>().HasMany<Rozcesti>(c => c.rozcesti).WithMany();
            modelBuilder.Entity<Cesta>().HasOne<Hrac>(c => c.hrac).WithMany();

            // Hrac
            modelBuilder.Entity<Hrac>().ToTable("Hraci");
            modelBuilder.Entity<Hrac>().HasKey(h => h.Id);

            // Hra
            modelBuilder.Entity<Hra>().ToTable("Hry");
            modelBuilder.Entity<Hra>().HasKey(h => h.Id);
            modelBuilder.Entity<Hra>().HasOne(h => h.mapka).WithOne(m => m.Hra).HasForeignKey<Mapka>(m => m.HraId);
            modelBuilder.Entity<Hra>().Property(h => h.hracNaTahu).IsRequired();
            modelBuilder.Entity<Hra>().HasMany(d => d.stavy).WithOne(d => d.hra);
            modelBuilder.Entity<Hra>().Property(h => h.stavHry).IsRequired();
            modelBuilder.Entity<Hra>().HasMany(h => h.bufferAktivit).WithOne();
            modelBuilder.Entity<Hra>().HasMany(h => h.aktivniSmeny).WithOne();
            modelBuilder.Entity<Hra>().HasMany(h => h.NerozdaneAkcniKarty).WithOne();
            modelBuilder.Entity<Hra>().HasMany(h => h.NerozdaneBodoveKarty).WithOne();
            modelBuilder.Entity<Hra>().Property(h => h.Hozene).IsRequired();

            // AkcniKarta
            modelBuilder.Entity<AkcniKarta>().ToTable("AkcniKarty");
            modelBuilder.Entity<AkcniKarta>().HasKey(h => h.Id);
            modelBuilder.Entity<AkcniKarta>().Property(d => d.Nazev).IsRequired();
            modelBuilder.Entity<AkcniKarta>().Property(d => d.Pocet).IsRequired();

            // SurovinaKarta
            modelBuilder.Entity<SurovinaKarta>().ToTable("SurovinaKarty");
            modelBuilder.Entity<SurovinaKarta>().HasKey(d => d.Id);
            modelBuilder.Entity<SurovinaKarta>().HasOne(d => d.Surovina).WithMany();
            modelBuilder.Entity<SurovinaKarta>().Property(d => d.Pocet).IsRequired();

            // Mapka
            modelBuilder.Entity<Mapka>().ToTable("Mapky");
            modelBuilder.Entity<Mapka>().HasKey(h => h.Id);
            modelBuilder.Entity<Mapka>().HasMany(m => m.Policka).WithOne(p => p.Mapka);
            modelBuilder.Entity<Mapka>().HasMany(m => m.Cesty).WithOne(c => c.Mapka);
            modelBuilder.Entity<Mapka>().HasMany(m => m.Rozcesti).WithOne(r => r.Mapka);
            modelBuilder.Entity<Mapka>().HasMany(m => m.Stavby).WithMany();
            modelBuilder.Entity<Mapka>().HasMany(m => m.Suroviny).WithMany();


            // Pole
            modelBuilder.Entity<Pole>().ToTable("Policka");
            modelBuilder.Entity<Pole>().HasKey(h => h.Id);
            modelBuilder.Entity<Pole>().Property(p => p.PoziceX).IsRequired();
            modelBuilder.Entity<Pole>().Property(p => p.PoziceY).IsRequired();
            modelBuilder.Entity<Pole>().HasOne(p => p.Surovina).WithMany();
            modelBuilder.Entity<Pole>().Property(p => p.Cislo).IsRequired();
            modelBuilder.Entity<Pole>().Property(p => p.Blokovane).IsRequired();
            modelBuilder.Entity<Pole>().HasMany(p => p.Rozcesti).WithMany();

            // Rozcesti
            modelBuilder.Entity<Rozcesti>().ToTable("Rozcesti");
            modelBuilder.Entity<Rozcesti>().HasKey(h => h.Id);
            modelBuilder.Entity<Rozcesti>().Property(r => r.PoziceX).IsRequired();
            modelBuilder.Entity<Rozcesti>().Property(r => r.PoziceY).IsRequired();
            modelBuilder.Entity<Rozcesti>().HasOne(r => r.Hrac).WithMany();
            modelBuilder.Entity<Rozcesti>().HasOne(r => r.Stavba).WithMany();

            //Smena
            modelBuilder.Entity<Smena>().ToTable("Smeny");
            modelBuilder.Entity<Smena>().HasKey(h => h.Id);
            modelBuilder.Entity<Smena>().HasOne(s => s.hrac).WithMany();
            modelBuilder.Entity<Smena>().HasMany(s => s.nabidka).WithMany();
            modelBuilder.Entity<Smena>().HasMany(s => s.poptavka).WithMany();

            // Stavba
            modelBuilder.Entity<Stavba>().ToTable("Stavby");
            modelBuilder.Entity<Stavba>().HasKey(h => h.Id);
            modelBuilder.Entity<Stavba>().Property(s => s.Nazev).IsRequired();
            modelBuilder.Entity<Stavba>().Property(s => s.Zisk).IsRequired();
            modelBuilder.Entity<Stavba>().Property(s => s.ImageUrl).IsRequired();
            modelBuilder.Entity<Stavba>().HasMany(s => s.Cena).WithOne();
            modelBuilder.Entity<Stavba>().Property(s => s.Body).IsRequired();

            // StavHrace
            modelBuilder.Entity<StavHrace>().ToTable("StavyHracu");
            modelBuilder.Entity<StavHrace>().HasKey(h => h.Id);
            modelBuilder.Entity<StavHrace>().HasOne(s => s.hra).WithMany(h => h.stavy);
            modelBuilder.Entity<StavHrace>().HasOne(s => s.hrac).WithMany();
            modelBuilder.Entity<StavHrace>().Property(s => s.barva).HasConversion(
                color => color.Name,
                name => Color.FromName(name)
            );
            modelBuilder.Entity<StavHrace>().Property(s => s.poradi).IsRequired();
            modelBuilder.Entity<StavHrace>().Property(s => s.nejdelsiCesta).IsRequired();
            modelBuilder.Entity<StavHrace>().Property(s => s.nejvetsiVojsko).IsRequired();
            modelBuilder.Entity<StavHrace>().Property(s => s.zahranychRytiru).IsRequired();

            modelBuilder.Entity<StavHrace>().HasMany<SurovinaKarta>(s => s.SurovinaKarty).WithMany();
            modelBuilder.Entity<StavHrace>().HasMany<AkcniKarta>(s => s.AkcniKarty).WithOne();
            modelBuilder.Entity<StavHrace>().HasMany<BodovaKarta>(s => s.BodoveKarty).WithOne();

            // Surovina
            modelBuilder.Entity<Surovina>().ToTable("Suroviny");
            modelBuilder.Entity<Surovina>().HasKey(h => h.Id);
            modelBuilder.Entity<Surovina>().Property(s => s.Nazev).IsRequired();
            modelBuilder.Entity<Surovina>().Property(s => s.ImageUrl).IsRequired();
            modelBuilder.Entity<Surovina>().Property(s => s.BackColor).IsRequired();
        }

        // DbSets pro každou entitu
        public DbSet<Aktivita> aktivity { get; set; }
        public DbSet<Cesta> cesty { get; set; }
        public DbSet<Hrac> hraci { get; set; }
        public DbSet<Hra> hry { get; set; }
        public DbSet<AkcniKarta> akcnikarty { get; set; }
        public DbSet<SurovinaKarta> surovinakarty { get; set; }
        public DbSet<Mapka> mapky { get; set; }
        public DbSet<Pole> policka { get; set; }
        public DbSet<Rozcesti> rozcesti { get; set; }
        public DbSet<Smena> smeny { get; set; }
        public DbSet<Stavba> stavby { get; set; }
        public DbSet<StavHrace> stavy { get; set; }
        public DbSet<Surovina> suroviny { get; set; }
    }
}
