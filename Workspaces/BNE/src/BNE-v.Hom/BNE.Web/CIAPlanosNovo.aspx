<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="CiaPlanosNovo.aspx.cs" Inherits="BNE.Web.CiaPlanosNovo" %>

<%@ Register Src="UserControls/Modais/EnvioEmail.ascx" TagName="ModalEnvioEmail" TagPrefix="uc" %>
<%@ Register Src="UserControls/Modais/ucWebCallBack_Modais.ascx" TagName="WebCallBack_Modais" TagPrefix="uc" %>

<asp:Content ID="ContentExperimentos" ContentPlaceHolderID="cphExperimentos" runat="server">
    <!-- Google Analytics Content Experiment code -->
    <!-- 1. Load the Content Experiments JavaScript Client -->
    <script src="//www.google-analytics.com/cx/api.js?experiment=h6aEAda2TuOtgMGK3TOOAQ"></script>
    <script>
        var css_variations = [
            'tabelaPrecoAbaixo',
            'tabelaPrecoTopo'
        ];

        // 2. Choose the Variation for the User
        var variation = cxApi.chooseVariation();

        window.onload = function () {
            // 3. Evaluate the result and update the image
            priceTable = document.getElementById('pnlPlanos');
            priceTable.classList.add(css_variations[variation]);
        }

        $(document).ready(function () {
            $('.painel_filtro').hide(); //Busca de Vagas e CVs
            $('.painel_icones').hide(); //Teste das Cores, SistMars, Média Salarial, etc
            $('.barra_rodape').hide();
        });
    </script>



    <!-- End of Google Analytics Content Experiment code -->
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/CIAPlanosNovo.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
<%--    <asp:UpdatePanel ID="updPnlConteudo" runat="server" RenderMode="Inline" UpdateMode="Conditional">
        <ContentTemplate>--%>
            <div id="main" class="escolha-de-plano" runat="server">
                <section id="selecionaTpPlanoCIA">

                    
                    <%-- Plano Cem --%>
                    <div runat="server" id="divPlanoCem">
                        <div class="nomePlanoCIA">
                            <asp:Literal ID="litNomePlanoCem" runat="server"></asp:Literal>
                            <div><asp:Literal ID="litPlanoCemQtde" runat="server"></asp:Literal></div>
                        </div>
                        <ul>
                            <li>
                                <asp:Literal ID="lblPlanoCemVisualizacao" runat="server"></asp:Literal> acessos completos aos currículos
                                <Componentes:BalaoSaibaMais ID="BalaoSaibaMais3" runat="server" ToolTipText="Este é o número de currículos com dados de contato que poderá acessar mensalmente."
                                    Text="Saiba mais" ToolTipTitle="" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True" />
                            </li>
                            <li>
                                <asp:Literal ID="lblPlanoCemSms" runat="server"></asp:Literal> envios de SMS para candidatos
                                <Componentes:BalaoSaibaMais ID="BalaoSaibaMais4" runat="server" ToolTipText="Você poderá enviar sms para os candidatos,  conforme a quantidade mensal descrita no plano selecionado."
                                    Text="Saiba mais" ToolTipTitle="" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True" />
                            </li>
                            <li>
                                <asp:Literal ID="lblPlanoCemEmail" runat="server"></asp:Literal> envios de email para candidatos
                                <Componentes:BalaoSaibaMais ID="BalaoSaibaMais5" runat="server" ToolTipText="Você poderá enviar e-mail para os candidatos, conforme a quantidade  mensal descrita no plano selecionado."
                                    Text="Saiba mais" ToolTipTitle="" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True"  />
                            </li>
                            <li>
                                Vagas ilimitadas com publicação imediata
                                <Componentes:BalaoSaibaMais ID="BalaoSaibaMais6" runat="server" ToolTipText="Anúncio ilimitado de vagas com gerenciador de candidatos inscritos. Para cada vaga cadastrada, será enviado 150 e-mails e 150 sms para candidatos, aumentando o número de inscritos."
                                    Text="Saiba mais" ToolTipTitle="" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True"/>
                            </li>
                            <li>
                                Acesso por 30 dias
                                <Componentes:BalaoSaibaMais ID="BalaoSaibaMais7" runat="server" ToolTipText="Sua equipe poderá utilizar os serviços contratados no período de 30 dias, sendo recarregado os limites mensalmente."
                                    Text="Saiba mais" ToolTipTitle="" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True"  />                                
                            </li>
                            <li>
                                Integração de vagas no Google, Sine e Facebook
                                <Componentes:BalaoSaibaMais ID="BalaoSaibaMais8" runat="server" ToolTipText="O sistema permite a integração de vagas com Sine e Google (conforme regras de indexação destes canais) e link para divulgação no Facebook pelo próprio recrutador."
                                    Text="Saiba mais" ToolTipTitle="" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True"  />
                                
                            </li>
                       </ul>                   
                       <div class="cancelaPlanoCIA">
                            Cancelamento simples, a qualquer momento
                            <Componentes:BalaoSaibaMais ID="tooltip_Cancelamento2" runat="server" ToolTipText="Você mesmo pode encerrar o serviço, selecionando o menu 'Meu Plano' e selecionando a opção encerrar"
                                    Text="Saiba mais" ToolTipTitle="" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True"  />
                        </div>
                        <asp:UpdatePanel ID="upPlanoCemPreco" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="descontoPlanoCIA">
                                    <div>
                                        <asp:Literal ID="litPlanoCemDe" runat="server" Visible="true"></asp:Literal>
                                    </div>
                                    <div>por apenas</div>
                                </div>
                                <div class="valorPlanoCIA">
                                    <div>
                                        <asp:Literal ID="litPlanoCemPor" runat="server" Visible="true"></asp:Literal>
                                    </div>
                                    <div>por mês</div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <div class="actionPlanoCIA">
                            <div>
                                <asp:LinkButton ID="lnkPlanoCem" runat="server" OnClick="lnkPlanoCem_Click" OnClientClick="trackEvent('Funil CIA', 'Plano Escolhido', 'Plano 100'); return true;">Continuar</asp:LinkButton>
                            </div>
                            <div><asp:LinkButton runat="server" OnClick="VerContrato1006_Click">Termos de Uso</asp:LinkButton></div>
                        </div>
                    </div>
                     <%-- Plano Básico --%>
                    <div>
                        <div class="nomePlanoCIA">
                            <asp:Literal ID="litNomePlanoBasico" runat="server"></asp:Literal>
                            <div><asp:Literal ID="litPlanoBasicoQtde" runat="server"></asp:Literal></div>
                        </div>
                        <ul>
                            <li>
                                <asp:Literal ID="lblPlanoBasicoVisualizacao" runat="server"></asp:Literal> acessos completos aos currículos
                                <Componentes:BalaoSaibaMais ID="tooltip_InfoAcesso" runat="server" ToolTipText="Este é o número de currículos com dados de contato que poderá acessar mensalmente."
                                    Text="Saiba mais" ToolTipTitle="" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True" />
                            </li>
                            <li>
                                <asp:Literal ID="lblPlanoBasicoSms" runat="server"></asp:Literal> envios de SMS para candidatos
                                <Componentes:BalaoSaibaMais ID="tooltip_InfoSms" runat="server" ToolTipText="Você poderá enviar sms para os candidatos,  conforme a quantidade mensal descrita no plano selecionado."
                                    Text="Saiba mais" ToolTipTitle="" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True" />
                            </li>
                            <li>
                                <asp:Literal ID="lblPlanoBasicoEmail" runat="server"></asp:Literal> envios de email para candidatos
                                <Componentes:BalaoSaibaMais ID="tooltip_InfoEmail" runat="server" ToolTipText="Você poderá enviar e-mail para os candidatos, conforme a quantidade  mensal descrita no plano selecionado."
                                    Text="Saiba mais" ToolTipTitle="" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True" />
                            </li>
                            <li>
                                Vagas ilimitadas com publicação imediata
                                <Componentes:BalaoSaibaMais ID="tooltip_InfoVagas" runat="server" ToolTipText="Anúncio ilimitado de vagas com gerenciador de candidatos inscritos. Para cada vaga cadastrada, será enviado 150 e-mails e 150 sms para candidatos, aumentando o número de inscritos."
                                    Text="Saiba mais" ToolTipTitle="" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True" />                                
                            </li>
                            <li>
                                Acesso por 30 dias
                                <Componentes:BalaoSaibaMais ID="BalaoSaibaMais1" runat="server" ToolTipText="Sua equipe poderá utilizar os serviços contratados no período de 30 dias, sendo recarregado os limites mensalmente."
                                    Text="Saiba mais" ToolTipTitle="" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True"  />
                                
                            </li>
                            <li>
                                Integração de vagas no Google, Sine e Facebook
                                <Componentes:BalaoSaibaMais ID="BalaoSaibaMais2" runat="server" ToolTipText="O sistema permite a integração de vagas com Sine e Google (conforme regras de indexação destes canais) e link para divulgação no Facebook pelo próprio recrutador."
                                    Text="Saiba mais" ToolTipTitle="" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True"/>                               
                            </li>
                        </ul>

                        
                        <div class="cancelaPlanoCIA">
                            Cancelamento simples, a qualquer momento
                            <Componentes:BalaoSaibaMais ID="tooltip_Cancelamento" runat="server" ToolTipText="Você mesmo pode encerrar o serviço, selecionando o menu 'Meu Plano' e selecionando a opção encerrar"
                                    Text="Saiba mais" ToolTipTitle="" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True"  />
                        </div>
                        <asp:UpdatePanel ID="upPlanoBasicoPreco" runat="server" RenderMode="Inline" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="descontoPlanoCIA">
                                    <div>
                                        <asp:Literal ID="litPlanoBasicoDe" runat="server" Visible="true"></asp:Literal>
                                    </div>
                                    <div>por apenas</div>
                                </div>
                                <div class="valorPlanoCIA">
                                    <div>
                                        <asp:Literal ID="litPlanoBasicoPor" runat="server" Visible="true"></asp:Literal>
                                    </div>
                                    <div>por mês</div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <div class="actionPlanoCIA">
                            <div>
                                <asp:LinkButton ID="lkbPlanoBasico" runat="server" OnClick="lkbPlanoBasico_Click" OnClientClick="trackEvent('Funil CIA', 'Plano Escolhido', 'Plano Basico'); return true;">Continuar</asp:LinkButton>
                            </div>
                            <div><asp:LinkButton runat="server" OnClick="VerContratoBasico_Click">Termos de Uso</asp:LinkButton></div>
                        </div>
                    </div>
                    <%-- Plano Ilimitado --%>
                    <div class="PlanoIlimitado">
                        <div class="nomePlanoIlimitado">
                            Disponibilizamos outros planos  <br />
                            <strong>para você</strong><br />
                            Ligue 0800 41 2400
                        </div>
                        <div class="converseConsultores">
                            Converse com nossos consultores
                        </div>

                        <div>
                            <button id="btlLigarAgora" runat="server" type="button" class="iniciarChamada" data-toggle="modal" data-target="#nomeDaModal">
                                <div>
                                    Fique tranquilo<br />
                                    <strong>nós ligamos pra você!</strong>
                                </div>

                                <div>
                                    <div class="ligarAgora">
                                        <img src="img/phone-ligaragora.png">
                                    </div>
                                    <div>Ligar agora</div>
                                </div>
                            </button>
                        </div>
                    </div>

                </section>
                <asp:UpdatePanel ID="upCodigoDesconto" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <section id="CupomDesconto" >
                            <div class="col-xs-12 col-sm-12 col-md-offset-2 col-md-2" >Cupom de Desconto</div>
                            <div class="col-xs-12 col-sm-12 col-md-4">
                                <asp:TextBox ID="txtCodigoCredito" runat="server" MaxLength="200"
                                AutoPostBack="true" OnTextChanged="txtCodigoCredito_TextChanged" CssClass="cod_de"> </asp:TextBox>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-2">
                                <asp:Button ID="btnValidarCodigoCredito" runat="server" Text="ATIVAR" CssClass="cupomAplicado" OnClick="btnValidarCodigoCredito_Click" />
                            </div>
                        </section>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
