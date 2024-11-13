using PeDJRMWinUI3UNO.Repositories;

namespace PeDJRMWinUI3UNO.Services
{
    public class TipoIngredienteService
    {
        private readonly TipoIngredienteRepository _tipoIngredienteRepository;

        public TipoIngredienteService(TipoIngredienteRepository tipoIngredienteRepository)
        {
            _tipoIngredienteRepository = tipoIngredienteRepository;
        }

        public async Task<List<TipoIngredienteModel>> ObterTodosAsync()
        {
            return await _tipoIngredienteRepository.ObterTodosAsync();
        }

        public async Task SalvarAsync(TipoIngredienteModel tipoIngrediente)
        {
            await _tipoIngredienteRepository.SalvarAsync(tipoIngrediente);
        }

        // Método para atualizar um registro existente de TipoIngrediente
        public async Task AtualizarAsync(TipoIngredienteModel tipoIngrediente)
        {
            await _tipoIngredienteRepository.AtualizarAsync(tipoIngrediente);
        }

        public async Task RemoverAsync(TipoIngredienteModel tipoIngrediente)
        {
            await _tipoIngredienteRepository.RemoverAsync(tipoIngrediente);
        }
    }
}
