// Declare a namespace.
Type.registerNamespace("Employer.Componentes.UI.Web");

// Define a simplified component.
Employer.Componentes.UI.Web.ValorNumerico = function (element) {
    Employer.Componentes.UI.Web.ValorNumerico.initializeBase(this, [element]);

    this._id = element.id;

    this._ValorMinimo = null;
    this._CampoValorMinimo = null;
    this._ValorMaximo = null;
    this._CampoValorMaximo = null;
    this._CasasDecimais = null;
    this._Tamanho = null;

    //Controles internos
    this._TextoAntesKeyPress = null;
    this._TextoAntesFocus = null;


    //Globalização
    this._NumberDecimalSeparator = null;
    this._NumberGroupSeparator = null;
    this._CurrencySymbol = null;
    this._NegativeSign = null;
    this._NumberGroupSizes = null;

    // Summary
    this._MensagemErroObrigatorioSummary = null;
    this._MensagemErroFormatoSummary = null;
    this._MensagemErroValorMinimoSummary = null;
    this._MensagemErroValorMaximoSummary = null;

    // Handlers for the events
    this._onKeyPress = null;
    this._onKeyUp = null;
    this._onBlur = null;
    this._onFocus = null;
    this._onChange = null;

    this.get_TextBox = function () {
        var espGrid = RegExp("(_[0-9]+)$");
        var m = espGrid.exec(this._id);

        if (m != null)
            return $get(this._id + "_txtValor" + m[0]);
        else
            return $get(this._id + "_txtValor");
    },
    this.initialize = function () {
        Employer.Componentes.UI.Web.ValorNumerico.callBaseMethod(this, 'initialize');

        // Wireup the event handlers
        var element = this.get_TextBox();
        if (element) {

            this._onKeyPress = Function.createDelegate(this, this.KeyPress);
            $addHandler(element, 'keypress', this._onKeyPress);

            this._onKeyUp = Function.createDelegate(this, this.KeyUp);
            $addHandler(element, 'keyup', this._onKeyUp);

            this._onBlur = Function.createDelegate(this, this.Blur);
            $addHandler(element, 'blur', this._onBlur);

            this._onFocus = Function.createDelegate(this, this.RemoverMascaraControleDecimal);
            $addHandler(element, 'focus', this._onFocus);
        }
    },
    this.dispose = function () {
        $clearHandlers(this.get_TextBox());

        Employer.Componentes.UI.Web.ValorNumerico.callBaseMethod(this, 'dispose');
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
    },
    this.get_CasasDecimais = function () {
        return this._CasasDecimais;
    },
    this.get_Tamanho = function () {
        return this._Tamanho;
    },
    this.get_CurrencySymbol = function () {
        return this._CurrencySymbol;
    },
    this.get_NumberDecimalSeparator = function () {
        return this._NumberDecimalSeparator;
    },
    this.get_NumberGroupSeparator = function () {
        return this._NumberGroupSeparator;
    },
    this.get_NegativeSign = function () {
        return this._NegativeSign;
    },
    this.get_NumberGroupSizes = function () {
        return this._NumberGroupSizes;
    },
    this.get_TextoAntesFocus = function () {
        if (this._TextoAntesFocus == null)
            return this.get_TextBox().value;
        return this._TextoAntesFocus;
    },
    this.get_CampoValorMinimo = function () {
        return this._CampoValorMinimo;
    },
    this.get_CampoValorMaximo = function () {
        return this._CampoValorMaximo;
    },


    this.set_ValorMinimo = function (value) {
        this._ValorMinimo = value;
    },
    this.set_ValorMaximo = function (value) {
        this._ValorMaximo = value;
    },
    this.set_CurrencySymbol = function (value) {
        this._CurrencySymbol = value;
    },
    this.set_NumberDecimalSeparator = function (value) {
        this._NumberDecimalSeparator = value;
    },
    this.set_NumberGroupSeparator = function (value) {
        this._NumberGroupSeparator = value;
    },
    this.set_NegativeSign = function (value) {
        this._NegativeSign = value;
    },
    this.set_NumberGroupSizes = function (value) {
        this._NumberGroupSizes = value;
    },
    this.set_CasasDecimais = function (value) {
        this._CasasDecimais = value;
    },
    this.set_Tamanho = function (value) {
        this._Tamanho = value;
    },
    this.set_CampoValorMinimo = function (value) {
        this._CampoValorMinimo = value;
    },
    this.set_CampoValorMaximo = function (value) {
        this._CampoValorMaximo = value;
    }
}

