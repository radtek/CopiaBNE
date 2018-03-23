/*PAGE LOAD*/
$("#IdCupomDeDescontoText").on('keydown', function (event) {

    if (event.keyCode === 13)
        calculaCupomDeDesconto();

});
/* FIM PAGE LOAD*/
/*-----------CUPOM DE DESCONTO -----------------------------*/
function calculaCupomDeDesconto() {


    var classoAplicada = $("#erroCupom").hasClass("has-error");

    var Dados = {};
    Dados.cupom = $("#IdCupomDeDescontoText").val();

    if (empty(Dados.cupom)) {
        if (!classoAplicada) {
            $("#erroCupom").toggleClass("has-error");
            $("#msgErroCupom").html("Digite um código de desconto!");
        }
        return;
    }

    Dados.idPlanoMensal = $("#txtIdPlanoMensal").val();
    Dados.idPlanoTrimestral = $("#txtIdPlanoTrimestral").val();
    //Envia a requisição
    $.ajax({
        type: "POST",
        url: "Payment.aspx/CalculaCupomDeDesconto",
        data: JSON.stringify(Dados),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            var result = response.d;

            if (!result.Falha) {

                //validacao
                if (classoAplicada)
                    $("#erroCupom").toggleClass("has-error");
                $("#msgErroCupom").html("");

                //insercao
                for (var i = 0; i < result.Dados.PlanosDescontos.length; i++) {
                    var item = result.Dados.PlanosDescontos[i];
                    if (item.TipoPlano == "Mensal")
                        $("#litValorPlanoMensal").html(item.Dados.Valor);
                    else if (item.TipoPlano == "Trimestral")
                        $("#litValorPlanoTrimestral").html(item.Dados.Valor);
                    else
                        $("#litValorPlanoRecorrente").html(item.Dados.Valor);
                }

                $("#txtIdCodigoDeDesconto").val(item.Dados.IdCodigoDeDesconto);
                var input = document.getElementById('cupom-desconto');
                var cupom = document.getElementsByClassName("com-desconto");

                $(input).css("display", "none");
                $(cupom).css("display", "inline-block");

                $("#cupomModal").modal('hide');

            } else {
                if (!empty(result.Url)) window.location = result.Url;
                else {
                    if (!classoAplicada) $("#erroCupom").toggleClass("has-error");
                    $("#msgErroCupom").html(empty(result.Mensagem) ? "Digite um cupom válido!" : result.Mensagem);
                    $("#IdCupomDeDescontoText").val("");
                    $("#txtIdCodigoDeDesconto").val("");
                }
            }
        }
    });
}

/*-----------FIM CUPOM DE DESCONTO -----------------------------*/