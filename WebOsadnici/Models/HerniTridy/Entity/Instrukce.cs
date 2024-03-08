namespace WebOsadnici.Models.HerniTridy;

public enum Instrukce
{
    StavbaCesty, // rucne nakup / automaticky rozestavovani,zahrani stavba silnic
    StavbaMesta, //jen rucne
    StavbaVesnice, //rucne nakup / automaticky rozestavovani
    PresunZlodeje, //automaticky hod 7 / rucne zahrani rytire
    VytvoritSmenu,
    VyberSurovinu, //automaticky zahrani vynalez, monopol;
}
public class Aktivita : HerniEntita
{
    public virtual Hrac Hrac { get; set; }
    public virtual Instrukce Akce { get; set; }
    public virtual int CisloAktivity { get; set; }
    public virtual bool Probiha { get; set; }
}
