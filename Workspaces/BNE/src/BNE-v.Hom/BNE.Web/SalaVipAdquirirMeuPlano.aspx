<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SalaVipAdquirirMeuPlano.aspx.cs" Inherits="BNE.Web.SalaVipAdquirirMeuPlano" %>

<%@ Register TagPrefix="uc5" TagName="ucmodallogin" Src="/UserControls/Modais/ucModalLogin.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/sala_vip.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaVipMeuPlano.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="painel_padrao_sala_vip">
        <p>
            Seu plano é <b class="destaque_texto">Básico</b>. Adquira um Plano VIP.</p>
        <div class="plano_vantagens">
            <div class="seta_plano">
                <span class="icn_plano">
                    <img src="img/icn_acesso_vip.png" alt="Acesso Livre a Todas as Vagas" />
                </span><span class="icn_plus">
                    <img src="img/icn_plus_azul.png" alt="mais" />
                </span><span class="icn_plano">
                    <img src="img/icn_vagas.png" alt="Vagas no Celular" />
                </span><span class="icn_plus">
                    <img src="img/icn_plus_azul.png" alt="mais" />
                </span><span class="icn_plano">
                    <img src="img/icn_vcem1lugar.png" alt="Você em Primeiro Lugar nas Pesquisas" />
                </span>
            </div>
            <%-- Informações do Plano e Botão eu Quero--%>
            <div class="info_add_button">
                <div class="info_add_plano">
                    <h2>
                        Informações Adicionais do Plano</h2>
                    <table class="tabela_planos" cellspacing="0">
                        <thead>
                            <tr>
                                <td class="col_serv">
                                    Serviços
                                </td>
                                <td class="col_basico">
                                    Básico
                                </td>
                                <td class="col_ilimitado">
                                    VIP
                                </td>
                            </tr>
                        </thead>
                        <tbody>
                            <tr class="linha_in">
                                <td class="col_serv">
                                    Acesso aos dados da empresa
                                </td>
                                <td class="col_basico">
                                    <img src="img/icn_bloqueado.png" alt="Bloqueado" title="Somente clientes VIP" />
                                </td>
                                <td class="col_ilimitado">
                                    <img src="img/icn_checado.png" alt="Checado" title="Acesso permitido" />
                                </td>
                            </tr>
                            <tr class="linha_normal">
                                <td class="col_serv">
                                    Avisos de vagas no celular
                                </td>
                                <td class="col_basico">
                                    <img src="img/icn_bloqueado.png" alt="Bloqueado" title="Somente clientes VIP" />
                                </td>
                                <td class="col_ilimitado">
                                    <img src="img/icn_checado.png" alt="Checado" title="Acesso permitido" />
                                </td>
                            </tr>
                            <tr class="linha_in">
                                <td class="col_serv">
                                    Seu currículo em primeiro lugar
                                </td>
                                <td class="col_basico">
                                    <img src="img/icn_bloqueado.png" alt="Bloqueado" title="Somente clientes VIP" />
                                </td>
                                <td class="col_ilimitado">
                                    <img src="img/icn_checado.png" alt="Checado" title="Acesso permitido" />
                                </td>
                            </tr>
                            <tr class="linha_normal">
                                <td class="col_serv">
                                    Quem me viu?
                                </td>
                                <td class="col_basico">
                                    <img src="img/icn_bloqueado.png" alt="Bloqueado" title="Somente clientes VIP" />
                                </td>
                                <td class="col_ilimitado">
                                    <img src="img/icn_checado.png" alt="Checado" title="Acesso permitido" />
                                </td>
                            </tr>
                            <tr class="linha_in">
                                <td class="col_serv">
                                    Acesso ilimitado às vagas
                                </td>
                                <td class="col_basico">
                                    <img src="img/icn_bloqueado.png" alt="Bloqueado" title="Somente clientes VIP" />
                                </td>
                                <td class="col_ilimitado">
                                    <img src="img/icn_checado.png" alt="Checado" title="Acesso permitido" />
                                </td>
                            </tr>
                            <tr class="linha_normal">
                                <td class="col_serv">
                                    Teste das cores
                                </td>
                                <td class="col_ilimitado">
                                    <img src="img/icn_checado.png" alt="Checado" title="Acesso permitido" />
                                </td>
                                <td class="col_ilimitado">
                                    <img src="img/icn_checado.png" alt="Checado" title="Acesso permitido" />
                                </td>
                            </tr>
                            <tr class="linha_in">
                                <td class="col_serv">
                                    Teste sistmars
                                </td>
                                <td class="col_ilimitado">
                                    <img src="img/icn_checado.png" alt="Checado" title="Acesso permitido" />
                                </td>
                                <td class="col_ilimitado">
                                    <img src="img/icn_checado.png" alt="Checado" title="Acesso permitido" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <%-- Botão eu Quero VIP --%>
                <div class="salavip_continuar">
                    <div class="icone_confirmacao_candidatura">
                        <%-- COMEÇO FORMATAÇÃO MEU PLANO --%>
                        <div class="campanha_carnaval_salaVipAdquirirPlano">
                            <asp:UpdatePanel runat="server" ID="upBtiEuQuero" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="preco_sem_desconto_mensal">
                                        <span>De R$ </span>
                                        <asp:Literal runat="server" ID="litPrecoSemDescontoMensal" Text="25,00"></asp:Literal>
                                    </div>
                                    <div class="preco_com_desconto_mensal">
                                        <span>Por R$ </span>
                                        <asp:Literal runat="server" ID="litPrecoDescontoMensal" Text="14,90"></asp:Literal>
                                    </div>
                                    <asp:Label ID="labelMensal" runat="server" Text="Mensal" CssClass="alinhaLabelMensal"></asp:Label>
                                    <asp:ImageButton ID="btnContinuar" runat="server" ImageUrl="/img/campanha/meu_plano/btn_continuar.png"
                                        ToolTip="Compre Já!" CssClass="alinha_btn_continuar" OnClick="btnContinuar_Click"
                                        CausesValidation="False" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <%-- FIM FORMATAÇÃO MEU PLANO --%>
                    </div>
                </div>
                <%-- Fim Botão eu Quero VIP --%>
                <div class="texto_e_botao">
                    <div class="texto_vantagens">
                        Todos estes benefícios e com a qualidade de serviços que só o BNE oferece. Aproveite,
                        compre já o seu Plano VIP.
                    </div>
                </div>
            </div>
            <%-- Fim Informações do Plano e Botão eu Quero--%>
        </div>
    </div>
    <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
        <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
            OnClick="btnVoltar_Click" />
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <uc5:ucmodallogin ID="ucModalLogin" runat="server" />
</asp:Content>