<%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
    <asp:UpdatePanel ID="upPlanoCustomizado" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False" RenderMode="Inline">
        <ContentTemplate>
            <asp:Panel ID="pnlPlanoCustomizado" runat="server" Visible="false" CssClass="plano_custom" Style="margin-top: 50px; left: 34%;">
                <p class="plan_cinza">
                    <strong>Plano Customizado</strong>

                </p>
                <p class="op_planos2">
                    <span>Escolha o plano
                                <asp:DropDownList runat="server" ID="ddlPlanoCustomizado" CssClass="dropdown-plano" OnSelectedIndexChanged="ddlPlanoCustomizado_OnSelectedIndexChanged" AutoPostBack="True" />
                    </span>
                </p>
                <p class="op_planos3">
                    <span>Convocações via SMS e/ou e-mail</span>
                    <span>
                        <asp:TextBox runat="server" ID="txtPlanoCustomizadoSms" Columns="3" CssClass="textbox_padrao" />
                    </span>
                </p>
                <p class="op_planos4">
                    <span style="color: #999;">
                        <asp:CheckBox runat="server" ID="ckbAplicarDesconto" />
                        Vender abaixo do mínimo?
                    </span>
                    <span style="padding-bottom: 9px;">
                        <asp:TextBox ID="txtMotivoVendaAbaixoMinimo" runat="server" CssClass="textarea_padrao" Columns="2000" TextMode="MultiLine" Rows="5" placeholder="Justificativa"></asp:TextBox>
                    </span>
                </p>
                
               <p class="op_planos4">
                    <span>Parcelas</span>
                    <span>
                        <asp:DropDownList runat="server" ID="ddlQuantidadeParcelas" Width="59" CssClass="dropdown-plano" OnSelectedIndexChanged="ddlQuantidadeParcelas_OnSelectedIndexChanged" AutoPostBack="True"/>
                    </span>
                </p>

                <p class="op_planos5">
                    <span>Preço</span>
                    <span style="font-size: 18px; color: #999; line-height: 22px;">R$
                                <asp:TextBox runat="server" ID="txtPlanoCustomizadoPor" Columns="4" CssClass="textbox_padrao" />
                        <small>p/mês</small>
                        <br />
                    </span>
                </p>
                <p class="op_planos6">
                    <span>Prazo Boleto:</span>
                    <span>
                        <asp:TextBox ID="txtPlanoCustomizadoPrazo" Columns="3" runat="server" CssClass="textbox_padrao"></asp:TextBox>
                    </span>
                </p>
                <p class="op-planos-set-button">
                    <asp:LinkButton ID="lkbPlanoCustomizado" runat="server" CssClass="fechar-venda" OnClick="lkbPlanoCustomizado_Click" Style="font-size: 12px;">Fechar Venda</asp:LinkButton>
                </p>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <Employer:DynamicScript runat="server" Src="/js/local/CIAPlanosNovo.js" type="text/javascript" />
    <asp:UpdatePanel ID="upL" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlL" runat="server" Visible="false">
                <uc:ModalEnvioEmail ID="ucModalEnvioEmail" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upModaisEmpresa" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:PlaceHolder ID="cphModaisEmpresa" runat="server"></asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>

    <uc:WebCallBack_Modais ID="ucWebCallBack_Modais" runat="server" />

</asp:Content>



