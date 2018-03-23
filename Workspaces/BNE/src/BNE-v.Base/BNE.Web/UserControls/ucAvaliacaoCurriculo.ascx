<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ucAvaliacaoCurriculo.ascx.cs"
    Inherits="BNE.Web.UserControls.ucAvaliacaoCurriculo" %>
<h2>Avaliação do Currículo</h2>
<asp:UpdatePanel ID="upAvaliacaoCurriculo" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlAvalicaoCurriculo" runat="server" CssClass="painel_padrao_linha">
            <!-- Linha Imagens -->
            <div class="bloco_form">
                <div class="avaliacao">
                    <div>
                        <asp:CustomValidator ID="cvAvaliacao" runat="server" ErrorMessage="Escolha uma das opções abaixo"
                            OnServerValidate="cvAvaliacao_ServerValidate" ValidateEmptyText="true" ValidationGroup="Classificar"></asp:CustomValidator>
                    </div>
                    <asp:Label ID="lblPontoVista" runat="server" Text="Ponto de Vista" CssClass="label_principal"></asp:Label>
                    <span>
                        <asp:RadioButton ID="rbPositiva" CssClass="radio" runat="server" GroupName="Imagens" />
                        <i class='fa fa-smile-o fa-2x'></i></span>
                    <span>
                        <asp:RadioButton CssClass="radio" ID="rbSemClassificacao" runat="server" GroupName="Imagens" />
                        <i class='fa fa-meh-o fa-2x'></i></span>
                    <span>
                        <asp:RadioButton ID="rbNegativa" runat="server" CssClass="radio" GroupName="Imagens" />
                        <i class='fa fa-frown-o fa-2x'></i></span>
                </div>
                <!-- FIM: Linha Imagens -->
                <div class="texto-avaliacao">
                    <asp:Label ID="lblObservacoes" AssociatedControlID="txtObservacoes" CssClass="label_principal"
                        runat="server" Text="Comentário" />
                    <div class="container_campo">
                        <asp:TextBox ID="txtObservacoes" CssClass="textarea_padrao textarea_avaliacao" TextMode="MultiLine"
                            Rows="3" runat="server"></asp:TextBox>
                    </div>
                    <div class="caracteres_cont">
                        <Componentes:ContagemCaracteres runat="server" ControlToValidate="txtObservacoes" MaxLength="2000" />
                        <%--CssClassLabel="caracteres_cont"--%>
                    </div>
                </div>
                <div class="btn_enviar_avaliacao">
                    <asp:Panel ID="pnlBotoes" CssClass="painel_botoes" runat="server">
                        <asp:Button ID="btnClassificar" runat="server" Text="Avaliar" CssClass="botao_padrao"
                            CausesValidation="true" ValidationGroup="Classificar" OnClick="btnClassificar_Click" />
                    </asp:Panel>
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<div class="linha">
    <asp:UpdatePanel runat="server" ID="upGvClassificacoes" UpdateMode="Conditional">
        <ContentTemplate>
            <Employer:EmployerGrid ID="gvClassificacoes" runat="server" DataKeyNames="Idf_Curriculo_Classificacao, Idf_Usuario_Filial_Perfil"
                AllowPaging="true" OnRowCommand="gvClassificacoes_ItemCommand" OnPageIndexChanging="gvClassificacoes_PageIndexChanging"
                CellSpacing="2" PageSize="4" AutoGenerateColumns="false" OnRowDataBound="gvClassificacoes_RowDataBound">
                <Columns>
                    <asp:BoundField HeaderText="Data" DataField="Dta_Cadastro" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                    <asp:BoundField HeaderText="Classificação" DataField="Des_Observacao" HtmlEncode="False" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="imgAvaliacao" runat="server" AlternateText="Avaliação" CausesValidation="false" CommandArgument='<%# Eval("Idf_Avaliacao") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Usuário" DataField="Nme_Pessoa" />
                    <asp:TemplateField HeaderText="Ações" HeaderStyle-CssClass="rgHeader centro">
                        <ItemTemplate>
                            <asp:ImageButton ID="btiEditar" runat="server" ToolTip="Editar" AlternateText="Editar"
                                CausesValidation="False" CommandArgument='<%# Eval("Idf_Curriculo_Classificacao") %>'
                                ImageUrl="~/img/icone_editar_16x16.gif" CommandName="Atualizar" Visible='<%# BotaoVisivel(Convert.ToInt32(Eval("Idf_Usuario_Filial_Perfil"))) %>' />
                            <asp:ImageButton ID="btiExcluir" runat="server" AlternateText="Excluir" CausesValidation="False"
                                ToolTip="Excluir" CommandArgument='<%# Eval("Idf_Curriculo_Classificacao") %>' ImageUrl="~/img/icone_excluir_16x16.png"
                                CommandName="Inativar" Visible='<%# BotaoVisivel(Convert.ToInt32(Eval("Idf_Usuario_Filial_Perfil"))) %>' />
                        </ItemTemplate>
                        <HeaderStyle CssClass="rgHeader centro coluna_icones" />
                        <ItemStyle CssClass="espaco_icones centro" />
                    </asp:TemplateField>
                </Columns>
            </Employer:EmployerGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

