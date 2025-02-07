//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BNE.Web.LanHouse.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class TAB_Pessoa_Fisica
    {
        public TAB_Pessoa_Fisica()
        {
            this.BNE_Curriculo = new HashSet<BNE_Curriculo>();
            this.BNE_Experiencia_Profissional = new HashSet<BNE_Experiencia_Profissional>();
            this.BNE_Formacao = new HashSet<BNE_Formacao>();
            this.TAB_Pessoa_Fisica_Rede_Social = new HashSet<TAB_Pessoa_Fisica_Rede_Social>();
            this.TAB_Pessoa_Fisica_Foto = new HashSet<TAB_Pessoa_Fisica_Foto>();
        }
    
        public int Idf_Pessoa_Fisica { get; set; }
        public decimal Num_CPF { get; set; }
        public string Nme_Pessoa { get; set; }
        public string Ape_Pessoa { get; set; }
        public Nullable<int> Idf_Sexo { get; set; }
        public Nullable<int> Idf_Nacionalidade { get; set; }
        public Nullable<int> Idf_Cidade { get; set; }
        public Nullable<System.DateTime> Dta_Chegada_Brasil { get; set; }
        public System.DateTime Dta_Nascimento { get; set; }
        public string Nme_Mae { get; set; }
        public string Nme_Pai { get; set; }
        public string Num_RG { get; set; }
        public Nullable<System.DateTime> Dta_Expedicao_RG { get; set; }
        public string Nme_Orgao_Emissor { get; set; }
        public string Sig_UF_Emissao_RG { get; set; }
        public string Num_PIS { get; set; }
        public string Num_CTPS { get; set; }
        public string Des_Serie_CTPS { get; set; }
        public string Sig_UF_CTPS { get; set; }
        public Nullable<int> Idf_Raca { get; set; }
        public Nullable<int> Idf_Deficiencia { get; set; }
        public Nullable<int> Idf_Endereco { get; set; }
        public string Num_DDD_Telefone { get; set; }
        public string Num_Telefone { get; set; }
        public string Num_DDD_Celular { get; set; }
        public string Num_Celular { get; set; }
        public string Eml_Pessoa { get; set; }
        public Nullable<bool> Flg_Possui_Dependentes { get; set; }
        public System.DateTime Dta_Cadastro { get; set; }
        public System.DateTime Dta_Alteracao { get; set; }
        public Nullable<bool> Flg_Importado { get; set; }
        public Nullable<int> Idf_Escolaridade { get; set; }
        public string Nme_Pessoa_Pesquisa { get; set; }
        public Nullable<int> Idf_Estado_Civil { get; set; }
        public string Sig_Estado { get; set; }
        public Nullable<bool> Flg_Inativo { get; set; }
        public string Des_IP { get; set; }
        public Nullable<int> Idf_Operadora_Celular { get; set; }
        public Nullable<int> Idf_Email_Situacao_Confirmacao { get; set; }
        public Nullable<int> Idf_Email_Situacao_Validacao { get; set; }
        public Nullable<int> Idf_Email_Situacao_Bloqueio { get; set; }
    
        public virtual ICollection<BNE_Curriculo> BNE_Curriculo { get; set; }
        public virtual ICollection<BNE_Experiencia_Profissional> BNE_Experiencia_Profissional { get; set; }
        public virtual ICollection<BNE_Formacao> BNE_Formacao { get; set; }
        public virtual TAB_Cidade TAB_Cidade { get; set; }
        public virtual ICollection<TAB_Pessoa_Fisica_Rede_Social> TAB_Pessoa_Fisica_Rede_Social { get; set; }
        public virtual TAB_Endereco TAB_Endereco { get; set; }
        public virtual ICollection<TAB_Pessoa_Fisica_Foto> TAB_Pessoa_Fisica_Foto { get; set; }
    }
}
