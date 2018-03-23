Type.registerNamespace("Employer.Componentes.UI.Web");

// Define a simplified component.
Employer.Componentes.UI.Web.EmployerListaConfColunas = function (element) {
    Employer.Componentes.UI.Web.EmployerListaConfColunas.initializeBase(this, [element]);

    this._id = element.id;

    this._ConfArquivo = null;
    this._Modelo = [];

    //Enum
    this.EnumTipoArquivo = { Excel: 3, Csv: 2, Posicional: 1 };
    this.EnumTipoLista = { Arquivo: 1, Configuracao: 2 };

    this.tagLinha = "<div class='linha'><label><span>*</span>{0}</label><div class='container'>{1}</div></div>";

    this.get_DivPai = function () {
        return this.get_Campo("_pnlTabela");
    },
    this.get_CssTabela = function () {
        var div = this.get_DivPai();
        return $(div).attr("class");
    },
    this.AdicionaClass = function (classe, busca) {
        var css = this.get_CssTabela();

        var index = css.indexOf(" ");
        var cssBase = css;
        var cssComplemento = ""
        if (index > -1) {
            cssBase = css.substring(0, index);
            cssComplemento = AjaxClientDataBoundControlBase.ReplaceAll(" ", "", css.substring(index));
        }

        if (busca) {
            if (AjaxClientDataBoundControlBase.IsNullOrEmpty(cssComplemento))
                return cssBase + classe;
            return cssBase + classe + "." + cssComplemento;
        }
        return cssBase + classe + " " + cssComplemento;
    },
    this.get_Campo = function (nome) {
        var espGrid = RegExp("(_[0-9]+)$");
        var m = espGrid.exec(this._id);

        if (m != null)
            return $get(this._id + nome + m[0]);
        else
            return $get(this._id + nome);
    },

    this.CarregarMetaDados = function () {
        var hdnMetaDados = this.get_Campo("_hdnMetaDados");
        var val = hdnMetaDados.value;

        this._Modelo = Sys.Serialization.JavaScriptSerializer.deserialize(val);
    },
    this.AtualizarMetaDados = function () {
        var hdnMetaDados = this.get_Campo("_hdnMetaDados");
        hdnMetaDados.value =
            Sys.Serialization.JavaScriptSerializer.serialize(this._Modelo);
    },
    this.get_Dom_Colunas = function () {
        return $(this.get_Campo("_divColunas"));
    },
    this.get_Dom_Colunas_Divs = function () {
        return this.get_Dom_Colunas().find("." + this.AdicionaClass("_Coluna", true));
    },
    this.get_Dom_Coluna = function (idColuna) {
        return this.get_Dom_Colunas().find(String.format("div[name='{0}']", idColuna));
    },
    this.get_Coluna_Index = function (idColuna) {
        for (var i = 0; this._Modelo.Colunas.length > i; i++) {
            var c = this._Modelo.Colunas[i];
            if (c.Id == idColuna)
                return i;
        }

        return -1;
    },
    this.get_Coluna = function (idColuna) {
        var index = this.get_Coluna_Index(idColuna);

        return index > -1 ? this._Modelo.Colunas[index] : null;
    },
    this.CriarColunaAppend = function (coluna) {
        var divColunas = this.get_Dom_Colunas();
        var col = this.CriarColuna(coluna);
        divColunas.append(col);
        Array.add(this._Modelo.Colunas, coluna);
    },
    this.LimparEventosColuna = function (idColuna) {
        var domCol = this.get_Dom_Coluna(idColuna);
        var domtitulo = domCol.find("." + this.AdicionaClass("_Coluna_Titulo", true));

        if (domtitulo.length > 0) {
            $clearHandlers(domtitulo[0]);

            var text = domtitulo.find("input");
            for (var i = 0; text.length > i; i++)
                $clearHandlers(text[i]);
        }

        var domConf = domCol.find("." + this.AdicionaClass("_Coluna_BoxConf", true));
        if (domConf.length > 0) {
            $clearHandlers(domConf[0]);

            var text = domConf.find("input");
            for (var i = 0; text.length > i; i++)
                $clearHandlers(text[i]);

            text = domConf.find("select");
            for (var i = 0; text.length > i; i++)
                $clearHandlers(text[i]);
        }
    },

    this.ExcluirColuna = function (idColuna) {
        var index = this.get_Coluna_Index(idColuna);
        if (index > -1) {
            var domCol = this.get_Dom_Coluna(idColuna);

            this.LimparEventosColuna(idColuna);
            domCol.remove();
            Array.removeAt(this._Modelo.Colunas, index);
        }
    },
    this.OrdenarMetadadosColunas = function () {
        var divs = this.get_Dom_Colunas_Divs();

        for (var i = 0; divs.length > i; i++) {
            var id = $(divs[i]).attr("name");
            var coluna = this.get_Coluna(id);
            coluna.Ordem = i;
        }

        this._Modelo.Colunas.sort(function (c1, c2) {
            return c1.Ordem - c2.Ordem
        });
    },

    this.CriarColuna = function (coluna) {
        //console.log("CriarColuna");

        var divColuna = $(String.format("<div name='{0}' class='{1}'>{2}</div>",
                coluna.Id, this.AdicionaClass("_Coluna"),
                (coluna.Obrigatorio ? "<span class='obrigatorio'>*</span>" : "")
                ));

        if (this._Modelo.TipoLista == this.EnumTipoLista.Configuracao) {
            var divTitulo = null;
            if (coluna.Edicao) {
                divTitulo = $(
                    String.format("<div class='{0}'><input type='text' name='titulo' value='{1}' /><input type='button'  class='btnSalvar' title='salvar' /></div>",
                        this.AdicionaClass("_Coluna_Titulo"),
                        coluna.Titulo
                        )
                );
            }
            else {
                divTitulo = $(
                            String.format("<div class='{0}'><span>{1}</span>{2}</div>",
                                this.AdicionaClass("_Coluna_Titulo"),
                                coluna.Titulo,
                                (this._Modelo.TipoLista == this.EnumTipoLista.Configuracao ? "<input type='button' class='btnEditar' title='editar' maxlength='50' />" : "")
                                )
                        );
            }
            divColuna.append(divTitulo);

            if (this._Modelo.PermiteEdicao) {
                if (coluna.Edicao) {
                    var salvarTitulo = Function.createDelegate(this, this.SalvarTitulo);

                    var click = divTitulo.find("input[type='button']")[0];
                    var input = divTitulo.find("input[name='titulo']")[0];
                    $addHandler(click, 'click', salvarTitulo);

                    var salvarEnterTitulo = Function.createDelegate(this, this.EnterSalvarTitulo);
                    $addHandler(input, 'keypress', salvarEnterTitulo);
                }
                else {
                    var click = Function.createDelegate(this, this.ClickTitulo);
                    $addHandler(divTitulo[0], 'click', click);
                }
            }
        }

        var divPadrao =
            $(String.format("<div class='{0}'></div>", this.AdicionaClass("_Coluna_BoxConf")));
        divColuna.append(divPadrao);

        if (this._Modelo.TipoLista == this.EnumTipoLista.Arquivo) {
            divPadrao.html(coluna.Titulo);
        }
        else {
            var spTConf = $("<span>" + coluna.Titulo + "</span>");
            //divPadrao.append(spTConf);

            if (this._Modelo.TipoArquivo == this.EnumTipoArquivo.Posicional) {
                var linha;
                if (coluna.Edicao) {
                    linha = $(String.format(this.tagLinha, "Tamanho", "<input type='text' name='tamanho' size='4' maxlength='4' />"))
                    divPadrao.append(linha);

                    var campo = linha.find("input");
                    campo.spinner({ min: 0, max: 1000 });
                    campo.val(coluna.Tamanho);

                    linha = $(String.format(this.tagLinha, "Tipo", "<select name='tipo' />"))
                    divPadrao.append(linha);

                    campo = linha.find("select");
                    campo.append($("<option value=''>Texto</option>"));
                    campo.append($("<option value='1'>Numérico</option>"));
                    campo.append($("<option value='2'>Data</option>"));
                    campo.val(coluna.Tipo);

                    var change = Function.createDelegate(this, this.SelecionaTipo);
                    $addHandler(campo[0], 'change', change);

                    this.CriarLinhaFormato(divPadrao, campo, false);
                }
            }
        }

        return divColuna;
    },

    this.SalvarTitulo = function (ev) {
        ev.preventDefault();
        ev.stopPropagation();

        var divClick = $(ev.target).parent().parent();
        var idColuna = divClick.attr("name");

        var input = divClick.find("input[name='titulo']");
        var cTamanho = divClick.find("input[name='tamanho']");
        var cTipo = divClick.find("select[name='tipo']");
        var cFormato = divClick.find("input[name='formato']");
        var cAlinhamento = divClick.find("input[name='alinhamento']:checked");
        var cTipoSeparador = divClick.find("input[name='tiposeparador']:checked");
        var cSeparador = divClick.find("input[name='separador']");

        var objColuna = this.get_Coluna(idColuna);

        objColuna.Titulo = input.val();
        objColuna.Tamanho = cTamanho.val();
        objColuna.Tipo = cTipo.val();
        objColuna.Formato = cFormato.val();
        objColuna.Alinhamento = cAlinhamento.val();
        objColuna.TipoSeparador = cTipoSeparador.val();
        objColuna.Separador = cSeparador.val();
        objColuna.Edicao = false;

        this.LimparEventosColuna(idColuna);
        divClick.replaceWith(this.CriarColuna(objColuna));

        this.AtualizarMetaDados();
    },

    //Impede disparar o Default button e dispara o ClickTitulo com o blur do campo
    this.EnterSalvarTitulo = function (ev) {
        var key = AjaxClientControlBase.Key.GetKeyCode(ev.rawEvent);

        if (key == AjaxClientControlBase.Key.Commands.ENTER) {
            AjaxClientControlBase.CancelEvent(ev);
            $(ev.target).blur();
        }
    },

    this.ClickTitulo = function (ev) {
        ev.preventDefault();
        ev.stopPropagation();

        var divClick = $(ev.target).parent().parent();
        var idColuna = divClick.attr("name");
        var objColuna = this.get_Coluna(idColuna);

        objColuna.Edicao = true;
        var colHtml = this.CriarColuna(objColuna);

        this.LimparEventosColuna(idColuna);
        divClick.replaceWith(colHtml);

        var input = colHtml.find("div input[name='titulo']");
        //input.focus();

        if (input.val().length > 0) {
            var fim = input.val().length;
            //Coloca o cursor no final do texto
            AjaxClientControlBase.SetInputSelection(input[0], fim, fim);
        }
    },

    this.SelecionaAlinhamento = function (ev) {
        var rb = $(ev.target);

        var divFormato = rb.parent().parent().parent();
        var inpFormato = divFormato.find("input[name='formato']");

        if (rb.val() == 'esquerda') {
            inpFormato.css("text-align", "left");
        }
        else {
            inpFormato.css("text-align", "right");
        }
    },

    this.CriarLinhaAlinhamento = function (divBoxConf, novo) {
        var divAlinhamento = divBoxConf.find("div[name='divAlinhamento']");
        var idColuna = divBoxConf.parent().attr("name");
        var objColuna = this.get_Coluna(idColuna);

        if (divAlinhamento.length > 0)
            divAlinhamento.remove();

        divAlinhamento = $(String.format(this.tagLinha, "Alinhamento", "<input type='radio' name='alinhamento' value='esquerda'  />Esquerda <input type='radio' name='alinhamento' value='direita' />Direita"));
        divBoxConf.append(divAlinhamento);
        divAlinhamento.attr("name", "divAlinhamento");

        if (novo)
            objColuna.Alinhamento = 'esquerda';

        var divFormato = divBoxConf.find("div[name='divFormato']");
        var inpFormato = divFormato.find("input[name='formato']");

        var inpAlinhamento = divAlinhamento.find("input[name='alinhamento']");
        var rbEsquerda = inpAlinhamento[0];
        var rbDireita = inpAlinhamento[1];

        if (objColuna.Alinhamento == 'esquerda') {
            $(rbEsquerda).attr("checked", true);
            inpFormato.css("text-align", "left");
        }
        else {
            $(rbDireita).attr("checked", true);
            inpFormato.css("text-align", "right");
        }

        var click = Function.createDelegate(this, this.SelecionaAlinhamento);
        $addHandler(rbEsquerda, 'click', click);
        $addHandler(rbDireita, 'click', click);
    },

    this.CriarLinhaFormato = function (divBoxConf, combo, novoFormato) {
        var divFormato = divBoxConf.find("div[name='divFormato']");
        var idColuna = divBoxConf.parent().attr("name");
        var objColuna = this.get_Coluna(idColuna);

        if (divFormato.length > 0)
            divFormato.remove();

        var divAlinhamento = divBoxConf.find("div[name='divAlinhamento']");
        if (divAlinhamento.length > 0)
            divAlinhamento.remove();

        if (combo.val() == 1) { //Numérico
            divFormato = $(String.format(this.tagLinha, "",
                "<input type='radio' name='tiposeparador' value='caracter' />Caracter" +
                "<input type='radio' name='tiposeparador' value='posicao' />Posição" +
                "<input type='text' name='separador' size='5' maxlength='5' />"));
            divBoxConf.append(divFormato);
            divFormato.attr("name", "divFormato");

            if (novoFormato) {
                objColuna.TipoSeparador = 'caracter';
                objColuna.Separador = ".";
            }

            var campoSeparador = divFormato.find("input[name='separador']");
            var rdCaracter = divFormato.find("input[value='caracter']");
            var rbPosicao = divFormato.find("input[value='posicao']");

            if (objColuna.TipoSeparador == 'caracter') {
                rdCaracter.attr("checked", true);
            }
            else {
                rbPosicao.attr("checked", true);

                campoSeparador.spinner({ min: 0, max: 1000 });
            }

            var click = Function.createDelegate(this, this.SelecionaTipoSeparador);
            $addHandler(rdCaracter[0], 'click', click);
            $addHandler(rbPosicao[0], 'click', click);

            campoSeparador.val(objColuna.Separador);
        }
        else if (combo.val() == 2) { //Data
            divFormato = $(String.format(this.tagLinha, "Formato", "<input type='text' name='formato' size='10' maxlength='20' />"));
            divBoxConf.append(divFormato);
            divFormato.attr("name", "divFormato");

            if (novoFormato)
                objColuna.Formato = 'ddMMyyyy';

            var inpFormato = divFormato.find("input[name='formato']");
            inpFormato.val(objColuna.Formato);

            this.CriarLinhaAlinhamento(divBoxConf, novoFormato);
        }
    },

    this.SelecionaTipoSeparador = function (ev) {
        var combo = $(ev.target);
        var divBoxConf = combo.parent().parent().parent();
        var campoSeparador = divBoxConf.find("input[name='separador']");

        if (combo.val() == "caracter") {
            campoSeparador.val(".");
            campoSeparador.spinner("destroy");
        }
        else {
            campoSeparador.val("2");
            campoSeparador.spinner({ min: 0, max: 1000 });
        }
    },

    this.SelecionaTipo = function (ev) {
        var combo = $(ev.target);
        var divBoxConf = combo.parent().parent().parent();

        this.CriarLinhaFormato(divBoxConf, combo, true);

        this.AtualizarMetaDados();
    },

    this.CriarListaColunas = function () {
        var divColunas = this.get_Dom_Colunas();

        if (divColunas.length == 0) {
            var divPai = $(this.get_DivPai());
            divColunas = $(String.format("<div id='{0}_divColunas' class='{1}' />",
                this._id, this.AdicionaClass("_Colunas")));
            divPai.append(divColunas);
        }
        else
            divColunas = $(divColunas);

        var comp = this;
        Array.forEach(this._Modelo.Colunas,
        function (elemento, index, array) {
            var col = comp.CriarColuna(elemento);
            divColunas.append(col);
        }, this);

        divColunas.droppable({
            drop: function (ev, ui) {
                var compConf = $find(comp._Modelo.IdListaConf);

                var colId = ui.draggable.attr("name");
                var coluna = compConf.get_Coluna(colId);

                //Arrastou dele para ele mesmo
                if (coluna == null)
                    return;

                coluna.Edicao = false;
                compConf.LimparEventosColuna(colId);

                comp.CriarColunaAppend(coluna);
                comp.AtualizarMetaDados();

                compConf.ExcluirColuna(colId);
                compConf.AtualizarMetaDados();
            }
        });

        divColunas.sortable({
            //revert: true,
            opacity: 0.35, //Ver css
            stop: function (event, ui) {
                comp.OrdenarMetadadosColunas();
                comp.AtualizarMetaDados();
            }
        });
    },
    this.initialize = function () {
        Employer.Componentes.UI.Web.EmployerListaConfColunas.callBaseMethod(this, 'initialize');

        this.CarregarMetaDados();
        this.CriarListaColunas();
    },
    this.dispose = function () {
        var divColunas = this.get_Dom_Colunas();

        divColunas.droppable("destroy");
        divColunas.sortable("destroy");

        var comp = this;
        Array.forEach(this._Modelo.Colunas,
        function (elemento, index, array) {
            comp.LimparEventosColuna(elemento.Id);
        }, this);

        Employer.Componentes.UI.Web.EmployerListaConfColunas.callBaseMethod(this, 'dispose');
    },
    this.Validar = function (sender, args) {
        args.IsValid = true;

        if (this._Modelo.TipoLista == this.EnumTipoLista.Arquivo) {
            var obrigatorio = false;

            for (var i = 0; this._Modelo.Colunas.length > i; i++) {
                var c = this._Modelo.Colunas[i];
                if (c.Obrigatorio) {
                    obrigatorio = true;
                    break;
                }
            }

            if (obrigatorio) {
                args.IsValid = false;
                sender.errormessage = "Existe(m) coluna(s) obrigatória(s) do arquivo não configurada(s)";
            }
        }

    }
}

Employer.Componentes.UI.Web.EmployerListaConfColunas.registerClass('Employer.Componentes.UI.Web.EmployerListaConfColunas', Sys.UI.Behavior);

Employer.Componentes.UI.Web.EmployerListaConfColunas.ValidarColunas = function (sender, args) {
    var espGrid = RegExp("(_[0-9]+)$");
    var m = espGrid.exec(sender._id);
    var controle = null;
    if (m != null) {
        controle = $find(sender.id.replace("_ctvColunas_" + m[0], ""));
    }
    else {
        controle = $find(sender.id.replace("_ctvColunas", ""));
    }
    if (controle != null)
        controle.Validar(sender, args);
}