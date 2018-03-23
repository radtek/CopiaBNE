<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="CIAPlanosNovo.aspx.cs" Inherits="BNE.Web.CIAPlanosNovo" %>

<%@ Register Src="UserControls/Modais/EnvioEmail.ascx" TagName="ModalEnvioEmail" TagPrefix="uc" %>
<%@ Register Src="UserControls/Modais/ucWebCallBack.ascx" TagName="WebCallBack" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <link href="css/local/Bootstrap/bootstrap.css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/CIAPlanosNovo.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div id="prices">
        <h1>Planos Disponíveis</h1>
        <div class="plan-table">
            <asp:UpdatePanel ID="upPrecos" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <span>Benefícios</span>
                    <span>Gratuito <strong>R$ 0</strong></span>
                    <span>
                        <asp:Literal ID="litNomePlanoBasico" runat="server"></asp:Literal>
                        <strong>R$
                            <asp:Literal ID="lblPlanoBasicoPor" runat="server" Visible="true"></asp:Literal>
                            <asp:TextBox runat="server" ID="txtPlanoBasicoPor" Columns="4" CssClass="textbox_padrao" Visible="false" />
                        </strong><small>
                            <asp:Literal ID="litDescricaoParcelaBasico" runat="server"></asp:Literal></small>
                    </span>
                    <span>
                        <asp:Literal ID="litNomePlanoIndicado" runat="server"></asp:Literal>
                        <strong>R$
                            <asp:Literal ID="lblPlanoIndicadoPor" runat="server" Visible="true"></asp:Literal>
                            <asp:TextBox runat="server" ID="txtPlanoIndicadoPor" Columns="4" CssClass="textbox_padrao" Visible="false" />
                        </strong><small>
                            <asp:Literal ID="litDescricaoParcelaIndicado" runat="server"></asp:Literal></small>
                    </span>
                    <span>
                        <asp:Literal ID="litNomePlanoPlus" runat="server"></asp:Literal>
                        <strong>R$ 
                            <asp:Literal ID="lblPlanoPlusPor" runat="server" Visible="true"></asp:Literal>
                            <asp:TextBox runat="server" ID="txtPlanoPlusPor" Columns="4" CssClass="textbox_padrao" Visible="false" />
                        </strong><small>
                            <asp:Literal ID="litDescricaoParcelaPlus" runat="server"></asp:Literal></small>
                    </span>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div>
                <span>Período de acesso</span>
                <span>Ilimitado</span>
                <span>
                    <asp:Literal ID="litPeriodoBasico" runat="server"></asp:Literal>
                </span>
                <span>
                    <asp:Literal ID="litPeriodoIndicado" runat="server"></asp:Literal></span>
                <span>
                    <asp:Literal ID="litPeriodoPlus" runat="server"></asp:Literal></span>
            </div>
            <div><span>Anúncio de vagas ilimitado</span> <span><i class="fa fa-check"></i></span><span><i class="fa fa-check"></i></span><span><i class="fa fa-check"></i></span><span><i class="fa fa-check"></i></span></div>
            <div><span>Vagas no jornal on-line de vagas (semanal)</span> <span><i class="fa fa-check"></i></span><span><i class="fa fa-check"></i></span><span><i class="fa fa-check"></i></span><span><i class="fa fa-check"></i></span></div>
            <div><span>Envio de e-mail para convocações por mês</span> <span><i class="fa fa-times"></i></span><span>30</span> <span>Ilimitado</span> <span>Ilimitado</span> </div>
            <asp:UpdatePanel ID="upSMS" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <span>Envio de SMS para convocações por mês</span>
                    <span><i class="fa fa-times"></i></span>
                    <span>
                        <asp:Literal ID="lblPlanoBasicoSms" runat="server"></asp:Literal>
                        <asp:TextBox runat="server" ID="txtPlanoBasicoSms" Columns="3" CssClass="textbox_padrao" Visible="false" />
                    </span>
                    <span>
                        <asp:Literal ID="lblPlanoIndicadoSms" runat="server"></asp:Literal>
                        <asp:TextBox runat="server" ID="txtPlanoIndicadoSms" Columns="3" CssClass="textbox_padrao" Visible="false" />
                    </span>
                    <span>
                        <asp:Literal ID="lblPlanoPlusSms" runat="server"></asp:Literal>
                        <asp:TextBox runat="server" ID="txtPlanoPlusSms" Columns="3" CssClass="textbox_padrao" Visible="false" />
                    </span>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div><span>Visualização ilimitada de currículos</span> <span><i class="fa fa-times"></i></span><span><i class="fa fa-check"></i></span><span><i class="fa fa-check"></i></span><span><i class="fa fa-check"></i></span></div>
            <div><span>Aviso imediato de vagas para candidatos</span> <span><i class="fa fa-times"></i></span><span><i class="fa fa-check"></i></span><span><i class="fa fa-check"></i></span><span><i class="fa fa-check"></i></span></div>
            <div><span>Filtro avançado de busca de currículos</span> <span><i class="fa fa-times"></i></span><span><i class="fa fa-check"></i></span><span><i class="fa fa-check"></i></span><span><i class="fa fa-check"></i></span></div>
            <div><span>Integração automática de vagas</span> <span><i class="fa fa-times"></i></span><span><i class="fa fa-times"></i></span><span><i class="fa fa-check"></i></span><span><i class="fa fa-check"></i></span></div>
            <div><span>Campanha de marketing de vagas</span> <span><i class="fa fa-times"></i></span><span><i class="fa fa-times"></i></span><span><i class="fa fa-times"></i></span><span><i class="fa fa-check"></i></span></div>
            <asp:UpdatePanel ID="upPrazo" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel ID="pnlPrazo" runat="server" Visible="false">
                        <span style="font-size: 13px; line-height: 23px;">Prazo Boleto:</span>
                        <span><i class="fa fa-times"></i></span>
                        <span>
                            <asp:TextBox ID="txtPlanoBasicoPrazo" Columns="3" runat="server" />
                        </span>
                        <span>
                            <asp:TextBox ID="txtPlanoIndicadoPrazo" Columns="3" runat="server"></asp:TextBox>
                        </span>
                        <span>
                            <asp:TextBox ID="txtPlanoPlusPrazo" Columns="3" runat="server"></asp:TextBox>
                        </span>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div>
                <span>Vagas no jornal Azulzinho
                <br />
                    (impresso e digital)</span> <span><i class="fa fa-times"></i></span><span><i class="fa fa-times"></i></span><span><i class="fa fa-times"></i></span><span><i class="fa fa-check"></i></span>
            </div>
            <div>
                <span>
                    <asp:CheckBox ID="cbContrato" runat="server" Checked="true" />
                    Li e aceito os termos e condições do CONTRATO</span>
                <span><i class="fa fa-times"></i></span>
                <span>
                    <asp:LinkButton ID="lnkContratoBasico" runat="server" OnClick="btnContratoBasico_Click" Target="_New" CssClass="ver_contrato">Ver contrato</asp:LinkButton></span>
                <span>
                    <asp:LinkButton ID="lnkContratoIndicado" runat="server" OnClick="btnContratoIndicado_Click" Target="_New" CssClass="ver_contrato">Ver contrato</asp:LinkButton></span>
                <span>
                    <asp:LinkButton ID="lnkContratoPlus" runat="server" OnClick="btnContratoPlus_Click" Target="_New" CssClass="ver_contrato">Ver contrato</asp:LinkButton></span>
            </div>
            <div id="f" class="clearfix block-bts">
                <div class="container-prices">
                    <span></span>
                    <span></span>
                    <span>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" class="code">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbPlanoBasico" runat="server" CssClass="btn gray" OnClick="lkbPlanoBasico_Click">Eu Quero</asp:LinkButton>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </span>
                    <span>
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" class="code">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbPlanoIndicado" runat="server" CssClass="btn green" OnClick="lkbPlanoIndicado_Click">Eu Quero</asp:LinkButton>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </span>
                    <span>
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional" class="code">
                            <ContentTemplate>
                                <asp:LinkButton ID="lkbPlanoPlus" runat="server" CssClass="btn gray" OnClick="lkbPlanoPlus_Click">Eu Quero</asp:LinkButton>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </span>
                </div>
            </div>
            <%-- Plano customizado --%>
        </div>
        <h2>Deseja solicitar o orçamento personalizado? Entre em contato: <strong>0800 41 2400</strong></h2>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" class="code">
            <ContentTemplate>
                <label>Eu tenho código de desconto:</label>
                <asp:TextBox ID="txtCodigoCredito" runat="server" MaxLength="200" Style="background-image: none;"
                    AutoPostBack="true" OnTextChanged="txtCodigoCredito_TextChanged"> </asp:TextBox>
                <asp:Button ID="btnValidarCodigoCredito" runat="server" Text="Ativar" OnClick="btnValidarCodigoCredito_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="upPlanoCustomizado" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
        <ContentTemplate>
            <asp:Panel ID="pnlPlanoCustomizado" runat="server" Visible="false" CssClass="plan-table" Style="margin-top: 50px;">
                <div>
                    <span style="font-size: 25px; line-height: 30px;">Plano Customizado</span>
                    <span>Escolha o plano
                                <asp:DropDownList runat="server" ID="ddlPlanoCustomizado" CssClass="dropdown-plano" OnSelectedIndexChanged="ddlPlanoCustomizado_OnSelectedIndexChanged" AutoPostBack="True" />
                    </span>
                </div>
                <div>
                    <span>Convocações via SMS e/ou e-mail</span>
                    <span>
                        <asp:TextBox runat="server" ID="txtPlanoCustomizadoSms" Columns="3" CssClass="textbox_padrao" />
                    </span>
                </div>
                <div>
                    <span>
                        <label style="color: #999;">
                            <asp:CheckBox runat="server" ID="ckbAplicarDesconto" />
                            Vender abaixo do mínimo?
                        </label>
                    </span>
                    <span style="padding-bottom: 9px;">
                        <asp:TextBox ID="txtMotivoVendaAbaixoMinimo" runat="server" CssClass="textarea_padrao" Columns="2000" TextMode="MultiLine" Rows="5" placeholder="Justificativa"></asp:TextBox>
                    </span>
                </div>
                <div>
                    <span>Preço</span>
                    <span style="font-size: 18px; color: #999; line-height: 22px;">R$
                                <asp:TextBox runat="server" ID="txtPlanoCustomizadoPor" Columns="4" CssClass="textbox_padrao" />
                        <small>p/mês</small>
                        <br />
                    </span>
                </div>
                <div>
                    <span>Prazo Boleto:</span>
                    <span>
                        <asp:TextBox ID="txtPlanoCustomizadoPrazo" Columns="3" runat="server" CssClass="textbox_padrao"></asp:TextBox>
                    </span>
                </div>
                <div class="clearfix block-bts">
                    <span></span>
                    <span>
                        <asp:LinkButton ID="lkbPlanoCustomizado" runat="server" CssClass="btn green" OnClick="lkbPlanoCustomizado_Click" Style="font-size: 12px;">Fechar Venda</asp:LinkButton>
                    </span>
                </div>
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
    <asp:UpdatePanel ID="upWebCallBack" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlWebCallBack" runat="server" Visible="true">
                <uc:WebCallBack ID="ucWebCallBack" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
