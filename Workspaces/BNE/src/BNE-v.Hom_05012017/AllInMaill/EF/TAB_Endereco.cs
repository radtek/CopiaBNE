namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Endereco")]
    public partial class TAB_Endereco
    {
        public TAB_Endereco()
        {
            TAB_Filial = new HashSet<TAB_Filial>();
            TAB_Pessoa_Fisica = new HashSet<TAB_Pessoa_Fisica>();
        }

        [Key]
        public int Idf_Endereco { get; set; }

        [StringLength(8)]
        public string Num_CEP { get; set; }

        [StringLength(100)]
        public string Des_Logradouro { get; set; }

        [StringLength(15)]
        public string Num_Endereco { get; set; }

        [StringLength(30)]
        public string Des_Complemento { get; set; }

        [StringLength(80)]
        public string Des_Bairro { get; set; }

        public int Idf_Cidade { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime? Dta_Alteracao { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }

        public virtual ICollection<TAB_Filial> TAB_Filial { get; set; }

        public virtual ICollection<TAB_Pessoa_Fisica> TAB_Pessoa_Fisica { get; set; }
    }
}
