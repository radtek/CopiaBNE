<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompartilhamentoVaga.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.CompartilhamentoVaga" %>
<link href='//fonts.googleapis.com/css?family=Open+Sans:400,600italic' rel='stylesheet' type='text/css'>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/CompartilhamentoVaga.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlCompartilhamentoVaga" runat="server" CssClass="modal_compartilhamento_vagaSucesso" style="display:none">
    <div>
        <h2 class="titulo_compartilhar_vaga">
            <asp:Label ID="lblTituloModal" runat="server" />
        </h2>

        <div class="limpa_float_compartilhar_vaga">&nbsp;</div>

        <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:ImageButton CssClass="botaoNovo_fechar_modal" ID="btiFechar" ImageUrl="/img/modal_nova/btnRedFecharModal.png"
                    runat="server" CausesValidation="false" OnClick="btiFechar_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="upEmailsDestinatario"  CssClass="containerConteudoModal" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlEmailsDestinatario" runat="server">
                    <p class="labelDigiteAbaixoAlinha">Digite abaixo o e-mail para quem gostaria de compartilhar a vaga:</p>

                    <div>
                        <asp:Label ID="lblEmailDestinatario" runat="server" CssClass="label_email_destinatario" 
                            Text="E-mail da pessoa:" AssociatedControlID="txtEmailDestinatario"></asp:Label>

                        <div class="controle_compartilhar_email">
                            <asp:RegularExpressionValidator ID="revEmailDestinatario" runat="server" ControlToValidate="txtEmailDestinatario"
                                ErrorMessage="Não está válido! Corrigir"
                                ValidationExpression="^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$" 
                                ValidationGroup="Email" />
                            <div class="email_destinatario">
                                <asp:TextBox ID="txtEmailDestinatario" runat="server" MaxLength="50" Columns="50" ClientIDMode="Predictable"
                                    CssClass="textbox_padrao textbox_email_destinatario" />
                                <asp:Button ID="btnAdicionarEmail" runat="server" Text="+" 
                                    onclick="btnAdicionarEmail_Click" ValidationGroup="Email" CausesValidation="true" />
                            </div>
                        </div>
                    </div>

                    <asp:Repeater ID="rptEmails" runat="server" onitemcommand="rptEmails_ItemCommand">
                        <ItemTemplate>
                            <asp:Panel ID="pnlEmail" runat="server" CssClass="panel_item_repeater_compartilhar_email">
                                <div class="grid_email">
                                    <%# Eval("Email") %>
                                    <asp:LinkButton ID="lnkDeletarEmail" runat="server" CssClass="btn_deletar_email_grid" 
                                    CommandName="DeletarEmail" CommandArgument='<%# Eval("Guid") %>'></asp:LinkButton>
                                </div>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:Repeater>

                    <asp:Button runat="server" ID="btnEnviar" Text="Enviar e-mails" 
                        CssClass="botao_padrao botao_enviar_compartilhar_vaga" onclick="btnEnviar_Click" 
                        ValidationGroup="Email" CausesValidation="true" />
                </asp:Panel>
                <asp:Panel ID="pnlSucesso" runat="server" Visible="false">
                    
                        <div>
  <p style=" text-align:center; font-size:30px; color:green" class="text-center">  <i class="fa  fa-check-circle center-block"></i></p>
                    </div>

                    <p class="texto_cadastro_sucesso_compartilhamento_vaga">
                        <asp:Label ID="lblSucesso" runat="server"></asp:Label>
                    </p>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
</div>

</asp:Panel>
<asp:HiddenField ID="hfVariavel" runat="server" />

<AjaxToolkit:ModalPopupExtender ID="mpeCompartilhamentoVaga" TargetControlID="hfVariavel"
    PopupControlID="pnlCompartilhamentoVaga" runat="server">
</AjaxToolkit:ModalPopupExtender>