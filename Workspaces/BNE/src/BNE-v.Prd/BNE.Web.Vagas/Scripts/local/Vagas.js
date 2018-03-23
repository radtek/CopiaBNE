$(document).ready(function () {
    FadeToggleMaisDados();

    AplicarVipUniversitarioRecomenda();
    setInterval(
        function () {
            PosicionarVipUniversitarioRecomenda();
        }, 50);

    QualificacaoVaga();

    setInterval(
        function () {
            PosicionarCandidaturaGratis();
        }, 50);

    $('.share-buttons a').on('click', function (e) {
        var self = $(this);
        var title = self.find(".rrssb-text").text();
        trackEvent("VisualizacaoVaga", "ShareClick", title);
    });

    $('.rrssb-email.deslogado a').off('click');
    $('.rrssb-email.deslogado a').on('click', function (e) {
        $(this).closest("form").submit();
    });
});

function QualificacaoVaga() {
    $(".vaga").mouseenter(function () {
        $(this).find(".qualificacaoVaga").animate({ top: "0px" }, 200);
    })
    .mouseleave(function () {
        $(this).find(".qualificacaoVaga").animate({ top: "-21px" }, 200);
    });
}

function FadeToggleMaisDados() {
    $(".vaga .painel_mais_dados_vaga").css('display', 'none');
    $(".vaga .painel_mais_dados_vaga").css('visibility', 'visible');
    $(".vaga").css('height', 'auto');

    $("*[class*='spanMaisDadosDaVaga']").click(function () {
        if ($(this).closest(".vaga").find(".painel_mais_dados_vaga").is(":hidden")) {
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
    //se a url Nao contem a palavra "vagas", ajustar css para pagina de visualizacao da Vaga
    if (!/\bvagas\b/.test(window.location.pathname)) {
        $(".imagem_candidatura_gratis").each(
           function () {
               $(this).css("top", $(this).closest(".vaga").offset().top - 228);
           }
       );
    }
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
    configuracoes: function () {
        return { url: window.location.href, title: $(document).find("title").text().trim() }
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
    },
    facebook: function () {
        var settings = compartilhar.configuracoes();
        return 'https://www.facebook.com/sharer/sharer.php?u=' + settings.url;
    },
    linkedin: function () {
        var settings = compartilhar.configuracoes();
        return 'http://www.linkedin.com/shareArticle?mini=true&url=' + compartilhar.url + (settings.title !== undefined ? '&title=' + settings.title : '') + (settings.description !== undefined ? '&summary=' + settings.description : '');
    },
    twitter: function () {
        var settings = compartilhar.configuracoes();
        return 'http://twitter.com/home?status=' + (settings.description !== undefined ? settings.description : '') + '%20' + settings.url;
    },
    googleplus: function () {
        var settings = compartilhar.configuracoes();
        return 'https://plus.google.com/share?url=' + (settings.description !== undefined ? settings.description : '') + '%20' + settings.url;
    },
    whatsapp: function () {
        var settings = compartilhar.configuracoes();
        return 'whatsapp://send?text=' + settings.title + ' ' + settings.url;
    }

};

function ValidarEmail() {
    var email = $.trim($('#Email').val());

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

    v_obj = o;
    v_fun = f;
    setTimeout("execmascara()", 1);
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



function ValResposta() {
    var result = true;
    var resp = document.getElementsByClassName("ValResposta");
    var aviso = document.getElementsByClassName("AvisoTexto");
    for (var i = 0; i <= aviso.length; i++) {
        if ($("textarea[name='Perguntas.Perguntas[" + i + "].DescricaoResposta']").length > 0) {
            if ($("textarea[name='Perguntas.Perguntas["+i+"].DescricaoResposta']")[0].value.length > 1) {
                aviso[i].style.display = 'none';
            }
            else {
                aviso[i].innerHTML = "Campo Obrigatório";
                aviso[i].style.display = 'block';
                aviso[i].style.color = "red";
                result = false;
            }
        }
    }
    return result;
}

function ValCheckBox(){
    var result = true;
    var resp = document.getElementsByClassName("ValCheckBox");
    var aviso = document.getElementsByClassName("AvisoCheck");
    for (var i = 0; i <= resp.length; i++) {
        if ($("input[name='Perguntas.Perguntas[" + i + "].Resposta']").length>0) {
            if ($("input[name='Perguntas.Perguntas[" + i + "].Resposta']:checked").val() != undefined) {
                aviso[i].style.display = 'none';
            }
            else {
                var numero = "";
                var thenum = numero.replace(/^.*(\d+).*$/i, '$1');
                aviso[i].innerHTML = "Campo Obrigatório";
                aviso[i].style.display = 'block';
                aviso[i].style.color = "red";
                result = false;
            }

            if (document.querySelectorAll('input[type="radio"]:checked').length !=
                (document.querySelectorAll('input[type="radio"').length / 2)) {
                result = false
            }
       
        }
    }
    return result;
}

function Validar() {
        if (ValResposta() && ValCheckBox()) {
            return true;
        }
        else{
            return false;
        }
}