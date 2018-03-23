//-- Data: 06/09/2016 17:22
//-- Autor: Marty Sroka
using BNE.EL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class NotificacaoVipCurriculo // Tabela: BNE_Notificacao_Vip_Curriculo
    {
        #region Atributos
        private int _idNotificacaoVipCurriculo;
        private Curriculo _curriculo;
        private NotificacaoVip _notificacaoVip;
        private StatusNotificacaoVIP _statusNotificacaoVIP;
        private DateTime _dataCadastro;
        private string _descricaoObservacao;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdNotificacaoVipCurriculo
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdNotificacaoVipCurriculo
        {
            get
            {
                return this._idNotificacaoVipCurriculo;
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

        #region NotificacaoVip
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public NotificacaoVip NotificacaoVip
        {
            get
            {
                return this._notificacaoVip;
            }
            set
            {
                this._notificacaoVip = value;
                this._modified = true;
            }
        }
        #endregion

        #region StatusNotificacaoVIP
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public StatusNotificacaoVIP StatusNotificacaoVIP
        {
            get
            {
                return this._statusNotificacaoVIP;
            }
            set
            {
                this._statusNotificacaoVIP = value;
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

        #region DescricaoObservacao
        /// <summary>
        /// Tamanho do campo: 50.
        /// Campo opcional.
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

        #endregion

        #region Construtores
        public NotificacaoVipCurriculo()
        {
        }
        public NotificacaoVipCurriculo(int idNotificacaoVipCurriculo)
        {
            this._idNotificacaoVipCurriculo = idNotificacaoVipCurriculo;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Notificacao_Vip_Curriculo (Idf_Curriculo, Idf_Notificacao_Vip, Idf_Status_Notificacao_VIP, Dta_Cadastro, Des_Observacao) VALUES (@Idf_Curriculo, @Idf_Notificacao_Vip, @Idf_Status_Notificacao_VIP, @Dta_Cadastro, @Des_Observacao);SET @Idf_Notificacao_Vip_Curriculo = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Notificacao_Vip_Curriculo SET Idf_Curriculo = @Idf_Curriculo, Idf_Notificacao_Vip = @Idf_Notificacao_Vip, Idf_Status_Notificacao_VIP = @Idf_Status_Notificacao_VIP, Dta_Cadastro = @Dta_Cadastro, Des_Observacao = @Des_Observacao WHERE Idf_Notificacao_Vip_Curriculo = @Idf_Notificacao_Vip_Curriculo";
        private const string SPDELETE = "DELETE FROM BNE_Notificacao_Vip_Curriculo WHERE Idf_Notificacao_Vip_Curriculo = @Idf_Notificacao_Vip_Curriculo";
        private const string SPSELECTID = "SELECT * FROM BNE_Notificacao_Vip_Curriculo WITH(NOLOCK) WHERE Idf_Notificacao_Vip_Curriculo = @Idf_Notificacao_Vip_Curriculo";
        #endregion

        #region Métodos

        #region GetParameters
        /// <summary>
        /// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        /// </summary>
        /// <returns>Lista de parâmetros SQL.</returns>
        /// <remarks>Marty Sroka</remarks>
        private List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Notificacao_Vip_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Notificacao_Vip", SqlDbType.Int, 2));
            parms.Add(new SqlParameter("@Idf_Status_Notificacao_VIP", SqlDbType.Int, 2));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Des_Observacao", SqlDbType.VarChar, 50));
            return (parms);
        }
        #endregion

        #region SetParameters
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>Marty Sroka</remarks>
        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idNotificacaoVipCurriculo;
            parms[1].Value = this._curriculo.IdCurriculo;
            parms[2].Value = this._notificacaoVip.IdNotificacaoVip;
            parms[3].Value = this._statusNotificacaoVIP.IdStatusNotificacaoVIP;

            if (!String.IsNullOrEmpty(this._descricaoObservacao))
                parms[5].Value = this._descricaoObservacao;
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
            parms[4].Value = this._dataCadastro;
        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de NotificacaoVipCurriculo no banco de dados.
        /// </summary>
        /// <remarks>Marty Sroka</remarks>
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
                        this._idNotificacaoVipCurriculo = Convert.ToInt32(cmd.Parameters["@Idf_Notificacao_Vip_Curriculo"].Value);
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
        /// Método utilizado para inserir uma instância de NotificacaoVipCurriculo no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Marty Sroka</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idNotificacaoVipCurriculo = Convert.ToInt32(cmd.Parameters["@Idf_Notificacao_Vip_Curriculo"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de NotificacaoVipCurriculo no banco de dados.
        /// </summary>
        /// <remarks>Marty Sroka</remarks>
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
        /// Método utilizado para atualizar uma instância de NotificacaoVipCurriculo no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Marty Sroka</remarks>
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
        /// Método utilizado para salvar uma instância de NotificacaoVipCurriculo no banco de dados.
        /// </summary>
        /// <remarks>Marty Sroka</remarks>
        public void Save()
        {
            if (!this._persisted)
                this.Insert();
            else
                this.Update();
        }
        /// <summary>
        /// Método utilizado para salvar uma instância de NotificacaoVipCurriculo no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Marty Sroka</remarks>
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
        /// Método utilizado para excluir uma instância de NotificacaoVipCurriculo no banco de dados.
        /// </summary>
        /// <param name="idNotificacaoVipCurriculo">Chave do registro.</param>
        /// <remarks>Marty Sroka</remarks>
        public static void Delete(int idNotificacaoVipCurriculo)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Notificacao_Vip_Curriculo", SqlDbType.Int, 4));

            parms[0].Value = idNotificacaoVipCurriculo;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de NotificacaoVipCurriculo no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idNotificacaoVipCurriculo">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Marty Sroka</remarks>
        public static void Delete(int idNotificacaoVipCurriculo, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Notificacao_Vip_Curriculo", SqlDbType.Int, 4));

            parms[0].Value = idNotificacaoVipCurriculo;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de NotificacaoVipCurriculo no banco de dados.
        /// </summary>
        /// <param name="idNotificacaoVipCurriculo">Lista de chaves.</param>
        /// <remarks>Marty Sroka</remarks>
        public static void Delete(List<int> idNotificacaoVipCurriculo)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Notificacao_Vip_Curriculo where Idf_Notificacao_Vip_Curriculo in (";

            for (int i = 0; i < idNotificacaoVipCurriculo.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idNotificacaoVipCurriculo[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idNotificacaoVipCurriculo">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Marty Sroka</remarks>
        private static IDataReader LoadDataReader(int idNotificacaoVipCurriculo)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Notificacao_Vip_Curriculo", SqlDbType.Int, 4));

            parms[0].Value = idNotificacaoVipCurriculo;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idNotificacaoVipCurriculo">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Marty Sroka</remarks>
        private static IDataReader LoadDataReader(int idNotificacaoVipCurriculo, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Notificacao_Vip_Curriculo", SqlDbType.Int, 4));

            parms[0].Value = idNotificacaoVipCurriculo;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Not.Idf_Notificacao_Vip_Curriculo, Not.Idf_Curriculo, Not.Idf_Notificacao_Vip, Not.Idf_Status_Notificacao_VIP, Not.Dta_Cadastro, Not.Des_Observacao FROM BNE_Notificacao_Vip_Curriculo Not";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de NotificacaoVipCurriculo a partir do banco de dados.
        /// </summary>
        /// <param name="idNotificacaoVipCurriculo">Chave do registro.</param>
        /// <returns>Instância de NotificacaoVipCurriculo.</returns>
        /// <remarks>Marty Sroka</remarks>
        public static NotificacaoVipCurriculo LoadObject(int idNotificacaoVipCurriculo)
        {
            using (IDataReader dr = LoadDataReader(idNotificacaoVipCurriculo))
            {
                NotificacaoVipCurriculo objNotificacaoVipCurriculo = new NotificacaoVipCurriculo();
                if (SetInstance(dr, objNotificacaoVipCurriculo))
                    return objNotificacaoVipCurriculo;
            }
            throw (new RecordNotFoundException(typeof(NotificacaoVipCurriculo)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de NotificacaoVipCurriculo a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idNotificacaoVipCurriculo">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de NotificacaoVipCurriculo.</returns>
        /// <remarks>Marty Sroka</remarks>
        public static NotificacaoVipCurriculo LoadObject(int idNotificacaoVipCurriculo, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idNotificacaoVipCurriculo, trans))
            {
                NotificacaoVipCurriculo objNotificacaoVipCurriculo = new NotificacaoVipCurriculo();
                if (SetInstance(dr, objNotificacaoVipCurriculo))
                    return objNotificacaoVipCurriculo;
            }
            throw (new RecordNotFoundException(typeof(NotificacaoVipCurriculo)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de NotificacaoVipCurriculo a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Marty Sroka</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idNotificacaoVipCurriculo))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de NotificacaoVipCurriculo a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Marty Sroka</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idNotificacaoVipCurriculo, trans))
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
        /// <param name="objNotificacaoVipCurriculo">Instância a ser manipulada.</param>
        /// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Marty Sroka</remarks>
        private static bool SetInstance(IDataReader dr, NotificacaoVipCurriculo objNotificacaoVipCurriculo, bool dispose = true)
        {
            try
            {
                if (dr.Read())
                {
                    objNotificacaoVipCurriculo._idNotificacaoVipCurriculo = Convert.ToInt32(dr["Idf_Notificacao_Vip_Curriculo"]);
                    objNotificacaoVipCurriculo._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
                    objNotificacaoVipCurriculo._notificacaoVip = new NotificacaoVip(Convert.ToInt16(dr["Idf_Notificacao_Vip"]));
                    objNotificacaoVipCurriculo._statusNotificacaoVIP = new StatusNotificacaoVIP(Convert.ToInt16(dr["Idf_Status_Notificacao_VIP"]));
                    objNotificacaoVipCurriculo._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    if (dr["Des_Observacao"] != DBNull.Value)
                        objNotificacaoVipCurriculo._descricaoObservacao = Convert.ToString(dr["Des_Observacao"]);

                    objNotificacaoVipCurriculo._persisted = true;
                    objNotificacaoVipCurriculo._modified = false;

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