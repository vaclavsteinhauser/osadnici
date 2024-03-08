using Microsoft.AspNetCore.SignalR;
using WebOsadnici.Data;
using WebOsadnici.Models.HerniTridy;

namespace WebOsadnici.Hubs
{
    public class ClickHub : Hub
    {
        private readonly ApplicationDbContext _dbContext;

        /// <summary>
        /// Konstruktor třídy ClickHub.
        /// </summary>
        /// <param name="dbContext">Instance aplikačního kontextu pro přístup k databázi.</param>
        public ClickHub(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Metoda pro obsluhu kliknutí na tlačítko pro změnu
        public async Task tlacitko_smena_klik(string connectionId, string HracId, string HraId)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                // Načtení hry, hráče a stavu hráče
                Hra hra = await Hra.NactiHru(Guid.Parse(HraId), _dbContext);
                Hrac hrac = hra.hraci.FirstOrDefault(h => h.Id.ToString().Equals(HracId));
                StavHrace stav = hra.stavy.FirstOrDefault(s => s.hrac == hrac);

                // Odeslání zprávy klientovi
                await Clients.Client(connectionId).SendAsync("NastavText", "Kliknuto na Směnu");

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                // Zpracování výjimky
            }
        }

        // Metoda pro obsluhu kliknutí na tlačítko pro další akci
        public async Task tlacitko_dalsi_klik(string connectionId, string HracId, string HraId)
        {
            // Implementace
        }

        // Metoda pro obsluhu kliknutí na cestu
        public async Task KliknutiNaCestu(string connectionId, string Id, string HracId, string HraId)
        {
            // Implementace
        }

        // Metoda pro obsluhu kliknutí na políčko
        public async Task KliknutiNaPolicko(string connectionId, string Id, string HracId, string HraId)
        {
            await Clients.Client(connectionId).SendAsync("ObnovitSekci", "#Body");
        }

        // Metoda pro obsluhu kliknutí na rozcestí
        public async Task KliknutiNaRozcesti(string connectionId, string Id, string HracId, string HraId)
        {
            Hra hra = await Hra.NactiHru(Guid.Parse(HraId), _dbContext);
            Hrac hrac = hra.DejHrace(HracId);
            StavHrace stav = hra.DejStav(hrac);
            Rozcesti rozcesti = hra.mapka.Rozcesti.FirstOrDefault(r => r.Id.ToString().Equals(Id));
            rozcesti.Hrac = stav.hrac;
            rozcesti.Stavba = hra.mapka.Stavby.FirstOrDefault(s => s.Nazev.Equals("Vesnice"));
            List<Task> proved = new();
            proved.Add(Clients.Client(connectionId).SendAsync("NastavBarvu", Id, stav.barva.Name));
            proved.Add(Clients.Client(connectionId).SendAsync("NastavStavbu", Id, "vesnicka.svg"));
            proved.Add(Clients.Client(connectionId).SendAsync("ObnovBody", HraId, hra.DejBodyHTML()));
            proved.Add(_dbContext.SaveChangesAsync());
            await Task.WhenAll(proved);
            
        }

        // Metoda pro obsluhu kliknutí na nákup
        public async Task KliknutiNaNakup(string connectionId, string Id, string HracId, string HraId)
        {
            await Clients.Client(connectionId).SendAsync("ObnovitSekci", "#Body");
        }

        // Metoda pro aktualizaci herní stránky pro všechny klienty
        public async Task AktualizovatHru(string hraId)
        {
            await Clients.All.SendAsync("ObnovitStrankuHry", hraId);
        }
    }
}
