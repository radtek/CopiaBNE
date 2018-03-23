namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Pessoa_Juridica")]
    public partial class TAB_Pessoa_Juridica
    {
        public TAB_Pessoa_Juridica()
        {
            TAB_Endereco_Pessoa_Juridica = new HashSet<TAB_Endereco_Pessoa_Juridica>();
            TAB_Inscricao_Estadual = new HashSet<TAB_Inscricao_Estadual>();
            TAB_Pessoa_Juridica_Logo = new HashSet<TAB_Pessoa_Juridica_Logo>();
        }

        [Key]
        public int Idf_Pessoa_Juridica { get; set; }

        public decimal? Num_CNPJ { get; set; }

        public decimal? Num_CEI { get; set; }

        [Required]
        [StringLength(60)]
        public string Raz_Social { get; set; }

        [Required]
        [StringLength(60)]
        public string Nme_Fantasia { get; set; }

        [StringLength(30)]
        public string Ape_Empresa { get; set; }

        public int Idf_CNAE_Principal { get; set; }

        public int? Idf_CNAE_Secundario { get; set; }

        public int Idf_Natureza_Juridica { get; set; }

        public int? Idf_GPS { get; set; }

        public int? Idf_RAT { get; set; }

        public int? Idf_Simples { get; set; }

        public int? Idf_Porte_Empresa { get; set; }

        [StringLength(100)]
        public string End_Site { get; set; }

        public DateTime? Dta_Fundacao { get; set; }

        public int? Qtd_Socios { get; set; }

        public int? Flg_Isencao_Filantropica { get; set; }

        public decimal? Num_FAP { get; set; }

        public decimal? Num_PAT { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public virtual TAB_CNAE_Sub_Classe TAB_CNAE_Sub_Classe { get; set; }

        public virtual TAB_CNAE_Sub_Classe TAB_CNAE_Sub_Classe1 { get; set; }

        public virtual ICollection<TAB_Endereco_Pessoa_Juridica> TAB_Endereco_Pessoa_Juridica { get; set; }

        public virtual TAB_GPS TAB_GPS { get; set; }

        public virtual ICollection<TAB_Inscricao_Estadual> TAB_Inscricao_Estadual { get; set; }

        public virtual TAB_Natureza_Juridica TAB_Natureza_Juridica { get; set; }

        public virtual TAB_Simples TAB_Simples { get; set; }

        public virtual TAB_Porte_Empresa TAB_Porte_Empresa { get; set; }

        public virtual ICollection<TAB_Pessoa_Juridica_Logo> TAB_Pessoa_Juridica_Logo { get; set; }

        public virtual TAB_RAT TAB_RAT { get; set; }
    }
}
