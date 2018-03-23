using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using System.Xml;

namespace BNE.Log.Base
{
    public class BaseMessage
    {
        public Guid Id { get; set; }
        public DateTime DataCadastro { get; set; }
        public string Aplicacao { get; set; }
        public string Usuario { get; set; }
        public string Session { get; set; }
        public string URL { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string Message { get; set; }
        public string CustomMessage { get; set; }
        public string Payload { get; set; }
        public string MachineName { get; set; }
        public string UrlReferrer { get; set; }

        protected BaseMessage()
        {
            Id = Guid.NewGuid();
            try
            {
                Aplicacao = ConfigurationManager.AppSettings["BNE.Log.App"];
            }
            catch
            {
                // ignored
            }

            if (string.IsNullOrWhiteSpace(Aplicacao))
            {
                var objAssembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
                Aplicacao = objAssembly.Location;
            }
            MachineName = Environment.MachineName;
            DataCadastro = DateTime.Now;
            //projeto web http
            if (HttpContext.Current != null)
            {
                var ctx = HttpContext.Current;
                if (ctx.Session != null)
                {
                    Session = GravaSession(ctx.Session);
                }

                //padrão Microsoft
                if (string.IsNullOrEmpty(Usuario) && ctx.User != null && ctx.User.Identity != null)
                    Usuario = ctx.User.Identity.Name;

                if (ctx.Handler != null)
                {
                    URL = GravaXml(ctx.Request.Url);
                    UrlReferrer = GravaXml(ctx.Request.UrlReferrer);

                    Request = GravaRequest(ctx.Request);
                    Response = GravaXml(ctx.Response);
                }
            }
            else //projeto windows
            {
                //padrão Microsoft
                if (string.IsNullOrEmpty(Usuario) && Thread.CurrentPrincipal != null &&
                    Thread.CurrentPrincipal.Identity != null)
                    Usuario = Thread.CurrentPrincipal.Identity.Name;
            }
        }

        public BaseMessage(string message, string customMessage, string payload)
        {
            Message = message;
            CustomMessage = customMessage;
            Payload = payload;
        }

        private static string GravaSession(HttpSessionState oSession)
        {
            var sWriter = new StringWriter(new StringBuilder(8000));
            var xWriter = new XmlTextWriter(sWriter)
            {
                Formatting = Formatting.Indented
            };

            var oTipo = oSession.GetType();

            xWriter.WriteStartElement(oTipo.Name);

            xWriter.WriteStartElement("Keys");
            GravaColecao(oSession.Keys, xWriter);
            xWriter.WriteEndElement();

            xWriter.WriteStartElement("Values");
            foreach (string sKey in oSession.Keys)
            {
                xWriter.WriteStartElement("Value");
                xWriter.WriteValue(oSession[sKey] != null ? oSession[sKey].ToString() : "");
                xWriter.WriteEndElement();
            }
            xWriter.WriteEndElement();

            xWriter.WriteEndElement();

            xWriter.Close();
            return sWriter.ToString();
        }

        /// <summary>
        ///     Grava o conteudo de um objeto para um xml verificando de é uma coleção ou não
        /// </summary>
        /// <param name="oValue"></param>
        /// <param name="xWriter"></param>
        private static void GravaObjeto(object oValue, XmlTextWriter xWriter)
        {
            var value = oValue as IEnumerable;
            if (value != null && oValue is ICollection)
                GravaColecao(value, xWriter);
            else
                xWriter.WriteValue(oValue.ToString());
        }

        /// <summary>
        ///     Le uma coleção e grava dentro do XmlTextWriter
        /// </summary>
        /// <param name="oblColecao"></param>
        /// <param name="xWriter"></param>
        private static void GravaColecao(IEnumerable oblColecao, XmlTextWriter xWriter)
        {
            Type tipoGenerico = null;
            if (oblColecao.GetType().IsGenericType)
                tipoGenerico = oblColecao.GetType().GetGenericTypeDefinition();

            foreach (var obj in oblColecao)
            {
                if (tipoGenerico == null)
                {
                    var tipo = obj.GetType();

                    xWriter.WriteStartElement(tipo.Name);
                }
                else
                {
                    //Otimização para coleção genericas
                    xWriter.WriteStartElement(tipoGenerico.Name);
                }

                GravaObjeto(obj, xWriter);
                xWriter.WriteEndElement();
            }
        }

        protected static string GravaRequest(HttpRequest obj)
        {
            var tipo = obj.GetType();

            var sWriter = new StringWriter(new StringBuilder(8000));
            var xWriter = new XmlTextWriter(sWriter) { Formatting = Formatting.Indented };

            xWriter.WriteStartElement(tipo.FullName);

            //Propriedades
            GravarXmlProperty(obj, tipo, xWriter);

            xWriter.WriteStartElement("FormValues");
            foreach (var sKey in obj.Form.AllKeys)
            {
                xWriter.WriteStartElement("FormValue");
                xWriter.WriteAttributeString("Key", sKey);
                GravaObjeto(obj.Form[sKey], xWriter);
                xWriter.WriteEndElement();
            }
            xWriter.WriteEndElement();

            xWriter.WriteEndElement();
            xWriter.Close();
            return sWriter.ToString();
        }

        /// <summary>
        ///     Método de um objeto e transforma em xml o conteudo via Refexão.
        ///     Obs: Não foi usado serelização em xml pois nem todos os objetos são serealizaveis p/ xml
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected static string GravaXml(object obj)
        {
            if (obj == null)
                return string.Empty;

            var tipo = obj.GetType();

            var sWriter = new StringWriter(new StringBuilder(8000));
            var xWriter = new XmlTextWriter(sWriter)
            {
                Formatting = Formatting.Indented
            };

            xWriter.WriteStartElement(tipo.FullName);

            //Propriedades
            GravarXmlProperty(obj, tipo, xWriter);


            xWriter.WriteEndElement();
            xWriter.Close();
            return sWriter.ToString();
        }

        /// <summary>
        ///     Le as propriedades de um objeto e grava em xml no XmlTextWriter
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="tipo"></param>
        /// <param name="xWriter"></param>
        private static void GravarXmlProperty(object obj, Type tipo, XmlTextWriter xWriter)
        {
            var lsPropriedads = tipo.GetProperties();
            xWriter.WriteStartElement("Property");
            foreach (var prop in lsPropriedads)
            {
                try
                {
                    var oValue = prop.GetValue(obj, null);

                    if (oValue == null)
                        continue;

                    xWriter.WriteStartElement(prop.Name);

                    GravaObjeto(oValue, xWriter);
                    xWriter.WriteEndElement();
                }
                catch
                {
                    // ignored
                }
            }

            xWriter.WriteEndElement();
        }
    }
}