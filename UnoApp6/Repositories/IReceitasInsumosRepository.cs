using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeDJRMWinUI3UNO.Repositories
{
    
    public interface IReceitasInsumosRepository // Contrato para manipulação dos dados de receitas insumos no banco de dados
    {       
        Task<int> AddAsync(ReceitasInsumosModel receitaInsumo); // Adiciona um novo insumo à receita no banco de dados
        Task<ReceitasInsumosModel> GetByIdAsync(int id); // Obtém um insumo de receita pelo ID
        Task<IEnumerable<ReceitasInsumosModel>> GetByVersaoReceitaIdAsync(int idVersaoReceita); // Obtém todos os insumos de uma versão específica de receita
        Task<bool> UpdateAsync(ReceitasInsumosModel receitaInsumo); // Atualiza os dados de um insumo de receita existente       
        Task<bool> DeleteAsync(int id); // Remove um insumo de receita pelo ID
    }
}
