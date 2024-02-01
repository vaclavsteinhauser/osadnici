using WebOsadnici.Models.HerniTridy;

public abstract class Surovina : HerniEntita
{
    public string Nazev { get; protected set; }
    public string ImageUrl { get; protected set; }
    public string BackColor { get; protected set; }
    public static Dictionary<String, Surovina> SurovinaList { get; private set; }
    static Surovina()
    {
        SurovinaList = new Dictionary<string, Surovina>
        {
            { "drevo", new Drevo() },
            { "cihla", new Cihla() },
            { "ovce", new Ovce() },
            { "obili", new Obili() },
            { "kamen", new Kamen() },
            { "poust", new Poust() }
        };
    }
}
public class Drevo : Surovina
{
    public Drevo() {
        Nazev = "Dřevo";
        ImageUrl = "drevo.svg";
        BackColor = "Sienna";
    }
}
public class Cihla : Surovina
{
    public Cihla()
    {
        Nazev = "Cihla";
        ImageUrl = "cihla.svg";
        BackColor = "Firebrick";
    }
}
public class Ovce : Surovina
{
    public Ovce()
    {
        Nazev = "Ovce";
        ImageUrl = "ovce.svg";
        BackColor = "Limegreen";
    }

}
public class Obili : Surovina
{
    public Obili()
    {
        Nazev = "Obili";
        ImageUrl = "obili.svg";
        BackColor = "Gold";
    }

}
public class Kamen : Surovina
{
    public Kamen()
    {
        Nazev = "Kámen";
        ImageUrl = "kamen.svg";
        BackColor = "Gray";
    }

}
public class Poust : Surovina
{
    public Poust()
    {
        Nazev = "Poušť";
        ImageUrl = "poust.svg";
        BackColor = "Yellow";
    }

}