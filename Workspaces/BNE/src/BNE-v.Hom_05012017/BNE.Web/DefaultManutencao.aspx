<%@ Page Title=""
    Language="C#"
    MasterPageFile="~/Master/Principal.Master"
    AutoEventWireup="true"
    CodeBehind="DefaultManutencao.aspx.cs"
    Inherits="BNE.Web.DefaultManutencao" %>

<asp:Content
    ID="Content1"
    ContentPlaceHolderID="cphHead"
    runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/Manutencao.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content
    ID="Content2"
    ContentPlaceHolderID="cphConteudo"
    runat="server">
    <div class="coluna_esquerda">
        <asp:ImageButton
            ID="btiConteudos"
            ImageUrl="~/img/btn_manutencao_conteudos.png"
            CssClass="btn esquerda btn_01"
            Title="Conteúdos"
            PostBackUrl="ManutencaoConteudosParametrosControle.aspx"
            runat="server" />
        <asp:ImageButton
            ID="btiCadastros"
            ImageUrl="~/img/btn_manutencao_cadastros.png"
            CssClass="btn esquerda btn_02"
            Title="Cadastros"
            PostBackUrl="ManutencaoPalavrasProibidas.aspx"
            runat="server" />
        <asp:ImageButton
            ID="btiFinanceiro"
            ImageUrl="~/img/btn_manutencao_financeiro.png"
            CssClass="btn esquerda btn_03"
            Title="Financeiro"
            PostBackUrl="ManutencaoFinanceiroBoleto.aspx"
            runat="server" />
        <asp:ImageButton
            ID="btiRelatorios"
            ImageUrl="~/img/btn_manutencao_relatorios.png"
            CssClass="btn esquerda btn_04"
            Title="Relatórios"
            PostBackUrl="ManutencaoRelatorios.aspx"
            runat="server" />
    </div>
    <div class="coluna_direita">
        <asp:ImageButton
            ID="btiRHOffice"
            ImageUrl="~/img/btn_manutencao_rhoffice.png"
            CssClass="btn btn_05"
            Title="RH Office"
            PostBackUrl="ManutencaoRHOffice.aspx"
            runat="server" />
        <asp:ImageButton
            ID="btiRedesSociais"
            ImageUrl="~/img/btn_manutencao_redes_sociais.png"
            CssClass="btn btn_06"
            Title="Redes Sociais"
            PostBackUrl="ManutencaoRedesSociais.aspx"
            runat="server" />
        <asp:ImageButton
            ID="ImageButton1"
            ImageUrl="~/img/btn_manutencao_cadastros_usuario_internos.png"
            CssClass="btn btn_07"
            Title="Cadastro de Usuários Internos"
            PostBackUrl="UsuarioCadastro.aspx"
            runat="server" />
        <asp:ImageButton
            ID="ImageButton2"
            ImageUrl="~/img/btn_manutencao_listagem_perfis.png"
            CssClass="btn btn_08"
            Title="Listagem de Perfis"
            PostBackUrl="PerfilUsuarioConsulta.aspx"
            runat="server" />
    </div>
</asp:Content>
<asp:Content
    ID="Content3"
    ContentPlaceHolderID="cphRodape"
    runat="server">
</asp:Content>
