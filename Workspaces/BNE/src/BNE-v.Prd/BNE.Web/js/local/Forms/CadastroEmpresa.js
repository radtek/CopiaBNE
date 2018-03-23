$(document).ready(
    function () {
        //Ajustando o background
        $("body").addClass("bg_fundo_empresa");
    }
);

var Parametro = {
    StatusCNAE: 0
};

var Flag = {
    Nao: 0,
    Sim: 1
};

function InicializarStatus(parametros) {
    if (parametros.StatusCNAE != '')
        Parametro.StatusCNAE = parametros.StatusCNAE;
}

function cvCidade_Validate(sender, args) {
    var res = CadastroEmpresaDados.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function cvCNAE_ServerValidate(sender, args) {
    var res = CadastroEmpresaDados.ValidarCNAE(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function txtCNAE_OnFocus(args) {
    employer.util.findControl('lblInfoCNAE').text('');
}

function txtCNAE_OnBlur(args) {
    var campo = $get(args.NomeCampoValor);

    var res = CadastroEmpresaDados.RecuperarCNAE(campo.value);

    if (res.error == null && res.value != null) {
        Parametro.StatusCNAE = "1";
        employer.util.findControl('lblInfoCNAE').text(res.value);
    }
    else {
        Parametro.StatusCNAE = "0";
        employer.util.findControl('lblInfoCNAE').text('');
    }
    
    if (typeof(AjustarCampos) == "function")
    {
        AjustarCampos();
    }
}

function cvNaturezaJuridica_ServerValidate(sender, args) {
    var res = CadastroEmpresaDados.ValidarNaturezaJuridica(args.Value);
    args.IsValid = (res.error == null && res.value);
}
function txtNaturezaJuridica_OnFocus(args) {
    employer.util.findControl('lblInfoNaturezaJuridica').text('');
}
function txtNaturezaJuridica_OnBlur(args) {
    var campo = $get(args.NomeCampoValor);
    var res = CadastroEmpresaDados.RecuperarNaturezaJuridica(campo.value);

    if (res.error == null && res.value != null) {
        employer.util.findControl('lblInfoNaturezaJuridica').text(res.value);
    }
    else {
        employer.util.findControl('lblInfoNaturezaJuridica').text('');
    }
}

function ImprimiCadastroEmpresa(divId) {

    var wrapperDiv = employer.util.findControl(divId)[0];
    var printIframe = document.createElement("IFRAME");
    document.body.appendChild(printIframe);
    var printDocument = printIframe.contentWindow.document;
    printDocument.designMode = "on";
    printDocument.open();
    printDocument.write("<html><head></head><body>" + wrapperDiv.innerHTML + "</body></html>");
    printIframe.style.position = "absolute";
    printIframe.style.top = "-1000px";
    printIframe.style.left = "-1000px";

    if (document.all) {
        printDocument.execCommand("Print", null, false);
    }
    else {
        printDocument.contentWindow.print();
    }
}

function NumeroBlur(sender, args) {
    var numero = employer.controles.recuperarValorControle('txtNumero');

    if (numero == "") {
        employer.controles.setValorControle('txtNumero', 'Sem Número');
    }
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

    if (!args.Value.match("^[A-Z,a-z,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç']{1,}( ){1,}[A-Z,a-z,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç']{2,}(( ){1,}[A-Z,a-z,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç']{1,})*$"))
        isValid = false;

    args.IsValid = isValid;
}

function CentralizarObjeto(IdObjeto) {
    $(IdObjeto).css("left", ($(window).width() / 2 - $(IdObjeto).width() / 2));
    $(IdObjeto).css("top", ($(window).height() / 2 - $(IdObjeto).height() / 2));
}

$(document).ready(function () {
    autocomplete.funcao("txtFuncaoExercida");
    //    window.onunload = OnUnload;
});

/*

function OnUnload() {
    SalvarDadosContato();
}

function SalvarDadosContato() {
    var cnpj = employer.controles.recuperarValorControle('txtCNPJ_fjkfjfkj');
    var cpf = employer.controles.recuperarValorControle('txtCPF');
    var dataNascimento = employer.controles.recuperarValorControle('txtDataNascimento');
    var nome = employer.controles.recuperarValorControle('txtNome');

    var rblSexo = employer.util.findControl('rblSexo');

    var sexo = '';
    if (typeof (rblSexo.find('input:radio:checked').val()) !== "undefined")
        sexo = rblSexo.find('input:radio:checked').next().text();

    var funcao = employer.controles.recuperarValor('txtFuncaoExercida');
    var dddCelular = employer.controles.recuperarValor('txtTelefoneCelularMaster_txtDDD');
    var celular = employer.controles.recuperarValor('txtTelefoneCelularMaster_txtFone');
    var dddTelefone = employer.controles.recuperarValor('txtTelefoneMaster_txtDDD');
    var telefone = employer.controles.recuperarValor('txtTelefoneMaster_txtFone');
    var emailComercial = employer.controles.recuperarValor('txtEmail');
    var site = employer.controles.recuperarValor('txtSite');
    var numeroFuncionario = employer.controles.recuperarValorControle('txtNumeroFuncionarios');
    var dataFundacao = employer.controles.recuperarValorControle('txtDataFundacao');

    var periodoInicial = ''; //employer.controles.recuperarValor('ddlInicioPeriodo');
    var periodoFinal = ''; //employer.controles.recuperarValor('ddlFimPeriodo');

    var dddComercial = employer.controles.recuperarValor('txtTelefoneComercialEmpresa_txtDDD');
    var telefoneComercial = employer.controles.recuperarValor('txtTelefoneComercialEmpresa_txtFone');
    var dddFax = ''; //employer.controles.recuperarValor('txtFaxEmpresa_txtDDD');
    var numeroFax = ''; //employer.controles.recuperarValor('txtFaxEmpresa_txtFone');

    var ckbCurso = employer.util.findControl('ckbCursos');
    var ofereceCurso = ckbCurso.checked ? 'Sim' : 'Não';

    var razaoSocial = employer.controles.recuperarValorControle('txtRazaoSocial');
    var nomeFantasia = employer.controles.recuperarValorControle('txtNomeFantasia');
    var cnae = employer.controles.recuperarValorControle('txtCNAE');
    var nj = employer.controles.recuperarValorControle('txtNaturezaJuridica');
    var cep = employer.controles.recuperarValorControle('txtCEP');
    var endereco = employer.controles.recuperarValor('txtLogradouro');
    var numero = employer.controles.recuperarValor('txtNumero');
    var complemento = employer.controles.recuperarValor('txtComplemento');
    var bairro = employer.controles.recuperarValor('txtBairro');
    var nomeCidade = employer.controles.recuperarValor('txtCidade');

    var dados = "{'cnpj':'" + cnpj + "','cpf':'" + cpf + "','dataNascimento':'" + dataNascimento + "','nome':'" + nome + "','sexo':'" + sexo + "','funcao':'" + funcao + "','dddCelular':'" + dddCelular + "' ,'celular':'" + celular + "' ,'dddTelefone':'" + dddTelefone + "' ,'telefone':'" + telefone + "' ,'emailComercial':'" + emailComercial + "' ,'site':'" + site + "' ,'numeroFuncionario':'" + numeroFuncionario + "' ,'dataFundacao':'" + dataFundacao + "' ,'periodoInicial':'" + periodoInicial + "' ,'periodoFinal':'" + periodoFinal + "' ,'dddComercial':'" + dddComercial + "' ,'telefoneComercial':'" + telefoneComercial + "' ,'dddFax':'" + dddFax + "' ,'numeroFax':'" + numeroFax + "' ,'ofereceCurso':'" + ofereceCurso + "' ,'razaoSocial':'" + razaoSocial + "' ,'nomeFantasia':'" + nomeFantasia + "' ,'cnae':'" + cnae + "' ,'nj':'" + nj + "' ,'cep':'" + cep + "' ,'endereco':'" + endereco + "' ,'numero':'" + numero + "' ,'complemento':'" + complemento + "' ,'bairro':'" + bairro + "' ,'nomeCidade':'" + nomeCidade + "' }";

    $.ajax({
        type: "POST",
        url: "/ajax.aspx/CadastroEmpresa_SalvarDadosContato",
        data: dados,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false
    });
}*/