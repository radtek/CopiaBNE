function ValidarAno(ano) {
    var ano4Digitos = FormatarAno(ano.replace('_','').replace('_',''))
    return (ano4Digitos > 1900);
}

function validarEmail(email)
{
    if (email == null || email == '')
        return true;

    return (/^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/.test(email))
}

function DataPorExtenso() {
    var dta = new Date();

    var dia = dta.getDate();
    var mes = dta.getMonth();
    var ano = dta.getFullYear();


    if (dia < 10)
        dia = "0" + dia;

    arrayDia = new Array();
    arrayDia[0] = "Domingo";
    arrayDia[1] = "Segunda-Feira";
    arrayDia[2] = "Terça-Feira";
    arrayDia[3] = "Quarta-Feira";
    arrayDia[4] = "Quinta-Feira";
    arrayDia[5] = "Sexta-Feira";
    arrayDia[6] = "Sabado";

    var arrayMes = new Array();
    arrayMes[0] = "Janeiro";
    arrayMes[1] = "Fevereiro";
    arrayMes[2] = "Março";
    arrayMes[3] = "Abril";
    arrayMes[4] = "Maio";
    arrayMes[5] = "Junho";
    arrayMes[6] = "Julho";
    arrayMes[7] = "Agosto";
    arrayMes[8] = "Setembro";
    arrayMes[9] = "Outubro";
    arrayMes[10] = "Novembro";
    arrayMes[11] = "Dezembro";

    return dia + ' de ' + arrayMes[mes] + ' de ' + ano;
}

function validarData(dta) {
    var valido = true;

    var dia = dta.substring(0, 2);
    var mes = dta.substring(3, 5);
    var ano = dta.substring(6, 10);

    //Criando um objeto Date usando os valores ano, mes e dia.
    var novaData = new Date(ano, (mes - 1), dia);

    var mesmoDia = parseInt(dia, 10) == parseInt(novaData.getDate());
    var mesmoMes = parseInt(mes, 10) == parseInt(novaData.getMonth()) + 1;
    var mesmoAno = parseInt(ano) == parseInt(novaData.getFullYear());

    if (!((mesmoDia) && (mesmoMes) && (mesmoAno)))
        valido = false;

    return valido;
}

//Validar data de demissão com a data de Admissão
function ValidarDataDemissao(mesAdmissao, anoAdmissao, mesDemissao, anoDemissao) {
    var validado = true;

    if (mesAdmissao != "" && mesDemissao != "" && anoAdmissao != "" && anoDemissao != "") {
        
        var mesAdmissaoAjustado = mesAdmissao;
        var mesDemissaoAjustado = mesDemissao;

        if (parseInt(mesAdmissao) < 10)
            var mesAdmissaoAjustado = '0' + mesAdmissao;
        
        if (parseInt(mesDemissao) < 10)
            var mesDemissaoAjustado = '0' + mesDemissao;

        var dataAdmissao = parseInt(anoAdmissao.toString() + mesAdmissaoAjustado.toString() + '01');
        var dataDemissao = parseInt(anoDemissao.toString() + mesDemissaoAjustado.toString() + '01');

        if (dataAdmissao < dataDemissao) {
            validado = true;
            $('#bntSalvarPasso2').removeAttr('disabled');
        } else {
            validado = false;
            $('#bntSalvarPasso2').attr('disabled', 'disabled');
        }
    }
    
    return validado;
}

function ValidarPretensaoMaiorqueSalarioMinimo(pretensao, vlrMinimo) {
    var validado = false;
    var pretensaoSalarial = pretensao;

    var valordoMinimo = vlrMinimo + '.00';
    if (parseFloat(pretensaoSalarial) >= parseFloat(valordoMinimo)) {
        validado = true;
    }
    return validado;
}

