<%@ Page AutoEventWireup="true" CodeBehind="PesquisaVaga.aspx.cs" EnableEventValidation="true"
    Inherits="BNE.Web.PesquisaVaga" Language="C#" MasterPageFile="~/Master/Principal.Master" %>

<%@ Register Src="UserControls/Modais/ucModalLogin.ascx" TagName="ucModalLogin" TagPrefix="uc1" %>
<%@ Register Src="UserControls/Modais/ModalConfirmacaoEnvioCurriculo.ascx" TagName="ModalConfirmacaoEnvioCurriculo"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/Modais/VerDadosEmpresa.ascx" TagName="VerDadosEmpresa"
    TagPrefix="uc4" %>
<%@ Register Src="UserControls/Modais/ModalQuestionarioVagas.ascx" TagName="ModalQuestionarioVagas"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/PaginacaoPesquisaVaga.ascx" TagName="Paginacao" TagPrefix="uc3" %>
<%@ Register Src="UserControls/Modais/ModalDegustacaoCandidatura.ascx" TagName="ModalDegustacaoCandidatura"
    TagPrefix="uc" %>
<%@ Register Src="UserControls/Modais/CompartilhamentoVaga.ascx" TagName="ModalCompartilhamentoVaga"
    TagPrefix="uc" %>
<%@ Import Namespace="BNE.Web.Code" %>

<asp:Content ID="conHead" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/PesquisaVaga.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicScript runat="server" Src="/js/local/PesquisaVaga.js" type="text/javascript" />
    <script type='text/javascript'>
        var urlredirect = 'http://<%= BNE.Web.Code.UIHelper.RecuperarURLAmbiente() %>/FecharJanelaFacebook.aspx';

        var abrirCompartilhamentoFacebook = function (funcao, salarioCidade, url) {
            window.open("https://www.facebook.com/dialog/feed?app_id=450210698402153&picture=<%= RecuperarUrlIconeFacebook() %>&display=popup&name=Vaga+de+" + funcao + "&caption=" + salarioCidade + "&link=" + url + "&redirect_uri=" + urlredirect, "facebook", "resizable=yes,width=640,height=240");
        }

        window.mostrarSucessoFacebook = function () {
            __doPostBack('__Page', 'mostrarSucessoFacebook');
        }
    </script>
