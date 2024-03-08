using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Text;
using WebOsadnici.Data;
using WebOsadnici.Hubs;

namespace WebOsadnici.Models.HerniTridy;
public enum StavHry { Nezacala, Probiha, Skoncila }

/// <summary>
/// Třída reprezentující hru.
/// </summary>
public class Hra : HerniEntita
{
    private ApplicationDbContext _dbContext;
    public static IHubContext<ClickHub> HubContext { get; set; }
    static public readonly int MaximalneHracu = 4;


    public async Task Inicializace(ApplicationDbContext _dbContext)
    {
        _mapka = new Mapka();
        await _mapka.Inicializace(this, _dbContext);
        this._dbContext = _dbContext;
    }
    [NotMapped]
    public DateTime PosledniPouziti { get; set; }
    private static List<Hra> NacteneHry;
    //metoda k vypusteni neaktivnich her z paměti
    private static void Cisteni()
    {
        while (true)
        {
            lock (NacteneHry)
            {
                for (int i = 0; i < NacteneHry.Count; i++)
                    if (DateTime.Now.Subtract(NacteneHry[i].PosledniPouziti).TotalMinutes > 30)
                    {
                        NacteneHry.RemoveAt(i);
                        i--;
                    }
            }
            Thread.Sleep(30 * 60 * 1000);
        }
    }
    //staticky konstruktor, který zapne automatické čištěni
    static Hra()
    {
        NacteneHry = new();
        new Thread(Cisteni).Start();
    }
    public static async Task<Hra> NactiHru(Guid hraId, ApplicationDbContext context)
    {
        Hra? hra;
        lock (NacteneHry)
        {
            hra = NacteneHry.FirstOrDefault(h => h.Id == hraId);
        }
        if(hra!=null)
        {
            hra.PosledniPouziti = DateTime.Now;
            return hra;
        }
        hra = await context.hry
            .Where(h => h.Id == hraId)
            .Include(h => h.stavy)
                .ThenInclude(s => s.SurovinaKarty)// Surovinové karty stavů
            .Include(h => h.stavy)
                .ThenInclude(s => s.AkcniKarty) // Akční karty stavů
            .Include(h => h.mapka)
                .ThenInclude(m => m.Policka) // Policka
                    .ThenInclude(p => p.Surovina) // Suroviny políček
            .Include(h => h.mapka)
                .ThenInclude(m => m.Cesty) // Cesty
                    .ThenInclude(m => m.rozcesti) // Rozcesti na cestach
            .FirstOrDefaultAsync();
        if (hra != null)
        {
            lock (NacteneHry)
            {
                NacteneHry.Add(hra);
            }
            
            hra._dbContext = context;
            hra.PosledniPouziti = DateTime.Now;
        }
            
        return hra;
    }

    private StavHry _stavHry;
    public virtual StavHry stavHry
    {
        get => _stavHry;
        set
        {
            if (_stavHry != value)
            {
                OnPropertyChanging(nameof(stavHry));
                _stavHry = value;
                OnPropertyChanged(nameof(stavHry));
            }
        }
    }
    [NotMapped]
    public List<Hrac> hraci
    {
        get => stavy.Select(s => s.hrac).ToList();
    }

    public virtual ObservableCollection<StavHrace> stavy { get; set; }

    public virtual ObservableCollection<Aktivita> bufferAktivit { get; set; }

    public virtual ObservableCollection<Smena> aktivniSmeny { get; set; }

    private int _hracNaTahu = -1;
    public virtual int hracNaTahu
    {
        get => _hracNaTahu;
        set
        {
            if (_hracNaTahu != value)
            {
                OnPropertyChanging(nameof(hracNaTahu));
                _hracNaTahu = value;
                OnPropertyChanged(nameof(hracNaTahu));
            }
        }
    }
    public Hrac? DejHrace(int poradi)
    {
        return stavy.FirstOrDefault(s => s.poradi == poradi)?.hrac;
    }
    public Hrac DejHrace(string id)
    {
        return stavy.FirstOrDefault(s => s.hrac.Id == id)?.hrac;
    }
    public Hrac AktualniHrac()
    {
        var ak = AktualniAktivita();
        if (ak != null)
            return ak.Hrac;
        return DejHrace(hracNaTahu);
    }

    public Aktivita AktualniAktivita()
    {
        return bufferAktivit.OrderBy(x => x.CisloAktivity).FirstOrDefault();
    }
    public void PridejAktivitu(Aktivita a)
    {
        int i = bufferAktivit.Select(x => x.CisloAktivity).Max() + 1;
        a.CisloAktivity = i;
        bufferAktivit.Add(a);
    }


    public StavHrace DejStav(Hrac h)
    {
        return stavy.FirstOrDefault(s => s.hrac == h);
    }

    public String DejInstrukci(String id) => DejInstrukci(DejHrace(id));

