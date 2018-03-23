<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucEndereco.ascx.cs"
    Inherits="BNE.Web.UserControls.ucEndereco" %>
<%@ Register Src="~/UserControls/BuscarCEP.ascx" TagName="BuscarCEP" TagPrefix="uc1" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/ucEndereco.css" type="text/css" rel="stylesheet" />
<script>
    autocomplete.cidade("txtCidadeEndereco");
</script>
<%-- Linha CEP --%>
<div class="linha">
    <asp:Label ID="lblCEP" CssClass="label_principal" runat="server" Text="CEP" AssociatedControlID="txtCEP" />
    <div class="container_campo">
        <asp:UpdatePanel ID="upTxtCEP" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <componente:CEP ID="txtCEP" runat="server" OnValorAlterado="txtCEP_ValorAlterado"
                    ValidationGroup="" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <uc1:BuscarCEP ID="ucBuscarCEP" runat="server" OnVoltarFoco="ucBuscarCEP_VoltarFoco" />
</div>
<%-- FIM: Linha CEP --%>
<%-- Linha Endereco --%>
<div class="linha">
    <asp:Label ID="lblLogradouro" CssClass="label_principal" runat="server" Text="Endereço"
        AssociatedControlID="txtLogradouroEndereco" />
    <div class="container_campo">
        <div>
            <asp:RequiredFieldValidator ID="rfvLogradouroEndereco" runat="server" ErrorMessage="Campo Obrigatório."
                ControlToValidate="txtLogradouroEndereco" ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
        </div>
        <asp:UpdatePanel ID="upTxtLogradouroEndereco" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:TextBox Columns="40" ID="txtLogradouroEndereco" runat="server"></asp:TextBox>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%-- Linha Numero --%>
    <asp:Label CssClass="label_numero" ID="lblNumero" runat="server" Text="Número" AssociatedControlID="txtNumero" />
    <div class="container_campo">
        <div>
            <asp:RequiredFieldValidator ID="rfvNumero" runat="server" ErrorMessage="Campo Obrigatório."
                ControlToValidate="txtNumero" ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
        </div>
        <asp:TextBox ID="txtNumero" Columns="5" runat="server" MaxLength="15"></asp:TextBox>
    </div>
    <%-- FIM: Linha Numero --%>
    <%-- FIM: Linha Endereco --%>
    <%-- Linha Complemento --%>
    
</div>
<div class="linha">
        <asp:Label ID="lblComplemento" CssClass="label_principal" runat="server" Text="Complemento"
            AssociatedControlID="txtComplementoEndereco" />
        <div class="container_campo">
            <asp:TextBox Columns="25" ID="txtComplementoEndereco" runat="server" CssClass="textbox_padrao textbox_complemento"></asp:TextBox>
        </div>
        <%-- Linha Bairro --%>
        <asp:Label CssClass="label_bairro" ID="lblBairro" AssociatedControlID="txtBairroEndereco"
            runat="server" Text="Bairro" />
        <div class="container_campo">
            <div>
                <asp:RequiredFieldValidator ID="rfvBairroEndereco" runat="server" ErrorMessage="Campo Obrigatório."
                    ControlToValidate="txtBairroEndereco" ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
            </div>
            <asp:UpdatePanel ID="upTxtBairroEndereco" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:TextBox ID="txtBairroEndereco" runat="server" MaxLength="50"></asp:TextBox>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <%-- FIM: Linha Bairro --%>
    </div>
<%-- FIM: Linha Complemento --%>
<%-- Linha Cidade --%>
<div class="linha">
    <asp:Label ID="lblCidade" runat="server" Text="Cidade" CssClass="label_principal"
        AssociatedControlID="txtCidadeEndereco" />
    <div class="container_campo">
        <asp:CustomValidator ID="cvCidadeEndereco" runat="server" ControlToValidate="txtCidadeEndereco"
            ValidationGroup="CadastroDadosPessoais" ErrorMessage="Valor Incorreto." ClientValidationFunction="cvCidadeDadosPessoais_Validate"></asp:CustomValidator>
        <asp:RequiredFieldValidator ID="rfvCidadeEndereco" runat="server" ErrorMessage="Campo Obrigatório."
            ControlToValidate="txtCidadeEndereco" ValidationGroup="CadastroDadosPessoais"></asp:RequiredFieldValidator>
        <asp:UpdatePanel ID="upTxtCidadeEndereco" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:TextBox ID="txtCidadeEndereco" runat="server" MaxLength="50"></asp:TextBox>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>

<%-- FIM: Linha Cidade --%>