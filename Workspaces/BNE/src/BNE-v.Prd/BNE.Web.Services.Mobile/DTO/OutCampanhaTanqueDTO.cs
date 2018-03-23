using System;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    public class OutCampanhaTanqueDTO
    {
        [DataMember(Name ="cr")]
        public int idCampanhaRecrutamento;

        //[DataMember(Name = "qt")]
        //public Int16 quantidadeRetorno;

        [DataMember(Name = "tcr")]
        public int tipoRetornoCampanhaRecrutamento;

        //[DataMember(Name ="dc")]
        //public DateTime dataCadastro;

        //[DataMember(Name ="pc")]
        //public int idPesquisaCurriculo;

        //[DataMember(Name = "cf")]
        //public int IdmotivoCampanhaFinalizada;

        [DataMember(Name = "ddd")]
        public decimal? NumeroDDDTelefoneContato;

        [DataMember(Name = "tel")]
        public decimal? NUMTelefoneContato;

        [DataMember(Name = "e")]
        public string erro { get; set; }

        [DataMember(Name = "s")]
        public int Status { get; set; }
    }
}