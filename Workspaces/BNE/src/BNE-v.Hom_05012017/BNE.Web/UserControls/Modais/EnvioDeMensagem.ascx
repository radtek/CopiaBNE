<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EnvioDeMensagem.ascx.cs"
Inherits="BNE.Web.UserControls.Modais.EnvioDeMensagem" %>

<Employer:DynamicScript runat="server" Src="/js/bootstrap/bootstrap.js" type="text/javascript"/>
<Employer:DynamicScript runat="server" Src="/js/local/UserControls/EnvioDeMensagem.js" type="text/javascript"/>
<Employer:DynamicScript runat="server" Src="/js/jquery.maskedinput.min.js" type="text/javascript"/>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/EnvioDeMensagem.css" type="text/css" rel="stylesheet"/>
<link href="//fonts.googleapis.com/css?family=Open+Sans+Condensed:300,700" rel="stylesheet" type="text/css"/>

<asp:Panel ID="pnlEnvioMensagem" runat="server" CssClass="empresa modal_envio_mensagem" Style="display: none;">
    <div id="envio-email-sms">
        <div class="box-content-modal">
            <div class="modal-head">
                <h3>Envio de Email e SMS</h3>
                <span class="pull-right"><i class="fa fa-times"></i>
                </span>
            </div>
            <div class="modal-contant">
                <div class="announced">
                    <i class="fa fa-envelope-o"></i>
                    Escreva sua mensagem para convocar o(s) candidato(s)
                    <hr class="clearfix">
                </div>
                <div id="container-sms">
                    <label>
                        <strong>SMS:</strong>
                    </label>
                    <label>
                        <strong>Agendar sms:</strong>
                    </label>
                    <asp:CheckBox ID="cbAgendar" runat="server"/>

                    <div id="container-agenda">
                   
                        <asp:TextBox runat="server" ID="txtAgendamento" MaxLength="10" Width="86px"></asp:TextBox>
                        <label>
                            <strong>Data de Envio:</strong>
                        </label>
                        <strong>às:</strong>
                        <asp:TextBox ID="txtHoraEnvio" Columns="2" runat="server"></asp:TextBox>


                        <%--  <asp:RegularExpressionValidator ValidationExpression="\\d{1,2}:\\d{2}\\s*(AM|PM)" runat="server"
                        ControlToValidate="txtHoraEnvio" ErrorMessage="Hora Invalida"></asp:RegularExpressionValidator>--%>
                    </div>
                    <asp:UpdatePanel ID="upMensagemSMS" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:TextBox ID="txtSMS" TextMode="MultiLine" Rows="3" runat="server" MaxLength="135"></asp:TextBox>
                            <span class="pull-right count">
                                <Componentes:ContagemCaracteres runat="server" ControlToValidate="txtSMS" MaxLength="135" CssClass="contagem-caracteres" />
                            </span>
                            <div class="suggest">
                                <ul>
                                    <li>
                                        <a class="suggest-text">
                                            <asp:Literal runat="server" ID="suggest1"></asp:Literal>
                                        </a>
                                    </li>
                                    <li>
                                        <a class="suggest-text">
                                            <asp:Literal runat="server" ID="suggest2"></asp:Literal>
                                        </a>
                                    </li>
                                    <li>
                                        <a class="suggest-text">
                                            <asp:Literal runat="server" ID="suggest3"></asp:Literal>
                                        </a>
                                    </li>
                                </ul>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="container-email">
                    <label>
                        <strong>EMAIL:</strong>
                    </label>
                    <asp:UpdatePanel ID="upMensagemEmail" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:TextBox ID="txtEmail" MaxLength="1000" ReadOnly="false" TextMode="MultiLine" Columns="250" Rows="4" runat="server"></asp:TextBox>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <label>
                        <strong>ENVIO POR:</strong>
                    </label>
                    <asp:CheckBox ID="ckbSMS" runat="server" Text="SMS" Checked="True" CssClass="checkbox sms"/>
                    <asp:CheckBox ID="ckbEmail" runat="server" Text="EMAIL" Checked="True" CssClass="checkbox"/>
                    <div id="DivBotoes">
                        <asp:UpdatePanel ID="upSaldoSMS" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <asp:Literal runat="server" ID="litSaldoSMS"></asp:Literal>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="upBtnEnviarMensagem" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Button ID="btnEnviarMensagem" runat="server" OnClick="btnEnviarMensagem_Click" CssClass="confirm" Text="Enviar" CausesValidation="False"/>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
<asp:HiddenField ID="hfVariavel" runat="server"/>
<AjaxToolkit:ModalPopupExtender ID="mpeEnvioMensagem" TargetControlID="hfVariavel" PopupControlID="pnlEnvioMensagem" runat="server" BehaviorID="mpeEnvioMensagem">
</AjaxToolkit:ModalPopupExtender>