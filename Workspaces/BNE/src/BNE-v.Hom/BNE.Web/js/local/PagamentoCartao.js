$(".painel_botao_opcao_bandeira img").live("click",
    function () {
        $(this).next().next().click();
    }
);
$().ready(function () {
    $("#cphConteudo_txtNumeroDoCartaoParte1_txtValor").keyup(
    function () {
        var val = $("#cphConteudo_txtNumeroDoCartaoParte1_txtValor").val();
        if (val == "")
            return;
        if (val[0] == "4") {
            $('#cphConteudo_rbtVisaCredito').attr('checked', true);
            return;
        }
        if (val[0] == "5") {
            $('#cphConteudo_rbtMastercardCredito').attr('checked', true);
            return;
        }
    }
)
    });