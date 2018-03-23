<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JaEnviei.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaVip.JaEnviei" %>
<div class="painel_padrao_sala_vip">
    <p>
        Confira as <span class="texto_vaga">VAGAS</span> que você já se candidatou..
    </p>
    <asp:HiddenField runat="server" ID="hdfIdc" ClientIDMode="Static" />
    <script src="../../../js/local/Forms/SalaVip/JaEnviei.js"></script>
    <script src="../../../js/bootstrap/jquery.bootpag.min.js"></script>
</div>


<%--INICIO NOVA GRID--%>

<section>
    <div class="panel-group panel-group-ja-enviei" id="accordion" role="tablist" aria-multiselectable="true">
        <div id="divCandidaturas"></div>
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
