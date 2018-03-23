function BuscarCandidatura(page) {
    document.getElementById("divQuemMeViu").innerHTML = '';
    var dados = "{'idc':" + $("#hdfIdc").val() + ", 'pag':" + page + ",'pagsize':6 }";
    $.ajax({
        type: "Post",
        url: "/ajax.aspx/QuemMeViu",
        data: dados,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (retorno) {
            console.log(retorno);
            for (var i = 0; i < retorno.d.length; i++) {
                var candidatura = ' <div class="panel panel-default is-finished"> \
                    <div class="panel-heading" role= "tab" id="reading'+ i + '"> \
                    <div class="panel-title">\
                        <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse'+ i + '" aria-expanded="true" aria-controls="collapse' + i + '">\
                            <div class="ja-enviei-icon">\
                                <img src="'+ retorno.d[i].UrlImg + '" style="width:48px;" aria-hidden="true" />\
                            </div>\
                            <div class="ja-enviei-desc">\
                                <div>\
                                    <p class="ja-enviei-desc-occupation">'+ retorno.d[i].RazSocial + '</p>\
                                    <p class="ja-enviei-desc-company {Embasado}">'+ retorno.d[i].AreaEmpresa + '</p>\
                                </div>\
                            </div>\
                            <div class="ja-enviei-status">\
                                <p><strong><span>Currículo visualizado em </span> - '+ retorno.d[i].DataQuemMeViu + ' às ' + retorno.d[i].HoraQuemMeViu + '</strong></p>\
                            </div>\
                    <div class="ja-enviei-tag">';

                candidatura += '</div><div class="ja-enviei-arrow-drop">\
                                <i class="fa fa-chevron-down" aria-hidden="true"></i>\
                            </div>\
                        </a>\
                    </div>\
                </div >\
                    <div id="collapse'+ i + '" class="panel-collapse collapse " role="tabpanel" aria-labelledby="reading' + i + '">\
                        <div class="panel-body">';

                //Qtd Funcionarios
                candidatura += '<div class="panel-body-quem-me-viu"> <div class="div_margin"></div>\
                                <div class="panel-body-columns">\
                                        <p><strong>Número de Funcionários:</strong> '+ retorno.d[i].QtdFuncionario + '</p>\
                                                <p><strong>Telefone:</strong> '+ retorno.d[i].Telefone + '</p>\
                                                  <p><strong>Cidade da empresa:</strong> '+ retorno.d[i].Cidade + '/' + retorno.d[i].SigEstado + '</p>\
                                                  <p><strong>Bairro da empresa:</strong>  '+ retorno.d[i].Bairro + '</p>\
                                                <p><strong>Empresa Cadastrada em:</strong> '+ retorno.d[i].DataCadastro + '</p>\
                                                <p><strong>Currículos Visualizados:</strong> '+ retorno.d[i].TotalVisualizacoes + '</p>\
                                               <p><strong>Vagas Divulgadas:</strong> '+ retorno.d[i].TotalVagas + '</p>\
                                </div>\
                                 <div class="panel-body-action">';
                if (retorno.d[i].LinkVagas != 'Confidencial') {
                    candidatura += '   <a  href="' + retorno.d[i].LinkVagas + '> <button type="button" class="btn">Ver vagas</button></a>';
                }
                candidatura += '</div>\
                  </div>\
                            </div>';

                $("#divQuemMeViu").append(candidatura);

            }


            console.log(retorno.d[0].TotalRegistros);
            if (page == 1) {
                paginacao(retorno.d[0].TotalRegistros);
            }
            $("#upgGlobalCarregandoInformacoes").hide();


        },
        error: function (err) {
            $("#upgGlobalCarregandoInformacoes").hide();
        }

    });
}

$(document).ready(function () {
    BuscarCandidatura(1);
});


function paginacao(totalRegistro) {
    var totalPagina = Math.ceil(totalRegistro / 6);
    $('.paginacaobootstrap').bootpag({
        total: totalPagina,
        page: 1,
        maxVisible: 6,
        leaps: true,
        firstLastUse: true,
        first: '←',
        last: '→',
        wrapClass: 'pagination',
        activeClass: 'active',
        disabledClass: 'disabled',
        nextClass: 'next',
        prevClass: 'prev',
        lastClass: 'last',
        firstClass: 'first'
    }).on("page", function (event, num) {
        ExibirCandidaturas(num);
    });
}

function ExibirCandidaturas(page) {
    $("#upgGlobalCarregandoInformacoes").show();
    setTimeout(function () {
        BuscarCandidatura(page);
    }, 5);
}

function FormatData(data) {
    var d = eval(data.slice(1, -1));
    return (new Date(d).toLocaleDateString() + ' às ' + new Date(d).toLocaleTimeString());
}