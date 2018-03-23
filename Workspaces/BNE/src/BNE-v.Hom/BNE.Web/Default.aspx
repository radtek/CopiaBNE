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
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/default.css" type="text/css" rel="stylesheet" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=0">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:UpdatePanel ID="upDefault" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <!-- Modal Mobile (?) -->
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
            <!-- Paineis de vagas -->
            <div class="paineis_home">
                <div class="paineis_home_wrapper">
                    <asp:Panel ID="pnlConteudoBNE" runat="server">
                        <div id="paineisVagas">
                            <!-- Últimas Vagas -->
                            <div id="divUltimasVagas" runat="server">
                                <h2 class="button_titulo_ultimas_vagas">
                                    <a id="btnUltimasVagas" runat="server" href="/vagas-de-emprego" onclick="trackEvent('Default','Click', 'AbaUltimasVagas_TituloTopo'); return true;"><strong>Últimas</strong> vagas</a>
                                </h2>
                                <asp:Panel ID="pnlUltimasVagas" CssClass="painel_ultimas_vagas" runat="server">
                                    <asp:Repeater ID="rptUltimasVagas" runat="server"
                                        OnItemDataBound="rptVagas_ItemDataBound">
                                        <HeaderTemplate>
                                            <ul>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li>
                                                <a href="<%# RetornarURLVaga(DataBinder.Eval(Container.DataItem, "Des_Area_BNE").ToString(), DataBinder.Eval(Container.DataItem, "Des_Funcao").ToString(),  DataBinder.Eval(Container.DataItem, "Nme_Cidade").ToString().Replace("/","-"), DataBinder.Eval(Container.DataItem, "Sig_Estado").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Idf_Vaga"))) + "?utm_term=VagaHome"%>">
                                                    <div>
                                                        <div class="horario_vagas">
                                                            <div class="texto_cadastrada_ha">
                                                                <asp:Literal ID="litUltimasVagasCadastradaHa" runat="server" Text="Cadastrada há"></asp:Literal>
                                                            </div>
                                                            <div class="texto_num_dias">
                                                                <asp:Label ID="lblHorario" runat="server" CssClass="horario"></asp:Label>
                                                            </div>
                                                        </div>

                                                        <div class="descvaga_ultimas-vagas">
                                                            <div class="descricao_funcao">
                                                                <asp:Label ID="HyperLink1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Des_Funcao").Equals("Estagiário") && DataBinder.Eval(Container.DataItem,"Des_curso") != DBNull.Value
                                                                                ? "Estágio de "+ DataBinder.Eval(Container.DataItem,"Des_curso")  
                                                                                : DataBinder.Eval(Container.DataItem, "Des_funcao")  %>'></asp:Label>
                                                            </div>
                                                            <div class="descricao_vaga">
                                                                <asp:Label ID="HyperLink2" runat="server" CssClass="vaga" Text='<%# RetornarDesricaoSalario(Eval("Vlr_Salario_De"), Eval("Vlr_Salario_Para"))%>'></asp:Label>
                                                                -
                                                                        <asp:Label ID="HyperLink3" runat="server" CssClass="vaga" Text='<%# Eval("Nme_Cidade").ToString().Contains("/") ? Eval("Nme_Cidade").ToString() :  FormatarCidade(Eval("Nme_Cidade").ToString(), Eval("Sig_Estado").ToString()) %>'></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </a>

                                            </li>

                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </asp:Panel>
                            </div>
                            <!-- Vagas mais buscadas -->
                            <div id="divVagasMaisBuscadas" runat="server">
                                <h2 class="button_titulo_ultimas_vagas">
                                    <a id="btnAbaVagasMaisBuscadas" runat="server" href="/vagas-de-emprego" onclick="trackEvent('Default','Click', 'AbaVagasMaisBuscadas_TituloTopo'); return true;">Vagas <strong>mais buscadas</strong></a>
                                </h2>
                                <asp:Panel ID="pnlVagasMaisBuscadas" CssClass="painel_vagas_mais_buscadas" runat="server" Style="text-align: center">
                                    <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-para-auxiliar-administrativo-em-belo-horizonte-mg" title="Vagas para Auxiliar Administrativo em Belo Horizonte / MG">Vagas para Auxiliar Administrativo em Belo Horizonte / MG</a>
                                    <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-para-recepcionista-em-rio-de-janeiro-rj" title="Vagas para Recepcionista em Rio de Janeiro / RJ">Vagas para Recepcionista em Rio de Janeiro / RJ</a>
                                    <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-para-assistente-administrativo-em-sao-paulo-sp" title="Vagas para Assistente Administrativo em São Paulo / SP">Vagas para Assistente Administrativo em São Paulo / SP</a>
                                    <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-para-motorista-em-rio-de-janeiro-rj" title="Vagas para Motorista em Rio de Janeiro / RJ">Vagas para Motorista em Rio de Janeiro / RJ</a>
                                    <%--<a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-para-vendedor-em-belo-horizonte-mg" title="Vagas para Vendedor em Belo Horizonte / MG">Vagas para Vendedor em Belo Horizonte / MG</a>
                                        <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-para-vendedor-externo-em-belo-horizonte-mg" title="Vagas para Vendedor Externo em Belo Horizonte / MG">Vagas para Vendedor Externo em Belo Horizonte / MG</a>
                                        <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-para-promotor-de-vendas-em-sao-paulo-sp" title="Vagas para Promotor de Vendas em São Paulo / SP">Vagas para Promotor de Vendas em São Paulo / SP</a>
                                        <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-para-auxiliar-de-servicos-gerais-em-rio-de-janeiro-rj" title="Vagas para Auxiliar de Serviços Gerais em Rio de Janeiro / RJ">Vagas para Auxiliar de Serviços Gerais em Rio de Janeiro / RJ</a>--%>
                                </asp:Panel>
                            </div>
                            <!-- Cidades mais buscadas  -->
                            <div id="divCidadesMaisBuscadas" runat="server">
                                <h2 class="button_titulo_ultimas_vagas">
                                    <a id="btnCidadesMaisBuscadas" runat="server" href="/vagas-de-emprego" onclick="trackEvent('Default','Click', 'AbaCidadesMaisBuscadas_TituloTopo'); return true;">Cidades <strong>mais buscadas</strong></a>
                                </h2>
                                <asp:Panel ID="pnlCidadesMaisBuscadas" CssClass="painel_vagas_mais_buscadas" runat="server" Style="text-align: center">
                                    <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-em-belo-horizonte-mg" title="Vagas de emprego em Belo Horizonte / MG">Vagas de emprego em Belo Horizonte / MG</a>
                                    <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-em-rio-de-janeiro-rj" title="Vagas de emprego em Rio de Janeiro / Rj">Vagas de emprego em Rio de Janeiro / RJ</a>
                                    <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-em-sao-paulo-sp" title="Vagas de emprego em São Paulo / SP">Vagas de emprego em São Paulo / SP</a>
                                    <a target="_blank" href="http://www.bne.com.br/vagas-de-emprego-em-curitiba-pr" title="Vagas de emprego em Curitiba / PR">Vagas de emprego em Curitiba / PR</a>
                                </asp:Panel>
                            </div>
                            <%-- <asp:Panel ID="pnlVagasDefault" runat="server">
                                    <div class="botao_ver_mais_vagas">
                                        <span onclick="ExibeDivUltimasVagas(); trackEvent('Default','Click','UltimasVagas_Aba'); return true;">Últimas vagas</span>
                                    </div>
                                    <div class="botao_ver_vagas_mais_buscadas">
                                        <span onclick="ExibeDivVagasMaisBuscadas(); trackEvent('Default','Click','VagasMaisBuscadas_Aba'); return true;">Vagas mais buscadas</span>
                                    </div>
                                    <div class="botao_ver_cidades_mais_buscadas">
                                        <span onclick="ExibeDivCidadesMaisBuscadas(); trackEvent('Default','Click','CidadeMaisBuscadas_Aba'); return true;">Cidades mais buscadas</span>
                                    </div>
                                </asp:Panel>--%>
                        </div>
                        <!-- Números -->
                        <div id="bne_numeros">
                            <div>
                                <i class="fa fa-file-text fa-3x"></i>
                                <h2>
                                    <asp:Literal runat="server" ID="litQtdCurriculos"></asp:Literal></h2>
                                <h6>Currículos</h6>
                            </div>
                            <div>
                                <i class="fa fa-window-maximize fa-3x"></i>
                                <h2>
                                    <asp:Literal runat="server" ID="litQtdVagas"></asp:Literal></h2>
                                <h6>Vagas</h6>
                            </div>
                            <div>
                                <i class="fa fa-industry fa-3x"></i>
                                <h2>
                                    <asp:Literal runat="server" ID="litQtdEmpresas"></asp:Literal></h2>
                                <h6>Empresas</h6>
                            </div>
                        </div>
                        <!-- Blocos -->
                        <div id="SecondRowHomeHolder">
                            <!-- Bloco Azul -->
                            <div class="quero_emprego">
                                <h2>Quer Emprego?</h2>
                                <h6>Não perca mais tempo,
                                    <br />
                                    <strong>comece agora mesmo</strong></h6>
                                <asp:LinkButton runat="server" ID="btiCadastreCurriculo" OnClick="btiCadastreCurriculo_Click" OnClientClick="trackEvent('Default','Click','QueroEmprego_Botao'); return true;">
                                    <div class="btn btn-default">Cadastrar Currículo</div>
                                </asp:LinkButton>
                                <asp:HyperLink runat="server" ID="hlQueroMaisCandidato">
                                    <div class="btn btn-default2" >
                                        Quero mais!
                                    </div>
                                </asp:HyperLink>
                            </div>
                            <!-- Bloco Verde -->
                            <div class="quero_contratar">
                                <h2>Quer Contratar?</h2>
                                <h6>Inicie seu recrutamento completo
                                    <br />
                                    e <strong>preencha suas vagas</strong></h6>
                                <asp:LinkButton runat="server" ID="btnCurriculoPesquisa" OnClick="btnCurriculoPesquisa_Click" OnClientClick="trackEvent('Default','Click','QueroContratar_Botao'); return true;">
                                    <div class="btn btn-default">Cadastrar Empresa</div>
                                </asp:LinkButton>
                                <asp:HyperLink runat="server" ID="hlQueroMaisEmpresa">
                                    <div class="btn btn-default2">
                                        Quero mais!
                                    </div>
                                </asp:HyperLink>
                            </div>
                        </div>
                        <!-- Parceiros -->
                        <div id="home-empresas_parceiras">
                            <h2>Empresas <strong>Parceiras</strong></h2>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="carousel carousel-showsixmoveone slide" id="empresas-parceiras">
                                        <div class="carousel-inner">
                                            <asp:Repeater ID="rptEmpresas" runat="server">
                                                <ItemTemplate>
                                                    <div class="item <%# Container.ItemIndex == 0 ? "active" : "" %>">
                                                        <div class="col-xs-12 col-sm-4 col-md-2">
                                                            <asp:HyperLink runat="server" NavigateUrl='<%# RetornarURLVagasEmpresa(DataBinder.Eval(Container.DataItem, "DescricaoNomeURl").ToString(), Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Filial.IdFilial"))) %>'>
                                                                <div class="centerflex">
                                                                    <img src='<%# DataBinder.Eval(Container.DataItem, "DescricaoCaminhoImagem") %>' class="img-responsive" onclick="trackEvent('Default','Click','<%# DataBinder.Eval(Container.DataItem,"DescricaoNomeURl") + " BannerClientesEmpresas" %>'); return true;"/>
                                                                </div>
                                                            </asp:HyperLink>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                        <a class="left carousel-control" href="#empresas-parceiras" data-slide="prev">
                                            <div>
                                                <img src="img/banner-rotativo/chevron-left.png">
                                            </div>
                                        </a>
                                        <a class="right carousel-control" href="#empresas-parceiras" data-slide="next">
                                            <div>
                                                <img src="img/banner-rotativo/chevron-right.png">
                                            </div>
                                        </a>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <!-- Vagas por Área -->
                        <div>
                            <div class="vagas_area">
                                <h2>Pesquise por <strong>área</strong></h2>
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
                                    <a href="/busca-de-vagas" onclick="trackEvent('Default','Click','PainelVagasArea_BotaoVejaMais'); return true;">VER MAIS</a>
                                </div>
                            </div>
                        </div>
                        <!-- Depoimentos -->
                        <div id="home-depoimentos_quem_usa">
                            <h2>Palavras de quem <strong>utiliza o BNE</strong></h2>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="carousel carousel-showtwomoveone slide" id="depoimentosbne">
                                        <div class="carousel-inner">
                                            <asp:Repeater ID="rptDepoimentos" runat="server">
                                                <ItemTemplate>
                                                    <div class="item <%# Container.ItemIndex == 0 ? "active" : "" %>">
                                                        <div class="col-sm-6 col-md-6">
                                                            <div>
                                                                <h5><%# DataBinder.Eval(Container.DataItem, "Descricao") %></h5>
                                                                <h6><%# DataBinder.Eval(Container.DataItem, "Nome") %> - <%# DataBinder.Eval(Container.DataItem, "Cidade") %></h6>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                        <a class="left carousel-control" href="#depoimentosbne" data-slide="prev">
                                            <div>
                                                <img src="img/banner-rotativo/chevron-left.png">
                                            </div>
                                        </a>
                                        <a class="right carousel-control" href="#depoimentosbne" data-slide="next">
                                            <div>
                                                <img src="img/banner-rotativo/chevron-right.png">
                                            </div>
                                        </a>
                                    </div>
                                </div>
                            </div>
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
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <Employer:DynamicScript runat="server" Src="js/local/default.js" type="text/javascript" />
    <Employer:DynamicScript runat="server" Src="js/jquery.jcarousel.min.js" type="text/javascript" />
</asp:Content>


