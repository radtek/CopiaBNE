<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DadosRepetidosEmpresa.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.DadosRepetidosEmpresa" %>
<asp:Panel ID="pnlDadosRepetidos" runat="server" CssClass="modal_conteudo candidato normal" Style="display: none;">
    <asp:UpdatePanel ID="upDadosRepetidos" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:ImageButton CssClass="botao_fechar_modal" ID="btiFechar" ImageUrl="/img/botao_padrao_fechar.gif"
                runat="server" CausesValidation="false" OnClick="btiFechar_Click" />
            <h2 class="titulo_modal">
                <span>Dados Repetidos</span>
            </h2>
            <telerik:RadGrid ID="gvDadosRepetidos" CssClass="gridview_padrao" AllowPaging="true" AllowCustomPaging="true"
                OnItemCommand="gvDadosRepetidos_ItemCommand" OnPageIndexChanged="gvDadosRepetidos_PageIndexChanged"
                runat="server">
                <MasterTableView DataKeyNames="Idf_Filial">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Nome da Empresa">
                            <ItemTemplate>
                                <asp:LinkButton ID="btlNomeEmpresa" runat="server" CommandName="Selecionar" CausesValidation="false"
                                    Text='<%# Eval("Raz_Social") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CNPJ">
                            <ItemTemplate>
                                <asp:LinkButton ID="btlCNPJ" runat="server" CommandName="Selecionar" CausesValidation="false" Text='<%# Eval("Num_CNPJ") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Dados Repetidos">
                            <ItemTemplate>
                                <asp:LinkButton ID="btlDadoRepetido" runat="server" CommandName="Selecionar" CausesValidation="false"
                                    Text='<%# Eval("Dado_Repetido") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Observações">
                            <ItemTemplate>
                                <asp:LinkButton ID="btlDescricaoObservacao" runat="server" CommandName="Selecionar" CausesValidation="false"
                                    Text='<%# Eval("Des_Observacao") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:HiddenField ID="hfDadosRepetidos" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeDadosRepetidos" runat="server" PopupControlID="pnlDadosRepetidos"
    TargetControlID="hfDadosRepetidos">
</AjaxToolkit:ModalPopupExtender>
