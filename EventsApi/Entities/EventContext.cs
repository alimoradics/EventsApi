namespace WebApi.Entities;

using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

public class EventContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public EventContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public EventContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // in memory database used for simplicity, change to a real db for production applications
        options.UseInMemoryDatabase("TestDb");
    }

    public virtual DbSet<Event>? Events { get; set; }
}
