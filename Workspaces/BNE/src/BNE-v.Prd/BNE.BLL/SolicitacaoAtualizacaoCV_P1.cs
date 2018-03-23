//-- Data: 08/02/2017 11:00
//-- Autor: Mailson

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class SolicitacaoAtualizacaoCV // Tabela: BNE_Solicitacao_Atualizacao_CV
    {
        #region Atributos
        private int _idSolicitacaoAtualizacaoCV;
        private UsuarioFilialPerfil _usuarioFilialPerfil;
        private Curriculo _curriculo;
        private DateTime _dataCadastro;
        private bool _flagEnviado;
        private DateTime? _dataAtualizacaoCV;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdSolicitacaoAtualizacaoCV
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdSolicitacaoAtualizacaoCV
        {
            get
            {
                return this._idSolicitacaoAtualizacaoCV;
            }
        }
        #endregion

        #region UsuarioFilialPerfil
        /// <summary>
        /// Campo obrigatório.
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

        #region Curriculo
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public Curriculo Curriculo
        {
            get
            {
                return this._curriculo;
            }
            set
            {
                this._curriculo = value;
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

        #region FlagEnviado
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagEnviado
        {
            get
            {
                return this._flagEnviado;
            }
            set
            {
                this._flagEnviado = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataAtualizacaoCV
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataAtualizacaoCV
        {
            get
            {
                return this._dataAtualizacaoCV;
            }
            set
            {
                this._dataAtualizacaoCV = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public SolicitacaoAtualizacaoCV()
        {
        }
        public SolicitacaoAtualizacaoCV(int idSolicitacaoAtualizacaoCV)
        {
            this._idSolicitacaoAtualizacaoCV = idSolicitacaoAtualizacaoCV;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Solicitacao_Atualizacao_CV (Idf_Usuario_Filial_Perfil, Idf_Curriculo, Dta_Cadastro, Flg_Enviado, Dta_Atualizacao_CV) VALUES (@Idf_Usuario_Filial_Perfil, @Idf_Curriculo, @Dta_Cadastro, @Flg_Enviado, @Dta_Atualizacao_CV);SET @Idf_Solicitacao_Atualizacao_CV = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Solicitacao_Atualizacao_CV SET Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Idf_Curriculo = @Idf_Curriculo, Dta_Cadastro = @Dta_Cadastro, Flg_Enviado = @Flg_Enviado, Dta_Atualizacao_CV = @Dta_Atualizacao_CV WHERE Idf_Solicitacao_Atualizacao_CV = @Idf_Solicitacao_Atualizacao_CV";
        private const string SPDELETE = "DELETE FROM BNE_Solicitacao_Atualizacao_CV WHERE Idf_Solicitacao_Atualizacao_CV = @Idf_Solicitacao_Atualizacao_CV";
        private const string SPSELECTID = "SELECT * FROM BNE_Solicitacao_Atualizacao_CV WITH(NOLOCK) WHERE Idf_Solicitacao_Atualizacao_CV = @Idf_Solicitacao_Atualizacao_CV";
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
            parms.Add(new SqlParameter("@Idf_Solicitacao_Atualizacao_CV", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Flg_Enviado", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Dta_Atualizacao_CV", SqlDbType.DateTime, 8));
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
            parms[0].Value = this._idSolicitacaoAtualizacaoCV;
            parms[1].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
            parms[2].Value = this._curriculo.IdCurriculo;
            parms[4].Value = this._flagEnviado;

            if (this._dataAtualizacaoCV.HasValue)
                parms[5].Value = this._dataAtualizacaoCV;
            else
                parms[5].Value = DBNull.Value;


            if (!this._persisted)
            {
                parms[0].Direction = ParameterDirection.Output;
                this._dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[3].Value = this._dataCadastro;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de SolicitacaoAtualizacaoCV no banco de dados.
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
                        this._idSolicitacaoAtualizacaoCV = Convert.ToInt32(cmd.Parameters["@Idf_Solicitacao_Atualizacao_CV"].Value);
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
        /// Método utilizado para inserir uma instância de SolicitacaoAtualizacaoCV no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Mailson</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idSolicitacaoAtualizacaoCV = Convert.ToInt32(cmd.Parameters["@Idf_Solicitacao_Atualizacao_CV"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de SolicitacaoAtualizacaoCV no banco de dados.
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
        /// Método utilizado para atualizar uma instância de SolicitacaoAtualizacaoCV no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de SolicitacaoAtualizacaoCV no banco de dados.
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
        /// Método utilizado para salvar uma instância de SolicitacaoAtualizacaoCV no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de SolicitacaoAtualizacaoCV no banco de dados.
        /// </summary>
        /// <param name="idSolicitacaoAtualizacaoCV">Chave do registro.</param>
        /// <remarks>Mailson</remarks>
        public static void Delete(int idSolicitacaoAtualizacaoCV)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Solicitacao_Atualizacao_CV", SqlDbType.Int, 4));

            parms[0].Value = idSolicitacaoAtualizacaoCV;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de SolicitacaoAtualizacaoCV no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idSolicitacaoAtualizacaoCV">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Mailson</remarks>
        public static void Delete(int idSolicitacaoAtualizacaoCV, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Solicitacao_Atualizacao_CV", SqlDbType.Int, 4));

            parms[0].Value = idSolicitacaoAtualizacaoCV;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de SolicitacaoAtualizacaoCV no banco de dados.
        /// </summary>
        /// <param name="idSolicitacaoAtualizacaoCV">Lista de chaves.</param>
        /// <remarks>Mailson</remarks>
        public static void Delete(List<int> idSolicitacaoAtualizacaoCV)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Solicitacao_Atualizacao_CV where Idf_Solicitacao_Atualizacao_CV in (";

            for (int i = 0; i < idSolicitacaoAtualizacaoCV.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idSolicitacaoAtualizacaoCV[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idSolicitacaoAtualizacaoCV">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Mailson</remarks>
        private static IDataReader LoadDataReader(int idSolicitacaoAtualizacaoCV)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Solicitacao_Atualizacao_CV", SqlDbType.Int, 4));

            parms[0].Value = idSolicitacaoAtualizacaoCV;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idSolicitacaoAtualizacaoCV">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Mailson</remarks>
        private static IDataReader LoadDataReader(int idSolicitacaoAtualizacaoCV, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Solicitacao_Atualizacao_CV", SqlDbType.Int, 4));

            parms[0].Value = idSolicitacaoAtualizacaoCV;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Sol.Idf_Solicitacao_Atualizacao_CV, Sol.Idf_Usuario_Filial_Perfil, Sol.Idf_Curriculo, Sol.Dta_Cadastro, Sol.Flg_Enviado, Sol.Dta_Atualizacao_CV FROM BNE_Solicitacao_Atualizacao_CV Sol";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de SolicitacaoAtualizacaoCV a partir do banco de dados.
        /// </summary>
        /// <param name="idSolicitacaoAtualizacaoCV">Chave do registro.</param>
        /// <returns>Instância de SolicitacaoAtualizacaoCV.</returns>
        /// <remarks>Mailson</remarks>
        public static SolicitacaoAtualizacaoCV LoadObject(int idSolicitacaoAtualizacaoCV)
        {
            using (IDataReader dr = LoadDataReader(idSolicitacaoAtualizacaoCV))
            {
                SolicitacaoAtualizacaoCV objSolicitacaoAtualizacaoCV = new SolicitacaoAtualizacaoCV();
                if (SetInstance(dr, objSolicitacaoAtualizacaoCV))
                    return objSolicitacaoAtualizacaoCV;
            }
            throw (new RecordNotFoundException(typeof(SolicitacaoAtualizacaoCV)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de SolicitacaoAtualizacaoCV a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idSolicitacaoAtualizacaoCV">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de SolicitacaoAtualizacaoCV.</returns>
        /// <remarks>Mailson</remarks>
        public static SolicitacaoAtualizacaoCV LoadObject(int idSolicitacaoAtualizacaoCV, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idSolicitacaoAtualizacaoCV, trans))
            {
                SolicitacaoAtualizacaoCV objSolicitacaoAtualizacaoCV = new SolicitacaoAtualizacaoCV();
                if (SetInstance(dr, objSolicitacaoAtualizacaoCV))
                    return objSolicitacaoAtualizacaoCV;
            }
            throw (new RecordNotFoundException(typeof(SolicitacaoAtualizacaoCV)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de SolicitacaoAtualizacaoCV a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Mailson</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idSolicitacaoAtualizacaoCV))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de SolicitacaoAtualizacaoCV a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Mailson</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idSolicitacaoAtualizacaoCV, trans))
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
        /// <param name="objSolicitacaoAtualizacaoCV">Instância a ser manipulada.</param>
        /// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Mailson</remarks>
        private static bool SetInstance(IDataReader dr, SolicitacaoAtualizacaoCV objSolicitacaoAtualizacaoCV, bool dispose = true)
        {
            try
            {
                if (dr.Read())
                {
                    objSolicitacaoAtualizacaoCV._idSolicitacaoAtualizacaoCV = Convert.ToInt32(dr["Idf_Solicitacao_Atualizacao_CV"]);
                    objSolicitacaoAtualizacaoCV._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
                    objSolicitacaoAtualizacaoCV._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
                    objSolicitacaoAtualizacaoCV._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objSolicitacaoAtualizacaoCV._flagEnviado = Convert.ToBoolean(dr["Flg_Enviado"]);
                    if (dr["Dta_Atualizacao_CV"] != DBNull.Value)
                        objSolicitacaoAtualizacaoCV._dataAtualizacaoCV = Convert.ToDateTime(dr["Dta_Atualizacao_CV"]);

                    objSolicitacaoAtualizacaoCV._persisted = true;
                    objSolicitacaoAtualizacaoCV._modified = false;

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