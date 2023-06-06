using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TestMessage.Data;
using TestMessage.Models;

public class ChatHub : Hub
{
    private readonly AppDbContext dbContext;

    public ChatHub(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task CreateGroup(string groupName)
    {
        var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Username == Context.User.Identity.Name);
        var group = new Groups { Name = groupName };
        group.Users.Add(user);
        dbContext.Groups.Add(group);
        await dbContext.SaveChangesAsync();

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.All.SendAsync("CreateGroup", groupName);
    }

    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        await Clients.Group(groupName).SendAsync("UserJoined", Context.User.Identity.Name);
    }

    public async Task SendMessage(string content, string groupName)
    {
        var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Username == Context.User.Identity.Name);
        var group = await dbContext.Groups.Include(g => g.Users).SingleOrDefaultAsync(g => g.Name == groupName);

        if (group != null && group.Users.Contains(user))
        {
            var message = new Messages { Content = content, Sender = user, Group = group };
            dbContext.Messages.Add(message);
            await dbContext.SaveChangesAsync();

            await Clients.Group(groupName).SendAsync("ReceiveMessage", content, Context.User.Identity.Name);
        }
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();

        var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Username == Context.User.Identity.Name);
        if (user != null)
        {
            await Clients.Caller.SendAsync("UserJoined", "You");
        }
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);

        var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Username == Context.User.Identity.Name);
        if (user != null)
        {
            await Clients.All.SendAsync("UserLeft", user.Username);
        }
    }
}
