<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Empresa.aspx.cs" Inherits="BNE.Web.Empresa" %>

<!DOCTYPE html>
<html lang="en" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>BNE - Banco Nacional de Empregos | Empresas</title>
    <!-- Material Design, Roboto Font and Google icons-->
    <link rel="stylesheet" href="https://storage.googleapis.com/code.getmdl.io/1.0.5/material.indigo-red.min.css" />
    <link href='https://fonts.googleapis.com/css?family=Roboto:400,300,500,700' rel='stylesheet' type='text/css' />
    <link rel="stylesheet" href="https://fonts.googleapis.com/icon?family=Material+Icons" />
    <link href="//maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.min.css" />
    <!-- Main CSS (sass compiled) -->
    <Employer:DynamicHtmlLink runat="server" Href="/content/sass/styles.css" type="text/css" rel="stylesheet" />
    <!-- Adjust Screen -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <!-- Jquery -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>
    <!-- Material Design -->
    <script src="https://storage.googleapis.com/code.getmdl.io/1.0.6/material.min.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.min.js"></script>
    <Employer:DynamicScript runat="server" Src="/js/employerFramework.combined.js" type="text/javascript" />
    <Employer:DynamicScript runat="server" Src="/js/global/geral.js" type="text/javascript" />
    <Employer:DynamicScript runat="server" Src="/js/local/Empresa.js" type="text/javascript" />
