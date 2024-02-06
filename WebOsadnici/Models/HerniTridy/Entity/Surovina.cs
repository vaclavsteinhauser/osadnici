using WebOsadnici.Models.HerniTridy;

public class Surovina : HerniEntita
{
    public string Nazev { get; protected set; }
    public string ImageUrl { get; protected set; }
    public string BackColor { get; protected set; }
    public static List<Surovina> SurovinaList { get; private set; }
    = new List<Surovina>()
        {
            
            new Surovina()
            {
                Nazev= "Dřevo",
                ImageUrl = "../drevo.svg",
                BackColor = "Sienna"
                },
             new Surovina()
             {
                Nazev= "Cihla",
                ImageUrl = "../cihla.svg",
                BackColor = "Firebrick"
                },
             new Surovina()
             {
                 Nazev = "Ovce",
                ImageUrl = "../ovce.svg",
                BackColor = "Limegreen"
             },
             new Surovina()
             {
                 Nazev = "Obilí",
                ImageUrl = "../obili.svg",
                BackColor = "Gold"
             },
             new Surovina()
             {
                 Nazev = "Kámen",
                ImageUrl = "../kamen.svg",
                BackColor = "Gray"
             },
             new Surovina()
             {
                 Nazev = "Poušť",
                ImageUrl = "../poust.svg",
                BackColor = "Yellow"
             }

            

        };
    public static Surovina GetSurovina(string surovinaId)
    {
        foreach(Surovina surovina in SurovinaList)
            if(surovina.Nazev == surovinaId)
                return surovina;
        return null;
    } 
public Surovina()
    {
        
    }
}

