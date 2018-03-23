Type.registerNamespace("BNE.Componentes");

BNE.Componentes.ContagemCaracteres = function (element) {
    BNE.Componentes.ContagemCaracteres.initializeBase(this, [element]);

    this._id = element.id;
    this._ControlToValidate = null;
    this._MaxLength = null;
    this._Label = null;
    this._PatternText = null;

    this.get_ControlToValidate = function () {
        return this._ControlToValidate;
    },
    this.set_ControlToValidate = function (value) {
        this._ControlToValidate = value;
    },
    this.get_MaxLength = function () {
        return this._MaxLength;
    },
    this.set_MaxLength = function (value) {
        this._MaxLength = value;
    },
    this.get_Label = function () {
        return this._Label;
    },
    this.set_Label = function (value) {
        this._Label = value;
    },
    this.get_PatternText = function () {
        return this._PatternText;
    },
    this.set_PatternText = function (value) {
        this._PatternText = value;
    }
}

BNE.Componentes.ContagemCaracteres.prototype.initialize = function () {
    this.InitializeContador();
    BNE.Componentes.ContagemCaracteres.callBaseMethod(this, "initialize");
}

BNE.Componentes.ContagemCaracteres.prototype.dispose = function () {
    BNE.Componentes.ContagemCaracteres.callBaseMethod(this, "dispose");
}

BNE.Componentes.ContagemCaracteres.prototype.Contador = function () {
    var target = $('#' + this._ControlToValidate);
    var maxlength = this._MaxLength;
    var label = $('#' + this._Label);
    var pattern = this._PatternText;
    var r = true;
    if (target.val().length >= maxlength) {
        target.val(target.val().substring(0, maxlength));
        r =  false;
    }

    if (label) {
        label.html(pattern.replace('{0}', maxlength - target.val().length));
    }

    return r;
}

BNE.Componentes.ContagemCaracteres.prototype.InitializeContador = function () {
    var componentInstance = this;

    $('#' + this._ControlToValidate).keyup(function () {
        return componentInstance.Contador();
    });

    this.Contador();
}

BNE.Componentes.ContagemCaracteres.registerClass("BNE.Componentes.ContagemCaracteres", Sys.UI.Control);