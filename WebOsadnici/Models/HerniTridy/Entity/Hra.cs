using System.Drawing;
using WebOsadnici.Models.HerniTridy;

public class Hra : HerniEntita
{
    public static readonly Dictionary<String,Surovina> suroviny = new Dictionary<string, Surovina>();
    static Hra()
    {

        suroviny.Add("drevo", new Drevo());
        suroviny.Add("cihla", new Cihla());
        suroviny.Add("obili", new Obili());
        suroviny.Add("ovce", new Ovce());
        suroviny.Add("kamen", new Kamen());
    }
    public Hra()
    {
        mapka = new Mapka(this);
    }
    public List<Hrac> hraci;
    public Mapka mapka;
}