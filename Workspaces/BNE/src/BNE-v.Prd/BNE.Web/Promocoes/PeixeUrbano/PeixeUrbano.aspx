<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="PeixeUrbano.aspx.cs" Inherits="BNE.Web.Promocoes.PeixeUrbano.PeixeUrbano" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphExperimentos" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="css/peixeUrbano.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:UpdatePanel runat="server" ID="upConteudo" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="peixeurbano-background">
                <div id="peixeurbano-content">
                    <div id="peixeurbano-title">Agora você é VIP!</div>
                    <div id="peixeurbano-divider"></div>
                    <div id="peixeurbano-text">Valide seu cupom abaixo e aproveite <strong>todas as vantagens</strong> de ser VIP!</div>


                    <!-- Para cupom inválido utilizar .peixeurbano-error no #peixeurbano-cupom -->
                    <div id="peixeurbano-cupom" class="peixeurbano-error">
                        <asp:TextBox ID="txtCodigo" runat="server" placeholder="Inserir código Peixe Urbano"></asp:TextBox>
                        <%--  <input type="text" placeholder="Inserir código Peixe Urbano" id="peixeurbano-input">--%>
                        <div runat="server" visible="false" id="divCodigoInvalido">Código inválido</div>
                        <div runat="server" visible="false" id="divCodigoUtilizado">Cupom já utilizado</div>
                    </div>

                    <asp:Button runat="server" ID="btnValidar" OnClick="btnValidar_Click" Text="validar cupom" />
                    <%--       <input type="button" value="validar cupom" id="peixeurbano-button">--%>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hfVariavelModal" runat="server" />
    <AjaxToolkit:ModalPopupExtender ID="mdConfirmacao" runat="server" PopupControlID="pnlConfirmacao"
        TargetControlID="hfVariavelModal">
    </AjaxToolkit:ModalPopupExtender>
    <asp:Panel ID="pnlConfirmacao" runat="server">

        <!-- START- Modal -->
        <div id="peixeurbano-modal">
            <div class="peixeurbano-modal-icon">
                <img src="/promocoes/peixeurbano/img/icon-success.png"></div>
            <div class="peixeurbano-modal-content">
                <span class="peixeurbano-close">Código validado com sucesso!</span><br />
                <asp:Button runat="server" ID="btnConcluir" OnClick="btnConcluir_Click"  OnClientClick="trackEvent('Plano-Peixe-Urbano', 'Peixe-Urbano', 'ValidarCupon'); return true;" Text="Completar cadastro" class="peixeurbano-modal-button" />
                <%-- <div><a href="#"><input type="button"  class="peixeurbano-modal-button"></a><br></div>--%>
            </div>
        </div>
        <!-- END - Modal -->

    </asp:Panel>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
