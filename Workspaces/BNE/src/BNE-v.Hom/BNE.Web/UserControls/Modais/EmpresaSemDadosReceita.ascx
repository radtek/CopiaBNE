<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmpresaSemDadosReceita.ascx.cs" Inherits="BNE.Web.UserControls.Modais.EmpresaSemDadosReceita" %>
<Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/EmpresaBloqueada.css" type="text/css" rel="stylesheet" />
<asp:Panel ID="pnlEmpresaSemDadosReceita" runat="server" CssClass="modal_confirmacao_registro candidato reduzida" Style="display: none; background-color: #ebeff2 !important; height: auto !important;">
    <asp:ImageButton CssClass="botao_fechar_modal" ID="btiFechar" ImageUrl="/img/modal_nova/btn_fechar_modal.png"
        runat="server" CausesValidation="false" />
   
       <asp:Panel CssClass="coluna_esquerda bloqueio" ID="pnlColunaEsquerda" runat="server">
        <asp:Panel CssClass="painel_bronquinha" ID="pnlEsquerdaBloquearCandidato" runat="server">
              <div class="alert-icon" id="divAlert" runat="server"></div>
        </asp:Panel>
    </asp:Panel>

   <div class="container_confirmacao_candidatura">
      <p class="texto_modal_empresa_bloqueada">
        Recebemos o cadastro de sua empresa! 
            Estamos realizando as liberações para seu acesso e entraremos em contato em seguida. 
         <br><br />
            Este processo é rápido, por favor aguarde.
        </p>
    </div>
</asp:Panel>
<asp:HiddenField ID="hf" runat="server" />
<AjaxToolkit:ModalPopupExtender ID="mpeEmpresaSemDadosReceita" PopupControlID="pnlEmpresaSemDadosReceita"
    runat="server" CancelControlID="btiFechar" TargetControlID="hf">
</AjaxToolkit:ModalPopupExtender>
