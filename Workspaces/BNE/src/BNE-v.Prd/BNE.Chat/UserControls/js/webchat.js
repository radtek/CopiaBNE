/// <reference path="viewmodelchat.js" />
/// <reference path="~/UserControls/RenderizarChat.ascx" />
var WebChat = function (ufp) {
    var self = this;
    if (isUndefined($.connection)) {
        return;
    }
    self.isValid = true;

    var webChatServer;
    function translateMessageDto(argReceived) {
        var messageDto;
        if (argReceived.EscritoPorUsuarioFilialPerfil) {
            messageDto = {
                guid: argReceived.GuidMensagem,
                content: argReceived.Conteudo,
                owner: true,
                receiveDate: null,
                sendDate: argReceived.CriacaoData,
                statusMessage: argReceived.TipoStatus,
                statusDate: argReceived.StatusData,
                errorToSend: false
            };
        } else {
            messageDto = {
                guid: argReceived.GuidMensagem,
                content: argReceived.Conteudo,
                owner: false,
                receiveDate: argReceived.CriacaoData,
                sendDate: null,
                statusMessage: argReceived.TipoStatus,
                statusDate: argReceived.StatusData,
                errorToSend: false
            };
        }
        return messageDto;
    }

    function translateChatDto(obj) {
        return {
            guid: obj.GuidChat,
            targetId: obj.CurriculoId,
            title: obj.PessoaFisicaNome,
            notification : isUndefined(obj.Notificacao) ? false : obj.Notificacao
        };
    }

    self.convertToMessageDto = translateMessageDto;
    self.convertToChatDto = translateChatDto;

    self.do = function () {
        //$.connection.hub.url = 'http://tanque.bne.com.br/signalr/hubs';
        $.connection.hub.url = 'http://tanque.bne.com.br/signalr/hubs';
        $.connection.hub.qs = "ufp=" + ufp;
        webChatServer = $.connection.celChatServer;
        $.connection.hub.stateChanged(function (arg) {
            if (arg.newState == 0)
                return;

            if (arg.newState == 1) {
                scopeViewModel.isConnected("Y");
            } else {
                if (arg.newState == 2) {
                    scopeViewModel.isConnected("R");
                } else {
                    scopeViewModel.isConnected("N");
                }
            }
        });

        function translateContactDto(argReceived) {
            var contactDto =
            {
                guid: argReceived.GuidChat,
                targetId: argReceived.CurriculoId,
                title: argReceived.PessoaFisicaNome,
                lastMessage: argReceived.UltimaMensagem,
                lastDate: argReceived.UltimaData,
                notification: isUndefined(argReceived.Notificacao) ? false : argReceived.Notificacao,
                owner: isUndefined(argReceived.Respondido) ? false : argReceived.Respondido
            };

            return contactDto;
        }

        $.connection.hub.start({ transport: 'auto' }, function () {
            var chatHistory = scopeViewModel.bringHistory();

            var quickHistory = function () {
                var chatDto = Enumerable.From(chatHistory).Select(function (obj) {
                    return {
                        guid: obj.guid,
                        targetId: obj.targetId
                    };
                }).ToArray();

                self.requestHistory(chatDto, function (res) {
                    boolIsLoadingState = true;
                    try
                    {
                        if (isUndefined(res)
                       || res.length == 0
                       || isUndefined(res.Result)
                       || res.Result.length == 0)
                            return;

                        if (!(res.Result instanceof Array)) {
                            //todo
                            return;
                        }
                        res.Result.forEach(function (obj) {

                            var messageDto = translateMessageDto(obj);
                            var chatInfoDto = translateChatDto(obj);

                            scopeViewModel.addReceivedMessage(messageDto, chatInfoDto);
                        });
                    }
                    finally
                    {
                        scopeViewModel.onHistoryLoaded();
                        boolIsLoadingState = false;
                    }
                }, function (ex) {
                    if (true) {
                        //todo
                    }
                });
            };

            
            webChatServer.server.requestOnlineContacts().done(function (res) {
                try {
                    if (isUndefined(res)
             || res.length == 0
             || isUndefined(res.Result)
             || res.Result.length == 0)
                        return;

                    if (!(res.Result instanceof Array)) {
                        //todo
                        return;
                    }

                    res.Result.forEach(function (obj) {
                        var contactDto = translateContactDto(obj);
                        scopeViewModel.addNewOnlineContact(contactDto);
                    });
                }
                finally {
                    if (!isUndefined(chatHistory)
               && chatHistory.length > 0) {
                        quickHistory();
                    }
                    else
                    {
                        scopeViewModel.historyLoaded(true);
                    }
                }

            }).fail(function (ex) {
                //todo
                if (!isUndefined(chatHistory)
             && chatHistory.length > 0) {

                    scopeViewModel.closeAllChats();
                }
                scopeViewModel.historyLoaded(true);
            });
        })
        .fail(function (ex) {
            scopeViewModel.isConnected("N");
        });

        webChatServer.client.receiveDeleteContact = function (targetId) {
            
            //todo

        };

        webChatServer.client.receiveChatMessage = function (argReceived) {

            var messageDto = translateMessageDto(argReceived);
            var chatInfoDto = {
                guid: argReceived.GuidChat,
                targetId: argReceived.CurriculoId,
                title: argReceived.PessoaFisicaNome
            };

            scopeViewModel.addReceivedMessage(messageDto, chatInfoDto);
        };

        webChatServer.client.receiveMessageStatus = function (argReceived) {

            var statusDto = {
                guid: argReceived.GuidMensagem,
                statusMessage: argReceived.TipoStatus,
                statusDate: argReceived.StatusData
            };

            var chatInfoDto = {
                guid: argReceived.GuidChat,
                targetId: argReceived.CurriculoId,
            };

            scopeViewModel.updateMessageStatus(statusDto, chatInfoDto);
        };

        webChatServer.client.forceStopConnection = function (argReceived) {
            //todo notify user
            $.connection.hub.stop();
        };

        webChatServer.client.receiveEndChat = function (argReceived) {
            if (true) { // todo

            }
        };

        webChatServer.client.receiveOnlineContacts = function (argReceived) {
            if (true) { // todo

            }
        };

        webChatServer.client.receiveOppositeTypingSignal = function (argReceived) {
            if (true) { // todo

            }
        };

        webChatServer.client.receiveDeleteContact = function (argReceived) {
            scopeViewModel.receiveRemoveContact(argReceived);
        };
    };

    self.sendMessageToServer = function (obj, doneCallBack, failCallBack) {
        if (isUndefined(failCallBack)) {
            webChatServer.server.sendMessage(obj).done(doneCallBack);
        } else {
            try {
                webChatServer.server.sendMessage(obj).done(doneCallBack).fail(failCallBack);
            } catch (ex) {
                failCallBack(ex);
            }
        }
    };

    self.requestOpenChat = function (obj, doneCallBack, failCallBack) {
        if (isUndefined(failCallBack)) {
            webChatServer.server.openChat(obj).done(doneCallBack);
        } else {
            try {
                webChatServer.server.openChat(obj).done(doneCallBack).fail(failCallBack);
            } catch (ex) {
                failCallBack(ex);
            }
        }
    };

    self.sendCloseChatToServer = function (obj, doneCallBack, failCallBack) {
        if (isUndefined(doneCallBack)) {
            if (isUndefined(failCallBack)) {
                webChatServer.server.closeChat(obj);
                return;
            }
        }

        if (isUndefined(failCallBack)) {
            webChatServer.server.closeChat(obj).done(doneCallBack);
        } else {
            try {
                webChatServer.server.closeChat(obj).done(doneCallBack).fail(failCallBack);
            } catch (ex) {
                failCallBack(ex);
            }
        }
    };

    self.requestMoreInfo = function (obj, doneCallBack, failCallBack) {
        if (isUndefined(failCallBack)) {
            webChatServer.server.getMoreInfo(obj).done(doneCallBack);
        } else {
            try {
                webChatServer.server.getMoreInfo(obj).done(doneCallBack).fail(failCallBack);
            } catch (ex) {
                failCallBack(ex);
            }
        }
    };

    self.requestHistory = function (obj, doneCallBack, failCallBack) {
        if (isUndefined(failCallBack)) {
            webChatServer.server.requestHistory(obj).done(doneCallBack);
        } else {
            try {
                webChatServer.server.requestHistory(obj).done(doneCallBack).fail(failCallBack);
            } catch (ex) {
                failCallBack(ex);
            }
        }
    };

    self.sendReadNotificationInConversation = function (obj, doneCallBack, failCallBack) {
        if (isUndefined(failCallBack)) {
            webChatServer.server.sendReadNotification(obj).done(doneCallBack);
        } else {
            try {
                webChatServer.server.sendReadNotification(obj).done(doneCallBack).fail(failCallBack);
            } catch (ex) {
                failCallBack(ex);
            }
        }
    };

    self.sendDeleteContact = function (contactDto, doneCallBack, failCallBack) {
        if (isUndefined(failCallBack)) {
            webChatServer.server.deleteContact(contactDto).done(doneCallBack);
        } else {
            try {
                webChatServer.server.deleteContact(contactDto).done(doneCallBack).fail(failCallBack);
            } catch (ex) {
                failCallBack(ex);
            }
        }
    };
};
