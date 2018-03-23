<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VisualizacaoCurriculo.aspx.cs"
    Inherits="BNE.Web.VisualizacaoCurriculo" %>

<%@ Register Src="~/UserControls/VisualizacaoCurriculo.ascx" TagName="VisualizacaoCurriculo"
    TagPrefix="UserControl" %>
<%@ Register Src="~/UserControls/ucAlertarBNE.ascx" TagName="ucAlertarBNE" TagPrefix="UserControl" %>
<%@ Register Src="~/UserControls/Modais/EnvioDeMensagem.ascx" TagName="EnvioDeMensagem"
    TagPrefix="UserControl" %>
<%@ Register Src="~/UserControls/Modais/ModalConfirmacao.ascx" TagName="ModalConfirmacao"
    TagPrefix="UserControl" %>
<%@ Register Src="~/UserControls/ucAvaliacaoCurriculo.ascx" TagName="ucAvaliacaoCurriculo"
    TagPrefix="UserControl" %>
<%@ Register Src="~/UserControls/Modais/ucAssociarCurriculoVaga.ascx" TagName="ucAssociarCurriculoVaga"
    TagPrefix="UserControl" %>
<%@ Register Src="~/UserControls/Modais/EmpresaAguardandoPublicacao.ascx" TagName="ucEmpresaAguardandoPublicacao"
    TagPrefix="UserControl" %>
<%@ Register Src="~/UserControls/Modais/ModalVendaChupaVIP.ascx" TagName="ModalVendaChupaVIP"
    TagPrefix="UserControl" %>
<%@ Register Src="~/UserControls/Modais/EmpresaBloqueada.ascx" TagPrefix="UserControl" TagName="EmpresaBloqueada" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <!-- Estilos -->
    <link href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" rel="stylesheet" />
    <link href="//maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css" rel="stylesheet" />
    <link href="//code.jquery.com/ui/1.9.1/themes/base/jquery-ui.css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/global/geral.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/global/topo.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/global/conteudo.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/global/componentes.css" type="text/css" rel="stylesheet" />
    <link href='https://fonts.googleapis.com/css?family=Roboto:400,300,500,700' rel='stylesheet' />
    <link rel="stylesheet" href="https://fonts.googleapis.com/icon?family=Material+Icons" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/VisualizacaoCurriculo.css" type="text/css" rel="stylesheet" />
    <%-- CSS Chame Facil--%>
    <link rel="stylesheet" href="//maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css" />


    <Employer:DynamicScript runat="server" Src="/js/jquery-1.8.3.js" type="text/javascript" />
    <script type="">
        function callbackPaging() {
            $('#btnAdjustPaging').trigger('click');
        }
    </script>

