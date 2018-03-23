//-- Data: 28/08/2017 11:39
//-- Autor: mailson

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class PesquisaCurriculoDeficiencia // Tabela: TAB_Pesquisa_Curriculo_Deficiencia
    {
        #region Atributos
        private int _idPesquisaCurriculoDeficiencia;
        private PesquisaCurriculo _pesquisaCurriculo;
        private Deficiencia _deficiencia;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdPesquisaCurriculoDeficiencia
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdPesquisaCurriculoDeficiencia
        {
            get
            {
                return this._idPesquisaCurriculoDeficiencia;
            }
        }
        #endregion

        #region PesquisaCurriculo
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public PesquisaCurriculo PesquisaCurriculo
        {
            get
            {
                return this._pesquisaCurriculo;
            }
            set
            {
                this._pesquisaCurriculo = value;
                this._modified = true;
            }
        }
        #endregion

        #region Deficiencia
        /// <summary>
        /// Campo obrigatório.
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

        #endregion

        #region Construtores
        public PesquisaCurriculoDeficiencia()
        {
        }
        public PesquisaCurriculoDeficiencia(int idPesquisaCurriculoDeficiencia)
        {
            this._idPesquisaCurriculoDeficiencia = idPesquisaCurriculoDeficiencia;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO TAB_Pesquisa_Curriculo_Deficiencia (Idf_Pesquisa_Curriculo, Idf_Deficiencia) VALUES (@Idf_Pesquisa_Curriculo, @Idf_Deficiencia);SET @Idf_Pesquisa_Curriculo_Deficiencia = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE TAB_Pesquisa_Curriculo_Deficiencia SET Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo, Idf_Deficiencia = @Idf_Deficiencia WHERE Idf_Pesquisa_Curriculo_Deficiencia = @Idf_Pesquisa_Curriculo_Deficiencia";
        private const string SPDELETE = "DELETE FROM TAB_Pesquisa_Curriculo_Deficiencia WHERE Idf_Pesquisa_Curriculo_Deficiencia = @Idf_Pesquisa_Curriculo_Deficiencia";
        private const string SPSELECTID = "SELECT * FROM TAB_Pesquisa_Curriculo_Deficiencia WITH(NOLOCK) WHERE Idf_Pesquisa_Curriculo_Deficiencia = @Idf_Pesquisa_Curriculo_Deficiencia";
        #endregion

        #region Métodos

        #region GetParameters
        /// <summary>
        /// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        /// </summary>
        /// <returns>Lista de parâmetros SQL.</returns>
        /// <remarks>mailson</remarks>
        private List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo_Deficiencia", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Deficiencia", SqlDbType.Int, 4));
            return (parms);
        }
        #endregion

        #region SetParameters
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>mailson</remarks>
        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idPesquisaCurriculoDeficiencia;
            parms[1].Value = this._pesquisaCurriculo.IdPesquisaCurriculo;
            parms[2].Value = this._deficiencia.IdDeficiencia;

            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de PesquisaCurriculoDeficiencia no banco de dados.
        /// </summary>
        /// <remarks>mailson</remarks>
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
                        this._idPesquisaCurriculoDeficiencia = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Curriculo_Deficiencia"].Value);
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
        /// Método utilizado para inserir uma instância de PesquisaCurriculoDeficiencia no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>mailson</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idPesquisaCurriculoDeficiencia = Convert.ToInt32(cmd.Parameters["@Idf_Pesquisa_Curriculo_Deficiencia"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de PesquisaCurriculoDeficiencia no banco de dados.
        /// </summary>
        /// <remarks>mailson</remarks>
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
        /// Método utilizado para atualizar uma instância de PesquisaCurriculoDeficiencia no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>mailson</remarks>
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
        /// Método utilizado para salvar uma instância de PesquisaCurriculoDeficiencia no banco de dados.
        /// </summary>
        /// <remarks>mailson</remarks>
        public void Save()
        {
            if (!this._persisted)
                this.Insert();
            else
                this.Update();
        }
        /// <summary>
        /// Método utilizado para salvar uma instância de PesquisaCurriculoDeficiencia no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>mailson</remarks>
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
        /// Método utilizado para excluir uma instância de PesquisaCurriculoDeficiencia no banco de dados.
        /// </summary>
        /// <param name="idPesquisaCurriculoDeficiencia">Chave do registro.</param>
        /// <remarks>mailson</remarks>
        public static void Delete(int idPesquisaCurriculoDeficiencia)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo_Deficiencia", SqlDbType.Int, 4));

            parms[0].Value = idPesquisaCurriculoDeficiencia;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de PesquisaCurriculoDeficiencia no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPesquisaCurriculoDeficiencia">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>mailson</remarks>
        public static void Delete(int idPesquisaCurriculoDeficiencia, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo_Deficiencia", SqlDbType.Int, 4));

            parms[0].Value = idPesquisaCurriculoDeficiencia;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de PesquisaCurriculoDeficiencia no banco de dados.
        /// </summary>
        /// <param name="idPesquisaCurriculoDeficiencia">Lista de chaves.</param>
        /// <remarks>mailson</remarks>
        public static void Delete(List<int> idPesquisaCurriculoDeficiencia)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from TAB_Pesquisa_Curriculo_Deficiencia where Idf_Pesquisa_Curriculo_Deficiencia in (";

            for (int i = 0; i < idPesquisaCurriculoDeficiencia.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idPesquisaCurriculoDeficiencia[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idPesquisaCurriculoDeficiencia">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>mailson</remarks>
        private static IDataReader LoadDataReader(int idPesquisaCurriculoDeficiencia)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo_Deficiencia", SqlDbType.Int, 4));

            parms[0].Value = idPesquisaCurriculoDeficiencia;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPesquisaCurriculoDeficiencia">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>mailson</remarks>
        private static IDataReader LoadDataReader(int idPesquisaCurriculoDeficiencia, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo_Deficiencia", SqlDbType.Int, 4));

            parms[0].Value = idPesquisaCurriculoDeficiencia;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Pes.Idf_Pesquisa_Curriculo_Deficiencia, Pes.Idf_Pesquisa_Curriculo, Pes.Idf_Deficiencia FROM TAB_Pesquisa_Curriculo_Deficiencia Pes";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de PesquisaCurriculoDeficiencia a partir do banco de dados.
        /// </summary>
        /// <param name="idPesquisaCurriculoDeficiencia">Chave do registro.</param>
        /// <returns>Instância de PesquisaCurriculoDeficiencia.</returns>
        /// <remarks>mailson</remarks>
        public static PesquisaCurriculoDeficiencia LoadObject(int idPesquisaCurriculoDeficiencia)
        {
            using (IDataReader dr = LoadDataReader(idPesquisaCurriculoDeficiencia))
            {
                PesquisaCurriculoDeficiencia objPesquisaCurriculoDeficiencia = new PesquisaCurriculoDeficiencia();
                if (SetInstance(dr, objPesquisaCurriculoDeficiencia))
                    return objPesquisaCurriculoDeficiencia;
            }
            throw (new RecordNotFoundException(typeof(PesquisaCurriculoDeficiencia)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de PesquisaCurriculoDeficiencia a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idPesquisaCurriculoDeficiencia">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de PesquisaCurriculoDeficiencia.</returns>
        /// <remarks>mailson</remarks>
        public static PesquisaCurriculoDeficiencia LoadObject(int idPesquisaCurriculoDeficiencia, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idPesquisaCurriculoDeficiencia, trans))
            {
                PesquisaCurriculoDeficiencia objPesquisaCurriculoDeficiencia = new PesquisaCurriculoDeficiencia();
                if (SetInstance(dr, objPesquisaCurriculoDeficiencia))
                    return objPesquisaCurriculoDeficiencia;
            }
            throw (new RecordNotFoundException(typeof(PesquisaCurriculoDeficiencia)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de PesquisaCurriculoDeficiencia a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>mailson</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idPesquisaCurriculoDeficiencia))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de PesquisaCurriculoDeficiencia a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>mailson</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idPesquisaCurriculoDeficiencia, trans))
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
        /// <param name="objPesquisaCurriculoDeficiencia">Instância a ser manipulada.</param>
        /// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>mailson</remarks>
        private static bool SetInstance(IDataReader dr, PesquisaCurriculoDeficiencia objPesquisaCurriculoDeficiencia, bool dispose = true)
        {
            try
            {
                if (dr.Read())
                {
                    objPesquisaCurriculoDeficiencia._idPesquisaCurriculoDeficiencia = Convert.ToInt32(dr["Idf_Pesquisa_Curriculo_Deficiencia"]);
                    objPesquisaCurriculoDeficiencia._pesquisaCurriculo = new PesquisaCurriculo(Convert.ToInt32(dr["Idf_Pesquisa_Curriculo"]));
                    objPesquisaCurriculoDeficiencia._deficiencia = new Deficiencia(Convert.ToInt32(dr["Idf_Deficiencia"]));

                    objPesquisaCurriculoDeficiencia._persisted = true;
                    objPesquisaCurriculoDeficiencia._modified = false;

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