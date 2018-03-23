namespace API.GatewayV2.APIModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Usuario")]
    public partial class Usuario
    {
        public Usuario()
        {
            Requisicao = new HashSet<Requisicao>();
        }

        [Key]
        public int Idf_Usuario { get; set; }

        public decimal Num_CPF { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_CNPJ { get; set; }

        [StringLength(10)]
        public string Senha { get; set; }

        [Column(TypeName = "date")]
        public DateTime Dta_Nascimento { get; set; }

        public int Idf_Perfil { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public bool Flg_Inativo { get; set; }

        [Column(TypeName = "date")]
        public DateTime Dta_Inicio_Plano { get; set; }

        public virtual Perfil Perfil { get; set; }

        public virtual ICollection<Requisicao> Requisicao { get; set; }

    }
}
