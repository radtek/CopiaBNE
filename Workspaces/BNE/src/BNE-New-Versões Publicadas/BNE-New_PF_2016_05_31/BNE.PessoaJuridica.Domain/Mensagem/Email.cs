using System;
using System.Web.Script.Serialization;
using BNE.ExceptionLog.Interface;

namespace BNE.PessoaJuridica.Domain.Mensagem
{
    public class Email
    {

        private readonly ILogger _logger;
        private readonly Parametro _parametro;

        public Email(Parametro parametro, ILogger logger)
        {
            _parametro = parametro;
            _logger = logger;
        }

        private string EnderecoAPI
        {
            get
            {
                string endereco = _parametro.RecuperarValor(Model.Enumeradores.Parametro.EnderecoAPIMensagem);
                endereco = endereco.Replace("{sms_ou_email}", "email");
                return endereco;
            }
        }

        #region EnviarEmail
        /// <summary>
        /// Processo para dispara um email para a API de mensagem
        /// </summary>
        /// <param name="carta"></param>
        /// <param name="emailDestino"></param>
        /// <param name="emailRemetente"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public bool EnviarEmail(Model.Enumeradores.Email carta, string emailRemetente, string emailDestino, dynamic parametros)
        {
            var service = new Core.Helpers.HttpService();
            var serializer = new JavaScriptSerializer();
            var model = new
            {
                emailDestino,
                emailRemetente,
                parametros
            };

            var payload = serializer.Serialize(model);

            var retorno = service.Post(EnderecoAPI, carta.ToString(), null, payload);

            if (retorno.IsSuccessStatusCode)
                return true;

            _logger.Error(new Exception(retorno.Content.ReadAsStringAsync().Result));

            return false;
        }
        #endregion

    }
}
