namespace enginehry;

public class Pole
{
    public Guid Id { get; set; } = Guid.NewGuid();
    private Hra hra;
    private Surovina surovina;
    private Int32 cislo;
}