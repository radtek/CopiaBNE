function AplicarMascaraCPF(controle) {
    var valor = Trim(controle.value);
    if (valor == '')
        return;

    if (valor.match(/^(\d{11})$/)) {
    	valor = valor.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
    }
    controle.value = valor;
   }

function TrimCPF(str) { return str.replace(/^\s+|\s+$/g, ""); }

function LimparMascaraCPF(cnpj) {
    cnpj = TrimCPF(cnpj);

    var rx = new RegExp("\\d{3}.\\d{3}.\\d{3}-\\d{2}");
    var m = rx.exec(cnpj);
    if (m != null) {
        var rxP = new RegExp("[.\\/-]");
        return ReplacaAllCNPJ("[.\\/-]", cnpj, '');
    }
    return cnpj;
}
function ReplacaAllCNPJ(sRx, cnpj, sReplace) {
    var rxP = new RegExp(sRx);
    var m = rxP.exec(cnpj);
    if (m != null)
        return ReplacaAllCNPJ(sRx, cnpj.replace(rxP, sReplace), sReplace);
    return cnpj;
}

function ValidarFormatoCPF(cpf) {
    try {
        if (cpf.length == 11 && ValidarValorLongClient(cpf)) {
            var vlr = parseFloat(cpf);
            for (var i = 0; i < cpf.length; i++) {
                if (cpf.charAt(0) != cpf.charAt(i))
                    return true;
            }
        }
    } catch (e) {
    }

    return false;
}

function ValidarValorLongClient(valor) {
    try {
        var valorD = parseFloat(valor);
        var sValorD = valorD.toString().replace(",", ".");

        var v = (!((valorD != 0 && !valorD) || RemoveZerosNaoUsadoCPF(valor) != sValorD));

        return (!((valorD != 0 && !valorD) || RemoveZerosNaoUsadoCPF(valor) != sValorD));
    }
    catch (e) {
        return false;
    }
}
function RemoveZerosNaoUsadoCPF(sValor) {
    var v = new String();
    var rx = RegExp(/^0+(.+)/);
    var m = rx.exec(sValor);
    if (m != null)
        sValor = m[1];

    var virgula = sValor.indexOf(".");
    if (virgula > -1) {
        return parseFloat(
        sValor.substring(0, virgula) + "." + sValor.substring(virgula + 1, sValor.length)).toString();
    }
    return sValor;
}

function ValidarCalculoCPF(cpf) {
    try {
        var soma1 = 0;
        var soma2 = 0;
        for (var i = 0; i < 9; i++) {
            var dig_cpf = cpf.substring(i, i + 1);
            soma1 += parseInt(dig_cpf) * (10 - i);
            soma2 += parseInt(dig_cpf) * (11 - i);
        }
        var dv1 = (11 - (soma1 % 11));
        dv1 = dv1 >= 10 ? 0 : dv1;
        soma2 = soma2 + (dv1 * 2);
        var dv2 = (11 - (soma2 % 11));
        dv2 = dv2 >= 10 ? 0 : dv2;
        var dv = cpf.substring(9, 11);
        return dv == (dv1.toString() + dv2.toString());
    }
    catch (e) {
        alert(e);
    }

    return false;
}

function ValidarCPFClient(cpf) {
    cpf = LimparMascaraCPF(cpf);

    return ValidarFormatoCPF(cpf) && ValidarCalculoCPF(cpf);
}

function ValidarCPF(sender, args) {
    if (Trim(args.Value) == "")
        args.IsValid = true;
    else {
        args.IsValid = ValidarCPFClient(args.Value);
    }
}

function ValidarSituacaoCampo(textbox) {

    for (var i = 0; i < textbox.Validators.length; i++) {

        if (textbox.Validators[i].isvalid === false) {
            return;
        }
    }

    this.onchange = null;
}
