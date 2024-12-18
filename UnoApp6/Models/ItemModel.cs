using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PeDJRMWinUI3UNO.Models;

public class ItemModel : INotifyPropertyChanged
{
    // Campos privados para armazenar os valores
    private string _codigoInterno;
    private string _nome;
    private decimal _quantidade;
    private string _unidadeMedida;
    private decimal _porcentagem;
    private int _idinsumo;
    private int _idflavorizante;
    private int _idflavorizanteInterno;
    private int _idcar;

    // Construtor para inicializar os campos
    public ItemModel()
    {
        _codigoInterno = string.Empty; // Inicializa com string vazia
        _nome = string.Empty; // Inicializa com string vazia
        _quantidade = 0; // Inicializa com 0
        _unidadeMedida = string.Empty; // Inicializa com string vazia
        _porcentagem = 0; // Inicializa com 0
        _idinsumo = 0; // Inicializa com 0
        _idflavorizante = 0; // Inicializa com 0
        _idflavorizanteInterno = 0; // Inicializa com 0
        _idcar = 0; // Inicializa com 0
    }

    // Propriedades públicas
    public string CodigoInterno
    {
        get => _codigoInterno;
        set
        {
            _codigoInterno = value;
            OnPropertyChanged(); // Notifica mudanças
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

    public int Idinsumo
    {
        get => _idinsumo;
        set
        {
            _idinsumo = value;
            OnPropertyChanged();
        }
    }

    public int Idflavorizante
    {
        get => _idflavorizante;
        set
        {
            _idflavorizante = value;
            OnPropertyChanged();
        }
    }

    public int IdflavorizanteInterno
    {
        get => _idflavorizanteInterno;
        set
        {
            _idflavorizanteInterno = value;
            OnPropertyChanged();
        }
    }

    public int idcar
    {
        get => _idcar;
        set
        {
            _idcar = value;
            OnPropertyChanged();
        }
    }

    // Evento para notificar mudanças nas propriedades
    public event PropertyChangedEventHandler? PropertyChanged;

    // Método auxiliar para disparar o evento de mudança de propriedade
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Sobrescrita para exibir o nome na interface
    public override string ToString()
    {
        return Nome;
    }
}
