var itemCount = 0;
var exitAttempt = 0;
var cnpj = "";
var qtdeCandidaturas = 1;
var spanSelecionado;

$(function () {

    (function inicializa() {

        var cpf = $("#txtCpf");
        cpf.inputmask({ mask: "999.999.999-99" });

        var datas = $("#txtDataInicioValidade,#txtDataFimValidade");
        datas.datepicker({
            changeMonth: true,
            changeYear: true
        });
        datas.datepicker($.datepicker.regional["pt-BR"]);

        $("#divSalvoComSucesso").dialog({
            autoOpen: false,
            modal: true,
            buttons: {
                "OK": function () {
                    $(this).dialog("close");
                }
            }
        });

        segundaTela();
        Vagas();

        $('#fileLogo').fileupload({
            url: 'upload',
            dataType: 'json',
            autoUpload: true,
            acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i,
            maxFileSize: 5000000,
            disableImageResize: /Android(?!.*Chrome)|Opera/
                .test(window.navigator.userAgent),
            previewMaxWidth: 100,
            previewMaxHeight: 100,
            previewCrop: true,
            done: function (e, data) {
                $('#logo').attr('src', data.result.url);
            }
        });

    })();


});

function modalSalvoComSucesso() {
    $("#divSalvoComSucesso").dialog("open");
}

function zerarExitAttempt() {
    exitAttempt = 0;
}

function primeiraTela() {
    modal.abrirModal('div1');
}

function segundaTela() {
    modal.abrirModal('div2');
}

function terceiraTela() {
    modal.abrirModal('div3');
}

function quartaTela() {
    modal.abrirModal('div4');
}

function Vagas() {
    $('#grid_list').jqGrid(
        {
            url: 'c',
            editurl: 'c',
            datatype: "json",
            contentType: "application/json; charset-utf-8",
            colNames: ['Nome', 'CNPJCompanhia', 'Logo', 'Acoes', 'id'],
            colModel: [
                { name: 'Nome', index: 'Nome', width: 120 },
                { name: 'CNPJCompanhia', index: 'CNPJCompanhia', width: 80 },
                { name: 'Logo', index: 'Logo', formatter: formatImage },
                { name: 'act', index: 'act', width: 75, sortable: false },
                { name: 'id', index: 'id', hidden: true }
            ],
            rowNum: 5,
            rowList: [10, 20, 30],
            pager: '#grid_pager',
            sortname: 'Nome',
            viewrecords: true,
            sortorder: "desc",
            jsonReader: { repeatitems: false },
            gridComplete: function () {
                var ids = $("#grid_list").jqGrid('getDataIDs');
                for (var i = 0; i < ids.length; i++) {
                    var cl = ids[i];
                    be = "<input style='height:22px' type='button' value='Editar' onclick=\"EditarCompanhia('" + cl + "');\"  />";
                    $("#grid_list").jqGrid('setRowData', ids[i], { act: be /* + se + ce*/ });
                }
            },
            /*caption: "JSON Example"*/
        }
    );
    $("#grid_list").jqGrid('navGrid', "#grid_pager", { edit: false, add: false, del: false });
}
function formatImage(cellValue, options, rowObject) {
    var imageHtml = "<img src='" + cellValue + "' style='max-width:50px; max-height:50px;' />";
    return imageHtml;
}

function Salvar() {
    var r = false;

    $.ajax({
        type: "post",
        url: 's',
        data: {
            //cb: $("#txtCnpjFilial").val(),
            c: $("#txtCnpjCompanhia").val(),
            n: $("#txtNomeCompanhia").val(),
            //di: $("#txtDataInicioValidade").val(),
            //df: $("#txtDataFimValidade").val(),
            l: $("#logo").attr('src'),
            i: $("#hdId").val()
        },
        dataType: "json",
        success: function (d, s, x) {
            if (d) {
                $("#hdId").val(0);
                $("#txtCnpjCompanhia").val('');
                $("#txtNomeCompanhia").val('');
                $('#grid_list').trigger('reloadGrid');
            }
        },
        error: function (x, s, e) { mostrarErro('Ocorreu um erro em seu acesso.'); },
        async: false
    });

    return r;
}

function EditarCompanhia(id) {
    $.ajax({
        type: "get",
        url: 'e',
        data: {
            //cb: $("#txtCnpjFilial").val(),
            //c: $("#txtCnpjCompanhia").val(),
            //n: $("#txtNomeCompanhia").val(),
            //di: $("#txtDataInicioValidade").val(),
            //df: $("#txtDataFimValidade").val(),
            id: id
        },
        dataType: "json",
        success: function (d, s, x) {
            //$("#fileLogo").val(),
            $("#hdId").val(id);
            $("#txtCnpjCompanhia").val(d.cnpj);
            $("#txtNomeCompanhia").val(d.nome);
        },
        error: function (x, s, e) { mostrarErro('Ocorreu um erro em seu acesso.'); },
        async: false
    });
}