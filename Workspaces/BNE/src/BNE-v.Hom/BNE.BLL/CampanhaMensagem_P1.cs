//-- Data: 25/11/2015 14:42
//-- Autor: Marty Sroka

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;

namespace BNE.BLL
{
    public partial class CampanhaMensagem // Tabela: BNE_Campanha_Mensagem
    {
        #region Atributos
        private int _idCampanhaMensagem;
        private DateTime _dataDisparo;
        private DateTime? _dataFinalizacao;
        private UsuarioFilialPerfil _usuarioFilialPerfil;
        private string _descricaomensagemEmail;
        private string _descricaomensagemSMS;
        private bool _flagEnviaEmail;
        private bool _flagEnviaSMS;
        private string _descricaoObsErroEnvio;

        private bool _persisted;
        private bool _modified;
        #endregion

        #region Propriedades

        #region IdCampanhaMensagem
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdCampanhaMensagem
        {
            get
            {
                return this._idCampanhaMensagem;
            }
        }
        #endregion

        #region DataDisparo
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public DateTime DataDisparo
        {
            get
            {
                return this._dataDisparo;
            }
            set
            {
                this._dataDisparo = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataFinalizacao
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public DateTime? DataFinalizacao
        {
            get
            {
                return this._dataFinalizacao;
            }
            set
            {
                this._dataFinalizacao = value;
                this._modified = true;
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

        #region DescricaomensagemEmail
        /// <summary>
        /// Tamanho do campo: 250.
        /// Campo obrigatório.
        /// </summary>
        public string DescricaomensagemEmail
        {
            get
            {
                return this._descricaomensagemEmail;
            }
            set
            {
                this._descricaomensagemEmail = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaomensagemSMS
        /// <summary>
        /// Tamanho do campo: 200.
        /// Campo obrigatório.
        /// </summary>
        public string DescricaomensagemSMS
        {
            get
            {
                return this._descricaomensagemSMS;
            }
            set
            {
                this._descricaomensagemSMS = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagEnviaEmail
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagEnviaEmail
        {
            get
            {
                return this._flagEnviaEmail;
            }
            set
            {
                this._flagEnviaEmail = value;
                this._modified = true;
            }
        }
        #endregion

        #region FlagEnviaSMS
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public bool FlagEnviaSMS
        {
            get
            {
                return this._flagEnviaSMS;
            }
            set
            {
                this._flagEnviaSMS = value;
                this._modified = true;
            }
        }
        #endregion

        #region DescricaoObsErroEnvio
        /// <summary>
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


        #endregion

        #region Construtores
        public CampanhaMensagem()
        {
        }
        public CampanhaMensagem(int idCampanhaMensagem)
        {
            this._idCampanhaMensagem = idCampanhaMensagem;
            this._persisted = true;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = "INSERT INTO BNE_Campanha_Mensagem (Dta_Disparo, Dta_Finalizacao, Idf_Usuario_Filial_Perfil, Des_mensagem_Email, Des_mensagem_SMS, Flg_Envia_Email, Flg_Envia_SMS, Des_Obs_Erro_Envio) VALUES (@Dta_Disparo, @Dta_Finalizacao, @Idf_Usuario_Filial_Perfil, @Des_mensagem_Email, @Des_mensagem_SMS, @Flg_Envia_Email, @Flg_Envia_SMS, @Des_Obs_Erro_Envio);SET @Idf_Campanha_Mensagem = SCOPE_IDENTITY();";
        private const string SPUPDATE = "UPDATE BNE_Campanha_Mensagem SET Dta_Disparo = @Dta_Disparo, Dta_Finalizacao = @Dta_Finalizacao, Idf_Usuario_Filial_Perfil = @Idf_Usuario_Filial_Perfil, Des_mensagem_Email = @Des_mensagem_Email, Des_mensagem_SMS = @Des_mensagem_SMS, Flg_Envia_Email = @Flg_Envia_Email, Flg_Envia_SMS = @Flg_Envia_SMS, Des_Obs_Erro_Envio = @Des_Obs_Erro_Envio WHERE Idf_Campanha_Mensagem = @Idf_Campanha_Mensagem";
        private const string SPDELETE = "DELETE FROM BNE_Campanha_Mensagem WHERE Idf_Campanha_Mensagem = @Idf_Campanha_Mensagem";
        private const string SPSELECTID = "SELECT * FROM BNE_Campanha_Mensagem WITH(NOLOCK) WHERE Idf_Campanha_Mensagem = @Idf_Campanha_Mensagem";
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
            parms.Add(new SqlParameter("@Idf_Campanha_Mensagem", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Disparo", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Finalizacao", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Idf_Usuario_Filial_Perfil", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Des_mensagem_Email", SqlDbType.VarChar, 2000));
            parms.Add(new SqlParameter("@Des_mensagem_SMS", SqlDbType.VarChar, 200));
            parms.Add(new SqlParameter("@Flg_Envia_Email", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Flg_Envia_SMS", SqlDbType.Bit, 1));
            parms.Add(new SqlParameter("@Des_Obs_Erro_Envio", SqlDbType.VarChar, 250));

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
            parms[0].Value = this._idCampanhaMensagem;
            parms[1].Value = this._dataDisparo;

            if (this._dataFinalizacao == null)
                parms[2].Value = DBNull.Value;
            else
                parms[2].Value = this._dataFinalizacao;
            
            parms[3].Value = this._usuarioFilialPerfil.IdUsuarioFilialPerfil;
            

            if (string.IsNullOrEmpty(this._descricaomensagemEmail))
                parms[4].Value = DBNull.Value;
            else
                parms[4].Value = this._descricaomensagemEmail;

            if (string.IsNullOrEmpty(this._descricaomensagemSMS))
                parms[5].Value = DBNull.Value;
            else
                parms[5].Value = this._descricaomensagemSMS;
            
            parms[6].Value = this._flagEnviaEmail;
            parms[7].Value = this._flagEnviaSMS;

            if (!string.IsNullOrEmpty(this._descricaoObsErroEnvio))
                parms[8].Value = this._descricaoObsErroEnvio;
            else
                parms[8].Value = DBNull.Value;


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
        /// Método utilizado para inserir uma instância de CampanhaMensagem no banco de dados.
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
                        this._idCampanhaMensagem = Convert.ToInt32(cmd.Parameters["@Idf_Campanha_Mensagem"].Value);
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
        /// Método utilizado para inserir uma instância de CampanhaMensagem no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Marty Sroka</remarks>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idCampanhaMensagem = Convert.ToInt32(cmd.Parameters["@Idf_Campanha_Mensagem"].Value);
            cmd.Parameters.Clear();
            this._persisted = true;
            this._modified = false;
        }
        #endregion

        #region Update
        /// <summary>
        /// Método utilizado para atualizar uma instância de CampanhaMensagem no banco de dados.
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
        /// Método utilizado para atualizar uma instância de CampanhaMensagem no banco de dados, dentro de uma transação.
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
        /// Método utilizado para salvar uma instância de CampanhaMensagem no banco de dados.
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
        /// Método utilizado para salvar uma instância de CampanhaMensagem no banco de dados, dentro de uma transação.
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
        /// Método utilizado para excluir uma instância de CampanhaMensagem no banco de dados.
        /// </summary>
        /// <param name="idCampanhaMensagem">Chave do registro.</param>
        /// <remarks>Marty Sroka</remarks>
        public static void Delete(int idCampanhaMensagem)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Campanha_Mensagem", SqlDbType.Int, 4));

            parms[0].Value = idCampanhaMensagem;

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma instância de CampanhaMensagem no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCampanhaMensagem">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Marty Sroka</remarks>
        public static void Delete(int idCampanhaMensagem, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Campanha_Mensagem", SqlDbType.Int, 4));

            parms[0].Value = idCampanhaMensagem;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETE, parms);
        }
        /// <summary>
        /// Método utilizado para excluir uma lista de CampanhaMensagem no banco de dados.
        /// </summary>
        /// <param name="idCampanhaMensagem">Lista de chaves.</param>
        /// <remarks>Marty Sroka</remarks>
        public static void Delete(List<int> idCampanhaMensagem)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            string query = "delete from BNE_Campanha_Mensagem where Idf_Campanha_Mensagem in (";

            for (int i = 0; i < idCampanhaMensagem.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString();

                if (i > 0)
                {
                    query += ", ";
                }
                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = idCampanhaMensagem[i];
            }

            query += ")";

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="idCampanhaMensagem">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Marty Sroka</remarks>
        private static IDataReader LoadDataReader(int idCampanhaMensagem)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Campanha_Mensagem", SqlDbType.Int, 4));

            parms[0].Value = idCampanhaMensagem;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTID, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCampanhaMensagem">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Marty Sroka</remarks>
        private static IDataReader LoadDataReader(int idCampanhaMensagem, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Campanha_Mensagem", SqlDbType.Int, 4));

            parms[0].Value = idCampanhaMensagem;

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

            string SPSELECTPAG = "SELECT ROW_NUMBER() OVER (ORDER BY " + colunaOrdenacao + " " + direcaoOrdenacao + ") AS RowID, Cam.Idf_Campanha_Mensagem, Cam.Dta_Disparo, Cam.Dta_Finalizacao, Cam.Idf_Usuario_Filial_Perfil, Cam.Des_mensagem_Email, Cam.Des_mensagem_SMS, Cam.Flg_Envia_Email, Cam.Flg_Envia_SMS FROM BNE_Campanha_Mensagem Cam";
            string SPSELECTCOUNT = "SELECT COUNT(*) FROM (" + SPSELECTPAG + ") AS temp";
            SPSELECTPAG = "SELECT * FROM (" + SPSELECTPAG + ") AS temp WHERE RowId BETWEEN " + inicio.ToString() + " AND " + fim.ToString();

            totalRegistros = Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SPSELECTCOUNT, null));
            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPAG, null);
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de CampanhaMensagem a partir do banco de dados.
        /// </summary>
        /// <param name="idCampanhaMensagem">Chave do registro.</param>
        /// <returns>Instância de CampanhaMensagem.</returns>
        /// <remarks>Marty Sroka</remarks>
        public static CampanhaMensagem LoadObject(int idCampanhaMensagem)
        {
            using (IDataReader dr = LoadDataReader(idCampanhaMensagem))
            {
                CampanhaMensagem objCampanhaMensagem = new CampanhaMensagem();
                if (SetInstance(dr, objCampanhaMensagem))
                    return objCampanhaMensagem;
            }
            throw (new RecordNotFoundException(typeof(CampanhaMensagem)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de CampanhaMensagem a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idCampanhaMensagem">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de CampanhaMensagem.</returns>
        /// <remarks>Marty Sroka</remarks>
        public static CampanhaMensagem LoadObject(int idCampanhaMensagem, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(idCampanhaMensagem, trans))
            {
                CampanhaMensagem objCampanhaMensagem = new CampanhaMensagem();
                if (SetInstance(dr, objCampanhaMensagem))
                    return objCampanhaMensagem;
            }
            throw (new RecordNotFoundException(typeof(CampanhaMensagem)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de CampanhaMensagem a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Marty Sroka</remarks>
        public bool CompleteObject()
        {
            using (IDataReader dr = LoadDataReader(this._idCampanhaMensagem))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de CampanhaMensagem a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Marty Sroka</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader(this._idCampanhaMensagem, trans))
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
        /// <param name="objCampanhaMensagem">Instância a ser manipulada.</param>
        /// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Marty Sroka</remarks>
        private static bool SetInstance(IDataReader dr, CampanhaMensagem objCampanhaMensagem, bool dispose = true)
        {
            try
            {
                if (dr.Read())
                {
                    objCampanhaMensagem._idCampanhaMensagem = Convert.ToInt32(dr["Idf_Campanha_Mensagem"]);
                    objCampanhaMensagem._dataDisparo = Convert.ToDateTime(dr["Dta_Disparo"]);

                    if (dr["Dta_Finalizacao"] != DBNull.Value)
                        objCampanhaMensagem._dataFinalizacao = Convert.ToDateTime(dr["Dta_Finalizacao"]);

                    objCampanhaMensagem._usuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(dr["Idf_Usuario_Filial_Perfil"]));

                    if (dr["Des_mensagem_Email"] != DBNull.Value)
                        objCampanhaMensagem._descricaomensagemEmail = Convert.ToString(dr["Des_mensagem_Email"]);

                    if (dr["Des_mensagem_SMS"] != DBNull.Value)
                        objCampanhaMensagem._descricaomensagemSMS = Convert.ToString(dr["Des_mensagem_SMS"]);

                    if (dr["Des_Obs_Erro_Envio"] != DBNull.Value)
                        objCampanhaMensagem._descricaoObsErroEnvio = dr["Des_Obs_Erro_Envio"].ToString();

                    objCampanhaMensagem._flagEnviaEmail = Convert.ToBoolean(dr["Flg_Envia_Email"]);
                    objCampanhaMensagem._flagEnviaSMS = Convert.ToBoolean(dr["Flg_Envia_SMS"]);

                    objCampanhaMensagem._persisted = true;
                    objCampanhaMensagem._modified = false;

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