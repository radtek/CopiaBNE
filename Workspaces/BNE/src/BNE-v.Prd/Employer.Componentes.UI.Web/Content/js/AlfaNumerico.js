// Declare a namespace.
Type.registerNamespace("Employer.Componentes.UI.Web");

// Define a simplified component.
Employer.Componentes.UI.Web.AlfaNumerico = function (element) {
    Employer.Componentes.UI.Web.AlfaNumerico.initializeBase(this, [element]);

    this._id = element.id;

    // Handlers for the events    
    this._onBlur = null;
    this._onFocus = null;
    this._textoAntesFocus = "";
    this._onKeyPress = null;
    this._expressaoRegular = null;
    this._tipoAlfanumerico = 0;

    // Compatibilidade com ControlBaseValidator
    this._mensagemErroValorMinimo = null;
    this._mensagemErroValorMaximo = null;
    this._mensagemErroFormato = null;

    // Summary
    this._MensagemErroObrigatorioSummary = null;
    this._MensagemErroFormatoSummary = null;
    this._MensagemErroValorMinimoSummary = null;
    this._MensagemErroValorMaximoSummary = null;

    //Range Validator
    this._mensagemErroMinimo = null;
    this._mensagemErroMaximo = null;
    this._mensagemErroIntervalo = null;

    this._mensagemErroMinimoSummary = null;
    this._mensagemErroMaximoSummary = null;
    this._mensagemErroIntervaloSummary = null;

    this._valorMaximo = null;
    this._valorMinimo = null;

    this._somenteNumeros = null;

    this.get_MensagemErroMinimoSummary = function () {
        return this._mensagemErroMinimoSummary;
    },
    this.set_MensagemErroMinimoSummary = function (valor) {
        this._mensagemErroMinimoSummary = valor;
    },

    this.get_MensagemErroMaximoSummary = function () {
        return this._mensagemErroMaximoSummary;
    },
    this.set_MensagemErroMaximoSummary = function (valor) {
        this._mensagemErroMaximoSummary = valor;
    },

    this.get_MensagemErroIntervaloSummary = function () {
        return this._mensagemErroIntervaloSummary;
    },
    this.set_MensagemErroIntervaloSummary = function (valor) {
        this._mensagemErroIntervaloSummary = valor;
    },

    this.get_TipoAlfanumerico = function () {
        return this._tipoAlfanumerico;
    },
    this.set_TipoAlfanumerico = function (valor) {
        this._tipoAlfanumerico = valor;
    },

    this.get_MensagemErroFormato = function () {
        return this._mensagemErroFormato;
    },
    this.set_MensagemErroFormato = function (valor) {
        this._mensagemErroFormato = valor;
    },
    this.get_MensagemErroIntervalo = function () {
        return this._mensagemErroIntervalo;
    },
    this.set_MensagemErroIntervalo = function (valor) {
        this._mensagemErroIntervalo = valor;
    },
    this.get_MensagemErroMaximo = function () {
        return this._mensagemErroMaximo;
    },
    this.set_MensagemErroMaximo = function (valor) {
        this._mensagemErroMaximo = valor;
    },
    this.get_MensagemErroMinimo = function () {
        return this._mensagemErroMinimo;
    },
    this.set_MensagemErroMinimo = function (valor) {
        this._mensagemErroMinimo = valor;
    },
    this.get_ExpressaoRegular = function () {
        return this._expressaoRegular;
    },
    this.set_ExpressaoRegular = function (valor) {
        this._expressaoRegular = valor;
    },        
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
    this.set_SomenteNumeros = function (valor) {
        this._somenteNumeros = valor;
    },
    this.get_SomenteNumeros = function () {
        return this._somenteNumeros;
    },
    this.get_ValorMinimo = function () {
        return this._valorMinimo;
    },
    this.set_ValorMinimo = function (valor) {
        this._valorMinimo = valor;
    },
    this.get_ValorMaximo = function () {
        return this._valorMaximo;
    },
    this.set_ValorMaximo = function (valor) {
        this._valorMaximo = valor;
    },
    this.set_SomenteNumeros = function (valor) {
        this._somenteNumeros = valor;
    },
    this.get_SomenteNumeros = function () {
        return this._somenteNumeros;
    },
    //Fim compatibilidade com ControlBaseValidator
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
         Employer.Componentes.UI.Web.AlfaNumerico.callBaseMethod(this, 'initialize');

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

        Employer.Componentes.UI.Web.AlfaNumerico.callBaseMethod(this, 'dispose');
    },
    this.get_TextoAntesFocus = function () {
        return this._textoAntesFocus;
    },

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
    }
}

//Simula herança
Employer.Componentes.UI.Web.AlfaNumerico.prototype = new Employer.Componentes.UI.Web.ControlBaseValidator;

Employer.Componentes.UI.Web.AlfaNumerico.prototype.Blur = function () {
    this.get_TextBox().value = AjaxClientControlBase.Trim(this.get_TextBox().value);

    this.PostPadrao();
}

Employer.Componentes.UI.Web.AlfaNumerico.prototype.Focus = function () {
    this._textoAntesFocus = this.get_TextBox().value;
}

