using System.Collections.ObjectModel;
using System.Drawing;
using WebOsadnici.Data;

namespace WebOsadnici.Models.HerniTridy;
/// <summary>
/// Reprezentuje herní mapu s políčky, cestami a rozcestími.
/// </summary>
public class Mapka : HerniEntita
{
    internal static readonly Size RozmeryMrizky = new Size(30, 25);

    /// <summary>
    /// Reference na herní hru, ke které je mapa přiřazena.
    /// </summary>
    public virtual Hra? Hra { get; set; }

    /// <summary>
    /// ID herní hry, ke které je mapa přiřazena.
    /// </summary>
    public virtual Guid? HraId { get; set; }
    /// <summary>
    /// Kolekce všech políček na mapě.
    /// </summary>
    public virtual ObservableCollection<Pole> Policka { get; set; } = new();
    /// <summary>
    /// Kolekce všech cest na mapě.
    /// </summary>
    public virtual ObservableCollection<Cesta> Cesty { get; set; } = new();
    /// <summary>
    /// Kolekce všech rozcestí na mapě.
    /// </summary>
    public virtual ObservableCollection<Rozcesti> Rozcesti { get; set; } = new();
    public virtual ObservableCollection<Surovina> Suroviny { get; set; } = new();
    public virtual ObservableCollection<Stavba> Stavby { get; set; } = new();
    static private (int, int)[] polohyPolicek =
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
    private List<Surovina> zasobaSurovin = new List<Surovina>();
    private List<int> zasobaCisel = new List<int>();
    private Random rnd = new Random();
    public Mapka() { }
    

    /// <summary>
    /// Inicializuje herní mapu s danou hrou.
    /// </summary>
    /// <param name="hra">Hra, ke které je mapa přiřazena.</param>
    /// <param name="_dbContext">Instance databázového kontextu.</param>
    public async Task Inicializace(Hra hra, ApplicationDbContext _dbContext)
    {
        // Vytvoření surovin a staveb
        await Surovina.VytvorSuroviny(_dbContext);
        await Stavba.VytvorStavby(_dbContext);
        await _dbContext.SaveChangesAsync();

        // Přidání surovin a staveb do mapy
        foreach (Surovina s in _dbContext.suroviny.ToArray())
        {
            Suroviny.Add(s);
        }
        foreach (Stavba s in _dbContext.stavby.ToArray())
        {
            Stavby.Add(s);
        }

        // Přiřazení herní hry a inicializace mapy
        Hra = hra;
        HraId = hra.Id;
        Generuj();
    }

    private void Generuj()
    {
        foreach (var s in nazvySurovin)
        {
            zasobaSurovin.Add(Suroviny.FirstOrDefault(x=>x.Nazev==s));
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
                p = new Pole(this,Suroviny.FirstOrDefault(x=>x.Nazev=="Poušť"), 0, 12, 12);
            }
            else
            {
                int r = rnd.Next() % zasobaCisel.Count;
                int cislo = zasobaCisel[r];
                zasobaCisel.RemoveAt(r);
                r = rnd.Next() % zasobaSurovin.Count;
                Surovina s = zasobaSurovin[r];
                zasobaSurovin.RemoveAt(r);
                p = new Pole(this,s, cislo, souradnice.Item1, souradnice.Item2);
            }
            sit[souradnice.Item1, souradnice.Item2] = p;
            Policka.Add(p);
        }

