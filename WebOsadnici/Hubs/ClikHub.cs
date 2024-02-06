using Microsoft.AspNetCore.SignalR;

namespace WebOsadnici.Hubs
{
    public class ClickHub : Hub
    {
        public async Task ReceiveSvgId(string connectionId, string hraId, string svgId)
        {
            // Zpracování id nadřazeného svg elementu, například odeslání zpět klientovi

            await Clients.Client(connectionId).SendAsync("SvgIdReceived", svgId+hraId);
        }
    }
}