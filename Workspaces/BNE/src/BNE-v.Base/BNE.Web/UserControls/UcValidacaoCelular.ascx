<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="UcValidacaoCelular.ascx.cs" Inherits="BNE.Web.UserControls.UcValidacaoCelular" %>
<asp:Label runat="server" ID="lblValidacaoCelular" Text="Código de validação" AssociatedControlID="txtCodigoValidacao" CssClass="label_principal" />
<div class="container_campo">
    <componente:AlfaNumerico ID="txtCodigoValidacao" runat="server" Columns="10" ValidationGroup="CodigoValidacao" OnValorAlterado="txtCodigoValidacao_OnValorAlterado" />
    <asp:Image runat="server" ID="imgCodigoValido" ImageUrl="../img/icn_checado.png" AlternateText="Código válido!" ToolTip="Código válido!" Visible="False" Style="float: none" />
    <span class="nao-recebeu-codigo"><%--Não recebeu o código ainda?--%><asp:LinkButton ID="btlEnviarNovoCodigo" runat="server"  CssClass="button_nao_recebeu_cod" OnClick="btlEnviarNovoCodigo_Click" CausesValidation="False"><i class="fa fa-paper-plane"></i> Enviar novo código</asp:LinkButton></span>
</div>
