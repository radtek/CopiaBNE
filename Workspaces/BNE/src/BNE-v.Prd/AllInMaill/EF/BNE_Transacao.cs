namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Transacao")]
    public partial class BNE_Transacao
    {
        public BNE_Transacao()
        {
            BNE_Linha_Arquivo = new HashSet<BNE_Linha_Arquivo>();
            BNE_Pagamento = new HashSet<BNE_Pagamento>();
            BNE_Transacao_Resposta = new HashSet<BNE_Transacao_Resposta>();
            BNE_Transacao_Retorno = new HashSet<BNE_Transacao_Retorno>();
        }

        [Key]
        public int Idf_Transacao { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public int? Idf_Plano_Adquirido { get; set; }

        public int? Idf_Tipo_Pagamento { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Vlr_Documento { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_Cartao_Credito { get; set; }

        public int? Num_Mes_Validade_Cartao_Credito { get; set; }

        public int? Num_Ano_Validade_Cartao_Credito { get; set; }

        public int? Num_Codigo_Verificador_Cartao_Credito { get; set; }

        [StringLength(15)]
        public string Des_IP_Comprador { get; set; }

        public int? Idf_Operadora { get; set; }

        public int? Num_Dia_Agendamento { get; set; }

        public int? Num_Meses_Agendamento { get; set; }

        public int? Num_Tentativas_Nao_Aprovado_Agendamento { get; set; }

        public int? Num_Dias_Entre_Tentativas_Agendamento { get; set; }

        public int? Idf_Banco { get; set; }

        [StringLength(50)]
        public string Des_Agencia_Debito { get; set; }

        [StringLength(50)]
        public string Des_Conta_Corrente_Debito { get; set; }

        [StringLength(40)]
        public string Nme_Titular_Conta_Corrente_Debito { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_CPF_Titular_Conta_Corrente_Debito { get; set; }

        [StringLength(50)]
        public string Des_Transacao { get; set; }

        [StringLength(200)]
        public string Des_Mensagem_Captura { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_CNPJ_Titular_Conta_Corrente_Debito { get; set; }

        public int Idf_Status_Transacao { get; set; }

        public virtual ICollection<BNE_Linha_Arquivo> BNE_Linha_Arquivo { get; set; }

        public virtual BNE_Operadora BNE_Operadora { get; set; }

        public virtual ICollection<BNE_Pagamento> BNE_Pagamento { get; set; }

        public virtual BNE_Plano_Adquirido BNE_Plano_Adquirido { get; set; }

        public virtual BNE_Tipo_Pagamento BNE_Tipo_Pagamento { get; set; }

        public virtual ICollection<BNE_Transacao_Resposta> BNE_Transacao_Resposta { get; set; }

        public virtual ICollection<BNE_Transacao_Retorno> BNE_Transacao_Retorno { get; set; }

        public virtual TAB_Status_Transacao TAB_Status_Transacao { get; set; }

        public virtual TAB_Banco TAB_Banco { get; set; }
    }
}
