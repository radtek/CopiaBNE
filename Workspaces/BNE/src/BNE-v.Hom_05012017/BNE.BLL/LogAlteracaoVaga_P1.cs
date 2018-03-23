//-- Data: 01/08/2016 16:05
//-- Autor: Mailson

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class LogAlteracaoVaga // Tabela: BNE_Log_Alteracao_Vaga
    {
        #region Atributos
        private int _idLogAlteracaoVaga;
        private UsuarioFilialPerfil _usuarioFilialPerfil;
        private Vaga _vaga;
        private DateTime _dataAlteracao;
        private string _descricaoAlteracao;
        private string _nomeServico;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdLogAlteracaoVaga
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdLogAlteracaoVaga
        {
            get
            {
                return this._idLogAlteracaoVaga;
            }
        }
        #endregion

        #region UsuarioFilialPerfil
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public UsuarioFilialPerfil UsuarioFilialPerfil
        {
            get
            {
                return this._usuarioFilialPerfil;
            }
            set
            {
                this._usuarioFilialPerfil = value;
                this._modified = true;
            }
        }
        #endregion

        #region Vaga
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public Vaga Vaga
        {
            get
            {
                return this._vaga;
            }
            set
            {
                this._vaga = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataAlteracao
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public DateTime DataAlteracao
        {
            get
            {
                return this._dataAlteracao;
            }
        }
        #endregion

        #region DescricaoAlteracao
        /// <summary>
        /// Tamanho do campo: 4000.
        /// Campo obrigatório.
        /// </summary>
        public string DescricaoAlteracao
        {
            get
            {
                return this._descricaoAlteracao;
            }
            set
            {
                this._descricaoAlteracao = value;
                this._modified = true;
            }
        }
        #endregion

        #region NomeServico
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        public string NomeServico
        {
            get
            {
                return this._nomeServico;
            }
            set
            {
                this._nomeServico = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public LogAlteracaoVaga()
        {
        }
        public LogAlteracaoVaga(int idLogAlteracaoVaga)
        {
            this._idLogAlteracaoVaga = idLogAlteracaoVaga;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Log_Alteracao_Vaga (Idf_Usuario_Filial_Perfil, Idf_Vaga, Dta_Alteracao, Des_Alteracao, Nme_Servico) VALUES (@Idf_Usuario_Filial_Perfil, @Idf_Vaga, @Dta_Alteracao, @Des_Alteracao, @Nme_Servico);SET @Idf_Log_Alteracao_Vaga = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Log_Alteracao_Vaga SET Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Idf_Vaga = @Idf_Vaga, Dta_Alteracao = @Dta_Alteracao, Des_Alteracao = @Des_Alteracao, Nme_Servico = @Nme_Servico WHERE Idf_Log_Alteracao_Vaga = @Idf_Log_Alteracao_Vaga";
        private const string SPDELETE = "DELETE FROM BNE_Log_Alteracao_Vaga WHERE Idf_Log_Alteracao_Vaga = @Idf_Log_Alteracao_Vaga";
        private const string SPSELECTID = "SELECT * FROM BNE_Log_Alteracao_Vaga WITH(NOLOCK) WHERE Idf_Log_Alteracao_Vaga = @Idf_Log_Alteracao_Vaga";
        #endregion

        #region Métodos

        #region GetParameters
        /// <summary>
        /// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        /// </summary>
        /// <returns>Lista de parâmetros SQL.</returns>
        /// <remarks>Mailson</remarks>
        private List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Log_Alteracao_Vaga", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Alteracao", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Des_Alteracao", SqlDbType.VarChar, 4000));
            parms.Add(new SqlParameter("@Nme_Servico", SqlDbType.VarChar, 100));
            return (parms);
        }
        #endregion

        #region SetParameters
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>Mailson</remarks>
        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idLogAlteracaoVaga;

            if (this._usuarioFilialPerfil != null)
                parms[1].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
            else
                parms[1].Value = DBNull.Value;

            parms[2].Value = this._vaga.IdVaga;
            parms[4].Value = this._descricaoAlteracao;

            if (!String.IsNullOrEmpty(this._nomeServico))
                parms[5].Value = this._nomeServico;
            else
                parms[5].Value = DBNull.Value;


            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            this._dataAlteracao = DateTime.Now;
            parms[3].Value = this._dataAlteracao;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de LogAlteracaoVaga no banco de dados.
        /// </summary>
        /// <remarks>Mailson</remarks>
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
                        this._idLogAlteracaoVaga = Convert.ToInt32(cmd.Parameters["@Idf_Log_Alteracao_Vaga"].Value);
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
        /// Método utilizado para inserir uma instância de LogAlteracaoVaga no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Mailson</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idLogAlteracaoVaga = Convert.ToInt32(cmd.Parameters["@Idf_Log_Alteracao_Vaga"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de LogAlteracaoVaga no banco de dados.
        /// </summary>
        /// <remarks>Mailson</remarks>
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
        /// Método utilizado para atualizar uma instância de LogAlteracaoVaga no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Mailson</remarks>
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
        /// Método utilizado para salvar uma instância de LogAlteracaoVaga no banco de dados.
        /// </summary>
        /// <remarks>Mailson</remarks>
        public void Save()
        {
            if (!this._persisted)
                this.Insert();
            else
                this.Update();
        }
        /// <summary>
        /// Método utilizado para salvar uma instância de LogAlteracaoVaga no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Mailson</remarks>
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
        /// Método utilizado para excluir uma instância de LogAlteracaoVaga no banco de dados.
        /// </summary>
        /// <param name="idLogAlteracaoVaga">Chave do registro.</param>
        /// <remarks>Mailson</remarks>
        public static void Delete(int idLogAlteracaoVaga)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Log_Alteracao_Vaga", SqlDbType.Int, 4));

            parms[0].Value = idLogAlteracaoVaga;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de LogAlteracaoVaga no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idLogAlteracaoVaga">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Mailson</remarks>
        public static void Delete(int idLogAlteracaoVaga, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Log_Alteracao_Vaga", SqlDbType.Int, 4));

            parms[0].Value = idLogAlteracaoVaga;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de LogAlteracaoVaga no banco de dados.
        /// </summary>
        /// <param name="idLogAlteracaoVaga">Lista de chaves.</param>
        /// <remarks>Mailson</remarks>
        public static void Delete(List<int> idLogAlteracaoVaga)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Log_Alteracao_Vaga where Idf_Log_Alteracao_Vaga in (";

            for (int i = 0; i < idLogAlteracaoVaga.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idLogAlteracaoVaga[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idLogAlteracaoVaga">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Mailson</remarks>
        private static IDataReader LoadDataReader(int idLogAlteracaoVaga)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Log_Alteracao_Vaga", SqlDbType.Int, 4));

            parms[0].Value = idLogAlteracaoVaga;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idLogAlteracaoVaga">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Mailson</remarks>
        private static IDataReader LoadDataReader(int idLogAlteracaoVaga, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Log_Alteracao_Vaga", SqlDbType.Int, 4));

            parms[0].Value = idLogAlteracaoVaga;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Log.Idf_Log_Alteracao_Vaga, Log.Idf_Usuario_Filial_Perfil, Log.Idf_Vaga, Log.Dta_Alteracao, Log.Des_Alteracao, Log.Nme_Servico FROM BNE_Log_Alteracao_Vaga Log";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de LogAlteracaoVaga a partir do banco de dados.
        /// </summary>
        /// <param name="idLogAlteracaoVaga">Chave do registro.</param>
        /// <returns>Instância de LogAlteracaoVaga.</returns>
        /// <remarks>Mailson</remarks>
        public static LogAlteracaoVaga LoadObject(int idLogAlteracaoVaga)
        {
            using (IDataReader dr = LoadDataReader(idLogAlteracaoVaga))
            {
                LogAlteracaoVaga objLogAlteracaoVaga = new LogAlteracaoVaga();
                if (SetInstance(dr, objLogAlteracaoVaga))
                    return objLogAlteracaoVaga;
            }
            throw (new RecordNotFoundException(typeof(LogAlteracaoVaga)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de LogAlteracaoVaga a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idLogAlteracaoVaga">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de LogAlteracaoVaga.</returns>
        /// <remarks>Mailson</remarks>
        public static LogAlteracaoVaga LoadObject(int idLogAlteracaoVaga, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idLogAlteracaoVaga, trans))
            {
                LogAlteracaoVaga objLogAlteracaoVaga = new LogAlteracaoVaga();
                if (SetInstance(dr, objLogAlteracaoVaga))
                    return objLogAlteracaoVaga;
            }
            throw (new RecordNotFoundException(typeof(LogAlteracaoVaga)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de LogAlteracaoVaga a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Mailson</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idLogAlteracaoVaga))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de LogAlteracaoVaga a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Mailson</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idLogAlteracaoVaga, trans))
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
        /// <param name="objLogAlteracaoVaga">Instância a ser manipulada.</param>
        /// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Mailson</remarks>
        private static bool SetInstance(IDataReader dr, LogAlteracaoVaga objLogAlteracaoVaga, bool dispose = true)
        {
            try
            {
                if (dr.Read())
                {
                    objLogAlteracaoVaga._idLogAlteracaoVaga = Convert.ToInt32(dr["Idf_Log_Alteracao_Vaga"]);
                    if (dr["Idf_Usuario_Filial_Perfil"] != DBNull.Value)
                        objLogAlteracaoVaga._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
                    objLogAlteracaoVaga._vaga = new Vaga(Convert.ToInt32(dr["Idf_Vaga"]));
                    objLogAlteracaoVaga._dataAlteracao = Convert.ToDateTime(dr["Dta_Alteracao"]);
                    objLogAlteracaoVaga._descricaoAlteracao = Convert.ToString(dr["Des_Alteracao"]);
                    if (dr["Nme_Servico"] != DBNull.Value)
                        objLogAlteracaoVaga._nomeServico = Convert.ToString(dr["Nme_Servico"]);

                    objLogAlteracaoVaga._persisted = true;
                    objLogAlteracaoVaga._modified = false;

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