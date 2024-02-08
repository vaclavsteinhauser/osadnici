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
        public async Task KliknutiNaCestu(string connectionId, string hraId, string Id)
        {
            Hra hra = _dbContext.hry.Where(h=>h.Id.ToString()==hraId).Include(h=>h.stavy).Single();
            await Clients.Client(connectionId).SendAsync("NastavBarvu", Id, hra.naTahu().barva.Name);
            await Clients.Client(connectionId).SendAsync("CestaKlikOdpoved", "Cesta: "+Id);
            hra.ZacniTah();
            _dbContext.SaveChanges();
        }
        public async Task KliknutiNaPolicko(string connectionId,string hraID, string Id)
        {

            await Clients.Client(connectionId).SendAsync("PolickoKlikOdpoved", "Policko: "+Id);
        }
        public async Task KliknutiNaRozcesti(string connectionId,string hraId, string Id)
        {
            Hra hra = _dbContext.hry.Where(h => h.Id.ToString() == hraId).Include(h => h.stavy).Single();
            await Clients.Client(connectionId).SendAsync("NastavBarvu", Id, hra.naTahu().barva.Name);
            await Clients.Client(connectionId).SendAsync("NastavVesnicku",Id);
            hra.ZacniTah();
            _dbContext.SaveChanges();
        }
    }
}