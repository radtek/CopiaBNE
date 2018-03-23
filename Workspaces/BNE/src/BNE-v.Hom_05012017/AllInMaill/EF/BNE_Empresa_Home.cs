namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Empresa_Home")]
    public partial class BNE_Empresa_Home
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Filial { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Nome_URL { get; set; }

        [Required]
        [StringLength(300)]
        public string Des_Caminho_Imagem { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual TAB_Filial TAB_Filial { get; set; }
    }
}
