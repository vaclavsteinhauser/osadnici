using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using WebOsadnici.Controllers;
using WebOsadnici.Models.HerniTridy;

/// <summary>
/// Reprezentuje stav hráče v aplikaci.
/// </summary>
public class StavHrace : HerniEntita, INotifyPropertyChanged, INotifyPropertyChanging
{
    private Hra _hra;
    private Hrac _hrac;
    private Color _barva;
    private int _poradi;
    private ObservableCollection<AkcniKarta> _akcniKarty = new();
    private ObservableCollection<SurovinaKarta> _surovinaKarty = new();

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
    /// Kolekce akčních karet hráče.
    /// </summary>
    public virtual ObservableCollection<AkcniKarta> AkcniKarty
    {
        get => _akcniKarty;
        set
        {
            if (_akcniKarty != value)
            {
                OnPropertyChanging(nameof(AkcniKarty));
                _akcniKarty = value;
                OnPropertyChanged(nameof(AkcniKarty));
            }
        }
    }

    /// <summary>
    /// Kolekce surovinových karet hráče.
    /// </summary>
    public virtual ObservableCollection<SurovinaKarta> SurovinaKarty
    {
        get => _surovinaKarty;
        set
        {
            if (_surovinaKarty != value)
            {
                OnPropertyChanging(nameof(SurovinaKarty));
                _surovinaKarty = value;
                OnPropertyChanged(nameof(SurovinaKarty));
            }
        }
    }

    /// <summary>
    /// Zjistí, zda je hráč aktuálně na tahu.
    /// </summary>
    /// <returns>True, pokud je hráč na tahu, jinak false.</returns>
    public bool JeNaTahu()
    {
        return hra.hracNaTahu == poradi;
    }
}