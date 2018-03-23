//-- Data: 16/03/2013 15:24
//-- Autor: Gieyson Stelmak

using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;
using Microsoft.SqlServer.Types;

namespace BNE.BLL
{
    public partial class Curriculo // Tabela: BNE_Curriculo
    {
        #region Atributos
        private int _idCurriculo;
        private PessoaFisica _pessoaFisica;
        private decimal? _valorPretensaoSalarial;
        private TipoCurriculo _tipoCurriculo;
        private SituacaoCurriculo _situacaoCurriculo;
        private string _descricaoMiniCurriculo;
        private DateTime _dataCadastro;
        private DateTime _dataAtualizacao;
        private bool? _flagManha;
        private bool? _flagTarde;
        private bool? _flagNoite;
        private string _observacaoCurriculo;
        private string _descricaoAnalise;
        private string _descricaoSugestaoCarreira;
        private string _descricaoCursosOferecidos;
        private bool _flagInativo;
        private decimal? _valorUltimoSalario;
        private string _descricaoIP;
        private Cidade _cidadePretendida;
        private bool? _flagFinalSemana;
        private bool _flagVIP;
        private bool? _flagBoasVindas;
        private bool _flagMSN;
        private Cidade _cidadeEndereco;
        private SqlGeography _descricaoLocalizacao;
        private DateTime? _dataModificacaoCV;
        private DateTime? _dataAceitePoliticaPrivacidade;

        private bool _persisted;
        private bool _modified;

        private List<string> _modifiedFields = new List<string>();
        #endregion

        #region Propriedades

        #region IdCurriculo
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdCurriculo
        {
            get
            {
                return this._idCurriculo;
            }
        }
        #endregion

        #region PessoaFisica
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public PessoaFisica PessoaFisica
        {
            get
            {
                return this._pessoaFisica;
            }
            set
            {
                if(!_modifiedFields.Contains("Idf_Pessoa_Fisica")) _modifiedFields.Add("Idf_Pessoa_Fisica");

                this._pessoaFisica = value;
                this._modified = true;
            }
        }
        #endregion

        #region ValorPretensaoSalarial
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public decimal? ValorPretensaoSalarial
        {
            get
            {
                return this._valorPretensaoSalarial;
            }
            set
            {
                if (!_modifiedFields.Contains("Vlr_Pretensao_Salarial")) _modifiedFields.Add("Vlr_Pretensao_Salarial");
                this._valorPretensaoSalarial = value;
                this._modified = true;
            }
        }
        #endregion

        #region TipoCurriculo
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public TipoCurriculo TipoCurriculo
        {
            get
            {
                return this._tipoCurriculo;
            }
            set
            {
                if (!_modifiedFields.Contains("Idf_Tipo_Curriculo")) _modifiedFields.Add("Idf_Tipo_Curriculo");
                this._tipoCurriculo = value;
                this._modified = true;
            }
        }
        #endregion

