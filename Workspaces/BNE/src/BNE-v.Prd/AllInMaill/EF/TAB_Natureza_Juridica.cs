namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Natureza_Juridica")]
    public partial class TAB_Natureza_Juridica
    {
        public TAB_Natureza_Juridica()
        {
            TAB_Filial = new HashSet<TAB_Filial>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Natureza_Juridica { get; set; }

        [Required]
        [StringLength(4)]
        public string Cod_Natureza_Juridica { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Natureza_Juridica { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<TAB_Filial> TAB_Filial { get; set; }

        public virtual TAB_Pessoa_Juridica TAB_Pessoa_Juridica { get; set; }
    }
}
