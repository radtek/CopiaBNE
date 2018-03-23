<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="RelatorioAmplitudeSalarial.aspx.cs" Inherits="BNE.Web.RelatorioAmplitudeSalarial" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <Employer:DynamicScript runat="server" Src="/js/local/UserControls/PesquisaSalarial.js" type="text/javascript" />
    <asp:Panel ID="pnlPesquisaSalarial" runat="server">
        <h1>
            Pesquisa Salarial</h1>
        <div class="painel_padrao">
            <div class="painel_padrao_topo">
                &nbsp</div>
            <p class="texto_pesquisa_salarial">
                Preencha os campos abaixo para fazer a pesquisa salarial.</p>
            <div class="formulario_pesquisa">
                <div class="linha">
                    <asp:Label ID="Label1" runat="server" CssClass="label_principal funcao" Text="Função"
                        AssociatedControlID="txtFuncao"></asp:Label>
                    <div class="container_campo">

                                <div>
                                    <asp:RequiredFieldValidator ID="rfvFuncao" runat="server" ValidationGroup="PesquisaSalarial"
                                        ControlToValidate="txtFuncao"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="cvFuncao" runat="server" ControlToValidate="txtFuncao" ValidationGroup="PesquisaSalarial"
                                        ErrorMessage="Função Inválida."></asp:CustomValidator>
                                </div>
                                <asp:TextBox ID="txtFuncao" runat="server" Width="180px" Visible="true"></asp:TextBox>
                                <AjaxToolkit:AutoCompleteExtender ID="aceFuncao" ServiceMethod="ListarFuncoes" TargetControlID="txtFuncao"
                                    UseContextKey="True" runat="server">
                                </AjaxToolkit:AutoCompleteExtender>

                            <telerik:RadComboBox ID="rcbCategoria" runat="server" Width="150px">
                            </telerik:RadComboBox>
                    </div>
                </div>
            </div>
                    <div class="painel_botao_pesquisa">
            <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" CssClass="botao_padrao"
                CausesValidation="false" ValidationGroup="PesquisaSalarial" OnClick="btnPesquisar_Click"  />
        </div>
            <telerik:RadGrid
                ID="gvAmplitudeSalarial"
                AllowPaging="True"
                AllowSorting="true"
                CssClass="gridview_padrao"
                runat="server"
                Skin="Office2007"
                GridLines="None"
                OnItemCommand="gvAmplitudeSalarial_ItemCommand"
                OnPageIndexChanged="gvAmplitudeSalarial_PageIndexChanged"
                OnSortCommand="gvAmplitudeSalarial_SortCommand"
                >
                <ClientSettings
                    AllowExpandCollapse="True" />
                <PagerStyle
                    Mode="NextPrevNumericAndAdvanced"
                    Position="TopAndBottom" />
                <AlternatingItemStyle
                    CssClass="alt_row" />
                <MasterTableView
                    DataKeyNames="Idf_Amplitude_Salarial"
                    AutoGenerateColumns="false">
                    <Columns>

                        <telerik:GridTemplateColumn
                            HeaderText="Função"
                            ItemStyle-CssClass="funcao"
                            SortExpression="Des_Funcao">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblFuncao"
                                    runat="server"
                                    Text='<%# Eval("Des_Funcao") %>'></asp:Label>
                                <asp:Label
                                    ID="lblIdFuncao"
                                    runat="server"
                                    Visible="false"
                                    Text='<%# Eval("Idf_Funcao") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn
                            HeaderText="População"
                            ItemStyle-CssClass=""
                            SortExpression="Nr_Populacao">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblPopulacao"
                                    runat="server"
                                    Text='<%# Eval("Nr_Populacao") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn
                            HeaderText="Média"
                            ItemStyle-CssClass=""
                            SortExpression="Vlr_Mediana">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblMediana"
                                    runat="server"
                                    Text='<%# Eval("Vlr_Mediana") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn
                            HeaderText="Limite Inferior"
                            ItemStyle-CssClass=""
                            SortExpression="Vlr_Amplitude_Inferior">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblInferior"
                                    runat="server"
                                    Text='<%# Eval("Vlr_Amplitude_Inferior") %>'></asp:Label>
                            </ItemTemplate>
                            
                             <EditItemTemplate>
                                 <asp:TextBox ID="txtLimiteInferior" Width="35px" runat="server" Text='<%# Eval("Vlr_Amplitude_Inferior") %>'></asp:TextBox>
                             </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn
                            HeaderText="Limite Superior"
                            ItemStyle-CssClass=""
                            SortExpression="Vlr_Amplitude_Superior">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblSuperior"
                                    runat="server"
                                    Text='<%# Eval("Vlr_Amplitude_Superior") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtLimiteSuperior" runat="server" Width="35px" Text='<%# Eval("Vlr_Amplitude_Superior") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn
                            HeaderText="Alteração"
                            ItemStyle-CssClass=""
                            SortExpression="Dta_Amostra">
                            <ItemTemplate>
                                <asp:Label
                                    ID="lblDataAmostra"
                                    runat="server"
                                    Text='<%# Eval("Dta_Amostra") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn
                            HeaderText="Ações"
                            ItemStyle-CssClass="col_action">

                            <ItemTemplate>
                                <asp:ImageButton
                                    ID="btiEditar"
                                    runat="server"
                                    ImageUrl="../../../img/icn_editar_lapis.png"
                                    ToolTip="Editar Amplitude"
                                    AlternateText="Editar Amplitude"
                                    CausesValidation="false"
                                    CommandName="EditarAmplitude" />
                            </ItemTemplate>

                            <EditItemTemplate>
                              <asp:ImageButton
                                    ID="btiSalvar"
                                    runat="server"
                                    ImageUrl="../../../img/icn_checado2.png"
                                    ToolTip="Salvar Alteração"
                                    AlternateText="Salvar Alteração"
                                    CausesValidation="false"
                                    CommandName="SalvarAmplitude" />
                                <asp:ImageButton
                                    ID="btiCancelar"
                                    runat="server"
                                    ImageUrl="../../../img/icn_bloqueado.png"
                                    ToolTip="Cancelar Edição"
                                    AlternateText="Cancelar Edição"
                                    CausesValidation="false"
                                    CommandName="Cancel" />
                            </EditItemTemplate>

                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <div class="mensagem_nodata">
                            Nenhum item
                            para mostrar.</div>
                    </NoRecordsTemplate>
                </MasterTableView>
            </telerik:RadGrid>


        </div>
    </asp:Panel>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
