function AjustarCampos() {
    AjustarCidade(employer.controles.recuperarValor('txtCidadePesquisa'));
    autocomplete.cidade('txtCidadePesquisa', cidade_selected);
    $('#escolaridade-estagiario').css('display', $('#cphConteudo_ucEstagiarioFuncao_chbEstagiario').is(':checked') ? 'block' : 'none');
}

function AjustarCidade(valor) {
    employer.controles.setAttr('txtBairro', 'disabled', valor === '');
    employer.controles.setAttr('txtLogradouro', 'disabled', valor === '');
    employer.util.findControl('lblAvisoCidadeEndereco').css("display", valor !== '' ? 'none' : '');

    var nomeCidade = valor.split('/')[0];
    var siglaEstado = valor.split('/')[1];
    autocomplete.bairro('txtBairro', nomeCidade, siglaEstado);
}

function cvCidade_Validate(sender, args) {
    var res = SalaSelecionadorRastreadorCurriculos.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
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

function RastreadorCurriculoAvancadaCidadeOnBlur (sender) {
    var res = SalaSelecionadorRastreadorCurriculos.RecuperarCidade(sender.value);

    if (res.error == null && res.value)
        employer.controles.setValor(sender.id, res.value);
}

$(document).ready(function () {
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
        $('#cphConteudo_bsmChkDisponibilidade').css('display', '');

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

function cidade_selected() {
    __doPostBack('ctl00$cphConteudo$txtCidadePesquisa', '');
    AjustarCampos();
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

    $(cursoGraduacaoID).keyup(function () {
        $(cursoGraduacaoEstagiarioID).val($(this).val());
    });

    $(cursoGraduacaoEstagiarioID).keyup(function () {
        $(cursoGraduacaoID).val($(this).val());
    });

    $(instituicaoGraduacaoID).keyup(function () {
        $(instituicaoGraduacaoEstagiarioID).val($(this).val());
    });

    $(instituicaoGraduacaoEstagiarioID).keyup(function () {
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

    AjustarCampos();
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