<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="ConcessaoVip.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.ConcessaoVip" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/VerDadosEmpresa.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlConcessaoVip" CssClass="modal_conteudo candidato reduzida" Style="display: none;"
    runat="server">
    <asp:UpdatePanel ID="upConcessaoVip" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <h2 class="titulo_modal">
                <asp:Label ID="Label1" runat="server"></asp:Label>
            </h2>
            <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:ImageButton CssClass="botao_fechar_modal" ID="btiFechar" ImageUrl="/img/botao_padrao_fechar.gif"
                        runat="server" CausesValidation="false" OnClick="btiFechar_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Panel CssClass="coluna_esquerda" ID="pnlColunaEsquerda" runat="server">
                <asp:Panel CssClass="painel_info_vip" ID="pnlEsquerdaInfoVip" runat="server">
                    <asp:Image CssClass="logo_empresa" ID="imgLogo" ImageUrl="/img/SalaAdministrador/icn_vip.png"
                        AlternateText="" runat="server" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="painel_empresa_confidencial" ID="pnlLiberarVIP" runat="server"
                Visible="false">
                <p>
                    Nome do Candidato:
                    <asp:Literal ID="lblNomeCandidatoLiberar" runat="server"></asp:Literal>
                </p>
                <p>
                    Tipo de VIP:
                    <telerik:RadComboBox ID="rcbPlanos" CssClass="" runat="server" ZIndex="100000">
                    </telerik:RadComboBox>
                    
                </p>
                <p>
                    <asp:Button ID="btnLiberarVIP" runat="server" Text="Liberar" CssClass="botao_padrao"
                        CausesValidation="false" OnClick="btnLiberar_Click" />
                </p>
            </asp:Panel>
            <asp:Panel CssClass="painel_empresa_confidencial" ID="pnlCancelarVIP" runat="server">
                <p>
                    Nome do Candidato:
                    <asp:Literal ID="lblNomeCandidatoCancelar" runat="server"></asp:Literal>
                </p>
                <p>
                    Tipo de VIP:
                    <asp:Literal ID="lblPlanoVIPAtual" runat="server"></asp:Literal>
                </p>
                <p>
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar VIP" CssClass="botao_padrao"
                        CausesValidation="false" OnClick="btnCancelar_Click" />
                </p>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:HiddenField ID="hfVariavel" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeConcessaoVip" TargetControlID="hfVariavel"
    PopupControlID="pnlConcessaoVip" runat="server">
</AjaxToolkit:ModalPopupExtender>
<%--Painel Confirmacao Envio--%>
<%-- FIM: Painel Confirmacao Envio--%>