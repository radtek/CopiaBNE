<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SiteTrabalheConoscoMenu.aspx.cs" Inherits="BNE.Web.SiteTrabalheConoscoMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/TrabalheConosco.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:Panel ID="pnlTrabalheConoscoMenu" runat="server">
        <div class="painel_padrao_sala_adm menu">
            <div class="menu_ss">


                <!-- INÍCIO DA NOVA ORDEM -->
                <div class="col01">

                    <asp:LinkButton ID="btlVagas" runat="server" CausesValidation="false" ToolTip="Vagas"
                        OnClick="btlVagas_Click">
                            <div class="btn_col">
                                <div class="fundo_btn_ss">
                                    <div class="sombra_btn_ss">
                                        <img src="/img/SalaSelecionadora/icn_noticias.png"
                                            alt="Vagas" />
                                        <div class="titulo_texto_btn">
                                            <span class="tit_btn_ss">
                                               Minhas Vagas</span>
                                            <span class="texto_btn_ss">
                                                <ul class="item_btn">
                                                    <li>Administre ou divulgue vagas aqui
                                                    </li>
                                                </ul>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                    </asp:LinkButton>

                    <%--<asp:LinkButton ID="btnConteudos" runat="server" CausesValidation="false" ToolTip="Configurações"
                        PostBackUrl="/SiteTrabalheConoscoSalaSelecionadorConteudos.aspx">
                        <div class="btn_col bottom"
                            title="Configurações">
                            <div class="fundo_btn_ss">
                                <div class="sombra_btn_ss">
                                    <img src="/img/SalaSelecionadora/icn_confs.png"
                                        alt="Conteúdos" />
                                    <div class="titulo_texto_btn">
                                        <span class="tit_btn_ss">
                                            Configurações</span>
                                        <span class="texto_btn_ss">
                                            <ul class="item_btn">
                                                <li>
                                                   Personalize o retorno para os candidatos
                                                </li>
                                            </ul>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:LinkButton>--%>

                    <asp:LinkButton ID="btlRastreadorCurriculos" runat="server" OnClick="btlRastreadorCurriculos_Click"
                        CausesValidation="false" ToolTip="Alerta de Currículos">
                        <div class="btn_col">
                            <div class="fundo_btn_ss">
                                <div class="sombra_btn_ss">
                                     <p class="block-ico">
                            <span class="fa-stack fa-lg fa-2x">
                                <i class="fa fa-comment-o fa-flip-horizontal fa-stack-2x"></i>
                                <i class="fa fa-exclamation fa-stack-1x"></i>
                            </span>
                        </p>
                                    <div class="titulo_texto_btn menor">
                                        <span class="tit_btn_ss">
                                            Alerta de Currículos</span>
                                        <span class="texto_btn_ss">
                                            <ul class="item_btn">
                                                <li>
                                                    Receba candidatos no perfil em seu e-mail</li>
                                            </ul>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:LinkButton>
                </div>
                <div class="col02">
                    <asp:LinkButton ID="LinkMeuPlano" runat="server" CausesValidation="false" ToolTip="Vagas"
                        OnClick="LinkMeuPlano_Click">
                        <div class="btn_col">
                            <div class="fundo_btn_ss">
                                <div class="sombra_btn_ss">
                                    <asp:Literal runat="server" ID="litSemPlano" Visible="False">
                                                <i class="fa fa-times fa-5x pull-left" style="color: red;"></i>
                                    </asp:Literal>
                                    <asp:Literal runat="server" ID="litComPlano" Visible="False">
                                                <i class="fa fa-check fa-5x pull-left"></i>
                                    </asp:Literal>
                                    <div class="titulo_texto_btn menor">
                                        <span class="tit_btn_ss">Meu Plano</span> <span class="texto_btn_ss">
                                            <ul class="item_btn">
                                                <li>
                                                    <asp:Label ID="lblPlanoAcessoValidade" runat="server"></asp:Label>
                                                </li>
                                                <li><b>
                                                    <asp:Label ID="lblQuantidadeSMSUtilizado" runat="server"></asp:Label></b>
                                                    <asp:Label ID="lblSMSUtilizadoMensagem" runat="server"></asp:Label></li>
                                            </ul>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnUsuarios" runat="server" CausesValidation="false" ToolTip="Usuários"
                        PostBackUrl="~/CadastroEmpresaUsuario.aspx">
                        <div class="btn_col"
                            title="Usuários">
                            <div class="fundo_btn_ss">
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

                <div class="col03">

                    <asp:LinkButton ID="btnCampanhaRecrutamento1"
                        runat="server" CausesValidation="false"
                        ToolTip="Campanha de Recrutamento"
                        OnClick="btnCampanhaRecrutamento">
                        <div class="btn_col">
                            <div class="fundo_btn_ss">
                                <div class="sombra_btn_ss">
                                    <img src="/img/SalaAdministrador/icn_vagas.png"
                                        alt="Recrutamento R1" />
                                    <div class="titulo_texto_btn menor">
                                        <span class="tit_btn_ss">Campanha de Recrutamento</span>
                                        <span class="texto_btn_ss">
                                            <ul class="item_btn">
                                                <li>
                                                    <asp:Literal ID="litSaldoCampanha" runat="server" Text="0"></asp:Literal></li>
                                            </ul>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:LinkButton>

                    <asp:LinkButton ID="btnPesquisaAvancada" runat="server" CausesValidation="false"
                        ToolTip="Pesquisa Avançada" PostBackUrl="/PesquisaCurriculoAvancada.aspx">
                        <div class="btn_col">
                            <div class="fundo_btn_ss">
                                <div class="sombra_btn_ss">
                                    <img src="/img/SalaSelecionadora/icn_rastreador_cv.png"
                                        alt="Pesquisa de Currículos" />
                                    <div class="titulo_texto_btn menor">
                                        <span class="tit_btn_ss">
                                            Pesquisa de Currículos</span>
                                        <span class="texto_btn_ss">
                                            <ul class="item_btn">
                                                <li>
                                                    Filtro completo para uma busca assertiva
                                                </li>
                                            </ul>
                                        </span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:LinkButton>

                </div>

            </div>
        </div>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
