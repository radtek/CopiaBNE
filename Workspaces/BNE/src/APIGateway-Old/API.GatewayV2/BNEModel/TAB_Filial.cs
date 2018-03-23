namespace API.GatewayV2.BNEModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Filial")]
    public partial class TAB_Filial
    {
        public TAB_Filial()
        {
            TAB_Usuario_Filial_Perfil = new HashSet<TAB_Usuario_Filial_Perfil>();
        }

        [Key]
        public int Idf_Filial { get; set; }

        public bool Flg_Matriz { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_CNPJ { get; set; }

        [Required]
        [StringLength(100)]
        public string Raz_Social { get; set; }

        [Required]
        [StringLength(100)]
        public string Nme_Fantasia { get; set; }

        public int? Idf_CNAE_Principal { get; set; }

        public int? Idf_Natureza_Juridica { get; set; }

        public int Idf_Endereco { get; set; }

        [StringLength(100)]
        public string End_Site { get; set; }

        [Required]
        [StringLength(2)]
        public string Num_DDD_Comercial { get; set; }

        [Required]
        [StringLength(10)]
        public string Num_Comercial { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public int? Qtd_Usuario_Adicional { get; set; }

        public int Qtd_Funcionarios { get; set; }

        [Required]
        [StringLength(15)]
        public string Des_IP { get; set; }

        public bool Flg_Oferece_Cursos { get; set; }

        public int Idf_Situacao_Filial { get; set; }

        [StringLength(200)]
        public string Des_Pagina_Facebook { get; set; }

        [StringLength(10)]
        public string Num_Comercial2 { get; set; }

        public DbGeography Des_Localizacao { get; set; }

        public int? Idf_Tipo_Parceiro { get; set; }

        public virtual ICollection<TAB_Usuario_Filial_Perfil> TAB_Usuario_Filial_Perfil { get; set; }
    }
}
