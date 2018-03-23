<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LiberacaoBoleto.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.LiberacaoBoleto" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<Employer:DynamicScript runat="server" Src="/js/local/UserControls/PlanoFidelidade.js" type="text/javascript" />
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/LiberacaoBoleto.css" type="text/css" rel="stylesheet" />
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/PlanoFidelidade.css" type="text/css" rel="stylesheet" />
<h2>Liberação de Boleto</h2>
<asp:Panel ID="idLiberarBoleto" runat="server" CssClass="coluna_direita">
    <h3>Pesquisar Boletos</h3>
    <%--<p> Enviar o arquivo CNR com os boletos pagos. </p>--%>
    <asp:UpdatePanel ID="upAnexo" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="pesquisarBoletos">
                <asp:Label ID="lblDataArquivo" runat="server" Text="Data Arquivo: "></asp:Label>
                <asp:TextBox ID="txtDataArquivo" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="calendar" runat="server" TargetControlID="txtDataArquivo"></asp:CalendarExtender>
                <br />
                <asp:Label ID="lblTipoPlano" runat="server" Text="Tipo de Plano: "></asp:Label>
                <telerik:RadComboBox runat="server" ID="rcbPlano" EmptyMessage="" AllowCustomText="false" CssClass="rcbCombo" OnSelectedIndexChanged="rcbPlano_SelectedIndexChanged" AutoPostBack="true"></telerik:RadComboBox>
                <br />
                <asp:Label ID="lblTipoCliente" runat="server" Text="Tipo Cliente: "></asp:Label>
                <asp:RadioButton ID="rbTodos" AutoPostBack="true" runat="server" GroupName="PesquisarBoletos" Checked="true" />&nbsp;<asp:Label ID="Label1" runat="server" Text="Todos"></asp:Label>&nbsp;
            <asp:RadioButton ID="rbPF" AutoPostBack="true" runat="server" GroupName="PesquisarBoletos" OnCheckedChanged="rbPF_CheckedChanged" />&nbsp;<asp:Label ID="Label2" runat="server" Text="Pessoa Física"></asp:Label>&nbsp;
            <asp:RadioButton ID="rbPJ" AutoPostBack="true" runat="server" GroupName="PesquisarBoletos" OnCheckedChanged="rbPJ_CheckedChanged" />&nbsp;<asp:Label ID="Label3" runat="server" Text="Pessoa Jurídica"></asp:Label>
            </div>
            <%--<div class="linha">
            <asp:FileUpload ID="fAnexo" runat="server"/>
        </div>--%>
            <div class="botoes">
                <asp:Button ID="btPesquisar" runat="server" Text="Pesquisar" OnClick="btPesquisar_Click" />
                <asp:Button ID="btAdicionarArquivo" runat="server" Text="Adicionar Arquivo" OnClick="btAdicionarArquivo_Click" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btPesquisar" />
            <asp:PostBackTrigger ControlID="btEnviar" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
