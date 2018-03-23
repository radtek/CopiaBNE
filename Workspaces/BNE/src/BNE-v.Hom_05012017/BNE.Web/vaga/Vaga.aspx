<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="Vaga.aspx.cs" Inherits="BNE.Web.vaga.Vaga" %>

<%@ Register Src="~/UserControls/Modais/ucModalLogin.ascx" TagName="ucModalLogin"
    TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Modais/ModalQuestionarioVagas.ascx" TagName="ModalQuestionarioVagas"
    TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Modais/ModalConfirmacaoEnvioCurriculo.ascx" TagName="ModalConfirmacaoEnvioCurriculo"
    TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Modais/ModalDegustacaoCandidatura.ascx" TagName="ModalDegustacaoCandidatura"
    TagPrefix="uc" %>
<%@ Register Src="~/UserControls/Modais/ConfirmacaoExclusao.ascx" TagName="ModalConfirmaExclusao" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/Vaga.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicScript runat="server" Src="/js/local/Vaga.js" type="text/javascript" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:Panel ID="pnlVaga" runat="server">
        <h2>
            <asp:Literal ID="litTituloVaga" runat="server">
            </asp:Literal>
            <asp:Label ID="lblQuantidadeVagas" CssClass="texto_quantidade_vagas" runat="server">
            </asp:Label>
        </h2>
        <p class="texto_resumo_vaga">
            <asp:Literal ID="litResumoDadosVaga" runat="server">
            </asp:Literal>
        </p>
        <asp:Panel ID="pnlAtribuicoes" runat="server">
            <h3>Atribuições:</h3>
            <p class="texto_atribuicoes">
                <asp:Literal ID="litTextoAtribuicoes" runat="server">
                </asp:Literal>
            </p>
        </asp:Panel>
        <asp:Panel ID="pnlBeneficios" runat="server">
            <h3>Benefícios:</h3>
            <p>
                <asp:Literal ID="litTextoBeneficios" runat="server">
                </asp:Literal>
            </p>
        </asp:Panel>
        <asp:Panel ID="pnlRequisitos" runat="server">
            <h3>Requisitos:</h3>
            <asp:Literal ID="litTextoRequisitos" runat="server">
            </asp:Literal>
            <asp:Panel ID="pnlDeficiencia" runat="server">
                Deficiência:
                <asp:Literal ID="litTextoDeficienciaVaga" runat="server">
                </asp:Literal>
            </asp:Panel>
            <asp:Panel ID="pnlDisponibilidadeTrabalho" runat="server">
                Disponibilidade de Trabalho:
                <asp:Literal ID="litTextoDisponibilidadeTrabalho" runat="server">
                </asp:Literal>
            </asp:Panel>
            <asp:Panel ID="pnlTipoVinculoTrabalho" runat="server">
                Tipo de Contrato:
                <asp:Literal ID="litTextoTipoVinculoTrabalho" runat="server">
                </asp:Literal>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="pnlDadosEmpresa" runat="server">
            <h3>Dados básicos da Empresa:</h3>
            <asp:Label runat="server" CssClass="texto_dados_empresa" Text="Número de Funcionários:"></asp:Label>
            <asp:Literal ID="lblNumeroFuncionarioValor" runat="server"></asp:Literal>
            <asp:Label runat="server" CssClass="texto_dados_empresa" Text="Cidade:"></asp:Label>
            <asp:Literal ID="lblCidadeValor" runat="server"></asp:Literal>
            <asp:Label runat="server" CssClass="texto_dados_empresa" Text="Bairro:"></asp:Label>
            <asp:Literal ID="lblBairroValor" runat="server"></asp:Literal>
            <asp:Label runat="server" CssClass="texto_dados_empresa" Text="Cadastrada em:"></asp:Label>
            <asp:Literal ID="lblDataCadastroValor" runat="server"></asp:Literal>
            <asp:Label runat="server" CssClass="texto_dados_empresa" Text="Currículos Visualizados:"></asp:Label>
            <asp:Literal ID="lblCurriculosVisualizadosValor" runat="server"></asp:Literal>
            <asp:Label runat="server" CssClass="texto_dados_empresa" Text="Vagas Divulgadas:"></asp:Label>
            <asp:Literal ID="lblVagasDivulgadasValor" runat="server"></asp:Literal>
        </asp:Panel>
        <p class="texto_codigo_vaga">
            Código da vaga:
            <asp:Literal ID="litTextoCodigoVaga" runat="server">
            </asp:Literal>
            &nbsp;&nbsp;&nbsp; Vaga Anunciada em
            <asp:Literal ID="litDataPublicacao" runat="server">
            </asp:Literal>
        </p>
        <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
            <asp:UpdatePanel ID="upControles" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:ImageButton ID="btiQueroMeCandidatar" CssClass="botao_quero_me_candidatar" AlternateText="Quero me candidatar!"
                        ImageUrl="/img/btn_quero_me_candidatar.png" runat="server" OnClick="btiQueroMeCandidatar_Click"
                        CausesValidation="False" />
                    <%-- Exibe o painel com a informação Já enviei --%>
                    <asp:Panel CssClass="painel_ja_enviei" ID="pnlJaEnviei" runat="server">
                        <img alt="" src="/img/img_check_vaga_ja_enviei.png" />
                        <p>
                            Já enviei!
                        </p>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </asp:Panel>
    <%-- FIM: pnlVaga --%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <uc:ucModalLogin ID="ucModalLogin" runat="server" />
    <uc:ModalQuestionarioVagas ID="ucModalQuestionarioVagas" runat="server" />
    <uc:ModalConfirmacaoEnvioCurriculo ID="ucModalConfirmacaoEnvioCurriculo" runat="server" />
    <uc:ModalDegustacaoCandidatura ID="ucModalDegustacaoCandidatura" runat="server" />
    <uc:ModalConfirmaExclusao ID="ucModalConfirmacaoExclusao" runat="server" />
</asp:Content>
