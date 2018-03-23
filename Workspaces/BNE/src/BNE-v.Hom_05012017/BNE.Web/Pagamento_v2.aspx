<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="Pagamento_v2.aspx.cs" Inherits="BNE.Web.Pagamento_v2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHead" runat="server">
    <link href="icons/styles.css" rel="stylesheet">
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/Pagamento.css" type="text/css" rel="stylesheet" />
    <link href="http://maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css" rel="stylesheet">
    <link href="http://fonts.googleapis.com/css?family=Roboto:400,400italic,300italic,500,500italic,700,700italic,900,300,100italic" rel="stylesheet" type="text/css">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div class="bredcrumb" runat="server" id="divBreadCrumb">
        <span class="tit_bdc">Passos para ser VIP:</span>
        <span class="txt_bdc">1 - Escolha o Plano </span>
        <span class="txt_bdc">/</span>
        <span class="txt_bdc"><strong><u><a href="javascript:history.go(-1);">2 - Formas de Pagamento</a></u></strong></span>
        <span class="txt_bdc">/</span>
        <span class="txt_bdc">3 - Confirmação</span>
        <span class="txt_bdc">-</span>
        <span class="txt_bdc">Parabéns, você é <strong>VIP!</strong> <i class="fa fa-trophy"></i></span>
    </div>
    <asp:UpdatePanel ID="updTextAcesso" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <div class="select_plano">
                <asp:Label class="txt_pla" ID="lblPlano" runat="server" Text="Você escolheu o Acesso:"></asp:Label>
                <asp:Label class="txt_pla" ID="lblPremium" runat="server" Visible="false" Text="Você escolheu uma candidatura avulsa por:"></asp:Label>
                <span class="plan_escolhido">
                    <asp:Literal runat="server" ID="ltNomePlano"></asp:Literal>
                    <asp:Literal runat="server" ID="ltValorPlano"></asp:Literal></span>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="cert_seg">
        <span><i class="fa fa-lock"></i>Você está em um ambiente seguro</span>
    </div>




    <div class="clear"></div>
    <br />
    <div id="liberacaoImediata">
    <p class="lib_imedi">Liberação <strong>imediata</strong></p>
     </div>
    <p class="lib_2dias">Liberação em até<strong> 2 dias úteis</strong></p>


    <div class="formas_pag" style="display:none">
        <div class="TabControl">
            <div id="DivInfoFormasPgto">
                <label id="lblFormasPgtoDisponiveis"></label>
            </div>
            <div id="headerTabs">
                <ul class="abas2">
                    <li>
                        <div id="DivSelCartaoDeCredito" class="aba imedi">
                            <span class="icon icon-credit-card"></span>
                            <br>
                            <span>Cartão de Crédito</span>
                        </div>
                    </li>
                    <li>
                        <div id="DivSelDebitoAutomatico" class="aba mg_l imedi">
                            <span class="icon icon-computer-accept"></span>
                            <br>
                            <span>Débito Automático</span>
                        </div>
                    </li>
                    <li>
                        <div id="DivSelBoleto" class="aba mg_l2 2dias">
                            <span class="icon icon-barcode"></span>
                            <br>
                            <span>Boleto</span>
                        </div>
                    </li>
                    <li>
                        <div id="DivSelPayPal" class="aba mg_l3 2dias">
                            <span class="icon icon-paypal"></span>
                            <br>
                            <span>Paypal</span>
                        </div>
                    </li>
                    <li>
                        <div id="DivSelPagSeguro" class="aba mg_l3 2dias">
                            <span class="icon icon-pagseguro-2"></span>
                            <br>
                            <span>Pagseguro</span>
                        </div>
                    </li>
                    <li>
                        <div id="DivSelDebitoEmConta" class="aba mg_l3 2dias">
                            <span class="icon icon-barcode"></span>
                            <br>
                            <span>Debito em Conta</span>
                        </div>
                    </li>
                </ul>
            </div>
            <div id="contentTabs">
               <%-- Cartao de Credito--%>
                <div class="conteudo">
                    <asp:UpdatePanel ID="upCartaoCredito" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            <div class="cartao_credito" id="conteudoCartaoCredito">
                                <span class="text_c">Selecione a Bandeira</span>
                                <div class="selec_bandeiras">
                                    <input type="radio" name="cartaoCredito" id="visa" value="Visa" checked>
                                    <label for="visa" class="bandeira">
                                        <img class="img_t2" src="/img/visa.png"></label>
                                    <br>
                                    <br>
                                    <input type="radio" name="cartaoCredito" id="master" value="Master">
                                    <label for="master" class="bandeira">
                                        <img class="img_t" src="/img/master.png"></label>
                                </div>

                                <div class="dados_cartao">
                                    <div style="margin-bottom: 20px;">
                                        <span class="text_d">Número do Cartão:</span>
                                        <componente:AlfaNumerico ID="txtNumeroCartao" Tipo="AlfaNumerico" MensagemErroObrigatorio="Por favor, informe o número do cartão" MensagemErroFormato="Cartão de Crédito Inválido" MensagemErroValor="Cartão de Crédito Inválido"
                                            runat="server" ValidationGroup="CartaoDeCredito" placeholder="Informe o número do cartão" ClientValidationFunction="valida_cartao" class="numero_car" />
                                    </div>
                                    <span class="text_e">Validade:</span>
                                    <label class="custom-select">
                                        <asp:CustomValidator ID="CustomValidatorVencimento" runat="server" CssClass="validador" ValidationGroup="CartaoDeCredito" Text="Por favor, informe o vencimento" ClientValidationFunction="valida_vencimento"></asp:CustomValidator>
                                        <asp:CustomValidator ID="CustomValidatorMesVencimento" runat="server" ControlToValidate="ddlMesVencimento" CssClass="validador" ValidationGroup="CartaoDeCredito" Text="Vencimento anterior ao mês atual" ClientValidationFunction="valida_vencimento_anterior"></asp:CustomValidator>
                                        <asp:DropDownList runat="server" ID="ddlMesVencimento" CausesValidation="true" AutoPostBack="false">
                                            <asp:ListItem Text="Mês" Value="00" disabled="true" Selected="True" class="invisible_option"></asp:ListItem>
                                            <asp:ListItem Text="Janeiro" Value="01"></asp:ListItem>
                                            <asp:ListItem Text="Fevereiro" Value="02"></asp:ListItem>
                                            <asp:ListItem Text="Março" Value="03"></asp:ListItem>
                                            <asp:ListItem Text="Abril" Value="04"></asp:ListItem>
                                            <asp:ListItem Text="Maio" Value="05"></asp:ListItem>
                                            <asp:ListItem Text="Junho" Value="06"></asp:ListItem>
                                            <asp:ListItem Text="Julho" Value="07"></asp:ListItem>
                                            <asp:ListItem Text="Agosto" Value="08"></asp:ListItem>
                                            <asp:ListItem Text="Setembro" Value="09"></asp:ListItem>
                                            <asp:ListItem Text="Outubro" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="Novembro" Value="11"></asp:ListItem>
                                            <asp:ListItem Text="Dezembro" Value="12"></asp:ListItem>
                                        </asp:DropDownList>
                                    </label>
                                    <label class="custom-select2">
                                        <asp:CustomValidator ID="CustomValidatorAnoVencimento" runat="server" ControlToValidate="ddlAnoVencimento" CssClass="validador" ValidationGroup="CartaoDeCredito" Text="Vencimento anterior ao mês atual" ClientValidationFunction="valida_vencimento_anterior"></asp:CustomValidator>
                                        <asp:DropDownList runat="server" ID="ddlAnoVencimento" CausesValidation="true">
                                            <asp:ListItem Text="Ano" Value="00" disabled="true" Selected="True" class="invisible_option"></asp:ListItem>
                                        </asp:DropDownList>
                                    </label>
                                    <br>
                                    <br>
                                    <span class="text_f">Código verificador:</span>
                                    <componente:AlfaNumerico ID="txtCodigoVerificadorCartao" Tipo="AlfaNumerico" MensagemErroFormato="Código verificador incorreto" MensagemErroObrigatorio="Por favor, informe o código verificador"
                                        runat="server" ValidationGroup="CartaoDeCredito" CssClass="seg_car" />

                                    <i class="fa fa-question-circle"></i>
                                    <div class="md_atent">
                                        <img class="img_t3" src="img/card_cod.png">
                                    </div>
                                    <br>
                                    <br>
                                    <asp:Button runat="server" ID="btnFinalizarCartaoCredito" class="finilazar_cp" CausesValidation="true" ValidationGroup="CartaoDeCredito" Text="Continuar" OnClick="btnFinalizarCartaoCredito_Click" />
                                    <asp:Panel ID="divInfoCancelamentoCartaoCredito" runat="server" ClientIDMode="Static">
                                        <Componentes:BalaoSaibaMais ID="BalaoSaibaMais_CancelamentoCartaoCredito" runat="server" ToolTipText="Sem burocracia, você pode encerrar seu plano no final do período contratado, optando pelo encerramento da recorrência. Na opção ''meu plano'', selecione ''cancelar plano''."
                                            Text="Saiba mais" CssClass="custom_pag_balao" ToolTipTitle="Info Cancelamento" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True" Style="margin-left: 355px; margin-top: -10px;" />
                                        <div style="font-size: 14px; font-weight: bold; margin-top: 20px; color: #333; margin-left: -30px;">
                                            <p>Cancelamento de plano simples, a qualquer momento =)</p>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <!-- DÉBITO AUTOMATICO -->
                <div class="conteudo" id="conteudoDebitoAutomatico">
                    <div class="cartao_credito">
                        <div class="selec_OpcaoPagto_title">Selecione o Banco:</div>
                        <div class="selec_Opcao--desc">(Você será direcionado para o site do banco)</div>
                        <ul class="selec_Banco">
                            <li class="selec_Banco_tp">
                                <asp:ImageButton runat="server" ID="bb" CssClass="btn_pg_banco" CausesValidtion="true" ImageUrl="~/img/pagamento/selec_Opcao_BancoBrasil.png" OnClick="ButtonBB_Click" Style="margin-right: 10px;" />
                            </li>
                            <li class="selec_Banco_tp">
                                <asp:ImageButton runat="server" ID="btPagamentoBradesco" CssClass="btn_pg_banco" CausesValidtion="true" ImageUrl="~/img/pagamento/selec_Opcao_BancoBradesco.png" OnClick="btPagamentoBradesco_Click" />
                            </li>
                        </ul>
                    </div>
                </div>
                <!-- BOLETO -->
                <div class="conteudo" id="conteudoBoleto">
                    <div class="conteudo_other">
                        <span class="text_j"><strong>Você será redirecionado pelo BNE para a tela de impressão do boleto.</strong></span><br>
                        <span class="text_m">Siga todas as instruções para concluir a compra do seu plano.</span>
                        <br>
                        <div class="esp_dados_b">
                            <i class="fa fa-print"></i>
                            <br>
                            <p class="text_n">Imprima o boleto e pague no banco...</p>
                        </div>
                        <i class="fa fa-long-arrow-right"></i>
                        <div class="esp_dados_b">
                            <i class="fa fa-desktop"></i>
                            <p class="text_n">...pague pela internet utilizando o código de barras do boleto...</p>
                        </div>
                        <i class="fa fa-long-arrow-right"></i>
                        <div class="esp_dados_b">
                            <i class="fa fa-calendar"></i>
                            <p class="text_n">...o prazo de <strong>validade do boleto é de 1 dia útil</strong>.</p>
                        </div>
                        <br>
                        <br>
                        <asp:Button class="finilazar_cp2" runat="server" ID="btnPagamentoBoleto" CausesValidation="true" ValidationGroup="Boleto" Text="Gerar Boleto" OnClick="btnPagamentoBoleto_Click" />
                    </div>
                </div>
                <!-- PAYPAL -->
                <div class="conteudo" id="conteudoPayPal">
                    <div class="conteudo_other">
                        <span class="text_q"><strong>Você será redirecionado pelo BNE para o site do Paypal para finalizar o pagamento.</strong></span><br>
                        <span class="text_m">Siga todas as instruções para concluir a compra do seu plano.</span>
                        <br>
                        <br>
                        <span class="text_o">com</span>
                        <img class="img_t4" src="img/paypal.png">
                        <br>
                        <br>
                        <br>
                        <asp:Button class="finilazar_cp3" runat="server" ID="btnPayPal" CausesValidation="true" ValidationGroup="PayPal" Text="Finalizar Compra" OnClick="btnPayPal_Click" />
                    </div>
                </div>
                <!-- PAGSEGURO -->
                <div class="conteudo" id="conteudoPagSeguro">
                    <div class="conteudo_other">
                        <span class="text_q"><strong>Você será redirecionado pelo BNE para o site do PagSeguro para finalizar o pagamento.</strong></span><br>
                        <span class="text_m">Siga todas as instruções para concluir a compra do seu plano.</span>
                        <br>
                        <br>
                        <span class="text_p">com</span>
                        <img class="img_t4" src="img/pagseguro.png">
                        <br>
                        <br>
                        <br>
                        <asp:Button class="finilazar_cp3" runat="server" ID="btnPagSeguro" CausesValidation="true" ValidationGroup="PagSeguro" Text="Finalizar Compra" OnClick="btnPagSeguro_Click" />
                    </div>
                </div>
                <!-- Débito em Conta -->
                <div class="conteudo" id="conteudoDebitoEmConta">
                    <div class="conteudo_other_1">
                        <div class="icon_hsbc">
                            <img class="img_t5 " src="img/hsbc.png" />
                            <br />
                            <span>(Pagamento será liberado em até 2 dias após a confirmação do banco.)</span>
                        </div>
                        <div class="dados_cartao">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                <ContentTemplate>
                                    <div id="pnHSBC">
                                        <div class="radChoice">
                                            <asp:RadioButton ID="radbtnCPF" runat="server" Text="CPF" GroupName="TipoPessoa" AutoPostBack="true" Width="80px" OnCheckedChanged="radbtnCPF_OR_CNPJ_CheckedChanged" Checked="true" />
                                            <asp:RadioButton ID="radbtnCNPJ" runat="server" Text="CNPJ" GroupName="TipoPessoa" AutoPostBack="true" Width="80px" OnCheckedChanged="radbtnCPF_OR_CNPJ_CheckedChanged" />
                                        </div>
                                        <div style="margin-top: 12px">
                                            <asp:Label ID="lblTextCPFouCNPJ" runat="server" Text="CPF do titular:" CssClass="text_g"></asp:Label>
                                            <componente:CPF runat="server" ID="txtCPFDebitoHSBC" Obrigatorio="true" Width="80" ValidationGroup="DebitoAutomaticoHSBC" Class="cpf_banco" MensagemErroObrigatorio="Por favor, informe o CPF" />
                                            <componente:CNPJ runat="server" ID="txtCNPJDebitoHSBC" Obrigatorio="true" Width="80" ValidationGroup="DebitoAutomaticoHSBC" Class="cpf_banco" MensagemErroObrigatorio="Por favor, informe o CNPJ" Visible="false" />
                                        </div>

                                        <div style="margin-top: 12px">
                                            <span class="text_h">Agência:</span>
                                            <asp:CustomValidator ID="CustomValidatorObrigaContaHSBC" runat="server" CssClass="validador" ValidationGroup="DebitoAutomaticoHSBC" Text="Por favor, indique o número da agência e conta com dígito verificador" ClientValidationFunction="valida_agencia_hsbc_obrigatoria"></asp:CustomValidator>
                                            <asp:TextBox runat="server" ID="txtAgenciaDebitoHSBC" placeholder="Agência" class="ag agen_c" ValidationGroup="DebitoAutomaticoHSBC"></asp:TextBox>
                                        </div>

                                        <div style="margin-top: 12px">
                                            <span class="text_i">Conta Corrente:</span>
                                            <asp:CustomValidator ID="CustomValidatorContaHSBC" runat="server" CssClass="validador" ValidationGroup="DebitoAutomaticoHSBC" Text="Conta corrente inválida. Por favor verifique os dados informados." ClientValidationFunction="valida_conta_corrente_hsbc"></asp:CustomValidator>
                                            <asp:TextBox runat="server" ID="txtContaCorrenteDebitoHSBC" placeholder="Conta Corrente" class="conta_corrente agen_c" ValidationGroup="DebitoAutomaticoHSBC"></asp:TextBox>
                                            <span class="text_t">Dígito: </span>
                                            <asp:TextBox runat="server" ID="txtDigitoDebitoHSBC" class="digito_conta" placeholder="Dígito" ValidationGroup="DebitoAutomaticoHSBC" CausesValidation="true"></asp:TextBox>
                                        </div>
                                        <div style="margin-top: 12px">
                                            <asp:Button class="finilazar_cp" runat="server" ID="btnFinalizarDebitoHSBC" CausesValidation="true" ValidationGroup="DebitoAutomaticoHSBC" Text="Finalizar Compra" OnClick="btnFinalizarDebito_Click" />
                                            <asp:Panel ID="divInfoCancelamentoHSBC" runat="server" ClientIDMode="Static">
                                                <Componentes:BalaoSaibaMais ID="BalaoSaibaMais_CancelamentoHsbc" runat="server" ToolTipText="Sem burocracia, você pode encerrar seu plano no final do período contratado, optando pelo encerramento da recorrência. Na opção ''meu plano'', selecione ''cancelar plano''."
                                                    Text="Saiba mais" CssClass="balao_pag_customizado" ToolTipTitle="Info Cancelamento" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True" Style="margin-left: 355px; margin-top: -10px;" />
                                                <div style="font-size: 14px; font-weight: bold; margin-top: 20px; color: #333; margin-left: -30px;">
                                                    <p>Cancelamento de plano simples, a qualquer momento =)</p>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                    <%--<div id="pnBB">
                                        <asp:ImageButton runat="server" ID="btPagamentoBB" CssClass="btn_pg_banco" CausesValidtion="true" ImageUrl="~/img/btn_pg_bb.png" OnClick="ButtonBB_Click" />
                                    </div>--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="clear"></div>

    <asp:UpdatePanel runat="server" ID="upCodigoCredito">
        <ContentTemplate>
            <asp:Panel ID="pnlCodigoCredito" runat="server" Visible="false" CssClass="codigo_credito">
                <div class="cadeado">
                    <img src="img/voce_esta_em_um_site_seguro.png" />
                </div>
                <div class="containerIsenrirCupom">
                    <div class="groupAll">
                        <asp:Label ID="lblCodigoCredito" AssociatedControlID="txtCodigoCredito" Text="Código promocional:"
                            runat="server">
                            <asp:TextBox ID="txtCodigoCredito" runat="server" Columns="40" MaxLength="200" CssClass="textbox_padrao complementaTextbox"
                                AutoPostBack="true" OnTextChanged="txtCodigoCredito_TextChanged"></asp:TextBox>
                            &nbsp;<asp:Button ID="btnValidarCodigoCredito" runat="server" CssClass="buttonStyle"
                                Text="Aplicar" OnClick="btnValidarCodigoCredito_Click" />
                        </asp:Label>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphRodape" runat="server">
    <Employer:DynamicScript runat="server" Src="/js/local/Pagamento.js" type="text/javascript" />
    <Employer:DynamicScript runat="server" Src="/js/jquery.maskedinput.min.js" type="text/javascript" />


    <script type="text/javascript">
        

        setTimeout(function () { CarregarTela(); }, 1);

        function CarregarTela() {
            $(".abas2 li:first div").addClass("selected");//Defini a primeira aba como selecionada
            $("#contentTabs .conteudo:nth-child(1)").show();//Mostra a div relacionada com a aba selecionada
            $(".formas_pag").show();
            $(".aba").click(function () {

                if ('<%= HabilitarApenasCartaoCredito %>' == '1') {
                    $('#lblFormasPgtoDisponiveis').text('* O pagamento deste plano pode ser realizado apenas via cartão de crédito.');
                    return; //não efetua processo de trocas de aba caso somente cartão possa ser exibido
                }

                $(".aba").removeClass("selected");// Remove estilo 'selected' de todas as abas
                $(this).addClass("selected");// Adcionar estilo 'selected' na aba clicada

                var indice = $(this).parent().index();// Retorna o indice da aba clicada
                indice++;//Como o indice comeca a contar do 0 devemos incrementar mais 1 para conseguimos pegar mostrar o conteudo da aba clicada
                $("#contentTabs .conteudo").hide();// Oculta todos os conteudo das abas anteriores
                $("#contentTabs .conteudo:nth-child(" + indice + ")").show();// Através do indice eu mostro o conteudo da aba clicada

                if (indice == 3) {
                    $("#conteudoBoleto").show();
                }
                $(".formas_pag").show();
            });

            
            
        }
        
    </script>
    
    <script type="text/javascript">
        $(document).ready(function () {
            $('p.lib_imedi').addClass('barra_azul');

            $('.painel_filtro').hide();
            $('.painel_icones').hide();
            $('.barra_rodape').hide();

            $(".imedi").hover(function () {
                $('p.lib_imedi').addClass('barra_azul');
            }, function () {
                $('p.lib_imedi').removeClass('barra_azul');
            });

            if ('<%= HabilitarApenasCartaoCredito %>' == '1') {
                $('.aba').addClass('cursorNotAllowed');
                $('#DivSelCartaoDeCredito').removeClass('cursorNotAllowed');
            }

            if ('<%= HabilitarFormasPagamentoPlanosRecorrentes %>' == '1') {
                if ('<%=FormaDePagamentoBoleto%>' == 'True') {
                    $('#DivSelCartaoDeCredito').remove();
                    $("#conteudoCartaoCredito").remove();

                    $("#liberacaoImediata").toggle();
                    $("#DivSelBoleto").click();
                    $('p.lib_imedi').remove();
                    $("#conteudoBoleto").show();
                    $(".formas_pag").show();
                }
                else {
                    $('#DivSelBoleto').remove();
                    $("#conteudoBoleto").remove();
                }
                $('#DivSelPayPal').remove();
                $("#conteudoPayPal").remove();

                $('#DivSelPagSeguro').remove();
                $("#conteudoPagSeguro").remove();

                $('#DivSelDebitoAutomatico').remove();
                $("#conteudoDebitoAutomatico").remove();

                $('#DivSelDebitoEmConta').removeClass('mg_l3').addClass('recorrente');
                $("#conteudoDebitoEmConta").remove();

                $('#cphConteudo_bb').remove();
                $('#spanInfo_bb').remove();
                $('#cphConteudo_btPagamentoBradesco').remove();
                $('#spanInfo_btPagamentoBradesco').remove();

               
            }
            else {
                $('#divInfoCancelamentoCartaoCredito').remove();
                $('#divInfoCancelamentoHSBC').remove();
            }

            if ('<%= DesabilitarTodosOsPagamentos %>' == '1') {
                $('#DivSelBoleto').remove();
                $('#DivSelPayPal').remove();
                $('#DivSelPagSeguro').remove();
                $('#DivSelDebitoAutomatico').remove();
                $('#DivSelDebitoEmConta').remove();

                $('#cphConteudo_bb').remove();
                $('#spanInfo_bb').remove();
                $('#cphConteudo_btPagamentoBradesco').remove();
                $('#spanInfo_btPagamentoBradesco').remove();
                $('#DivSelCartaoDeCredito').remove();
                $('.TabControl').remove();
            }
            $('#DivSelDebitoEmConta').remove();
        });

    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".2dias").hover(function () {

                $('p.lib_2dias').addClass('barra_azul');
            }, function () {

                $('p.lib_2dias').removeClass('barra_azul');
            });
        });

    </script>
</asp:Content>
