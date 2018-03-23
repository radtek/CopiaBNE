<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ucCurso.ascx.cs" Inherits="BNE.Web.UserControls.ucCurso" %>
<div class="container_curso">
    <div class="imagem_miniatura">
        <asp:HyperLink runat="server" ID="hlImagemCurso"></asp:HyperLink>
    </div>
    <div class="conteudo">
        <div class="curso_titulo">
            <asp:Literal runat="server" ID="litTitulo"></asp:Literal>
        </div>
        <div class="curso_carga_horaria">
            Carga horária:
            <asp:Literal runat="server" ID="litCargaHoraria"></asp:Literal>
            horas
        </div>
        <div class="curso_valor">
            Valor: R$
            <asp:Literal runat="server" ID="litValor"></asp:Literal>
        </div>
        <asp:Panel ID="pnlParcela" runat="server">
            <div class="curso_valor_parcela">
                ou em
                <asp:Literal runat="server" ID="litQuantidadeParcela"></asp:Literal>
                vezes de R$
                <asp:Literal runat="server" ID="liValorParcela"></asp:Literal>
            </div>
        </asp:Panel>
    </div>
    <div class="curso_botao">
        <asp:ImageButton ID="btiDetalhes" AlternateText="Detalhes" ImageUrl="/img/btn_ver_curso.png"
            CommandName="detalhes" runat="server" />
    </div>
</div>
