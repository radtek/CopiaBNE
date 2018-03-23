//Page load específica desta página
function pageLoadSpecific() {
    inicializar();
}

function inicializar()
{
    $(".container_cv_foto_candidato").css("left", ($(window).width() / 2) - ($(".container_cv_foto_candidato").width() / 2));
    $(".container_cv_foto_candidato").css("top", $(".container_topo_quem_me_viu").offset().top + $(".container_topo_quem_me_viu").outerHeight(true) - 140);

    $(".container_texto_item_timeline div").each(
		function () {
		    if ($(this).height() < 50) {
		        $(this).css("margin-top", "44px");
		    }
		    else if ($(this).height() > 50) {
		        $(this).css("margin-top", "32px");
		    }
		}
	);

    $(".painel_opcoes_pagamento img").live("click",
		function () {
		    $(this).nextAll(":radio:first").get()[0].checked = true;
		}
	);
}

$(document).ready(
    function () {

    }
);