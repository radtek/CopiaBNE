<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AguardeProcessando.ascx.cs" 
    Inherits="BNE.Web.UserControls.Modais.AguardeProcessando" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/ModalConfirmacaoEnvioCurriculo.css" type="text/css" rel="stylesheet" />
<asp:UpdatePanel ID="upModalAguarde" runat="server" UpdateMode="Conditional">
    <ContentTemplate>

        <asp:HiddenField ID="hfModalAguarde" runat="server" />
        <asp:Panel ID="pnlModalAguarde" runat="server" CssClass="modal_confirmacao_registro candidato reduzida"
            Style="display: none">


            <asp:UpdatePanel ID="upConteudo" runat="server" UpdateMode="Conditional">

                <ContentTemplate>
                    <h2 class="titulo_modal">
                        <asp:Label ID="lblTitulo" runat="server" Text="Aguarde"></asp:Label></h2>
                    <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <p class="texto_cadastro_sucesso">
                        <asp:Label ID="lblTexto" runat="server" Text="Por Favor Aguarde!
                        A sua transação esta sendo processada." ></asp:Label>
                        <asp:Label ID="lblTextoAuxiliar" runat="server" Visible="false" CssClass="texto_menor"></asp:Label>
           
                    </p>
                </ContentTemplate>
            </asp:UpdatePanel>
       
        </asp:Panel>
        <AjaxToolkit:ModalPopupExtender ID="mpeModalAguarde" runat="server" PopupControlID="pnlModalAguarde"
            TargetControlID="hfModalAguarde" />
    </ContentTemplate>
</asp:UpdatePanel>
