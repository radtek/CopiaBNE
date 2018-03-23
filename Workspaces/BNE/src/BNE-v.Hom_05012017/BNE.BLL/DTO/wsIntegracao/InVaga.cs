using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BNE.BLL.DTO.wsIntegracao
{
    [DataContract]
    public class InVaga
    {
        [DataMember(Name = "id")]
        public int IdVaga { get; set; }

        [DataMember(Name = "cod")]
        public string CodigoVaga { get; set; }

        [DataMember(Name = "nmeEmpresa")]
        public string NomeEmpresa { get; set; }

        [DataMember(Name = "emailVaga")]
        public string EmailVaga { get; set; }

        [DataMember(Name = "qtdVagas")]
        public int QuantidadeVaga { get; set; }

        [DataMember(Name = "desRequisitos")]
        public string DescricaoRequisito { get; set; }

        [DataMember(Name = "desAtributos")]
        public string DescricaoAtribuicoes { get; set; }

        [DataMember(Name = "desBeneficio")]
        public string DescricaoBeneficio { get; set; }

        [DataMember(Name = "dtAbertura")]
        public string DataAbertura { get; set; }

        [DataMember(Name = "dtPrazo")]
        public string DataPrazo { get; set; }

        [DataMember(Name = "vlrSalIni")]
        public decimal? ValorSalarioInicial { get; set; }

        [DataMember(Name = "vlrSalFim")]
        public decimal? ValorSalarioFinal { get; set; }

        [DataMember(Name = "desFunc")]
        public string DescricaoFuncao { get; set; }

        [DataMember(Name = "desCidade")]
        public string DescricaoCidade { get; set; }

        [DataMember(Name = "confidencial")]
        public bool FlagConfidencial { get; set; }

        [DataMember(Name = "desEscolaridade")]
        public string DescricaoEscolaridade { get; set; }

        [DataMember(Name = "desDeficiencia")]
        public string DescricaoDeficiencia { get; set; }

        [DataMember(Name = "desDisponibilidade")]
        public string DescricaoDisponibilidade { get; set; }

        [DataMember(Name = "desTipoVinculo")]
        public string DescricaoTipoVinculo { get; set; }

        [DataMember(Name = "numDDD")]
        public string NumeroDDD { get; set; }

        [DataMember(Name = "numFone")]
        public string NumeroTelefone { get; set; }

        [DataMember(Name = "idadeIni")]
        public string NumeroIdadeMinima { get; set; }

        [DataMember(Name = "idadeMax")]
        public string NumeroIdadeMaxima { get; set; }

        [DataMember(Name = "inativa")]
        public string Inativa { get; set; }

        
    }
}