</head>
<body>

    <div class="conteudo" id="conteudo">
        <form id="form1" autocomplete="off" runat="server">
            <asp:ScriptManager runat="server" ID="scriptmanager43"></asp:ScriptManager>

            <%--  <telerik:RadScriptManager AsyncPostBackTimeout="900" ID="smGlobalVisualizacaoCV"
                OutputCompression="AutoDetect" runat="server" ScriptMode="Release">
            </telerik:RadScriptManager>--%>

            <asp:UpdateProgress ID="upgGlobalCarregandoInformacoes" runat="server" DisplayAfter="5">
                <ProgressTemplate>
                    <div class="progress_background" style="z-index: 10000;">
                        &nbsp;
                    </div>
                    <div class="progress_img_container" id="progress_img_container" style="z-index: 10001;">
                        <div class="img_container" id="img_container">
                            <i id="imgProgressTemplateAlternativo" runat="server" class="fa fa-spinner fa-spin fa-3x" style="margin-top: 10px;"></i>
                            <asp:Label ID="Label1" CssClass="carregando" runat="server" Text="carregando..."></asp:Label>
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel ID="upChameFacil" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Panel CssClass="painel_chame_facil divSlowDown" ID="pnlChameFacil" runat="server" Visible="False">
                        <blockquote><em><strong>CHAME FÁCIL</strong> SEUS CANDIDATOS:</em></blockquote>

                        <asp:UpdatePanel ID="upSaldoSMS" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <asp:Literal runat="server" ID="litSaldoSMS"></asp:Literal>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <div class="container-balao">
                            <h1>
                                <label class="text-info">LIVRE</label></h1>
                            <div class="balao fonte">
                                <span class="">
                                    <span class="details type"><strong>
                                        <asp:Literal ID="litNomeCandidatoLivre" runat="server"></asp:Literal></strong>
                                    </span>
                                    <span class="details type">
                                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtLivre" ValidationGroup="ChameFacilLivre"></asp:RequiredFieldValidator>
                                        <asp:TextBox MaxLength="130" runat="server" CssClass="textarea_balao3" ID="txtLivre" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                    </span>
                                    <Componentes:ContagemCaracteres runat="server" ControlToValidate="txtLivre" MaxLength="135" CssClassLabel="remaining type" />
                                </span>
                                <div class="arrow"></div>
                            </div>
                            <asp:LinkButton runat="server" Text="ENVIAR" CssClass="btn btn-warning pull-right" ID="btlEnviarLivre" OnClick="btlEnviarLivre_Click" ValidationGroup="ChameFacilLivre">
                                 <i class="fa fa-paper-plane fa-adjust"></i>  ENVIAR
                            </asp:LinkButton>
                        </div>
                        <div class="container-balao">
                            <h1>
                                <label class="text-info">CONVITE</label></h1>
                            <div class="balao fonte">
                                <span class="">
                                    <span class="details type"><strong>
                                        <asp:Literal ID="litNomeCandidatoConvite" runat="server"></asp:Literal></strong>
                                        <button class="btn btn-link pull-right" title="Edite esta mensagem" id="btConvite">
                                            <i class="fa fa-pencil-square-o"></i>
                                        </button>
                                    </span>
                                    <asp:Label ID="lblConvite" runat="server" CssClass="details type"></asp:Label>
                                    <div id="edicao-convite">
                                        <asp:TextBox runat="server" ID="txtConvite" TextMode="MultiLine" Rows="3" CssClass="textarea_padrao campo_obrigatorio"></asp:TextBox>
                                        <Componentes:ContagemCaracteres runat="server" ControlToValidate="txtConvite" MaxLength="135" CssClassLabel="remaining type" />
                                    </div>
                                </span>
                                <div class="arrow"></div>
                            </div>
                            <asp:LinkButton runat="server" Text="ENVIAR" CssClass="btn btn-warning pull-right" ID="btlEnviarConvite" OnClick="btlEnviarConvite_Click" CausesValidation="False">
                               <i class="fa fa-paper-plane fa-adjust"></i>  ENVIAR
                            </asp:LinkButton>
                        </div>
                        <div class="container-balao">
                            <h1>
                                <label class="text-info">CONVOCAÇÃO</label></h1>
                            <div class="balao fonte">
                                <span class="">
                                    <span class="details type"><strong>
                                        <asp:Literal ID="litNomeCandidatoConvocacao" runat="server"></asp:Literal></strong>
                                        <button class="btn btn-link pull-right" title="Edite esta mensagem" id="btConvocacao">
                                            <i class="fa fa-pencil-square-o"></i>
                                        </button>
                                    </span>
                                    <asp:Label ID="lblConvocacao" runat="server" CssClass="details type"></asp:Label>
                                    <div id="edicao-convocacao">
                                        <asp:TextBox runat="server" ID="txtConvocacao" TextMode="MultiLine" Rows="3" CssClass="textarea_padrao campo_obrigatorio"></asp:TextBox>
                                        <Componentes:ContagemCaracteres runat="server" ControlToValidate="txtConvocacao" MaxLength="135" CssClassLabel="remaining type" />
                                    </div>
                                </span>
                                <div class="arrow"></div>
                            </div>
                            <asp:LinkButton runat="server" Text="ENVIAR" CssClass="btn btn-warning pull-right" ID="btlEnviarConvocacao" OnClick="btlEnviarConvocacao_Click" CausesValidation="False">
                                <i class="fa fa-paper-plane fa-adjust"></i> ENVIAR
                            </asp:LinkButton>
                        </div>
                        <asp:Panel runat="server" ID="pnlUltimaMensagem" Visible="False" CssClass="container-balao">
                            <h1>
                                <label class="text-info">ÚLTIMA</label>
                                <label class="time-info">
                                    em
                                    <asp:Literal runat="server" ID="litUltimaMensagem"></asp:Literal></label></h1>
                            <div class="balao fonte">
                                <span class="">
                                    <span class="details type"><strong>
                                        <button class="btn btn-link pull-right" title="Edite esta mensagem" id="btUltima">
                                            <i class="fa fa-pencil-square-o"></i>
                                        </button></span>
                                    <asp:Label ID="lblUltima" runat="server" CssClass="details type"></asp:Label>
                                    <div id="edicao-ultima">
                                        <asp:TextBox runat="server" ID="txtUltima" TextMode="MultiLine" Rows="3" CssClass="textarea_padrao campo_obrigatorio"></asp:TextBox>
                                        <Componentes:ContagemCaracteres runat="server" ControlToValidate="txtUltima" MaxLength="135" CssClassLabel="remaining type" />
                                    </div>
                                </span>
                                <div class="arrow"></div>
                            </div>
                            <asp:LinkButton runat="server" Text="ENVIAR" CssClass="btn btn-warning pull-right" ID="btlUltima" OnClick="btlUltima_Click" CausesValidation="False">
                                <i class="fa fa-paper-plane fa-adjust"></i> ENVIAR
                            </asp:LinkButton>
                        </asp:Panel>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="container_curriculo">
                <div class="teste">
                    <asp:LinkButton runat="server" ID="btlPrevious" CssClass="fa fa-arrow-left navegacao left" OnClick="btlPrevious_OnClick" CausesValidation="False" ToolTip="Currículo Anterior"></asp:LinkButton>
                    <UserControl:VisualizacaoCurriculo ID="ucVisualizacaoCurriculo" runat="server" />
                    <asp:UpdatePanel ID="upNext" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:Button ID="btnAdjustPaging" runat="server" Style="display: none;" OnClick="btnAdjustPaging_OnClick" CausesValidation="False" />
                            <asp:LinkButton runat="server" ID="btlNext" CssClass="fa fa-arrow-right navegacao right" OnClick="btlNext_OnClick" CausesValidation="False" ToolTip="Próximo Currículo"></asp:LinkButton>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <UserControl:ucAlertarBNE ID="ucAlertarBNE" runat="server" />
            <div style="width: 700px; margin-left: 200px;">
                <%-- Painel de avaliação --%>
                <asp:UpdatePanel ID="upAvaliacao" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="painel_avaliacao_curriculo">
                            <UserControl:ucAvaliacaoCurriculo ID="ucAvaliacaoCurriculo" runat="server" Visible="False" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%-- FIM: Painel de avaliação --%>
                <%-- Painel de botões --%>
                <div id="divBotoes" visible="true" runat="server" class="painel_botoes" style="height: 28px;">
                    <asp:UpdatePanel ID="upBotoes" runat="server" UpdateMode="Conditional" style="float: left; line-height: 47px;">
                        <ContentTemplate>
                            <asp:Button ID="btnImprimirVisualizacaoCurriculo" runat="server" Visible="false"
                                CssClass="botao_padrao" CausesValidation="false" OnClientClick="window.print(); return false;"
                                PostBackUrl="javascript:;" Text="Imprimir" />
                            <asp:Button ID="btnAlertar" runat="server" CssClass="botao_padrao" CausesValidation="false"
                                OnClick="btnAlertar_Click" />
                            <asp:Button ID="btnFechar" runat="server" CssClass="botao_padrao" CausesValidation="false"
                                OnClientClick="window.close();" Text="Fechar" />
                            <asp:Button ID="btnSolicitar" runat="server" CssClass="botao_padrao" CausesValidation="false"
                                OnClick="btnSolicitar_Click" Text="Solicitar Atualização"></asp:Button>
                            <asp:Button ID="btnEnviarMensagemVisualizacao" runat="server" CausesValidation="false"
                                CssClass="botao_padrao" Text="Enviar Mensagem" OnClick="btnEnviarMensagemVisualizacao_Click" />
                            <asp:Button ID="btnAssociar" runat="server" CssClass="botao_padrao" CausesValidation="false"
                                OnClick="btnAssociar_Click" Text="Associar à uma Vaga" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <%-- FIM: Painel de botões --%>
            </div>
            <UserControl:EnvioDeMensagem ID="ucEnvioMensagem" runat="server" />
            <UserControl:ucAssociarCurriculoVaga ID="ucAssociarCurriculoVaga" runat="server" />
            <UserControl:ModalConfirmacao ID="ucModalConfirmacao" runat="server" />
            <UserControl:ucEmpresaAguardandoPublicacao ID="ucEmpresaAguardandoPublicacao" runat="server" />
            <UserControl:EmpresaBloqueada runat="server" ID="ucEmpresaBloqueada" />
            <UserControl:ModalVendaChupaVIP ID="ModalVendaChupaVIP" runat="server" />
            <asp:Panel ID="pnlAviso" runat="server" class="painel_avisos" Style="display: none">
                <asp:UpdatePanel ID="updAviso" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div style="position: relative">
                            <span id="icoavisos_img_container" class="icoavisos_img_container">
                                <i class="fa fa-exclamation-triangle fa-3x"></i>
                            </span>
                            <asp:Label ID="lblAviso" runat="server" Text=""></asp:Label>
                            <div class="botao_fechar_aviso" onclick="OcultarAviso();">
                                <div style="margin-top: 22px;">Fechar <i class="fa fa-times-circle"></i></div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </form>
    </div>
</body>

<script type="text/javascript" src="https://use.fontawesome.com/731d28b804.js"></script>
<script type="text/javascript" src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
<Employer:DynamicScript runat="server" Src="/js/employerFramework.combined.js" type="text/javascript" />
<Employer:DynamicScript runat="server" Src="/js/global/geral.js" type="text/javascript" />
<Employer:DynamicScript runat="server" Src="/js/global/topo.js" type="text/javascript" />
<Employer:DynamicScript runat="server" Src="/js/Controle.js" type="text/javascript" />
<Employer:DynamicScript runat="server" Src="/js/local/chameFacil/ChameFacil.js" type="text/javascript" />
</html>
