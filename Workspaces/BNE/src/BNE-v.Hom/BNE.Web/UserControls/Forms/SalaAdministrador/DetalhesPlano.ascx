<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="DetalhesPlano.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.DetalhesPlano" %>
<h2>Detalhes do Plano</h2>
<asp:UpdatePanel ID="upPnlEditarParcela" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlEditarParcela" runat="server" CssClass="blocodados">
            <h3>Editar Parcela</h3>
            <%--Linha Número de Parcelas --%>
            <div class="linha">
                <asp:Label ID="lblNumeroParcela" runat="server" Text="Parcela" CssClass="label_principal" />
                <div class="container_campo small">
                    <asp:Literal ID="litNumeroParcela" runat="server"></asp:Literal>
                </div>
            </div>
            <%--FIM Número de Parcelas --%>
            <%--Linha Situação --%>
            <div class="linha">
                <asp:Label ID="lblSituacao" runat="server" Text="Situação" CssClass="label_principal" />
                <div class="container_campo">
                    <telerik:RadComboBox runat="server" ID="rcbSituacao" EmptyMessage=" " AllowCustomText="false" CssClass="checkbox_large" />
                </div>
            </div>
            <%--FIM Situação --%>
            <%--Linha Data de Envio --%>
            <div class="linha">
                <asp:Label ID="lblDataEnvio" runat="server" Text="Data de Envio" CssClass="label_principal" />
                <div class="container_campo">
                    <componente:Data ID="txtDataEnvio" runat="server" ValidationGroup="SalvarParcela" />
                </div>
            </div>
            <%--FIM Data de Envio--%>
            <%--Linha Data de Vencimento--%>
            <div class="linha">
                <asp:Label ID="lblDataVencimento" runat="server" Text="Data de Vencimento" CssClass="label_principal" />
                <div class="container_campo">
                    <componente:Data ID="txtDataVencimento" runat="server" ValidationGroup="SalvarParcela" />
                </div>
            </div>
            <%--FIM Quantidade de Visualizações --%>
            <%-- Linha Nota Fiscal Antecipada --%>
            <div id="divNotaAntecipada" runat="server" class="linha">
                <asp:Label ID="lblNotaAntecipada" runat="server" Text="Nota Fiscal Antecipada" CssClass="label_principal" />
                <div class="container_campo">
                    <asp:CheckBox runat="server" ID="chkNotaAntecipada" AutoPostBack="true" OnCheckedChanged="chkNotaAntecipada_CheckedChanged" />
                </div>
            </div>
            <%--Linha Data de Envio De Nota Antecipada--%>
            <div class="linha" id="divDataNFAntecipada" runat="server" visible="false">
                <asp:Label ID="lblDataNFAntecipada" runat="server" Text="Data de Envio de Nota Fiscal Antecipada"
                    CssClass="label_principal" />
                <div class="container_campo">
                    <componente:Data ID="txtDataNFAntecipada" runat="server" ValidationGroup="SalvarParcela" />
                </div>
            </div>
            <%-- Fim Linha Nota Fiscal Antecipada --%>
            <%--Linha Valor--%>
            <div class="linha">
                <asp:Label ID="lblValor" runat="server" Text="Valor" CssClass="label_principal" />
                <div class="container_campo">
                    <componente:ValorDecimal ID="txtValor" runat="server" ValidationGroup="SalvarParcela" />
                </div>
            </div>
            <%--FIM Valor--%>

            <div class="linha">
                <asp:Label ID="lblJuros" runat="server" Text="Juros" CssClass="label_principal" ></asp:Label>
                <div class="container_campo">
                    <asp:CheckBox ID="chkJuros" runat="server" />
                </div>
            </div>



            <%--Email Envio Nota --%>
                
            <div class="linha">
                <asp:Label runat="server" Text="Email Envio da Nota" CssClass="label_principal"></asp:Label>
                <div class="container_campo">
                    <componente:AlfaNumerico runat="server" ID="txtEmailBoleto" ValidationGroup="SalvarParcela" MaxLength="50" Obrigatorio="False" CssClassTextBox="textbox_padrao"/>
                </div>
            </div>

            <%--Numero da Nota--%>
            <div class="linha">
                <asp:Label ID="lblNumeroNota" runat="server" Text="Número da Nota" CssClass="label_principal" />
                <div class="container_campo">
                    <componente:AlfaNumerico ID="txtNumeroNota" runat="server" ValidationGroup="SalvarParcela" MaxLength="10" Obrigatorio="False" CssClassTextBox="textbox_padrao" />
                </div>
            </div>
            <%--FIM Numero da Nota--%>
            <%-- Ordem de Compra --%>
            <div class="linha">
                <asp:Label ID="lblOrdemDeCompra" runat="server" Text="Ordem de Compra" CssClass="label_principal" />
                <div class="container_campo">
                    <componente:AlfaNumerico ID="txtOrdemDeCompra" runat="server" ValidationGroup="SalvarParcela" MaxLength="50" Obrigatorio="False" CssClassTextBox="textbox_padrao" />
                </div>
            </div>
            <%--Fim Ordem de Compra --%>
            <%--Qtde SMS e liberar total de SMS--%>
            <asp:Panel ID="pnlQtdeSMS" runat="server">
                <div class="linha">
                    <asp:Label ID="lblQtdeSMSTotal" runat="server" Text="Qtde. SMS" CssClass="label_principal" />
                    <div class="container_campo">
                        <componente:AlfaNumerico ID="txtQtdeSMSTotal" runat="server"
                            ValidationGroup="SalvarParcela" ClientIDMode="Predictable" Columns="4"
                            MaxLength="4" Tipo="Numerico" Obrigatorio="False"
                            CssClassTextBox="textbox_padrao" />
                        &nbsp;&nbsp;
                    </div>
                </div>
                <div class="linha">
                    <asp:Label ID="lblQtdeSMSLiberada" runat="server" Text="Qtde. SMS Liberada" CssClass="label_principal" />
                    <div class="container_campo">
                        <componente:AlfaNumerico ID="txtQtdeSMSLiberada" runat="server" ValidationGroup="SalvarParcela" ClientIDMode="Predictable" Columns="4"
                            MaxLength="4" Tipo="Numerico" Obrigatorio="False" CssClassTextBox="textbox_padrao" />
                        &nbsp;&nbsp;
                        <asp:CheckBox ID="chkLiberarSMS" runat="server" Text="Liberar saldo do plano" AutoPostBack="true"
                            TextAlign="Right" OnCheckedChanged="chkLiberarSMS_CheckedChanged" />
                    </div>
                </div>
            </asp:Panel>
            <%--FIM Qtde SMS e liberar total de SMS--%>
        </asp:Panel>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnSalvar" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>
