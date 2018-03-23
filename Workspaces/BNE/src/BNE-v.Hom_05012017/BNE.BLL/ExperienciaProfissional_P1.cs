//-- Data: 03/10/2013 11:28
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    
    public partial class ExperienciaProfissional // Tabela: BNE_Experiencia_Profissional
    {
        #region Atributos
        private int _idExperienciaProfissional;
        private AreaBNE _areaBNE;
        private PessoaFisica _pessoaFisica;
        private string _razaoSocial;
        private string _descricaoFuncaoExercida;
        private Funcao _funcao;
        private DateTime _dataAdmissao;
        private DateTime? _dataDemissao;
        private string _descricaoAtividade;
        private bool _flagInativo;
        private DateTime _dataCadastro;
        private bool? _flagImportado;
        private decimal? _vlrSalario;
        private string _descricaoNavegador;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdExperienciaProfissional
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdExperienciaProfissional
        {
            get
            {
                return this._idExperienciaProfissional;
            }
        }
        #endregion

        #region AreaBNE
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public AreaBNE AreaBNE
        {
            get
            {
                return this._areaBNE;
            }
            set
            {
                this._areaBNE = value;
                this._modified = true;
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
                this._pessoaFisica = value;
                this._modified = true;
            }
        }
        #endregion

        #region RazaoSocial
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo obrigatório.
        /// </summary>
        public string RazaoSocial
        {
            get
            {
                return this._razaoSocial;
            }
            set
            {
                this._razaoSocial = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoFuncaoExercida
        /// <summary>
        /// Tamanho do campo: 50.
        /// Campo opcional.
        /// </summary>
        public string DescricaoFuncaoExercida
        {
            get
            {
                return this._descricaoFuncaoExercida;
            }
            set
            {
                this._descricaoFuncaoExercida = value;
                this._modified = true;
            }
        }
        #endregion

        #region Funcao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public Funcao Funcao
        {
            get
            {
                return this._funcao;
            }
            set
            {
                this._funcao = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataAdmissao
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public DateTime DataAdmissao
        {
            get
            {
                return this._dataAdmissao;
            }
            set
            {
                this._dataAdmissao = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataDemissao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataDemissao
        {
            get
            {
                return this._dataDemissao;
            }
            set
            {
                this._dataDemissao = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoAtividade
        /// <summary>
        /// Tamanho do campo: 2000.
        /// Campo opcional.
        /// </summary>
        public string DescricaoAtividade
        {
            get
            {
                return this._descricaoAtividade;
            }
            set
            {
                this._descricaoAtividade = value;
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
                this._flagInativo = value;
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

        #region VlrSalario

        public decimal? VlrSalario
        {
            get { return this._vlrSalario; }
            set { this._vlrSalario = value; }
        }

        #endregion

        #region DescricaoNavegador
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo obrigatório.
        /// </summary>
        public string DescricaoNavegador
        {
            get
            {
                return this._descricaoNavegador;
            }
            set
            {
                this._descricaoNavegador = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public ExperienciaProfissional()
        {
        }
        public ExperienciaProfissional(int idExperienciaProfissional)
        {
            this._idExperienciaProfissional = idExperienciaProfissional;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Experiencia_Profissional (Idf_Area_BNE, Idf_Pessoa_Fisica, Raz_Social, Des_Funcao_Exercida, Idf_Funcao, Dta_Admissao, Dta_Demissao, Des_Atividade, Flg_Inativo, Dta_Cadastro, Flg_Importado,Vlr_Salario,Des_Navegador) VALUES (@Idf_Area_BNE, @Idf_Pessoa_Fisica, @Raz_Social, @Des_Funcao_Exercida, @Idf_Funcao, @Dta_Admissao, @Dta_Demissao, @Des_Atividade, @Flg_Inativo, @Dta_Cadastro, @Flg_Importado, @Vlr_Salario,@Des_Navegador);SET @Idf_Experiencia_Profissional = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Experiencia_Profissional SET Idf_Area_BNE = @Idf_Area_BNE, Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica, Raz_Social = @Raz_Social, Des_Funcao_Exercida = @Des_Funcao_Exercida, Idf_Funcao = @Idf_Funcao, Dta_Admissao = @Dta_Admissao, Dta_Demissao = @Dta_Demissao, Des_Atividade = @Des_Atividade, Flg_Inativo = @Flg_Inativo, Dta_Cadastro = @Dta_Cadastro, Flg_Importado = @Flg_Importado, Vlr_Salario = @Vlr_Salario, Des_Navegador =@Des_Navegador WHERE Idf_Experiencia_Profissional = @Idf_Experiencia_Profissional";
        private const string SPDELETE = "DELETE FROM BNE_Experiencia_Profissional WHERE Idf_Experiencia_Profissional = @Idf_Experiencia_Profissional";
        private const string SPSELECTID = "SELECT * FROM BNE_Experiencia_Profissional WITH(NOLOCK) WHERE Idf_Experiencia_Profissional = @Idf_Experiencia_Profissional";
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
            parms.Add(new SqlParameter("@Idf_Experiencia_Profissional", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Area_BNE", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Raz_Social", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Des_Funcao_Exercida", SqlDbType.VarChar, 50));
            parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Admissao", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Demissao", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Des_Atividade", SqlDbType.VarChar, 2000));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Flg_Importado", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Vlr_Salario", SqlDbType.Decimal, 9));
            parms.Add(new SqlParameter("@Des_Navegador", SqlDbType.VarChar, 200));
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
            parms[0].Value = this._idExperienciaProfissional;

            if (this._areaBNE != null)
                parms[1].Value = this._areaBNE.IdAreaBNE;
            else
                parms[1].Value = DBNull.Value;

            parms[2].Value = this._pessoaFisica.IdPessoaFisica;
            parms[3].Value = this._razaoSocial;

            if (!String.IsNullOrEmpty(this._descricaoFuncaoExercida))
                parms[4].Value = this._descricaoFuncaoExercida;
            else
                parms[4].Value = DBNull.Value;


            if (this._funcao != null)
                parms[5].Value = this._funcao.IdFuncao;
            else
                parms[5].Value = DBNull.Value;

            parms[6].Value = this._dataAdmissao;

            if (this._dataDemissao.HasValue)
                parms[7].Value = this._dataDemissao;
            else
                parms[7].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoAtividade))
                parms[8].Value = this._descricaoAtividade;
            else
                parms[8].Value = DBNull.Value;

            parms[9].Value = this._flagInativo;

            if (this._flagImportado.HasValue)
                parms[11].Value = this._flagImportado;
            else
                parms[11].Value = DBNull.Value;

            if (this._vlrSalario.HasValue)
                parms[12].Value = this._vlrSalario;
            else
                parms[12].Value = DBNull.Value;

            if (!string.IsNullOrEmpty(this._descricaoNavegador))
                parms[13].Value = this._descricaoNavegador;
            else
                parms[13].Value = DBNull.Value;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                this._dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[10].Value = this._dataCadastro;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de ExperienciaProfissional no banco de dados.
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
                        this._idExperienciaProfissional = Convert.ToInt32(cmd.Parameters["@Idf_Experiencia_Profissional"].Value);
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
        /// Método utilizado para inserir uma instância de ExperienciaProfissional no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idExperienciaProfissional = Convert.ToInt32(cmd.Parameters["@Idf_Experiencia_Profissional"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de ExperienciaProfissional no banco de dados.
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
        /// Método utilizado para atualizar uma instância de ExperienciaProfissional no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de ExperienciaProfissional no banco de dados.
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
        /// Método utilizado para salvar uma instância de ExperienciaProfissional no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de ExperienciaProfissional no banco de dados.
        /// </summary>
        /// <param name="idExperienciaProfissional">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idExperienciaProfissional)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Experiencia_Profissional", SqlDbType.Int, 4));

            parms[0].Value = idExperienciaProfissional;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de ExperienciaProfissional no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idExperienciaProfissional">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idExperienciaProfissional, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Experiencia_Profissional", SqlDbType.Int, 4));

            parms[0].Value = idExperienciaProfissional;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de ExperienciaProfissional no banco de dados.
        /// </summary>
        /// <param name="idExperienciaProfissional">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idExperienciaProfissional)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Experiencia_Profissional where Idf_Experiencia_Profissional in (";

            for (int i = 0; i < idExperienciaProfissional.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idExperienciaProfissional[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idExperienciaProfissional">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idExperienciaProfissional)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Experiencia_Profissional", SqlDbType.Int, 4));

            parms[0].Value = idExperienciaProfissional;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idExperienciaProfissional">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idExperienciaProfissional, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Experiencia_Profissional", SqlDbType.Int, 4));

            parms[0].Value = idExperienciaProfissional;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Exp.Idf_Experiencia_Profissional, Exp.Idf_Area_BNE, Exp.Idf_Pessoa_Fisica, Exp.Raz_Social, Exp.Des_Funcao_Exercida, Exp.Idf_Funcao, Exp.Dta_Admissao, Exp.Dta_Demissao, Exp.Des_Atividade, Exp.Flg_Inativo, Exp.Dta_Cadastro, Exp.Flg_Importado, Exp.Vlr_Salario FROM BNE_Experiencia_Profissional Exp";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de ExperienciaProfissional a partir do banco de dados.
        /// </summary>
        /// <param name="idExperienciaProfissional">Chave do registro.</param>
        /// <returns>Instância de ExperienciaProfissional.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static ExperienciaProfissional LoadObject(int idExperienciaProfissional)
        {
            using (IDataReader dr = LoadDataReader(idExperienciaProfissional))
            {
                ExperienciaProfissional objExperienciaProfissional = new ExperienciaProfissional();
                if (SetInstance(dr, objExperienciaProfissional))
                    return objExperienciaProfissional;
            }
            throw (new RecordNotFoundException(typeof(ExperienciaProfissional)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de ExperienciaProfissional a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idExperienciaProfissional">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de ExperienciaProfissional.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static ExperienciaProfissional LoadObject(int idExperienciaProfissional, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idExperienciaProfissional, trans))
            {
                ExperienciaProfissional objExperienciaProfissional = new ExperienciaProfissional();
                if (SetInstance(dr, objExperienciaProfissional))
                    return objExperienciaProfissional;
            }
            throw (new RecordNotFoundException(typeof(ExperienciaProfissional)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de ExperienciaProfissional a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idExperienciaProfissional))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de ExperienciaProfissional a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idExperienciaProfissional, trans))
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
        /// <param name="objExperienciaProfissional">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, ExperienciaProfissional objExperienciaProfissional)
        {
            try
            {
                if (dr.Read())
                {
                    objExperienciaProfissional._idExperienciaProfissional = Convert.ToInt32(dr["Idf_Experiencia_Profissional"]);
                    if (dr["Idf_Area_BNE"] != DBNull.Value)
                        objExperienciaProfissional._areaBNE = new AreaBNE(Convert.ToInt32(dr["Idf_Area_BNE"]));
                    objExperienciaProfissional._pessoaFisica = new PessoaFisica(Convert.ToInt32(dr["Idf_Pessoa_Fisica"]));
                    objExperienciaProfissional._razaoSocial = Convert.ToString(dr["Raz_Social"]);
                    if (dr["Des_Funcao_Exercida"] != DBNull.Value)
                        objExperienciaProfissional._descricaoFuncaoExercida = Convert.ToString(dr["Des_Funcao_Exercida"]);
                    if (dr["Idf_Funcao"] != DBNull.Value)
                        objExperienciaProfissional._funcao = new Funcao(Convert.ToInt32(dr["Idf_Funcao"]));
                    objExperienciaProfissional._dataAdmissao = Convert.ToDateTime(dr["Dta_Admissao"]);
                    if (dr["Dta_Demissao"] != DBNull.Value)
                        objExperienciaProfissional._dataDemissao = Convert.ToDateTime(dr["Dta_Demissao"]);
                    
                    objExperienciaProfissional._descricaoAtividade = (dr["Des_Atividade"] != DBNull.Value ? Convert.ToString(dr["Des_Atividade"]) : "");

                    objExperienciaProfissional._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    objExperienciaProfissional._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    if (dr["Flg_Importado"] != DBNull.Value)
                        objExperienciaProfissional._flagImportado = Convert.ToBoolean(dr["Flg_Importado"]);
                        objExperienciaProfissional._vlrSalario = (dr["Vlr_Salario"] != DBNull.Value ? Convert.ToDecimal(dr["Vlr_Salario"]):0);

                    objExperienciaProfissional._descricaoNavegador = (dr["Des_Navegador"] != DBNull.Value ? Convert.ToString(dr["Des_Navegador"]) : "");

                    objExperienciaProfissional._persisted = true;
                    objExperienciaProfissional._modified = false;

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