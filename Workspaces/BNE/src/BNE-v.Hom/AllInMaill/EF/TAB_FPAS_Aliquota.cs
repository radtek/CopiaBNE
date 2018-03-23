namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_FPAS_Aliquota")]
    public partial class TAB_FPAS_Aliquota
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_FPAS_Aliquota { get; set; }

        [Required]
        [StringLength(100)]
        public string Nme_FPAS_Aliquota { get; set; }

        public short Num_Somatoria { get; set; }

        public int Idf_FPAS { get; set; }

        public double Vlr_Previdencia { get; set; }

        public double Vlr_Autonomo { get; set; }

        public double Vlr_Risco_Acidente { get; set; }

        public double Vlr_Salario_Educacao { get; set; }

        public double Vlr_Incra { get; set; }

        public double Vlr_Senai { get; set; }

        public double Vlr_Sesi { get; set; }

        public double Vlr_Senac { get; set; }

        public double Vlr_Sesc { get; set; }

        public double Vlr_Sebrae { get; set; }

        public double Vlr_Dpc { get; set; }

        public double Vlr_Fundo_Aeroviario { get; set; }

        public double Vlr_Senar { get; set; }

        public double Vlr_Sest { get; set; }

        public double Vlr_Senat { get; set; }

        public double Vlr_Sescoop { get; set; }
    }
}
