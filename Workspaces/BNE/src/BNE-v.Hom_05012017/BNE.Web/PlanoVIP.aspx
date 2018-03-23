<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="PlanoVIP.aspx.cs" Inherits="BNE.Web.PlanoVIP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/PlanoVIP.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/UserControls/Modais/ModalTemplateIndique3Amigos.css" type="text/css" rel="stylesheet" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.6.3/css/font-awesome.min.css">
    <link href='http://fonts.googleapis.com/css?family=Roboto: 400,500,700' rel='stylesheet' type='text/css'>

    <script type="text/javascript">
        function destacaValor() {
            $('.preco_desconto_mensal,.preco_desconto_tri').hide().fadeIn('slow');
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="escolha_plano">
        <div class="bredcrumb">
            <span class="tit_bdc">Passos para ser VIP:</span>
            <span class="txt_bdc"><strong><u>1 - Escolha o Acesso</u></strong></span>
            <span class="txt_bdc">/</span>
            <span class="txt_bdc">2 - Formas de Pagamento</span>
            <span class="txt_bdc">/</span>
            <span class="txt_bdc">3 - Confirmação</span>
            <span class="txt_bdc">-</span>
            <span class="txt_bdc">Parabéns, você é <strong>VIP!</strong> <i class="fa fa-trophy"></i></span>
        </div>
        <h4 class="tit_pag">Escolha o Acesso</h4>
        <div class="cod_promo">
            <span><strong>Código Promocional:</strong></span>
            <asp:TextBox ID="txtCodigoCredito" runat="server" MaxLength="200" AutoPostBack="true" OnTextChanged="txtCodigoCredito_TextChanged"></asp:TextBox>
            <asp:Button ID="btnValidarCodigoCredito" runat="server" CssClass="cod_promo" Text="Usar Código" OnClick="btnValidarCodigoCredito_Click" />

        </div>
        <div class="clear"></div>
        <br />
        <section id="selecionaTpPlanoVIP">
            <div>
                <div class="nomePlanoVIP mensal">Mensal</div>
                <ul>
                    <li><i class="fa fa-check"></i>Jornal de Vagas por e-mail</li>
                    <li><i class="fa fa-check"></i>Candidaturas ilimitadas</li>
                    <li><i class="fa fa-check"></i>Pesquisa salarial</li>
                    <li><i class="fa fa-check"></i>Destaque de currículo nas buscas</li>
                    <li><i class="fa fa-check"></i>Contato direto com a empresa</li>
                    <li><i class="fa fa-check"></i>Saiba quais empresas viram seu currículo</li>
                </ul>
                <div class="valorPlano mensal">
                    <div>
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upPrecoDescontoMensal" RenderMode="Inline">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblPrecoDescontoMensal"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div>taxa única</div>
                </div>
                <div class="queroPlano mensal">
                    <asp:Button ID="btnPlanoMensal" runat="server" CssClass="azul_m" Text="Eu Quero Este" OnClick="btnPlanoMensal_Click" />
                </div>
            </div>
            <div>
                <div class="melhorOpcao">Melhor Opção</div>
                <div class="nomePlanoVIP assinatura">Assinatura</div>
                <ul>
                    <li><i class="fa fa-check"></i>Jornal de Vagas por e-mail</li>
                    <li><i class="fa fa-check"></i>Candidaturas ilimitadas</li>
                    <li><i class="fa fa-check"></i>Pesquisa salarial</li>
                    <li><i class="fa fa-check"></i>Destaque de currículo nas buscas</li>
                    <li><i class="fa fa-check"></i>Contato direto com a empresa</li>
                    <li><i class="fa fa-check"></i>Saiba quais empresas viram seu currículo</li>
                </ul>
                <div class="cancelaPlano">Cancele a qualquer momento</div>
                <div class="valorPlano assinatura">
                    <div>
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upPrecoDescontoRecorrente" RenderMode="Inline">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblPrecoDescontoRecorrente"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div>por mês</div>
                </div>
                <div class="queroPlano assinatura">
                    <asp:Button ID="btnPlanoRecorrente" runat="server" Text="Eu Quero" OnClick="btnPlanoRecorrente_Click" />
                </div>
            </div>
            <div>
                <div class="nomePlanoVIP trimestral">Trimestral</div>
                <ul>
                    <li><i class="fa fa-check"></i>Jornal de Vagas por e-mail</li>
                    <li><i class="fa fa-check"></i>Candidaturas ilimitadas</li>
                    <li><i class="fa fa-check"></i>Pesquisa salarial</li>
                    <li><i class="fa fa-check"></i>Destaque de currículo nas buscas</li>
                    <li><i class="fa fa-check"></i>Contato direto com a empresa</li>
                    <li><i class="fa fa-check"></i>Saiba quais empresas viram seu currículo</li>
                </ul>
                <div class="valorPlano trimestral">
                    <div>
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="upPrecoDescontoTri" RenderMode="Inline">
                            <ContentTemplate>
                                <asp:Label runat="server" ID="lblPrecoDescontoTri"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div>taxa única</div>
                </div>
                <div class="queroPlano trimestral">
                    <asp:Button ID="btnPlanoTrimestral" runat="server" Text="Eu Quero Este" OnClick="btnPlanoTrimestral_Click" />
                </div>
            </div>
        </section>
        <asp:Panel ID="panelPlanoPromocional" runat="server" CssClass="box_painel promo" Visible="false">
            <asp:Image runat="server" ID="imgCodigoPromocional" ImageUrl="~/img/CodigoPromocional/9.jpg" AlternateText="Promoção BNE" />
            <asp:RadioButton runat="server" ID="rbPlanoPromocional" GroupName="Plano" Checked="false" CssClass="radiobutton_padrao" Text="Plano Promocional" Enabled="false"></asp:RadioButton>
        </asp:Panel>
        <script>
            function trim(str) {
                return str.replace(/^\s+|\s+$/g, "");
            }

            $(document).ready(function () {
                $('.painel_filtro').hide();
                $('.painel_icones').hide();
                $('.barra_rodape').hide();

                $('#btnIrHome').hide();

                $('.fa-times').click(function () {
                    $('.container_fundo').hide();
                });

                var dadosCandidato = $('#NomeCandidato').html();
                var nomeCandidato = dadosCandidato.split('(');
                $('#ltNomeCandidato').html(trim(nomeCandidato[0].slice(4)));

            });

            function ModalDesconto(acao) {

                if ($('#hddJaClicou').val() == "0") {

                    $('.container_fundo').show();

                    if (acao == 'Home') {
                        $('#btnIrHome').show();
                        $('#cphConteudo_btnSairModal').hide();
                    } else {
                        $('#btnIrHome').hide();
                        $('#cphConteudo_btnSairModal').show();
                    }
                } else {
                    if (acao == 'Home') {
                        window.location.href = "/";
                    } else {
                        $('#cphConteudo_btnSairModal').trigger('click');
                    }
                }
            }

        </script>
        <div class="container_fundo hideModal">
            <div class="modal_container  ">
                <div class="panel_top">
                    <h4 class="tit_modal">Ei... <span id="ltNomeCandidato"></span>, por que você desistiu?</h4>
                    <i class="fa fa-times"></i>
                </div>
                <div class="content_dados">
                    <h2>Leve agora qualquer um dos serviços com</h2>
                    <h1><span>10%</span> de desconto em qualquer plano.</h1>
                    <h2>Só clicar no aproveite para gerar o novo valor!</h2>
                </div>
                <div class="section_btn">

                    <a href="/" id="btnIrHome" type="button" class="btn_cancel">IR PARA A HOME</a>
                    <asp:LinkButton runat="server" ID="btnSairModal" class="btn_cancel" OnClick="btiSair_Click">QUERO SAIR</asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnAproveitarDesconto" OnClick="btnAproveitarDescontoCodigoBNE10_Click" class="btn_send">APROVEITE AGORA!</asp:LinkButton>
                    <%--<button type="button" class="btn_send">APROVEITE AGORA!</button>--%>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
