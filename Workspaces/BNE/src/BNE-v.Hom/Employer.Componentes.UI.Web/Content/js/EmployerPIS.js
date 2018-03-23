Type.registerNamespace("Employer.Componentes.UI.Web");

// Define a simplified component.
Employer.Componentes.UI.Web.EmployerPIS = function (element) {
    Employer.Componentes.UI.Web.EmployerPIS.initializeBase(this, [element]);

    this._id = element.id;

    this._ExibirDescricao = null;

    // Compatibilidade com ControlBaseValidator
    this._mensagemErroValorMinimo = null;
    this._mensagemErroValorMaximo = null;
    this._TextoAntesFocus = null;

    // Handlers for the events    
    this._onBlur = null;
    this._onKeyPress = null;
    this._onFocus = null;

    //enums
    this.TipoValor =
    {
        Pasep: "Pasep",
        PIS: "PIS",
        NIT: "NIT",
        SUS: "SUS"
    };

    this.initialize = function () {
        Employer.Componentes.UI.Web.EmployerPIS.callBaseMethod(this, 'initialize');

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

        Employer.Componentes.UI.Web.EmployerPIS.callBaseMethod(this, 'dispose');
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

    this.get_TextoAntesFocus = function () {
        return this._TextoAntesFocus;
    },

    this.get_ExibirDescricao = function () {
        return _ExibirDescricao;
    },
    this.set_ExibirDescricao = function (value) {
        _ExibirDescricao = value;
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
Employer.Componentes.UI.Web.EmployerPIS.prototype = new Employer.Componentes.UI.Web.ControlBaseValidator;

Employer.Componentes.UI.Web.EmployerPIS.prototype.LimparMascara = function (valor) {

    valor = AjaxClientControlBase.Trim(valor);
    valor = valor.replace(".","");
    valor = valor.replace(".","");
    valor = valor.replace("-","");
            
    return valor;
}

Employer.Componentes.UI.Web.EmployerPIS.prototype.ValidarFormato = function (valor) {
    if (valor.length == 11 && !isNaN(valor))
        return true;
    return false;
}

Employer.Componentes.UI.Web.EmployerPIS.prototype.ValidarCalculo = function (valor) {
    var aux = [3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
    var result = 0;
    for (var i = 0; i < 10; i++)
        result += (valor.charAt(i) * aux[i]);

    result = result % 11;
    if (result <= 1)
        result = 0;
    else
        result = 11 - result;

    return valor.charAt(10) == result;
}

Employer.Componentes.UI.Web.EmployerPIS.prototype.ValidarIntervalo = function (valor) {
    var vlr = parseFloat(valor.substring(0, 10));

    if ((vlr >= 1000000001 && vlr <= 1022000000) || (vlr >= 1700000001 && vlr <= 1999999999))
        return true;
    else if ((vlr >= 1022000001 && vlr <= 1089999999) || (vlr >= 1200000001 && vlr <= 1669999999) || (vlr >= 1690000000 && vlr <= 1699999999))
        return true;
    else if ((vlr >= 1090000000 && vlr <= 1199999999) || (vlr >= 1670000000 && vlr <= 1689999999) || (vlr >= 2670000000 && vlr <= 2679999999))
        return true;
    //else if (vlr >= 2000000000 && vlr <= 2999999999)
    else if ((vlr >= 2000000000 && vlr <= 2669999999) || (vlr >= 2680000000 && vlr <= 2999999999))
        return true;

    return false;
}

Employer.Componentes.UI.Web.EmployerPIS.prototype.RecuperarTipo = function (valor) 
{
    valor = this.LimparMascara(valor);
    if (this.ValidarFormato(valor))
    {
        if (this.ValidarCalculo(valor))
        {
            var vlr = parseFloat(valor.substring(0, 10));


            if ((vlr >= 1000000000 && vlr <= 1019999999) || (vlr >= 1700000000 && vlr <= 1909999999))
                return this.TipoValor.Pasep;
            else if ((vlr >= 1020000000 && vlr <= 1089999999) || (vlr >= 1200000000 && vlr <= 1669999999) || (vlr >= 1690000000 && vlr <= 1699999999))
                return this.TipoValor.PIS;
            else if ((vlr >= 1090000000 && vlr <= 1199999999) || (vlr >= 1670000000 && vlr <= 1689999999) || (vlr >= 2670000000 && vlr <= 2679999999))
                return this.TipoValor.NIT;
                //else if (vlr >= 2000000000 && vlr <= 2999999999)
            else if ((vlr >= 2000000000 && vlr <= 2669999999) || (vlr >= 2680000000 && vlr <= 2999999999))
                return this.TipoValor.PIS;
        }
    }

    return null;
}

Employer.Componentes.UI.Web.EmployerPIS.prototype.ValidarRegex = function () {
    var valor = this.get_TextBox().value;

    valor = this.LimparMascara(valor);

    if (!this.ValidarFormato(valor))
        return false;

    if (valor <= 1)
        return false;

    if (!this.ValidarCalculo(valor))
        return false;

    if (!this.ValidarIntervalo(valor))
        return false;

    return true;
}

Employer.Componentes.UI.Web.EmployerPIS.prototype.Validar = function (args) {
    var validator = this.get_Validator();

    var value = this.get_TextBox().value;

    var div = this.get_PnlValidador();

    args.IsValid = true;
    if (value == "" || value == null) {
        if (this.get_Obrigatorio()) {
            validator.innerHTML = this.get_MensagemErroObrigatorio();
            validator.errormessage = this.get_MensagemErroObrigatorioSummary();
            args.IsValid = false;
        }
    }
    else {
        if (!this.ValidarRegex()) {
            validator.innerHTML = this.get_MensagemErroFormato();
            validator.errormessage = this.get_MensagemErroFormatoSummary();
            args.IsValid = false;
        }
    }

    if (args.IsValid) {
        div.style.display = "none";
        validator.style.display = "none"
    }
    else {
        validator.style.display = "block"
        div.style.display = "block";
    }

    return args.IsValid;
}

Employer.Componentes.UI.Web.EmployerPIS.prototype.Focus = function (ev) {
    ev.preventDefault();
    ev.stopPropagation();

    var campo = this.get_TextBox();

    this._TextoAntesFocus = campo.value;

    campo.value = campo.value.replace("-", "");
    campo.value = campo.value.replace(".", "");
    campo.value = campo.value.replace(".", "");
}

Employer.Componentes.UI.Web.EmployerPIS.prototype.AplicarMascara = function () {
    var controle = this.get_TextBox();

    var valor = AjaxClientControlBase.Trim(controle.value);

    if (this.get_ExibirDescricao()) {
        var tipo = this.RecuperarTipo(valor);

        var campo = this.get_Campo("_lblValor");
        campo.innerHTML = (tipo == null ? "" : tipo);
    }

    if (valor == '')
        return;

    if (valor.match("\\d{11}")) {
        valor = valor.replace(/(\d{3})(\d)/, "$1.$2");
        valor = valor.replace(/(\d{5})(\d)/, "$1.$2");
        valor = valor.replace(/(\d{2})(\d{1})$/, "$1-$2");
    }

    controle.value = valor;
}

Employer.Componentes.UI.Web.EmployerPIS.prototype.Blur = function (ev) {
    ev.preventDefault();
    ev.stopPropagation();

    this.AplicarMascara();

    this.PostPadrao();
}

Employer.Componentes.UI.Web.EmployerPIS.prototype.KeyPress = function (e) {

    var key = AjaxClientControlBase.Key.GetKeyCode(e.rawEvent);

    if (e.rawEvent.keyCode && AjaxClientControlBase.Key.isCommand(key))
        return true;

    if (key == Sys.UI.Key.space) {
        e.preventDefault();
        return false;
    }

    var caracter = String.fromCharCode(key);

    //É letra
    if (isNaN(caracter))
        return AjaxClientControlBase.CancelEvent(e)
}

Employer.Componentes.UI.Web.EmployerPIS.registerClass('Employer.Componentes.UI.Web.EmployerPIS', Sys.UI.Behavior);

Employer.Componentes.UI.Web.EmployerPIS.ValidarTextBox = function (sender, args) {
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