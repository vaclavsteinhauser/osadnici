using System.Collections.ObjectModel;

namespace WebOsadnici.Models.HerniTridy;

public class Smena : HerniEntita
{
    public virtual ObservableCollection<SurovinaKarta> poptavka { get; set; }
    public virtual ObservableCollection<SurovinaKarta> nabidka { get; set; }
    public virtual Hrac nabizejici { get; set; }
}
