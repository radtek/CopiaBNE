
<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="CadastroCurriculoCompleto.aspx.cs" Inherits="BNE.Web.CadastroCurriculoCompleto" %>

<%@ Register Src="UserControls/Forms/CadastroCurriculo/MiniCurriculo.ascx" TagName="MiniCurriculo"
    TagPrefix="uc1" %>
<%@ Register Src="UserControls/Forms/CadastroCurriculo/DadosPessoais.ascx" TagName="DadosPessoais"
    TagPrefix="uc2" %>
<%@ Register Src="UserControls/Forms/CadastroCurriculo/FormacaoCursos.ascx" TagName="FormacaoCursos"
    TagPrefix="uc3" %>
<%@ Register Src="UserControls/Forms/CadastroCurriculo/DadosComplementares.ascx"
    TagName="DadosComplementares" TagPrefix="uc4" %>
<%@ Register Src="UserControls/Modais/ModalConfirmacao.ascx" TagName="ModalConfirmacao"
    TagPrefix="uc5" %>
<%@ Register Src="UserControls/ucObservacaoCurriculo.ascx" TagName="ObservacaoCurriculo"
    TagPrefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/cadastro_curriculo_mini.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:Panel ID="pnlInformacoesCurriculo" runat="server">
        <div class="linha">
            <asp:Label ID="lblCodigo" runat="server" Text="Código" AssociatedControlID="litCodigo"
                Style="font-weight: bold; font-size: 11px;"></asp:Label>
            <label class="container_campo" style="font-size: 11px;">
                <asp:Literal ID="litCodigo" runat="server"></asp:Literal></label>
            <div class="linha">
            </div>
            <asp:Label ID="lblCadastradoEm" runat="server" Text="Cadastrado Em" AssociatedControlID="litCadastradoEm"
                Style="font-weight: bold; font-size: 11px;"></asp:Label>
            <label class="container_campo" style="font-size: 11px;">
                <asp:Literal ID="litCadastradoEm" runat="server"></asp:Literal></label>
        </div>
    </asp:Panel>
    <h1>
        Críticas</h1>
    <asp:Panel ID="pnlCriticasCurriculo" runat="server">
        <div class="painel_padrao">
            <div class="painel_padrao_topo">
                &nbsp;
            </div>
            <!--GridView Crítica-->
            <telerik:RadGrid ID="gvCritica" runat="server" AllowSorting="false" AllowPaging="false"
                AllowCustomPaging="true">
                <MasterTableView TableLayout="Fixed" OverrideDataSourceControlSorting="true" AutoGenerateColumns="False">
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Campo" HeaderStyle-CssClass="rgHeader centro "
                            DataField="Des_Campo_Erro" />
                        <telerik:GridBoundColumn HeaderText="Crítica" HeaderStyle-CssClass="rgHeader centro"
                            DataField="Des_Critica" />
                        <telerik:GridBoundColumn HeaderText="Erro" HeaderStyle-CssClass="rgHeader centro"
                            DataField="Des_Erro" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <!--FIM: Gridview Crítica -->
        </div>
    </asp:Panel>
    <uc1:MiniCurriculo ID="ucMiniCurriculo" runat="server" />
    <uc2:DadosPessoais ID="ucDadosPessoais" runat="server" />
    <uc3:FormacaoCursos ID="ucFormacaoCursos" runat="server" />
    <uc4:DadosComplementares ID="ucDadosComplementares" runat="server" />
    <uc6:ObservacaoCurriculo ID="ucObservacaoCurriculo" runat="server" />
    <asp:Panel runat="server" CssClass="painel_botoes" ID="pnlBotoes">
        <asp:Panel runat="server" ID="pnlSituacaoCurriculo" Visible="false">
            <p>
                &nbsp;<telerik:RadComboBox ID="rcbSituacaoCurriculo" CssClass="" runat="server">
                </telerik:RadComboBox>
                <p>
                </p>
            </p>
        </asp:Panel>
        <asp:Button runat="server" ID="btnSalvarCurriculo" Text="Salvar" CssClass="botao_padrao"
            CausesValidation="false" OnClick="btnSalvarCurriculo_Click" />
        <asp:Button runat="server" ID="btnVoltar" Text="Voltar" CssClass="botao_padrao" CausesValidation="false"
            OnClick="btnVoltar_Click" />
    </asp:Panel>
    <uc5:ModalConfirmacao ID="ucModalConfirmacao" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
