// Declare a namespace.
Type.registerNamespace("Employer.Componentes.UI.Web");

// Define a simplified component.
Employer.Componentes.UI.Web.ListaSugestao = function (element) {
    Employer.Componentes.UI.Web.ListaSugestao.initializeBase(this, [element]);

    this._id = element.id;

    // Handlers for the events    
    this._onBlur = null;
    this._onFocus = null;
    this._onKeyPress = null;
    this._width = null;
    this._dicionario = null;
    this._descricao = null;
    this._isPostBack = null;
    this._isServerBlur = null;
    this._mensagemErroInvalido = null;
    this._mensagemErroObrigatorio = null;
    this._obrigatorio = null;
    this._mostrarDescricao = null;
    this._mostrarListaSugestao = null;
    this._tipoSugestao = null;
    this._textoAntesFocus = "";
    // Summary
    this._mensagemErroObrigatorioSummary = null;
    this._mensagemErroInvalidoSummary = null;


    this.get_IsServerBlur = function () {
        return this._isServerBlur;
    },
    this.set_IsServerBlur = function (value) {
        this._isServerBlur = value;
    },

    this.get_Width = function () {
        return this._width;
    },
    this.set_Width = function (value) {
        this._width = value;
    },
    this.get_Dicionario = function () {
        return this._dicionario;
    },
    this.set_Dicionario = function (value) {
        this._dicionario = value;
    },
    this.get_Descricao = function () {
        return this._descricao;
    },
    this.set_Descricao = function (value) {
        this._descricao = value;
        this.mostrarDescricao(value);
    },
    this.get_IsPostBack = function () {
        return this._isPostBack;
    },
    this.set_IsPostBack = function (value) {
        this._isPostBack = value;
    },
    this.get_TextBox = function () {
        var espGrid = RegExp("(_[0-9]+)$");
        var m = espGrid.exec(this._id);

        if (m != null)
            return $get(this._id + "_txtValor" + m[0]);
        else
            return $get(this._id + "_txtValor");
    },
    this.get_Label = function () {
        var espGrid = RegExp("(_[0-9]+)$");
        var m = espGrid.exec(this._id);

        if (m != null)
            return $get(this._id + "_lblDescricao" + m[0]);
        else
            return $get(this._id + "_lblDescricao");
    },
    this.get_LabelJQuery = function () {
        var espGrid = RegExp("(_[0-9]+)$");
        var m = espGrid.exec(this._id);

        if (m != null)
            return $(this._id + "_lblDescricao" + m[0]);
        else
            return $(this._id + "_lblDescricao");
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
    this.get_MensagemErroInvalido = function () {
        return this._mensagemErroInvalido;
    },
    this.get_MensagemErroObrigatorio = function () {
        return this._mensagemErroObrigatorio;
    },
    this.set_MensagemErroInvalido = function (value) {
        this._mensagemErroInvalido = value;
    },
    this.set_MensagemErroObrigatorio = function (value) {
        this._mensagemErroObrigatorio = value;
    },
    this.get_TextoAntesFocus = function () {
        return this._textoAntesFocus;
    },
    this.get_MensagemErroObrigatorioSummary = function () {
        if (AjaxClientDataBoundControlBase.IsNullOrEmpty(this._mensagemErroObrigatorioSummary) == false)
            return this._mensagemErroObrigatorioSummary;

        return this.get_MensagemErroObrigatorio();
    },
    this.set_MensagemErroObrigatorioSummary = function (value) {
        this._mensagemErroObrigatorioSummary = value;
    },
    this.get_MensagemErroInvalidoSummary = function () {
        if (AjaxClientDataBoundControlBase.IsNullOrEmpty(this._mensagemErroInvalidoSummary) == false)
            return this._mensagemErroInvalidoSummary;

        return this.get_MensagemErroInvalido();
    },
    this.set_MensagemErroInvalidoSummary = function (value) {
        this._mensagemErroInvalidoSummary = value;
    },
    this.get_Obrigatorio = function () {
        return this._obrigatorio;
    },
    this.set_Obrigatorio = function (value) {
        this._obrigatorio = value;
    },
    this.get_MostrarDescricao = function () {
        return this._mostrarDescricao;
    },
    this.set_MostrarDescricao = function (value) {
        this._mostrarDescricao = value;
    },
    this.get_MostrarListaSugestao = function () {
        return this._mostrarListaSugestao;
    },
    this.set_MostrarListaSugestao = function (value) {
        this._mostrarListaSugestao = value;
    },
    this.get_TipoSugestao = function () {
        return this._tipoSugestao;
    },
    this.set_TipoSugestao = function (value) {
        this._tipoSugestao = value;
    },
    this.initialize = function () {
        Employer.Componentes.UI.Web.ListaSugestao.callBaseMethod(this, 'initialize');

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

        if (this.get_Validador()) {
            if (this.get_Validador().isvalid == true) {
                this.get_Validador().style.display = "none"
            }
        }
        this.esconderLista();

        this.AddHandlerLista();

        //Focus
        var cpCampo = $(":focus");
        if (cpCampo.length > 0 && cpCampo.attr("id") == element.id) {
            this.Focus();
        }
    },
    this.dispose = function () {
        $clearHandlers(this.get_TextBox());

        this.ClearHandlersLista();

        Employer.Componentes.UI.Web.ListaSugestao.callBaseMethod(this, 'dispose');
    },
    this.get_DivLista = function () {
        var espGrid = RegExp("(_[0-9]+)$");
        var m = espGrid.exec(this._id);

        if (m != null)
            return document.getElementById(this._id + "_pnlOculto" + m[0]);
        else
            return document.getElementById(this._id + "_pnlOculto");
    },
    this.get_PnlValidador = function () {
        var espGrid = RegExp("(_[0-9]+)$");
        var m = espGrid.exec(this._id);

        if (m != null)
            return document.getElementById(this._id + "_pnlValidador" + m[0]);
        else
            return document.getElementById(this._id + "_pnlValidador");
    },
    this.esconderLista = function () {
        var element = this.get_DivLista();
        if (element) {
            element.style.display = "none";
        }
    },
    this.mostrarLista = function () {
        var element = this.get_DivLista();
        element.style.display = "";
        try { this.set_Descricao(""); }
        catch (ex) { }
    },
    this.mostrarDescricao = function (descricao) {
        if (this.get_Label() == null)
            return;

        this.get_Label().style.display = "block";
        if (this.get_MostrarDescricao()) {
            if (descricao !== undefined) {
                this.get_Label().innerHTML = descricao;
            }
        }
    },
    this.RetornaTr = function () {
        var tds = [];
        var div = this.get_DivLista();

        if (div.childNodes.length > 0) {
            var tbDados = div.childNodes.length > 1 ? div.childNodes.item(1).childNodes.item(0) : div.childNodes.item(0); //IE8

            for (var i = 0; tbDados.childNodes.length > i; i++) {
                var tr = tbDados.childNodes.item(i);

                for (var a = 0; tr.childNodes.length > a; a++) {
                    tds[tds.length] = tr.childNodes.item(a);
                }
            }
        }

        return tds;
    },

    this.AddHandlerLista = function () {
        var tds = this.RetornaTr();

        for (var i = 0; tds.length > i; i++) {
            var td = tds[i];

            var clickLista = Function.createDelegate(this, this.ClickLista);
            $addHandler(td, 'click', clickLista);

        }
    },

    this.ClearHandlersLista = function () {
        var tds = this.RetornaTr();

        for (var i = 0; tds.length > i; i++) {
            var td = tds[i];

            $clearHandlers(td);
        }
    },

    this.ClickLista = function (arg, b) {
        var td = arg.target;
        var id = td.getAttribute("key");

        this.get_TextBox().value = id;

        this.SelecionaValor(true);
    }
}



Employer.Componentes.UI.Web.ListaSugestao.prototype.SelecionaValor = function (click) {
    if (this.get_TipoSugestao() == 2) {
        this.get_TextBox().value = this.get_TextBox().value.toUpperCase();
    }

    this.set_Descricao(this.get_Dicionario()[this.get_TextBox().value]);
    //var val = this.get_Validador();
    //ValidatorValidate(val);
    if (this.MostrarErros()) {
        if (this.get_IsPostBack() == true && this.get_TextBox().value != this.get_TextoAntesFocus()
        || this.get_IsServerBlur() == true) {

            //Foco automático
            if (typeof (Employer_Componentes_UI_Web_AutoTabIndex) != "undefined")
                Employer_Componentes_UI_Web_AutoTabIndex.Set_Focus_Server(this.get_TextBox().id);

            __doPostBack(this._id, 'Change');
        }
        else if (click) {
             if (typeof (Employer_Componentes_UI_Web_AutoTabIndex_NextFocus) != "undefined")
                 Employer_Componentes_UI_Web_AutoTabIndex_NextFocus();
        }
    }
}

Employer.Componentes.UI.Web.ListaSugestao.Esconde = function (comp) {
    comp.esconderLista();
    comp.SelecionaValor();
}

Employer.Componentes.UI.Web.ListaSugestao.prototype.Blur = function () {

    var cp = this;

    window.setTimeout(

    function () {
        Employer.Componentes.UI.Web.ListaSugestao.Esconde(cp);
    }
    , 200);
}

Employer.Componentes.UI.Web.ListaSugestao.prototype.Focus = function () {
    this._textoAntesFocus = this.get_TextBox().value;
    this.mostrarLista();
    try {
        this.get_TextBox().select();
    } catch (e) {
        /*Eu sei que isso é gambiarra, mas se nem o javascript sabe qual é o erro o que eu posso fazer??? :'( */
    }
}
Employer.Componentes.UI.Web.ListaSugestao.prototype.KeyPress = function (e) {

    var key = AjaxClientDataBoundControlBase.Key.GetKeyCode(e.rawEvent);

    if (e.rawEvent.keyCode && AjaxClientDataBoundControlBase.Key.isCommand(key))
        return true;

    if (key == Sys.UI.Key.space) {
        e.preventDefault();
        return false;
    }

    var caracter = String.fromCharCode(key);

    if (this.get_TipoSugestao() == 1) {
        //É letra
        if (isNaN(caracter))
            return AjaxClientDataBoundControlBase.CancelEvent(e);
    }
}

/*if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();*/

Employer.Componentes.UI.Web.ListaSugestao.prototype.Validar = function (arg) {
    arg.Value = this.get_TextBox().value;

    var validator = this.get_Validador();
    var div = this.get_PnlValidador();

    arg.IsValid = false;
    if (arg.Value == "" || arg.Value == null) { 
        if (this.get_Obrigatorio()){
            validator.innerHTML = this.get_MensagemErroObrigatorio();
            validator.errormessage = this.get_MensagemErroObrigatorioSummary();
        }
        else
            arg.IsValid = true;
    }
    else {
        if (this.get_TextBox().value != "") {
            if (this.get_Dicionario()[this.get_TextBox().value] == null){
                validator.innerHTML = this.get_MensagemErroInvalido();
                validator.errormessage = this.get_MensagemErroInvalidoSummary();
            }
            else {
                arg.IsValid = true;
            }
        }
        else
            arg.IsValid = true;
    }

    if (arg.IsValid) {
        this.get_Validador().style.display = "none"
    }
    else {
        this.get_Validador().style.display = "block"

    }
}

Employer.Componentes.UI.Web.ListaSugestao.prototype.MostrarErros = function () {
    var validator = this.get_Validador();
    var div = this.get_PnlValidador();

    var IsValid = false;

    if (this.get_TextBox().value == "" || this.get_TextBox().value == null) {
        /*if (this.get_Obrigatorio())
        validator.innerHTML = this.get_MensagemErroObrigatorio();
        else*/
        IsValid = true;
    }
    else {
        if (this.get_TextBox().value != "") {
            if (this.get_Dicionario()[this.get_TextBox().value] == null) {
                validator.innerHTML = this.get_MensagemErroInvalido();
                validator.errormessage = this.get_MensagemErroInvalidoSummary();
            }
            else {
                IsValid = true;
            }
        }
        else
            IsValid = true;
    }
    if (IsValid) {
        this.get_Validador().style.display = "none";
    }
    else {
        this.get_Validador().style.display = "block";
        this.get_Validador().style.visibility = "visible";
    }
    return IsValid;
}

Employer.Componentes.UI.Web.ListaSugestao.ValidarTextBox = function (sender, args) {
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

// Register the class as a type that inherits from Sys.UI.Control.
Employer.Componentes.UI.Web.ListaSugestao.registerClass('Employer.Componentes.UI.Web.ListaSugestao', Sys.UI.Behavior);