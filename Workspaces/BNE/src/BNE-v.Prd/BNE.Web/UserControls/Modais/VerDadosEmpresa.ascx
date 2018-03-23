<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VerDadosEmpresa.ascx.cs" Inherits="BNE.Web.UserControls.Modais.VerDadosEmpresa" %>
<%@ Register Src="../UCLogoFilial.ascx" TagName="UCLogoFilial" TagPrefix="uc1" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/VerDadosEmpresa.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlVerDadosEmpresa" CssClass="modal_conteudo candidato  " Style="display: none" runat="server">
    <asp:UpdatePanel ID="upVerDadosEmpresa" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:LinkButton CssClass=" modal_fechar" ID="btiFechar" runat="server" CausesValidation="false" OnClick="btiFechar_Click"></asp:LinkButton>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="dados_da_empresa">
                <h3 class="titulo_modal_Dados_Empresa">
                    <asp:Label ID="lblTitulo" runat="server"></asp:Label>
                </h3>
                <div class="painel_info_vip">
                    <uc1:UCLogoFilial ID="imgLogo" CssClass="logo_empresa" runat="server" Visible="false" />
                </div>


                <asp:Panel CssClass="painel_dados_empresa" ID="pnlDadosEmpresaVisualizacao" runat="server">
                    <div class="painel_dados_empresa">
                        <div class="linhaEmpresa">
                            <asp:Label ID="lblNomeEmpresa" CssClass="label_principal" Text="Nome da Empresa:" runat="server" AssociatedControlID="lblNomeEmpresaValor"></asp:Label>
                            <div class="container_campo">
                                <asp:Literal ID="lblNomeEmpresaValor" runat="server"></asp:Literal>
                                <asp:LinkButton ID="lbtEmpresaBloqueada" OnClick="lbtVendaVip_Click" runat="server">Somente para Cliente <span class="texto_vip">VIP</span></asp:LinkButton>
                            </div>
                        </div>
                        <div class="linhaEmpresa">
                            <asp:Label ID="lblAtividadeEmpresa" CssClass="label_principal" Text="Atividade da Empresa:" runat="server" AssociatedControlID="lblAtividadeEmpresaValor"></asp:Label>
                            <div class="container_campo">
                                <asp:Literal ID="lblAtividadeEmpresaValor" runat="server"></asp:Literal>
                            </div>
                        </div>
                        <div class="linhaEmpresa">
                            <asp:Label ID="lblNumeroFuncionario" CssClass="label_principal" Text="Número de Funcionários:" runat="server" AssociatedControlID="lblNumeroFuncionarioValor"></asp:Label>
                            <div class="container_campo">
                                <asp:Literal ID="lblNumeroFuncionarioValor" runat="server"></asp:Literal>
                            </div>
                        </div>
                        <div class="linhaEmpresa">
                            <asp:Label ID="lblTelefone" CssClass="label_principal" Text="Telefone:" runat="server" AssociatedControlID="lblTelefoneValor"></asp:Label>
                            <div class="container_campo">
                                <asp:Label ID="lblTelefoneValor" runat="server"></asp:Label>
                                <asp:LinkButton ID="lbtTelefoneBloqueado" OnClick="lbtVendaVip_Click" runat="server">Somente para Cliente <span class="texto_vip">VIP</span></asp:LinkButton>
                            </div>
                        </div>
                        <div class="linhaEmpresa">
                            <asp:Label ID="lblCidade" CssClass="label_principal" Text="Cidade da Empresa:" runat="server" AssociatedControlID="lblCidadeValor"></asp:Label>
                            <div class="container_campo">
                                <asp:Literal ID="lblCidadeValor" runat="server"></asp:Literal>
                            </div>
                        </div>
                        <div class="linhaEmpresa">
                            <asp:Label ID="lblBairro" Text="Bairro da Empresa:" CssClass="label_principal" runat="server" AssociatedControlID="lblBairroValor"></asp:Label>
                            <div class="container_campo">
                                <asp:Literal ID="lblBairroValor" runat="server"></asp:Literal>
                            </div>
                        </div>
                        <div class="linhaEmpresa">
                            <asp:Label ID="lblDataCadastro" CssClass="label_principal" Text="Empresa Cadastrada em:" runat="server" AssociatedControlID="lblDataCadastroValor"></asp:Label>
                            <div class="container_campo">
                                <asp:Literal ID="lblDataCadastroValor" runat="server"></asp:Literal>
                            </div>
                        </div>
                        <div class="linhaEmpresa">
                            <asp:Label ID="lblCurriculosVisualizados" CssClass="label_principal" Text="Currículos Visualizados:" runat="server" AssociatedControlID="lblCurriculosVisualizadosValor"></asp:Label>
                            <div class="container_campo">
                                <asp:Literal ID="lblCurriculosVisualizadosValor" runat="server"></asp:Literal>
                            </div>
                        </div>
                        <div class="linhaEmpresa">
                            <asp:Label ID="lblVagasDivulgadas" CssClass="label_principal" Text="Vagas Divulgadas:" runat="server" AssociatedControlID="lblVagasDivulgadasValor"></asp:Label>
                            <div class="container_campo">
                                <asp:Literal ID="lblVagasDivulgadasValor" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                    <asp:Panel CssClass="painel_botoes" ID="pnlBotaoCandidatar" runat="server" Visible="false">
                        <div class="comprar-vip">
                            <asp:Button ID="btnCandidata" AlternateText="Candidatar" ToolTip="Cantidatar" Text="Candidatar-se" runat="server"
                                CausesValidation="false" OnClick="btnCandidata_Click" />
                        </div>
                    </asp:Panel>
                </asp:Panel>
                <asp:Panel CssClass="painel_empresa_confidencial" ID="pnlDadosEmpresaConfidencial" runat="server">
                    <p>
                        <asp:Literal ID="lblMensagem" runat="server"></asp:Literal>
                    </p>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:HiddenField ID="hfVariavel" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeVerDadosEmpresa" runat="server" PopupControlID="pnlVerDadosEmpresa" TargetControlID="hfVariavel">
</AjaxToolkit:ModalPopupExtender>
