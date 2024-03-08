namespace WebOsadnici.Models.HerniTridy;

/// <summary>
/// Abstraktní třída reprezentující herní kartu.
/// </summary>
public class Karta : HerniEntita
{
    private int _pocet;

    /// <summary>
    /// Počet karet.
    /// </summary>
    public virtual int Pocet
    {
        get => _pocet;
        set
        {
            if (_pocet != value)
            {
                OnPropertyChanging(nameof(Pocet));
                _pocet = value;
                OnPropertyChanged(nameof(Pocet));
            }
        }
    }
}

/// <summary>
/// Třída reprezentující akční kartu.
/// </summary>
public class AkcniKarta : Karta
{
    public static string[] nazvy = new string[] { "Rytíř", "Monopol", "Vynález", "Stavba silnic" };
    public static Dictionary<string,string> obrazky = new Dictionary<string, string>()
    {
        {"Rytíř","rytir.svg" },
        {"Monopol","monopol.svg" },
        {"Vynález","vynalez.svg" },
        {"Stavba silnic","stavba_silnic.svg" }
    };
    private string _nazev;
    private string _imageUrl;

    /// <summary>
    /// Název akční karty.
    /// </summary>
    public virtual string Nazev
    {
        get => _nazev;
        set
        {
            if (_nazev != value)
            {
                OnPropertyChanging(nameof(Nazev));
                _nazev = value;
                OnPropertyChanged(nameof(Nazev));
            }
        }
    }

    /// <summary>
    /// URL obrázku akční karty.
    /// </summary>
    public virtual string ImageUrl
    {
        get => _imageUrl;
        set
        {
            if (_imageUrl != value)
            {
                OnPropertyChanging(nameof(ImageUrl));
                _imageUrl = value;
                OnPropertyChanged(nameof(ImageUrl));
            }
        }
    }

    public void Zahraj(Hra hra, Hrac hrac)
    {
        switch (Nazev)
        {
            case "Rytíř":
                hra.PridejAktivitu(new Aktivita()
                {
                    Hrac=hrac,
                    Akce=Instrukce.PresunZlodeje
                });
                break;
            case "Monopol":
                hra.PridejAktivitu(new Aktivita()
                {
                    Hrac=hrac,
                    Akce=Instrukce.VyberSurovinu
                });
                break;
            case "Vynález":
                hra.PridejAktivitu(new Aktivita()
                {
                    Hrac=hrac,
                    Akce=Instrukce.VyberSurovinu
                });
                hra.PridejAktivitu(new Aktivita()
                {
                    Hrac = hrac,
                    Akce = Instrukce.VyberSurovinu
                });
                break;
            case "Stavba silnic":
                hra.PridejAktivitu(new Aktivita()
                {
                    Hrac = hrac,
                    Akce = Instrukce.StavbaCesty
                });
                hra.PridejAktivitu(new Aktivita()
                {
                    Hrac = hrac,
                    Akce = Instrukce.StavbaCesty
                });
                break;
        }
    }
}
public class BodovaKarta : AkcniKarta
{
    public static string[] nazvy = new string[] { "Nejdelší cesta", "Největší vojsko", "Akční body" };
    public static Dictionary<string, string> obrazky = new Dictionary<string, string>()
    {
        {"Nejdelší cesta","cesta.svg" },
        {"Největší vojsko","rytir.svg" },
        {"Akční body","vynalez.svg" }
    };
    private int _body;

    /// <summary>
    /// Počet bodů, které karta přináší.
    /// </summary>
    public virtual int Body
    {
        get => _body;
        set
        {
            if (_body != value)
            {
                OnPropertyChanging(nameof(Body));
                _body = value;
                OnPropertyChanged(nameof(Body));
            }
        }
    }                                                                                         
}

/// <summary>
/// Třída reprezentující kartu suroviny.
/// </summary>
public class SurovinaKarta : Karta
{
    private Surovina _surovina;

    /// <summary>
    /// Surovina, kterou karta reprezentuje.
    /// </summary>
    public virtual Surovina Surovina
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
}