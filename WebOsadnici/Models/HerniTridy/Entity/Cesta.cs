using System.Collections.ObjectModel;
using System.Drawing;

namespace WebOsadnici.Models.HerniTridy;
public class Cesta : HerniEntita
{
    internal static readonly Size velikost = new(2, 2);
    public virtual Mapka? Mapka { get; set; }
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
    internal Cesta(Mapka m, int poziceX, int poziceY, int natoceni)
    {
        Mapka = m;
        this.poziceX = poziceX;
        this.poziceY = poziceY;
        this.natoceni = natoceni;
    }
    public string VykresleniHTML(Hra h)
    {
        int odsazeniX = (poziceX - (velikost.Width / 2)) * Mapka.RozmeryMrizky.Width;
        int odsazeniY = (poziceY - (velikost.Height / 2)) * Mapka.RozmeryMrizky.Height;
        int vyska = velikost.Height * Mapka.RozmeryMrizky.Height;
        int sirka = velikost.Width * Mapka.RozmeryMrizky.Width;
        String polygon;
        switch (natoceni)
        {
            case 0: polygon = $"{13 * sirka / 30},0 {17 * sirka / 30},0 {17 * sirka / 30},{vyska} {13 * sirka / 30},{vyska}"; break;
            case 1: polygon = $"0,0 {1 * sirka / 10},0 {sirka},{9 * vyska / 10} {sirka},{vyska} {9 * sirka / 10},{vyska} 0,{1 * vyska / 10}"; break;
            default: polygon = $"{9 * sirka / 10},0 {sirka},0 {sirka},{1 * vyska / 10} {1 * sirka / 10},{vyska} 0,{vyska} 0,{9 * vyska / 10}"; break;
        }
        string barva = (hrac == null) ? "black" : h.DejStav(hrac).barva.Name;
        return $@"
            <svg style='position: absolute; pointer-events: none;
                    top: {odsazeniY}px;
                    left: {odsazeniX}px;'
                    width='{sirka}px'
                    height='{vyska}px'
                    xmlns='http://www.w3.org/2000/svg' alt='cesta'>
                <polygon class='cesta' id='{Id}' points='{polygon}' fill='{barva}' style='pointer-events: auto;' onclick='klik_cesta(event)' />
            </svg>";
    }
}