function ValidarCelular(celular) {
    var validado = true;
    var numeroCelular = celular;

    if (numeroCelular.length < 8) {
        validado = false;
        return validado;
    }

    var totalCaracteres = numeroCelular.length;
    var key = event.which || event.keyCode || event.charCode;

    if (key != 8 && totalCaracteres > 9) { validado = false; }

    if (key == 8 && totalCaracteres <= 7) {
        validado = false;
    }
    if (key != 8 && totalCaracteres <= 6) {
        validado = false;
    }

    if (key != 8) {
        totalCaracteres++;
        if (totalCaracteres == 8 && numeroCelular <= 4999999) {
            validado = false;
        }
        if (totalCaracteres == 9 && numeroCelular <= 49999999) {
            validado = false;
        }
        if (totalCaracteres == 10 && numeroCelular <= 499999999) {
            validado = false;
        }

    } else {
        numeroCelular = numeroCelular.substring(0, numeroCelular.length - 1);

        if (totalCaracteres - 1 == 7) {
            validado = false;
        }

        if (totalCaracteres == 8 && numeroCelular <= 4999999) {
            validado = false;
        }
        if (totalCaracteres == 9 && numeroCelular <= 49999999) {
            validado = false;
        }
        if (totalCaracteres == 10 && numeroCelular <= 499999999) {
            validado = false;
        }
    }

    return validado;
}

function ValidarTelefone(telefone) {
   
    var validado = true;
    var numeroTelefone = telefone;
    if (numeroTelefone.length < 8) {
        validado = false;
        return validado;
    }

    if (numeroTelefone <= 49999999) {
        validado = true;
    }

    return validado;
}

function ValidarNome(args) {
    if (args === undefined) {
        return false;
    }
    var w, z, y, x;
    var isValid = true;
    for (x = 0; x < args.length; x++) {
        z = args.substring(x, x + 1);
        if ((x >= 2 && z == y && z == w)) {
            isValid = false;
        }
        else {
            y = w;
            w = z;
            z = '-';
        }
    }

    if (!args.match("^[A-Z,a-z,ÁáÂâÃãÄäÉéÊêËëÍíÏïÓóÔôÕõÖöÚúÛûÜüÇç']{1,}( ){1,}[A-Z,a-z,ÁáÂâÃãÄäÉéÊêËëÍíÏïÓóÔôÕõÖöÚúÛûÜüÇç']{2,}(( ){1,}[A-Z,a-z,ÁáÂâÃãÄäÉéÊêËëÍíÏïÓóÔôÕõÖöÚúÛûÜüÇç']{1,})*$"))
        isValid = false;
    
    return isValid;
}

function FormatarAno(ano) {
    if (ano != null) {
        if (ano.toString().length < 4) {
            if (ano >= 30 && ano <= 99)
                ano = "1900".substring(0, 4 - ano.length) + ano;
            else
                ano = "2000".substring(0, 4 - ano.length) + ano;
        }
        else
            ano = ano.substring(0, 4);
    }
    return ano;
}

function ValCPF(cpf_view) {
    cpf = cpf_view || "";

    cpf = cpf.replace(/\./gi, '');
    cpf = cpf.replace(/-/gi, '');
    cpf = cpf.replace(/\_/gi, '');

    var validado = false;
    var numeros, digitos, soma, i, resultado, digitos_iguais;
    digitos_iguais = 1;

    if (cpf.length < 11) {
        cpf = ("00000000000" + cpf).slice(-11);
    }


    for (i = 0; i < cpf.length - 1; i++)
        if (cpf.charAt(i) != cpf.charAt(i + 1)) {
            digitos_iguais = 0;
            break;
        }
    if (!digitos_iguais) {
        numeros = cpf.substring(0, 9);
        digitos = cpf.substring(9);
        soma = 0;
        for (i = 10; i > 1; i--)
            soma += numeros.charAt(10 - i) * i;
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(0))
            validado = false;
        else {
            numeros = cpf.substring(0, 10);
            soma = 0;
            for (i = 11; i > 1; i--)
                soma += numeros.charAt(11 - i) * i;
            resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
            if (resultado != digitos.charAt(1))
                validado = false;
            else
                validado = true;
        }
    }
    else
        validado = false;

    return validado;
}

