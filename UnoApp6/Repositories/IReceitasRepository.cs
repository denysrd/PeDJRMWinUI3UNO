using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeDJRMWinUI3UNO.Repositories;

public interface IReceitasRepository
{
    Task<int> AddAsync(ReceitasModel receita);
    Task<ReceitasModel> GetByIdAsync(int id);
    Task<ReceitasModel> ObterPorCodigoAsync(string codigoReceita);
    Task<IEnumerable<ReceitasModel>> GetAllAsync();
    Task<bool> UpdateAsync(ReceitasModel receita);
    Task<bool> DeleteAsync(int id);
    Task<int> ObterProximoIdReceitaAsync();
    Task<bool> DeleteByVersaoReceitaIdAsync(int versaoReceitaId);
    /// Obtém uma receita pelo ID.
    /// <param name="id">O ID da receita.</param>
    /// <returns>Retorna a receita correspondente ao ID ou null se não for encontrada.</returns>
    Task<ReceitasModel?> ObterReceitaPorIdAsync(int id);
    Task DeleteByIdAsync(int id);
}
