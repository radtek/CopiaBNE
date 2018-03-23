$(document).ready(function () {

    dtpiker();
    $(document).bind("ajaxSend", function () {
    }).bind("ajaxComplete", function () {
        $("#upgGlobalCarregandoInformacoes").hide();
    });

});

function DesableBtn() {
    $("#btnInformacoesCand");
    $("#btnInformacaoesVisualizacao");
    $("#btnInformacaoesEnvioCV");
    $("#btnInformacaoesAlerta");
}

function upDate(campo) {
    if (campo == "Inicio") {
        $("#hdfDtaInicio").val($("#txtDataInicio").val());
    }
    else {
        $("#hdfDtaFim").val($("#txtDataFim").val());
    }
}


function ExibirCandidaturas(page) {
    $("#upgGlobalCarregandoInformacoes").show();
    setTimeout(function () {
        ExibirCandidaturas2(page);
    }, 10);
}


function ExibirCandidaturas2(page) {
    hidediv();

    var dados = "{'idc':" + $("#hdfCV").val() + ", 'page':" + page + ",'pagesize':10, 'DtaInicio': '" + ajustedata($("#txtDataInicio").val()) + "', 'DtaFim':'" + ajustedata($("#txtDataFim").val()) + "'}";
    $.ajax({
        type: "Post",
        url: "/ajax.aspx/CarregarCandidaturas",
        data: dados,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (retorno) {
            document.getElementById("divTitulo").innerHTML = document.getElementById("divPaginacao").innerHTML = document.getElementById("tbRegistros").innerHTML = '';
            $("#divTitulo").append('Candidaturas entre ' + $("#txtDataInicio").val() + ' e ' + $("#txtDataFim").val() + ' ');


            if (retorno.d.Candidaturas != null) {
                for (var i = 0; i < retorno.d.Candidaturas.length; i++) {
                    document.getElementById("tbRegistros").innerHTML = document.getElementById("tbRegistros").innerHTML + '<tr><td>' + retorno.d.Candidaturas[i].codVaga + '</td><td>' + retorno.d.Candidaturas[i].Funcao + '</td> <td>' + retorno.d.Candidaturas[i].Cidade + '</td><td>' + retorno.d.Candidaturas[i].DataCadastro + '</td><td>' + (retorno.d.Candidaturas[i].Status == "Inativa" ? '' : '<a target="_blank" href="' + retorno.d.Candidaturas[i].linkVaga + '"> Visualizar Vaga</a>') + '</td><td>' + (retorno.d.Candidaturas[i].Status == "Ativa" ? '<span class="label label-success">' : retorno.d.Candidaturas[i].Status == "Oportunidade" ? '<span class="label label-warning">' : '<span class="label label-default">') + retorno.d.Candidaturas[i].Status + '</span></td></tr>';
                }
            }
            $("#divPaginacao").append(' <ul class="pagination pull-right">');

            var pag = Math.ceil(retorno.d.TotalRegistros / 10);
            for (var i = 1; i <= pag; i++) {

                if (i == page) {
                    $("#divPaginacao").append('<li onclick="ExibirCandidaturas(' + i + ');" class="active"><a >' + i + '</a></li>');
                }
                else {
                    $("#divPaginacao").append('<li onclick="ExibirCandidaturas(' + i + ');"  ><a >' + i + '</a></li>');
                }
            }

            $("#divCandidaturas").show();
        },
        error: function (err) {


        }

    });



}


function hidediv() {
    $("#divCandidaturas").hide();
    $("#divEmpresasVisualizadas").hide();
    $("#divEmpresasEnvio").hide();
    $("#divAlertaCvs").hide();
}

function dtpiker() {
    $("#txtDataInicio").datetimepicker({
        format: "dd/mm/yyyy",
        todayBtn: true,
        language: "pt-BR",
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        minView: 2,
        forceParse: 0,
        showMeridian: 1,
        pickTime: false,
        default: false,
        allowInputToggle: true
    });

    $("#txtDataFim").datetimepicker({
        format: "dd/mm/yyyy",
        todayBtn: true,
        minuteStep: 5,
        language: "pt-BR",
        autoclose: 1,
        todayHighlight: 1,
        startView: 2,
        forceParse: 0,
        pickTime: false,
        minView: 2,
        showMeridian: 1,
        allowInputToggle: true
    });
}

function ajustedata(data) {
    var aj = data.split('/');
    return aj[1] + '/' + aj[0] + '/' + aj[2];
}

function ExibirVisualizacoes(page) {
    $("#upgGlobalCarregandoInformacoes").show();
    setTimeout(function () {
        ExibirVisualizacoes2(page);
    }, 10);
}

