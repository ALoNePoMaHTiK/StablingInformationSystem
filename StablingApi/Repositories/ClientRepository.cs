using Microsoft.EntityFrameworkCore;
using StablingApi.Contexts;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;

namespace StablingApi.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly IDbContextFactory<ClientContext> _contextFactory;
        public ClientRepository(IDbContextFactory<ClientContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Client> Create(Client client)
        {
            ClientContext context = _contextFactory.CreateDbContext();
            context.Clients.Add(client);
            await context.SaveChangesAsync();
            return client;
        }
        public async Task Delete(int id)
        {
            ClientContext context = _contextFactory.CreateDbContext();
            Client clientToDelete = await context.Clients.FindAsync(id);
            context.Clients.Remove(clientToDelete);
            await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Client>> GetAll()
        {
            ClientContext context = _contextFactory.CreateDbContext();
            return await context.Clients.ToListAsync();
        }
        public async Task<Client> Get(int id)
        {
            ClientContext context = _contextFactory.CreateDbContext();
            return await context.Clients.FindAsync(id);
        }
        public async Task<IEnumerable<Client>> GetByAvailability(bool IsAvailable)
        {
            ClientContext context = _contextFactory.CreateDbContext();
            return await context.Clients.Where(client => client.IsAvailable == IsAvailable).ToListAsync();
        }
        public async Task<IEnumerable<Client>> GetByTrainer(int trainerId)
        {
            ClientContext context = _contextFactory.CreateDbContext();
            return await context.Clients.Where(client => client.TrainerId == trainerId).ToListAsync();
        }
        public async Task Update(Client client)
        {
            ClientContext context = _contextFactory.CreateDbContext();
            Client clientToUpdate = await context.Clients.FindAsync(client.ClientId);
            if (clientToUpdate != null)
            {
                clientToUpdate.FullName = client.FullName;
                clientToUpdate.Email = client.Email;
                clientToUpdate.PhoneNumber = client.PhoneNumber;
                clientToUpdate.TrainerId = client.TrainerId;
                clientToUpdate.IsAvailable = client.IsAvailable;
            }
            await context.SaveChangesAsync();
        }
    }
}