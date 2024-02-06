using System.Drawing;
using WebOsadnici.Models.HerniTridy;

public class Pole : HerniEntita
{
    public static readonly Size velikost = new Size(4, 6);
    public int poziceX,poziceY;
    internal Mapka mapka;
    public Surovina surovina;
    internal Int32 cislo;
    internal bool blokovane;
    internal readonly List<Rozcesti> rozcesti=new List<Rozcesti>(6) { null, null, null, null, null, null };
    public Pole() { }
    public Pole(Mapka mapka, Surovina surovina, int cislo,int sloupec, int radek, bool blokovane=false)
    {
        this.mapka = mapka;
        this.surovina = surovina;
        this.cislo = cislo;
        this.poziceX = sloupec;
        this.poziceY =radek;
        this.blokovane = blokovane;
    }

    public override string ToString()
    {
        return surovina.Nazev + " na " + poziceX.ToString() + " " + poziceY.ToString() +" s cislem " +cislo.ToString();
    }
}