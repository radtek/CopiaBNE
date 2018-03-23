<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EscolherEmpresa.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.SalaVip.EscolherEmpresa" %>
<%@ Register Src="../../Modais/VerDadosEmpresa.ascx" TagName="VerDadosEmpresa" TagPrefix="uc1" %>
<%@ Register Src="../../Modais/ModalConfirmacao.ascx" TagName="ModalConfirmacao"
    TagPrefix="uc2" %>
<%@ Register Src="../../Modais/IndicarEmpresa.ascx" TagName="IndicarEmpresa" TagPrefix="uc3" %>

<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/EscolherEmpresa.css" type="text/css" rel="stylesheet" />

<div class="painel_padrao_sala_vip">
    <p>
        Escolha onde você deseja trabalhar, as empresas serão avisadas pelo BNE do seu interesse.
    </p>
    <asp:UpdatePanel ID="upGvEscolherEmpresa" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <telerik:RadTextBox runat="server" ID="tbxFiltroBusca" Columns="72" TextMode="SingleLine"
                    ValidationGroup="Filtrar" EmptyMessage="Você pode pesquisar por nome, ramo de atividade ou cidade"
                    CssClass="textbox_filtro_busca">
                </telerik:RadTextBox>
                <asp:Button ID="btnFiltrar" runat="server" CssClass="botao_padrao filtrar" Text="Filtrar Empresas"
                    ToolTip="Filtrar Empresas" CausesValidation="True" ValidationGroup="Filtrar"
                    OnClick="btnFiltrar_Click" />
            </div>
            <telerik:RadGrid ID="gvEscolherEmpresa" AllowPaging="True" AllowCustomPaging="true"
                CssClass="gridview_padrao" runat="server" Skin="Office2007" GridLines="None"
                OnPageIndexChanged="gvEscolherEmpresa_PageIndexChanged" OnItemCommand="gvEscolherEmpresa_ItemCommand">
                <pagerstyle mode="NextPrevNumericAndAdvanced" position="TopAndBottom" />
                <alternatingitemstyle cssclass="alt_row" />
                <mastertableview datakeynames="Idf_Filial, Nme_Empresa">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Nome da Empresa" ItemStyle-CssClass="nome_empresa">
                            <ItemTemplate>
                                <asp:Label ID="lblNomeEmpresa" runat="server" Text='<%# Eval("Nme_Empresa") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nº Funcionários" ItemStyle-CssClass="nr_funcionarios">
                            <ItemTemplate>
                                <asp:Label ID="lblNumFuncionario" runat="server" Text='<%# Eval("Num_Funcionario") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ramo de Atividade" ItemStyle-CssClass="ramo_atividade">
                            <ItemTemplate>
                                <asp:Label ID="lblRamoDeAtividade" runat="server" Text='<%#  Eval("Des_Area_BNE")%>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cidade/UF" ItemStyle-CssClass="cidade_uf">
                            <ItemTemplate>
                                <asp:Label ID="lblNmeCidade" runat="server" Text='<%# String.IsNullOrEmpty(DataBinder.Eval(Container.DataItem, "Nme_Cidade").ToString()) ? "<i>Não Informado</i>" : Eval("Nme_Cidade") + "/" + Eval("Sig_Estado") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ação" ItemStyle-CssClass="col_action_arq">
                            <ItemTemplate>
                                <asp:LinkButton ID="btiCandidatar" runat="server" 
                                    ToolTip="Candidatar-se" CommandName="Candidatar"
                                    Visible='<%# !Convert.ToBoolean(Eval("Flg_Candidatou")) %>'>
                                    <i class="fa fa-file-text icon-"></i> 
                                    Candidatar
                                </asp:LinkButton> 
                                <span runat="server" id="spanJaEnviei" visible='<%# Convert.ToBoolean(Eval("Flg_Candidatou")) %>'>
                                    <i class="fa fa-check-circle"></i>
                                    Já Enviei!
                                </span>
                                <asp:LinkButton ID="btiBloquear" runat="server"
                                    ToolTip="Bloquear Empresa" CommandName="Bloquear">
                                    <i class="fa fa-ban"></i> Bloquear </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle ShowPagerText="true" Position="TopAndBottom" PagerTextFormat=" {4} Empresas {2} a {3} de {5}" />
                </mastertableview>
            </telerik:RadGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<%-- Botões --%>
<asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
    <asp:UpdatePanel ID="upNaoAcheiEmpresa" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <asp:Button ID="btnNaoAcheiEmpresa" runat="server" CssClass="botao_padrao" Text="Não Achei Empresa"
                CausesValidation="false" OnClick="btnNaoAcheiEmpresa_Click" />
            <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
                PostBackUrl="/SalaVip.aspx" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<%-- Fim Botões --%>


