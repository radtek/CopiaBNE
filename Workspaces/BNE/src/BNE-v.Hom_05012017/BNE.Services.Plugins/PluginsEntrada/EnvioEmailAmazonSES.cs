using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Dynamic;
using System.Xml;
using BNE.BLL;
using BNE.BLL.Common;
using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;

namespace BNE.Services.Plugins.PluginsEntrada
{
    [Export(typeof(IInputPlugin))]
    [ExportMetadata("Type", "EnvioEmailAmazonSES")]
    public class EnvioEmailAmazonSES : InputPlugin
    {
        #region DoExecuteTask
        protected override IPluginResult DoExecuteTask(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            var idMensagem = objParametros["idMensagem"].ValorInt;

            try
            {
                var objMensagem = EmailHotmail.LoadObject(idMensagem.Value);

                var objMensagemEmail = new MensagemPlugin.MensagemEmail
                {
                    Assunto = objMensagem.DescricaoAssunto,
                    From = objMensagem.EmailRemetente,
                    To = objMensagem.EmailDestinatario,
                    IdMensagem = idMensagem.Value,
                    Saida = MensagemPlugin.MensagemEmail.Provider.AmazonSES
                };

                if (objMensagem.DescricaoParametros != null)
                {
                    ExpandoObject parametros = XMLToDynamic(objMensagem.DescricaoParametros);
                    var mensagem = parametros.ToString(objMensagem.DescricaoMensagem);
                    objMensagemEmail.Descricao = mensagem;
                }
                else
                {
                    objMensagemEmail.Descricao = objMensagem.DescricaoMensagem;
                }

                return new MensagemPlugin(this, new List<MensagemPlugin.MensagemEmail> { objMensagemEmail }, false);
            }
            catch (Exception ex)
            {
                Core.LogError(ex);
            }

            return new MensagemPlugin(this, true);
        }
        #endregion

        #region XMLToDynamic
        /// <summary>
        ///     Carrega a coleção a partir de uma string XML
        /// </summary>
        /// <param name="xml">A string XML</param>
        /// <returns>A coleção carregada</returns>
        public static dynamic XMLToDynamic(XmlDocument xml)
        {
            var obj = new ExpandoObject();

            var nodes = xml.DocumentElement.SelectNodes("//parametros//*");
            foreach (XmlNode node in nodes)
                ((IDictionary<string, object>) obj)[node.Name] = node.InnerText;

            return obj;
        }
        #endregion
    }
}