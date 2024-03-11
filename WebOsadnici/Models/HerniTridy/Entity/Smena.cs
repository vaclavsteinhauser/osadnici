using System.Collections.ObjectModel;

namespace WebOsadnici.Models.HerniTridy;

public class Smena : HerniEntita
{
    public virtual ObservableCollection<SurovinaKarta> poptavka { get; set; } = new();
    public virtual ObservableCollection<SurovinaKarta> nabidka { get; set; } = new();
    public virtual Hrac hrac { get; set; }
}
