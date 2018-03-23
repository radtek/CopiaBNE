// ATENÇÃO!
// Em caso de Atualização/Modificação:
// Utilizar http://jscompress.com/ com o método 'Packer' para comprimir e atualizar o arquivo /min/employer.design-pkd.js!
// --
// Este arquivo contém funcionalidades relacionadas ao design das telas e é mantido pela equipe de Design.
// Qualquer necessidade de alteração deve ser reportada para o responsável da equipe.

//Ajusta os validadores passando todos de inline para block
function AjustarValidadores()
{
    $(".validador[style*='inline']").css("display","block");
}

//Ajusta a posição do Menu Cortina
function AjustarMenuCortina()
{
    $(".botao_puxador_tela").css("left", $(".logo_sistema_simbolo").offset().left);
    $(".menu_cortina").css("left", $(".logo_sistema_simbolo").offset().left);
}

//Ajusta a posição dos botões da lateral direita com base na viewport
function AjustarBotoesLateralDireita()
{
    //Posiciona horizontalmente
    $(".painel_botoes_lateral_direita").css("left", ($(window).width() - ((($(window).width() - $(".conteudo .interna").width())/2)/2) - ($(".painel_botoes_lateral_direita a").width()/2)) );

    //Soma as dimensões e margens de cada botão do painel lateral
    var t=0;
    $(".painel_botoes_lateral_direita a").each(
        function()
        {
            t += $(this).outerHeight() + parseInt($(this).css("margin-top")) + parseInt($(this).css("margin-bottom"));
        }
    );

    //Posiciona verticalmente
    $(".painel_botoes_lateral_direita").css("top", $(window).height()/2 - t/2);
}


//Ajusta itens da RadGrid após renderizar a página
function AjustarRadGrid()
{
    //Aplica a classe específica para as textbox de filtros das radgrid
    $("*[id*='FilterTextBox']").addClass("textbox_filtro");
    $("*[id*='FilterTextBox']").parent().css("text-align","center");
    
    
    $("*[id*='FilterTextBox']").each(
        function()
        {
            var parent_width;
            var border_right_width;
            var border_left_width;
            
            parent_width = $(this).parent().width();
            border_right_width = $(this).css("border-right-width");
            border_left_width = $(this).css("border-left-width");
            
            $(this).width(parent_width - 4);
            $(this).parent().width(parent_width + parseFloat(border_left_width) + parseFloat(border_right_width));
        }
    );

    //Acerta a borda de todos os paginadores superiores de Grid Telerik
    $(".RadGrid").each(
        function()
        {
         $(this).find(".rgPagerCell:first")
            .css("border-style","none none solid none")
            .css("border-width","0 0 1px 0")
            .css("border-color","#5d8cc9")
            ;
        }
    );
}

