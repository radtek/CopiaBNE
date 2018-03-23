<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SalaSelecionador.aspx.cs" Inherits="BNE.Web.SalaSelecionador" %>

<%@ Register Src="UserControls/Forms/SalaSelecionador/Dados.ascx" TagName="Dados"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/Sala_Selecionador.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicScript runat="server" Src="/js/local/SalaSelecionador.js" type="text/javascript" />
        <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=0">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="painel_padrao_sala_selecionador">
        <!--Saudação e logo empresa -->
        <asp:Panel ID="pnlDados" runat="server">
            <uc1:Dados ID="ucDados" runat="server" />
        </asp:Panel>
        <asp:HiddenField ID="hdfUfP" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="hdfFilial" ClientIDMode="Static" runat="server" />
        <!-- Grid botões -->
        <asp:UpdatePanel ID="upMenuNavegacao" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlNavegacao" CssClass="rtsUL" runat="server">                    
                    <div class="menu_sv">
                        <%-- Minhas Vagas --%>
                        <div class="menu_sv_block">
                            <asp:LinkButton ID="BotaoVagas" runat="server" OnClick="btlMinhasVagas_Click" CausesValidation="false"
                                ToolTip="Vagas">
                                <div>
                                    <i class="fa fa-newspaper-o fa-5x"></i>
                                </div>
                                <div class="menu_sv_block-title">Minhas Vagas</div>
                                <ul  class="item_btn">
                                    <li>Administre ou divulgue vagas aqui </li>
                                    <li id="liVagasAnunciadas" runat="server" style="display: none"><b>
                                        <asp:Label ID="lblVagasAnunciadasQuantidade" runat="server"></asp:Label></b>
                                        <asp:Label ID="lblVagasAnunciadasMensagem" runat="server"></asp:Label>
                                    </li>
                                    <li id="liCVsRecebidos" runat="server" style="display: none"><b>
                                        <asp:Label ID="lblCurriculosRecebidosQuantidade" runat="server"></asp:Label></b>
                                        <asp:Label ID="lblCurriculosRecebidosMensagem" runat="server"></asp:Label>
                                    </li>
                                </ul>
                            </asp:LinkButton>                           
                        </div>
                        <%-- Meu Plano --%>
                        <div class="menu_sv_block">
                            <asp:LinkButton ID="BotaoMeuPlano" runat="server" OnClick="btlCompraPlanos_Click"
                                CausesValidation="false" ToolTip="Meu Plano">
                                <div>
                                    <asp:Literal runat="server" ID="litSemPlano" Visible="False">
                                        <i class="fa fa-times fa-5x red"></i>
                                    </asp:Literal>
                                    <asp:Literal runat="server" ID="litComPlano" Visible="False">
                                        <i class="fa fa-check fa-5x"></i>
                                    </asp:Literal>
                                </div>
                                <div class="menu_sv_block-title">Meu Plano</div>
                                <ul class="item_btn">
                                    <li>
                                        <asp:Label ID="lblPlanoAcessoValidade" runat="server"></asp:Label>
                                    </li>
                                    <li>
                                        <b><asp:Label ID="lblQuantidadeSMSUtilizado" runat="server"></asp:Label></b>
                                        <asp:Label ID="lblSMSUtilizadoMensagem" runat="server"></asp:Label>
                                    </li>
                                </ul>
                            </asp:LinkButton>                            
                        </div>
                        <%-- Campanha Recrutamento --%>
                        <div class="menu_sv_block">
                            <asp:LinkButton ID="BotaoRecrutamento" runat="server" OnClick="btlCampanhaRecrutamento_Click"
                                CausesValidation="false" ToolTip="Campanha">
                                <div>
                                    <i class="fa fa-bullhorn fa-5x "></i>
                                </div>
                                <div class="menu_sv_block-title">Campanha de Recrutamento</div>
                                <ul class="item_btn">
                                    <li>
                                        <asp:Literal ID="litSaldoCampanha" runat="server" Text="0"></asp:Literal>
                                    </li>
                                </ul>
                            </asp:LinkButton>                            
                        </div>
                        <%-- Alerta de Currículos --%>
                        <div class="menu_sv_block">
                            <asp:LinkButton ID="btlRastreadorCurriculos" runat="server" OnClick="btlRastreadorCurriculos_Click"
                                CausesValidation="false" ToolTip="Alerta de Currículos">
                                <div>
                                    <span class="fa-stack fa-lg fa-2x">
                                        <i class="fa fa-comment-o fa-flip-horizontal fa-stack-2x"></i>
                                        <i class="fa fa-exclamation fa-stack-1x"></i>
                                    </span>
                                </div>
                                <div class="menu_sv_block-title">Alerta de Currículos</div>
                                <ul class="item_btn">
                                    <li>Receba candidatos no perfil em seu e-mail</li>
                                </ul>
                            </asp:LinkButton>
                        </div>
                        <%-- Mensagens --%>
                        <div class="menu_sv_block">
                            <asp:LinkButton ID="btlMensagens" runat="server" OnClick="btlMensagensEnviadas_Click"
                                CausesValidation="false" ToolTip="Mensagens">
                                <div>
                                    <i class="fa fa-envelope-o fa-5x"></i>
                                </div>
                                <div class="menu_sv_block-title">Mensagens</div>
                                <ul class="item_btn">
                                    <%-- Esse <li> não pode aparecer sem ter dados, o IE7 esta interpretando como se ele tivesse conteúdo --%>
                                    <li>
                                        <asp:Label ID="lblMensagemPossuiMensagem" runat="server"></asp:Label></li>
                                    <li><b>
                                        <asp:Label ID="lblQuantidadeMensagens" runat="server"></asp:Label></b>
                                        <asp:Label ID="lblTextoMensagem" runat="server"></asp:Label>
                                    </li>
                                </ul>
                            </asp:LinkButton>
                        </div>
                        <%-- Pesquisa de Currículos --%>
                        <div class="menu_sv_block">
                            <asp:LinkButton ID="btlPesquisaAvancada" runat="server" CausesValidation="false"
                                OnClick="btlPesquisaAvancada_OnClick" ToolTip="Pesquisa Avançada">
                                <div>
                                    <span class="fa-stack fa-2x">
                                        <i class="fa fa-file-o fa-stack-2x"></i>
                                        <i class="fa fa-search fa-stack-1x"></i>
                                    </span>
                                </div>
                                <div class="menu_sv_block-title">Pesquisa de Currículos</div>
                                <ul class="item_btn">
                                    <li>Filtro completo para uma busca assertiva</li>
                                </ul>
                            </asp:LinkButton>
                        </div>
                        <%-- Configurações --%>
                        <div class="menu_sv_block"> 
                            <asp:LinkButton ID="btlConfiguracoes" runat="server" CausesValidation="false" OnClick="btlConfiguracoes_OnClick">
                                <div>
                                    <span class="fa-stack fa-2x">
                                        <i class="fa fa-cogs fa-stack-2x"></i>
                                    </span>
                                </div>
                                <div class="menu_sv_block-title">Configurações</div>
                                <ul class="item_btn">
                                    <li>Configure as respostas automáticas</li>
                                </ul>
                            </asp:LinkButton>
                        </div>
                        <%-- Gerenciar usuários --%>
                        <div class="menu_sv_block"> 
                            <asp:HyperLink ID="hlGerenciarUsuario" runat="server">
                                <div>
                                    <span class="fa-stack fa-2x">
                                        <i class="fa fa-users fa-stack-2x"></i>
                                    </span>                                    
                                </div>
                                <div class="menu_sv_block-title">Usuários</div>
                                <ul class="item_btn">
                                    <li>Crie e gerencie usuários</li>
                                </ul>
                            </asp:HyperLink>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnAviso" Visible="false">
                    <div class="alert alert-error">Sua empresa está bloqueada, para mais informações ligue 0800 41 2400</div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%--MODAL DE PESQUISA FOLHA DE PAGAMENTO--%>


    <div class="modal fade " id="modal_melhoria" tabindex="-1" role="dialog" >
			<div class="modal-dialog" role="document">
			    <div class="modal-content">
			     	<div class="modal-header">
			        	<!-- <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button> -->
			        	<h4 id="mdlTitle" class="modal-title" >O que você acha?</h4>
			      	</div>
			      	<div class="modal-body">
						<div class="row">
							<div class="col-md-12 step1"  >
								<p style="text-align:center">
											Gostaria que o BNE disponibilizasse a integração<br />
									dos dados do candidato com a sua folha de pagamento?
								</p>
							</div>
							<div class="col-md-6 step1">
								<button type="button" class="btn btn-default " onclick="NaoUtiliza();" data-dismiss="modal" style="width:100%">Não, obrigado</button>
							</div>
							<div class="col-md-6 step1" >
					        	<button type="button" class="btn btn-success gostep2" style="width:100%">Sim, ajudaria muito!</button>
							</div>
							<div class="col-md-12 step2"  >
							<%--	<p style="text-align:center">
									Ok, qual a folha de pagamento sua empresa utiliza?
								</p>--%>
							</div>

                            <div id="divOpcoes"></div>
							<div id="divOutroEsc"></div>
							<div id="divOutro" class="col-md-12 form-group folha_option_outro ">
								<label for="outro_txt">Qual?</label>
  								<input type="text" class="form-control" id="outro_txt">
                                <div class="validationmsg">Insira o nome da sua folha de pagamento</div>
							</div>
						</div>
			      	</div>
			      	<div class="modal-footer" id="modal_melhoria_footer">
			        	<button type="button" class="btn btn-success" onclick="return validateFolha();" >Pronto!</button>
			      	</div>

			    </div>
			</div>
		</div>



</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
