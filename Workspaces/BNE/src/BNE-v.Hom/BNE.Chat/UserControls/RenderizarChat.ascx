<%@ control language="C#" autoeventwireup="true" codebehind="RenderizarChat.ascx.cs" inherits="BNE.Chat.UserControls.RenderizarChat" %>
<%@ register tagprefix="up" namespace="System.Web.UI" assembly="System.Web.Extensions" %>

<up:updatepanel runat="server" id="upPrincipalChat" updatemode="Conditional">
    <contenttemplate>
        <script type="text/javascript">var pathRenderJs = "<%= PathToRenderJs %>"; var ufp = "<%=UsuarioFilialPerfilLogado%>"; var pathDashboardLink = "<%= DashboardLink %>"; var pathRenderImg = "<%= PathToRenderImg %>"; var pathRenderCss = "<%= PathToRenderCss %>"; var pathRenderThumb = "<%= PathToRenderThumb %>"; var pathRenderLink = "<%= PathToTargetLink %>";</script>

        <script type="text/javascript" src="<%= UrlAplicacao + PathToRenderJs + "/moment.js" %>"></script>
        <script type="text/javascript" src="<%= UrlAplicacao + PathToRenderJs + "/linq.js" %>"></script>
        <script type="text/javascript" src="<%= UrlAplicacao + PathToRenderJs + "/knockout-3.1.0.js" %>"></script>

        <script type="text/javascript" src="<%= UrlAplicacao + PathToRenderJs + "/json2.js" %>"></script>
        <script type="text/javascript" src="<%= UrlAplicacao + PathToRenderJs + "/jstorage.js" %>"></script>
        <script type="text/javascript" src="<%= UrlAplicacao + PathToRenderJs + "/jquery.signalR-1.2.1.js" %>"></script>
        <script type="text/javascript" src="http://tanque.bne.com.br/signalr/hubs"></script>
       <%-- <script type="text/javascript" src="http://localhost:2000/signalr/hubs"></script>--%>
        <script type="text/javascript" src="<%= UrlAplicacao + PathToRenderJs + "/webchat.js" %>"></script>

        <script type="text/javascript" src="<%= UrlAplicacao + PathToRenderJs + "/statechat.js" %>"></script>
        <script type="text/javascript" src="<%= UrlAplicacao + PathToRenderJs + "/visualchat.js" %>"></script>
        <script type="text/javascript" src="<%= UrlAplicacao + PathToRenderJs + "/viewmodelchat.js" %>"></script>

        <link rel="stylesheet" type="text/css" href="<%= UrlAplicacao + PathToRenderCss + "/chat.css" %>" />
        <link rel="stylesheet" type="text/css" href="<%= UrlAplicacao + PathToRenderCss + "/balao.css" %>" />
        <link rel="stylesheet" type="text/css" href="<%= UrlAplicacao + PathToRenderCss + "/info.css" %>" />
        <link rel="stylesheet" type="text/css" href="<%= UrlAplicacao + PathToRenderCss + "/pessoasFila.css" %>" />

        <style>
            .containerIconTopoDoc {
                background-image: url("<%= UrlAplicacao + PathToRenderImg + "/iconTopoDoc.png"%>");
                background-repeat: no-repeat;
                display: block;
                float: left;
                height: 32px;
                margin-left: 11px;
                margin-top: 8px;
                width: 32px;
            }

            .containerPessoa1 {
                background-repeat: no-repeat;
                display: block;
                float: left;
                height: 32px;
                margin-left: 9px;
                margin-top: 3px;
                width: 32px;
            }
        </style>

        <script type="text/javascript">
            function MoreInfoClick(data, e) {
                e.cancelBubble = true;
                if (e.stopPropagation)
                    e.stopPropagation();
                data.changeMoreInfoVisibility(data);
            }

            function DeleteContactClick(data, e) {
                e.cancelBubble = true
                if (e.stopPropagation) {
                    e.stopPropagation();
                }
                if (confirm("Tem certeza que deseja excluir esta pessoa? Ele não estará mais disponível nesta lista de contatos.")) {
                    data.deleteContact();
                }
            }

            function CloseActiveWindowClick(data, e) {
                e.cancelBubble = true;
                if (e.stopPropagation)
                    e.stopPropagation();

                data.closeActiveChat(data);
            }

            function AccessShortCutLink(data, e) {
                e.cancelBubble = true;
                if (e.stopPropagation)
                    e.stopPropagation();

                data.accessTargetLink();
            }

            function CloseExceededChatClick(data, e) {
                e.cancelBubble = true;
                if (e.stopPropagation)
                    e.stopPropagation();

                data.closeExceededChat(data);
            }
        </script>

        <script type="text/javascript">
            var chatSize = 270; var contactSize = 210; var marginLeft = 90;
            var windowSize = $(window).width();
            var maxLength = Math.floor(Math.max(1, Math.min(4, (windowSize - contactSize - marginLeft) / chatSize)));
            //var toDispose = [];

            var enableSaveState = true;
            var msStateTime = 86400000 // 4 dias 345600000;
            var enableRecoverState = true;
            var boolIsLoadingState = false;

            var scopeViewModel;
            var chatConnection;

            function positionContactList() {
                var bodyHeight = $(window).height();
                if (isUndefined(bodyHeight) || bodyHeight.length == 0 || bodyHeight <= 0)
                    return;

                $("#container_lista_contatos_direita").css({
                    'max-height': (bodyHeight - 33) + 'px'
                });
            }

            $(document).ready(function () {
                $("#link_dashboard").attr("href", pathDashboardLink)
                if (pathDashboardLink == '') {
                    $("#link_dashboard").hide();
                }
                else {
                    $("#link_dashboard").mousedown(function (e) {
                        e.cancelBubble = true;
                        if (e.stopPropagation)
                            e.stopPropagation();

                        window.open($("#link_dashboard").attr("href"), "_blank");
                    });
                }
                function monitorExit() {
                    if ($("#btiSair").length <= 0)
                        return;

                    $("#btiSair").bind("click", function () {
                        scopeViewModel.clearCacheHistory();
                    });
                }

                $(window)
                  .resize(function () {
                      positionContactList();
                  });

                positionContactList();

                monitorExit();
                chatConnection = new WebChat(ufp);
                if (chatConnection.isValid)
                    chatConnection.do();

                scopeViewModel.activeChats.subscribe(function (changes) {
                    var janelaChat = $("#painel_janelas");

                    changes.forEach(function (entry) {
                        if (entry.status == 'added') {

                            entry.value.takeFocus.subscribe(function () {
                                if (entry.value.takeFocus()) {
                                    setTimeout(function () {
                                        if (entry.value.allMessages.totalVisible() > 1) {
                                            var windowChatContent = janelaChat.find("[id*='container_texto_pessoa_" + entry.value.letter + "']").first();
                                            if ($(windowChatContent).hasScrollBar()) {
                                                $(windowChatContent).scrollTop($(windowChatContent)[0].scrollHeight);
                                            }
                                        }
                                        var input = janelaChat.find("[id*='input_message_chat_" + entry.value.letter + "']").first();
                                        if (input.length >= 1) {
                                            $(input[0]).focus();
                                        }
                                        entry.value.takeFocus(false);
                                    }, 40);
                                }
                            });

                            entry.value.allMessages.subscribe(function (innerChanges) {

                                if (boolIsLoadingState) {
                                    return;
                                }

                                var element = janelaChat.find("[id*='container_texto_pessoa_" + entry.value.letter + "']").first();

                                setTimeout(function () {
                                    if ($(element).hasScrollBar()) {
                                        $(element).scrollTop($(element)[0].scrollHeight);
                                    }
                                }, 28);
                            });

                            var cancellation = false;
                            entry.value.activeBlink.subscribe(function (newValue) {
                                if (newValue) {
                                    var colorBar = janelaChat.find("[id*='topo_verde_" + entry.value.letter + "']").first();

                                    var firstOpacity = colorBar.css("opacity");
                                    var current = true;

                                    var blink = function () {
                                        if (cancellation)
                                            return;

                                        setTimeout(function () {
                                            if (entry.value.activeBlink()) {

                                                if (current) {
                                                    current = false;
                                                    colorBar.css("opacity", firstOpacity);
                                                } else {
                                                    current = true;
                                                    colorBar.css("opacity", 1);
                                                }
                                                blink();
                                            } else {
                                                if (!entry.value.userFocus())
                                                    colorBar.css("opacity", firstOpacity);
                                            }
                                        }, 1000);
                                    }

                                    cancellation = false;
                                    colorBar.css("opacity", 1);
                                    blink();
                                } else {
                                    cancellation = true;
                                }
                            });
                            setTimeout(function () {
                                var element1 = janelaChat.find("[id*='janela_chat_" + entry.value.letter + "']").first();

                                var oldOpacity;
                                var colorState = janelaChat.find("[id*='topo_verde_" + entry.value.letter + "']").first();

                                element1.bind("focusin", function () {

                                    var auxOpacity = colorState.css("opacity");
                                    if (auxOpacity != 1) {
                                        oldOpacity = auxOpacity;
                                    }


                                    colorState.css("opacity", 1);
                                });

                                element1.bind("focusout", function () {
                                    colorState.css("opacity", oldOpacity);
                                });
                            }, 28);
                        }
                    });
                }, null, "arrayChange");

                var subsHistoryLoaded = scopeViewModel.historyLoaded.subscribe(function () {
                    if (scopeViewModel.historyLoaded()) {

                        setTimeout(function () {
                            var elements = $("#painel_janelas").find("[id*='container_texto_pessoa_']");
                            elements.each(function (i, element) {
                                if ($(element).hasScrollBar()) {
                                    $(element).scrollTop($(element)[0].scrollHeight);
                                }
                            });
                        }, 30);

                        subsHistoryLoaded.dispose();
                    }
                });

                scopeViewModel.contactListMaximized.subscribe(function (changes) {
                    if (changes) {
                        positionContactList();
                    }
                });
            });
        </script>

        <script type="text/html" id="accumulated-chat-template">
            <div class="pessoa1" data-bind="click: activeThisChat">
                <p class="pessoa_na_lista" data-bind="text: title">
                </p>
                <p class="text_close_janela_pendente" data-bind="click: CloseExceededChatClick">
                    X
                </p>
            </div>
        </script>
        
        <script type="text/html" id="message-chat-template">
                <div>
                    <img class="containerPessoa1" data-bind="visible: !owner(), attr: { src: $data.thumbTargetImage() }"></img>
                    <p data-bind="attr: { class: owner() ? 'triangle-border right' : 'triangle-border left' }, text: content"></p>
                </div>

                <!-- ko if: !errorToSend() -->
                    <!-- ko if: (statusMessage() >= 0 && statusMessage() < 5) -->
                        <img class="checked" src="<%= UrlAplicacao + PathToRenderImg + "/checked.png"%>" data-bind="visible: owner" />
                    <!-- /ko -->

                    <p class="visualizada_txt" data-bind="visible: owner()">
                       <span data-bind="text: translateStatusMensagem(statusMessage())"></span>  
                        <span>
                            às
                        </span>
                        <span data-bind="text: moment((isUndefined(statusDate()) ? sendDate() : statusDate())).format('DD/MM/YYYY HH:mm:ss')"></span>  
                    </p>
                <!-- /ko -->

                <p class="data_txt" data-bind="visible: !owner()">
                    <span>recebida</span>
                        <span>
                            às
                        </span>
                    <span data-bind="text: moment(receiveDate()).format('DD/MM/YYYY HH:mm:ss')"></span>  
                </p>
            
                <p class="error_enviar_txt" data-bind="visible: errorToSend()">
                    Falha ao enviar a mensagem, verifique sua conexão e tente novamente.
                </p>
        </script>

        <script type="text/html" id="chat-window-template">
            <div class="janela_chat" data-bind="uniqueId: 'janela_chat_' + letter + '_', attr: { style: maximized() ? 'position: inherit; bottom:0px;' : 'position:relative; bottom:-288px;' }" >
                <div class="container_conversa_informacoes" data-bind="visible: showMoreInfo, if: maximized">
                    <div class="primaryContainerInfo">
                        <div class="informacoes">
                            <div class="topoInfo">
                                <p class="text_informacoes">
                                Informações
                                </p>
                            </div>
                            <div class="valor_informacoes" data-bind="html: details">
                               
                            </div>
                        </div>
                    </div>
                </div>

                <div class="primaryContainerChat">
                    <div class="topo_verde"
                         data-bind="uniqueId: 'topo_verde_' + letter + '_', click: changeMaximizedStatus">
                        <div class="pessoa_titulo_chat">
                            <p class="containerIconTopoDoc" data-bind="click: AccessShortCutLink"  title="Acesse o Currículo"></p>
                            <p class="pessoa_Aleatoria_txt p" data-bind="text: title"></p>
                        </div>
                        <img  data-bind="visible: maximized, click: function (s, e) { MoreInfoClick(s, e); $data.getMoreInfo(); }"  src="<%= UrlAplicacao + PathToRenderImg + "/info.png"%>" class="image" />
                        <img  data-bind="click: CloseActiveWindowClick"  src="<%= UrlAplicacao + PathToRenderImg + "/fechar.png"%>" class="fechar" />
                    </div>

                    <div class="container_conversa_chat" data-bind="attr: { style: maximized() ? 'display:block;' : 'display:none;' }, uniqueId: 'container_conversa_chat_' + letter + '_'">
