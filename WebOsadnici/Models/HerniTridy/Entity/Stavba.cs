using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using WebOsadnici.Data;

namespace WebOsadnici.Models.HerniTridy;
/// <summary>
/// Reprezentuje stavbu v aplikaci.
/// </summary>
public class Stavba : HerniEntita
{
    private string _nazev;
    private int _zisk;
    private string _imageUrl;
    private int _body;
    public virtual ObservableCollection<SurovinaKarta> Cena { get; set; }

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
    /// Kolik bodů dává stavba.
    /// </summary>
    internal virtual int Body
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
    public Stavba() { }

    /// <summary>
    /// Metoda pro vytvoření výchozích staveb, pokud ještě neexistují.
    /// </summary>
    internal static async Task VytvorStavby(ApplicationDbContext _dbContext)
    {
        // Pokud ještě neexistuje vesnice, vytvoříme ji.
        Stavba vesnice = await _dbContext.stavby.Where(s => s.Nazev.Equals("Vesnice")).SingleOrDefaultAsync();
        if (vesnice == null)
        {
            vesnice = new Stavba()
            {
                Nazev = "Vesnice",
                Zisk = 1,
                ImageUrl = "vesnicka.svg",
                Body = 1,
                Cena = new ObservableCollection<SurovinaKarta>() {
                    new SurovinaKarta() {
                        Surovina = _dbContext.suroviny.FirstOrDefault(s=> s.Nazev == "Dřevo"),
                        Pocet = 1
                    },
                    new SurovinaKarta() {
                        Surovina = _dbContext.suroviny.FirstOrDefault(s=> s.Nazev == "Cihla"),
                        Pocet = 1
                    },
                    new SurovinaKarta() {
                        Surovina = _dbContext.suroviny.FirstOrDefault(s=> s.Nazev == "Ovce"),
                        Pocet = 1
                    },
                    new SurovinaKarta() {
                        Surovina = _dbContext.suroviny.FirstOrDefault(s=> s.Nazev == "Obilí"),
                        Pocet = 1
                    },
                }
            };
            await _dbContext.stavby.AddAsync(vesnice);
        }

        // Pokud ještě neexistuje město, vytvoříme ho.
        Stavba mesto = await _dbContext.stavby.Where(s => s.Nazev.Equals("Město")).SingleOrDefaultAsync();
        if (mesto == null)
        {
            mesto = new Stavba()
            {
                Nazev = "Město",
                Zisk = 2,
                ImageUrl = "mesto.svg",
                Body = 2,
                Cena = new ObservableCollection<SurovinaKarta>()
                {
                    new SurovinaKarta()
                    {
                        Surovina = _dbContext.suroviny.FirstOrDefault(s=> s.Nazev == "Obilí"),
                        Pocet = 2
                    },
                    new SurovinaKarta()
                    {
                        Surovina = _dbContext.suroviny.FirstOrDefault(s=> s.Nazev == "Kámen"),
                        Pocet = 3
                    },
                }
            };
            await _dbContext.stavby.AddAsync(mesto);
        }
        // Pokud ještě neexistuje Cesta, vytvoříme ho.
        Stavba cesta = await _dbContext.stavby.Where(s => s.Nazev.Equals("Cesta")).SingleOrDefaultAsync();
        if (cesta == null)
        {
            cesta = new Stavba()
            {
                Nazev = "Cesta",
                Zisk = 0,
                ImageUrl = "cesta.svg",
                Body = 0,
                Cena = new ObservableCollection<SurovinaKarta>()
                {
                    new SurovinaKarta()
                    {
                        Surovina = _dbContext.suroviny.FirstOrDefault(s=> s.Nazev == "Dřevo"),
                        Pocet = 1
                    },
                    new SurovinaKarta()
                    {
                        Surovina = _dbContext.suroviny.FirstOrDefault(s=> s.Nazev == "Cihla"),
                        Pocet = 1
                    },
                }
            };
            await _dbContext.stavby.AddAsync(cesta);
        }

        //Pokud ještě neexistuje Akční karta, vytvoříme ji.
        Stavba AkcniKarta = await _dbContext.stavby.Where(s => s.Nazev.Equals("Akční karta")).SingleOrDefaultAsync();
        if (AkcniKarta == null)
        {
            AkcniKarta = new Stavba()
            {
                Nazev = "Akční karta",
                Zisk = 0,
                ImageUrl = "akcni_karta.svg",
                Body = 0,
                Cena = new ObservableCollection<SurovinaKarta>()
                {
                    new SurovinaKarta()
                    {
                        Surovina = _dbContext.suroviny.FirstOrDefault(s=> s.Nazev == "Ovce"),
                        Pocet = 1
                    },
                    new SurovinaKarta()
                    {
                        Surovina = _dbContext.suroviny.FirstOrDefault(s=> s.Nazev == "Kámen"),
                        Pocet = 1
                    },
                    new SurovinaKarta()
                    {
                        Surovina = _dbContext.suroviny.FirstOrDefault(s=> s.Nazev == "Obilí"),
                        Pocet = 1
                    },
                }
            };
            await _dbContext.stavby.AddAsync(AkcniKarta);
        }
        await _dbContext.SaveChangesAsync();
    }
}