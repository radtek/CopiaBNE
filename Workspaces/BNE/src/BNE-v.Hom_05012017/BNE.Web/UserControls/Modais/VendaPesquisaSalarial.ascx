<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VendaPesquisaSalarial.ascx.cs" Inherits="BNE.Web.UserControls.Modais.VendaPesquisaSalarial" %>
<link href="//fonts.googleapis.com/css?family=Roboto:400,900italic,700italic,900,700,500italic,500,300italic,400italic,300,100italic,100" rel="stylesheet" type="text/css">
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/VendaPesquisaSalarial.css" type="text/css" rel="stylesheet" />
<Employer:DynamicScript runat="server" Src="/js/local/UserControls/VendaPesquisaSalarial.js" type="text/javascript" />
<div id="modalVendaInformacaoEmpresa" class="modal fade hide modal-container modal-venda-informacoes" tabindex="-1" role="dialog">
    <div class="modal-header">
        <h4 class="title">Conteúdo exclusivo... :(</h4>
        <i class="fa fa-times"></i>
    </div>
    <div class="modal-body">
        <p class="aviso">Esse serviço está disponível apenas para clientes Salário BR...</p>
        <p class="descricao">Se deseja ter acesso a essas informações exclusivas, preencha seus dados e solicite uma proposta:</p>
        <div class="campo">
            <asp:Label runat="server" AssociatedControlID="txtNomeEmpresa" Text="Nome da Empresa"></asp:Label>
            <div class="container-campo">
                <asp:UpdatePanel ID="upTxtEmpresa" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <componente:AlfaNumerico ID="txtNomeEmpresa" runat="server" ValidationGroup="Enviar" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="campo">
            <asp:Label runat="server" AssociatedControlID="txtFuncao" Text="Função"></asp:Label>
            <div class="container-campo">
                <asp:RequiredFieldValidator ID="rfvFuncao" runat="server" ControlToValidate="txtFuncao"
                    ValidationGroup="Enviar"></asp:RequiredFieldValidator>
                <asp:UpdatePanel ID="upTxtFuncao" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:TextBox ID="txtFuncao" runat="server"></asp:TextBox>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="campo">
            <asp:Label runat="server" AssociatedControlID="txtEmail" Text="Email"></asp:Label>
            <span>
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                    ValidationGroup="Enviar" ErrorMessage="Email Inválido.">
                </asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                    ValidationGroup="Enviar"></asp:RequiredFieldValidator>
            </span>
            <div class="container-campo">
                <asp:UpdatePanel ID="upTxtEmail" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:TextBox ID="txtEmail" runat="server" Columns="50" MaxLength="50"></asp:TextBox>
                        <AjaxToolkit:AutoCompleteExtender ID="aceEmail" runat="server" TargetControlID="txtEmail"
                            UseContextKey="False" ServiceMethod="ListarSugestaoEmail">
                        </AjaxToolkit:AutoCompleteExtender>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="campo">
            <asp:Label runat="server" AssociatedControlID="txtTelefone" Text="Telefone"></asp:Label>
            <div class="container-campo">
                <asp:UpdatePanel ID="upTxtTelefone" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <componente:Telefone ID="txtTelefone" Obrigatorio="true" runat="server" ValidationGroup="Enviar" Tipo="FixoCelular" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div class="modal-footer">
        <asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <button id="btnCancel" class="btn_cancel">CANCELAR</button>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:LinkButton ID="btnEnviar" runat="server" class="btn_confirm" OnClick="btnEnviar_Click" ValidationGroup="Enviar" Text="ENVIAR" />
    </div>
</div>
