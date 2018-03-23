// Declare a namespace.
Type.registerNamespace("BNE.Componentes");

// Define a simplified component.
BNE.Componentes.ControlCNPJ = function (element) {
    BNE.Componentes.ControlCNPJ.initializeBase(this, [element]);

    this._id = element.id;

    // Handlers for the events    
    this._onBlur = null;
    this._onFocus = null;
    this._onKeyPress = null;
    this._textoAntesFocus = null;

    // Compatibilidade com ControlBaseValidator
    this._mensagemErroValorMinimo = null;
    this._mensagemErroValorMaximo = null;

    // Summary
    this._MensagemErroObrigatorioSummary = null;
    this._MensagemErroFormatoSummary = null;
    this._MensagemErroValorMinimoSummary = null;
    this._MensagemErroValorMaximoSummary = null;



    //Compatibilidade com ControlBaseValidator
    this.get_MensagemErroValorMinimo = function () {
        return this._mensagemErroValorMinimo;
    },
    this.set_MensagemErroValorMinimo = function (valor) {
        this._mensagemErroValorMinimo = valor;
    },
    this.get_MensagemErroValorMaximo = function () {
        return this._mensagemErroValorMaximo;
    },
    this.set_MensagemErroValorMaximo = function (valor) {
        this._mensagemErroValorMaximo = valor;
    },
    //Fim compatibilidade com ControlBaseValidator

    //Summary
    this.get_MensagemErroObrigatorioSummary = function () {
        if (AjaxClientControlBase.IsNullOrEmpty(this._MensagemErroObrigatorioSummary) == false)
            return this._MensagemErroObrigatorioSummary;

        return this.get_MensagemErroObrigatorio();
    },
    this.set_MensagemErroObrigatorioSummary = function (value) {
        this._MensagemErroObrigatorioSummary = value;
    },
    this.get_MensagemErroFormatoSummary = function () {
        if (AjaxClientControlBase.IsNullOrEmpty(this._MensagemErroFormatoSummary) == false)
            return this._MensagemErroFormatoSummary;

        return this.get_MensagemErroFormato();
    },
    this.set_MensagemErroFormatoSummary = function (value) {
        this._MensagemErroFormatoSummary = value;
    },
    this.get_MensagemErroValorMinimoSummary = function () {
        if (AjaxClientControlBase.IsNullOrEmpty(this._MensagemErroValorMinimoSummary) == false)
            return this._MensagemErroValorMinimoSummary;

        return this.get_MensagemErroValorMinimo();
    },
    this.set_MensagemErroValorMinimoSummary = function (value) {
        this._MensagemErroValorMinimoSummary = value;
    },
    this.get_MensagemErroValorMaximoSummary = function () {
        if (AjaxClientControlBase.IsNullOrEmpty(this._MensagemErroValorMaximoSummary) == false)
            return this._MensagemErroValorMaximoSummary;

        return this.get_MensagemErroValorMaximo();
    },
    this.set_MensagemErroValorMaximoSummary = function (value) {
        this._MensagemErroValorMaximoSummary = value;
    },

    this.get_TextBox = function () {
        var espGrid = RegExp("(_[0-9]+)$");
        var m = espGrid.exec(this._id);

        if (m != null)
            return $get(this._id + "_txtValor" + m[0]);
        else
            return $get(this._id + "_txtValor");
    },
    this.get_TextBox_Id = function () {
        var espGrid = RegExp("(_[0-9]+)$");
        var m = espGrid.exec(this._id);

        if (m != null)
            return this._id + "_txtValor" + m[0];
        else
            return this._id + "_txtValor";
    },
    this.get_Validador = function () {
        var espGrid = RegExp("(_[0-9]+)$");
        var m = espGrid.exec(this._id);

        if (m != null) {
            return $get(this._id + "_cvValor" + m[0]);
        }
        else {
            return $get(this._id + "_cvValor");
        }
    },
    this.get_PnlValidador = function () {
        var espGrid = RegExp("(_[0-9]+)$");
        var m = espGrid.exec(this._id);

        if (m != null)
            return document.getElementById(this._id + "_pnlValidador" + m[0]);
        else
            return document.getElementById(this._id + "_pnlValidador");
    },
    this.find_Validador = function () {
        var espGrid = RegExp("(_[0-9]+)$");
        var m = espGrid.exec(this._id);

        if (m != null) {
            return $find(this._id + "_cvValor" + m[0]);
        }
        else {
            return $find(this._id + "_cvValor");
        }
    },
     this.initialize = function () {
         BNE.Componentes.ControlCNPJ.callBaseMethod(this, 'initialize');

         // Wireup the event handlers
         var element = this.get_TextBox();
         if (element) {
             this._onBlur = Function.createDelegate(this, this.Blur);
             $addHandler(element, 'blur', this._onBlur);

             this._onKeyPress = Function.createDelegate(this, this.KeyPress);
             $addHandler(element, 'keypress', this._onKeyPress);

             this._onFocus = Function.createDelegate(this, this.Focus);
             $addHandler(element, 'focus', this._onFocus);
         }
     },
    this.dispose = function () {
        $clearHandlers(this.get_TextBox());

        BNE.Componentes.ControlCNPJ.callBaseMethod(this, 'dispose');
    },
    this.get_TextoAntesFocus = function () {
        if (this._textoAntesFocus == null)
            return this.get_TextBox().value;

        return this._textoAntesFocus;
    },
    this.aplicarMascaraCNPJ = function () {
        var valor = this.trim(this.get_TextBox().value);
        if (valor == '')
            return;

        if (valor.length < 10)
            return;

        var l = 14 - valor.length;

        for (i = 0; i < l; i++) {
            valor = '0' + valor;
        }

        if (valor.match("\\d{14}")) {
            valor = valor.replace(/(\d{2})(\d)/, "$1.$2");
            valor = valor.replace(/(\d{3})(\d)/, "$1.$2");
            valor = valor.replace(/(\d{3})(\d)/, "$1/$2");
            valor = valor.replace(/(\d{4})(\d)/, "$1-$2");
        }
        this.get_TextBox().value = valor;
    },
    this.limparCNPJ = function (cnpj) {
        var valor = new String(cnpj);
        valor = valor.replace(".", "");
        valor = valor.replace(".", "");
        valor = valor.replace("/", "");
        valor = valor.replace("-", "");
        return this.trim(valor);
    },
    this.validarCNPJ = function (vcnpj) {
        var numeros, digitos, soma, i, resultado, pos, tamanho, digitos_iguais, cnpj = vcnpj.replace(/\D+/g, '');
        digitos_iguais = 1;
        if (cnpj.length != 14) {
            return false;
        }

        for (i = 0; i < cnpj.length - 1; i++)
            if (cnpj.charAt(i) != cnpj.charAt(i + 1)) {
                digitos_iguais = 0;
                break;
            }
        if (!digitos_iguais) {
            tamanho = cnpj.length - 2
            numeros = cnpj.substring(0, tamanho);
            digitos = cnpj.substring(tamanho);
            soma = 0;
            pos = tamanho - 7;
            for (i = tamanho; i >= 1; i--) {
                soma += numeros.charAt(tamanho - i) * pos--;
                if (pos < 2)
                    pos = 9;
            }
            resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
            if (resultado != digitos.charAt(0)) {
                return false;
            }

            tamanho = tamanho + 1;
            numeros = cnpj.substring(0, tamanho);
            soma = 0;
            pos = tamanho - 7;
            for (i = tamanho; i >= 1; i--) {
                soma += numeros.charAt(tamanho - i) * pos--;
                if (pos < 2)
                    pos = 9;
            }
            resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
            if (resultado != digitos.charAt(1)) {
                return false;
            }
            else {
                return true;
            }
        }
        else {
            return false;
        }
    }
}

