<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="AlertaVagasFormulario.aspx.cs" Inherits="BNE.Web.AlertaVagasFormulario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/AlertaVagasFormulario.css" type="text/css" rel="stylesheet" />
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:Panel ID="pnlMainContent" runat="server">
        <div class="painel_padrao">
        <asp:UpdatePanel ID="upPnlAlertaVagas" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlAlertaVagas" runat="server">
                  
                  

                    <div class="container_alertaVaga">
                        <div class="controlador_detalhes">
                            <span class="label_tituloAlerta">Alerta de Vaga</span>
                        </div>
                        <div class="controlador_detalhes2">
                            <div class="label_textoParagrafo">
                                No <b>Alerta de Vagas</b> é você quem escolhe de onde e quais as funções que quer receber!
                            </div>
                        </div>
              
                        <div class="controlador_detalhes3">
            
                        <%-- Linha Cidade --%>
                            <div class="linha">
                                <asp:Label ID="lblCidade" runat="server" CssClass="label_principal" Text="Cidade" AssociatedControlID="txtCidade" />
                                <asp:UpdatePanel ID="upTxtCidade" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div>
                                            <asp:CustomValidator ID="cvCidade" runat="server" ControlToValidate="txtCidade" ValidationGroup="validarCidade"
                                            ErrorMessage="Valor Incorreto."></asp:CustomValidator>
                                        </div>
                                        <div class="container_campo">
                                            <asp:TextBox ID="txtCidade" runat="server" MaxLength="50" Columns="50" 
                                                CssClass="txCidade textbox_padrao detalheLarguraCampoField campo_obrigatorio" 
                                                CausesValidation="False" OnTextChanged="OnTextChanged" AutoPostBack="True"></asp:TextBox>    
                                            <AjaxToolkit:AutoCompleteExtender runat="server" ID="aceCidade" TargetControlID="txtCidade"
                                                ServiceMethod="ListarCidadesSiglaEstadoPorNomeParcialEstado" OnClientItemSelected="clientSelect"/>
                                        </div>
                                        <%-- Linha Grid Cidade --%>
                                    <asp:Repeater ID="rptCidades" runat="server" onitemcommand="repeater_ItemCommand" 
                                            onitemcreated="repeater_ItemCreated">
                                        <HeaderTemplate>
                                            <ul id="gridCidade" class="GridItems">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li value="<%# DataBinder.Eval(Container.DataItem,"IdCidade")%>"  class="<%# DataBinder.Eval(Container.DataItem,"class")%>">
                                                <%# DataBinder.Eval(Container.DataItem, "NomeCidade")%>
                                                <asp:LinkButton ID="lnkDeletarCidade" runat="server" CommandName="deletarCidade" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IdCidade") %>' BorderStyle="None" CausesValidation="False">
                                                    <img src="img/pacote_alertaVaga/btn_excluir.png" alt="" />
                                                </asp:LinkButton>
                                            </li>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </ul>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                <asp:HiddenField ID="CidadesSel" runat="server" Value="" />
                                <%-- FIM: Grid Cidade --%>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSalvar" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="txtCidade" EventName="TextChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        <%-- FIM: Linha Cidade --%>
                        <%-- Linha Função --%>
                            <div class="linha">
                                <asp:Label ID="lblFuncaoPretendida" runat="server" CssClass="label_principal" Text="Função"
                                    AssociatedControlID="txtFuncaoPretendida" />
                                <asp:UpdatePanel ID="upTxtFuncao" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div>
                                            <asp:CustomValidator ID="cvFuncaoPretendida" runat="server" ControlToValidate="txtFuncaoPretendida"
                                                ErrorMessage="Função Inválida" ValidationGroup="ValidarFuncao"></asp:CustomValidator>
                                        </div>
                                        <div class="container_campo">
                                            <asp:TextBox ID="txtFuncaoPretendida" runat="server" Columns="40" 
                                                CssClass="txFuncao textbox_padrao detalheLarguraCampoField campo_obrigatorio" 
                                                ontextchanged="OnTextChanged" AutoPostBack="True"></asp:TextBox>
                                            <AjaxToolkit:AutoCompleteExtender ID="aceFuncaoPretendida" runat="server" TargetControlID="txtFuncaoPretendida"
                                                UseContextKey="True" ServiceMethod="ListarFuncoesComId" OnClientItemSelected="clientSelect">
                                            </AjaxToolkit:AutoCompleteExtender>
                                        </div>
                                        <asp:Repeater ID="rptFuncoes" runat="server" 
                                            onitemcommand="repeater_ItemCommand" OnItemCreated="repeater_ItemCreated">
                                    <HeaderTemplate>
                                        <ul id="gridFuncoes" class="GridItems">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li value="<%# DataBinder.Eval(Container.DataItem,"IdFuncao")%>" class="<%# DataBinder.Eval(Container.DataItem,"class")%>">
                                            <%# DataBinder.Eval(Container.DataItem, "DescFuncao")%>
                                            <asp:LinkButton ID="lnkDeletarFuncao" runat="server" CommandName="deletarFuncao" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IdFuncao") %>' BorderStyle="None">
                                                <img src="img/pacote_alertaVaga/btn_excluir.png" alt="" />
                                            </asp:LinkButton>
                                        </li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>     
                                <asp:HiddenField ID="FuncoesSel" runat="server" Value=""/> 
                                    </ContentTemplate>
                                    <Triggers>
                                         <asp:AsyncPostBackTrigger ControlID="btnSalvar" EventName="Click" />
                                         <asp:AsyncPostBackTrigger ControlID="txtFuncaoPretendida" EventName="TextChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        <%-- FIM: Linha Função --%>
                        </div>
                    </div>
                    <%-- FIM: Container Alerta de Vagas --%>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
                                <%--Linha Botão --%>
                                <div class="btn_azul">
                                    <asp:LinkButton ID="btnSalvar" runat="server" Text="Salvar" CssClass="label_vagaSalvar" OnClick="btnSalvar_Click" CausesValidation="True"></asp:LinkButton>
                                </div>
                                <%--FIM: Linha Botão --%>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
<%-- Modal Solicitação Online --%>
    <asp:HiddenField ID="hfModalConfirmacaoAlerta" runat="server" />
    <AjaxToolkit:ModalPopupExtender CancelControlID="btiModalConfirmacaoAlertaFechar" ID="mpeModalConfirmacaoAlerta"
        runat="server" PopupControlID="pnlModalConfirmacaoAlerta" TargetControlID="hfModalConfirmacaoAlerta">
    </AjaxToolkit:ModalPopupExtender>
    <asp:Panel ID="pnlModalConfirmacaoAlerta" runat="server" CssClass="modalConfirmacaoAlerta"
        Style="display:block">
        <asp:UpdatePanel ID="upBtiModalConfirmacaoAlertaFechar" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                
                <asp:LinkButton CssClass="botao_fechar_m" ID="btiModalConfirmacaoAlertaFechar" 
                    runat="server" CausesValidation="false"><i class="fa fa-times-circle"></i>
</asp:LinkButton>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="texto_aviso">
            Seu Alerta foi cadastrado com sucesso!<br/>
        </div>
        <asp:LinkButton runat="server" ID="btlModalOk" CssClass="label_alerta_btnOk"
            ValidationGroup="SolicitarR1" CausesValidation="False" Text="OK" PostBackUrl="~/SalaVip.aspx">
        </asp:LinkButton>
    </asp:Panel>
    <%-- FIM: Modal Solicitação Online --%>
    <Employer:DynamicScript runat="server" Src="/js/local/AlertaVagasFormulario.js" type="text/javascript" />
</asp:Content>