</asp:Content>
<asp:Content ID="conConteudo" ContentPlaceHolderID="cphConteudo" runat="server">
    <h1>Resultados da Pesquisa de Vagas</h1>
    <asp:UpdatePanel ID="upRlvVaga" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Repeater ID="rptVagas" runat="server" OnItemDataBound="rptVagas_ItemDataBound"
                OnItemCommand="rptVagas_ItemCommand">
                <ItemTemplate>
                    <%-- pnlVaga --%>
                    <asp:Panel ID="pnlVaga" runat="server" CssClass='<%# RecuperarCssPainelVaga(DynamicHelper.GetValue(Container.DataItem,"Flg_BNE_Recomenda"), DynamicHelper.GetValue(GetDataItem(),"Idf_Origem")) %>'>
                        <div class="qualificacaoVaga <%# (Convert.ToInt32(DynamicHelper.GetValue(GetDataItem(),"Idf_Tipo_Origem")).Equals(3) ? "parceiro" : "bne")%>">
                            <div class="img"><span><%# (Convert.ToInt32(DynamicHelper.GetValue(GetDataItem(),"Idf_Tipo_Origem")).Equals(3) ? "vaga de parceiro do BNE" : "vaga auditada pelo BNE")%></span></div>
                        </div>
                            <%-- pnlVagaColunaEsquerda --%>
                        <asp:Panel ID="pnlVagaColunaEsquerda" CssClass="coluna_esquerda_vaga" runat="server">
                            <div id="lnidentVaga">
                                
                                <div id="lnidentVaga_info" >
           
                                    <asp:Panel ID="pnlVagaWebEstagiosEsquerda" CssClass="coluna_web_estagios_vaga" runat="server" 
                                        Visible='<%# DynamicHelper.GetValue(GetDataItem(),"Des_Tipo_Vinculo").ToString().IndexOf("Estagiário", StringComparison.OrdinalIgnoreCase) >=0 
                                    //|| DynamicHelper.GetValue(GetDataItem(),"Des_Tipo_Vinculo").ToString().IndexOf("Aprendiz", StringComparison.OrdinalIgnoreCase) >=0 
                                    || DynamicHelper.GetValue(GetDataItem(),"Des_Tipo_Vinculo").ToString().IndexOf("Estágio", StringComparison.OrdinalIgnoreCase) >=0  %>'>
                                        <a href="http://employerestagios.com.br/?utm_source=bne&utm_campaign=gridvagas&utm_term=bne_pesquisa_vaga" target="_blank">
                                            <asp:Image CssClass="imagem_web_estagios" ToolTip="Vaga WebEstágios" runat="server" ImageUrl="~/img/icone-we.png" />
                                        </a>
                                    </asp:Panel>
 
                                    <div id="lnidentVaga_txt">
                                        <h2>
                                            <span><%# !String.IsNullOrEmpty(DynamicHelper.GetValue(GetDataItem(),"Des_Deficiencia").ToString()) && !DynamicHelper.GetValue(GetDataItem(),"Des_Deficiencia").ToString().Equals("Nenhuma")  ? "<i class='fa fa-wheelchair fa-1x ico_pcd'></i>" : "" %></span>
                                            <%--<asp:Literal ID="litAntesDoTituloVaga" runat="server" Text="Estágio para " Visible='<%# DynamicHelper.GetValue(GetDataItem(),"Des_Funcao").ToString() != "Estagiário"  && DynamicHelper.GetValue(GetDataItem(),"Des_Tipo_Vinculo").ToString().IndexOf("Estágio", StringComparison.OrdinalIgnoreCase) >=0%>'></asp:Literal>--%>
                                            <asp:Literal ID="litTituloVaga" runat="server" Text='<%# string.IsNullOrEmpty(DynamicHelper.GetValue(GetDataItem(), "Des_Curso").ToString()) ? DynamicHelper.GetValue(GetDataItem(),"Des_Funcao") : "Estágio para "+DynamicHelper.GetValue(GetDataItem(),"Des_Curso")  %>'></asp:Literal>
                                            <asp:Label ID="lblQuantidadeVagas" CssClass="texto_quantidade_vagas" runat="server"
                                                Text='<%# "(" +DynamicHelper.GetValue(GetDataItem(),"Qtd_Vaga") + (Convert.ToInt32(DynamicHelper.GetValue(GetDataItem(),"Qtd_Vaga")).Equals(1) ? " vaga" : " vagas") + ")" %>'></asp:Label>
                                        </h2>
                                        <p class="texto_resumo_vaga" >
                                            <asp:literal ID="litResumodadosVaga" runat="server" Text='<%#  DynamicHelper.GetValue(GetDataItem(), "Nme_Cidade").ToString().Contains("/") ? DynamicHelper.GetValue(GetDataItem(), "Nme_Cidade") + (string.IsNullOrEmpty(DynamicHelper.GetValue(GetDataItem(), "Nme_Bairro").ToString()) ? "":" no bairro "+DynamicHelper.GetValue(GetDataItem(), "Nme_Bairro")) : DynamicHelper.GetValue(GetDataItem(), "Nme_Cidade") + "/" + DynamicHelper.GetValue(GetDataItem() , "Sig_Estado") + (string.IsNullOrEmpty(DynamicHelper.GetValue(GetDataItem(), "Nme_Bairro").ToString()) ? "":" no bairro "+DynamicHelper.GetValue(GetDataItem(), "Nme_Bairro"))  %>'> </asp:Literal><br />
                                            <asp:Literal ID="litSalario" runat="server" Text='<%# RetornarDesricaoSalario(DynamicHelper.GetValue(GetDataItem(), "Vlr_Salario_De"), DynamicHelper.GetValue(GetDataItem(), "Vlr_Salario_Para")) %>'></asp:Literal>
                                        </p>
                                    </div>
                                   
                                </div>
                                <div id="lnidentVaga_compartilhar" >
                                    <asp:Panel runat="server" ID="pnlVagaColunaMeio" CssClass="coluna_meio_compartilhar">
                                        <div>
                                            <asp:UpdatePanel runat="server" ID="upRedeSocial" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <span class="labelCompartilhar">COMPARTILHE:</span>
                                                    <asp:LinkButton runat="server" CssClass="fa fa-facebook-square compartilharFacebook" ID="ibtCompartilharFacebook"
                                                        CommandName="facebook" CommandArgument='<%# DynamicHelper.GetValue(GetDataItem(),"Idf_Vaga") %>' ToolTip="Compartilhe no Facebook"
                                                        OnClientClick='<%# String.Format("abrirCompartilhamentoFacebook(&apos;{0}&apos;,&apos;{1}&apos;,&apos;{2}&apos;)", 
                                                                                                                                    HttpUtility.UrlEncode(DynamicHelper.GetValue(GetDataItem(),"Des_Funcao").ToString()), 
                                                                                                                                    HttpUtility.UrlEncode(RetornarDesricaoSalario(DynamicHelper.GetValue(GetDataItem(),"Vlr_Salario_De"), DynamicHelper.GetValue(GetDataItem(),"Vlr_Salario_Para")) + " - " + DynamicHelper.GetValue(GetDataItem(),"Nme_Cidade").ToString() + "/" + DynamicHelper.GetValue(GetDataItem(),"Sig_Estado").ToString()), 
                                                                                                                                    HttpUtility.UrlEncode(DynamicHelper.GetValue(GetDataItem(),"Url_Vaga").ToString())) %>' />
                                                    <asp:LinkButton runat="server" CssClass="fa fa-envelope-square compartilharEmail" ID="ibtCompartilharEmail" CommandName="compartilharEmail" CommandArgument='<%# DynamicHelper.GetValue(GetDataItem(),"Idf_Vaga") %>' ToolTip="Compartilhe por e-mail" />
                                                    <asp:HiddenField runat="server" ID="hfUrlVaga" Value='<%# DynamicHelper.GetValue(GetDataItem(),"Url_Vaga") %>' />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>                                
                                    </asp:Panel>
                                </div>
                                
                            </div>
                            


                            <asp:Panel ID="pnlAtribuicoes" runat="server" Visible='<%# !Convert.ToBoolean(DynamicHelper.GetValue(GetDataItem(),"Flg_Vaga_Rapida")) && !String.IsNullOrEmpty(DynamicHelper.GetValue(GetDataItem(),"Des_Atribuicoes").ToString()) %>'>
                                <h3>Atribuições:</h3>
                                <p class="texto_atribuicoes">
                                    <asp:Literal ID="litTextoAtribuicoes" runat="server" Text='<%# TruncarTextoAtribuicoes(DynamicHelper.GetValue(GetDataItem(),"Des_Atribuicoes")) %>'></asp:Literal>
                                </p>
                            </asp:Panel>
                            <%-- pnlMaisDadosVaga --%>
                            <asp:Panel CssClass="painel_mais_dados_vaga" ID="pnlMaisDadosVaga" runat="server">
                                <p class="texto_atribuicoes_completo">
                                    <asp:Literal ID="litTextoAtribuicoesCompleto" Visible='<%# !Convert.ToBoolean(DynamicHelper.GetValue(GetDataItem(),"Flg_Vaga_Rapida")) %>'
                                        runat="server" Text='<%# DynamicHelper.GetValue(GetDataItem(),"Des_Atribuicoes") %>'></asp:Literal>
                                </p>
                                <asp:Panel ID="pnlBeneficio" runat="server" Visible='<%# !String.IsNullOrEmpty(DynamicHelper.GetValue(GetDataItem(),"Des_Beneficio").ToString()) %>'>
                                    <h3>Benefícios:</h3>
                                    <p>
                                        <asp:Literal ID="litTextoBeneficios" runat="server" Text='<%# DynamicHelper.GetValue(GetDataItem(),"Des_Beneficio") %>'></asp:Literal>
                                    </p>
                                </asp:Panel>
                                <asp:Panel ID="pnlRequisito" runat="server" Visible='<%# !Convert.ToBoolean(DynamicHelper.GetValue(GetDataItem(),"Flg_Vaga_Rapida")) || !String.IsNullOrEmpty(DynamicHelper.GetValue(GetDataItem(),"Des_Escolaridade") + DynamicHelper.GetValue(GetDataItem(),"Des_Requisito").ToString()) %>'>
                                    <h3>Requisitos:</h3>
                                    <asp:Literal ID="litTextoRequisitos" runat="server" Text='<%# DynamicHelper.GetValue(GetDataItem(),"Des_Escolaridade") + " " + DynamicHelper.GetValue(GetDataItem(),"Des_Requisito") %>'></asp:Literal>
                                    <asp:Panel ID="pnlDeficiencia" runat="server" Visible='<%# !String.IsNullOrEmpty(DynamicHelper.GetValue(GetDataItem(),"Idf_Deficiencia").ToString()) && !DynamicHelper.GetValue(GetDataItem(),"Idf_Deficiencia").ToString().Equals("0") %>'>
                                        Deficiência:
                                        <asp:Literal ID="litTextoDeficienciaVaga" runat="server" Text='<%# DynamicHelper.GetValue(GetDataItem(),"Des_Deficiencia") %>'></asp:Literal>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlDisponibilidadeTrabalho" runat="server" Visible='<%# !String.IsNullOrEmpty(DynamicHelper.GetValue(GetDataItem(),"Des_Disponibilidade").ToString())%>'>
                                        Disponibilidade de Trabalho:
                                        <asp:Literal ID="litTextoDisponibilidadeTrabalho" runat="server" Text='<%# DynamicHelper.GetValue(GetDataItem(),"Des_Disponibilidade") %>'></asp:Literal>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlTipoVinculoTrabalho" runat="server" Visible='<%# !String.IsNullOrEmpty(DynamicHelper.GetValue(GetDataItem(),"Des_Tipo_Vinculo").ToString())%>'>
                                        Tipo de Contrato:
                                        <asp:Literal ID="litTextoTipoVinculoTrabalho" runat="server" Text='<%# DynamicHelper.GetValue(GetDataItem(),"Des_Tipo_Vinculo") %>'></asp:Literal>
                                    </asp:Panel>
                                </asp:Panel>
                                <p class="texto_codigo_vaga">
                                    Código da vaga:
                                    <asp:Literal ID="litTextoCodigoVaga" runat="server" Text='<%# DynamicHelper.GetValue(GetDataItem(),"Cod_Vaga") %>'></asp:Literal>
                                </p>
                                <p class="texto_codigo_vaga">
                                    Vaga Anunciada em
                                    <asp:Literal ID="litDataPublicacao" runat="server" Text='<%# DynamicHelper.GetValue(GetDataItem(),"Dta_Abertura") %>'></asp:Literal>
                                </p>
                            </asp:Panel>
                        </asp:Panel>

                        <%-- pnlVagaColunaDireita --%>
                        <asp:Panel CssClass="coluna_direita_vaga" ID="pnlVagaColunaDireita" runat="server">
                            <asp:UpdatePanel ID="upControles" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                     <p class="btn">  
                                         <asp:LinkButton CssClass="botao_quero_me_candidatar btn-primary" ID="ibtQueroMeCandidatar"
                                            CommandName="candidatar" CommandArgument='<%# DynamicHelper.GetValue(GetDataItem(),"Idf_Vaga") %>'
                                            runat="server" Visible='<%# !Convert.ToBoolean(DynamicHelper.GetValue(GetDataItem(),"Flg_Candidatou")) %>'>  <i class="fa fa-file-text"></i>  Quero me Candidatar </asp:LinkButton>
                                     </p>
                                    <%-- Exibe o painel com a informação Já enviei --%>
                                    <asp:Panel CssClass="painel_ja_enviei" ID="pnlJaEnviei" runat="server" Visible='<%# Convert.ToBoolean(DynamicHelper.GetValue(GetDataItem(),"Flg_Candidatou")) %>'>
                                        <img alt="" src="/img/img_check_vaga_ja_enviei.png" />
                                        <p>
                                            Já enviei!
                                        </p>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <ul>
                                <li><span>
                                    <asp:HyperLink ID="hlkMaisDadosDaVaga" NavigateUrl="javascript:;" runat="server">
                                        <asp:HiddenField runat="server" ID="IdentificadorVaga" Value='<%# DynamicHelper.GetValue(GetDataItem(),"Idf_Vaga") %>' />
                                        Mais dados da vaga
                                    </asp:HyperLink>
                                </span></li>
                                <li id="liMaisDadosEmpresa" runat="server">
                                    <asp:UpdatePanel ID="upBtlkMaisDadosDaEmpresa" runat="server" UpdateMode="Conditional"
                                        RenderMode="Inline">
                                        <ContentTemplate>
                                            <asp:LinkButton ID="btlkMaisDadosDaEmpresa" NavigateUrl="javascript:;" CommandName="empresa"
                                                CommandArgument='<%# DynamicHelper.GetValue(GetDataItem(),"Idf_Vaga") %>' runat="server">Mais dados da empresa</asp:LinkButton>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </li>
                            </ul>

                        </asp:Panel>
                    </asp:Panel>
                    <%-- FIM: pnlVaga --%>
                </ItemTemplate>
            </asp:Repeater>
            <uc3:Paginacao ID="Paginacao" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- Painel Botões -->
    <asp:UpdatePanel ID="upBotoes" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
                <asp:Button ID="btnVoltar" runat="server" Text="Voltar" CausesValidation="false"
                    CssClass="botao_padrao" OnClick="btnVoltar_Click" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <!-- FIM: Painel botoes -->
</asp:Content>
<asp:Content ID="conRodape" ContentPlaceHolderID="cphRodape" runat="server">
    <asp:UpdatePanel ID="upL" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlL" runat="server" Visible="false">
                <uc1:ucModalLogin ID="ucModalLogin" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upVDE" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlVDE" runat="server" Visible="false">
                <uc4:VerDadosEmpresa ID="ucVerDadosEmpresa" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upMCEC" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlMCEC" runat="server" Visible="false">
                <uc:ModalConfirmacaoEnvioCurriculo ID="ucModalConfirmacaoEnvioCurriculo" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upMDC" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlMDC" runat="server" Visible="false">
                <uc:ModalDegustacaoCandidatura ID="ucModalDegustacaoCandidatura" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upMQV" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlMQV" runat="server" Visible="false">
                <uc:ModalQuestionarioVagas ID="ucModalQuestionarioVagas" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upCompVaga" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlCompVaga" runat="server" Visible="false">
                <uc:ModalCompartilhamentoVaga ID="ucModalCompartilhamentoVaga" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
