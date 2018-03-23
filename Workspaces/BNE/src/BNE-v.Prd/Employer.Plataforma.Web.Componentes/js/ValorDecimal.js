function AplicarMascaraDecimal(controle, casas) {
    var valor = Trim(controle.value);
    if (valor == '')
        return;

    if (ValidarValorDecimalClient(valor))
        valor = FotmatDecimal(RecuperarValorClient(valor, casas), casas);

    controle.value = valor;
}
function RemoverMascaraControleDecimal(controle) {
    if (controle.disabled)
        return;

    var valor = Trim(controle.value);

    if (ValidarValorDecimalClient(valor)) {
        valor = valor.replace(/\./g, '');
        arr = valor.split(',');
        if (arr.length == 2) {
            /*
              Força o default como base 10
            */
            if (parseInt(arr[1],10) == 0)
                valor = arr[0];
        }
    }
    controle.value = valor;
    controle.select();
}

function ApenasNumerosDecimal(controle, evt, casas, tam, cultura) {
    /// <summary>
    /// Retorna somente nros ou backspace/delete/return.
    /// </summary>
    var tecla = employer.event.getKey(evt);
    //verifica se é uma tecla especial
    if (employer.key.isSpecial(tecla[0])) return true;

    //48 = '0', 57='9', 44=',', 45='-' 46='.' 
    if (cultura == 'pt-BR') {
        if (((tecla[1] >= 48) && (tecla[1] <= 57)) || (tecla[1] == 44) || (tecla[1] == 45)) {
            if (tecla[1] == 44) {
                if (casas == 0) return false;
                if (controle.value.indexOf(',') > -1) return false;
            }
            if (tecla[1] == 45) {
                if (employer.util.getSelectionSize(controle) != controle.value.length) {
                    return false;
                } else if (employer.util.getSelectionSize(controle) == 0 && controle.value.indexOf('-') > -1) {
                    return false;
                }
            }
        } else { return false; }
    } else {
        if (((tecla[1] >= 48) && (tecla[1] <= 57)) || (tecla[1] == 46) || (tecla[1] == 45)) {
            if (tecla[1] == 46) {
                if (casas == 0) return false;
                if (controle.value.indexOf('.') > -1) return false;
            }
            if (tecla[1] == 45) {
                if (employer.util.getSelectionSize(controle) != controle.value.length) {
                    return false;
                } else if (employer.util.getSelectionSize(controle) == 0 && controle.value.indexOf('-') > -1) {
                    return false;
                }
            }
        } else { return false; }
    }
}
function ValidarValorDecimal(sender, args) {
    //Bug no FF 3.6
    if (sender.parentNode == null)
        return;
    var controle = sender.parentNode.parentNode;
    if (args.Value == "") {
        args.IsValid = ((controle.getAttribute("Obrigatorio") != true) || (controle.getAttribute("Obrigatorio") != "1"));
        var newtext = document.createTextNode(controle.getAttribute("MensagemErroObrigatorio"));
        if (sender.childNodes[0]) {
            sender.removeChild(sender.childNodes[0]);
        }
        sender.appendChild(newtext);
        if (!args.IsValid && sender.style.visibility == "hidden") {
            sender.style.visibility = "visible";
        }
    }
    else {
        if (ValidarValorDecimalClient(args.Value)) {
            var res = ValidarIntervaloDecimalClient(
                 args.Value, controle.getAttribute("ValorMinimo"), controle.getAttribute("ValorMaximo"));

            args.IsValid = (res == 0);
            var newtext = "";
            if (res == 1)
            // Se a função retornou 1, o intervalo informado foi um intervalo inválido
                newtext = document.createTextNode(controle.getAttribute("MensagemErroIntervaloInvalido"));
            else
            // Se a função retornou 2, o texto entrado pelo usuário é inválido, 
            // ou está fora do intervalo estipulado
                newtext = document.createTextNode(controle.getAttribute("MensagemErroIntervalo"));

            if (sender.childNodes[0]) {
                sender.removeChild(sender.childNodes[0]);
            }
            sender.appendChild(newtext);
            if (!args.IsValid && sender.style.visibility == "hidden") {
                sender.style.visibility = "visible";
            }
        }
        else {
            var newtext = document.createTextNode(controle.getAttribute("MensagemErroFormato"));
            if (sender.childNodes[0]) {
                sender.removeChild(sender.childNodes[0]);
            }
            sender.appendChild(newtext);
            args.IsValid = false;
            if (!args.IsValid && sender.style.visibility == "hidden") {
                sender.style.visibility = "visible";
            }
        }
    }
}

function GetDecimal(valor) {
    if (valor == null || valor == "")
        return 0.0;

    //converte p/ string
    valor = "" + valor;
    var sValor = valor.replace(/\./g, "").replace(/\,/g, ".");
    return parseFloat(sValor);
}

function ValidarIntervaloDecimalClient(valor, valorMin, valorMax) {
    if (ValidarValorDecimalClient(valor) && ValidarValorDecimalClient(valorMin) && ValidarValorDecimalClient(valorMax)) {

        var vlr = GetDecimal(valor);
        var vlrMin = GetDecimal(valorMin);
        var vlrMax = GetDecimal(valorMax);

        if (vlrMin == 0 && vlrMax == 0)
            return 0;

        if (vlrMin > vlrMax)
            return 1; /* Erro 1: Intervalo inválido */

        if ((vlrMin <= vlr) && (vlr <= vlrMax))
            return 0;
    }
    /*Erro 2: Valor fora do intervalo*/
    return 2;
}

function FotmatDecimal(valor, casasDecimais) {
    var sValor = valor.toString();
    var p = sValor.indexOf(".");
    var sNValor = "";
    if (p > -1)
        sNValor = "," + padRight(sValor.substring(p + 1), '0', casasDecimais);
    else if (casasDecimais > 0)
        sNValor = "," + padRight('', '0', casasDecimais);
    else
        p = sValor.length;

    var sInteiro = sValor.substring(0, p);
    var pt = 0;
    for (var i = sInteiro.length - 1; i >= 0; i--, pt++) {
        if (sInteiro.charAt(i) == "-")
            pt--;
        if (pt == 3) {
            pt = 0;
            sNValor = '.' + sNValor;
        }
        sNValor = sInteiro.charAt(i) + sNValor;
    }
    return sNValor;
}
function padRight(str, pad, count) {
    str = str.toString();
    while (str.length < count)
        str = str + pad;
    return str;
}
function RecuperarValorClient(valor, casasDecimais) {
    var dValor = GetDecimal(valor);
    return dValor.toFixed(casasDecimais);
}

function ValidarValorDecimalClient(valor) {
    try {
        //Converte p/ string
        valor = "" + valor;
        var v = valor.replace(/\./g, "").replace(/\,/g, ".");
        var valorD = parseFloat(v);
        var sValorD = valorD.toString().replace(/\,/g, ".");

        return (!((valorD != 0 && !valorD) || RemoveZerosNaoUsado(v) != sValorD));
    }
    catch (e) {
        return false;
    }
}
function RemoveZerosNaoUsado(sValor) {
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