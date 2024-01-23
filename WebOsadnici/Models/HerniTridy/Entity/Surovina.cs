using WebOsadnici.Models.HerniTridy;

public abstract class Surovina : HerniEntita
{
    public string Nazev { get; protected set; }
    public string ImageUrl { get; protected set; }
    public string BackColor { get; protected set; }

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