    public String DejInstrukci(Hrac h)
    {
        if (stavHry == StavHry.Nezacala)
        {
            if (DejStav(h).poradi == 0)
            {
                if (stavy.Count > 1)
                {
                    return "Začni hru nebo čekej na další hráče";
                }
                else
                {
                    return "Čekej na další hráče";
                }
            }
            else
            {
                return "Čekat na začátek hry";
            }
        }
        else if (stavHry == StavHry.Probiha)
        {
            if (AktualniHrac() == h)
            {
                var a = AktualniAktivita();
                if (a != null)
                {

                    return AktualniAktivita().Akce.ToString();
                }
                else
                {
                    return "Vyber akci";
                }
            }
            else
            {
                return "Nejsi na tahu, můžeš jen souhlasit se směnami.";
            }
        }
        else
        {
            return "Hra skončila";
        }
    }

    public string DejNakupHTML(Hrac hrac)
    {
        StringBuilder sb = new();
        foreach(Stavba s in mapka.Stavby)
        {
            bool zapnute=false;
            if(s.Cena.All(sc => DejStav(hrac).SurovinaKarty.Any(sk => sk.Surovina == sc.Surovina && sk.Pocet >= sc.Pocet)))
            {
                zapnute = true;
            }
            sb.AppendLine($"<button class='stavba' id={s.Nazev} {(zapnute? "":"disabled")}>");
            sb.AppendLine("<span class='stavba-nazev'>");
            sb.AppendLine(s.Nazev);
            sb.AppendLine("</span>");
            sb.AppendLine("<span class='stavba-cena'>");
            foreach(SurovinaKarta sc in s.Cena)
            {
                sb.AppendLine($"<p>{sc.Pocet}x {sc.Surovina.Nazev}</p>");
            }
            sb.AppendLine("</span>");
            sb.AppendLine($"<img class='stavba-obrazek obrazek' src='../../{s.ImageUrl}' />");
            sb.AppendLine("</button>");
        }
        return sb.ToString();
    }

    public string DejAkcniKartyHTML(Hrac h)
    {
        StringBuilder sb = new();
        foreach(AkcniKarta a in DejStav(h).AkcniKarty)
        {
            bool zapnuto = (a.Pocet > 0);
            sb.AppendLine($"<button class='akcni-karta karta' {(zapnuto? "":"disabled")}>");
            sb.AppendLine($"<span class='akcni-karta-Nazev'>{a.Nazev}</span>");
            sb.AppendLine($"<img class='akcni-karta-obrazek obrazek' src='../../{a.ImageUrl}' />");
            sb.AppendLine($"<span class='akcni-karta-pocet'>{a.Pocet}</span>");
            sb.AppendLine("</button>");
        }
        return sb.ToString();
    }
    public string DejBodyHTML()
    {
        StringBuilder sb=new StringBuilder();
        foreach (Hrac h in hraci)
        {
            sb.AppendLine($"<p>{h.UserName}</p>");
            sb.AppendLine($"<p>{mapka.Rozcesti.Where(r => r.Hrac == h && r.Stavba != null).Sum(r => r.Stavba.Body).ToString()}</p>");
        }
        return sb.ToString();
    }

    public string DejBodoveKartyHTML(Hrac h)
    {
        StringBuilder sb = new();
        foreach (BodovaKarta a in DejStav(h).BodoveKarty)
        {
            bool zapnuto = (a.Pocet > 0);
            sb.AppendLine($"<div class='akcni-karta karta' {(zapnuto ? "" : "disabled")}>");
            sb.AppendLine($"<span class='akcni-karta-Nazev'>{a.Nazev}</span>");
            sb.AppendLine($"<img class='akcni-karta-obrazek obrazek' src='../../{a.ImageUrl}' />");
            sb.AppendLine($"<span class='akcni-karta-pocet'>{a.Pocet}</span>");
            sb.AppendLine("</div>");
        }
        return sb.ToString();
    }

    public string DejSurovinoveKartyHTML(Hrac h)
    {
        StringBuilder sb = new();
        foreach (SurovinaKarta a in DejStav(h).SurovinaKarty)
        {
            sb.AppendLine($"<div class='surovina-karta karta' disabled>");
            sb.AppendLine($"<span class='akcni-karta-Nazev'>{a.Surovina.Nazev}</span>");
            sb.AppendLine($"<img class='akcni-karta-obrazek obrazek' src='../../{a.Surovina.ImageUrl}' />");
            sb.AppendLine($"<span class='akcni-karta-pocet'>{a.Pocet}</span>");
            sb.AppendLine("</div>");
        }
        return sb.ToString();
    }
    public string DejSmenyHTML(Hrac h) => "";
    private Mapka? _mapka;
    public virtual Mapka? mapka { get => _mapka; }

    private Random kostka = new Random();


    private int HodKostkou(int pocet = 2)
    {
        int hodnotaKostek = 0;
        for (int i = 0; i < pocet; i++)
        {
            hodnotaKostek += (int)(kostka.Next() % 6) + 1;
        }
        return hodnotaKostek;
    }

