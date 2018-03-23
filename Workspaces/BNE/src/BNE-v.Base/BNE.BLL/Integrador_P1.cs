//-- Data: 17/05/2013 18:06
//-- Autor: Francisco Ribas

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class Integrador // Tabela: TAB_Integrador
    {
        #region Atributos
        private int _idIntegrador;
        private Filial _filial;
        private bool _flagInativo;
        private TipoIntegrador _tipoIntegrador;
        private UsuarioFilialPerfil _usuarioFilialPerfil;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdIntegrador
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdIntegrador
        {
            get
            {
                return this._idIntegrador;
            }
        }
        #endregion

        #region Filial
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public Filial Filial
        {
            get
            {
                return this._filial;
            }
            set
            {
                this._filial = value;
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

        #endregion

        #region Construtores
        public Integrador()
        {
        }
        public Integrador(int idIntegrador)
        {
            this._idIntegrador = idIntegrador;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO TAB_Integrador (Idf_Filial, Flg_Inativo, Idf_Tipo_Integrador, Idf_Usuario_Filial_Perfil) VALUES (@Idf_Filial, @Flg_Inativo, @Idf_Tipo_Integrador, @Idf_Usuario_Filial_Perfil);SET @Idf_Integrador = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE TAB_Integrador SET Idf_Filial = @Idf_Filial, Flg_Inativo = @Flg_Inativo, Idf_Tipo_Integrador = @Idf_Tipo_Integrador, Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil WHERE Idf_Integrador = @Idf_Integrador";
        private const string SPDELETE = "DELETE FROM TAB_Integrador WHERE Idf_Integrador = @Idf_Integrador";
        private const string SPSELECTID = "SELECT * FROM TAB_Integrador WITH(NOLOCK) WHERE Idf_Integrador = @Idf_Integrador";
        #endregion

        #region Métodos

        #region GetParameters
        /// <summary>
        /// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        /// </summary>
        /// <returns>Lista de parâmetros SQL.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Integrador", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Idf_Tipo_Integrador", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
            return (parms);
        }
        #endregion

        #region SetParameters
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>Francisco Ribas</remarks>
        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idIntegrador;
            parms[1].Value = this._filial.IdFilial;
            parms[3].Value = this._flagInativo;

            if (this._tipoIntegrador != null)
                parms[4].Value = this._tipoIntegrador.IdTipoIntegrador;
            else
                parms[4].Value = DBNull.Value;

            if (this._usuarioFilialPerfil != null)
                parms[6].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
            else
                parms[6].Value = DBNull.Value;

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
        /// Método utilizado para inserir uma instância de Integrador no banco de dados.
        /// </summary>
        /// <remarks>Francisco Ribas</remarks>
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
                        this._idIntegrador = Convert.ToInt32(cmd.Parameters["@Idf_Integrador"].Value);
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
        /// Método utilizado para inserir uma instância de Integrador no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Francisco Ribas</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idIntegrador = Convert.ToInt32(cmd.Parameters["@Idf_Integrador"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de Integrador no banco de dados.
        /// </summary>
        /// <remarks>Francisco Ribas</remarks>
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
        /// Método utilizado para atualizar uma instância de Integrador no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Francisco Ribas</remarks>
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
        /// Método utilizado para salvar uma instância de Integrador no banco de dados.
        /// </summary>
        /// <remarks>Francisco Ribas</remarks>
        public void Save()
        {
            if (!this._persisted)
                this.Insert();
            else
                this.Update();
        }
        /// <summary>
        /// Método utilizado para salvar uma instância de Integrador no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Francisco Ribas</remarks>
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
        /// Método utilizado para excluir uma instância de Integrador no banco de dados.
        /// </summary>
        /// <param name="idIntegrador">Chave do registro.</param>
        /// <remarks>Francisco Ribas</remarks>
        public static void Delete(int idIntegrador)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Integrador", SqlDbType.Int, 4));

            parms[0].Value = idIntegrador;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de Integrador no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idIntegrador">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Francisco Ribas</remarks>
        public static void Delete(int idIntegrador, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Integrador", SqlDbType.Int, 4));

            parms[0].Value = idIntegrador;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de Integrador no banco de dados.
        /// </summary>
        /// <param name="idIntegrador">Lista de chaves.</param>
        /// <remarks>Francisco Ribas</remarks>
        public static void Delete(List<int> idIntegrador)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from TAB_Integrador where Idf_Integrador in (";

            for (int i = 0; i < idIntegrador.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idIntegrador[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idIntegrador">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private static IDataReader LoadDataReader(int idIntegrador)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Integrador", SqlDbType.Int, 4));

            parms[0].Value = idIntegrador;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idIntegrador">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private static IDataReader LoadDataReader(int idIntegrador, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Integrador", SqlDbType.Int, 4));

            parms[0].Value = idIntegrador;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Int.Idf_Integrador, Int.Idf_Filial, Int.Url_Integracao, Int.Flg_Inativo, Int.Idf_Tipo_Integrador, Int.Flg_Excluir_Vagas_Nao_Importadas, Int.Idf_Usuario_Filial_Perfil, Int.Flg_Receber_Cada_CV, Int.Flg_Receber_Todos_CV FROM TAB_Integrador Int";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de Integrador a partir do banco de dados.
        /// </summary>
        /// <param name="idIntegrador">Chave do registro.</param>
        /// <returns>Instância de Integrador.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public static Integrador LoadObject(int idIntegrador)
        {
            using (IDataReader dr = LoadDataReader(idIntegrador))
            {
                Integrador objIntegrador = new Integrador();
                if (SetInstance(dr, objIntegrador))
                    return objIntegrador;
            }
            throw (new RecordNotFoundException(typeof(Integrador)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de Integrador a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idIntegrador">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de Integrador.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public static Integrador LoadObject(int idIntegrador, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idIntegrador, trans))
            {
                Integrador objIntegrador = new Integrador();
                if (SetInstance(dr, objIntegrador))
                    return objIntegrador;
            }
            throw (new RecordNotFoundException(typeof(Integrador)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de Integrador a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idIntegrador))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de Integrador a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idIntegrador, trans))
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
        /// <param name="objIntegrador">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private static bool SetInstance(IDataReader dr, Integrador objIntegrador)
        {
            try
            {
                if (dr.Read())
                {
                    objIntegrador._idIntegrador = Convert.ToInt32(dr["Idf_Integrador"]);
                    objIntegrador._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
                    objIntegrador._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    if (dr["Idf_Tipo_Integrador"] != DBNull.Value)
                        objIntegrador._tipoIntegrador = new TipoIntegrador(Convert.ToInt32(dr["Idf_Tipo_Integrador"]));
                    if (dr["Idf_Usuario_Filial_Perfil"] != DBNull.Value)
                        objIntegrador._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));

                    objIntegrador._persisted = true;
                    objIntegrador._modified = false;

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