<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuemMeViu.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaVip.QuemMeViu" %>

  <asp:HiddenField runat="server" ID="hdfIdc" ClientIDMode="Static" />
    <script src="../../../js/local/Forms/SalaVip/QuemMeViu.js"></script>
    <script src="../../../js/bootstrap/jquery.bootpag.min.js"></script>

<%--INICIO NOVA GRID--%>

<section>
    <div class="panel-group panel-group-ja-enviei" id="accordion" role="tablist" aria-multiselectable="true">
        <div id="divQuemMeViu"></div>
        <div class="text-center">
            <p class="paginacaobootstrap"></p>
        </div>
    </div>
</section>

<%--FIM NOVA GRID--%>


   <%-- Botões --%>
        <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
            <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
                OnClick="btnVoltar_Click" />
        </asp:Panel>
        <%-- Fim Botões --%>
