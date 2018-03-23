var possuiMediaSalarial = false;

var AutoCompleteExtenders = {
    cidade: '',
    MsgFaixaSalarial: ''
}

function InicializarAutoCompleteMiniCurriculo(parametros) {
    AutoCompleteExtenders.MsgFaixaSalarial = parametros.msgFaixaSalarial;
}

function cvCidadeMini_Validate(sender, args) {
    var res = MiniCurriculo.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function FuncaoPretendida_OnChange(args) {
    var mediaSalarial = MiniCurriculo.PesquisarMediaSalarial(employer.controles.recuperarValor('txtFuncaoPretendida1'), employer.controles.recuperarValor('txtCidadeMini'));
    if (mediaSalarial.value != null && mediaSalarial.value != 0) {
        var valorMinimo = mediaSalarial.value.split(';')[0];
        var valorMaximo = mediaSalarial.value.split(';')[1];

        var mensagem = AutoCompleteExtenders.MsgFaixaSalarial.replace('{0}', valorMinimo).replace('{1}', valorMaximo);
        employer.controles.setAttr('faixa_salarial', 'innerHTML', mensagem);

        possuiMediaSalarial = true;
    }
    else {
        possuiMediaSalarial = false;
    }
}

function FuncaoPretendida_OnBlur() {
    var funcaoPretendida = employer.controles.recuperarValor('txtFuncaoPretendida1');

    if (funcaoPretendida != "") {
        if (possuiMediaSalarial)
            employer.util.findControl('faixa_salarial').css('display', 'block');
        else
            employer.util.findControl('faixa_salarial').css('display', 'none');
    }
    else
        employer.util.findControl('faixa_salarial').css('display', 'none');
}
//destacar estagio e função pretendida quando escolaridade incompleta
function FuncaoEscolaridade_OnChange() {
    var idEscolaridade = employer.controles.recuperarValor('ddlEscolaridade');

    if (idEscolaridade != "") {

        if (idEscolaridade == 6) {//ensino medio incompleto
            employer.util.findControl('divAceitaEstagio').css('display', 'block');
            employer.util.findControl('ckbAceitaEstagio').prop('checked', 'true');
            document.getElementById('cphConteudo_ucMiniCurriculo_txtTituloCurso').value = '';
            document.getElementById('cphConteudo_ucMiniCurriculo_ddlSituacao').value = '0';
            document.getElementById('cphConteudo_ucMiniCurriculo_txtPeriodo_txtValor').value = '';

        } else if (idEscolaridade == 8) {
            employer.util.findControl('divAceitaEstagio').css('display', 'block');
            employer.util.findControl('divLinhaTituloCurso').css('display', 'block');
            employer.util.findControl('ckbAceitaEstagio').prop('checked', 'true');
            document.getElementById('cphConteudo_ucMiniCurriculo_txtTituloCurso').value = '';
            document.getElementById('cphConteudo_ucMiniCurriculo_ddlSituacao').value = '0';
            document.getElementById('cphConteudo_ucMiniCurriculo_txtPeriodo_txtValor').value = '';
        }
        else if (idEscolaridade == 10 || idEscolaridade == 11) {//ensino tecnico ou superior incompleto)
            employer.util.findControl('divAceitaEstagio').css('display', 'block');
            employer.util.findControl('divLinhaTituloCurso').css('display', 'block');
            employer.util.findControl('ckbAceitaEstagio').prop('checked', 'true');
            employer.util.findControl('divLinhaSituacao').css('display', 'block');
        }
        else {
            employer.util.findControl('ckbAceitaEstagio').removeAttr('checked');
            employer.util.findControl('divAceitaEstagio').css('display', 'none');
            employer.util.findControl('divLinhaTituloCurso').css('display', 'none');
            employer.util.findControl('divLinhaSituacao').css('display', 'none');
            document.getElementById('cphConteudo_ucMiniCurriculo_txtTituloCurso').value = '';
            document.getElementById('cphConteudo_ucMiniCurriculo_ddlSituacao').value = '0';
            document.getElementById('cphConteudo_ucMiniCurriculo_txtPeriodo_txtValor').value = '';
        }
    }
}




function ClosePopUpFotos(newwindow) {
    window.focus();
}

function ValidarNome(source, args) {
    var w, z, y, x;
    var isValid = true;
    for (x = 0; x < args.Value.length; x++) {
        z = args.Value.substring(x, x + 1);
        if ((x >= 2 && z == y && z == w)) {
            isValid = false;
        }
        else {
            y = w;
            w = z;
            z = '-';
        }
    }

    if (!args.Value.match("^[A-Z,a-z,ÁáÂâÃãÄäÉéÊêËëÍíÏïÓóÔôÕõÖöÚúÛûÜüÇç']{1,}( ){1,}[A-Z,a-z,ÁáÂâÃãÄäÉéÊêËëÍíÏïÓóÔôÕõÖöÚúÛûÜüÇç']{2,}(( ){1,}[A-Z,a-z,ÁáÂâÃãÄäÉéÊêËëÍíÏïÓóÔôÕõÖöÚúÛûÜüÇç']{1,})*$"))
        isValid = false;

    args.IsValid = isValid;
}

function cvFuncaoPretendida_Validate(sender, args) {
    var res = MiniCurriculo.ValidarFuncao(args.Value);
    args.IsValid = (res.error == null && res.value);
}

$(document).ready(function () {
    window.onunload = OnUnload;
    AjustarPretensaoSalarial();

    autocomplete.cidade("txtCidadeMini");
    autocomplete.funcao("txtFuncaoPretendida1");
    autocomplete.funcao("txtFuncaoPretendida2");
    autocomplete.funcao("txtFuncaoPretendida3");
});

function OnUnload() {
    SalvarDadosContato();
}

function SalvarDadosContato() {
    var cpf = employer.controles.recuperarValorControle('txtCPF');
    var dataNascimento = employer.controles.recuperarValorControle('txtDataDeNascimento');
    var dddCelular = employer.controles.recuperarValor('txtTelefoneCelular_txtDDD');
    var celular = employer.controles.recuperarValor('txtTelefoneCelular_txtFone');
    var email = employer.controles.recuperarValor('txtEmail');
    var nome = employer.controles.recuperarValorControle('txtNome');

    var rblSexo = employer.util.findControl('rblSexo');
    var sexo = '';
    if (typeof (rblSexo.find('input:radio:checked').val()) !== "undefined")
        sexo = rblSexo.find('input:radio:checked').next().text();

    var nomeCidade = employer.controles.recuperarValor('txtCidadeMini');

    var funcao1 = employer.controles.recuperarValor('txtFuncaoPretendida1');
    var ano1 = employer.controles.recuperarValor('txtAnoExperiencia1');
    var mes1 = employer.controles.recuperarValor('txtMesExperiencia1');

    var funcao2 = employer.controles.recuperarValor('txtFuncaoPretendida2');
    var ano2 = employer.controles.recuperarValor('txtAnoExperiencia2');
    var mes2 = employer.controles.recuperarValor('txtMesExperiencia2');

    var funcao3 = employer.controles.recuperarValor('txtFuncaoPretendida3');
    var ano3 = employer.controles.recuperarValor('txtAnoExperiencia3');
    var mes3 = employer.controles.recuperarValor('txtMesExperiencia3');

    var pretensao = employer.controles.recuperarValorControle('txtPretensaoSalarial');

    var dados = "{'cpf':'" + cpf + "','dataNascimento':'" + dataNascimento + "','nome':'" + nome + "','sexo':'" + sexo + "','funcao1':'" + funcao1 + "','ano1':'" + ano1 + "','mes1':'" + mes1 + "','funcao2':'" + funcao2 + "','ano2':'" + ano2 + "','mes2':'" + mes2 + "','funcao3':'" + funcao3 + "','ano3':'" + ano3 + "','mes3':'" + mes3 + "','dddCelular':'" + dddCelular + "' ,'celular':'" + celular + "','email':'" + email + "','nomeCidade':'" + nomeCidade + "','pretensao':'" + pretensao + "' }";

    $.ajax({
        type: "POST",
        url: "/ajax.aspx/CadastroCurriculo_SalvarDadosContato",
        data: dados,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false
    });
}

function AjustarPretensaoSalarial() {

    $('#cphConteudo_ucMiniCurriculo_avisoSalarionivalido').hide();

    $('#cphConteudo_ucMiniCurriculo_txtPretensaoSalarial').on('focus', function () { $('#cphConteudo_ucMiniCurriculo_avisoSalarionivalido').hide(); });

    var options = {
        reverse: true
    }
    $('#cphConteudo_ucMiniCurriculo_txtPretensaoSalarial').mask('0.000.000,00', options);
    $('#cphConteudo_ucMiniCurriculo_txtPretensaoSalarial').on('blur', function () {
        validarSalarioMinimo($('#cphConteudo_ucMiniCurriculo_hdfValorSalarioMinimo').val(), $('#cphConteudo_ucMiniCurriculo_txtPretensaoSalarial').val());
    });

    function validarSalarioMinimo(salarioMinimo, pretensao) {
        var valordoMinimo = salarioMinimo.replace(',', '.');
        var pretensaoSalarial = pretensao.replace('.', '').replace(',', '.');

        if (parseFloat(pretensaoSalarial) >= parseFloat(valordoMinimo)) {
            $('#cphConteudo_ucMiniCurriculo_avisoSalarionivalido').hide();
        } else {
            $('#cphConteudo_ucMiniCurriculo_avisoSalarionivalido').show();
        }
    }
}

function ValEmail(source, arguments) {
    if (arguments.Value.toLowerCase().includes('hotmail')) {
        arguments.isValid = false;
    }
    else {
        $('#dvEmail').css('display', 'none');
        arguments.isValid = true;
    }
}



function blockAt() {
    $("#cphConteudo_ucMiniCurriculo_txtAT").keypress(function (e) {
        if (String.fromCharCode(e.which) == '@') {
            event.preventDefault();
            return false;
        }
    });

    $("#cphConteudo_ucMiniCurriculo_txtEmail").keypress(function (e) {
        if (String.fromCharCode(e.which) == '@') {
            $("#cphConteudo_ucMiniCurriculo_ddlAt").focus();
            $("#cphConteudo_ucMiniCurriculo_ddlAt").simulate('mousedown');
            event.preventDefault();
            return false;
        }
    });
}


$("#cphConteudo_ucMiniCurriculo_txtDataDeNascimento_txtValor").bind("keydown keypress keyup change", function (zEvent) {
    if (zEvent.keyCode == 13) {
        $("#cphConteudo_ucMiniCurriculo_txtTelefoneCelular_txtDDD")[0].focus();
    }

});

function CepRequired() {
    if (document.getElementById("cphConteudo_ucMiniCurriculo_ucEndereco_txtCEP_txtValor").value.length > 1) {
        return true;
    }
    return false;

}