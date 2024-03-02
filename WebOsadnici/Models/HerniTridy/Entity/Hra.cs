using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebOsadnici.Controllers;
using WebOsadnici.Data;
using WebOsadnici.Hubs;
using WebOsadnici.Models.HerniTridy;

public enum StavHry { Vytvorena, Rozestavovani, Probiha, Skoncila }

/// <summary>
/// Třída reprezentující hru.
/// </summary>
public class Hra : HerniEntita
{
    public static IHubContext<ClickHub> HubContext { get; set; }
    static public readonly int MaximalneHracu = 4;
    static private List<Hra> NacteneHry;


    public async Task Inicializace(ApplicationDbContext _dbContext)
    {
        mapka = new Mapka();
        await mapka.Inicializace(this, _dbContext);
    }

    public static async Task<Hra> NactiHru(Guid hraId, ApplicationDbContext context)
    {
        var hra = await context.hry
            .Where(h => h.Id == hraId)
            .Include(h => h.hraci) // Hráči
            .Include(h => h.stavy)
                .ThenInclude(s => s.hrac) // Hráči stavů
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

        return hra;
    }


    public static IQueryable<Hra> NactiNahledyHer(ApplicationDbContext context)
    {
        return context.hry.Include(h => h.hraci);
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

    private ObservableCollection<Hrac> _hraci = new();
    public virtual ObservableCollection<Hrac> hraci
    {
        get => _hraci;
        set
        {
            if (_hraci != value)
            {
                OnPropertyChanging(nameof(hraci));
                _hraci = value;
                OnPropertyChanged(nameof(hraci));
            }
        }
    }

    private ObservableCollection<StavHrace> _stavy = new();
    public virtual ObservableCollection<StavHrace> stavy
    {
        get => _stavy;
        set
        {
            if (_stavy != value)
            {
                OnPropertyChanging(nameof(stavy));
                _stavy = value;
                OnPropertyChanged(nameof(stavy));
            }
        }
    }

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

    public Hrac AktualniHrac()
    {
        var stav = stavy.FirstOrDefault(s => s.JeNaTahu());
        return stav?.hrac;
    }

    public StavHrace StavProHrace(Hrac h)
    {
        return stavy.FirstOrDefault(s => s.hrac == h);
    }

    public virtual Mapka? mapka { get; set; }

    private Random kostka = new Random();
    int hodnotaKostek;

    private void HodKostkou(int pocet = 2)
    {
        hodnotaKostek = 0;
        for (int i = 0; i < pocet; i++)
        {
            hodnotaKostek += (int)(kostka.Next() % 6) + 1;
        }
    }

    public bool JeObsazenaBarva(string b)
    {
        return stavy.Any(s => b.Equals(s.barva.Name));
    }

    internal async Task PridejHrace(Hrac h, Color barva, ApplicationDbContext _dbContext)
    {
        Hrac hrac = await _dbContext.hraci.FindAsync(h.Id);
        Hra aktualniHra = await _dbContext.hry.FindAsync(this.Id);

        hraci.Add(hrac);
        if (!aktualniHra.hraci.Contains(hrac))
        {
            aktualniHra.hraci.Add(hrac);
        }

        StavHrace s = new StavHrace()
        {
            poradi = stavy.Count,
            hra = aktualniHra,
            barva = barva,
            hrac = hrac
        };
        stavy.Add(s);
        if (!aktualniHra.stavy.Contains(s))
        {
            aktualniHra.stavy.Add(s);
        }
        await _dbContext.AddAsync(s);

        var suroviny = await _dbContext.suroviny.ToListAsync();

        foreach (Surovina x in suroviny)
        {
            SurovinaKarta k = new SurovinaKarta()
            {
                Surovina = x,
                Pocet = 0
            };

            await _dbContext.AddAsync(k);
            s.SurovinaKarty.Add(k);
        }
        await _dbContext.SaveChangesAsync();

        if (hraci.Count == Hra.MaximalneHracu)
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
        stavHry = StavHry.Rozestavovani;
        hracNaTahu = 0;
        await ObnovStrankuVsem();
    }

    private async Task ObnovStrankuVsem()
    {
        await Hra.HubContext.Clients.All.SendAsync("ObnovitStrankuHry", Id.ToString());
    }

    private void ZacniDalsiTah()
    {
        hracNaTahu = hracNaTahu % stavy.Count + 1;
        HodKostkou();
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
