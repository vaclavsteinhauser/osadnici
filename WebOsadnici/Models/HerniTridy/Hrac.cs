using System.ComponentModel;
using Microsoft.AspNetCore.Identity;

/// <summary>
/// Reprezentuje hráče v aplikaci.
/// </summary>
public class Hrac : IdentityUser, INotifyPropertyChanging, INotifyPropertyChanged
{
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
