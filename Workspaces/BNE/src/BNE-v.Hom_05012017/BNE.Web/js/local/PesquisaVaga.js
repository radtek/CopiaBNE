$(document).ready(function () {
    FadeToggleMaisDados();

    AplicarBneRecomenda();
    setInterval(
        function () {
            PosicionarBneRecomenda();
        }, 50);
    AplicarVipUniversiatarioRecomenda();
    setInterval(
        function () {
            PosicionarVipUniversitarioRecomenda();
        }, 50);

    AjaxLoad();
});

function AjaxLoad() {
    $(".painel_vaga").mouseenter(function () {
        $(this).find(".qualificacaoVaga").animate({ top: "0px" }, 200);
    })
    .mouseleave(function () {
        $(this).find(".qualificacaoVaga").animate({ top: "-25px" }, 200);
    });
}

function FadeToggleMaisDados() {
	$("*[id*='hlkMaisDadosDaVaga']").click(
        function () {
            if ($(this).closest(".painel_vaga").find(".painel_mais_dados_vaga").is(":hidden")) {
                var idvaga = $(this).find("[name$='IdentificadorVaga']").val();
                console.log(idvaga);
                var dados = "{'i':" + idvaga + "}";
                $.ajax({
                    type: "POST",
                    url: "/ajax.aspx/PesquisaVaga_VisualizacaoVaga",
                    data: dados,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false
                });
            }

        	$(this).closest(".painel_vaga").find(".painel_mais_dados_vaga").fadeToggle();
        	$(this).closest(".painel_vaga").find(".texto_atribuicoes").toggle();
        	$(this).closest(".painel_vaga").find(".texto_atribuicoes_completo").toggle();
        }
    );
}

function AplicarBneRecomenda() {
	$("*[class*='bne_recomenda']").each(
        function () {
        	$(this).append("<img alt=\"BNE Recomenda\" class=\"imagem_bne_recomenda\" src=\"/img/img_orelha_bne_recomenda.png\" />");
        }
    );

	PosicionarBneRecomenda();

	$(".imagem_bne_recomenda").fadeIn();
}

function AplicarVipUniversiatarioRecomenda() {
    $("*[class*='vip_universitario_recomenda']").each(
        function () {
            $(this).append("<img alt=\"VIP Universitário Recomenda\" class=\"imagem_vip_universitario_recomenda\" src=\"/img/vip_universitario_recomendado.png\" />");
        }
    );

    PosicionarVipUniversitarioRecomenda();

    $(".imagem_vip_universitario_recomenda").fadeIn();
}

function PosicionarBneRecomenda() {
	$(".imagem_bne_recomenda").each(
        function () {
        	$(this).css("position", "absolute");
        	$(this).css("top", $(this).closest(".painel_vaga").offset().top - 3);
        	$(this).css("left", $(this).closest(".painel_vaga").offset().left + $(this).closest(".painel_vaga").outerWidth() - $(this).outerWidth() + 3);
        }
    );
}

 function PosicionarVipUniversitarioRecomenda() {
     $(".imagem_vip_universitario_recomenda").each(
        function () {
            $(this).css("position", "absolute");
            $(this).css("top", $(this).closest(".painel_vaga").offset().top - 8);
            $(this).css("left", $(this).closest(".painel_vaga").offset().left + $(this).closest(".painel_vaga").outerWidth() - $(this).outerWidth() + 15);
        }
    );
 }

function MaisDadosIPhone() {
	$(".painel_vaga").each(
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
