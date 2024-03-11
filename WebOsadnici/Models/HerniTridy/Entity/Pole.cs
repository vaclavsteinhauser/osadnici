using System.Collections.ObjectModel;
using System.Drawing;

namespace WebOsadnici.Models.HerniTridy;

/// <summary>
/// Reprezentuje herní pole v aplikaci.
/// </summary>
public class Pole : HerniEntita
{
    // Velikost herního pole
    public static readonly Size Velikost = new Size(4, 6);
    public virtual Mapka? Mapka { get; set; }

    private int _poziceX;
    private int _poziceY;
    private Surovina? _surovina;
    private int _cislo;
    private bool _blokovane;
    private ObservableCollection<Rozcesti>? _rozcesti;

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
    public Pole(Mapka m,Surovina surovina, int cislo, int sloupec, int radek, bool blokovane = false)
    {
        Mapka = m;
        Surovina = surovina;
        Cislo = cislo;
        PoziceX = sloupec;
        PoziceY = radek;
        Blokovane = blokovane;
        Rozcesti = new ObservableCollection<Rozcesti>() { null, null, null, null, null, null };
    }
    public Pole() { }

    public string VykresleniHTML(Hra h)
    {
        int odsazeniX = (PoziceX - (Pole.Velikost.Width / 2)) * Mapka.RozmeryMrizky.Width;
        int odsazeniY = (PoziceY - (Pole.Velikost.Height / 2)) * Mapka.RozmeryMrizky.Height;
        int vyska = Pole.Velikost.Height * Mapka.RozmeryMrizky.Height;
        int sirka = Pole.Velikost.Width * Mapka.RozmeryMrizky.Width;
        string zobrazitZlodeje = Blokovane? "block" : "none";
        string cislo = (Cislo != 0) ? $@"<circle cx='{sirka / 2}' cy='{vyska / 2}' r='{Math.Min(sirka, vyska) / 8}' fill='white' stroke='black' stroke-width='2' style='pointer-events: none;' />
                <text x='{sirka / 2}' y='{vyska / 2}' text-anchor='middle' alignment-baseline='middle' fill='black' font-weight='bold' font-size='{Math.Min(sirka, vyska) / 6}' style='pointer-events: none;'>{Cislo}</text>" : "";
        return $@"
            <svg style='position: absolute; pointer-events: none;
                    top: {odsazeniY}px;
                    left: {odsazeniX}px;'
                    width='{sirka}px'
                    height='{vyska}px'
                    xmlns='http://www.w3.org/2000/svg' alt='{Surovina.Nazev}'>
                <polygon class='policko' id='{Id}' points='{sirka / 2},0 {sirka},{vyska / 3} {sirka},{2 * vyska / 3} {sirka / 2},{vyska} 0,{2 * vyska / 3} 0,{vyska / 3}'  style='fill:{Surovina.BackColor}; pointer-events: auto;' onclick='klik_policko(event)' />
                <clipPath id='hexMask'>
                    <polygon points='{sirka / 2},0 {sirka},{vyska / 3} {sirka},{2 * vyska / 3} {sirka / 2},{vyska} 0,{2 * vyska / 3} 0,{vyska / 3}' style='pointer-events: none;' />
                </clipPath>
                <image xlink:href='../../{Surovina.ImageUrl}' width='80%' height='80%' x='10%' y='10%' clip-path='url(#hexMask)' style='pointer-events: none;' />
                <image id='{Id}-zlodej' xlink:href='../../zlodej.svg' width='80%' height='80%' x='10%' y='10%' clip-path='url(#hexMask)' style='pointer-events: none; display:{zobrazitZlodeje};' />
                {cislo}
            </svg>";
    }
}
