namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Pessoa_Fisica_Complemento")]
    public partial class TAB_Pessoa_Fisica_Complemento
    {
        public TAB_Pessoa_Fisica_Complemento()
        {
            TAB_Contato = new HashSet<TAB_Contato>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Pessoa_Fisica { get; set; }

        [StringLength(15)]
        public string Num_Habilitacao { get; set; }

        public int? Idf_Categoria_Habilitacao { get; set; }

        public DateTime? Dta_Validade_Habilitacao { get; set; }

        [StringLength(16)]
        public string Num_Titulo_Eleitoral { get; set; }

        [StringLength(4)]
        public string Num_Zona_Eleitoral { get; set; }

        [StringLength(4)]
        public string Num_Secao_Eleitoral { get; set; }

        [StringLength(20)]
        public string Num_Registro_Conselho { get; set; }

        public int? Idf_Conselho { get; set; }

        public int? Idf_Tipo_Conselho { get; set; }

        public DateTime? Dta_Validade_Conselho { get; set; }

        [StringLength(20)]
        public string Des_Pais_Visto { get; set; }

        [StringLength(15)]
        public string Num_Visto { get; set; }

        public DateTime? Dta_Validade_Visto { get; set; }

        public bool? Flg_Veiculo_Proprio { get; set; }

        public short? Ano_Veiculo { get; set; }

        [StringLength(7)]
        public string Des_Placa_Veiculo { get; set; }

        [StringLength(15)]
        public string Num_Renavam { get; set; }

        [StringLength(12)]
        public string Num_Doc_Reservista { get; set; }

        public bool? Flg_Aposentado { get; set; }

        public DateTime? Dta_Aposentadoria { get; set; }

        public int? Idf_Tipo_Sanguineo { get; set; }

        public bool? Flg_Doador { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public bool Flg_Importado { get; set; }

        public bool? Flg_Seguro_Veiculo { get; set; }

        public DateTime? Dta_Vencimento_Seguro { get; set; }

        public int? Idf_Motivo_Aposentadoria { get; set; }

        public int? Idf_CID { get; set; }

        [StringLength(12)]
        public string Num_CEI { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_Altura { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_Peso { get; set; }

        public int? Idf_Tipo_Veiculo { get; set; }

        public int? Num_Registro_Habilitacao { get; set; }

        [StringLength(2000)]
        public string Des_Conhecimento { get; set; }

        [StringLength(200)]
        public string Des_Complemento_Deficiencia { get; set; }

        public bool? Flg_Filhos { get; set; }

        public bool? Flg_Viagem { get; set; }

        public bool? Flg_Mudanca { get; set; }

        public byte[] Arq_Anexo { get; set; }

        [StringLength(100)]
        public string Nme_Anexo { get; set; }

        public virtual ICollection<TAB_Contato> TAB_Contato { get; set; }

        public virtual TAB_Pessoa_Fisica TAB_Pessoa_Fisica { get; set; }

        public virtual TAB_Categoria_Habilitacao TAB_Categoria_Habilitacao { get; set; }

        public virtual TAB_Tipo_Veiculo TAB_Tipo_Veiculo { get; set; }

        public virtual TAB_Tipo_Sanguineo TAB_Tipo_Sanguineo { get; set; }
    }
}
