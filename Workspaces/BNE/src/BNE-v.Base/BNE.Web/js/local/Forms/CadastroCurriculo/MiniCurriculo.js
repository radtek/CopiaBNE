var possuiMediaSalarial = false;

var AutoCompleteExtenders = {
    cidade: '',
    MsgFaixaSalarial: ''
}

function InicializarAutoCompleteMiniCurriculo(parametros) {
    AutoCompleteExtenders.MsgFaixaSalarial = parametros.msgFaixaSalarial;
    AutoCompleteExtenders.cidade = parametros.aceCidade;
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

        if ( idEscolaridade == 6) {//ensino medio
            employer.util.findControl('divAceitaEstagio').css('display', 'block');
            employer.util.findControl('ckbAceitaEstagio').prop('checked', 'true');
        } else if (idEscolaridade == 8 || idEscolaridade == 10 || idEscolaridade == 11) {//ensino tecnico ou superior
            employer.util.findControl('divAceitaEstagio').css('display', 'block');
            employer.util.findControl('divLinhaTituloCurso').css('display', 'block');
            
            employer.util.findControl('ckbAceitaEstagio').prop('checked', 'true');
            
        } else {
            employer.util.findControl('divAceitaEstagio').css('display', 'none');
            employer.util.findControl('divLinhaTituloCurso').css('display', 'none');

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