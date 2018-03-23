<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PresidenteEditarAgradecimento.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.EditarAgradecimento" %>

<%@ Register Src="~/UserControls/Modais/ModalConfirmacao.ascx" TagName="ModalConfirmacao" TagPrefix="uc1" %>

<Employer:DynamicScript runat="server" Src="/js/local/Forms/SalaAdministrador/CadastroAgradecimento.js" type="text/javascript" />

<div class="painel_configuracao_conteudo">
    <h3 class="titulo_configuracao_content">Editar Agradecimento</h3>
    <div class="linha">
        <asp:RequiredFieldValidator ID="rfvtxtNomeEditar" ControlToValidate="txtNomeEditar" runat="server" ValidationGroup="salvarAgradecimento" ErrorMessage="Campo obrigatório"></asp:RequiredFieldValidator>
        <div>
            <asp:Label ID="lblNomeEditar" runat="server" Text="Nome" CssClass="label_principal texto_normal editar" />
            <asp:TextBox ID="txtNomeEditar" runat="server" CssClass="textbox_padrao campo_obrigatorio" />
        </div>
    </div>
    <div class="linha">
        <asp:RegularExpressionValidator
            ID="revEmail"
            runat="server"
            ControlToValidate="txtEmailEditar"
            ValidationGroup="CadastroDadosPessoais"
            ErrorMessage="Email Inválido.">
        </asp:RegularExpressionValidator>

        <div>
            <asp:Label ID="lblEmailEditar" runat="server" Text="Email" CssClass="label_principal texto_normal editar" />
            <asp:TextBox ID="txtEmailEditar" runat="server" CssClass="textbox_padrao" />
        </div>
    </div>

    <div class="linha">
        <asp:CustomValidator
            ID="cvCidade"
            runat="server"
            ErrorMessage="Cidade Inválida."
            ClientValidationFunction="cvCidade_Validate"
            ValidateEmptyText="true"
            ControlToValidate="txtCidade"
            ValidationGroup="salvarAgradecimento"></asp:CustomValidator>

        <div>
            <asp:Label
                runat="server"
                ID="lblCidade"
                CssClass="label_principal editar"
                Text="Cidade"
                AssociatedControlID="txtCidade" />
            <asp:TextBox
                ID="txtCidade"
                runat="server"
                CssClass="textbox_padrao campo_obrigatorio textbox_cidade_mini_cv"
                Columns="40"></asp:TextBox>
            <AjaxToolkit:AutoCompleteExtender
                ID="aceCidadeMini"
                runat="server"
                TargetControlID="txtCidade"
                ServiceMethod="ListarCidadesSiglaEstadoPorNomeParcialEstado">
            </AjaxToolkit:AutoCompleteExtender>
        </div>
    </div>
    <div>
        <asp:Label ID="lblAgradecimentoNovo" runat="server" Text="Mensagem" CssClass="label_principal texto_normal editar" />
        <asp:RequiredFieldValidator ID="rfvtxtAgradecimentoEditar" ControlToValidate="txtAgradecimentoEditar" runat="server" ValidationGroup="salvarAgradecimento" ErrorMessage="Campo obrigatório"></asp:RequiredFieldValidator><br />
        <asp:TextBox ID="txtAgradecimentoEditar" runat="server" Columns="65" Rows="8" CssClass="textarea_padrao campo_obrigatorio"
            TextMode="MultiLine"></asp:TextBox>
    </div>
    <div class="form_menor">
        <%--Linha Auditado--%>
        <div class="linha">
            <asp:Label ID="lblAuditadoNovo" runat="server" Text="Publicado" CssClass="label_principal texto_normal " />
            <div class="container_campo texto_normal">
                <span>
                    <asp:RadioButton ID="rdbSimEditar" runat="server" GroupName="rdbPublicado"
                        OnCheckedChanged="rdbSimEditar_CheckedChanged" />
                    Sim</span> <span>
                        <asp:RadioButton ID="rdbNaoEditar" runat="server" GroupName="rdbPublicado"
                            OnCheckedChanged="rdbNaoEditar_CheckedChanged" />
                        Não</span>
            </div>
        </div>
        <%--FIM Em Exibição --%>
    </div>



    <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
        <asp:Button ID="btnSalvar" runat="server" CssClass="botao_padrao painelinterno" Text="Salvar"
            OnClick="btnSalvar_Click" ValidationGroup="salvarAgradecimento" />
    </asp:Panel>
</div>

<uc1:ModalConfirmacao ID="ucModalConfirmacao" runat="server" />
