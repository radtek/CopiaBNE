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

    <Employer:DynamicHtmlLink runat="server" Href="/css/local/BannerPromocionalDobro/demo.css" type="text/css" rel="stylesheet" media="screen" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/BannerPromocionalDobro/flexslider.css" type="text/css" rel="stylesheet" media="screen" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/default.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/banners.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/carroussel/skin.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <script type="text/javascript">

      

        function ExibeDivUltimasVagas() {
            $('#cphConteudo_divUltimasVagas').show();
            $('#cphConteudo_divVagasMaisBuscadas').hide();
            $('#cphConteudo_divCidadesMaisBuscadas').hide();
        }
        function ExibeDivVagasMaisBuscadas() {
            $('#cphConteudo_divUltimasVagas').hide();
            $('#cphConteudo_divVagasMaisBuscadas').show();
            $('#cphConteudo_divCidadesMaisBuscadas').hide();
        }
        function ExibeDivCidadesMaisBuscadas() {
            $('#cphConteudo_divUltimasVagas').hide();
            $('#cphConteudo_divVagasMaisBuscadas').hide();
            $('#cphConteudo_divCidadesMaisBuscadas').show();
        }
    </script>
    <asp:UpdatePanel ID="upDefault" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modalMobile" style="display: none;">
                <div class="titulo">
                    <a onclick="$('.modalMobile').hide();" class="fechar"><i class="fa fa-times fa-2x"></i></a>
                </div>
                <div class="conteudo">
                    <span class="img"><a onclick="$('.modalMobile').hide();">
                        <img alt="Tenha destaque no mercado de trabalho!" src="/img/iphone-hand.png" /></a></span>
                </div>
                <div class="botao">
                    <a title="APP do BNE na Google Play" href="market://details?id=br.com.employer.bne">Sim, ir para o Google Play</a>
                </div>
            </div>
            <asp:Panel ID="pnlBanner" runat="server" Visible="false">
                <div id="bannerCampanha">
                    <a href="http://www.bne.com.br/vip?utm_source=bne&utm_medium=banner&utm_campaign=HomeMoeda" title="Seja VIP e tenha acesso ilitado e muito mais!">
                        <div id="link-area">
                            <div id="bannerCampanha-titulo"><strong>Sabe aquela<br />
                                moedinha?</strong></div>
                            <div id="bannerCampanha-subtitulo">Consiga um <strong>novo emprego</strong> com ela!</div>
                            <div id="bannerCampanha-complemento"><strong>acesso ilimitado por menos de 50 centavos por dia</strong></div>
                            <div id="bannerCampanha-button"><strong>quero ser vip!</strong></div>
                        </div>
                    </a>
                </div>
            </asp:Panel>
            <div class="paineis_home">
                <div class="paineis_home_wrapper">
                    <%-- COMEÇA - BANNER PROMOCIONAL O DOBRO--%>
                    <asp:Panel ID="pnlConteudoBNE" runat="server">
                        <div class="vagas_area">
                            <h2>Qual é a sua área?</h2>
                            <div class="areas">
                                <asp:Repeater ID="rptAreas" runat="server">
                                    <HeaderTemplate>
                                        <ul>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li>
                                            <a href="/vagas-de-emprego-na-area-de-<%# BNE.Web.Code.UIHelper.RemoverAcentos(((System.Collections.Generic.KeyValuePair<string,int>)Container.DataItem).Key).ToLower().Replace(" ", "-") %>"
                                                title="Vagas de emprego na área de <%# ((System.Collections.Generic.KeyValuePair<string,int>)Container.DataItem).Key %>" onclick="trackEvent('Default','Click','PainelVagasArea_LinkArea'); return true;">
                                                <%# DataBinder.Eval((System.Collections.Generic.KeyValuePair<string,int>)Container.DataItem,"Key") %>
                                                <span>(<%# DataBinder.Eval((System.Collections.Generic.KeyValuePair<string,int>)Container.DataItem,"Value") %> vagas)
                                                </span>
                                            </a>
                                        </li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="botao">
                                <a href="/busca-de-vagas" onclick="trackEvent('Default','Click','PainelVagasArea_BotaoVejaMais'); return true;">VEJA MAIS VAGAS POR ÁREA</a>
                            </div>
                        </div>
                        <!-- TERMINA - SCRIPT BANNER PROMOCIONAL O DOBRO -->

                        <%--Container da segunda linha--%>
                        <div id="SecondRowHomeHolder">
                            <div class="quero_emprego">
                                <asp:LinkButton runat="server" ID="btiCadastreCurriculo" OnClick="btiCadastreCurriculo_Click" OnClientClick="trackEvent('Default','Click','QueroEmprego_Botao'); return true;">
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
                                <asp:LinkButton runat="server" ID="btnCurriculoPesquisa" OnClick="btnCurriculoPesquisa_Click" OnClientClick="trackEvent('Default','Click','QueroContratar_Botao'); return true;">
                                    <strong>Quero Contratar!</strong>
                                    <div class="icon-group">
                                         <span class="label-free">Grátis</span>
                                        <i class="fa fa-file-o"></i>
                                        <i class="fa fa-search"></i>
                            
                                          <h3> <small>Pesquisar </small>Currículos</h3>
                                    </div>
                                </asp:LinkButton>

                            </div>
                            <div id="paineisVagas" class="ultimas_vagas">
                                <div id="divUltimasVagas" runat="server" style="display: block">
                                    <h2 class="button_titulo_ultimas_vagas">
                                        <a id="btnUltimasVagas" runat="server" href="/vagas-de-emprego" onclick="trackEvent('Default','Click', 'AbaUltimasVagas_TituloTopo'); return true;">Últimas vagas</a>
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
                                                        <asp:HyperLink ID="HyperLink1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Des_Funcao")%>' NavigateUrl='<%# RetornarURLVaga(DataBinder.Eval(Container.DataItem, "Des_Area_BNE").ToString(), DataBinder.Eval(Container.DataItem, "Des_Funcao").ToString(),  DataBinder.Eval(Container.DataItem, "Nme_Cidade").ToString(), DataBinder.Eval(Container.DataItem, "Sig_Estado").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Idf_Vaga"))) + "?utm_term=CadastroRapido"%>'></asp:HyperLink>
                                                    </div>
                                                    <div class="descricao_vaga">
                                                        <asp:HyperLink ID="HyperLink2" runat="server" CssClass="vaga" Text='<%# RetornarDesricaoSalario(Eval("Vlr_Salario_De"), Eval("Vlr_Salario_Para"))%>' NavigateUrl='<%# RetornarURLVaga(DataBinder.Eval(Container.DataItem, "Des_Area_BNE").ToString(), DataBinder.Eval(Container.DataItem, "Des_Funcao").ToString(),  DataBinder.Eval(Container.DataItem, "Nme_Cidade").ToString(), DataBinder.Eval(Container.DataItem, "Sig_Estado").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Idf_Vaga"))) + "?utm_term=CadastroRapido"%>'></asp:HyperLink>
                                                        -
                                                    <asp:HyperLink ID="HyperLink3" runat="server" CssClass="vaga" Text='<%# FormatarCidade(Eval("Nme_Cidade").ToString(), Eval("Sig_Estado").ToString()) %>' NavigateUrl='<%# RetornarURLVaga(DataBinder.Eval(Container.DataItem, "Des_Area_BNE").ToString(), DataBinder.Eval(Container.DataItem, "Des_Funcao").ToString(),  DataBinder.Eval(Container.DataItem, "Nme_Cidade").ToString(), DataBinder.Eval(Container.DataItem, "Sig_Estado").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Idf_Vaga"))) + "?utm_term=CadastroRapido"%>'></asp:HyperLink>
                                                    </div>
                                                </li>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </ul>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </asp:Panel>
                                </div>
                                <div id="divVagasMaisBuscadas" runat="server" style="display: none">
                                    <h2 class="button_titulo_ultimas_vagas">
                                        <a id="btnAbaVagasMaisBuscadas" runat="server" href="/vagas-de-emprego" onclick="trackEvent('Default','Click', 'AbaVagasMaisBuscadas_TituloTopo'); return true;">Vagas mais buscadas</a>
                                    </h2>
                                    <asp:Panel ID="pnlVagasMaisBuscadas" CssClass="painel_vagas_mais_buscadas" runat="server" Style="text-align: center">
                                        <ul>
                                            <li>
                                                <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-para-auxiliar-administrativo-em-belo-horizonte-mg" title="Vagas para Auxiliar Administrativo em Belo Horizonte / MG">Vagas para Auxiliar Administrativo em Belo Horizonte / MG</a><br />
                                                <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-para-recepcionista-em-rio-de-janeiro-rj" title="Vagas para Recepcionista em Rio de Janeiro / RJ">Vagas para Recepcionista em Rio de Janeiro / RJ</a><br />
                                                <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-para-assistente-administrativo-em-sao-paulo-sp" title="Vagas para Assistente Administrativo em São Paulo / SP">Vagas para Assistente Administrativo em São Paulo / SP</a><br />
                                                <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-para-motorista-em-rio-de-janeiro-rj" title="Vagas para Motorista em Rio de Janeiro / RJ">Vagas para Motorista em Rio de Janeiro / RJ</a><br />
                                                <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-para-vendedor-em-belo-horizonte-mg" title="Vagas para Vendedor em Belo Horizonte / MG">Vagas para Vendedor em Belo Horizonte / MG</a><br />
                                                <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-para-vendedor-externo-em-belo-horizonte-mg" title="Vagas para Vendedor Externo em Belo Horizonte / MG">Vagas para Vendedor Externo em Belo Horizonte / MG</a><br />
                                                <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-para-promotor-de-vendas-em-sao-paulo-sp" title="Vagas para Promotor de Vendas em São Paulo / SP">Vagas para Promotor de Vendas em São Paulo / SP</a><br />
                                                <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-para-auxiliar-de-servicos-gerais-em-rio-de-janeiro-rj" title="Vagas para Auxiliar de Serviços Gerais em Rio de Janeiro / RJ">Vagas para Auxiliar de Serviços Gerais em Rio de Janeiro / RJ</a>
                                            </li>
                                        </ul>
                                    </asp:Panel>
                                </div>
                                <div id="divCidadesMaisBuscadas" runat="server" style="display: none">
                                    <h2 class="button_titulo_ultimas_vagas">
                                        <a id="btnCidadesMaisBuscadas" runat="server" href="/vagas-de-emprego" onclick="trackEvent('Default','Click', 'AbaCidadesMaisBuscadas_TituloTopo'); return true;">Cidades mais buscadas</a>
                                    </h2>
                                    <asp:Panel ID="pnlCidadesMaisBuscadas" CssClass="painel_vagas_mais_buscadas" runat="server" Style="text-align: center">
                                        <ul>
                                            <li>
                                                <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-em-belo-horizonte-mg" title="Vagas de emprego em Belo Horizonte / MG">Vagas de emprego em Belo Horizonte / MG</a><br />
                                                <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-em-rio-de-janeiro-rj" title="Vagas de emprego em Rio de Janeiro / Rj">Vagas de emprego em Rio de Janeiro / RJ</a><br />
                                                <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-em-sao-paulo-sp" title="Vagas de emprego em São Paulo / SP">Vagas de emprego em São Paulo / SP</a><br />
                                                <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-em-porto-alegre-rs" title="Vagas de emprego em Porto Alegre / RS">Vagas de emprego em Porto Alegre / RS</a><br />
                                                <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-em-florianopolis-sc" title="Vagas de emprego em Florianópolis / SC">Vagas de emprego em Florianópolis / SC</a><br />
                                                <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-em-curitiba-pr" title="Vagas de emprego em Curitiba / PR">Vagas de emprego em Curitiba / PR</a><br />
                                                <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-em-brasilia-df" title="Vagas de emprego em Brasília / DF">Vagas de emprego em Brasília / DF</a><br />
                                                <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-em-fortaleza-ce" title="Vagas de emprego em Fortaleza / CE">Vagas de emprego em Fortaleza / CE</a>
                                            </li>
                                        </ul>
                                    </asp:Panel>
                                </div>
                                <asp:Panel ID="pnlVagasDefault" runat="server">
                                    <div class="botao_ver_mais_vagas">
                                        <span onclick="ExibeDivUltimasVagas(); trackEvent('Default','Click','UltimasVagas_Aba'); return true;">Últimas vagas</span>
                                    </div>
                                    <div class="botao_ver_vagas_mais_buscadas">
                                        <span onclick="ExibeDivVagasMaisBuscadas(); trackEvent('Default','Click','VagasMaisBuscadas_Aba'); return true;">Vagas mais buscadas</span>
                                    </div>
                                    <div class="botao_ver_cidades_mais_buscadas">
                                        <span onclick="ExibeDivCidadesMaisBuscadas(); trackEvent('Default','Click','CidadeMaisBuscadas_Aba'); return true;">Cidades mais buscadas</span>
                                    </div>
                                </asp:Panel>
                            </div>

                            <%--INICIO: Depoimentos--%>
                            <br />
                            <div class="Titulo-Depoimento">
                                <hr color="#1A4784" />
                                <span class="depoimento_span">Palavras de quem usa o BNE
                                </span></hr>
                            </div>

                            <div class="containerDepoimento">

                                <ul id="carrousel_depoimento" class="jcarousel-skin-carrousel_depoimento">
                                    <li id="depoimento1">
                                        <div class="depoimento1">
                                            <asp:Label runat="server" ID="lblDepoimento1" Text="Cadastrei meu currículo e em menos de 30 dias fui chamado para uma entrevista e já estou trabalhando. E mesmo já empregado fui chamado pela segunda vez por outra grande empresa. Deixo aqui meus agradecimentos ao Banco Nacional de Empregos. Incentivo a todos a cadastrarem seus currículos!!!"> </asp:Label>
                                            <br />
                                            <br />
                                            <b style="font-weight: bold;">Flavio - Curitiba/PR</b>
                                        </div>
                                    </li>
                                    <li id="depoimento2">
                                        <div class="depoimento1">
                                            <asp:Label runat="server" ID="Label1" Text="Agradeço o apoio e empenho de Vcs, consegui o trabalho na minha região...sou-lhes muito Grata..Obrigada."></asp:Label>
                                            <br />
                                            <br />
                                            <b style="font-weight: bold;">Ana- Fortaleza/CE</b>
                                        </div>
                                    </li>
                                    <li id="depoimento3">
                                        <div class="depoimento1">
                                            <asp:Label runat="server" ID="Label3" Text="É um excelente site, me ajudou bastante na busca por um emprego."> </asp:Label>
                                            <br />
                                            <br />
                                            <b style="font-weight: bold;">Bianca- Duque de caxias/RJ</b>
                                        </div>
                                    </li>
                                    <li id="depoimento4">
                                        <div class="depoimento1">
                                            <asp:Label runat="server" ID="Label2" Text="Realmente é um site de extrema importância e é perfeito para quem quer ingressar no mercado de trabalho. O site está de parabéns e espero que essa iniciativa se desenvolva ainda mais."></asp:Label>
                                            <br />
                                            <br />
                                            <b style="font-weight: bold;">Josielen- Ananindeua/PA</b>
                                        </div>
                                    </li>

                                </ul>

                                <div class="jcarousel-control " style="margin-left: 372px; margin-top: 4px; position: absolute; width: 46px;">
                                    <a href="#" id="d1">
                                        <img id="dep1" src="img/banner_rotativo/bolinha.png" alt="" style="border: 0" /></a>
                                    <a href="#" id="d2">
                                        <img id="dep2" src="img/banner_rotativo/bolinha.png" alt="" style="border: 0" /></a>
                                    <a href="#" id="d3">
                                        <img id="dep3" src="img/banner_rotativo/bolinha.png" alt="" style="border: 0" /></a>
                                    <a href="#" id="d4">
                                        <img id="dep4" src="img/banner_rotativo/bolinha.png" alt="" style="border: 0" /></a>

                                </div>
                            </div>
                            <%--FIM: Depoimentos--%>

                            <%--START:Banners R1 CIA e Clientes--%>
                            <div class="containerBanners">
                                <div class="containerBanner1">
                                    <div class="bannerServicos">
                                        <ul id="carrousel_servicos" class="jcarousel-skin-carrousel_servicos">
                                            <li id="Item1">
                                                <%--<asp:HyperLink runat="server" ID="hlR1">
                                                    <img src="img/banner_rotativo/servicos_bne/imgR1.png" class="imgSlide" width="388" height="82" alt="Clique e saiba mais!" title="Clique e saiba Mais!"  style="border: 0;"/>
                                                </asp:HyperLink>--%>
                                                <asp:HyperLink runat="server" ID="hlCampanha">
                                                    <img src="img/banner_rotativo/servicos_bne/banner-campanhas.jpg" class="imgSlide" width="382" height="73" alt="Clique e saiba mais!" title="Clique e saiba Mais!"  style="margin-top: 9px; border: 0;" onclick="trackEvent('Default','Click','BannerCampanha'); return true;"/>
                                                </asp:HyperLink>
                                            </li>
                                            <li id="Item2">
                                                <asp:HyperLink runat="server" ID="hlCIA">
                                                    <img src="img/banner_rotativo/servicos_bne/img-cia.jpg"  class="imgSlide" width="382" height="73" alt="Clique e saiba mais!" title="Clique e saiba Mais!" style="margin-top: 9px; border: 0;" onclick="trackEvent('Default','Click','BannerCia'); return true;"/>
                                                </asp:HyperLink>
                                            </li>
                                            <li id="Item3">
                                                <a id="hlSMS" href="/sms-selecionadora">
                                                    <img src="img/banner_rotativo/servicos_bne/banner_sms.jpg" class="imgSMS" width="382" height="73" alt="Clique e saiba mais!" title="Clique e saiba Mais!" style="margin-top: 9px; border: 0;" onclick="trackEvent('Default','Click','BannerSmsSelecionadora'); return true;" />
                                                </a>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="jcarousel-control" style="margin-left: 172px; margin-top: 4px; position: absolute; width: 36px;">
                                        <a href="#" id="a1">
                                            <img id="b1" src="img/banner_rotativo/bolinha.png" alt="" style="border: 0" /></a>
                                        <a href="#" id="a2">
                                            <img id="b2" src="img/banner_rotativo/bolinha.png" alt="" style="border: 0" /></a>
                                        <a href="#" id="a3">
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
                                                            <img src='<%# DataBinder.Eval(Container.DataItem, "DescricaoCaminhoImagem") %>' style="border: 0" class="imgSlide empresa" width="100" height="38" alt="" onclick="trackEvent('Default','Click','<%# DataBinder.Eval(Container.DataItem,"DescricaoNomeURl").ToString() + " BannerClientesEmpresas" %>'); return true;"/>
                                                        </asp:HyperLink>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </div>
                                    <div class="jcarousel-control" style="margin-left: 240px; margin-top: 4px; position: absolute; width: 38px;">
                                        <a href="#" id="aImg1">
                                            <img id="Img1" src="img/banner_rotativo/bolinha.png" style="border: 0" alt="" /></a>
                                        <a href="#" id="aImg2">
                                            <img id="Img2" src="img/banner_rotativo/bolinha.png" style="border: 0" alt="" /></a>
                                        <a href="#" id="aImg3">
                                            <img id="Img3" src="img/banner_rotativo/bolinha.png" style="border: 0" alt="" /></a>
                                    </div>
                                </div>
                            </div>
                            <%--FIM:Banners R1 CIA e Clientes--%>
                        </div>

                    </asp:Panel>
                    <asp:Panel ID="pnlConteudoRHOffice" runat="server">
                        <asp:Literal ID="litConteudoRHOffice" runat="server"></asp:Literal>
                    </asp:Panel>
                </div>
            </div>
            <asp:Panel runat="server" ID="stc_spacer_" Style="height: 380px;"></asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>



    <asp:Panel ID="pnlPreCadastro" runat="server" Visible="false">
        <div class="preCadastroClose" id="PreCadastroClose">
            <a onclick="AbrirBoxLateral()" class="linksAcoes">
                <i class="fa fa-chevron-up"></i>
            </a>
        </div>
        <div id="divOpen" class="preCadastroOpen">


            <div>
                <a onclick="FecharBoxLateral()" class="linksAcoes">

                    <i class="fa fa-times bnt_sair_modal"></i>
                </a>
            </div>
            <asp:Panel ID="pnlPreCad" runat="server">
                <h4 class="titulo-modal-gratis">Quer receber vagas? É grátis! ;)</h4>
                &nbsp;
                            <div class="row">

                                <div class="form-group col-md-12 input_modal">
                                    <div class="Validate2">

                                        <asp:RequiredFieldValidator ID="rfvNome" runat="server" ValidationGroup="vlgPreCadastro"
                                            ControlToValidate="txtNomeC" CssClass="Validate" ErrorMessage="Campo Obrigatório."></asp:RequiredFieldValidator>
                                    </div>

                                    <asp:TextBox ID="txtNomeC" MaxLength="100" placeholder="Nome Completo" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="form-group col-md-6 input_modal">
                                    <div class="container_campo">
                                        <div class="valAlign">
                                            <asp:RegularExpressionValidator ID="regexEmail"
                                                runat="server"
                                                ErrorMessage="E-mail Inválido."
                                                ControlToValidate="txtEmail"
                                                CssClass="Validate2"
                                                ValidationGroup="vlgPreCadastro"
                                                ValidationExpression="(\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)">
                                            </asp:RegularExpressionValidator>

                                            <asp:RequiredFieldValidator ID="rfvEmail"
                                                runat="server"
                                                CssClass="Validate2"
                                                ControlToValidate="txtEmail"
                                                ValidationGroup="vlgPreCadastro" />
                                        </div>
                                        <asp:TextBox ID="txtEmail" MaxLength="100" runat="server" placeholder="Email" CssClass="form-control" />
                                    </div>
                                </div>
                            </div>
                <div class="form-group col-md-12 input_modal">
                    <%--cvFuncaoPrincipal_Validate--%>

                    <div class="valAlign">
                        <asp:CustomValidator ID="cvFuncaoPre" runat="server" CssClass="Validate2"
                            ErrorMessage="Função Inexistente." ClientValidationFunction="cvFuncaoPre_Validate"
                            ControlToValidate="txtFuncaoPre" ValidationGroup="vlgPreCadastro">
                        </asp:CustomValidator>

                        <div id="divFuncaoInexistentePre" style="display: none">
                        </div>

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="vlgPreCadastro"
                            ControlToValidate="txtFuncaoPre" CssClass="Validate2"></asp:RequiredFieldValidator>
                    </div>
                    <br />

                    <asp:TextBox ID="txtFuncaoPre" runat="server"
                        CssClass="form-control dados_alert" placeholder="Função Desejada"
                        CausesValidation="False"></asp:TextBox>
                </div>
                <div class="form-group col-md-6 input_modal">
                    <div class="Validate2">
                        <asp:CustomValidator ID="cvCidadePre" runat="server" CssClass="validate"
                            ErrorMessage="Cidade Inexistente." ClientValidationFunction="cvCidadePre_Validate"
                            ControlToValidate="txtCidadePre" ValidationGroup="vlgPreCadastro"></asp:CustomValidator>
                        <div id="divCidadeInexistentePre" style="display: none">
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="vlgPreCadastro"
                            ControlToValidate="txtCidadePre" CssClass="Validate" ErrorMessage="Campo Obrigatório."></asp:RequiredFieldValidator>
                    </div>
                    <asp:TextBox ID="txtCidadePre" CssClass="form-control" placeholder="Cidade" runat="server"></asp:TextBox>
                </div>
                <div class="form-group col-md-6">
                    <asp:LinkButton runat="server" ID="btnQuero" ValidationGroup="vlgPreCadastro"
                        OnClick="btnQuero_Click" CssClass=" btn_eu_quero">Eu quero!</asp:LinkButton>
                </div>

            </asp:Panel>

            <asp:Panel ID="pnlPreCadConfirmacao" runat="server" Visible="false">
                <div class="txt_confirmacao">

                    <i class="fa fa-check fa-2x"></i>
                    <asp:Label ID="lblTexto" runat="server" Text="Seu cadastro foi realizado com sucesso!" Font-Bold="true"></asp:Label>

                </div>
            </asp:Panel>
        </div>
    </asp:Panel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <Employer:DynamicScript runat="server" Src="js/local/default.js" type="text/javascript" />
    <Employer:DynamicScript runat="server" Src="js/jquery.jcarousel.min.js" type="text/javascript" />
    <Employer:DynamicScript runat="server" Src="/js/local/BannerPromocionalDobro/jquery.flexslider-min.js" type="text/javascript" />
</asp:Content>
