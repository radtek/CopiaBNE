namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Banco")]
    public partial class TAB_Banco
    {
        public TAB_Banco()
        {
            BNE_Transacao = new HashSet<BNE_Transacao>();
            GLO_Cobranca_Boleto = new HashSet<GLO_Cobranca_Boleto>();
            GLO_Convenio_Bancario = new HashSet<GLO_Convenio_Bancario>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Banco { get; set; }

        [Required]
        [StringLength(100)]
        public string Nme_Banco { get; set; }

        [Required]
        [StringLength(30)]
        public string Ape_Banco { get; set; }

        public bool Flg_Oficial { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Vincula_Conta { get; set; }

        public short? Num_Hora { get; set; }

        public short? Num_Minuto { get; set; }

        public virtual ICollection<BNE_Transacao> BNE_Transacao { get; set; }

        public virtual ICollection<GLO_Cobranca_Boleto> GLO_Cobranca_Boleto { get; set; }

        public virtual ICollection<GLO_Convenio_Bancario> GLO_Convenio_Bancario { get; set; }
    }
}
