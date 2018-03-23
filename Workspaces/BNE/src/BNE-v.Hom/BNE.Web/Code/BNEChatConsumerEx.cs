using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Web;
using BNE.BLL.BNETanqueService;
using BNE.BLL.Custom;
using BNE.Chat.Core;
using BNE.Chat.Core.EventModel;
using BNE.Chat.Core.Hubs;
using BNE.Chat.Core.Interface;
using BNE.Chat.Core.Notification;
using BNE.Chat.Helper;
using BNE.Chat.Model;
using BNE.Web.UserControls;
using System.Web.UI;
using BNE.BLL.Common;

namespace BNE.Web.Code
{
    public partial class BNEChatConsumer : BNEChatConsumerBasic
    {
        public static readonly SetValueOrDefaultFact<HardConfig<int>, int> ContactHistoryLimit =
            new SetValueOrDefaultFact<HardConfig<int>, int>(
            new HardConfig<int>("chat_contact_history_date_limit_in_days", 7), a => a.Value);

        public static readonly SetValueOrDefaultFact<HardConfig<string>, string> DashboardLinkTemplate =
            new SetValueOrDefaultFact<HardConfig<string>, string>(
            new HardConfig<string>("chat_dashboard_link_template", @"http://dashboard.apprh.com.br/dashboard/redirecionar?identificacao={0}"), a => a.Value);

        private static readonly SetValueOrDefaultFact<HardConfig<int>, int> CacheInformacoesExtrasCurriculoEmMinutos =
            new HardConfig<int>("chat_cache_mais_informacoes_do_curriculo_em_minutos", 10).Wrap(a => a.Value);

        private static readonly CooperativeTimeCache<int, BLL.DTO.Curriculo> CacheInfo =
            new CooperativeTimeCache<int, BLL.DTO.Curriculo>(() =>
                TimeSpan.FromMinutes(CacheInformacoesExtrasCurriculoEmMinutos.Value));

        private readonly CooperativeTimeCache<int, OutConversaAtiva> _onlineContacts =
            new CooperativeTimeCache<int, OutConversaAtiva>(() => TimeSpan.FromMinutes(CacheRequestOnlineContactsTime.Value));


        public override string GetDashboardLink()
        {
            var value = DashboardLinkTemplate.Value ?? string.Empty;
            if (value.Contains("{0}"))
            {
                return string.Format(value, GetUniqueIdentifierUnsafe());
            }
            return string.Empty;
        }

        public override int GetUsuarioFilialPerfil()
        {
            return GetUniqueIdentifierUnsafe();
        }

