using WebOsadnici.Models.HerniTridy;

public class Mapka : HerniEntita
{
    private Hra hra;
    public List<Pole> policka=new();
    private List<Cesta> cesty=new();
    private List<Rozcesti> rozcesti = new();

    static private string[] nazvySurovin = { "drevo", "drevo", "drevo", "drevo", "cihla", "cihla", "cihla", "ovce", "ovce", "ovce", "ovce", "obili", "obili", "obili", "obili", "kamen", "kamen", "kamen" };
    static private int[] cislaPolicek = { 2, 3, 3, 4, 4, 5, 5, 6, 6, 8, 8, 9, 9, 10, 10, 11, 11, 12 };
    public Mapka() { }
    public Mapka(Hra h)
    {
        hra = h;
        Generuj();
        /*policka.Add(new Pole(hra, Surovina.SurovinaList["obili"], 1,1,1));
        policka.Add(new Pole(hra, Surovina.SurovinaList["kamen"], 1,1,2));
        policka.Add(new Pole(hra, Surovina.SurovinaList["drevo"], 1,1,3));
        policka.Add(new Pole(hra, Surovina.SurovinaList["ovce"], 1,2,1));
        policka.Add(new Pole(hra, Surovina.SurovinaList["cihla"], 1, 2, 2));
        policka.Add(new Pole(hra, Surovina.SurovinaList["poust"], 1, 2, 3));*/
    }
    private List<Surovina> zasobaSurovin = new List<Surovina>();
    private List<int> zasobaCisel = new List<int>();
    private Random rnd = new Random();

    private void Generuj()
    {
        foreach(var s in nazvySurovin)
        {
            zasobaSurovin.Add(Surovina.SurovinaList[s]);
        }
        foreach(var i in cislaPolicek)
        {
            zasobaCisel.Add(i);
        }
        List<List<Pole>> sit=new List<List<Pole>>();
        int[] radky = { 3, 4, 5, 4, 3 };
        for(int i = 0; i < radky.Length; i++)
        {   
            sit.Add(new List<Pole>());
            for(int j = 0; j < radky[i]; j++)
            {
                if(i==2 && j == 2)
                {
                    sit.Last().Add(new Pole(hra, Surovina.SurovinaList["poust"],0,2,2));
                    continue;
                }
                int r = rnd.Next() % zasobaCisel.Count;
                int cislo = zasobaCisel[r];
                zasobaCisel.RemoveAt(r);
                r = rnd.Next() % zasobaSurovin.Count;
                Surovina s = zasobaSurovin[r];
                zasobaSurovin.RemoveAt(r);
                Pole p = new Pole(hra, s, cislo, i, j);
                sit.Last().Add(p);
            }
        }
        foreach(var l in sit)
        {
            policka.AddRange(l);
        }
    }

}