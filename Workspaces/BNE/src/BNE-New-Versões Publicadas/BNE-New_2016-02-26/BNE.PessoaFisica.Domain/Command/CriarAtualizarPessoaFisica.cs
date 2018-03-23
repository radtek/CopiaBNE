using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.PessoaFisica.Domain.Command
{
    public class CriarAtualizarPessoaFisica
    {
        public int Id { get; set; }
        public decimal CPF { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public byte DDDCelular { get; set; }
        public decimal Celular { get; set; }
        public int VagaId { get; set; }
        public int? IdVaga { get; set; }
        public int IdSexo { get; set; }
        public int IdCidade { get; set; }
        public int IdEscolaridade { get; set; }
        public int IdDeficiencia { get; set; }


        public bool FlgInativo { get; set; }
        
        public Curriculo Curriculo { get; set; }
    }
}