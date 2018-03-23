$(document).ready
(
    function () {
        FadeToggleMaisDados();
    }
);

function FadeToggleMaisDados() {
   // $("*[id*='btlVerMaisNoticiasData']").click(


    $("*[id*='hlkMaisDadosDaNoticia']").click(
        function () {
            var actualObject = this;
            $(".noticias_dia").filter(function () { return ($(this).children().children().find("#" + actualObject.id)).length == 0; }).toggle();
           // $(".noticias_dia").filter(function () { return ($(this).children().children().find("#" + actualObject.id)).length == 0; }).find(".box_sombra").toggle();

            $(this).closest(".noticias_dia").find(".painel_mais_noticias").fadeToggle();
            //$(this).closest(".noticias_dia").find(".texto_mais_noticias").toggle();
           // $(this).closest(".box_sombra").fadeToggle();

            //Ajustando o texto;           
            var text = $(this).text();
            var textoVer = "Ver Notícias dessa data";
            var textoEsconder = "Esconder Notícias dessa data";
            $(this).text(text == textoVer ? textoEsconder : textoVer);
        }
    );
}