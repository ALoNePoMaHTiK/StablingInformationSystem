using Microsoft.EntityFrameworkCore;
using StablingApi.Contexts;
using StablingApi.Models;
using StablingApi.Repositories.Interfaces;

namespace StablingApi.Repositories
{
    public class AbonementTypeRepository : IAbonementTypeRepository
    {
        private readonly IDbContextFactory<AbonementsContext> _contextFactory;
        public AbonementTypeRepository(IDbContextFactory<AbonementsContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<AbonementType> Create(AbonementType type)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            await context.AbonementTypes.AddAsync(type);
            await context.SaveChangesAsync();
            return type;
        }

        public async Task Delete(int id)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            AbonementType typeToDelete = await context.AbonementTypes.FindAsync(id);
            context.AbonementTypes.Remove(typeToDelete);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<AbonementType>> GetAll()
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            return await context.AbonementTypes.ToListAsync();
        }

        public async Task<AbonementType> Get(int id)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            return await context.AbonementTypes.FindAsync(id);
        }

        public async Task Update(AbonementType type)
        {
            AbonementsContext context = _contextFactory.CreateDbContext();
            AbonementType typeToUpdate = await context.AbonementTypes.FindAsync(type.AbonementTypeId);
            if (typeToUpdate != null)
            {
                typeToUpdate.TypeName = type.TypeName;
                typeToUpdate.TypePrice = type.TypePrice;
                typeToUpdate.TypeUsesCount = type.TypeUsesCount;
                typeToUpdate.IsAvailable = type.IsAvailable;
                await context.SaveChangesAsync();
            }
        }
    }
}