using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PeDJRMWinUI3UNO.Models;
using PeDJRMWinUI3UNO.Repositories;

namespace PeDJRMWinUI3UNO.Services
{
    /// <summary>
    /// Serviço responsável pela lógica de negócios para versões de receitas.
    /// </summary>
    public class VersoesReceitasService
    {
        private readonly VersoesReceitasRepository _repository;

        /// <summary>
        /// Construtor do serviço VersoesReceitasService.
        /// </summary>
        public VersoesReceitasService(VersoesReceitasRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Obtém todas as versões de receitas cadastradas.
        /// </summary>
        public async Task<List<VersoesReceitasModel>> GetAllVersoesReceitasAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// Obtém uma versão específica pelo ID.
        /// </summary>
        public async Task<VersoesReceitasModel> GetVersaoReceitaByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        /// <summary>
        /// Adiciona uma nova versão de receita.
        /// </summary>
        public async Task AddVersaoReceitaAsync(VersoesReceitasModel versaoReceita)
        {
            await _repository.AddAsync(versaoReceita);
        }

        /// <summary>
        /// Atualiza uma versão de receita existente.
        /// </summary>
        public async Task UpdateVersaoReceitaAsync(VersoesReceitasModel versaoReceita)
        {
            await _repository.UpdateAsync(versaoReceita);
        }

        /// <summary>
        /// Remove uma versão de receita pelo ID.
        /// </summary>
        public async Task DeleteVersaoReceitaAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
