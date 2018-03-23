//-- Data: 23/10/2013 16:56
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class FilialObservacao // Tabela: TAB_Filial_Observacao
    {
        #region Atributos
        private int _idFilialObservacao;
        private Filial _filial;
        private UsuarioFilialPerfil _usuarioFilialPerfil;
        private DateTime _dataCadastro;
        private bool _flagInativo;
        private string _descricaoObservacao;
        private bool _flagSistema;
        private DateTime? _dataProximaAcao;
        private string _descricaoProximaAcao;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdFilialObservacao
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdFilialObservacao
        {
            get
            {
                return this._idFilialObservacao;
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

        #region DescricaoObservacao
        /// <summary>
        /// Tamanho do campo: -1.
        /// Campo obrigatório.
        /// </summary>
        public string DescricaoObservacao
        {
            get
            {
                return this._descricaoObservacao;
            }
            set
            {
                this._descricaoObservacao = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagSistema
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagSistema
        {
            get
            {
                return this._flagSistema;
            }
            set
            {
                this._flagSistema = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataProximaAcao
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataProximaAcao
        {
            get
            {
                return this._dataProximaAcao;
            }
            set
            {
                this._dataProximaAcao = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoProximaAcao
        /// <summary>
        /// Tamanho do campo: 2000.
        /// Campo opcional.
        /// </summary>
        public string DescricaoProximaAcao
        {
            get
            {
                return this._descricaoProximaAcao;
            }
            set
            {
                this._descricaoProximaAcao = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public FilialObservacao()
        {
        }
        public FilialObservacao(int idFilialObservacao)
        {
            this._idFilialObservacao = idFilialObservacao;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO TAB_Filial_Observacao (Idf_Filial, Idf_Usuario_Filial_Perfil, Dta_Cadastro, Flg_Inativo, Des_Observacao, Flg_Sistema, Dta_Proxima_Acao, Des_Proxima_Acao) VALUES (@Idf_Filial, @Idf_Usuario_Filial_Perfil, @Dta_Cadastro, @Flg_Inativo, @Des_Observacao, @Flg_Sistema, @Dta_Proxima_Acao, @Des_Proxima_Acao);SET @Idf_Filial_Observacao = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE TAB_Filial_Observacao SET Idf_Filial = @Idf_Filial, Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo, Des_Observacao = @Des_Observacao, Flg_Sistema = @Flg_Sistema, Dta_Proxima_Acao = @Dta_Proxima_Acao, Des_Proxima_Acao = @Des_Proxima_Acao WHERE Idf_Filial_Observacao = @Idf_Filial_Observacao";
        private const string SPDELETE = "DELETE FROM TAB_Filial_Observacao WHERE Idf_Filial_Observacao = @Idf_Filial_Observacao";
        private const string SPSELECTID = "SELECT * FROM TAB_Filial_Observacao WITH(NOLOCK) WHERE Idf_Filial_Observacao = @Idf_Filial_Observacao";
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
            parms.Add(new SqlParameter("@Idf_Filial_Observacao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Des_Observacao", SqlDbType.VarChar));
            parms.Add(new SqlParameter("@Flg_Sistema", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Dta_Proxima_Acao", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Des_Proxima_Acao", SqlDbType.VarChar, 2000));
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
            parms[0].Value = this._idFilialObservacao;
            parms[1].Value = this._filial.IdFilial;

            if (this._usuarioFilialPerfil != null)
                parms[2].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
            else
                parms[2].Value = DBNull.Value;

            parms[4].Value = this._flagInativo;
            parms[5].Value = this._descricaoObservacao;
            parms[6].Value = this._flagSistema;

            if (this._dataProximaAcao.HasValue)
                parms[7].Value = this._dataProximaAcao;
            else
                parms[7].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoProximaAcao))
                parms[8].Value = this._descricaoProximaAcao;
            else
                parms[8].Value = DBNull.Value;


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
        /// Método utilizado para inserir uma instância de FilialObservacao no banco de dados.
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
                        this._idFilialObservacao = Convert.ToInt32(cmd.Parameters["@Idf_Filial_Observacao"].Value);
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
        /// Método utilizado para inserir uma instância de FilialObservacao no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idFilialObservacao = Convert.ToInt32(cmd.Parameters["@Idf_Filial_Observacao"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de FilialObservacao no banco de dados.
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
        /// Método utilizado para atualizar uma instância de FilialObservacao no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de FilialObservacao no banco de dados.
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
        /// Método utilizado para salvar uma instância de FilialObservacao no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de FilialObservacao no banco de dados.
        /// </summary>
        /// <param name="idFilialObservacao">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idFilialObservacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial_Observacao", SqlDbType.Int, 4));

            parms[0].Value = idFilialObservacao;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de FilialObservacao no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idFilialObservacao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idFilialObservacao, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial_Observacao", SqlDbType.Int, 4));

            parms[0].Value = idFilialObservacao;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de FilialObservacao no banco de dados.
        /// </summary>
        /// <param name="idFilialObservacao">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idFilialObservacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from TAB_Filial_Observacao where Idf_Filial_Observacao in (";

            for (int i = 0; i < idFilialObservacao.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idFilialObservacao[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idFilialObservacao">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idFilialObservacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial_Observacao", SqlDbType.Int, 4));

            parms[0].Value = idFilialObservacao;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idFilialObservacao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idFilialObservacao, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial_Observacao", SqlDbType.Int, 4));

            parms[0].Value = idFilialObservacao;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Fil.Idf_Filial_Observacao, Fil.Idf_Filial, Fil.Idf_Usuario_Filial_Perfil, Fil.Dta_Cadastro, Fil.Flg_Inativo, Fil.Des_Observacao, Fil.Flg_Sistema, Fil.Dta_Proxima_Acao, Fil.Des_Proxima_Acao FROM TAB_Filial_Observacao Fil";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de FilialObservacao a partir do banco de dados.
        /// </summary>
        /// <param name="idFilialObservacao">Chave do registro.</param>
        /// <returns>Instância de FilialObservacao.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static FilialObservacao LoadObject(int idFilialObservacao)
        {
            using (IDataReader dr = LoadDataReader(idFilialObservacao))
            {
                FilialObservacao objFilialObservacao = new FilialObservacao();
                if (SetInstance(dr, objFilialObservacao))
                    return objFilialObservacao;
            }
            throw (new RecordNotFoundException(typeof(FilialObservacao)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de FilialObservacao a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idFilialObservacao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de FilialObservacao.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static FilialObservacao LoadObject(int idFilialObservacao, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idFilialObservacao, trans))
            {
                FilialObservacao objFilialObservacao = new FilialObservacao();
                if (SetInstance(dr, objFilialObservacao))
                    return objFilialObservacao;
            }
            throw (new RecordNotFoundException(typeof(FilialObservacao)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de FilialObservacao a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idFilialObservacao))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de FilialObservacao a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idFilialObservacao, trans))
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
        /// <param name="objFilialObservacao">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, FilialObservacao objFilialObservacao)
        {
            try
            {
                if (dr.Read())
                {
                    objFilialObservacao._idFilialObservacao = Convert.ToInt32(dr["Idf_Filial_Observacao"]);
                    objFilialObservacao._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
                    if (dr["Idf_Usuario_Filial_Perfil"] != DBNull.Value)
                        objFilialObservacao._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
                    objFilialObservacao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objFilialObservacao._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    objFilialObservacao._descricaoObservacao = Convert.ToString(dr["Des_Observacao"]);
                    objFilialObservacao._flagSistema = Convert.ToBoolean(dr["Flg_Sistema"]);
                    if (dr["Dta_Proxima_Acao"] != DBNull.Value)
                        objFilialObservacao._dataProximaAcao = Convert.ToDateTime(dr["Dta_Proxima_Acao"]);
                    if (dr["Des_Proxima_Acao"] != DBNull.Value)
                        objFilialObservacao._descricaoProximaAcao = Convert.ToString(dr["Des_Proxima_Acao"]);

                    objFilialObservacao._persisted = true;
                    objFilialObservacao._modified = false;

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