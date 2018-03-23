namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Endereco_Pessoa_Juridica")]
    public partial class TAB_Endereco_Pessoa_Juridica
    {
        [Key]
        public int Idf_Endereco_Pessoa_Juridica { get; set; }

        public int Idf_Pessoa_Juridica { get; set; }

        public int? Idf_Cidade { get; set; }

        [StringLength(8)]
        public string Num_CEP { get; set; }

        [StringLength(100)]
        public string Des_Logradouro { get; set; }

        [StringLength(15)]
        public string Num_Endereco { get; set; }

        [StringLength(100)]
        public string Des_Complemento { get; set; }

        [StringLength(80)]
        public string Des_Bairro { get; set; }

        public int Idf_Tipo_Endereco { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }

        public virtual TAB_Pessoa_Juridica TAB_Pessoa_Juridica { get; set; }

        public virtual TAB_Tipo_Endereco TAB_Tipo_Endereco { get; set; }
    }
}
