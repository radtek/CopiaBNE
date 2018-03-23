<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NovasEmpresas.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.NovasEmpresas" %>
<div class="painel_configuracao_conteudo">
    <div class="linha">
        <asp:Label ID="lblDtIni" runat="server" Text="Data Inicial" AssociatedControlID="txtDataInicial" />
        <div class="container_campo">
            <componente:Data ID="txtDataInicial" runat="server" MensagemErroIntervalo="Data Inválida"
                CssClassTextBox="textbox_padrao" ValidationGroup="DataPeriodo" 
                MostrarCalendario="True" Obrigatorio="True" ValorAlterado="txtDt_ValorAlterado" />
        </div>
        <asp:Label ID="lblDtFim" runat="server" Text="Data Final" AssociatedControlID="txtDataFinal" />
        <div class="container_campo">
            <componente:Data ID="txtDataFinal" runat="server" ValidationGroup="DataPeriodo"
                MensagemErroIntervalo="Data Inválida" Obrigatorio="True" CssClassTextBox="textbox_padrao"
                MostrarCalendario="True" ValorAlterado="txtDt_ValorAlterado" />
        </div>
        <div class="container_campo">
            <asp:Button ID="btnGerar" runat="server" CssClass="botao_padrao painelinterno" 
                Text="Gerar" CausesValidation="true" onclick="btnGerar_Click1" />
        </div>
    </div>
    <asp:Panel CssClass="aviso" ID="pnlAviso" runat="server" Visible="false">
        <div style="margin-top:190px">
            <img src="../../../img/img_icone_aviso.png" />
            <p>
                <asp:Label ID="lbAviso" runat="server" CssClass="texto_aviso"></asp:Label>
            </p>
        </div>
    </asp:Panel>
</div>
