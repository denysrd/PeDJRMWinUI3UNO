using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KeyAttribute = System.ComponentModel.DataAnnotations.KeyAttribute;

namespace PeDJRMWinUI3UNO.Models
{
    public class VersoesReceitasModel
    {
        public int Id { get; set; } // Identificador único da versão (chave primária).
        public int Id_Receita { get; set; } // Identificador da receita associada (chave estrangeira para tbl_receitas).
        public decimal Versao { get; set; } // Número da versão da receita.
        public DateTime Data { get; set; } // Data de criação da versão.
        public string Descricao_Processo { get; set; } // Descrição detalhada do processo da versão.

        public ReceitasModel Receita { get; set; } // Relacionamento com a entidade de receitas (opcional).
    }
}

