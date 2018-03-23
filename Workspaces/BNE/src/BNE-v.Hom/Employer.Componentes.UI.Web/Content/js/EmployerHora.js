Type.registerNamespace("Employer.Componentes.UI.Web");

// Define a simplified component.
Employer.Componentes.UI.Web.EmployerHora = function (element) {
    Employer.Componentes.UI.Web.EmployerHora.initializeBase(this, [element]);

    this._id = element.id;
    this._ClientOnBlur;

    // Compatibilidade com ControlBaseValidator
    this._mensagemErroValorMinimo = null;
    this._mensagemErroValorMaximo = null;
    this._TextoAntesFocus = null;

    // Handlers for the events    
    this._onFocus = null;
    this._onKeyPress = null;
    this._onBlur = null;

    this.initialize = function () {
        Employer.Componentes.UI.Web.EmployerHora.callBaseMethod(this, 'initialize');

        // Wireup the event handlers
        var element = this.get_TextBox();
        if (element) {
            this._onBlur = Function.createDelegate(this, this.Blur);
            $addHandler(element, 'blur', this._onBlur);

            this._onFocus = Function.createDelegate(this, this.Focus);
            $addHandler(element, 'focus', this._onFocus);

            this._onKeyPress = Function.createDelegate(this, this.KeyPress);
            $addHandler(element, 'keypress', this._onKeyPress);
        }
    },
    this.dispose = function () {
        var element = this.get_TextBox();
        if (element) {
            //$(element).uploadify('destroy');
            $clearHandlers(element);
        }

        Employer.Componentes.UI.Web.EmployerHora.callBaseMethod(this, 'dispose');
    },

    this.get_ClientOnBlur = function () {
        return this._ClientOnBlur;
    },
    this.set_ClientOnBlur = function (valor) {
        this._ClientOnBlur = valor;
    },

    this.get_TextoAntesFocus = function () {
        return this._TextoAntesFocus;
    },

    this.get_Campo2 = function (nome) {
        var espGrid = RegExp("(_[0-9]+)$");
        var m = espGrid.exec(this._id);

        if (m != null)
            return $get(this._id + nome + m[0]);
        else
            return $get(this._id + nome);
    },

    this.get_TextBox = function () {
        return this.get_Campo2("_txtValor");
    },
    this.set_TextBox = function (value) {
        this.get_TextBox().value = value;
    },

    this.get_PnlValidador = function () {
        return this.get_Campo2("_pnlValidador");
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
    }
    //Fim compatibilidade com ControlBaseValidator
}

//Simula herança
Employer.Componentes.UI.Web.EmployerHora.prototype = new Employer.Componentes.UI.Web.ControlBaseValidator;

Employer.Componentes.UI.Web.EmployerHora.prototype.AplicarMascaraHora = function () {
    var controle = this.get_TextBox();
    var valor = AjaxClientControlBase.Trim(controle.value);
    if (valor == '')
        return;

    if (valor.length == 4 && valor.indexOf(":") > -1) {
        if (valor.indexOf(":") == 1)
            valor = "0" + valor;
        else
            valor = valor + "0";
    }

    if (valor.match(/(^([01][0-9]|2[0-3])[:]([0-5][0-9])$)|(^\d{1,4}$)/)) {
        valor = valor.replace(':', '');
        switch (valor.length) {
            case 1:
                valor = '0' + valor + "00";
                break;
            case 2:
                valor += "00";
                break;
            case 3:
                valor = '0' + valor;
                break;
        }

        valor = valor.replace(/(\d{2})(\d{2})/, "$1:$2");
    } else if (valor.length == 3 && valor.indexOf(":") == (valor.length - 1)) {
        valor += "00";
    }

    controle.value = valor;
}

Employer.Componentes.UI.Web.EmployerHora.prototype.Focus = function () {
    this._TextoAntesFocus = this.get_TextBox().value;
}

