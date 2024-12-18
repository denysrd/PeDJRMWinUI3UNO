using System.Collections.Generic; // Importa suporte para coleções genéricas
using System.Linq; // Importa suporte para consultas LINQ
using System.Threading.Tasks; // Importa funcionalidades assíncronas
using Microsoft.EntityFrameworkCore; // Importa o suporte para Entity Framework Core
using Microsoft.EntityFrameworkCore.Infrastructure;
using PeDJRMWinUI3UNO.Data;
using PeDJRMWinUI3UNO.Models; // Importa o modelo ComponenteAromaticoModel
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace PeDJRMWinUI3UNO.Repositories // Define o namespace do repositório
{
    // Implementação do repositório de ComponenteAromatico
    public class ComponenteAromaticoRepository : IComponenteAromaticoRepository
    {
        // Contexto do banco de dados
        private readonly AppDbContext _context; // Representa o contexto do Entity Framework

        // Construtor que injeta o contexto do banco
        public ComponenteAromaticoRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); // Garante que o contexto não seja nulo
        }

        // Método para buscar todos os componentes aromáticos
        public async Task<IEnumerable<ComponenteAromaticoModel>> ObterTodosAsync()
        {
            //return await _context.ComponentesAromaticosModel.ToListAsync(); // Retorna todos os componentes do banco
            try
            {
                //using (var context = new AppDbContext(_context))
                {
                    return await _context.ComponentesAromaticosModel.AsNoTracking().ToListAsync();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao obter Componentes: {ex.Message}");
                return new List<ComponenteAromaticoModel>();
            }
        }

        // Método para buscar um componente aromático pelo ID
        public async Task<ComponenteAromaticoModel?> ObterPorIdAsync(int id)
        {
            return await _context.ComponentesAromaticosModel
                .FirstOrDefaultAsync(c => c.Id == id); // Retorna o componente correspondente ao ID
        }

        // Método para salvar um novo componente aromático
        public async Task<bool> SalvarAsync(ComponenteAromaticoModel componente)
        {
            
                if (componente.Id != 0)
                {
                    componente.Id = 0; // Ignora o ID existente ao salvar um novo componente
                }

                _context.Add(componente);
                return await _context.SaveChangesAsync() > 0;
            
        }

        // Método para atualizar um componente aromático existente
        public async Task<bool> AtualizarAsync(ComponenteAromaticoModel componente)
        {
            using (var context = _context.GetService<AppDbContext>())
            { 
                try
                {
                    // Verifica se o insumo existe no banco de dados antes de atualizar
                    var componenteExistente = await context.ComponentesAromaticosModel.AsNoTracking().FirstOrDefaultAsync(i => i.Id == componente.Id);

                    if (componenteExistente == null)
                    {
                        throw new DbUpdateConcurrencyException("O insumo que você está tentando atualizar foi excluído ou modificado.");
                    }

                // Atualiza o insumo no contexto para aplicar as modificações
                _context.Entry(componente).State = EntityState.Modified;

                    // Salva as mudanças
                    return await _context.SaveChangesAsync() > 0;
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new DbUpdateConcurrencyException("O insumo que você está tentando atualizar foi excluído ou modificado.");
                }
            }
        }
        // Método para excluir um componente aromático pelo ID
        public async Task ExcluirAsync(int id)
        {
            var componente = await ObterPorIdAsync(id); // Busca o componente pelo ID
            if (componente != null) // Verifica se o componente foi encontrado
            {
                _context.ComponentesAromaticosModel.Remove(componente); // Remove o componente do contexto
                await _context.SaveChangesAsync(); // Salva as alterações no banco
            }
        }

        // Método para obter o próximo ID disponível
        public async Task<int> ObterProximoIdAsync()
        {
            try
            {
                var ultimoInsumo = await _context.ComponentesAromaticosModel
                    .OrderByDescending(i => i.Id)
                    .FirstOrDefaultAsync();
                // Se não houver registros, retorna 1 como o próximo ID
                if (ultimoInsumo == null)
                {
                    Console.WriteLine("Nenhum registro encontrado. Retornando ID inicial 1.");
                    return 1;
                }

                return ultimoInsumo?.Id + 1 ?? 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao obter próximo ID: {ex.Message}");
                throw new InvalidOperationException("Erro ao calcular o próximo ID.", ex);
            }                       
        }
    }
}