</asp:Panel>
<%----%>
<asp:UpdatePanel ID="upLiberacaoBoleto" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <telerik:RadGrid ID="gvLiberacaoBoletos" CssClas="gridview_padrao" runat="server"
            Skin="Office2007" GridLines="None" AllowPaging="True" PageSize="15" OnNeedDataSource="gvLiberacaoBoletos_NeedDataSource" OnItemCommand="gvLiberacaoBoletos_ItemCommand" OnItemDataBound="gvLiberacaoBoletos_ItemDataBound">
            <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
            <AlternatingItemStyle CssClass="alt_row" />
            <MasterTableView DataKeyNames="IdPagamento">
                <Columns>
                    <telerik:GridTemplateColumn ItemStyle-CssClass="center">
                        <HeaderTemplate>
                            <asp:CheckBox ID="cbHeaderItem" runat="server" AutoPostBack="true" OnCheckedChanged="cbHeaderItem_CheckedChanged" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="cbDataItem" runat="server" AutoPostBack="true" OnCheckedChanged="cbDataItem_CheckedChanged" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Plano" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblNomePlano" runat="server" Text='<%# Eval("Plano") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Nome/Razao Social" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblNome" runat="server" Text='<%# Eval("Nome") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="CPF/CNPJ" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblNumDoc" runat="server" Text='<%# Eval("NumeroDocumento") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <%--<telerik:GridTemplateColumn HeaderText="Cidade" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblCidade" runat="server" Text='<%# Eval("Cidade") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>
                    <telerik:GridTemplateColumn HeaderText="N. Boleto" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblNumBoleto" runat="server" Text='<%# Eval("NumeroBoleto") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vlr. Plano" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblVlrPlano" runat="server" Text='<%# Eval("ValorPlano") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <%--<telerik:GridTemplateColumn HeaderText="Vlr. Boleto" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblVlrBoleto" runat="server" Text='<%# Eval("Vlr Boleto") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vlr. Pago" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblVlrPago" runat="server" Text='<%# Eval("Vlr Pago") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>
                    <telerik:GridTemplateColumn HeaderText="Parcela" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblParcela" runat="server" Text='<%# Eval("Parcela") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <%-- <telerik:GridTemplateColumn HeaderText="Situação" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblSituacao" runat="server" Text='<%# Eval("Situacao") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>
                    <%--<telerik:GridTemplateColumn HeaderText="CR. Baixado" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblCrBaixado" runat="server" Text='<%# Eval("Cr Baixado") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>
                    <telerik:GridTemplateColumn HeaderText="Nota Fiscal" ItemStyle-CssClass="center">
                        <ItemTemplate>
                            <asp:Label ID="lblNF" runat="server" Text='<%# Eval("NotaFiscal") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Ações">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgInformacoes" runat="server" ToolTip="Mais informações" AlternateText="Mais Informações" CausesValidation="false" CommandName="MaisInformacoes" ImageUrl="~/img/btn_mais_informacoes.png" Width="15px" Height="15px" />
                            <asp:ImageButton ID="imgEmitirNF" runat="server" ToolTip="Emitir Nota Fiscal" AlternateText="Emitir Nota Fiscal" CausesValidation="false" CommandName="EmitirNF" ImageUrl="~/img/icone-notafiscal.png" Width="15px" Height="15px" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
            <HeaderContextMenu EnableAutoScroll="true"></HeaderContextMenu>
        </telerik:RadGrid>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:Panel runat="server" ID="pnlBotoes" CssClass="painel_botoes" Visible="false">
    <asp:Button ID="btEmitirTodasNF" runat="server" Text="Emitir Todas" OnClick="btEmitirTodasNF_Click" />
</asp:Panel>


<!-- Modal Anexar arquivo CNR -->

<asp:HiddenField ID="hidModalAnexarArquivo" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeModalAnexarArquivo" PopupControlID="pnlModalAnexarArquivo" runat="server" TargetControlID="hidModalAnexarArquivo"></AjaxToolkit:ModalPopupExtender>
<asp:Panel ID="pnlModalAnexarArquivo" runat="server" CssClass="pnlModalArquivo" Style="display: none">
    <asp:Panel CssClass="pnlModalArquivo_fechar" ID="pnlFechar" runat="server">
        <asp:ImageButton ImageUrl="~/img/botao_padrao_fechar_escuro.png" ID="btFechar" runat="server" OnClick="btFechar_Click" />
    </asp:Panel>
    <asp:Panel CssClass="pnlModalArquivo_topo" runat="server" ID="pnlTopo">
        <h3>Adicionar Novo Arquivo</h3>
    </asp:Panel>
    <asp:Panel CssClass="pnlModalArquivo_conteudo" runat="server" ID="pnlConteudo">
        Arquivo:
        <asp:FileUpload ID="fAnexo" runat="server" />
    </asp:Panel>
    <asp:Panel ID="pnlModalArquivo_botoes" runat="server" CssClass="pnlModalArquivo_botoes">
        <asp:Button ID="btEnviar" runat="server" Text="Enviar" OnClick="btEnviar_Click" />
        <asp:Button ID="btVoltar" runat="server" Text="Voltar" OnClick="btVoltar_Click" />
    </asp:Panel>
</asp:Panel>

