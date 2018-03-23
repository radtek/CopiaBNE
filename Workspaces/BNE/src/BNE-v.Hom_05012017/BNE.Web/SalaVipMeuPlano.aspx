<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SalaVipMeuPlano.aspx.cs" Inherits="BNE.Web.SalaVipMeuPlano" %>

<%@ Register Src="~/UserControls/Modais/ModalConfirmacaoRetornoPagamento.ascx" TagName="ModalConfirmacaoRetornoPagamento"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/Modais/ucWebCallBack_Modais.ascx" TagName="WebCallBack_Modais" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicScript runat="server" Src="/js/local/Forms/SalaVip.js" type="text/javascript" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/sala_vip.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaVipMeuPlano.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:UpdatePanel ID="upPlano" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <asp:Panel ID="pnlPlano" runat="server">
                <asp:UpdatePanel ID="upMeuPlano" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="painel_padrao_sala_vip">
                            <div class="caracteristicas_plano">
                                <ul>
                                    <li>
                                        <asp:Label ID="lblDataPlanoAdquirido" Text="Plano Adquirido em: " runat="server"
                                            CssClass="label_campo"></asp:Label>
                                        <asp:Label ID="lblDataPlanoAdquiridoValor" runat="server"></asp:Label>
                                    </li>
                                    <li>
                                        <asp:Label ID="lblPlanoValidade" Text="Plano Válido até: " runat="server" CssClass="label_campo"></asp:Label>
                                        <asp:Label ID="lblPlanoValidadeValor" runat="server"></asp:Label>
                                        <asp:Label ID="lblSeparador" Visible="false" runat="server" Text=" - "></asp:Label>
                                        <asp:LinkButton ID="lnkRenovarPlano" Visible="false" Text="Renovar Plano" runat="server"
                                            OnClick="lnkRenovarPlano_Click"></asp:LinkButton>
                                        <asp:Label ID="lblPlanoRenovado" Text="( Plano Renovado )" Visible="false" runat="server"></asp:Label>
                                    </li>
                                    <li>
                                        <asp:Label ID="lblPlanoValor" Text="Valor do Plano: " runat="server" CssClass="label_campo"></asp:Label>
                                        <asp:Label ID="lblPlanoValorTexto" runat="server"></asp:Label>
                                    </li>
                                    <li>
                                        <asp:Label ID="lblTipoPlano" runat="server" Text="Plano de Acesso: " CssClass="label_campo"></asp:Label>
                                        <asp:Label ID="lblTipoPlanoValor" runat="server"></asp:Label>
                                    </li>
                                    <li>
                                        <button id="btnCancelarAssinatura" runat="server" class="btn_cancelar_assinatura" type="button" data-toggle="modal" data-target="#modalCancelarAssinaturaRecorrente" visible="false">
                                            Cancelar Plano
                                        </button>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="upErroTransacaoCartao" runat="server" UpdateMode="Conditional"
                    Visible="false">
                    <ContentTemplate>
                        <div class="painel_padrao_sala_vip">
                            <div class="caracteristicas_plano">
                                <ul>
                                    <li>
                                        <asp:Label ID="Label11" Text="Erro ao Efetuar Pagamento. " runat="server" CssClass="label_campo"></asp:Label>
                                        <asp:Button ID="Button1" runat="server" CssClass="botao_padrao" Text="Tente Novamente"
                                            CausesValidation="false" OnClick="btnTenteNovamente_Click" />
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
                    <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
                        OnClick="btnVoltar_Click" />
                </asp:Panel>
                <uc:ModalConfirmacaoRetornoPagamento ID="ucModalConfirmacaoRetornoPagamento" runat="server"></uc:ModalConfirmacaoRetornoPagamento>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="modalCancelarAssinaturaRecorrente" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="top: -500px;">
        <asp:UpdatePanel ID="upModalCancelarAssinaturaRecorrente" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-header">
                    <button type="button" class="close" id="closeModal" data-dismiss="modal" aria-hidden="true">×</button>
                    <h2 id="modalTitulo">Cancelamento de Plano</h2>
                </div>
                <div id="divConteudoCancelarAssinatura" runat="server" class="modal-body" style="font-size: 16px; color: #666">
                    <div style="margin-top: 10px;">
                        <asp:Label ID="lblDataInicioPlanoAdquirido" Text="Seu cadastro é desde" runat="server"></asp:Label>
                        <asp:Label ID="lblDataInicioPlanoAdquiridoValor" Style="color: #555555" runat="server"></asp:Label>
                    </div>
                    <div style="margin-top: 10px;">
                        <asp:Label ID="lblDataFimPlanoAdquirido" Text="Sua Assinatura está ativa até" runat="server"></asp:Label>
                        <asp:Label ID="lblDataFimPlanoAdquiridoValor" Style="color: #555555" runat="server"></asp:Label>
                    </div>
                    <div style="margin-top: 10px;">
                        <asp:Label ID="lblDesejaCancelar" Text="Tem certeza que deseja cancelar a partir desta data?" runat="server"></asp:Label>
                    </div>
                    <div style="margin-top: 20px;">
                        <div style="float: left">
                            <asp:Button runat="server" CssClass="btn_nao_cancelarAss" Text="Não" data-toggle="modal" data-target="#modalCancelarAssinaturaRecorrente" />
                        </div>
                        <div style="margin-left: 20px;">
                            <asp:Button CssClass="btn_sim_cancelarAss" runat="server" Text="Sim" OnClick="btnEfetivarCancelamentoAssinatura_Click" OnClientClick="OcultarBtnCancelar();" />
                        </div>
                    </div>
                    <div style="margin-top: 20px; clear: both">
                        <asp:Label runat="server" Text="Se preferir ligamos para você "></asp:Label>
                        <button id="modalWebCallBack" class="btn_MeLigue_cancelarAss" runat="server" type="button" data-toggle="modal" data-target="#nomeModal"><i class="fa fa-phone"></i>Me Ligue</button>
                    </div>
                </div>
                <div id="divSucessoCancelamentoAssinatura" class="modal-footer" runat="server" visible="false">
                    <div class="alert">
                        A assinatura do seu plano foi cancelada com sucesso!</br>
                        Seu plano continuará válido até <strong>
                            <asp:Label ID="lblPlanoRecorrenteValidade" Style="color: #555555" runat="server"></asp:Label></strong>
                    </div>
                </div>
                <div id="divErroCancelamentoAssinatura" class="modal-footer" runat="server" visible="false">
                    <div class="alert">
                        Ocorreu um erro e não foi possível cancelar a assinatura do seu plano,</br>
                        Para efetivar o cancelamento, por favor entre em contato através do <strong>0800 41 2400</strong>.
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="upCancelamento" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <asp:Panel ID="pnlCancelarPlanoRecorrenteEtapa01" runat="server" Visible="false">
                <div class="mobile-cancelation">
                    <!-- Etapa 01 -->
                    <section id="etapa01">
                        <div class="cancelation-title">
                            <h2>Que pena!</h2>
                            <h4>Você está sendo visto pelas empresas!</h4>
                        </div>
                        <div class="cancelation-facets">
                            <div class="cancelation-facet">
                                <div>
                                    <svg style="width: 32px; height: 32px" viewBox="0 0 24 24">
                                        <path d="M9,13A3,3 0 0,0 12,16A3,3 0 0,0 15,13A3,3 0 0,0 12,10A3,3 0 0,0 9,13M20,19.59V8L14,2H6A2,2 0 0,0 4,4V20A2,2 0 0,0 6,22H18C18.45,22 18.85,21.85 19.19,21.6L14.76,17.17C13.96,17.69 13,18 12,18A5,5 0 0,1 7,13A5,5 0 0,1 12,8A5,5 0 0,1 17,13C17,14 16.69,14.96 16.17,15.75L20,19.59Z" />
                                    </svg>
                                </div>
                                <div>
                                    <h6>seu cv foi visualizado</h6>
                                    <h3>
                                        <asp:Label runat="server" ID="lblCvVisualizado"></asp:Label>
                                        VEZES</h3>
                                </div>
                            </div>
                            <div class="cancelation-facet">
                                <div>
                                    <svg style="width: 32px; height: 32px" viewBox="0 0 24 24">
                                        <path d="M9.5,3A6.5,6.5 0 0,1 16,9.5C16,11.11 15.41,12.59 14.43,13.73L14.71,14H15.5L20.5,19L19,20.5L14,15.5V14.71L13.73,14.43C12.59,15.41 11.11,16 9.5,16A6.5,6.5 0 0,1 3,9.5A6.5,6.5 0 0,1 9.5,3M9.5,14C11.11,14 12.5,13.15 13.32,11.88C12.5,10.75 11.11,10 9.5,10C7.89,10 6.5,10.75 5.68,11.88C6.5,13.15 7.89,14 9.5,14M9.5,5A1.75,1.75 0 0,0 7.75,6.75A1.75,1.75 0 0,0 9.5,8.5A1.75,1.75 0 0,0 11.25,6.75A1.75,1.75 0 0,0 9.5,5Z" />
                                    </svg>
                                </div>
                                <div>
                                    <h6>você apareceu nas pesquisas</h6>
                                    <h3>
                                        <asp:Label runat="server" ID="lblApareceuNasPesquisas"></asp:Label>
                                        VEZES</h3>
                                </div>
                            </div>
                            <div class="cancelation-facet">
                                <div>
                                    <svg style="width: 32px; height: 32px" viewBox="0 0 24 24">
                                        <path d="M4,18V20H8V18H4M4,14V16H14V14H4M10,18V20H14V18H10M16,14V16H20V14H16M16,18V20H20V18H16M2,22V8L7,12V8L12,12V8L17,12L18,2H21L22,12V22H2Z" />
                                    </svg>
                                </div>
                                <div>
                                    <h6>pesquisaram seu perfil</h6>
                                    <h3>
                                        <asp:Label runat="server" ID="lblPesquisaramSeuPerfil"></asp:Label>
                                        EMPRESAS </h3>
                                </div>
                            </div>
                        </div>
                        <div class="cancelation-actions ">
                            <div>
                                <asp:Button ID="btnFicar01" runat="server" PostBackUrl="~/SalaVip.aspx" Text="Não, espere! Vou ficar" CssClass="btn btn-lg btn-success" />
                            </div>
                            <div>
                                <asp:LinkButton runat="server" CssClass="btn btn-link" OnClick="lnkCancelarEtapa01_Click" ID="lnkCancelarEtapa01">Cancelar minha assinatura</asp:LinkButton>
                            </div>
                        </div>
                    </section>
                    <!-- Fim Etapa 01 -->
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlCancelarPlanoRecorrenteEtapa02" runat="server" Visible="false">
                <!-- Etapa 02 -->
                <div class="mobile-cancelation">
                    <section id="etapa02">
                        <div class="cancelation-title">
                            <h4>Antes de cancelar sua assinatura, confira o que fizemos juntos nesses dias:</h4>
                        </div>
                        <div class="cancelation-facets">
                            <div class="cancelation-facet">
                                <div>
                                    <svg style="width: 32px; height: 32px" viewBox="0 0 24 24">
                                        <path d="M20,11H4V8H20M20,15H13V13H20M20,19H13V17H20M11,19H4V13H11M20.33,4.67L18.67,3L17,4.67L15.33,3L13.67,4.67L12,3L10.33,4.67L8.67,3L7,4.67L5.33,3L3.67,4.67L2,3V19A2,2 0 0,0 4,21H20A2,2 0 0,0 22,19V3L20.33,4.67Z" />
                                    </svg>
                                </div>
                                <div>
                                    <h3>
                                        <asp:Label runat="server" ID="lblCvVisualizado02"></asp:Label>
                                        VEZES</h3>
                                    <h6>seu cv foi visualizado</h6>
                                </div>
                            </div>
                            <div class="cancelation-facet">
                                <div>
                                    <svg style="width: 32px; height: 32px" viewBox="0 0 24 24">
                                        <path d="M12,17.27L18.18,21L16.54,13.97L22,9.24L14.81,8.62L12,2L9.19,8.62L2,9.24L7.45,13.97L5.82,21L12,17.27Z" />
                                    </svg>
                                </div>
                                <div>
                                    <h3>
                                        <asp:Label runat="server" ID="lblApareceuNasPesquisas02"></asp:Label>
                                        VEZES</h3>
                                    <h6>você apareceu nas pesquisas</h6>
                                </div>
                            </div>
                        </div>
                        <div class="cancelation-actions ">
                            <div>
                                <asp:Button ID="btnFicar02" runat="server" PostBackUrl="~/SalaVip.aspx" Text="Não, espere! Vou ficar" CssClass="btn btn-lg btn-success" />
                            </div>
                            <div>
                                <asp:LinkButton ID="lnkCancelarEtapa02" runat="server" OnClick="lnkCancelarEtapa02_Click" CssClass="btn btn-link">Cancelar minha assinatura</asp:LinkButton>
                                <%--<input type="button" class="btn btn-link" value="Cancelar minha assinatura" id="btn-cancel-02"></input>--%>
                            </div>
                        </div>
                    </section>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlCancelarPlanoRecorrenteEtapa03" runat="server" Visible="false">
                <div class="mobile-cancelation">
                    <!-- Etapa 03 -->
                    <section id="etapa03">
                        <div class="cancelation-title">
                            <h2>Volte quando quiser!</h2>
                            <h4>Ajude a melhorar nosso serviço respondendo apenas uma pergunta.</br>
                Sua opinião é muito importante para nós.
                            </h4>
                        </div>
                        <div class="cancelation-question row">
                            <div>
                                <h4>Qual o real motivo do cancelamento?</h4>
                            </div>
                            <div>
                                <asp:CustomValidator id="cvMotivo" runat="server" ErrorMessage="Selecione pelo menos um dos motivos"
                                     ClientValidationFunction="ValCheckBoxList" ></asp:CustomValidator>
                                <asp:CheckBoxList runat="server" ID="cblMotivoCancelar" onclick="selectdItem();" RepeatDirection="Vertical" TextAlign="Right">
                                </asp:CheckBoxList>
                            </div>
                            <div class="togglevisibility" style="display: none;">
                                <asp:TextBox ID="txtOutro" TextMode="MultiLine" CssClass=""  runat="server" placeholder="Quais? (opcional)"></asp:TextBox>
                            </div>
                        </div>

                        <div class="cancelation-actions ">
                            <div>
                                <asp:Button runat="server" ID="lnkCancelarEtapa03"  OnClick="lnkCancelarEtapa03_Click"
                                      OnClientClick="trackEvent('vip-meu-plano','Click','CancelarPlanoRecorrenteFinalizou'); return true;" CssClass="btn btn-log btn-sucess" Text="Cancelar minha assinatura" />
                                <%--<input type="button" class="btn btn-lg btn-success " value="Cancelar minha assinatura" id="btn-cancel-03"></input>--%>
                            </div>
                        </div>
                    </section>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlCancelarPlanoRecorrenteEtapa04" runat="server" Visible="false">
                <div class="mobile-cancelation">
                    <!-- Etapa 04 -->
                    <section id="etapa04">
                        <div class="cancelation-title">
                            <h2>Até breve!</h2>
                        </div>
                        <div class="cancelation-success-icon">
                            <svg style="width: 112px; height: 112px" viewBox="0 0 24 24">
                                <path fill="#8bc34a" d="M12,2A10,10 0 0,1 22,12A10,10 0 0,1 12,22A10,10 0 0,1 2,12A10,10 0 0,1 12,2M11,16.5L18,9.5L16.59,8.09L11,13.67L7.91,10.59L6.5,12L11,16.5Z" />
                            </svg>
                        </div>
                        <div class="cancelation-title">
                            <h4>Sua assinatura do Plano VIP foi cancelada.</br>
                         Seu acesso estará disponível até <strong><asp:Label id="lblDataVencimentoVip" runat="server"></asp:Label></strong>.
                            </h4>
                        </div>
                        <div class="cancelation-actions ">
                            <div>
                                <asp:Button ID="lnkCancelarEtapa04" runat="server" Text="Página inicial" OnClick="lnkCancelarEtapa04_Click" CssClass="btn btn-sucess" />
                                <%--<input type="button" class="btn btn-success" value="Página inicial" id="btn-cancel-04"></input>--%>
                            </div>
                        </div>
                    </section>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <asp:UpdatePanel ID="upWebCallBack" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlWebCallBack" runat="server" Visible="true">
                <uc:WebCallBack_Modais ID="ucWebCallBack_Modais" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
