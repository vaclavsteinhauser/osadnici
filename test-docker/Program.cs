// See https://aka.ms/new-console-template for more information
using databaze;
using Microsoft.EntityFrameworkCore;

using (var db = new GameContext())
{
    var hra = db.hry.Include("hraci").ToList()
        ;
    
    /*db.Database.EnsureCreated();
    Hrac v = new Hrac
    {
        Name = "Vaclav"
    };
    Hrac j = new Hrac
    {
        Name = "Janka"
    };
    Hra h = new Hra();
    Mapka m = new Mapka();
    db.hraci.Add(v);
    db.hraci.Add(j);
    db.hry.Add(h);
    db.mapky.Add(m);
    h.hraci = new List<Hrac>();
    h.mapka = new Mapka();
    h.hraci.Add(v);
    h.hraci.Add(j);
    db.SaveChanges();
 */
}