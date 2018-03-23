<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="SalaSelecionadorPlanoIlimitado.aspx.cs"
    Inherits="BNE.Web.SalaSelecionadorPlanoIlimitado" %>

<%@ Register Src="~/UserControls/Modais/ModalConfirmacaoRetornoPagamento.ascx" TagName="ModalConfirmacaoRetornoPagamento"
    TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicScript runat="server" Src="/js/local/SalaSelecionadorPlanoIlimitado.js" type="text/javascript" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/Sala_Selecionador.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaSelecionadorMensagens.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/MeuPlano.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="painel_padrao_sala_selecionador">
        <div class="caracteristicas_plano">
            <ul>
                <asp:UpdatePanel ID="upPlanoIlimitado" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="alinhar">
                            <li>
                                <asp:Label ID="lblDataPlanoAdquirido" Text="Plano Adquirido em: " runat="server"></asp:Label>
                                <asp:Label ID="lblDataPlanoAdquiridoValor" Style="color: #555555" runat="server"></asp:Label>
                            </li>
                            <li>
                                <asp:Label ID="lblPlanoValidade" Text="Plano Válido até: " runat="server"></asp:Label>
                                <asp:Label ID="lblPlanoValidadeValor" Style="color: #555555" runat="server"></asp:Label>
                                <asp:Label ID="lblSeparador" runat="server" Text=" - " Visible="false"></asp:Label>
                                <asp:LinkButton ID="lbRenovarPlano" Text="Renovar Plano" Visible="false" runat="server" OnClick="lbRenovarPlano_Click"></asp:LinkButton>
                                <asp:Label ID="lblPlanoRenovado" Text="( Plano Renovado )" runat="server" Visible="false"></asp:Label>
                            </li>
                            <li>
                                <asp:Label ID="lblPlanoValor" Text="Valor do Plano: " runat="server"></asp:Label>
                                <asp:Label ID="lblPlanoValorTexto" Style="color: #555555" runat="server"></asp:Label>
                            </li>
                            <li>
                                <asp:Label ID="lblTipoPlano" runat="server" Text="Plano de Acesso: "></asp:Label>
                                <asp:Label ID="lblTipoPlanoValor" Style="color: #555555" runat="server"></asp:Label>
                            </li>
                        </div>
                        <div class="alinhar">
                            <li>
                                <asp:Label ID="lblCurriculosVisualizados" runat="server" Text="Currículos Visualizados: "></asp:Label>
                                <asp:Label ID="lblCurriculosVisualizadosValor" Style="color: #555555" runat="server"></asp:Label>
                            </li>
                            <li>
                                <asp:Label ID="litQuantidadeVisualizacaoDisponivel" runat="server" Text="Visualização Disponíveis: "></asp:Label>
                                <asp:Label ID="litQuantidadeVisualizacaoDisponivelValor" Style="color: #555555" runat="server"></asp:Label>
                            </li>
                            <li>
                                <asp:Label ID="lblQuantidadeSMSDisponivel" runat="server" Text="SMS Disponíveis: "></asp:Label>
                                <asp:Label ID="lblQuantidadeSMSDisponivelValor" Style="color: #555555" runat="server"></asp:Label>
                                <asp:Label ID="Label1" runat="server" Text=" - "></asp:Label>
                                <asp:LinkButton ID="lbComprarSMS" Text="Comprar mais SMS" runat="server" OnClick="lbComprarSMS_Click"></asp:LinkButton>
                            </li>
                            <li>
                                <asp:Label ID="lblVerContratoClique" runat="server" Text="Para visualizar o contrato atual"></asp:Label>
                                <asp:LinkButton ID="lbVerContrato" Text="clique aqui." runat="server" OnClick="lbVerContrato_OnClick" Font-Underline="True" Style="color: #555555" />
                            </li>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="lbVerContrato" />
                    </Triggers>
                </asp:UpdatePanel>
            </ul>
        </div>
    </div>
    <asp:UpdatePanel ID="upParcelas" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlParcelas" Visible="False" CssClass="extrato_plano">
                <h2>Extrato do Plano</h2>
                <telerik:RadGrid ID="gvParcelas" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao"
                    runat="server" Skin="Office2007" GridLines="None" OnPageIndexChanged="gvParcelas_PageIndexChanged" OnItemDataBound="gvParcelas_ItemDataBound">
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                    <AlternatingItemStyle CssClass="alt_row" />
                    <MasterTableView DataKeyNames="Idf_Pagamento, Parcela, Des_Identificador">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Parcela" ItemStyle-CssClass="center">
                                <ItemTemplate>
                                    <asp:Literal runat="server" Text='<%# Eval("Parcela") %>'></asp:Literal>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Data de Vencimento" ItemStyle-CssClass="center">
                                <ItemTemplate>
                                    <asp:Literal runat="server" Text='<%# Convert.ToDateTime(Eval("Dta_Vencimento")).ToString("dd/MM/yyyy") %>'></asp:Literal>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Data de Pagamento" ItemStyle-CssClass="center">
                                <ItemTemplate>
                                    <asp:Literal runat="server" Text='<%# Eval("Dta_Pagamento") != DBNull.Value ? Convert.ToDateTime(Eval("Dta_Pagamento")).ToString("dd/MM/yyyy") : string.Empty %>'>
                                    </asp:Literal>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Valor" ItemStyle-CssClass="center">
                                <ItemTemplate>
                                    <asp:Literal runat="server" Text='<%# Eval("Vlr_Pagamento") %>'></asp:Literal>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Situação" ItemStyle-CssClass="center">
                                <ItemTemplate>
                                    <asp:Literal ID="lblSituacao" runat="server" Text='<%# Eval("Des_Pagamento_Situacao") %>'></asp:Literal>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Forma Pagamento" ItemStyle-CssClass="center">
                                <ItemTemplate>
                                    <asp:Literal ID="lblTipo" runat="server" Text='<%# Eval("Des_Tipo_Pagamaneto") %>'></asp:Literal>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="col_action tab_tipoplano">
                                <ItemTemplate>
                                    <asp:HyperLink ID="btlBoleto" runat="server" ToolTip="Visualizar Boleto" AlternateText="Visualizar Boleto" Visible='<%#  
                                            !String.IsNullOrEmpty(Eval("Idf_Tipo_Pagamento").ToString()) ?                                                                
                                            AjustarVisibilidadeVisualizarBoleto(Convert.ToInt32(Eval("Idf_Pagamento_Situacao")),Convert.ToBoolean(Eval("Cortesia")), Convert.ToInt32(Eval("Idf_Tipo_Pagamento")))  :
                                            AjustarVisibilidadeVisualizarBoleto(Convert.ToInt32(Eval("Idf_Pagamento_Situacao")),Convert.ToBoolean(Eval("Cortesia")), 0) 
                                            %>'
                                        CausesValidation="false" Style="font-size: 16px;" Target="_blank">
                                        <i class="fa fa-barcode"></i>
                                    </asp:HyperLink>
                                    <asp:HyperLink ID="btlVisualizarNotaFiscal" runat="server" ToolTip="Visualizar Nota Fiscal" AlternateText="Visualizar Nota Fiscal" Visible='<%# AjustarVisibilidadeNotaFiscal(Convert.ToInt32(Eval("Idf_Pagamento_Situacao")),Convert.ToBoolean(Eval("Cortesia")), Convert.ToBoolean(Eval("Flg_Nota_Antecipada"))) %>' CausesValidation="false" Style="font-size: 16px;" NavigateUrl='<%# Eval("Url_Nota_Fiscal") %>' Target="_blank">
                                        <i class="fa fa-file-text-o"></i>
                                    </asp:HyperLink>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                </telerik:RadGrid>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
        <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
            OnClick="btnVoltar_Click" />
    </asp:Panel>
    <asp:UpdatePanel ID="upErroTransacaoCartao" runat="server" UpdateMode="Conditional" Visible="false">
        <ContentTemplate>
            <div class="painel_padrao_sala_vip">
                <div class="caracteristicas_plano">
                    <ul>
                        <li>
                            <asp:Label ID="Label11" Text="Erro ao Efetuar Pagamento. " runat="server" CssClass="label_campo"></asp:Label>
                            <asp:Button ID="Button1" runat="server" CssClass="botao_padrao" Text="Tente Novamente" CausesValidation="false"
                                OnClick="btnTenteNovamente_Click" />
                        </li>
                    </ul>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc:ModalConfirmacaoRetornoPagamento ID="ucModalConfirmacaoRetornoPagamento" runat="server"></uc:ModalConfirmacaoRetornoPagamento>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
