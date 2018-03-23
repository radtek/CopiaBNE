<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PresidenteEditarRespostaAutomatica.ascx.cs" Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.EditarRespostaAutomatica" %>
<%@ Register
    Src="~/UserControls/Modais/ModalConfirmacao.ascx"
    TagName="ModalConfirmacao"
    TagPrefix="uc1" %>

<div class="painel_configuracao_conteudo">
    <h3 class="titulo_configuracao_content">
        Editar Resposta Automática</h3>
    <%--Linha Titulo ----%>
       <div class="linha titulo_noticia">
       <div class="container_campo texto_normal editar">
            <asp:Label ID="lblConfirmacaoCadastroCurriculo" runat="server" Text="Título da Resposta Automática"
                CssClass="label_principal texto_normal editar" /><br/>
                 <asp:RequiredFieldValidator ID="rfvtxtTitulo" ControlToValidate="txtTitulo" runat="server" ValidationGroup="salvarRespostaAutomatica" ErrorMessage="Campo obrigatório"></asp:RequiredFieldValidator>
                </div>
           
           <div class="texto_normal">
                <asp:TextBox ID="txtTitulo" runat="server" CssClass="textbox_padrao editar campo_obrigatorio" />
            </div>
        </div>
    <%--FIM Titulo --%>
   <asp:RequiredFieldValidator ID="rfvtxtDescRespostaAutomatica" ControlToValidate="txtDescRespostaAutomatica" runat="server" ValidationGroup="salvarRespostaAutomatica" ErrorMessage="Campo obrigatório"></asp:RequiredFieldValidator><br/>
   <asp:TextBox ID="txtDescRespostaAutomatica" runat="server" Columns="65" Rows="8" cssClass="textarea_padrao campo_obrigatorio" 
            TextMode="MultiLine"></asp:TextBox>

    <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
        <asp:Button ID="btnSalvar" runat="server" CssClass="botao_padrao painelinterno" Text="Salvar"
            CausesValidation="true" ValidationGroup="salvarRespostaAutomatica"  onclick="btnSalvar_Click" />
    </asp:Panel>

</div>

<uc1:ModalConfirmacao ID="ucModalConfirmacao" runat="server" />