        foreach (Pole p in Policka)
        {
            //Zkontroluje a popáruje rozcestí s políčky nalevo nahoru, případně nové vytvoří pokračuje pak po směru ručiček
            //Rozcestí u políčka jsou očíslované, 0 je nahoře a pak po směru ručiček
            if (p.PoziceY >= 4 && p.PoziceX >= 2)
            {
                poparujPolicka(p, sit[p.PoziceX - 2, p.PoziceY - 4], 5, 3);
                poparujPolicka(p, sit[p.PoziceX - 2, p.PoziceY - 4], 0, 2);
            }
            if (p.PoziceY >= 4 && p.PoziceX < sit.GetLength(0) - 2)
            {
                poparujPolicka(p, sit[p.PoziceX + 2, p.PoziceY - 4], 0, 4);
                poparujPolicka(p, sit[p.PoziceX + 2, p.PoziceY - 4], 1, 3);
            }
            if (p.PoziceX < sit.GetLength(0) - 4)
            {
                poparujPolicka(p, sit[p.PoziceX + 4, p.PoziceY], 1, 5);
                poparujPolicka(p, sit[p.PoziceX + 4, p.PoziceY], 2, 4);
            }
            if (p.PoziceY < sit.GetLength(1) - 4 && p.PoziceX < sit.GetLength(0) - 2)
            {
                poparujPolicka(p, sit[p.PoziceX + 2, p.PoziceY + 4], 2, 0);
                poparujPolicka(p, sit[p.PoziceX + 2, p.PoziceY + 4], 3, 5);
            }
            if (p.PoziceY < sit.GetLength(1) - 4 && p.PoziceX >= 2)
            {
                poparujPolicka(p, sit[p.PoziceX - 2, p.PoziceY + 4], 3, 1);
                poparujPolicka(p, sit[p.PoziceX - 2, p.PoziceY + 4], 4, 0);
            }
            if (p.PoziceY >= 4)
            {
                poparujPolicka(p, sit[p.PoziceX - 4, p.PoziceY], 4, 2);
                poparujPolicka(p, sit[p.PoziceX - 4, p.PoziceY], 5, 1);
            }
        }
        foreach (Pole p in Policka)
        {
            foreach (Rozcesti r in p.Rozcesti)
            {
                if (r == null) throw new Exception("spatne se vygenerovaly rozcesti");
                if (r != null && !Rozcesti.Contains(r)) Rozcesti.Add(r);
            }
            for (int i = 0; i < p.Rozcesti.Count; i++)
            {
                vytvorCestu(p.Rozcesti[i], p.Rozcesti[(i + 1) % p.Rozcesti.Count]);
            }
        }
    }

    private void vytvorCestu(Rozcesti a, Rozcesti b)
    {
        bool zbytecna = false;
        foreach (Cesta c in Cesty)
        {
            if (a != null && b != null && c.rozcesti.Contains(a) && c.rozcesti.Contains(b)) zbytecna = true;
        }
        if (!zbytecna)
        {
            int natoceni;
            if (a.PoziceX == b.PoziceX)
                natoceni = 0;
            else if ((a.PoziceX > b.PoziceX && a.PoziceY < b.PoziceY) || (a.PoziceX < b.PoziceX && a.PoziceY > b.PoziceY))
                natoceni = 2;
            else natoceni = 1;
            Cesta c = new Cesta(this,(a.PoziceX + b.PoziceX) / 2, (a.PoziceY + b.PoziceY) / 2, natoceni);
            c.rozcesti.Add(a);
            c.rozcesti.Add(b);
            Cesty.Add(c);
        }
    }

    private void poparujPolicka(Pole vstupni, Pole cizi, int indexVstupni, int indexCizi)
    {
        if (cizi != null && cizi.Rozcesti[indexCizi] != null)
        {
            vstupni.Rozcesti[indexVstupni] = cizi.Rozcesti[indexCizi];
        }
        if (vstupni.Rozcesti[indexVstupni] == null)
        {
            Size umisteni;
            switch (indexVstupni)
            {
                case 0: umisteni = new Size(vstupni.PoziceX, vstupni.PoziceY - 3); break;
                case 1: umisteni = new Size(vstupni.PoziceX + 2, vstupni.PoziceY - 1); break;
                case 2: umisteni = new Size(vstupni.PoziceX + 2, vstupni.PoziceY + 1); break;
                case 3: umisteni = new Size(vstupni.PoziceX, vstupni.PoziceY + 3); break;
                case 4: umisteni = new Size(vstupni.PoziceX - 2, vstupni.PoziceY + 1); break;
                default: umisteni = new Size(vstupni.PoziceX - 2, vstupni.PoziceY - 1); break;

            }
            vstupni.Rozcesti[indexVstupni] = new Rozcesti(this,umisteni.Width, umisteni.Height);
        }
    }
}
