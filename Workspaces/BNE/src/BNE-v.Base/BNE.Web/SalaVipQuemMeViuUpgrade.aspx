<%@ Page Title=""
    Language="C#"
    MasterPageFile="~/Master/Principal.Master"
    AutoEventWireup="true"
    CodeBehind="SalaVipQuemMeViuUpgrade.aspx.cs"
    Inherits="BNE.Web.SalaVipQuemMeViuUpgrade" %>

<%@ Register
    Src="UserControls/Forms/SalaVip/QuemMeViu.ascx"
    TagName="QuemMeViu"
    TagPrefix="uc1" %>
<%@ Register
    Src="UserControls/Modais/ModalConfirmacao.ascx"
    TagName="ModalConfirmacao"
    TagPrefix="uc2" %>
<asp:Content
    ID="Content1"
    ContentPlaceHolderID="cphHead"
    runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/sala_vip.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaVipQuemMeViu.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content
    ID="Content2"
    ContentPlaceHolderID="cphConteudo"
    runat="server">
    <asp:UpdatePanel
        ID="upQuemMeViuUpgrade"
        runat="server"
        UpdateMode="Conditional">
        <ContentTemplate>
            <div class="painel_padrao_sala_vip">
                <h2 class="alerta_h2">
                    <img src="img/alerta_cv_nao_visualizado.png" />
                    <span>Seu currículo
                        ainda não
                        foi visualizado
                        por nenhuma
                        empresa.</span></h2>
                <p>
                    Verifique:</p>
                <ul class="item">
                    <li>Você pode
                        adicionar
                        até três funções
                        em seu currículo,
                        <asp:LinkButton
                            ID="lbAtualizeAquiFuncoes"
                            Text="Atualize aqui"
                            runat="server"
                            OnClick="lbAtualizeAquiFuncoes_Click"></asp:LinkButton>
                    <li>Você pode
                        adicionar
                        outra cidade
                        que deseja
                        trabalhar,
                        <asp:LinkButton
                            ID="lbAtualizaAquiOutraCidade"
                            Text="Atualize aqui"
                            runat="server"
                            OnClick="lbAtualizaAquiOutraCidade_Click"></asp:LinkButton>
                    <li>Sua pretensão
                        salarial pode
                        estar fora
                        da média praticada
                        no mercado,
                        <asp:LinkButton
                            ID="lbAtualizeAquiPretensaoSalarial"
                            Text="Atualize aqui"
                            runat="server"
                            OnClick="lbAtualizeAquiPretensaoSalarial_Click"></asp:LinkButton>
                </ul>
            </div>
            <%-- Botões --%>
            <asp:Panel
                ID="pnlBotoes"
                runat="server"
                CssClass="painel_botoes">
                <asp:Button
                    ID="btnAtualizarCurriculo"
                    runat="server"
                    CssClass="botao_padrao"
                    Text="Atualizar Currículo"
                    CausesValidation="false"
                    OnClick="btnAtualizarCurriculo_Click" />
<%-- Optado por remove a função (regra de negócio)
    <asp:Button
                    ID="btnPecaAjuda"
                    runat="server"
                    CssClass="botao_padrao"
                    Text="Peça Ajuda"
                    CausesValidation="false"
                    OnClick="btnPecaAjuda_Click" />--%>
                <asp:Button
                    ID="btnVoltar"
                    runat="server"
                    CssClass="botao_padrao"
                    Text="Voltar"
                    CausesValidation="false"
                    OnClick="btnVoltar_Click" />
            </asp:Panel>
            <%-- Fim Botões --%>
            <uc2:ModalConfirmacao
                ID="ucModalConfirmacao"
                runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content
    ID="Content3"
    ContentPlaceHolderID="cphRodape"
    runat="server">
</asp:Content>
