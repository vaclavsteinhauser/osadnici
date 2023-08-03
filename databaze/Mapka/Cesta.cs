namespace databaze;

public class Cesta
{
    public Guid Id { get; set; } = Guid.NewGuid();
    private StavHrace hrac;
    private List<Rozcesti> konce;
}