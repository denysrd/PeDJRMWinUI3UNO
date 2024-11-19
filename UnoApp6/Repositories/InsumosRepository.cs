using Microsoft.EntityFrameworkCore;
using PeDJRMWinUI3UNO.Data;
using PeDJRMWinUI3UNO.Models; // Certifique-se de incluir o namespace para os modelos
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PeDJRMWinUI3UNO.Repositories
{
    public class InsumosRepository
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;

        public InsumosRepository(DbContextOptions<AppDbContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        // Método para obter o próximo ID disponível
        public async Task<int> ObterProximoIdAsync()
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var ultimoInsumo = await context.InsumosModel.OrderByDescending(i => i.Id_Insumo).FirstOrDefaultAsync();
                return ultimoInsumo?.Id_Insumo + 1 ?? 1; // Se não houver registros, começa com 1
            }
        }

        // Método para obter todos os insumos
        public async Task<List<InsumosModel>> ObterTodosAsync()
        {
            try
            {
                using (var context = new AppDbContext(_dbContextOptions))
            {
                return await context.InsumosModel.ToListAsync();
            }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao obter insumos: {ex.Message}");
                return new List<InsumosModel>();
            }
        }

        // Método para encontrar um insumo pelo ID
        public async Task<InsumosModel> FindAsync(int idInsumo)
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                return await context.InsumosModel.FindAsync(idInsumo);
            }
        }

        // Método para salvar um novo insumo
        public async Task<bool> SalvarAsync(InsumosModel insumo)
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                if (insumo.Id_Insumo != 0)
                {
                    insumo.Id_Insumo = 0; // Ignora o ID existente ao salvar um novo insumo
                }

                context.InsumosModel.Add(insumo);
                return await context.SaveChangesAsync() > 0;
            }
        }


        // Método para atualizar um insumo existente com verificação de concorrência
        public async Task<bool> AtualizarAsync(InsumosModel insumo)
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                try
                {
                    // Verifica se o insumo existe no banco de dados antes de atualizar
                    var insumoExistente = await context.InsumosModel.AsNoTracking().FirstOrDefaultAsync(i => i.Id_Insumo == insumo.Id_Insumo);

                    if (insumoExistente == null)
                    {
                        throw new DbUpdateConcurrencyException("O insumo que você está tentando atualizar foi excluído ou modificado.");
                    }

                    // Atualiza o insumo no contexto para aplicar as modificações
                    context.Entry(insumo).State = EntityState.Modified;

                    // Salva as mudanças
                    return await context.SaveChangesAsync() > 0;
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new DbUpdateConcurrencyException("O insumo que você está tentando atualizar foi excluído ou modificado.");
                }
            }
        }



        // Método para remover um insumo pelo ID
        public async Task<bool> RemoverAsync(int id)
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var insumo = await context.InsumosModel.FindAsync(id);
                if (insumo != null)
                {
                    context.InsumosModel.Remove(insumo);
                    return await context.SaveChangesAsync() > 0;
                }
                return false;
            }
        }
    }
}
