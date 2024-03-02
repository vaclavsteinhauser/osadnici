using System.Collections.ObjectModel;
using System.Drawing;
using WebOsadnici.Models.HerniTridy;

public class Cesta : HerniEntita
{
    internal static readonly Size velikost = new Size(2, 2);

    // Pozice X cesty na herní mapě
    private int _poziceX;
    internal virtual int poziceX
    {
        get => _poziceX;
        set
        {
            if (_poziceX != value)
            {
                OnPropertyChanging(nameof(poziceX));
                _poziceX = value;
                OnPropertyChanged(nameof(poziceX));
            }
        }
    }

    // Pozice Y cesty na herní mapě
    private int _poziceY;
    internal virtual int poziceY
    {
        get => _poziceY;
        set
        {
            if (_poziceY != value)
            {
                OnPropertyChanging(nameof(poziceY));
                _poziceY = value;
                OnPropertyChanged(nameof(poziceY));
            }
        }
    }

    // Hráč vlastnící cestu
    private Hrac? _hrac;
    public virtual Hrac? hrac
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

    // Seznam rozcestí připojených k této cestě
    private ObservableCollection<Rozcesti>? _rozcesti = new();
    public virtual ObservableCollection<Rozcesti>? rozcesti
    {
        get => _rozcesti;
        set
        {
            if (_rozcesti != value)
            {
                OnPropertyChanging(nameof(rozcesti));
                _rozcesti = value;
                OnPropertyChanged(nameof(rozcesti));
            }
        }
    }

    // Natáčení cesty při vykreslování
    // 0 - |
    // 1 - /
    // 2 - \
    private int _natoceni;
    internal virtual int natoceni
    {
        get => _natoceni;
        set
        {
            if (_natoceni != value)
            {
                OnPropertyChanging(nameof(natoceni));
                _natoceni = value;
                OnPropertyChanged(nameof(natoceni));
            }
        }
    }

    /// <summary>
    /// Výchozí konstruktor třídy Cesta.
    /// </summary>
    public Cesta() { }

    /// <summary>
    /// Konstruktor třídy Cesta s inicializací pozice X, Y a natáčením.
    /// </summary>
    /// <param name="poziceX">Pozice X na herní mapě.</param>
    /// <param name="poziceY">Pozice Y na herní mapě.</param>
    /// <param name="natoceni">Natáčení cesty při vykreslování.</param>
    internal Cesta(int poziceX, int poziceY, int natoceni)
    {
        this.poziceX = poziceX;
        this.poziceY = poziceY;
        this.natoceni = natoceni;
    }
}