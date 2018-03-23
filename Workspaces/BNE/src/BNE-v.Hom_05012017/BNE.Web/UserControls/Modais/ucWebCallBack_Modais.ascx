<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucWebCallBack_Modais.ascx.cs"
    Inherits="BNE.Web.UserControls.Modais.ucWebCallBack_Modais" %>

<%--<link href="css/local/Bootstrap/bootstrap.css" rel="stylesheet" />
<script src="js/bootstrap/bootstrap.js"></script>--%>

<script type="text/javascript">

    $('#divJanelaLigacao input').each(function () {
        $(this).val('');
    });

    //limpar campos
    $('#divJanelaMensagem input').each(function () {
        $(this).val('');
    });

    $(document).ready(function () {
        $('#cphRodape_ucWebCallBack_Modais_txtTelefone_txtDDD').prop('placeholder', 'DDD');
        $('#cphRodape_ucWebCallBack_Modais_txtTelefone_txtFone').prop('placeholder', 'Telefone');
        $('#cphRodape_ucWebCallBack_Modais_txtTelefoneClienteMsg_txtDDD').prop('placeholder', 'DDD');
        $('#cphRodape_ucWebCallBack_Modais_txtTelefoneClienteMsg_txtFone').prop('placeholder', 'Telefone');
    });
</script>
<style>
    input.spanddd {
        width: 50px;
        margin-left: 35px;
    }

    input.spanNome {
        width: 435px;
        margin-left: 35px;
        margin-bottom: 11px;
    }

    input.spanfone {
        width: 136px;
        margin-left: 10px !important;
    }

    input.spanfoneText {
        width: 50px;
        margin-left: 35px !important;
    }

    input.spandddMsg {
        width: 50px;
        margin-bottom: 10px;
    }

    input.spanfoneMsg {
        width: 136px;
        margin-left: 10px !important;
        margin-bottom: 8px;
    }

    .modal-body .input-group {
        width: 100%;
    }
</style>

<!-- Modal qdo estiver em horário comercial -->
<div id="myModalComercial" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="display: none;" runat="server" clientidmode="Static">
    <asp:UpdatePanel ID="upMyModalComercial" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h4 id="myModalLabel">O BNE LIGA PARA VOCÊ</h4>
            </div>
            <div id="divJanelaLigacao" runat="server" class="modal-body" style="font-size: 16px; color: #666">
                <p>
                    Para a sua comodidade preencha o telefone abaixo,  
                        após clicar no botão, por favor aguarde, em instantes ligaremos para você.
                </p>
                <div class="input-group pull-right">
                    <div style="margin-left: 35px;">Nome:</div>
                    <asp:TextBox ID="txtNome" runat="server" MaxLength="60" ValidationGroup="btnReceberLigacao"
                        CssClass="textbox_padrao spanNome form-control input-large input-group-adjust"></asp:TextBox>
                    <br />
                    <div style="margin-left: 35px;">DDD: &nbsp&nbsp&nbsp&nbsp&nbsp Telefone:</div>

                    <componente:Telefone ID="txtTelefone" runat="server" ValidationGroup="btnReceberLigacao" Tipo="FixoCelular" CssClassTextBoxDDD="textbox_padrao spanddd form-control input-large input-group-adjust" CssClassTextBoxFone="textbox_padrao spanfone form-control input-large input-group-adjust" />
                    <span class="input-group-addon" id="basic-addon2">
                        <asp:LinkButton runat="server" CssClass="btn btn-warning btn-large" Style="border-radius: 0px;"
                            ValidationGroup="btnReceberLigacao" OnClick="btnReceberLigacao_Click"><i class="fa fa-phone"></i> 
                                RECEBER LIGAÇÃO
                        </asp:LinkButton>
                    </span>
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
</div>
<!-- FIM Modal qdo estiver em horário comercial -->

<!-- Modal qdo estiver FORA do horário comercial -->
<div id="myModalMensagem" class="modal hide fade modal-size" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" runat="server" clientidmode="Static">
    <asp:UpdatePanel ID="upMyModalMensagem" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h4 id="myModalLabel">FALE COM O BNE</h4>
            </div>
            <div id="divJanelaMensagem" runat="server" class="modal-body" style="font-size: 16px; color: #666">
                <p>Nosso horário de atendimento encerrou, nos envie uma mensagem!</p>
                <div class="form-group">
                    <p>
                        <div>
                            <asp:RequiredFieldValidator ID="rfNomeClienteMsg" runat="server" ControlToValidate="txtNomeClienteMsg" Text="Campo Obrigatório."
                                ValidationGroup="btnEnviar"></asp:RequiredFieldValidator>
                        </div>
                        <input id="txtNomeClienteMsg" runat="server" placeholder="Nome" type="text"
                            class="form-control input-large input-group-adjust" name="nomeClienteMsg" aria-describedby="basic-addon2">
                    </p>
                    <p>
                        <div class="input-group pull-right">
                            <componente:Telefone ID="txtTelefoneClienteMsg" runat="server" ValidationGroup="btnEnviar" Tipo="FixoCelular" CssClassTextBoxDDD="textbox_padrao spandddMsg form-control input-large input-group-adjust" CssClassTextBoxFone="textbox_padrao spanfoneMsg form-control input-large input-group-adjust" />
                        </div>
                    </p>
                    <p>
                        <div>
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmailClienteMsg" ErrorMessage="E-mail Inválido"
                                ValidationGroup="btnEnviar" ValidationExpression="\w+([-+.&apos;]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="rfEmailClienteMsg" runat="server" ControlToValidate="txtEmailClienteMsg" Text="Campo Obrigatório." ValidationGroup="btnEnviar"></asp:RequiredFieldValidator>

                        </div>
                        <input id="txtEmailClienteMsg" runat="server" type="text" placeholder="E-mail" columns="50" maxlength="50"
                            class="form-control input-large input-group-adjust" name="nomeClienteMsg" aria-describedby="basic-addon2">
                    </p>
                    <p>
                        <div>
                            <asp:RequiredFieldValidator ID="rfClienteMensagem" runat="server" ControlToValidate="txtMensagemCliente" Text="Campo Obrigatório." ValidationGroup="btnEnviar"></asp:RequiredFieldValidator>
                        </div>
                        <textarea id="txtMensagemCliente" runat="server" rows="5" placeholder="Escreva aqui sua mensagem!" style="width: 350px!important; font-size: 16px; font-family: Arial, Helvetica, sans-serif" maxlength="200"></textarea>

                    </p>
                    <p>
                        <asp:LinkButton ID="lnkEnviarMsg" runat="server" class="btn btn-warning btn-large" Style="border-radius: 0px;" OnClick="btnEnviarMensagem_Click"
                            ValidationGroup="btnEnviar">
                                <i class="fa fa-paper-plane-o"></i>ENVIAR</asp:LinkButton>
                    </p>
                </div>
            </div>
            <div id="divAguardarContatoMsg" runat="server" class="modal-footer" visible="false">
                <div class="alert">
                    <%--<button type="button" class="close" data-dismiss="alert">&times;</button>--%>
                    <strong>Obrigado</strong>, nossa equipe entrará em contato no <strong>horário comercial</strong>!
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<!-- FIM Modal qdo estiver FORA do horário comercial -->
