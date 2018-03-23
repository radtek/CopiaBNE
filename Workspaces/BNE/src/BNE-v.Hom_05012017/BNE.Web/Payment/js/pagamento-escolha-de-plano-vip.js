/*PAGE LOAD*/
/*------------------- Scroll Collapse Formas de Pagamento -------------------*/

/*REMOVENDO OS POSTBACKS AUTOMATICOS DOS INPUTS*/
$('#btPayPal').click(function (event) {
    event.preventDefault();
});

$('#btnCopiar').click(function (event) {
    event.preventDefault();
});

$('#btPagSeguro').click(function (event) {
    event.preventDefault();
});

$('#btPagamentoBB').click(function (event) {
    event.preventDefault();
});

$('#btnEnviarPorEmail').click(function (event) {
    event.preventDefault();
});

$('#btPagamentoBradesco').click(function (event) {
    event.preventDefault();
});

$('#btnFinalizarDebitoHSBC').click(function (event) {
    event.preventDefault();
});

$('#btFinalizarCartaoDeCredito').click(function (event) {
    event.preventDefault();
});

/*FIM*/

var hasClicked = false;
$('.payment-method-type').click(function (e) {
    if (!hasClicked) {
        $('html, body').animate({
            scrollTop: $(this).offset().top - 8
        }, 'slow');
        hasClicked = !hasClicked;
    }
});



/*------------------- Botão Rolar pra baixo —-----------------*/

function exibirFormasDePagamentoRecorrente() {
    $("#updDebitoOnline").remove();
    $("#updBoleto").remove();
    $("#updPagseguro").remove();
    $("#updPaypal").remove();
}


/*----------------------------------------------------------------*/
/*-------------------------FIM ROLAGEM ---------------------------*/


/*-----------------------ESCOLHA PLANO Events----------------------------*/
function VerificaUsuarioLogado() {

    if (!empty($("#txtCurriculo").val())) {

        var Dados = {};
        Dados.idCurriculo = $("#txtCurriculo").val();
        Dados.stcUniversitario = $("#txtUniversitarioSTC").val() == "1" ? true : false;

        $.ajax({
            type: "POST",
            url: "Payment.aspx/PlanosDisponiveis",
            data: JSON.stringify(Dados),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var result = response.d;
                if (result.Falha) {
                    if (!empty(result.Url))
                        window.location = result.Url;
                    exibirLogin(true);
                }
                else {
                    for (var i = 0; i < result.Dados.length; i++) {
                        var item = result.Dados[i];
                        if (item.TipoPlano == "Mensal") {
                            $("#litNomePlanoMensal").html(item.Plano);
                            $("#litValorPlanoMensal").html(item.Valor);
                            $("#txtIdPlanoMensal").val(item.Id);
                        } else if (item.TipoPlano == "Trimestral") {
                            $("#litNomePlanoTrimestral").html(item.Plano);
                            $("#litValorPlanoTrimestral").html(item.Valor);
                            $("#txtIdPlanoTrimestral").val(item.Id);
                        } else {
                            $("#litNomePlanoRecorrente").html(item.Plano);
                            $("#litValorPlanoRecorrente").html(item.Valor);
                            $("#txtIdPlanoRecorrente").val(item.Id);
                        }
                    }
                    scroll_to_anchor("summary-stage");
                }
            }
        });
    }
    return false;
}

function EscolhaDePlano(plano) {

    var Dados = {};

    Dados.idCupomDeDesconto = empty($("#txtIdCodigoDeDesconto").val()) ? null : $("#txtIdCodigoDeDesconto").val();
    Dados.idPessoaFisica = empty($("#txtPessoaFisica").val()) ? null : $("#txtPessoaFisica").val(); 
    Dados.idUsuarioFilialPerfilLogadoCandidato = empty($("#txtUsuarioFilialPerfil").val()) ? null : $("#txtUsuarioFilialPerfil").val();

    if('campanha' == plano)
        Dados.idPlano = $('#txtIdPlanoCampanha').val();
    else if ('Recorrente' == plano)
        Dados.idPlano = $('#txtIdPlanoRecorrente').val();
    else if ('Mensal' == plano)
        Dados.idPlano = $('#txtIdPlanoMensal').val();
    else
        Dados.idPlano = $('#txtIdPlanoTrimestral').val();

    //Envia a requisição
    $.ajax({
        type: "POST",
        url: "Payment.aspx/ValidarCriacaoDoPlano",
        data: JSON.stringify(Dados),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            var result = response.d;

            if (result.Falha) {
                if (!empty(result.Url))
                    window.location = result.Url;
                exibirLogin(true);
            }
            else {
                $("#txtPlanoAdquirido").val(result.Dados.IdPlanoAdquirido);
                $("#litNomeCliente").html(result.Dados.NomeUsuario);
                $("#ltNomePlano").html(result.Dados.NomePlano);

                if (result.Dados.Recorrente)
                    exibirFormasDePagamentoRecorrente();
                scroll_to_anchor('payment-stage');
            }
        }
    });
}



/*-----------------------FIM ESCOLHA PLANO------------------------*/

