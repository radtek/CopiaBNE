namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Classe_Salarial")]
    public partial class TAB_Classe_Salarial
    {
        public TAB_Classe_Salarial()
        {
            TAB_Funcao = new HashSet<TAB_Funcao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Classe_Salarial { get; set; }

        [Column(TypeName = "money")]
        public decimal Vlr_Salario_Medio { get; set; }

        [Column(TypeName = "money")]
        public decimal Vlr_Piso { get; set; }

        [Column(TypeName = "money")]
        public decimal Vlr_Teto { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<TAB_Funcao> TAB_Funcao { get; set; }
    }
}
