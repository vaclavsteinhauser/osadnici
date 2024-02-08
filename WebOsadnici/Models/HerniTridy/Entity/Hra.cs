using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WebOsadnici.Controllers;
using WebOsadnici.Data;
using WebOsadnici.Models.HerniTridy;
public enum StavHry { Vytvorena, rozestavovani, probiha, skoncila}
public class Hra : HerniEntita
{
    public StavHry stavHry = StavHry.Vytvorena;
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
            hrac = h,
            poradi = hraci.Count
        };
        stavy.Add(s);
        _dbContext.Add(s);
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