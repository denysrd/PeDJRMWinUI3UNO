using Microsoft.EntityFrameworkCore;
using PeDJRMWinUI3UNO.Data;
using PeDJRMWinUI3UNO.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeDJRMWinUI3UNO.Repositories
{
    public class FlavorizantesRepository
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;

        public FlavorizantesRepository(DbContextOptions<AppDbContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        public async Task<int> ObterProximoIdAsync()
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var ultimo = await context.FlavorizantesModel.OrderByDescending(f => f.Id_Flavorizante).FirstOrDefaultAsync();
                return ultimo?.Id_Flavorizante + 1 ?? 1;
            }
        }

        public async Task<List<FlavorizantesModel>> ObterTodosAsync()
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                return await context.FlavorizantesModel.ToListAsync();
            }
        }

        public async Task<FlavorizantesModel> FindAsync(int id)
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                return await context.FlavorizantesModel.FindAsync(id);
            }
        }

        public async Task<bool> SalvarAsync(FlavorizantesModel flavorizante)
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                context.FlavorizantesModel.Add(flavorizante);
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> AtualizarAsync(FlavorizantesModel flavorizante)
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                context.Entry(flavorizante).State = EntityState.Modified;
                return await context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> RemoverAsync(int id)
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var flavorizante = await context.FlavorizantesModel.FindAsync(id);
                if (flavorizante != null)
                {
                    context.FlavorizantesModel.Remove(flavorizante);
                    return await context.SaveChangesAsync() > 0;
                }
                return false;
            }
        }
    }
}
