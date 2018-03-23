
Type.registerNamespace("Employer.Componentes.UI.Web");

// Define a simplified component.
Employer.Componentes.UI.Web.ControlAutoCompleteBase = function (element) {
    Employer.Componentes.UI.Web.ControlAutoCompleteBase.initializeBase(this, [element]);

    this._id = element.id;

    this._IdModalAjax = null;
    this._CustomClientValidate = null;
    this._IdModalEstado = null;

    // Handlers for the events
    this._onKeyPress = null;
    this._onKeyUp = null;
    this._onBlur = null;
    this._onFocus = null;
    this._onChange = null;
    this._TextoAntesFocus = null;
    this._tipoAlfanumerico = 0;
    this._ClientOnFocus;
    this._ClientOnBlur;
    this._CarregarOnAjaxBlur;
    this._TerminouCarregarOnAjaxBlur = true;
    this._CarregouOnAjaxBlur = false;

    // Summary
    this._MensagemErroObrigatorioSummary = null;
    this._MensagemErroFormatoSummary = null;
    this._MensagemErroValorMinimoSummary = null;
    this._MensagemErroValorMaximoSummary = null;

    this._ValidarTextChanging = null;

    this.get_TextBox = function () {
        var espGrid = RegExp("(_[0-9]+)$");
        var m = espGrid.exec(this._id);

        if (m != null)
            return $get(this._id + "_txtValor" + m[0]);
        else
            return $get(this._id + "_txtValor");
    },

    this.get_ClientOnBlur = function () {
        return this._ClientOnBlur;
    },
    this.set_ClientOnBlur = function (valor) {
        this._ClientOnBlur = valor;
    },

    this.get_CarregarOnAjaxBlur = function () {
        return this._CarregarOnAjaxBlur;
    },
    this.set_CarregarOnAjaxBlur = function (valor) {
        this._CarregarOnAjaxBlur = valor;
    },

    this.get_ClientOnFocus = function () {
        return this._ClientOnFocus;
    },
    this.set_ClientOnFocus = function (valor) {
        this._ClientOnFocus = valor;
    },
    this.get_TipoAlfanumerico = function () {
        return this._tipoAlfanumerico;
    },
    this.set_TipoAlfanumerico = function (valor) {
        this._tipoAlfanumerico = valor;
    },

    this.set_IdModalEstado = function (valor) {
        this._IdModalEstado = valor;
    },
    this.get_IdModalEstado = function () {
        return this._IdModalEstado;
    },

    this.set_TextBox = function (value) {
        this.get_TextBox().value = value;
    },
    this.get_TextoAntesFocus = function () {
        //        if (AjaxClientControlBase.IsNullOrEmpty(this._TextoAntesFocus))
        //            return this.get_TextBox().value;
        return this._TextoAntesFocus;
    },
    this.initialize = function () {
        Employer.Componentes.UI.Web.ControlAutoCompleteBase.callBaseMethod(this, 'initialize');

        // Wireup the event handlers
        var element = this.get_TextBox();
        if (element) {

            this._onBlur = Function.createDelegate(this, this.Blur);
            $addHandler(element, 'blur', this._onBlur);

            this._onFocus = Function.createDelegate(this, this.Focus);
            $addHandler(element, 'focus', this._onFocus);

            this._onKeyUp = Function.createDelegate(this, this.KeyUP);
            $addHandler(element, 'keyup', this._onKeyUp);

            this._onKeyPress = Function.createDelegate(this, this.KeyPress);
            $addHandler(element, 'keypress', this._onKeyPress);

            window.setTimeout(function () {
                var fc = $(':focus');

                if (fc.length > 0 && fc.attr("id") == element.id) {
                    element.blur();
                    element.focus();
                }
            }, 500);
        }
    },
     this.dispose = function () {
         //$clearHandlers(this.get_TextBox());
         var element = this.get_TextBox();

         if (element) {
             $removeHandler(element, 'blur', this._onBlur);
             $removeHandler(element, 'focus', this._onFocus);
             $removeHandler(element, 'keyup', this._onKeyUp);
             $removeHandler(element, 'keypress', this._onKeyPress);
         }

         Employer.Componentes.UI.Web.ControlAutoCompleteBase.callBaseMethod(this, 'dispose');
     }
    this.get_Id = function () {
        return this._id;
    },
    this.get_MensagemErroValorMaximo = function () {
        return this._MensagemErroValorMaximo;
    },
    this.get_MensagemErroValorMinimo = function () {
        return this._MensagemErroValorMinimo;
    },
    this.get_IdModalAjax = function () {
        return this._IdModalAjax;
    },
    this.set_IdModalAjax = function (value) {
        this._IdModalAjax = value;
    },
    this.get_ModalAjax = function () {
        if (AjaxClientControlBase.IsNullOrEmpty(this.get_IdModalAjax()) == false)
            return $find(this.get_IdModalAjax());
        return null;
    },
    this.get_CustomClientValidate = function () {
        return this._CustomClientValidate;
    },
    this.set_CustomClientValidate = function (value) {
        this._CustomClientValidate = value;
    },
    this.get_ValidarTextChanging = function () {
        return this._ValidarTextChanging;
    },
    this.set_ValidarTextChanging = function (value) {
        this._ValidarTextChanging = value;
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
Employer.Componentes.UI.Web.ControlAutoCompleteBase.prototype = new Employer.Componentes.UI.Web.ControlBaseValidator;

Employer.Componentes.UI.Web.ControlAutoCompleteBase.prototype.get_codigo = function () {
    return this.get_Campo("_hdnCodigo");
}
Employer.Componentes.UI.Web.ControlAutoCompleteBase.prototype.set_codigo = function (value) {
    this.get_codigo().value = value;
}

Employer.Componentes.UI.Web.ControlAutoCompleteBase.prototype.get_AutoComplete = function () {
    return this.get_Controle("_AutoComplete");
}

Employer.Componentes.UI.Web.ControlAutoCompleteBase.prototype.Focus = function (ev) {
    ev.preventDefault();
    ev.stopPropagation();

    var read = this.get_TextBox().getAttribute("readonly");

    if (!AjaxClientControlBase.IsNullOrEmpty(read)) {
        return false;
    }

    this._TextoAntesFocus = this.get_TextBox().value;
    if (this.get_ClientOnFocus() != null && this.get_ClientOnFocus() != "") {
        eval(this.get_ClientOnFocus());
    }
}

Employer.Componentes.UI.Web.ControlAutoCompleteBase.prototype.Validar = function (arg) {
    arg.IsValid = true;

    //var validator = $get(this._id + "_cvValor");
    var div = this.get_Campo("_pnlValidador");
    var validator = this.get_Validador();

    if (AjaxClientControlBase.IsNullOrEmpty(this.get_CustomClientValidate())) {

        // Verifica se é obrigatório
        if (this.get_Obrigatorio() == true && this.get_TextBox().value == "") {
            arg.IsValid = false;
            validator.innerHTML = this.get_MensagemErroObrigatorio();
            validator.errormessage = this.get_MensagemErroObrigatorioSummary();
        }
        else {
            // Verifica valor inválido
            if (this.get_ValidarTextChanging() == false && (this.get_codigo().value == "" || this.get_codigo().value == null) && (this.get_TextBox().value != "" && this.get_TextBox().value != null)) {
                arg.IsValid = false;
                validator.innerHTML = this.get_MensagemErroFormato();
                validator.errormessage = this.get_MensagemErroFormatoSummary();
            }
        }
    }
    else {
        eval(this.get_CustomClientValidate() + "(arg,$find('" + this._id + "'));");
        if (!arg.IsValid) {
            // Verifica se é obrigatório
            if (this.get_Obrigatorio() == true && this.get_TextBox().value == "") {
                arg.IsValid = false;
                validator.innerHTML = this.get_MensagemErroObrigatorio();
                validator.errormessage = this.get_MensagemErroObrigatorioSummary();
            }
            else {
                validator.innerHTML = this.get_MensagemErroFormato();
                validator.errormessage = this.get_MensagemErroFormatoSummary();
            }
        }
    }

    if (arg.IsValid) {
        div.style.display = "none";
        validator.innerHTML = "";
        validator.errormessage = "";
    }
    else {
        div.style.display = "block";
        this.get_codigo().value = "";
        this._TextoAntesFocus = null;
    }
}

Employer.Componentes.UI.Web.ControlAutoCompleteBase.prototype.MostrarErros = function () {
    IsValid = true;
    //if (this.get_TextBox().value != this.get_TextoAntesFocus()) {
    //var validator = $get(this._id + "_cvValor");
    var div = this.get_Campo("_pnlValidador");
    var validator = this.get_Validador();

    if (AjaxClientControlBase.IsNullOrEmpty(this.get_CustomClientValidate())) {

        // Verifica se é obrigatório
        if (this.get_Obrigatorio() == true && AjaxClientControlBase.IsNullOrEmpty(this.get_TextBox().value)) {
            IsValid = false;
            validator.innerHTML = this.get_MensagemErroObrigatorio();
            validator.errormessage = this.get_MensagemErroObrigatorioSummary();
        }
        else {
            // Verifica valor inválido
            if (this.get_ValidarTextChanging() == false && (this.get_codigo().value == "" || this.get_codigo().value == null) && (this.get_TextBox().value != "" && this.get_TextBox().value != null)) {
                IsValid = false;
                validator.innerHTML = this.get_MensagemErroFormato();
                validator.errormessage = this.get_MensagemErroFormatoSummary();
            }
        }
    }
    else {
        var arg = new Object();
        arg.IsValid = true;
        eval(this.get_CustomClientValidate() + "(arg,$find('" + this._id + "'));");
        IsValid = arg.IsValid;
        if (!IsValid) {
            if (this.get_Obrigatorio() == true && AjaxClientControlBase.IsNullOrEmpty(this.get_TextBox().value)) {
                validator.innerHTML = this.get_MensagemErroObrigatorio();
                validator.errormessage = this.get_MensagemErroObrigatorioSummary();
            }
            else {
                validator.innerHTML = this.get_MensagemErroFormato();
                validator.errormessage = this.get_MensagemErroFormatoSummary();
            }
        }
    }

    if (IsValid) {
        div.style.display = "none";
        validator.style.display = "none";
        validator.innerHTML = "";
        validator.errormessage = "";
    }
    else {
        div.style.display = "block";
        validator.style.display = "block";
        this.get_codigo().value = "";
        this._TextoAntesFocus = null;
    }
    return IsValid;
    //}
    //return false;

}

Employer.Componentes.UI.Web.ControlAutoCompleteBase.prototype.SetFocus = function () {
    var autoComplete = this.get_Controle("_AutoComplete");
    var campo = this.get_TextBox();
    campo.focus();
    autoComplete._onGotFocus();
}

Employer.Componentes.UI.Web.ControlAutoCompleteBase.prototype.Post = function (forcaPort) {
    if (forcaPort || !AjaxClientControlBase.EqualsIsNullOrEmpty(this.get_TextBox().value, this.get_TextoAntesFocus())) {
        //if (this.get_TextBox().value != this.get_TextoAntesFocus()) {
        if (AjaxClientControlBase.IsNullOrEmpty(this.get_ValorAlteradoClient()) == false)
            eval(this.get_ValorAlteradoClient() + "($find('" + this._id + "'));");

        if (this.get_isPostBack() & this.MostrarErros()) {
            var argumento = "";
            if (this.get_TextBox().value != "")
                argumento = this.get_TextBox().id;

            //Foco automático
            if (typeof (Employer_Componentes_UI_Web_AutoTabIndex) != "undefined")
                Employer_Componentes_UI_Web_AutoTabIndex.Set_Focus_Server(this.get_TextBox().id);

            __doPostBack(this.get_TextBox().id, argumento);
        }
    }
}

Employer.Componentes.UI.Web.ControlAutoCompleteBase.prototype.CarregarPorCodigo = function (codigo, valor) {
    var div = document.createElement('div');
    div.innerHTML = valor;

    this._TextoAntesFocus = div.textContent == undefined ? div.innerText : div.textContent;

    this.get_TextBox().value = this._TextoAntesFocus;
    this.set_codigo(codigo);

    var modal = this.get_ModalAjax();
    if (modal) {
        modal.hide();
        if (AjaxClientControlBase.IsNullOrEmpty(this.get_IdModalEstado()) == false)
            $get(this.get_IdModalEstado()).value = "False";
    }


    this.Post(true);
}

Employer.Componentes.UI.Web.ControlAutoCompleteBase.prototype.Blur2 = function () {
    if (!AjaxClientControlBase.IsNullOrEmpty(this.get_CarregarOnAjaxBlur()) &&
    AjaxClientControlBase.EqualsIsNullOrEmpty(this.get_TextBox().value, this.get_TextoAntesFocus()) &&
    this._CarregouOnAjaxBlur) {
        this._TextoAntesFocus = "";//Limpa texto antes do foco p/ poder dar post caso tenha
    }

    if (!AjaxClientControlBase.IsNullOrEmpty(this.get_ClientOnBlur())) {
        eval(this.get_ClientOnBlur());
    }

    if (this.MostrarErros()) {
        this.Post();
    }
}

Employer.Componentes.UI.Web.ControlAutoCompleteBase.prototype.Blur = function (ev) {
    ev.preventDefault();
    ev.stopPropagation();

    var read = this.get_TextBox().getAttribute("readonly");

    if (!AjaxClientControlBase.IsNullOrEmpty(read)) {
        return false;
    }

    if (!AjaxClientControlBase.IsNullOrEmpty(this.get_CarregarOnAjaxBlur()) &&
    !AjaxClientControlBase.IsNullOrEmpty(this.get_TextBox().value) &&
    !AjaxClientControlBase.EqualsIsNullOrEmpty(this.get_TextBox().value, this.get_TextoAntesFocus())) {

        var autocomplete = this.get_AutoComplete();
        var webServicePath = autocomplete._servicePath;
        var webMethod = this.get_CarregarOnAjaxBlur();

        var parans = { "texto": this.get_TextBox().value, "contextKey": autocomplete.get_contextKey() };

        this._CarregouOnAjaxBlur = false;

        this._TerminouCarregarOnAjaxBlur = false;
        var controle = this;

        Sys.Net.WebServiceProxy.invoke(webServicePath,
        webMethod, false, parans,
        function (result, eventArgs) { //OnSucceeded
            if (!AjaxClientControlBase.IsNullOrEmpty(result)) {
                result = JSON.parse(result);
                controle._CarregouOnAjaxBlur = true;
                controle._TextoAntesFocus = result.First;
                controle.set_TextBox(result.First);
                controle.set_codigo(result.Second);
            }

            controle._TerminouCarregarOnAjaxBlur = true;
        },
        function (error) { //OnFailed
            controle._TerminouCarregarOnAjaxBlur = true;
        },
        "User Context",
        1000);
    }

    var instancia = this;

    var interval =
    window.setInterval(function () {
        if (instancia._TerminouCarregarOnAjaxBlur) {
            clearInterval(interval);
            instancia.Blur2();
        }
    }, 100);
}

Employer.Componentes.UI.Web.ControlAutoCompleteBase.prototype.KeyUP = function () {
    if (this.get_TipoAlfanumerico() == 1) {
        this.get_TextBox().value = this.get_TextBox().value.toUpperCase();
    }
    if (this.get_TipoAlfanumerico() == 2) {
        this.get_TextBox().value = this.get_TextBox().value.toLowerCase();
    }

    if (this.get_TextBox().value != this._TextoAntesFocus) {
        this.set_codigo("");
    }
}

Employer.Componentes.UI.Web.ControlAutoCompleteBase.prototype.KeyPress = function (e) {
    var key = AjaxClientControlBase.Key.GetKeyCode(e.rawEvent);

    //Verificar pq os KeyCode estão vindo incorretos.

    if (e.rawEvent.keyCode && AjaxClientControlBase.Key.isCommand(key))
        return true;

    var caracter = String.fromCharCode(key);

    if (this.get_TipoAlfanumerico() == 3) {
        //É letra
        if (isNaN(caracter))
            return AjaxClientControlBase.CancelEvent(e);
    }

    if (this.get_TipoAlfanumerico() == 4) {
        if (!isNaN(caracter) && caracter != " ")
            return AjaxClientControlBase.CancelEvent(e);
    }

    return true;
}

Employer.Componentes.UI.Web.ControlAutoCompleteBase.ValidarTextBox = function (sender, args) {
    var i = sender.id.indexOf("_cvValor");

    var controle = $find(sender.id.substring(0, i));
    if (controle == null)
        return;
    controle.Validar(args);
}

Employer.Componentes.UI.Web.ControlAutoCompleteBase.ItemSelected = function (source, eventArgs) {
    var i = source.get_id().lastIndexOf("_AutoComplete");
    var controle = $find(source.get_id().substring(0, i));

//    if (!AjaxClientControlBase.IsNullOrEmpty(controle.get_CarregarOnAjaxBlur()) &&
//    !AjaxClientControlBase.EqualsIsNullOrEmpty(this.get_TextBox().value, this.get_TextoAntesFocus())) {

    

    controle._TextoAntesFocus = eventArgs.get_text();
    controle.set_TextBox(eventArgs.get_text());
    controle.set_codigo(eventArgs.get_value());

    controle.Post(true);
}

Employer.Componentes.UI.Web.ControlAutoCompleteBase.descriptor = {
    properties: [
                { name: 'Obrigatorio', type: Boolean },
                { name: 'MensagemErroObrigatorio', type: String },
                { name: 'MensagemErroFormato', type: String },
                { name: 'MensagemErroValorMinimo', type: String },
                { name: 'MensagemErroValorMaximo', type: String }
                ]
}

// Register the class as a type that inherits from Sys.UI.Control.
Employer.Componentes.UI.Web.ControlAutoCompleteBase.registerClass('Employer.Componentes.UI.Web.ControlAutoCompleteBase', Sys.UI.Behavior);

//if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();