</head>
<body class="homePJ">

    <form id="form1" runat="server" class="conteudo">
        <asp:ScriptManager ID="smGlobal" runat="server" AsyncPostBackTimeout="900" EnablePartialRendering="True"
            EnableScriptLocalization="True" EnableScriptGlobalization="True" EnablePageMethods="True" ScriptMode="Release">
        </asp:ScriptManager>

        <!-- Change User -->
        <section id="changeUser" class="mdl-grid mdl-grid--no-spacing" name="user">
            <div class="mdl-cell mdl-cell--2-col-phone mdl-cell--4-col-tablet mdl-cell--6-col right"><a href="/soucandidato"><strong>Sou Candidato</strong></a>&nbsp;&nbsp;|</div>
            <div class="mdl-cell mdl-cell--2-col-phone mdl-cell--4-col-tablet mdl-cell--6-col left">&nbsp;&nbsp;<a href="/souempresa"><strong> Sou Empresa</strong></a></div>
        </section>
        <!-- Header-->
        <section id="homeHeader">
            <div class="mdl-grid container">
                <div id="homeHeader-logo" class="mdl-cell mdl-cell--3-col-phone mdl-cell--3-col-tablet mdl-cell--3-col">
                    <a href="/">
                        <img src="content/img/logo-bne-empresas.png" alt="Empresas" /></a>
                </div>
                <div id="homeHeader-nav" class="mdl-cell mdl-cell--1-col-phone mdl-cell--5-col-tablet mdl-cell--9-col">
                    <ul>


                        <li>
                            <asp:LinkButton runat="server" ID="btlEntrar" OnClick="btlEntrar_OnClick" CausesValidation="False">Entrar</asp:LinkButton>
                        </li>
                        <li>
                            <a href="#homeCadastro">Criar Conta
                            </a>
                        </li>
                        <li>
                            <a href="#homeDepoimentos">Depoimentos
                            </a>
                        </li>
                        <li>
                            <a href="#homePlanos">Planos
                            </a>
                        </li>
                        <li>
                            <a href="#cadastroVaga">Anunciar Vaga
                            </a>
                        </li>
                        <li>
                            <a href="#homeBuscaAdvance">Pesquisa Currículos
                            </a>
                        </li>
                        <li>
                            <a href="#homeVideo" id="playVideo">Sobre o BNE
                            </a>
                        </li>
                    </ul>
                </div>
                <div id="homeHeader-nav-mobile" class="mdl-cell mdl-cell--1-col-phone mdl-cell--5-col-tablet">
                    <button id="demo-menu-lower-right" class="mdl-button mdl-js-button mdl-button--icon">
                        <i class="material-icons">&#xE5D2;</i>
                    </button>
                    <ul class="mdl-menu mdl-menu--bottom-right mdl-js-menu mdl-js-ripple-effect" for="demo-menu-lower-right">
                        <a href="#Planos">
                            <li class="mdl-menu__item">Planos</li>
                        </a>
                        <li>
                            <a href="#CriarConta">Criar Conta
                            </a>
                        </li>
                        <a href="#Depoimentos">
                            <li class="mdl-menu__item">Depoimentos</li>
                        </a>
                        <a href="#">
                            <li class="mdl-menu__item">Entrar</li>
                        </a>
                        <a href="#Vaga">
                            <li class="mdl-menu__item">Anunciar Vaga</li>
                        </a>
                        <a href="#PesquisarCurriculo">
                            <li class="mdl-menu__item">Pesquisar Currículos</li>
                        </a>
                        <a href="#homeVideo" id="playVideo">
                            <li class="mdl-menu__item">Sobre o BNE</li>
                        </a>
                    </ul>
                </div>
            </div>
        </section>
        <!-- Banner -->
        <section id="homeBanner" class="mdl-grid">
            <div class="mdl-cell mdl-cell--hide-phone mdl-cell--1-col-tablet mdl-cell--3-col"></div>
            <div class="mdl-cell mdl-cell--4-col-phone mdl-cell--6-col-tablet mdl-cell--6-col">
                <div class="homeBanner-title">
                    <h5><strong>BNE</strong> para empresas</h5>
                </div>
                <div class="homeBanner-subtitle">Inicie seu <strong>recrutamento completo</strong> e preencha suas vagas!</div>
                <div class="homeBanner-button">
                    <a href="#homeVideo">
                        <input type="button" value="Conheça o BNE" id="Banner-button"></a>
                </div>
            </div>
            <div class="mdl-cell mdl-cell--hide-phone mdl-cell--1-col-tablet mdl-cell--3-col"></div>
        </section>
        <!-- Números -->
        <section id="homeNumeros" class="mdl-grid container">
            <div class="mdl-cell mdl-cell--4-col">
                <div class="homeNumeros-icon"><i class="material-icons">&#xE880;</i></div>
                <div class="homeNumeros-title">
                    <h5>15 Milhões</h5>
                </div>
                <div class="homeNumeros-subtitle">de Currículos</div>
                <div class="homeNumeros-desc">Somos especializados em currículos <strong>operacionais</strong> e de <strong>apoio</strong></div>
            </div>
            <div class="mdl-cell mdl-cell--4-col">
                <div class="homeNumeros-icon"><i class="material-icons">&#xE1B1;</i></div>
                <div class="homeNumeros-title">
                    <h5>100% Integrado</h5>
                </div>
                <div class="homeNumeros-subtitle">SMS + Email</div>
                <div class="homeNumeros-desc">Comunicação <strong>100% integrada</strong> ao e-mail e celular dos candidatos</div>
            </div>
            <div class="mdl-cell mdl-cell--8-col-tablet mdl-cell--4-col">
                <div class="homeNumeros-icon"><i class="material-icons">&#xE89C;</i></div>
                <div class="homeNumeros-title">
                    <h5>+15 mil</h5>
                </div>
                <div class="homeNumeros-subtitle">Currículos Atualizados</div>
                <div class="homeNumeros-desc"><strong>Diariamente</strong> mais de 15 mil currículos novos atualizados em nossa base</div>
            </div>
        </section>
        <!-- Cadastro -->
        <section id="homeCadastro" class="mdl-grid container" name="CriarConta">
            <div class="mdl-cell mdl-cell--12-col center">
                <h5><strong>Crie sua conta grátis! </strong>Pesquise currículos e utilize ferramentas de recrutamento</h5>
            </div>
            <div class="mdl-cell mdl-cell--hide-phone mdl-cell--1-col-tablet mdl-cell--hide-desktop"></div>
            <div class="mdl-cell mdl-cell--4-col-phone mdl-cell--6-col-tablet mdl-cell--3-col">
                <asp:RequiredFieldValidator runat="server" ID="rvTxtNome" ControlToValidate="txtNome"
                    ValidationGroup="vlCadastroGratis"></asp:RequiredFieldValidator>
                <asp:TextBox runat="server" ID="txtNome" placeholder="Nome Completo" CssClass="textbox_padrao"></asp:TextBox>
            </div>
            <div class="mdl-cell mdl-cell--hide-phone mdl-cell--1-col-tablet mdl-cell--hide-desktop"></div>
            <div class="mdl-cell mdl-cell--hide-phone mdl-cell--1-col-tablet mdl-cell--hide-desktop"></div>
            <div class="mdl-cell mdl-cell--4-col-phone mdl-cell--6-col-tablet mdl-cell--3-col">
                <asp:RequiredFieldValidator runat="server" ID="rfvtxtEmail" ControlToValidate="txtEmail"
                    ValidationGroup="vlCadastroGratis"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator runat="server" ID="revTxtEmail" ControlToValidate="txtEmail"
                    ValidationGroup="vlCadastroGratis" ErrorMessage="Email Inválido"></asp:RegularExpressionValidator>
                <asp:TextBox ID="txtEmail" runat="server" placeholder="Email" CssClass="textbox_padrao"></asp:TextBox>
            </div>
            <div class="mdl-cell mdl-cell--hide-phone mdl-cell--1-col-tablet mdl-cell--hide-desktop"></div>
            <div class="mdl-cell mdl-cell--hide-phone mdl-cell--1-col-tablet mdl-cell--hide-desktop"></div>
            <div class="mdl-cell mdl-cell--4-col-phone mdl-cell--6-col-tablet mdl-cell--3-col">
                <Componentes:ControlCNPJ runat="server" Obrigatorio="true" MensagemErroFormato="CNPJ Inválido" ValidationGroup="vlCadastroGratis" ID="txtCNPJ" placeholder="CNPJ"/>
            </div>
            <div class="mdl-cell mdl-cell--hide-phone mdl-cell--1-col-tablet mdl-cell--hide-desktop"></div>
            <div class="mdl-cell mdl-cell--hide-phone mdl-cell--2-col-tablet mdl-cell--hide-desktop"></div>
            <div class="mdl-cell mdl-cell--4-col-phone mdl-cell--4-col-tablet mdl-cell--3-col">
                <asp:Button runat="server" ID="btnCadastrar" Text="cadastrar grátis" OnClick="btnCadastrar_OnClick"
                    ValidationGroup="vlCadastroGratis" UseSubmitBehavior="False" CausesValidation="True" />
            </div>
            <div class="mdl-cell mdl-cell--hide-phone mdl-cell--2-col-tablet mdl-cell--hide-desktop"></div>
        </section>
        <!-- Vídeos -->
        <section id="homeVideo" class="mdl-grid mdl-grid--no-spacing" name="homeVideo">
            <div class="mdl-cell mdl-cell--12-col">
                <h5>Conheça um pouco sobre o BNE</h5>
            </div>
            <div class="mdl-cell mdl-cell--12-col">
                <iframe src="https://www.youtube.com/embed/ucE371I2dyU?rel=0&amp;showinfo=0" frameborder="0" allowfullscreen id="BNEEmpresa"></iframe>
            </div>
        </section>
        <!-- Cadastro Vaga -->
        <section id="cadastroVaga" name="Vaga">
            <div class="mdl-grid container">
                <div class="mdl-cell mdl-cell--12-col">
                    <h5>Publique sua vaga. É fácil, rápido e de graça!</h5>
                </div>
                <div class="mdl-cell mdl-cell--1-col"></div>
                <div class="mdl-cell mdl-cell--5-col mdl-cell--6-col-tablet">
                    <div>
                        <asp:RequiredFieldValidator ID="rfvFuncaoVaga" runat="server" ControlToValidate="txtFuncaoVaga"
                            ValidationGroup="SalvarVaga"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cvFuncaoVaga" runat="server" ControlToValidate="txtFuncaoVaga" ClientValidationFunction="cvFuncaoVaga_Validate"
                            ErrorMessage="Função Inválida" ValidationGroup="SalvarVaga"></asp:CustomValidator>
                    </div>
                    <asp:TextBox ID="txtFuncaoVaga" runat="server" placeholder="Função"></asp:TextBox>
                </div>
                <div class="mdl-cell mdl-cell--hide-phone mdl-cell--1-col-tablet mdl-cell--hide-desktop"></div>
                <div class="mdl-cell mdl-cell--hide-phone mdl-cell--1-col-tablet mdl-cell--hide-desktop"></div>
                <div class="mdl-cell mdl-cell--5-col mdl-cell--6-col-tablet">
                    <div>
                        <asp:RequiredFieldValidator ID="rfCidadeVaga" ValidationGroup="SalvarVaga" runat="server"
                            ControlToValidate="txtCidadeVaga"></asp:RequiredFieldValidator>
                        <asp:CustomValidator ID="cvCidadeVaga" runat="server" ErrorMessage="Cidade Inválida."
                            ClientValidationFunction="cvCidadeVaga_Validate" ControlToValidate="txtCidadeVaga"
                            ValidationGroup="SalvarVaga"></asp:CustomValidator>
                    </div>
                    <asp:TextBox ID="txtCidadeVaga" runat="server" placeholder="Cidade/Estado"></asp:TextBox>
                </div>
                <div class="mdl-cell mdl-cell--1-col"></div>
                <div class="mdl-cell mdl-cell--1-col"></div>
                <div class="mdl-cell mdl-cell--5-col mdl-cell--6-col-tablet relative">
                    <componente:ValorDecimal ID="txtSalarioDe" Obrigatorio="false" CssClassTextBox="salario" CssClass="salario"
                        runat="server" CasasDecimais="2" ValidationGroup="SalvarVaga" placeholder="Salário Mínimo"/>
                    <componente:ValorDecimal ID="txtSalarioAte" Obrigatorio="false" CssClassTextBox="salario" CssClass="salario"
                        runat="server" CasasDecimais="2" ValidationGroup="SalvarVaga" placeholder="Salário Máximo" />
                    <i class="material-icons" id="tt1" tabindex="0"></i>
                    <div class="mdl-tooltip" for="tt1">
                        <div class="mdl-tooltip-nm">Salário</div>
                        <div class="mdl-tooltip-desc">Informe o salário da vaga. Vagas com salário recebem o dobro de inscritos</div>
                    </div>
                </div>
                <div class="mdl-cell mdl-cell--hide-phone mdl-cell--1-col-tablet mdl-cell--hide-desktop"></div>
                <div class="mdl-cell mdl-cell--hide-phone mdl-cell--1-col-tablet mdl-cell--hide-desktop"></div>
                <div class="mdl-cell mdl-cell--5-col mdl-cell--6-col-tablet relative">
                    <div>
                        <asp:RequiredFieldValidator ID="rfvEmailVaga" runat="server" ValidationGroup="SalvarVaga"
                            ControlToValidate="txtEmailVaga"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revEmailVaga" runat="server" ValidationGroup="SalvarVaga"
                            ControlToValidate="txtEmailVaga" ErrorMessage="Email Inválido"></asp:RegularExpressionValidator>
                    </div>
                    <asp:TextBox ID="txtEmailVaga" CssClass="textbox_padrao campo_obrigatorio" runat="server" placeholder="Email para retorno"></asp:TextBox>
                    <i class="material-icons intro" id="tt2" tabindex="0"></i>
                    <div class="mdl-tooltip" for="tt2">
                        <div class="mdl-tooltip-nm">Email</div>
                        <div class="mdl-tooltip-desc">Informe o seu e-mail para recebimento. Seu e-mail não será divulgado para candidatos.</div>
                    </div>
                </div>
                <div class="mdl-cell mdl-cell--1-col"></div>
                <div class="mdl-cell mdl-cell--1-col"></div>
                <div class="mdl-cell mdl-cell--5-col mdl-cell--6-col-tablet relative">
                    <componente:AlfaNumerico CssClassTextBox="textbox_padrao campo_obrigatorio textbox_nome"
                        ID="txtNomeVaga" runat="server" Columns="40" MensagemErroFormato="<%$ Resources: MensagemAviso, _100107 %>"
                        MensagemErroIntervalo="<%$ Resources: MensagemAviso, _100107 %>" MensagemErroValor="<%$ Resources: MensagemAviso, _100107 %>"
                        ValidationGroup="SalvarVaga" ClientValidationFunction="ValidarNome" placeholder="Nome" />
                    <i class="material-icons intro" id="tooltipNomeVaga" tabindex="0"></i>
                    <div class="mdl-tooltip" for="tooltipNomeVaga">
                        <div class="mdl-tooltip-nm">Nome</div>
                        <div class="mdl-tooltip-desc">Informe o seu nome para contato. Seu nome não será divulgado para candidatos.</div>
                    </div>
                </div>
                <div class="mdl-cell mdl-cell--hide-phone mdl-cell--1-col-tablet mdl-cell--hide-desktop"></div>
                <div class="mdl-cell mdl-cell--hide-phone mdl-cell--1-col-tablet mdl-cell--hide-desktop"></div>
                <div class="mdl-cell mdl-cell--5-col mdl-cell--6-col-tablet relative">
                    <componente:Telefone ID="txtTelefoneVaga" runat="server" MensagemErroFormatoFone="<%$ Resources: MensagemAviso, _100006 %>"
                        ValidationGroup="SalvarVaga" Tipo="FixoCelular" placeholderFone="Telefone" placeholderDDD="DDD" />
                    <i class="material-icons intro" id="tooltipTelefoneVaga" tabindex="0"></i>
                    <div class="mdl-tooltip" for="tooltipTelefoneVaga">
                        <div class="mdl-tooltip-nm">Telefone</div>
                        <div class="mdl-tooltip-desc">Informe o seu telefone para contato. Seu telefone não será divulgado para candidatos.</div>
                    </div>
                </div>
                <div class="mdl-cell mdl-cell--1-col"></div>
                <div class="mdl-cell mdl-cell--1-col"></div>
                <div class="mdl-cell mdl-cell--10-col mdl-cell--6-col-tablet">
                    <asp:TextBox ID="txtAtribuicoesVaga" runat="server" CssClass="textarea_padrao" Columns="99999" Rows="4"
                        TextMode="MultiLine" placeholder="Informe as atividades que serão realizadas"></asp:TextBox>
                </div>
                <div class="mdl-cell mdl-cell--1-col"></div>
                <div class="mdl-cell mdl-cell--12-col center">
                    <asp:Button runat="server" ID="btnSalvarVaga" Text="Publicar Vaga" OnClick="btnSalvarVaga_OnClick"
                        UseSubmitBehavior="False" CausesValidation="True" ValidationGroup="SalvarVaga" />
                </div>
            </div>
            <div style="text-align: center; width: 100%;" id="modal_sucesso_vaga">
                <input type="checkbox" id="modal_sucesso_vaga_check" runat="server" />
                <div class="modal">
                    <div class="modal-content">
                        <div class="modal-content-header">
                            Vaga Cadastrada com Sucesso!
                        </div>
                        <div class="modal-content-body">
                            <i class="fa fa-check-circle icone_sucesso"></i>
                            <p>
                                Código da Vaga <b>
                                    <asp:Literal ID="litCodigoVaga" runat="server"></asp:Literal></b>.<br />
                                Sua vaga foi cadastrada com sucesso e será publicada em breve.
                            </p>
                        </div>
                        <asp:Button runat="server" ID="btnConfirmarVaga" Text="CONFIRMAR" OnClick="btnConfirmarVaga_OnClick" UseSubmitBehavior="False" CausesValidation="False" />
                    </div>
                    <label class="modal-close" for="modal_sucesso_vaga_check"></label>
                </div>
            </div>
        </section>

        <!--Pesquisar Curriculos-->
        <section id="homeBuscaAdvance" name="PesquisarCurriculo" class="mdl-grid mdl-grid--no-spacing">
            <div class="mdl-cell mdl-cell--12-col center">
                <h5>Aproveite para fazer uma pesquisa de currículos!</h5>
            </div>
            <div class="mdl-cell mdl-cell--12-col center">
                <input id="btnPesquisarCurriculo" type="button" value="Pesquisar Currículos" />
            </div>
        </section>
        <!-- Planos -->
        <section id="homePlanos" class="mdl-grid container" name="Planos">
            <div class="mdl-cell mdl-cell--12-col center">
                <h5>Conheça <strong>nossos Planos</strong></h5>
            </div>
            <%--INICIO PLANO 100--%>
            <div class="mdl-cell mdl-cell--6-col-tablet mdl-cell--4-col plano50">
                <div class="homePlanos-title">
                    Plano Mensal
                    <div>100</div>
                </div>
                <div class="homePlanos-advantages">
                    <ul>
                        <li><strong>100</strong> acessos completos aos currículos</li>
                        <li><strong>100</strong> envios de SMS para candidatos</li>
                        <li><strong>100</strong> envios de email para candidatos</li>
                        <li>Vagas divulgadas <strong>imediatamente</strong></li>
                        <li>Divulgação <strong>ilimitada</strong> de vagas</li>
                        <li>Envio de <strong>alerta de vagas</strong> para os candidatos</li>
                    </ul>
                </div>
                <div class="homePlanos-action">
                    <a runat="server" id="a1">
                        <input type="button" value="mais detalhes"></a>
                </div>
            </div>
            <%--FIM PLANO 100--%>

            <%--INICIO PLANO 50--%>
            <div class="mdl-cell mdl-cell--6-col-tablet mdl-cell--4-col plano50">
                <div class="homePlanos-title">
                    Plano Mensal
                    <div>50</div>
                </div>

                <div class="homePlanos-advantages">
                    <ul>
                        <li><strong>50</strong> acessos completos aos currículos</li>
                        <li><strong>50</strong> envios de SMS para candidatos</li>
                        <li><strong>50</strong> envios de email para candidatos</li>
                        <li>Vagas divulgadas <strong>imediatamente</strong></li>
                        <li>Divulgação <strong>ilimitada</strong> de vagas</li>
                        <li>Envio de <strong>alerta de vagas</strong> para os candidatos</li>
                    </ul>
                </div>
                <div class="homePlanos-action">
                    <a runat="server" id="anchorPlanos1">
                        <input type="button" value="mais detalhes"></a>
                </div>
            </div>

            <div class="mdl-cell mdl-cell--hide-phone mdl-cell--1-col-tablet mdl-cell--hide-desktop"></div>
            <div class="mdl-cell mdl-cell--hide-phone mdl-cell--1-col-tablet mdl-cell--hide-desktop"></div>
            <a runat="server" id="anchorPlanos2" class="mdl-cell mdl-cell--6-col-tablet mdl-cell--4-col planoilimitado">
                <div>Outras opções de planos</br></div>
                <div class="divider"></div>
                <div>Converse com nossos</br>consultores</div>
            </a>
            <div class="mdl-cell mdl-cell--hide-phone mdl-cell--1-col-tablet mdl-cell--hide-desktop"></div>
        </section>
        <!-- Depoimentos -->
        <section id="homeDepoimentos" class="center" name="Depoimentos">
            <div class="mdl-tabs mdl-js-tabs ">

                <div class="mdl-tabs__panel is-active" id="dep01">
                    <div class="mdl-grid">
                        <div class="mdl-cell mdl-cell--12-col">
                            <img src="content/img/depoimento-01.jpg" class="profileDepoimento">
                        </div>
                        <div class="mdl-cell mdl-cell--1-col-tablet  mdl-cell--3-col"></div>
                        <div class="mdl-cell mdl-cell--6-col depoimento">"O BNE é uma das ferramentas utilizadas pela nossa consultoria na área de seleção de profissionais, que contribui eficientemente na busca de profissionais nos mais variados níveis profissionais, empresas de pequeno grande e médio porte, regiões e/ou Estados."</div>
                        <div class="mdl-cell mdl-cell--1-col-tablet  mdl-cell--3-col"></div>
                        <div class="mdl-cell mdl-cell--12-col">
                            <strong>Sandra Maria Salles | Diretora Comercial</strong><br>
                            Consultoria Empresarial Nacional
                        </div>
                    </div>
                </div>
                <div class="mdl-tabs__panel" id="dep02">
                    <div class="mdl-grid">
                        <div class="mdl-cell mdl-cell--12-col">
                            <img src="content/img/depoimento-02.jpg" class="profileDepoimento">
                        </div>
                        <div class="mdl-cell mdl-cell--1-col-tablet  mdl-cell--3-col"></div>
                        <div class="mdl-cell mdl-cell--6-col depoimento">"Consegui preencher minha equipe, estava a muito tempo procurando um profissional com experiência em licitações e encontrei! Os candidatos do BNE geralmente são bons e alinhados com a realidade de mercado."</div>
                        <div class="mdl-cell mdl-cell--1-col-tablet  mdl-cell--3-col"></div>
                        <div class="mdl-cell mdl-cell--12-col">
                            <strong>Adélia Margarida | Coordenadora de RH</strong><br>
                            Administradora de Shoppings | Bahia, Minas Gerais, Pernambuco e Amazonas
                        </div>
                    </div>
                </div>
                <div class="mdl-tabs__panel" id="dep04">
                    <div class="mdl-grid">
                        <div class="mdl-cell mdl-cell--12-col">
                            <img src="content/img/depoimento-04.jpg" class="profileDepoimento">
                        </div>
                        <div class="mdl-cell mdl-cell--1-col-tablet mdl-cell--3-col"></div>
                        <div class="mdl-cell mdl-cell--6-col depoimento">"Utilizamos o BNE há muito anos e conseguimos acompanhar a evolução da empresa, possuem como foco a melhoria continua e consequentemente os resultados são atingidos através de inovações que facilitam parte do processo de recrutamento e seleção. Além disso a equipe do BNE está sempre a disposição nos atendendo com rapidez e qualidade."</div>
                        <div class="mdl-cell mdl-cell--1-col-tablet mdl-cell--3-col"></div>
                        <div class="mdl-cell mdl-cell--12-col">
                            <strong>Fabiolla Mattozo | Selecionadora</strong><br />
                            Cliente desde 05/01/2001<br />
                            Assessoria em RH | Paraná e São Paulo
                        </div>
                    </div>
                </div>
                <div class="mdl-tabs__tab-bar">
                    <a href="#dep01" class="mdl-tabs__tab is-active">
                        <img src="content/img/depoimento-01.jpg" class="profileDepoimento"></a>
                    <a href="#dep02" class="mdl-tabs__tab">
                        <img src="content/img/depoimento-02.jpg" class="profileDepoimento"></a>
                    <a href="#dep04" class="mdl-tabs__tab">
                        <img src="content/img/depoimento-04.jpg" class="profileDepoimento"></a>
                </div>
            </div>
        </section>
        <!-- Rodapé -->
        <section id="homeFooter">
            <div class="mdl-grid container">
                <div id="homeFooter-info" class="mdl-cell mdl-cell--8-col-tablet mdl-cell--4-col">
                    <strong>BNE - Banco Nacional de Empregos</strong><br>
                    0800 41 2400
                </div>
                <div class="mdl-cell mdl-cell--4-col-phone mdl-cell--8-col-tablet mdl-cell--8-col">
                    <ul>
                        <li>
                            <a href="#changeUser" class="backtotop">Voltar para o topo <i class="material-icons">&#xE5C7;</i>
                            </a>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="btlEntrarRodape" OnClick="btlEntrar_OnClick" CausesValidation="False" CssClass="desktop-links">Entrar</asp:LinkButton>
                        </li>
                        <li>
                            <a href="#homeCadastro" class="desktop-links">Criar Conta
                            </a>
                        </li>
                        <li>
                            <a href="#homeDepoimentos" class="desktop-links">Depoimentos
                            </a>
                        </li>
                        <li>
                            <a href="#homePlanos" class="desktop-links">Planos
                            </a>
                        </li>
                        <li>
                            <a href="#cadastroVaga" class="desktop-links">Anunciar Vaga
                            </a>
                        </li>
                        <li>
                            <a href="#homeBuscaAdvance" class="desktop-links">Pesquisar Currículos
                            </a>
                        </li>
                        <li>
                            <a href="#homeVideo" id="playVideofooter" class="desktop-links">Sobre o BNE
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
        </section>
    </form>
    <script type="text/javascript">
        (function (i, s, o, g, r, a, m) {
            i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                (i[r].q = i[r].q || []).push(arguments)
            }, i[r].l = 1 * new Date(); a = s.createElement(o),
                m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
        })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');

        var uri = document.baseURI || document.URL;
        var baseAddress = uri.replace("http://", "").replace("https://", "");
        if (baseAddress.indexOf("www.bne.com.br") == 0 || baseAddress.indexOf("bne.com.br") == 0)
            ga('create', 'UA-1937941-6', 'auto');
        else
            ga('create', 'UA-1937941-8', 'auto');

        ga('send', 'pageview');
    </script>
</body>
</html>
