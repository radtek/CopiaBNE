var _outros = {
    index: 0,
    name: "Outros",
    regex: "^[2-9]{1}\d{3}\-{0,1}\d{4}$|^[0][1-9]\d{2}\-\d{3}\-\d{4}$|^[0][1-9]\d{2}\d{7}$",
    size: 11
};

var _fixo = {
    index: 1,
    name: "Fixo",
    regex: "^[2-5]{1}\\d{3}\\-{0,1}\\d{4}$",
    size: 8
};

var _celular = {
    index: 2,
    name: "Celular",
    regex: "^[6-9]{1}\\d{3}\\-{0,1}\\d{4}$",
    size: 8
};

var _celularNoveDigitos = {
    index: 2,
    name: "Celular",
    regex: "^[9]\\d{4}\\-{0,1}\\d{4}|[7]{1}\\d{3}\\-{0,1}\\d{4}$",
    size: 9
};

var _fixocelular = {
    index: 3,
    name: "FixoCelular",
    regex: "^[2-5]{1}\\d{3}\\-{0,1}\\d{4}|[6-9]{1}\\d{3}\\-{0,1}\\d{4}$"
};

var _fixocelularNoveDigitos = {
    index: 3,
    name: "FixoCelular",
    regex: "^[2-5]{1}\\d{3}\\-{0,1}\\d{4}|[9]\\d{4}\\-{0,1}\\d{4}|[7]{1}\\d{3}\\-{0,1}\\d{4}$"
};

var _dddNoveDigitos = ['11', '12', '13', '14', '15', '16', '17', '18', '19', '21', '22', '24', '27', '28', '31', '32', '33', '34', '35', '37', '38', '41', '42', '43', '44', '45', '46', '47', '48', '49', '51', '53', '54', '55', '61', '62', '63', '64', '65', '66', '67', '68', '69', '71', '73', '74', '75', '77', '79', '81', '82', '83', '84', '85', '86', '87', '88', '89', '91', '92', '93', '94', '95', '96', '97', '98', '99'];
function RemoverZeroEsq(valor) {
    for (i = 0; i < valor.length; i++) {
        if (valor.charAt(i) != '0')
            break;
    }

    return valor.substring(i);
}
function AplicarMascaraDD(controle) {
    var valor = Trim(controle.value);
    if (valor == '')
        return;
    if (valor.match("\\d{1,3}")) {
        valor = valor.substring(0, 2);
    }
    controle.value = valor;
}

function AplicarMascaraFone(controle) {
    var valor = Trim(controle.value);
    if (valor == '') { return; }
    if (valor.match(/^(\d{8,9})$/)) { valor = valor.replace(/(\d{4,5})(\d{4})/, "$1-$2"); }
    else if (valor.match(/^(\d{11})$/) && valor.substring(0, 1) == "0") {
        valor = valor.replace(/(\d{4})(\d{3})(\d{4})/, "$1-$2-$3");
    }
    controle.value = valor;
}

function ProxControleDD(controle, tecla) {
    if (controle.value.length >= 1) {
        if (controle.value.charAt(0) == '0' && controle.value.length != 2)
            return true;
        controle.value += String.fromCharCode(tecla);

        for (i = 0; i < document.forms[0].elements.length; i++) {
            if (document.forms[0].elements[i] == controle)
                document.forms[0].elements[++i].focus();
        }
        return false;
    }
    return true;
}

function getPhoneSize(controle, tipo, idControleDDD) {
    /// <summary>
    /// Retorna dinâmicamente o tamanho do campo telefone.
    /// </summary>
    var size;
    if (tipo == _outros.index) {
        size = _outros.size;
        if (controle.value.charAt(0) != '0') { size = 8; }
    } else if (tipo == _fixo.index) {
        size = _fixo.size;
    } else if (tipo == _celular.index) {
        var ddd = document.getElementById(idControleDDD).value;

        if (_dddNoveDigitos.contains(ddd)) {
            size = _celularNoveDigitos.size;
        }
        else {
            size = _celular.size;
        }
    } else if (tipo == _fixocelular.index) {
        var ddd = document.getElementById(idControleDDD).value;

        if (_dddNoveDigitos.contains(ddd)) {
            size = _celularNoveDigitos.size;
        }
        else {
            size = _celular.size;
        }
    } else {
        size = _outros.size;
        if (controle.value.charAt(0) != '0') { size = 8; }
    }
    return size;
}

