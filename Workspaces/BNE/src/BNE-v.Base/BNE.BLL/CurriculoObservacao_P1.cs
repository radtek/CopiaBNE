//-- Data: 23/10/2013 16:56
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class CurriculoObservacao // Tabela: BNE_Curriculo_Observacao
    {
        #region Atributos
        private int _idCurriculoObservacao;
        private Curriculo _curriculo;
        private UsuarioFilialPerfil _usuarioFilialPerfil;
        private DateTime _dataCadastro;
        private bool _flagInativo;
        private string _descricaoObservacao;
        private bool _flagSistema;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdCurriculoObservacao
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdCurriculoObservacao
        {
            get
            {
                return this._idCurriculoObservacao;
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

        #endregion

        #region Construtores
        public CurriculoObservacao()
        {
        }
        public CurriculoObservacao(int idCurriculoObservacao)
        {
            this._idCurriculoObservacao = idCurriculoObservacao;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Curriculo_Observacao (Idf_Curriculo, Idf_Usuario_Filial_Perfil, Dta_Cadastro, Flg_Inativo, Des_Observacao, Flg_Sistema) VALUES (@Idf_Curriculo, @Idf_Usuario_Filial_Perfil, @Dta_Cadastro, @Flg_Inativo, @Des_Observacao, @Flg_Sistema);SET @Idf_Curriculo_Observacao = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Curriculo_Observacao SET Idf_Curriculo = @Idf_Curriculo, Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Dta_Cadastro = @Dta_Cadastro, Flg_Inativo = @Flg_Inativo, Des_Observacao = @Des_Observacao, Flg_Sistema = @Flg_Sistema WHERE Idf_Curriculo_Observacao = @Idf_Curriculo_Observacao";
        private const string SPDELETE = "DELETE FROM BNE_Curriculo_Observacao WHERE Idf_Curriculo_Observacao = @Idf_Curriculo_Observacao";
        private const string SPSELECTID = "SELECT * FROM BNE_Curriculo_Observacao WITH(NOLOCK) WHERE Idf_Curriculo_Observacao = @Idf_Curriculo_Observacao";
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
            parms.Add(new SqlParameter("@Idf_Curriculo_Observacao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Flg_Inativo", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Des_Observacao", SqlDbType.VarChar));
            parms.Add(new SqlParameter("@Flg_Sistema", SqlDbType.Bit, 1));
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
            parms[0].Value = this._idCurriculoObservacao;
            parms[1].Value = this._curriculo.IdCurriculo;

            if (this._usuarioFilialPerfil != null)
                parms[2].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
            else
                parms[2].Value = DBNull.Value;

            parms[4].Value = this._flagInativo;
            parms[5].Value = this._descricaoObservacao;
            parms[6].Value = this._flagSistema;

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
        /// Método utilizado para inserir uma instância de CurriculoObservacao no banco de dados.
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
                        this._idCurriculoObservacao = Convert.ToInt32(cmd.Parameters["@Idf_Curriculo_Observacao"].Value);
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
        /// Método utilizado para inserir uma instância de CurriculoObservacao no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idCurriculoObservacao = Convert.ToInt32(cmd.Parameters["@Idf_Curriculo_Observacao"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de CurriculoObservacao no banco de dados.
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
        /// Método utilizado para atualizar uma instância de CurriculoObservacao no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de CurriculoObservacao no banco de dados.
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
        /// Método utilizado para salvar uma instância de CurriculoObservacao no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de CurriculoObservacao no banco de dados.
        /// </summary>
        /// <param name="idCurriculoObservacao">Chave do registro.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idCurriculoObservacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo_Observacao", SqlDbType.Int, 4));

            parms[0].Value = idCurriculoObservacao;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de CurriculoObservacao no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCurriculoObservacao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(int idCurriculoObservacao, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo_Observacao", SqlDbType.Int, 4));

            parms[0].Value = idCurriculoObservacao;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de CurriculoObservacao no banco de dados.
        /// </summary>
        /// <param name="idCurriculoObservacao">Lista de chaves.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        public static void Delete(List<int> idCurriculoObservacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Curriculo_Observacao where Idf_Curriculo_Observacao in (";

            for (int i = 0; i < idCurriculoObservacao.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idCurriculoObservacao[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idCurriculoObservacao">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idCurriculoObservacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo_Observacao", SqlDbType.Int, 4));

            parms[0].Value = idCurriculoObservacao;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCurriculoObservacao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idCurriculoObservacao, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo_Observacao", SqlDbType.Int, 4));

            parms[0].Value = idCurriculoObservacao;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cur.Idf_Curriculo_Observacao, Cur.Idf_Curriculo, Cur.Idf_Usuario_Filial_Perfil, Cur.Dta_Cadastro, Cur.Flg_Inativo, Cur.Des_Observacao, Cur.Flg_Sistema FROM BNE_Curriculo_Observacao Cur";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de CurriculoObservacao a partir do banco de dados.
        /// </summary>
        /// <param name="idCurriculoObservacao">Chave do registro.</param>
        /// <returns>Instância de CurriculoObservacao.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static CurriculoObservacao LoadObject(int idCurriculoObservacao)
        {
            using (IDataReader dr = LoadDataReader(idCurriculoObservacao))
            {
                CurriculoObservacao objCurriculoObservacao = new CurriculoObservacao();
                if (SetInstance(dr, objCurriculoObservacao))
                    return objCurriculoObservacao;
            }
            throw (new RecordNotFoundException(typeof(CurriculoObservacao)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de CurriculoObservacao a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCurriculoObservacao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de CurriculoObservacao.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static CurriculoObservacao LoadObject(int idCurriculoObservacao, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idCurriculoObservacao, trans))
            {
                CurriculoObservacao objCurriculoObservacao = new CurriculoObservacao();
                if (SetInstance(dr, objCurriculoObservacao))
                    return objCurriculoObservacao;
            }
            throw (new RecordNotFoundException(typeof(CurriculoObservacao)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de CurriculoObservacao a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idCurriculoObservacao))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de CurriculoObservacao a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idCurriculoObservacao, trans))
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
        /// <param name="objCurriculoObservacao">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, CurriculoObservacao objCurriculoObservacao)
        {
            try
            {
                if (dr.Read())
                {
                    objCurriculoObservacao._idCurriculoObservacao = Convert.ToInt32(dr["Idf_Curriculo_Observacao"]);
                    objCurriculoObservacao._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
                    if (dr["Idf_Usuario_Filial_Perfil"] != DBNull.Value)
                        objCurriculoObservacao._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));
                    objCurriculoObservacao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objCurriculoObservacao._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                    objCurriculoObservacao._descricaoObservacao = Convert.ToString(dr["Des_Observacao"]);
                    objCurriculoObservacao._flagSistema = Convert.ToBoolean(dr["Flg_Sistema"]);

                    objCurriculoObservacao._persisted = true;
                    objCurriculoObservacao._modified = false;

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