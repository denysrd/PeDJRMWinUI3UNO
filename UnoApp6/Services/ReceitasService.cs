using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PeDJRMWinUI3UNO.Models;
using PeDJRMWinUI3UNO.Repositories;

namespace PeDJRMWinUI3UNO.Services
{
    /// <summary>
    /// Serviço responsável pela lógica de negócios para receitas.
    /// </summary>
    public class ReceitasService
    {
        private readonly ReceitasRepository _repository;

        /// <summary>
        /// Construtor do serviço ReceitasService.
        /// </summary>
        public ReceitasService(ReceitasRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Obtém todas as receitas cadastradas.
        /// </summary>
        public async Task<List<ReceitasModel>> GetAllReceitasAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// Obtém uma receita específica pelo ID.
        /// </summary>
        public async Task<ReceitasModel> GetReceitaByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        /// <summary>
        /// Adiciona uma nova receita.
        /// </summary>
        public async Task AddReceitaAsync(ReceitasModel receita)
        {
            await _repository.AddAsync(receita);
        }

        /// <summary>
        /// Atualiza uma receita existente.
        /// </summary>
        public async Task UpdateReceitaAsync(ReceitasModel receita)
        {
            await _repository.UpdateAsync(receita);
        }

        /// <summary>
        /// Remove uma receita pelo ID.
        /// </summary>
        public async Task DeleteReceitaAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
