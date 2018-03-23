var cabecalhoCurriculo;
var contadorAltura;

var checkCurrentUrl = function () {
    var currentUrl = location.href;
};

//Adiciona uma nova página à partir do template definido
function NovaPagina() {
    //Template de nova página
    var htmlNovaPagina = '<div class="div_pagina_impressao"><div class="div_pagina_impressao_conteudo_interno"></div></div>';

    //Adiciona a nova página para o começo do elemento form
    $("body").append(htmlNovaPagina);

    //Copia o cabeçalho para dentro da nova página
    $(".div_pagina_impressao_conteudo_interno:last").append(cabecalhoCurriculo);

    //Reseta o contador de altura
    contadorAltura = 0;
    contadorAltura += $(".painel_cabecalho_curriculo").outerHeight();
}

function PrepararImpressao() {
    //Define o cabeçalho do currículo
    cabecalhoCurriculo = '<div class="painel_cabecalho_curriculo">' + $(".painel_cabecalho_curriculo").html() + '</div>';

    //Remove todas as páginas de impressão anteriores
    $(".div_pagina_impressao").remove();

    //Adiciona a primeira página no documento
    NovaPagina();

    //Passa por cada elemento do conteúdo de impressão para distribuir em páginas
    $(".div_curriculo_conteudo").find("h2, h3, table:not(#tblNomeCandidato)").each(
                function () {
                    //Soma as alturas de todos os elementos do conteúdo atual da página para definir a quebra de página
                    contadorAltura += $(this).outerHeight();

                    if (contadorAltura < 860) {
                        //Verifica se o elemento atual não é o update panel com o botão
                        if ($(this).attr("id") != "#uppBotaoVerDados") {
                            //Adiciona o elemento atual no conteúdo interno da página
                            $(this).clone().appendTo(".div_pagina_impressao_conteudo_interno:last");
                        }
                    } else {
                        //Adiciona uma nova página para fazer a quebra
                        NovaPagina();

                        //Verifica se o último elemento adicionado na página anterior é um título h2 ou h3 e move o mesmo para a nova página
                        if 
                        (
                            $(".div_pagina_impressao:last").prev().find(":last").is("h2")
                            ||
                            $(".div_pagina_impressao:last").prev().find(":last").is("h3")
                        ) {
                            $(".div_pagina_impressao:last").prev().find(":last").clone().appendTo(".div_pagina_impressao_conteudo_interno:last");
                            $(".div_pagina_impressao:last").prev().find(":last").remove();
                        }

                        //Adiciona o elemento atual no conteúdo interno da página
                        $(this).clone().appendTo(".div_pagina_impressao_conteudo_interno:last");
                    }
                }
            );

    //Corrige o bug do IE que adiciona uma página em branco ao final
    $(".div_pagina_impressao:last").css("page-break-after", "avoid");
}

function pageLoad() {
    var verCurriculoCompleto = $("[id*='btnVerDados']");
    var queroContratar = $("[id*='pnlQueroContratarWebEstagios']");

    var btnCurriculoCompleto = $("#ucVisualizacaoCurriculo_btnVerDados");
    var resumo = $("#container_resumo_info");
    if (btnCurriculoCompleto.length > 0) {
        if (resumo.length > 0) {
            if (btnCurriculoCompleto.is(":visible")) {
                resumo.css(
              {
                  width: 'auto'
              });
            }
            else {
                resumo.css(
                {
                    width: '654px'
                });
            }
        }
    }
    else {
        if (resumo.length > 0) {
            resumo.css(
                {
                    width: '654px'
                });
        }
    }

    if (verCurriculoCompleto != typeof undefined) {
        if (queroContratar != typeof undefined) {
            if (verCurriculoCompleto.is(":visible")) {
                queroContratar.hide();
            } else {
                var hfEstagiario = $("[id*='hfEstagiario']");
                if (hfEstagiario != typeof (undefined) && hfEstagiario.length > 0) {
                    if (hfEstagiario.val().toString().toLowerCase() == 'true')
                        queroContratar.show();
                }

            }
        }
    }
    //Remove as tags de HTML velho usadas somente para o envio do e-mail
    $("font").contents().unwrap();
    $("hr").remove();
    $("*[width]").removeAttr("width");
    $("*[align]").removeAttr("align");
    $("*[valign]").removeAttr("valign");
    checkCurrentUrl();

    //PrepararImpressao();
}