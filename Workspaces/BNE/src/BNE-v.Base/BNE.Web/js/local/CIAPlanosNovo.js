$(document).ready(LocalPageLoad());

var planoBasico;
var planoIndicado;
var planoPlus;

function LocalPageLoad() {

    $(".containerPlano").unbind("click");
    $(".containerPlano input[checked=checked]").closest(".containerPlano ").addClass("escolhido");
    $(".containerPlano").click(function (e) {
        $(".containerPlanos").find("[type=radio]").removeAttr("checked");
        $(".containerPlano").removeClass('escolhido');
        $(this).addClass("escolhido");
        $(this).find("[type=radio]").attr('checked', 'checked');
        if ($('#cphConteudo_cbPlanoBasico').attr('checked') == 'checked') {
            $('.divAlinhaCheckBoxContrato').hide();
        } else {
            $('.divAlinhaCheckBoxContrato').show();
        }
    });

    var botaoBasico = $("#cphConteudo_lkbPlanoBasico");
    if (planoBasico) {
        MarcarPlanoBasico(botaoBasico);
    }

    var botaoIndicado = $("#cphConteudo_lkbPlanoIndicado");
    if (planoIndicado) {
        MarcarPlanoIndicado(botaoIndicado);
    }

    var botaoPlus = $("#cphConteudo_lkbPlanoPlus");
    if (planoPlus) {
        MarcarPlanoPlus(botaoPlus);
    }

    botaoBasico.click(function (e) {
        MarcarPlanoBasico($(this));
    });

    botaoIndicado.click(function (e) {
        MarcarPlanoIndicado($(this));
    });

    botaoPlus.click(function (e) {
        MarcarPlanoPlus($(this));
    });
}

function CodigoDescontoAplicarAnimacao() {
    $(".deDesconto").hide("pulsate", 2, "fast");
    $(".deDesconto").show("pulsate", 500, "slow");
}

function MarcarPlanoBasico(btn) {
    planoBasico = true;
    planoIndicado = false;
    planoPlus = false;

    var nova = $('.plan-table div span:nth-child(3)');
    AlterarCSS(nova, btn);
}

function MarcarPlanoIndicado(btn) {
    planoBasico = false;
    planoIndicado = true;
    planoPlus = false;

    var nova = $('.plan-table div span:nth-child(4)');
    AlterarCSS(nova, btn);
}

function MarcarPlanoPlus(btn) {
    planoBasico = false;
    planoIndicado = false;
    planoPlus = true;

    var nova = $('.plan-table div span:nth-child(5)');
    AlterarCSS(nova, btn);
}

function AlterarCSS(selecionada, botao) {
    $('.plan-table .block-bts span .btn').css('background', '#ccc').css('color', '#fff');

    var planos = $('.plan-table div span:nth-child(3), .plan-table div span:nth-child(4), .plan-table div span:nth-child(5)');
    planos.css('background', '#fff').css('color', '#a7a7a7').css('border-bottom', '1px solid #ccc');

    var links = $('.plan-table div span:nth-child(3) a, .plan-table div span:nth-child(4) a, .plan-table div span:nth-child(5) a');
    links.css('color', '#a7a7a7');

    botao.css('background', '#7aa037').css('color', '#fff');

    selecionada.css('background', '#7aa037').css('color', '#fff').css('border-bottom', '1px solid #6f8f37');
    selecionada.find('.ver_contrato').css('color', '#fff');

}