namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Simulacao_R1")]
    public partial class BNE_Simulacao_R1
    {
        public BNE_Simulacao_R1()
        {
            BNE_Solicitacao_R1 = new HashSet<BNE_Solicitacao_R1>();
        }

        [Key]
        public int Idf_Simulacao_R1 { get; set; }

        public int? Idf_Usuario_Filial_Perfil { get; set; }

        [Required]
        [StringLength(200)]
        public string Nme_Pessoa { get; set; }

        [Required]
        [StringLength(2)]
        public string Num_DDD_Telefone { get; set; }

        [Required]
        [StringLength(10)]
        public string Num_Telefone { get; set; }

        [StringLength(200)]
        public string Eml_Pessoa { get; set; }

        public int Idf_Cidade { get; set; }

        public int Idf_Funcao { get; set; }

        public decimal Vlr_Salario_Max { get; set; }

        public decimal Vlr_Salario_Min { get; set; }

        public int Num_Vagas { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public int Idf_Consultor_R1 { get; set; }

        public decimal Vlr_Taxa_Abertura { get; set; }

        public decimal Vlr_Servico_Prestado { get; set; }

        public decimal Vlr_Bonus_Solicitacao_Online { get; set; }

        public decimal Vlr_Total { get; set; }

        public decimal Vlr_Margem_Percentual_Servico { get; set; }

        public int Qtd_Dias_Atendimento { get; set; }

        public virtual BNE_Consultor_R1 BNE_Consultor_R1 { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }

        public virtual TAB_Funcao TAB_Funcao { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }

        public virtual ICollection<BNE_Solicitacao_R1> BNE_Solicitacao_R1 { get; set; }
    }
}
