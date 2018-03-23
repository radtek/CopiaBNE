<%@ Page Title=""
    Language="C#"
    MasterPageFile="~/Master/Principal.Master"
    AutoEventWireup="true"
    CodeBehind="SalaAdministradorFinanceiro.aspx.cs"
    Inherits="BNE.Web.SalaAdministradorFinanceiro" %>

<%@ Register
    Src="UserControls/Forms/SalaAdministrador/PlanoFidelidade.ascx"
    TagName="PlanoFidelidade"
    TagPrefix="uc1" %>
<%@ Register
    Src="UserControls/Forms/SalaAdministrador/TipoPlano.ascx"
    TagName="PlanoFidelidade"
    TagPrefix="uc2" %>
<%@ Register
    Src="UserControls/Forms/SalaAdministrador/DetalhesPlano.ascx"
    TagName="PlanoFidelidade"
    TagPrefix="uc3" %>
<%@ Register
    Src="UserControls/Forms/SalaAdministrador/LiberacaoBoleto.ascx"
    TagName="LiberacaoBoleto"
    TagPrefix="uc4" %>
<%@ Register
    Src="UserControls/Forms/SalaAdministrador/PagamentosCielo.ascx"
    TagName="PagamentosCielo"
    TagPrefix="uc5" %>
<%@ Register
    Src="UserControls/Forms/SalaAdministrador/DownloadBoletoRegistrado.ascx"
    TagName="DownloadBoletoRegistrado"
    TagPrefix="uc6" %>