<%--                        <div style="position: relative; top:32px; background: #f4f4f4;width: 85px;height: 18px;z-index: 9999;left: 160px;" data-bind="visible: $parent.isConnected() == 'N'">
                            <span style="color:red;font-size: 12px;position: relative; top: -32px; left:2px;">Sem Conexão!</span>
                        </div>--%>
                        <div class="container_texto_pessoa"  data-bind="template: { name: 'message-chat-template', foreach: allMessages }, uniqueId: 'container_texto_pessoa_' + letter + '_'">
                            
                        </div>
                        <div class="formgroup">
                            <input class="input textinput text_input_front_hack" maxlength="140" type="text" data-bind="value: textToSend, uniqueId: 'input_message_chat_' + letter + '_', returnAction: sendMessage, escAction: closeActiveChat, valueUpdate: 'afterkeydown', hasFocus: userFocus" autocomplete="off"></input>
                        </div>
                    </div>
                </div>
            </div>
        </script>
        
    
        <script type="text/html" id="contact-list-template">
             <li class="online_contact" data-bind="event: { mouseover: overEvent, mouseout: overEvent }, click: function (s, e) { $data.openChat(); }">
                 <div class="container_online_contact_info" data-bind="attr: { title: title }">
                    <div class="container_contact_info_left">
                        <img src="" class="img_online_contact" data-bind="attr: { src: $data.thumbTargetImage() }" />
                    </div>
                    <div class="container_contact_info_right">
                        <p class="contact_title" data-bind="text: title"></p>
                        <span class="contact_last_message_date" data-bind="text: notificationDate, attr: { style: notification() ? 'font-style:italic;' : 'font-style:normal;' }"></span>
                        <br/>
                        <span class="contact_last_message_content" data-bind="text: lastMessage, attr: { style: notification() ? 'font-weight:bold;color:rgb(230, 150, 4);' : (ownerOfLastMessage() ? 'font-weight:normal;color:gray;' : 'font-weight:normal;color:black;') }" ></span> 
                        <input type="button" value="x" class="btn_deletar_contato" data-bind="visible: mouseOver(), click: DeleteContactClick" />   
                    </div>
                 </div>
             </li>
         </script>
        
        <script type="text/html" id="contact-group-template">
            <li>
                <ul class="lista_contatos"  data-bind="template: { name: 'contact-list-template', foreach: $parent.onlineContacts }">
                </ul>
            </li>        
        </script>
        
        <asp:Panel runat="server" ID="pnlPrincipalChat" CssClass="container_chat">
            <div class="container_inner_chat">
                <div class="primaryContainerFila" data-bind="visible: accumulatedChats.totalVisible() > 0" style="display: none;">
                    <div class="container_notificacoes">
                        <div id="container_excesso_pessoas"  data-bind="template: { name: 'accumulated-chat-template', foreach: accumulatedChats }">
                        </div>
                        <div id="box_excesso_pessoas">
                            <div class="box_janelas_pendentes">
                                <img src="<%= UrlAplicacao + PathToRenderImg + "/balaob.png"%>" class="balaob" />
                                <img src="<%= UrlAplicacao + PathToRenderImg + "/balao.png"%>" class="balao" />
                            </div>
                            <p class="text2" data-bind="text: accumulatedChats.totalVisible()">
                            </p>
                        </div>
                    </div>
                </div>
                
                <div class="painel_contatos" id="container_contatos">
                    <div class="container_geral_contatos">
                        <div>
                            <div class="container_header_contatos" data-bind="click: changeContactListMaximizedStatus">
                                <div class="pessoa_titulo_chat">
                                    <p class="pessoa_Aleatoria_txt" style="margin-left:12px;">Contatos | </p>
                                    <a class="dashboard_link" href="#" id="link_dashboard">Dashboard</a>
                                    <%--<span data-bind="visible: isConnected() == 'N'" style="color:red;font-size: 12px;position: relative; top: -22px; left: -19px;float: right;background: rgb(188, 188, 188);">Sem conexão!</span>--%>
                                </div>
                            </div>
                            <div class="container_lista_contatos" id="container_lista_contatos_direita" data-bind="attr: { style: contactListMaximized() ? 'display:block;' : 'display:none;' }">
                                <div data-bind="visible: !showContactList() && historyLoaded()" style="font-size: 11px; margin: 3px 11px; color: steelblue;">
                                    Sem conversas recentes ou ativas.
                                </div>
                                <ul class="lista_contatos"  data-bind="template: { name: 'contact-list-template', foreach: onlineContacts }">
                                
                                </ul>
                               
                            </div>
                           
                        </div>
                    </div> 
                </div>
                <div class="painel_janelas" id="painel_janelas" data-bind="template: { name: 'chat-window-template', foreach: activeChats }">
                </div>

                <script type="text/javascript">
                    scopeViewModel = new MainViewModel();
                    ko.applyBindings(scopeViewModel);
                </script>
            </div>
        </asp:Panel>
    </contenttemplate>
</up:updatepanel>
