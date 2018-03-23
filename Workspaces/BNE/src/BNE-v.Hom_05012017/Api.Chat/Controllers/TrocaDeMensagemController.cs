using BNE.Chat.Core;
using BNE.Chat.Core.Notification;
using BNE.Web.Code;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Api.Chat.Controllers
{
    public class TrocaDeMensagemController : ApiController
    {
        /// <summary>
        /// Tanque chama BNE
        /// </summary>
        /// <param name="Mensagem">Conteúdo da Mensagem</param>
        /// <param name="NumeroCelular">Celular que recebeu mensagem da selecionadora e está enviando.</param>
        /// <param name="CodigoUsuario"> Idf Usuario Filial Perfil </param>
        /// <param name="CodigoDestinatario"> Idf Curriculo</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage EnviaMensagemParaChat(string Guid, string Mensagem, decimal NumeroCelular, int CodigoUsuario, int CodigoDestinatario)
        {
            HttpStatusCode enviaMensagemParaChat;
            if (!ArgumentosEnvioChatValido(Guid, Mensagem, NumeroCelular, CodigoUsuario, CodigoDestinatario, out enviaMensagemParaChat))
                return new HttpResponseMessage(enviaMensagemParaChat);

            try
            {
                var not = ChatService.Instance.NotificationComponent;
                if (not != null)
                {
                    not.NotificationMediator.RaiseNotificationEntry(new NotificationEventArgs
                    {
                        PartyWith = CodigoDestinatario,
                        OwnerId = CodigoUsuario
                    });
                }
            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex,
                   string.Format(
                       "#Chat #Notificacao Guid='{0}' | Mensagem='{1}' | NumeroCelular='{2}' | CodigoUsuario='{3}' | CodigoDestinatario='{4}'",
                       Guid ?? string.Empty, Mensagem ?? string.Empty, NumeroCelular, CodigoUsuario, CodigoDestinatario));

                Trace.WriteLine(ex);
            }

            try
            {
                var producer = ChatService.Instance.GetChatProducer();
                ((BNEChatProducer)producer).EnviarMensagemChat(new BNE.Chat.DTO.MensagemChatDTO
                {
                    GuidMessage = Guid,
                    Mensagem = Mensagem,
                    NumeroCelular = NumeroCelular.ToString(CultureInfo.InvariantCulture),
                    UsuarioFilialPerfilId = CodigoUsuario,
                    CurriculoId = CodigoDestinatario,
                    EscritoPorUsuarioFilialPerfil = false,
                });
            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex,
                    string.Format(
                        "#Chat #EnvioMensagem Guid='{0}' | Mensagem='{1}' | NumeroCelular='{2}' | CodigoUsuario='{3}' | CodigoDestinatario='{4}'",
                        Guid ?? string.Empty, Mensagem ?? string.Empty, NumeroCelular, CodigoUsuario, CodigoDestinatario));

                Trace.WriteLine(ex);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private static bool ArgumentosEnvioChatValido(string guid, string mensagem, decimal numeroCelular, int codigoUsuario,
                                                int codigoDestinatario, out HttpStatusCode enviaMensagemParaChat)
        {
            if (string.IsNullOrWhiteSpace(guid))
            {
                enviaMensagemParaChat = HttpStatusCode.BadRequest;
                return false;
            }
            if (codigoUsuario <= 0)
            {
                enviaMensagemParaChat = HttpStatusCode.BadRequest;
                return false;
            }

            if (numeroCelular <= 0)
            {
                // todo log
                {
                    enviaMensagemParaChat = HttpStatusCode.BadRequest;
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(mensagem))
            {
                // todo log
                {
                    enviaMensagemParaChat = HttpStatusCode.NoContent;
                    return false;
                }
            }

            if (codigoDestinatario <= 0)
            {
                enviaMensagemParaChat = HttpStatusCode.BadRequest;
                return false;
            }

            enviaMensagemParaChat = HttpStatusCode.OK;
            return true;
        }

        /// <summary>
        /// Tanque chama BNE
        /// </summary>
        /// <param name="Guid">Código da Mensagem</param>
        /// <param name="Status">Resultado da Mensagem</param>
        /// <param name="CodigoUsuario">Idf Usuario Filial Perfil </param>
        [HttpGet]
        public HttpResponseMessage AlteraStatusMensagem(string Guid, int Status, int CodigoUsuario, int CodigoDestinatario)
        {
            HttpStatusCode confirmacaoDeEntregueNoCelular;
            if (!ArgumentosEntregueNoCelularValido(Guid, Status, CodigoUsuario, out confirmacaoDeEntregueNoCelular))
                return new HttpResponseMessage(confirmacaoDeEntregueNoCelular);

            try
            {

                var producer = ChatService.Instance.GetChatProducer();
                ((BNEChatProducer)producer).AtualizarStatusMensagemChat(new BNE.Chat.DTO.StatusMensagemChatDTO
                {
                    GuidMensagem = Guid,
                    TipoStatus = Status,
                    UsuarioFilialPerfilId = CodigoUsuario,
                    CurriculoId = CodigoDestinatario,
                    StatusData = DateTime.Now
                });

            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex,
                    string.Format("#Chat Guid='{0}' | Status='{1}' | CodigoUsuario='{2}' | CodigoDestinatario='{3}'",
                        Guid ?? string.Empty, Status, CodigoUsuario, CodigoDestinatario));

                Trace.WriteLine(ex);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private static bool ArgumentosEntregueNoCelularValido(string guidMensagem, int status, int codigoUsuario,
                                                           out HttpStatusCode confirmacaoDeEntregueNoCelular)
        {
            if (string.IsNullOrWhiteSpace(guidMensagem))
            {
                {
                    confirmacaoDeEntregueNoCelular = HttpStatusCode.NoContent;
                    return false;
                }
            }

            if (status <= 0)
            {
                {
                    confirmacaoDeEntregueNoCelular = HttpStatusCode.BadRequest;
                    return false;
                }
            }

            if (codigoUsuario <= 0)
            {
                {
                    confirmacaoDeEntregueNoCelular = HttpStatusCode.BadRequest;
                    return false;
                }
            }
            confirmacaoDeEntregueNoCelular = HttpStatusCode.OK;
            return true;
        }
    }
}
