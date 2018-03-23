// ATENÇÃO!
// Em caso de Atualização/Modificação:
// Utilizar http://jscompress.com/ com o método 'Packer' para comprimir e atualizar o arquivo /min/employer.form-pkd.js!


/// <reference path="employer.js" />

employer.form = {
    nav: function(evt) {
        /// <summary>
        /// Inicializa a captura das teclas de navegação do formulario.
        /// </summary>
        var k = employer.event.getKey(evt)[0];
        var t = employer.event.getTarget(evt);
        var ac = employer.form.util.hasBehaviorAutoComplete(t);
        var selectok = true;
        var tag = String(t.nodeName).toLowerCase();
        var type = String(t.type).toLowerCase();
        var id = t.id;
        var mod = employer.key.hasModifierKeyPressed(evt);
        if (employer.key.isArrow(k) && k == employer.key._arrows.UP_ARROW) {
            employer.event.cancel(evt);
            if (t.tagName.toLowerCase() == 'select') {
                if (!employer.form.util.isSelectedIndexAtMinMaxRange(t, 'min')) {
                    selectok = false;
                    t.selectedIndex--;
                }
            }
            // só deve mudar de campo se não estiver navegando dentro da lista de um autocomplete
            // caso select (combobox) so muda para próximo campo se estiver no indice min
            if (ac[0] && !ac[1] && selectok) {
                t.blur();
                window.setTimeout(function() {
                    employer.form.previous(t);
                }, 300);
            } else if (!ac[0] && selectok) {
                t.blur();
                window.setTimeout(function() {
                    employer.form.previous(t);
                }, 200);
            }
        } else if (employer.key.isArrow(k) && k == employer.key._arrows.DOWN_ARROW) {
            employer.event.cancel(evt);
            if (t.tagName.toLowerCase() == 'select') {
                if (!employer.form.util.isSelectedIndexAtMinMaxRange(t, 'max')) {
                    selectok = false;
                    t.selectedIndex++;
                }
            }
            // só deve mudar de campo se não estiver navegando dentro da lista de um autocomplete
            // caso select-one (combobox) so muda para próximo campo se estiver no indice max
            // campos com autocomplete estão com um delay de meio segundo
            if (ac[0] && !ac[1] && selectok) {
                t.blur();
                window.setTimeout(function() {
                    employer.form.next(t);
                }, 300);
            } else if (!ac[0] && selectok) {
                t.blur();
                window.setTimeout(function() {
                    employer.form.next(t);
                }, 200);
            }
        } else if (k == employer.key._commands.TAB || (k == employer.key._commands.ENTER && (id != 'ctl00_txtFiltroPesquisa_txtValor' && id != 'ctl00_cphConteudo_txtFiltroPesquisa_txtValor'))) {
        //Gieyson ref bug#3163 adicionado - || k == employer.key._commands.ENTER no if para a navegação pela tecla enter.
        //Gieyson ref bug#3536 adicionado - && id != 'ctl00_txtFiltroPesquisa_txtValor') para a pesquisa na master funcionar e não pular campos.
            employer.event.cancel(evt);
            if (!evt.shiftKey && !evt.shiftLeft) {
                if (t.tagName.toLowerCase() == 'select') {
                    if (!employer.form.util.isSelectedIndexAtMinMaxRange(t, 'max')) {
                        selectok = false;
                        t.selectedIndex++;
                    }
                }
                if (ac[0] && !ac[1] && selectok) {
                    t.blur();
                    window.setTimeout(function() {
                        employer.form.next(t);
                    }, 300);
                } else if (!ac[0] && selectok) {
                    t.blur();
                    window.setTimeout(function() {
                        employer.form.next(t);
                    }, 200);
                }
            } else if (evt.shiftKey || evt.shiftLeft) {
                if (t.tagName.toLowerCase() == 'select') {
                    if (!employer.form.util.isSelectedIndexAtMinMaxRange(t, 'min')) {
                        selectok = false;
                        t.selectedIndex--;
                    }
                }
                if (ac[0] && !ac[1] && selectok) {
                    t.blur();
                    window.setTimeout(function() {
                        employer.form.previous(t);
                    }, 300);
                } else if (!ac[0] && selectok) {
                    t.blur();
                    window.setTimeout(function() {
                        employer.form.previous(t);
                    }, 200);
                }
            }
        }
    },
    getNavigationListFromElement: function(e) {
        /// <summary>
        /// Retorna a lista de campos do formulário ativos para navegação por teclado.
        /// </summary>
        var parent = e.parentNode;
        // navega em até o pai máximo
        //Alterado dia 16/04/2010 por Gieyson Stelmak, adicionado (&& parent.id != "conteudoRodape") para corrigir um problema com o
        //contentPlaceHolder do rodapé
        while (parent != null && parent.id != "conteudo" && parent.id != "conteudoRodape" && parent.id != "topo") {
            var pClass = parent.className;
            if (pClass != null && (pClass.match("painel_resultado_padrao") || pClass == "uc_cep")){
                break;
            }
            parent = parent.parentNode;
        }
        // seta form para navegação 
        var container;
        if (parent == null) {
            container = "#conteudo";
        } else if (typeof parent.id == 'string' && parent.id) {
            container = "#" + parent.id;
        } else {
            container = "." + parent.className;
        }
        // busca todos os campos (input, textarea, select and button elements) menos (input image)
        return $(container).find(":input:visible:not(:image)");
    },
    getNavigationList: function() {
        /// <summary>
        /// Retorna a lista de campos do formulário ativos para navegação por teclado.
        /// </summary>
        // busca todos os campos (input, textarea, select and button elements) menos (input image)
        return $("#conteudo").find(":input:visible:not(:image)");
    },
    getFirstElement: function() {
        /// <summary>
        /// Retorna o primeiro campo possivel de foco do formulário.
        /// </summary>
        var list = employer.form.getNavigationList();
        for (var i = 0; i < list.length; i++) {
            if (employer.form.util.canHaveFocus(list[i])) {
                return list[i];
                break;
            }
        }
    },
    getLastElement: function() {
        /// <summary>
        /// Retorna o ultimo campo possivel de foco do formulário.
        /// </summary>
        var list = employer.form.getNavigationList();
        for (var i = list.length - 1; i >= 0; i--) {
            if (employer.form.util.canHaveFocus(list[i])) {
                return list[i];
                break;
            }
        }
    },
    getLastFocus: function() {
        /// <summary>
        /// Retorna o ultimo campo com foco guardado no campo HIDDEN do form (caso exista).
        /// </summary>
        var temp = $get('ctl00_hfUltimoFoco');
        if (temp != null) {
            if (temp.value != "") {
                var lastFocus = $get(temp.value);
                if (lastFocus != null) {
                    return lastFocus;
                }
            }
        }
        return null;
    },
    setLastFocus: function(event) {
        /// <summary>
        /// Atualiza campo HIDDEN do form (caso exista) com o id do ultimo campo com foco.
        /// </summary>
        var t = employer.event.getTarget(event);
        if (t != null) {
            var lastFocus = $get('ctl00_hfUltimoFoco');
            if (lastFocus != null) {
                lastFocus.value = t.id;
            }
        }
    },
    next: function(e) {
        /// <summary>
        /// Retorna o proximo campo possivel de foco, ou ele mesmo caso não haja próximo campo ou algum validator obrigue o foco.
        /// </summary>
        if (e == $get(e.id)) {
            var eform = employer.form.getNavigationListFromElement(e);
            var foundElement = false;
            var nextElement;
            var n;

            if (e.className.match("loader"))
                return;

            if (!employer.form.util.validateFocusOnError(e)) {
                for (var i = 0; i < eform.length; i++) {
                    if (eform[i].id == e.id) {
                        foundElement = true;
                    }
                    // prox element pos target ativo que não seja ignorado
                    if (foundElement && eform[i].id != e.id && employer.form.util.canHaveFocus(eform[i])) {
                        nextElement = eform[i];
                        break;
                    }
                }
            }
            if (typeof nextElement == "object") {
                p = nextElement;
            } else {
                p = e;
            }
            p.focus();
            if ((p.type && p.type != 'button' && p.type != 'reset' && p.type != 'submit') && p.select) {
                p.select();
            }
        }
    },
    previous: function(e) {
        /// <summary>
        /// Retorna o campo anterior possivel de foco, ou ele mesmo caso não haja campo anterior ou algum validator obrigue o foco.
        /// ELEMENT OBJECT
        /// </summary>
        if (e == $get(e.id)) {
            var eform = employer.form.getNavigationListFromElement(e);
            var foundElement = false;
            var previousElement;
            var p;
            if (!employer.form.util.validateFocusOnError(e)) {
                for (var i = eform.length - 1; i >= 0; i--) {
                    if (eform[i].id == e.id) {
                        foundElement = true;
                    }
                    // prox element pos target ativo que não seja ignorado
                    // alterado comparações para ID para evitar erros de comparação quando
                    // há postback ne tela durante
                    if (foundElement && eform[i].id != e.id && employer.form.util.canHaveFocus(eform[i])) {  //
                        previousElement = eform[i];
                        break;
                    }
                }
            }
            if (typeof previousElement == "object") {
                p = previousElement;
            } else {
                p = e;
            }
            p.focus();
            if ((p.type && p.type != 'button' && p.type != 'reset' && p.type != 'submit') && p.select) {
                p.select();
            }
        }
    }
};