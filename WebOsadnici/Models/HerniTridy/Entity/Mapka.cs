using WebOsadnici.Models.HerniTridy;

public class Mapka : HerniEntita
{
    private Hra hra;
    public List<Pole> policka=new();
    private List<Cesta> cesty=new();
    private List<Rozcesti> rozcesti = new();
    public Mapka() { }
    public Mapka(Hra h)
    {
        hra = h;
        policka.Add(new Pole(hra, Hra.suroviny["obili"], 1,1,1));
        policka.Add(new Pole(hra, Hra.suroviny["kamen"], 1,1,2));
        policka.Add(new Pole(hra, Hra.suroviny["drevo"], 1,1,3));
        policka.Add(new Pole(hra, Hra.suroviny["ovce"], 1,2,1));
        policka.Add(new Pole(hra, Hra.suroviny["cihla"], 1,2,2));
    }
}