//Aplica o padrão novo de botões
function AplicarBotoesPadraoNovo()
{
    //Passa por cada LinkButton transformando para a estrutura de botão com cantos arredondados
    $(".botao_padrao_novo,.painel_botoes_secao a").each(
        function()
        {
            var texto_original_botao;
            var texto_novo_botao;
            var icone_botao;
            var icone_botao_url;
            var icone_botao_inativo_url;
            var possui_icone;
            
            //Pega o texto original do botão
            texto_original_botao = $(this).text();
            
            //Avançar
            if($(this).hasClass("avancar"))
            {
                icone_botao_url = "/PadronizacaoInterface/img/icones/ico_botao_padrao_avancar.png";
                icone_botao_inativo_url = "/PadronizacaoInterface/img/icones/ico_botao_padrao_avancar_inativo.png";
            }
            
            //Cancelar
            if($(this).hasClass("cancelar"))
            {
                icone_botao_url = "/PadronizacaoInterface/img/icones/ico_botao_padrao_cancelar.png";
                icone_botao_inativo_url = "/PadronizacaoInterface/img/icones/ico_botao_padrao_cancelar_inativo.png";
            }
            
            //Enviar por e-mail
            if($(this).hasClass("enviar_por_email"))
            {
                icone_botao_url = "/PadronizacaoInterface/img/icones/ico_botao_padrao_enviar_email.png";
                icone_botao_inativo_url = "/PadronizacaoInterface/img/icones/ico_botao_padrao_enviar_email_inativo.png";
            }
            
            //Excel
            if($(this).hasClass("excel"))
            {
                icone_botao_url = "/PadronizacaoInterface/img/icones/ico_botao_padrao_excel.png";
                icone_botao_inativo_url = "/PadronizacaoInterface/img/icones/ico_botao_padrao_excel_inativo.png";
            }
            
            //Excluir
            if($(this).hasClass("excluir"))
            {
                icone_botao_url = "/PadronizacaoInterface/img/icones/ico_botao_padrao_excluir.png";
                icone_botao_inativo_url = "/PadronizacaoInterface/img/icones/ico_botao_padrao_excluir_inativo.png";
            }
            
            //Imprimir
            if($(this).hasClass("imprimir"))
            {
                icone_botao_url = "/PadronizacaoInterface/img/icones/ico_botao_padrao_imprimir.png";
                icone_botao_inativo_url = "/PadronizacaoInterface/img/icones/ico_botao_padrao_imprimir_inativo.png";
            }
            
            //Limpar tela
            if($(this).hasClass("limpar_tela"))
            {
                icone_botao_url = "/PadronizacaoInterface/img/icones/ico_botao_padrao_limpar_tela.png";
                icone_botao_inativo_url = "/PadronizacaoInterface/img/icones/ico_botao_padrao_limpar_tela_inativo.png";
            }
            
            //PDF
            if($(this).hasClass("pdf"))
            {
                icone_botao_url = "/PadronizacaoInterface/img/icones/ico_botao_padrao_pdf.png";
                icone_botao_inativo_url = "/PadronizacaoInterface/img/icones/ico_botao_padrao_pdf_inativo.png";
            }
            
            //Pesquisar
            if($(this).hasClass("pesquisar"))
            {
                icone_botao_url = "/PadronizacaoInterface/img/icones/ico_botao_padrao_pesquisar.png";
                icone_botao_inativo_url = "/PadronizacaoInterface/img/icones/ico_botao_padrao_pesquisar_inativo.png";
            }
            
            //Salvar
            if($(this).hasClass("salvar"))
            {
                icone_botao_url = "/PadronizacaoInterface/img/icones/ico_botao_padrao_salvar.png";
                icone_botao_inativo_url = "/PadronizacaoInterface/img/icones/ico_botao_padrao_salvar_inativo.png";
            }
            
            //Voltar
            if($(this).hasClass("voltar"))
            {
                icone_botao_url = "/PadronizacaoInterface/img/icones/ico_botao_padrao_voltar.png";
                icone_botao_inativo_url = "/PadronizacaoInterface/img/icones/ico_botao_padrao_voltar_inativo.png";
            }

            //Verifica se o botão deve ter ícone
            if(icone_botao_url != undefined)
            {                    
                //Define o ícone no estado inativo
                if($(this).hasClass("inativo"))
                {
                    icone_botao = '<img alt="" src="' + icone_botao_inativo_url + '" />';
                    $(this).attr("href","javascript:;");
                    //alert("if = " + icone_botao);
                }
                else
                {
                    icone_botao = '<img alt="" src="' + icone_botao_url + '" />';
                    //alert("else = " + icone_botao);
                }
            }
            else {
                icone_botao = "";
            }

            //Monta o HTML do botão padrão com ícone
            texto_novo_botao = '<span class="BotaoPadraoEsq">&nbsp;</span><span class="BotaoPadrao">' + icone_botao + texto_original_botao + '</span><span class="BotaoPadraoDir">&nbsp;</span>';
                                
            //Define o conteúdo do botão com os cantos arredondados
            $(this).html(texto_novo_botao);
        }
    );

    //Quando o botão ganha o foco (para o teclado)
    $(".botao_padrao_novo,.painel_botoes_secao a, .painel_botoes_lateral_direita .salvar, .painel_botoes_lateral_direita .voltar").focus(
        function()
        {
            if($(this).hasClass("inativo") != true)
            {
                $(this).addClass("mouseover");
            }
        }
    );
    
    //Quando o botão perde o foco (para o teclado)
    $(".botao_padrao_novo,.painel_botoes_secao a, .painel_botoes_lateral_direita .salvar, .painel_botoes_lateral_direita .voltar").blur(
        function()
        {
            $(this).removeClass("mousedown");
            $(this).removeClass("mouseover");
        }
    );

    //Quando o cursor passa sobre o botão
    $(".botao_padrao_novo,.painel_botoes_secao a, .painel_botoes_lateral_direita .salvar, .painel_botoes_lateral_direita .voltar").mouseover(
        function()
        {
            if($(this).hasClass("inativo") != true)
            {
                $(this).addClass("mouseover");
            }
        }
    );
    
    //Quando tira o cursor da área do botão. Deve ser usado no lugar do 'mouseout'
    //para esta situação
    $(".botao_padrao_novo,.painel_botoes_secao a, .painel_botoes_lateral_direita .salvar, .painel_botoes_lateral_direita .voltar").mouseleave(
        function()
        {
            $(this).removeClass("mousedown");
            $(this).removeClass("mouseover");
        }
    );

    //Quando se clica (e enquanto está clicando) no botão
    $(".botao_padrao_novo,.painel_botoes_secao a, .painel_botoes_lateral_direita .salvar, .painel_botoes_lateral_direita .voltar").mousedown(
        function()
        {
            if($(this).hasClass("inativo") != true)
            {
                $(this).addClass("mousedown");
            }
        }
    );
    
    //Quando solta o click
    $(".botao_padrao_novo,.painel_botoes_secao a, .painel_botoes_lateral_direita .salvar, .painel_botoes_lateral_direita .voltar").mouseup(
        function()
        {
            $(this).removeClass("mousedown");
        }
    );
}

