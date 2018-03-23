<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExportarCurriculo.ascx.cs" Inherits="BNE.Web.UserControls.Modais.ExportarCurriculo" %>
<asp:Panel ID="pnlExportarCurriculo" CssClass="modal_conteudo candidato reduzida painel_exportar_curriculo"
    Style="display: none;" runat="server">
    <asp:UpdatePanel ID="upExportarCurriculo" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <h2 class="titulo_modal">
                <asp:Label ID="Label1" runat="server" Text="Exportar Currículo"></asp:Label>
            </h2>
            <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:ImageButton CssClass="botao_fechar_modal" ID="btiFechar" ImageUrl="/img/botao_padrao_fechar.gif"
                        runat="server" CausesValidation="false" OnClick="btiFechar_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Panel CssClass="coluna_esquerda" ID="pnlColunaEsquerda" runat="server">
                <asp:Panel CssClass="painel_info_vip" ID="pnlEsquerdaExportarCurriculo" runat="server">
                    <asp:Image CssClass="logo_empresa" ID="imgLogo" ImageUrl="/img/SalaAdministrador/icn_empresas.png" AlternateText=""
                        runat="server" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="painel_empresa_exportar_curriculo" ID="pnlExportarCurriculoCentro" runat="server">
                <div class="linha">
                    <asp:Label runat="server" ID="IdNomeCandidato" CssClass="label_principal modal" Text=" Nome do Candidato:" />
                    <div class="container_campo modal">
                        <asp:Literal ID="lblNomeCandidato" runat="server"></asp:Literal></div>
                </div>
                <div class="linha">
                    <asp:Label runat="server" ID="IdSiteTrabalheConoscoEscolhe" CssClass="label_principal modal" Text="Escolha o Exclusivo Banco de Currículos:" />
                    <telerik:RadComboBox ID="rcbExportarCurriculo" CssClass="" runat="server" ZIndex="200000" Width="300px"
                        ValidationGroup="Exportar">
                    </telerik:RadComboBox>
                </div>
                <asp:Panel CssClass="painel_botoes" ID="pnlBTNExportarCurriculo" runat="server">
                    <asp:Button ID="btnExportarCurriculo" runat="server" Text="Exportar" CssClass="botao_padrao" CausesValidation="true"
                        ValidationGroup="Exportar" OnClick="btnExportarCurriculo_Click" />
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:HiddenField ID="hfVariavel" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeExportarCurriculo" TargetControlID="hfVariavel" PopupControlID="pnlExportarCurriculo"
    runat="server">
</AjaxToolkit:ModalPopupExtender>
