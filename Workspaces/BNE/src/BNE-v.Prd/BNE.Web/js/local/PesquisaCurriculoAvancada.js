var Parametros = {
    IdadeMin: 0,
    IdadeMax: 0
}

function InicializarParametro(parametros) {
    Parametros.IdadeMin = parametros.IdadeMin;
    Parametros.IdadeMax = parametros.IdadeMax;
}

function cvIdade_ClientValidationFunction(sender, args) {
    //Valor da mensagem de erro _304501
    var erro = '304501 - O valor {0} deve ser igual ou {1} do que o valor {2}.';
    var txtIdadeDe = (+employer.controles.recuperarValorControle('txtIdadeDe'));
    var txtIdadeAte = (+employer.controles.recuperarValorControle('txtIdadeAte'));
    args.IsValid = true;
    if (txtIdadeDe != '' && txtIdadeAte != '') {
        if (txtIdadeAte < txtIdadeDe) {
            erro = erro.replace('{0}', 'Máximo').replace('{1}', 'maior').replace('{2}', 'Mínimo');
            args.IsValid = false;
            employer.controles.setAttr(sender.id, 'innerText', erro);
        }
    }
}

function txtSalarioDe_ValorAlterado(args) {
    var valor = employer.controles.recuperarValor(args.NomeCampoValor);
    if (valor == '')
        return;
    employer.controles.setAttr('txtSalarioAte', 'ValorMinimo', valor);
}

function AjustarCampos() {
    AjustarCidade(employer.controles.recuperarValor('txtCidadePesquisa'));
    autocomplete.cidade('txtCidadePesquisa', cidade_selected);

    $('#escolaridade-estagiario').css('display', $('#cphConteudo_ucEstagiarioFuncao_chbEstagiario').is(':checked') ? 'block' : 'none');
}

function AjustarCidade(valor) {
    employer.controles.setAttr('txtBairro', 'disabled', valor == '');
    employer.controles.setAttr('txtLogradouro', 'disabled', valor == '');
    employer.util.findControl('lblAvisoCidadeEndereco').css("display", valor != '' ? 'none' : '');

    var nomeCidade = valor.split('/')[0];
    var siglaEstado = valor.split('/')[1];
    autocomplete.bairro('txtBairro', nomeCidade, siglaEstado);
}

