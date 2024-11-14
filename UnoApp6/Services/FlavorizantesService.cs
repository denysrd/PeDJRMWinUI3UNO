using PeDJRMWinUI3UNO.Repositories;
using PeDJRMWinUI3UNO.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PeDJRMWinUI3UNO.Services
{
    public class FlavorizantesService
    {
        private readonly FlavorizantesRepository _flavorizantesRepository;

        public FlavorizantesService(FlavorizantesRepository flavorizantesRepository)
        {
            _flavorizantesRepository = flavorizantesRepository;
        }

        public Task<int> ObterProximoIdAsync()
        {
            return _flavorizantesRepository.ObterProximoIdAsync();
        }

        public Task<List<FlavorizantesModel>> ObterTodosAsync()
        {
            return _flavorizantesRepository.ObterTodosAsync();
        }

        public Task<FlavorizantesModel> FindAsync(int id)
        {
            return _flavorizantesRepository.FindAsync(id);
        }

        public async Task<bool> SalvarAsync(FlavorizantesModel flavorizante)
        {
            return await _flavorizantesRepository.SalvarAsync(flavorizante);
        }

        public async Task<bool> AtualizarAsync(FlavorizantesModel flavorizante)
        {
            return await _flavorizantesRepository.AtualizarAsync(flavorizante);
        }

        public Task<bool> RemoverAsync(int id)
        {
            return _flavorizantesRepository.RemoverAsync(id);
        }
    }
}
