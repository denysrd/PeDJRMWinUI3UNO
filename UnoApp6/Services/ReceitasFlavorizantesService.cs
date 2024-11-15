using System.Collections.Generic;
using System.Threading.Tasks;
using PeDJRMWinUI3UNO.Models;
using PeDJRMWinUI3UNO.Repositories;

namespace PeDJRMWinUI3UNO.Services
{
    /// <summary>
    /// Serviço responsável pela lógica de negócios para flavorizantes de receitas.
    /// </summary>
    public class ReceitasFlavorizantesService
    {
        private readonly ReceitasFlavorizantesRepository _repository;

        /// <summary>
        /// Construtor do serviço ReceitasFlavorizantesService.
        /// </summary>
        public ReceitasFlavorizantesService(ReceitasFlavorizantesRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Obtém todos os flavorizantes de receitas cadastrados.
        /// </summary>
        public async Task<List<ReceitasFlavorizantesInsumosModel>> GetAllReceitasFlavorizantesAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// Obtém um flavorizante de receita específico pelo ID.
        /// </summary>
        public async Task<ReceitasFlavorizantesInsumosModel> GetReceitaFlavorizanteByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        /// <summary>
        /// Adiciona um novo flavorizante para uma receita.
        /// </summary>
        public async Task AddReceitaFlavorizanteAsync(ReceitasFlavorizantesInsumosModel receitaFlavorizante)
        {
            await _repository.AddAsync(receitaFlavorizante);
        }

        /// <summary>
        /// Atualiza um flavorizante de receita existente.
        /// </summary>
        public async Task UpdateReceitaFlavorizanteAsync(ReceitasFlavorizantesInsumosModel receitaFlavorizante)
        {
            await _repository.UpdateAsync(receitaFlavorizante);
        }

        /// <summary>
        /// Remove um flavorizante de receita pelo ID.
        /// </summary>
        public async Task DeleteReceitaFlavorizanteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
