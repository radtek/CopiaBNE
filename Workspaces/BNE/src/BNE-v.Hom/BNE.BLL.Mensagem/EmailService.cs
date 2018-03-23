using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using BNE.BLL.Mensagem.Contracts;
using log4net;
using MailSender;
using MailSender.Models;

namespace BNE.BLL.Mensagem
{
    public class EmailService : IEmailService
    {
        private static IMailSenderAPI _mailSenderApi;
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public EmailService()
        {
            if (_mailSenderApi == null)
            {
                _mailSenderApi = InitializeApi();
            }
        }

        public MailSenderAPI InitializeApi()
        {
            _logger.Info($"API Inicializada {DateTime.Now}");
            return new MailSenderAPI(new Uri("http://mailsender.bne.com.br"));
        }

        public IEnumerable<Email> GetAllToSend()
        {
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "[mensagem].[RecuperarAguardandoEnvio]", null))
            {
                while (dr.Read())
                {
                    var objEmail = new Email();
                    objEmail.SetInstance(dr);
                    yield return objEmail;
                }
            }
        }

        public void Deletar(Email objEmail)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@id", SqlDbType = SqlDbType.Int, Size = 4, Value = objEmail.Id}
            };
            DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "[mensagem].[DeletarEmail]", parms);
        }

        public void Salvar(string key, string de, string para, string assunto, string mensagem)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@key", SqlDbType = SqlDbType.VarChar, Size = 20, Value = key},
                new SqlParameter {ParameterName = "@de", SqlDbType = SqlDbType.VarChar, Size = 200, Value = de},
                new SqlParameter {ParameterName = "@para", SqlDbType = SqlDbType.VarChar, Size = 200, Value = para},
                new SqlParameter {ParameterName = "@assunto", SqlDbType = SqlDbType.VarChar, Size = 200, Value = assunto},
                new SqlParameter {ParameterName = "@mensagem", SqlDbType = SqlDbType.VarChar, Size = -1, Value = mensagem}
            };

            try
            {
                DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "[mensagem].[Salvar]", parms);
            }
            catch (Exception ex)
            {
                _logger.Error("Ocorreu um erro ao salvar uma mensagem no banco para envio.", ex);
            }
        }

        public void Salvar(string key, string de, string para, string assunto, string mensagem, string nomeAnexo, string anexo)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@key", SqlDbType = SqlDbType.VarChar, Size = 20, Value = key},
                new SqlParameter {ParameterName = "@de", SqlDbType = SqlDbType.VarChar, Size = 200, Value = de},
                new SqlParameter {ParameterName = "@para", SqlDbType = SqlDbType.VarChar, Size = 200, Value = para},
                new SqlParameter {ParameterName = "@assunto", SqlDbType = SqlDbType.VarChar, Size = 200, Value = assunto},
                new SqlParameter {ParameterName = "@mensagem", SqlDbType = SqlDbType.VarChar, Size = -1, Value = mensagem},
                new SqlParameter {ParameterName = "@nomeanexo", SqlDbType = SqlDbType.VarChar, Size = 50, Value = nomeAnexo},
                new SqlParameter {ParameterName = "@anexo", SqlDbType = SqlDbType.VarChar, Size = -1, Value = anexo}
            };

            try
            {
                DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "[mensagem].[Salvar]", parms);
            }
            catch (Exception ex)
            {
                _logger.Error("Ocorreu um erro ao salvar uma mensagem no banco para envio.", ex);
            }
        }

        /// <summary>
        ///     Método que manda o email para a api de envio, se nao conseguir comunicacao, salva na base para o servico tentar
        ///     enviar
        /// </summary>
        /// <param name="key"></param>
        /// <param name="de"></param>
        /// <param name="para"></param>
        /// <param name="assunto"></param>
        /// <param name="mensagem"></param>
        /// <param name="deveTentarReenvio">Na bne_mensagem_cs já tem robo que fica lendo as mensagens que nao foram enviadas e tenta o reenvio</param>
        public void EnviarEmail(string key, string de, string para, string assunto, string mensagem, bool deveTentarReenvio = false)
        {
            try
            {
                var retorno = _mailSenderApi.Mail.Post(new SendCommand(key, de, new List<string> { para }, null, null, null, assunto, mensagem));
                if (retorno == null && deveTentarReenvio) //Deu algum problema ao chamar a API
                {
                    Salvar(key, de, para, assunto, mensagem);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Ocorreu um erro ao enviar uma mensagem para a api.", ex);
                throw;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="de"></param>
        /// <param name="para"></param>
        /// <param name="assunto"></param>
        /// <param name="mensagem"></param>
        /// <param name="nomeAnexo"></param>
        /// <param name="anexo">Base64 String</param>
        /// <param name="deveTentarReenvio">Na bne_mensagem_cs já tem robo que fica lendo as mensagens que nao foram enviadas e tenta o reenvio</param>
        public void EnviarEmail(string key, string de, string para, string assunto, string mensagem, string nomeAnexo, string anexo, bool deveTentarReenvio = false)
        {
            try
            {
                var retorno = _mailSenderApi.Mail.Post(new SendCommand(key, de, new List<string> { para }, null, null, null, assunto, mensagem, null, null, null, new Dictionary<string, string> { { nomeAnexo, anexo } }));
                if (retorno == null && deveTentarReenvio) //Deu algum problema ao chamar a API
                {
                    Salvar(key, de, para, assunto, mensagem, nomeAnexo, anexo);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Ocorreu um erro ao enviar uma mensagem para a api.", ex);
                throw;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="de"></param>
        /// <param name="para"></param>
        /// <param name="assunto"></param>
        /// <param name="mensagem"></param>
        /// <param name="anexos">Dicionario com o nome do anexo e o anexo em Base64 String</param>
        /// <param name="deveSalvarParaTentarReenvio">Na bne_mensagem_cs já tem robo que fica lendo as mensagens que nao foram enviadas e tenta o reenvio</param>
        public void EnviarEmail(string key, string de, string para, string assunto, string mensagem, Dictionary<string, string> anexos, bool deveSalvarParaTentarReenvio = false)
        {
            try
            {
                var retorno = _mailSenderApi.Mail.Post(new SendCommand(key, de, new List<string> { para }, null, null, null, assunto, mensagem, null, null, null, anexos));
                if (retorno == null && deveSalvarParaTentarReenvio) //Deu algum problema ao chamar a API
                {
                    var anexo = anexos.FirstOrDefault();
                    Salvar(key, de, para, assunto, mensagem, anexo.Key, anexo.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Ocorreu um erro ao enviar uma mensagem para a api.", ex);
                throw;
            }
        }

        public void EnviarEmail(string key, string de, List<string> para, Guid templateId, dynamic substitutionParameters = null, dynamic sectionParameters = null)
        {
            _mailSenderApi.Mail.Post(new SendCommand(key, de, para, templateId: templateId, substitution: substitutionParameters, section: sectionParameters));
        }

        public void FalhaAoEnviar(Email email)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@id", SqlDbType = SqlDbType.Int, Size = 4, Value = email.Id}
            };
            DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "[mensagem].[Falha]", parms);
        }

        public void Enviados(ConcurrentBag<int> enviados)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@idfs", SqlDbType = SqlDbType.VarChar, Size = -1, Value = string.Join(",", enviados)},
                new SqlParameter {ParameterName = "@Data", SqlDbType = SqlDbType.DateTime, Value = DateTime.Now}
            };
            DataAccessLayer.ExecuteNonQuery(CommandType.StoredProcedure, "[mensagem].[AtualizarEnviados]", parms);
        }
    }
}