Employer.Componentes.UI.Web.EmployerHora.prototype.KeyPress = function (e) {
    var key = AjaxClientControlBase.Key.GetKeyCode(e);

    if (AjaxClientControlBase.Key.isCommand(key))
        return true;

    var caracter = String.fromCharCode(key);

    //Somente números e pontos
    if (caracter != ":" && (key == Sys.UI.Key.space || isNaN(caracter))) {
        AjaxClientControlBase.CancelEvent(e);
        return false;
    }

    if (caracter == ":") {
        //Impede o caracter ':' de ser digitado mais de uma vez
        if (this.get_TextBox().value.indexOf(":") > -1) {
            AjaxClientControlBase.CancelEvent(e);
            return false;
        }
    }
    else {
        if (this.get_TextBox().value.indexOf(":") == -1 && this.get_TextBox().value.length == 4) {
            AjaxClientControlBase.CancelEvent(e);
            return false;
        }
    }
}

Employer.Componentes.UI.Web.EmployerHora.prototype.Blur = function (ev) {
    ev.preventDefault();
    ev.stopPropagation();

    var read = this.get_TextBox().getAttribute("readonly");

    if (!AjaxClientControlBase.IsNullOrEmpty(read)) {
        return false;
    }

    this.AplicarMascaraHora();

    if (!AjaxClientControlBase.IsNullOrEmpty(this.get_ClientOnBlur())) {
        eval(this.get_ClientOnBlur() + "($find('" + this._id + "'));");
    }

    this.PostPadrao();
}



Employer.Componentes.UI.Web.EmployerHora.prototype.LimparMascara = function () {
    var valor = this.get_TextBox().value;

    valor = AjaxClientControlBase.Trim(valor);

    if (/^\d{1,2}:\d{1,2}$/.test(valor))
    {
        var sPadrao = "[:]+";
        var oRegEx = new RegExp(sPadrao, "i");
        return valor.replace(oRegEx, "");
    }

    return valor;
}

Employer.Componentes.UI.Web.EmployerHora.prototype.ValidarFormato = function (valor) {
    if (!AjaxClientControlBase.IsNullOrEmpty(valor) > 0 && !isNaN(valor)) {
        switch (valor.length) {
            case 1:
                return true;
            case 2:
                return (valor < 24);
            case 3:
                return (valor.substring(1) < 60);
            case 4:
                return (valor.substring(0, 2) < 24 && valor.substring(2) < 60);
            default:
                return false;
        }
    }
    return false;
}

Employer.Componentes.UI.Web.EmployerHora.prototype.ValidarHora = function () {
    valor = this.LimparMascara();

    if (!this.ValidarFormato(valor))
        return false;

    return true;
}

Employer.Componentes.UI.Web.EmployerHora.prototype.Validar = function (arg) {
    arg.Value = this.get_TextBox().value;

    var validator = this.get_Validator();
    var div = this.get_PnlValidador();

    arg.IsValid = true;
    if (arg.Value == "" || arg.Value == null) {
        if (this.get_Obrigatorio()) {
            validator.innerHTML = this.get_MensagemErroObrigatorio();
            validator.errormessage = this.get_MensagemErroObrigatorioSummary();
            arg.IsValid = false;
        }
    } 
    else if (arg.IsValid && !this.ValidarHora())
    {
        validator.innerHTML = this.get_MensagemErroFormato();
        validator.errormessage = this.get_MensagemErroFormatoSummary();
        arg.IsValid = false;
    }    

    if (arg.IsValid) {
        div.style.display = "none";
        this.get_Validator().style.display = "none"
    }
    else {
        this.get_Validator().style.display = "block"
        div.style.display = "block";
    }
}

Employer.Componentes.UI.Web.EmployerHora.registerClass('Employer.Componentes.UI.Web.EmployerHora', Sys.UI.Behavior);


Employer.Componentes.UI.Web.EmployerHora.ValidarTextBox = function (sender, args) {
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
