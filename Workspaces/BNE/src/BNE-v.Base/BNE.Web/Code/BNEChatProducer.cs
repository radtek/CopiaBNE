using System.Collections.Generic;
using System.Globalization;
using BNE.BLL;
using BNE.Chat.Core;
using BNE.Chat.Core.Base;
using BNE.Chat.Core.Interface;
using BNE.Chat.Core.Notification;
using BNE.Chat.Helper;
using BNE.Web.Api;
using System;
using System.Linq;
using BNE.Chat.Model;

namespace BNE.Web.Code
{
    public class BNEChatProducer : BNEChatProducerBase
    {
        #region [ Constructor ]
        public BNEChatProducer(IChatListener manager, ChatStore chatStore, NotificationHandler handler)
            : base(manager, chatStore, handler)
        {
        }
        #endregion

        #region [ Public Methods ]
        public void AtualizarStatusMensagemChatOutrasConexoes(StatusMensagemChatDTO statusMensagemChatDTO, string conexaoDeEnvio)
        {
            AtualizarHistorico(statusMensagemChatDTO);

            IBuffer<string> connections;
            if (!PegarConexoesAtivasNoChat(statusMensagemChatDTO.UsuarioFilialPerfilId, out connections))
                return;

            try
            {
                IEnumerable<string> outrasConexoes = connections.Where(obj => obj != conexaoDeEnvio);
                foreach (var con in GetHubContext().CreatePrivateSelectionProxy(outrasConexoes))
                {
                    con.SendStatusOfMessage(new { statusMensagemChatDTO.GuidMensagem, Status = statusMensagemChatDTO.TipoStatus });
                }
            }
            finally
            {
                connections.Dispose();
            }

        }

        public void AtualizarStatusMensagemChat(StatusMensagemChatDTO statusMensagemChatDTO)
        {
            AtualizarHistorico(statusMensagemChatDTO);

            IBuffer<string> connections;
            if (!PegarConexoesAtivasNoChat(statusMensagemChatDTO.UsuarioFilialPerfilId, out connections))
                return;

            try
            {
                foreach (var con in GetHubContext().CreatePrivateSelectionProxy(connections))
                {
                    con.SendStatusOfMessage(
                        new
                        {
                            statusMensagemChatDTO.GuidMensagem,
                            statusMensagemChatDTO.TipoStatus,
                            statusMensagemChatDTO.StatusData,
                            statusMensagemChatDTO.GuidChat,
                            statusMensagemChatDTO.CurriculoId
                        });
                }
            }
            finally
            {
                connections.Dispose();
            }

        }

        public void EnviarMensagemChatOutrasConexoes(MensagemChatDTO mensagemChatDTO, string conexaoDeEnvio)
        {
            var history = ChatStore.AddPrivateMessage(mensagemChatDTO.CurriculoId,
                                                      mensagemChatDTO.UsuarioFilialPerfilId,
                                                      mensagemChatDTO.GuidMessage, mensagemChatDTO.EscritoPorUsuarioFilialPerfil);

            if (history.Value != null)
            {
                history.Value.MessageContent = mensagemChatDTO.Mensagem;
                history.Value.CreationDate = mensagemChatDTO.CriacaoData;
                history.Value.StatusDate = mensagemChatDTO.StatusData;
                history.Value.StatusTypeValue = mensagemChatDTO.TipoStatus;
            }
            mensagemChatDTO.GuidChat = history.Key.GuidChat;

            IBuffer<string> connections;
            if (!PegarConexoesAtivasNoChat(mensagemChatDTO.UsuarioFilialPerfilId, out connections))
                return;

            try
            {
                Tuple<int, string, string> pessoaFisicaInfo;
                if (CarregarOutrasInformacoesParaMensagem(mensagemChatDTO.CurriculoId, out pessoaFisicaInfo))
                {
                    IEnumerable<string> outrasConexoes = connections.Where(obj => obj != conexaoDeEnvio);
                    EnviarParaJanelasDeChat(mensagemChatDTO, pessoaFisicaInfo, outrasConexoes);
                }
            }
            finally
            {
                connections.Dispose();
            }
        }

        public void EnviarMensagemChat(MensagemChatDTO mensagemChatDto)
        {
            var chat = ChatStore.GetOrAddHistory(mensagemChatDto.UsuarioFilialPerfilId, mensagemChatDto.CurriculoId);
            chat.AddMessage(mensagemChatDto.GuidMessage,
                                     mensagemChatDto.EscritoPorUsuarioFilialPerfil, mensagemChatDto.Mensagem,
                                     mensagemChatDto.CriacaoData, mensagemChatDto.StatusData, mensagemChatDto.TipoStatus);

            mensagemChatDto.GuidChat = chat.GuidChat;

            IBuffer<string> connections;
            if (!PegarConexoesAtivasNoChat(mensagemChatDto.UsuarioFilialPerfilId, out connections))
                return;

            try
            {
                Tuple<int, string, string> pessoaFisicaInfo;
                CarregarOutrasInformacoesParaMensagem(mensagemChatDto.CurriculoId, out pessoaFisicaInfo);
                // todo review (to do) revisar se não encontrar curriculoId 
                EnviarParaJanelasDeChat(mensagemChatDto, pessoaFisicaInfo, connections);
            }
            finally
            {
                connections.Dispose();
            }

        }
        #endregion

