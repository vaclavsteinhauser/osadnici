using WebOsadnici.Models.HerniTridy;
public class Karta : HerniEntita
{
    public int pocet;
}
public class AkcniKarta : Karta
{
    public String Nazev;
    public String ImageUrl;
}
public class SurovinaKarta : Karta
{
    public Surovina surovina;
}
