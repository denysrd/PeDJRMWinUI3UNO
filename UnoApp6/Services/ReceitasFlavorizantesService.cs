using System.Collections.Generic;
using System.Threading.Tasks;
using PeDJRMWinUI3UNO.Models;
using PeDJRMWinUI3UNO.Repositories;

namespace PeDJRMWinUI3UNO.Services
{
    /// Serviço responsável pela lógica de negócios para flavorizantes de receitas.
    public class ReceitasFlavorizantesService
    {
        private readonly ReceitasFlavorizantesRepository _repository;

        /// Construtor do serviço ReceitasFlavorizantesService.
        public ReceitasFlavorizantesService(ReceitasFlavorizantesRepository repository)
        {
            _repository = repository;
        }

        /// Obtém todos os flavorizantes de receitas cadastrados.
        public async Task<List<ReceitasFlavorizantesModel>> GetAllReceitasFlavorizantesAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// Obtém um flavorizante de receita específico pelo ID.
        public async Task<ReceitasFlavorizantesModel> GetReceitaFlavorizanteByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        /// Adiciona um novo flavorizante para uma receita.
        public async Task AddReceitaFlavorizanteAsync(ReceitasFlavorizantesModel receitaFlavorizante)
        {
            await _repository.AddAsync(receitaFlavorizante);
        }
          
        /// Atualiza um flavorizante de receita existente.
        public async Task UpdateReceitaFlavorizanteAsync(ReceitasFlavorizantesModel receitaFlavorizante)
        {
            await _repository.UpdateAsync(receitaFlavorizante);
        }

        /// Remove um flavorizante de receita pelo ID.
        public async Task DeleteReceitaFlavorizanteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