function cvCidade_Validate(sender, args) {
    var res = PesquisaCurriculoAvancada.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function cvFuncao_Validate(sender, args) {
    var res = PesquisaCurriculoAvancada.ValidarFuncao(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function cvFuncaoFaixaCepAte_Validate(sender, args) {
    //    var erro = '304501 - O valor {0} deve ser igual ou {1} do que o valor {2}.';
    var txtFaixaCepDe = (+(employer.controles.recuperarValorControle('txtFaixaCep').replace('-', '')));
    var txtFaixaCepAte = (+(employer.controles.recuperarValorControle('txtFaixaCepAte').replace('-', '')));
    args.IsValid = true;
    if (typeof (txtFaixaCepDe) != 'undefined' && txtFaixaCepDe != NaN && typeof (txtFaixaCepAte) != 'undefined' && txtFaixaCepAte != NaN) {
        if (txtFaixaCepAte < txtFaixaCepDe) {
            //erro = erro.replace('{0}', 'Máximo').replace('{1}', 'maior').replace('{2}', 'Mínimo');
            args.IsValid = false;
            //employer.controles.setAttr(sender.id, 'innerText', erro);
        }
    }
}

//Completa uma string com zeros à esquerda de acordo com o comprimento informado
function StringPad(number, length) {
    var str = '' + number;

    while (str.length < length) {
        str = '0' + str;
    }

    return str;
}

function txtFaixaCep_ValorAlteradoClient(sender) {
    var txtCepDe = employer.controles.recuperarValorControle('txtFaixaCep').replace('-', '');
    var txtCepAte = employer.controles.recuperarValorControle('txtFaixaCepAte').replace('-', '');

    if (txtCepDe.length > 0) {
        if (txtCepAte.length == 0) {
            employer.controles.setValorControle('txtFaixaCepAte', StringPad(Number(txtCepDe) + 500, 8));
        }
    }


}

function txtFaixaCepAte_ValorAlteradoClient(sender) {
    var txtCepDe = employer.controles.recuperarValorControle('txtFaixaCep').replace('-', '');
    var txtCepAte = employer.controles.recuperarValorControle('txtFaixaCepAte').replace('-', '');

    if (txtCepAte.length > 0) {
        if (txtCepDe.length == 0) {
            employer.controles.setValorControle('txtFaixaCep', StringPad(Number(txtCepAte) - 500, 8));
        }
    }
}

function onCheckBoxIdiomaClick(chk) {
    var combo = $find(employer.util.findControl('rcbIdioma')[0].id);
    //holds the text of all checked items
    var text = '';
    //holds the values of all checked items
    var values = "";
    //get the collection of all items
    var items = combo.get_items();
    //enumerate all items
    for (var i = 0; i < items.get_count() ; i++) {
        var item = items.getItem(i);
        //get the checkbox element of the current item
        var chk = $get(combo.get_id() + "_i" + i + "_chkIdioma");
        if (chk.checked) {
            text += item.get_text() + ",";
            values += item.get_value() + ",";
        }
    }
    //remove the last comma from the string
    text = removerUltimaVirgula(text);
    values = removerUltimaVirgula(values);

    if (text.length > 0) {
        //set the text of the combobox
        combo.set_text(text);
    }
    else {
        //all checkboxes are unchecked 
        //so reset the controls
        combo.set_text("");
    }
}

function onCheckBoxDisponibilidadeClick(chk) {
    var combo = $find(employer.util.findControl('rcbDisponibilidade')[0].id);
    //holds the text of all checked items
    var text = '';
    //holds the values of all checked items
    var values = "";
    //get the collection of all items
    var items = combo.get_items();
    //enumerate all items
    for (var i = 0; i < items.get_count() ; i++) {
        var item = items.getItem(i);
        //get the checkbox element of the current item
        var chk = $get(combo.get_id() + "_i" + i + "_chkDisponibilidade");
        if (chk.checked) {
            text += item.get_text() + ",";
            values += item.get_value() + ",";
        }
    }
    //remove the last comma from the string
    text = removerUltimaVirgula(text);
    values = removerUltimaVirgula(values);

    if (text.length > 0) {
        //set the text of the combobox
        combo.set_text(text);
    }
    else {
        //all checkboxes are unchecked 
        //so reset the controls
        combo.set_text("");
    }
}

function removerUltimaVirgula(str) {
    return str.replace(/,$/, "");
}

function InstituicaoGraduacao(sender, args) {
    var hfInstituicaoGraduacao = employer.util.findControl('hfInstituicaoGraduacao');
    hfInstituicaoGraduacao[0].value = args.get_value();

    employer.form.util.autoCompleteClientSelected(sender, args);
}

function InstituicaoOutrosCursos(sender, args) {
    var hfInstituicaoOutrosCursos = employer.util.findControl('hfInstituicaoOutrosCursos');
    hfInstituicaoOutrosCursos[0].value = args.get_value();

    employer.form.util.autoCompleteClientSelected(sender, args);
}

function PesquisaCurriculoAvancadaCidadeOnBlur(sender) {
    var res = PesquisaCurriculoAvancada.RecuperarCidade(sender.value);

    if (res.error == null && res.value)
        employer.controles.setValor(sender.id, res.value);
}

$(document).ready(function () {
    //Ajustando o scroll
    $(window).scroll(function () {
        //$("#divSlowDown").stop().animate({ "top": $(window).scrollTop() + ($(screen)[0].height / 4) + "px" }, { duration: 700, queue: false });
        var vTop = ($(window).height() / 2) - $("#divSlowDown").height() / 2 + $(top.window).scrollTop();

        //Ajustando a altura para não estourar o layout
        if (vTop <= 323.5)
            vTop = 262.5;
        else if (vTop >= 1368)
            vTop = 1450;

        $("#divSlowDown").stop().animate({ "top": vTop + "px" }, { duration: 700, queue: false });
    });

    //Ajustando o background
    $('body').addClass('bg_fundo_empresa');
    localPageLoad();
    AjustarCampos();
    ajustarCollapse();
});

function ajustarCollapse() {
    $('#collapsePretenCand').on('hide.bs.collapse', function () {
        $('#cphConteudo_bsmChkDisponibilidade').hide();

    });

    $('#collapsePretenCand').on('show.bs.collapse', function () {
        $('#cphConteudo_bsmChkDisponibilidade').show();
        $('#cphConteudo_bsmChkDisponibilidade').css('display','');

    });

    $('#collapseOne').on('hide.bs.collapse', function () {
        $('#cphConteudo_bsmPalavraChave,#cphConteudo_BalaoSaibaMaiPalavraChave,#cphConteudo_bsmExcluirPalavraChave').hide();

    });

    $('#collapseOne').on('show.bs.collapse', function () {
        $('#cphConteudo_bsmPalavraChave,#cphConteudo_BalaoSaibaMaiPalavraChave,#cphConteudo_bsmExcluirPalavraChave').show();
        $('#cphConteudo_bsmPalavraChave,#cphConteudo_BalaoSaibaMaiPalavraChave,#cphConteudo_bsmExcluirPalavraChave').css('display', '');

    });

    $('#collapseFormEsc').on('hide.bs.collapse', function () {
        $('#cphConteudo_ucPesquisaIdioma_bsmChkIdioma,#cphConteudo_bsmTxtInstituicaoTecnicoGraduacao,#cphConteudo_bsmTxtInstituicaoOutrosCursos').hide();

    });

    $('#collapseFormEsc').on('show.bs.collapse', function () {
        $('#cphConteudo_ucPesquisaIdioma_bsmChkIdioma,#cphConteudo_bsmTxtInstituicaoTecnicoGraduacao,#cphConteudo_bsmTxtInstituicaoOutrosCursos').show();
        $('#cphConteudo_ucPesquisaIdioma_bsmChkIdioma,#cphConteudo_bsmTxtInstituicaoTecnicoGraduacao,#cphConteudo_bsmTxtInstituicaoOutrosCursos').css('display', '');

    });

    $('#collapseCandidato').on('hide.bs.collapse', function () {
        $('#cphConteudo_bsmNomeCpfCodigo').hide();

    });

    $('#collapseCandidato').on('show.bs.collapse', function () {
        $('#cphConteudo_bsmNomeCpfCodigo').show();
        $('#cphConteudo_bsmNomeCpfCodigo').css('display', '');

    });

    $('#pnl_avaliacao_candidato_inner').on('hide.bs.collapse', function () {
        $('#cphConteudo_BalaoSaibaMaisAvaliacao').hide();

    });

    $('#pnl_avaliacao_candidato_inner').on('show.bs.collapse', function () {
        $('#cphConteudo_BalaoSaibaMaisAvaliacao').show();
        $('#cphConteudo_BalaoSaibaMaisAvaliacao').css('display', '');

    });
}

function ClientDropDownClosing(sender, args) {
    var customText = [];
    for (var i = 0; i < sender.get_items().get_count() ; i++) {
        var comboBoxItem = sender.get_items().getItem(i);
        if ($(comboBoxItem._element).find('input:checkbox').is(':checked')) {
            customText.push(comboBoxItem.get_text());
        }
    }
    sender.set_text(customText.join(', '));
}

function cidade_selected() {
    // __doPostBack('ctl00$cphConteudo$txtCidadePesquisa','');
    txtCidadeTextChanged();
}

function txtCidade_onChange(sender) {
    cidade_selected();
}

var cursoGraduacaoID = "#cphConteudo_txtTituloTecnicoGraduacao";
var cursoGraduacaoEstagiarioID = "#cphConteudo_txtTituloTecnicoGraduacaoEstag";
var instituicaoGraduacaoID = "#cphConteudo_txtInstituicaoTecnicoGraduacao";
var instituicaoGraduacaoEstagiarioID = "#cphConteudo_txtInstituicaoTecnicoGraduacaoEstag";

function localPageLoad() {
    ToggleCusoEstagiario();

    $(cursoGraduacaoID).blur(function () {
        $(cursoGraduacaoEstagiarioID).val($(this).val());
    });

    $(cursoGraduacaoEstagiarioID).blur(function () {
        $(cursoGraduacaoID).val($(this).val());
    });

    $(instituicaoGraduacaoID).blur(function () {
        $(instituicaoGraduacaoEstagiarioID).val($(this).val());
    });

    $(instituicaoGraduacaoEstagiarioID).blur(function () {
        $(instituicaoGraduacaoID).val($(this).val());
    });

    $("#cphConteudo_ucEstagiarioFuncao_chbEstagiario").on('change', function (e) {
        ToggleCusoEstagiario();
    });

    var dropDownEscolaridade = $("#cphConteudo_rcbNivelEscolaridade");
    var dropDownEscolaridadeEstagiario = $("#cphConteudo_rcbNivelEscolaridadeEstagiario");
    dropDownEscolaridadeEstagiario.on('change', function (e) {
        if (dropDownEscolaridade.val() !== dropDownEscolaridadeEstagiario.val()) {
            $find("cphConteudo_rcbNivelEscolaridade").findItemByText($(this).val()).select();
        }
    });

    dropDownEscolaridade.on('change', function (e) {
        if (dropDownEscolaridade.val() !== dropDownEscolaridadeEstagiario.val()) {
            $find("cphConteudo_rcbNivelEscolaridadeEstagiario").findItemByText($(this).val()).select();
        }
    });
}

function ToggleCusoEstagiario() {
    $('#escolaridade-estagiario').css('display', $('#cphConteudo_ucEstagiarioFuncao_chbEstagiario').is(':checked') ? 'block' : 'none');

    if (!$('#cphConteudo_ucEstagiarioFuncao_chbEstagiario').is(':checked')) {
        var rcbNivelEstag = $find("cphConteudo_rcbNivelEscolaridadeEstagiario");
        if (rcbNivelEstag != null) {
            rcbNivelEstag.findItemByValue("0").select();
        }
        var rcbNivel = $find("cphConteudo_rcbNivelEscolaridade");
        if (rcbNivel != null) {
            rcbNivel.findItemByValue("0").select();
        }

        $(cursoGraduacaoEstagiarioID).val('');
        $(cursoGraduacaoID).val('');
        $(instituicaoGraduacaoEstagiarioID).val('');
        $(instituicaoGraduacaoID).val('');
    }
}

function GetItems(sender, args) {
    itemsRequesting(sender, args);
}

function GetBairros(cidade) {
    if (cidade !== '' && cidade.indexOf('/') > -1) {

        var nomeCidade = cidade.split('/')[0];
        var siglaEstado = cidade.split('/')[1];

        autocomplete.bairro('cphConteudo_txtBairro_txtValor', nomeCidade, siglaEstado);

        
        url = url.replace('{cidade}', nomeCidade);
        url = url.replace('{estado}', siglaEstado);

        $.ajax({
            url: url,
            success: function (response) {
                LoadCombos(response);
            }
        });
    }
}

// This cancels the default RadComboBox behavior 
function itemsRequesting(sender, args) {
    if (args.set_cancel != null) {
        args.set_cancel(true);
    }
    if (sender.get_emptyMessage() === sender.get_text())
        sender.set_text("");
}

function distinct(obj, propertyName) {
    var result = [];
    $.each(obj, function (i, v) {
        var prop = eval("v." + propertyName);
        if ($.inArray(prop, result) == -1) result.push(prop);
    });
    return result;
}

// Use this for all of your RadComboBox to populate from JQuery 
function LoadCombos(result) {

    var items = result.d || result;

    if (items != null && items['Centro'] != null) {
        var centro = 'Centro';
        var norte = 'Zona Norte';
        var sul = 'Zona Sul';
        var leste = 'Zona Leste';
        var oeste = 'Zona Oeste';

        var itemsZonaCentral = items[centro];
        var itemsZonaNorte = items[norte];
        var itemsZonaSul = items[sul];
        var itemsZonaLeste = items[leste];
        var itemsZonaOeste = items[oeste];

        var rcbBairroZonaCentral = $find('cphConteudo_rcbBairroZonaCentral');
        var rcbBairroZonaNorte = $find('cphConteudo_rcbBairroZonaNorte');
        var rcbBairroZonaSul = $find('cphConteudo_rcbBairroZonaSul');
        var rcbBairroZonaLeste = $find('cphConteudo_rcbBairroZonaLeste');
        var rcbBairroZonaOeste = $find('cphConteudo_rcbBairroZonaOeste');

        var rcbBairroZonaCentral1 = $find('cphConteudo_rcbBairroZonaCentral1');
        
        LoadComboEmployer(rcbBairroZonaCentral1, itemsZonaCentral, centro);

        LoadCombo(rcbBairroZonaCentral, itemsZonaCentral, centro);
        LoadCombo(rcbBairroZonaNorte, itemsZonaNorte, norte);
        LoadCombo(rcbBairroZonaSul, itemsZonaSul, sul);
        LoadCombo(rcbBairroZonaLeste, itemsZonaLeste, leste);
        LoadCombo(rcbBairroZonaOeste, itemsZonaOeste, oeste);

        $('#bairro-zona').css('display', 'block');
        $('#bairro-texto').css('display', 'none');
        $('#bairro-endereco').css('position', 'absolute');
        $('#bairro-endereco').css('margin-top', '65px');
    } else {
        $('#bairro-zona').css('display', 'none');
        $('#bairro-texto').css('display', 'block');
        $('#bairro-endereco').css('position', 'relative');
        $('#bairro-endereco').css('margin-top', '0');
    }
}

function LoadCombo(combo, items, defaultText) {
    combo.clearItems();

    var comboItem = new Telerik.Web.UI.RadComboBoxItem();

    comboItem.set_text(defaultText);
    comboItem.set_value("-1");

    combo.get_items().add(comboItem);
    comboItem.select();

    for (var i = 0; i < items.length; i++) {
        var item = items[i];
        comboItem = new Telerik.Web.UI.RadComboBoxItem();

        comboItem.set_text(item.Nome);
        comboItem.set_value(item.ID);

        combo.get_items().add(comboItem);
    }
}

function AbrirModalAnuncioVaga() {
    $('#modal-anuncio-vaga').modal('show');
}

function txtCidadeTextChanged() {
    $("#upgGlobalCarregandoInformacoes").show();
    var dados = "{'Cidade':'" + $("#txtCidadePesquisa").val() + "'}";
    $.ajax({
        type: "Post",
        url: "/ajax.aspx/CidadeEstadoZona",
        data: dados,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (retorno) {
            var obj = JSON.parse(retorno.d);
            //if (retorno.d) {
            if (obj.siglaEstado != null) {
                $("#rcbEstado").val(obj.SiglaEstado);
            }
            if (obj.Centro != null && obj.Centro.length > 0) {
                $('#btnZonas').css('display', 'block');
            }
            else {
                $('#btnZonas').css('display', 'none');
                $('#pnlBairroZona').css('display', 'none');
                document.getElementById("cphConteudo_pnlBairroTexto").classList.remove('Invisible');
                
            }
            $("#upgGlobalCarregandoInformacoes").hide();
        }
    });
    $("#upgGlobalCarregandoInformacoes").hide();
}

