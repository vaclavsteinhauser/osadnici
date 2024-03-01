using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebOsadnici.Data;

namespace WebOsadnici.Hubs
{
    public class ClickHub : Hub

    {
        private readonly ApplicationDbContext _dbContext;
        public ClickHub(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task tlacitko_smena_klik(string connectionId, string HracId, string HraId)
        {
            Hra hra = Hra.NactiHru(Guid.Parse(HraId), _dbContext);
            Hrac hrac = hra.hraci.Where(h => h.Id.ToString().Equals(HracId)).Single();
            StavHrace stav = hra.stavy.Where(s=>s.hrac==hrac).Single();
            await Clients.Client(connectionId).SendAsync("NastavText", "Kliknuto na Směnu");
        }
        public async Task tlacitko_dalsi_klik(string connectionId, string HracId, string HraId)
        {
            Hra hra = Hra.NactiHru(Guid.Parse(HraId), _dbContext);
            Hrac hrac = hra.hraci.Where(h => h.Id.ToString().Equals(HracId)).Single();
            StavHrace stav = hra.stavy.Where(s => s.hrac == hrac).Single();
            await Clients.Client(connectionId).SendAsync("NastavText", "Kliknuto na Dalsi");
        }




        public async Task KliknutiNaCestu(string connectionId, string Id, string HraId)
        {
            Hra h = Hra.NactiHru(Guid.Parse(HraId), _dbContext);
            lock (h)
            {
                h.KliknutiNaCestu(Guid.Parse(Id), connectionId);

                
            }
        }
        public async Task KliknutiNaPolicko(string connectionId, string Id, string HraId)
        {

        }
        public async Task KliknutiNaRozcesti(string connectionId, string Id, string HraId)
        {
            Hra h = Hra.NactiHru(Guid.Parse(HraId),_dbContext);

            await Clients.Client(connectionId).SendAsync("NastavBarvu", Id, "red");
            
            await Clients.Client(connectionId).SendAsync("NastavStavbu",Id, _dbContext.stavby.Where(s=>s.Nazev.Equals("Vesnice")).First().ImageUrl);
        }
        public async Task AktualizovatHru(string hraId)
        {
            await Clients.All.SendAsync("ObnovitStrankuHry", hraId);
        }
    }
}