namespace API.GatewayV2.BNEModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Pessoa_Fisica")]
    public partial class TAB_Pessoa_Fisica
    {
        public TAB_Pessoa_Fisica()
        {
            TAB_Usuario_Filial_Perfil = new HashSet<TAB_Usuario_Filial_Perfil>();
        }

        [Key]
        public int Idf_Pessoa_Fisica { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Num_CPF { get; set; }

        [Required]
        [StringLength(100)]
        public string Nme_Pessoa { get; set; }

        [StringLength(30)]
        public string Ape_Pessoa { get; set; }

        public int? Idf_Sexo { get; set; }

        public int? Idf_Nacionalidade { get; set; }

        public int? Idf_Cidade { get; set; }

        public DateTime? Dta_Chegada_Brasil { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Dta_Nascimento { get; set; }

        [StringLength(100)]
        public string Nme_Mae { get; set; }

        [StringLength(100)]
        public string Nme_Pai { get; set; }

        [StringLength(20)]
        public string Num_RG { get; set; }

        public DateTime? Dta_Expedicao_RG { get; set; }

        [StringLength(20)]
        public string Nme_Orgao_Emissor { get; set; }

        [StringLength(2)]
        public string Sig_UF_Emissao_RG { get; set; }

        [StringLength(11)]
        public string Num_PIS { get; set; }

        [StringLength(8)]
        public string Num_CTPS { get; set; }

        [StringLength(5)]
        public string Des_Serie_CTPS { get; set; }

        [StringLength(2)]
        public string Sig_UF_CTPS { get; set; }

        public int? Idf_Raca { get; set; }

        public int? Idf_Deficiencia { get; set; }

        public int? Idf_Endereco { get; set; }

        [StringLength(2)]
        public string Num_DDD_Telefone { get; set; }

        [StringLength(10)]
        public string Num_Telefone { get; set; }

        [StringLength(2)]
        public string Num_DDD_Celular { get; set; }

        [StringLength(10)]
        public string Num_Celular { get; set; }

        [StringLength(100)]
        public string Eml_Pessoa { get; set; }

        public bool? Flg_Possui_Dependentes { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public bool? Flg_Importado { get; set; }

        public int? Idf_Escolaridade { get; set; }

        [Required]
        [StringLength(100)]
        public string Nme_Pessoa_Pesquisa { get; set; }

        public int? Idf_Estado_Civil { get; set; }

        [StringLength(2)]
        public string Sig_Estado { get; set; }

        public bool? Flg_Inativo { get; set; }

        [StringLength(15)]
        public string Des_IP { get; set; }

        public int? Idf_Operadora_Celular { get; set; }

        public int? Idf_Email_Situacao_Confirmacao { get; set; }

        public int? Idf_Email_Situacao_Validacao { get; set; }

        public int? Idf_Email_Situacao_Bloqueio { get; set; }

        public bool Flg_Email_Confirmado { get; set; }

        public bool Flg_Celular_Confirmado { get; set; }

        public virtual ICollection<TAB_Usuario_Filial_Perfil> TAB_Usuario_Filial_Perfil { get; set; }
    }
}
