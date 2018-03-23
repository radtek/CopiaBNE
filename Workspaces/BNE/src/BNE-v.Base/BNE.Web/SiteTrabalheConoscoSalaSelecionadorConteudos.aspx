<%@ Page Title=""
    Language="C#"
    MasterPageFile="~/Master/Principal.Master"
    AutoEventWireup="true"
    CodeBehind="SiteTrabalheConoscoSalaSelecionadorConteudos.aspx.cs"
    Inherits="BNE.Web.SiteTrabalheConoscoSalaSelecionadorConteudos" %>

<asp:Content
    ID="conHead"
    ContentPlaceHolderID="cphHead"
    runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SiteTrabalheConoscoSalaSelecionadorConteudos.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicScript runat="server" Src="/js/local/SiteTrabalheConoscoSalaSelecionadorConteudos.js" type="text/javascript" />
</asp:Content>
<asp:Content
    ID="conConteudo"
    ContentPlaceHolderID="cphConteudo"
    runat="server">
    <h1>
        Conteúdos</h1>
    <asp:Panel
        ID="pnlConteudos"
        runat="server"
        CssClass="painel_padrao">
        <div class="painel_padrao_topo">
            &nbsp;</div>
        <p>
            Digite o conteúdo
            desejado nos
            campos abaixo:</p>
        <h2>
            Página Inicial</h2>
        <telerik:RadEditor
            CssClass="radeditor_pagina_inicial"
            Height="180px"
            ID="rePaginaInicial"
            runat="server"
            SkinID="RadEditorControlesBasicos"
            Width="850px">
        </telerik:RadEditor>
        <h2>
            E-mail de
            Boas Vindas
            ao Candidato</h2>
        <telerik:RadEditor
            CssClass="radeditor_boas_vindas_candidato"
            Height="180px"
            ID="reBoasVindasCandidato"
            runat="server"
            SkinID="RadEditorControlesBasicos"
            Width="850px">
        </telerik:RadEditor>
    </asp:Panel>
    <asp:Panel
        ID="pnlBotoes"
        runat="server"
        CssClass="painel_botoes">
        <asp:UpdatePanel
            ID="upBotoes"
            runat="server"
            UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button
                    ID="btnSalvar"
                    runat="server"
                    CssClass="botao_padrao"
                    Text="Salvar"
                    CausesValidation="false"
                    OnClick="btnSalvar_Click" />
                <asp:Button
                    ID="btnVoltar"
                    runat="server"
                    CssClass="botao_padrao"
                    Text="Voltar"
                    CausesValidation="false"
                    PostBackUrl="SiteTrabalheConoscoMenu.aspx" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
<asp:Content
    ID="conRodape"
    ContentPlaceHolderID="cphRodape"
    runat="server">
</asp:Content>
