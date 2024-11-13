using PeDJRMWinUI3UNO.Repositories;

namespace PeDJRMWinUI3UNO.Services
{
    public class TipoFormulacaoService
    {
        private readonly TipoFormulacaoRepository _tipoFormulacaoRepository;

        public TipoFormulacaoService(TipoFormulacaoRepository tipoFormulacaoRepository)
        {
            _tipoFormulacaoRepository = tipoFormulacaoRepository;
        }

        // Encontra um tipo de formulação pelo ID
        public async Task<TipoFormulacaoModel> FindAsync(int id)
        {
            return await _tipoFormulacaoRepository.FindAsync(id);
        }

        // Salva um novo tipo de formulação
        public async Task<bool> SalvarAsync(TipoFormulacaoModel tipoFormulacao)
        {
            if (string.IsNullOrWhiteSpace(tipoFormulacao.Tipo_Formula))
            {
                // Validação básica antes de salvar
                return false;
            }

            return await _tipoFormulacaoRepository.SalvarAsync(tipoFormulacao);
        }

        // Atualiza um tipo de formulação existente
        public async Task<bool> AtualizarAsync(TipoFormulacaoModel tipoFormulacao)
        {
            if (tipoFormulacao.Id_Tipo_Formulacao == 0 || string.IsNullOrWhiteSpace(tipoFormulacao.Tipo_Formula))
            {
                // Validação básica antes de atualizar
                return false;
            }

            return await _tipoFormulacaoRepository.AtualizarAsync(tipoFormulacao);
        }

        // Remove um tipo de formulação pelo ID
        public async Task<bool> RemoverAsync(int id)
        {
            return await _tipoFormulacaoRepository.RemoverAsync(id);
        }

        // Retorna uma lista de todos os tipos de formulações
        public async Task<List<TipoFormulacaoModel>> ObterTodosAsync()
        {
            return await _tipoFormulacaoRepository.ObterTodosAsync();
        }
    }
}
