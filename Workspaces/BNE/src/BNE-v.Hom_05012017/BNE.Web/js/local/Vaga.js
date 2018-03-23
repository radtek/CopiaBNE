$(document).ready(
    function () {

        AplicarBneRecomenda();

        setInterval(
            function () {
                PosicionarBneRecomenda();
            }, 50);
    }
);

function AplicarBneRecomenda() {
    $("*[class*='bne_recomenda']").each(
        function () {
            $(this).append("<img alt=\"BNE Recomenda\" class=\"imagem_bne_recomenda\" src=\"/img/img_orelha_bne_recomenda.png\" />");
        }
    );

    /* PosicionarBneRecomenda(); */

    $(".imagem_bne_recomenda").fadeIn();
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