//Simula herança
BNE.Componentes.ControlCNPJ.prototype = new BNE.Componentes.ControlBaseValidator;

BNE.Componentes.ControlCNPJ.prototype.Focus = function (ev) {
    ev.preventDefault();
    ev.stopPropagation();

    var read = this.get_TextBox().getAttribute("readonly");

    if (read != null) {
        return false;
    }

    this._textoAntesFocus = this.get_TextBox().value;

    this.get_TextBox().value = this.limparCNPJ(this.get_TextBox().value);
    try {
        this.get_TextBox().select();
    } catch (e) {
    }
}

BNE.Componentes.ControlCNPJ.prototype.Blur = function (ev) {
    ev.preventDefault();
    ev.stopPropagation();

    var read = this.get_TextBox().getAttribute("readonly");

    if (read != null) {
        return false;
    }

    // Aplica a mascara do CNPJ
    this.aplicarMascaraCNPJ();

    //Foi Alterado valor
    this.PostPadrao();
}

BNE.Componentes.ControlCNPJ.prototype.KeyPress = function (e) {

    var key = AjaxClientControlBase.Key.GetKeyCode(e.rawEvent);

    if (e.rawEvent.keyCode && AjaxClientControlBase.Key.isCommand(key))
        return true;

    var caracter = String.fromCharCode(key);

    if (isNaN(caracter) || (this.limparCNPJ(this.get_TextBox().value).length >= 14 && !this.has_selected_text(this.get_TextBox())))
        return AjaxClientControlBase.CancelEvent(e);

}

