using System.Collections.Generic;
using System.Threading.Tasks;
using PeDJRMWinUI3UNO.Models;
using PeDJRMWinUI3UNO.Repositories;

namespace PeDJRMWinUI3UNO.Services
{
    // Serviço responsável por encapsular a lógica de negócios para receitas insumos
    public class ReceitasInsumosService
    {
        // Repositório de receitas insumos
        private readonly IReceitasInsumosRepository _receitasInsumosRepository;

        // Injeta o repositório no serviço
        public ReceitasInsumosService(IReceitasInsumosRepository receitasInsumosRepository)
        {
            _receitasInsumosRepository = receitasInsumosRepository;
        }

        // Adiciona um novo insumo à receita
        public async Task<int> AdicionarReceitaInsumoAsync(ReceitasInsumosModel receitaInsumo)
        {
            // Valida o ID da versão da receita
            if (receitaInsumo.Id_Versao_Receita <= 0)
            {
                throw new ArgumentException("ID da versão da receita é obrigatório.");
            }
            
            // Adiciona o registro no banco de dados
            return await _receitasInsumosRepository.AddAsync(receitaInsumo);
        }

        // Obtém um insumo de receita pelo ID
        public async Task<ReceitasInsumosModel> ObterPorIdAsync(int id)
        {
            return await _receitasInsumosRepository.GetByIdAsync(id);
        }

        // Obtém todos os insumos de uma versão específica de receita
        public async Task<IEnumerable<ReceitasInsumosModel>> ObterPorVersaoReceitaIdAsync(int idVersaoReceita)
        {
            return await _receitasInsumosRepository.GetByVersaoReceitaIdAsync(idVersaoReceita);
        }

        // Atualiza os dados de um insumo de receita
        public async Task<bool> AtualizarReceitaInsumoAsync(ReceitasInsumosModel receitaInsumo)
        {
            if (receitaInsumo.Id <= 0)
            {
                throw new ArgumentException("ID do insumo de receita é obrigatório.");
            }

            return await _receitasInsumosRepository.UpdateAsync(receitaInsumo);
        }

        // Remove um insumo de receita pelo ID
        public async Task<bool> RemoverReceitaInsumoAsync(int id)
        {
            return await _receitasInsumosRepository.DeleteAsync(id);
        }
    }
}
