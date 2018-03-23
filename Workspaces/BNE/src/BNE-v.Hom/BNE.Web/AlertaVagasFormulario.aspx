<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true" CodeBehind="AlertaVagasFormulario.aspx.cs" Inherits="BNE.Web.AlertaVagasFormulario" %>

<%@ Register TagName="ucConfirmacao" Src="~/UserControls/Modais/ModalConfirmacao.ascx" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/AlertaVagasFormulario.css" type="text/css" rel="stylesheet" />
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:Panel ID="pnlMainContent" runat="server">
        
        <asp:UpdatePanel ID="upPnlAlertaVagas" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
               <div class="container_alerta">
		        <div class="linha_cinza">
		        </div>
		        <div class="ct_funcao">
			<i class="fa fa-suitcase icons_alert"></i><h4 class="tit_alerta">| Informe <strong>a função</strong></h4>
			<div class="aside_funcoes">
                <asp:UpdatePanel ID="upTxtFuncao" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                            <asp:CustomValidator ID="cvFuncaoPretendida" runat="server" ControlToValidate="txtFuncaoPretendida"
                                                ErrorMessage="Função Inválida" ValidationGroup="ValidarFuncao"></asp:CustomValidator>
                                            <asp:TextBox ID="txtFuncaoPretendida" runat="server" Columns="40" 
                                                CssClass="campo_obrigatorio dados_alert"  placeholder="Digite a função"
                                                CausesValidation="False" ontextchanged="OnTextChanged" ></asp:TextBox>
                                        <button class="adicionar_alert">+</button>
				<p class="inf_alertav">Para <strong>remover as funções desejadas</strong>, basta clicar no <strong>“X”</strong> ao lado do nome.</p>
                                            <AjaxToolkit:AutoCompleteExtender ID="aceFuncaoPretendida" runat="server" TargetControlID="txtFuncaoPretendida"
                                                UseContextKey="True" ServiceMethod="ListarFuncoesComId" OnClientItemSelected="clientSelect">
                                            </AjaxToolkit:AutoCompleteExtender>
                                        <asp:Repeater ID="rptFuncoes" runat="server" 
                                            onitemcommand="repeater_ItemCommand" OnItemCreated="repeater_ItemCreated">
                                    <HeaderTemplate>
                                        <ul id="gridFuncoes" class="GridItems">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li value="<%# DataBinder.Eval(Container.DataItem,"IdFuncao")%>" class="<%# DataBinder.Eval(Container.DataItem,"class")%>">
                                            <%# DataBinder.Eval(Container.DataItem, "DescFuncao")%>
                                            <asp:LinkButton ID="lnkDeletarFuncao" runat="server" CommandName="deletarFuncao" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IdFuncao") %>' BorderStyle="None">
                                                <img class="fechar_img" src="img/pacote_alertaVaga/btn_excluirnew.png" alt="" />
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
		</div>
		<div class="ct_cidade">
			<i class="fa fa-map-marker icons_alert"></i><h4 class="tit_alerta">| Informe <strong>o local</strong></h4>
			<div class="aside_cidade">
                <asp:UpdatePanel ID="upTxtCidade" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:CustomValidator ID="cvCidade" runat="server" ControlToValidate="txtCidade" ValidationGroup="validarCidade"
                            ErrorMessage="Valor Incorreto."></asp:CustomValidator>
                        <asp:TextBox ID="txtCidade" runat="server" MaxLength="50" Columns="50"
                            CssClass="campo_obrigatorio dados_alert" placeholder="Digite a cidade"
                            CausesValidation="False" ontextchanged="OnTextChanged" AutoPostBack="True"></asp:TextBox>
                        <button class="adicionar_alert">+</button>
                        <p class="inf_alertav">Para <strong>remover as cidades desejadas</strong>, basta clicar no <strong>“X”</strong> ao lado do nome.</p>
                        <%-- Linha Grid Cidade --%>
                        <asp:Repeater ID="rptCidades" runat="server" OnItemCommand="repeater_ItemCommand"
                            OnItemCreated="repeater_ItemCreated">
                            <HeaderTemplate>
                                <ul id="gridCidade" class="GridItems">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li value="<%# DataBinder.Eval(Container.DataItem,"IdCidade")%>" class="<%# DataBinder.Eval(Container.DataItem,"class")%>">
                                    <%# DataBinder.Eval(Container.DataItem, "NomeCidade")%>
                                    <asp:LinkButton ID="lnkDeletarCidade" runat="server" CommandName="deletarCidade" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "IdCidade") %>' BorderStyle="None" CausesValidation="False">
                                                    <img class="fechar_img" src="img/pacote_alertaVaga/btn_excluirnew.png" alt="" />
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
		</div>
		<div class="ct_dia_semana">
			<i class="fa fa-clock-o icons_alert"></i><h4 class="tit_alerta">| Informe <strong>quando</strong></h4>
			<div class="aside_semana">
				<div class="list_dias">
					
					<input id="domingo" runat="server" type="checkbox" value="1"><label for="cphConteudo_domingo" class="checkbox" for="Option">Domingo</label><Br>
					<input id="segunda" runat="server" type="checkbox" value="2"><label for="cphConteudo_segunda" class="checkbox" for="Option">Segunda-feira</label><Br>
					<input id="terca" runat="server" type="checkbox" value="3"><label for="cphConteudo_terca" class="checkbox " for="Option">Terça-feira</label><Br>
					<input id="quarta" runat="server" type="checkbox" value="4"><label for="cphConteudo_quarta" class="checkbox " for="Option">Quarta-feira</label><Br>
					<input id="quinta" runat="server" type="checkbox" value="5"><label for="cphConteudo_quinta" class="checkbox " for="Option">Quinta-feira</label><Br>
					<input id="sexta" runat="server" type="checkbox" value="6"><label for="cphConteudo_sexta" class="checkbox " for="Option">Sexta-feira</label><Br>
					<input id="sabado" runat="server" type="checkbox" value="7"><label for="cphConteudo_sabado" class="checkbox " for="Option">Sábado</label><Br>
				</div>
                <br />
				<p class="inf_alertas"><i class="fa fa-exclamation-circle icons_alert2"></i>Você só receberá o alerta de vaga no dias de semana selecionados acima</p>
                <asp:LinkButton ID="btnSalvar" runat="server" Text="SALVAR INFORMAÇÕES" CssClass="salvar_alerta_v" OnClick="btnSalvar_Click" CausesValidation="True"></asp:LinkButton>
			</div>
		</div>
        
         
	</div>
        
            </ContentTemplate>
        </asp:UpdatePanel>
            
        </div>
       
    </asp:Panel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <uc:ucConfirmacao runat="server" ID="ucConfirmacao" />
    <Employer:DynamicScript runat="server" Src="/js/local/AlertaVagasFormulario.js" type="text/javascript" />
</asp:Content>