        protected override void NewMessage(EventPattern<ChatResultEventArgs> requestParams)
        {
            int user;
            if (!IsValidToProcessNewMessage(requestParams, out user))
                return;

            dynamic callParams = requestParams.EventArgs.CallParams;
            var currentConnectionId = requestParams.EventArgs.Hub.Context.ConnectionId;

            try
            {
                var message = callParams["message"];
                var chat = callParams["chat"];

                if (!ValidateNewMessageRequest(message, chat))
                {
                    var tcs = new TaskCompletionSource<ISignalRGenericResult>();
                    tcs.SetResult(new SignalRGenericResult<object>(new { Status = 400 })); //bad request
                    requestParams.EventArgs.TaskValueResult = tcs.Task;
                    return;
                }

                var value = ((object)message["content"]).ToString();
                if (value.Length > MessageLengthLimit.Value)
                {
                    var tcs = new TaskCompletionSource<ISignalRGenericResult>();
                    tcs.SetResult(new SignalRGenericResult<object>(new { Status = 417 })); //Expectation Failed
                    requestParams.EventArgs.TaskValueResult = tcs.Task;
                    return;
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "#Chat Rethrow exception | HTTP Status 400 (Bad Request).");
                var tcs = new TaskCompletionSource<ISignalRGenericResult>();
                tcs.SetResult(new SignalRGenericResult<object>(new { Status = 400 })); // 400  Bad Request
                requestParams.EventArgs.TaskValueResult = tcs.Task;
                return;
            }

            int curriculoId = Convert.ToInt32(callParams["chat"]["targetId"]);

            _notificationHandler.NotificationMediator.RaiseNotificationRead(new NotificationEventArgs
            {
                PartyWith = curriculoId,
                OwnerId = user
            });

            DateTime sendDate = DateTime.Now;
            // todo review (to do) revisar se não encontrar curriculoId 
            Tuple<int, string, string> moreInfo;
            BNEChatProducer.CarregarOutrasInformacoesParaMensagem(curriculoId, out moreInfo);

            var t1 = Task.Factory.StartNew<ISignalRGenericResult>(() =>
            {
                Func<string, decimal> converToDecimalSafe = a => { decimal value; if (decimal.TryParse(a, out value)) return value; return 0; };

                var inEnviaSmsResposta = new InEnviaSMSResposta
                {
                    c = user.ToString(CultureInfo.InvariantCulture),
                    m = callParams["message"]["content"],
                    cd = curriculoId.ToString(CultureInfo.InvariantCulture),
                    n = converToDecimalSafe(moreInfo.Item3),
                    np = moreInfo.Item2
                };

                try
                {
                    OutEnviaSMSCelular resEnviaResposaSms;
                    using (var client = new BNE.BLL.BNETanqueService.AppClient())
                    {
                        resEnviaResposaSms = client.EnviaSMSResposta_New(inEnviaSmsResposta);
                    }

                    if (resEnviaResposaSms == null)
                    {
                        var info =
                            string.Format(
                                "UsuarioFilialPerfilId='{0}' Conteudo='{1}' CurriculoId='{2}' PFTelefone='{3}' PFNome='{4}'",
                                user.ToString(CultureInfo.InvariantCulture),
                                callParams["chat"]["content"],
                                curriculoId.ToString(CultureInfo.InvariantCulture),
                                converToDecimalSafe(moreInfo.Item3),
                                moreInfo.Item2);

                        EL.GerenciadorException.GravarExcecao(new NullReferenceException("resEnviaResponsaSms"),
                            "#Chat HTTP Status 500 (Internal Error). Retorno inválido de 'EnviaSMSResposta' | Infomações: " + info);

                        return new SignalRGenericResult<object>(new { Status = 500 });
                    }

                    return new SignalRGenericResult<object>(new
                    {
                        Id = resEnviaResposaSms.id,
                        Status = resEnviaResposaSms.s
                    });
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex, "#Chat HTTP Status 503 (Service Unavailable).");
                    return new SignalRGenericResult<object>(new { Status = 503 });
                }
            });

