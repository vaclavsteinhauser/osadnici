using WebOsadnici.Models.HerniTridy;

internal abstract class Stavba : HerniEntita
{
    internal int zisk;
}
internal class Vesnice : Stavba
{
    public Vesnice() {
        zisk = 1;
    }
}
internal class Mesto : Stavba
{
    public Mesto()
    {
        zisk = 2;
    }
}