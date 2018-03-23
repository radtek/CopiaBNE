<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailRetornoPF.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.EmailRetornoPF" %>
<div class="painel_configuracao_conteudo">
    <%-- bloco Email de Retorno Pessoa Fisica --%>
    <h3 class="titulo_configuracao_content">
        <asp:Label ID="Label1" Text="E-mail de Retorno - Pessoa Física" runat="server"></asp:Label>
    </h3>
    <asp:Panel ID="idEmailRetornoPessoaFisica" runat="server" CssClass="blocodados">
        <%--Linha Confirmacao Cadastro Curriculo--%>
        <div class="linha">
            <asp:Label ID="lblConfirmacaoCadastroCurriculo" runat="server" Text="Confirmação de Cadastro do Currículo"
                CssClass="label_principal texto_normal" />
            <div class="container_campo texto_normal">
                <componente:AlfaNumerico ID="txtConfirmacaoCadastroCurriculo" runat="server" CssClassTextBox="textbox_padrao"
                    ValidationGroup="SalvarEmailRetorno" />
            </div>
        </div>
        <%--FIM Confirmacao Cadastro Curriculo --%>
        <%--Linha Passo a Passodo VIP--%>
        <div class="linha">
            <asp:Label ID="lblPassoaPassodoVIP" runat="server" Text="Passo a Passo do VIP" CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <componente:AlfaNumerico ID="txtPassoaPassodoVIP" runat="server" CssClassTextBox="textbox_padrao"
                    ValidationGroup="SalvarEmailRetorno" />
            </div>
        </div>
        <%--FIM Passo a Passodo VIP --%>
        <%--Linha Abandono de Compra PF--%>
        <div class="linha">
            <asp:Label ID="lblAbandonoCompraPF" runat="server" Text="Abandono de Compra PF" CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <componente:AlfaNumerico ID="txtAbandonoCompraPF" runat="server" CssClassTextBox="textbox_padrao"
                    ValidationGroup="SalvarEmailRetorno" />
            </div>
        </div>
        <%--FIM Abandono de Compra PF --%>
        <%--Linha Boleto de Renovação do VIP--%>
        <div class="linha">
            <asp:Label ID="lblBoletoRenovacaoVIP" runat="server" Text="Boleto de Renovação do VIP"
                CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <componente:AlfaNumerico ID="txtBoletoRenovacaoVIP" runat="server" CssClassTextBox="textbox_padrao"
                    ValidationGroup="SalvarEmailRetorno" />
            </div>
        </div>
        <%--FIM  Boleto de Renovação do VIP --%>
        <%--Linha Convite para Atualizar Currículo--%>
        <div class="linha">
            <asp:Label ID="lblConviteAtualizarCurriculo" runat="server" Text="Convite para Atualizar Currículo"
                CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <componente:AlfaNumerico ID="txtConviteAtualizarCurriculo" runat="server" CssClassTextBox="textbox_padrao"
                    ValidationGroup="SalvarEmailRetorno" />
            </div>
        </div>
        <%--FIM Convite para Atualizar Currículo--%>
        <%--Linha Convite para Atualizar Currículo--%>
        <div class="linha">
            <asp:Label ID="Label2" runat="server" Text="Peça Ajuda" CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <componente:AlfaNumerico ID="txtPecaAjuda" runat="server" CssClassTextBox="textbox_padrao"
                    ValidationGroup="SalvarEmailRetorno" />
            </div>
        </div>
        <%--FIM Convite para Atualizar Currículo--%>
    </asp:Panel>
    <%-- email Retorno Pessoa Juridica --%>
    <h3 class="titulo_configuracao_content">
        <asp:Label ID="Label3" Text="E-mail de Retorno - Pessoa Jurídica" runat="server"></asp:Label>
    </h3>
    <asp:Panel ID="Panel2" runat="server" CssClass="blocodados">
        <%--Linha Confirma Cadastro Empresa--%>
        <div class="linha">
            <asp:Label ID="lblConfirmaCadastroEmpresa" runat="server" Text="Confirmação de Cadastro da Empresa"
                CssClass="label_principal texto_normal" />
            <div class="container_campo texto_normal">
                <componente:AlfaNumerico ID="txtConfirmaCadastroEmpresa" runat="server" CssClassTextBox="textbox_padrao"
                    ValidationGroup="SalvarEmailRetorno" />
            </div>
        </div>
        <%--FIM Confirmacao de Publica--%>
        <%--Linha Confimacao Publicacao Vagas--%>
        <div class="linha">
            <asp:Label ID="lblConfimacaoPublicacaoVagas" runat="server" Text="Confirmação de Publicação de Vagas"
                CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <componente:AlfaNumerico ID="txtConfimacaoPublicacaoVagas" runat="server" CssClassTextBox="textbox_padrao"
                    ValidationGroup="SalvarEmailRetorno" />
            </div>
        </div>
        <%--FIM Confirmação de Publicação de vagas --%>
        <%--Linha Confirmação de Criação de Site Trabalhe Conosco--%>
        <div class="linha">
            <asp:Label ID="lblConfirmacaoCriacaoSiteTrabalheConosco" runat="server" Text="Confirmação de Criação de Exclusivo Banco de Currículos"
                CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <componente:AlfaNumerico ID="txtConfirmacaoCriacaoSiteTrabalheConosco" runat="server"
                    CssClassTextBox="textbox_padrao" ValidationGroup="SalvarEmailRetorno" />
            </div>
        </div>
        <%--FIM   Confirmação de Criação de Site Trabalhe Conosco --%>
        <%--Linha Envio de Solicitação R1--%>
        <div class="linha">
            <asp:Label ID="lblEnvioSolicitacaoR1" runat="server" Text="Envio de Solicitação R1"
                CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <componente:AlfaNumerico ID="txtEnvioSolicitacaoR1" runat="server" CssClassTextBox="textbox_padrao"
                    ValidationGroup="SalvarEmailRetorno" />
            </div>
        </div>
        <%--FIM Convite para Atualizar Currículo--%>
        <%--Linha Abandono de Compra PJ--%>
        <div class="linha">
            <asp:Label ID="lblAbandonoCompraPJ" runat="server" Text="Abandono de Compra PJ" CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <componente:AlfaNumerico ID="txtAbandonoCompraPJ" runat="server" CssClassTextBox="textbox_padrao"
                    ValidationGroup="SalvarEmailRetorno" />
            </div>
        </div>
        <%--FIM Convite para Atualizar Currículo--%>
    </asp:Panel>
    <%-- email Retorno Geral --%>
    <h3 class="titulo_configuracao_content">
        <asp:Label ID="Label4" Text="E-mail de Retorno - Geral" runat="server"></asp:Label>
    </h3>
    <asp:Panel ID="Panel1" runat="server" CssClass="blocodados">
        <%--Linha Fale com o Presidente--%>
        <div class="linha">
            <asp:Label ID="lblFalePresidente" runat="server" Text="Fale com o Presidente" CssClass="label_principal texto_normal" />
            <div class="container_campo texto_normal">
                <componente:AlfaNumerico ID="txtFalePresidente" runat="server" CssClassTextBox="textbox_padrao"
                    ValidationGroup="SalvarEmailRetorno" />
            </div>
        </div>
        <%--FIM Fale com o Presidente--%>
        <%--Linha Agradecimento--%>
        <div class="linha">
            <asp:Label ID="lblAgradecimento" runat="server" Text="Agradecimento" CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <componente:AlfaNumerico ID="txtAgradecimento" runat="server" CssClassTextBox="textbox_padrao"
                    ValidationGroup="SalvarEmailRetorno" />
            </div>
        </div>
        <%--FIM Agradecimento--%>
        <%--Linha CContato--%>
        <div class="linha">
            <asp:Label ID="lblContato" runat="server" Text="Contato" CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <componente:AlfaNumerico ID="txtContato" runat="server" CssClassTextBox="textbox_padrao"
                    ValidationGroup="SalvarEmailRetorno" />
            </div>
        </div>
        <%--FIM Contato --%>
    </asp:Panel>
    <%-- Botão Salvar --%>
    <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
        <asp:Button ID="btnSalvar" runat="server" CssClass="botao_padrao painelinterno" Text="Salvar"
            CausesValidation="true" ValidationGroup="SalvarEmailRetorno" OnClick="btnSalvar_Click" />
    </asp:Panel>
</div>
<%-- bloco Email de Retorno --%>