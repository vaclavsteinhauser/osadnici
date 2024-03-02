using System.Collections.ObjectModel;
using System.Drawing;
using WebOsadnici.Models.HerniTridy;

/// <summary>
/// Reprezentuje herní pole v aplikaci.
/// </summary>
public class Pole : HerniEntita
{
    // Velikost herního pole
    public static readonly Size Velikost = new Size(4, 6);

    private int _poziceX;
    private int _poziceY;
    private Surovina? _surovina;
    private int _cislo;
    private bool _blokovane;
    private ObservableCollection<Rozcesti>? _rozcesti = new ObservableCollection<Rozcesti>() { null, null, null, null, null, null };

    /// <summary>
    /// X-ová pozice pole.
    /// </summary>
    public virtual int PoziceX
    {
        get => _poziceX;
        set
        {
            if (_poziceX != value)
            {
                OnPropertyChanging(nameof(PoziceX));
                _poziceX = value;
                OnPropertyChanged(nameof(PoziceX));
            }
        }
    }

    /// <summary>
    /// Y-ová pozice pole.
    /// </summary>
    public virtual int PoziceY
    {
        get => _poziceY;
        set
        {
            if (_poziceY != value)
            {
                OnPropertyChanging(nameof(PoziceY));
                _poziceY = value;
                OnPropertyChanged(nameof(PoziceY));
            }
        }
    }

    /// <summary>
    /// Surovina přiřazená k poli (pokud je).
    /// </summary>
    public virtual Surovina? Surovina
    {
        get => _surovina;
        set
        {
            if (_surovina != value)
            {
                OnPropertyChanging(nameof(Surovina));
                _surovina = value;
                OnPropertyChanged(nameof(Surovina));
            }
        }
    }

    /// <summary>
    /// Číslo pole.
    /// </summary>
    internal virtual int Cislo
    {
        get => _cislo;
        set
        {
            if (_cislo != value)
            {
                OnPropertyChanging(nameof(Cislo));
                _cislo = value;
                OnPropertyChanged(nameof(Cislo));
            }
        }
    }

    /// <summary>
    /// Určuje, zda je pole blokováno.
    /// </summary>
    internal virtual bool Blokovane
    {
        get => _blokovane;
        set
        {
            if (_blokovane != value)
            {
                OnPropertyChanging(nameof(Blokovane));
                _blokovane = value;
                OnPropertyChanged(nameof(Blokovane));
            }
        }
    }

    /// <summary>
    /// Kolekce sousedních rozcestí.
    /// </summary>
    internal virtual ObservableCollection<Rozcesti>? Rozcesti
    {
        get => _rozcesti;
        set
        {
            if (_rozcesti != value)
            {
                OnPropertyChanging(nameof(Rozcesti));
                _rozcesti = value;
                OnPropertyChanged(nameof(Rozcesti));
            }
        }
    }

    /// <summary>
    /// Konstruktor pro vytvoření instance pole se zadanými vlastnostmi.
    /// </summary>
    /// <param name="surovina">Přiřazená surovina.</param>
    /// <param name="cislo">Číslo pole.</param>
    /// <param name="sloupec">X-ová pozice pole.</param>
    /// <param name="radek">Y-ová pozice pole.</param>
    /// <param name="blokovane">Určuje, zda je pole blokováno.</param>
    public Pole(Surovina surovina, int cislo, int sloupec, int radek, bool blokovane = false)
    {
        Surovina = surovina;
        Cislo = cislo;
        PoziceX = sloupec;
        PoziceY = radek;
        Blokovane = blokovane;
    }
    public Pole() { }
}
