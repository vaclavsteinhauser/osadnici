using WebOsadnici.Models.HerniTridy;

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