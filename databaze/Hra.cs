namespace databaze;

public class Hra
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public ICollection<StavHrace> hraci;
    public Mapka mapka;
}