using System.Drawing;
using WebOsadnici.Models.HerniTridy;

public class Pole : HerniEntita
{
    public static readonly Size velikost = new Size(200, 230);
    public readonly Size pozice;
    private Hra hra;
    public Surovina surovina;
    private Int32 cislo;
    public Pole(Hra hra, Surovina surovina, int cislo, int radek,int sloupec)
    {
        this.hra = hra;
        this.surovina = surovina;
        this.cislo = cislo;
        this.pozice = new Size(sloupec,radek);
    }
}