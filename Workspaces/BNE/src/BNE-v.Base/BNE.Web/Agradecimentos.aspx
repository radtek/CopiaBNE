<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="Agradecimentos.aspx.cs" Inherits="BNE.Web.Agradecimentos" %>

<%@ Register Src="UserControls/Modais/ModalConfirmacao.ascx" TagName="ModalConfirmacao"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/Agradecimentos.css" type="text/css" rel="stylesheet" />
    <!--[if lte IE 7]>
            <style type="text/css">
                .painel_agradecimento *
                {
                    float: left;
                }
               
            </style>
        <![endif]-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:UpdatePanel ID="upPainelAgradecimentos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <h1>
                Agradecimentos</h1>
            <div id="pnlAgradecimentos" class="painel_agradecimentos">
                <div class="botao_esquerda">
                    <asp:Image ID="imgEsquerda" ImageUrl="/img/agradecimentos/icon_seta_esq.png" runat="server" /></div>
                <div class="botao_direita">
                    <asp:Image ID="imgDireita" runat="server" ImageUrl="img/agradecimentos/icon_seta_dir.png" /></div>
                <div id="pnlAgradecimentosContainer" class="painel_com_agradecimentos">
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel runat="server" ID="AgradecimentoFormulario" CssClass="painel_agradecimento_form">
        <asp:UpdatePanel ID="upAgradecimentos" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:UpdatePanel ID="upEnviar" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div id="divEnviarAgradecimentos" class="painel_agradecimento">
                            <asp:Panel ID="pnlEnviarAgradecimentos" runat="server">
                                <h2>
                                    Enviar Agradecimento</h2>
                                <div class="linha">
                                    <asp:Label ID="Label1" CssClass="label_principal" runat="server" AssociatedControlID="txtNome"
                                        Text="Nome"></asp:Label>
                                    <div class="container_campo">
                                        <div>
                                            <asp:RequiredFieldValidator ID="rfNome" runat="server" ValidationGroup="EnviarAgradecimento"
                                                ControlToValidate="txtNome"></asp:RequiredFieldValidator>
                                        </div>
                                        <asp:TextBox ID="txtNome" CssClass="textbox_padrao campo_obrigatorio" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="linha">
                                    <asp:Label ID="Label5" CssClass="label_principal" runat="server" Text="E-mail" AssociatedControlID="txtEmail"></asp:Label>
                                    <div class="container_campo">
                                        <div>
                                            <asp:RequiredFieldValidator ID="rfEmail" runat="server" ValidationGroup="EnviarAgradecimento"
                                                ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ValidationGroup="EnviarAgradecimento"
                                                ControlToValidate="txtEmail" ErrorMessage="Email Inválido"></asp:RegularExpressionValidator>
                                        </div>
                                        <asp:TextBox ID="txtEmail" CssClass="textbox_padrao campo_obrigatorio" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="linha">
                                    <asp:Label ID="Label6" CssClass="label_principal" runat="server" Text="Cidade" AssociatedControlID="txtCidadeAgradecimento"></asp:Label>
                                    <div class="container_campo">
                                        <div>
                                            <asp:RequiredFieldValidator ID="rfCidadeAgradecimento" runat="server" ValidationGroup="EnviarAgradecimento"
                                                ControlToValidate="txtCidadeAgradecimento"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvCidade" runat="server" ErrorMessage="Cidade Inexistente."
                                                ClientValidationFunction="cvCidade_Validate" ControlToValidate="txtCidadeAgradecimento"
                                                ValidationGroup="EnviarAgradecimento"></asp:CustomValidator>
                                        </div>
                                        <asp:TextBox ID="txtCidadeAgradecimento" CssClass="textbox_padrao campo_obrigatorio"
                                            runat="server"></asp:TextBox>
                                        <AjaxToolkit:AutoCompleteExtender ID="aceCidadeAgradecimento" runat="server" TargetControlID="txtCidadeAgradecimento"
                                            UseContextKey="True" OnClientItemSelected="CidadeSelecionadaMasterPage" ServiceMethod="ListarCidadesSiglaEstadoPorNomeParcialEstado"
                                            FirstRowSelected="True">
                                        </AjaxToolkit:AutoCompleteExtender>
                                    </div>
                                </div>
                                <div class="linha">
                                    <asp:Label ID="Label7" CssClass="label_principal" runat="server" Text="Mensagem"
                                        AssociatedControlID="txtMensagem"></asp:Label>
                                    <div class="container_campo">
                                        <div>
                                            <asp:RequiredFieldValidator ID="rfvMensagem" runat="server" ValidationGroup="EnviarAgradecimento"
                                                ControlToValidate="txtMensagem"></asp:RequiredFieldValidator>
                                        </div>
                                        <asp:TextBox ID="txtMensagem" TextMode="MultiLine" Rows="7" runat="server" CssClass="campo_mensagem textbox_padrao campo_obrigatorio"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="painel_botoes">
                                    <asp:Button ID="btnEnviar" runat="server" CssClass="botao_padrao" Text="Enviar" ValidationGroup="EnviarAgradecimento"
                                        OnClick="btnEnviar_Click" />
                                </div>
                            </asp:Panel>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div id="pnlVisualizarAgradecimento" class="painel_agradecimento" style="display: none">
                </div>
                <div class="imagens_direita">
                    <div class="botao_meu_agradecimento">
                        <div class="btn_sombra">
                            <asp:ImageButton ID="btiVip" ImageUrl="img/agradecimentos/btn_vip.png" runat="server"
                                OnClick="btiVip_Click" />
                        </div>
                        <div class="btn_sombra">
                            <asp:UpdatePanel ID="upMeuAgradecimento" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <asp:ImageButton ID="btiMeuAgradecimento" OnClientClick="javascript:MeuAgradecimento();"
                                        ImageUrl="img/agradecimentos/btn_meu_agradecimento.png" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel runat="server" CssClass="painel_botoes" ID="pnlBotoes">
        <asp:Button runat="server" ID="btnVoltar" Text="Voltar" CausesValidation="false"
            CssClass="botao_padrao" OnClick="btnVoltar_Click" />
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <uc1:ModalConfirmacao ID="ucModalConfirmacao" runat="server" />
    <Employer:DynamicScript runat="server" Src="/js/local/Agradecimento.js" type="text/javascript" />
    <Employer:DynamicScript runat="server" Src="/js/jquery.scrollTo.js" type="text/javascript" />
</asp:Content>
