using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebOsadnici.Data;
using WebOsadnici.Models.HerniTridy;

namespace WebOsadnici.Hubs
{
    public class ClickHub : Hub
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public ClickHub(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        // Metoda pro obsluhu kliknutí na tlačítko pro změnu
        public async Task VytvorSmenuHraci(string connectionId, string HracId, string HraId, Dictionary<string, string> data)
        {
            Hra hra = await Hra.NactiHru(Guid.Parse(HraId), _dbContextFactory.CreateDbContext());
            Hrac hrac = hra.DejHrace(HracId);
            StavHrace stav = hra.DejStav(hrac);
            var nenulove = data.Where(d => !string.IsNullOrEmpty(d.Value) && int.Parse(d.Value) > 0).ToDictionary(d => d.Key, d => d.Value);
            var vstupni = nenulove.Where(d => d.Key.StartsWith("vstup-")).ToDictionary(d => d.Key, d => d.Value);
            var vystupni = nenulove.Where(d => d.Key.StartsWith("vystup-")).ToDictionary(d => d.Key, d => d.Value);
            Smena smena = new Smena();
            foreach (var vstup in vstupni)
            {
                var surovina = stav.SurovinaKarty.FirstOrDefault(s => s.Surovina.Nazev == vstup.Key.Substring(6));
                if (surovina == null || surovina.Pocet < int.Parse(vstup.Value))
                {
                    await Clients.Client(connectionId).SendAsync("NastavText", $"Nelze vyměnit. Není dostatek surovin. {hra.DejInstrukci(hrac)}");
                    return;
                }
                smena.nabidka.Add(new SurovinaKarta() { Surovina = surovina.Surovina, Pocet = int.Parse(vstup.Value) });
            }
            foreach (var vystup in vystupni)
            {
                var surovina = hra.mapka.Suroviny.FirstOrDefault(s => s.Nazev == vystup.Key.Substring(7));
                smena.poptavka.Add(new SurovinaKarta() { Surovina = surovina, Pocet = int.Parse(vystup.Value) });
            }
            smena.hrac = hrac;
            hra.PridejSmenu(smena);
            await hra._dbContext.SaveChangesAsync();
            await AktualizovatHru(hra.Id.ToString());
        }
        // Metoda pro obsluhu kliknutí na provedeni smeny
        public async Task KliknutiNaProvedeniSmeny(string connectionId, string Id, string HracId, string HraId)
        {
            Hra hra = await Hra.NactiHru(Guid.Parse(HraId), _dbContextFactory.CreateDbContext());
            Hrac hrac = hra.DejHrace(HracId);
            Smena smena = hra.aktivniSmeny.FirstOrDefault(s => s.Id.ToString().Equals(Id));
            StavHrace stavmuj = hra.DejStav(hrac);
            StavHrace stavcizi = hra.DejStav(smena.hrac);
            foreach (var s in smena.nabidka)
            {
                if(s.Pocet > stavcizi.SurovinaKarty.FirstOrDefault(n=>n.Surovina.Nazev == s.Surovina.Nazev).Pocet)
                {
                    await Clients.Client(connectionId).SendAsync("NastavText", "Nelze provést směnu. Cizí hráč nemá dostatek surovin.");
                    return;
                }
            }
            foreach (var s in smena.poptavka)
            {
                if (s.Pocet > stavmuj.SurovinaKarty.FirstOrDefault(n => n.Surovina.Nazev == s.Surovina.Nazev).Pocet)
                {
                    await Clients.Client(connectionId).SendAsync("NastavText", "Nelze provést směnu. Nemáte dostatek surovin.");
                    return;
                }
            }
            foreach (var s in smena.nabidka)
            {
                var kartamoje = stavmuj.SurovinaKarty.FirstOrDefault(x => x.Surovina.Nazev == s.Surovina.Nazev);
                var kartacizi = stavcizi.SurovinaKarty.FirstOrDefault(x => x.Surovina.Nazev == s.Surovina.Nazev);
                kartamoje.Pocet += s.Pocet;
                kartacizi.Pocet -= s.Pocet;
            }
            foreach (var s in smena.poptavka)
            {
                var kartamoje = stavmuj.SurovinaKarty.FirstOrDefault(x => x.Surovina.Nazev == s.Surovina.Nazev);
                var kartacizi = stavcizi.SurovinaKarty.FirstOrDefault(x => x.Surovina.Nazev == s.Surovina.Nazev);
                kartamoje.Pocet -= s.Pocet;
                kartacizi.Pocet += s.Pocet;
            }
            hra.SmazSmenu(smena);
            await hra._dbContext.SaveChangesAsync();
            await AktualizovatHru(hra.Id.ToString());

        }
        // Metoda pro obsluhu kliknutí na tlačítko pro změnu
        public async Task VytvorSmenuHra(string connectionId, string HracId, string HraId, Dictionary<string, string> data)
        {
            Hra hra = await Hra.NactiHru(Guid.Parse(HraId), _dbContextFactory.CreateDbContext());
            Hrac hrac = hra.DejHrace(HracId);
            StavHrace stav = hra.DejStav(hrac);
            if (hra.stavHry != StavHry.Probiha || hrac.Id != hra.AktualniHrac().Id)
            {
                return;
            }
            SurovinaKarta vstup = stav.SurovinaKarty.FirstOrDefault(s => s.Surovina.Nazev == data["vstup"]);
            SurovinaKarta vystup = stav.SurovinaKarty.FirstOrDefault(s => s.Surovina.Nazev == data["vystup"]);
            if(vstup==null || vstup.Pocet < 4)
            {
                await Clients.Client(connectionId).SendAsync("NastavText", $"Nelze vyměnit. Není dostatek surovin. {hra.DejInstrukci(hrac)}");
            }
            else
            {
                vstup.Pocet -= 4;
                vystup.Pocet++;
                await hra._dbContext.SaveChangesAsync();
                await Clients.Client(connectionId).SendAsync("ObnovSekci", HraId, "Suroviny", hra.DejSurovinoveKartyHTML(hrac));
                await Clients.Client(connectionId).SendAsync("ObnovSekci", HraId, "nakup", hra.DejNakupHTML(hrac));
                await Clients.Client(connectionId).SendAsync("ObnovSekci", HraId, "smena-s-hrou", hra.DejSmenuHraHTML(hrac));
                await Clients.Client(connectionId).SendAsync("ObnovSekci", HraId, "smena-s-hraci", hra.DejSmenuHraciHTML(hrac));
                await Clients.Client(connectionId).SendAsync("NastavText", hra.DejInstrukci(hrac));
            }
        }

        // Metoda pro obsluhu kliknutí na tlačítko pro další akci
        public async Task tlacitko_dalsi_klik(string connectionId, string HracId, string HraId)
        {
            Hra h=await Hra.NactiHru(Guid.Parse(HraId), _dbContextFactory.CreateDbContext());
            if (h.hraci.Count < 2 || h.stavHry == StavHry.Skoncila)
            {
                await Clients.Client(connectionId).SendAsync("NastavText", "Hra nemůže být spuštěna.");
                return;
            }
            Hrac hrac = h.hraci.FirstOrDefault(h => h.Id.ToString().Equals(HracId));
            if(h.stavHry==StavHry.Nezacala && hrac==h.DejHrace(0))
            {
                await h.ZacniHru();
                return;
            }
            if(hrac==h.AktualniHrac() && h.AktualniAktivita()==null)
            {
                await h.DalsiHrac();
            }
            await AktualizovatHru(HraId);
        }

        // Metoda pro obsluhu kliknutí na cestu
        public async Task KliknutiNaCestu(string connectionId, string Id, string HracId, string HraId)
        {
            Hra hra = await Hra.NactiHru(Guid.Parse(HraId), _dbContextFactory.CreateDbContext());
            Hrac hrac = hra.DejHrace(HracId);
            StavHrace stav = hra.DejStav(hrac);
            Cesta cesta = hra.mapka.Cesty.FirstOrDefault(r => r.Id.ToString().Equals(Id));
            if (hra.stavHry != StavHry.Probiha || hrac.Id != hra.AktualniHrac().Id)
            {
                return;
            }
            Aktivita a = hra.AktualniAktivita();
            if (a == null) return;
            if (a.Akce == Instrukce.StavbaCesty)
            {
                if (cesta.hrac != null ||
                   (!cesta.rozcesti.Any(r=>r.Hrac != null && r.Hrac.Id == hrac.Id)
                   && cesta.rozcesti.All(r=>!r.Cesty.Any(c=>c.hrac!=null && c.hrac.Id == hrac.Id))))
                {
                    await Clients.Client(connectionId).SendAsync("NastavText", "Nelze postavit cestu.");
                    return;
                }
                else
                {
                    cesta.hrac = hrac;
                    foreach (Rozcesti r in cesta.rozcesti)
                    {
                        if(r.Hrac == null && r.Cesty.Count(c=>c.hrac!=null && c.hrac.Id==hrac.Id)>1)
                        {
                            r.Hrac = hrac;
                            await Clients.Client(connectionId).SendAsync("NastavBarvu", r.Id.ToString(), stav.barva.Name);
                        }
                    }
                    await hra._dbContext.SaveChangesAsync();
                    hra.SmazAktivitu(a);
                    hra.PrepocitejNejdelsiCestu();
                    await hra._dbContext.SaveChangesAsync();
                    if (hra.AktualniAktivita() != null && hra.AktualniAktivita().Hrac.Id != hrac.Id)
                    {
                        await AktualizovatHru(hra.Id.ToString());
                    }
                    else
                    {
                        await Clients.Client(connectionId).SendAsync("NastavBarvu", Id, stav.barva.Name);
                        await Clients.Client(connectionId).SendAsync("ObnovSekci", HraId, "Body", hra.DejBodyHTML());
                        await Clients.Client(connectionId).SendAsync("ObnovSekci", HraId, "BodoveKarty", hra.DejBodoveKartyHTML(hrac));
                        await Clients.Client(connectionId).SendAsync("NastavText", hra.DejInstrukci(hrac));
                    }
                }
                
            }
            
        }

        // Metoda pro obsluhu kliknutí na políčko
        public async Task KliknutiNaPolicko(string connectionId, string Id, string HracId, string HraId)
        {
            Hra hra = await Hra.NactiHru(Guid.Parse(HraId), _dbContextFactory.CreateDbContext());
            Hrac hrac = hra.DejHrace(HracId);
            StavHrace stav = hra.DejStav(hrac);
            Pole pole = hra.mapka.Policka.FirstOrDefault(r => r.Id.ToString().Equals(Id));
            if (hra.stavHry != StavHry.Probiha || hrac.Id != hra.AktualniHrac().Id)
            {
                return;
            }
            Aktivita a = hra.AktualniAktivita();
            if (a == null) return;
            if (a.Akce == Instrukce.PresunZlodeje)
            {
                foreach (Pole p in hra.mapka.Policka)
                {
                    if (p.Blokovane)
                    {
                        await Clients.All.SendAsync("ZmenZlodeje", hra.Id.ToString(), p.Id.ToString());
                        p.Blokovane = false;
                    }
                    
                }
                    
                pole.Blokovane = true;
                await Clients.All.SendAsync("ZmenZlodeje", hra.Id.ToString(), pole.Id.ToString());
                if(pole.Rozcesti.Any(r=>r.Hrac!=null && r.Hrac.Id!=hrac.Id))
                {
                    hra.PridejAktivitu(new Aktivita()
                    {
                        Hrac = hrac,
                        Akce = Instrukce.VyberHrace
                    }) ;
                }
                hra.SmazAktivitu(a);
                
                await hra._dbContext.SaveChangesAsync();
                await AktualizovatHru(hra.Id.ToString());
            }
        }

        // Metoda pro obsluhu kliknutí na rozcestí
        public async Task KliknutiNaRozcesti(string connectionId, string Id, string HracId, string HraId)
        {
            Hra hra = await Hra.NactiHru(Guid.Parse(HraId), _dbContextFactory.CreateDbContext());
            Hrac hrac = hra.DejHrace(HracId);
            StavHrace stav = hra.DejStav(hrac);
            Rozcesti rozcesti = hra.mapka.Rozcesti.FirstOrDefault(r => r.Id.ToString().Equals(Id));
            if (hra.stavHry != StavHry.Probiha || hrac.Id != hra.AktualniHrac().Id)
            {
                return;
            }
            Aktivita a = hra.AktualniAktivita();
            if (a == null) return;
            if (a.Akce == Instrukce.StavbaVesnice)
            {
                if (rozcesti.Stavba != null ||
                   (rozcesti.Hrac != null && rozcesti.Hrac.Id != hrac.Id) ||
                    rozcesti.Blokovane)
                {
                    await Clients.Client(connectionId).SendAsync("NastavText", "Nelze postavit vesnici na cizím.");
                    return;
                }
                else if (stav.Stavby.Count() >= 2 && !rozcesti.Cesty.Any(c => c.hrac != null && c.hrac.Id == hrac.Id))
                {
                    await Clients.Client(connectionId).SendAsync("NastavText", "Jen první dvě vesnicky jde postavit bez cesty.");
                    return;
                }
                else
                {
                    rozcesti.Hrac = stav.hrac;
                    rozcesti.Stavba = hra.mapka.Stavby.FirstOrDefault(s => s.Nazev.Equals("Vesnice"));
                    hra.SmazAktivitu(a);
                    await hra._dbContext.SaveChangesAsync();
                    if (hra.AktualniAktivita() != null && hra.AktualniAktivita().Hrac.Id != hrac.Id)
                    {
                        await AktualizovatHru(hra.Id.ToString());
                    }
                    else
                    {
                        await Clients.Client(connectionId).SendAsync("NastavBarvu", Id, stav.barva.Name);
                        await Clients.Client(connectionId).SendAsync("NastavStavbu", Id, "vesnicka.svg");
                        await Clients.Client(connectionId).SendAsync("ObnovSekci", HraId, "Body", hra.DejBodyHTML());
                        await Clients.Client(connectionId).SendAsync("NastavText", hra.DejInstrukci(hrac));
                    }
                }
                return;
            }
            if (a.Akce == Instrukce.StavbaMesta)
            {
                if (rozcesti.Stavba == null ||
                    rozcesti.Stavba.Nazev !="Vesnice" ||
                    rozcesti.Hrac.Id != hrac.Id
                    )
                {
                    await Clients.Client(connectionId).SendAsync("NastavText", "Nelze postavit město mimo vlastní vesnici.");
                    return;
                }
                else
                {
                    rozcesti.Stavba = hra.mapka.Stavby.FirstOrDefault(s => s.Nazev.Equals("Město"));
                    hra.SmazAktivitu(a);
                    await hra._dbContext.SaveChangesAsync();
                    if (hra.AktualniAktivita() != null && hra.AktualniAktivita().Hrac.Id != hrac.Id)
                    {
                        await AktualizovatHru(hra.Id.ToString());
                    }
                    else
                    {
                        await Clients.Client(connectionId).SendAsync("NastavStavbu", Id, "mesto.svg");
                        await Clients.Client(connectionId).SendAsync("ObnovSekci", HraId, "Body", hra.DejBodyHTML());
                        await Clients.Client(connectionId).SendAsync("NastavText", hra.DejInstrukci(hrac));
                    }
                }

            }


        }

        // Metoda pro obsluhu kliknutí na nákup
        public async Task KliknutiNaNakup(string connectionId, string Id, string HracId, string HraId)
        {
            Hra hra = await Hra.NactiHru(Guid.Parse(HraId), _dbContextFactory.CreateDbContext());
            Hrac hrac = hra.DejHrace(HracId);
            StavHrace stav = hra.DejStav(hrac);
            if (hra.stavHry != StavHry.Probiha || hrac.Id!=hra.AktualniHrac().Id)
            {
                await Clients.Client(connectionId).SendAsync("NastavText", "Nákup není možný.");
                return;
            }
            if(hra.AktualniAktivita()!=null)
            {
                await Clients.Client(connectionId).SendAsync("NastavText", $"Nákup není možný:{hra.DejInstrukci(hrac)}");
                return;
            }
            Stavba kliknuta = hra.mapka.Stavby.FirstOrDefault(s => s.Id.ToString().Equals(Id));
            if (kliknuta == null)
            {
                await Clients.Client(connectionId).SendAsync("NastavText", "Nákup není možný.");
                return;
            }
            foreach(var sk in kliknuta.Cena)
            {
                if (stav.SurovinaKarty.FirstOrDefault(s => s.Surovina == sk.Surovina).Pocet < sk.Pocet)
                {
                    await Clients.Client(connectionId).SendAsync("NastavText", "Nákup není možný.");
                    return;
                }
            }

            switch (kliknuta.Nazev) {                 
                case "Vesnice":
                    if(!stav.MistaProVesnici)
                        await Clients.Client(connectionId).SendAsync("NastavText", "Není kam postavit.");
                    else
                    {
                        hra.PridejAktivitu(new Aktivita() { Hrac = hrac, Akce = Instrukce.StavbaVesnice });
                    }
                    
                    break;
                case "Město":
                    if (!stav.MistaProMesto)
                        await Clients.Client(connectionId).SendAsync("NastavText", "Není kam postavit.");
                    else
                    {
                        hra.PridejAktivitu(new Aktivita() { Hrac = hrac, Akce = Instrukce.StavbaMesta });
                    }
                    break;
                case "Cesta":
                    if (!stav.MistaProMesto)
                        await Clients.Client(connectionId).SendAsync("NastavText", "Není kam postavit.");
                    else
                    {
                        hra.PridejAktivitu(new Aktivita() { Hrac = hrac, Akce = Instrukce.StavbaCesty });
                    }
                    break;
                case "Akční karta":
                    if(hra.pocetNerozdanychKaret>0)
                    {
                        hra.DejAkcniKartu(hrac);
                    }
                    else
                    {
                        await Clients.Client(connectionId).SendAsync("NastavText", "Není žádná karta.");
                    }
                    break;
            }
            foreach (var sk in kliknuta.Cena)
            {
                stav.SurovinaKarty.FirstOrDefault(s => s.Surovina == sk.Surovina).Pocet -= sk.Pocet;
            }
            
            await hra._dbContext.SaveChangesAsync();
            await Clients.Client(connectionId).SendAsync("ObnovSekci", HraId, "akcni-karty", hra.DejAkcniKartyHTML(hrac));
            await Clients.Client(connectionId).SendAsync("ObnovSekci", HraId, "Suroviny", hra.DejSurovinoveKartyHTML(hrac));
            await Clients.Client(connectionId).SendAsync("ObnovSekci", HraId, "tlacitka", hra.DejTlacitkaHTML(hrac));
            await Clients.Client(connectionId).SendAsync("ObnovSekci", HraId, "nakup", hra.DejNakupHTML(hrac));
            await Clients.Client(connectionId).SendAsync("ObnovSekci", HraId, "smena-s-hrou", hra.DejSmenuHraHTML(hrac));
            await Clients.Client(connectionId).SendAsync("ObnovSekci", HraId, "smena-s-hraci", hra.DejSmenuHraciHTML(hrac));
            await Clients.Client(connectionId).SendAsync("NastavText", hra.DejInstrukci(hrac));
        }

        public async Task KliknutiNaSurovinu(string connectionId, string Id, string HracId, string HraId)
        {
            Hra hra = await Hra.NactiHru(Guid.Parse(HraId), _dbContextFactory.CreateDbContext());
            Hrac hrac = hra.DejHrace(HracId);
            StavHrace stav = hra.DejStav(hrac);
            SurovinaKarta sk = stav.SurovinaKarty.FirstOrDefault(s => s.Id.ToString().Equals(Id));
            if (hra.stavHry != StavHry.Probiha || hrac.Id != hra.AktualniHrac().Id)
            {
                return;
            }
            Aktivita a = hra.AktualniAktivita();
            if (a == null) return;
            if (a.Akce == Instrukce.VyberPrvniSurovinu || a.Akce == Instrukce.VyberDruhouSurovinu)
            {
                stav.SurovinaKarty.First(s => s.Surovina.Nazev == sk.Surovina.Nazev).Pocet++;
                hra.SmazAktivitu(a);
                await hra._dbContext.SaveChangesAsync();
                await Clients.Client(connectionId).SendAsync("ObnovSekci", HraId, "Suroviny", hra.DejSurovinoveKartyHTML(hrac));
                await Clients.Client(connectionId).SendAsync("ObnovSekci", HraId, "tlacitka", hra.DejTlacitkaHTML(hrac));
                await Clients.Client(connectionId).SendAsync("ObnovSekci", HraId, "nakup", hra.DejNakupHTML(hrac));
                await Clients.Client(connectionId).SendAsync("ObnovSekci", HraId, "smena-s-hrou", hra.DejSmenuHraHTML(hrac));
                await Clients.Client(connectionId).SendAsync("ObnovSekci", HraId, "smena-s-hraci", hra.DejSmenuHraciHTML(hrac));
                await Clients.Client(connectionId).SendAsync("NastavText", hra.DejInstrukci(hrac));
            }
            else if (a.Akce == Instrukce.VyberSurovinuMonopol)
            {
                foreach (var s in hra.stavy.Where(x => x.hrac.Id != hrac.Id))
                {

                    stav.SurovinaKarty.First(x => x.Surovina.Nazev == sk.Surovina.Nazev).Pocet += s.SurovinaKarty.First(x => x.Surovina.Nazev == sk.Surovina.Nazev).Pocet;
                    s.SurovinaKarty.First(x => x.Surovina.Nazev == sk.Surovina.Nazev).Pocet = 0;
                }
                hra.SmazAktivitu(a);
                await hra._dbContext.SaveChangesAsync();
                await AktualizovatHru(HraId);
            }
        }
        public async Task KliknutiNaHrace(string connectionId, string Id, string HracId, string HraId)
        {
            Hra hra = await Hra.NactiHru(Guid.Parse(HraId), _dbContextFactory.CreateDbContext());
            Hrac hrac = hra.DejHrace(HracId);
            StavHrace stav = hra.DejStav(hrac);
            Hrac cilovyHrac = hra.hraci.FirstOrDefault(s => s.Id.ToString().Equals(Id));
            if (hra.stavHry != StavHry.Probiha || hrac.Id != hra.AktualniHrac().Id)
            {
                return;
            }
            Aktivita a = hra.AktualniAktivita();
            if (a == null) return;
            if (a.Akce == Instrukce.VyberHrace)
            {
                var blokovanepolicko= hra.mapka.Policka.FirstOrDefault(p => p.Blokovane);
                if(blokovanepolicko.Rozcesti.Any(r=>r.Hrac!=null && r.Hrac.Id.ToString().Equals(Id)))
                {
                    Surovina sebrana = hra.SeberSurovinu(cilovyHrac);
                    stav.SurovinaKarty.First(s => s.Surovina.Nazev == sebrana.Nazev).Pocet++;
                    hra.SmazAktivitu(a);
                    await hra._dbContext.SaveChangesAsync();
                    await AktualizovatHru(HraId);
                }
                else
                {
                    await Clients.Client(connectionId).SendAsync("NastavText", "Nelze vybrat. Nesousedí s políčkem zloděje.");
                }
            }
        }
        public async Task KliknutiNaAkcniKartu(string connectionId, string Id, string HracId, string HraId)
        {
            Hra hra = await Hra.NactiHru(Guid.Parse(HraId), _dbContextFactory.CreateDbContext());
            Hrac hrac = hra.DejHrace(HracId);
            StavHrace stav = hra.DejStav(hrac);
            AkcniKarta ak = stav.AkcniKarty.FirstOrDefault(s => s.Id.ToString().Equals(Id));
            if (hra.stavHry != StavHry.Probiha || hrac.Id != hra.AktualniHrac().Id || hra.AktualniAktivita()!=null)
            {
                await Clients.Client(connectionId).SendAsync("NastavText", $"Zahrání není možné. {hra.DejInstrukci(hrac)}");
                return;
            }
            if (ak.Pocet > 0)
            {
                ak.Pocet--;
                ak.Zahraj(hra, hrac);
                await hra._dbContext.SaveChangesAsync();
                await AktualizovatHru(HraId);
            }
            
        }
        // Metoda pro aktualizaci herní stránky pro všechny klienty
        public async Task AktualizovatHru(string hraId)
        {
            await Clients.All.SendAsync("ObnovitStrankuHry", hraId);
        }
    }
}
