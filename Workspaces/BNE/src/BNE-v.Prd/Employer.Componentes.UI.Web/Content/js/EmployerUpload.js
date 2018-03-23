Type.registerNamespace("Employer.Componentes.UI.Web");

// Define a simplified component.
Employer.Componentes.UI.Web.EmployerUpload = function (element) {
    Employer.Componentes.UI.Web.EmployerUpload.initializeBase(this, [element]);

    this._id = element.id;
    this._RegexValidacao = null;

    // Compatibilidade com ControlBaseValidator
    this._mensagemErroValorMinimo = null;
    this._mensagemErroValorMaximo = null;

    // Handlers for the events    
    this._onChange = null;

    this.initialize = function () {
        Employer.Componentes.UI.Web.EmployerUpload.callBaseMethod(this, 'initialize');

        // Wireup the event handlers
        var element = this.get_CampoArquivo();
        if (element) {
            this._onChange = Function.createDelegate(this, this.Change);
            $addHandler(element, 'change', this._onChange);

            //            this._onKeyPress = Function.createDelegate(this, this.KeyPress);
            //            $addHandler(element, 'keypress', this._onKeyPress);

            //            this._onFocus = Function.createDelegate(this, this.Focus);
            //            $addHandler(element, 'focus', this._onFocus);
        }
    },
    this.dispose = function () {
        $clearHandlers(this.get_CampoArquivo());

        Employer.Componentes.UI.Web.EmployerUpload.callBaseMethod(this, 'dispose');
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
        return this.get_Campo2("_txtCampoVal");
    },
    this.set_TextBox = function (value) {
        this.get_TextBox().value = value;
    },

    this.get_CampoArquivo = function () {
        return this.get_Campo2("_CampoArquivo");
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
Employer.Componentes.UI.Web.EmployerUpload.prototype = new Employer.Componentes.UI.Web.ControlBaseValidator;

Employer.Componentes.UI.Web.EmployerUpload.prototype.Change = function () {
    this.set_TextBox(this.get_CampoArquivo().value);

    var val = this.get_Validator();
    ValidatorEnable(val);
}

Employer.Componentes.UI.Web.EmployerUpload.prototype.ValidarRegex = function () {
    if (this.get_RegexValidacao() == null)
        return true;

    var valor = AjaxClientControlBase.Trim(this.get_TextBox().value);

    var patt = new RegExp(this.get_RegexValidacao(), "g");

    return patt.test(valor);
}

Employer.Componentes.UI.Web.EmployerUpload.prototype.Validar = function (arg) {
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
    else {
        if (!this.ValidarRegex()) {
            validator.innerHTML = this.get_MensagemErroFormato();
            validator.errormessage = this.get_MensagemErroFormatoSummary();
            arg.IsValid = false;
        }
    }

    if (arg.IsValid) {
        div.style.display = "none";
        this.get_Validator().style.visibility = "hidden"
    }
    else {
        this.get_Validator().style.visibility = "visible"
        div.style.display = "block";
    }
}

Employer.Componentes.UI.Web.EmployerUpload.registerClass('Employer.Componentes.UI.Web.EmployerUpload', Sys.UI.Behavior);


Employer.Componentes.UI.Web.EmployerUpload.ValidarTextBox = function (sender, args) {
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
