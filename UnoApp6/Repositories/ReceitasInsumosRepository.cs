using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PeDJRMWinUI3UNO.Data;
using PeDJRMWinUI3UNO.Models;

namespace PeDJRMWinUI3UNO.Repositories
{
    // Implementação do repositório para manipulação da tabela tbl_receitas_insumos
    public class ReceitasInsumosRepository : IReceitasInsumosRepository
    {
        // Contexto do banco de dados
        private readonly AppDbContext _dbContext;

        // Injeta o contexto no repositório
        public ReceitasInsumosRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Adiciona um novo registro na tabela tbl_receitas_insumos
        public async Task<int> AddAsync(ReceitasInsumosModel receitaInsumo)
        {
            _dbContext.ReceitasInsumos.Add(receitaInsumo); // Adiciona o registro ao DbSet
            await _dbContext.SaveChangesAsync(); // Salva as alterações no banco
            return receitaInsumo.Id; // Retorna o ID gerado
        }

        // Obtém um registro da tabela tbl_receitas_insumos pelo ID
        public async Task<ReceitasInsumosModel> GetByIdAsync(int id)
        {
            return await _dbContext.ReceitasInsumos
                .Include(ri => ri.VersaoReceita) // Inclui a entidade relacionada de versão
                .Include(ri => ri.Insumo) // Inclui a entidade relacionada de insumo
                .Include(ri => ri.Flavorizante) // Inclui a entidade relacionada de flavorizante
                .FirstOrDefaultAsync(ri => ri.Id == id); // Filtra pelo ID
        }

        // Obtém todos os registros relacionados a uma versão específica de receita
        public async Task<IEnumerable<ReceitasInsumosModel>> GetByVersaoReceitaIdAsync(int idVersaoReceita)
        {
            return await _dbContext.ReceitasInsumos
                .Where(ri => ri.Id_Versao_Receita == idVersaoReceita) // Filtra pela versão
                .Include(ri => ri.Insumo) // Inclui informações do insumo
                .Include(ri => ri.Flavorizante) // Inclui informações do flavorizante
                .ToListAsync(); // Retorna como uma lista
        }

        // Atualiza um registro existente na tabela tbl_receitas_insumos
        public async Task<bool> UpdateAsync(ReceitasInsumosModel receitaInsumo)
        {
            var receitaInsumoExistente = await GetByIdAsync(receitaInsumo.Id); // Busca o registro pelo ID
            if (receitaInsumoExistente == null)
            {
                return false; // Retorna falso se o registro não for encontrado
            }

            // Atualiza os campos necessários
            receitaInsumoExistente.Id_Versao_Receita = receitaInsumo.Id_Versao_Receita;
            receitaInsumoExistente.Id_Insumo = receitaInsumo.Id_Insumo;
            receitaInsumoExistente.Unidade_Medida = receitaInsumo.Unidade_Medida;
            receitaInsumoExistente.Quantidade = receitaInsumo.Quantidade;
            receitaInsumoExistente.Id_Flavorizante = receitaInsumo.Id_Flavorizante;

            _dbContext.ReceitasInsumos.Update(receitaInsumoExistente); // Marca o registro como atualizado
            return await _dbContext.SaveChangesAsync() > 0; // Salva as alterações e retorna true se bem-sucedido
        }

        // Remove um registro da tabela tbl_receitas_insumos pelo ID
        public async Task<bool> DeleteAsync(int id)
        {
            var receitaInsumo = await GetByIdAsync(id); // Busca o registro pelo ID
            if (receitaInsumo == null)
            {
                return false; // Retorna falso se o registro não for encontrado
            }

            _dbContext.ReceitasInsumos.Remove(receitaInsumo); // Remove o registro do DbSet
            return await _dbContext.SaveChangesAsync() > 0; // Salva as alterações e retorna true se bem-sucedido
        }
    }
}
