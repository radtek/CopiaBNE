namespace API.GatewayV2.APIModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Requisicao")]
    public partial class Requisicao
    {
        [Key]
        public long Idf_Requisicao { get; set; }

        public int Idf_Usuario { get; set; }

        public int Idf_Endpoint { get; set; }

        public string Conteudo { get; set; }

        public byte Idf_Metodo { get; set; }

        public double Tempo_Execucao { get; set; }

        public int Codigo_Respsota { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public Guid? Idf_Cliente { get; set; }

        public virtual Cliente Cliente { get; set; }

        public virtual Endpoint Endpoint { get; set; }

        public virtual Metodo Metodo { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