<!-- Modal Mais Informações -->
<asp:HiddenField ID="hidModalMaisInformacoes" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeModalMaisInformacoes" PopupControlID="pnlMaisInformacoes" runat="server" TargetControlID="hidModalMaisInformacoes"></AjaxToolkit:ModalPopupExtender>
<asp:Panel ID="pnlMaisInformacoes" runat="server" CssClass="pnlModalMaisInformacoes" Style="display: none">
    <asp:UpdatePanel ID="upMaisInformacoes" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:Panel CssClass="pnlModalMaisInformacoes_fechar" ID="pnlFecharMaisInformacoes" runat="server">
                <asp:ImageButton ImageUrl="~/img/botao_padrao_fechar_escuro.png" ID="btFecharMaisInformacoes" runat="server" OnClick="btFecharMaisInformacoes_Click" />
            </asp:Panel>
            <asp:Panel ID="pnlTopoMaisInformacoes" runat="server" CssClass="pnlModalMaisInformacoes_Topo">
                <h3>Detalhe de Liberação do Boleto</h3>
            </asp:Panel>
            <asp:Panel ID="pnlConteudoMaisInformacoes" runat="server" CssClass="pnlModalMaisInformacoes_conteudo">
                <asp:Panel ID="pnlInformacoes" runat="server" CssClass="pnlModalMaisInformacoes_Info">
                    <asp:Panel ID="pnlInfoColunaUm" runat="server" CssClass="pnlModalMaisInformacoes_Info_Coluna_Um">
                        <asp:Label ID="lblTipoPlanoInfo" runat="server" Text="Tipo de Plano"></asp:Label>
                        <asp:TextBox ID="txtTipoPlanoInfo" runat="server" CssClass="textbox_padrao" Style="margin-left: 35px"></asp:TextBox>
                        <br />
                        <asp:Label ID="lblNome" runat="server" Text="Nome/Razao Social"></asp:Label>
                        <asp:TextBox ID="txtNome" runat="server" CssClass="textbox_padrao" Style="margin-left: 2px"></asp:TextBox>
                        <br />
                        <asp:Label ID="lblCPF" runat="server" Text="CPF/CNPJ"></asp:Label>
                        <asp:TextBox ID="txtCPF" runat="server" CssClass="textbox_padrao" Style="margin-left: 52px"></asp:TextBox>
                    </asp:Panel>
                    <asp:Panel ID="pnlInfoColunaDois" runat="server" CssClass="pnlModalMaisInformacoes_Info_Coluna_Dois">
                        <asp:Label ID="lblNumBoleto" runat="server" Text="N. Boleto"></asp:Label>
                        <asp:TextBox ID="txtNumBoleto" runat="server" CssClass="textbox_padrao" Style="margin-left: 14px"></asp:TextBox>
                        <br />
                        <asp:Label ID="lblNF" runat="server" Text="Nota Fiscal"></asp:Label>
                        <asp:TextBox ID="txtNF" runat="server" CssClass="textbox_padrao" Style="margin-left: 2px"></asp:TextBox>
                        <br />
                        <asp:Label ID="lblSituacao" runat="server" Text="Situação"></asp:Label>
                        <asp:TextBox ID="txtSituacao" runat="server" CssClass="textbox_padrao" Style="margin-left: 16px"></asp:TextBox>
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="pnlEventos" runat="server" CssClass="pnlModalMaisInformacoes_eventos" BorderColor="#c0c0c0" BorderWidth="1px">
                <asp:Panel ID="pnlTexto" runat="server" CssClass="pnlTexto">
                    <asp:Label ID="lblInfoUm" runat="server" />
                    <br />
                    <asp:Label ID="lblInfoDois" runat="server"></asp:Label>
                    <br />
                    <asp:Label ID="lblInfoTres" runat="server" />
                    <br />
                    <asp:Label ID="lblInfoQuatro" runat="server" />
                    <br />
                    <asp:Label ID="lblInfoCinco" runat="server" />
                    <br />
                    <asp:Label ID="lblInfoSeis" runat="server" />
                    <br />
                    <asp:Label ID="lblInfoSete" runat="server" />
                    <br />
                    <asp:Label ID="lblInfoOito" runat="server" />
                    <br />
                    <br />
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="pnlModalMaisInformacoes_botoes" runat="server" CssClass="pnlModalMaisInformacoes_botoes">
                <asp:Button ID="btVoltarMaisInfo" runat="server" Text="Voltar" OnClick="btVoltarMaisInfo_Click" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
