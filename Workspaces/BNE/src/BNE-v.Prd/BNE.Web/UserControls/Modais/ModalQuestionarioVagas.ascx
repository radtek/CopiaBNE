﻿<%@ Control
    Language="C#"
    AutoEventWireup="true"
    CodeBehind="ModalQuestionarioVagas.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.ModalQuestionarioVagas" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/ModalQuestionarioVagas.css" type="text/css" rel="stylesheet" />
<asp:Panel
    ID="pnlModalQuestionarioVagas"
    CssClass="modal_conteudo candidato"
    Style="display: none"
    runat="server">
    <h2 class="titulo_modal">
        <span>Questionário</span></h2>
    <asp:ImageButton
        CssClass="botao_fechar_modal"
        ID="btiFechar"
        ImageUrl="/img/botao_padrao_fechar.gif"
        runat="server"
        CausesValidation="false" />
    <asp:UpdatePanel
        ID="uppModalQuestionarioVagas"
        runat="server"
        UpdateMode="Conditional">
        <ContentTemplate>
            <asp:UpdatePanel
                ID="upPnlPergunta01"
                runat="server"
                UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel
                        CssClass="painel_pergunta"
                        ID="pnlPergunta01"
                        runat="server">
                        <p>
                            01.
                            <asp:Literal
                                ID="litPergunta01"
                                runat="server"></asp:Literal>
                        </p>
                        <div class="opcoes_pergunta">
                            <asp:RadioButton
                                ID="rbtRespostaSim01"
                                CssClass="resposta_sim"
                                GroupName="Pergunta01"
                                AutoPostBack="true"
                                runat="server"
                                Text="Sim"></asp:RadioButton>
                            <asp:RadioButton
                                ID="rbtRespostaNao01"
                                CssClass="resposta_nao"
                                GroupName="Pergunta01"
                                AutoPostBack="true"
                                runat="server"
                                Text="Não"></asp:RadioButton>
                            <componente:AlfaNumerico runat="server" ID="txtResposta1" />
                            <asp:Label runat="server" ID="lblResposta1" Visible="false"></asp:Label>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel
                ID="upPnlPergunta02"
                runat="server"
                UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel
                        CssClass="painel_pergunta"
                        ID="pnlPergunta02"
                        runat="server">
                        <p>
                            02.
                            <asp:Literal
                                ID="litPergunta02"
                                runat="server"></asp:Literal>
                        </p>
                        <div class="opcoes_pergunta">
                            <asp:RadioButton
                                ID="rbtRespostaSim02"
                                CssClass="resposta_sim"
                                GroupName="Pergunta02"
                                runat="server"
                                Text="Sim"></asp:RadioButton>
                            <asp:RadioButton
                                ID="rbtRespostaNao02"
                                CssClass="resposta_nao"
                                GroupName="Pergunta02"
                                runat="server"
                                Text="Não"></asp:RadioButton>
                            <componente:AlfaNumerico runat="server" ID="txtResposta2" />
                               <asp:Label runat="server" ID="lblResposta2" Visible="false"></asp:Label>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel
                ID="upPnlPergunta03"
                runat="server"
                UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel
                        CssClass="painel_pergunta"
                        ID="pnlPergunta03"
                        runat="server">
                        <p>
                            03.
                            <asp:Literal
                                ID="litPergunta03"
                                runat="server"></asp:Literal>
                        </p>
                        <div class="opcoes_pergunta">
                            <asp:RadioButton
                                ID="rbtRespostaSim03"
                                CssClass="resposta_sim"
                                GroupName="Pergunta03"
                                runat="server"
                                Text="Sim"></asp:RadioButton>
                            <asp:RadioButton
                                ID="rbtRespostaNao03"
                                CssClass="resposta_nao"
                                GroupName="Pergunta03"
                                runat="server"
                                Text="Não"></asp:RadioButton>
                            <componente:AlfaNumerico runat="server" ID="txtResposta3" />
                               <asp:Label runat="server" ID="lblResposta3" Visible="false"></asp:Label>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel
                ID="upPnlPergunta04"
                runat="server"
                UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel
                        CssClass="painel_pergunta ultima"
                        ID="pnlPergunta04"
                        runat="server">
                        <p>
                            04.
                            <asp:Literal
                                ID="litPergunta04"
                                runat="server"></asp:Literal>
                        </p>
                        <div class="opcoes_pergunta">
                            <asp:RadioButton
                                ID="rbtRespostaSim04"
                                CssClass="resposta_sim"
                                GroupName="Pergunta04"
                                runat="server"
                                Text="Sim"></asp:RadioButton>
                            <asp:RadioButton
                                ID="rbtRespostaNao04"
                                CssClass="resposta_nao"
                                GroupName="Pergunta04"
                                runat="server"
                                Text="Não"></asp:RadioButton>
                            <componente:AlfaNumerico runat="server" ID="txtResposta4" />
                               <asp:Label runat="server" ID="lblResposta4" Visible="false"></asp:Label>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Panel
                ID="pnlBotoes"
                runat="server"
                CssClass="painel_botoes">
                <asp:Button
                    ID="btnEnviar"
                    runat="server"
                    Text="Enviar"
                    CssClass="botao_padrao"
                    ValidationGroup="QuestionarioVagas"
                    CausesValidation="True"
                    OnClick="btnEnviar_Click" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:HiddenField
    ID="hfModalQuestionarioVagas"
    runat="server" />
<AjaxToolkit:ModalPopupExtender
    ID="mpeModalQuestionarioVagas"
    runat="server"
    PopupControlID="pnlModalQuestionarioVagas"
    TargetControlID="hfModalQuestionarioVagas">
</AjaxToolkit:ModalPopupExtender>
