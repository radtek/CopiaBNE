// ATENÇÃO!
// Em caso de Atualização/Modificação:
// Utilizar http://jscompress.com/ com o método 'Packer' para comprimir e atualizar o arquivo /min/employer.form.util-pkd.js!


/// <reference path="employer.js" />
/// <reference path="employer.form.js" />

employer.form.util = {
    truncate: function (evt) {
        /// <summary>
        /// Remove caracteres maiores que o maxlength e espaços em branco.
        /// </summary>
        var fc = function () {
            var e = employer.event.getTarget(evt);
            e.value = employer.util.string.trim(e.value);
            var tstart = 0, tend = e.value.length;
            var maxlength = (+e.getAttribute('MaxLength'));
            //Alterado dia 21/06 para solucionar um comportamento de textarea.
            if (maxlength == 0) {
                maxlength = (+e.parentNode.getAttribute('MaxLength'));
                if (maxlength == 0)
                    maxlength = (+e.getAttribute("rows")) * (+e.getAttribute("cols"));
            }
            if (tend > maxlength) {
                tend = maxlength;
            }
            if (maxlength > 0) {
                e.value = e.value.substring(tstart, tend);
            }
            switch (Sys.Browser.agent) {
                case Sys.Browser.InternetExplorer:
                    if ('fireEvent' in e) {
                        try {
                            e.fireEvent("onChange");
                        } catch (error) {
                            // Erro não definido 
                            //alert(error.description);   
                        }
                    }
                    return;
                default:
                    var newevt = document.createEvent("HTMLEvents");
                    newevt.initEvent("change", true, true);
                    e.dispatchEvent(newevt);
                    return;
            }
        };
        window.setTimeout(fc, 300);
    },
    disableComponent: function (c) {
        /// <summary>
        /// Desabilita campos do formulario e seus respectivos validadores.
        /// </summary>
        var e;
        var validators = [];
        if (typeof c == 'string') {
            e = $get(c);
        } else {
            e = c;
        }
        if (e) {
            for (var i = 0; i < e.children.length; i++) {
                // navega nos validadores
                if (e.children[i].id.toLowerCase().indexOf("validador") > -1) {
                    for (var j = 0; j < e.children[i].children.length; j++) {
                        if (e.children[i].children[0].enabled) {
                            ValidatorEnable(e.children[i].children[0], false);
                        }
                    }
                    // desabilita os campos de formulario
                } else if (employer.form.util.isValidFormElement(e.children[i])) {
                    if (!e.children[i].disabled) {
                        e.children[i].disabled = true;
                    }
                }
            }
        }
    },
    enableComponent: function (c) {
        /// <summary>
        /// Habilita campos do formulario e seus respectivos validadores.
        /// </summary>
        var e;
        var validators = [];
        if (typeof c == 'string') {
            e = $get(c);
        } else {
            e = c;
        }
        if (e) {
            for (var i = 0; i < e.children.length; i++) {
                // navega nos validadores
                if (e.children[i].id.toLowerCase().indexOf("validador") > -1) {
                    for (var j = 0; j < e.children[i].children.length; j++) {
                        if (!e.children[i].children[0].enabled) {
                            ValidatorEnable(e.children[i].children[0], true);
                        }
                    }
                    // desabilita os campos de formulario
                } else if (employer.form.util.isValidFormElement(e.children[i])) {
                    if (e.children[i].disabled) {
                        e.children[i].disabled = false;
                    }
                }
            }
        }
    },
    getValidators: function (e) {
        /// <summary>
        /// Retorna os validators do objeto, caso exista.
        /// </summary>
        if (typeof (e.Validators) != "undefined") {
            return e.Validators;
        }
    },
    isBtnPesquisa: function (e) {
        /// <summary>
        /// Verifica se o objeto é o filtro de pesquisa da página.
        /// </summary>
        var idPesquisa = 'ctl00_txtFiltroPesquisa_txtValor';
        var idPesquisaConteudo = 'ctl00_cphConteudo_txtFiltroPesquisa_txtValor';
        if (typeof e == 'string') {
            if (e == idPesquisa || e == idPesquisaConteudo) {
                return true;
            }
        } else {
            if (e.id == idPesquisa || e.id == idPesquisaConteudo) {
                return true;
            }
        }
        return false;
    },
    focusid: false,
    ffe: function (e) {
        var id;
        if (typeof e == 'string') {
            if ($get(e).tagName == 'INPUT') {
                id = e;
            } else {
                id = RecuperarComponenteValor($get(e)).id;
            }
        } else if (typeof e == 'object') {
            if (e.tagName == "SPAN") {
                id = RecuperarComponenteValor(e).id;
            } else {
                id = e.id;
            }
        } else {
            id = this.getFirstElement().id;
        }
        employer.form.util.focusid = id;
    },
    hadFocusBtnPesquisa: "",
    focusBtnPesquisa: function (evt) {
        if (this.hadFocusBtnPesquisa != "") {
            var lf = $get(this.hadFocusBtnPesquisa);
            lf.focus();
            if (lf.select) {
                lf.select();
            }
            this.hadFocusBtnPesquisa = "";
        } else {
            this.hadFocusBtnPesquisa = employer.event.getTarget(evt).id;
            var bp = $get('ctl00_txtFiltroPesquisa_txtValor');
            bp.focus();
            if (bp.select) {
                bp.select();
            }
        }
    },
    isValidFormElement: function (e) {
        /// <summary>
        /// Valida se o objeto é um componente valido de navegação.
        /// </summary>
        if (String(e.nodeName).toLowerCase() == 'input' && String(e.type).toLowerCase() == 'text') { // caixa texto
            return true;
        } else if (String(e.nodeName).toLowerCase() == 'input' && String(e.type).toLowerCase() == 'submit') { // botoes
            return true;
        } else if (String(e.nodeName).toLowerCase() == 'textarea') { // caixa texto multi linha
            return true;
        } else if (String(e.nodeName).toLowerCase() == 'select') { // dropdown list
            return true;
        } else {
            return false;
        }
    },
    isSelectedIndexAtMinMaxRange: function (e, axis) {
        /// <summary>
        /// Verifica se o componente SELECT está com a menor opção ou maior opção selecionado
        /// axis = min / max
        /// </summary>
        var node = e.nodeName.toLowerCase();
        var type = String(e.type).toLowerCase();
        if (e && node == 'select' && type == 'select-one') {
            if (e.selectedIndex == '0' && axis.toLowerCase() == 'min') {
                return true;
            } else if (e.selectedIndex == e.length - 1 && axis.toLowerCase() == 'max') {
                return true;
            } else {
                return false;
            }
        } else {
            // caso não seja um SELECT retorna TRUE
            return true;
        }
    },
    hasBehaviorAutoComplete: function (e) {
        /// <summary>
        /// Verifica se é um componente AJAX autocomplete e se está navegando nas opções.
        /// hasBehavior = se é um componente autocomplete.
        /// hasFlyout = é um componetne autocomplete e a lista de opções está sendo exibida.
        /// </summary>
        var listBehavior = Sys.UI.Behavior.getBehaviors(e);
        var hasAutoComplete = false;
        var hasFlyout = false;
        if (listBehavior.length > 0) {
            for (var i = 0; i < listBehavior.length; i++) {
                if (listBehavior[i].get_name() == "AutoCompleteBehavior") {
                    hasAutoComplete = true;
                    if (listBehavior[i]._completionListElement.childNodes.length > 0) {
                        hasFlyout = true;
                    }
                    break;
                }
            }
        }
        return [hasAutoComplete, hasFlyout];
    },
    hasVisibleParent: function (e) {
        /// <summary>
        /// Verifica se os pais do elemento estão escondidos
        /// </summary>
        if (!e.disabled && // form element
           (e.type && (e.type != "hidden")) && // form element
           (e.style && e.style.display != "none")) { // other elements
            var eparent = e.parentNode;
            //Alterado dia 16/04/2010 por Gieyson Stelmak, adicionado (&& parent.id != "conteudoRodape") para corrigir um problema com o
            //contentPlaceHolder do rodapé
            while (eparent.id != "conteudo" && eparent.id != "topo" &&
             (eparent.style && eparent.style.display != "none") && eparent.id != "conteudoRodape") {
                eparent = eparent.parentNode;
            }
            if (eparent.style.display != "none") { return true; }
            else { return false; }
        } else {
            return false;
        }
    },
    canHaveFocus: function (e) {
        /// <summary>
        /// Verifica se o campo é passivel de foco. Está visível, com os pais visíveis e 
        /// não está na lista de elementos ignorados e não está desabilitado.
        /// </summary>
        if (employer.form.util.hasVisibleParent(e) && !employer.form.util.ignoreElement(e) && !e.disabled
        //&& (e.id.search(/_Filter_/gi) <= 0) // Remove campos de Filtro da Grid da Telerik
            )
            return true;
        else
            return false;
    },
    autoCompleteClientSelected: function (source, eventArgs) {
        /// <summary>
        /// Seleciona o próximo campo possível de foco.
        /// *** Essa func é utilizada somente dentro do AjaxTookit:AutoCompleteExtender ***
        /// *** OnClientItemSelected="employer.form.util.autoCompleteClientSelected" ***
        /// </summary>
        var t = source.get_element();
        employer.form.next(t);
    },
    ignoreElement: function (e) {
        /// <summary>
        /// Verifica padrões de nomenclatura de elementos da navegação que deverão ser ignorados
        /// </summary>
        if (typeof e == "object" && e.id && e.id.match(".*_ign.*")) //tela "lançamento folha lote"
            return true;
        else if (typeof e == "string" && e.match(".*_ign.*"))
            return true;
        else
            return false;
    },
    validateFocusOnError: function (e) {
        /// <summary>
        /// Verifica se os validadores do componente permitem a ida para o proximo campo
        /// TRUE : campo com erro e o validador obriga o foco.
        /// FALSE : campo com erro e o validator não obriga o foco ou campo sem erro de validação.
        /// </summary>
        var validator = e.getAttribute("Validators");
        var pattern = /(\d|[a-z]|[A-Z]|[_])+(([_][r][e])|([_][c][v])|([_][r][v]))(\d|[A-Z]|[a-z])+$/;
        if (typeof (validator) == 'object' && validator) {
            for (var i = 0; i < validator.length; i++) {
                // valida os 3 tipos de validadores _re (req field) _rv (req validator) _cv (custom validator)
                if (pattern.exec(validator[i].id)) {
                    var focusOnError = validator[i].getAttribute("focusOnError");
                    var isValid = validator[i].getAttribute("isValid");
                    if ((typeof (isValid) == "boolean" && isValid == false) &&
                        (typeof (focusOnError) == "string" && focusOnError === "t")) {
                        return true;
                    }
                }
            }
        }
        return false;
    },
    searchInvalidValidator: function (e, vg) {
        /// <summary>
        /// Procura o primeiro validador inválido da página e traz o focus para o controle.
        /// </summary>
        if (!e.isDisabled) {
            employer.form.util.validarPagina(vg);
            for (var i = 0; i < Page_Validators.length; i++) {
                if (!Page_Validators[i].isvalid) {
                    document.getElementById(Page_Validators[siv].controltovalidate).focus();
                    return false;
                }
            }
            return true;
        } else {
            return false;
        }
    },
    validarPagina: function (vg) {
        /// <summary>
        /// Valida pagina.
        /// </summary>
        for (var i = 0; i < Page_Validators.length; i++) {
            if (Page_Validators[i].validationGroup == vg) {
                ValidatorValidate(Page_Validators[i]);
            }
        }
    },
    isValidPage: function (validationGroup) {
        /// <summary>
        /// Valida pagina.
        /// </summary>
        this.validarPagina(validationGroup);

        for (var i = 0; i < Page_Validators.length; i++) {
            if (Page_Validators[i].validationGroup == validationGroup
                && !Page_Validators[i].isvalid) {
                return false;
            }
        }
        return true;
    },
    getValueListaSugestao: function (tls, pos) {
        /// <summary>
        /// Retorna um elemento de uma lista de sugestao.
        /// pos: -2 (ultimo elemento da ls) -1 (primeiro elemento da ls)
        /// </summary>
        var _FIRST = -1;
        var _LAST = -2;
        var _ls = employer.form.util.parseListaSugestaoToArray(tls);
        if (pos == _FIRST && _ls.length > 0) {
            return _ls[0];
        } else if (pos == _LAST && _ls.length > 0) {
            return _ls[_ls.length - 1];
        } else if (_ls.length > 0) {
            return _ls[pos];
        } else {
            return false;
        }
    },
    setValueListaSugestao: function (tls, pos) {
        /// <summary>
        /// Seta um elemento de uma lista de sugestao .
        /// pos: -2 (ultimo elemento da ls) -1 (primeiro elemento da ls)
        /// </summary>
        var _FIRST = -1;
        var _LAST = -2;
        var val;
        var _ls = employer.form.util.parseListaSugestaoToArray(tls);
        if (pos == _FIRST && _ls.length > 0) {
            val = _ls[0];
        } else if (pos == _LAST && _ls.length > 0) {
            val = _ls[_ls.length - 1];
        } else if (_ls.length > 0) {
            val = _ls[pos];
        }

        var lsgt = tls.parentNode.parentNode;
        for (var i = 0; i < lsgt.children.length; i++) {
            if (lsgt.children[i].id.match("/*txtValor$")) {
                lsgt.children[i].value = val.codigo;
            } else if (lsgt.children[i].id.match("/*lblValor$")) {
                lsgt.children[i].childNodes[0].nodeValue = val.descricao;
            }
        }
    },
    parseListaSugestaoToArray: function (table) {
        /// <summary>
        /// Retorna um elemento de uma lista de sugestao.
        /// if (i % 2 == 0) MOD
        /// </summary>
        var cells = table.cells;
        var json = "[ ";
        for (var i = 0; i < cells.length; i++) {
            if (cells[i].className) {
                var str_id = cells[i].className;
                var val_id = cells[i].firstChild.data;
                i++;
                var str_desc = cells[i].className;
                var val_desc = cells[i].firstChild.data;

                json += "{ " + str_id + " : " + val_id + " , " + str_desc + " : \"" + val_desc + "\" }";

                if (i + 1 != cells.length) {
                    json += ' , ';
                }
            }
        }
        json += ' ]';
        return eval(json);
    }
};