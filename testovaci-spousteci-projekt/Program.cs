
using Microsoft.EntityFrameworkCore;
using enginehry;

using (var db = new GameContext())
{
    db.Database.EnsureCreated();
    StavHrace v = new StavHrace
    {
        Name = "Vaclav"
    };
    StavHrace j = new StavHrace
    {
        Name = "Janka"
    };
    Hra h = new Hra();
    Mapka m = new Mapka();
    db.hraci.Add(v);
    db.hraci.Add(j);
    db.hry.Add(h);
    db.mapky.Add(m);
    h.hraci = new List<StavHrace>();
    h.mapka = new Mapka();
    h.hraci.Add(v);
    h.hraci.Add(j);
    db.SaveChanges();

    var hra = db.hry.Include("hraci").ToList();
    Console.WriteLine("");
}