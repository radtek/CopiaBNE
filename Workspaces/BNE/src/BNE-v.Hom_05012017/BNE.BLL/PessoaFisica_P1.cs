//-- Data: 10/06/2014 10:57
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;
using System.ComponentModel.DataAnnotations;

namespace BNE.BLL
{
    public partial class PessoaFisica // Tabela: TAB_Pessoa_Fisica
    {
        #region Atributos
        private int _idPessoaFisica;
        private decimal _numeroCPF;
        private string _nomePessoa;
        private string _apelidoPessoa;
        private Sexo _sexo;
        private Nacionalidade _nacionalidade;
        private Cidade _cidade;
        private DateTime? _dataChegadaBrasil;
        private DateTime _dataNascimento;
        private string _nomeMae;
        private string _nomePai;
        private string _numeroRG;
        private DateTime? _dataExpedicaoRG;
        private string _nomeOrgaoEmissor;
        private string _siglaUFEmissaoRG;
        private string _numeroPIS;
        private string _numeroCTPS;
        private string _descricaoSerieCTPS;
        private string _siglaUFCTPS;
        private Raca _raca;
        private Deficiencia _deficiencia;
        private Endereco _endereco;
        private string _numeroDDDTelefone;
        private string _numeroTelefone;
        private string _numeroDDDCelular;
        private string _numeroCelular;
        private string _emailPessoa;
        private bool? _flagPossuiDependentes;
        private DateTime _dataCadastro;
        private DateTime _dataAlteracao;
        private bool? _flagImportado;
        private Escolaridade _escolaridade;
        private string _nomePessoaPesquisa;
        private EstadoCivil _estadoCivil;
        private string _siglaEstado;
        private bool? _flagInativo;
        private string _descricaoIP;
        private OperadoraCelular _operadoraCelular;
        private EmailSituacao _emailSituacaoConfirmacao;
        private EmailSituacao _emailSituacaoValidacao;
        private EmailSituacao _emailSituacaoBloqueio;
        private bool _flagCelularConfirmado;
        private bool _flagEmailConfirmado;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdPessoaFisica
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdPessoaFisica
        {
            get
            {
                return this._idPessoaFisica;
            }
        }
        #endregion

        #region NomePessoa
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo obrigatório.
        /// </summary>
        [Display(Name = "Nome")]
        public string NomePessoa
        {
            get
            {
                return this._nomePessoa;
            }
            set
            {
                this._nomePessoa = value;
                this._modified = true;
            }
        }
        #endregion

        #region ApelidoPessoa
        /// <summary>
        /// Tamanho do campo: 30.
        /// Campo opcional.
        /// </summary>
        /// 
        [Display(Name = "Apelido")]
        public string ApelidoPessoa
        {
            get
            {
                return this._apelidoPessoa;
            }
            set
            {
                this._apelidoPessoa = value;
                this._modified = true;
            }
        }
        #endregion

        #region Sexo
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Sexo Sexo
        {
            get
            {
                return this._sexo;
            }
            set
            {
                this._sexo = value;
                this._modified = true;
            }
        }
        #endregion

        #region Nacionalidade
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Nacionalidade Nacionalidade
        {
            get
            {
                return this._nacionalidade;
            }
            set
            {
                this._nacionalidade = value;
                this._modified = true;
            }
        }
        #endregion