function getAreaCodeSize(controle) {
    /// <summary>
    /// Retorna dinâmicamente o tamanho do campo código de área.
    /// </summary>
    return controle.value.charAt(0) == '0' ? 3 : 2;
}

function ApenasNumerosDD(controle, event) {
    /// <summary>
    /// Retorna somente nros ou backspace/delete/return e impede ZERO (0) no primeiro digito.
    /// </summary>
    var tecla = employer.event.getKey(event);
    var sizeSelected = component_getSizeTextSelected(controle);
    if (controle.value.length == getAreaCodeSize(controle) && component_isTextNotSelected(controle) && tecla[0] == 0) { return false; }
    if (controle.value.length < getAreaCodeSize(controle) || sizeSelected > 0 || employer.key.isSpecial(tecla[0])) {
        // 48='0', 57='9'  
        return ((tecla[1] >= 48 && tecla[1] <= 57) || employer.key.isSpecial(tecla[0])) ? true : false;
    } else {
        return false;
    }
}

function ApenasNumerosFone(controle, event, tipo, idControleDDD) {
    /// <summary>
    /// Retorna somente nros ou backspace/delete/return.
    /// </summary>
    var tecla = employer.event.getKey(event);
    var pos = component_getSelectionStart(controle);
    var sizeSelected = component_getSizeTextSelected(controle);
    if (controle.value.length == getPhoneSize(controle, tipo, idControleDDD) && component_isTextNotSelected(controle) && tecla[0] == 0) { return false; }
    if (controle.value.length < getPhoneSize(controle, tipo, idControleDDD) || sizeSelected > 0 || employer.key.isSpecial(tecla[0])) {
        // 48='0', 57='9'        
        return ((tecla[1] >= 48 && tecla[1] <= 57) || employer.key.isSpecial(tecla[0])) ? true : false;
    } else {
        return false;
    }
}

function ConfigurarValidador(controleDDD, tipo, validador) {
    var ddd = controleDDD.value;
    var val = employer.util.findControl(validador)[0];

    if (tipo == _celular.index) {
        if (_dddNoveDigitos.contains(ddd)) {
            val.validationexpression = _celularNoveDigitos.regex;
        }
        else {
            val.validationexpression = _celular.regex;
        }
    } else if (tipo == _fixocelular.index) {
        if (_dddNoveDigitos.contains(ddd)) {
            val.validationexpression = _fixocelularNoveDigitos.regex;
        }
        else {
            val.validationexpression = _fixocelular.regex;
        }
    }
}

function ConfigurarObrigatoriedade(id, obrigatorio) {
    //Só faz a validação se o campo não for obrigatório
    //Se o campo for obrigatório o trecho de código abaixo pode retirar a obrigatoriedade do campo.
    //---
    //Ex. Tenho um campo obrigatorio com o DDD e o FONE preenchido.
    //Se eu apagar o fone, o DDD estará preenchido, logo o FONE continuará obrigatório.
    //Agora, se eu apagar o DDD, o campo FONE tem sua obrigatoriedade retirada
    if (!obrigatorio) {
        var ddd = document.getElementById(id + "_txtDDD");
        var ddi = document.getElementById(id + "_txtDDI");
        var fone = document.getElementById(id + "_txtFone");

        if (ddd != null)
            employer.controles.enableValidator(id + "_rfDDD", fone.value != '');

        if (ddi != null)
            employer.controles.enableValidator(id + "_rfDDI", fone.value != '');

        var ena = false;

        if (ddd != null)
            ena = ena | (ddd.value != '');

        if (ddi != null)
            ena = ena | (ddi.value != '');

        employer.controles.enableValidator(id + "_rfFone", ena);
    }
}

Array.prototype.contains = function (objeto) {
    var i = this.length;
    while (i--) {
        if (this[i] === objeto) {
            return true;
        }
    }
    return false;
}