<%-- Botões --%>
<asp:Panel ID="Panel1" runat="server" CssClass="painel_botoes">
    <asp:Button ID="btnSalvar" runat="server" CssClass="botao_padrao" Text="Salvar" CausesValidation="true"
        ValidationGroup="SalvarParcela" OnClick="btnSalvar_Click" />
</asp:Panel>
<%-- Fim Botões --%>
<%--Parcelas do Plano--%>
<h3>Parcela Plano</h3>
<asp:UpdatePanel ID="upGvParcelas" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <telerik:RadGrid ID="gvParcelas" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao"
            runat="server" Skin="Office2007" GridLines="None" OnItemCommand="gvParcelas_ItemCommand" OnPageIndexChanged="gvParcelas_PageIndexChanged">
            <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
            <AlternatingItemStyle CssClass="alt_row" />
            <MasterTableView DataKeyNames="Idf_Pagamento, Parcela, Des_Identificador">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Número" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblNumero" runat="server" Text='<%# Eval("Des_Identificador") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Número da Nota" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblNumeroNota" runat="server" Text='<%# Eval("Num_Nota_Fiscal") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Enviado em" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblDtaEmissao" runat="server" Text='<%# Convert.ToDateTime(Eval("Dta_Emissao")).ToString("dd/MM/yyyy") %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vencimento" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblDtaVencimento" runat="server" Text='<%# Convert.ToDateTime(Eval("Dta_Vencimento")).ToString("dd/MM/yyyy") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Valor" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblValor" runat="server" Text='<%# Eval("Vlr_Pagamento") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Qtde SMS" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblSMSTotal" runat="server" Text='<%# Eval("Qtd_SMS_Total") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Qtde SMS Liberada" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblSMSLiberada" runat="server" Text='<%# Eval("Qtd_SMS_Liberada") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Situação" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblSituacao" runat="server" Text='<%# Eval("Des_Pagamento_Situacao") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Enviado para" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblEnviadoPara" runat="server" ToolTip='<%# Eval("Eml_Envio_Boleto") %>' Text='<%# AjustarTamanhoEmail(DataBinder.Eval(Container.DataItem, "Eml_Envio_Boleto").ToString()) %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Tipo" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblTipo" runat="server" Text='<%# Eval("Des_Tipo_Pagamaneto") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="col_action tab_tipoplano">
                        <ItemTemplate>
                            <%-- É mostrado apenas se a parcela está em aberto (pendente) --%>
                            <asp:ImageButton ID="btiEditarParcela" runat="server" ImageUrl="../../../img/icn_editar_lapis.png" ToolTip="Editar Parcela"
                                AlternateText="Editar Parcela" CommandName="Editar" Visible='<%# AjustarVisibilidadeEditarParcela(Convert.ToInt32(Eval("Idf_Pagamento_Situacao")),Convert.ToBoolean(Eval("Cortesia"))) %>'
                                CausesValidation="false" />
                            <%-- É mostrado apenas se for boleto --%>
                            <asp:ImageButton ID="btiVisualizarBoleto" runat="server" ImageUrl="../../../img/icn_visualizarcurriculo.png"
                                ToolTip="Visualizar Boleto" AlternateText="Visualizar Boleto" CommandName="Visualizar" Visible='<%#  
                                            !String.IsNullOrEmpty(Eval("Idf_Tipo_Pagamento").ToString()) ?                                                                
                                            AjustarVisibilidadeVisualizarBoleto(Convert.ToInt32(Eval("Idf_Tipo_Pagamento")),Convert.ToBoolean(Eval("Cortesia")),Convert.ToInt32(Eval("Idf_Pagamento_Situacao")))  :
                                            AjustarVisibilidadeVisualizarBoleto(0,Convert.ToBoolean(Eval("Cortesia")),Convert.ToInt32(Eval("Idf_Pagamento_Situacao"))) 
                                            %>'
                                CausesValidation="false" />
                            <%-- É mostrado apenas se a parcela está em aberto (pendente) --%>
                            <asp:ImageButton ID="imgEmitirNF" runat="server" ToolTip="Emitir Nota Fiscal" AlternateText="Emitir Nota Fiscal" Visible='<%# AjustarVisibilidadeEditarParcela(Convert.ToInt32(Eval("Idf_Pagamento_Situacao")),Convert.ToBoolean(Eval("Cortesia"))) %>' CausesValidation="false" CommandName="EmitirNF" ImageUrl="~/img/icone-notafiscal.png" Width="15px" Height="15px" />

                            <asp:ImageButton ID="imgEstorno" runat="server" ImageUrl="../../../img/icn_reversal.png" ToolTip="Editar Estorno"
                                AlternateText="Estorno" CommandName="Estornar" Visible='<%# AjustarVisibilidadeEstorno(Convert.ToInt32(Eval("Idf_Tipo_Pagamento")), Convert.ToInt32(Eval("Idf_Pagamento_Situacao"))) %>'
                                CausesValidation="false" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </ContentTemplate>
