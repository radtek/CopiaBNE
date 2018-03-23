<%@ Page Title="" Description="" Language="C#" MasterPageFile="~/Master/Principal.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BNE.Web.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <!--[if gte IE 9]>
      <style type="text/css">
    .gradient {
       filter: none;
    }
  </style>
<![endif]-->
    <link rel="canonical" href="http://www.bne.com.br/Default.aspx" />



    <%--START: Banner Carousel Jquery e CSS--%>
    <%--<script type="text/javascript" src="js/jquery-1.8.3.js"></script>--%>
    <script type="text/javascript" src="js/jquery.jcarousel.min.js"></script>
    <script type="text/javascript" src="js/local/default.js"></script>

    <%--FIM:  Banner Carousel Jquery e CSS--%>

    <!-- COMEÇO Banner Promocional Dobro FlexSlider pieces -->
    <link rel="stylesheet" href="/css/local/BannerPromocionalDobro/flexslider.css" type="text/css" media="screen" />
    <link rel="stylesheet" href="/css/local/BannerPromocionalDobro/demo.css" type="text/css" media="screen" />
    <%--<script src="//ajax.googleapis.com/ajax/libs/jquery/1/jquery.min.js"></script>--%>
    <script type="text/javascript" src="/js/local/BannerPromocionalDobro/jquery.flexslider-min.js"></script>
    <!-- FIM Banner Promocional Dobro FlexSlider pieces -->

    <Employer:DynamicHtmlLink runat="server" Href="/css/local/BannerPromocionalDobro/demo.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/BannerPromocionalDobro/flexslider.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/default.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/banners.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/carroussel/skin.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">

    <asp:UpdatePanel ID="upDefault" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modalMobile" style="display: none;">
                <div class="titulo">
                    <a onclick="$('.modalMobile').hide();" class="fechar"><i class="fa fa-times fa-2x"></i> </a>
                </div>
                <div class="conteudo">
                    <span class="img"> <a onclick="$('.modalMobile').hide();" ><img alt="Tenha destaque no mercado de trabalho!" src="/img/iphone-hand.png" /></a></span>
                </div>
                <div class="botao">
                    <a title="APP do BNE na Google Play" href="market://details?id=br.com.employer.bne">
                        Sim, ir para o Google Play</a>
                </div>
            </div>
            <div class="paineis_home">
                <%-- COMEÇA - BANNER PROMOCIONAL O DOBRO--%>
                <asp:Panel ID="pnlConteudoBNE" runat="server">
                    <!-- TERMINA - SCRIPT BANNER PROMOCIONAL O DOBRO -->
                    <div id="container">
                        <div class="slidebox">
                            <div class="shadow"></div>
                            <!-- Markup for FADE animation-->
                            <div class="flexslider">
                                <ul class="slides">
                                    <li>
                                        <a href="/vip" onclick="trackOutboundLink(this, 'Anuncios', 'Click', 'Banner_Home_VIP'); return false;">
                                            <img alt="Tenha destaque no mercado de trabalho!" src="/img/banners/bne_vip.jpg" />
                                        </a>
                                    </li>

                                    <li>
                                        <a href="/cia" onclick="trackOutboundLink(this, 'Anuncios', 'Click', 'Banner_Home_CIA'); return false;">
                                            <img alt="97 mil empresas cadastradas!" src="/img/banners/iceberg_banner.png" />
                                        </a>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                    <%-- TERMINA - BANNER PROMOCIONAL O DOBRO--%>
                    <div class="quero_emprego">
                        <asp:LinkButton runat="server" ID="btiCadastreCurriculo" OnClick="btiCadastreCurriculo_Click">
                            <strong>Quero Emprego</strong>
                       
                            <div class="icon-group">
                                 <span class="label-free">Grátis</span>
                                <i class="fa fa-file-o"></i>
                                <i class="fa fa-check"></i>
                                  <h3> <small>Cadastrar    </small>Currículos</h3>
                    </div>
                        </asp:LinkButton>
                    </div>
                    <div class="quero_contratar">
                        <asp:LinkButton runat="server" ID="btnCurriculoPesquisa" OnClick="btnCurriculoPesquisa_Click">
                            <strong>Quero Contratar!</strong>
                            <div class="icon-group">
                                 <span class="label-free">Grátis</span>
                                <i class="fa fa-file-o"></i>
                                <i class="fa fa-search"></i>
                            
                                  <h3> <small>Pesquisar </small>Currículos</h3>
                            </div>
                        </asp:LinkButton>

                    </div>
                    <div class="ultimas_vagas">
                        <h2 class="button_titulo_ultimas_vagas">
                            <a id="btnUltimasVagas" runat="server"  class="barra_ultimas_vagas" href="/vagas de emprego">Últimas vagas</a>
                        </h2>
                        <asp:Panel ID="pnlUltimasVagas" CssClass="painel_ultimas_vagas" runat="server">
                            <asp:Repeater ID="rptUltimasVagas" runat="server"
                                OnItemDataBound="rptVagas_ItemDataBound">
                                <HeaderTemplate>
                                    <ul>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li>
                                        <div class="horario_vagas">
                                            <div class="texto_cadastrada_ha">
                                                <asp:Literal ID="litUltimasVagasCadastradaHa" runat="server" Text="Cadastrada há"></asp:Literal>
                                            </div>
                                            <div class="texto_num_dias">
                                                <asp:Label ID="lblHorario" runat="server" CssClass="horario" Text='<%# DataBinder.Eval(Container.DataItem, "Descricao_Data") %>'></asp:Label>
                                            </div>
                                        </div>
                                        <div class="descricao_funcao">
                                            <asp:HyperLink ID="HyperLink1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Des_Funcao")%>' NavigateUrl='<%# RetornarURLVaga(DataBinder.Eval(Container.DataItem, "Des_Area_BNE").ToString(), DataBinder.Eval(Container.DataItem, "Des_Funcao").ToString(),  DataBinder.Eval(Container.DataItem, "Nme_Cidade").ToString(), DataBinder.Eval(Container.DataItem, "Sig_Estado").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Idf_Vaga")))%>'></asp:HyperLink>
                                        </div>
                                        <div class="descricao_vaga">
                                            <asp:HyperLink ID="HyperLink2" runat="server" CssClass="vaga" Text='<%# RetornarDesricaoSalario(Eval("Vlr_Salario_De"), Eval("Vlr_Salario_Para"))%>' NavigateUrl='<%# RetornarURLVaga(DataBinder.Eval(Container.DataItem, "Des_Area_BNE").ToString(), DataBinder.Eval(Container.DataItem, "Des_Funcao").ToString(),  DataBinder.Eval(Container.DataItem, "Nme_Cidade").ToString(), DataBinder.Eval(Container.DataItem, "Sig_Estado").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Idf_Vaga")))%>'></asp:HyperLink>
                                            -
                                            <asp:HyperLink ID="HyperLink3" runat="server" CssClass="vaga" Text='<%# FormatarCidade(Eval("Nme_Cidade").ToString(), Eval("Sig_Estado").ToString()) %>' NavigateUrl='<%# RetornarURLVaga(DataBinder.Eval(Container.DataItem, "Des_Area_BNE").ToString(), DataBinder.Eval(Container.DataItem, "Des_Funcao").ToString(),  DataBinder.Eval(Container.DataItem, "Nme_Cidade").ToString(), DataBinder.Eval(Container.DataItem, "Sig_Estado").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Idf_Vaga")))%>'></asp:HyperLink>
                                        </div>
                                    </li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                        </asp:Panel>
                        <asp:Panel ID="pnlVerMaisVagas" CssClass="button_ver_mais_vagas" runat="server">
                            <a id="btnVerMaisVagas" runat="server" class="botao_ver_mais_vagas" href="/busca-de-vagas"><span>Ver mais vagas</span></a>
                        </asp:Panel>
                    </div>
                    <%--START:Banners R1 CIA e Clientes--%>
                    <div class="containerBanners">
                        <div class="containerBanner1">
                            <div class="bannerServicos">
                                <ul id="carrousel_servicos" class="jcarousel-skin-carrousel_servicos">
                                    <li id="Item1">
                                        <asp:HyperLink runat="server" ID="hlR1">
                                            <img src="img/banner_rotativo/servicos_bne/imgR1.png" class="imgSlide" width="388" height="82" alt="Clique e saiba mais!" title="Clique e saiba Mais!"  style="border: 0;"/>
                                        </asp:HyperLink>
                                    </li>
                                    <li id="Item2">
                                        <asp:HyperLink runat="server" ID="hlCIA">
                                            <img src="img/banner_rotativo/servicos_bne/imgCia.png"  class="imgSlide" width="382" height="73" alt="Clique e saiba mais!" title="Clique e saiba Mais!" style="margin-top: 9px; border: 0;"/>
                                        </asp:HyperLink>
                                    </li>
                                    <li id="Item3">
                                        <a ID="hlSMS" href="/cia">
                                            <img src="img/banner_rotativo/servicos_bne/banner_sms.jpg"  class="imgSMS" width="382" height="73" alt="Clique e saiba mais!" title="Clique e saiba Mais!" style="margin-top: 9px; border: 0;"/>
                                        </a>
                                    </li>
                                </ul>
                            </div>
                            <div class="jcarousel-control" style="margin-left: 172px; margin-top: 4px; position: absolute; width: 36px;">
                                <a href="#">
                                    <img id="b1" src="img/banner_rotativo/bolinha.png" alt="" style="border: 0" /></a>
                                <a href="#">
                                    <img id="b2" src="img/banner_rotativo/bolinha.png" alt="" style="border: 0" /></a>
                                <a href="#">
                                    <img id="b3" src="img/banner_rotativo/bolinha.png" alt="" style="border: 0" /></a>
                            </div>
                        </div>
                        <div class="containerBanner2">
                            <div class="bannerClientes">
                                <ul id="carrousel_empresas" class="jcarousel-skin-carrousel_empresas" style="display: inline; float: left; list-style: none">
                                    <asp:Repeater ID="rptEmpresasHome" runat="server">
                                        <ItemTemplate>
                                            <li id="ItemEmpresa<%# Container.ItemIndex + 1 %>">
                                                <asp:HyperLink runat="server" NavigateUrl='<%# RetornarURLVagasEmpresa(DataBinder.Eval(Container.DataItem, "DescricaoNomeURl").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Filial.IdFilial"))) %>'>
                                                    <img src='<%# DataBinder.Eval(Container.DataItem, "DescricaoCaminhoImagem") %>' style="border: 0" class="imgSlide empresa" width="100" height="38" alt="" />
                                                </asp:HyperLink>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                            <div class="jcarousel-control" style="margin-left: 240px; margin-top: 4px; position: absolute; width: 38px;">
                                <a href="#">
                                    <img id="Img1" src="img/banner_rotativo/bolinha.png" style="border: 0" alt="" /></a>
                                <a href="#">
                                    <img id="Img2" src="img/banner_rotativo/bolinha.png" style="border: 0" alt="" /></a>
                                <a href="#">
                                    <img id="Img3" src="img/banner_rotativo/bolinha.png" style="border: 0" alt="" /></a>
                            </div>
                        </div>
                    </div>
                    <%--FIM:Banners R1 CIA e Clientes--%>
                </asp:Panel>
                <asp:Panel ID="pnlConteudoRHOffice" runat="server">
                    <asp:Literal ID="litConteudoRHOffice" runat="server"></asp:Literal>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <Employer:DynamicScript runat="server" Src="js/local/default.js" type="text/javascript" />
    <Employer:DynamicScript runat="server" Src="js/jquery.jcarousel.min.js" type="text/javascript" />
    <Employer:DynamicScript runat="server" Src="/js/local/BannerPromocionalDobro/jquery.flexslider-min.js" type="text/javascript" />
</asp:Content>
