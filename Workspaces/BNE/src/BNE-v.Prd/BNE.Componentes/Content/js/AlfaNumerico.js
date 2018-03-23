// Declare a namespace.
Type.registerNamespace("BNE.Componentes.UI.Web");

// Define a simplified component.
BNE.Componentes.AlfaNumerico = function (element) {
    BNE.Componentes.AlfaNumerico.initializeBase(this, [element]);

    this._id = element.id;

    // Handlers for the events    
    this._onBlur = null;
    this._onFocus = null;
    this._textoAntesFocus = "";
    this._onKeyPress = null;
    this._onKeyUp = null;
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
    this._valorMaximo = null;
    this._valorMinimo = null;

    this._somenteNumeros = null;

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
         BNE.Componentes.AlfaNumerico.callBaseMethod(this, 'initialize');

         // Wireup the event handlers
         var element = this.get_TextBox();
         if (element) {
             this._onBlur = Function.createDelegate(this, this.Blur);
             $addHandler(element, 'blur', this._onBlur);

             this._onKeyPress = Function.createDelegate(this, this.KeyPress);
             $addHandler(element, 'keypress', this._onKeyPress);

             this._onFocus = Function.createDelegate(this, this.Focus);
             $addHandler(element, 'focus', this._onFocus);

             this._onKeyUp = Function.createDelegate(this, this.KeyUp);
             $addHandler(element, 'keyup', this._onKeyUp);

         }
     },
    this.dispose = function () {
        $clearHandlers(this.get_TextBox());

        BNE.Componentes.AlfaNumerico.callBaseMethod(this, 'dispose');
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
    };
};
//Simula herança
BNE.Componentes.AlfaNumerico.prototype = new BNE.Componentes.ControlBaseValidator;

BNE.Componentes.AlfaNumerico.prototype.Blur = function () {
    this.get_TextBox().value = AjaxClientControlBase.Trim(this.get_TextBox().value);

    this.PostPadrao();
};
BNE.Componentes.AlfaNumerico.prototype.Focus = function () {
    this._textoAntesFocus = this.get_TextBox().value;
};
BNE.Componentes.AlfaNumerico.prototype.KeyPress = function (e) {
    var key = AjaxClientControlBase.Key.GetKeyCode(e.rawEvent);

    if (e.rawEvent.keyCode && AjaxClientControlBase.Key.isCommand(key))
        return true;

    var caracter = String.fromCharCode(key);


    if (this.get_SomenteNumeros() == true) {
        //É letra
        if (isNaN(caracter))
            return AjaxClientControlBase.CancelEvent(e);
    }

    if (this.get_TipoAlfanumerico() == 4) {
        if (!isNaN(caracter) && caracter != " ")
            return AjaxClientControlBase.CancelEvent(e);
    }

    return true;
};
BNE.Componentes.AlfaNumerico.prototype.KeyUp = function (e) {
    if (this.get_TipoAlfanumerico() == 1) {
        this.get_TextBox().value = this.get_TextBox().value.toUpperCase();
    }
    if (this.get_TipoAlfanumerico() == 2) {
        this.get_TextBox().value = this.get_TextBox().value.toLowerCase();
    }
};
BNE.Componentes.AlfaNumerico.registerClass('BNE.Componentes.AlfaNumerico', Sys.UI.Behavior);

/*if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();*/


BNE.Componentes.AlfaNumerico.prototype.ValidarExpressaoRegular = function () {

    if (this.get_ExpressaoRegular() == null || this.get_ExpressaoRegular() == "")
        return true;

    var pat = new RegExp(this.get_ExpressaoRegular());

    return pat.test(this.get_TextBox().value);
};
BNE.Componentes.AlfaNumerico.prototype.MostrarErros = function () {
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
        var mensagem = "";
        if (this.get_ValorMaximo() == "" && this.get_ValorMinimo() != "")
            mensagem = this.get_MensagemErroMinimo();
        else if (this.get_ValorMaximo() != "" && this.get_ValorMinimo() == "")
            mensagem = this.get_MensagemErroMaximo();
        else if (this.get_ValorMaximo() != "" && this.get_ValorMinimo() != "")
            mensagem = this.get_MensagemErroIntervalo();

        if (parseInt(this.get_TextBox().value) != NaN) {
            if (parseInt(this.get_ValorMaximo()) != NaN) {
                if (parseInt(this.get_TextBox().value) > parseInt(this.get_ValorMaximo()))
                    IsValid = false;
            }
            if (parseInt(this.get_ValorMinimo()) != NaN) {
                if (parseInt(this.get_TextBox().value) < parseInt(this.get_ValorMinimo()))
                    IsValid = false;
            }
            if (!IsValid) {
                validator.innerHTML = mensagem;
                validator.errormessage = mensagem;
            }
        }
    }

    if (IsValid) {
        div.style.display = "none";
        this.get_Validador().style.visibility = "hidden";
    }
    else {
        this.get_Validador().style.visibility = "visible";
        div.style.display = "block";
    }
    return IsValid;
};
BNE.Componentes.AlfaNumerico.prototype.Validar = function (arg) {
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
        var mensagem = "";
        if (this.get_ValorMaximo() == "" && this.get_ValorMinimo() != "")
            mensagem = this.get_MensagemErroMinimo();
        else if (this.get_ValorMaximo() != "" && this.get_ValorMinimo() == "")
            mensagem = this.get_MensagemErroMaximo();
        else if (this.get_ValorMaximo() != "" && this.get_ValorMinimo() != "")
            mensagem = this.get_MensagemErroIntervalo();

        if (parseInt(arg.Value) != NaN) {
            if (parseInt(this.get_ValorMaximo()) != NaN) {
                if (parseInt(arg.Value) > parseInt(this.get_ValorMaximo()))
                    arg.IsValid = false;
            }
            if (parseInt(this.get_ValorMinimo()) != NaN) {
                if (parseInt(arg.Value) < parseInt(this.get_ValorMinimo()))
                    arg.IsValid = false;
            }
            if (!arg.IsValid) {
                validator.innerHTML = mensagem;
                validator.errormessage = mensagem;
            }
        }
    }

    if (arg.IsValid) {
        div.style.display = "none";
        this.get_Validador().style.visibility = "hidden";
    }
    else {
        this.get_Validador().style.visibility = "visible";
        div.style.display = "block";
    }
};
BNE.Componentes.AlfaNumerico.ValidarTextBox = function (sender, args) {
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
};