</asp:UpdatePanel>
<%--FIM: Parcelas do Plano--%>
<%--Parcelas Adicionais--%>
<h3>Parcela Adicional</h3>
<asp:UpdatePanel ID="upGvParcelasAdicional" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <telerik:RadGrid ID="gvParcelasAdicional" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao"
            runat="server" Skin="Office2007" GridLines="None" OnItemCommand="gvParcelasAdicional_ItemCommand"
            OnPageIndexChanged="gvParcelasAdicional_PageIndexChanged">
            <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
            <AlternatingItemStyle CssClass="alt_row" />
            <MasterTableView DataKeyNames="Idf_Pagamento">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Número" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblNumero" runat="server" Text='<%# Eval("Des_Identificador") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Número da Nota" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblNumeroNota" runat="server" Text='<%# Eval("Num_Nota_Fiscal") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Enviado em" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblDtaEmissao" runat="server" Text='<%# Convert.ToDateTime(Eval("Dta_Emissao")).ToString("dd/MM/yyyy") %>'>
                            </asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vencimento" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblDtaVencimento" runat="server" Text='<%# Convert.ToDateTime(Eval("Dta_Vencimento")).ToString("dd/MM/yyyy") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Valor" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblValor" runat="server" Text='<%# Eval("Vlr_Pagamento") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Situação" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblSituacao" runat="server" Text='<%# Eval("Des_Pagamento_Situacao") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Tipo" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblTipo" runat="server" Text='<%# Eval("Des_Tipo_Pagamaneto") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="col_action tab_tipoplano">
                        <ItemTemplate>
                            <%-- É mostrado apenas se a parcela está em aberto (pendente) --%>
                            <asp:ImageButton ID="btiEditarParcela" runat="server" ImageUrl="../../../img/icn_editar_lapis.png" ToolTip="Editar Parcela"
                                AlternateText="Editar Parcela" CommandName="Editar" CausesValidation="false" Visible='<%# AjustarVisibilidadeEditarParcela(Convert.ToInt32(Eval("Idf_Pagamento_Situacao")),false) %>' />
                            <%-- É mostrado apenas se for boleto --%>
                            <asp:ImageButton ID="btiVisualizarBoleto" runat="server" ImageUrl="../../../img/icn_visualizarcurriculo.png"
                                ToolTip="Visualizar Boleto" AlternateText="Visualizar Boleto" CommandName="Visualizar" Visible='<%#  
                                            !String.IsNullOrEmpty(Eval("Idf_Tipo_Pagamento").ToString()) ?                                                                
                                            AjustarVisibilidadeVisualizarBoleto(Convert.ToInt32(Eval("Idf_Tipo_Pagamento")),false,Convert.ToInt32(Eval("Idf_Pagamento_Situacao")))  :
                                            AjustarVisibilidadeVisualizarBoleto(0,false,Convert.ToInt32(Eval("Idf_Pagamento_Situacao"))) 
                                            %>'
                                CausesValidation="false" />
                            <asp:ImageButton ID="imgEmitirNF" runat="server" ToolTip="Emitir Nota Fiscal" AlternateText="Emitir Nota Fiscal" Visible='<%# AjustarVisibilidadeEditarParcela(Convert.ToInt32(Eval("Idf_Pagamento_Situacao")),false) %>' CausesValidation="false" CommandName="EmitirNF" ImageUrl="~/img/icone-notafiscal.png" Width="15px" Height="15px" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
    </ContentTemplate>