        #region SituacaoCurriculo
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public SituacaoCurriculo SituacaoCurriculo
        {
            get
            {
                return this._situacaoCurriculo;
            }
            set
            {
                if (!_modifiedFields.Contains("Idf_Situacao_Curriculo")) _modifiedFields.Add("Idf_Situacao_Curriculo");
                this._situacaoCurriculo = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoMiniCurriculo
        /// <summary>
        /// Tamanho do campo: -1.
        /// Campo opcional.
        /// </summary>
        public string DescricaoMiniCurriculo
        {
            get
            {
                return this._descricaoMiniCurriculo;
            }
            set
            {
                if (!_modifiedFields.Contains("Des_Mini_Curriculo")) _modifiedFields.Add("Des_Mini_Curriculo");
                this._descricaoMiniCurriculo = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataAtualizacao
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public DateTime DataAtualizacao
        {
            get
            {
                return this._dataAtualizacao;
            }
            set
            {
                if (!_modifiedFields.Contains("Dta_Atualizacao")) _modifiedFields.Add("Dta_Atualizacao");

                this._dataAtualizacao = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagManha
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public bool? FlagManha
        {
            get
            {
                return this._flagManha;
            }
            set
            {
                if (!_modifiedFields.Contains("Flg_Manha")) _modifiedFields.Add("Flg_Manha");
                this._flagManha = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagTarde
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public bool? FlagTarde
        {
            get
            {
                return this._flagTarde;
            }
            set
            {
                if (!_modifiedFields.Contains("Flg_Tarde")) _modifiedFields.Add("Flg_Tarde");
                this._flagTarde = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagNoite
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public bool? FlagNoite
        {
            get
            {
                return this._flagNoite;
            }
            set
            {
                if (!_modifiedFields.Contains("Flg_Noite")) _modifiedFields.Add("Flg_Noite");
                this._flagNoite = value;
                this._modified = true;
            }
        }
        #endregion

        #region ObservacaoCurriculo
        /// <summary>
        /// Tamanho do campo: 2000.
        /// Campo opcional.
        /// </summary>
        public string ObservacaoCurriculo
        {
            get
            {
                return this._observacaoCurriculo;
            }
            set
            {
                if (!_modifiedFields.Contains("Obs_Curriculo")) _modifiedFields.Add("Obs_Curriculo");
                this._observacaoCurriculo = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoAnalise
        /// <summary>
        /// Tamanho do campo: 2000.
        /// Campo opcional.
        /// </summary>
        public string DescricaoAnalise
        {
            get
            {
                if (!_modifiedFields.Contains("Des_Analise")) _modifiedFields.Add("Des_Analise");
                return this._descricaoAnalise;
            }
            set
            {
                this._descricaoAnalise = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoSugestaoCarreira
        /// <summary>
        /// Tamanho do campo: 2000.
        /// Campo opcional.
        /// </summary>
        public string DescricaoSugestaoCarreira
        {
            get
            {
                return this._descricaoSugestaoCarreira;
            }
            set
            {
                if (!_modifiedFields.Contains("Des_Sugestao_Carreira")) _modifiedFields.Add("Des_Sugestao_Carreira");
                this._descricaoSugestaoCarreira = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoCursosOferecidos
        /// <summary>
        /// Tamanho do campo: 2000.
        /// Campo opcional.
        /// </summary>
        public string DescricaoCursosOferecidos
        {
            get
            {
                return this._descricaoCursosOferecidos;
            }
            set
            {
                if (!_modifiedFields.Contains("Des_Cursos_Oferecidos")) _modifiedFields.Add("Des_Cursos_Oferecidos");
                this._descricaoCursosOferecidos = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagInativo
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagInativo
        {
            get
            {
                return this._flagInativo;
            }
            set
            {
                if (!_modifiedFields.Contains("Flg_Inativo")) _modifiedFields.Add("Flg_Inativo");
                this._flagInativo = value;
                this._modified = true;
            }
        }
        #endregion

        #region ValorUltimoSalario
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public decimal? ValorUltimoSalario
        {
            get
            {
                return this._valorUltimoSalario;
            }
            set
            {
                if (!_modifiedFields.Contains("Vlr_Ultimo_Salario")) _modifiedFields.Add("Vlr_Ultimo_Salario");
                this._valorUltimoSalario = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoIP
        /// <summary>
        /// Tamanho do campo: 15.
        /// Campo obrigatório.
        /// </summary>
        public string DescricaoIP
        {
            get
            {
                return this._descricaoIP;
            }
            set
            {
                if (!_modifiedFields.Contains("Des_IP")) _modifiedFields.Add("Des_IP");
                this._descricaoIP = value;
                this._modified = true;
            }
        }
        #endregion

        #region CidadePretendida
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Cidade CidadePretendida
        {
            get
            {
                return this._cidadePretendida;
            }
            set
            {
                if (!_modifiedFields.Contains("Idf_Cidade_Pretendida")) _modifiedFields.Add("Idf_Cidade_Pretendida");
                this._cidadePretendida = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagFinalSemana
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public bool? FlagFinalSemana
        {
            get
            {
                return this._flagFinalSemana;
            }
            set
            {
                if (!_modifiedFields.Contains("Flg_Final_Semana")) _modifiedFields.Add("Flg_Final_Semana");
                this._flagFinalSemana = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagVIP
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagVIP
        {
            get
            {
                return this._flagVIP;
            }
            set
            {
                if (!_modifiedFields.Contains("Flg_VIP")) _modifiedFields.Add("Flg_VIP");
                this._flagVIP = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagBoasVindas
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public bool? FlagBoasVindas
        {
            get
            {
                return this._flagBoasVindas;
            }
            set
            {
                if (!_modifiedFields.Contains("Flg_Boas_Vindas")) _modifiedFields.Add("Flg_Boas_Vindas");
                this._flagBoasVindas = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagMSN
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagMSN
        {
            get
            {
                return this._flagMSN;
            }
            set
            {
                if (!_modifiedFields.Contains("Flg_MSN")) _modifiedFields.Add("Flg_MSN");
                this._flagMSN = value;
                this._modified = true;
            }
        }
        #endregion

        #region CidadeEndereco
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Cidade CidadeEndereco
        {
            get
            {
                return this._cidadeEndereco;
            }
            set
            {
                if (!_modifiedFields.Contains("Idf_Cidade_Endereco")) _modifiedFields.Add("Idf_Cidade_Endereco");
                this._cidadeEndereco = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoLocalizacao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public SqlGeography DescricaoLocalizacao
        {
            get
            {
                return this._descricaoLocalizacao;
            }
            set
            {
                if (!_modifiedFields.Contains("Des_Localizacao")) _modifiedFields.Add("Des_Localizacao");
                this._descricaoLocalizacao = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataModificacaoCV
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataModificacaoCV
        {
            get
            {
                return this._dataModificacaoCV;
            }
            set
            {
                if (!_modifiedFields.Contains("Dta_Modificacao_CV")) _modifiedFields.Add("Dta_Modificacao_CV");
                this._dataModificacaoCV = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataAceitePoliticaPrivacidade
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataAceitePoliticaPrivacidade
        {
            get
            {
                return this._dataAceitePoliticaPrivacidade;
            }
            set
            {
                if (!_modifiedFields.Contains("Dta_Aceite_Politica_Privacidade")) _modifiedFields.Add("Dta_Aceite_Politica_Privacidade");
                this._dataAceitePoliticaPrivacidade = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public Curriculo()
        {
        }
        public Curriculo(int idCurriculo)
        {
            this._idCurriculo = idCurriculo;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Curriculo (Idf_Pessoa_Fisica, Vlr_Pretensao_Salarial, Idf_Tipo_Curriculo, Idf_Situacao_Curriculo, Des_Mini_Curriculo, Dta_Cadastro, Dta_Atualizacao, Flg_Manha, Flg_Tarde, Flg_Noite, Obs_Curriculo, Des_Analise, Des_Sugestao_Carreira, Des_Cursos_Oferecidos, Flg_Inativo, Vlr_Ultimo_Salario, Des_IP, Idf_Cidade_Pretendida, Flg_Final_Semana, Flg_VIP, Flg_Boas_Vindas, Flg_MSN, Idf_Cidade_Endereco, Des_Localizacao, Dta_Modificacao_CV, Dta_Aceite_Politica_Privacidade) VALUES (@Idf_Pessoa_Fisica, @Vlr_Pretensao_Salarial, @Idf_Tipo_Curriculo, @Idf_Situacao_Curriculo, @Des_Mini_Curriculo, @Dta_Cadastro, @Dta_Atualizacao, @Flg_Manha, @Flg_Tarde, @Flg_Noite, @Obs_Curriculo, @Des_Analise, @Des_Sugestao_Carreira, @Des_Cursos_Oferecidos, @Flg_Inativo, @Vlr_Ultimo_Salario, @Des_IP, @Idf_Cidade_Pretendida, @Flg_Final_Semana, @Flg_VIP, @Flg_Boas_Vindas, @Flg_MSN, @Idf_Cidade_Endereco, @Des_Localizacao, @Dta_Modificacao_CV, @Dta_Aceite_Politica_Privacidade);SET @Idf_Curriculo = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Curriculo SET Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica, Vlr_Pretensao_Salarial = @Vlr_Pretensao_Salarial, Idf_Tipo_Curriculo = @Idf_Tipo_Curriculo, Idf_Situacao_Curriculo = @Idf_Situacao_Curriculo, Des_Mini_Curriculo = @Des_Mini_Curriculo, Dta_Cadastro = @Dta_Cadastro, Dta_Atualizacao = @Dta_Atualizacao, Flg_Manha = @Flg_Manha, Flg_Tarde = @Flg_Tarde, Flg_Noite = @Flg_Noite, Obs_Curriculo = @Obs_Curriculo, Des_Analise = @Des_Analise, Des_Sugestao_Carreira = @Des_Sugestao_Carreira, Des_Cursos_Oferecidos = @Des_Cursos_Oferecidos, Flg_Inativo = @Flg_Inativo, Vlr_Ultimo_Salario = @Vlr_Ultimo_Salario, Des_IP = @Des_IP, Idf_Cidade_Pretendida = @Idf_Cidade_Pretendida, Flg_Final_Semana = @Flg_Final_Semana, Flg_VIP = @Flg_VIP, Flg_Boas_Vindas = @Flg_Boas_Vindas, Flg_MSN = @Flg_MSN, Idf_Cidade_Endereco = @Idf_Cidade_Endereco, Des_Localizacao = @Des_Localizacao, Dta_Modificacao_CV = @Dta_Modificacao_CV, Dta_Aceite_Politica_Privacidade = @Dta_Aceite_Politica_Privacidade WHERE Idf_Curriculo = @Idf_Curriculo";
        private const string SPDELETE = "DELETE FROM BNE_Curriculo WHERE Idf_Curriculo = @Idf_Curriculo";
        private const string SPSELECTID = "SELECT * FROM BNE_Curriculo with(nolock) WHERE Idf_Curriculo = @Idf_Curriculo";
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
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Vlr_Pretensao_Salarial", SqlDbType.Decimal, 9));
            parms.Add(new SqlParameter("@Idf_Tipo_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Situacao_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Des_Mini_Curriculo", SqlDbType.VarChar));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Atualizacao", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Flg_Manha", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Tarde", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Noite", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Obs_Curriculo", SqlDbType.VarChar, 2000));
            parms.Add(new SqlParameter("@Des_Analise", SqlDbType.VarChar, 2000));
            parms.Add(new SqlParameter("@Des_Sugestao_Carreira", SqlDbType.VarChar, 2000));
            parms.Add(new SqlParameter("@Des_Cursos_Oferecidos", SqlDbType.VarChar, 2000));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Vlr_Ultimo_Salario", SqlDbType.Decimal, 9));
            parms.Add(new SqlParameter("@Des_IP", SqlDbType.Char, 15));
            parms.Add(new SqlParameter("@Idf_Cidade_Pretendida", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Flg_Final_Semana", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_VIP", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Boas_Vindas", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_MSN", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Idf_Cidade_Endereco", SqlDbType.Int, 4));
            parms.Add(new SqlParameter { ParameterName = "@Des_Localizacao", UdtTypeName = "Geography" });
            parms.Add(new SqlParameter("@Dta_Modificacao_CV", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Aceite_Politica_Privacidade", SqlDbType.DateTime, 8));
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
            parms[0].Value = this._idCurriculo;
            parms[1].Value = this._pessoaFisica.IdPessoaFisica;

            if (this._valorPretensaoSalarial.HasValue)
                parms[2].Value = this._valorPretensaoSalarial;
            else
                parms[2].Value = DBNull.Value;

            parms[3].Value = this._tipoCurriculo.IdTipoCurriculo;
            parms[4].Value = this._situacaoCurriculo.IdSituacaoCurriculo;

            if (!String.IsNullOrEmpty(this._descricaoMiniCurriculo))
                parms[5].Value = this._descricaoMiniCurriculo;
            else
                parms[5].Value = DBNull.Value;

            parms[7].Value = this._dataAtualizacao;

            if (this._flagManha.HasValue)
                parms[8].Value = this._flagManha;
            else
                parms[8].Value = DBNull.Value;


            if (this._flagTarde.HasValue)
                parms[9].Value = this._flagTarde;
            else
                parms[9].Value = DBNull.Value;


            if (this._flagNoite.HasValue)
                parms[10].Value = this._flagNoite;
            else
                parms[10].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._observacaoCurriculo))
                parms[11].Value = this._observacaoCurriculo;
            else
                parms[11].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoAnalise))
                parms[12].Value = this._descricaoAnalise;
            else
                parms[12].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoSugestaoCarreira))
                parms[13].Value = this._descricaoSugestaoCarreira;
            else
                parms[13].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoCursosOferecidos))
                parms[14].Value = this._descricaoCursosOferecidos;
            else
                parms[14].Value = DBNull.Value;

            parms[15].Value = this._flagInativo;

            if (this._valorUltimoSalario.HasValue)
                parms[16].Value = this._valorUltimoSalario;
            else
                parms[16].Value = DBNull.Value;

            parms[17].Value = this._descricaoIP;

            if (this._cidadePretendida != null)
                parms[18].Value = this._cidadePretendida.IdCidade;
            else
                parms[18].Value = DBNull.Value;


            if (this._flagFinalSemana.HasValue)
                parms[19].Value = this._flagFinalSemana;
            else
                parms[19].Value = DBNull.Value;

            parms[20].Value = this._flagVIP;

            if (this._flagBoasVindas.HasValue)
                parms[21].Value = this._flagBoasVindas;
            else
                parms[21].Value = DBNull.Value;

            parms[22].Value = this._flagMSN;

            if (this._cidadeEndereco != null)
                parms[23].Value = this._cidadeEndereco.IdCidade;
            else
                parms[23].Value = DBNull.Value;

            if (this._descricaoLocalizacao != null)
                parms[24].Value = this._descricaoLocalizacao;
            else
                parms[24].Value = SqlGeography.Null;

            if (this._dataModificacaoCV.HasValue)
                parms[25].Value = this._dataModificacaoCV;
            else
                parms[25].Value = DBNull.Value;

            if (this._dataAceitePoliticaPrivacidade.HasValue)
                parms[26].Value = this._dataAceitePoliticaPrivacidade;
            else
                parms[26].Value = DBNull.Value;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                this._dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[6].Value = this._dataCadastro;
        }
        #endregion

        #region Save
        /// <summary>
        /// Método utilizado para salvar uma instância de Curriculo no banco de dados.
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
        /// Método utilizado para salvar uma instância de Curriculo no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de Curriculo no banco de dados.
        /// </summary>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idCurriculo)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

            parms[0].Value = idCurriculo;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de Curriculo no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idCurriculo, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

            parms[0].Value = idCurriculo;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de Curriculo no banco de dados.
        /// </summary>
        /// <param name="idCurriculo">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idCurriculo)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Curriculo where Idf_Curriculo in (";

            for (int i = 0; i < idCurriculo.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idCurriculo[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idCurriculo)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

            parms[0].Value = idCurriculo;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idCurriculo, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));

            parms[0].Value = idCurriculo;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cur.Idf_Curriculo, Cur.Idf_Pessoa_Fisica, Cur.Vlr_Pretensao_Salarial, Cur.Idf_Tipo_Curriculo, Cur.Idf_Situacao_Curriculo, Cur.Des_Mini_Curriculo, Cur.Dta_Cadastro, Cur.Dta_Atualizacao, Cur.Flg_Manha, Cur.Flg_Tarde, Cur.Flg_Noite, Cur.Obs_Curriculo, Cur.Des_Analise, Cur.Des_Sugestao_Carreira, Cur.Des_Cursos_Oferecidos, Cur.Flg_Inativo, Cur.Vlr_Ultimo_Salario, Cur.Des_IP, Cur.Idf_Cidade_Pretendida, Cur.Flg_Final_Semana, Cur.Flg_VIP, Cur.Flg_Boas_Vindas, Cur.Flg_MSN, Cur.Idf_Cidade_Endereco, Cur.Dta_Modificacao_CV FROM BNE_Curriculo Cur";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de Curriculo a partir do banco de dados.
        /// </summary>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <returns>Instância de Curriculo.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Curriculo LoadObject(int idCurriculo)
        {
            using (IDataReader dr = LoadDataReader(idCurriculo))
            {
                Curriculo objCurriculo = new Curriculo();
                if (SetInstance(dr, objCurriculo))
                    return objCurriculo;
            }
           throw (new RecordNotFoundException(typeof(Curriculo)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de Curriculo a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de Curriculo.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Curriculo LoadObject(int idCurriculo, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idCurriculo, trans))
            {
                Curriculo objCurriculo = new Curriculo();
                if (SetInstance(dr, objCurriculo))
                    return objCurriculo;
            }
            throw (new RecordNotFoundException(typeof(Curriculo)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de Curriculo a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idCurriculo))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de Curriculo a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idCurriculo, trans))
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
        /// <param name="objCurriculo">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, Curriculo objCurriculo, bool dispose = true)
        {
            try
            {
                if (dr.Read())
                {
                    objCurriculo._idCurriculo = Convert.ToInt32(dr["Idf_Curriculo"]);
                    objCurriculo._pessoaFisica = new PessoaFisica(Convert.ToInt32(dr["Idf_Pessoa_Fisica"]));
                    if (dr["Vlr_Pretensao_Salarial"] != DBNull.Value)
                        objCurriculo._valorPretensaoSalarial = Convert.ToDecimal(dr["Vlr_Pretensao_Salarial"]);
                    objCurriculo._tipoCurriculo = new TipoCurriculo(Convert.ToInt32(dr["Idf_Tipo_Curriculo"]));
                    objCurriculo._situacaoCurriculo = new SituacaoCurriculo(Convert.ToInt32(dr["Idf_Situacao_Curriculo"]));
                    if (dr["Des_Mini_Curriculo"] != DBNull.Value)
                        objCurriculo._descricaoMiniCurriculo = Convert.ToString(dr["Des_Mini_Curriculo"]);
                    objCurriculo._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objCurriculo._dataAtualizacao = Convert.ToDateTime(dr["Dta_Atualizacao"]);
                    if (dr["Flg_Manha"] != DBNull.Value)
                        objCurriculo._flagManha = Convert.ToBoolean(dr["Flg_Manha"]);
                    if (dr["Flg_Tarde"] != DBNull.Value)
                        objCurriculo._flagTarde = Convert.ToBoolean(dr["Flg_Tarde"]);
                    if (dr["Flg_Noite"] != DBNull.Value)
                        objCurriculo._flagNoite = Convert.ToBoolean(dr["Flg_Noite"]);
                    if (dr["Obs_Curriculo"] != DBNull.Value)
                        objCurriculo._observacaoCurriculo = Convert.ToString(dr["Obs_Curriculo"]);
                    if (dr["Des_Analise"] != DBNull.Value)
                        objCurriculo._descricaoAnalise = Convert.ToString(dr["Des_Analise"]);
                    if (dr["Des_Sugestao_Carreira"] != DBNull.Value)
                        objCurriculo._descricaoSugestaoCarreira = Convert.ToString(dr["Des_Sugestao_Carreira"]);
                    if (dr["Des_Cursos_Oferecidos"] != DBNull.Value)
                        objCurriculo._descricaoCursosOferecidos = Convert.ToString(dr["Des_Cursos_Oferecidos"]);
                    objCurriculo._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    if (dr["Vlr_Ultimo_Salario"] != DBNull.Value)
                        objCurriculo._valorUltimoSalario = Convert.ToDecimal(dr["Vlr_Ultimo_Salario"]);
                    objCurriculo._descricaoIP = Convert.ToString(dr["Des_IP"]);
                    if (dr["Idf_Cidade_Pretendida"] != DBNull.Value)
                        objCurriculo._cidadePretendida = new Cidade(Convert.ToInt32(dr["Idf_Cidade_Pretendida"]));
                    if (dr["Flg_Final_Semana"] != DBNull.Value)
                        objCurriculo._flagFinalSemana = Convert.ToBoolean(dr["Flg_Final_Semana"]);
                    objCurriculo._flagVIP = Convert.ToBoolean(dr["Flg_VIP"]);
                    if (dr["Flg_Boas_Vindas"] != DBNull.Value)
                        objCurriculo._flagBoasVindas = Convert.ToBoolean(dr["Flg_Boas_Vindas"]);
                    objCurriculo._flagMSN = Convert.ToBoolean(dr["Flg_MSN"]);
                    if (dr["Idf_Cidade_Endereco"] != DBNull.Value)
                        objCurriculo._cidadeEndereco = new Cidade(Convert.ToInt32(dr["Idf_Cidade_Endereco"]));
                    if (dr["Des_Localizacao"] != DBNull.Value)
                        objCurriculo._descricaoLocalizacao = (SqlGeography)(dr["Des_Localizacao"]);
                    if (dr["Dta_Modificacao_CV"] != DBNull.Value)
                        objCurriculo._dataModificacaoCV = Convert.ToDateTime(dr["Dta_Modificacao_CV"]);
                    if (dr["Dta_Aceite_Politica_Privacidade"] != DBNull.Value)
                        objCurriculo._dataAceitePoliticaPrivacidade = Convert.ToDateTime(dr["Dta_Aceite_Politica_Privacidade"]);

                    objCurriculo._persisted = true;
                    objCurriculo._modified = false;

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
                if (dispose)
                    dr.Dispose();
            }
        }
        #endregion

        #endregion
    }
}