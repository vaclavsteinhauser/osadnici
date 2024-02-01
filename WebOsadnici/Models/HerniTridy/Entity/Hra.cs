using System.Drawing;
using WebOsadnici.Models.HerniTridy;

public class Hra : HerniEntita
{
    public Hra()
    {
        mapka = new Mapka(this);
    }
    public List<Hrac> hraci=new List<Hrac>();
    public static readonly Dictionary<Hrac, List<Surovina>> ruka = new Dictionary<Hrac, List<Surovina>>();
    private int hracNaTahu = -1;
    public Mapka mapka;
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
        ruka.Add(h,new List<Surovina>());
    }
    private void ZacniTah()
    {
        hracNaTahu += 1;
        HodKostkou();
        foreach (var p in mapka.policka) {
            if (p.cislo == hodnotaKostek)
            {
                foreach (var r in p.rozcesti) {
                if(r.stavba!=null)
                        for(int i = 0; i < r.stavba.zisk; i++)
                        {
                            ruka[r.hrac].Add(p.surovina);
                        }
                }
            }
        }

    }
}