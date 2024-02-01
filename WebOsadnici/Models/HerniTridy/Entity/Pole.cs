using System.Drawing;
using WebOsadnici.Models.HerniTridy;

public class Pole : HerniEntita
{
    public static readonly Size velikost = new Size(200, 230);
    public readonly Size pozice;
    internal readonly Hra hra;
    public readonly Surovina surovina;
    internal readonly Int32 cislo;
    internal bool blokovane;
    internal  readonly List<Rozcesti> rozcesti=new List<Rozcesti>();
    public Pole(Hra hra, Surovina surovina, int cislo, int radek,int sloupec, bool blokovane=false)
    {
        this.hra = hra;
        this.surovina = surovina;
        this.cislo = cislo;
        this.pozice = new Size(sloupec, radek);
        this.blokovane = blokovane;
    }


}