//-- Data: 05/11/2013
//   BNE

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using RestSharp;

namespace BNE.BLL
{
    public partial class EmailSituacao // Tabela: plataforma.TAB_Email_Situacao
    {

        #region [Private Class]

        [JsonObject]
        [Serializable]
        private class BloqueioMailSender
        {
            public string Email { get; set; }
            public string Message { get; set; }
        }

        [JsonObject]
        [Serializable]
        private class DesbloquearMailSender
        {
            public string[] Email { get; set; }
        }

        #endregion

        #region [Consultas]

        #region [BloquearEmailPF]

        public static string BloquearEmailPF = @"update bne.tab_pessoa_fisica set idf_email_situacao_bloqueio = @Idf_Email_Situacao
                                                    where idf_pessoa_fisica = @Idf_Pessoa_Fisica ";

        #endregion

        #endregion

        #region [DesbloquearEmail]
        /// <summary>
        ///  Desbloquear envio de e-mail para o e-mail recebido como parametro.
        /// </summary>
        /// <param name="idf_pessoa_fisca"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool DesbloquearEmail(int idf_pessoa_fisca, string email)
        {
            return AlterarSituacaoEmail(idf_pessoa_fisca, email, string.Empty, Enumeradores.EmailSituacao.Validado);
         }
        #endregion

        #region [BloquearEmail]
        /// <summary>
        ///  Bloquear envio de e-mail para o e-mail recebido como parametro.
        /// </summary>
        /// <param name="Idf_pessoa_Fisica"></param>
        /// <param name="email"></param>
        /// <param name="Motivo"></param>
        /// <returns></returns>
        public static bool BloquearEmail(int Idf_pessoa_Fisica, string email, string Motivo)
        {
            return AlterarSituacaoEmail(Idf_pessoa_Fisica, email, Motivo, Enumeradores.EmailSituacao.Bloqueado);
        }
        #endregion

        #region [Metodos Privados]

        #region [AlterarSituacaoEmail]
        /// <summary>
        /// Alterar os status do situacao e-mail bloqueio da pf.
        /// </summary>
        /// <param name="Idf_pessoa_Fisica"></param>
        /// <param name="email"></param>
        /// <param name="Motivo"></param>
        /// <returns></returns>
        private static bool AlterarSituacaoEmail(int Idf_pessoa_Fisica, string email, string Motivo, Enumeradores.EmailSituacao EmailSituacao)
        {
            try
            {
                #region [Coloca a situação do e-mail no pesssoa fisica como bloqueado]

                var sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int, 4));
                sqlParams.Add(new SqlParameter("@Idf_Email_Situacao", SqlDbType.Int, 4));

                sqlParams[0].Value = Idf_pessoa_Fisica;
                if (Enumeradores.EmailSituacao.Bloqueado.Equals(EmailSituacao))
                    sqlParams[1].Value = (int)Enumeradores.EmailSituacao.Bloqueado;//bloqueado
                else
                    sqlParams[1].Value = DBNull.Value;//Liberado


                DataAccessLayer.ExecuteNonQuery(CommandType.Text, BloquearEmailPF, sqlParams);

                
                #endregion

                #region [Post na api mailsender para bloquear o e-mail]
                string urlApi = Parametro.RecuperaValorParametro(Enumeradores.Parametro.UrlBloqueioEmail);

                // Create a request using a URL that can receive a post. 
                HttpWebRequest request = WebRequest.Create(urlApi) as HttpWebRequest;
                var json = string.Empty;
                if (Enumeradores.EmailSituacao.Bloqueado.Equals(EmailSituacao))
                {
                    json = JsonConvert.SerializeObject(new BloqueioMailSender
                    {
                        Email = email,
                        Message = Motivo
                    });
                    request.Method = "POST";
                }
                else
                {
                    json = JsonConvert.SerializeObject(new DesbloquearMailSender
                    {
                        Email = new string[] { email }
                    });
                    request.Method = "DELETE";
                }

                request.ContentType = "application/json";

                request.Proxy = WebRequest.DefaultWebProxy;
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;

                byte[] byteArray = Encoding.UTF8.GetBytes(json);
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                // Get the response.
                WebResponse response = request.GetResponse();

                using (Stream stream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream, Encoding.GetEncoding("iso-8859-1"));
                    var simg = reader.ReadToEnd();
                }
                #endregion

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, $"Erro ao bloquear/desbloquear o e-mail {email} - {EmailSituacao.ToString()}");
                return false;
            }

            return true;
        }
        #endregion

       #endregion
    }
 }