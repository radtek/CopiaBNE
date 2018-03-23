namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.GLO_Cobranca_Boleto")]
    public partial class GLO_Cobranca_Boleto
    {
        public GLO_Cobranca_Boleto()
        {
            BNE_Linha_Arquivo = new HashSet<BNE_Linha_Arquivo>();
            GLO_Cobranca_Boleto_Lista_Remessa = new HashSet<GLO_Cobranca_Boleto_Lista_Remessa>();
            GLO_Cobranca_Boleto_LOG = new HashSet<GLO_Cobranca_Boleto_LOG>();
            GLO_Cobranca_Boleto_Lista_Retorno = new HashSet<GLO_Cobranca_Boleto_Lista_Retorno>();
        }

        [Key]
        public int Idf_Cobranca_Boleto { get; set; }

        public int? Idf_Transacao { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_CNPJ_Cedente { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_CPF_Cedente { get; set; }

        [StringLength(10)]
        public string Num_Agencia_Bancaria { get; set; }

        [StringLength(10)]
        public string Num_Conta { get; set; }

        [StringLength(2)]
        public string Num_DV_Conta { get; set; }

        [StringLength(60)]
        public string Raz_Social_Cedente { get; set; }

        [StringLength(100)]
        public string Nme_Pessoa_Cedente { get; set; }

        public int? Idf_Banco { get; set; }

        public bool Flg_Registra_Boleto { get; set; }

        public DateTime? Dta_Emissao { get; set; }

        public DateTime? Dta_Vencimento { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Vlr_Boleto { get; set; }

        [StringLength(20)]
        public string Num_Nosso_Numero { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_CPF_Sacado { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_CNPJ_Sacado { get; set; }

        [StringLength(100)]
        public string Nme_Pessoa_Sacado { get; set; }

        [StringLength(60)]
        public string Raz_Social_Sacado { get; set; }

        [StringLength(100)]
        public string End_Email_Sacado { get; set; }

        [StringLength(100)]
        public string Des_Logradouro_Sacado { get; set; }

        [StringLength(15)]
        public string Num_Endereço_Sacado { get; set; }

        [StringLength(30)]
        public string Des_Complemento_Sacado { get; set; }

        public int? Idf_Cidade_Sacado { get; set; }

        [StringLength(8)]
        public string Num_Cep_Sacado { get; set; }

        [StringLength(30)]
        public string Des_Bairro_Sacado { get; set; }

        [StringLength(100)]
        public string Des_Instrucao_Caixa { get; set; }

        [StringLength(50)]
        public string Cod_Barras { get; set; }

        public string Arq_Boleto { get; set; }

        public int? Idf_Mensagem_Retorno_Boleto { get; set; }

        public virtual ICollection<BNE_Linha_Arquivo> BNE_Linha_Arquivo { get; set; }

        public virtual ICollection<GLO_Cobranca_Boleto_Lista_Remessa> GLO_Cobranca_Boleto_Lista_Remessa { get; set; }

        public virtual ICollection<GLO_Cobranca_Boleto_LOG> GLO_Cobranca_Boleto_LOG { get; set; }

        public virtual ICollection<GLO_Cobranca_Boleto_Lista_Retorno> GLO_Cobranca_Boleto_Lista_Retorno { get; set; }

        public virtual GLO_Mensagem_Retorno_Boleto GLO_Mensagem_Retorno_Boleto { get; set; }

        public virtual GLO_Transacao GLO_Transacao { get; set; }

        public virtual TAB_Banco TAB_Banco { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }
    }
}
