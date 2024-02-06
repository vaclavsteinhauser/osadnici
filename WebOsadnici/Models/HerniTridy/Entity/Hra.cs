using Microsoft.EntityFrameworkCore;
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
    public Hra()
    {
        mapka = new Mapka(this);
    }
    public List<Hrac> hraci=new List<Hrac>();
    //public readonly Dictionary<Hrac, Dictionary<Surovina,int>> ruka = new();
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
    internal void PridejHrace(Hrac h)
    {
        hraci.Add(h);
        //ruka.Add(h, new Dictionary<Surovina, int>());
        
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
                        /*if (!ruka[r.hrac].ContainsKey(p.surovina))
                        {
                            ruka[r.hrac].Add(p.surovina, r.stavba.zisk);
                        }
                        else
                        {
                            ruka[r.hrac][p.surovina] += r.stavba.zisk;
                        }*/
                    }
                }
            }
        }

    }
}