using Microsoft.EntityFrameworkCore;
using StablingApi.Models;

namespace StablingApi.Contexts
{
    public class ClientContext : DbContext
    {
        public ClientContext(DbContextOptions<ClientContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Client> Clients { get; set; }
    }
}