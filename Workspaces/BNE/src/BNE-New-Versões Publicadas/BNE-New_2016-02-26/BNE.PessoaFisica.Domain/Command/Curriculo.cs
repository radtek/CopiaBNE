using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.PessoaFisica.Domain.Command
{
    public class Curriculo
    {
        public int Id { get; set; }
        public decimal PretensaoSalarial { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public bool FlgDisponivelViagem { get; set; }

        public bool FlgVIP { get; set; }
        public bool FlgInativo { get; set; }
        public string Observacao { get; set; }
        public string Conhecimento { get; set; }

        public int IdTipoCurriculo { get; set; }
        public int IdSituacaoCurriculo { get; set; }
        public int IdOrigem { get; set; }

        public FuncaoPretendida FuncaoPretendida { get; set; }
    }

    public class FuncaoPretendida
    {
        public int? idFuncao { get; set; }
        public short? TempoExperiencia { get; set; }
        public string Descricao { get; set; }
    }
}