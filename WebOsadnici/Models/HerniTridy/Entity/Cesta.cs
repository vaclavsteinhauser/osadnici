using System.Drawing;
using WebOsadnici.Models.HerniTridy;

public class Cesta : HerniEntita
{
    internal static readonly Size velikost = new Size(2,2);
    internal readonly int poziceX, poziceY;
    internal Hrac? hrac;
    internal readonly List<Rozcesti> konce= new();
    //jak natocena ma byt pri vykreslovani cesta
    //0 - |
    //1 - /
    //2 - \
    internal readonly int natoceni;

    internal Cesta(int poziceX,int poziceY, int natoceni)
    {
        this.poziceX = poziceX;
        this.poziceY = poziceY;
        this.natoceni = natoceni;
    }
}