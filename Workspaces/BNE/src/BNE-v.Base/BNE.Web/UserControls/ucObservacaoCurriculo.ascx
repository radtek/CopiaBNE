<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucObservacaoCurriculo.ascx.cs"
    Inherits="BNE.Web.UserControls.ucObservacaoCurriculo" %>
<h2 class="titulo_painel_padrao">
    <asp:Label ID="Label2" runat="server" Text="Observações do Currículo" />
</h2>
<div class="painel_padrao">
    <div class="painel_padrao_topo">
    </div>
    <div class="linha">
        <asp:Label ID="lblObservacoes" runat="server" CssClass="label_principal" Text="Observações"
            AssociatedControlID="txtObservacoes" />
        <asp:UpdatePanel runat="server" ID="upTxtObservacoes" UpdateMode="Conditional">
            <ContentTemplate>
                <componente:AlfaNumerico ID="txtObservacoes" runat="server" Rows="1000" MaxLength="2000"
                    ValidationGroup="Observacao" CssClassTextBox="textbox_padrao multiline atividades_exercidas campo_obrigatorio"
                    TextMode="Multiline" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
                <asp:Button ID="btnSalvar" runat="server" Text="Adicionar" CausesValidation="true"
                    CssClass="botao_padrao" ValidationGroup="Observacao" OnClick="btnSalvar_Click" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="linha">
        <asp:UpdatePanel runat="server" ID="upGvObservacoes" UpdateMode="Conditional">
            <ContentTemplate>
                <Employer:EmployerGrid ID="gvObservacoes" runat="server" DataKeyNames="Idf_Curriculo_Observacao, Idf_Usuario_Filial_Perfil"
                    AllowPaging="true" OnRowCommand="gvObservacoes_ItemCommand" OnPageIndexChanging="gvObservacoes_PageIndexChanging"
                    CellSpacing="2" PageSize="4" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField HeaderText="Data" DataField="Dta_Cadastro" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                        <asp:BoundField HeaderText="Observação" DataField="Des_Observacao" HtmlEncode="False" />
                        <asp:BoundField HeaderText="Usuário" DataField="Nme_Usuario" />
                        <asp:TemplateField HeaderText="Ações" HeaderStyle-CssClass="rgHeader centro">
                            <ItemTemplate>
                                <asp:ImageButton ID="btiEditar" runat="server" ToolTip="Editar" AlternateText="Editar"
                                    CausesValidation="False" CommandArgument='<%# Eval("Idf_Curriculo_Observacao") %>'
                                    ImageUrl="~/img/icone_editar_16x16.gif" CommandName="Atualizar" Visible='<%# !Convert.ToBoolean(Eval("Flg_Sistema")) && BotaoVisivel(Convert.ToInt32(Eval("Idf_Usuario_Filial_Perfil"))) %>' />
                                <asp:ImageButton ID="btiExcluir" runat="server" AlternateText="Excluir" CausesValidation="False"
                                    ToolTip="Excluir" CommandArgument='<%# Eval("Idf_Curriculo_Observacao") %>' ImageUrl="~/img/icone_excluir_16x16.png"
                                    CommandName="Inativar" Visible='<%# !Convert.ToBoolean(Eval("Flg_Sistema")) && BotaoVisivel(Convert.ToInt32(Eval("Idf_Usuario_Filial_Perfil"))) %>' />
                            </ItemTemplate>
                            <HeaderStyle CssClass="rgHeader centro coluna_icones" />
                            <ItemStyle CssClass="espaco_icones centro" />
                        </asp:TemplateField>
                    </Columns>
                </Employer:EmployerGrid>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
