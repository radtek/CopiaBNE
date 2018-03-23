using System;
using System.Configuration;
using System.Net;

namespace BNE.Services.AsyncExecutor
{
    public static class SMSController
    {

        public static ISms ObterGateway()
        {
            var objSMSTWW = new SMSTWW();
            var oSms = (ISms)objSMSTWW;
            return oSms;
        }

        public interface ISms
        {
            bool Enviar(string number, string message, string tracker);
        }

        internal class SMSTWW : ISms
        {

            private wsSMSTWW.ReluzCapWebServiceSoapClient _oReluzCap;
            private readonly string _login;
            private readonly string _password;

            public SMSTWW()
            {
                _oReluzCap = new wsSMSTWW.ReluzCapWebServiceSoapClient();

                if (ConfigurationManager.AppSettings["usarProxy"].Equals("1"))
                {
                    var oProxy = new WebProxy(ConfigurationManager.AppSettings["proxyServer"], true);
                    WebRequest.DefaultWebProxy = oProxy;
                }

                _login = ConfigurationManager.AppSettings["usuarioSMSTWW"];
                _password = ConfigurationManager.AppSettings["senhaSMSTWW"];
            }

            ~SMSTWW()
            {
                _oReluzCap = null;
            }

            private String EliminarCaracteres(String entrada)
            {
                const string acentos = "áàäâãéèêëíìïîóòöõôùúüûçÁÀÄÂÃÉÈÊËÍÌÏÎÓÒÖÕÔÙÚÜÛÇ";
                const string semAcentos = "aaaaaeeeeiiiiooooouuuucAAAAAEEEEIIIIOOOOOUUUUC";
                const string invalidos = "ºª~^|§";

                // Remove acentos inválidos
                for (int i = 0; i < acentos.Length; i++)
                {
                    entrada = entrada.Replace(acentos[i], semAcentos[i]);
                }

                // Remove caracteres inválidos
                for (int i = 0; i < invalidos.Length; i++)
                {
                    entrada = entrada.Replace(Convert.ToString(invalidos[i]), String.Empty);
                }

                return entrada;
            }

            #region Enviar
            public bool Enviar(string number, string message, string tracker)
            {
                string retorno = "nok";
                string mensagemTratada = EliminarCaracteres(message);

                retorno = _oReluzCap.EnviaSMS(_login, _password, tracker, number, mensagemTratada);

                if ("ok".Equals(retorno, StringComparison.OrdinalIgnoreCase))
                    return true;

                if ("nok".Equals(retorno, StringComparison.OrdinalIgnoreCase))
                    return false;

                Dispose();

                throw new InvalidOperationException(
                    String.Format("Não foi possível enviar o sms. Mensagem do webservice: {0}", retorno));
            }
            #endregion Enviar

            public void Dispose()
            {
                var service = _oReluzCap as IDisposable;
                if (service != null)
                {
                    try
                    {
                        service.Dispose();
                        _oReluzCap = null;
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                    }
                }

            }

        }

    }
}