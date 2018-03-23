<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="EditarDadosPessoais.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.EditarDadosPessoais" %>
<Employer:DynamicScript runat="server" Src="/js/local/UserControls/EditarDadosPessoais.js" type="text/javascript" />
<asp:Panel ID="pnlEditarDadosPessoais" Style="display: none; background-color: rgb(235, 239, 242) !important; height: auto !important;" CssClass="modal_conteudo candidato reduzida editar_dados"
    runat="server">
    <asp:UpdatePanel ID="upEditarDadosPessoais" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <h2 class="titulo_modal" style="color:#0A3079 !important">
                <asp:Label ID="lblTitulo" Text="Editar Dados Pessoais" runat="server"></asp:Label>
            </h2>
            <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:ImageButton CssClass="modal_fechar" ID="btiFechar" ImageUrl="/img/modal_nova/btn_amarelo_fechar_modal.png"
                        runat="server" CausesValidation="false" OnClick="BtiFecharClick" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Panel CssClass="coluna_esquerda editar_dados" runat="server">
                <asp:Panel CssClass="painel_editar_dados_esquerda" runat="server">
                    <asp:Image CssClass="icon_editar_dados" ImageUrl="/img/SalaAdministrador/icn_curriculos.png" AlternateText=""
                        runat="server" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="coluna_direita modal_corpo" runat="server">
                <div class="linha">
                    <asp:Label ID="lblCPF" Text="CPF" CssClass="label_principal" runat="server"></asp:Label>
                    <div class="container_campo">
                        <asp:Label ID="lblCPFValor" Text="" runat="server" CssClass="label_principal"></asp:Label>
                    </div>
                </div>
                <div class="linha">
                    <asp:Label ID="lblDataNascimento" Text="Data de Nascimento" CssClass="label_principal"
                        runat="server"></asp:Label>
                    <div class="container_campo">
                        <componente:Data ID="txtDataNascimento" runat="server" ValidationGroup="Salvar" />
                    </div>
                </div>
                <div class="linha">
                    <asp:Label ID="lblTelefone" Text="Telefone Celular" CssClass="label_principal" runat="server"></asp:Label>
                    <div class="container_campo">
                        <componente:Telefone ID="txtTelefone" Tipo="Celular" MensagemErroFormatoFone='<%$ Resources: MensagemAviso, _100006 %>'
                            runat="server" ValidationGroup="Salvar" />
                    </div>
                </div>
                <div class="linha">
                    <asp:Label runat="server" ID="lblNome" Text="Nome" CssClass="label_principal" />
                    <div class="container_campo">
                        <componente:AlfaNumerico CssClassTextBox="textbox_padrao campo_obrigatorio textbox_nome"
                            ID="txtNome" runat="server" Columns="35" MensagemErroFormato='<%$ Resources: MensagemAviso, _100107 %>'
                            MensagemErroIntervalo='<%$ Resources: MensagemAviso, _100107 %>' MensagemErroValor='<%$ Resources: MensagemAviso, _100107 %>'
                            ValidationGroup="Salvar" ClientValidationFunction="ValidarNome" />
                    </div>
                </div>
                <asp:Panel ID="pnlBotoes" CssClass="painel_botoes" runat="server">
                    <div class="btnVoltar">
                    <asp:Button ID="btnSalvar" runat="server" Text="Salvar" CausesValidation="true" ValidationGroup="Salvar"
                         OnClick="BtnConfirmarClick" />
                        </div>
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:HiddenField ID="hfVar" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeEditarDadosPessoais" runat="server" PopupControlID="pnlEditarDadosPessoais"
    TargetControlID="hfVar">
</AjaxToolkit:ModalPopupExtender>
