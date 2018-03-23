<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfiguracoesValores.ascx.cs"
    Inherits="BNE.Web.UserControls.Forms.SalaAdministrador.ConfiguracoesValores" %>
<div class="painel_configuracao_conteudo">
    <%-- bloco Valores Pessoa Fisica --%>
    <h3 class="titulo_configuracao_content">
        <asp:Label ID="idValoresPF" Text="Valores - Pessoa Fisíca" runat="server"></asp:Label>
    </h3>
    <asp:Panel ID="idEmailRetornoPessoaFisica" runat="server" CssClass="blocodados">
        <%--Linha Valor do Salário Mínimo --%>
        <div class="linha">
            <asp:Label ID="lblValorSalarioMínimo" runat="server" Text="Valor do Salário Mínimo"
                CssClass="label_principal texto_normal" />
            <div class="container_campo texto_normal">
                <componente:ValorDecimal ID="txtValorSalarioMínimo" runat="server" CssClassTextBox="textbox_padrao small"
                    ValidationGroup="SalvarValores" />
            </div>
        </div>
        <%--FIM Confirmacao Cadastro Curriculo --%>
        <%--Linha Limite de dias para atualizar currículo --%>
        <div class="linha">
            <asp:Label ID="lblLimiteDiasAtualizarCurriculo" runat="server" Text="Limite de dias para atualizar currículo"
                CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <componente:AlfaNumerico Tipo="Numerico" ID="txtLimiteDiasAtualizarCurriculo" runat="server"
                    CssClassTextBox="textbox_padrao small" ValidationGroup="SalvarValores" />
            </div>
        </div>
        <%--FIM Limite de dias para atualizar currículo --%>
        <%--Linha Idade  Miníma--%>
        <div class="linha">
            <asp:Label ID="lblIdadeMiníma" runat="server" Text="Idade Miníma" CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <componente:AlfaNumerico Tipo="Numerico" ID="txtIdadeMinima" runat="server" CssClassTextBox="textbox_padrao small"
                    ValidationGroup="SalvarValores" /> 
                <label class="container_campo">Máxima</label></div>
                <div class="container_campo">
                <componente:AlfaNumerico Tipo="Numerico" ID="txtIdadeMaxima" runat="server" CssClassTextBox="textbox_padrao small"
                    ValidationGroup="SalvarValores" />
                <label>Anos</label>
            </div>
        </div>
        <%--FIM Idade  Miníma --%>
        <%--Linha Limite de dias para acessar o currículo sem descontar--%>
        <div class="linha">
            <asp:Label ID="lblLimiteDiasAcessarCurriculoDescontar" runat="server" Text="Limite de dias para acessar o currículo sem descontar"
                CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <componente:AlfaNumerico Tipo="Numerico" ID="txtLimiteDiasAcessarCurriculoDescontar"
                    runat="server" CssClassTextBox="textbox_padrao small" ValidationGroup="SalvarValores" />
            </div>
        </div>
        <%--FIM  Limite de dias para acessar o currículo sem descontar --%>
        <%--Linha Quantidade de dias após a emissão para o vencimento do boleto PF--%>
        <div class="linha">
            <asp:Label ID="lblQuantidadeDiasEmissaoVencimentoboletoPF" runat="server" Text="Quantidade de dias após a emissão para o vencimento do boleto PF"
                CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <componente:AlfaNumerico Tipo="Numerico" ID="txtQuantidadeDiasEmissaoVencimentoboletoPF"
                    runat="server" CssClassTextBox="textbox_padrao small" ValidationGroup="SalvarValores" />
            </div>
        </div>
        <%--FIM Quantidade de dias após a emissão para o vencimento do boleto PF--%>
        <%--FIM Convite para Atualizar Currículo--%>
    </asp:Panel>
    <%-- email Valores Pessoa Juridica --%>
    <h3 class="titulo_configuracao_content">
        <asp:Label ID="lblValoresPessoaJuridica" Text="Valores Pessoa Juridica" runat="server"></asp:Label>
    </h3>
    <asp:Panel ID="Panel2" runat="server" CssClass="blocodados">
        <%--Linha Quantidade de dias que a vaga anunciada ficará ativa--%>
        <div class="linha">
            <asp:Label ID="lblQuantidadeDiasVagaAnunciadaAtiva" runat="server" Text="Quantidade de dias que a vaga anunciada ficará ativa"
                CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <componente:AlfaNumerico Tipo="Numerico" ID="txtQuantidadeDiasVagaAnunciadaAtiva"
                    runat="server" CssClassTextBox="textbox_padrao small" ValidationGroup="SalvarValores" />
            </div>
        </div>
        <%--FIM Quantidade de dias que a vaga anunciada ficará ativa--%>
        <%--Linha Quantidade limite de usuários por empresa--%>
        <div class="linha">
            <asp:Label ID="lblQuantidadeLimiteUsuariosEmpresa" runat="server" Text="Quantidade limite de usuários por empresa" CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <componente:AlfaNumerico Tipo="Numerico" ID="txtQuantidadeLimiteUsuariosEmpresa"
                    runat="server" CssClassTextBox="textbox_padrao small" ValidationGroup="SalvarValores" />
            </div>
        </div>
        <%--FIM Quantidade limite de usuários por empresa --%>
        <%--Linha Quantidade limite de usuário master por empresa--%>
        <div class="linha">
            <asp:Label ID="lblQuantidadeLimiteUsuarioMasterEmpresa" runat="server" Text="Quantidade limite de usuário master por empresa"
                CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <componente:AlfaNumerico Tipo="Numerico" ID="txtQuantidadeLimiteUsuarioMasterEmpresa"
                    runat="server" CssClassTextBox="textbox_padrao small" ValidationGroup="SalvarValores" />
            </div>
        </div>
        <%--FIM Quantidade limite de usuário master por empresa --%>
        <%--LinhaQuantidade de dias após a emissão para o vencimento do boleto PJ--%>
        <div class="linha">
            <asp:Label ID="lblQuantidadeDiaEmissaoVencimentoBoletoPJ" runat="server" Text="Quantidade de dias após a emissão para o vencimento do boleto PJ"
                CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <componente:AlfaNumerico Tipo="Numerico" ID="txtQuantidadeDiaEmissaoVencimentoBoletoPJ"
                    runat="server" CssClassTextBox="textbox_padrao small" ValidationGroup="SalvarValores" />
            </div>
        </div>
        <%--FIM Quantidade de dias após a emissão para o vencimento do boleto PJ--%>
        <%--Linha Quantidade miníma de SMS para venda--%>
        <div class="linha">
            <asp:Label ID="lblQuantidadeMinímaSMSVenda" runat="server" Text="Quantidade miníma de SMS para venda"
                CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <componente:AlfaNumerico Tipo="Numerico" ID="txtQuantidadeMinimaSMSVenda" runat="server"
                    CssClassTextBox="textbox_padrao small" ValidationGroup="SalvarValores" />
            </div>
        </div>
        <%--FIM Quantidade miníma de SMS para venda--%>
        <%--Linha  Valor do SMS Avulso--%>
        <div class="linha">
            <asp:Label ID="lblValorSMSAvulso" runat="server" Text="Valor do SMS Avulso" CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <componente:ValorDecimal ID="txtValorSMSAvulso" runat="server" CssClassTextBox="textbox_padrao small"
                    ValidationGroup="SalvarValores" />
            </div>
        </div>
        <%--FIM Valor do SMS Avulso--%>
    </asp:Panel>
    <%-- email Retorno Geral --%>
    <asp:Panel ID="Panel1" runat="server" CssClass="blocodados extras" Visible="false">
        <%--LinhaTipo de Plano--%>
        <div class="linha">
            <asp:Label ID="lblTipoPlano" runat="server" Text="Tipo de Plano" CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <Employer:ComboCheckbox runat="server" ID="ccFiltrarlblTipoPlano" EmptyMessage="--"
                    AllowCustomText="false" CssClass="checkbox_large" Width="100">
                </Employer:ComboCheckbox>
            </div>
        </div>
        <%--FIM Tipo de Plano--%>
        <%--Linha Valor --%>
        <div class="linha">
            <asp:Label ID="lblValor" runat="server" Text="Valor" CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <asp:TextBox ID="txtValor" runat="server" CssClass="textbox_padrao small" />
            </div>
        </div>
        <%--FIM Valor --%>
        <%--Linha Validade--%>
        <div class="linha">
            <asp:Label ID="lblValidade" runat="server" Text="Validade" CssClass="label_principal texto_normal" />
            <div class="container_campo">
                <asp:TextBox ID="txtValidade" runat="server" CssClass="textbox_padrao small" />
            </div>
        </div>
        <%--FIM Validade --%>
    </asp:Panel>
    <%-- Botão Salvar --%>
    <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
        <asp:Button ID="btnSalvar" runat="server" CssClass="botao_padrao painelinterno" Text="Salvar"
            CausesValidation="true" ValidationGroup="SalvarValores" 
            onclick="btnSalvar_Click" />
    </asp:Panel>
</div>