<%--INICIO - EMPRESAS BLOQUEADAS--%>
<div class="painel_padrao_sala_vip">
    <p>
        Empresas bloqueadas.
    </p>
    <asp:UpdatePanel ID="upGvEmpresaBloqueada" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <telerik:RadGrid ID="gvEmpresaBloqueada" AllowPaging="True" AllowCustomPaging="true"
                CssClass="gridview_padrao" runat="server" Skin="Office2007" GridLines="None"
                 OnItemCommand="gvEmpresaBloqueada_ItemCommand">
                <pagerstyle mode="NextPrevNumericAndAdvanced" position="TopAndBottom" />
                <alternatingitemstyle cssclass="alt_row" />
                <mastertableview datakeynames="Idf_Filial,Nme_Empresa ">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Nome da Empresa" ItemStyle-CssClass="empresaColuna">
                            <ItemTemplate>
                                <asp:Label ID="lblNomeEmpresa" runat="server" Text='<%# Eval("Nme_Empresa") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Motivo do Bloqueio" ItemStyle-CssClass="motivo_bloqueio">
                            <ItemTemplate>
                                <asp:Label ID="lblNumFuncionario" runat="server" Text='<%# Eval("des_motivo_curriculo_nao_visivel_filial") != DBNull.Value ? Eval("des_motivo_curriculo_nao_visivel_filial") : Eval("des_motivo_bloqueio") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Data do Bloqueio" ItemStyle-CssClass="dta_bloqueio">
                            <ItemTemplate>
                                <asp:Label ID="lblRamoDeAtividade" runat="server" Text='<%#  Eval("dta_bloqueio", "{0:dd/MM/yyyy}") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                     
                        <telerik:GridTemplateColumn HeaderText="Ação" ItemStyle-CssClass="acao">
                            <ItemTemplate>
                                <asp:LinkButton ID="btidesbloquear" runat="server"
                                    ToolTip="Bloquear Empresa" CommandName="Desbloquear">
                                    <i class="fa fa-unlock"></i> Desbloquear </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle ShowPagerText="true" Position="TopAndBottom" PagerTextFormat=" {4} Empresas {2} a {3} de {5}" />
                </mastertableview>
            </telerik:RadGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<%--FIM - EMPRESAS BLOQUEADAS--%>
  
<asp:Panel ID="pnlConfirmacaoExclusao" runat="server" CssClass="modal_confirmacao"
    Style="display: none">
    <asp:UpdatePanel ID="upBtiFechar" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ImageButton CssClass="botao_fechar_modal" ID="btiFechar" OnClick="btnCancelar_Click" ImageUrl="/img/botao_padrao_fechar.gif"
                runat="server" CausesValidation="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upConfirmacaoExclusao" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlConfirmarBloqueio" runat="server">
                <div>
                    <h2 class="titulo_modal">
                        <asp:Label ID="lblTitulo" runat="server" Text="Confirmação"></asp:Label>
                    </h2>
                </div>
                <div class="texto_confirmacao">
                    Deseja bloquear a empresa:
                    <br />
                    <asp:Label ID="lblNomeEmpresa" runat="server"></asp:Label><br />
                    <asp:RequiredFieldValidator runat="server" ID="rfvMotivoBloqueio" ControlToValidate="ddlMotivoBloqueio"
                        InitialValue="0" ErrorMessage="Campo Obrigatório" ValidationGroup="valBloqueio"></asp:RequiredFieldValidator>
                    <asp:DropDownList runat="server" Style="width: 300px;" AutoPostBack="true" OnSelectedIndexChanged="ddlMotivoBloqueio_SelectedIndexChanged"
                        ID="ddlMotivoBloqueio" CssClass="textbox_padrao campo_obrigatorio">
                    </asp:DropDownList><br />
                    <asp:RequiredFieldValidator runat="server" ID="rfvMotivo" ControlToValidate="txtMotivoBloqueio"
                        ValidationGroup="valBloqueio" ErrorMessage="Campo Obrigatório" Enabled="false"></asp:RequiredFieldValidator><br />
                    <asp:TextBox ID="txtMotivoBloqueio" runat="server" Style="width: 286px;" Visible="false" CssClass="textbox_padrao campo_obrigatorio" MaxLength="99"></asp:TextBox>

                  
                    <asp:Panel ID="Panel1" CssClass="painel_botoes" runat="server">
                        <asp:Button ID="btnConfirmar" runat="server" ValidationGroup="valBloqueio" Text="Confirmar" CssClass="botao_padrao"
                            OnClick="btnConfirmar_Click" />
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="botao_padrao"
                            OnClick="btnCancelar_Click" CausesValidation="false" />
                    </asp:Panel>
                      </div>
                
            </asp:Panel>

            <asp:Panel ID="pnlconfirmarDesbloqueio" runat="server" Visible="false">
                <div>
                    <h2 class="titulo_modal">
                        <asp:Label ID="Label1" runat="server" Text="Desbloquear"></asp:Label>
                    </h2>
                </div>
                <div class="texto_confirmacao">
                    Deseja Desbloquear a empresa:
                    <br />
                    <asp:Label ID="lblNomeEmpresaDesbloquear" runat="server"></asp:Label><br />
              
                    </div>
                <asp:Panel ID="Panel2" CssClass="painel_botoes" runat="server">
                    <asp:Button ID="btnDesbloquear" runat="server" Text="Confirmar" CssClass="botao_padrao"
                        OnClick="btnDesbloquear_Click" CausesValidation="false" />
                    <asp:Button ID="Button2" runat="server" Text="Cancelar" CssClass="botao_padrao"
                        OnClick="btnCancelar_Click" CausesValidation="false" />
                </asp:Panel>
                      
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:HiddenField ID="hfVariavel" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeConfirmacaoExclusao" runat="server" TargetControlID="hfVariavel"
    PopupControlID="pnlConfirmacaoExclusao">
</AjaxToolkit:ModalPopupExtender>


<uc1:VerDadosEmpresa ID="ucVerDadosEmpresa" runat="server" />
<uc2:ModalConfirmacao ID="ucModalConfirmacao" runat="server" />
<uc3:IndicarEmpresa ID="ucIndicarEmpresa" runat="server" />