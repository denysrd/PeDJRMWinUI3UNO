using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PeDJRMWinUI3UNO.Data;
using PeDJRMWinUI3UNO.Models;
using PeDJRMWinUI3UNO.Data;
using PeDJRMWinUI3UNO.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace PeDJRMWinUI3UNO
{
    public class ReceitasRepository : IReceitasRepository
    {
        private readonly AppDbContext _dbContext;

        public ReceitasRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
                
        public async Task<int> AddAsync(ReceitasModel receita)
        {
            _dbContext.ReceitasModel.Add(receita);
            await _dbContext.SaveChangesAsync();
            return receita.Id; // Retorna o ID gerado
        }

        public async Task<ReceitasModel> GetByIdAsync(int id)
        {
            return await _dbContext.ReceitasModel.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ReceitasModel> ObterPorCodigoAsync(string codigoReceita)
        {
            return await _dbContext.ReceitasModel.FirstOrDefaultAsync(r => r.Codigo_Receita == codigoReceita);
        }

        public async Task<IEnumerable<ReceitasModel>> GetAllAsync()
        {
            return await _dbContext.ReceitasModel.ToListAsync();
        }

        public async Task<bool> UpdateAsync(ReceitasModel receita)
        {
            var receitaExistente = await GetByIdAsync(receita.Id);
            if (receitaExistente == null)
                return false;

            receitaExistente.Codigo_Receita = receita.Codigo_Receita;
            receitaExistente.Nome_Receita = receita.Nome_Receita;
            receitaExistente.Data = receita.Data;
            receitaExistente.Descricao_Processo = receita.Descricao_Processo;

            _dbContext.ReceitasModel.Update(receitaExistente);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var receita = await GetByIdAsync(id);
            if (receita == null)
                return false;

            _dbContext.ReceitasModel.Remove(receita);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        // Método para obter o próximo ID disponível
        public async Task<int> ObterProximoIdReceitaAsync()
        {
            
                var ultimaReceita = await _dbContext.ReceitasModel.OrderByDescending(i => i.Id).FirstOrDefaultAsync();
                return ultimaReceita?.Id + 1 ?? 1; // Se não houver registros, começa com 1
            
        }
    }
}
