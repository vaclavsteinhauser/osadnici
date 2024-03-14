using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WebOsadnici.Data;

namespace WebOsadnici.Models.HerniTridy;

/// <summary>
/// Reprezentuje surovinu v aplikaci.
/// </summary>
public class Surovina : HerniEntita
{
    private string _nazev;

    /// <summary>
    /// Název suroviny.
    /// </summary>
    [MaxLength(100)]
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

    private string _imageUrl;

    /// <summary>
    /// URL adresa obrázku suroviny.
    /// </summary>
    [MaxLength(100)]
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

    private string _backColor;

    /// <summary>
    /// Barva pozadí suroviny.
    /// </summary>
    [MaxLength(100)]
    public virtual string BackColor
    {
        get => _backColor;
        set
        {
            if (_backColor != value)
            {
                OnPropertyChanging(nameof(BackColor));
                _backColor = value;
                OnPropertyChanged(nameof(BackColor));
            }
        }
    }

    /// <summary>
    /// Vytvoří suroviny a přidá je do databáze, pokud neexistují.
    /// </summary>
    /// <param name="_suroviny">Databáze surovin.</param>
    internal static async Task VytvorSuroviny(ApplicationDbContext _dbContext)
    {
        string[] nazvy = { "Dřevo", "Cihla", "Obilí", "Ovce", "Kámen", "Poušť" };
        string[] imageUrls = { "drevo.svg", "cihla.svg", "obili.svg", "ovce.svg", "kamen.svg", "poust.svg" };
        string[] backColors = { "Sienna", "Firebrick", "Gold", "Limegreen", "Gray", "Yellow" };

        for (int i = 0; i < nazvy.Length; i++)
        {
            Surovina s = await _dbContext.suroviny.Where(s => s.Nazev.Equals(nazvy[i])).FirstOrDefaultAsync();
            if (s == null)
            {
                s = new Surovina
                {
                    Nazev = nazvy[i],
                    ImageUrl = imageUrls[i],
                    BackColor = backColors[i]
                };
                await _dbContext.suroviny.AddAsync(s);
            }
        }
        await _dbContext.SaveChangesAsync();
    }
}