function ValidaCPF(cpf_view, ctrl, modelValue) {
    cpf = cpf_view || "";

    cpf = cpf.replace(/\./gi, '');
    cpf = cpf.replace(/-/gi, '');
    cpf = cpf.replace(/\_/gi, '');

    var validado = false;
    var numeros, digitos, soma, i, resultado, digitos_iguais;
    digitos_iguais = 1;

    if (cpf.length < 11) {
        cpf = ("00000000000" + cpf).slice(-11);
    }


    for (i = 0; i < cpf.length - 1; i++)
        if (cpf.charAt(i) != cpf.charAt(i + 1)) {
            digitos_iguais = 0;
            break;
        }
    if (!digitos_iguais) {
        numeros = cpf.substring(0, 9);
        digitos = cpf.substring(9);
        soma = 0;
        for (i = 10; i > 1; i--)
            soma += numeros.charAt(10 - i) * i;
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(0))
            validado = false;
        else {
            numeros = cpf.substring(0, 10);
            soma = 0;
            for (i = 11; i > 1; i--)
                soma += numeros.charAt(11 - i) * i;
            resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
            if (resultado != digitos.charAt(1))
                validado = false;
            else
                validado = true;
        }
    }
    else
        validado = false;

    return validado;
}

function formatarCPF(ctrl, modelValue) {
    return modelValue.substr(0, 3) + '.' + modelValue.substr(3, 3) + '.' + modelValue.substr(6, 3) + '-' + modelValue.substr(9, 2);
}
function LimparCPF(ctrl, modelValue) {
    return modelValue.replace('.','').replace('.','').replace('-','');
}

function CalcularIdade(dataNascimento) {

    var anoNiver = dataNascimento.substr(6, 4);
    var mesNiver = dataNascimento.substr(3, 2);
    var diaNiver = dataNascimento.substr(0, 2);

    var dtaNiver = new Date(anoNiver,mesNiver,diaNiver,6);

    var d = new Date(),
        ano_atual = d.getFullYear(),
        mes_atual = d.getMonth() + 1,
        dia_atual = d.getDate(),

        ano_aniversario = +dtaNiver.getFullYear(),
        mes_aniversario = +dtaNiver.getMonth(),
        dia_aniversario = +dtaNiver.getDate(),

        quantos_anos = ano_atual - ano_aniversario;

    if (mes_atual < mes_aniversario || mes_atual == mes_aniversario && dia_atual < dia_aniversario) {
        quantos_anos--;
    }

    return quantos_anos < 0 ? 0 : quantos_anos;
}

function ValidarCidade(nomeCidade)
{
    if (nomeCidade == '')
        return true;

    var validado = false;

    if (nomeCidade.length > 3)
        validado = true;


    return validado;
}

function getBase64Image(img) {
    // Create an empty canvas element
    var canvas = document.createElement("canvas");
    canvas.width = img.width;
    canvas.height = img.height;

    // Copy the image contents to the canvas
    var ctx = canvas.getContext("2d");
    ctx.drawImage(img, 0, 0);

    // Get the data-URL formatted image
    // Firefox supports PNG and JPEG. You could check img.src to
    // guess the original format, but be aware the using "image/jpg"
    // will re-encode the image.
    var dataURL = canvas.toDataURL("image/png");

    return dataURL;
    //return dataURL.replace(/^data:image\/(png|jpg);base64,/, "");
}

function dataURItoBlob(dataURI) {
    // convert base64/URLEncoded data component to raw binary data held in a string
    var byteString;
    if (dataURI.split(',')[0].indexOf('base64') >= 0)
        byteString = atob(dataURI.split(',')[1]);
    else
        byteString = unescape(dataURI.split(',')[1]);

    // separate out the mime component
    var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];

    // write the bytes of the string to a typed array
    var ia = new Uint8Array(byteString.length);
    for (var i = 0; i < byteString.length; i++) {
        ia[i] = byteString.charCodeAt(i);
    }

    return new Blob([ia], { type: mimeString });
}

