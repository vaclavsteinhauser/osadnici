using WebOsadnici.Models.HerniTridy;

public class Rozcesti : HerniEntita
{
    internal Hrac hrac;
    private List<Cesta> cesty;
    private List<Pole> policka;
    private bool blokovane=false;
    internal Stavba stavba;
}