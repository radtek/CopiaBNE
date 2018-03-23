// Declare a namespace.
Type.registerNamespace("Employer.Componentes.UI.Web");

// Define a simplified component.
Employer.Componentes.UI.Web.ControlTelefone = function (element) {
    Employer.Componentes.UI.Web.ControlTelefone.initializeBase(this, [element]);

    this._id = element.id;

    // Handlers for the events    
    this._onBlur = null;
    this._onFocus = null;
    this._textoAntesFocus = "";
    this._onKeyPress = null;
    this._tipo = null;

    this._Ddd9 = [
            12, 13, 14, 15, 16, 17, 18, 19, 11,
            21, 22, 24, 27, 28,
            //41, 42, 43, 44, 45, 46, 47, 48, 49, 51, 53, 54, 55, 
            61, 62, 63, 64, 65, 66, 67, 68, 69, //Até 31/2016
            31, 32, 33, 34, 35, 37, 38, 71, 73, 74, 75, 77, 79, //Novo até 31/2015
            81, 82, 83, 84, 85, 86, 87, 88, 89,
            91, 92, 93, 94, 95, 96, 97, 98, 99
    ];

    // Compatibilidade com ControlBaseValidator
    this._mensagemErroValorMinimo = null;
    this._mensagemErroValorMaximo = null;
    this._mensagemErroFormato = null;

    // Summary
    this._MensagemErroObrigatorioSummary = null;
    this._MensagemErroFormatoSummary = null;
    this._MensagemErroValorMinimoSummary = null;
    this._MensagemErroValorMaximoSummary = null;

    this.get_MensagemErroFormato = function () {
        return this._mensagemErroFormato;
    },
    this.set_MensagemErroFormato = function (valor) {
        this._mensagemErroFormato = valor;
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
    this.get_Tipo = function () {
        return this._tipo;
    },
    this.set_Tipo = function (valor) {
        this._tipo = valor;
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
         Employer.Componentes.UI.Web.ControlTelefone.callBaseMethod(this, 'initialize');

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

        Employer.Componentes.UI.Web.ControlTelefone.callBaseMethod(this, 'dispose');
    },
    this.get_TextoAntesFocus = function () {
        return this._textoAntesFocus;
    }
}

//Simula herança
Employer.Componentes.UI.Web.ControlTelefone.prototype = new Employer.Componentes.UI.Web.ControlBaseValidator;



Employer.Componentes.UI.Web.ControlTelefone.prototype.Blur = function () {

    var valorAtual = this.get_TextBox().value;

    var ddd = this.get_TextBox().value.substr(0, 2);
    if (AjaxClientControlBase.Key.isExistsList(this._Ddd9, ddd ) && this.get_Tipo() == 1) {
        // regra do ddd área 11 - Para celulares
        if (this.get_TextBox().value.length < 11 &&
            !AjaxClientControlBase.IsNullOrEmpty(this.get_TextBox().value.substr(3))
             ) {
            this.get_TextBox().value = this.get_TextBox().value.substr(0, 2) + "9" + this.get_TextBox().value.substr(2)
        }
        this.get_TextBox().value = this.FormatarMascara(this.get_TextBox().value, "(99) 99999-9999");
    } else {
        // Demais localidades
        this.get_TextBox().value = this.FormatarMascara(this.get_TextBox().value, "(99) 9999-9999");
    }

    this.PostPadrao(valorAtual);
}

Employer.Componentes.UI.Web.ControlTelefone.prototype.KeyPress = function (e) {
    var key = AjaxClientControlBase.Key.GetKeyCode(e.rawEvent);

    if (e.rawEvent.keyCode && AjaxClientControlBase.Key.isCommand(key))
        return true;

    var caracter = String.fromCharCode(key);

    if (isNaN(caracter))
        return AjaxClientControlBase.CancelEvent(e);


    return true;
}

Employer.Componentes.UI.Web.ControlTelefone.prototype.LimparFone = function (fone) {
    var valor = new String(fone);
    valor = valor.replace("(", "");
    valor = valor.replace(")", "");
    valor = valor.replace(" ", "");
    valor = valor.replace("-", "");
    return this.trim(valor);
}


Employer.Componentes.UI.Web.ControlTelefone.prototype.FormatarMascara = function (s, mascara) {

    var stringToFormat = new String(this.LimparFone(s.toString()));
    var tamString = stringToFormat.length;
    var mc = 0;
    var i = 0;
    var result = "";

    while (i < tamString) {
        if (mc >= mascara.length)
            break;

        if (mascara.charAt(mc) == '9') {
            if (!isNaN(stringToFormat.charAt(i))) {
                result += stringToFormat.charAt(i);
                mc++;
                i++;
            }
            else
                i++;
        }
        else
            if (mascara.charAt(mc) == '#') {
                result += stringToFormat.charAt(i);
                mc++;
                i++;
            }
            else {
                result += mascara.charAt(mc);
                mc++;
            }
    }

    if (result.length == 1)
        return "";
    return result;
}


Employer.Componentes.UI.Web.ControlTelefone.prototype.Focus = function () {
    this.get_TextBox().value = this.LimparFone(this.get_TextBox().value);
    this._textoAntesFocus = this.get_TextBox().value;
    try {
        this.get_TextBox().select();
    } catch (e) {
    }
}




Employer.Componentes.UI.Web.ControlTelefone.registerClass('Employer.Componentes.UI.Web.ControlTelefone', Sys.UI.Behavior);

/*if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();*/


Employer.Componentes.UI.Web.ControlTelefone.prototype.ValidarTelefone = function () {

    var fone = new String(this.LimparFone(this.get_TextBox().value));
    //Pega o primeiro número do telefone
    var numero = fone.substr(2, 1);

    if (fone.length < 10)
        return false;

    // Número inválido
    if (numero == 1)
        return false;

    var ddd = fone.substr(0, 2);
    var preficoCelular = [2, 3, 4, 5];
    var execao = ddd == 11 ? [ 5 ] : [];

    if (AjaxClientControlBase.Key.isExistsList(preficoCelular, numero))
        return AjaxClientControlBase.Key.isExistsList(execao, numero) || this.get_Tipo() == 0;
    else
        return AjaxClientControlBase.Key.isExistsList(execao, numero) || this.get_Tipo() == 1;
}

Employer.Componentes.UI.Web.ControlTelefone.prototype.MostrarErros = function () {
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
        IsValid = this.ValidarTelefone();
        if (IsValid == false) {
            validator.innerHTML = this.get_MensagemErroFormato();
            validator.errormessage = this.get_MensagemErroFormatoSummary();
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
}

Employer.Componentes.UI.Web.ControlTelefone.prototype.Validar = function (arg) {
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
        arg.IsValid = this.ValidarTelefone();
        if (arg.IsValid == false) {
            validator.innerHTML = this.get_MensagemErroFormato();
            validator.errormessage = this.get_MensagemErroFormatoSummary();
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


Employer.Componentes.UI.Web.ControlTelefone.ValidarTextBox = function (sender, args) {
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
