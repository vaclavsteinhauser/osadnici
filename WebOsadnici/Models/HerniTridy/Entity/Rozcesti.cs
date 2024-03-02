using System.Drawing;
using WebOsadnici.Models.HerniTridy;

/// <summary>
/// Reprezentuje rozcestí v aplikaci.
/// </summary>
public class Rozcesti : HerniEntita
{
    // Velikost určuje průměr kruhu
    internal readonly static Size Velikost = new Size(2, 2);

    private int _poziceX;
    private int _poziceY;
    private Hrac? _hrac;
    private bool _blokovane;
    private Stavba? _stavba;

    /// <summary>
    /// X-ová pozice rozcestí.
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
    /// Y-ová pozice rozcestí.
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
    /// Hráč přiřazený k rozcestí (pokud je).
    /// </summary>
    public virtual Hrac? Hrac
    {
        get => _hrac;
        set
        {
            if (_hrac != value)
            {
                OnPropertyChanging(nameof(Hrac));
                _hrac = value;
                OnPropertyChanged(nameof(Hrac));
            }
        }
    }

    /// <summary>
    /// Určuje, zda je rozcestí blokováno.
    /// </summary>
    public virtual bool Blokovane
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
    /// Stavba přiřazená k rozcestí (pokud je).
    /// </summary>
    internal virtual Stavba? Stavba
    {
        get => _stavba;
        set
        {
            if (_stavba != value)
            {
                OnPropertyChanging(nameof(Stavba));
                _stavba = value;
                OnPropertyChanged(nameof(Stavba));
            }
        }
    }

    /// <summary>
    /// Konstruktor pro vytvoření instance rozcestí se zadanými souřadnicemi.
    /// </summary>
    /// <param name="poziceX">X-ová pozice rozcestí.</param>
    /// <param name="poziceY">Y-ová pozice rozcestí.</param>
    internal Rozcesti(int poziceX, int poziceY)
    {
        PoziceX = poziceX;
        PoziceY = poziceY;
    }
    public Rozcesti() { }
}