<asp:Content
    ID="Content1"
    ContentPlaceHolderID="cphHead"
    runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaAdministrador.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaAdministradorFinanceiro.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content
    ID="Content2"
    ContentPlaceHolderID="cphConteudo"
    runat="server">
    <div class="painel_padrao_sala_adm">
        <%-- Configuração do Layout do Menu--%>
        <asp:Panel
            ID="pnlContainerFinanceiro"
            CssClass="container_financeiro"
            runat="server">
            <asp:Panel
                ID="pnlTipoFinanceiro"
                CssClass="tipo_financeiro"
                runat="server">
                <div class="barra_lateral">
                    <%--       <div class="bordas_superior">
                        <span class="borda_sup_esquerda">
                            <img src="img/bg_lado_esq_sup.png" /></span>
                        <span class="borda_sup_direita">
                            <img src="img/bg_lado_dir_sup.png" /></span>
                    </div>--%>
                    <div class="menu">
                        <ul class="menu_financeiro">
                            <asp:LinkButton
                                ID="btlPlano"
                                runat="server"
                                CausesValidation="false"
                                ToolTip="Plano"
                                OnClick="btlPlano_Click">
                                <li class="selected">
                                    Plano
                                </li>
                            </asp:LinkButton>
                            <asp:LinkButton
                                ID="btlTipoPlano"
                                runat="server"
                                CausesValidation="false"
                                ToolTip="Tipo de Plano"
                                OnClick="btlTipoPlano_Click">
                                <li class="selected">
                                    Tipo de Plano
                                </li>
                            </asp:LinkButton>
                            <asp:LinkButton
                                ID="btlRetornoBoleto"
                                runat="server"
                                CauseValidation="false"
                                ToolTip="Liberação de Boleto"
                                OnClick="btlRetornoBoleto_Click">
                                <li class="ultimoitem">
                                    Liberação de Boleto
                                </li>
                            </asp:LinkButton>
                            <asp:LinkButton
                                ID="lkbPagamentosCielo"
                                runat="server"
                                CauseValidation="false"
                                ToolTip="Liberação de Boleto"
                                OnClick="lkbPagamentosCielo_Click">
                                <li class="ultimoitem">
                                    Pagamentos CIELO
                                </li>
                            </asp:LinkButton>
                            <asp:LinkButton
                                ID="lkbDownloadBolReg"
                                runat="server"
                                CauseValidation="false"
                                ToolTip="Download dos Boletos Registrados"
                                OnClick="lkbDownloadBolReg_Click">
                                <li class="ultimoitem">
                                    Download Boleto Registrados
                                </li>
                            </asp:LinkButton>
                        </ul>
                    </div>
                    <%--   <div class="bordas_inferior">
                        <span class="borda_inf_esquerda">
                            <img src="img/bg_lado_esq_inf.png" /></span>
                        <span class="borda_inf_direita">
                            <img src="img/bg_lado_dir_inf.png" /></span>
                    </div>--%>
                </div>
            </asp:Panel>
            <%-- Fim Configuração do Layout do Menu--%>
            <%-- Configuração do Conteudo do Layout do Financeiro--%>
            <div class="painel_financeiro_conteudo">
                <asp:UpdatePanel
                    ID="upPlanoFidelidade"
                    runat="server"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel
                            ID="pnlPlanoFidelidade"
                            runat="server"
                            Visible="false">
                            <uc1:PlanoFidelidade
                                ID="ucPlanoFidelidade"
                                runat="server" />
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger
                            ControlID="btlPlano"
                            EventName="Click" />
                        <asp:AsyncPostBackTrigger
                            ControlID="btlTipoPlano"
                            EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel
                    ID="upTipoPlano"
                    runat="server"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel
                            ID="pnlTipoPlano"
                            runat="server"
                            Visible="false">
                            <uc2:PlanoFidelidade
                                ID="ucTipoPlano"
                                runat="server" />
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger
                            ControlID="btlPlano"
                            EventName="Click" />
                        <asp:AsyncPostBackTrigger
                            ControlID="btlTipoPlano"
                            EventName="Click" />

                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel
                    ID="upDetalhesPlano"
                    runat="server"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel
                            ID="pnlDetalhesPlano"
                            runat="server"
                            Visible="false">
                            <uc3:PlanoFidelidade
                                ID="ucDetalhesPlano"
                                runat="server" />
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger
                            ControlID="btlPlano"
                            EventName="Click" />
                        <asp:AsyncPostBackTrigger
                            ControlID="btlTipoPlano"
                            EventName="Click" />

                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel
                    ID="upLiberacaoBoleto"
                    runat="server"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel
                            ID="pnlLiberacaoBoleto"
                            runat="server"
                            Visible="false">
                            <uc4:LiberacaoBoleto
                                ID="ucLiberacaoBoleto"
                                runat="server" />
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger
                            ControlID="btlRetornoBoleto"
                            EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel
                    ID="upPagamentosCielo"
                    runat="server"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel
                            ID="pnlPagamentosCielo"
                            runat="server"
                            Visible="false">
                            <uc5:PagamentosCielo
                                ID="PagamentosCielo"
                                runat="server" />
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger
                            ControlID="lkbPagamentosCielo"
                            EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:UpdatePanel
                    ID="upDownloadBoletoRegistrado"
                    runat="server"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel
                            ID="pnDownloadBoletoRegistrado"
                            runat="server"
                            Visible="false">
                            <uc6:DownloadBoletoRegistrado
                                ID="DownloadBoletoRegistrado"
                                runat="server" />
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger
                            ControlID="lkbDownloadBolReg"
                            EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <%-- Configuração do Conteudo do Layout do Financeiro--%>
        </asp:Panel>
    </div>
    <%-- Botões --%>
    <asp:Panel
        ID="pnlBotoes"
        runat="server"
        CssClass="painel_botoes">
        <asp:Button
            ID="btnVoltar"
            runat="server"
            CssClass="botao_padrao"
            Text="Voltar"
            PostBackUrl="SalaAdministrador.aspx"
            CausesValidation="false" />
    </asp:Panel>
    <%-- Fim Botões --%>
</asp:Content>
<asp:Content
    ID="Content3"
    ContentPlaceHolderID="cphRodape"
    runat="server">
</asp:Content>
