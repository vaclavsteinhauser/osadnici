using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Drawing;
using WebOsadnici.Controllers;
using WebOsadnici.Data;
using WebOsadnici.Hubs;
using WebOsadnici.Models.HerniTridy;

public enum StavHry { Vytvorena, Rozestavovani, Probiha, Skoncila }

public class Hra : HerniEntita
{
    public static IHubContext<ClickHub> HubContext { get; set; }
    //statické proměnné
    static public readonly int MaximalneHracu = 4; 
    static private List<Hra> NacteneHry;
    //metoda k vypusteni neaktivnich her z paměti
    private static void Cisteni()
    {
        while (true)
        {
            lock (NacteneHry)
            {
                for (int i = 0; i < NacteneHry.Count; i++)
                    if (DateTime.Now.Subtract(NacteneHry[i].naposledyPouzito).TotalMinutes > 10)
                    {
                        NacteneHry.RemoveAt(i);
                        i--;
                    }
            }
            Thread.Sleep(10 * 60 * 1000);
        }
    }
    //proměnná určující poslední použití, na zakladě které probíhá čištění
    DateTime naposledyPouzito = DateTime.Now;
    //staticky konstruktor, který zapne automatické čištěni
    static Hra()
    {
        NacteneHry = new();
        new Thread(Cisteni).Start();
    }
    //dvě funkce k načtení hry, jedna k přidání do správy právě vytvořené hry, druhá k vydání spravované hry nebo načtení z databáze
    public static void NactiHru(Hra h)
    {
        lock (NacteneHry)
        {
            NacteneHry.Add(h);
        }
    }
    public static Hra NactiHru(Guid hraId, ApplicationDbContext context)
    {
        Hra hra;
        lock (NacteneHry)
        {
            hra = NacteneHry.Find(h => h.Id == hraId);
        }
        
        if (hra == null)
        {
            hra = context.hry
                .Where(h => h.Id == hraId)
                .Include(h => h.hraci) // Hráči
                .Include(h => h.stavy)
                    .ThenInclude(s => s.hrac) // Hráči stavů
                .Include(h => h.stavy)
                    .ThenInclude(s => s.SurovinaKarty)// Surovinové karty stavů
                .Include(h => h.stavy)
                    .ThenInclude(s => s.AkcniKarty) // Akční karty stavů
                .Include(h => h.mapka)
                    .ThenInclude(m => m.policka) // Policka
                        .ThenInclude(p => p.surovina) // Suroviny políček
                .Include(h => h.mapka)
                    .ThenInclude(p => p.rozcesti) // Rozcestí políček
                        .ThenInclude(r => r.hrac) // Hráči na rozcestích
                .Include(h => h.mapka)
                    .ThenInclude(p => p.rozcesti)
                        .ThenInclude(r => r.stavba) // Stavby na rozcestích
                .Include(h => h.mapka)
                    .ThenInclude(m => m.cesty) // Cesty
                .Include(h => h.mapka)
                    .ThenInclude(c => c.rozcesti) // Rozcestí cest
                        .ThenInclude(r => r.hrac) // Hráči na rozcestích
                .Include(h => h.mapka)
                    .ThenInclude(c => c.rozcesti)
                        .ThenInclude(r => r.stavba) // Stavby na rozcestích
                .SingleOrDefault();
            lock (NacteneHry)
            {
                if(hra!=null)
                NacteneHry.Add(hra);
            }
            
        }
        return hra;
        
    }
    //funkce, která vrací náhled hry jen se seznamem hráčů, slouží k určení, které hry jsou relevantní pro aktuálního uživatele
    public static IQueryable<Hra> NactiNahledyHer(ApplicationDbContext context)
    {
        return context.hry
                .Include(h => h.hraci);
    }
    //konstruktor a inicializace
    public async Task Inicializace(ApplicationDbContext _dbContext)
    {
        mapka = new Mapka();
        await mapka.Inicializace(this, _dbContext);
    }
    //Aktuální stav hry na základě kterého jsou povolené a zakázané funkce
    public StavHry stavHry = StavHry.Vytvorena;
    //list hráčů
    public List<Hrac> hraci=new List<Hrac>();
    //list list stavů hráčů zahrnující jejich vlastnosti v této hře
    public List<StavHrace> stavy = new List<StavHrace>();
    //index hráče, který je na tahu
    public int hracNaTahu = -1;
    public Hrac AktualniHrac()
    {
        var stav=stavy.Where(s => s.JeNaTahu()).FirstOrDefault();
        if (stav == null)
            return null;
        return stav.hrac;
    }
    public Mapka? mapka;

