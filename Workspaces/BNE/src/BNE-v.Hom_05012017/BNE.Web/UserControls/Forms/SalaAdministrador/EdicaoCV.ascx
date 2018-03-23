<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EdicaoCV.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.EdicaoCV" %>
<%@ Register Src="../../Modais/ConcessaoVip.ascx" TagName="ConcessaoVip" TagPrefix="uc1" %>
<%@ Register Src="../../Modais/ModalConfirmacao.ascx" TagName="ModalConfirmacao"
    TagPrefix="uc2" %>
<%@ Register Src="../../Modais/ExportarCurriculo.ascx" TagName="ExportarCurriculo"
    TagPrefix="uc3" %>
<%@ Register Src="../../Modais/BloquearCandidato.ascx" TagName="BloquearCandidato"
    TagPrefix="uc4" %>
<%@ Register Src="../../Modais/EditarDadosPessoais.ascx" TagName="EditarDadosPessoais"
    TagPrefix="uc5" %>
<asp:UpdatePanel ID="upGvEdicaoCV" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="painel_padrao_sala_adm">
            <p>
                Você pode pesquisar por Código, CPF, nome, e-mail ou telefone:</p>
            <asp:Panel runat="server" DefaultButton="btnFiltrar">
                <telerik:RadTextBox runat="server" ID="tbxFiltroBusca" TextMode="SingleLine" ValidationGroup="Filtrar"
                    MaxLength="255" EmptyMessage="" CssClass="textbox_padrao">
                </telerik:RadTextBox>
                <asp:Button ID="btnFiltrar" runat="server" CssClass="botao_padrao filtrar" Text="Pesquisar"
                    ToolTip="Filtrar Currículos" CausesValidation="True" ValidationGroup="Filtrar"
                    OnClick="btnFiltrar_Click" />
            </asp:Panel>
            <telerik:RadGrid ID="gvEdicaoCV" AllowPaging="True" AllowCustomPaging="true" CssClass="gridview_padrao"
                runat="server" Skin="Office2007" GridLines="None" OnPageIndexChanged="gvEdicaoCV_PageIndexChanged"
                OnItemCommand="gvEdicaoCV_ItemCommand">
                <PagerStyle PagerTextFormat=" {4} Currículo {2} a {3} de {5}" FirstPageToolTip="Primeira página"
                    LastPageToolTip="Ultima página" NextPageToolTip="Próxima página" PrevPageToolTip="Página anterior"
                    Position="TopAndBottom" />
                <AlternatingItemStyle CssClass="alt_row" />
                <MasterTableView DataKeyNames="Idf_Curriculo,Idf_Pessoa_Fisica">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Nome do Candidato" ItemStyle-CssClass="nome_candidato">
                            <ItemTemplate>
                                <asp:Label ID="lblNomeCandidato" runat="server" Text='<%# Eval("Nme_Pessoa") %>'></asp:Label>
                                <asp:Label ID="lblPlano" Visible="false" runat="server" Text='<%# Eval("Des_Plano") %>'></asp:Label>
                                <asp:Label ID="lblIDPlano" Visible="false" runat="server" Text='<%# Eval("Idf_Plano_Adquirido") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CPF" ItemStyle-CssClass="cpf">
                            <ItemTemplate>
                                <asp:Label ID="lblCPF" runat="server" Text='<%# FormatarCPF(Eval("Num_CPF").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Data de Nascimento" ItemStyle-CssClass="dt_nascimento">
                            <ItemTemplate>
                                <asp:Label ID="lblDtaNascimento" runat="server" Text='<%# Eval("Dta_Nascimento") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Data Validade VIP" ItemStyle-CssClass="dv_validade_vip">
                            <ItemTemplate>
                                <asp:Label ID="lblDtaValidade" runat="server" Text='<%# Eval("Validade_Vip") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ações" ItemStyle-CssClass="col_action_arq">
                            <ItemTemplate>
                                <asp:HyperLink ID="btiVisualizar" runat="server" ImageUrl="../../../img/icn_binoculo.png"
                                    ToolTip="Visualizar Currículo" AlternateText="Visualizar Currículo" CausesValidation="false"
                                    NavigateUrl='<%# RetornarURL(Eval("Des_Funcao").ToString(), Eval("Nme_Cidade").ToString(), Eval("Sig_Estado").ToString(), Convert.ToInt32(Eval("Idf_Curriculo"))) %>'
                                    Target="_blank" />
                                <asp:ImageButton ID="btiEditarCurriculo" runat="server" ImageUrl="../../../img/icn_editar_lapis.png"
                                    ToolTip="Editar Currículo" AlternateText="Editar Currículo" CausesValidation="false"
                                    CommandName="EditarCurriculo" />
                                <asp:ImageButton ID="btiSetaCima" runat="server" ImageUrl="../../../img/icn_seta_cima.png"
                                    ToolTip="Exportar Currículo" AlternateText="Exportar Currículo" CausesValidation="false"
                                    CommandName="ExportarCurriculo" CommandArgument='<%# Eval("Idf_Curriculo")%>' />
                                <asp:ImageButton ID="btiAvaliacao" runat="server" ImageUrl="../../../img/SalaAdministrador/smile-green.jpg"
                                    ToolTip="Bronquinha" Visible='<%# Convert.ToInt32(Eval("Idf_Situacao_Curriculo"))!=6 %>'
                                    AlternateText="Bronquinha" CausesValidation="false" CommandName="BronquinhaBloquear"
                                    CommandArgument='<%# Eval("Idf_Curriculo")%>' />
                                <asp:ImageButton ID="btiAvaliacaoBloqueado" runat="server" ImageUrl= "../../../img/SalaAdministrador/smile-red.jpg" 
                                    ToolTip="Bronquinha" Visible='<%# Convert.ToInt32(Eval("Idf_Situacao_Curriculo"))==6 %>'
                                    AlternateText="Bronquinha" CausesValidation="false" CommandName="BronquinhaBloqueado"
                                    CommandArgument='<%# Eval("Idf_Curriculo")%>' />
                                <asp:ImageButton ID="btiAlterarDadosPessoais" runat="server" ImageUrl="~/img/icn_editarvagas.png"
                                    ToolTip="Editar dados Pessoais" AlternateText="Alter dados Pessoais" CausesValidation="false"
                                    CommandName="EditarDadosPessoais" />
                                <asp:ImageButton ID="btiQuemMeViu" runat="server" ImageUrl="~/img/icn_quem_me_viu.png"
                                    ToolTip="Quem Me Viu" AlternateText="Quem Me Viu" CausesValidation="false" CommandName="QuemMeViu" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <div class="mensagem_nodata">
                            Nenhum item para mostrar.</div>
                    </NoRecordsTemplate>
                </MasterTableView>
            </telerik:RadGrid>
        </div>
        <%-- Botões --%>
        <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
            <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
                OnClick="btnVoltar_Click" />
        </asp:Panel>
        <%-- Fim Botões --%>
        <uc1:ConcessaoVip ID="ucConcessaoVip" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>
<uc2:ModalConfirmacao ID="ucModalConfirmacao" runat="server" />
<uc3:ExportarCurriculo ID="ucExportarCurriculo" runat="server" />
<uc4:BloquearCandidato ID="ucBloquearCandidato" runat="server" />
<uc5:EditarDadosPessoais ID="ucEditarDadosPessoais" runat="server" />