        #region Cidade
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Cidade Cidade
        {
            get
            {
                return this._cidade;
            }
            set
            {
                this._cidade = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataChegadaBrasil
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataChegadaBrasil
        {
            get
            {
                return this._dataChegadaBrasil;
            }
            set
            {
                this._dataChegadaBrasil = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataNascimento
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        /// 
        [Display(Name = "Data de Nascimento")]
        public DateTime DataNascimento
        {
            get
            {
                return this._dataNascimento;
            }
            set
            {
                this._dataNascimento = value;
                this._modified = true;
            }
        }
        #endregion

        #region NomeMae
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        /// 
        [Display(Name = "Nome da Mãe")]
        public string NomeMae
        {
            get
            {
                return this._nomeMae;
            }
            set
            {
                this._nomeMae = value;
                this._modified = true;
            }
        }
        #endregion

        #region NomePai
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        /// 
        [Display(Name = "Nome do Pai")]
        public string NomePai
        {
            get
            {
                return this._nomePai;
            }
            set
            {
                this._nomePai = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroRG
        /// <summary>
        /// Tamanho do campo: 20.
        /// Campo opcional.
        /// </summary>
        public string NumeroRG
        {
            get
            {
                return this._numeroRG;
            }
            set
            {
                this._numeroRG = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataExpedicaoRG
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataExpedicaoRG
        {
            get
            {
                return this._dataExpedicaoRG;
            }
            set
            {
                this._dataExpedicaoRG = value;
                this._modified = true;
            }
        }
        #endregion

        #region NomeOrgaoEmissor
        /// <summary>
        /// Tamanho do campo: 20.
        /// Campo opcional.
        /// </summary>
        public string NomeOrgaoEmissor
        {
            get
            {
                return this._nomeOrgaoEmissor;
            }
            set
            {
                this._nomeOrgaoEmissor = value;
                this._modified = true;
            }
        }
        #endregion

        #region SiglaUFEmissaoRG
        /// <summary>
        /// Tamanho do campo: 2.
        /// Campo opcional.
        /// </summary>
        public string SiglaUFEmissaoRG
        {
            get
            {
                return this._siglaUFEmissaoRG;
            }
            set
            {
                this._siglaUFEmissaoRG = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroPIS
        /// <summary>
        /// Tamanho do campo: 11.
        /// Campo opcional.
        /// </summary>
        public string NumeroPIS
        {
            get
            {
                return this._numeroPIS;
            }
            set
            {
                this._numeroPIS = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroCTPS
        /// <summary>
        /// Tamanho do campo: 8.
        /// Campo opcional.
        /// </summary>
        public string NumeroCTPS
        {
            get
            {
                return this._numeroCTPS;
            }
            set
            {
                this._numeroCTPS = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoSerieCTPS
        /// <summary>
        /// Tamanho do campo: 5.
        /// Campo opcional.
        /// </summary>
        public string DescricaoSerieCTPS
        {
            get
            {
                return this._descricaoSerieCTPS;
            }
            set
            {
                this._descricaoSerieCTPS = value;
                this._modified = true;
            }
        }
        #endregion

        #region SiglaUFCTPS
        /// <summary>
        /// Tamanho do campo: 2.
        /// Campo opcional.
        /// </summary>
        public string SiglaUFCTPS
        {
            get
            {
                return this._siglaUFCTPS;
            }
            set
            {
                this._siglaUFCTPS = value;
                this._modified = true;
            }
        }
        #endregion

        #region Raca
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Raca Raca
        {
            get
            {
                return this._raca;
            }
            set
            {
                this._raca = value;
                this._modified = true;
            }
        }
        #endregion

        #region Deficiencia
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Deficiencia Deficiencia
        {
            get
            {
                return this._deficiencia;
            }
            set
            {
                this._deficiencia = value;
                this._modified = true;
            }
        }
        #endregion

        #region Endereco
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Endereco Endereco
        {
            get
            {
                return this._endereco;
            }
            set
            {
                this._endereco = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroDDDTelefone
        /// <summary>
        /// Tamanho do campo: 2.
        /// Campo opcional.
        /// </summary>
        public string NumeroDDDTelefone
        {
            get
            {
                return this._numeroDDDTelefone;
            }
            set
            {
                this._numeroDDDTelefone = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroTelefone
        /// <summary>
        /// Tamanho do campo: 10.
        /// Campo opcional.
        /// </summary>
        public string NumeroTelefone
        {
            get
            {
                return this._numeroTelefone;
            }
            set
            {
                this._numeroTelefone = value;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroDDDCelular
        /// <summary>
        /// Tamanho do campo: 2.
        /// Campo opcional.
        /// </summary>
        public string NumeroDDDCelular
        {
            get
            {
                return this._numeroDDDCelular;
            }
            set
            {
                this._numeroDDDCelular = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagPossuiDependentes
        /// <summary>
        /// Campo opcional.
        /// </summary>
        /// 
        [Display(Name = "Possui Dependentes")]
        public bool? FlagPossuiDependentes
        {
            get
            {
                return this._flagPossuiDependentes;
            }
            set
            {
                this._flagPossuiDependentes = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagImportado
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public bool? FlagImportado
        {
            get
            {
                return this._flagImportado;
            }
            set
            {
                this._flagImportado = value;
                this._modified = true;
            }
        }
        #endregion

        #region Escolaridade
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Escolaridade Escolaridade
        {
            get
            {
                return this._escolaridade;
            }
            set
            {
                this._escolaridade = value;
                this._modified = true;
            }
        }
        #endregion

        #region NomePessoaPesquisa
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo obrigatório.
        /// </summary>
        public string NomePessoaPesquisa
        {
            get
            {
                return this._nomePessoaPesquisa;
            }
            set
            {
                this._nomePessoaPesquisa = value;
                this._modified = true;
            }
        }
        #endregion

        #region EstadoCivil
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public EstadoCivil EstadoCivil
        {
            get
            {
                return this._estadoCivil;
            }
            set
            {
                this._estadoCivil = value;
                this._modified = true;
            }
        }
        #endregion

        #region SiglaEstado
        /// <summary>
        /// Tamanho do campo: 2.
        /// Campo opcional.
        /// </summary>
        public string SiglaEstado
        {
            get
            {
                return this._siglaEstado;
            }
            set
            {
                this._siglaEstado = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagInativo
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public bool? FlagInativo
        {
            get
            {
                return this._flagInativo;
            }
            set
            {
                this._flagInativo = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoIP
        /// <summary>
        /// Tamanho do campo: 15.
        /// Campo opcional.
        /// </summary>
        public string DescricaoIP
        {
            get
            {
                return this._descricaoIP;
            }
            set
            {
                this._descricaoIP = value;
                this._modified = true;
            }
        }
        #endregion

        #region OperadoraCelular
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public OperadoraCelular OperadoraCelular
        {
            get
            {
                return this._operadoraCelular;
            }
            set
            {
                this._operadoraCelular = value;
                this._modified = true;
            }
        }
        #endregion

        #region EmailSituacaoConfirmacao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public EmailSituacao EmailSituacaoConfirmacao
        {
            get
            {
                return this._emailSituacaoConfirmacao;
            }
            set
            {
                this._emailSituacaoConfirmacao = value;
                this._modified = true;
            }
        }
        #endregion

        #region EmailSituacaoValidacao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public EmailSituacao EmailSituacaoValidacao
        {
            get
            {
                return this._emailSituacaoValidacao;
            }
            set
            {
                this._emailSituacaoValidacao = value;
                this._modified = true;
            }
        }
        #endregion

        #region EmailSituacaoBloqueio
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public EmailSituacao EmailSituacaoBloqueio
        {
            get
            {
                return this._emailSituacaoBloqueio;
            }
            set
            {
                this._emailSituacaoBloqueio = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagCelularConfirmado
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public bool FlagCelularConfirmado
        {
            get
            {
                return this._flagCelularConfirmado;
            }
            set
            {
                this._flagCelularConfirmado = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagEmailConfirmado
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public bool FlagEmailConfirmado
        {
            get
            {
                return this._flagEmailConfirmado;
            }
            set
            {
                this._flagEmailConfirmado = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public PessoaFisica()
        {
        }
        public PessoaFisica(int idPessoaFisica)
        {
            this._idPessoaFisica = idPessoaFisica;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO TAB_Pessoa_Fisica (Num_CPF, Nme_Pessoa, Ape_Pessoa, Idf_Sexo, Idf_Nacionalidade, Idf_Cidade, Dta_Chegada_Brasil, Dta_Nascimento, Nme_Mae, Nme_Pai, Num_RG, Dta_Expedicao_RG, Nme_Orgao_Emissor, Sig_UF_Emissao_RG, Num_PIS, Num_CTPS, Des_Serie_CTPS, Sig_UF_CTPS, Idf_Raca, Idf_Deficiencia, Idf_Endereco, Num_DDD_Telefone, Num_Telefone, Num_DDD_Celular, Num_Celular, Eml_Pessoa, Flg_Possui_Dependentes, Dta_Cadastro, Dta_Alteracao, Flg_Importado, Idf_Escolaridade, Nme_Pessoa_Pesquisa, Idf_Estado_Civil, Sig_Estado, Flg_Inativo, Des_IP, Idf_Operadora_Celular, Idf_Email_Situacao_Confirmacao, Idf_Email_Situacao_Validacao, Idf_Email_Situacao_Bloqueio, Flg_Celular_Confirmado, Flg_Email_Confirmado) VALUES (@Num_CPF, @Nme_Pessoa, @Ape_Pessoa, @Idf_Sexo, @Idf_Nacionalidade, @Idf_Cidade, @Dta_Chegada_Brasil, @Dta_Nascimento, @Nme_Mae, @Nme_Pai, @Num_RG, @Dta_Expedicao_RG, @Nme_Orgao_Emissor, @Sig_UF_Emissao_RG, @Num_PIS, @Num_CTPS, @Des_Serie_CTPS, @Sig_UF_CTPS, @Idf_Raca, @Idf_Deficiencia, @Idf_Endereco, @Num_DDD_Telefone, @Num_Telefone, @Num_DDD_Celular, @Num_Celular, @Eml_Pessoa, @Flg_Possui_Dependentes, @Dta_Cadastro, @Dta_Alteracao, @Flg_Importado, @Idf_Escolaridade, @Nme_Pessoa_Pesquisa, @Idf_Estado_Civil, @Sig_Estado, @Flg_Inativo, @Des_IP, @Idf_Operadora_Celular, @Idf_Email_Situacao_Confirmacao, @Idf_Email_Situacao_Validacao, @Idf_Email_Situacao_Bloqueio, @Flg_Celular_Confirmado, @Flg_Email_Confirmado);SET @Idf_Pessoa_Fisica = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE TAB_Pessoa_Fisica SET Num_CPF = @Num_CPF, Nme_Pessoa = @Nme_Pessoa, Ape_Pessoa = @Ape_Pessoa, Idf_Sexo = @Idf_Sexo, Idf_Nacionalidade = @Idf_Nacionalidade, Idf_Cidade = @Idf_Cidade, Dta_Chegada_Brasil = @Dta_Chegada_Brasil, Dta_Nascimento = @Dta_Nascimento, Nme_Mae = @Nme_Mae, Nme_Pai = @Nme_Pai, Num_RG = @Num_RG, Dta_Expedicao_RG = @Dta_Expedicao_RG, Nme_Orgao_Emissor = @Nme_Orgao_Emissor, Sig_UF_Emissao_RG = @Sig_UF_Emissao_RG, Num_PIS = @Num_PIS, Num_CTPS = @Num_CTPS, Des_Serie_CTPS = @Des_Serie_CTPS, Sig_UF_CTPS = @Sig_UF_CTPS, Idf_Raca = @Idf_Raca, Idf_Deficiencia = @Idf_Deficiencia, Idf_Endereco = @Idf_Endereco, Num_DDD_Telefone = @Num_DDD_Telefone, Num_Telefone = @Num_Telefone, Num_DDD_Celular = @Num_DDD_Celular, Num_Celular = @Num_Celular, Eml_Pessoa = @Eml_Pessoa, Flg_Possui_Dependentes = @Flg_Possui_Dependentes, Dta_Cadastro = @Dta_Cadastro, Dta_Alteracao = @Dta_Alteracao, Flg_Importado = @Flg_Importado, Idf_Escolaridade = @Idf_Escolaridade, Nme_Pessoa_Pesquisa = @Nme_Pessoa_Pesquisa, Idf_Estado_Civil = @Idf_Estado_Civil, Sig_Estado = @Sig_Estado, Flg_Inativo = @Flg_Inativo, Des_IP = @Des_IP, Idf_Operadora_Celular = @Idf_Operadora_Celular, Idf_Email_Situacao_Confirmacao = @Idf_Email_Situacao_Confirmacao, Idf_Email_Situacao_Validacao = @Idf_Email_Situacao_Validacao, Idf_Email_Situacao_Bloqueio = @Idf_Email_Situacao_Bloqueio, Flg_Celular_Confirmado = @Flg_Celular_Confirmado, Flg_Email_Confirmado = @Flg_Email_Confirmado WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";
        private const string SPDELETE = "DELETE FROM TAB_Pessoa_Fisica WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";
        private const string SPSELECTID = "SELECT * FROM TAB_Pessoa_Fisica WHERE Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica";
        #endregion

        #region Métodos

        #region GetParameters
        /// <summary>
        /// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        /// </summary>
        /// <returns>Lista de parâmetros SQL.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Num_CPF", SqlDbType.Decimal, 9));
            parms.Add(new SqlParameter("@Nme_Pessoa", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Ape_Pessoa", SqlDbType.VarChar, 30));
            parms.Add(new SqlParameter("@Idf_Sexo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Nacionalidade", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Cidade", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Chegada_Brasil", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Nascimento", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Nme_Mae", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Nme_Pai", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Num_RG", SqlDbType.VarChar, 20));
            parms.Add(new SqlParameter("@Dta_Expedicao_RG", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Nme_Orgao_Emissor", SqlDbType.VarChar, 20));
            parms.Add(new SqlParameter("@Sig_UF_Emissao_RG", SqlDbType.Char, 2));
            parms.Add(new SqlParameter("@Num_PIS", SqlDbType.VarChar, 11));
            parms.Add(new SqlParameter("@Num_CTPS", SqlDbType.VarChar, 8));
            parms.Add(new SqlParameter("@Des_Serie_CTPS", SqlDbType.VarChar, 5));
            parms.Add(new SqlParameter("@Sig_UF_CTPS", SqlDbType.Char, 2));
            parms.Add(new SqlParameter("@Idf_Raca", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Deficiencia", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Endereco", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Num_DDD_Telefone", SqlDbType.Char, 2));
            parms.Add(new SqlParameter("@Num_Telefone", SqlDbType.Char, 10));
            parms.Add(new SqlParameter("@Num_DDD_Celular", SqlDbType.Char, 2));
            parms.Add(new SqlParameter("@Num_Celular", SqlDbType.Char, 10));
            parms.Add(new SqlParameter("@Eml_Pessoa", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Flg_Possui_Dependentes", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Alteracao", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Flg_Importado", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Idf_Escolaridade", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Nme_Pessoa_Pesquisa", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Idf_Estado_Civil", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Sig_Estado", SqlDbType.Char, 2));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Des_IP", SqlDbType.Char, 15));
            parms.Add(new SqlParameter("@Idf_Operadora_Celular", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Email_Situacao_Confirmacao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Email_Situacao_Validacao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Email_Situacao_Bloqueio", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Flg_Celular_Confirmado", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Email_Confirmado", SqlDbType.Bit, 1));
            return (parms);
        }
        #endregion

        #region SetParameters
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idPessoaFisica;
            parms[1].Value = this._numeroCPF;
            parms[2].Value = this._nomePessoa;

            if (!String.IsNullOrEmpty(this._apelidoPessoa))
                parms[3].Value = this._apelidoPessoa;
            else
                parms[3].Value = DBNull.Value;


            if (this._sexo != null)
                parms[4].Value = this._sexo.IdSexo;
            else
                parms[4].Value = DBNull.Value;


            if (this._nacionalidade != null)
                parms[5].Value = this._nacionalidade.IdNacionalidade;
            else
                parms[5].Value = DBNull.Value;


            if (this._cidade != null)
                parms[6].Value = this._cidade.IdCidade;
            else
                parms[6].Value = DBNull.Value;


            if (this._dataChegadaBrasil.HasValue)
                parms[7].Value = this._dataChegadaBrasil;
            else
                parms[7].Value = DBNull.Value;

            parms[8].Value = this._dataNascimento;

            if (!String.IsNullOrEmpty(this._nomeMae))
                parms[9].Value = this._nomeMae;
            else
                parms[9].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._nomePai))
                parms[10].Value = this._nomePai;
            else
                parms[10].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroRG))
                parms[11].Value = this._numeroRG;
            else
                parms[11].Value = DBNull.Value;


            if (this._dataExpedicaoRG.HasValue)
                parms[12].Value = this._dataExpedicaoRG;
            else
                parms[12].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._nomeOrgaoEmissor))
                parms[13].Value = this._nomeOrgaoEmissor;
            else
                parms[13].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._siglaUFEmissaoRG))
                parms[14].Value = this._siglaUFEmissaoRG;
            else
                parms[14].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroPIS))
                parms[15].Value = this._numeroPIS;
            else
                parms[15].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroCTPS))
                parms[16].Value = this._numeroCTPS;
            else
                parms[16].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoSerieCTPS))
                parms[17].Value = this._descricaoSerieCTPS;
            else
                parms[17].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._siglaUFCTPS))
                parms[18].Value = this._siglaUFCTPS;
            else
                parms[18].Value = DBNull.Value;


            if (this._raca != null)
                parms[19].Value = this._raca.IdRaca;
            else
                parms[19].Value = DBNull.Value;


            if (this._deficiencia != null)
                parms[20].Value = this._deficiencia.IdDeficiencia;
            else
                parms[20].Value = DBNull.Value;


            if (this._endereco != null)
                parms[21].Value = this._endereco.IdEndereco;
            else
                parms[21].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroDDDTelefone))
                parms[22].Value = this._numeroDDDTelefone;
            else
                parms[22].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroTelefone))
                parms[23].Value = this._numeroTelefone;
            else
                parms[23].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroDDDCelular))
                parms[24].Value = this._numeroDDDCelular;
            else
                parms[24].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._numeroCelular))
                parms[25].Value = this._numeroCelular;
            else
                parms[25].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._emailPessoa))
                parms[26].Value = this._emailPessoa;
            else
                parms[26].Value = DBNull.Value;


            if (this._flagPossuiDependentes.HasValue)
                parms[27].Value = this._flagPossuiDependentes;
            else
                parms[27].Value = DBNull.Value;


            if (this._flagImportado.HasValue)
                parms[30].Value = this._flagImportado;
            else
                parms[30].Value = DBNull.Value;


            if (this._escolaridade != null)
                parms[31].Value = this._escolaridade.IdEscolaridade;
            else
                parms[31].Value = DBNull.Value;

            parms[32].Value = this._nomePessoaPesquisa;

            if (this._estadoCivil != null)
                parms[33].Value = this._estadoCivil.IdEstadoCivil;
            else
                parms[33].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._siglaEstado))
                parms[34].Value = this._siglaEstado;
            else
                parms[34].Value = DBNull.Value;


            if (this._flagInativo.HasValue)
                parms[35].Value = this._flagInativo;
            else
                parms[35].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoIP))
                parms[36].Value = this._descricaoIP;
            else
                parms[36].Value = DBNull.Value;


            if (this._operadoraCelular != null)
                parms[37].Value = this._operadoraCelular.IdOperadoraCelular;
            else
                parms[37].Value = DBNull.Value;


            if (this._emailSituacaoConfirmacao != null)
                parms[38].Value = this._emailSituacaoConfirmacao.IdEmailSituacao;
            else
                parms[38].Value = DBNull.Value;


            if (this._emailSituacaoValidacao != null)
                parms[39].Value = this._emailSituacaoValidacao.IdEmailSituacao;
            else
                parms[39].Value = DBNull.Value;


            if (this._emailSituacaoBloqueio != null)
                parms[40].Value = this._emailSituacaoBloqueio.IdEmailSituacao;
            else
                parms[40].Value = DBNull.Value;

            parms[41].Value = this._flagCelularConfirmado;
            parms[42].Value = this._flagEmailConfirmado;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                this._dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[28].Value = this._dataCadastro;
            this._dataAlteracao = DateTime.Now;
            parms[29].Value = this._dataAlteracao;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de PessoaFisica no banco de dados.
        /// </summary>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert()
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);

            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
                        this._idPessoaFisica = Convert.ToInt32(cmd.Parameters["@Idf_Pessoa_Fisica"].Value);
                        cmd.Parameters.Clear();
                        this._persisted = true;
                        this._modified = false;
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        /// <summary>
        /// Método utilizado para inserir uma instância de PessoaFisica no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idPessoaFisica = Convert.ToInt32(cmd.Parameters["@Idf_Pessoa_Fisica"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de PessoaFisica no banco de dados.
        /// </summary>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Update()
        {
            if (this._modified)
            {
                List<SqlParameter> parms = GetParameters();
                SetParameters(parms);
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPUPDATE, parms);
                this._modified = false;
            }
        }
        /// <summary>
        /// Método utilizado para atualizar uma instância de PessoaFisica no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Update(SqlTransaction trans)
        {
            if (this._modified)
            {
                List<SqlParameter> parms = GetParameters();
                SetParameters(parms);
                DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPUPDATE, parms);
                this._modified = false;
            }
        }
        #endregion

        #region Save
        /// <summary>
        /// Método utilizado para salvar uma instância de PessoaFisica no banco de dados.
        /// </summary>
        /// <remarks>Gieyson Stelmak</remarks>
        public void Save()
        {
            if (!this._persisted)
                this.Insert();
            else
                this.Update();
        }
        /// <summary>
        /// Método utilizado para salvar uma instância de PessoaFisica no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public void Save(SqlTransaction trans)
        {
            if (!this._persisted)
                this.Insert(trans);
            else
                this.Update(trans);
        }
        #endregion

        #region Delete
        /// <summary>
        /// Método utilizado para excluir uma instância de PessoaFisica no banco de dados.
        /// </summary>
        /// <param name="idPessoaFisica">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idPessoaFisica)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));

            parms[0].Value = idPessoaFisica;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de PessoaFisica no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPessoaFisica">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idPessoaFisica, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));

            parms[0].Value = idPessoaFisica;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de PessoaFisica no banco de dados.
        /// </summary>
        /// <param name="idPessoaFisica">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idPessoaFisica)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from TAB_Pessoa_Fisica where Idf_Pessoa_Fisica in (";

            for (int i = 0; i < idPessoaFisica.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idPessoaFisica[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idPessoaFisica">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idPessoaFisica)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));

            parms[0].Value = idPessoaFisica;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPessoaFisica">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idPessoaFisica, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));

            parms[0].Value = idPessoaFisica;

            return DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar uma consulta paginada do banco de dados.
        /// </summary>
        /// <param name="colunaOrdenacao">Nome da coluna pela qual será ordenada.</param>
        /// <param name="direcaoOrdenacao">Direção da ordenação (ASC ou DESC).</param>
        /// <param name="paginaCorrente">Número da página que será exibida.</param>
        /// <param name="tamanhoPagina">Quantidade de itens em cada página.</param>
        /// <param name="totalRegistros">Quantidade total de registros na tabela.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        public static IDataReader LoadDataReader(string colunaOrdenacao, string direcaoOrdenacao, int paginaCorrente, int tamanhoPagina, out int totalRegistros)
        {
            int inicio = ((paginaCorrente - 1) * tamanhoPagina) + 1;
            int fim = paginaCorrente * tamanhoPagina;

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pes.Idf_Pessoa_Fisica, Pes.Num_CPF, Pes.Nme_Pessoa, Pes.Ape_Pessoa, Pes.Idf_Sexo, Pes.Idf_Nacionalidade, Pes.Idf_Cidade, Pes.Dta_Chegada_Brasil, Pes.Dta_Nascimento, Pes.Nme_Mae, Pes.Nme_Pai, Pes.Num_RG, Pes.Dta_Expedicao_RG, Pes.Nme_Orgao_Emissor, Pes.Sig_UF_Emissao_RG, Pes.Num_PIS, Pes.Num_CTPS, Pes.Des_Serie_CTPS, Pes.Sig_UF_CTPS, Pes.Idf_Raca, Pes.Idf_Deficiencia, Pes.Idf_Endereco, Pes.Num_DDD_Telefone, Pes.Num_Telefone, Pes.Num_DDD_Celular, Pes.Num_Celular, Pes.Eml_Pessoa, Pes.Flg_Possui_Dependentes, Pes.Dta_Cadastro, Pes.Dta_Alteracao, Pes.Flg_Importado, Pes.Idf_Escolaridade, Pes.Nme_Pessoa_Pesquisa, Pes.Idf_Estado_Civil, Pes.Sig_Estado, Pes.Flg_Inativo, Pes.Des_IP, Pes.Idf_Operadora_Celular, Pes.Idf_Email_Situacao_Confirmacao, Pes.Idf_Email_Situacao_Validacao, Pes.Idf_Email_Situacao_Bloqueio, Pes.Flg_Celular_Confirmado FROM TAB_Pessoa_Fisica Pes";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de PessoaFisica a partir do banco de dados.
        /// </summary>
        /// <param name="idPessoaFisica">Chave do registro.</param>
        /// <returns>Instância de PessoaFisica.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PessoaFisica LoadObject(int idPessoaFisica)
        {
            using (IDataReader dr = LoadDataReader(idPessoaFisica))
            {
                PessoaFisica objPessoaFisica = new PessoaFisica();
                if (SetInstance(dr, objPessoaFisica))
                    return objPessoaFisica;
            }
            throw (new RecordNotFoundException(typeof(PessoaFisica)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de PessoaFisica a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPessoaFisica">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de PessoaFisica.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static PessoaFisica LoadObject(int idPessoaFisica, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idPessoaFisica, trans))
            {
                PessoaFisica objPessoaFisica = new PessoaFisica();
                if (SetInstance(dr, objPessoaFisica))
                    return objPessoaFisica;
            }
            throw (new RecordNotFoundException(typeof(PessoaFisica)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de PessoaFisica a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idPessoaFisica))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de PessoaFisica a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idPessoaFisica, trans))
            {
                return SetInstance(dr, this);
            }
        }
        #endregion

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objPessoaFisica">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, PessoaFisica objPessoaFisica, bool dispose = true)
        {
            try
            {
                if (dr.Read())
                {
                    objPessoaFisica._idPessoaFisica = Convert.ToInt32(dr["Idf_Pessoa_Fisica"]);
                    objPessoaFisica._numeroCPF = Convert.ToDecimal(dr["Num_CPF"]);
                    objPessoaFisica._nomePessoa = Convert.ToString(dr["Nme_Pessoa"]);
                    if (dr["Ape_Pessoa"] != DBNull.Value)
                        objPessoaFisica._apelidoPessoa = Convert.ToString(dr["Ape_Pessoa"]);
                    if (dr["Idf_Sexo"] != DBNull.Value)
                        objPessoaFisica._sexo = new Sexo(Convert.ToInt32(dr["Idf_Sexo"]));
                    if (dr["Idf_Nacionalidade"] != DBNull.Value)
                        objPessoaFisica._nacionalidade = new Nacionalidade(Convert.ToInt32(dr["Idf_Nacionalidade"]));
                    if (dr["Idf_Cidade"] != DBNull.Value)
                        objPessoaFisica._cidade = new Cidade(Convert.ToInt32(dr["Idf_Cidade"]));
                    if (dr["Dta_Chegada_Brasil"] != DBNull.Value)
                        objPessoaFisica._dataChegadaBrasil = Convert.ToDateTime(dr["Dta_Chegada_Brasil"]);
                    objPessoaFisica._dataNascimento = Convert.ToDateTime(dr["Dta_Nascimento"]);
                    if (dr["Nme_Mae"] != DBNull.Value)
                        objPessoaFisica._nomeMae = Convert.ToString(dr["Nme_Mae"]);
                    if (dr["Nme_Pai"] != DBNull.Value)
                        objPessoaFisica._nomePai = Convert.ToString(dr["Nme_Pai"]);
                    if (dr["Num_RG"] != DBNull.Value)
                        objPessoaFisica._numeroRG = Convert.ToString(dr["Num_RG"]);
                    if (dr["Dta_Expedicao_RG"] != DBNull.Value)
                        objPessoaFisica._dataExpedicaoRG = Convert.ToDateTime(dr["Dta_Expedicao_RG"]);
                    if (dr["Nme_Orgao_Emissor"] != DBNull.Value)
                        objPessoaFisica._nomeOrgaoEmissor = Convert.ToString(dr["Nme_Orgao_Emissor"]);
                    if (dr["Sig_UF_Emissao_RG"] != DBNull.Value)
                        objPessoaFisica._siglaUFEmissaoRG = Convert.ToString(dr["Sig_UF_Emissao_RG"]);
                    if (dr["Num_PIS"] != DBNull.Value)
                        objPessoaFisica._numeroPIS = Convert.ToString(dr["Num_PIS"]);
                    if (dr["Num_CTPS"] != DBNull.Value)
                        objPessoaFisica._numeroCTPS = Convert.ToString(dr["Num_CTPS"]);
                    if (dr["Des_Serie_CTPS"] != DBNull.Value)
                        objPessoaFisica._descricaoSerieCTPS = Convert.ToString(dr["Des_Serie_CTPS"]);
                    if (dr["Sig_UF_CTPS"] != DBNull.Value)
                        objPessoaFisica._siglaUFCTPS = Convert.ToString(dr["Sig_UF_CTPS"]);
                    if (dr["Idf_Raca"] != DBNull.Value)
                        objPessoaFisica._raca = new Raca(Convert.ToInt32(dr["Idf_Raca"]));
                    if (dr["Idf_Deficiencia"] != DBNull.Value)
                        objPessoaFisica._deficiencia = new Deficiencia(Convert.ToInt32(dr["Idf_Deficiencia"]));
                    if (dr["Idf_Endereco"] != DBNull.Value)
                        objPessoaFisica._endereco = new Endereco(Convert.ToInt32(dr["Idf_Endereco"]));
                    if (dr["Num_DDD_Telefone"] != DBNull.Value)
                        objPessoaFisica._numeroDDDTelefone = Convert.ToString(dr["Num_DDD_Telefone"]);
                    if (dr["Num_Telefone"] != DBNull.Value)
                        objPessoaFisica._numeroTelefone = Convert.ToString(dr["Num_Telefone"]);
                    if (dr["Num_DDD_Celular"] != DBNull.Value)
                        objPessoaFisica._numeroDDDCelular = Convert.ToString(dr["Num_DDD_Celular"]);
                    if (dr["Num_Celular"] != DBNull.Value)
                        objPessoaFisica._numeroCelular = Convert.ToString(dr["Num_Celular"]);
                    if (dr["Eml_Pessoa"] != DBNull.Value)
                        objPessoaFisica._emailPessoa = Convert.ToString(dr["Eml_Pessoa"]);
                    if (dr["Flg_Possui_Dependentes"] != DBNull.Value)
                        objPessoaFisica._flagPossuiDependentes = Convert.ToBoolean(dr["Flg_Possui_Dependentes"]);
                    objPessoaFisica._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objPessoaFisica._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
                    if (dr["Flg_Importado"] != DBNull.Value)
                        objPessoaFisica._flagImportado = Convert.ToBoolean(dr["Flg_Importado"]);
                    if (dr["Idf_Escolaridade"] != DBNull.Value)
                        objPessoaFisica._escolaridade = new Escolaridade(Convert.ToInt32(dr["Idf_Escolaridade"]));
                    objPessoaFisica._nomePessoaPesquisa = Convert.ToString(dr["Nme_Pessoa_Pesquisa"]);
                    if (dr["Idf_Estado_Civil"] != DBNull.Value)
                        objPessoaFisica._estadoCivil = new EstadoCivil(Convert.ToInt32(dr["Idf_Estado_Civil"]));
                    if (dr["Sig_Estado"] != DBNull.Value)
                        objPessoaFisica._siglaEstado = Convert.ToString(dr["Sig_Estado"]);
                    if (dr["Flg_Inativo"] != DBNull.Value)
                        objPessoaFisica._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    if (dr["Des_IP"] != DBNull.Value)
                        objPessoaFisica._descricaoIP = Convert.ToString(dr["Des_IP"]);
                    if (dr["Idf_Operadora_Celular"] != DBNull.Value)
                        objPessoaFisica._operadoraCelular = new OperadoraCelular(Convert.ToInt32(dr["Idf_Operadora_Celular"]));
                    if (dr["Idf_Email_Situacao_Confirmacao"] != DBNull.Value)
                        objPessoaFisica._emailSituacaoConfirmacao = new EmailSituacao(Convert.ToInt32(dr["Idf_Email_Situacao_Confirmacao"]));
                    if (dr["Idf_Email_Situacao_Validacao"] != DBNull.Value)
                        objPessoaFisica._emailSituacaoValidacao = new EmailSituacao(Convert.ToInt32(dr["Idf_Email_Situacao_Validacao"]));
                    if (dr["Idf_Email_Situacao_Bloqueio"] != DBNull.Value)
                        objPessoaFisica._emailSituacaoBloqueio = new EmailSituacao(Convert.ToInt32(dr["Idf_Email_Situacao_Bloqueio"]));
                    if (dr["Flg_Celular_Confirmado"] != DBNull.Value)
                        objPessoaFisica._flagCelularConfirmado = Convert.ToBoolean(dr["Flg_Celular_Confirmado"]);
                    if (dr["Flg_Email_Confirmado"] != DBNull.Value)
                        objPessoaFisica._flagEmailConfirmado = Convert.ToBoolean(dr["Flg_Email_Confirmado"]);

                    objPessoaFisica._persisted = true;
                    objPessoaFisica._modified = false;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                dr.Dispose();
            }
        }
        #endregion

        #endregion
    }
}