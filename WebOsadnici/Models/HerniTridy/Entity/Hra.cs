using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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
    public ApplicationDbContext _dbContext;
    public static IHubContext<ClickHub> HubContext { get; set; }
    static public readonly int MaximalneHracu = 4;


    public async Task Inicializace()
    {
        _mapka = new Mapka();
        await _mapka.Inicializace(this, _dbContext);
        for (int i = 0; i < AkcniKarta.nazvy.Length; i++)
        {
            NerozdaneAkcniKarty.Add(new AkcniKarta()
            {
                Nazev = AkcniKarta.nazvy[i],
                Pocet = AkcniKarta.pocty[i],
                ImageUrl = AkcniKarta.obrazky[AkcniKarta.nazvy[i]]
            });
        }
        NerozdaneBodoveKarty.Add(new BodovaKarta()
        {
            Nazev = "Akční body",
            ImageUrl = BodovaKarta.obrazky["Akční body"],
            Body = 1,
            Pocet = 5
        });
        _dbContext.Add(this);
        await _dbContext.SaveChangesAsync();
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
            hra = NacteneHry.FirstOrDefault(h => h.Id == hraId)
                                            ;
        }
        if (hra != null)
        {
            hra.PosledniPouziti = DateTime.Now;
            return hra;
        }
        var hraQuery = context.hry
    .Where(h => h.Id == hraId)
    .Include(h => h.stavy)
        .ThenInclude(s => s.SurovinaKarty)
    .Include(h => h.stavy)
        .ThenInclude(s => s.AkcniKarty)
    .Include(h => h.mapka)
        .ThenInclude(m => m.Policka)
            .ThenInclude(p => p.Surovina);

        var mapkaQuery = context.mapky
            .Where(m => m.HraId == hraId)
            .Include(m => m.Policka)
                .ThenInclude(p => p.Rozcesti)
            .Include(m => m.Cesty)
                .ThenInclude(c => c.rozcesti);

        hra = await hraQuery.FirstOrDefaultAsync();
        var mapka = await mapkaQuery.FirstOrDefaultAsync();

        if (hra != null && mapka != null)
        {
            hra.mapka = mapka;
        }
        if (hra != null)
        {
            lock (NacteneHry)
            {
                if (!NacteneHry.Any(h => h != null && h.Id == hra.Id))
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

    public virtual ObservableCollection<StavHrace> stavy { get; set; } = new();
    public virtual ObservableCollection<AkcniKarta> NerozdaneAkcniKarty { get; set; } = new();
    public virtual ObservableCollection<BodovaKarta> NerozdaneBodoveKarty { get; set; } = new();

    [NotMapped]
    public IEnumerable<AkcniKarta> NerozdaneKarty { get => NerozdaneAkcniKarty.Concat(NerozdaneBodoveKarty); }
    [NotMapped]
    public int pocetNerozdanychKaret { get => NerozdaneKarty.Sum(k => k.Pocet); }

    public virtual ObservableCollection<Aktivita> bufferAktivit { get; set; } = new();

    public virtual ObservableCollection<Smena> aktivniSmeny { get; set; } = new();

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
    private int _hozene = 0;
    public virtual int Hozene
    {
        get => _hozene;
        set
        {
            if (_hozene != value)
            {
                OnPropertyChanging(nameof(Hozene));
                _hozene = value;
                OnPropertyChanged(nameof(Hozene));
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
        int i;
        if (bufferAktivit.Count() > 0)
            i = bufferAktivit.Select(x => x.CisloAktivity).Max() + 1;
        else
            i = 1;
        a.CisloAktivity = i;
        bufferAktivit.Add(a);
        _dbContext.Add(a);
    }
    public void SmazAktivitu(Aktivita a)
    {
        bufferAktivit.Remove(a);
        _dbContext.Remove(a);
    }
    public void PridejSmenu(Smena s)
    {
        aktivniSmeny.Add(s);
        _dbContext.Add(s);
        foreach (var surovina in s.nabidka)
        {
            _dbContext.surovinakarty.Add(surovina);
        }

        // Přidání každé suroviny poptávky do kontextu databáze
        foreach (var surovina in s.poptavka)
        {
            _dbContext.surovinakarty.Add(surovina);
        }

        // Uložení změn v databázi
        _dbContext.SaveChanges();
    }
    public void SmazSmenu(Smena s)
    {
        aktivniSmeny.Remove(s);
        _dbContext.Remove(s);
        foreach (var surovina in s.nabidka)
        {
            _dbContext.surovinakarty.Remove(surovina);
        }

        // Odstranění každé suroviny poptávky z kontextu databáze
        foreach (var surovina in s.poptavka)
        {
            _dbContext.surovinakarty.Remove(surovina);
        }

        // Uložení změn v databázi
        _dbContext.SaveChanges();
    }

    public void DejAkcniKartu(Hrac h)
    {
        List<AkcniKarta> vyber = new();
        foreach (AkcniKarta k in NerozdaneKarty)
        {
            for (int i = 0; i < k.Pocet; i++)
            {
                vyber.Add(k);
            }
        }
        var karta = vyber[kostka.Next() % vyber.Count];

        if (karta is BodovaKarta)
        {
            var k = DejStav(h).BodoveKarty.FirstOrDefault(b => b.Nazev.Equals(karta.Nazev));
            if (k == null)
            {
                k = new BodovaKarta()
                {
                    Nazev = karta.Nazev,
                    ImageUrl = karta.ImageUrl,
                    Body = 1
                };
                DejStav(h).BodoveKarty.Add(k);
            }
            else
            {
                k.Body++;
            }
        }
        else
        {
            var k = DejStav(h).AkcniKarty.FirstOrDefault(b => b.Nazev.Equals(karta.Nazev));
            if (k == null)
            {
                k = new AkcniKarta()
                {
                    Nazev = karta.Nazev,
                    ImageUrl = karta.ImageUrl,
                    Pocet = 1
                };
                DejStav(h).AkcniKarty.Add(k);
            }
            else
            {
                k.Pocet++;
            }
        }
        karta.Pocet--;
    }
    public Surovina SeberSurovinu(Hrac h)
    {
        var s = DejStav(h).SurovinaKarty.Where(s => s.Pocet > 0).ToList();
        if (s.Count() == 0)
            return null;
        List<SurovinaKarta> vybirane = new();
        foreach (var sk in s)
        {
            for (int i = 0; i < sk.Pocet; i++)
            {
                vybirane.Add(sk);
            }
        }
        var surovina = vybirane[kostka.Next() % vybirane.Count()];
        surovina.Pocet--;
        return surovina.Surovina;
    }
    public StavHrace DejStav(Hrac h)
    {
        return stavy.FirstOrDefault(s => s.hrac.Id == h.Id);
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
                return "Čekej na začátek hry";
            }
        }
        else if (stavHry == StavHry.Probiha)
        {
            if (AktualniHrac() == h)
            {
                var a = AktualniAktivita();
                if (a != null)
                {

                    switch (AktualniAktivita().Akce)
                    {
                        case Instrukce.StavbaVesnice:
                            return "Vyber místo pro stavbu vesnice.";
                        case Instrukce.StavbaCesty:
                            return "Vyber místo pro stavbu cesty.";
                        case Instrukce.StavbaMesta:
                            return "Vyber místo pro stavbu města.";
                        case Instrukce.PresunZlodeje:
                            return "Vyber políčko, kam se má přesunout zloděj.";
                        case Instrukce.VyberPrvniSurovinu:
                            return "Vyber surovinu, kterou chceš získat.";
                        case Instrukce.VyberDruhouSurovinu:
                            return "Vyber druhou surovinu, kterou chceš získat.";
                        case Instrukce.VyberHrace:
                            return "Vyber hráče(vpravo nahoře), který sousedí s políčkem, kam přijde zloděj a chceš mu sebrat surovinu.";
                        default:
                            return "";
                    }
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
        foreach (Stavba s in mapka.Stavby)
        {
            bool zapnute = false;
            if (s.Cena.All(sc => DejStav(hrac).SurovinaKarty.Any(sk => sk.Surovina == sc.Surovina && sk.Pocet >= sc.Pocet)))
            {
                zapnute = true;
            }
            if (s.Nazev == "Vesnice" && !DejStav(hrac).MistaProVesnici)
            {
                zapnute = false;
            }
            else if (s.Nazev == "Město" && !DejStav(hrac).MistaProMesto)
            {
                zapnute = false;
            }
            else if (s.Nazev == "Cesta" && !DejStav(hrac).MistaProCestu)
            {
                zapnute = false;
            }
            else if (s.Nazev == "Akční karta" && pocetNerozdanychKaret == 0)
            {
                zapnute = false;
            }
            if (AktualniHrac() == null || hrac.Id != AktualniHrac().Id)
                zapnute = false;
            sb.AppendLine($"<button class='stavba' id='{s.Id}' onclick='klik_nakup(event)' {(zapnute ? "" : "disabled")}>");
            sb.AppendLine("<span class='stavba-nazev'>");
            sb.AppendLine(s.Nazev);
            sb.AppendLine("</span>");
            sb.AppendLine("<span class='stavba-cena'>");
            foreach (SurovinaKarta sc in s.Cena)
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
        foreach (AkcniKarta a in DejStav(h).AkcniKarty)
        {
            bool zapnuto = (a.Pocet > 0);
            sb.AppendLine($"<button id='{a.Id}' onclick='klik_akcni_karta(event)' class='akcni-karta karta' {(zapnuto ? "" : "disabled")}>");
            sb.AppendLine($"<span class='akcni-karta-Nazev' style='pointer-events: none;' >{a.Nazev}</span>");
            sb.AppendLine($"<img class='akcni-karta-obrazek obrazek' src='../../{a.ImageUrl}' style='pointer-events: none;' />");
            sb.AppendLine($"<span class='akcni-karta-pocet' style='pointer-events: none;' >{a.Pocet}</span>");
            sb.AppendLine("</button>");
        }
        return sb.ToString();
    }
    public string DejTlacitkaHTML(Hrac h)
    {
        StringBuilder sb = new();
        if (stavHry != StavHry.Nezacala)
        {
            if (AktualniHrac().Id == h.Id)
            {
                sb.AppendLine("<button class='button' id='tlacitko_smena' onclick='tlacitko_smena(event)'=> Nabídnout směnu </button>");
                sb.AppendLine("<button class='button' id='tlacitko_dalsi' onclick='dalsi_tah(event)'> Ukončit tah </button>");
            }
            else
            {
                sb.AppendLine("<button id='tlacitko_smena' disabled> Nabídnout směnu </button>");
                sb.AppendLine("<button id='tlacitko_dalsi' disabled> Ukončit tah </button>");
            }

        }
        else
        {
            sb.AppendLine("<button id='tlacitko_smena' disabled> Nabídnout směnu </button>");
            if (hraci.Count > 1 && h.Id == DejHrace(0).Id)
            {
                sb.AppendLine("<button class='button' id='tlacitko_dalsi' onclick='dalsi_tah(event)'> Začít hru </button>");
            }
            else
            {
                sb.AppendLine("<button id='tlacitko_dalsi' disabled> Začít hru </button>");
            }
        }
        return sb.ToString();
    }
    public string DejBodyHTML()
    {
        StringBuilder sb = new StringBuilder();
        foreach (Hrac h in hraci)
        {
            string tucne = (AktualniHrac() != null && h.Id == AktualniHrac().Id) ? "font-weight: bold;" : "";
            StavHrace s = DejStav(h);
            sb.AppendLine($"<div id='{h.Id}' class='body-hrace' style='color: {s.barva.Name}; {tucne}' onclick='klik_hrac(event)'>");
            sb.AppendLine($"<p style='pointer-events: none;'>{h.UserName}</p>");
            sb.AppendLine($"<p style='pointer-events: none;'>{s.body}</p>");
            sb.AppendLine("</div>");
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
            sb.AppendLine($"<span class='akcni-karta-pocet'>{a.Body}</span>");
            sb.AppendLine("</div>");
        }
        return sb.ToString();
    }

    public string DejSurovinoveKartyHTML(Hrac h)
    {
        StringBuilder sb = new();
        foreach (SurovinaKarta a in DejStav(h).SurovinaKarty)
        {
            sb.AppendLine($"<div id='{a.Id}' class='surovina-karta karta' onclick ='klik_surovina(event)'>");
            sb.AppendLine($"<span class='akcni-karta-Nazev' style='pointer-events: none;' >{a.Surovina.Nazev}</span>");
            sb.AppendLine($"<img class='akcni-karta-obrazek obrazek' src='../../{a.Surovina.ImageUrl}' style='pointer-events: none;' />");
            sb.AppendLine($"<span class='akcni-karta-pocet' style='pointer-events: none;'>{a.Pocet}</span>");
            sb.AppendLine("</div>");
        }
        return sb.ToString();
    }
    public string DejSmenyHTML(Hrac h)
    {

        StringBuilder sb = new StringBuilder();
        sb.Append("<table border='1'>");
        sb.Append("<tr><th>Nabídka</th><th>Poptávka</th><th>Hráč</th></tr>");

        foreach (var smena in aktivniSmeny.Where(a => a.hrac != null && a.hrac.Id != h.Id))
        {
            sb.Append($"<tr id='{smena.Id}' onclick='provedeni_smeny(\"{smena.Id}\")'>");
            sb.Append("<td style='{display: flex; flex-direction: column;}'>");
            foreach (var surovinaKarta in smena.nabidka)
            {
                sb.Append($"<span>{surovinaKarta}</span>");
            }
            sb.Append("</td>");

            sb.Append("<td style='{display: flex; flex-direction: column;}'>");
            foreach (var surovinaKarta in smena.poptavka)
            {
                sb.Append($"<span>{surovinaKarta}</span>");
            }
            sb.Append("</td>");

            sb.Append($"<td>{smena.hrac}</td>");

            sb.Append("</tr>");
        }

        sb.Append("</table>");

        return sb.ToString();

    }
    public string DejSmenuHraHTML(Hrac h)
    {
        StavHrace s = DejStav(h);

        var vstupniSurovina = s.SurovinaKarty
            .Where(s => s.Pocet >= 4)
            .Select(s => s.Surovina.Nazev)
            .ToList();

        var vystupniSurovina = mapka.Suroviny
            .Where(s => s.Nazev != "Poušť")
            .Select(s => s.Nazev)
            .ToList();


        var sb = new StringBuilder();
        if (vstupniSurovina.Count() > 0)
        {
            sb.AppendLine("<form id='smena-s-hrou-form'>");
            sb.AppendLine("<label for='smena_hra_vstup'>Vyberte nabízené suroviny:</label>");
            sb.AppendLine("<select id='smena_hra_vstup' name='vstup'>");
            foreach (var surovina in vstupniSurovina)
            {
                sb.AppendLine($"<option value='{surovina}'>{surovina}</option>");
            }
            sb.AppendLine("</select>");
            sb.AppendLine("<label for='smena_hra_vystup'>Vyberte požadované suroviny:</label>");
            sb.AppendLine("<select id='smena_hra_vystup' name='vystup'>");
            foreach (var surovina in vystupniSurovina)
            {
                sb.AppendLine($"<option value='{surovina}'>{surovina}</option>");
            }
            sb.AppendLine("</select>");

            sb.AppendLine("<p class='button' id='vytvorit_smenu_hra' onclick='vytvorit_smenu_hra(event)'> Směnit </p>");
            sb.AppendLine("</form>");
        }
        else
        {
            sb.AppendLine("<p> Nemáte dostatek surovin (4 od stejného druhu) pro směnu</p>");
        }
        return sb.ToString();

    }
    public string DejSmenuHraciHTML(Hrac h)
    {
        StavHrace s = DejStav(h);

        var surovinyHrace = s.SurovinaKarty
                .ToDictionary(s => s.Surovina.Nazev, s => s.Pocet);

        var vystupniSuroviny = mapka.Suroviny
            .Where(s => s.Nazev != "Poušť")
            .Select(s => s.Nazev)
            .ToList();

        var sb = new StringBuilder();
        sb.AppendLine("<form id=\"smena-s-hraci-form\">");
        sb.AppendLine("<span id=\"smena-s-hraci-form-rozdeleni\">");
        sb.AppendLine("<span>");
        // Vstupní suroviny hráče
        sb.AppendLine("<h4>Vyberte nabízené (tvoje) suroviny:</h4>");
        foreach (var surovina in surovinyHrace)
        {
            string zapnuto = (surovina.Value == 0) ? "disabled" : "";
            sb.AppendLine($"<label for='vstup-{surovina.Key}'>{surovina.Key}:</label>");
            sb.AppendLine($"<input type='number' id='vystup-{surovina.Key}' name='vstup-{surovina.Key}' min='0' max='{surovina.Value}' step='1' value='0' size='3' {zapnuto}><br>");
        }
        sb.AppendLine("</span>");
        sb.AppendLine("<span>");
        sb.AppendLine("<h4>Vyberte poptávané (cizí) suroviny:</h4>");
        foreach (var surovina in vystupniSuroviny)
        {
            sb.AppendLine($"<label for='vystup-{surovina}'>{surovina}:</label>");
            sb.AppendLine($"<input type='number' id='vystup-{surovina}' name='vystup-{surovina}' min='0' max='99' step='1' value='0' size='3' ><br>");
        }
        sb.AppendLine("</span>");
        sb.AppendLine("</span>");
        sb.AppendLine("<p class='button' id='vytvorit_smenu_hraci' onclick='vytvorit_smenu_hraci(event)'> Vytvořit nabídku</p>");

        sb.AppendLine("</form>");

        return sb.ToString();
    }
    private Mapka? _mapka;
    public virtual Mapka? mapka { get => _mapka; set => _mapka = value; }

    public Random kostka = new Random();

    public void PrepocitejNejdelsiCestu()
    {
        foreach (StavHrace s in stavy)
        {
            s.nejdelsiVlastnenaCesta = s.NejdelsiCesta();
        }
        int nejdelsicesta = stavy.Max(s => s.nejdelsiVlastnenaCesta);
        if (nejdelsicesta <= 3)
        {
            return;
        }
        IEnumerable<StavHrace> nejdelsi = stavy.Where(x => x.nejdelsiVlastnenaCesta == nejdelsicesta);
        if (nejdelsi.Count() == 1)
        {
            nejdelsi.First().nejdelsiCesta = true;
            nejdelsi.First().BodoveKarty.FirstOrDefault(b => b.Nazev == "Nejdelší cesta").Body = 2;
            foreach (StavHrace x in stavy.Where(x => x != nejdelsi.First()))
            {
                x.nejdelsiCesta = false;
                x.BodoveKarty.FirstOrDefault(b => b.Nazev == "Nejdelší cesta").Body = 0;
            }
        }
    }
    public void PrepocitejNejvicRytiru()
    {
        var max = stavy.Select(s => s.zahranychRytiru).Max();
        if (max < 3) return;
        IEnumerable<StavHrace> nejvic = stavy.Where(s => s.zahranychRytiru == max);
        if (nejvic.Count() == 1)
        {
            foreach (StavHrace x in stavy.Where(x => x != nejvic.First()))
            {
                x.nejvetsiVojsko = false;
                x.BodoveKarty.FirstOrDefault(b => b.Nazev == "Největší vojsko").Body = 0;
            }
            nejvic.First().nejvetsiVojsko = true;
            nejvic.First().BodoveKarty.FirstOrDefault(b => b.Nazev == "Největší vojsko").Body = 2;

        }
    }
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
                .Where(r => r.Id == h.Id)
                .FirstOrDefaultAsync();
            StavHrace s = new StavHrace()
            {
                poradi = stavy.Count,
                hra = AktualniHra,
                barva = barva,
                hrac = AktualniHrac
            };
            stavy.Add(s);
            await _dbContext.AddAsync(s);

            var suroviny = await _dbContext.suroviny.ToListAsync();

            foreach (Surovina x in suroviny)
            {
                if (x.Nazev == "Poušť")
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

    public async Task ZacniHru()
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
        for (int i = stavy.Count - 1; i >= 0; i--)
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
        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();
        await ObnovStrankuVsem();
    }

    private async Task ObnovStrankuVsem()
    {
        await Hra.HubContext.Clients.All.SendAsync("ObnovitStrankuHry", Id.ToString());
    }

    public async Task DalsiHrac()
    {
        if (stavy.Any(s => s.body >= 10))
        {
            stavHry = StavHry.Skoncila;
            await _dbContext.SaveChangesAsync();
            await ObnovStrankuVsem();
            return;
        }
        var SmenyKeKontrole = aktivniSmeny.Where(s => s.hrac.Id == DejHrace(hracNaTahu).Id);
        foreach (Smena s in SmenyKeKontrole)
        {
            if (s.nabidka.Any(n => n.Pocet > DejStav(DejHrace(hracNaTahu)).SurovinaKarty.FirstOrDefault(sk => sk.Surovina.Nazev == n.Surovina.Nazev).Pocet))
                SmazSmenu(s);
        }
        hracNaTahu = (hracNaTahu + 1) % stavy.Count;
        var SmenyKeSmazani = aktivniSmeny.Where(s => s.hrac.Id == DejHrace(hracNaTahu).Id);
        foreach (Smena s in SmenyKeSmazani)
        {
            SmazSmenu(s);
        }
        Hozene = HodKostkou();
        if (Hozene == 7)
        {
            PridejAktivitu(new Aktivita() { Akce = Instrukce.PresunZlodeje, Hrac = AktualniHrac() });
        }
        else
        {
            foreach (Pole p in mapka.Policka)
            {
                if (!p.Blokovane && p.Cislo == Hozene)
                {
                    foreach (Rozcesti r in p.Rozcesti)
                    {
                        if (r != null && r.Hrac != null && r.Stavba != null)
                        {
                            DejStav(r.Hrac).SurovinaKarty.FirstOrDefault(s => s.Surovina == p.Surovina).Pocet += r.Stavba.Zisk;
                        }
                    }
                }
            }
        }
        await _dbContext.SaveChangesAsync();
        await ObnovStrankuVsem();
    }


}
