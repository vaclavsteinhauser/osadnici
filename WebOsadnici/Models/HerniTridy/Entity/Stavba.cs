using Microsoft.EntityFrameworkCore;
using WebOsadnici.Models.HerniTridy;

/// <summary>
/// Reprezentuje stavbu v aplikaci.
/// </summary>
public class Stavba : HerniEntita
{
    private string _nazev;
    private int _zisk;
    private string _imageUrl;

    /// <summary>
    /// Název stavby.
    /// </summary>
    internal virtual string Nazev
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
    /// Zisk z dané stavby.
    /// </summary>
    internal virtual int Zisk
    {
        get => _zisk;
        set
        {
            if (_zisk != value)
            {
                OnPropertyChanging(nameof(Zisk));
                _zisk = value;
                OnPropertyChanged(nameof(Zisk));
            }
        }
    }

    /// <summary>
    /// URL obrázku stavby.
    /// </summary>
    internal virtual string ImageUrl
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

    /// <summary>
    /// Konstruktor pro vytvoření instance stavby.
    /// </summary>
    /// <param name="nazev">Název stavby.</param>
    /// <param name="zisk">Zisk z dané stavby.</param>
    /// <param name="imageUrl">URL obrázku stavby.</param>
    public Stavba(string nazev, int zisk, string imageUrl)
    {
        Nazev = nazev;
        Zisk = zisk;
        ImageUrl = imageUrl;
    }
    public Stavba() {}

    /// <summary>
    /// Metoda pro vytvoření výchozích staveb, pokud ještě neexistují.
    /// </summary>
    internal static async Task VytvorStavby(DbSet<Stavba> stavby)
    {
        // Pokud ještě neexistuje vesnice, vytvoříme ji.
        Stavba vesnice = await stavby.Where(s => s.Nazev.Equals("Vesnice")).SingleOrDefaultAsync();
        if (vesnice == null)
        {
            vesnice = new Stavba("Vesnice", 1, "vesnicka.svg");
            await stavby.AddAsync(vesnice);
        }

        // Pokud ještě neexistuje město, vytvoříme ho.
        Stavba mesto = await stavby.Where(s => s.Nazev.Equals("Město")).SingleOrDefaultAsync();
        if (mesto == null)
        {
            mesto = new Stavba("Město", 2, "mesto.svg");
            await stavby.AddAsync(mesto);
        }
    }
}