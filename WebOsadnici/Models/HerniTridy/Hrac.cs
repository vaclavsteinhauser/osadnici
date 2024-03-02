using Microsoft.AspNetCore.Identity;
using System.ComponentModel;

/// <summary>
/// Reprezentuje hráče v aplikaci.
/// </summary>
public class Hrac : IdentityUser, INotifyPropertyChanging, INotifyPropertyChanged
{
    private string? _name;

    /// <summary>
    /// Jméno hráče.
    /// </summary>
    public virtual string? Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                OnPropertyChanging(nameof(Name));
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    /// <summary>
    /// Událost oznamující, že se vlastnost mění.
    /// </summary>
    public event PropertyChangingEventHandler PropertyChanging;

    /// <summary>
    /// Událost oznamující, že se vlastnost změnila.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Určuje, zda je tento objekt roven jinému objektu.
    /// </summary>
    /// <param name="obj">Objekt pro porovnání.</param>
    /// <returns>True, pokud jsou objekty rovny, jinak false.</returns>
    public override bool Equals(object? obj)
    {
        if (!(obj is IdentityUser hrac))
            return false;

        return hrac.Id == Id;
    }

    /// <summary>
    /// Vyvolá událost oznamující změnu vlastnosti.
    /// </summary>
    /// <param name="propertyName">Název změněné vlastnosti.</param>
    protected virtual void OnPropertyChanging(string propertyName)
    {
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    }

    /// <summary>
    /// Vyvolá událost oznamující změnu vlastnosti.
    /// </summary>
    /// <param name="propertyName">Název změněné vlastnosti.</param>
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
