<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Principal.Master" AutoEventWireup="true"
    CodeBehind="SiteTrabalheConoscoCriacao.aspx.cs" Inherits="BNE.Web.SiteTrabalheConoscoCriacao" %>

<asp:Content ID="conHead" ContentPlaceHolderID="cphHead" runat="server">
    
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/paleta_de_cores_stc.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicHtmlLink runat="server" Href="/css/local/SiteTrabalheConoscoPasso01.css" type="text/css" rel="stylesheet" />
    <Employer:DynamicScript runat="server" Src="/js/local/SiteTrabalheConoscoPasso01.js" type="text/javascript" />
    <script type="text/javascript">

        $(document).ready(function ()
        {
            $("li.theme-color > input").click(function ()
            {
                $("#current-color").removeClass().addClass("theme-color " + $(this).prop('id'));
                $("#current-color-name").text($(this).data('themeName'));
                $("#cphConteudo_hfTemplate").val($(this).data('themeId'));
                $("#CurrentTemaLabel").html("Novo tema selecionado:");
            });

        });

    </script>
</asp:Content>
<asp:Content ID="conConteudo" ContentPlaceHolderID="cphConteudo" runat="server">
    <h1>Exclusivo Banco de Currículos</h1>
    <div>
        <br />
        <asp:Label ID="lblDescricaoPasso1" runat="server" Text="Ao criar um Exclusivo Banco de Currículos você adquire um endereço exclusivo e com a cara da sua empresa para que os candidatos interessados em suas vagas se cadastrem. A ferramenta oferece também pesquisa avançada e rastreador de currículos, anúncio de vagas e para seus candidatos cadastro de currículo e pesquisa de vagas.">
        </asp:Label>
    </div>
    <div>
        <br />
    </div>
    <div class="interno_abas">
        <asp:Panel ID="pnlDadosPessoais" runat="server" CssClass="painel_padrao">
            <div class="painel_padrao_topo">
                &nbsp;
            </div>
           

            <!--Configurações e Paleta -->

            <div id="stc_select-theme">
                <div>Escolha nesta tela as principais características do seu site de currículos como: endereço na Internet, tema e logo. </div>
                <hr class="stc"/>
                <div id="stc_select-theme_tit">Configurações</div>
                <div>
                    <div class="stc_select-theme-col ">
                        <div id="stc_select-address" class="stc_select-col_bg">
                            <div class="stc_select-theme_titArea">Escolha o endereço:</div>
                            <div>
                                <span style="display: inline-block;">www.bne.com.br/ </span>
                                <span style="display: inline-block;">
                                    
                                    <componente:AlfaNumerico Columns="25" ID="txtEnderecoSite" runat="server" ValidationGroup="SiteTrabalheConosco"
                                        Tipo="AlfaNumerico" CssClass="texto_inline" ClientValidationFunction="ValidarEnderecoSTC"></componente:AlfaNumerico>

                                    <Componentes:BalaoSaibaMais ID="bsmIdade" runat="server" ToolTipText="Utilize a sugestão do sistema ou configure o seu endereço no BNE conforme sua escolha."
                                            Text="Saiba mais" ToolTipTitle="Escolha o Endereço:" CssClassLabel="balao_saiba_mais" ShowOnMouseover="True" />
                                   
                                </span>
                            </div>
                        </div>
                        <div id="stc_select-logo" class="stc_select-col_bg">

                            <div class="inserir_logo_empresa">

                                <asp:UpdatePanel ID="upFoto" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <Componentes:SelecionarFoto ID="ucFoto" runat="server" InitialSelection="5;5;20;20"
                                            PainelFileUploadCssClass="painel_upload_foto" PainelImagemFotoCssClass="painel_imagem_foto_logo"
                                            SemFotoImagemUrl="/img/btn_coloque_sua_logo.png" ThumbDir="~/ArquivosTemporarios/" BotaoFecharImageUrl="/img/botao_padrao_fechar.gif"
                                            MinAcceptedHeight="100" MinAcceptedWidth="100" ResizeImageHeight="203" ResizeImageWidth="1400"
                                            MaxHeight="1024" MaxWidth="1280" OnError="ucFoto_Error" />
                                        <asp:LinkButton runat="server" ID="btlExisteLogoWS" OnClick="btlExisteLogoWS_Click"
                                            Visible="False" Text="Já existe uma logo. Clique para recuperar." ToolTip="Já existe uma foto. Clique para recuperar."
                                            CssClass="link_alterar_foto" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </div>

                             
                        </div>
                        <div id="stc_selected-theme" class="stc_select-col_bg">

                            <div id="CurrentTemaLabel"  class="stc_select-theme_titArea">Tema atual:</div>
                            <div>
                                <ul class="color-themes">
                                    <li class="theme-color <%=hfTemplate.Value%>" id="current-color">
                                        <input type="radio">
                                        <label></label>
                                    </li>
                                    <li id="current-color-name" class="theme-name">
                                        
                                    </li>
                                </ul>
                            </div>


                        </div>
                    </div>
                    <div class="stc_select-theme-col ">
                
                        <div class="stc_select-col_bg ">
                            <div class="stc_select-theme_titArea">Selecione um tema para sua página:</div>

                            <ul class="color-themes">
                                <li class="theme-color PadraoBNE">
                                    <input type="radio" id="PadraoBNE"  data-theme-name="Padrão BNE" data-theme-id="PadraoBNE">
                                    <label for="PadraoBNE"></label>
                                </li>
                                <li class="theme-color Rose">
                                    <input type="radio" id="Rose" data-theme-name="Rosé" data-theme-id="Rose">
                                    <label for="Rose"></label>
                                </li>
                                <li class="theme-color Ambar">
                                    <input type="radio" id="Ambar" data-theme-name="Âmbar"  data-theme-id="Ambar">
                                    <label for="Ambar"></label>
                                </li>
                                <li class="theme-color Aquamarinho">
                                    <input type="radio" id="Aquamarinho" data-theme-name="Aquamarinho"  data-theme-id="Aquamarinho">
                                    <label for="Aquamarinho"></label>
                                </li>
                                <li class="theme-color AzulCeleste">
                                    <input type="radio" id="AzulCeleste" data-theme-name="Azul Celeste"  data-theme-id="AzulCeleste">
                                    <label for="AzulCeleste"></label>
                                </li>
                                <li class="theme-color Bege">
                                    <input type="radio" id="Bege" data-theme-name="Bege"  data-theme-id="Bege">
                                    <label for="Bege"></label>
                                </li>
                                <li class="theme-color Preto">
                                    <input type="radio" id="Preto" data-theme-name="Preto"  data-theme-id="Preto">
                                    <label for="Preto"></label>
                                </li>
                                <li class="theme-color AzulRoyal">
                                    <input type="radio" id="AzulRoyal" data-theme-name="Azul Royal"  data-theme-id="AzulRoyal">
                                    <label for="AzulRoyal"></label>
                                </li>
                                <li class="theme-color VerdeAzulado">
                                    <input type="radio" id="VerdeAzulado" data-theme-name="Verde Azulado"  data-theme-id="VerdeAzulado">
                                    <label for="VerdeAzulado"></label>
                                </li>
                                <li class="theme-color Violeta">
                                    <input type="radio" id="Violeta" data-theme-name="Violeta"  data-theme-id="Violeta">
                                    <label for="Violeta"></label>
                                </li>
                                <li class="theme-color Castanho">
                                    <input type="radio" id="Castanho" data-theme-name="Castanho" data-theme-id="Castanho">
                                    <label for="Castanho"></label>
                                </li>
                                <li class="theme-color Bizancio">
                                    <input type="radio" id="Bizancio" data-theme-name="Bizâncio" data-theme-id="Bizancio">
                                    <label for="Bizancio"></label>
                                </li>
                                <li class="theme-color Carmin">
                                    <input type="radio" id="Carmin" data-theme-name="Carmin"  data-theme-id="Carmin">
                                    <label for="Carmin"></label>
                                </li>
                                <li class="theme-color Celeste">
                                    <input type="radio" id="Celeste" data-theme-name="Celeste"  data-theme-id="Celeste">
                                    <label for="Celeste"></label>
                                </li>
                                <li class="theme-color Champagne">
                                    <input type="radio" id="Champagne" data-theme-name="Champagne"  data-theme-id="Champagne">
                                    <label for="Champagne"></label>
                                </li>
                                <li class="theme-color VerdeMonster">
                                    <input type="radio" id="VerdeMonster" data-theme-name="Verde Monster"  data-theme-id="VerdeMonster">
                                    <label for="VerdeMonster"></label>
                                </li>
                                <li class="theme-color Coral">
                                    <input type="radio" id="Coral" data-theme-name="Coral" data-theme-id="Coral">
                                    <label for="Coral"></label>
                                </li>
                                <li class="theme-color Carmesin">
                                    <input type="radio" id="Carmesin" data-theme-name="Carmesin" data-theme-id="Carmesin">
                                    <label for="Carmesin"></label>
                                </li>
                                <li class="theme-color Ouro">
                                    <input type="radio" id="Ouro" data-theme-name="Ouro" data-theme-id="Ouro">
                                    <label for="Ouro"></label>
                                </li>
                                <li class="theme-color ErvaDoce">
                                    <input type="radio" id="ErvaDoce" data-theme-name="Erva Doce" data-theme-id="ErvaDoce">
                                    <label for="ErvaDoce"></label>
                                </li>
                                <li class="theme-color Arlequin">
                                    <input type="radio" id="Arlequin" data-theme-name="Arlequin" data-theme-id="Arlequin">
                                    <label for="Arlequin"></label>
                                </li>
                                <li class="theme-color Indigo">
                                    <input type="radio" id="Indigo" data-theme-name="Índigo" data-theme-id="Indigo">
                                    <label for="Indigo"></label>
                                </li>
                                <li class="theme-color Marfim">
                                    <input type="radio" id="Marfim" data-theme-name="Marfim" data-theme-id="Marfim">
                                    <label for="Marfim"></label>
                                </li>
                                <li class="theme-color Jade">
                                    <input type="radio" id="Jade" data-theme-name="Jade" data-theme-id="Jade">
                                    <label for="Jade"></label>
                                </li>
                                <li class="theme-color Lavanda">
                                    <input type="radio" id="Lavanda" data-theme-name="Lavanda" data-theme-id="Lavanda">
                                    <label for="Lavanda"></label>
                                </li>
                                <li class="theme-color Lilas">
                                    <input type="radio" id="Lilas" data-theme-name="Lilás" data-theme-id="Lilas">
                                    <label for="Lilas"></label>
                                </li>
                                <li class="theme-color Magenta">
                                    <input type="radio" id="Magenta" data-theme-name="Magenta" data-theme-id="Magenta">
                                    <label for="Magenta"></label>
                                </li>
                                <li class="theme-color Marrom">
                                    <input type="radio" id="Marrom"  data-theme-name="Marrom" data-theme-id="Marrom">
                                    <label for="Marrom"></label>
                                </li>
                                <li class="theme-color Cafe">
                                    <input type="radio" id="Cafe"  data-theme-name="Café" data-theme-id="Cafe">
                                    <label for="Cafe"></label>
                                </li>
                                <li class="theme-color AzulMarinho">
                                    <input type="radio" id="AzulMarinho"  data-theme-name="Azul Marinho" data-theme-id="AzulMarinho">
                                    <label for="AzulMarinho"></label>
                                </li>
                                <li class="theme-color VerdeOliva">
                                    <input type="radio" id="VerdeOliva"  data-theme-name="Verde Oliva" data-theme-id="VerdeOliva">
                                    <label for="VerdeOliva"></label>
                                </li>
                                <li class="theme-color Caqui">
                                    <input type="radio" id="Caqui"  data-theme-name="Caqui" data-theme-id="Caqui">
                                    <label for="Caqui"></label>
                                </li>
                                <li class="theme-color LaranjaAverm">
                                    <input type="radio" id="LaranjaAverm"  data-theme-name="Laranja Avermelhado" data-theme-id="LaranjaAverm">
                                    <label for="LaranjaAverm"></label>
                                </li>
                                <li class="theme-color Pessego">
                                    <input type="radio" id="Pessego" data-theme-name="Pêssego" data-theme-id="Pessego">
                                    <label for="Pessego"></label>
                                </li>
                                <li class="theme-color AzulPersa">
                                    <input type="radio" id="AzulPersa" data-theme-name="Azul Persa" data-theme-id="AzulPersa">
                                    <label for="AzulPersa"></label>
                                </li>
                                <li class="theme-color Rubro">
                                    <input type="radio" id="Rubro" data-theme-name="Rubro" data-theme-id="Rubro">
                                    <label for="Rubro"></label>
                                </li>
                                <li class="theme-color Saara">
                                    <input type="radio" id="Saara" data-theme-name="Saara" data-theme-id="Saara">
                                    <label for="Saara"></label>
                                </li>
                                <li class="theme-color Salmao">
                                    <input type="radio" id="Salmao" data-theme-name="Salmão" data-theme-id="Salmao">
                                    <label for="Salmao"></label>
                                </li>
                                <li class="theme-color Sambar">
                                    <input type="radio" id="Sambar" data-theme-name="Sambar" data-theme-id="Sambar">
                                    <label for="Sambar"></label>
                                </li>
                                <li class="theme-color Bharal">
                                    <input type="radio" id="Bharal" data-theme-name="Bharal" data-theme-id="Bharal">
                                    <label for="Bharal"></label>
                                </li>
                                <li class="theme-color Cargo">
                                    <input type="radio" id="Cargo" data-theme-name="Cargo" data-theme-id="Cargo">
                                    <label for="Cargo"></label>
                                </li>
                                <li class="theme-color CinzaClaro">
                                    <input type="radio" id="CinzaClaro" data-theme-name="Cinza Claro" data-theme-id="CinzaClaro">
                                    <label for="CinzaClaro"></label>
                                </li>
                                <li class="theme-color CinzaMedio">
                                    <input type="radio" id="CinzaMedio" data-theme-name="Cinza Médio" data-theme-id="CinzaMedio">
                                    <label for="CinzaMedio"></label>
                                </li>
                                <li class="theme-color CinzaEscuro">
                                    <input type="radio" id="CinzaEscuro" data-theme-name="Cinza Escuro" data-theme-id="CinzaEscuro">
                                    <label for="CinzaEscuro"></label>
                                </li>
                                <li class="theme-color CinzaAzulado">
                                    <input type="radio" id="CinzaAzulado" data-theme-name="Cinza Azulado" data-theme-id="CinzaAzulado">
                                    <label for="CinzaAzulado"></label>
                                </li>
                                <li class="theme-color Esmeralda">
                                    <input type="radio" id="Esmeralda" data-theme-name="Esmeralda" data-theme-id="Esmeralda">
                                    <label for="Esmeralda"></label>
                                </li>
                                <li class="theme-color Agata">
                                    <input type="radio" id="Agata" data-theme-name="Ágata" data-theme-id="Agata">
                                    <label for="Agata"></label>
                                </li>
                                <li class="theme-color Opala">
                                    <input type="radio" id="Opala" data-theme-name="Opala" data-theme-id="Opala">
                                    <label for="Opala"></label>
                                </li>
                                <li class="theme-color Turquesa">
                                    <input type="radio" id="Turquesa" data-theme-name="Turquesa" data-theme-id="Turquesa">
                                    <label for="Turquesa"></label>
                                </li>
                                <li class="theme-color Amarelo">
                                    <input type="radio" id="Amarelo" data-theme-name="Amarelo" data-theme-id="Amarelo">
                                    <label for="Amarelo"></label>
                                </li>
                                <li class="theme-color Cinza">
                                    <input type="radio" id="Cinza" data-theme-name="Cinza" data-theme-id="Cinza">
                                    <label for="Cinza"></label>
                                </li>
                                <li class="theme-color Laranja">
                                    <input type="radio" id="Laranja" data-theme-name="Laranja" data-theme-id="Laranja">
                                    <label for="Laranja"></label>
                                </li>
                                <li class="theme-color Verde">
                                    <input type="radio" id="Verde" data-theme-name="Verde" data-theme-id="Verde">
                                    <label for="Verde"></label>
                                </li>
                                <li class="theme-color Vermelho">
                                    <input type="radio" id="Vermelho" data-theme-name="Vermelho" data-theme-id="Vermelho">
                                    <label for="Vermelho"></label>
                                </li>
                            </ul>
                            <div class="clear"></div>
                        </div>
                    </div>
                </div>
                <div class="clear"></div>
                <hr class="stc" />
            </div>

            <!-- Fim da paleta -->

        </asp:Panel>
        <asp:Panel ID="pnlBotoes" runat="server" CssClass="painel_botoes">
            <asp:UpdatePanel ID="upBotoes" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:Button ID="btnSalvarAvancar" runat="server" CssClass="botao_padrao" Text="Salvar e Avançar"
                        ValidationGroup="SiteTrabalheConosco" OnClick="btnSalvarAvancar_Click" />
                    <asp:Button ID="btnVoltar" runat="server" CssClass="botao_padrao" Text="Voltar" CausesValidation="false"
                        OnClick="btnVoltar_Click" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <asp:HiddenField ID="hfTemplate" runat="server" Value="0" />
</asp:Content>
<asp:Content ID="conRodape" ContentPlaceHolderID="cphRodape" runat="server">
</asp:Content>
