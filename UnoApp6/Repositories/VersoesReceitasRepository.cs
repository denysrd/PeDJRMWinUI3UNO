using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PeDJRMWinUI3UNO.Models;
using PeDJRMWinUI3UNO.Data;

namespace PeDJRMWinUI3UNO.Repositories
{
    public class VersoesReceitasRepository : IVersoesReceitasRepository
    {
        private readonly AppDbContext _dbContext; // Contexto do banco de dados

        public VersoesReceitasRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext; // Injeta o contexto no repositório
        }

        public async Task<int> AddAsync(VersoesReceitasModel versaoReceita)
        {
            _dbContext.VersoesReceitas.Add(versaoReceita); // Adiciona a versão ao DbSet
            await _dbContext.SaveChangesAsync(); // Salva as alterações no banco de dados
            return versaoReceita.Id; // Retorna o ID gerado
        }

        public async Task<VersoesReceitasModel> GetByIdAsync(int id)
        {
            // Obtém uma versão pelo ID, incluindo dados da receita associada
            return await _dbContext.VersoesReceitas
                .Include(v => v.Receita) // Inclui a entidade relacionada, se necessário
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<VersoesReceitasModel>> GetByReceitaIdAsync(int idReceita)
        {
            // Obtém todas as versões associadas a uma receita específica
            return await _dbContext.VersoesReceitas
                .Where(v => v.Id_Receita == idReceita)
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(VersoesReceitasModel versaoReceita)
        {
            var versaoExistente = await GetByIdAsync(versaoReceita.Id); // Obtém a versão existente pelo ID
            if (versaoExistente == null)
            {
                return false; // Retorna falso se a versão não for encontrada
            }

            // Atualiza os campos
            versaoExistente.Versao = versaoReceita.Versao;
            versaoExistente.Data = versaoReceita.Data;
            versaoExistente.Descricao_Processo = versaoReceita.Descricao_Processo;

            _dbContext.VersoesReceitas.Update(versaoExistente); // Marca a entidade como atualizada
            return await _dbContext.SaveChangesAsync() > 0; // Retorna true se a atualização for bem-sucedida
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var versao = await GetByIdAsync(id); // Obtém a versão pelo ID
            if (versao == null)
            {
                return false; // Retorna falso se a versão não for encontrada
            }

            _dbContext.VersoesReceitas.Remove(versao); // Remove a versão do DbSet
            return await _dbContext.SaveChangesAsync() > 0; // Retorna true se a remoção for bem-sucedida
        }
    }
}
