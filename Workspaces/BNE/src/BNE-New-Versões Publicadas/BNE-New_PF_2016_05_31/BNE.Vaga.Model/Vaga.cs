using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Vaga.Model
{
    public class Vaga
    {
        public BNE.Vaga.Model.TipoContrato TipoContrato { get; set; }

        public decimal SalarioMinimo { get; set; }

        public decimal SalarioMaximo { get; set; }

        public int NumeroVagas { get; set; }

        public int IdCidade { get; set; }

        public int IdBairro { get; set; }

        public string Email { get; set; }

        public bool EmailCadaCandidatura { get; set; }

        public bool Confidencial { get; set; }

        public TelefoneVaga TelefoneVaga { get; set; }

        public bool EmailCandidaturasDiarias { get; set; }

        public List<BNE.Vaga.Model.Beneficio> Beneficios { get; set; }

        public string Descricao { get; set; }

        public BNE.Vaga.Model.Grupo Grupo
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public DateTime DataCadastro
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public DateTime DataPublicacao
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public int IdFuncao
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public BNE.Vaga.Model.Enumeradores.SituacaoVaga SituacaoVaga
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public Requisitos Requisitos
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
    }
}
