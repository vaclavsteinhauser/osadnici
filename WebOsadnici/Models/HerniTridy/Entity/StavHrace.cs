using System.Collections.ObjectModel;
using System.ComponentModel;
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

    /// <summary>
    /// Zjistí, zda je hráč aktuálně na tahu.
    /// </summary>
    /// <returns>True, pokud je hráč na tahu, jinak false.</returns>
    public bool JeNaTahu()
    {
        return hra.hracNaTahu == poradi;
    }
}