//Simula herança
Employer.Componentes.UI.Web.ValorNumerico.prototype = new Employer.Componentes.UI.Web.ControlBaseValidator;

// Create protytype.
Employer.Componentes.UI.Web.ValorNumerico.prototype.GetDecimal = function (value) {
    var valor = value;
    if (valor == null || valor == "")
        return 0.0;

    var rxGroupSeparator = new RegExp("\\" + this.get_NumberGroupSeparator() + "+");
    var rxDecimalSeparator = new RegExp("\\" + this.get_NumberDecimalSeparator() + "+");

    //converte p/ string
    valor = "" + valor;
    var sValor = valor.replace(rxGroupSeparator, "").replace(rxDecimalSeparator, ".");
    return parseFloat(sValor);
}
Employer.Componentes.UI.Web.ValorNumerico.prototype.GetDecimalValue = function () {
    return this.GetDecimal(this.get_TextBox().value);
}
Employer.Componentes.UI.Web.ValorNumerico.prototype.FotmatDecimalValue = function (sValor) {
    if (sValor == '')
        return;

    var casasDecimais = this.get_CasasDecimais();


    var p = sValor.indexOf(".");
    var sNValor = "";
    if (p > -1)
        sNValor = this.get_NumberDecimalSeparator() + AjaxClientControlBase.padRight(sValor.substring(p + 1), '0', casasDecimais);
    else if (casasDecimais > 0)
        sNValor = this.get_NumberDecimalSeparator() + AjaxClientControlBase.padRight('', '0', casasDecimais);
    else
        p = sValor.length;

    var sInteiro = sValor.substring(0, p);
    var pt = 0;
    for (var i = sInteiro.length - 1; i >= 0; i--, pt++) {
        if (sInteiro.charAt(i) == this.get_NegativeSign())
            pt--;
        if (pt == 3) {
            pt = 0;
            sNValor = this.get_NumberGroupSeparator() + sNValor;
        }
        sNValor = sInteiro.charAt(i) + sNValor;
    }
    return sNValor;
}


Employer.Componentes.UI.Web.ValorNumerico.prototype.FotmatDecimal = function () {
    if (this.get_TextBox().value == '')
        return;

    var sValor = this.GetDecimalValue().toFixed(this.get_CasasDecimais()).toString();

    this.get_TextBox().value = this.FotmatDecimalValue(sValor);
}

