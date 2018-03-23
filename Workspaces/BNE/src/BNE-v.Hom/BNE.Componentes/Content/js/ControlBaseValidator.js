
Type.registerNamespace("BNE.Componentes");

BNE.Componentes.ControlBaseValidator = function() {
    this._Obrigatorio = false;

    this._MensagemErroObrigatorio = null;
    this._MensagemErroFormato = null;
    this._MensagemErroValorMinimo = null;
    this._MensagemErroValorMaximo = null;

    // Summary
    this._MensagemErroObrigatorioSummary = null;
    this._MensagemErroFormatoSummary = null;
    this._MensagemErroValorMinimoSummary = null;
    this._MensagemErroValorMaximoSummary = null;


    this._isPostBack = false;
    this._ValorAlteradoClient = null;
    this._isValorAlterado = null;

};
BNE.Componentes.ControlBaseValidator.prototype = {
    get_isValorAlterado: function() {
        return this._isValorAlterado;
    },
    set_isValorAlterado: function(value) {
        this._isValorAlterado = value;
    },
    get_isPostBack: function() {
        return this._isPostBack;
    },
    set_isPostBack: function(value) {
        this._isPostBack = value;
    },
    get_ValorAlteradoClient: function() {
        return this._ValorAlteradoClient;
    },
    set_ValorAlteradoClient: function(value) {
        this._ValorAlteradoClient = value;
    },
    get_Obrigatorio: function() {
        return this._Obrigatorio;
    },
    set_Obrigatorio: function(value) {
        this._Obrigatorio = value;
    },
    get_MensagemErroObrigatorio: function() {
        return this._MensagemErroObrigatorio;
    },
    get_MensagemErroFormato: function() {
        return this._MensagemErroFormato;
    },

    //Necessário pois ele usa terminador quando usado dentro de grid
    get_Campo: function(prefixo) {
        var espGrid = RegExp("(_[0-9]+)$");
        var m = espGrid.exec(this._id);

        if (m != null)
            return $get(this._id + prefixo + m[0]);
        else
            return $get(this._id + prefixo);
    },
    get_Controle: function(prefixo) {
        var espGrid = RegExp("(_[0-9]+)$");
        var m = espGrid.exec(this._id);

        if (m != null)
            return $find(this._id + prefixo + m[0]);
        else
            return $find(this._id + prefixo);
    },

    get_Validator: function() {
        return this.get_Campo("_cvValor");
    },
    get_PainelValidador: function() {
        return this.get_Campo("_pnlValidador");
    },

    set_MensagemErroObrigatorio: function(value) {
        this._MensagemErroObrigatorio = value;
    },
    set_MensagemErroFormato: function(value) {
        this._MensagemErroFormato = value;
    },
    set_MensagemErroValorMinimo: function(value) {
        this._MensagemErroValorMinimo = value;
    },
    set_MensagemErroValorMaximo: function(value) {
        this._MensagemErroValorMaximo = value;
    },
    trim: function(valor) {
        return valor.replace(/^\s+|\s+$/g, "");
    },
    has_selected_text: function(element) {
        var value = true;
        if (document.selection) {
            //IE support
            element.focus();
            sel = document.selection.createRange();
            if (sel.text == "") {
                value = false;
            }
        } else if (typeof element.selectionStart == "number") {
            // selectionStart == 0 é false então é melhor verificar a tipagem da propriedade
            //MOZILLA/NETSCAPE support
            var startPos = element.selectionStart;
            var endPos = element.selectionEnd;
            if (startPos == endPos) {
                value = false;
            }
        }
        return value;
    },

    //Summary
    get_MensagemErroObrigatorioSummary: function() {
        if (AjaxClientControlBase.IsNullOrEmpty(this._MensagemErroObrigatorioSummary) == false)
            return this._MensagemErroObrigatorioSummary;

        return this.get_MensagemErroObrigatorio();
    },
    set_MensagemErroObrigatorioSummary: function(value) {
        this._MensagemErroObrigatorioSummary = value;
    },

    get_MensagemErroFormatoSummary: function() {
        if (AjaxClientControlBase.IsNullOrEmpty(this._MensagemErroFormatoSummary) == false)
            return this._MensagemErroFormatoSummary;

        return this.get_MensagemErroFormato();
    },
    set_MensagemErroFormatoSummary: function(value) {
        this._MensagemErroFormatoSummary = value;
    },

    get_MensagemErroValorMinimoSummary: function() {
        if (AjaxClientControlBase.IsNullOrEmpty(this._MensagemErroValorMinimoSummary) == false)
            return this._MensagemErroValorMinimoSummary;

        return this.get_MensagemErroValorMinimo();
    },
    set_MensagemErroValorMinimoSummary: function(value) {
        this._MensagemErroValorMinimoSummary = value;
    },

    get_MensagemErroValorMaximoSummary: function() {
        if (AjaxClientControlBase.IsNullOrEmpty(this._MensagemErroValorMaximoSummary) == false)
            return this._MensagemErroValorMaximoSummary;

        return this.get_MensagemErroValorMaximo();
    },
    set_MensagemErroValorMaximoSummary: function(value) {
        this._MensagemErroValorMaximoSummary = value;
    },


    PostPadrao: function(valorAtual) {
        var read = this.get_TextBox().getAttribute("readonly");

        if (!AjaxClientControlBase.IsNullOrEmpty(read)) {
            return false;
        }

        var valor = valorAtual ? valorAtual : this.get_TextBox().value;

        //Foi Alterado valor
        if (valor != this.get_TextoAntesFocus()) {
            if (this.get_ValorAlteradoClient() != null) {
                eval(this.get_ValorAlteradoClient() + "($find('" + this._id + "'));");
            }
        } else if (this.get_isValorAlterado()) {
            return false;
        }

        var val = this.get_Validator();
        ValidatorEnable(val);

        if (val.isvalid) {
            if (this.get_isPostBack() == true) {
                var argumento = "";
                if (this.get_TextBox().value != "")
                    argumento = this.get_TextBox().id;
                __doPostBack(this.get_TextBox().id, argumento);
                return true;
            }
        }

        return false;
    }
};