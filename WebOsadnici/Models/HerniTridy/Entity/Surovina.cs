using Microsoft.EntityFrameworkCore;
using WebOsadnici.Models.HerniTridy;

public class Surovina : HerniEntita
{
    public string Nazev { get; set; }
    public string ImageUrl { get; set; }
    public string BackColor { get; set; }

    internal static async Task VytvorSuroviny(DbSet<Surovina> _suroviny)
    {
        Surovina s = _suroviny.Where(s => s.Nazev.Equals("Dřevo")).FirstOrDefault();
        if (s == null)
        {
            s = new Surovina()
            {
                Nazev = "Dřevo",
                ImageUrl = "drevo.svg",
                BackColor = "Sienna"
            };
            _suroviny.AddAsync(s);
        }
        s = _suroviny.Where(s => s.Nazev.Equals("Cihla")).FirstOrDefault();
        if (s == null)
        {
            s = new Surovina()
            {
                Nazev = "Cihla",
                ImageUrl = "cihla.svg",
                BackColor = "Firebrick"
            };
            _suroviny.AddAsync(s);
        }
        s = _suroviny.Where(s => s.Nazev.Equals("Obilí")).FirstOrDefault();
        if (s == null)
        {
            s = new Surovina()
            {
                Nazev = "Obilí",
                ImageUrl = "obili.svg",
                BackColor = "Gold"
            };
            _suroviny.AddAsync(s);
        }
        s = _suroviny.Where(s => s.Nazev.Equals("Ovce")).FirstOrDefault();
        if (s == null)
        {
            s = new Surovina()
            {
                Nazev = "Ovce",
                ImageUrl = "ovce.svg",
                BackColor = "Limegreen"
            };
            _suroviny.AddAsync(s);
        }
        s = _suroviny.Where(s => s.Nazev.Equals("Kámen")).FirstOrDefault();
        if (s == null)
        {
            s = new Surovina()
            {
                Nazev = "Kámen",
                ImageUrl = "kamen.svg",
                BackColor = "Gray"
            };
            _suroviny.AddAsync(s);
        }
        s = _suroviny.Where(s => s.Nazev.Equals("Poušť")).FirstOrDefault();
        if (s == null)
        {
            s = new Surovina()
            {
                Nazev = "Poušť",
                ImageUrl = "poust.svg",
                BackColor = "Yellow"
            };
            _suroviny.AddAsync(s);
        }
    }
}

