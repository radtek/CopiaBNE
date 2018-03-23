<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BoletoAvulsoPJ.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.BoletoAvulsoPJ" %>


<asp:Panel ID="idPesquisarCliente" runat="server" CssClass="blocodados">
    <h3>
        Pesquisar Cliente</h3>
    <p>
        Encontre o cliente desejado utilizando, CNPJ, CPF, Nome, Razão Social, Telefone, Número do Boleto, Endereço ou E-mail
    </p>
    <div>
        <asp:Label ID="Label1" runat="server" CssClass="label_principal"></asp:Label>
        <telerik:RadTextBox runat="server" ID="tbxFiltroBusca" TextMode="SingleLine" ValidationGroup="Filtrar"
            EmptyMessage="" CssClass="textbox_padrao_pesquisa">
        </telerik:RadTextBox>
        <asp:Button ID="btnFiltrar" runat="server" CssClass="botao_padrao filtrar" Text="Pesquisar"
            ToolTip="Filtrar Currículos" CausesValidation="True" ValidationGroup="Filtrar" />
    </div>
</asp:Panel>

<asp:Panel ID="idDadosCliente" runat="server" CssClass="blocodados">
    <h3>
        Dados do Cliente</h3>
    <%--Linha Nome Completo----%>
    <div class="linha">
        <asp:Label ID="lblRazaoSocial" runat="server" Text="Razão Social" CssClass="label_principal" />
        <span class="container_campo dados_empresa">Employer Organização de Recursos Humanos</span>
    </div>
    <%--FIM Razão Social--%>
    <%--Linha CPF----%>
    <div class="linha">
        <asp:Label ID="lblNomeFantasiaouApelido" runat="server" Text="Nome Fantasia ou Apelido" CssClass="label_principal" />
        <span class="container_campo dados_empresa">Employer RH</span>
    </div>
    <%--FIM CPF--%>
    <%--Linha Data de Nascimento----%>
    <div class="linha">
        <asp:Label ID="lblCNPJ" runat="server" Text="CNPJ" CssClass="label_principal" />
        <span class="container_campo dados_empresa">82.344.425/0001-82</span>
    </div>
    <%--FIM Data de Nascimento--%>
    <%--Linha e-mail----%>
    <div class="linha">
        <asp:Label ID="lblCidade" runat="server" Text="Cidade" CssClass="label_principal" />
        <span class="container_campo dados_empresa">Curitiba</span>
    </div>
    <%--FIM e-mail--%>
    <%--Linha Último Plano----%>
    <div class="linha">
        <asp:Label ID="lblUltimoPlanoAdquirido" runat="server" Text="Último Plano Adquirido" CssClass="label_principal" />
        <span class="container_campo dados_empresa">Ilimitado - 30 dias</span>
    </div>
    <%--FIM Último Plano--%> 
    <%--Linha Adquirido----%>
    <div class="linha">
        <asp:Label ID="lblValidade" runat="server" Text="Validade" CssClass="label_principal" />
        <span class="container_campo dados_empresa">05/01/2010</span>
    </div>
    <%--FIM Adquirido--%>
        <%--Linha Validade----%>
    <div class="linha">
        <asp:Label ID="lblBoleto" runat="server" Text="Boleto" CssClass="label_principal" />
        <span class="container_campo dados_empresa">Pago</span>
    </div>
    <%--FIM Nome Fantasia--%>

        
        <%--Linha Liberar Cliente--%>
    <div class="linha liberar_cliente">
       <span><a href="#">Liberar Cliente</a></span>
    </div>
    <%--FIM Liberar Cliente --%>
    
    </asp:Panel>