Employer.Componentes.UI.Web.AlfaNumerico.prototype.KeyPress = function (e) {
    var key = AjaxClientControlBase.Key.GetKeyCode(e.rawEvent);

    if (e.rawEvent.keyCode && AjaxClientControlBase.Key.isCommand(key))
        return true;

    var caracter = String.fromCharCode(key);

    var campo = this.get_TextBox();
    var tamMax = campo.getAttribute("maxlength");
    var tam = campo.value.length;

    if (tam != tamMax) {
        //return AjaxClientControlBase.CancelEvent(e);

        if (this.get_SomenteNumeros() == true) {
            //É letra
            if (isNaN(caracter))
                return AjaxClientControlBase.CancelEvent(e);
        }

        if (this.get_TipoAlfanumerico() == 4) {
            if (!isNaN(caracter) && caracter != " ")
                return AjaxClientControlBase.CancelEvent(e);
        }
        //LetraMaiuscula
        else if (this.get_TipoAlfanumerico() == 1) {
            if (caracter != caracter.toUpperCase()) {
                var sel = AjaxClientControlBase.GetInputSelection(campo), val = campo.value;
                campo.value = val.slice(0, sel.start) + caracter.toUpperCase() + val.slice(sel.end);
                AjaxClientControlBase.SetInputSelection(campo, sel.start + 1, sel.start + 1);
                return AjaxClientControlBase.CancelEvent(e);
            }
        }
        //LetraMinuscula
        else if (this.get_TipoAlfanumerico() == 2) {
            if (caracter != caracter.toLowerCase()) {
                var sel = AjaxClientControlBase.GetInputSelection(campo), val = campo.value;
                campo.value = val.slice(0, sel.start) + caracter.toLowerCase() + val.slice(sel.end);
                AjaxClientControlBase.SetInputSelection(campo, sel.start + 1, sel.start + 1);
                return AjaxClientControlBase.CancelEvent(e);
            }
        }
    }

    return true;
}

Employer.Componentes.UI.Web.AlfaNumerico.registerClass('Employer.Componentes.UI.Web.AlfaNumerico', Sys.UI.Behavior);

/*if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();*/


Employer.Componentes.UI.Web.AlfaNumerico.prototype.ValidarExpressaoRegular = function () {

    if (AjaxClientControlBase.IsNullOrEmpty(this.get_ExpressaoRegular()))
        return true;

    var value = this.get_TextBox().value;
    var rx = new RegExp(this.get_ExpressaoRegular());

    var matches = rx.exec(value);
    return (matches != null && value == matches[0]);

    //return pat.test(this.get_TextBox().value);
}

Employer.Componentes.UI.Web.AlfaNumerico.prototype.MostrarErrosTipo3 = function () {
    var validator = this.get_Validador();

    var mensagem = "";
    var mensagemSummary = "";

    var IsValid = true;
    if (AjaxClientControlBase.IsNullOrEmpty(this.get_ValorMaximo()) && !AjaxClientControlBase.IsNullOrEmpty(this.get_ValorMinimo())) {
        mensagem = this.get_MensagemErroMinimo();
        mensagemSummary = this.get_MensagemErroMinimoSummary();
    }
    else if (!AjaxClientControlBase.IsNullOrEmpty(this.get_ValorMaximo()) && AjaxClientControlBase.IsNullOrEmpty(this.get_ValorMinimo())) {
        mensagem = this.get_MensagemErroMaximo();
        mensagemSummary = this.get_MensagemErroMaximoSummary();
    }
    else if (!AjaxClientControlBase.IsNullOrEmpty(this.get_ValorMaximo()) && !AjaxClientControlBase.IsNullOrEmpty(this.get_ValorMinimo())) {
        mensagem = this.get_MensagemErroIntervalo();
        mensagemSummary = this.get_MensagemErroIntervaloSummary();
    }

    var vInt = parseInt(this.get_TextBox().value);

    if (vInt != NaN) {

        var vMax = parseInt(this.get_ValorMaximo());
        if (vMax != NaN && vInt > vMax) {
            IsValid = false;
        }

        var vMin = parseInt(this.get_ValorMinimo());
        if (vMin != NaN && vInt < vMin) {
            IsValid = false;
        }

        if (!IsValid) {
            validator.innerHTML = mensagem;
            validator.errormessage = mensagemSummary;
        }
    }

    return IsValid;
}

Employer.Componentes.UI.Web.AlfaNumerico.prototype.MostrarErros = function () {
    var validator = this.get_Validador();
    var div = this.get_PnlValidador();

    var IsValid = true;
    if (this.get_TextBox().value == "" || this.get_TextBox().value == null) {
        /*if (this.get_Obrigatorio()) {
        validator.innerHTML = this.get_MensagemErroObrigatorio();
        IsValid = false;
        }*/
    }
    else {
        IsValid = this.ValidarExpressaoRegular();
        if (IsValid == false) {
            validator.innerHTML = this.get_MensagemErroFormato();
            validator.errormessage = this.get_MensagemErroFormatoSummary();
        }
    }

    //Validador Range
    if (IsValid && this.get_TipoAlfanumerico() == 3) {
        IsValid = this.MostrarErrosTipo3();
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

Employer.Componentes.UI.Web.AlfaNumerico.prototype.Validar = function (arg) {
    this.get_TextBox().value = AjaxClientControlBase.Trim(this.get_TextBox().value);
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
        arg.IsValid = this.ValidarExpressaoRegular();
        if (arg.IsValid == false) {
            validator.innerHTML = this.get_MensagemErroFormato();
            validator.errormessage = this.get_MensagemErroFormatoSummary();
        }
    }


    //Validador Range
    if (arg.IsValid && this.get_TipoAlfanumerico() == 3) {
        arg.IsValid = this.MostrarErrosTipo3();
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

Employer.Componentes.UI.Web.AlfaNumerico.ValidarTextBox = function (sender, args) {
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