using Microsoft.EntityFrameworkCore;
using WebOsadnici.Models.HerniTridy;

public class Stavba : HerniEntita
{
    internal string Nazev;
    internal int zisk;
    internal string ImageUrl;
    public Stavba(string Nazev,int zisk, string imageUrl)
    {
        this.Nazev = Nazev;
        this.zisk = zisk;
        ImageUrl = imageUrl;
    }
    internal static async Task VytvorStavby(DbSet<Stavba> _stavby)
    {
        Stavba v = _stavby.Where(s => s.Nazev.Equals("Vesnice")).SingleOrDefault();
        if (v == null)
        {
            v = new Stavba("Vesnice", 1, "vesnicka.svg");
            _stavby.AddAsync(v);
        }
        v = _stavby.Where(s => s.Nazev.Equals("Město")).SingleOrDefault();
        if (v == null)
        {
            v = new Stavba("Město", 2, "mesto.svg");
            _stavby.AddAsync(v);
        }
    }
}