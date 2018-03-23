namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Usuario_Filial_Perfil")]
    public partial class TAB_Usuario_Filial_Perfil
    {
        public TAB_Usuario_Filial_Perfil()
        {
            BNE_Agradecimento = new HashSet<BNE_Agradecimento>();
            BNE_Auditor_Publicador = new HashSet<BNE_Auditor_Publicador>();
            BNE_Auditor_Publicador1 = new HashSet<BNE_Auditor_Publicador>();
            BNE_Celular_Selecionador = new HashSet<BNE_Celular_Selecionador>();
            BNE_Codigo_Desconto = new HashSet<BNE_Codigo_Desconto>();
            BNE_Conversas_Ativas = new HashSet<BNE_Conversas_Ativas>();
            BNE_Curriculo_Auditoria = new HashSet<BNE_Curriculo_Auditoria>();
            BNE_Curriculo_Auditoria1 = new HashSet<BNE_Curriculo_Auditoria>();
            BNE_Curriculo_Classificacao = new HashSet<BNE_Curriculo_Classificacao>();
            BNE_Curriculo_Correcao = new HashSet<BNE_Curriculo_Correcao>();
            BNE_Curriculo_Observacao = new HashSet<BNE_Curriculo_Observacao>();
            BNE_Curriculo_Publicacao = new HashSet<BNE_Curriculo_Publicacao>();
            BNE_Email_Destinatario = new HashSet<BNE_Email_Destinatario>();
            BNE_Email_Destinatario_Cidade = new HashSet<BNE_Email_Destinatario_Cidade>();
            BNE_Financeiro = new HashSet<BNE_Financeiro>();
            BNE_Mensagem_CS = new HashSet<BNE_Mensagem_CS>();
            BNE_Mensagem_CS1 = new HashSet<BNE_Mensagem_CS>();
            BNE_Noticia_Visualizacao = new HashSet<BNE_Noticia_Visualizacao>();
            BNE_Pagamento = new HashSet<BNE_Pagamento>();
            BNE_Pagamento1 = new HashSet<BNE_Pagamento>();
            BNE_Plano_Adquirido = new HashSet<BNE_Plano_Adquirido>();
            BNE_Publicador = new HashSet<BNE_Publicador>();
            BNE_Revidor = new HashSet<BNE_Revidor>();
            BNE_Simulacao_R1 = new HashSet<BNE_Simulacao_R1>();
            BNE_Solicitacao_R1 = new HashSet<BNE_Solicitacao_R1>();
            BNE_Usuario_Filial = new HashSet<BNE_Usuario_Filial>();
            BNE_Vaga = new HashSet<BNE_Vaga>();
            TAB_Filial_Observacao = new HashSet<TAB_Filial_Observacao>();
            TAB_Integrador = new HashSet<TAB_Integrador>();
            TAB_Perfil_Usuario = new HashSet<TAB_Perfil_Usuario>();
            TAB_Pesquisa_Curriculo = new HashSet<TAB_Pesquisa_Curriculo>();
            TAB_Pesquisa_Vaga = new HashSet<TAB_Pesquisa_Vaga>();
            TAB_Usuario_Funcao = new HashSet<TAB_Usuario_Funcao>();
        }

        public int? Idf_Filial { get; set; }

        [Key]
        public int Idf_Usuario_Filial_Perfil { get; set; }

        [StringLength(15)]
        public string Des_IP { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public int Idf_Pessoa_Fisica { get; set; }

        public int? Idf_Perfil { get; set; }

        [Required]
        [StringLength(10)]
        public string Sen_Usuario_Filial_Perfil { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public virtual ICollection<BNE_Agradecimento> BNE_Agradecimento { get; set; }

        public virtual ICollection<BNE_Auditor_Publicador> BNE_Auditor_Publicador { get; set; }

        public virtual ICollection<BNE_Auditor_Publicador> BNE_Auditor_Publicador1 { get; set; }

        public virtual ICollection<BNE_Celular_Selecionador> BNE_Celular_Selecionador { get; set; }

        public virtual ICollection<BNE_Codigo_Desconto> BNE_Codigo_Desconto { get; set; }

        public virtual ICollection<BNE_Conversas_Ativas> BNE_Conversas_Ativas { get; set; }

        public virtual ICollection<BNE_Curriculo_Auditoria> BNE_Curriculo_Auditoria { get; set; }

        public virtual ICollection<BNE_Curriculo_Auditoria> BNE_Curriculo_Auditoria1 { get; set; }

        public virtual ICollection<BNE_Curriculo_Classificacao> BNE_Curriculo_Classificacao { get; set; }

        public virtual ICollection<BNE_Curriculo_Correcao> BNE_Curriculo_Correcao { get; set; }

        public virtual ICollection<BNE_Curriculo_Observacao> BNE_Curriculo_Observacao { get; set; }

        public virtual ICollection<BNE_Curriculo_Publicacao> BNE_Curriculo_Publicacao { get; set; }

        public virtual ICollection<BNE_Email_Destinatario> BNE_Email_Destinatario { get; set; }

        public virtual ICollection<BNE_Email_Destinatario_Cidade> BNE_Email_Destinatario_Cidade { get; set; }

        public virtual ICollection<BNE_Financeiro> BNE_Financeiro { get; set; }

        public virtual ICollection<BNE_Mensagem_CS> BNE_Mensagem_CS { get; set; }

        public virtual ICollection<BNE_Mensagem_CS> BNE_Mensagem_CS1 { get; set; }

        public virtual ICollection<BNE_Noticia_Visualizacao> BNE_Noticia_Visualizacao { get; set; }

        public virtual ICollection<BNE_Pagamento> BNE_Pagamento { get; set; }

        public virtual ICollection<BNE_Pagamento> BNE_Pagamento1 { get; set; }

        public virtual ICollection<BNE_Plano_Adquirido> BNE_Plano_Adquirido { get; set; }

        public virtual ICollection<BNE_Publicador> BNE_Publicador { get; set; }

        public virtual ICollection<BNE_Revidor> BNE_Revidor { get; set; }

        public virtual ICollection<BNE_Simulacao_R1> BNE_Simulacao_R1 { get; set; }

        public virtual ICollection<BNE_Solicitacao_R1> BNE_Solicitacao_R1 { get; set; }

        public virtual ICollection<BNE_Usuario_Filial> BNE_Usuario_Filial { get; set; }

        public virtual ICollection<BNE_Vaga> BNE_Vaga { get; set; }

        public virtual TAB_Filial TAB_Filial { get; set; }

        public virtual ICollection<TAB_Filial_Observacao> TAB_Filial_Observacao { get; set; }

        public virtual ICollection<TAB_Integrador> TAB_Integrador { get; set; }

        public virtual TAB_Perfil TAB_Perfil { get; set; }

        public virtual ICollection<TAB_Perfil_Usuario> TAB_Perfil_Usuario { get; set; }

        public virtual ICollection<TAB_Pesquisa_Curriculo> TAB_Pesquisa_Curriculo { get; set; }

        public virtual ICollection<TAB_Pesquisa_Vaga> TAB_Pesquisa_Vaga { get; set; }

        public virtual TAB_Pessoa_Fisica TAB_Pessoa_Fisica { get; set; }

        public virtual ICollection<TAB_Usuario_Funcao> TAB_Usuario_Funcao { get; set; }
    }
}
