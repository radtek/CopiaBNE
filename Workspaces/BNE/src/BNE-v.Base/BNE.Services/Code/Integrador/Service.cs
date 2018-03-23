using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BNE.BLL;
using BNE.Services.Code.Integrador.Estrutura;

namespace BNE.Services.Code.Integrador
{
    public class Service
    {
        #region EfetuarRequisicao
        /// <summary>
        /// Método antigo, usado no serviço sincrono de importação de vagas. Será apagado assim que o assincrono esteja estavel
        /// </summary>
        public static IEnumerable<VagaIntegracao> EfetuarRequisicao(BLL.Integrador objIntegrador)
        {
            //Em caso de novos layouts, implementar nova estrutura e adicionar a chamada aqui
            switch (objIntegrador.TipoIntegrador)
            {
                case BLL.Enumeradores.TipoIntegrador.Trovit:
                    return new Trovit.Response(objIntegrador).RecuperarVagas();
                case BLL.Enumeradores.TipoIntegrador.VisualWebRipper:
                    return new VisualWebRipper().RecuperarVagas(objIntegrador);
                case BLL.Enumeradores.TipoIntegrador.BNE:
                    return new BNE.Services.Code.Integrador.Estrutura.Bne().RecuperarVagas(objIntegrador);
                default:
                    break;
            }
            return null;           
        }
        #endregion

        #region EfetuarRequisicaoAssincrona
        /// <summary>
        /// Métodos de importação de vagas assincrono. Realiza a busca dos atributos de vaga nos XMLs. Não carrega nenhum objeto, apenas retira os dados do XML.
        /// As vagas do SINE não são mais importadar por aqui (via XML), agora são importadas via Web Service.
        /// </summary>
        public static IEnumerable<VagaIntegracao> EfetuarRequisicaoAssincrona(BLL.Integrador objIntegrador)
        {
            //Em caso de novos layouts, implementar nova estrutura e adicionar a chamada aqui
            switch (objIntegrador.TipoIntegrador)
            {
                case BLL.Enumeradores.TipoIntegrador.Trovit:
                    return new Estrutura.Assincrono.Trovit.Response(objIntegrador).RecuperarVagas();
                case BLL.Enumeradores.TipoIntegrador.VisualWebRipper:
                    return new Estrutura.Assincrono.Ripper().RecuperarVagas(objIntegrador);
                default:
                    break;
            }
            return null;
        }
        #endregion
    }
}
