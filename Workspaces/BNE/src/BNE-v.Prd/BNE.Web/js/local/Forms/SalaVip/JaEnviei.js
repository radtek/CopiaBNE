function BuscarCandidatura(page) {
    document.getElementById("divCandidaturas").innerHTML = '';
    var dados = "{'idc':'" + $("#hdfIdc").val() + "', 'pag':" + page + ",'pagsize':6 }";
    $.ajax({
        type: "Post",
        url: "/ajax.aspx/JaEnviei",
        data: dados,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (retorno) {
            console.log(retorno);
            for (var i = 0; i < retorno.d.length; i++) {
                var candidatura = ' <div class="panel panel-default {Termino}"> \
                    <div class="panel-heading" role= "tab" id="reading'+ i + '"> \
                    <div class="panel-title">\
                        <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapse'+ i + '" aria-expanded="true" aria-controls="collapse' + i + '">\
                            <div class="ja-enviei-icon">\
                                <i class="{Icone}" aria-hidden="true"></i>\
                            </div>\
                            <div class="ja-enviei-desc">\
                                <div>\
                                    <p class="ja-enviei-desc-occupation">'+ retorno.d[i].Funcao + '</p>\
                                    <p class="ja-enviei-desc-company {Embasado}">{NomeEmpresa}</p>\
                                </div>\
                            </div>\
                            <div class="ja-enviei-status">\
                                <p>Status:</p>\
                                <p><strong><span>{StatusFrase}</span> - {DataHora}</strong></p>\
                            </div>\
                    <div class="ja-enviei-tag"> ';
                if (retorno.d[i].PCD) {
                    candidatura += '<p class="tag pcd"><i class="fa fa-wheelchair"></i></p>';
                }
                if (retorno.d[i].Inativo) {
                    candidatura += '<p class="tag inativa">vaga inativa</p>';
                }
                else if (retorno.d[i].Oportunidade) {
                    candidatura += '<p class="tag oportunidade">oportunidade</p>';
                }
                else {
                    candidatura += '<p class="tag vaga">vaga aberta</p>';
                }
                candidatura += '</div><div class="ja-enviei-arrow-drop">\
                                <i class="fa fa-chevron-down" aria-hidden="true"></i>\
                            </div>\
                        </a>\
                    </div>\
                </div >\
                    <div id="collapse'+ i + '" class="panel-collapse collapse " role="tabpanel" aria-labelledby="reading' + i + '">\
                        <div class="panel-body">';
                var indiceUltimaEtpa = UltimaEtapa(retorno.d[i].Etapas);

                for (var e = 0; e < retorno.d[i].Etapas.length; e++) {
                    switch (retorno.d[i].Etapas[e].Etapa) {

                        case 'Visualizado':
                            candidatura += '<div class="ja-enviei-timeline-item"> \
                                <div class="item-marking">\
                                    <div>\
                                        <div class="item-check '+ (indiceUltimaEtpa == e ? 'item-check-etapa' : '') +'"></div> \
                                        <div class="item-separator"></div> \
                                    </div>\
                                </div>\
                                 <div class="item-desc '+ (indiceUltimaEtpa <= e ? 'item-etapa' : '') + '">\
                                    <p><span>'+ (retorno.d[i].Etapas[e].Data.length > 0 ? retorno.d[i].Etapas[e].Data + ' às ' + retorno.d[i].Etapas[e].Hora + ' - ' : '') + '</span> Seu Currículo foi visualizado pelo responsável da vaga</p>\
                                </div>\
                            </div>';
                            break;
                        case 'Enviado':
                            candidatura += '<div class="ja-enviei-timeline-item"> \
                                <div class="item-marking">\
                                    <div>\
                                        <div class="item-check '+ (indiceUltimaEtpa == e ? 'item-check-etapa' : '') +'"></div> \
                                        <div class="item-separator"></div> \
                                    </div>\
                                </div>\
                                   <div class="item-desc '+ (indiceUltimaEtpa <= e ? 'item-etapa' : '') + '">\
                                    <p><span>'+ (retorno.d[i].Etapas[e].Data.length > 0 ? retorno.d[i].Etapas[e].Data + ' às ' + retorno.d[i].Etapas[e].Hora +' - ' : '') + '</span>  Seu Currículo foi entregue para a empresa <div class="{Embasado}" style="margin-left:5px;"> </div></p>\
                                </div>\
                            </div>';
                            break;
                        case 'Analise':
                            candidatura += '<div class="ja-enviei-timeline-item ">\
                                <div class="item-marking">\
                                    <div>\
                                        <div class="item-check '+ (indiceUltimaEtpa == e ? 'item-check-etapa' : '' )+'"></div>\
                                        <div class="item-separator"></div>\
                                    </div>\
                                </div>\
                                  <div class="item-desc '+ (indiceUltimaEtpa <= e ? 'item-etapa' : '') + '">\
                                    <p><span>'+ (retorno.d[i].Etapas[e].Data.length > 0 ? retorno.d[i].Etapas[e].Data + ' às ' + retorno.d[i].Etapas[e].Hora + ' - ' : '') + '</span> Currículo entregue para o selecionador da vaga</p>\
                                </div>\
                            </div>';
                            break;
                        case 'Processamento':
                            candidatura += '<div class="ja-enviei-timeline-item ">\
                                <div class="item-marking">\
                                    <div>\
                                        <div class="item-check '+ (indiceUltimaEtpa == e ? 'item-check-etapa' : '') +'"></div>\
                                        <div class="item-separator"></div>\
                                    </div>\
                                </div>\
                                  <div class="item-desc '+ (indiceUltimaEtpa <= e ? 'item-etapa' : '') + '">\
                                    <p><span>'+ (retorno.d[i].Etapas[e].Data.length > 0 ? retorno.d[i].Etapas[e].Data + ' às ' + retorno.d[i].Etapas[e].Hora + ' - ' : '') + '</span> Currículo encaminhado para o email do selecionador</p>\
                                </div>\
                            </div>';
                            break;
                        case 'Candidatura':
                            candidatura += '<div class="ja-enviei-timeline-item ">\
                                <div class="item-marking">\
                                    <div>\
                                        <div class="item-check '+ (indiceUltimaEtpa == e ? 'item-check-etapa' : '') +'"></div>\
                                        <div class="item-separator"></div>\
                                    </div>\
                                </div>\
                                <div class="item-desc '+ (indiceUltimaEtpa <= e ? 'item-etapa' : '') + '">\
                                    <p><span>'+ retorno.d[i].Etapas[e].Data + ' às ' + retorno.d[i].Etapas[e].Hora + '</span> - Candidatura realizada</p>\
                                </div>\
                            </div>';
                             
                            break;
                        default:
                    }

                    //candidatura += ' </div>\
                    //                </div>\
                    //        </div>';
                    console.log(indiceUltimaEtpa);
                    if (retorno.d[i].NomeEmpresa == '') {
                        candidatura = candidatura.replace('{Embasado}', 'embasado').replace('{NomeEmpresa}', 'Nome da empresa disponível apenas para VIP');
                    }
                    else {
                        candidatura = candidatura.replace('{Embasado}', '').replace('{NomeEmpresa}', retorno.d[i].NomeEmpresa);
                    }
                    //Status
                    switch (retorno.d[i].Etapas[indiceUltimaEtpa].Etapa) {
                        case 'Enviado':
                            candidatura = candidatura.replace('{StatusFrase}', 'Currículo Entregue').replace("{DataHora}", retorno.d[i].Etapas[indiceUltimaEtpa].Data + ' às ' + retorno.d[i].Etapas[indiceUltimaEtpa].Hora)
                                .replace("{Termino}", "is-finished").replace("{Icone}", "fa fa-check fa-is-finished");
                            break;
                        case 'Analise':
                            candidatura = candidatura.replace('{StatusFrase}', 'Currículo Entregue').replace("{DataHora}", retorno.d[i].Etapas[indiceUltimaEtpa].Data + ' às ' + retorno.d[i].Etapas[indiceUltimaEtpa].Hora)
                                .replace("{Termino}", "is-finished").replace("{Icone}", "fa fa-map-marker fa-is-finished");
                            break;
                        case 'Processamento':
                            candidatura = candidatura.replace('{StatusFrase}', 'Candidatura Encaminhada').replace("{DataHora}", retorno.d[i].Etapas[indiceUltimaEtpa].Data + ' às ' + retorno.d[i].Etapas[indiceUltimaEtpa].Hora)
                                .replace("{Termino}", "is-started").replace("{Icone}", "fa fa-hourglass-half fa-is-started");
                        case 'Visualizado':
                            candidatura = candidatura.replace('{StatusFrase}', 'Currículo Visualizado').replace("{DataHora}", retorno.d[i].Etapas[indiceUltimaEtpa].Data + ' às ' + retorno.d[i].Etapas[indiceUltimaEtpa].Hora)
                                .replace("{Termino}", "is-finished").replace("{Icone}", "fa fa-check fa-is-finished");
                        default:
                            candidatura = candidatura.replace('{StatusFrase}', 'Candidatura realizada').replace("{DataHora}", retorno.d[i].DataCandidatura + ' às ' + retorno.d[i].HoraCandidatura)
                                .replace("{Termino}", "is-started").replace("{Icone}", "fa fa-paper-plane ");
                            break;
                    }
                }



                $("#divCandidaturas").append(candidatura);

            }


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

function UltimaEtapa(etapa) {

    for (var i = 0; i < etapa.length; i++) {
        if (!etapa[i].Data == '') {
            return i;
        }
    }

    return -1;

}