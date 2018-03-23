// Declare a namespace.
Type.registerNamespace("Employer.Componentes.UI.Web");

// Define a simplified component.
Employer.Componentes.UI.Web.ControlCPF = function (element) {
    Employer.Componentes.UI.Web.ControlCPF.initializeBase(this, [element]);

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
         Employer.Componentes.UI.Web.ControlCPF.callBaseMethod(this, 'initialize');

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

        Employer.Componentes.UI.Web.ControlCPF.callBaseMethod(this, 'dispose');
    },
    this.get_TextoAntesFocus = function () {
        return this._textoAntesFocus;
    },
    this.aplicarMascaraCPF = function () {
        var valor = this.trim(this.get_TextBox().value);
        var tam = valor.length;
        if (valor == '')
            return;

        if (tam >= 12)
            valor = valor.substr(0, 11);

        if (tam != 11) {
            for (contador = 0; contador < 11 - tam; contador++) {
                valor = '0' + valor;
            }
        }

        tam = valor.length;


        if (tam > 3 && tam < 7)
            this.get_TextBox().value = (valor.substr(0, 3) + '.' + valor.substr(3, tam));
        if (tam >= 7 && tam < 10)
            this.get_TextBox().value = (valor.substr(0, 3) + '.' + valor.substr(3, 3) + '.' + valor.substr(6, tam - 6));
        if (tam >= 10 && tam < 12)
            this.get_TextBox().value = (valor.substr(0, 3) + '.' + valor.substr(3, 3) + '.' + valor.substr(6, 3) + '-' + valor.substr(9, tam - 9));
    },
    this.limparCPF = function (cpf) {
        var valor = new String(cpf);
        valor = valor.replace(".", "");
        valor = valor.replace(".", "");
        valor = valor.replace("/", "");
        valor = valor.replace("-", "");
        return this.trim(valor);
    },
    this.validarCPF = function (cpf) {
        cpf = this.limparCPF(cpf);
        var numeros, digitos, soma, i, resultado, digitos_iguais;
        digitos_iguais = 1;
        if (cpf.length < 11)
            return false;
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
                return false;
            numeros = cpf.substring(0, 10);
            soma = 0;
            for (i = 11; i > 1; i--)
                soma += numeros.charAt(11 - i) * i;
            resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
            if (resultado != digitos.charAt(1))
                return false;
            return true;
        }
        else
            return false;
    }
}

//Simula herança
Employer.Componentes.UI.Web.ControlCPF.prototype = new Employer.Componentes.UI.Web.ControlBaseValidator;

Employer.Componentes.UI.Web.ControlCPF.prototype.Focus = function (ev) {
    ev.preventDefault();
    ev.stopPropagation();

    this._textoAntesFocus = this.get_TextBox().value;

    this.get_TextBox().value = this.limparCPF(this.get_TextBox().value);
    try {
        this.get_TextBox().select();
    } catch (e) {
    }
}

Employer.Componentes.UI.Web.ControlCPF.prototype.Blur = function (ev) {
    ev.preventDefault();
    ev.stopPropagation();

    // Aplica a mascara do CPF
    this.aplicarMascaraCPF();

    this.PostPadrao();

    var btnValidarReceita = this.get_Campo("_btnValidarReceita");
    var campoTexto = this.get_TextBox();

    if (btnValidarReceita != null && btnValidarReceita != undefined) {
        if (AjaxClientControlBase.IsNullOrEmpty(campoTexto.value))
            btnValidarReceita.style.display = "none";
        else
            btnValidarReceita.style.display = null;
    }
}

Employer.Componentes.UI.Web.ControlCPF.prototype.NextFocus = function () {
    if (typeof (Employer_Componentes_UI_Web_AutoTabIndex_NextFocus) != "undefined") {
        Employer_Componentes_UI_Web_AutoTabIndex_NextFocus();
    }
}

Employer.Componentes.UI.Web.ControlCPF.prototype.KeyPress = function (e) {

    var key = AjaxClientControlBase.Key.GetKeyCode(e.rawEvent);

    var caracter = String.fromCharCode(key);

    if (caracter == ".")
        return AjaxClientControlBase.CancelEvent(e);

    if (e.rawEvent.keyCode && AjaxClientControlBase.Key.isCommand(key))
        return true;

    //var caracter = String.fromCharCode(key);

    if (isNaN(caracter) || (this.limparCPF(this.get_TextBox().value).length >= 11 && !this.has_selected_text(this.get_TextBox())))
        return AjaxClientControlBase.CancelEvent(e);

    if (this.limparCPF(this.get_TextBox().value).length == 10) {
        var inst = this;
        window.setTimeout(function () {
            inst.NextFocus();
        }, 500);
    }
}

Employer.Componentes.UI.Web.ControlCPF.registerClass('Employer.Componentes.UI.Web.ControlCPF', Sys.UI.Behavior);

/*if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();*/

Employer.Componentes.UI.Web.ControlCPF.prototype.Validar = function (arg) {    
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
        if (!this.validarCPF(arg.Value)) {
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

Employer.Componentes.UI.Web.ControlCPF.prototype.MostrarErros = function () {

    var validator = this.get_Validador();
    var div = this.get_PnlValidador();

    IsValid = true;
    if (this.get_TextBox().value == "" || this.get_TextBox().value == null) {
        /*if (this.get_Obrigatorio()) {
        validator.innerHTML = this.get_MensagemErroObrigatorio();
        IsValid = false;
        }*/
    }
    else {
        if (!this.validarCPF(this.get_TextBox().value)) {
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


Employer.Componentes.UI.Web.ControlCPF.ValidarTextBox = function (sender, args) {    
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