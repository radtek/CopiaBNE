<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PresidenteNovoAgradecimento.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.NovoAgradecimento" %>

<%@ Register
    Src="~/UserControls/Modais/ModalConfirmacao.ascx"
    TagName="ModalConfirmacao"
    TagPrefix="uc1" %>

<Employer:DynamicScript runat="server" Src="/js/local/Forms/SalaAdministrador/CadastroAgradecimento.js" type="text/javascript" />

<div class="painel_configuracao_conteudo">
    <h3 class="titulo_configuracao_content">Novo Agradecimento</h3>
    <div class="linha titulo_noticia">
        <asp:RequiredFieldValidator ID="rfvtxtNomeNovo" ControlToValidate="txtNomeNovo" runat="server" ValidationGroup="salvarAgradecimento" ErrorMessage="Campo obrigatório"></asp:RequiredFieldValidator>
        <div>
            <asp:Label ID="lblNomeNovo" runat="server" Text="Nome" CssClass="label_principal texto_normal editar" />
            <asp:TextBox ID="txtNomeNovo" runat="server" CssClass="textbox_padrao campo_obrigatorio" />
        </div>
    </div>
    <div class="linha titulo_noticia">
        <asp:RegularExpressionValidator
            ID="revEmail"
            runat="server"
            ControlToValidate="txtEmailNovo"
            ValidationGroup="CadastroDadosPessoais"
            ErrorMessage="Email Inválido.">
        </asp:RegularExpressionValidator>

        <div>
            <asp:Label ID="lblEmailNovo" runat="server" Text="Email" CssClass="label_principal texto_normal editar" />
            <asp:TextBox ID="txtEmailNovo" runat="server" CssClass="textbox_padrao " />
        </div>
    </div>
    <div class="linha titulo_noticia">
        <asp:CustomValidator
            ID="cvCidade"
            runat="server"
            ErrorMessage="Cidade Inválida."
            ValidateEmptyText="true"
            ClientValidationFunction="cvCidade_Validate2"
            ControlToValidate="txtCidade"
            ValidationGroup="salvarAgradecimento"></asp:CustomValidator>
        <div>
            <asp:Label
                runat="server"
                ID="lblCidade"
                CssClass="label_principal texto_normal editar"
                Text="Cidade"
                AssociatedControlID="txtCidade" />
            <asp:TextBox
                ID="txtCidade"
                runat="server"
                CssClass="textbox_padrao campo_obrigatorio textbox_cidade_mini_cv"
                Columns="40"></asp:TextBox>
        </div>
    </div>
    <div>
        <asp:Label ID="lblAgradecimentoNovo" runat="server" Text="Mensagem" CssClass="label_principal texto_normal editar" />
        <asp:RequiredFieldValidator ID="rfvtxtAgradecimentoNovo" ControlToValidate="txtAgradecimentoNovo" runat="server" ValidationGroup="salvarAgradecimento" ErrorMessage="Campo obrigatório"></asp:RequiredFieldValidator><br />
        <asp:TextBox ID="txtAgradecimentoNovo" runat="server" Columns="65" Rows="8" CssClass="textarea_padrao campo_obrigatorio"
            TextMode="MultiLine"></asp:TextBox>
    </div>
    <div class="form_menor">
        <%--Linha Auditado--%>
        <div class="linha">
            <asp:Label ID="lblAuditadoNovo" runat="server" Text="Publicado" CssClass="label_principal texto_normal " />
            <div class="container_campo texto_normal">
                <span>
                    <asp:RadioButton ID="rdbSimNovo" runat="server" GroupName="rdbPublicado" Checked="true" />
                    Sim</span> <span>
                        <asp:RadioButton ID="rdbNaoNovo" runat="server" GroupName="rdbPublicado" />
                        Não</span>
            </div>
        </div>
        <%--FIM Em Exibição --%>
    </div>
    <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
        <asp:Button ID="btnSalvar" runat="server" CssClass="botao_padrao painelinterno" Text="Salvar"
            CausesValidation="true" ValidationGroup="salvarAgradecimento" OnClick="btnSalvar_Click" />
    </asp:Panel>
</div>

<uc1:ModalConfirmacao ID="ucModalConfirmacao" runat="server" />
