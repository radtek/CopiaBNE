namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Plano_Adquirido")]
    public partial class BNE_Plano_Adquirido
    {
        public BNE_Plano_Adquirido()
        {
            BNE_Adicional_Plano = new HashSet<BNE_Adicional_Plano>();
            BNE_Transacao = new HashSet<BNE_Transacao>();
            BNE_Plano_Parcela = new HashSet<BNE_Plano_Parcela>();
            BNE_Plano_Adquirido_Detalhes = new HashSet<BNE_Plano_Adquirido_Detalhes>();
            BNE_Plano_Quantidade = new HashSet<BNE_Plano_Quantidade>();
        }

        public int Idf_Plano { get; set; }

        [Key]
        public int Idf_Plano_Adquirido { get; set; }

        public int Idf_Usuario_Filial_Perfil { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Inicio_Plano { get; set; }

        public DateTime Dta_Fim_Plano { get; set; }

        public int Idf_Plano_Situacao { get; set; }

        public int? Idf_Filial { get; set; }

        public int Qtd_SMS { get; set; }

        public decimal Vlr_Base { get; set; }

        public int? Qtd_Prazo_Boleto { get; set; }

        public bool Flg_Boleto_Registrado { get; set; }

        public bool Flg_Nota_Antecipada { get; set; }

        public virtual ICollection<BNE_Adicional_Plano> BNE_Adicional_Plano { get; set; }

        public virtual BNE_Plano BNE_Plano { get; set; }

        public virtual ICollection<BNE_Transacao> BNE_Transacao { get; set; }

        public virtual ICollection<BNE_Plano_Parcela> BNE_Plano_Parcela { get; set; }

        public virtual ICollection<BNE_Plano_Adquirido_Detalhes> BNE_Plano_Adquirido_Detalhes { get; set; }

        public virtual BNE_Plano_Situacao BNE_Plano_Situacao { get; set; }

        public virtual ICollection<BNE_Plano_Quantidade> BNE_Plano_Quantidade { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }

        public virtual TAB_Filial TAB_Filial { get; set; }
    }
}
