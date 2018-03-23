Type.registerNamespace("Employer.Componentes.UI.Web");

// Define a simplified component.
Employer.Componentes.UI.Web.ControlPlacaVeiculo = function (element) {
    Employer.Componentes.UI.Web.ControlPlacaVeiculo.initializeBase(this, [element]);

    this._id = element.id;
    this._RegexValidacao = null;

    // Compatibilidade com ControlBaseValidator
    this._mensagemErroValorMinimo = null;
    this._mensagemErroValorMaximo = null;
    this._TextoAntesFocus = null;

    // Handlers for the events    
    this._onBlur = null;
    this._onKeyPress = null;
    this._onFocus = null;

    this.initialize = function () {
        Employer.Componentes.UI.Web.ControlPlacaVeiculo.callBaseMethod(this, 'initialize');

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

        Employer.Componentes.UI.Web.ControlPlacaVeiculo.callBaseMethod(this, 'dispose');
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

    this.get_RegexValidacao = function () {
        return this._RegexValidacao;
    },
    this.set_RegexValidacao = function (valor) {
        this._RegexValidacao = valor;
    },

    this.get_TextoAntesFocus = function () {
        return this._TextoAntesFocus;
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
Employer.Componentes.UI.Web.ControlPlacaVeiculo.prototype = new Employer.Componentes.UI.Web.ControlBaseValidator;

//Employer.Componentes.UI.Web.ControlPlacaVeiculo.prototype.Change = function () {
//    this.set_TextBox(this.get_CampoArquivo().value);

//    var val = this.get_Validator();
//    ValidatorEnable(val);
//}

Employer.Componentes.UI.Web.ControlPlacaVeiculo.prototype.ValidarRegex = function () {
    var value = this.get_TextBox().value;

    var rx = new RegExp(this.get_RegexValidacao());
    var matches = rx.exec(value);
    return (matches != null && value == matches[0]);
}

Employer.Componentes.UI.Web.ControlPlacaVeiculo.prototype.Validar = function (args) {
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

Employer.Componentes.UI.Web.ControlPlacaVeiculo.prototype.Focus = function (ev) {
    ev.preventDefault();
    ev.stopPropagation();

    var campo = this.get_TextBox();

    this._TextoAntesFocus = campo.value;

    campo.value = campo.value.replace("-", "");
}

Employer.Componentes.UI.Web.ControlPlacaVeiculo.prototype.AplicarMascara = function () {
    var controle = this.get_TextBox();

    var valor = AjaxClientControlBase.Trim(controle.value);
    if (valor == '')
        return;

    if (valor.match(/^[A-Za-z]{3}-{0,1}\d{4}$/))
        valor = valor.toUpperCase().replace(/([A-Z]{3})(\d{4})/, "$1-$2");

    controle.value = valor;
}

Employer.Componentes.UI.Web.ControlPlacaVeiculo.prototype.Blur = function (ev) {
    ev.preventDefault();
    ev.stopPropagation();

    this.AplicarMascara();

    this.PostPadrao();
}

Employer.Componentes.UI.Web.ControlPlacaVeiculo.prototype.KeyPress = function (event) {
    /// <summary>
    /// Retorna somente nros ou backspace/delete/return e impede ZERO (0) no primeiro digito.
    /// </summary>

    var tecla = AjaxClientControlBase.Key.GetKeyCode(event);

    if (tecla == AjaxClientControlBase.Key.Commands.BACKSPACE ||
        tecla == AjaxClientControlBase.Key.Commands.TAB ||
        tecla == AjaxClientControlBase.Key.Commands.ENTER ||
      (tecla >= AjaxClientControlBase.Key.Commands.LEFT && tecla <= AjaxClientControlBase.Key.Commands.DOWN) ||
      tecla == AjaxClientControlBase.Key.Commands.DELETE_KEY)
        return true;

    var controle = this.get_TextBox();

    var pos = AjaxClientControlBase.GetSelectionStart(controle);
    var sizeSelected = AjaxClientControlBase.GetSizeTextSelected(controle);

    if (controle.value.length < 7 || (controle.value.length == 7 && sizeSelected > 0)) {
        if (pos < 3) {
            // letras
            //65 = 'A', 90 = 'Z', 97 = 'a', 122 = 'z'
            if ((tecla >= 65 && tecla <= 90) || (tecla >= 97 && tecla <= 122)) {
                return true;
            }
        } else {
            // nros
            // 48='0', 57='9'  
            if ((tecla >= 48 && tecla <= 57)) {
                return true;
            }
        }
    }

    AjaxClientControlBase.CancelEvent(event);
    return false;
}

Employer.Componentes.UI.Web.ControlPlacaVeiculo.registerClass('Employer.Componentes.UI.Web.ControlPlacaVeiculo', Sys.UI.Behavior);


Employer.Componentes.UI.Web.ControlPlacaVeiculo.ValidarTextBox = function (sender, args) {
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
