<%@ Control Language="C#" AutoEventWireup="false" CodeBehind="ModalVendaCIA.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.ModalVendaCIA" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/ModalVendaCIA.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlVendaCIA" runat="server" CssClass="modal_confirmacao_registro candidato reduzida" Style="display: none">

    <asp:ImageButton CssClass="modal_fechar_404" ID="btiFechar" ImageUrl="/img/modal_nova/btn_fechar_modal.png"
        runat="server" CausesValidation="false" />
    <!--<div class="modal_nova_imagem">
        <asp:Image runat="server" ImageUrl="/img/modal_nova/img_alerta_bne.png" ID="imgAlerta"
            AlternateText="Alerta" />
    </div>-->
    <div class="modal_nova_conteudo">
        <asp:UpdatePanel ID="upNomeReponsavel" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:Literal ID="lblNomeReponsavel" runat="server" EnableViewState="false" />
                <!-- Plano 40 -->
                <asp:Panel ID="pnDesconto40" runat="server">
                    <p class="text_modal_vip">
                        Você ganhou <b>
                            <asp:Label ID="lblDescontoExperimente" runat="server"></asp:Label>% de desconto</b> para experimentar o site agora!
                    </p>
                    <%--<p class="center">Cupom de desconto: <b>Des50sempl</b></p>                    --%>
                    <div class="btnVoltar">
                        <asp:LinkButton runat="server" ID="btlQueroComprarPlano" CausesValidation="False"
                            Text="Utilizar desconto" OnClick="btlQueroComprarPlano_Click" CssClass="botao_ver_planos">
                        </asp:LinkButton>
                    </div>
                    <small class="aviso_cupom">*Desconto não cumulativo</small>
                </asp:Panel>
                <!-- Plano 15 -->
                <asp:Panel ID="pnDesconto15" runat="server">
                    <p class="text_modal_vip">
                        Reative agora sua assinatura com <b>
                            <asp:Label runat="server" ID="lblDescontoTempoMaior"></asp:Label>% de desconto!</b>
                    </p>
                    <p class="center">Cupom de desconto: <b>des15reat60</b></p>
                    <asp:LinkButton runat="server" ID="lnkLigarAgora" CausesValidation="False"
                        Text="LIGAR AGORA" CssClass="botao_ver_planos" OnClick="btiLigarAgora_Click">
                    </asp:LinkButton>
                    <%--<a id="teste" data-toggle="modal" data-target="#nomeDaModal" style="display:none;">Ligar agora</a>--%>
                    <small class="aviso_cupom">*Desconto não cumulativo</small>
                </asp:Panel>
                <!-- Plano 3 -->
                <asp:Panel ID="pnDesconto3" runat="server">
                    <p class="text_modal_vip">Reative agora sua assinatura!</p>
                    <p>Fale agora com o consultor <b>
                        <asp:Label runat="server" ID="lblVendedor" /></b> e informe o seu cupom de desconto!</p>
                    <p class="center">Cupom de desconto: <b>des03reat60</b></p>
                    <asp:LinkButton runat="server" ID="lnkLigueAgora3" CausesValidation="False"
                        Text="LIGAR AGORA" CssClass="botao_ver_planos" OnClick="btiLigarAgora_Click">
                    </asp:LinkButton>
                    <small class="aviso_cupom">*Desconto não cumulativo</small>
                </asp:Panel>
                <!-- O BNE Liga pra você -->
                <asp:Panel runat="server" ID="pnLigueMe">
                    <!-- Default -->
                    <asp:UpdatePanel ID="upMyModalComercial" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div>
                                <h4 id="myModalLabel">O BNE LIGA PARA VOCÊ</h4>
                            </div>
                            <div id="divJanelaLigacao" runat="server" style="font-size: 16px; color: #666">
                                <p>Para a sua comodidade preencha o telefone abaixo, após clicar no botão, por favor aguarde, em instantes ligaremos para você.</p>
                                <div class="input-group">
                                    <div class="input-modal-vendacia-label">Nome:</div>
                                    <div style="width: 400px;">
                                        <asp:TextBox ID="txtNome" runat="server" MaxLength="60" ValidationGroup="btnReceberLigacao"
                                            CssClass="textbox_padrao spanNome form-control input-large input-group-adjust input-modal-vendacia"></asp:TextBox>
                                    </div>
                                    <div class="modal-vendacia-label-ddd">DDD:</div>
                                    <div class="modal-vendacia-label-telefone">Telefone:</div>
                                    <div style="height: 90px; width: 400px;">
                                        <componente:Telefone ID="txtTelefone" runat="server" ValidationGroup="btnReceberLigacao" Tipo="FixoCelular"
                                            CssClassTextBoxDDD="textbox_padrao spanddd form-control input-large input-group-adjust input-modal-vendacia-ddd"
                                            CssClassTextBoxFone="textbox_padrao spanfone form-control input-large input-group-adjust input-modal-vendacia-fone" />
                                    </div>
                                    <div class="input-group-addon" id="basic-addon2">
                                        <asp:LinkButton runat="server" CssClass="botao_ver_planos" Style="border-radius: 0px;"
                                            ValidationGroup="btnReceberLigacao" OnClick="btnReceberLigacao_Click"><i class="fa fa-phone"></i> 
                                            RECEBER LIGAÇÃO
                                        </asp:LinkButton>
                                    </div>

                                </div>
                            </div>
                            <div id="divAguardarContato" class="modal-footer" runat="server" visible="false">
                                <div class="alert">
                                    <%--<button type="button" class="close" data-dismiss="alert">&times;</button>--%>
                                    <strong>Obrigado</strong>, nossa equipe entrará em contato em instantes!
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <!-- Mensagens -->
                    <asp:UpdatePanel ID="upMyModalMensagem" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="modal-header">
                                <h4>FALE COM O BNE</h4>
                            </div>
                            <div id="divJanelaMensagem" runat="server" class="modal-body" style="font-size: 16px; color: #666">
                                <p>Nosso horário de atendimento encerrou, nos envie uma mensagem!</p>
                                <div class="form-group">
                                    <div class="input-modal-vendacia-label">Nome:</div>
                                    <div style="width: 400px;">
                                        <div>
                                            <asp:RequiredFieldValidator ID="rfNomeClienteMsg" runat="server" ControlToValidate="txtNomeClienteMsg" Text="Campo Obrigatório."
                                                ValidationGroup="btnEnviar"></asp:RequiredFieldValidator>
                                        </div>
                                        <input id="txtNomeClienteMsg" runat="server" placeholder="Nome" type="text"
                                            class="form-control input-large input-group-adjust input-modal-vendacia" name="nomeClienteMsg" aria-describedby="basic-addon2">
                                    </div>
                                    <div class="modal-vendacia-label-ddd">DDD:</div>
                                    <div class="modal-vendacia-label-telefone">Telefone:</div>
                                    <div style="width: 400px;">
                                        <div class="input-group">
                                            <componente:Telefone ID="txtTelefoneClienteMsg" runat="server" ValidationGroup="btnEnviar" Tipo="FixoCelular" CssClassTextBoxDDD="textbox_padrao spandddMsg form-control input-large input-group-adjust input-modal-vendacia-ddd" CssClassTextBoxFone="textbox_padrao spanfoneMsg form-control input-large input-group-adjust input-modal-vendacia-fone" />
                                        </div>
                                    </div>
                                    <div style="clear: both"></div>
                                    <div class="input-modal-vendacia-label">E-mail:</div>
                                    <div style="width: 400px;">

                                        <div>
                                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmailClienteMsg" ErrorMessage="E-mail Inválido"
                                                ValidationGroup="btnEnviar" ValidationExpression="\w+([-+.&apos;]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="rfEmailClienteMsg" runat="server" ControlToValidate="txtEmailClienteMsg" Text="Campo Obrigatório." ValidationGroup="btnEnviar"></asp:RequiredFieldValidator>

                                        </div>
                                        <input id="txtEmailClienteMsg" runat="server" type="text" placeholder="E-mail" columns="50" maxlength="50"
                                            class="form-control input-large input-group-adjust input-modal-vendacia" name="nomeClienteMsg" aria-describedby="basic-addon2">
                                    </div>
                                    <div class="input-modal-vendacia-label">Mensagem:</div>
                                    <div style="width: 400px;">
                                        <div>
                                            <asp:RequiredFieldValidator ID="rfClienteMensagem" runat="server" ControlToValidate="txtMensagemCliente" Text="Campo Obrigatório." ValidationGroup="btnEnviar"></asp:RequiredFieldValidator>
                                        </div>
                                        <textarea id="txtMensagemCliente" runat="server" rows="5" placeholder="Escreva aqui sua mensagem!" style="width: 350px!important; font-size: 16px; font-family: Arial, Helvetica, sans-serif" maxlength="200"></textarea>

                                    </div>
                                    <div style="width: 400px;" class="input-group-addon">
                                        <asp:LinkButton ID="lnkEnviarMsg" runat="server" class="btn btn-warning btn-large" Style="border-radius: 0px;" OnClick="btnEnviarMensagem_Click"
                                            ValidationGroup="btnEnviar">
                                                <i class="fa fa-paper-plane-o"></i>ENVIAR</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <div id="divAguardarContatoMsg" runat="server" class="modal-footer" visible="false">
                                <div class="alert" style="margin-bottom: 20px;">
                                    <%--<button type="button" class="close" data-dismiss="alert">&times;</button>--%>
                                    <strong>Obrigado</strong>, nossa equipe entrará em contato no <strong>horário comercial</strong>!
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
                <asp:HiddenField ID="hddIdFilial" runat="server" />
                <asp:HiddenField ID="hddNomeCliente" runat="server" />
                <asp:HiddenField ID="hddDesconto" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Panel>
<asp:HiddenField ID="hf" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeVendaCIA" PopupControlID="pnlVendaCIA" runat="server"
    CancelControlID="btiFechar" TargetControlID="hf">
</AjaxToolkit:ModalPopupExtender>
<script type="text/javascript">
    function AbreModalLigarAgora() {
        $('#teste').trigger('click');
        alert('ha!');
    }
</script>
