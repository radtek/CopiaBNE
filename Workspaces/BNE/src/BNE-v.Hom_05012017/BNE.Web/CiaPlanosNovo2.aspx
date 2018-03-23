<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="CiaPlanosNovo2.aspx.cs" Inherits="BNE.Web.CiaPlanosNovo2" %>

<%@ Register Src="UserControls/Modais/EnvioEmail.ascx" TagName="ModalEnvioEmail" TagPrefix="uc" %>
<%@ Register Src="UserControls/Modais/ucWebCallBack.ascx" TagName="WebCallBack" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/CIAPlanosNovo.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/CIAPlanosNovo2.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:Panel ID="pnlPlanos" runat="server" CssClass="table_planos">
        <div class="dados_planos">
            <p class="planos_tit">Planos</p>
            <p class="text_valor">ESCOLHA O PLANO CERTO PARA SUA EMPRESA</p>
            <p class="op_planos planos_acesso"><i class="fa fa-newspaper-o"></i>Acessos completo ao currículo *</p>
            <p class="op_planos planos_sms"><i class="fa fa-mobile"></i>Envio de SMS para candidatos *</p>
            <p class="op_planos planos_email"><i class="fa fa-envelope"></i>Envio de e-mail para candidatos *</p>
            <p class="op_planos planos_vagas"><i class="fa fa-bullhorn"></i>Divulgação de vagas</p>
            <p class="op_planos planos_alerta">
                <i class="fa fa-check-square-o"></i>Alerta imediato de suas vagas<br>
                para os candidatos
            </p>
            <p class="op_planos planos_contrato"><i class="fa fa-suitcase"></i>Termo e Condições</p>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upPnlPrazoBoleto" RenderMode="Inline">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="pnlPrazoBoleto" Visible="False">
                        <p class="op_planos planos_boleto"><i class="fa fa-calendar"></i>Prazo do Boleto</p>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="plano_10">
            <p class="plan_cinza">
                <strong>
                    <asp:Literal ID="litNomePlanoBasico" runat="server"></asp:Literal></strong>
            </p>
            <p class="precos_de">
                de R$ <span class="cents"><span class="linha_pre"></span><span class="linha_pre2"></span><strong class="p_m">
                    <asp:Literal ID="litPlanoBasicoDe" runat="server"></asp:Literal>
                </strong></span><span class="cents2">,00</span>
            </p>
            <p class="precos_de2 desconto">
                por R$ 
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upPlanoBasicoPor" RenderMode="Inline">
                    <ContentTemplate>
                        <span class="cents">
                            <strong class="p_m2">
                                <asp:Literal ID="lblPlanoBasicoPor" runat="server" Visible="true"></asp:Literal>
                                <asp:TextBox runat="server" ID="txtPlanoBasicoPor" Columns="4" CssClass="textbox_padrao" Visible="false" /></strong>
                        </span><span class="cents3">,<asp:Literal ID="litPlanoBasicoPorCentavos" runat="server"></asp:Literal></span>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </p>
            <p class="op_planos2">
                <strong>
                    <asp:Literal ID="litPlanoBasicoVisualizacao" runat="server"></asp:Literal></strong>
            </p>
            <p class="op_planos3">
                <strong>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upPlanoBasicoSMS" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:Literal ID="lblPlanoBasicoSms" runat="server"></asp:Literal>
                            <asp:TextBox runat="server" ID="txtPlanoBasicoSms" Columns="3" CssClass="textbox_padrao" Visible="false" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </strong>
            </p>
            <p class="op_planos4">
                <strong>
                    <asp:Literal ID="litPlanoBasicoEmail" runat="server"></asp:Literal>
                </strong>
            </p>
            <p class="op_planos5"><strong>Ilimitado</strong></p>
            <p class="op_planos6"><strong>Ilimitado</strong></p>
            <p class="op_planos7">
                <asp:LinkButton ID="lnkContratoBasico" runat="server" OnClick="btnContratoBasico_Click" Target="_New" CssClass="ver_contrato"><strong>Ver contrato</strong></asp:LinkButton>
            </p>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upPnlPlanoBasicoPrazo" RenderMode="Inline">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="pnlPlanoBasicoPrazo" Visible="False">
                        <p class="op_planos8">
                            <asp:TextBox ID="txtPlanoBasicoPrazo" Columns="3" runat="server" />
                        </p>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
        <div class="plano_20">
            <div class="estrelas_pag">
                <i class="fa fa-star"></i>
                <i class="fa fa-star"></i>
                <i class="fa fa-star"></i>
            </div>
            <div id="twelve-point-star"><span class="desc_t">Desconto <strong class="t_maior">40%</strong></span></div>
            <p class="plan_azul">
                <strong>
                    <asp:Literal ID="litNomePlanoIndicado" runat="server"></asp:Literal></strong>
            </p>
            <p class="precos_de">
                de R$ <span class="cents"><span class="linha_pre"></span><span class="linha_pre2"></span><strong class="p_m">
                    <asp:Literal ID="litPlanoIndicadoDe" runat="server"></asp:Literal>
                </strong></span><span class="cents2">,00</span>
            </p>
            <p class="precos_de2 desconto">
                por R$ 
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upPlanoIndicadoPor" RenderMode="Inline">
                    <ContentTemplate>
                        <span class="cents"><strong class="p_m2">
                            <asp:Literal ID="lblPlanoIndicadoPor" runat="server" Visible="true"></asp:Literal>
                            <asp:TextBox runat="server" ID="txtPlanoIndicadoPor" Columns="4" CssClass="textbox_padrao" Visible="false" />
                        </strong></span><span class="cents3">,<asp:Literal ID="litPlanoIndicadoPorCentavos" runat="server"></asp:Literal></span>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </p>
            <p class="op_planos2">
                <strong>
                    <asp:Literal ID="litPlanoIndicadoVisualizacao" runat="server"></asp:Literal></strong>
            </p>
            <p class="op_planos3">
                <strong>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upPlanoIndicadoSMS" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:Literal ID="lblPlanoIndicadoSms" runat="server"></asp:Literal>
                            <asp:TextBox runat="server" ID="txtPlanoIndicadoSms" Columns="3" CssClass="textbox_padrao" Visible="false" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </strong>
            </p>
            <p class="op_planos4">
                <strong>
                    <asp:Literal ID="litPlanoIndicadoEmail" runat="server"></asp:Literal></strong>
            </p>
            <p class="op_planos5"><strong>Ilimitado</strong></p>
            <p class="op_planos6"><strong>Ilimitado</strong></p>
            <p class="op_planos7">
                <asp:LinkButton ID="lnkContratoIndicado" runat="server" OnClick="btnContratoIndicado_Click" Target="_New" CssClass="ver_contrato"><strong>Ver Contrato</strong></asp:LinkButton>
            </p>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upPnlPlanoIndicadoPrazo" RenderMode="Inline">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="pnlPlanoIndicadoPrazo" Visible="False">
                        <p class="op_planos8">
                            <asp:TextBox ID="txtPlanoIndicadoPrazo" Columns="3" runat="server"></asp:TextBox>
                        </p>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
        <div class="plano_30">
            <p class="plan_cinza">
                <strong>
                    <asp:Literal ID="litNomePlanoPlus" runat="server"></asp:Literal></strong>
            </p>
            <p class="precos_de">
                de R$ <span class="cents"><span class="linha_pre"></span><span class="linha_pre2"></span><strong class="p_m">
                    <asp:Literal ID="litPlanoPlusDe" runat="server"></asp:Literal>
                </strong></span><span class="cents2">,00</span>
            </p>
            <p class="precos_de2 desconto">
                por R$ 
                <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upPlanoPlusPor" RenderMode="Inline">
                    <ContentTemplate>
                        <span class="cents"><strong class="p_m2">
                            <asp:Literal ID="lblPlanoPlusPor" runat="server" Visible="true"></asp:Literal>
                            <asp:TextBox runat="server" ID="txtPlanoPlusPor" Columns="4" CssClass="textbox_padrao" Visible="false" />
                        </strong></span><span class="cents3">,<asp:Literal ID="litPlanoPlusPorCentavos" runat="server"></asp:Literal></span>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </p>
            <p class="op_planos2">
                <strong>
                    <asp:Literal ID="litPlanoPlusVisualizacao" runat="server"></asp:Literal></strong>
            </p>
            <p class="op_planos3">
                <strong>
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upPlanoPlusSMS" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:Literal ID="lblPlanoPlusSms" runat="server"></asp:Literal>
                            <asp:TextBox runat="server" ID="txtPlanoPlusSms" Columns="3" CssClass="textbox_padrao" Visible="false" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </strong>
            </p>
            <p class="op_planos4">
                <strong>
                    <asp:Literal ID="litPlanoPlusEmail" runat="server"></asp:Literal></strong>
            </p>
            <p class="op_planos5"><strong>Ilimitado</strong></p>
            <p class="op_planos6"><strong>Ilimitado</strong></p>
            <p class="op_planos7">
                <asp:LinkButton ID="lnkContratoPlus" runat="server" OnClick="btnContratoPlus_Click" Target="_New" CssClass="ver_contrato"><strong>Ver Contrato</strong></asp:LinkButton>
            </p>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upPnlPlanoPlusPrazo" RenderMode="Inline">
                <ContentTemplate>
                    <asp:Panel runat="server" ID="pnlPlanoPlusPrazo" Visible="False">
                        <p class="op_planos8">
                            <asp:TextBox ID="txtPlanoPlusPrazo" Columns="3" runat="server"></asp:TextBox>
                        </p>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:LinkButton ID="lkbPlanoBasico" runat="server" CssClass="contratar_n" OnClick="lkbPlanoBasico_Click">CONTRATAR</asp:LinkButton>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:LinkButton ID="lkbPlanoIndicado" runat="server" CssClass="contratar_n2" OnClick="lkbPlanoIndicado_Click">CONTRATAR</asp:LinkButton>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:LinkButton ID="lkbPlanoPlus" runat="server" CssClass="contratar_n3" OnClick="lkbPlanoPlus_Click">CONTRATAR</asp:LinkButton>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:UpdatePanel ID="upPlanoCustomizado" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False" RenderMode="Inline">
        <ContentTemplate>
            <asp:Panel ID="pnlPlanoCustomizado" runat="server" Visible="false" CssClass="plano_custom" Style="margin-top: 50px;">
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

    <p class="text_inf"><strong>(*)</strong> Quantidades para utilização durante a vigência/período do plano contratado.</p>
    <div class="cup_desc">
        <p class="txt_cup">Insira seu código de desconto, no campo abaixo e clique em ativar</p>
        <div class="form_desc">
            <p class="txt_cup2">Eu tenho código de desconto</p>
            <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:TextBox ID="txtCodigoCredito" runat="server" MaxLength="200" Style="background-image: none;"
                        AutoPostBack="true" OnTextChanged="txtCodigoCredito_TextChanged" CssClass="cod_de"> </asp:TextBox>
                    <asp:Button ID="btnValidarCodigoCredito" runat="server" Text="ATIVAR" CssClass="ativar_cod" OnClick="btnValidarCodigoCredito_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

    <img class="banner_cia" src="/img/pacotes_cia/ciaPlanos/banner_tela_pagamento_cia.png">
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
    <asp:UpdatePanel ID="upWebCallBack" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlWebCallBack" runat="server" Visible="true">
                <uc:WebCallBack ID="ucWebCallBack" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
