$(document).ready(function () {
    FadeToggleMaisDados();

    AplicarVipUniversitarioRecomenda();
    setInterval(
        function () {
            PosicionarVipUniversitarioRecomenda();
        }, 50);

    QualificacaoVaga();
    CandidaturaGratis();

    setInterval(
        function () {
            PosicionarCandidaturaGratis();
        }, 50);

});

function QualificacaoVaga() {
    $(".vaga").mouseenter(function () {
        $(this).find(".qualificacaoVaga").animate({ top: "-15px" }, 200);
    })
    .mouseleave(function () {
        $(this).find(".qualificacaoVaga").animate({ top: "-37px" }, 200);
    });
}

function CandidaturaGratis() {
    $(".vaga").each(
        function () {
            var idvaga = $(this).find("[name='IdentificadorVaga']").val();
            var vaga = $(this);
            $.ajax({
                url: "/Vaga/CG",
                data: {
                    i: idvaga
                },
                type: "POST",
                async: true,
                success: function (d) {
                    if (d) {
                        var img = "<img alt='Candidatura Grátis' class='imagem_candidatura_gratis' src='/Images/img_bne_acessolivre.png' />";
                        vaga.prepend(img);
                    }
                }
            });
        }
    );
}

function FadeToggleMaisDados() {
    $("*[class*='spanMaisDadosDaVaga']").click(function () {
        if ($(this).closest(".vaga").find(".painel_mais_dados_vaga").is( ":hidden" )) {
            var idvaga = $(this).find("[id='IdentificadorVaga']").val();

            $.ajax({
                url: "/Vaga/VV",
                data: {
                    i: idvaga
                },
                type: "POST",
                async: true
            });
        }

        $(this).closest(".vaga").find(".painel_mais_dados_vaga").fadeToggle();
        $(this).closest(".vaga").find(".atribuicao").toggle();
        $(this).closest(".vaga").closest(".painel_mais_dados_vaga").find(".atribuicao").toggle();
    }
    );
}

function AplicarVipUniversitarioRecomenda() {
    $("*[class*='vip_universitario_recomenda']").each(
        function () {
            $(this).append("<img alt=\"VIP Universitário Recomenda\" class=\"imagem_vip_universitario_recomenda\" src=\"/Images/img_vip_universitario_recomenda.png\" />");
        }
    );

    PosicionarVipUniversitarioRecomenda();

    $(".imagem_vip_universitario_recomenda").fadeIn();
}

function PosicionarVipUniversitarioRecomenda() {
    $(".imagem_vip_universitario_recomenda").each(
       function () {
           $(this).css("position", "absolute");
           $(this).css("top", $(this).closest(".vaga").offset().top - 8);
           $(this).css("left", $(this).closest(".vaga").offset().left + $(this).closest(".vaga").outerWidth() - $(this).outerWidth() + 15);
       }
   );
}

function PosicionarCandidaturaGratis() {
    $(".imagem_candidatura_gratis").each(
        function () {
            $(this).css("position", "absolute");
            $(this).css("top", $(this).closest(".vaga").offset().top - 208);
            $(this).css("left", $(this).closest(".vaga").outerWidth() - 78);
        }
    );
}

function MaisDadosIPhone() {
    $(".vaga").each(
		  function () {
		      $(this).find(".painel_mais_dados_vaga").fadeToggle();
		      $(this).find(".texto_atribuicoes").toggle();
		      $(this).find(".texto_atribuicoes_completo").toggle();
		  }
	);
}

function AjustarRolagemParaTopo() {
    $('html,body').animate({ scrollTop: 0 });
}

var compartilhar = {
    facebook: function (funcao, salarioCidade, url, urlIconeFacebook) {
        FB.ui({
            method: 'feed',
            name: 'Vaga de ' + funcao,
            link: url,
            picture: urlIconeFacebook,
            caption: salarioCidade
        },
            function (response) {
                if (response && response.post_id) {
                    $.ajax({
                        url: "/Vaga/ConfirmacaoCompatilhamentoFacebook",
                        type: "GET",
                        async: true,
                        success: function (result) {
                            $("#modal_container").html(result);
                        }
                    });

                }
            });
    },
    email: function (identificador, url) {
        $.ajax({
            url: "/Vaga/AbrirCompartilhamentoEmail",
            data: {
                identificador: identificador,
                url: url
            },
            type: "GET",
            async: true,
            success: function (result) {
                $("#modal_container").html(result);
            }
        });
    }
};

function ValidarEmail() {

    var email = $.trim($('#Email').val())

    if (email.length == 0) {
        $('.btn-primary').attr('disabled', 'disabled');
        return;
    }

    $('.btn-primary').removeAttr('disabled');

}

function ValidarCelular() {

    var numeroCelular = $.trim($('#NumeroCelular').val());
    var totalCaracteres = numeroCelular.length;
    var key = event.which || event.keyCode || event.charCode;

    if (key != 8 && totalCaracteres >= 9) { return; }

    $('.botaoCorreto, .espacoBotao').attr('disabled', 'disabled');
    $('#error').html('Telefone inválido.');

    if (key == 8 && totalCaracteres <= 7) {
        return;
    }
    if (key != 8 && totalCaracteres <= 6) {
        return;
    }

    if (key != 8) {
        totalCaracteres++;
        if (totalCaracteres == 8 && numeroCelular <= 4999999) {
            return;
        }
        if (totalCaracteres == 9 && numeroCelular <= 49999999) {
            return;
        }

    } else {
        numeroCelular = numeroCelular.substring(0, numeroCelular.length - 1);

        if (totalCaracteres - 1 == 7) {
            return;
        }

        if (totalCaracteres == 8 && numeroCelular <= 4999999) {
            return;
        }
        if (totalCaracteres == 9 && numeroCelular <= 49999999) {
            return;
        }
        if (totalCaracteres == 10 && numeroCelular <= 499999999) {
            return;
        }
    }

    $('.botaoCorreto, .espacoBotao').removeAttr('disabled');
    $('#error').html('');
}

function SetarFocus() {

    var ddd = $('#NumeroDDDCelular').val();
    var numeroTel = $('#NumeroCelular').val();

    if (ddd == 11 && numeroTel.length == 9) {
        $('#CodigoValidacao').focus();
    } else if (numeroTel.length == 8) {
        $('#CodigoValidacao').focus();
    }

}

function SomenteNumero(e) {
    var tecla = (window.event) ? event.keyCode : e.which;
    if ((tecla > 47 && tecla < 58)) return true;
    else {
        if (tecla == 8 || tecla == 0) return true;
        else return false;
    }
}

/* Máscaras ER */
function mascara(o, f) {

    //ocultar botao "está correto" ao digitar o telefone
    $('.botaoCorreto').hide();

    v_obj = o
    v_fun = f
    setTimeout("execmascara()", 1)
}
function execmascara() {
    v_obj.value = v_fun(v_obj.value);
}
function DDDtel(v) {
    v = v.replace(/\D/g, ""); //Remove tudo o que não é dígito
    return v;
}

function mtel(v) {
    v = v.replace(/\D/g, ""); //Remove tudo o que não é dígito
    return v;
}