</asp:UpdatePanel>
<%--FIM: Parcelas Adicionais--%>
<%-- Botões --%>
<asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
    <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
        OnClick="btnVoltar_Click" />
</asp:Panel>
<%-- Fim Botões --%>
<%--Modal paraVisualização de Boleto--%>
<asp:Panel ID="pnlVisualizacaoBoleto" runat="server" CssClass="modal_conteudo reduzida" Style="display: none;">
    <h2 class="titulo_modal">
        <span>Visualização de Boleto</span>
    </h2>
    <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ImageButton CssClass="botao_fechar_modal" ID="btiFechar" ImageUrl="/img/botao_padrao_fechar.gif"
                runat="server" CausesValidation="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel CssClass="coluna_esquerda" ID="pnlColunaEsquerda" runat="server">
        <asp:Panel CssClass="painel_imagem" ID="pnlEsquerdaImagem" runat="server">
            <img style="margin-left:25px;" alt="" class="icone" src="../../../img/SalaSelecionadora/icn_meuplano.png" />
        </asp:Panel>
    </asp:Panel>
    <asp:Panel CssClass="coluna_direita" ID="pnlColunaDireita" runat="server">
        <asp:UpdatePanel ID="upDados" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <ul>
                    <li>
                        <asp:HyperLink ID="hlPDFNormal" runat="server" Text="PDF Normal" Target="_blank"></asp:HyperLink>
                    </li>
                    <li>
                        <asp:HyperLink ID="hlPDFCobranca" runat="server" Text="PDF Texto Cobrança" Target="_blank"></asp:HyperLink>
                    </li>
                </ul>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Panel>
<asp:HiddenField ID="hfVisualizacaoBoleto" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeVisualizacaoBoleto" runat="server" PopupControlID="pnlVisualizacaoBoleto"
    CancelControlID="btiFechar" TargetControlID="hfVisualizacaoBoleto">
</AjaxToolkit:ModalPopupExtender>
<%--Modal para Liberar Cliente--%>