Employer.Componentes.UI.Web.ValorNumerico.prototype.RemoveZerosNaoUsado = function (sValor) {
    var v = new String();
    var srx = "/^0+(.+)/";
    var rx = RegExp(srx);
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
Employer.Componentes.UI.Web.ValorNumerico.prototype.ValidarValorDecimal = function (valor) {
    try {
        //Converte p/ string
        valor = "" + valor;
        var rxGroupSeparator = new RegExp("\\" + this.get_NumberGroupSeparator() + "+");
        var rxDecimalSeparator = new RegExp("\\" + this.get_NumberDecimalSeparator() + "+");

        var v = valor.replace(rxGroupSeparator, "").replace(rxDecimalSeparator, ".");
        var valorD = parseFloat(v);
        var sValorD = valorD.toString().replace(rxDecimalSeparator, ".");

        return (!((valorD != 0 && !valorD) || this.RemoveZerosNaoUsado(v) != sValorD));
    }
    catch (e) {
        return false;
    }
}
Employer.Componentes.UI.Web.ValorNumerico.prototype.get_ValorMinimo = function () {
    if (this.get_CampoValorMinimo() != null) {
        var sValue = $get(this.get_CampoValorMinimo()).value;
        if (this.ValidarValorDecimal(sValue))
            return this.GetDecimal(sValue);
    }

    return this._ValorMinimo;
}
Employer.Componentes.UI.Web.ValorNumerico.prototype.get_MensagemErroValorMinimo = function () {
    if (this.get_ValorMinimo() != null && this._MensagemErroValorMinimo != null) {
        return this._MensagemErroValorMinimo.replace("{0}",
            this.FotmatDecimalValue(this.get_ValorMinimo().toFixed(this.get_CasasDecimais()).toString())
        );
    }
    return this._MensagemErroValorMinimo;
}
Employer.Componentes.UI.Web.ValorNumerico.prototype.ValidarIntervaloMinimo = function () {
    if (this.get_ValorMinimo() == null)
        return true;

    return this.GetDecimalValue() >= this.get_ValorMinimo();
}
Employer.Componentes.UI.Web.ValorNumerico.prototype.get_ValorMaximo = function () {
    if (this.get_CampoValorMaximo() != null) {
        var sValue = $get(this.get_CampoValorMaximo()).value;
        if (this.ValidarValorDecimal(sValue))
            return this.GetDecimal(sValue);
    }

    return this._ValorMaximo;
}
Employer.Componentes.UI.Web.ValorNumerico.prototype.get_MensagemErroValorMaximo = function () {
    if (this.get_ValorMaximo() != null && this._MensagemErroValorMaximo != null) {
        return this._MensagemErroValorMaximo.replace("{0}",
            this.FotmatDecimalValue(this.get_ValorMaximo().toFixed(this.get_CasasDecimais()).toString())
        );
    }
    return this._MensagemErroValorMaximo;
}
Employer.Componentes.UI.Web.ValorNumerico.prototype.ValidarIntervaloMaximo = function () {
    if (this.get_ValorMaximo() == null)
        return true;

    return this.GetDecimalValue() <= this.get_ValorMaximo();
}
Employer.Componentes.UI.Web.ValorNumerico.prototype.Validar = function (arg) {
    var validator = this.get_Validator();
    var div = this.get_PainelValidador();

    arg.IsValid = false;
    if (arg.Value == "") {
        if (this.get_Obrigatorio() && (this.get_TextoAntesFocus() == null || this.get_TextBox().value == this.get_TextoAntesFocus())) {
            validator.innerHTML = this.get_MensagemErroObrigatorio();
            validator.errormessage = this.get_MensagemErroObrigatorioSummary();
        }
        else
            arg.IsValid = true;
    }
    else if (this.ValidarValorDecimal(arg.Value)) {
        if (this.ValidarIntervaloMinimo() == false) {
            validator.innerHTML = this.get_MensagemErroValorMinimo();
            validator.errormessage = this.get_MensagemErroValorMinimoSummary();
        }
        else if (this.ValidarIntervaloMaximo() == false) {
            validator.innerHTML = this.get_MensagemErroValorMaximo();
            validator.errormessage = this.get_MensagemErroValorMaximoSummary();
        }
        else if ((this.get_Tamanho() != null && this.get_Tamanho() > 0) && this.get_TextBox().value.length > this.get_Tamanho()) {
            validator.innerHTML = this.get_MensagemErroFormato();
            validator.errormessage = this.get_MensagemErroFormatoSummary();
        }
        else
            arg.IsValid = true;
    }
    else {
        if (AjaxClientControlBase.Trim(this.get_TextBox().value) != "") {
            validator.innerHTML = this.get_MensagemErroFormato();
            validator.errormessage = this.get_MensagemErroFormatoSummary();
        } else {
            arg.IsValid = true;
        }
    }

    if (arg.IsValid) {
        this.get_Validator().style.display = "none";
        div.style.display = "none";
    }
    else {
        this.get_Validator().style.display = "block";
        div.style.display = "block";
    }

}
Employer.Componentes.UI.Web.ValorNumerico.prototype.MostrarErros = function () {
    var validator = this.get_Validator();
    var div = this.get_PainelValidador();
    
    var IsValid = false;
    if (this.ValidarValorDecimal(this.get_TextBox().value)) {
        if (this.ValidarIntervaloMinimo() == false) {
            validator.innerHTML = this.get_MensagemErroValorMinimo();
            validator.errormessage = this.get_MensagemErroValorMinimoSummary();
        }
        else if (this.ValidarIntervaloMaximo() == false) {
            validator.innerHTML = this.get_MensagemErroValorMaximo();
            validator.errormessage = this.get_MensagemErroValorMaximoSummary();
        }
        else if ((this.get_Tamanho() != null && this.get_Tamanho() > 0) && this.get_TextBox().value.length > this.get_Tamanho()) {
            validator.innerHTML = this.get_MensagemErroFormato();
            validator.errormessage = this.get_MensagemErroFormatoSummary();
        }
        else
            IsValid = true;
    }
    else {
        if (AjaxClientControlBase.Trim(this.get_TextBox().value) != "") {
            validator.innerHTML = this.get_MensagemErroFormato();
            validator.errormessage = this.get_MensagemErroFormatoSummary();
        } else {
            IsValid = true;
        }
    }

    if (IsValid) {
        div.style.display = "none";
        this.get_Validator().style.display = "none";
    }
    else {
        div.style.display = "block";
        this.get_Validator().style.display = "block";
    }

    return IsValid;
}
Employer.Componentes.UI.Web.ValorNumerico.prototype.ValidarBlur = function () {
    var val = this.get_Validator();

    return this.MostrarErros();

    return val.isvalid;
}
Employer.Componentes.UI.Web.ValorNumerico.prototype.RemoverMascaraControleDecimal = function () {
    this._TextoAntesFocus = this.get_TextBox().value;

    var valor = AjaxClientControlBase.Trim(this.get_TextBox().value);

    var rx = new RegExp("\\" + this.get_NumberGroupSeparator() + "+");
    if (this.ValidarValorDecimal(valor)) {
        valor = valor.replace(rx, '');
        arr = valor.split(this.get_NumberDecimalSeparator());
        if (arr.length == this.get_CasasDecimais()) {
            if (parseInt(arr[1]) == 0)
                valor = arr[0];
        }
    }
    this.get_TextBox().value = valor;
    this.get_TextBox().select();
}
Employer.Componentes.UI.Web.ValorNumerico.prototype.KeyPress = function (e) {
    this._TextoAntesKeyPress = this.get_TextBox().value;

    var key = AjaxClientControlBase.Key.GetKeyCode(e.rawEvent);

    if (e.rawEvent.keyCode && AjaxClientControlBase.Key.isCommand(key))
        return true;

    if (key == Sys.UI.Key.space) {
        e.preventDefault();
        return false;
    }

    var caracter = String.fromCharCode(key);

    if (caracter == this.get_NumberDecimalSeparator() &&
        this.get_TextBox().value.indexOf(this.get_NumberDecimalSeparator()) > -1)
        return AjaxClientControlBase.CancelEvent(e);

    if (caracter == this.get_NegativeSign() &&
        this.get_TextBox().value.indexOf(this.get_NegativeSign()) > -1)
        return AjaxClientControlBase.CancelEvent(e);

    if (caracter == this.get_NumberDecimalSeparator() ||
        caracter == this.get_NegativeSign())
        return;

    //É letra
    if (isNaN(caracter))
        return AjaxClientControlBase.CancelEvent(e);
}
Employer.Componentes.UI.Web.ValorNumerico.prototype.KeyUp = function (e) {
    var valor = this.get_TextBox().value;
    if (valor == "" || valor == this.get_NegativeSign())
        return;

    if (this.ValidarValorDecimal(valor) == false)
        this.get_TextBox().value = this._TextoAntesKeyPress;


    AjaxClientControlBase.NextFocul(e.rawEvent, this.get_TextBox());
}
Employer.Componentes.UI.Web.ValorNumerico.prototype.Blur = function () {
    this.FotmatDecimal();

    this.PostPadrao();
}
// End of prototype definition.

// Optional descriptor for JSON serialization.
Employer.Componentes.UI.Web.ValorNumerico.descriptor = {
    properties: [
                { name: 'Obrigatorio', type: Boolean },
                { name: 'ValorMinimo', type: Number },
                { name: 'ValorMaximo', type: Number },
                { name: 'MensagemErroObrigatorio', type: String },
                { name: 'MensagemErroFormato', type: String },
                { name: 'MensagemErroValorMinimo', type: String },
                { name: 'MensagemErroValorMaximo', type: String }
                ]
}


// Register the class as a type that inherits from Sys.UI.Control.
Employer.Componentes.UI.Web.ValorNumerico.registerClass('Employer.Componentes.UI.Web.ValorNumerico', Sys.UI.Behavior);

/*if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();*/


Employer.Componentes.UI.Web.ValorNumerico.ValidarTextBox = function (sender, args) {

    var i = sender.id.indexOf("_cvValor");

    var controle = $find(sender.id.substring(0, i));

    if (controle == null)
        return;

    controle.Validar(args);
}