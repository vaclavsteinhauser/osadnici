using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace WebOsadnici.Models.HerniTridy;

/// <summary>
/// Reprezentuje rozcestí v aplikaci.
/// </summary>
public class Rozcesti : HerniEntita
{
    // Velikost určuje průměr kruhu
    internal readonly static Size Velikost = new Size(2, 2);
    public virtual Mapka? Mapka { get; set; }

    private int _poziceX;
    private int _poziceY;
    private Hrac? _hrac;
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
    [NotMapped]
    public virtual bool Blokovane
    {
        get => Sousedi.Any(s => s.Stavba != null);
    }
    ///<summary>
    /// Cesty u rozcesti
    /// </summary>
    [NotMapped]
    public IEnumerable<Cesta> Cesty { get => Mapka.Cesty.Where(c => c.rozcesti.Contains(this)); }

    ///<summary>
    /// Sousední rozcestí
    /// </summary>
    [NotMapped]
    public IEnumerable<Rozcesti> Sousedi { get => Cesty.SelectMany(c => c.rozcesti).Where(r => r.Id != Id); }



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
    internal Rozcesti(Mapka m, int poziceX, int poziceY)
    {
        Mapka = m;
        PoziceX = poziceX;
        PoziceY = poziceY;
    }
    public Rozcesti() { }

    public string VykresleniHTML(Hra h)
    {
        int odsazeniX = (PoziceX * Mapka.RozmeryMrizky.Width - (Rozcesti.Velikost.Width * Mapka.RozmeryMrizky.Width / 2));
        int odsazeniY = (PoziceY * Mapka.RozmeryMrizky.Height - (Rozcesti.Velikost.Height * Mapka.RozmeryMrizky.Height / 2));
        int vyska = Rozcesti.Velikost.Height * Mapka.RozmeryMrizky.Height;
        int sirka = Rozcesti.Velikost.Width * Mapka.RozmeryMrizky.Width;
        string barva;
        if (Hrac == null)
        {
            barva = "black";
        }
        else
        {
            barva = h.DejStav(Hrac).barva.Name;
        }
        string obrazek = (Stavba == null) ? "" : "../../" + Stavba.ImageUrl;
        string zobrazit = (Stavba == null) ? "none" : "inline";
        return $@"
        <svg style='position: absolute; pointer-events: none;
            top: {odsazeniY}px;
            left: {odsazeniX}px;'
            width='{sirka}px'
            height='{vyska}px'
            xmlns='http://www.w3.org/2000/svg'>
        <circle class='rozcesti' id='{Id}' cx='{sirka / 2}' cy='{vyska / 2}' r='{Math.Min(sirka, vyska) / 4}' fill='{barva}' style='pointer-events: auto;' onclick='klik_rozcesti(event)' />
        <image xlink:href='{obrazek}' width='{Math.Min(sirka, vyska) * 0.7}' height='{Math.Min(sirka, vyska) * 0.7}' display='{zobrazit}' />
        </svg>";
    }
}