<asp:Panel ID="idPlanoFidelidade" runat="server" CssClass="blocodados">
    <h3>
       Gerar Boleto Avulso</h3>
    <%--Linha Tipo do Plano --%>
    <div class="linha">
        <asp:Label ID="lblTipoPlano" runat="server" Text="Tipo de Plano" CssClass="label_principal modal" />
        <div class="container_campo">
            <Employer:ComboCheckbox runat="server" ID="ccFiltrarTipoPlano" EmptyMessage=" " AllowCustomText="false"
                CssClass="checkbox_large  ">
            </Employer:ComboCheckbox>
        </div>
    </div>
    <%--FIM Tipo do Plano --%>
    <%--Linha Data Envio Boleto--%>
    <div class="linha">
        <asp:Label ID="lblDataEnvio" runat="server" Text="Data de Envio"
            CssClass="label_principal" />
        <div class="container_campo">
            <asp:TextBox ID="txtDataEnvio" runat="server" />
        </div>
    </div>
    <%--FIM Data Envio Boleto --%>
    <%--Linha Data de Vencimento do Boleto--%>
    <div class="linha">
        <asp:Label ID="lblDataVencimento" runat="server" Text="Data de Vencimento"
            CssClass="label_principal" />
        <div class="container_campo">
            <asp:TextBox ID="txtDataVencimento" runat="server" />
        </div>
    </div>
    <%--FIM Data de Vencimento do Boleto --%>
    <%--Linha Enviar Para--%>
    <div class="linha">
        <asp:Label ID="lblEnviarPara" runat="server" Text="Enviar Para" CssClass="label_principal" />
        <div class="container_campo">
            <asp:TextBox ID="txtEnviarPara" runat="server" />
        </div>
    </div>
    <%--FIM  Enviar Para --%>
         <%--Linha Enviar Nota Fiscal--%>
    <div class="linha">
        <asp:Label ID="lblEnviarNF" runat="server" Text="Enviar Nota Fiscal" CssClass="label_principal" />
        <div class="container_campo">
            <asp:RadioButton ID="rbSim" runat="server" />
            <span>Sim</span>
            <asp:RadioButton ID="rbNao" runat="server" />
            <span>Não</span>
        </div>
    </div>
    <%--FIM Enviar Nota Fiscal --%>

    <%--Linha Desconto Oferecido--%>
    <div class="linha">
        <asp:Label ID="lblDescontoOferecido" runat="server" Text="Desconto Oferecido" CssClass="label_principal" />
        <div class="container_campo">
            <asp:TextBox ID="txtDescontoOferecido" runat="server" />
        </div>
    </div>
    <%--FIM Desconto Oferecido --%>



   </asp:Panel>

   <%-- Botões --%>
<asp:Panel ID="Panel1" runat="server" CssClass="painel_botoes">
    <asp:Button ID="btnEnviat" runat="server" CssClass="botao_padrao" Text="Enviar" CausesValidation="false" />
</asp:Panel>
<%-- Fim Botões --%>



<asp:UpdatePanel ID="upBoletoAvulsoPJ" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
            <telerik:RadGrid ID="gvBoletoAvulsoPJ" AllowPaging="True" AllowCustomPaging="true"
                CssClass="gridview_padrao" runat="server" Skin="Office2007" GridLines="None"
                OnPageIndexChanged="gvBoletoAvulsoPJ_PageIndexChanged">
                <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                <AlternatingItemStyle CssClass="alt_row" />
                <MasterTableView>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Nosso Número" ItemStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:Label ID="lblNossoNumero" runat="server" Text='<%# Eval("Des_NossoNumero") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Enviado em" ItemStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:Label ID="lblDtaNascimento" runat="server" Text='<%# Eval("Dta_Enviado") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vencimento" ItemStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:Label ID="lblDtaValidade" runat="server" Text='<%# Eval("Dta_Vencimento") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Valor" ItemStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:Label ID="lblValor" runat="server" Text='<%# Eval("Vlr_Parcela") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Situação" ItemStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:Label ID="lblSituacao" runat="server" Text='<%# Eval("Nme_Situacao") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Enviado para" ItemStyle-CssClass="center">
                            <ItemTemplate>
                                <asp:Label ID="lblEnviadoPara" runat="server" Text='<%# Eval("Nme_Enviado") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="col_action tab_boleto_avulso_pj">
                            <ItemTemplate>
                                <asp:ImageButton ID="btiVisualizarCurriculo" runat="server" ImageUrl="../../../img/icn_visualizarcurriculo.png"
                                    ToolTip="Visualizar Currículo" AlternateText="Visualizar Currículo" CausesValidation="false" />
                                <asp:ImageButton ID="btiEditarVaga" runat="server" ImageUrl="../../../img/icn_editar_lapis.png"
                                    ToolTip="Editar Vagas" AlternateText="Editar Vaga" CausesValidation="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>

    </ContentTemplate>
</asp:UpdatePanel>