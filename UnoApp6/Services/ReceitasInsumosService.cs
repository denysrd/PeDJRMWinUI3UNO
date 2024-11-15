using System.Collections.Generic;
using System.Threading.Tasks;
using PeDJRMWinUI3UNO.Models;
using PeDJRMWinUI3UNO.Repositories;

namespace PeDJRMWinUI3UNO.Services
{
    /// <summary>
    /// Serviço responsável pela lógica de negócios para insumos de receitas.
    /// </summary>
    public class ReceitasInsumosService
    {
        private readonly ReceitasInsumosRepository _repository;

        /// <summary>
        /// Construtor do serviço ReceitasInsumosService.
        /// </summary>
        public ReceitasInsumosService(ReceitasInsumosRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Obtém todos os insumos de receitas cadastrados.
        /// </summary>
        public async Task<List<ReceitasInsumosModel>> GetAllReceitasInsumosAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// Obtém um insumo de receita específico pelo ID.
        /// </summary>
        public async Task<ReceitasInsumosModel> GetReceitaInsumoByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        /// <summary>
        /// Adiciona um novo insumo para uma receita.
        /// </summary>
        public async Task AddReceitaInsumoAsync(ReceitasInsumosModel receitaInsumo)
        {
            await _repository.AddAsync(receitaInsumo);
        }

        /// <summary>
        /// Atualiza um insumo de receita existente.
        /// </summary>
        public async Task UpdateReceitaInsumoAsync(ReceitasInsumosModel receitaInsumo)
        {
            await _repository.UpdateAsync(receitaInsumo);
        }

        /// <summary>
        /// Remove um insumo de receita pelo ID.
        /// </summary>
        public async Task DeleteReceitaInsumoAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
