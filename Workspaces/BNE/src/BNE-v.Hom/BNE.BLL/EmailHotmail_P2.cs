//-- Data: 15/06/2016 12:19
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace BNE.BLL
{
    public partial class EmailHotmail // Tabela: TAB_Email_Hotmail
    {
        #region Atributos
        private XmlDocument _descricaoParametros;
        #endregion

        #region Propriedades

        #region DescricaoParametros
        /// <summary>
        ///     Campo opcional.
        /// </summary>
        public XmlDocument DescricaoParametros
        {
            get { return _descricaoParametros; }
            set
            {
                _descricaoParametros = value;
                _modified = true;
            }
        }
        #endregion

        #endregion

        #region Consultas
        private const string SpSalvarDataEnvio = @"
        UPDATE TAB_Email_Hotmail SET Dta_Envio = GETDATE() WHERE Idf_Email_Hotmail = @Idf_Email_Hotmail
        ";
        #endregion

        #region Métodos

        #region GetParameters
        /// <summary>
        ///     Método auxiliar utilizado para preparar a entrada para a procedure de insert e update.
        /// </summary>
        /// <returns>Lista de parâmetros SQL.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private List<SqlParameter> GetParameters()
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Email_Hotmail", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Dta_Cadastro", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Dta_Envio", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@Eml_Destinatario", SqlDbType.VarChar, 200));
            parms.Add(new SqlParameter("@Eml_Remetente", SqlDbType.VarChar, 200));
            parms.Add(new SqlParameter("@Des_Assunto", SqlDbType.VarChar, 400));
            parms.Add(new SqlParameter("@Des_Parametros", SqlDbType.Text));
            parms.Add(new SqlParameter("@Des_Mensagem", SqlDbType.VarChar, -1));
            return (parms);
        }
        #endregion

        #region SetParameters
        /// <summary>
        ///     Método auxiliar que recebe e preenche a lista de parâmetros passada de acordo com os valores da instância.
        /// </summary>
        /// <param name="parms">Lista de parâmetros SQL.</param>
        /// <remarks>Gieyson Stelmak</remarks>
        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = _idEmailHotmail;

            if (_dataEnvio.HasValue)
                parms[2].Value = _dataEnvio;
            else
                parms[2].Value = DBNull.Value;

            parms[3].Value = _emailDestinatario;
            parms[4].Value = _emailRemetente;
            parms[5].Value = _descricaoAssunto;

            if (_descricaoParametros != null)
                parms[6].Value = _descricaoParametros.InnerXml;
            else
                parms[6].Value = DBNull.Value;

            parms[7].Value = _descricaoMensagem;

            if (!_persisted)
            {
				parms[0].Direction = ParameterDirection.Output;
                _dataCadastro = DateTime.Now;
            }
            else
            {
                parms[0].Direction = ParameterDirection.Input;
            }
            parms[1].Value = _dataCadastro;
        }
        #endregion

        #region SetInstance
        /// <summary>
        ///     Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as
        ///     colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objEmailHotmail">Instância a ser manipulada.</param>
        /// <param name="dispose">Deve dar um dipose no IDataReader ou não.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance(IDataReader dr, EmailHotmail objEmailHotmail, bool dispose = true)
        {
            try
            {
                if (dr.Read())
                {
                    objEmailHotmail._idEmailHotmail = Convert.ToInt32(dr["Idf_Email_Hotmail"]);
                    objEmailHotmail._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    if (dr["Dta_Envio"] != DBNull.Value)
                        objEmailHotmail._dataEnvio = Convert.ToDateTime(dr["Dta_Envio"]);
                    objEmailHotmail._emailDestinatario = Convert.ToString(dr["Eml_Destinatario"]);
                    objEmailHotmail._emailRemetente = Convert.ToString(dr["Eml_Remetente"]);
                    objEmailHotmail._descricaoAssunto = Convert.ToString(dr["Des_Assunto"]);
                    objEmailHotmail._descricaoMensagem = Convert.ToString(dr["Des_Mensagem"]);
                    if (dr["Des_Parametros"] != DBNull.Value)
                    {
                        var xml = new XmlDocument();
                        xml.LoadXml(dr["Des_Parametros"].ToString());
                        objEmailHotmail._descricaoParametros = xml;
                    }

                    objEmailHotmail._persisted = true;
                    objEmailHotmail._modified = false;

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

        #region SalvarDataEnvio
        public void SalvarDataEnvio(int idMensagem)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Email_Hotmail", SqlDbType = SqlDbType.Int, Size = 4, Value = idMensagem }
                };

            DataAccessLayer.ExecuteScalar(CommandType.Text, SpSalvarDataEnvio, parms);
        }
        #endregion

        #endregion
    }
}