BNE.Componentes.ControlCNPJ.registerClass('BNE.Componentes.ControlCNPJ', Sys.UI.Behavior);

/*if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();*/

BNE.Componentes.ControlCNPJ.prototype.Validar = function (arg) {    
    arg.Value = this.get_TextBox().value;

    var validator = this.get_Validador();
    var div = this.get_PnlValidador();

    arg.IsValid = true;
    if (arg.Value == "" || arg.Value == null) {
        if (this.get_Obrigatorio()) {
            validator.innerHTML = this.get_MensagemErroObrigatorio();
            validator.errormessage = this.get_MensagemErroObrigatorioSummary();
            arg.IsValid = false;
        }
    }
    else {
        if (!this.validarCNPJ(arg.Value)) {
            validator.innerHTML = this.get_MensagemErroFormato();
            validator.errormessage = this.get_MensagemErroFormatoSummary();
            arg.IsValid = false;
        }
    }

    if (arg.IsValid) {
        div.style.display = "none";
        this.get_Validador().style.visibility = "hidden"
    }
    else {
        this.get_Validador().style.visibility = "visible"
        div.style.display = "block";
    }
}

BNE.Componentes.ControlCNPJ.prototype.MostrarErros = function () {

    var validator = this.get_Validador();
    var div = this.get_PnlValidador();

    IsValid = true;
    if (this.get_TextBox().value == "" || this.get_TextBox().value == null) {
        /*if (this.get_Obrigatorio()) {
        validator.innerHTML = this.get_MensagemErroObrigatorio();
        arg.IsValid = false;
        }*/
    }
    else {
        if (!this.validarCNPJ(this.get_TextBox().value)) {
            validator.innerHTML = this.get_MensagemErroFormato();
            validator.errormessage = this.get_MensagemErroFormatoSummary();
            IsValid = false;
        }
    }

    if (IsValid) {
        div.style.display = "none";
        this.get_Validador().style.visibility = "hidden"
    }
    else {
        this.get_Validador().style.visibility = "visible"
        div.style.display = "block";
    }

    return IsValid;
}

BNE.Componentes.ControlCNPJ.ValidarTextBox = function (sender, args) {
    var espGrid = RegExp("(_[0-9]+)$");
    var m = espGrid.exec(sender._id);

    var controle = null;


    if (m != null) {
        controle = $find(sender.id.replace("_cvValor_" + m[0], ""));
    }
    else {
        controle = $find(sender.id.replace("_cvValor", ""));
    }

    if (controle != null)
        controle.Validar(args);    
} 