$(document).ready(function () {

    $(".balao").hide();
    $(".textoCidade").hide();
    $(".txCidade").show();
    $(".textoFuncao").hide();
    $(".txFuncao").show();

    $('.txFuncao').click(function () {
        $(".balao").show();
        $(".body2").show();
        $(".handup2").show();
        $(".handown2").show();
        $("#texto2").fadeIn('1000');
        $(".body2").fadeOut('slow');
        $(".handup2").fadeOut('slow');
        $(".handown2").fadeOut('slow');
        $(".textoFuncao").show();
        $(".textoCidade").hide();
    });

    $('.txCidade').click(function () {
        $(".balao").show();
        $(".body2").hide();
        $(".handup2").hide();
        $(".handown2").hide();
        $(".body2").fadeIn('slow');
        $(".handup2").fadeIn('slow');
        $(".handown2").fadeIn('slow');
        $("#texto1").fadeIn('1000');

        $(".textoCidade").show();
        $(".textoFuncao").hide();
    });

    //$(".txCidade").click();
    //$(".txCidade").focus().val($(".txCidade").val());
});
function clientSelect(source, eventArgs) {

    var hf = (source._id.indexOf("Cidade") > 0) ? employer.util.findControl('CidadesSel') : employer.util.findControl('FuncoesSel');
    hf[0].value = eventArgs._value;
}