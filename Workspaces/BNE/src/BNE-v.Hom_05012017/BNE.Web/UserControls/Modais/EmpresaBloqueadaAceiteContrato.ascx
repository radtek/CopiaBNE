<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmpresaBloqueadaAceiteContrato.ascx.cs" Inherits="BNE.Web.UserControls.Modais.EmpresaBloqueadaAceiteContrato" %>
<style>
    .modal_nova_conteudo.aceite-contrato { width: 587px; font-size: 12px; }
        .modal_nova_conteudo.aceite-contrato .contrato { overflow-y: scroll; height: 182px; }
        .modal_nova_conteudo.aceite-contrato .painel_botoes { margin-top: 5px; }
        .modal_nova_conteudo.aceite-contrato .aviso { font-size: 19px; margin: 35px; }
        .modal_nova_conteudo.aceite-contrato .aceite { text-align: center; }
        .modal_nova_conteudo.aceite-contrato .checkbox_aceite input { display: inline; }
        .modal_nova_conteudo.aceite-contrato .checkbox_aceite label { float: none; display: inline; }
        .modal_nova_conteudo.aceite-contrato ul { margin: 0; }
            .modal_nova_conteudo.aceite-contrato ul li { list-style-type: none; }
</style>
<asp:Panel ID="pnlEmpresaBloqueadaAceiteContrato" runat="server" CssClass="modal_nova" Style="display: none;">
    <div class="modal_nova_conteudo aceite-contrato">
        <asp:Panel runat="server" ID="pnlAviso" Visible="False" CssClass="aviso">
            <asp:Literal runat="server" ID="litAviso" Text="Para sua segurança, o usuário master precisa acessar o BNE e aceitar o Termo de utilização do Site. O acesso será liberado imediatamente após o aceite."></asp:Literal>
            <br />
            <br />
            <span>Usuário(s) Master:</span>
            <ul>
                <asp:Repeater runat="server" ID="rptUsuariosMaster">
                    <ItemTemplate>
                        <li>
                            <span><%# Container.DataItem %></span>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
            <div class="painel_botoes">
                <asp:Button ID="btnOK" runat="server" CssClass="botao_padrao" Text="OK" CausesValidation="false"
                    OnClick="btnOk_Click" />
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlContrato" Visible="False">
            <div class="contrato">
                <asp:Literal ID="litContrato" runat="server"></asp:Literal>
            </div>
            <div class="aceite">
                <asp:CheckBox runat="server" ID="ckbAceitoContrato" Text="Eu aceito os termos e condições do contrato" CssClass="checkbox_aceite" Checked="True" Enabled="False" />
            </div>
            <div class="painel_botoes">
                <asp:Button ID="btnConfirmar" runat="server" CssClass="botao_padrao" Text="Confirmar" CausesValidation="false"
                    OnClick="btnConfirmar_Click" />
            </div>
        </asp:Panel>
    </div>
</asp:Panel>
<asp:HiddenField ID="hf" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeEmpresaBloqueadaAceiteContrato" PopupControlID="pnlEmpresaBloqueadaAceiteContrato"
    runat="server" TargetControlID="hf">
</AjaxToolkit:ModalPopupExtender>
