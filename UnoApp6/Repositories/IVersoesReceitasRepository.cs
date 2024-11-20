using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeDJRMWinUI3UNO.Repositories
{
    public interface IVersoesReceitasRepository
    {
        Task<int> AddAsync(VersoesReceitasModel versaoReceita); // Adiciona uma nova versão ao banco de dados.
        Task<VersoesReceitasModel> GetByIdAsync(int id); // Obtém uma versão pelo seu ID.
        Task<IEnumerable<VersoesReceitasModel>> GetByReceitaIdAsync(int idReceita); // Obtém todas as versões de uma receita.
        Task<bool> UpdateAsync(VersoesReceitasModel versaoReceita); // Atualiza os dados de uma versão existente.
        Task<bool> DeleteAsync(int id); // Remove uma versão do banco de dados.
    }
}
