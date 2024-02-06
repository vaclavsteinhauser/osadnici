using Org.BouncyCastle.Security;
using System.Drawing;
using WebOsadnici.Models.HerniTridy;

public class Mapka : HerniEntita
{
    internal static readonly Size rozmeryMrizky = new Size(50, 40);
    public Hra? hra { get; set; }
    public Guid? hraId { get; set; }

    public List<Pole> policka = new();
    public List<Cesta> cesty = new();
    public List<Rozcesti> rozcesti = new();

    //Na mřížce si vyznačím stredy políček, item 1 je posun po X ose a item2 po ose Y pocitam od horního levého rohu
    static private (int,int)[] polohyPolicek = 
        { 
            (8, 4 ), 
            (12, 4 ), 
            (16, 4 ), 
            (6, 8 ),
            (10, 8 ),
            (14, 8 ),
            (18, 8 ),
            (4, 12 ),
            (8, 12 ),
            (12, 12 ),
            (16, 12 ),
            (20, 12 ),
            (6, 16 ),
            (10, 16 ),
            (14, 16 ),
            (18, 16 ),
            (8, 20 ),
            (12, 20 ),
            (16, 20 ) 
        };
    Pole[,] sit = new Pole[25, 25]; 
    static private string[] nazvySurovin = { "Dřevo", "Dřevo", "Dřevo", "Dřevo", "Cihla", "Cihla", "Cihla", "Ovce", "Ovce", "Ovce", "Ovce", "Obilí", "Obilí", "Obilí", "Obilí", "Kámen", "Kámen", "Kámen" };
    static private int[] cislaPolicek = { 2, 3, 3, 4, 4, 5, 5, 6, 6, 8, 8, 9, 9, 10, 10, 11, 11, 12 };
    public Mapka() { }
    public Mapka(Hra hra)
    {
        this.hra = hra;
        hraId = hra.Id;
        Generuj();
    }
    private List<Surovina> zasobaSurovin = new List<Surovina>();
    private List<int> zasobaCisel = new List<int>();
    private Random rnd = new Random();