        #region [ Private Methods ]
        private bool PegarConexoesAtivasNoChat(int usuarioFilialPerfil, out IBuffer<string> connections)
        {
            var session = ChatStore.GetSession(usuarioFilialPerfil);
            if (session == null)
            {
                connections = Enumerable.Empty<string>().Memoize();
                return false;
            }

            connections = ChatStore.GetConnections(session).Memoize();
            if (!connections.Any())
            {
                connections = Enumerable.Empty<string>().Memoize();
                return false;
            }

            return true;
        }

        private void EnviarParaJanelasDeChat(MensagemChatDTO mensagemChatDTO, Tuple<int, string, string> pfExtraInfo, IEnumerable<string> conexoes)
        {
            foreach (var con in GetHubContext().CreatePrivateSelectionProxy(conexoes))
            {
                con.SendMessage(
                    new
                        {
                            GuidMensagem = mensagemChatDTO.GuidMessage,
                            mensagemChatDTO.GuidChat,
                            mensagemChatDTO.CurriculoId,
                            PessoaFisicaId = pfExtraInfo.Item1,
                            PessoaFisicaNome = pfExtraInfo.Item2,
                            PessoaFisicaCelular = mensagemChatDTO.NumeroCelular != null ? mensagemChatDTO.NumeroCelular.ToString(CultureInfo.InvariantCulture) : pfExtraInfo.Item3,
                            Conteudo = mensagemChatDTO.Mensagem,
                            mensagemChatDTO.TipoStatus,
                            mensagemChatDTO.CriacaoData,
                            mensagemChatDTO.StatusData,
                            mensagemChatDTO.EscritoPorUsuarioFilialPerfil
                        });
            }
        }

        private void AtualizarHistorico(StatusMensagemChatDTO statusMensagemChatDTO)
        {
            using (var cacheHistory = ChatStore.GetHistorical(statusMensagemChatDTO.UsuarioFilialPerfilId).Memoize())
            {
                Func<PrivateHistoryChat, bool> finder;
                if (!string.IsNullOrWhiteSpace(statusMensagemChatDTO.GuidChat))
                {
                    finder = a => a.GuidChat == statusMensagemChatDTO.GuidChat;
                }
                else
                {
                    finder = a => a.PartyWith == statusMensagemChatDTO.CurriculoId;
                }

                var relative = cacheHistory.FirstOrDefault(finder);

                PrivateHistoryMessage message;
                if (relative == null)
                {
                    var res =
                        cacheHistory.SelectMany(a => a.GetMessageHistory().Select(b => new { a = a.GuidChat, b }))
                                    .FirstOrDefault(a => a.b.Guid == statusMensagemChatDTO.GuidMensagem);
                    if (res == null)
                    {
                        message = null;
                    }
                    else
                    {
                        statusMensagemChatDTO.GuidChat = res.a;
                        message = res.b;
                    }
                }
                else
                {
                    message = relative.GetMessageHistory().FirstOrDefault(a => a.Guid == statusMensagemChatDTO.GuidMensagem);
                    statusMensagemChatDTO.GuidChat = relative.GuidChat;
                }

                if (message == null)
                    return;

                message.StatusTypeValue = statusMensagemChatDTO.TipoStatus;
                message.StatusDate = DateTime.Now;
            }
        }
        #endregion

        #region [ Static ]
        public static readonly SetValueOrDefaultFact<HardConfig<int>, int> CacheInformacoesCurriculoParaChatEmMinutos =
            new HardConfig<int>("chat_cache_informacoes_do_curriculo_para_funcionamento_em_minutos", 15).Wrap(
                a => a.Value);

        private static readonly CooperativeTimeCache<int, Tuple<string, string>> CacheInformacoesCurriculo =
            new CooperativeTimeCache<int, Tuple<string, string>>(() =>
                TimeSpan.FromMinutes(CacheInformacoesCurriculoParaChatEmMinutos.Value));

        public static bool CarregarOutrasInformacoesParaMensagem(int curriculoId, out Tuple<int, string, string> totalResult)
        {
            var value = CacheInformacoesCurriculo.GetOrAdd(curriculoId, keyFactory =>
            {
                try
                {
                    var curriculo = new Curriculo(keyFactory);
                    var pessoaFisicaId = PessoaFisica.RecuperarIdPorCurriculo(curriculo);

                    var objPf = new PessoaFisica(pessoaFisicaId);
                    var res = objPf.RecuperarNomeECelularPessoa();

                    string nome;
                    string celular;
                    if (string.IsNullOrWhiteSpace(res.Key))
                    {
                        objPf = PessoaFisica.LoadObject(pessoaFisicaId);
                        if (objPf != null)
                        {
                            nome = objPf.NomePessoa;
                            celular = objPf.NumeroDDDCelular + objPf.NumeroCelular;
                        }
                        else
                        {
                            nome = string.Empty;
                            celular = string.Empty;
                        }
                    }
                    else
                    {
                        nome = res.Key;
                        celular = res.Value;
                    }
                    return new Tuple<string, string>(nome, celular);
                }
                catch (Exception ex)
                {
                    //todo log (to do)
                    return new Tuple<string, string>(string.Empty, string.Empty);
                }

            });

            totalResult = new Tuple<int, string, string>(curriculoId, value.Item1, value.Item2);
            if (string.IsNullOrWhiteSpace(value.Item1) && string.IsNullOrWhiteSpace(value.Item2))
            {
                return false;
            }
            return true;
        }
        #endregion

    }
}