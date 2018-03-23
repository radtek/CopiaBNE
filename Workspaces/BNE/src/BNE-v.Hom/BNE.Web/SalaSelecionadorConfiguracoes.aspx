<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SalaSelecionadorConfiguracoes.aspx.cs" Inherits="BNE.Web.SalaSelecionadorConfiguracoes" %>

<%@ Register Src="UserControls/Modais/ConfirmacaoExclusao.ascx" TagName="ConfirmacaoExclusao"
    TagPrefix="uc1" %>
<asp:Content ID="conHead" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="conConteudo" ContentPlaceHolderID="cphConteudo" runat="server">
    <h1>Configurações</h1>
    <asp:Panel ID="pnlConteudos" runat="server" CssClass="painel_padrao">
        <div class="painel_padrao_topo">
            &nbsp;
        </div>
        <p>
            Personalize o e-mail será enviado automaticamente para o candidato excluído da sua
            vaga.
        </p>
        <telerik:RadEditor CssClass="radeditor_pagina_inicial" Height="180px" ID="reExclusaoCandidatura"
            runat="server" SkinID="RadEditorControlesBasicos" Width="850px">
        </telerik:RadEditor>
        <p>
            Personalize o e-mail que será enviado automaticamente para o candidato que se candidatou à sua
            vaga.
        </p>
        <telerik:RadEditor CssClass="radeditor_pagina_inicial" Height="180px" ID="reAgradecimentoCandidatura"
            runat="server" SkinID="RadEditorControlesBasicos" Width="850px">
        </telerik:RadEditor>
        <p>
            Personalize o texto que será exibido como a descrição de sua empresa.
        </p>
        <telerik:RadEditor CssClass="radeditor_pagina_inicial" Height="180px" ID="reCartaApresentacao"
            runat="server" SkinID="RadEditorControlesBasicos" Width="850px">
        </telerik:RadEditor>
    </asp:Panel>
    <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
        <asp:UpdatePanel ID="upBotoes" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="btnSalvar" runat="server" CssClass="botao_padrao" Text="Salvar" CausesValidation="false"
                    OnClick="btnSalvar_Click" />
                <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
                    PostBackUrl="SalaSelecionador.aspx" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
<asp:Content ID="conRodape" ContentPlaceHolderID="cphRodape" runat="server">
    <uc1:ConfirmacaoExclusao ID="ucConfirmacaoExclusao" runat="server" />
</asp:Content>
