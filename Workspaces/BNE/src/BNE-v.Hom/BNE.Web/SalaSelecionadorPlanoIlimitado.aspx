<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="SalaSelecionadorPlanoIlimitado.aspx.cs"
    Inherits="BNE.Web.SalaSelecionadorPlanoIlimitado" %>

<%@ Register Src="~/UserControls/Modais/ModalConfirmacaoRetornoPagamento.ascx" TagName="ModalConfirmacaoRetornoPagamento" TagPrefix="uc" %>
<%@ Register Src="UserControls/Modais/EnvioEmail.ascx" TagName="EnvioEmail" TagPrefix="uc1" %>
<%@ Register Src="UserControls/Modais/ucWebCallBack_Modais.ascx" TagName="WebCallBack_Modais" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicScript runat="server" Src="/js/local/SalaSelecionadorPlanoIlimitado.js" type="text/javascript" />
    <Employer:DynamicScript runat="server" Src="js/jquery.maskMoney.min.js" type="text/javascript" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/Sala_Selecionador.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaSelecionadorMensagens.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/MeuPlano.css" type="text/css" rel="stylesheet" />


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:UpdatePanel runat="server" ID="upPlano" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hdfValorPlano" runat="server" />
            <asp:Panel ID="pnlPlano" runat="server">
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
                                            <asp:Label ID="lblPlanoValor" Text="Valor da Parcela: " runat="server"></asp:Label>
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
                                            <asp:Label ID="litQuantidadeVisualizacaoDisponivel" runat="server" Text="Visualizações Disponíveis: "></asp:Label>
                                            <asp:Label ID="litQuantidadeVisualizacaoDisponivelValor" Style="color: #555555" runat="server"></asp:Label>
                                        </li>
                                        <li>
                                            <asp:Label ID="lblQuantidadeSMSDisponivel" runat="server" Text="SMS Disponíveis: "></asp:Label>
                                            <asp:Label ID="lblQuantidadeSMSDisponivelValor" Style="color: #555555" runat="server"></asp:Label>
                                        </li>
                                        <li>
                                            <asp:Label ID="litQuantidadeCampanhaDisponivel" runat="server" Text="Campanhas Disponíveis: "></asp:Label>
                                            <asp:Label ID="lblQuantidadeCampanhaDisponivel" Style="color: #555555" runat="server"></asp:Label>
                                        </li>

                                        <li>
                                            <asp:Label ID="lblVerContratoClique" runat="server" Text="Para visualizar o contrato atual"></asp:Label>
                                            <asp:LinkButton ID="lbVerContrato" Text="clique aqui." runat="server" OnClick="lbVerContrato_OnClick" Font-Underline="True" Style="color: #555555" />
                                        </li>
                                        <li>
                                            <button id="btnCancelarAssinatura" runat="server" class="btn_cancelar_assinatura" type="button" data-toggle="modal" data-target="#modalCancelarAssinaturaRecorrente" visible="false">
                                                Cancelar Plano
                                            </button>
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
                <!-- INICIO PLANO DE LIBERADO
     /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////-->
                <asp:UpdatePanel ID="upParcelas" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="pnlParcelas" Visible="False" CssClass="extrato_plano">
                            <h2>Extrato Plano Atual</h2>
                            <telerik:RadGrid ID="gvParcelas" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao"
                                runat="server" Skin="Office2007" GridLines="None" OnPageIndexChanged="gvParcelas_PageIndexChanged" OnItemDataBound="gvParcelas_ItemDataBound" PageSize="24">
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
                                                <asp:HyperLink ID="btlVisualizarNotaFiscal" runat="server" ToolTip="Visualizar Nota Fiscal" AlternateText="Visualizar Nota Fiscal" Visible='<%# AjustarVisibilidadeNotaFiscal(Eval("Url_Nota_Fiscal").ToString()) %>' CausesValidation="false" Style="font-size: 16px;" NavigateUrl='<%# Eval("Url_Nota_Fiscal") %>' Target="_blank">
                                        <b>NF</b>
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
                    <asp:Button ID="btnEnviarPorEmail" runat="server" CssClass="botao_padrao" Text="Enviar Extrato Por E-mail" CausesValidation="false"
                        OnClick="btnEnviarPorEmail_Click" />
                    <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
                        OnClick="btnVoltar_Click" />
                </asp:Panel>
                <!-- FIM PLANO DE LIBERADO
     /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////-->

                <!-- PLANO DE LIBERAÇÃO LiberacaoAutomatica OU AUTOMATICA 
     /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////-->
                <asp:UpdatePanel ID="upParcelasLiberacaoAutomatica" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="pnlParcelasLiberacaoAutomatica" Visible="False" CssClass="extrato_plano">
                            <h2>Extrato Plano Renovação</h2>
                            <telerik:RadGrid ID="gvParcelasLiberacaoAutomatica" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao"
                                runat="server" Skin="Office2007" GridLines="None" OnPageIndexChanged="gvParcelasLiberacaoAutomatica_PageIndexChanged" OnItemDataBound="gvParcelasLiberacaoAutomatica_ItemDataBound" PageSize="24">
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
                                                <asp:HyperLink ID="btlBoletoLiberacaoAutomatica" runat="server" ToolTip="Visualizar Boleto" AlternateText="Visualizar Boleto" Visible='<%#  
                                            !String.IsNullOrEmpty(Eval("Idf_Tipo_Pagamento").ToString()) ?                                                                
                                            AjustarVisibilidadeVisualizarBoleto(Convert.ToInt32(Eval("Idf_Pagamento_Situacao")),Convert.ToBoolean(Eval("Cortesia")), Convert.ToInt32(Eval("Idf_Tipo_Pagamento")))  :
                                            AjustarVisibilidadeVisualizarBoleto(Convert.ToInt32(Eval("Idf_Pagamento_Situacao")),Convert.ToBoolean(Eval("Cortesia")), 0) 
                                            %>'
                                                    CausesValidation="false" Style="font-size: 16px;" Target="_blank">
                                        <i class="fa fa-barcode"></i>
                                                </asp:HyperLink>
                                                <asp:HyperLink ID="btlVisualizarNotaFiscalLiberacaoAutomatica" runat="server" ToolTip="Visualizar Nota Fiscal" AlternateText="Visualizar Nota Fiscal" Visible='<%# AjustarVisibilidadeNotaFiscal(Eval("Url_Nota_Fiscal").ToString()) %>' CausesValidation="false" Style="font-size: 16px;" NavigateUrl='<%# Eval("Url_Nota_Fiscal") %>' Target="_blank">
                                        <b>NF</b>
                                                </asp:HyperLink>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Panel ID="pnlBotoesLiberacaoAutomatica" runat="server" CssClass="painel_botoes" Visible="false">
                    <asp:Button ID="btnEnviarPorEmailLiberacaoAutomatica" runat="server" CssClass="botao_padrao" Text="Enviar Extrato Por E-mail" CausesValidation="false"
                        OnClick="btnEnviarPorEmailLiberacaoAutomatica_Click" />
                    <asp:Button ID="Button2" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
                        OnClick="btnVoltar_Click" />
                </asp:Panel>
                <!-- FIM PLANO DE LIBERAÇÃO LiberacaoAutomatica OU AUTOMATICA 
     /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////-->
                <!--
        ADICIONAL PLANO /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        -->
                <asp:UpdatePanel ID="upAdicional" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel runat="server" ID="pnlAdicional" Visible="False" CssClass="extrato_plano">
                            <h2>Extrato Plano Adicional Atual</h2>
                            <telerik:RadGrid ID="gvAdicional" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao"
                                runat="server" Skin="Office2007" GridLines="None" OnPageIndexChanged="gvAdicional_PageIndexChanged" OnItemDataBound="gvAdicional_ItemDataBound" PageSize="24">
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                                <AlternatingItemStyle CssClass="alt_row" />
                                <MasterTableView DataKeyNames="Idf_Pagamento, Idf_Adicional_Plano">
                                    <Columns>
                                        <telerik:GridBoundColumn HeaderText="Tipo" DataField="Des_Tipo_Adicional" ItemStyle-CssClass="center">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderText="Quantidade" DataField="Qtd_Adicional" ItemStyle-CssClass="center">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderText="Valor" DataField="Vlr_Pagamento" ItemStyle-CssClass="center">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderText="Status" DataField="Des_Adicional_Plano_Situacao"
                                            ItemStyle-CssClass="center">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="col_action tab_tipoplano">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="btlBoleto" runat="server" ToolTip="Visualizar Boleto" AlternateText="Visualizar Boleto" Visible='<%#  
                                            !String.IsNullOrEmpty(Eval("Idf_Tipo_Pagamento").ToString()) ?                                                                
                                            AjustarVisibilidadeVisualizarBoleto(Convert.ToInt32(Eval("Idf_Pagamento_Situacao")),false, Convert.ToInt32(Eval("Idf_Tipo_Pagamento")))  :
                                            AjustarVisibilidadeVisualizarBoleto(Convert.ToInt32(Eval("Idf_Pagamento_Situacao")),false, 0) 
                                            %>'
                                                    CausesValidation="false" Style="font-size: 16px;" Target="_blank">
                                        <i class="fa fa-barcode"></i>
                                                </asp:HyperLink>
                                                <asp:HyperLink ID="btlVisualizarNotaFiscal" runat="server" ToolTip="Visualizar Nota Fiscal" AlternateText="Visualizar Nota Fiscal" Visible='<%# AjustarVisibilidadeNotaFiscal(Eval("Url_Nota_Fiscal").ToString()) %>' CausesValidation="false" Style="font-size: 16px;" NavigateUrl='<%# Eval("Url_Nota_Fiscal") %>' Target="_blank">
                                        <b>NF</b>
                                                </asp:HyperLink>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!-- FIM PLANO ADICIONAL /////////////////////////////////////////////////////////////////////////////-->
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
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc:ModalConfirmacaoRetornoPagamento ID="ucModalConfirmacaoRetornoPagamento" runat="server"></uc:ModalConfirmacaoRetornoPagamento>
    <div id="modalCancelarAssinaturaRecorrente" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="width: 32%;
    top: 7%;left:31%; background-color: #fff; height: 63%;">
        <asp:UpdatePanel ID="upModalCancelarAssinaturaRecorrente" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-header">
                    <button type="button" id="closeModal" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h2 id="modalTitulo">Cancelamento de Plano</h2>
                </div>
                <div id="divConteudoCancelarAssinatura" runat="server" class="modal-body" style="font-size: 16px; color: #666">
                    <div style="margin-top: 10px;">
                        <asp:Label ID="lblQtdCurriculosVisualizados" Text="Você já visualizou" runat="server"></asp:Label>
                        <asp:Label ID="lblQtdCurriculosVisualizadosValor" runat="server"></asp:Label>
                    </div>
                    <div style="margin-top: 10px;">
                        <asp:Label ID="lblQtdEnvioMensagens" Text="Enviou" runat="server"></asp:Label>
                        <asp:Label ID="lblQtdEnvioMensagensValor" runat="server"></asp:Label>
                    </div>
                    <div style="margin-top: 10px;">
                        <asp:Label ID="lblQtdAnuncioVagas" Text="Anunciou" runat="server"></asp:Label>
                        <asp:Label ID="lblQtdAnuncioVagasValor" runat="server"></asp:Label>
                    </div>
                    <div style="margin-top: 10px;">
                        <asp:Label ID="lblDataInicioPlanoAdquirido" Text="Seu cadastro é desde" runat="server"></asp:Label>
                        <asp:Label ID="lblDataInicioPlanoAdquiridoValor" Style="color: #555555" runat="server"></asp:Label>
                    </div>
                    <div style="margin-top: 10px;">
                        <asp:Label ID="lblDataFimPlanoAdquirido" Text="Sua Assinatura está ativa até" runat="server"></asp:Label>
                        <asp:Label ID="lblDataFimPlanoAdquiridoValor" Style="color: #555555" runat="server"></asp:Label>
                    </div>
                    <div style="margin-top: 10px;">
                        <asp:Label ID="lblDesejaCancelar" Text="Tem certeza que deseja cancelar a partir desta data?" runat="server"></asp:Label>
                    </div>
                    <div style="margin-top: 20px;">
                        <div style="float: left">
                            <asp:Button runat="server" CssClass="btn_nao_cancelarAss" Text="Não" data-toggle="modal" data-target="#modalCancelarAssinaturaRecorrente" />
                        </div>
                        <div style="margin-left: 20px;">
                            <asp:Button CssClass="btn_sim_cancelarAss" runat="server" Text="Sim" OnClick="btnEfetivarCancelamentoAssinatura_Click" OnClientClick="OcultarBtnCancelar();" />
                        </div>
                    </div>
                    <div style="margin-top: 20px; clear: both">
                        <asp:Label runat="server" Text="Se preferir ligamos para você "></asp:Label>
                        <button id="modalWebCallBack" class="btn_MeLigue_cancelarAss" runat="server" type="button" data-toggle="modal" data-target="#nomeModal"><i class="fa fa-phone"></i>Me Ligue</button>
                    </div>
                </div>
                <div id="divSucessoCancelamentoAssinatura" class="modal-footer" runat="server" visible="false">
                    <div class="alert">
                        A assinatura do seu plano foi cancelada com sucesso!<br></br>
                        Seu plano continuará válido até <strong>
                            <asp:Label ID="lblPlanoRecorrenteValidade" Style="color: #555555" runat="server"></asp:Label></strong>
                    </div>
                </div>
                <div id="divErroCancelamentoAssinatura" class="modal-footer" runat="server" visible="false">
                    <div class="alert">
                        Ocorreu um erro e não foi possível cancelar a assinatura do seu plano,<br></br>
                        Para efetivar o cancelamento, por favor entre em contato através do <strong>0800 41 2400</strong>.
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="upCancelamento" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlCancelamento" runat="server" Visible="false">
                <div class="cia-cancelation">
                    <section id="etapa01">
                        <div class="cancelation-title">
                            <h2>
                                <asp:Label runat="server" ID="lblNomeResponsavel"></asp:Label><span style="font-weight: lighter;"> suas ações no site tiveram resultados!</span></h2>
                            <h4>Acreditamos que você possa aproveitar muito mais...</h4>
                        </div>
                        <div class="cancelation-facets">
                            <%--INICIO - INSCRITOS NAS VAGAS NÃO VISUALIZADOS--%>
                            <asp:Panel ID="pnlInscritosNaoVisualizados" runat="server">
                                <div class="cancelation-facet">
                                    <div class="cancelation-facet-icon">
                                        <svg style="width: 32px; height: 32px" viewBox="0 0 24 24">
                                            <path d="M20,11H4V8H20M20,15H13V13H20M20,19H13V17H20M11,19H4V13H11M20.33,4.67L18.67,3L17,4.67L15.33,3L13.67,4.67L12,3L10.33,4.67L8.67,3L7,4.67L5.33,3L3.67,4.67L2,3V19A2,2 0 0,0 4,21H20A2,2 0 0,0 22,19V3L20.33,4.67Z" />
                                        </svg>
                                    </div>
                                    <div class="cancelation-facet-desc">
                                        <h3>
                                            <asp:Label ID="lblCurriculosNaoVisualizados" runat="server"></asp:Label>
                                            <span style="font-weight: lighter;">inscritos</span></h3>
                                        <h6>não visualizados em suas vagas</h6>
                                    </div>
                                    <asp:Button runat="server" ID="btnVisualizarCurriculos" CssClass="btn" OnClick="btnVisualizarCurriculos_Click" Text="visualizar currículos" />

                                </div>
                            </asp:Panel>
                            <%--FIM -INSCRITOS NAS VAGAS NÃO VISUALIZADOS--%>

                            <%--INICIO -  NÃO TEM CURRICULOS NÃO VISUALIZADOS EM SUAS VAGAS OU NÃO TEM VAGA ANUNCIADA--%>
                            <asp:Panel ID="pnlCurriculosPesquisa" runat="server" Visible="false">
                                <div class="cancelation-facet">
                                    <div class="cancelation-facet-icon">
                                        <svg style="width: 32px; height: 32px" viewBox="0 0 24 24">
                                            <path d="M20,11H4V8H20M20,15H13V13H20M20,19H13V17H20M11,19H4V13H11M20.33,4.67L18.67,3L17,4.67L15.33,3L13.67,4.67L12,3L10.33,4.67L8.67,3L7,4.67L5.33,3L3.67,4.67L2,3V19A2,2 0 0,0 4,21H20A2,2 0 0,0 22,19V3L20.33,4.67Z" />
                                        </svg>
                                    </div>
                                    <div class="cancelation-facet-desc">
                                        <h3>
                                            <asp:Label ID="lblQtdCvNaoVisualizadosDePesq" runat="server"></asp:Label>
                                            <span style="font-weight: lighter;">inscritos</span></h3>
                                        <h6>não visualizados em sua última pesquisa</h6>
                                        <asp:HiddenField runat="server" ID="hfIDPesquisaCurriculo" />
                                    </div>
                                    <asp:Button runat="server" ID="btnCurriculosPesquisa" CssClass="btn" OnClick="btnCurriculosPesquisa_Click"  Text="visualizar currículos" />

                                </div>
                            </asp:Panel>
                            <%-- FIM -INSCRITOS NAS VAGAS NÃO VISUALIZADOS--%>


                            <div class="separator-vert"></div>
                            <%-- INICIO - TEM SMS DISPONIVEL--%>
                            <asp:Panel ID="pnlTemSms" runat="server">
                                <div class="cancelation-facet light">
                                    <div class="cancelation-facet-icon">
                                        <svg style="width: 32px; height: 32px" viewBox="0 0 24 24">
                                            <path d="M20,2H4A2,2 0 0,0 2,4V22L6,18H20A2,2 0 0,0 22,16V4A2,2 0 0,0 20,2M6,9H18V11H6M14,14H6V12H14M18,8H6V6H18" />
                                        </svg>
                                    </div>
                                    <div class="cancelation-facet-desc">
                                        <h3>
                                            <asp:Label ID="lblQtdSms" runat="server"></asp:Label>
                                            <span style="font-weight: lighter;">SMS</span></h3>
                                        <h6>disponíveis para recrutamento</h6>
                                    </div>
                                    <asp:Button runat="server" ID="BntRecrutarSms" OnClick="BntRecrutarSms_Click" CssClass="btn" Text="Recrutar utilizando SMS" />
                                </div>
                            </asp:Panel>
                            <%-- FIM - TEM SMS DISPONIVEL--%>

                            <%-- INICIO - NÃO SMS DISPONIVEL --%>
                            <asp:Panel ID="pnlSemSMS" runat="server" Visible="false">
                                <div class="cancelation-facet light">
                                    <div class="cancelation-facet-icon">
                                        <svg style="width: 32px; height: 32px" viewBox="0 0 24 24">
                                            <path d="M20,2H4A2,2 0 0,0 2,4V22L6,18H20A2,2 0 0,0 22,16V4A2,2 0 0,0 20,2M6,9H18V11H6M14,14H6V12H14M18,8H6V6H18" />
                                        </svg>
                                    </div>
                                    <div class="cancelation-facet-desc">
                                        <h3>
                                            <asp:Label ID="lblqtdPesquisa" runat="server"></asp:Label>
                                            <span style="font-weight: lighter;">Pesquisas</span></h3>
                                        <h6>de currículos realizadas</h6>
                                    </div>
                                    <asp:Button runat="server" ID="btnPesquisaAvançada" OnClick="BntRecrutarSms_Click" CssClass="btn" Text="Realizar nova pesquisa" />
                                </div>
                            </asp:Panel>
                            <%-- FIM - NÃO TEM SMS DISPONIVEL--%>

                        </div>
                        <div class="cancelation-actions ">
                            <div>
                                <asp:Button runat="server" ID="btnManterPlano" Text="Manter plano ativo" CssClass="btn btn-lg btn-success" OnClick="btnManterPlano_Click" />
                            </div>
                            <div>
                                <input type="button" class="btn btn-link" value="Cancelar minha assinatura" id="btn-cancel-01"></input>
                            </div>
                        </div>
                        <asp:HiddenField ID="hfTotalCv" runat="server" />
                    </section>
                    <section id="etapa02">
                        <div class="cancelation-title">
                            <h2><span style="font-weight: lighter;">Alguns candidatos estão esperando seu contato!</span></h2>
                            <h4>Temos mais de<strong>
                                <asp:Label runat="server" ID="lblQtdCurriculosPesquisa"></asp:Label>
                                currículos</strong> de <strong>
                                    <asp:Label runat="server" ID="lblFuncaoPesquisa"></asp:Label></strong> em <strong>
                                        <asp:Label runat="server" ID="lblCidadePesquisa"></asp:Label></strong> que você não acessou. Confira alguns: </h4>
                        </div>
                        <div class="cancelation-facets">
                            <asp:Repeater ID="rpCvsNaoVisualizados" runat="server">
                                <ItemTemplate>
                                    <div class="candidato-facet">
                                        <h5><%# DataBinder.Eval(Container.DataItem, "Nome") %>, <%# DataBinder.Eval(Container.DataItem, "Idade") %> anos </h5>
                                        <h6><%# DataBinder.Eval(Container.DataItem, "Funcao") %> - <%# DataBinder.Eval(Container.DataItem, "Cidade") %></h6>
                                        <p><%# DataBinder.Eval(Container.DataItem, "Descricao") %>...</p>
                                        <a href="<%# DataBinder.Eval(Container.DataItem, "URL") %>">
                                            <input type="button" class="btn" value="ver currículo"></input>
                                        </a>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <div class="cancelation-actions ">
                            <div>
                                <asp:Button runat="server" ID="btnVerListaCompleta" OnClick="btnCurriculosPesquisa_Click" CssClass="btn btn-lg btn-success" Text="ver lista completa" />
                            </div>
                            <div>
                                <input type="button" class="btn btn-link" value="Cancelar minha assinatura" id="btn-cancel-02"></input>
                            </div>
                        </div>
                    </section>
                    <section id="etapa03">
                        <div class="cancelation-title">
                            <h2>Nos ajude a melhorar!</h2>
                            <h4>Investimos muito tempo para que os candidatos mantenham seus<br />
                                currículos completos e atualizados para você!
                            </h4>
                        </div>
                        <div class="cancelation-question row">
                            <div>
                                <h4>Qual o principal motivo do cancelamento?</h4>
                            </div>
                            <asp:CheckBoxList ID="cblMotivo" runat="server" RepeatDirection="Vertical" TextAlign="Right"></asp:CheckBoxList>
                            <div class="checkbox ">
                                <label>
                                    <input type="checkbox" value="" onclick="valCheck(this);" id="motivo1">Já finalizei meu processo seletivo
                                </label>
                            </div>
                            <div class="motivo1-opcoes">
                                <div class="radio ">
                                    <label>
                                        <input type="radio" value="" runat="server" class="valCheck" onclick="valCheck(this);" id="rbCandidatoBNE" name="motivo1">Candidato BNE
                                    </label>

                                </div>
                                <div class="radio ">
                                    <label>
                                        <input type="radio" value="" runat="server" class="valCheck" onclick="valCheck(this);" id="rbIndicacao" name="motivo1">Indicação
                                    </label>
                                </div>
                                <div class="radio ">
                                    <label>
                                        <input type="radio" value="" runat="server" class="valCheck" onclick="valCheck(this);" name="motivo1" id="outro_site">Outro site
                                    </label>
                                </div>
                                <input type="text" name="" placeholder="Qual?" runat="server" id="qual" />
                            </div>
                            <div class="checkbox">
                                <label>
                                    <input type="checkbox" runat="server" class="valCheck" onclick="valCheck(this);" id="ckNaoConsegui" value="">Não consegui utilizar o site
                                </label>

                            </div>
                            <div class="motivo5-opcoes">
                                <textarea placeholder="(opcional)" runat="server" id="txtMotivoNaoConseguir"></textarea>
                            </div>
                            <div class="checkbox">
                                <label>
                                    <input type="checkbox" runat="server" class="valCheck" onclick="valCheck(this);" id="ckNaoObtiveResultados" value="">Não obtive resultados com os anúncios
                                </label>

                            </div>
                            <div class="checkbox">
                                <label>
                                    <input type="checkbox" runat="server" class="valCheck" onclick="valCheck(this);" id="ckNaoConseguiContato" value="">Não consegui contato com os candidatos
                                </label>

                            </div>
                            <div class="checkbox">
                                <label>
                                    <input type="checkbox" value="" runat="server" class="valCheck" onclick="valCheck(this);" id="motivo2">O valor da assinatura está muito alto
                                </label>

                            </div>
                            <div class="motivo2-opcoes">
                                <label>Quanto você acredita ser o valor ideal?</label>
                            
                               <input  name="" value="180" runat="server" id="txtValorAssinatura2" step="10"  />
                                 <span>R$</span>
                             
                            </div>
                            <div class="checkbox">
                                <label>
                                    <input type="checkbox" value="" runat="server" class="valCheck" onclick="valCheck(this);" id="motivo3">Vou testar outras ferramentas
                                </label>

                            </div>
                            <div class="motivo3-opcoes">
                                <textarea placeholder="Quais? (opcional)" runat="server" id="txtFerramentasTestar"></textarea>
                            </div>
                            <div class="checkbox">
                                <label>
                                    <input type="checkbox" runat="server" class="valCheck" onclick="valCheck(this);" id="ckAssinaturaNecessidade" value="">Esta assinatura não atende às minhas necessidades.
                                </label>

                            </div>
                            <div class="checkbox">
                                <label>
                                    <input type="checkbox" value="" class="valCheck" onclick="valCheck(this);" runat="server" id="motivo4">Outros
                                </label>

                            </div>
                            <div class="motivo4-opcoes">
                                <textarea placeholder="Quais? (opcional)" runat="server" id="txtOutros"></textarea>
                            </div>
                            <div class="checkbox">
                                <label class="" style="display: none; color: red !important" id="lblMensagem">Selecione pelo menos um dos motivos.</label>
                                <label class="" style="display: none; color: red !important" id="lblMensagemErro">Ocorreu um erro ao encerrar o plano.</label>
                            </div>
                        </div>
                        <div class="cancelation-actions ">
                            <div>

                                <asp:Button runat="server" OnClientClick="return Validar();" ID="btnFinalizarCancelamento" OnClick="btnFinalizarCancelamento_Click" Text="Cancelar minha assinatura" CssClass="btn btn-lg btn-sucess" />
                                <%--<input type="button" class="btn btn-lg btn-success " value="Cancelar minha assinatura" id="btn-cancel-03"></input>--%>
                            </div>
                        </div>
                    </section>
                    <section id="etapa04">
                        <div class="cancelation-title">
                            <h2>Até breve!</h2>
                        </div>
                        <div class="cancelation-success-icon">
                            <svg style="width: 112px; height: 112px" viewBox="0 0 24 24">
                                <path fill="#8bc34a" d="M12,2A10,10 0 0,1 22,12A10,10 0 0,1 12,22A10,10 0 0,1 2,12A10,10 0 0,1 12,2M11,16.5L18,9.5L16.59,8.09L11,13.67L7.91,10.59L6.5,12L11,16.5Z" />
                            </svg>
                        </div>
                        <div class="cancelation-title">
                            <h4>Sua assinatura do Plano CIA foi cancelada.<br></br>
                                Seu acesso estará disponível até <strong><asp:Label runat="server" ID="lblDtaCancelamento"></asp:Label></strong>.
                            </h4>
                        </div>
                        <div class="cancelation-actions ">
                            <div>
                                <a href="SalaSelecionador.aspx">
                                    <input type="button" class="btn btn-success" value="Página inicial" id="btn-cancel-04"></input>
                                </a>
                            </div>
                        </div>
                    </section>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <uc1:EnvioEmail ID="ucEnvioEmail" runat="server" />
    <uc1:EnvioEmail ID="ucEnvioEmailLiberacaoAutomatica" runat="server" />

    <asp:UpdatePanel ID="upWebCallBack" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlWebCallBack" runat="server" Visible="true">
                <uc:WebCallBack_Modais ID="ucWebCallBack_Modais" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
