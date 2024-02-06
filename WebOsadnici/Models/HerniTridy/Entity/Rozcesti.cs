using System.Drawing;
using WebOsadnici.Models.HerniTridy;

public class Rozcesti : HerniEntita
{
    //velikost urcuje prumer kruhu
    internal readonly static Size velikost = new Size(2,2);
    internal readonly int poziceX,poziceY;
    internal Hrac? hrac;
    internal readonly List<Cesta> cesty=new();
    public bool blokovane=false;
    internal Stavba stavba;
    internal Rozcesti(int poziceX,int poziceY)
    {
        this.poziceX = poziceX;
        this.poziceY = poziceY;
    }
    internal Rozcesti[] sousedi()
    {
        Rozcesti[] r=new Rozcesti[cesty.Count];
        for (int i=0; i<cesty.Count; i++)
        {
            if (cesty[i].konce[0] == this)
                r[i] = cesty[i].konce[1];
            else
                r[i] = cesty[i].konce[0];
        }
        return r;
    }
}