//-- Data: 15/02/2016 16:55
//-- Autor: Mailson

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class CampanhaMensagemEnvios // Tabela: BNE_Campanha_Mensagem_Envios
    {
        #region Atributos
        private int _idCampanhaMensagemEnvios;
        private CampanhaMensagem _campanhaMensagem;
        private Curriculo _curriculo;
        private DateTime? _dataProcessamento;
        private bool? _flagEnviouEmail;
        private bool? _flagEnviouSMS;
        private string _descricaoObsErroEnvio;
        private DateTime? _dataAgendamento;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdCampanhaMensagemEnvios
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdCampanhaMensagemEnvios
        {
            get
            {
                return this._idCampanhaMensagemEnvios;
            }
        }
        #endregion

        #region CampanhaMensagem
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public CampanhaMensagem CampanhaMensagem
        {
            get
            {
                return this._campanhaMensagem;
            }
            set
            {
                this._campanhaMensagem = value;
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

        #region DataProcessamento
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataProcessamento
        {
            get
            {
                return this._dataProcessamento;
            }
            set
            {
                this._dataProcessamento = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagEnviouEmail
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public bool? FlagEnviouEmail
        {
            get
            {
                return this._flagEnviouEmail;
            }
            set
            {
                this._flagEnviouEmail = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagEnviouSMS
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public bool? FlagEnviouSMS
        {
            get
            {
                return this._flagEnviouSMS;
            }
            set
            {
                this._flagEnviouSMS = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoObsErroEnvio
        /// <summary>
        /// Tamanho do campo: 250.
        /// Campo opcional.
        /// </summary>
        public string DescricaoObsErroEnvio
        {
            get
            {
                return this._descricaoObsErroEnvio;
            }
            set
            {
                this._descricaoObsErroEnvio = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataAgendamento
        /// <summary>
        /// Campo opcional.
        /// </summary>
        public DateTime? DataAgendamento
        {
            get
            {
                return this._dataAgendamento;
            }
            set
            {
                this._dataAgendamento = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Construtores
        public CampanhaMensagemEnvios()
        {
        }
        public CampanhaMensagemEnvios(int idCampanhaMensagemEnvios)
        {
            this._idCampanhaMensagemEnvios = idCampanhaMensagemEnvios;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Campanha_Mensagem_Envios (Idf_Campanha_Mensagem, Idf_Curriculo, Dta_Processamento, Flg_Enviou_Email, Flg_Enviou_SMS, Des_Obs_Erro_Envio, Dta_Agendamento) VALUES (@Idf_Campanha_Mensagem, @Idf_Curriculo, @Dta_Processamento, @Flg_Enviou_Email, @Flg_Enviou_SMS, @Des_Obs_Erro_Envio, @Dta_Agendamento);SET @Idf_Campanha_Mensagem_Envios = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Campanha_Mensagem_Envios SET Idf_Campanha_Mensagem = @Idf_Campanha_Mensagem, Idf_Curriculo = @Idf_Curriculo, Dta_Processamento = @Dta_Processamento, Flg_Enviou_Email = @Flg_Enviou_Email, Flg_Enviou_SMS = @Flg_Enviou_SMS, Des_Obs_Erro_Envio = @Des_Obs_Erro_Envio, Dta_Agendamento = @Dta_Agendamento WHERE Idf_Campanha_Mensagem_Envios = @Idf_Campanha_Mensagem_Envios";
        private const string SPDELETE = "DELETE FROM BNE_Campanha_Mensagem_Envios WHERE Idf_Campanha_Mensagem_Envios = @Idf_Campanha_Mensagem_Envios";
        private const string SPSELECTID = "SELECT * FROM BNE_Campanha_Mensagem_Envios WITH(NOLOCK) WHERE Idf_Campanha_Mensagem_Envios = @Idf_Campanha_Mensagem_Envios";
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
            parms.Add(new SqlParameter("@Idf_Campanha_Mensagem_Envios", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Campanha_Mensagem", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Processamento", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Flg_Enviou_Email", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Enviou_SMS", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Des_Obs_Erro_Envio", SqlDbType.VarChar, 250));
            parms.Add(new SqlParameter("@Dta_Agendamento", SqlDbType.DateTime, 8));
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
            parms[0].Value = this._idCampanhaMensagemEnvios;
            parms[1].Value = this._campanhaMensagem.IdCampanhaMensagem;
            parms[2].Value = this._curriculo.IdCurriculo;

            if (this._dataProcessamento.HasValue)
                parms[3].Value = this._dataProcessamento;
            else
                parms[3].Value = DBNull.Value;


            if (this._flagEnviouEmail.HasValue)
                parms[4].Value = this._flagEnviouEmail;
            else
                parms[4].Value = DBNull.Value;


            if (this._flagEnviouSMS.HasValue)
                parms[5].Value = this._flagEnviouSMS;
            else
                parms[5].Value = DBNull.Value;


            if (!String.IsNullOrEmpty(this._descricaoObsErroEnvio))
                parms[6].Value = this._descricaoObsErroEnvio;
            else
                parms[6].Value = DBNull.Value;


            if (this._dataAgendamento.HasValue)
                parms[7].Value = this._dataAgendamento;
            else
                parms[7].Value = DBNull.Value;


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
        /// Método utilizado para inserir uma instância de CampanhaMensagemEnvios no banco de dados.
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
                        this._idCampanhaMensagemEnvios = Convert.ToInt32(cmd.Parameters["@Idf_Campanha_Mensagem_Envios"].Value);
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
        /// Método utilizado para inserir uma instância de CampanhaMensagemEnvios no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Mailson</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idCampanhaMensagemEnvios = Convert.ToInt32(cmd.Parameters["@Idf_Campanha_Mensagem_Envios"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de CampanhaMensagemEnvios no banco de dados.
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
        /// Método utilizado para atualizar uma instância de CampanhaMensagemEnvios no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de CampanhaMensagemEnvios no banco de dados.
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
        /// Método utilizado para salvar uma instância de CampanhaMensagemEnvios no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de CampanhaMensagemEnvios no banco de dados.
        /// </summary>
        /// <param name="idCampanhaMensagemEnvios">Chave do registro.</param>
        /// <remarks>Mailson</remarks>
        public static void Delete(int idCampanhaMensagemEnvios)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Campanha_Mensagem_Envios", SqlDbType.Int, 4));

            parms[0].Value = idCampanhaMensagemEnvios;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de CampanhaMensagemEnvios no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCampanhaMensagemEnvios">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Mailson</remarks>
        public static void Delete(int idCampanhaMensagemEnvios, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Campanha_Mensagem_Envios", SqlDbType.Int, 4));

            parms[0].Value = idCampanhaMensagemEnvios;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de CampanhaMensagemEnvios no banco de dados.
        /// </summary>
        /// <param name="idCampanhaMensagemEnvios">Lista de chaves.</param>
        /// <remarks>Mailson</remarks>
        public static void Delete(List<int> idCampanhaMensagemEnvios)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Campanha_Mensagem_Envios where Idf_Campanha_Mensagem_Envios in (";

            for (int i = 0; i < idCampanhaMensagemEnvios.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idCampanhaMensagemEnvios[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idCampanhaMensagemEnvios">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Mailson</remarks>
        private static IDataReader LoadDataReader(int idCampanhaMensagemEnvios)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Campanha_Mensagem_Envios", SqlDbType.Int, 4));

            parms[0].Value = idCampanhaMensagemEnvios;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCampanhaMensagemEnvios">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Mailson</remarks>
        private static IDataReader LoadDataReader(int idCampanhaMensagemEnvios, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Campanha_Mensagem_Envios", SqlDbType.Int, 4));

            parms[0].Value = idCampanhaMensagemEnvios;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cam.Idf_Campanha_Mensagem_Envios, Cam.Idf_Campanha_Mensagem, Cam.Idf_Curriculo, Cam.Dta_Processamento, Cam.Flg_Enviou_Email, Cam.Flg_Enviou_SMS, Cam.Des_Obs_Erro_Envio, Cam.Dta_Agendamento FROM BNE_Campanha_Mensagem_Envios Cam";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de CampanhaMensagemEnvios a partir do banco de dados.
        /// </summary>
        /// <param name="idCampanhaMensagemEnvios">Chave do registro.</param>
        /// <returns>Instância de CampanhaMensagemEnvios.</returns>
        /// <remarks>Mailson</remarks>
        public static CampanhaMensagemEnvios LoadObject(int idCampanhaMensagemEnvios)
        {
            using (IDataReader dr = LoadDataReader(idCampanhaMensagemEnvios))
            {
                CampanhaMensagemEnvios objCampanhaMensagemEnvios = new CampanhaMensagemEnvios();
                if (SetInstance(dr, objCampanhaMensagemEnvios))
                    return objCampanhaMensagemEnvios;
            }
            throw (new RecordNotFoundException(typeof(CampanhaMensagemEnvios)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de CampanhaMensagemEnvios a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCampanhaMensagemEnvios">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de CampanhaMensagemEnvios.</returns>
        /// <remarks>Mailson</remarks>
        public static CampanhaMensagemEnvios LoadObject(int idCampanhaMensagemEnvios, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idCampanhaMensagemEnvios, trans))
            {
                CampanhaMensagemEnvios objCampanhaMensagemEnvios = new CampanhaMensagemEnvios();
                if (SetInstance(dr, objCampanhaMensagemEnvios))
                    return objCampanhaMensagemEnvios;
            }
            throw (new RecordNotFoundException(typeof(CampanhaMensagemEnvios)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de CampanhaMensagemEnvios a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Mailson</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idCampanhaMensagemEnvios))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de CampanhaMensagemEnvios a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Mailson</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idCampanhaMensagemEnvios, trans))
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
        /// <param name="objCampanhaMensagemEnvios">Instância a ser manipulada.</param>
        /// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Mailson</remarks>
        private static bool SetInstance(IDataReader dr, CampanhaMensagemEnvios objCampanhaMensagemEnvios, bool dispose = true)
        {
            try
            {
                if (dr.Read())
                {
                    objCampanhaMensagemEnvios._idCampanhaMensagemEnvios = Convert.ToInt32(dr["Idf_Campanha_Mensagem_Envios"]);
                    objCampanhaMensagemEnvios._campanhaMensagem = new CampanhaMensagem(Convert.ToInt32(dr["Idf_Campanha_Mensagem"]));
                    objCampanhaMensagemEnvios._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
                    if (dr["Dta_Processamento"] != DBNull.Value)
                        objCampanhaMensagemEnvios._dataProcessamento = Convert.ToDateTime(dr["Dta_Processamento"]);
                    if (dr["Flg_Enviou_Email"] != DBNull.Value)
                        objCampanhaMensagemEnvios._flagEnviouEmail = Convert.ToBoolean(dr["Flg_Enviou_Email"]);
                    if (dr["Flg_Enviou_SMS"] != DBNull.Value)
                        objCampanhaMensagemEnvios._flagEnviouSMS = Convert.ToBoolean(dr["Flg_Enviou_SMS"]);
                    if (dr["Des_Obs_Erro_Envio"] != DBNull.Value)
                        objCampanhaMensagemEnvios._descricaoObsErroEnvio = Convert.ToString(dr["Des_Obs_Erro_Envio"]);
                    if (dr["Dta_Agendamento"] != DBNull.Value)
                        objCampanhaMensagemEnvios._dataAgendamento = Convert.ToDateTime(dr["Dta_Agendamento"]);

                    objCampanhaMensagemEnvios._persisted = true;
                    objCampanhaMensagemEnvios._modified = false;

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
        private static bool SetInstanceNotDipose(IDataReader dr, CampanhaMensagemEnvios objCampanhaMensagemEnvios, bool dispose = true)
        {
            try
            {
                objCampanhaMensagemEnvios._idCampanhaMensagemEnvios = Convert.ToInt32(dr["Idf_Campanha_Mensagem_Envios"]);
                objCampanhaMensagemEnvios._campanhaMensagem = new CampanhaMensagem(Convert.ToInt32(dr["Idf_Campanha_Mensagem"]));
                objCampanhaMensagemEnvios._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
                if (dr["Dta_Processamento"] != DBNull.Value)
                    objCampanhaMensagemEnvios._dataProcessamento = Convert.ToDateTime(dr["Dta_Processamento"]);
                if (dr["Flg_Enviou_Email"] != DBNull.Value)
                    objCampanhaMensagemEnvios._flagEnviouEmail = Convert.ToBoolean(dr["Flg_Enviou_Email"]);
                if (dr["Flg_Enviou_SMS"] != DBNull.Value)
                    objCampanhaMensagemEnvios._flagEnviouSMS = Convert.ToBoolean(dr["Flg_Enviou_SMS"]);
                if (dr["Des_Obs_Erro_Envio"] != DBNull.Value)
                    objCampanhaMensagemEnvios._descricaoObsErroEnvio = dr["Des_Obs_Erro_Envio"].ToString();
                if (dr["Dta_Agendamento"] != DBNull.Value)
                    objCampanhaMensagemEnvios._dataAgendamento = Convert.ToDateTime(dr["Dta_Agendamento"]);
                objCampanhaMensagemEnvios._persisted = true;
                objCampanhaMensagemEnvios._modified = false;

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #endregion
    }
}