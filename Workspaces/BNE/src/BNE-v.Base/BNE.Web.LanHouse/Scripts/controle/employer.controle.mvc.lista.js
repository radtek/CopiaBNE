employer.controle.mvc.listaTipo = {
    Texto: 0,
    Numero: 1,
    TextoMaiusculo: 2
}

employer.controle.mvc.lista = function (elemento, lista, tipoSugestao, callbackChange, cssPnlOculto, csslblDescricao) {

    var _campo = elemento;

    this._callbackChange = callbackChange;
    this._descricao = null;
    this._mostrarDescricao = true;
    this._CssPnlOculto = cssPnlOculto;
    this._CsslblDescricao = csslblDescricao;
    this._textoAntesFocus = _campo.val();
    this._tipoSugestao = tipoSugestao ? tipoSugestao : employer.controle.mvc.listaTipo.Texto;

    this.get_TipoSugestao = function () {
        return this._tipoSugestao;
    },
    this.set_TipoSugestao = function (value) {
        this._tipoSugestao = value;
    },
    this.get_DivConteine = function () {
        var id = _campo.attr("id");
        return $("#" + id + "_pnlConteiner");
    },
    this.get_DivLista = function () {
        var id = _campo.attr("id");
        return $("#" + id + "_pnlOculto");
    },
    this.get_TextoAntesFocus = function () {
        return this._textoAntesFocus;
    },

    this.CriarCampos = function () {
        var id = _campo.attr("id");

        var max = 1;
        for (var i = 0; lista.length > i; i++) {
            var ls = lista[i].key.toString().length;
            if (ls > max)
                max = ls;
        }

        _campo.attr("maxlength", max);
        _campo.attr("size", max);

        var pnlConteiner = $("<div id='" + id + "_pnlConteiner'></div>");
        var lblDescricao = $("<span id='" + id + "_lblDescricao'class='" + this._CsslblDescricao + "'></span>");
        var pnlOculto = $("<div id='" + id + "_pnlOculto' style='display:none;' class='" + this._CssPnlOculto + "'></div>");

        _campo.after(pnlConteiner);
        pnlConteiner.append(lblDescricao);
        pnlConteiner.append(pnlOculto);
    },

    this.CriarLista = function () {
        var id = _campo.attr("id");

        var html = "<table id='" + id + "_tabela'>";
        html += "<tbody>";

        for (var i = 0; lista.length > i; i++) {
            var ls = lista[i];
            html += "<tr>";
            html += "<td class='lsg_col1' key='" + ls.key + "'>";
            html += ls.key;
            html += "</td>";
            html += "<td class='lsg_col2' key='" + ls.key + "'>";
            html += ls.value;
            html += "</td>";
            html += "</tr>";
        }
        html += "</tbody>";
        html += "</table>";

        var id = _campo.attr("id");
        id = id + "_pnlOculto";

        //html = $(html);

        //thi.insertAfter(html);
        this.get_DivLista().html(html);

        //ClickLista
    },

    this.get_TextBox = function () {
        return _campo;
    },
    this.get_Label = function () {
        var id = _campo.attr("id");
        return $("#" + id + "_lblDescricao");
    },
    this.get_MostrarDescricao = function () {
        return this._mostrarDescricao;
    },
    this.set_MostrarDescricao = function (value) {
        this._mostrarDescricao = value;
    },
    this.mostrarDescricao = function (descricao) {
        if (this.get_Label() == null)
            return;

        this.get_Label().show();
        if (this.get_MostrarDescricao()) {
            if (descricao !== undefined) {
                this.get_Label().text(descricao);
            }
        }
    },
    this.set_Descricao = function (value) {
        this._descricao = value;
        this.mostrarDescricao(value);
    },


    this.esconderLista = function () {
        var element = this.get_DivLista();
        if (element) {
            element.hide();
        }
    },
    this.mostrarLista = function () {
        var element = this.get_DivLista();
        element.show();
        try { this.set_Descricao(""); }
        catch (ex) { }
    },

    this.SelecionaValor = function (click) {
        if (this.get_TipoSugestao() == employer.controle.mvc.listaTipo.TextoMaiusculo) {
            this.get_TextBox().val(this.get_TextBox().value.toUpperCase());
        }

        var desc = "";
        for (var i = 0; lista.length > i; i++) {
            var ls = lista[i];
            if (ls.key == this.get_TextBox().val()) {
                desc = ls.value;
                break;
            }
        }

        this.set_Descricao(desc);

        if (this._callbackChange != undefined && this.get_TextBox().val() != this.get_TextoAntesFocus()) {
            this._textoAntesFocus = this.get_TextBox().val();
            this._callbackChange(this.get_TextBox(), this);
        }
    },

    this.ClickLista = function (td) {
        var id = td.attr("key");

        this.get_TextBox().val(id);

        this.SelecionaValor(true);
    },

    this.Esconde = function () {
        this.esconderLista();
        this.SelecionaValor();
    },

    this.Focus = function () {
        this._textoAntesFocus = this.get_TextBox().val();
        this.mostrarLista();
        try {
            this.get_TextBox().select();
        } catch (e) {

        }
    },

    this.KeyPress = function (e) {

        var key = employer.event.getKey(e);

        key = key[0] != null ? key[0] : key[1];

        if (employer.key.isCommand(key))
            return true;

        if (key == employer.key._commands.ESCAPE) {
            e.preventDefault();
            return false;
        }

        var caracter = String.fromCharCode(key);

        if (this.get_TipoSugestao() == employer.controle.mvc.listaTipo.Numero) {
            //É letra
            if (isNaN(caracter)) {
                e.preventDefault();
                e.stopPropagation();
                return false;
            }
        }
    },

    this.Blur = function () {
        var cp = this;
        window.setTimeout(
        function () {
            cp.Esconde();
        }
        , 200);
    },

    this.Iniciar = function () {
        var ctr = this;

        _campo.unbind("blur").blur(function (ev) {
            ev.preventDefault();
            ev.stopPropagation();
            //console.log("blur")
            ctr.Blur();
        });
        _campo.unbind("focus").focus(function () {
            ctr.Focus();
        });
        _campo.unbind("keypress").keypress(function (event) {
            return ctr.KeyPress(event);
        });

        this.CriarCampos();
        this.CriarLista();
        this.SelecionaValor();

        var inst = this;

        var id = _campo.attr("id");
        $("#" + id + "_tabela td").unbind("click").click(function (ev, el) {
            //console.log("click")
            ev.preventDefault();
            ev.stopPropagation();
            inst.ClickLista($(this));
        });
    },

    this.Iniciar();
}