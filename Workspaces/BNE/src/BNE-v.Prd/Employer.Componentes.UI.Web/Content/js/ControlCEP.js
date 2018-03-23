// Declare a namespace.
Type.registerNamespace("Employer.Componentes.UI.Web");

// Define a simplified component.
Employer.Componentes.UI.Web.ControlCEP = function (element) {
    Employer.Componentes.UI.Web.ControlCEP.initializeBase(this, [element]);

    this._id = element.id;

    // Handlers for the events    
    this._onBlur = null;
    this._onFocus = null;
    this._onKeyPress = null;
    this._textoAntesFocus = null;
    this._doPostBack = false;

    // Compatibilidade com ControlBaseValidator
    this._mensagemErroValorMinimo = null;
    this._mensagemErroValorMaximo = null;

    // Summary
    this._MensagemErroObrigatorioSummary = null;
    this._MensagemErroFormatoSummary = null;
    this._MensagemErroValorMinimoSummary = null;
    this._MensagemErroValorMaximoSummary = null;

    this.get_DoPostBack = function () {
        return this._doPostBack;
    },
    this.set_DoPostBack = function (valor) {
        this._doPostBack = valor;
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
         Employer.Componentes.UI.Web.ControlCEP.callBaseMethod(this, 'initialize');

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

        Employer.Componentes.UI.Web.ControlCEP.callBaseMethod(this, 'dispose');
    },
    this.get_TextoAntesFocus = function () {
        return this._textoAntesFocus;
    },
    this.aplicarMascaraCEP = function () {
        var valor = this.trim(this.get_TextBox().value);
        if (valor == '')
            return;

        if (valor.match(/^(\d{2})(\d{3})(\d{3})$/))
            valor = valor.replace(/(\d{2})(\d{3})(\d{3})/, "$1.$2-$3");

        this.get_TextBox().value = valor;
    },
    this.limparCEP = function (CEP) {
        var valor = new String(CEP);
        valor = valor.replace(".", "");
        valor = valor.replace(".", "");
        valor = valor.replace("/", "");
        valor = valor.replace("-", "");
        return this.trim(valor);
    },

    this.validarCEP = function (cep) {
        cep = this.limparCEP(cep);
        if (cep.length < 8)
            return false;
        return true;
    }
}

//Simula herança
Employer.Componentes.UI.Web.ControlCEP.prototype = new Employer.Componentes.UI.Web.ControlBaseValidator;

Employer.Componentes.UI.Web.ControlCEP.prototype.Focus = function () {

    this._textoAntesFocus = this.get_TextBox().value;

    this.get_TextBox().value = this.limparCEP(this.get_TextBox().value);
    try {
        this.get_TextBox().select();
    } catch (e) {
    }
}

Employer.Componentes.UI.Web.ControlCEP.prototype.Blur = function () {
    // Aplica a mascara do CEP
    this.aplicarMascaraCEP();

    //Foi Alterado valor
    this.PostPadrao();
}

Employer.Componentes.UI.Web.ControlCEP.prototype.KeyPress = function (e) {

    var key = AjaxClientControlBase.Key.GetKeyCode(e.rawEvent);

    if (!(key == 46 && Sys.Browser.name.indexOf("Internet Explorer") > -1)) {
        if (e.rawEvent.keyCode && AjaxClientControlBase.Key.isCommand(key))
            return true;
    }

    var caracter = String.fromCharCode(key);

    if (isNaN(caracter) || (this.limparCEP(this.get_TextBox().value).length >= 8 && !this.has_selected_text(this.get_TextBox())))
        return AjaxClientControlBase.CancelEvent(e);

}

Employer.Componentes.UI.Web.ControlCEP.registerClass('Employer.Componentes.UI.Web.ControlCEP', Sys.UI.Behavior);

/*if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();*/

Employer.Componentes.UI.Web.ControlCEP.prototype.Validar = function (arg) {
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
        if (!this.validarCEP(arg.Value)) {
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

Employer.Componentes.UI.Web.ControlCEP.prototype.MostrarErros = function () {

    var validator = this.get_Validador();
    var div = this.get_PnlValidador();

    IsValid = true;
    IsValid = true;
    if (this.get_TextBox().value == "" || this.get_TextBox().value == null) {
        /*if (this.get_Obrigatorio()) {
        validator.innerHTML = this.get_MensagemErroObrigatorio();
        IsValid = false;
        }*/
    }
    else {
        if (!this.validarCEP(this.get_TextBox().value)) {
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

Employer.Componentes.UI.Web.ControlCEP.ValidarTextBox = function (sender, args) {
    var espGrid = RegExp("(_[0-9]+)$");
    var m = espGrid.exec(sender.id);

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