    private Random kostka=new Random();
    int hodnotaKostek;
    private void HodKostkou(int pocet=2)
    {
        hodnotaKostek = 0;
        for (int i = 0; i < pocet; i++)
        {
            hodnotaKostek += (int)(kostka.Next() % 6) + 1;
        }
    }
    public bool JeObsazenaBarva(String b)
    {
        foreach(StavHrace s in stavy)
        {
            if (b.Equals(s.barva.Name))
                return true;
        }
        return false;
    }
    internal async Task PridejHrace(Hrac h, Color barva, ApplicationDbContext _dbContext)
    {
        naposledyPouzito = DateTime.Now;
        _dbContext.Database.BeginTransaction();
        hraci.Add(h);
        StavHrace s = new StavHrace()
        {
            poradi=stavy.Count,
            hra = this,
            barva = barva,
            hrac = h
        };
        _dbContext.Update(this);
        _dbContext.Add(s);
        foreach (Surovina x in _dbContext.suroviny)
        {
            SurovinaKarta k = new SurovinaKarta()
            {
                surovina = x,
                pocet = 0
            };
            _dbContext.Update(x);
            _dbContext.Add(k);
            s.SurovinaKarty.Add(k);
        }
        stavy.Add(s);
        await _dbContext.Database.CommitTransactionAsync();

        await _dbContext.SaveChangesAsync();
        
        if(hraci.Count == Hra.MaximalneHracu)
        {
            ZacniHru();
        }
    }
    private void ZacniHru()
    {
        stavHry = StavHry.Rozestavovani;
        hracNaTahu = 0;
        var hraId = this.Id.ToString();
        Hra.HubContext.Clients.All.SendAsync("ObnovitStrankuHry", hraId);
    }
    private void ZacniDalsiTah()
    {
        hracNaTahu = hracNaTahu%stavy.Count+1;
        HodKostkou();
        /*foreach (var p in mapka.policka) {
            if (p.cislo == hodnotaKostek)
            {
                foreach (var r in p.rozcesti) {
                    if (r.stavba != null)
                    {
                        stavy.Where(s => s.hrac == r.hrac).First().PridejSurovinu(p.surovina, r.stavba.zisk);
                    }
                }
            }
        }*/

    }
    public void KliknutiNaPolicko(Guid idPolicka, string _connectionId)
    {
        if (hracNaTahu != -1 && hraci[hracNaTahu].Id == _connectionId)
        {
            var policko = mapka.policka.Find(p => p.Id == idPolicka);
            if (policko != null)
            {
                // Zkontrolujte, zda je políčko obsazeno
                if (policko.rozcesti.Count > 0 && policko.rozcesti[0].hrac != null)
                {
                    // Získejte suroviny
                    stavy[hracNaTahu].PridejSurovinu(policko.surovina, policko.rozcesti[0].stavba.zisk);
                }
            }
        }
    }
    public void KliknutiNaCestu(Guid idCesty, string _connectionId)
    {
        if (hracNaTahu != -1 && hraci[hracNaTahu].Id == _connectionId)
        {
            var cesta = mapka.cesty.Find(c => c.Id == idCesty);
            if (cesta != null)
            {
                if(cesta.hrac == null)
                {
                    cesta.hrac = hraci[hracNaTahu];
                }

            }
        }
    }
    public void KliknutiNaRozcesti(Guid idRozcesti, string _connectionId)
    {
        if (hracNaTahu != -1 && hraci[hracNaTahu].Id == _connectionId)
        {
            var rozcestí = mapka.rozcesti.Find(r => r.Id == idRozcesti);
            if (rozcestí != null)
            {
                if (rozcestí.hrac == null)
                {
                    // Postavte vesnici/město
                    //rozcestí.PostavStavbu(stavy[hracNaTahu]);

                }
            }
        }
    }
}