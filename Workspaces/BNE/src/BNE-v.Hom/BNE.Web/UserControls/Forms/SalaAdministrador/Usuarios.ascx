<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Usuarios.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.Usuarios" %>
<%@ Register Src="/UserControls/Modais/ConfirmacaoExclusao.ascx" TagName="ConfirmacaoExclusao"
    TagPrefix="uc" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Usuarios.css" type="text/css" rel="stylesheet" />
<div class="painel_configuracao_conteudo">
    <div class="titulo_filtro">
        <h2 class="titulo_carta_content">
            <asp:Label ID="lblTitulo" Text="Cadastro de Usuários" runat="server"></asp:Label>
        </h2>
    </div>

    <div id="DivFiltros" style="width: 100%; float: left; padding: 6px;">
        <div id="divCPF" class="linhaUsuarios" style="text-align: right; width: 170px; float: left;">
            <asp:Label runat="server" ID="Label4" Text="CPF" AssociatedControlID="txtCPFPesquisa"></asp:Label>
            <div class="container_campo">
                <componente:AlfaNumerico runat="server" ID="txtCPFPesquisa"  Tipo="AlfaNumerico" MaxLength="14" Obrigatorio="false" CssClassTextBox="textbox_padrao" Width="120px"/>
            </div>
        </div>
        <div id="divTiposPerfil" class="linhaUsuarios" style="width: auto; float: left;">
            <asp:Label ID="lblPerfil" runat="server" Text="Tipos de Perfil" AssociatedControlID="ccbTipo" />
            <div class="container_campo">
                <Employer:ComboCheckbox runat="server" ID="ccbTipo" EmptyMessage="Filtrar por tipo"
                    AllowCustomText="false" CssClass="checkbox_large" DropDownWidth="197">
                </Employer:ComboCheckbox>
            </div>
        </div>  
        <div id="divNome" class="linhaUsuarios" style="text-align: right; width: auto; float: left;">
            <asp:Label runat="server" ID="lblNomePesquisa" Text="Nome" AssociatedControlID="txtNomePesquisa"></asp:Label>
            <div class="container_campo">
                <componente:AlfaNumerico runat="server" ID="txtNomePesquisa" Obrigatorio="false" CssClassTextBox="textbox_padrao" />
            </div>
        </div>
        <div id="divStatus" class="linhaUsuarios margemButtonsStatus" style="text-align: right; float: left; position: relative; margin-left: 5px;">
            <asp:Label runat="server" ID="lblStatus" Text="Status" AssociatedControlID="rbLstStatusUsuarios"></asp:Label>
            <div class="container_campo ">
                <asp:RadioButtonList runat="server" ID="rbLstStatusUsuarios" RepeatDirection="Horizontal">
                    <asp:ListItem Value="0" Selected="True">Ativos</asp:ListItem>
                    <asp:ListItem Value="1">Inativos</asp:ListItem>
                    <asp:ListItem Value="2">Todos</asp:ListItem>
                </asp:RadioButtonList>
            </div>
        </div>              
    </div>

    <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes margemBotoes">
        <div style="margin-bottom: -5px">
            <asp:Button ID="btnFiltrar" runat="server" CssClass="botao_padrao filtrar"
                Text="Pesquisar" ToolTip="Filtrar" OnClick="btnFiltrar_Click" />
            <asp:Button ID="btnCadastro" runat="server" CssClass="botao_padrao filtrar margemBotoes"
                Text="Adicionar Usuário" CausesValidation="false" OnClick="btnCadastro_Click" />
            <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao filtrar margemBotoes" Text="Voltar"
                CausesValidation="false" OnClick="btnVoltar_Click" />
        </div>
    </asp:Panel>

    <asp:UpdatePanel ID="upGvUsuarios" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <telerik:RadGrid ID="gvUsuarios" AllowPaging="True" AllowCustomPaging="True" CssClass="gridview_padrao"
                runat="server" Skin="Office2007" GridLines="None" OnPageIndexChanged="gvUsuarios_PageIndexChanged"
                OnItemCommand="gvUsuarios_ItemCommand">
                <PagerStyle Mode="NextPrevNumericAndAdvanced" Position="TopAndBottom" />
                <AlternatingItemStyle CssClass="alt_row" />
                <MasterTableView DataKeyNames="Idf_Usuario_Filial_Perfil">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Nome">
                            <ItemTemplate>
                                <asp:Literal runat="server" Text='<%# Eval("Nme_Pessoa") %>'></asp:Literal>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CPF">
                            <ItemTemplate>
                                <asp:Literal runat="server" Text='<%# FormatarCPF(Eval("Num_CPF").ToString()) %>'></asp:Literal>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Dt de Nascimento">
                            <ItemTemplate>
                                <asp:Literal runat="server" Text='<%# Convert.ToDateTime(Eval("Dta_Nascimento")).ToShortDateString() %>'></asp:Literal>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tipo">
                            <ItemTemplate>
                                <asp:Literal runat="server" Text='<%# Eval("Des_Perfil") %>'></asp:Literal>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <ItemTemplate>
                                <asp:Literal runat="server" Text='<%# RetornarStatus(Eval("Flg_Inativo").ToString()) %>'></asp:Literal>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="col_action_arq">
                            <ItemTemplate>
                                <asp:ImageButton ID="btiEditar" runat="server" ImageUrl="../../../img/icn_editar_lapis.png"
                                    ToolTip="Editar Perfil" CommandName="EditarUsuario" AlternateText="Editar Perfil"
                                    Visible='<%# !Convert.ToBoolean(Eval("Flg_Inativo")) %>' CausesValidation="False" />
                                <asp:ImageButton ID="btiSenha" runat="server" ImageUrl="/img/icn_closed_p.png" ToolTip="Editar Senha"
                                    CommandName="EditarSenha" AlternateText="Editar Senha" Visible='<%# !Convert.ToBoolean(Eval("Flg_Inativo")) %>'
                                    CausesValidation="False" />
                                <asp:ImageButton ID="btiExcluir" runat="server" ImageUrl="../../../img/icn_excluirvaga.png"
                                    ToolTip="Excluir" CommandName="ExcluirUsuario" AlternateText="Excluir Usuário"
                                    Visible='<%# !Convert.ToBoolean(Eval("Flg_Inativo")) %>' CausesValidation="False" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSalvar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnFiltrar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>