    private void Generuj()
    {
        foreach (var s in nazvySurovin)
        {
            zasobaSurovin.Add(Surovina.GetSurovina(s));
        }
        foreach (var i in cislaPolicek)
        {
            zasobaCisel.Add(i);
        }


        foreach ((int, int) souradnice in polohyPolicek)
        {

            Pole p;
            if (souradnice.Item1 == 12 && souradnice.Item2 == 12)
            {
                p = new Pole(this, Surovina.GetSurovina("Poušť"), 0, 12, 12);
            }
            else
            {
                int r = rnd.Next() % zasobaCisel.Count;
                int cislo = zasobaCisel[r];
                zasobaCisel.RemoveAt(r);
                r = rnd.Next() % zasobaSurovin.Count;
                Surovina s = zasobaSurovin[r];
                zasobaSurovin.RemoveAt(r);
                p = new Pole(this, s, cislo, souradnice.Item1, souradnice.Item2);
            }
            sit[souradnice.Item1, souradnice.Item2] = p;
            policka.Add(p);
        }

        foreach (Pole p in policka)
        {
            //Zkontroluje a popáruje rozcestí s políčky nalevo nahoru, případně nové vytvoří pokračuje pak po smeru ručiček
            //Rozcestí u políčka jsou očíslované, 0 je nahoře a pak po směru ručiček
            if (p.poziceY >= 4 && p.poziceX >= 2)
            {
                poparujPolicka(p, sit[p.poziceX - 2, p.poziceY - 4], 5, 3);
                poparujPolicka(p, sit[p.poziceX - 2, p.poziceY - 4], 0, 2);
            }
            if (p.poziceY >= 4 && p.poziceX < sit.GetLength(0)-2)
            {
                poparujPolicka(p, sit[p.poziceX + 2, p.poziceY - 4], 0, 4);
                poparujPolicka(p, sit[p.poziceX + 2, p.poziceY - 4], 1, 3);
            }
            if (p.poziceX < sit.GetLength(0) - 4)
            {
                poparujPolicka(p, sit[p.poziceX + 4, p.poziceY], 1, 5);
                poparujPolicka(p, sit[p.poziceX + 4, p.poziceY], 2, 4);
            }
            if (p.poziceY < sit.GetLength(1) - 4 && p.poziceX < sit.GetLength(0) - 2)
            {
                poparujPolicka(p, sit[p.poziceX + 2, p.poziceY + 4], 2, 0);
                poparujPolicka(p, sit[p.poziceX + 2, p.poziceY + 4], 3, 5);
            }
            if (p.poziceY < sit.GetLength(1) - 4 && p.poziceX >= 2)
            {
                poparujPolicka(p, sit[p.poziceX - 2, p.poziceY + 4], 3, 1);
                poparujPolicka(p, sit[p.poziceX - 2, p.poziceY + 4], 4, 0);
            }
            if (p.poziceY >= 4)
            {
                poparujPolicka(p, sit[p.poziceX - 4, p.poziceY], 4, 2);
                poparujPolicka(p, sit[p.poziceX - 4, p.poziceY], 5, 1);
            }
        }
        foreach (Pole p in policka)
        {
            foreach(Rozcesti r in p.rozcesti)
            {
                if (r == null) throw new Exception("spatne se vygenerovaly rozcesti");
                if(r !=null && !rozcesti.Contains(r)) rozcesti.Add(r);
            }
            for (int i = 0; i < p.rozcesti.Count; i++)
            {
                vytvorCestu(p.rozcesti[i], p.rozcesti[(i+1)%p.rozcesti.Count]);
            }
        }
        Console.WriteLine();
    }
    private void vytvorCestu(Rozcesti a, Rozcesti b)
    {
        bool zbytecna=false;
        foreach(Cesta c in cesty)
        {
            if(a!=null && b!=null && c.konce.Contains(a) && c.konce.Contains(b)) zbytecna = true;
        }
        if (!zbytecna)
        {
            int natoceni;
            if (a.poziceX == b.poziceX)
                natoceni = 0;
            else if ((a.poziceX > b.poziceX && a.poziceY < b.poziceY) || (a.poziceX < b.poziceX && a.poziceY > b.poziceY))
                natoceni = 2;
            else natoceni = 1;
            Cesta c = new Cesta((a.poziceX + b.poziceX) / 2, (a.poziceY + b.poziceY) / 2, natoceni);
            c.konce.Add(a);
            c.konce.Add(b);
            cesty.Add(c);
        }
    }
    private void poparujPolicka(Pole vstupni, Pole cizi, int indexVstupni, int indexCizi)
    {
        if (cizi != null && cizi.rozcesti[indexCizi] != null)
        {
                vstupni.rozcesti[indexVstupni] = cizi.rozcesti[indexCizi];
        }
        if (vstupni.rozcesti[indexVstupni] == null)
        {
            Size umisteni;
            switch (indexVstupni)
            {
                case 0: umisteni = new Size(vstupni.poziceX, vstupni.poziceY - 3); break;
                case 1: umisteni = new Size(vstupni.poziceX + 2, vstupni.poziceY - 1); break;
                case 2: umisteni = new Size(vstupni.poziceX + 2, vstupni.poziceY + 1); break;
                case 3: umisteni = new Size(vstupni.poziceX, vstupni.poziceY + 3); break;
                case 4: umisteni = new Size(vstupni.poziceX - 2, vstupni.poziceY + 1); break;
                default: umisteni = new Size(vstupni.poziceX - 2, vstupni.poziceY - 1); break;

            }
            vstupni.rozcesti[indexVstupni] = new Rozcesti(umisteni.Width,umisteni.Height);
        }
    }
}