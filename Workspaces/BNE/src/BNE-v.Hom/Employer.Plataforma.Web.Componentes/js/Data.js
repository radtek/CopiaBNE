function DefinirMascaraData(data, dataAtual) {
    if (data.match("\\d{1,2}(/)*(\\d{1,2})*(/)*(\\d{1,})*")) {
        arr = dataAtual.split('/');
        dia = arr[0];
        mes = arr[1];
        ano = arr[2];

        arr = data.split('/');
        if (arr.length == 1) {
            if (arr[0].length != 0) {
                if (arr[0].length <= 2)
                    dia = arr[0];
                else {
                    if (arr[0].length <= 4) {
                        dia = arr[0].substring(0, 2);
                        mes = arr[0].substring(2);
                    }
                    else {
                        dia = arr[0].substring(0, 2);
                        mes = arr[0].substring(2, 4);
                        ano = arr[0].substring(4);
                    }
                }
            }
        }
        else {
            if (arr.length == 2) {
                if (arr[0].length != 0)
                    dia = arr[0];
                if (arr[1].length != 0)
                    mes = arr[1];
            }
            else {
                if (arr[0].length != 0)
                    dia = arr[0];
                if (arr[1].length != 0)
                    mes = arr[1];
                if (arr[2].length != 0)
                    ano = arr[2];
            }
        }

        if (dia.length < 2)
            dia = '0' + dia;
        else
            dia = dia.substring(0, 2);

        if (mes.length < 2)
            mes = '0' + mes;
        else
            mes = mes.substring(0, 2);

        if (ano.length < 4) {
            if (ano > 30 && ano < 99)
                ano = "1900".substring(0, 4 - ano.length) + ano;
            else
                ano = "2000".substring(0, 4 - ano.length) + ano;
        }
        else
            ano = ano.substring(0, 4);

        data = dia + '/' + mes + '/' + ano;
    }
    return data;
}
function AplicarMascaraData(controle, dataAtual) {
    var valor = Trim(controle.value);
    if (valor == '')
        return;

    controle.value = DefinirMascaraData(valor, dataAtual);
}
function ValidarSeparador(controle) {
    data = controle.value;
    if (data.indexOf("/") >= 0)
        if (data.substring(data.indexOf("/") + 1).indexOf("/") >= 0)
        return false;
    return true;
}

function ValidarData(sender, args) {
    var controle = sender.parentNode.parentNode;
    var dataMinima = document.getElementById(controle.id + "_hfDataMinima").value;
    var dataMaxima = document.getElementById(controle.id + "_hfDataMaxima").value;


    if (sender.firstChild != null) {
        if (args.Value == "") {
            sender.firstChild.nodeValue = controle.getAttribute("MensagemErroObrigatorio");
            args.IsValid = ((controle.getAttribute("Obrigatorio") != true) || (controle.getAttribute("Obrigatorio") != "1"));
        }
        else {
            sender.firstChild.nodeValue = controle.getAttribute("MensagemErroFormato");

            dataAtual = GetDataAtual();
            args.IsValid = ValidarDataClient(DefinirMascaraData(args.Value, dataAtual.value));
            if (args.IsValid) {
                if (sender.firstChild != null) {
                    sender.firstChild.nodeValue = controle.getAttribute("MensagemErroIntervalo");
                    args.IsValid = ValidarIntervaloDataClient(
                        DefinirMascaraData(args.Value, dataAtual.value),
                        dataMinima,
                        dataMaxima);
                }
            }
        }
    }
}
 
var dtAtual = null;
function GetDataAtual() {
    if (dtAtual == null)
        dtAtual = Data.DataAtual();
    return dtAtual;
}

function padLeft(str, pad, count) {
    str = str.toString();
    while (str.length < count)
        str = pad + str;
    return str;
}

//Valida a data com ano bisexto e formato.
//É obrigado estar formatada dd/MM/yyyy
function ValidarDataClient(sDate) {
    try {
        var dtVal = GetDataFormata(sDate);

        var fData =
        padLeft(dtVal.getDate(), '0', 2) + '/' +
        padLeft(dtVal.getMonth() + 1, '0', 2) + '/' +
        dtVal.getFullYear();

        //O javascript troca de mes caso passe os dis válidos de mes.
        return sDate == fData;
    }
    catch (e) {
        return false;
    }
}
function GetDataFormata(sDate) {
    var sPlit = sDate.split("/");
    return new Date(sPlit[2], sPlit[1] - 1, sPlit[0], 12, 0, 0, 0);
}

function ValidarIntervaloDataClient(valor, valorMin, valorMax) {
    if (ValidarDataClient(valor)) {
        if (ValidarDataClient(valorMin)) {
            var data = GetDataFormata(valor);
            var dataAux = GetDataFormata(valorMin);
            if (data >= dataAux) {
                if (ValidarDataClient(valorMax)) {
                    dataAux = GetDataFormata(valorMax);
                    return data <= dataAux;
                }
            }
        }
    }
}

function ApenasNumerosData(controle, event, dataAtual) {
    /// <summary>
    /// Valida tecla digitada, retorna somente nros.
    /// </summary>
    var tecla = employer.event.getKey(event);
    //48 = '0', 57='9', 47='/', 32=' '
    if (((tecla[1] >= 48) && (tecla[1] <= 57)) || (tecla[1] == 47) || (tecla[1] == 32) || employer.key.isSpecial(tecla[0])) {
        if (tecla[1] == 32) {
            if (controle.value.length == 0) {
                employer.event.cancel(event);
                // seta data atual caso aperte espaço no campo
                controle.value = dataAtual;
                // Se o controle tiver evento de onchange associado, dispara o evento
                if (controle.onchange)
                    controle.onchange();
                ProxControle(controle);
            }
            return false;
        } else if (tecla[1] == 47) {
            return ValidarSeparador(controle);
        }
    } else { return false; }
}

function getDataSize(controle) {
    /// <summary>
    /// Retorna tamanho da campo data.
    /// </summary>
    if (controle.value.indexOf("/") >= 0) {
        return 10;
    } else {
        return 8;
    }
}