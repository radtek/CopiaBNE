<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master/Principal.Master" CodeBehind="ExcluirCurriculo.aspx.cs" Inherits="BNE.Web.ExcluirCurriculo" %>
<%@ Register Src="~/UserControls/Modais/ucModalLogin.ascx" TagName="ucModalLogin"
    TagPrefix="uc7" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">

    <meta charset="utf-8" />
    <title>BNE - Excluir CV</title>
    <!-- Adjust Screen -->
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <!-- Roboto Font  -->  
    <link href='https://fonts.googleapis.com/css?family=Roboto:400,300,500,700' rel='stylesheet' type='text/css'>
    <link rel="stylesheet" href="css/local/ExcluirCurriculo.css">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <asp:Panel runat="server" ID="pnExcluirCV">
        <asp:HiddenField ID="hfVip" runat="server"/>
        <div id="fluxo-excluir-curriculo">
        <!-- Etapa 01 -->
        <section id="etapa01" runat="server">
            <div ID="pnlNaoVIP" runat="server" class="exclusion-title">
                <h2>Que pena!</h2>
                <h4>Você está sendo visto por várias de nossas empresas!</h4>
            </div>
            <div ID="pnlVip" runat="server" class="exclusion-title">
                <h2>Inativando o currículo seu serviço VIP também será cancelado.</h2>
                <h4>Tem certeza que deseja cancelar o serviço VIP e deixar de receber alertas instantâneos de vagas?</h4>
            </div>
            <div class="exclusion-facets">
                <div class="exclusion-facet" runat="server" id="cardUm" enableviewstate="false">
                    <div>
                        <svg style="width:32px;height:32px" viewBox="0 0 24 24">
                            <path  d="M9,13A3,3 0 0,0 12,16A3,3 0 0,0 15,13A3,3 0 0,0 12,10A3,3 0 0,0 9,13M20,19.59V8L14,2H6A2,2 0 0,0 4,4V20A2,2 0 0,0 6,22H18C18.45,22 18.85,21.85 19.19,21.6L14.76,17.17C13.96,17.69 13,18 12,18A5,5 0 0,1 7,13A5,5 0 0,1 12,8A5,5 0 0,1 17,13C17,14 16.69,14.96 16.17,15.75L20,19.59Z" />
                        </svg>                        
                    </div>
                    <div>
                        <asp:Literal runat="server" ID="ltQuemMeViu"></asp:Literal>
                        <asp:Literal runat="server" ID="ltVagasNaoVisualizadas"></asp:Literal>
                    </div>
                </div>
                <div class="exclusion-facet" runat="server" id="cardDois" enableviewstate="false">
                    <div>
                        <svg style="width:32px;height:32px" viewBox="0 0 24 24">
                            <path  d="M9.5,3A6.5,6.5 0 0,1 16,9.5C16,11.11 15.41,12.59 14.43,13.73L14.71,14H15.5L20.5,19L19,20.5L14,15.5V14.71L13.73,14.43C12.59,15.41 11.11,16 9.5,16A6.5,6.5 0 0,1 3,9.5A6.5,6.5 0 0,1 9.5,3M9.5,14C11.11,14 12.5,13.15 13.32,11.88C12.5,10.75 11.11,10 9.5,10C7.89,10 6.5,10.75 5.68,11.88C6.5,13.15 7.89,14 9.5,14M9.5,5A1.75,1.75 0 0,0 7.75,6.75A1.75,1.75 0 0,0 9.5,8.5A1.75,1.75 0 0,0 11.25,6.75A1.75,1.75 0 0,0 9.5,5Z" />
                        </svg> 
                    </div>
                    <div>
                        <asp:Literal runat="server" ID="ltVezesApareciNabusca"></asp:Literal>
                        <asp:Literal runat="server" ID="ltVagasNaCidadeERegiao"></asp:Literal>
                    </div>
                </div>
                <div class="exclusion-facet" runat="server" id="cardTres" enableviewstate="false">
                    <div>
                        <svg style="width:32px;height:32px" viewBox="0 0 24 24">
                            <path  d="M4,18V20H8V18H4M4,14V16H14V14H4M10,18V20H14V18H10M16,14V16H20V14H16M16,18V20H20V18H16M2,22V8L7,12V8L12,12V8L17,12L18,2H21L22,12V22H2Z" />
                        </svg> 
                    </div>
                    <div>
                        <asp:Literal runat="server" ID="ltEmpresasPesquisaramnoPerfil"></asp:Literal>
                        <asp:Literal runat="server" ID="ltBuscaPerfil"></asp:Literal>
                        <%--<h6>pesquisaram seu perfil</h6>
                        <h3><asp:Label ID="lblEmpresas" runat="server" EnableViewState="false" Text="0"></asp:Label></h3>--%>
                    </div>
                </div>
            </div>
            <div class="exclusion-actions ">
                <div>
                    <input id="bntContinuar" type="button" class="btn btn-lg btn-success " onclick="trackEvent('Excluir-Cv', 'Desistir da Exclusão', 'Não, espere! Vou ficar'); return true;" value="Não, espere! Vou ficar" />
                </div>
                <div>
                    <asp:Button runat="server" ID="btnIrParaExcluir" CssClass="btn btn-link" Text="Inativar meu Currículo" OnClick="btnIrParaExcluirCV_Click" />
                </div>
            </div>
        </section>
        <!-- Etapa 02 -->
        <section id="etapa02" runat="server">
            <div class="exclusion-title">
                <h2>Volte quando quiser!</h2>
                <h4>
                    Ajude a melhorar nosso serviço respondendo apenas uma pergunta.<br>
                    Sua opinião é muito importante para nós.
                </h4>
               <%-- <h6 ID="titVIP" runat="server" Visible="False" >
                    Inativando o currículo seu serviço VIP também será cancelado. Tem certeza que deseja cancelar o serviço VIP e deixar de receber alertas instantâneos de Vagas?
                </h6>--%>
            </div>
            <div class="exclusion-question row">
                <div>
                   <h4>Qual o real motivo do cancelamento?</h4>
                </div>
                <div><asp:Label ID="lblAvisoErro" runat="server" CssClass="aviso_erro" Visible="false"></asp:Label></div>
                <div class="checkbox ">
                    <label>
                        <asp:CheckBox runat="server" ID="chkEmpregado" Text="Já estou empregado" />
                    </label>
                </div>
                <div class="toggleempregado">
                    <div><h6>O emprego que conseguiu, foi através do BNE?</h6></div>

                    <label>
                        <asp:RadioButtonList RepeatColumns="2" runat="server" ID="rblEmpregoBNE" CssClass="radio_motivo">
                            <asp:ListItem Value="1" Text="Sim" Selected></asp:ListItem>
                            <asp:ListItem Value="0" Text="Não"></asp:ListItem>
                        </asp:RadioButtonList>
                    </label>    
                </div>
                <div class="checkbox">
                    <label>
                        <asp:CheckBox runat="server" ID="chkPoucasVagas" Text="Poucas Vagas" />
                    </label>                
                </div>
               
                <div class="checkbox">
                    <label>
                        <asp:CheckBox runat="server" ID="chkMuitosEmails" Text="Recebi muitos e-mails" />
                        
                    </label>                
                </div>
                <div class="checkbox">
                    <label>
                        <asp:CheckBox runat="server" ID="chkOutros" Text="Outros" />
                    </label>                
                </div>
                <div class="toggleoutros">
                    <textarea runat="server" id="txtOutroMotivo" placeholder="Quais? (opcional)"></textarea>           
                </div>
            </div>
            <div class="exclusion-actions ">
                <div>
                    <asp:Button runat="server" ID="btnExcluirCV" CssClass="btn btn-success" Text="Inativar meu Currículo" OnClick="btnExcluirCV_Click" />
                </div>
                
            </div>
        </section>
        <!-- Etapa 03 -->
        <section id="etapa03" runat="server" > 
            <div class="exclusion-title">
                <h2>Pronto!</h2>
            </div>
            <div class="exclusion-success-icon">
                <svg style="width:112px;height:112px" viewBox="0 0 24 24">
                    <path fill="#8bc34a" d="M12,2A10,10 0 0,1 22,12A10,10 0 0,1 12,22A10,10 0 0,1 2,12A10,10 0 0,1 12,2M11,16.5L18,9.5L16.59,8.09L11,13.67L7.91,10.59L6.5,12L11,16.5Z" />
                </svg>                   
            </div>
            <div class="exclusion-title">            
                <h4>
                    Seu currículo foi inativado em nossa base.
                    <br />
                    Volte sempre que quiser!
                </h4>
            </div>
            <div class="exclusion-actions ">                
                <div>
                    
                    <%--<input type="button" class="btn btn-success" value="Página inicial" id="btn-cancel-03"></input>--%>
                </div>
            </div>
        </section>
    </div>
    </asp:Panel>
    </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <!-- jQuery -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script>
        $(document).ready(function () {
            //$("#cphConteudo_etapa02").hide();
            //$("#etapa03").hide();
            //$("#etapa04").hide();
            //$("#btn-cancel-01").click(function () {
            //    $('#cphConteudo_etapa02').show();
            //    $('#etapa01').hide();
            //});
            //$("#btn-cancel-02").click(function () {
            //    $('#etapa03').show();
            //    $('#etapa02').hide();
            //});
            //$("#btn-cancel-03").click(function () {
            //    $('#etapa01').show();
            //    $('#etapa03').hide();
            //});
            $("#bntContinuar").click(function () {
                document.location.href = 'Default.aspx';
            });

            AJustarMotivoOutros();
        });
        // Campos ocultos
        $(".toggleempregado").hide();
        $(".toggleoutros").hide();
        // Se marcar Já estou empregado
        $('#cphConteudo_chkEmpregado').click(function () {
            if ($("#cphConteudo_chkEmpregado").is(':checked')) {
                $(".toggleempregado").show();
            } else {
                $(".toggleempregado").hide();
            }
        });
        // Se marcar Outros, exibe textarea
        $('#cphConteudo_chkOutros').click(function () {
            AJustarMotivoOutros();
        });

        function AJustarMotivoOutros()
        {
            if ($("#cphConteudo_chkOutros").is(':checked')) {
                $(".toggleoutros").show();
            } else {
                $(".toggleoutros").hide();
            }
        };
    </script>
</asp:Content>