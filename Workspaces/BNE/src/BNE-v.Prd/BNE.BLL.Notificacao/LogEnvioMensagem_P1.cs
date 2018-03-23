using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL.Notificacao
{
    public partial class LogEnvioMensagem
    {

        #region Atributos

        private int _idLogEnvioMensagem;
        private string _desAssunto;
        private string _emlDestinatario;
        private DateTime _dataCadastro;
        private int? _curriculo;
        private string _obsMensagem;
        private int? _cartaEmail;
        private string _emlRemetente;
        
        #endregion

        #region Propriedades

        #region IdLogEnvioMensagem
        /// <summary>
        /// Campo obrigatório.
        /// Campo auto-numerado.
        /// </summary>
        public int IdLogEnvioMensagem
        {
            get
            {
                return this._idLogEnvioMensagem;
            }
        }
        #endregion

        #region DesAssunto
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        public string DesAssunto
        {
            get
            {
                return this._desAssunto;
            }
            set
            {
                this._desAssunto = value;
            }
        }
        #endregion

        #region EmlDestinatario
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        public string EmlDestinatario
        {
            get
            {
                return this._emlDestinatario;
            }
            set
            {
                this._emlDestinatario = value;
            }
        }
        #endregion

        #region Curriculo
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int? curriculo
        {
            get
            {
                return this._curriculo;
            }
            set
            {
                this._curriculo = value;
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
            set
            {
                this._dataCadastro = value;
            }
        }
        #endregion

        #region ObsMensagem
        /// <summary>
        /// Tamanho do campo: 100.
        /// Campo opcional.
        /// </summary>
        public string ObsMensagem
        {
            get
            {
                return this._obsMensagem;
            }
            set
            {
                this._obsMensagem = value;
            }
        }
        #endregion

        #region CartaEmail
        /// <summary>
        /// Campo obrigatório.
        /// </summary>
        public int? CartaEmail
        {
            get
            {
                return this._cartaEmail;
            }
            set
            {
                this._cartaEmail = value;
            }
        }
        #endregion

        #region EmlRemetente
        /// <summary>
        /// Tamanho do campo 50 .
        /// Campo opcional.
        /// </summary>
        public string EmlRemetente
        {
            get
            {
                return this._emlRemetente;
            }
            set
            {
                this._emlRemetente = value;
            }
        }

        #endregion

        #endregion

        #region Construtores
        public LogEnvioMensagem()
        {
        }
        public LogEnvioMensagem(int idlogEnvioMensagem)
        {
            this._idLogEnvioMensagem = idlogEnvioMensagem;
        }
        #endregion

        #region Consultas
        private const string SPINSERT = @"INSERT INTO alerta.Log_Envio_Mensagem
                                                (Des_Assunto,Eml_Destinatario,Dta_Cadastro,
                                                 Idf_Curriculo,Obs_Mensagem,Idf_Carta_Email, Eml_Remetente) 
                                          VALUES 
                                                (@Des_Assunto, @Eml_Destinatario, @Dta_Cadastro, @Idf_Curriculo, @Obs_Mensagem,
                                                 @Idf_Carta_Email, @Eml_Remetente);SET @Idf_Log_Envio_Mensagem = SCOPE_IDENTITY();";
        #endregion

        #region Métodos

        #region GetParameters
        /// <summary>
        /// Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        /// </summary>
        /// <returns>Lista de parâmetros SQL.</returns>
        private List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Log_Envio_Mensagem", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Des_Assunto", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Eml_Destinatario", SqlDbType.VarChar, 50));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Obs_Mensagem", SqlDbType.VarChar, 200));
            parms.Add(new SqlParameter("@Idf_Carta_Email", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Eml_Remetente", SqlDbType.VarChar, 50));
            return (parms);
        }
        #endregion

        #region SetParameters
        /// <summary>
        /// Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idLogEnvioMensagem;
            parms[1].Value = this._desAssunto;
            parms[2].Value = this._emlDestinatario;
            parms[3].Value = this._dataCadastro;

            if (this._curriculo != null)
                parms[4].Value = this._curriculo;
            else
                parms[4].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(this._obsMensagem))
                parms[5].Value = this._obsMensagem;
            else
                parms[5].Value = DBNull.Value;

            if (this._cartaEmail != null)
                parms[6].Value = this._cartaEmail;
            else
                parms[6].Value = DBNull.Value;

            if (!String.IsNullOrEmpty(this._emlRemetente))
                parms[7].Value = this._emlRemetente;
            else
                parms[7].Value = DBNull.Value;

        }
        #endregion

        #region Insert
        /// <summary>
        /// Método utilizado para inserir uma instância de Mensagem no banco de dados.
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
                        this._idLogEnvioMensagem = Convert.ToInt32(cmd.Parameters["@Idf_Log_Envio_Mensagem"].Value);
                        cmd.Parameters.Clear();
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
        /// Método utilizado para inserir uma instância de Mensagem no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        private void Insert(SqlTransaction trans)
        {
            List<SqlParameter> parms = GetParameters();
            SetParameters(parms);
            SqlCommand cmd = DataAccessLayer.ExecuteNonQueryCmd(trans, CommandType.Text, SPINSERT, parms);
            this._idLogEnvioMensagem = Convert.ToInt32(cmd.Parameters["@Idf_Log_Envio_Mensagem"].Value);
            cmd.Parameters.Clear();
        }
        #endregion

        #region Save
        /// <summary>
        /// Método utilizado para salvar uma instância de Mensagem no banco de dados.
        /// </summary>
        public void Save()
        {
            this.Insert();
        }
        /// <summary>
        /// Método utilizado para salvar uma instância de Mensagem no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        public void Save(SqlTransaction trans)
        {
            this.Insert(trans);
        }
        #endregion

        #endregion

    }
}