function ExibirVisualizacoes2(page) {
    hidediv();
    var dados = "{'idc':" + $("#hdfCV").val() + ", 'page':" + page + ",'pagesize':10, 'DtaInicio': '" + ajustedata($("#txtDataInicio").val()) + "', 'DtaFim':'" + ajustedata($("#txtDataFim").val()) + "'}";
    $.ajax({
        type: "Post",
        url: "/ajax.aspx/EmpresaVisualizaram",
        data: dados,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (retorno) {
            document.getElementById("divTituloVisualizado").innerHTML = document.getElementById("divPaginacaoVisualizados").innerHTML = document.getElementById("tbRegistrosVisualizados").innerHTML = '';
            $("#divTituloVisualizado").append('Visualizações entre ' + $("#txtDataInicio").val() + ' e ' + $("#txtDataFim").val() + ' ');

            if (retorno.d.Visualizacoes != null) {
                for (var i = 0; i < retorno.d.Visualizacoes.length; i++) {
                    document.getElementById("tbRegistrosVisualizados").innerHTML = document.getElementById("tbRegistrosVisualizados").innerHTML + '<tr><td>' + retorno.d.Visualizacoes[i].Nome_Empresa + '</td><td>' + retorno.d.Visualizacoes[i].Data_Visualizacao + '</td></tr>';
                }
            }
            document.getElementById("divPaginacaoVisualizados").innerHTML = document.getElementById("divPaginacaoVisualizados").innerHTML + ' <ul class="pagination pull-right">';

            var pag = Math.ceil(retorno.d.TotalRegistros / 10);
            for (var i = 1; i <= pag; i++) {

                if (i == page) {
                    document.getElementById("divPaginacaoVisualizados").innerHTML = document.getElementById("divPaginacaoVisualizados").innerHTML + '<li onclick="ExibirVisualizacoes(' + i + ');" class="active"><a >' + i + '</a></li>';
                }
                else {
                    $("#divPaginacaoVisualizados").append('<li onclick="ExibirVisualizacoes(' + i + ');"  ><a >' + i + '</a></li>');
                }
            }

            $("#divEmpresasVisualizadas").show();
        },
        error: function (err) {

        }

    });



}

function ExibirEnvioEmpresa(page) {
    $("#upgGlobalCarregandoInformacoes").show();
    setTimeout(function () {
        ExibirEnvioEmpresa2(page);
    }, 10);
}

function ExibirEnvioEmpresa2(page) {
    hidediv();
    var dados = "{'idc':" + $("#hdfCV").val() + ", 'page':" + page + ",'pagesize':10, 'DtaInicio': '" + ajustedata($("#txtDataInicio").val()) + "', 'DtaFim':'" + ajustedata($("#txtDataFim").val()) + "'}";
    $.ajax({
        type: "Post",
        url: "/ajax.aspx/EnvioCurriculoParaEmpresa",
        data: dados,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (retorno) {

            document.getElementById("divTituloEmpresaEnvio").innerHTML = document.getElementById("divPaginacaoEmpresaEnvio").innerHTML = document.getElementById("tbRegistrosEmpresaEnvio").innerHTML = '';
            $("#divTituloEmpresaEnvio").append('Envio de currículo entre ' + $("#txtDataInicio").val() + ' e ' + $("#txtDataFim").val() + ' ');
            if (retorno.d.EnvioEmpresa != null) {
                for (var i = 0; i < retorno.d.EnvioEmpresa.length; i++) {
                    document.getElementById("tbRegistrosEmpresaEnvio").innerHTML = document.getElementById("tbRegistrosEmpresaEnvio").innerHTML + '<tr class="hoverable"><td>' + retorno.d.EnvioEmpresa[i].Nome_Empresa + '</td><td>' + retorno.d.EnvioEmpresa[i].Data_Envio + '</td></tr>';
                }
            }
            $("#divPaginacaoEmpresaEnvio").append(' <ul class="pagination pull-right">');

            var pag = Math.ceil(retorno.d.TotalRegistros / 10);
            for (var i = 1; i <= pag; i++) {

                if (i == page) {
                    $("#divPaginacaoEmpresaEnvio").append('<li onclick="ExibirEnvioEmpresa(' + i + ');" class="active"><a >' + i + '</a></li>');
                }
                else {
                    $("#divPaginacaoEmpresaEnvio").append('<li onclick="ExibirEnvioEmpresa(' + i + ');"  ><a >' + i + '</a></li>');
                }
            }

            $("#divEmpresasEnvio").show();
        },
        error: function (err) {

        }

    });
}

function ExibirAlertaVaga(page) {
    $("#upgGlobalCarregandoInformacoes").show();
    setTimeout(function () {
        ExibirAlertaVaga2(page);
    }, 10);
}

function ExibirAlertaVaga2() {
    hidediv();
    var dados = "{'idc':" + $("#hdfCV").val() + "}";
    $.ajax({
        type: "Post",
        url: "/ajax.aspx/AlertaVagas",
        data: dados,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (retorno) {
            document.getElementById("divAlertaCidade").innerHTML = document.getElementById("divAlertaFuncoes").innerHTML = document.getElementById("divAlertaDiaSemana").innerHTML = '';

            if (retorno.d.Dias != null) {
                for (var i = 0; i < retorno.d.Dias.length; i++) {
                    $("#divAlertaDiaSemana").append('<div class=" btn-group "  style="padding:4px;">  <label for="fancy-checkbox-success" class=" btn btn-success "> <span class=" glyphicon glyphicon-ok "></span> <span></span></label > <label for="fancy-checkbox-success" class=" btn btn-default active" style="padding: 7.5px;"> ' + retorno.d.Dias[i] + '</label></div >');
                }
            }
            if (retorno.d.Funcoes != null) {
                for (var i = 0; i < retorno.d.Funcoes.length; i++) {
                    $("#divAlertaFuncoes").append('<div class=" btn-group " style="padding:4px;"> <label for="fancy-checkbox-success" class=" btn btn-default active" style="padding: 7.5px;"> ' + retorno.d.Funcoes[i] + '</label></div >');
                }
            }
            if (retorno.d.Cidades != null) {
                for (var i = 0; i < retorno.d.Cidades.length; i++) {
                    $("#divAlertaCidade").append('<div class=" btn-group " style="padding:4px;"> <label for="fancy-checkbox-success" class=" btn btn-default active" style="padding: 7.5px;"> ' + retorno.d.Cidades[i] + '</label></div >');
                }
            }

            $("#divAlertaCvs").show();
        },
        error: function (err) {

        }

    });
}

