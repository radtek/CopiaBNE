<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SalaVipCurso.aspx.cs" Inherits="BNE.Web.SalaVipCurso" %>

<%@ Register TagPrefix="uc" TagName="ucModalLogin" Src="/UserControls/Modais/ucModalLogin.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicScript runat="server" Src="/js/local/SalaVipCurso.js" type="text/javascript" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SalaVipCurso.css" type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="painel_padrao">
        <div class="painel_padrao_topo">
            &nbsp;
        </div>
        <asp:Panel runat="server" ID="pnlBanner" CssClass="cursos_banner">
            <div class="btns_abas">
                <div id="divVerDescricao" class="descricao_curso_azul" onclick="Visualizar('D')">
                    <a id="A1" href="javascript:;" onclick="Visualizar('D')" runat="server"></a>
                </div>
                <div id="divVerConteudo" class="conteudo_curso_cinza" onclick="Visualizar('C')">
                    <a id="A2" href="javascript:;" onclick="Visualizar('C')" runat="server"></a>
                </div>
            </div>
        </asp:Panel>
        <div class="container_topico_redesocial">
            <div class="container_nome_curso">
                <asp:Literal runat="server" ID="litNomeCurso"></asp:Literal>
            </div>
            <div class="container_redesocial">
                <div class="facebook">
                    <asp:Literal runat="server" ID="litLikeButtonFacebook"></asp:Literal>
                </div>
                <div class="tweeter">
                    <asp:Literal runat="server" ID="litTweetButton"></asp:Literal>
                </div>
            </div>
        </div>
        <br />
        <br />
        <br />
        <br />
        <br />
        <div class="texto_paragrafo_descricao" id="descricao_curso">
            <asp:Literal runat="server" ID="litDescricao"></asp:Literal>
            <br />
            <span class="destaque_aumentado">
                <asp:Literal runat="server" ID="litInstrutor"></asp:Literal></span>
            <br />
            <span class="normal">
                <asp:Literal runat="server" ID="litInstrutorAssinatura"></asp:Literal></span>
            <br />
            <br />
            <br />
            <p>
                <img src="/img/cursos/img_enfeite_links.png" class="efeite_links" />
                <asp:UpdatePanel ID="upBtlMatriculese" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:LinkButton runat="server" ID='btlMatriculese' CssClass="matriculese" Text="Matricule-se já"
                            OnClick="btlMatriculese_OnClick" CausesValidation="False"></asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--| <a href="#">Enviar para e-mail</a>--%>
            </p>
        </div>
        <div class="texto_paragrafo_conteudo" id="conteudo_curso" style="display: none;">
            <asp:Literal runat="server" ID="litConteudo"></asp:Literal>
        </div>
        <div class="">
            <img src="/img/cursos/box_azul_resumo_ementa.png" class="box_azul_resumo_ementa">
        </div>
        <div class="texto_box">
            <h2>
                <b>Informações</b>
            </h2>
            <p>
                <b>Modalidade:</b><br />
                <asp:Literal runat="server" ID="litModalidade"></asp:Literal>
            </p>
            <p>
                <b>Público Alvo:</b><br />
                <asp:Literal runat="server" ID="litPublicoAlvo"></asp:Literal>
            </p>
            <p>
                <b>Duração Máxima:</b><br />
                <asp:Literal runat="server" ID="litDuracao"></asp:Literal>
                horas
            </p>
            <p>
                <b>Certificação:</b><br />
                <asp:Literal runat="server" ID="litCertificado"></asp:Literal>
            </p>
            <p>
                <h2>
                    <b>Investimento</b>
                </h2>
            </p>
            <p>
                <font size="3px"><b>R$
                    <asp:Literal runat="server" ID="litValor"></asp:Literal>
                    à vista </b></font>
                <asp:Panel ID="pnlParcela" runat="server">
                    <div class="curso_valor_parcela">
                        ou em
                        <asp:Literal runat="server" ID="litQuantidadeParcela"></asp:Literal>
                        vezes de R$
                        <asp:Literal runat="server" ID="litValorParcela"></asp:Literal>
                    </div>
                </asp:Panel>
            </p>
        </div>
        <div class="botao_matricule_se">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:ImageButton ID="btnQueroCursar" runat="server" CausesValidation="false" OnClick="btnQueroCursar_Click"
                        Text="Quero Cursar" ImageUrl="/img/cursos/btn_matricule_se.png" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="comentariosFacebook">
            <asp:Literal runat="server" ID="litComentariosFacebook"></asp:Literal>
        </div>
        <div class="curtirFacebook">
            <asp:Literal runat="server" ID="litLikeButtonFacesFacebook"></asp:Literal>
        </div>
        <div class="limpa">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <uc:ucModalLogin ID="ucModalLogin" runat="server" />
    <asp:Panel ID="pnlRedirecionamento" runat="server" CssClass="modal_conteudo candidato reduzida"
        Style="display: none">
        <h2 class="titulo_modal">
            <span>Cursos</span></h2>
        <asp:UpdatePanel ID="upBtiModalRedirecionamento" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:ImageButton CssClass="botao_fechar_modal" ID="btiModalRedirecionamento" ImageUrl="/img/botao_padrao_fechar.gif"
                    runat="server" CausesValidation="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="alinha_texto_modal">
            <span>Você será direcionando para o site do nosso parceiro: <strong>Tecla Cursos</strong>
            </span>
            <div class="painel_botoes cursos">
                <asp:UpdatePanel ID="upSiteParceiro" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HyperLink runat="server" ID="hlSiteParceiro" CssClass="me_leve_para_la " Target="_new">IR PARA TECLA CURSOS
                        </asp:HyperLink>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </asp:Panel>
    <asp:HiddenField ID="hfRedirecionamento" runat="server" />
    <AjaxToolkit:ModalPopupExtender ID="mpeRedirecionamento" BackgroundCssClass="modal_fundo"
        runat="server" PopupControlID="pnlRedirecionamento" TargetControlID="hfRedirecionamento"
        CancelControlID="btiModalRedirecionamento" RepositionMode="RepositionOnWindowResizeAndScroll" />
</asp:Content>
