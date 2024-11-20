using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PeDJRMWinUI3UNO.Models
{
    public class ItemModel : INotifyPropertyChanged
    {
        // Campo privado para armazenar o código interno
        private string _codigoInterno;

        // Campo privado para armazenar o nome do item
        private string _nome;

        // Campo privado para armazenar a quantidade
        private decimal _quantidade;

        // Campo privado para armazenar a unidade de medida
        private string _unidadeMedida;

        // Campo privado para armazenar a porcentagem
        private decimal _porcentagem;

        // Campo privado para armazenar o ID do insumo
        private int _idinsumo;

        // Campo privado para armazenar o ID do flavorizante
        private int _idflavorizante;

        // Propriedade para o código interno (string)
        public string CodigoInterno
        {
            get => _codigoInterno;
            set
            {
                _codigoInterno = value;
                OnPropertyChanged(); // Notifica mudanças na propriedade
            }
        }

        // Propriedade para o nome (string)
        public string Nome
        {
            get => _nome;
            set
            {
                _nome = value;
                OnPropertyChanged(); // Notifica mudanças na propriedade
            }
        }

        // Propriedade para a quantidade (decimal)
        public decimal Quantidade
        {
            get => _quantidade;
            set
            {
                _quantidade = value;
                OnPropertyChanged(); // Notifica mudanças na propriedade
            }
        }

        // Propriedade para a unidade de medida (string)
        public string UnidadeMedida
        {
            get => _unidadeMedida;
            set
            {
                _unidadeMedida = value;
                OnPropertyChanged(); // Notifica mudanças na propriedade
            }
        }

        // Propriedade para a porcentagem (decimal)
        public decimal Porcentagem
        {
            get => _porcentagem;
            set
            {
                _porcentagem = value;
                OnPropertyChanged(); // Notifica mudanças na propriedade
            }
        }

        // Propriedade para o ID do insumo (int)
        public int Idinsumo
        {
            get => _idinsumo;
            set
            {
                _idinsumo = value;
                OnPropertyChanged(); // Notifica mudanças na propriedade
            }
        }

        // Propriedade para o ID do flavorizante (int)
        public int Idflavorizante
        {
            get => _idflavorizante;
            set
            {
                _idflavorizante = value;
                OnPropertyChanged(); // Notifica mudanças na propriedade
            }
        }

        // Evento para notificar mudanças nas propriedades
        public event PropertyChangedEventHandler PropertyChanged;

        // Método auxiliar para disparar o evento de mudança de propriedade
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Sobrescrita para exibir o nome no lugar do objeto na interface
        public override string ToString()
        {
            return Nome; // Retorna o nome do item
        }
    }
}
