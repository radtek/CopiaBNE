<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VerDadosEmpresa.ascx.cs" Inherits="BNE.Web.UserControls.Modais.VerDadosEmpresa" %>
<%@ Register Src="../UCLogoFilial.ascx" TagName="UCLogoFilial" TagPrefix="uc1" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/VerDadosEmpresa.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlVerDadosEmpresa" CssClass="modal_conteudo candidato reduzida dados_empresa " Style="display: none" runat="server">
    <asp:UpdatePanel ID="upVerDadosEmpresa" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <h2 class="titulo_modal">
                <asp:Label ID="lblTitulo" runat="server"></asp:Label> 
            </h2>
            <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
<asp:LinkButton CssClass="botao_fechar_modal" ID="btiFechar" runat="server" CausesValidation="false" OnClick="btiFechar_Click" ><i class="fa fa-times-circle"></i>
 Fechar</asp:LinkButton>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:Panel CssClass="coluna_esquerda" ID="pnlColunaEsquerda" runat="server">
                <asp:LinkButton ID="lbtInfoNaoVip" OnClick="lbtVendaVip_Click" runat="server">
                    <asp:Panel CssClass="painel_info_nao_vip" ID="pnlEsquerdaInfoNaoVip" runat="server">
                        <img alt="Eu quero!" class="icone_fechadura" src="/img/img_modal_empresa_fechadura.png" />
                        <p class="texto_tenha_acesso_livre">
                            Tenha acesso livre a todas as vagas e aos dados das empresas.</p>
                        <p class="texto_seja_vip">
                            Seja VIP!</p>
                        <p class="texto_por_apenas">
                            por apenas</p>
                        <p class="texto_valor">
                            <asp:Literal ID="litValorPlanoVip" runat="server"></asp:Literal></p>
                        <img alt="Eu quero!" class="botao_eu_quero" src="/img/btn_modal_empresa_eu_quero.png" />
                    </asp:Panel>
                </asp:LinkButton>
                <asp:Panel CssClass="painel_info_vip" ID="pnlEsquerdaInfoVip" runat="server">
                    <uc1:UCLogoFilial ID="imgLogo" CssClass="logo_empresa" runat="server" Visible="false" />
                </asp:Panel>
                <asp:Panel CssClass="painel_icone_confidencial" ID="pnlEsquerdaIconeConfidencial" runat="server">
                    <img alt="Confidencial" src="/img/img_modal_empresa_confidencial_icone.png" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="painel_dados_empresa" ID="pnlDadosEmpresaVisualizacao" runat="server">
                <div class="linha">
                    <asp:Label ID="lblNomeEmpresa" CssClass="label_principal" Text="Nome da Empresa:" runat="server" AssociatedControlID="lblNomeEmpresaValor"></asp:Label>
                    <div class="container_campo">
                        <asp:Literal ID="lblNomeEmpresaValor" runat="server"></asp:Literal>
                        <asp:LinkButton ID="lbtEmpresaBloqueada" OnClick="lbtVendaVip_Click" runat="server">Somente para Cliente <span class="texto_vip">VIP</span></asp:LinkButton>
                    </div>
                </div>
                <div class="linha">
                    <asp:Label ID="lblAtividadeEmpresa" CssClass="label_principal" Text="Atividade da Empresa:" runat="server" AssociatedControlID="lblAtividadeEmpresaValor"></asp:Label>
                    <div class="container_campo">
                        <asp:Literal ID="lblAtividadeEmpresaValor" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="linha">
                    <asp:Label ID="lblNumeroFuncionario" CssClass="label_principal" Text="Número de Funcionários:" runat="server" AssociatedControlID="lblNumeroFuncionarioValor"></asp:Label>
                    <div class="container_campo">
                        <asp:Literal ID="lblNumeroFuncionarioValor" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="linha">
                    <asp:Label ID="lblTelefone" CssClass="label_principal" Text="Telefone:" runat="server" AssociatedControlID="lblTelefoneValor"></asp:Label>
                    <div class="container_campo">
                        <asp:Label ID="lblTelefoneValor" runat="server"></asp:Label>
                        <asp:LinkButton ID="lbtTelefoneBloqueado" OnClick="lbtVendaVip_Click" runat="server">Somente para Cliente <span class="texto_vip">VIP</span></asp:LinkButton>
                    </div>
                </div>
                <div class="linha">
                    <asp:Label ID="lblCidade" CssClass="label_principal" Text="Cidade:" runat="server" AssociatedControlID="lblCidadeValor"></asp:Label>
                    <div class="container_campo">
                        <asp:Literal ID="lblCidadeValor" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="linha">
                    <asp:Label ID="lblBairro" Text="Bairro:" CssClass="label_principal" runat="server" AssociatedControlID="lblBairroValor"></asp:Label>
                    <div class="container_campo">
                        <asp:Literal ID="lblBairroValor" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="linha">
                    <asp:Label ID="lblDataCadastro" CssClass="label_principal" Text="Cadastrada em:" runat="server" AssociatedControlID="lblDataCadastroValor"></asp:Label>
                    <div class="container_campo">
                        <asp:Literal ID="lblDataCadastroValor" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="linha">
                    <asp:Label ID="lblCurriculosVisualizados" CssClass="label_principal" Text="Currículos Visualizados:" runat="server" AssociatedControlID="lblCurriculosVisualizadosValor"></asp:Label>
                    <div class="container_campo">
                        <asp:Literal ID="lblCurriculosVisualizadosValor" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="linha">
                    <asp:Label ID="lblVagasDivulgadas" CssClass="label_principal" Text="Vagas Divulgadas:" runat="server" AssociatedControlID="lblVagasDivulgadasValor"></asp:Label>
                    <div class="container_campo">
                        <asp:Literal ID="lblVagasDivulgadasValor" runat="server"></asp:Literal>
                    </div>
                </div>
                <asp:Panel CssClass="painel_botoes" ID="pnlBotaoCandidatar" runat="server" Visible="false">
                    <asp:ImageButton ID="btnCandidatar" ImageUrl="/img/candidatar_se.png" AlternateText="Candidatar" ToolTip="Cantidatar" runat="server" CausesValidation="false" OnClick="btnCandidatar_Click" />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel CssClass="painel_empresa_confidencial" ID="pnlDadosEmpresaConfidencial" runat="server">
                <p>
                    <asp:Literal ID="lblMensagem" runat="server"></asp:Literal>
                </p>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:HiddenField ID="hfVariavel" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeVerDadosEmpresa" runat="server" PopupControlID="pnlVerDadosEmpresa" TargetControlID="hfVariavel">
</AjaxToolkit:ModalPopupExtender>
