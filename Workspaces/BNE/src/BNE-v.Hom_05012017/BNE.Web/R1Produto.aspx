<%@ Page Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="R1Produto.aspx.cs" Inherits="BNE.Web.R1Produto" EnableEventValidation="true" %>

<asp:Content ID="experimento" runat="server" ContentPlaceHolderID="cphExperimentos">
    <%--bootstrap adaptado para  usar no BNE--%>
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/Bootstrap/bootstrap.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/R1Produto.css" type="text/css" rel="stylesheet" />
    <%--Mensura de maneira térmica o fluxo de acesso--%>
    <script type="text/javascript">
        (function () {
            var hm = document.createElement('script'); hm.type = 'text/javascript'; hm.async = true;
            hm.src = ('++u-heatmap-it+log-js').replace(/[+]/g, '/').replace(/-/g, '.');
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(hm, s);
        })();
    </script>
    <script type="text/javascript">
        autocomplete.funcao("txtFuncao");
        autocomplete.cidade("txtCidade");
    </script>
</asp:Content>
<asp:Content ID="head" runat="server" ContentPlaceHolderID="cphHead">
    <Employer:DynamicScript runat="server" Src="/js/local/R1.js" type="text/javascript" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphConteudo" runat="server">
    <div id="gal" class="textBox r1produto">
        <nav class="galnav">
            <ul class="nav navbar-nav text-center">
                <!--Start Tela Mágica-->
                <li>
                    <input type="radio" name="btn" class="" value="one" checked="checked" disabled="disabled" />
                    <label for="btn"><strong><i class="fa fa-home fa-2x fa-adjustPadding"></i></strong></label>
                    <figure class="figureAdjust1">
                        <div class="span5 figureTextAdjust1 navigationAdjust"></div>
                        <img src="img/pacote_r1/r1_produto/telaMagica.jpg" />
                        <figcaption>
                            <nav role='navigation' class="navigationAdjustHome">
                                <blockquote>
                                    <p class="adjustBlockquote"><i>Em apenas <span class="adjustEnfase">3 passos</span>, você define sua necessidade e <span class="adjustEnfase">inicia o recrutamento</span> através da nossa equipe.</i></p>
                                </blockquote>
                                <button class="btnNext pull-right btnNextAdjust btnNextHome" id="btnSolicitarAgora">Solicitar agora <i class="fa fa-hand-o-right"></i></button>
                            </nav>
                        </figcaption>
                    </figure>
                </li>
                <!--End Tela Mágica-->
                <!--Start Passo 1-->
                <li>
                    <input type="radio" name="btn" value="two" disabled="disabled" />
                    <label for="btn" class="galText"><strong>1</strong></label>
                    <figure>
                        <img src="img/pacote_r1/r1_produto/passos.jpg" />
                        <figcaption>
                            <nav role='navigation' class="navigationAdjust">
                                <blockquote>
                                    <h1><i><strong class="blueR1">Qual profissional</strong> está buscando?</i></h1>
                                </blockquote>
                                <div class="adjustPaddingTop"></div>
                                <div class="row span8 inline adjustLabelForm span8Adjust">
                                    <span class="span3 adjustLabelRight">
                                        <asp:Label ID="lblFuncaoPretendida" runat="server" Text="A função dele é?" /></span>
                                    <span class="span4 pull-right adjustInputRight">
                                        <div>
                                            <asp:RequiredFieldValidator ID="rfvFuncao" runat="server" ControlToValidate="txtFuncao"
                                                ValidationGroup="Passo1"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvFuncao" runat="server"
                                                ErrorMessage="Função Inexistente." ClientValidationFunction="ValidarFuncao"
                                                ControlToValidate="txtFuncao" ValidationGroup="Passo1" ValidateEmptyText="True">
                                            </asp:CustomValidator>
                                        </div>
                                        <asp:TextBox ID="txtFuncao" runat="server" Columns="40" CssClass="txFuncao textbox_padrao detalheLarguraCampoField"></asp:TextBox>
                                    </span>
                                </div>
                                <div class="adjustPadding"></div>
                                <div class="row span8 inline adjustLabelForm span8Adjust">
                                    <span class="span3 adjustLabelRight">
                                        <asp:Label ID="lblCidade" runat="server" Text="Em qual cidade?" /></span>
                                    <span class="span4 pull-right adjustInputRight">
                                        <div>
                                            <asp:RequiredFieldValidator ID="rfvCidade" runat="server" ControlToValidate="txtCidade"
                                                ValidationGroup="Passo1"></asp:RequiredFieldValidator>
                                            <asp:CustomValidator ID="cvCidade" runat="server"
                                                ErrorMessage="Cidade Inexistente." ClientValidationFunction="ValidarCidade"
                                                ControlToValidate="txtCidade" ValidationGroup="Passo1"></asp:CustomValidator>
                                        </div>
                                        <asp:TextBox ID="txtCidade" runat="server" MaxLength="50" Columns="50" CssClass="txCidade textbox_padrao detalheLarguraCampoField" CausesValidation="False"></asp:TextBox>
                                    </span>
                                </div>
                                <div class="adjustPadding"></div>
                                <div class="row span8 inline adjustLabelForm span8Adjust">
                                    <span class="span3 adjustLabelRight">
                                        <asp:Label ID="lblNumeroVagas" Text="Quantas vagas?" runat="server"></asp:Label></span>
                                    <span class="span4 pull-right adjustInputRight">
                                        <componente:AlfaNumerico ID="txtNumeroVagas" MaxLength="2" CssClassTextBox="textbox_padrao input-mini" ContemIntervalo="true" ValorMaximo="99" ValorMinimo="1" ValidationGroup="Passo1" MensagemErroIntervalo="O numero de vagas deve ser maior que zero" Obrigatorio="true" runat="server" Tipo="Numerico" Columns="2" />
                                    </span>
                                </div>
                                <div class="adjustPaddingButtonPasso1"></div>
                                <div class="row span8 inline adjustLabelForm span8Adjust linha">
                                    <span class="span3 adjustLabelRight">
                                        <asp:Label ID="lblVinculo" Text="Qual tipo de<br> vínculo?" runat="server"></asp:Label></span>
                                    <span class="span4 pull-right adjustInputRight">
                                        <telerik:RadComboBox ID="rcbTipoVinculo" runat="server">
                                        </telerik:RadComboBox>
                                    </span>
                                </div>
                                <div class="adjustPadding"></div>
                                <div class="row span8 inline adjustLabelForm span8Adjust linha">
                                    <span class="span3 pull-left adjustLabelRight">
                                        <asp:Label ID="lblPretensaoSalarial" runat="server" Text="Informe a<br> remuneração: R$" /></span>
                                    <span class="span4 pull-right adjustInputRight">
                                        <asp:UpdatePanel ID="upTxtPretensaoSalarial" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                            <ContentTemplate>
                                                <componente:ValorDecimal ID="txtSalarioDe" runat="server" CasasDecimais="2" ValidationGroup="Passo1" ValorMaximo="999999" ValorMinimo="1" CssClassTextBox="textbox_padrao salario" CssClass="inline adjust-inline"/>
                                                <span Class="inline adjust-inline positionAte">até</span>
                                                <componente:ValorDecimal ID="txtSalarioAte" runat="server" CasasDecimais="2" ValidationGroup="Passo1" ValorMaximo="999999" ValorMinimo="1" CssClassTextBox="textbox_padrao salario" CssClass="inline adjust-inline"/>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </span>
                                </div>
                                <div class="adjustPadding"></div>
                                <div class="row span8 inline adjustLabelForm span8Adjust">
                                    <span class="span3 pull-left adjustLabelRight"></span>
                                    <span class="span4 pull-right adjustInputRight">
                                        <button class="btnNext pull-right btnNextAdjust btnContinuarAdjust" id="btnPasso1Continuar">Continuar <i class="fa fa-hand-o-right"></i></button>
                                    </span>
                                </div>
                            </nav>
                        </figcaption>
                    </figure>
                </li>
                <!--End Passo 1-->
                <!--Start Passo 2-->
                <li>
                    <input type="radio" name="btn" value="three" disabled="disabled" />
                    <label for="btn" class="galText"><strong>2</strong></label>
                    <figure>
                        <img src="img/pacote_r1/r1_produto/passos.jpg" />
                        <figcaption>
                            <nav role='navigation' class="navigationAdjust">
                                <blockquote>
                                    <h1><i><strong class="blueR1">Sobre</strong> o <strong class="blueR1">trabalho</strong>:</i></h1>
                                </blockquote>
                                <div class="adjustPaddingTop_c"></div>
                                <div class="row span8 inline adjustLabelForm span8Adjust">
                                    <span class="span3 adjustLabelRight">
                                        <asp:Label ID="Label1" runat="server" Text="Quais as atividades ele vai realizar?" /></span>
                                    <span class="span4 pull-right adjustInputRight">
                                        <asp:TextBox ID="txtAtribuicoes" runat="server" CssClass="textarea_padrao" Rows="4" Columns="99999" TextMode="MultiLine"></asp:TextBox></span>
                                </div>
                                <div class="adjustPaddingButtonPassos"></div>
                                <div class="row span8 inline adjustLabelForm span8Adjust">
                                    <span class="span3 adjustLabelRight">
                                        <asp:Label ID="lblDisponibilidade" Text="Qual dia e horário de trabalho?" runat="server"></asp:Label></span>
                                    <span class="span4 pull-right adjustInputRight">
                                        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <Employer:ComboCheckbox ID="rcbDisponibilidade" EmptyMessage="Indiferente" runat="server" />
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </span>
                                </div>
                                <div class="adjustPadding"></div>
                                <div class="row span8 inline adjustLabelForm span8Adjust">
                                    <span class="span3 adjustLabelRight">
                                        <asp:Label ID="Label2" runat="server" Text="Qual bairro?" /></span>
                                    <span class="span4 pull-right adjustInputRight">
                                        <asp:TextBox ID="txtBairro" runat="server" MaxLength="50" Columns="50" CssClass="txCidade textbox_padrao detalheLarguraCampoField" CausesValidation="False"></asp:TextBox></span>
                                </div>
                                <div class="adjustPadding"></div>
                                <div class="row span8 inline adjustLabelForm span8Adjust">
                                    <span class="span3 pull-left adjustLabelRight">
                                        <asp:Label ID="Label3" runat="server" Text="Quais os benefícios<br> serão oferecidos?" /></span>
                                    <span class="span4 pull-right adjustInputRight">
                                        <asp:TextBox ID="txtBeneficios" runat="server" CssClass="textarea_padrao" Rows="4" Columns="99999" TextMode="MultiLine"></asp:TextBox></span>
                                </div>
                                <div class="adjustPaddingButtonPassos"></div>
                                <div class="row span8 inline">
                                    <span class="span3 adjustLabelRight">
                                        <button class="btn btn-link adjustBtnLink" id="btnPasso2Voltar"><i class="fa fa-hand-o-left"></i>Voltar</button></span>
                                    <span class="span4 pull-right adjustInputRight">
                                        <button class="btnNext pull-right btnNextAdjust" id="btnPasso2Continuar">Continuar <i class="fa fa-hand-o-right"></i></button>
                                    </span>
                                </div>
                            </nav>
                        </figcaption>
                    </figure>
                </li>
                <li>
                    <!--End Passo 2-->
                    <!--Start Passo 3-->
                    <input type="radio" name="btn" value="four" disabled="disabled" />
                    <label for="btn" class="galText"><strong>3</strong></label>
                    <figure>
                        <img src="img/pacote_r1/r1_produto/passos.jpg" />
                        <figcaption>
                            <nav role='navigation' class="navigationAdjust">
                                <blockquote>
                                    <h1><i>As <strong class="blueR1">características</strong> do <strong class="blueR1">candidato:</strong></i></h1>
                                </blockquote>
                                <div class="adjustPaddingTop_c"></div>
                                <div class="row span8 inline adjustLabelForm">
                                    <span class="span3">
                                        <asp:Label ID="lblIdade" CssClass="adjustLabel" Text="A Idade pode ser entre:" runat="server"></asp:Label></span>
                                    <span class="span4 pull-right adjustInputRight">
                                        <componente:AlfaNumerico ID="txtIdadeMinima" MaxLength="2" Obrigatorio="false" CssClassTextBox="textbox_padrao input-mini inline" runat="server" Tipo="Numerico" Columns="2" ContemIntervalo="False" MensagemErroFormato="formato" Style="width: 40px!important" />
                                        <small>
                                            <asp:Label ID="lblAte" Text="até" runat="server"></asp:Label></small>
                                        <componente:AlfaNumerico ID="txtIdadeMaxima" Obrigatorio="false" CssClassTextBox="textbox_padrao input-mini inline" CssClass="adjustInputMini adjustInputMargin" MaxLength="2" runat="server" Tipo="Numerico" Columns="2" ContemIntervalo="False" />
                                    </span>
                                </div>
                                <div class="adjustPadding"></div>
                                <div class="row span8 inline adjustLabelForm">
                                    <span class="span3 adjustLabelRight">
                                        <asp:Label ID="lblGeneroVaga" Text="Qual o gênero para esta vaga?" runat="server"></asp:Label>
                                    </span>
                                    <span class="span4 pull-right adjustInputRight">
                                        <telerik:RadComboBox ID="rcbSexo" runat="server">
                                        </telerik:RadComboBox>
                                    </span>
                                </div>
                                <div class="adjustPadding"></div>
                                <div class="row span8 inline adjustLabelForm">
                                    <span class="span3">
                                        <asp:Label ID="lblEscolaridadeConfirmacao" CssClass="adjustLabel" Text="Qual a escolaridade mínima?" runat="server"></asp:Label></span>
                                    <span class="span4 pull-right adjustInputRight">
                                        <telerik:RadComboBox ID="rcbEscolaridade" runat="server">
                                        </telerik:RadComboBox>
                                    </span>
                                </div>
                                <div class="adjustPadding"></div>
                                <div class="row span8 inline adjustLabelForm">
                                    <span class="span3">
                                        <asp:Label ID="lblHabilitacao" runat="server" CssClass="adjustLabel" Text="Precisa de CNH?" /></span>
                                    <span class="span4 pull-right adjustInputRight">
                                        <telerik:RadComboBox ID="rcbHabilitacao" runat="server">
                                        </telerik:RadComboBox>
                                    </span>
                                </div>
                                <div class="adjustPadding"></div>
                                <div class="row span8 inline adjustLabelForm">
                                    <span class="span3 adjustLabelRight">
                                        <asp:Label ID="Label4" runat="server" Text="Faltou alguma informação importante?" /></span>
                                    <span class="span4 pull-right adjustInputRight">
                                    <asp:TextBox ID="txtInformacoesAdicionais" runat="server" CssClass="textarea_padrao" Columns="99999" Rows="3" TextMode="MultiLine"></asp:TextBox></span>
                                </div>
                                <div class="adjustPaddingButtonPassos_c"></div>
                                <div class="row span8 inline">
                                    <span class="span3 adjustLabelRight">
                                        <button class="btn btn-link adjustBtnLink" id="btnPasso3Voltar"><i class="fa fa-hand-o-left"></i>Voltar</button></span>
                                    <span class="span4 pull-right adjustInputRight">
                                        <asp:UpdatePanel runat="server" ID="upSolicitar">
                                            <ContentTemplate>
                                                <asp:LinkButton runat="server" ID="btlSolicitar" CssClass="btnNext pull-right btnNextAdjust"
                                                    CausesValidation="False" OnClick="btlSolicitar_Click">
                                                        Finalizar <i class="fa fa-check-square-o"></i>
                                                </asp:LinkButton>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </span>
                                </div>
                            </nav>
                        </figcaption>
                    </figure>
                </li>
                <!--End Passo 3-->
            </ul>
        </nav>
    </div>

    <!-- START Modal -->
    <div id="myModal" class="modal modal-adjust hide fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
            <h5 id="myModalLabel">DADOS DE CONTATO</h5>
        </div>
        <div class="modal-body" style="font-size: 16px; color: #666">
            <p>Entraremos em contato com você. Informe seus dados de contato:</p>
            <div class="btn-group">
                <p>
                    <div>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNome" ValidationGroup="OK"></asp:RequiredFieldValidator>
                    </div>
                    <asp:TextBox ID="txtNome" runat="server" Columns="40" CssClass="textbox_padrao span4 adjustInput" placeholder="NOME"></asp:TextBox>
                </p>
                <p>
                    <div>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" ValidationGroup="OK"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                            ValidationGroup="OK" ErrorMessage="Email Inválido.">
                        </asp:RegularExpressionValidator>
                    </div>
                    <asp:TextBox ID="txtEmail" runat="server" Columns="40" CssClass="textbox_padrao span4 adjustInput" placeholder="E-MAIL"></asp:TextBox>
                </p>
                <p>
                    <componente:Telefone ID="txtTelefone" runat="server" ValidationGroup="OK" Tipo="FixoCelular" placeholder="TELEFONE" CssClassTextBoxDDD="textbox_padrao spanddd adjustInput" CssClassTextBoxFone="textbox_padrao spanfone adjustInput" />
                </p>
            </div>
        </div>
        <div class="modal-footer">
            <p>
                <asp:UpdatePanel runat="server" ID="upBotoes">
                    <ContentTemplate>
                        <asp:LinkButton runat="server" ID="btlOK" CssClass="btn btnBack btn-large"
                            CausesValidation="True" OnClick="btlOK_Click" ValidationGroup="OK">OK
                        </asp:LinkButton>
                        <asp:LinkButton runat="server" ID="btlJaSouCadastrado" CssClass="btn btnNext btn-large"
                            CausesValidation="False" OnClick="btlJaSouCadastrado_Click">JÁ SOU CADASTRADO
                        </asp:LinkButton>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </p>
        </div>
    </div>
    <!-- END Modal -->
    <!-- JS -->
    <script src="js/bootstrap/bootstrap.js"></script>
</asp:Content>

