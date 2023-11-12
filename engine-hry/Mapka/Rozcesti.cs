namespace enginehry;

public class Rozcesti
{
    public Guid Id { get; set; } = Guid.NewGuid();
    private StavHrace hrac;
    private List<Cesta> cesty;
    private List<Pole> policka;
}