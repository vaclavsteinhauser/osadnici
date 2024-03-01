using System.Drawing;
using WebOsadnici.Controllers;
using WebOsadnici.Models.HerniTridy;
public class StavHrace : HerniEntita
{
    //spojení se hrou a hráčem
    public Hra hra {  get; set; }
    public Hrac hrac { get; set; }
    public Color barva { get; set; }
    public int poradi;

    public List<AkcniKarta> AkcniKarty { get; } = new List<AkcniKarta>();
    public List<SurovinaKarta> SurovinaKarty { get; } = new List<SurovinaKarta>();
    public bool JeNaTahu()
    {
        return hra.hracNaTahu == poradi;
    }
    public void PridejSurovinu(Surovina s, int pocet=1)
    {
        foreach(var karta in SurovinaKarty)
        {
            if (karta.surovina == s)
            {
                karta.pocet += pocet;
                return;
            }
        }
    }
    public void PridejAkcniKartu(AkcniKarta k)
    {
        foreach (var karta in AkcniKarty)
        {
            if (karta.Nazev.Equals(k.Nazev))
            {
                karta.pocet ++;
                return;
            }
        }
        AkcniKarty.Add(k);
    }
}