            t1.ContinueWith(t =>
            {
                string idMessage;
                try
                {
                    dynamic res = t.Result.Value;
                    if (res.Status == 500 || res.Status == 503 || res.Status == 400 || res.Status == 401)
                        return;

                    idMessage = res.Id.ToString();
                }
                catch
                {
                    idMessage = callParams["message"]["guid"];
                }

                Func<string, int> converToIntSafe = a =>
                {
                    int value;
                    if (int.TryParse(a, out value))
                        return value;

                    return 0;
                };

                var msg = (BNEChatProducer)ChatService.Instance.GetChatProducer();
                msg.EnviarMensagemChatOutrasConexoes(new BNE.Chat.DTO.MensagemChatDTO
                {
                    UsuarioFilialPerfilId = user,
                    EscritoPorUsuarioFilialPerfil = true,
                    CriacaoData = sendDate,
                    GuidChat = callParams["chat"]["guid"],
                    GuidMessage = idMessage,
                    Mensagem = callParams["message"]["content"],
                    CurriculoId = curriculoId,
                    Nome = moreInfo.Item2,
                    NumeroCelular = moreInfo.Item3,
                    TipoStatus = converToIntSafe(t.Result.Value.ToString()),
                    StatusData = DateTime.Now

                }, currentConnectionId);

            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            requestParams.EventArgs.TaskValueResult = t1;
        }

        private bool IsValidToProcessNewMessage(EventPattern<ChatResultEventArgs> requestParams, out int user)
        {
            user = Convert.ToInt32(HttpContext.Current.Request.QueryString["ufp"]);// GetUniqueIdentifierUnsafe();
            if (user <= 0)
            {
                var tcs = new TaskCompletionSource<ISignalRGenericResult>();
                tcs.SetResult(new SignalRGenericResult<object>(new { Status = 401 })); // 401 Unauthorized
                requestParams.EventArgs.TaskValueResult = tcs.Task;
                return false;
            }

            if (requestParams.EventArgs.CallParams == null)
            {
                var tcs = new TaskCompletionSource<ISignalRGenericResult>();
                tcs.SetResult(new SignalRGenericResult<object>(new { Status = 400 })); // 400  Bad Request
                requestParams.EventArgs.TaskValueResult = tcs.Task;
                return false;
            }
            return true;
        }

        private static bool ValidateNewMessageRequest(dynamic message, dynamic chat)
        {
            Func<object, bool> validTargetId = arg =>
            {
                if (arg is int)
                    return true;

                int targetId;
                if (Int32.TryParse(arg.ToString(), out targetId))
                    return true;

                return false;
            };

            if (message == null || chat == null)
                return false;

            try
            {
                if (message["guid"] == null
            || message["content"] == null
            || message["owner"] == null
            || chat["targetId"] == null
            || chat["guid"] == null
            || !validTargetId(chat["targetId"]))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        protected override void NewReadNotification(EventPattern<ChatDefaultEventArgs> requestParams)
        {
            var user = Convert.ToInt32(HttpContext.Current.Request.QueryString["ufp"]);// GetUniqueIdentifierUnsafe();
            if (user <= 0)
                return;

            var dyn = requestParams.EventArgs.CallParams;

            if (dyn == null)
                return;

            dynamic dynObj = requestParams.EventArgs.CallParams;
            int partyWith;
            try
            {
                var chatGuid = dynObj["guid"] != null ? dynObj["guid"].ToString() : null;
                var chatTargetId = dynObj["targetId"] != null ? dynObj["targetId"].ToString() : null;

                if (chatGuid == null && chatTargetId == null)
                    return;

                using (var currentHist = ChatStore.GetHistorical(user).Memoize())
                {
                    if (!Int32.TryParse(chatTargetId, out partyWith)
                        || partyWith <= 0
                        || currentHist.All(obj => obj.PartyWith != partyWith))
                    {
                        var partner = currentHist.FirstOrDefault(a => a.GuidChat == chatGuid);
                        if (partner == null)
                            return;

                        partyWith = partner.PartyWith;
                    }
                }

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "#Chat Rethrow exception | HTTP Status 400 (Bad Request).");

                var tcs = new TaskCompletionSource<ISignalRGenericResult>();
                tcs.SetException(ex);
                requestParams.EventArgs.TaskResult = tcs.Task;
                return;
            }

            _notificationHandler.NotificationMediator.RaiseNotificationRead(new NotificationEventArgs
            {
                PartyWith = partyWith,
                OwnerId = user
            });
        }

        protected override void GetMoreInfo(EventPattern<ChatResultEventArgs> obj)
        {
            var userId = GetUniqueIdentifierUnsafe();
            if (userId <= 0)
                return;

            if (obj.EventArgs.CallParams == null)
                return;

            int IdCurriculo;
            try
            {
                dynamic arguments = obj.EventArgs.CallParams;
                var valueId = arguments["targetId"];
                if (valueId == null)
                    return;

                if (!Int32.TryParse(valueId.ToString(), out IdCurriculo))
                    return;

                if (IdCurriculo <= 0)
                    return;
            }
            catch (Exception)
            {
                return;
            }

            obj.EventArgs.TaskValueResult = Task.Factory.StartNew<ISignalRGenericResult>(() =>
                {
                    if (IdCurriculo <= 0)
                        return null;

                    var maisInfo = @"
                        <div style='float: left; height: 48px; width: 225px;margin-left: 4px;'>
                            <span>Telefone:</span>
                            <span ID='lblTelefoneCurriculo'>{telefone}</span>
                            <br />
                            <span>Função:</span>
                            <span ID='lblFuncaoCurriculo' style='font-weight: bold;' >{funcao}</span>
                            <br />
                            <span style='font-size: 8pt;'>Cód. Currículo:</span>
                            <span ID='lblIdCurriculo' style='font-size: 11px;' >{IdCurriculo}</span>
                            <br />
                        </div>
                    ";

                    var curDto = CacheInfo.GetOrAdd(IdCurriculo, keyFactory =>
                    {
                        var res = BNE.BLL.Curriculo.CarregarCurriculoDTO(keyFactory, BNE.BLL.Curriculo.DadosCurriculo.FuncoesPretendidas);
                        return res;
                    });

                    if (curDto == null)
                        return null;

                    maisInfo = maisInfo.Replace("{telefone}", PegarTelefone(curDto));
                    maisInfo = maisInfo.Replace("{funcao}", PegarFuncao(curDto));
                    maisInfo = maisInfo.Replace("{IdCurriculo}", IdCurriculo.ToString());

                    return new SignalRGenericResult<string>(maisInfo.Trim());
                });
        }

        private string PegarTelefone(BLL.DTO.Curriculo curDto)
        {
            var celularTexto = (curDto.NumeroCelular ?? "").Trim();

            if (celularTexto.Length > 4)
            {
                if (celularTexto.Length > 8)
                {
                    celularTexto = new string(celularTexto.Take(5).ToArray()) + "-" +
                                   new string(celularTexto.Skip(5).ToArray());
                }
                else
                {
                    celularTexto = new string(celularTexto.Take(4).ToArray()) + "-" +
                                  new string(celularTexto.Skip(4).ToArray());
                }
            }

            return string.Format("({0}) {1}", curDto.NumeroDDDCelular ?? "", celularTexto);
        }

        private string PegarFuncao(BLL.DTO.Curriculo curDto)
        {
            if (curDto.FuncoesPretendidas == null)
                return "";

            var funcao = curDto.FuncoesPretendidas.FirstOrDefault();

            if (funcao == null)
                return "";

            return funcao.NomeFuncaoPretendida ?? "";
        }

        protected override void RequestOnlineContacts(EventPattern<ChatResultEventArgs> obj)
        {
            var user = Convert.ToInt32(HttpContext.Current.Request.QueryString["ufp"]);// GetUniqueIdentifierUnsafe();
            if (user <= 0)
                return;

            obj.EventArgs.TaskValueResult = Task.Factory.StartNew(() =>
            {
                OutConversaAtiva res;
                try
                {
                    res = _onlineContacts.GetOrAdd(user, keyFact =>
                        {
                            return BuiltExternalOnlineContactsResult(keyFact);
                        });
                }
                catch (Exception ex)
                {
                    _onlineContacts.TryRemove(user, out res);
                    EL.GerenciadorException.GravarExcecao(ex, "#Chat HTTP Status 503 (Service Unavailable).");
                    return new SignalRGenericResult<object>(503); //503 Service Unavailable
                }

                if (res == null
                    || res.l == null
                    || res.l.Length == 0)
                {
                    return GenerateOnlineContactsResult(user, new Mensagem[0]);
                }

                return GenerateOnlineContactsResult(user, res.l);
            });

        }

        private OutConversaAtiva BuiltExternalOnlineContactsResult(int keyFact)
        {
            OutConversaAtiva produced;
            using (var client = new AppClient())
            {
                produced =
                    client.ConversasAtivas_New(new InConversaAtiva
                    {
                        cUsu = keyFact.ToString(CultureInfo.InvariantCulture),
                        dIn = DateTime.Now.Date.AddDays(-ContactHistoryLimit.Value)
                    });

                if (produced == null || produced.l.Length == 0)
                {
                    produced = client.ConversasAtivas_New(new InConversaAtiva
                    {
                        cUsu = keyFact.ToString(CultureInfo.InvariantCulture),
                        qtd = 2
                    });

                    if (produced != null && produced.l != null)
                    {
                        if (produced.l.Length == 2 && produced.l.First().dcm.Date == produced.l.Last().dcm.Date)
                        {
                            produced = client.ConversasAtivas_New(new InConversaAtiva
                            {
                                cUsu = keyFact.ToString(CultureInfo.InvariantCulture),
                                dIn = produced.l.First().dcm.Date
                            });
                        }
                        else
                        {
                            var unique = produced.l.Where(a => a != null).OrderByDescending(a => a.dcm).FirstOrDefault();
                            if (unique != null)
                                produced.l = new[] { unique };
                        }
                    }
                }

                if (produced == null
                    || produced.l == null)
                {
                    return produced;
                }

            }

            Func<string, int> convertToId = arg => { int targetId; Int32.TryParse(arg, out targetId); return targetId; };
            var res = BLL.ConversasAtivas.RemoverConversasInativas(keyFact,
                                                        produced.l.Where(a => a != null).Select(a => new KeyValuePair<int, DateTime>(convertToId(a.ci), a.dcm))
                                                                    .Where(a => a.Key > 0), DateTime.Now.Date.AddDays(-ContactHistoryLimit.Value));

            produced.l = produced.l.Where(obj => res.Any(a => a == convertToId(obj.ci))).ToArray();

            return produced;
        }

        private ISignalRGenericResult GenerateOnlineContactsResult(int user, IEnumerable<Mensagem> lastMessagesOfContacts)
        {
            Func<string, int> convertToId = arg => { int targetId; Int32.TryParse(arg, out targetId); return targetId; };
            var privateHistoryChats = ChatStore.GetHistorical(user)
                .Where(a => (a.GetMessageHistory()
                    .Any(
                        obj =>
                            obj.StatusTypeValue != -2 || obj.CreatorType != PrivateHistoryMessage.MessageOwner.Self)));


            using (var validHist = privateHistoryChats.Memoize())
            {
                var dtoResult = new List<dynamic>();
                var currentWatched = new List<KeyValuePair<string, int>>();
                foreach (var item in lastMessagesOfContacts)
                {
                    int otherId = item.ci == null ? 0 : convertToId(item.ci);
                    if (otherId <= 0)
                        continue;

                    var existent = validHist.FirstOrDefault(a => a.PartyWith == otherId);

                    Tuple<string, string, DateTime, bool> info;
                    if (existent == null)
                    {
                        existent = ChatStore.GetOrAddHistory(user, otherId);
                        info = Tuple.Create(item.im.ToString(CultureInfo.InvariantCulture), item.dm, item.dcm, !item.fr);
                    }
                    else
                    {
                        var lastMessage = existent.GetMessageHistory().LastOrDefault();
                        if (lastMessage != null
                            && lastMessage.CreationDate > item.dcm)
                        {
                            info = Tuple.Create(lastMessage.Guid, lastMessage.MessageContent, lastMessage.CreationDate,
                                lastMessage.CreatorType == PrivateHistoryMessage.MessageOwner.Self);
                        }
                        else
                        {
                            info = Tuple.Create(item.im.ToString(CultureInfo.InvariantCulture), item.dm, item.dcm,
                                !item.fr);
                        }
                    }
                    // to do (invalid id)
                    Tuple<int, string, string> otherInfo;
                    if (!BNEChatProducer.CarregarOutrasInformacoesParaMensagem(existent.PartyWith, out otherInfo))
                        continue;

                    dtoResult.Add(new
                    {
                        GuidMensagem = info.Item1,
                        GuidChat = existent.GuidChat,
                        CurriculoId = existent.PartyWith,
                        PessoaFisicaNome = otherInfo.Item2,
                        PessoaFisicaCelular = otherInfo.Item3,
                        UltimaMensagem = info.Item2,
                        UltimaData = info.Item3,
                        Respondido = info.Item4,
                        Notificacao =
                            _notificationHandler.NotificationController.HasPendingNotification(user, existent.PartyWith)
                    });

                    currentWatched.Add(new KeyValuePair<string, int>(existent.GuidChat, existent.PartyWith));
                }

                var outsideOfOnlineContacts = (from s in validHist
                                               let find = currentWatched.Any(a => a.Key == s.GuidChat || (a.Value == s.PartyWith && a.Value != 0))
                                               where !find
                                               select s);

                outsideOfOnlineContacts.ForEach(obj =>
                {
                    Tuple<int, string, string> extraInfo;
                    if (!BNEChatProducer.CarregarOutrasInformacoesParaMensagem(obj.PartyWith, out extraInfo))
                        return;

                    var lastMessage = obj.GetMessageHistory().LastOrDefault();
                    dtoResult.Add(new
                    {
                        GuidMensagem = lastMessage == null ? "" : lastMessage.Guid,
                        GuidChat = obj.GuidChat,
                        CurriculoId = obj.PartyWith,
                        PessoaFisicaNome = extraInfo.Item2,
                        PessoaFisicaCelular = extraInfo.Item3,
                        UltimaMensagem = lastMessage == null ? "" : lastMessage.MessageContent,
                        UltimaData = lastMessage == null ? DateTime.Now : lastMessage.CreationDate,
                        Respondido =
                            lastMessage != null && lastMessage.CreatorType == PrivateHistoryMessage.MessageOwner.Self,
                        Notificacao =
                            _notificationHandler.NotificationController.HasPendingNotification(user, obj.PartyWith)
                    });
                });

                return
                    new SignalRGenericResult<object>(
                        dtoResult.OrderByDescending(a => a.UltimaData)
                            .ThenBy(a => a.GuidMensagem)
                            .Distinct(a => a.CurriculoId)
                            .ToArray());
            }
        }

        protected override void DeleteChat(EventPattern<ChatDefaultEventArgs> obj)
        {
            var user = Convert.ToInt32(HttpContext.Current.Request.QueryString["ufp"]); //GetUniqueIdentifierUnsafe();
            if (user <= 0)
                return;

            if (obj.EventArgs.CallParams == null)
                return;

            obj.EventArgs.TaskResult = Task.Factory.StartNew(() =>
            {
                dynamic dynObj = obj.EventArgs.CallParams;

                var chatTargetId = dynObj["targetId"];
                if (chatTargetId == null)
                    return;

                Func<string, int> parseInt = arg => { int value; Int32.TryParse(arg, out value); return value; };
                var targetId = parseInt(((object)chatTargetId).ToString());

                if (targetId <= 0)
                    return;

                OutConversaAtiva conversa;
                if (_onlineContacts.TryGetValue(user, out conversa))
                {
                    if (conversa.l != null)
                        conversa.l = conversa.l.Where(a => parseInt(a.ci) != targetId).ToArray();
                }
                BLL.ConversasAtivas.InativarConversa(user, targetId);
                ChatStore.RemoveHistory(user, targetId);

                var session = ChatStore.GetSession(user);
                if (session != null)
                {
                    var cons = ChatStore.GetConnections(session);
                    var res = cons.Where(a => a != obj.EventArgs.Hub.Context.ConnectionId);

                    foreach (var proxy in BNEChatClientProxyHelper.CreatePrivateSelectionProxy(Manager.GetHubContext().Clients, res))
                    {
                        proxy.SendDeleteContact(targetId);
                    }
                }
            });
        }

        protected override void RequestHistory(EventPattern<ChatResultEventArgs> requestParams)
        {
            RequestSpecificHistory(requestParams);
        }

        private void RequestSpecificHistory(EventPattern<ChatResultEventArgs> requestParams)
        {
            if (requestParams.EventArgs.CallParams == null)
                return;

            var user = Convert.ToInt32(HttpContext.Current.Request.QueryString["ufp"]);
            if (user <= 0)
                return;

            var call = requestParams.EventArgs.CallParams as IEnumerable<object>;
            if (call == null)
            {
                try
                {
                    dynamic dynObj = requestParams.EventArgs.CallParams;
                    var chatGuid = dynObj["guid"];
                    var chatTargetId = dynObj["targetId"];

                    if (chatGuid == null && chatTargetId == null)
                        return;

                    call = new[] { requestParams.EventArgs.CallParams };

                    var validItems =
                        call.Where(obj => ((dynamic)obj)["guid"] != null || ((dynamic)obj)["targetId"] != null);

                    if (!validItems.Any())
                    {
                        var tcs = new TaskCompletionSource<ISignalRGenericResult>();
                        tcs.SetResult(new SignalRGenericResult<object>());
                        requestParams.EventArgs.TaskValueResult = tcs.Task;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex, "#Chat HTTP Status 400 (Bad Request).");
                    var tcs = new TaskCompletionSource<ISignalRGenericResult>();
                    tcs.SetException(ex);
                    requestParams.EventArgs.TaskValueResult = tcs.Task;
                    return;
                }
            }

            // ReSharper disable PossibleMultipleEnumeration
            requestParams.EventArgs.TaskValueResult = Task.Factory.StartNew(() => GetSpecificHistory(call, user));
            // ReSharper restore PossibleMultipleEnumeration
        }

        private ISignalRGenericResult GetSpecificHistory(IEnumerable<object> call, int user)
        {
            var validItems =
                call.Where(obj => ((dynamic)obj)["guid"] != null || ((dynamic)obj)["targetId"] != null).ToArray();

            Func<object, string> convertStringSafe = arg => arg != null ? arg.ToString() : "";
            Func<object, int> parseInt = arg => { int value; Int32.TryParse(arg.ToString(), out value); return value; };
            Func<object, int> convertIntSafe = arg => arg != null ? arg is int ? (int)arg : parseInt(arg) : 0;

            var history = ChatStore.GetHistorical(user);
            var matchHistory = (from h in history
                                let res = (from v in validItems
                                           where h.GuidChat == convertStringSafe(((dynamic)v)["guid"])
                                                 || h.PartyWith == convertIntSafe(((dynamic)v)["targetId"])
                                           select v).FirstOrDefault()
                                let messages = h.GetMessageHistory().Memoize()
                                let onlyCampaign = messages.All(obj => obj.StatusTypeValue == -2 && obj.CreatorType == PrivateHistoryMessage.MessageOwner.Self)
                                where res != null && !onlyCampaign
                                select new { res, h, messages }).ToArray();

            var messageHistoryDto =
                matchHistory.Do(a => a.messages.Dispose()).Select(a => a.h).ToDictionary(a => a, b => b.GetMessageHistory());

            var unmatchHistory = validItems.ToArray();

            Dictionary<PrivateHistoryChat, IEnumerable<PrivateHistoryMessage>> newHistoryToManage;
            if (unmatchHistory.Length <= 0)
            {
                newHistoryToManage = new Dictionary<PrivateHistoryChat, IEnumerable<PrivateHistoryMessage>>();
            }
            else
            {
                var withoutHistory = GetHistoryFromService(user, unmatchHistory);
                newHistoryToManage = PopulateHistory(user, withoutHistory);
            }

            var total =
                messageHistoryDto.Concat(newHistoryToManage)
                    .SelectMany(
                        a =>
                        {
                            Tuple<int, string, string> info;
                            BNEChatProducer.CarregarOutrasInformacoesParaMensagem(a.Key.PartyWith, out info);

                            return a.Value.OrderBy(seq => seq.CreationDate).ThenBy(seq => seq.Guid).Select(
                                 obj =>
                                    ConverToMessageDto(a.Key, obj, info));
                        })
                    .ToArray();

            return new SignalRGenericResult<object>(total);
        }

        private Dictionary<PrivateHistoryChat, IEnumerable<PrivateHistoryMessage>> PopulateHistory(int user, Dictionary<int, IList<Mensagem>> withoutHistory)
        {
            var newHistoryToManage = new Dictionary<PrivateHistoryChat, IEnumerable<PrivateHistoryMessage>>();
            foreach (var item in withoutHistory)
            {
                var historyChat = ChatStore.GetOrAddHistory(user, item.Key);

                var histMessages = new List<PrivateHistoryMessage>();
                newHistoryToManage[historyChat] = histMessages;
                foreach (var value in item.Value)
                {
                    var campanha = !value.fr;
                    if (campanha)
                    {
                        if (historyChat.GetMessageHistory()
                            .Any(a => a.MessageContent == value.dm
                                      && a.StatusTypeValue == -2))
                        {
                            continue;
                        }
                    }

                    var res = historyChat.AddMessage(
                        value.im == 0 ? Guid.NewGuid().ToString() : value.im.ToString(CultureInfo.InvariantCulture),
                        value.fr == false, value.dm, value.dcm, value.dcm, campanha ? -2 : value.sm);

                    if (res != null)
                    {
                        histMessages.Add(res);
                    }
                }
            }
            return newHistoryToManage;
        }

        private Dictionary<int, IList<Mensagem>> GetHistoryFromService(int userId, IEnumerable<object> unmatchHistoryArgs)
        {
            var withoutHistory = new Dictionary<int, IList<Mensagem>>();
            using (var client = new AppClient())
            {
                Func<string, int> convertToInt = arg => { int targetId; Int32.TryParse(arg, out targetId); return targetId; };

                foreach (var item in unmatchHistoryArgs)
                {
                    dynamic objValue = ((dynamic)item)["targetId"];
                    var key = objValue == null ? 0 : convertToInt(objValue.ToString());
                    if (key <= 0)
                        continue;

                    var otherMessages =
                        client.HistoricoConversas_New(new InHistorico
                        {
                            ci = userId.ToString(CultureInfo.InvariantCulture),
                            cv = key,
                            im = 0,
                            qm = QuantityOfMessagesToGetInService.Value
                        });

                    if (otherMessages == null || otherMessages.l == null || !otherMessages.l.Any())
                        continue;

                    withoutHistory.Add(key, otherMessages.l.ToArray());
                }
            }
            return withoutHistory;
        }

        private object ConverToMessageDto(PrivateHistoryChat chat, PrivateHistoryMessage message, Tuple<int, string, string> extraInfo)
        {
            int pfId = extraInfo.Item1;
            string pfName = extraInfo.Item2;
            string pfCelular = extraInfo.Item3;

            var obj = new
              {
                  GuidMensagem = message.Guid,
                  GuidChat = chat.GuidChat,
                  CurriculoId = chat.PartyWith,
                  PessoaFisicaId = pfId,
                  PessoaFisicaNome = pfName,
                  PessoaFisicaCelular = pfCelular,
                  StatusData = message.StatusDate,
                  TipoStatus = message.StatusTypeValue,
                  Conteudo = message.StatusTypeValue == -2 ? new { nome = extraInfo.Item2 }.ToString(message.MessageContent) : message.MessageContent,
                  CriacaoData = message.CreationDate,
                  EscritoPorUsuarioFilialPerfil = message.CreatorType == PrivateHistoryMessage.MessageOwner.Self,
              };

            return obj;
        }
    }
}
