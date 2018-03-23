<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SiteTrabalheConoscoMenu.aspx.cs" Inherits="BNE.Web.SiteTrabalheConoscoMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/TrabalheConosco.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:Panel ID="pnlTrabalheConoscoMenu" runat="server">
        <div class="painel_padrao_sala_adm menu">
            <div class="menu_ss">
                <%-- Coluna 01 do Menu da Tela Selecionadora --%>
                <div class="col01">
                    <asp:LinkButton ID="btnPesquisaAvancada" runat="server" CausesValidation="false"
                        ToolTip="Pesquisa Avançada" PostBackUrl="/PesquisaCurriculoAvancada.aspx">
                        <div class="btn_col">
                            <div class="fundo_btn_ss">
                                <div class="sombra_btn_ss">
                                    <img src="/img/SalaSelecionadora/icn_rastreador_cv.png"
                                        alt="Pesquisa Avançada" />
                                    <div class="titulo_texto_btn menor">
                                        <span class="tit_btn_ss">
                                            Pesquisa Avançada</span>
                                        <span class="texto_btn_ss">
                                            <ul class="item_btn">
                                                <li>
                                                    Filtro completo para uma busca assertiva</li>
                                            </ul>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnRecrutamentoR1" runat="server" CausesValidation="false" ToolTip="Recrutamento R1">
                        <div class="btn_col bottom">
                            <div class="fundo_btn_ss">
                                <div class="sombra_btn_ss">
                                    <img src="/img/SalaSelecionadora/icn_R1.png"
                                        alt="Recrutamento R1" />
                                    <div class="titulo_texto_btn menor">
                                        <span class="tit_btn_ss">
                                            Recrutamento R1</span>
                                        <span class="texto_btn_ss">
                                            <ul class="item_btn">
                                                <li>
                                                    Não achou currículos? <br />O BNE encontra para você!</li>
                                            </ul>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:LinkButton>
                </div>
                <%-- Fim da Coluna 01 do Menu da Tela Selecionadora --%>
                <%-- Coluna 02 do Menu da Tela Selecionadora --%>
                <div class="col02">
                    <asp:LinkButton ID="btlVagas" runat="server" CausesValidation="false" ToolTip="Vagas"
                        OnClick="btlVagas_Click">
                            <div class="btn_col">
                                <div class="fundo_btn_ss">
                                    <div class="sombra_btn_ss">
                                        <img src="/img/SalaAdministrador/icn_vagas.png"
                                            alt="Vagas" />
                                        <div class="titulo_texto_btn">
                                            <span class="tit_btn_ss">
                                               Vagas</span>
                                            <span class="texto_btn_ss">
                                                <ul class="item_btn">
                                                    <li>Anuncie gratuitamente suas vagas 
                                                    </li>
                                                </ul>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnConteudos" runat="server" CausesValidation="false" ToolTip="Conteúdos"
                        PostBackUrl="/SiteTrabalheConoscoSalaSelecionadorConteudos.aspx">
                        <div class="btn_col bottom"
                            title="Conteúdos">
                            <div class="fundo_btn_ss">
                                <div class="sombra_btn_ss">
                                    <img src="/img/icn_conteudos.png"
                                        alt="Conteúdos" />
                                    <div class="titulo_texto_btn">
                                        <span class="tit_btn_ss">
                                            Conteúdos</span>
                                        <span class="texto_btn_ss">
                                            <ul class="item_btn">
                                                <%-- Esse <li> não pode aparecer sem ter dados, o IE7 esta interpretando como se ele tivesse conteúdo --%>
                                                <li>
                                                   Personalize os textos de e-mail e a página inicial do seu site
                                                </li>
                                            </ul>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:LinkButton>
                </div>
                <%-- Fim da Coluna 01 do Menu da Tela Selecionadora --%>
                <%-- Coluna 03 do Menu da Tela Selecionadora --%>
                <div class="col03">
                    <asp:LinkButton ID="btnMeusCVs" runat="server" CausesValidation="false" ToolTip="Meus CV's"
                        PostBackUrl="/SalaSelecionadorMeusCurriculos.aspx">
                        <div class="btn_col"
                            title="Meus CV's">
                            <div class="fundo_btn_ss_g">
                                <div class="sombra_btn_ss_g">
                                    <img src="/img/icn_meuscvs.png"
                                        alt="Meus CV's" />
                                    <div class="titulo_texto_btn">
                                        <span class="tit_btn_ss">
                                            Meus CV's</span>
                                        <span class="texto_btn_ss">
                                            <ul class="item_btn">
                                                <li>Visualize os seus currículos</li>
                                            </ul>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnUsuarios" runat="server" CausesValidation="false" ToolTip="Usuários"
                        PostBackUrl="~/CadastroEmpresaUsuario.aspx">
                        <div class="btn_col bottom"
                            title="Usuários">
                            <div class="fundo_btn_ss_g">
                                <div class="sombra_btn_ss_g">
                                    <img src="../../../img/icn_usuarios.png"
                                        alt="Usuários" />
                                    <div class="titulo_texto_btn">
                                        <span class="tit_btn_ss">
                                            Usuários</span>
                                        <span class="texto_btn_ss">
                                            <ul class="item_btn">
                                                <li>Cadastre, altere ou exclua os usuários de seu site</li>
                                            </ul>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:LinkButton>
                </div>
                <%-- Fim da Coluna 03 do Menu da Tela Selecionadora --%>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
