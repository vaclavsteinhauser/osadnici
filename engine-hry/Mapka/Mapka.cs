namespace enginehry;

public class Mapka
{
    public Guid Id { get; set; } = Guid.NewGuid();
    private Hra hra;
    private List<Pole> policka;
    private List<Cesta> cesty;
    private List<Rozcesti> rozcesti;
}