    public bool JeObsazenaBarva(string b)
    {
        return stavy.Any(s => b.Equals(s.barva.Name));
    }

    internal async Task PridejHrace(Hrac h, Color barva)
    {
        var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var AktualniHra = await _dbContext.hry
                .Where(h => h.Id == Id)
                .FirstOrDefaultAsync();
            var AktualniHrac = await _dbContext.hraci
                .Where(h => h.Id == h.Id)
                .FirstOrDefaultAsync();
            StavHrace s = new StavHrace()
            {
                poradi = stavy.Count,
                hra = AktualniHra,
                barva = barva,
                hrac = AktualniHrac
            };
            await _dbContext.AddAsync(s);
            AktualniHra.stavy.Add(s);
            var suroviny = await _dbContext.suroviny.ToListAsync();

            foreach (Surovina x in suroviny)
            {
                if(x.Nazev=="Poušť")
                {
                    continue;
                }
                SurovinaKarta k = new SurovinaKarta()
                {
                    Surovina = x,
                    Pocet = 0
                };

                await _dbContext.AddAsync(k);
                s.SurovinaKarty.Add(k);
            }

            foreach (String nazev in AkcniKarta.nazvy)
            {
                s.AkcniKarty.Add(new AkcniKarta()
                {
                    Nazev = nazev,
                    Pocet = 0,
                    ImageUrl = AkcniKarta.obrazky[nazev]
                });
            }

            foreach (String nazev in BodovaKarta.nazvy)
            {
                s.BodoveKarty.Add(new BodovaKarta()
                {
                    Nazev = nazev,
                    Pocet = 0,
                    ImageUrl = BodovaKarta.obrazky[nazev]
                });
            }

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw ex;
        }
        if (stavy.Count == Hra.MaximalneHracu)
        {
            await ZacniHru();
        }
        else
        {
            await ObnovStrankuVsem();
        }
    }

    private async Task ZacniHru()
    {
        if (stavHry != StavHry.Nezacala)
        {
            return;
        }
        var transaction = await _dbContext.Database.BeginTransactionAsync();
        stavHry = StavHry.Probiha;
        for (int i = 0; i < stavy.Count; i++)
        {
            PridejAktivitu(new Aktivita()
            {
                Akce = Instrukce.StavbaVesnice,
                Hrac = DejStav(DejHrace(i)).hrac

            });
            PridejAktivitu(new Aktivita()
            {
                Akce = Instrukce.StavbaCesty,
                Hrac = DejStav(DejHrace(i)).hrac
            });
        }
        for (int i = stavy.Count-1; i >= 0; i--)
        {
            PridejAktivitu(new Aktivita()
            {
                Akce = Instrukce.StavbaVesnice,
                Hrac = DejStav(DejHrace(i)).hrac

            });
            PridejAktivitu(new Aktivita()
            {
                Akce = Instrukce.StavbaCesty,
                Hrac = DejStav(DejHrace(i)).hrac
            });
        }
        hracNaTahu = 0;
        await transaction.CommitAsync();
        await ObnovStrankuVsem();
    }

    private async Task ObnovStrankuVsem()
    {
        await Hra.HubContext.Clients.All.SendAsync("ObnovitStrankuHry", Id.ToString());
    }

    private void ZacniDalsiTah()
    {
        hracNaTahu = (hracNaTahu + 1) % stavy.Count;
        int hozene = HodKostkou();
        if (hozene == 7)
        {
            // Přesun zloděje
            PridejAktivitu(new Aktivita() { Akce = Instrukce.PresunZlodeje, Hrac = AktualniHrac() });
        }
        else
        {
            foreach (Pole p in mapka.Policka)
            {
                if (p.Cislo == hozene)
                {
                    foreach (Rozcesti r in p.Rozcesti)
                    {
                        if (r.Hrac != null && r.Stavba != null)
                        {
                            DejStav(r.Hrac).SurovinaKarty.FirstOrDefault(s => s.Surovina == p.Surovina).Pocet += r.Stavba.Zisk;
                        }
                    }
                }
            }
        }
        ObnovStrankuVsem();
    }

    public void KliknutiNaCestu(Guid idCesty, string _connectionId)
    {
        if (hracNaTahu != -1 && hraci[hracNaTahu].Id == _connectionId)
        {
            var cesta = mapka.Cesty.FirstOrDefault(c => c.Id == idCesty);
            if (cesta != null && cesta.hrac == null)
            {
                cesta.hrac = hraci[hracNaTahu];
            }
        }
    }

    public void KliknutiNaRozcesti(Guid idRozcesti, string _connectionId)
    {
        if (hracNaTahu != -1 && hraci[hracNaTahu].Id == _connectionId)
        {
            var rozcestí = mapka.Rozcesti.FirstOrDefault(r => r.Id == idRozcesti);
            if (rozcestí != null && rozcestí.Hrac == null)
            {
                // Postavte vesnici/město
                //rozcestí.PostavStavbu(stavy[hracNaTahu]);
            }
        }
    }
}
