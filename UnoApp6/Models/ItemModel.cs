using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PeDJRMWinUI3UNO.Models
{
    public class ItemModel : INotifyPropertyChanged
    {
        private string _codigoInterno;
        private string _nome;
        private decimal _quantidade;
        private string _unidadeMedida;
        private decimal _porcentagem;

        public string CodigoInterno
        {
            get => _codigoInterno;
            set
            {
                _codigoInterno = value;
                OnPropertyChanged();
            }
        }

        public string Nome
        {
            get => _nome;
            set
            {
                _nome = value;
                OnPropertyChanged();
            }
        }

        public decimal Quantidade
        {
            get => _quantidade;
            set
            {
                _quantidade = value;
                OnPropertyChanged();
            }
        }

        public string UnidadeMedida
        {
            get => _unidadeMedida;
            set
            {
                _unidadeMedida = value;
                OnPropertyChanged();
            }
        }

        public decimal Porcentagem
        {
            get => _porcentagem;
            set
            {
                _porcentagem = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return Nome; // Exibe o nome do item
        }
    }
}