if (typeof KeyEvent == "undefined") {
    var KeyEvent = {
        DOM_VK_CANCEL: 3,
        DOM_VK_HELP: 6,
        DOM_VK_BACK_SPACE: 8,
        DOM_VK_TAB: 9,
        DOM_VK_CLEAR: 12,
        DOM_VK_RETURN: 13,
        DOM_VK_ENTER: 14,
        DOM_VK_SHIFT: 16,
        DOM_VK_CONTROL: 17,
        DOM_VK_ALT: 18,
        DOM_VK_PAUSE: 19,
        DOM_VK_CAPS_LOCK: 20,
        DOM_VK_ESCAPE: 27,
        DOM_VK_SPACE: 32,
        DOM_VK_PAGE_UP: 33,
        DOM_VK_PAGE_DOWN: 34,
        DOM_VK_END: 35,
        DOM_VK_HOME: 36,
        DOM_VK_LEFT: 37,
        DOM_VK_UP: 38,
        DOM_VK_RIGHT: 39,
        DOM_VK_DOWN: 40,
        DOM_VK_PRINTSCREEN: 44,
        DOM_VK_INSERT: 45,
        DOM_VK_DELETE: 46,
        DOM_VK_0: 48,
        DOM_VK_1: 49,
        DOM_VK_2: 50,
        DOM_VK_3: 51,
        DOM_VK_4: 52,
        DOM_VK_5: 53,
        DOM_VK_6: 54,
        DOM_VK_7: 55,
        DOM_VK_8: 56,
        DOM_VK_9: 57,
        DOM_VK_SEMICOLON: 59,
        DOM_VK_EQUALS: 61,
        DOM_VK_A: 65,
        DOM_VK_B: 66,
        DOM_VK_C: 67,
        DOM_VK_D: 68,
        DOM_VK_E: 69,
        DOM_VK_F: 70,
        DOM_VK_G: 71,
        DOM_VK_H: 72,
        DOM_VK_I: 73,
        DOM_VK_J: 74,
        DOM_VK_K: 75,
        DOM_VK_L: 76,
        DOM_VK_M: 77,
        DOM_VK_N: 78,
        DOM_VK_O: 79,
        DOM_VK_P: 80,
        DOM_VK_Q: 81,
        DOM_VK_R: 82,
        DOM_VK_S: 83,
        DOM_VK_T: 84,
        DOM_VK_U: 85,
        DOM_VK_V: 86,
        DOM_VK_W: 87,
        DOM_VK_X: 88,
        DOM_VK_Y: 89,
        DOM_VK_Z: 90,
        DOM_VK_CONTEXT_MENU: 93,
        DOM_VK_NUMPAD0: 96,
        DOM_VK_NUMPAD1: 97,
        DOM_VK_NUMPAD2: 98,
        DOM_VK_NUMPAD3: 99,
        DOM_VK_NUMPAD4: 100,
        DOM_VK_NUMPAD5: 101,
        DOM_VK_NUMPAD6: 102,
        DOM_VK_NUMPAD7: 103,
        DOM_VK_NUMPAD8: 104,
        DOM_VK_NUMPAD9: 105,
        DOM_VK_MULTIPLY: 106,
        DOM_VK_ADD: 107,
        DOM_VK_SEPARATOR: 108,
        DOM_VK_SUBTRACT: 109,
        DOM_VK_DECIMAL: 110,
        DOM_VK_DIVIDE: 111,
        DOM_VK_F1: 112,
        DOM_VK_F2: 113,
        DOM_VK_F3: 114,
        DOM_VK_F4: 115,
        DOM_VK_F5: 116,
        DOM_VK_F6: 117,
        DOM_VK_F7: 118,
        DOM_VK_F8: 119,
        DOM_VK_F9: 120,
        DOM_VK_F10: 121,
        DOM_VK_F11: 122,
        DOM_VK_F12: 123,
        DOM_VK_F13: 124,
        DOM_VK_F14: 125,
        DOM_VK_F15: 126,
        DOM_VK_F16: 127,
        DOM_VK_F17: 128,
        DOM_VK_F18: 129,
        DOM_VK_F19: 130,
        DOM_VK_F20: 131,
        DOM_VK_F21: 132,
        DOM_VK_F22: 133,
        DOM_VK_F23: 134,
        DOM_VK_F24: 135,
        DOM_VK_NUM_LOCK: 144,
        DOM_VK_SCROLL_LOCK: 145,
        DOM_VK_COMMA: 188,
        DOM_VK_PERIOD: 190,
        DOM_VK_SLASH: 191,
        DOM_VK_BACK_QUOTE: 192,
        DOM_VK_OPEN_BRACKET: 219,
        DOM_VK_BACK_SLASH: 220,
        DOM_VK_CLOSE_BRACKET: 221,
        DOM_VK_QUOTE: 222,
        DOM_VK_META: 224
    };
}