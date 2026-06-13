using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HortaGestao.Infrastructure.Messaging;

[Authorize]
public class ImportHub : Hub
{
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"ConnectionId: {Context.ConnectionId}");
            Console.WriteLine($"UserIdentifier: {Context.UserIdentifier}");
        

            foreach (var claim in Context.User.Claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }

            await base.OnConnectedAsync();
        }
}