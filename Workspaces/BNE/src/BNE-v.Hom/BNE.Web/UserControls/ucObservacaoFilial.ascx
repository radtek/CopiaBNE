<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucObservacaoFilial.ascx.cs"
    Inherits="BNE.Web.UserControls.ucObservacaoFilial" %>

<Employer:DynamicHtmlLink runat="server" Href="/css/local/ucObservacaoFilial.css" type="text/css" rel="stylesheet" />
<Employer:DynamicScript  runat="server" type="text/javascript" src="/js/local/UserControls/ucObservacaoFilial.js"></Employer:DynamicScript>
<Employer:DynamicScript runat="server" Src="/js/jquery.maskedinput.min.js" type="text/javascript"/>
<div class="linha">
    <asp:UpdatePanel runat="server" ID="upTxtObservacoes" style="width:69%;" UpdateMode="Conditional">
        <ContentTemplate>
         
            <div id="ObservacaoEsq" class="Observacao">
                <asp:Label ID="lblObservacoes" runat="server" CssClass="label_principal"  style="text-align:left;" Text="O que aconteceu?"
                    AssociatedControlID="txtObservacoes" />
                <br />
                <componente:AlfaNumerico ID="txtObservacoes" runat="server" Rows="1000" MaxLength="2000"
                    ValidationGroup="Observacao"  Width="580px"  CssClassTextBox="textbox_padrao multiline atividades_exercidas campo_obrigatorio"
                    TextMode="Multiline" />
            </div>

            <div id="ObservacaoDir" class="DataDesc">
                <asp:Label ID="lblProximoPasso" runat="server" Text="Próximo Passo" CssClass="label_principal"></asp:Label>
                <br />
                <br />
                <table cellspacing="5">
                    <tr>
                        <td>
                            <asp:Label ID="lblDataProximoPasso" runat="server" Text="Data"></asp:Label></td>
                        <td>
                            <componente:Data ID="txtDtaProximoPasso" runat="server" Width="100px" Obrigatorio="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblHora"   Text="Hora"></asp:Label>
                        </td>
                        <td>
                        
                            <asp:RegularExpressionValidator runat="server" ErrorMessage="Hora Inválida" 
                                ControlToValidate="txtHora" ValidationExpression="^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$" ValidationGroup="Observacao"></asp:RegularExpressionValidator>
                            <asp:TextBox ID="txtHora"  runat="server"   Width="100px" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDescricaoProximoPasso" runat="server" Text="Descrição"></asp:Label></td>
                        <td>
                            <asp:TextBox ID="txtDescricaoProximoPasso" runat="server" Text="" Width="100px"></asp:TextBox></td>
                    </tr>
                </table>
                <br />
                <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
                            <asp:Button ID="btnSalvar" runat="server" Text="Adicionar" CausesValidation="true"
                                CssClass="botao_padrao" ValidationGroup="Observacao" OnClick="btnSalvar_Click" />
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <br />
           
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

<div class="linha">
    <asp:UpdatePanel runat="server" ID="upGvObservacoes" UpdateMode="Conditional">
        <ContentTemplate>
             <asp:Panel ID="pnlPesquisar" runat="server" DefaultButton="btiPesquisar">
                <div class="linhaSearch">
                    <asp:Label runat="server" ID="lblPesquisar" Text="Pesquisar:"></asp:Label>
                    <asp:TextBox
                        ID="txtPesquisar"
                        runat="server"></asp:TextBox>
                    <asp:ImageButton
                        ID="btiPesquisar"
                        runat="server"
                        ImageUrl="img"
                        src="../../../img/icn_filtro_pesquisa_msg.png"
                        CssClass="btn_pesquisa"
                        OnClick="btiPesquisar_Click"
                        Style="width: 16px; height: 16px" />
                </div>
                  </asp:Panel>
            <br />
            <Employer:EmployerGrid ID="gvObservacoes" runat="server" DataKeyNames="Idf_Filial_Observacao, Idf_Usuario_Filial_Perfil"
                AllowPaging="true" OnRowCommand="gvObservacoes_ItemCommand" OnPageIndexChanging="gvObservacoes_PageIndexChanging"
                CellSpacing="2" PageSize="5" AutoGenerateColumns="false" OnPageSizeChanging="gvObservacoes_PageSizeChanging" >
                <Columns>
                    <asp:BoundField HeaderText="Data" DataField="Dta_Cadastro" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}" />
                    <asp:BoundField HeaderText="Observação" DataField="Des_Observacao" HtmlEncode="False" />
                    <asp:BoundField HeaderText="Usuário" DataField="Nme_Usuario" />
                    <asp:BoundField HeaderText="Data Retorno" DataField="Dta_Proxima_Acao" />
                    <asp:BoundField HeaderText="Descrição Retorno" DataField="Des_Proxima_Acao" />
                    <asp:TemplateField HeaderText="Ações" HeaderStyle-CssClass="rgHeader centro">
                        <ItemTemplate>
                            <asp:ImageButton ID="btiEditar" runat="server" ToolTip="Editar" AlternateText="Editar"
                                CausesValidation="False" CommandArgument='<%# Eval("Idf_Filial_Observacao") %>'
                                ImageUrl="~/img/icone_editar_16x16.gif" CommandName="Atualizar" Visible='<%# !Convert.ToBoolean(Eval("Flg_Sistema")) && (Eval("Idf_Usuario_Filial_Perfil") == DBNull.Value ? false : BotaoVisivel(Convert.ToInt32(Eval("Idf_Usuario_Filial_Perfil")))) %>' />
                            <asp:ImageButton ID="btiExcluir" runat="server" AlternateText="Excluir" CausesValidation="False"
                                ToolTip="Excluir" CommandArgument='<%# Eval("Idf_Filial_Observacao") %>' ImageUrl="~/img/icone_excluir_16x16.png"
                                CommandName="Inativar" Visible='<%# !Convert.ToBoolean(Eval("Flg_Sistema")) && (Eval("Idf_Usuario_Filial_Perfil") == DBNull.Value ? false : BotaoVisivel(Convert.ToInt32(Eval("Idf_Usuario_Filial_Perfil")))) %>' />
                        </ItemTemplate>
                        <HeaderStyle CssClass="rgHeader centro coluna_icones" />
                        <ItemStyle CssClass="espaco_icones centro" />
                    </asp:TemplateField>
                </Columns>
            </Employer:EmployerGrid>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
