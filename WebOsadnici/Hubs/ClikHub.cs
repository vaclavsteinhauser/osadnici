using Microsoft.AspNetCore.SignalR;
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
        public async Task KliknutiNaCestu(string connectionId, string Id)
        {
            // Zpracování id nadřazeného svg elementu, například odeslání zpět klientovi

            await Clients.Client(connectionId).SendAsync("CestaKlikOdpoved", "Cesta: "+Id);
        }
        public async Task KliknutiNaPolicko(string connectionId, string Id)
        {
            // Zpracování id nadřazeného svg elementu, například odeslání zpět klientovi

            await Clients.Client(connectionId).SendAsync("PolickoKlikOdpoved", "Policko: "+Id);
        }
        public async Task KliknutiNaRozcesti(string connectionId, string Id)
        {
            // Zpracování id nadřazeného svg elementu, například odeslání zpět klientovi

            //await Clients.Client(connectionId).SendAsync("RozcestiKlikOdpoved", "Rozcesti: "+Id);
            await Clients.Client(connectionId).SendAsync("NastavBarvu", Id, "red");
            await Clients.Client(connectionId).SendAsync("NastavVesnicku",Id);
        }
    }
}