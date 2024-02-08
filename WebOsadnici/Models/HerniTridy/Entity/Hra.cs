using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WebOsadnici.Controllers;
using WebOsadnici.Data;
using WebOsadnici.Models.HerniTridy;

public class Hra : HerniEntita
{
    public static IQueryable<Hra> NactiHry(ApplicationDbContext context)
    {
        return context.hry
                .Include(h => h.hraci)
                .Include(h => h.mapka)
                .Include(h => h.mapka.cesty)
                .Include(h => h.mapka.rozcesti)
                .Include(h => h.mapka.policka);
    }
    public Hra() { }
    public Hra(DbSet<Surovina> suroviny, DbSet<Stavba> stavby)
    {
        mapka = new Mapka(this,suroviny,stavby);
    }
    public List<Hrac> hraci=new List<Hrac>();
    public List<StavHrace> stavy = new List<StavHrace>();
    public int hracNaTahu = -1;
    public Mapka? mapka;

    private Random kostka=new Random();
    int hodnotaKostek;
    private void HodKostkou(int pocet=2)
    {
        hodnotaKostek = 0;
        for (int i = 0; i < pocet; i++)
        {
            hodnotaKostek += (int)(kostka.Next() % 6) + 1;
        }
    }
    public bool JeObsazenaBarva(String b)
    {
        foreach(StavHrace s in stavy)
        {
            if (b.Equals(s.barva.Name))
                return true;
        }
        return false;
    }
    internal void PridejHrace(Hrac h, Color barva, ApplicationDbContext _dbContext)
    {
        hraci.Add(h);
        StavHrace s = new StavHrace()
        {
            hra = this,
            barva = barva,
            hrac = h
        };
        stavy.Add(s);
    }
    private void ZacniTah()
    {
        hracNaTahu += 1;
        HodKostkou();
        foreach (var p in mapka.policka) {
            if (p.cislo == hodnotaKostek)
            {
                foreach (var r in p.rozcesti) {
                    if (r.stavba != null)
                    {
                        stavy.Where(s => s.hrac == r.hrac).First().PridejSurovinu(p.surovina, r.stavba.zisk);
                    }
                }
            }
        }

    }
}