//Faz o arredondamento das bordas de todos os "Painel Padrão"
function ArredondarPainelPadrao()
{
    //Radius dos cantos
    //Pode ser 4, 6 ou 8 pixels
    radius = 6;

    //Cria a variável com as classes necessárias para o arredondamento
    style_css  = "<style type=\"text/css\">";
    
    //CSS do top left
    style_css += ".CantosArredondadosTopLeft";
    style_css += "{";
    style_css += "clear: both";
    style_css += "width: 100%;";
    style_css += "height: 16px;";
    style_css += "background-image: url('/Global/img/CantosArredondados/" + radius + "px_top_left.png');";
    style_css += "background-repeat: no-repeat;";
    style_css += "background-position: top left;";
    style_css += "}";
    
    //CSS do top right
    style_css += ".CantosArredondadosTopRight";
    style_css += "{";
    style_css += "width: 100%;";
    style_css += "height: 100%;";
    style_css += "background-image: url('/Global/img/CantosArredondados/" + radius + "px_top_right.png');";
    style_css += "background-repeat: no-repeat;";
    style_css += "background-position: top right;";
    style_css += "}";
    
    //CSS do bottom right
    style_css += ".CantosArredondadosBottomRight";
    style_css += "{";
    style_css += "clear: both";
    style_css += "width: 100%;";
    style_css += "height: 16px;";
    style_css += "background-image: url('/Global/img/CantosArredondados/" + radius + "px_bottom_right.png');";
    style_css += "background-repeat: no-repeat;";
    style_css += "background-position: bottom right;";
    style_css += "}";
    
    //CSS do bottom left
    style_css += ".CantosArredondadosBottomLeft";
    style_css += "{";
    style_css += "width: 100%;";
    style_css += "height: 100%;";
    style_css += "background-image: url('/Global/img/CantosArredondados/" + radius + "px_bottom_left.png');";
    style_css += "background-repeat: no-repeat;";
    style_css += "background-position: bottom left;";
    style_css += "}";
    
    style_css += "</style>";
    
    //Adiciona as classes no head da página
    $("head").append(style_css);
    
    //Aplica o arredondamento para CADA painel padrão
    $(".painel_padrao").each(
        function()
        {
            //Testa se já foram aplicados os cantos arredondados para não duplicar
            if(($(this).find("div:first-child").attr("class")).indexOf("CantosArredondadosTopLeft") < 0)
            {
                //Monta a estrutura com os cantos arredondados
                pnl_top = "<div class=\"CantosArredondadosTopLeft\" style=\"clear: both\">";
                pnl_top += "    <div class=\"CantosArredondadosTopRight\"></div>";
                pnl_top += "</div>";
                
                pnl_bottom = "<div class=\"CantosArredondadosBottomRight\" style=\"clear: both\">";
                pnl_bottom += "    <div class=\"CantosArredondadosBottomLeft\"></div>";
                pnl_bottom += "</div>";
                
                //Adiciona os cantos arredondados superiores antes do conteúdo de cada painel
                $(this).prepend(pnl_top);
                
                //Adiciona os cantos arredondados inferiores após o conteúdo de cada painel
                $(this).append(pnl_bottom);
            }
        }
    );
}
//FIM: ArredondarPainelPadrao()

/*

PAREI AQUI ANTES DAS FÉRIAS (CAIO)

$("*").focus(
    function()
    {
        AjustarValidadores();
    }
);

$("*").blur(
    function()
    {
        AjustarValidadores();
    }
);
*/

$(document).ready(
    function()
    {
        //Executa funções de ajuste de design após redimensionar a janela
        $(window).resize(
            function()
            {
                //Ajusta a posição do Menu Cortina
                //AjustarMenuCortina();
                
                //Ajusta a posição dos botões da lateral direita com base na viewport
                AjustarBotoesLateralDireita();
            }
        );

        
        

        //Ajusta a posição do Menu Cortina
        //AjustarMenuCortina();

        //Ajusta a posição dos botões da lateral direita com base na viewport
        AjustarBotoesLateralDireita();

        //Ajusta itens da RadGrid após renderizar a página
        AjustarRadGrid();

        //Aplica o padrão novo de botões
        AplicarBotoesPadraoNovo();

        //Faz o arredondamento das bordas de todos os "Painel Padrão"
        //ArredondarPainelPadrao();
        
        //Carrega os eventos ao terminar de carregar a página
        CarregarEventos();
    }
);

//Eventos (Caio, fiz isso aqui para funcionar o menu cortina - Fabito)
function CarregarEventos()
{
    //Exibe ou esconde o menu cortina ao clicar no botão na tela
    $(".botao_puxador_tela").click(
        function()
        {
            $(".menu_cortina").slideToggle();
        }
    );

    //Exibe ou esconde o menu cortina ao clicar no botão no menu
    $(".botao_puxador_menu").click(
        function()
        {
            $(".menu_cortina").slideToggle();
        }
    );
}