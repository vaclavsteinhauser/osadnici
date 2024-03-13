using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace WebOsadnici.Models.HerniTridy;

/// <summary>
/// Reprezentuje stav hráče v aplikaci.
/// </summary>
public class StavHrace : HerniEntita, INotifyPropertyChanged, INotifyPropertyChanging
{
    private Hra _hra;
    private Hrac _hrac;
    private Color _barva;
    private int _poradi;
    private bool _nejdelsiCesta;
    private bool _nejvetsiVojsko;
    private int _zahranychRytiru;

    /// <summary>
    /// Reference na aktuální hru, ve které se hráč nachází.
    /// </summary>
    public virtual Hra hra
    {
        get => _hra;
        set
        {
            if (_hra != value)
            {
                OnPropertyChanging(nameof(hra));
                _hra = value;
                OnPropertyChanged(nameof(hra));
            }
        }
    }

    /// <summary>
    /// Reference na hráče.
    /// </summary>
    public virtual Hrac hrac
    {
        get => _hrac;
        set
        {
            if (_hrac != value)
            {
                OnPropertyChanging(nameof(hrac));
                _hrac = value;
                OnPropertyChanged(nameof(hrac));
            }
        }
    }

    /// <summary>
    /// Barva hráče.
    /// </summary>
    public virtual Color barva
    {
        get => _barva;
        set
        {
            if (_barva != value)
            {
                OnPropertyChanging(nameof(barva));
                _barva = value;
                OnPropertyChanged(nameof(barva));
            }
        }
    }

    /// <summary>
    /// Pořadí hráče.
    /// </summary>
    public virtual int poradi
    {
        get => _poradi;
        set
        {
            if (_poradi != value)
            {
                OnPropertyChanging(nameof(poradi));
                _poradi = value;
                OnPropertyChanged(nameof(poradi));
            }
        }
    }

    /// <summary>
    /// Jestli hráč vlastní nejdelší cestu.
    /// </summary>
    public virtual bool nejdelsiCesta
    {
        get => _nejdelsiCesta;
        set
        {
            if (_nejdelsiCesta != value)
            {
                OnPropertyChanging(nameof(nejdelsiCesta));
                _nejdelsiCesta = value;
                OnPropertyChanged(nameof(nejdelsiCesta));
            }
        }
    }

    /// <summary>
    /// Jestli hráč má největší vojsko.
    /// </summary>
    public virtual bool nejvetsiVojsko
    {
        get => _nejvetsiVojsko;
        set
        {
            if (_nejvetsiVojsko != value)
            {
                OnPropertyChanging(nameof(nejvetsiVojsko));
                _nejvetsiVojsko = value;
                OnPropertyChanged(nameof(nejvetsiVojsko));
            }
        }
    }

    /// <summary>
    /// Počet zahraných akčních karet Rytíř.
    /// </summary>
    public virtual int zahranychRytiru
    {
        get => _zahranychRytiru;
        set
        {
            if (_zahranychRytiru != value)
            {
                OnPropertyChanging(nameof(zahranychRytiru));
                _zahranychRytiru = value;
                OnPropertyChanged(nameof(zahranychRytiru));
            }
        }
    }

    /// <summary>
    /// Kolekce akčních karet hráče.
    /// </summary>
    public virtual ObservableCollection<AkcniKarta> AkcniKarty { get; set; }

    /// <summary>
    /// Kolekce surovinových karet hráče.
    /// </summary>
    public virtual ObservableCollection<SurovinaKarta> SurovinaKarty { get; set; }

    /// <summary>
    /// Kolekce bodových karet hráče.
    /// </summary>
    public virtual ObservableCollection<BodovaKarta> BodoveKarty { get; set; }

    [NotMapped]
    public IEnumerable<Stavba> Stavby { get => hra.mapka.Rozcesti.Where(r => r.Hrac != null && r.Hrac.Id == hrac.Id).Select(r => r.Stavba).Where(s => s != null); }
    [NotMapped]
    public int body { get => Stavby.Select(s => s.Body).Sum() + BodoveKarty.Select(b => b.Body).Sum(); }

    [NotMapped]
    public bool MistaProVesnici
    {
        get
        {
            if (Stavby.Count() < 2)
            {
                return true;
            }
            if (Stavby.Where(s => s.Nazev == "Vesnice").Count() >= 5)
            {
                return false;
            }
            return hra.mapka.Rozcesti.Where(r => (r.Hrac == null || r.Hrac.Id == hrac.Id)
                                                && r.Stavba == null
                                                && r.Blokovane == false
                                                && r.Cesty.Any(c => c.hrac != null && c.hrac.Id == hrac.Id))
            .Count() > 0;
        }
    }

    [NotMapped]
    public bool MistaProMesto
    {
        get
        {
            if (Stavby.Where(s => s.Nazev == "Město").Count() >= 4)
            {
                return false;
            }
            return Stavby.Where(s => s.Nazev == "Vesnice").Count() > 0;
        }
    }

    [NotMapped]
    public bool MistaProCestu
    {
        get
        {
            if (hra.mapka.Cesty.Where(c => c.hrac != null && c.hrac.Id == hrac.Id).Count() >= 60)
            {
                return false;
            }
            return hra.mapka.Cesty.Where(c => c.hrac == null
                                    && c.rozcesti.Any(r => (r.Hrac != null && r.Hrac.Id == hrac.Id)
                                                        || r.Cesty.Any(x => x.hrac != null && x.hrac.Id == hrac.Id))).Count() > 0;
        }
    }

    [NotMapped]
    public IEnumerable<Rozcesti> vlastnenaRozcesti { get => hra.mapka.Rozcesti.Where(r => r.Hrac != null && r.Hrac.Id == hrac.Id); }

    [NotMapped]
    public IEnumerable<Cesta> vlastneneCesty { get => hra.mapka.Cesty.Where(c => c.hrac != null && c.hrac.Id == hrac.Id); }

    [NotMapped]
    public int nejdelsiVlastnenaCesta = 0;
    private HashSet<Cesta> navstivene;
    private List<Cesta> nejdelsiNalezenaCesta;
    public int NejdelsiCesta()
    {
        navstivene = new HashSet<Cesta>();
        nejdelsiNalezenaCesta = new List<Cesta>();
        foreach (var startCesta in vlastneneCesty)
        {
            navstivene.Clear();
            foreach (Rozcesti r in startCesta.rozcesti)
            {
                DFS(new List<Cesta>(), startCesta, r);
            }
        }

        return nejdelsiNalezenaCesta.Count();

    }

    private void DFS(List<Cesta> aktualniCesta, Cesta nova, Rozcesti minuleRozcesti)
    {
        if (aktualniCesta.Count > nejdelsiNalezenaCesta.Count)
        {
            nejdelsiNalezenaCesta = new List<Cesta>(aktualniCesta);
        }

        navstivene.Add(nova);
        Rozcesti konecneRozcesti = nova.rozcesti.Where(r => r != minuleRozcesti).First();
        foreach (var navazujiciCesta in vlastneneCesty.Where(c => !navstivene.Contains(c) && c.rozcesti.Contains(konecneRozcesti)))
        {
            if (navazujiciCesta != null)
            {
                aktualniCesta.Add(navazujiciCesta);
                DFS(aktualniCesta, navazujiciCesta, konecneRozcesti);
                aktualniCesta.Remove(navazujiciCesta);
            }
        }

        navstivene.Remove(nova);
    }
}