$(function () {
    positionFooter();

    function positionFooter() {
        if ($("#conteudo").length == 0)
            return;

        var bodyHeight = $(document).height();
        var windowHeight = $(window).height();

        if (bodyHeight > windowHeight) {
            $("#barra_rodape").css({
                margin: "0 0 0 0"
            });
            return;
        }
        var contentInitial = $("#header").height();
        var contentHeight = $("#conteudo").height();

        if (contentHeight == 0
            || contentHeight == typeof (undefined)) {
            return;
        }

        var heightBottom = 96;

        var sumOfPage = (Math.max(contentInitial, $("#conteudo").position().top) + contentHeight + heightBottom) + 35;
        if (windowHeight > sumOfPage) {
            var difference = windowHeight - sumOfPage;
            if (difference < 0)
                difference = 0;

            $("#barra_rodape").css({
                margin: difference + "px 0 0 0"
            });
        }
    }

    $(window)
        .resize(function () {
            positionFooter();
        });
});

$(document).ready(
    function () {
        autocomplete.funcao('txtFuncao');
        autocomplete.cidade('txtCidade');
    }
);
