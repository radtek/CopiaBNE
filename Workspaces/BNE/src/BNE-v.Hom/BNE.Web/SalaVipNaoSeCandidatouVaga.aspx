<%@ Page Title=""
    Language="C#"
    MasterPageFile="~/Master/Principal.Master"
    AutoEventWireup="true"
    CodeBehind="SalaVipNaoSeCandidatouVaga.aspx.cs"
    Inherits="BNE.Web.SalaVipNaoSeCandidatouVaga" %>

<asp:Content
    ID="Content1"
    ContentPlaceHolderID="cphHead"
    runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/sala_vip.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaVipJaEnviei.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content
    ID="Content2"
    ContentPlaceHolderID="cphConteudo"
    runat="server">
    <asp:UpdatePanel
        ID="upNaoSeCandidatouVaga"
        runat="server"
        UpdateMode="Conditional">
        <ContentTemplate>
            <div class="painel_padrao_sala_vip">
                <%-- Aviso de Candidatação --%>
                <div class="painel_padrao_sala_vip textocandidato">
                    <span>
                        <asp:Image
                            ID="imgNaoSeCandidatouVaga"
                            runat="server"
                            AlternateText="Você ainda não se candidatou para nenhuma vaga!"
                            ImageUrl="~/img/texto_candidatacao.png" />
                    </span>
                </div>
                <%-- Fim Aviso de Candidatação --%>
            </div>
            <%-- Botões --%>
            <asp:Panel
                ID="pnlBotoes"
                runat="server"
                CssClass="painel_botoes">
                <asp:Button
                    ID="btnVerVagas"
                    runat="server"
                    CssClass="botao_padrao"
                    Text="Ver Vagas"
                    CausesValidation="false"
                    OnClick="btnVerVagas_Click" />
                <asp:Button
                    ID="btnVoltar"
                    runat="server"
                    CssClass="botao_padrao"
                    Text="Voltar"
                    CausesValidation="false"
                    PostBackUrl="/SalaVip.aspx"
                    OnClick="btnVoltar_Click" />
            </asp:Panel>
            <%-- Fim Botões --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content
    ID="Content3"
    ContentPlaceHolderID="cphRodape"
    runat="server">
</asp:Content>