</div>
<AjaxToolkit:ModalPopupExtender CancelControlID="btiModalCadastrarFechar" ID="mpeModalCadastrar"
    runat="server" PopupControlID="pnlModalCadastrar" TargetControlID="hfMC">
</AjaxToolkit:ModalPopupExtender>
<asp:HiddenField runat="server" ID="hfMC" />
<asp:Panel ID="pnlModalCadastrar" runat="server" CssClass="modal_conteudo candidato reduzida"
    Style="display: none">
    <h2 class="titulo_modal">
        <span>Adicionar Novo Usuário</span></h2>
    <asp:UpdatePanel ID="upBtiModalCadastrarFechar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ImageButton ID="btiModalCadastrarFechar" CssClass="botao_fechar_modal" ImageUrl="/img/botao_padrao_fechar.gif"
                runat="server" CausesValidation="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div>
        <div class="linha">
            <asp:UpdatePanel ID="upTxtCadastroCPF" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label runat="server" ID="lblCadastroCPF" CssClass="label_principal" Text="CPF"
                        AssociatedControlID="txtCadastroCPF" />
                    <div class="container_campo">
                        <componente:CPF ID="txtCadastroCPF" runat="server" ValidationGroup="Cadastro" />
                        <asp:Label runat="server" ID="lblCadastroValorCPF" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSalvar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnSalvarSenha" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="linha">
            <asp:UpdatePanel ID="upDdlCadastroTipoPerfil" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label runat="server" ID="lblCadastroTipoPerfil" CssClass="label_principal" Text="Tipo"
                        AssociatedControlID="ddlCadastroTipoPerfil" />
                    <div class="container_campo">
                        <asp:DropDownList ID="ddlCadastroTipoPerfil" runat="server" CssClass="textbox_padrao campo_obrigatorio">
                        </asp:DropDownList>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSalvar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnSalvarSenha" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="linha">
            <asp:UpdatePanel ID="upTxtCadastroSenha" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label runat="server" ID="lblCadastroSenha" CssClass="label_principal" Text="Senha"
                        AssociatedControlID="txtCadastroSenha" />
                    <div class="container_campo">
                        <componente:AlfaNumerico ID="txtCadastroSenha" runat="server" Columns="10" MaxLength="10"
                            ValidationGroup="Senha" Obrigatorio="True" TextMode="Password" Tipo="AlfaNumericoMaiusculo" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSalvar" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnSalvarSenha" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="painel_botoes">
        <asp:UpdatePanel ID="upBotoes" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Button ID="btnSalvar" runat="server" CssClass="botao_padrao painelinterno" Text="Salvar"
                    CausesValidation="True" ValidationGroup="Cadastro" OnClick="btnSalvar_Click" />
                <asp:Button ID="btnSalvarSenha" runat="server" CssClass="botao_padrao painelinterno"
                    Text="Salvar" CausesValidation="True" ValidationGroup="Senha" OnClick="btnSalvarSenha_Click" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Panel>
<uc:ConfirmacaoExclusao ID="ucConfirmacaoExclusao" runat="server" />
