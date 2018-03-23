//-- Data: 20/02/2014 15:23
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class IntegracaoAdmissao // Tabela: plataforma.BNE_Integracao_Admissao
    {
        #region Atributos
        private int _idIntegracaoAdmissao;
        private decimal _numeroCPF;
        private DateTime _dataCadastro;
        private DateTime? _dataIntegracao;
        private IntegracaoSituacao _integracaoSituacao;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdIntegracaoAdmissao
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdIntegracaoAdmissao
        {
            get
            {
                return this._idIntegracaoAdmissao;
            }
        }
        #endregion

        #region NumeroCPF
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public decimal NumeroCPF
        {
            get
            {
                return this._numeroCPF;
            }
            set
            {
                this._numeroCPF = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataCadastro
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public DateTime DataCadastro
        {
            get
            {
                return this._dataCadastro;
            }
        }
        #endregion

        #region DataIntegracao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataIntegracao
        {
            get
            {
                return this._dataIntegracao;
            }
            set
            {
                this._dataIntegracao = value;
                this._modified = true;
            }
        }
        #endregion

        #region IntegracaoSituacao
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public IntegracaoSituacao IntegracaoSituacao
        {
            get
            {
                return this._integracaoSituacao;
            }
            set
            {
                this._integracaoSituacao = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public IntegracaoAdmissao()
        {
        }
        public IntegracaoAdmissao(int idIntegracaoAdmissao)
        {
            this._idIntegracaoAdmissao = idIntegracaoAdmissao;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO plataforma.BNE_Integracao_Admissao (Num_CPF, Dta_Cadastro, Dta_Integracao, Idf_Integracao_Situacao) VALUES (@Num_CPF, @Dta_Cadastro, @Dta_Integracao, @Idf_Integracao_Situacao);SET @Idf_Integracao_Admissao = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE plataforma.BNE_Integracao_Admissao SET Num_CPF = @Num_CPF, Dta_Cadastro = @Dta_Cadastro, Dta_Integracao = @Dta_Integracao, Idf_Integracao_Situacao = @Idf_Integracao_Situacao WHERE Idf_Integracao_Admissao = @Idf_Integracao_Admissao";
        private const string SPDELETE = "DELETE FROM plataforma.BNE_Integracao_Admissao WHERE Idf_Integracao_Admissao = @Idf_Integracao_Admissao";
        private const string SPSELECTID = "SELECT * FROM plataforma.BNE_Integracao_Admissao WITH(NOLOCK) WHERE Idf_Integracao_Admissao = @Idf_Integracao_Admissao";
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
            parms.Add(new SqlParameter("@Idf_Integracao_Admissao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Num_CPF", SqlDbType.Decimal, 9));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Integracao", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Idf_Integracao_Situacao", SqlDbType.Int, 4));
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
            parms[0].Value = this._idIntegracaoAdmissao;
            parms[1].Value = this._numeroCPF;

            if (this._dataIntegracao.HasValue)
                parms[3].Value = this._dataIntegracao;
            else
                parms[3].Value = DBNull.Value;

            parms[4].Value = this._integracaoSituacao.IdIntegracaoSituacao;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                this._dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[2].Value = this._dataCadastro;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de IntegracaoAdmissao no banco de dados.
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
                        this._idIntegracaoAdmissao = Convert.ToInt32(cmd.Parameters["@Idf_Integracao_Admissao"].Value);
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
        /// Método utilizado para inserir uma instância de IntegracaoAdmissao no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idIntegracaoAdmissao = Convert.ToInt32(cmd.Parameters["@Idf_Integracao_Admissao"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de IntegracaoAdmissao no banco de dados.
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
        /// Método utilizado para atualizar uma instância de IntegracaoAdmissao no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de IntegracaoAdmissao no banco de dados.
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
        /// Método utilizado para salvar uma instância de IntegracaoAdmissao no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de IntegracaoAdmissao no banco de dados.
        /// </summary>
        /// <param name="idIntegracaoAdmissao">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idIntegracaoAdmissao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Integracao_Admissao", SqlDbType.Int, 4));

            parms[0].Value = idIntegracaoAdmissao;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de IntegracaoAdmissao no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idIntegracaoAdmissao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idIntegracaoAdmissao, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Integracao_Admissao", SqlDbType.Int, 4));

            parms[0].Value = idIntegracaoAdmissao;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de IntegracaoAdmissao no banco de dados.
        /// </summary>
        /// <param name="idIntegracaoAdmissao">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idIntegracaoAdmissao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from plataforma.BNE_Integracao_Admissao where Idf_Integracao_Admissao in (";

            for (int i = 0; i < idIntegracaoAdmissao.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idIntegracaoAdmissao[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idIntegracaoAdmissao">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idIntegracaoAdmissao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Integracao_Admissao", SqlDbType.Int, 4));

            parms[0].Value = idIntegracaoAdmissao;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idIntegracaoAdmissao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idIntegracaoAdmissao, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Integracao_Admissao", SqlDbType.Int, 4));

            parms[0].Value = idIntegracaoAdmissao;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Int.Idf_Integracao_Admissao, Int.Num_CPF, Int.Dta_Cadastro, Int.Dta_Integracao, Int.Idf_Integracao_Situacao FROM plataforma.BNE_Integracao_Admissao Int";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de IntegracaoAdmissao a partir do banco de dados.
        /// </summary>
        /// <param name="idIntegracaoAdmissao">Chave do registro.</param>
        /// <returns>Instância de IntegracaoAdmissao.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static IntegracaoAdmissao LoadObject(int idIntegracaoAdmissao)
        {
            using (IDataReader dr = LoadDataReader(idIntegracaoAdmissao))
            {
                IntegracaoAdmissao objIntegracaoAdmissao = new IntegracaoAdmissao();
                if (SetInstance(dr, objIntegracaoAdmissao))
                    return objIntegracaoAdmissao;
            }
            throw (new RecordNotFoundException(typeof(IntegracaoAdmissao)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de IntegracaoAdmissao a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idIntegracaoAdmissao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de IntegracaoAdmissao.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static IntegracaoAdmissao LoadObject(int idIntegracaoAdmissao, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idIntegracaoAdmissao, trans))
            {
                IntegracaoAdmissao objIntegracaoAdmissao = new IntegracaoAdmissao();
                if (SetInstance(dr, objIntegracaoAdmissao))
                    return objIntegracaoAdmissao;
            }
            throw (new RecordNotFoundException(typeof(IntegracaoAdmissao)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de IntegracaoAdmissao a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idIntegracaoAdmissao))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de IntegracaoAdmissao a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idIntegracaoAdmissao, trans))
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
        /// <param name="objIntegracaoAdmissao">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, IntegracaoAdmissao objIntegracaoAdmissao)
        {
            try
            {
                if (dr.Read())
                {
                    objIntegracaoAdmissao._idIntegracaoAdmissao = Convert.ToInt32(dr["Idf_Integracao_Admissao"]);
                    objIntegracaoAdmissao._numeroCPF = Convert.ToDecimal(dr["Num_CPF"]);
                    objIntegracaoAdmissao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    if (dr["Dta_Integracao"] != DBNull.Value)
                        objIntegracaoAdmissao._dataIntegracao = Convert.ToDateTime(dr["Dta_Integracao"]);
                    objIntegracaoAdmissao._integracaoSituacao = new IntegracaoSituacao(Convert.ToInt32(dr["Idf_Integracao_Situacao"]));

                    objIntegracaoAdmissao._persisted = true;
                    objIntegracaoAdmissao._modified = false;

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