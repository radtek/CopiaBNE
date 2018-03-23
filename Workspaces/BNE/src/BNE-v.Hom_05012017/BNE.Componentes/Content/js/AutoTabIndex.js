
Type.registerNamespace("BNE.Componentes");

var Employer_Componentes_UI_Web_AutoTabIndex = null;
var Employer_Componentes_UI_Web_AutoTabIndex_NextFocus_Timeout = null;

function Employer_Componentes_UI_Web_AutoTabIndex_BeginRequestHandler(p1, post) {
    window.clearTimeout(Employer_Componentes_UI_Web_AutoTabIndex_NextFocus_Timeout);
    Employer_Componentes_UI_Web_AutoTabIndex_NextFocus_Timeout = 0;

    if (Employer_Componentes_UI_Web_AutoTabIndex
        && p1 != undefined) {
        Employer_Componentes_UI_Web_AutoTabIndex._canFocos = false;
    }
}

var Employer_Componentes_UI_Web_AutoTabIndex_Submit = null;
var Employer_Componentes_UI_Web_AutoTabIndex_Submit_Base = null;

function Employer_Componentes_UI_Web_AutoTabIndex_BeginRequestHandlerPost() {
    Employer_Componentes_UI_Web_AutoTabIndex_BeginRequestHandler();

    return Employer_Componentes_UI_Web_AutoTabIndex_Submit_Base();
}

function Employer_Componentes_UI_Web_AutoTabIndex_NextFocusTimeout(time) {
    Employer_Componentes_UI_Web_AutoTabIndex_NextFocus_Timeout =
            window.setTimeout(function () {
                //console.log("this.NextFocus -> Employer_Componentes_UI_Web_AutoTabIndex_NextFocusTimeout");
                Employer_Componentes_UI_Web_AutoTabIndex_NextFocus();
            }, time);
}

function Employer_Componentes_UI_Web_AutoTabIndex_NextFocus(shit) {
    if (Employer_Componentes_UI_Web_AutoTabIndex) {
        if (Employer_Componentes_UI_Web_AutoTabIndex._canFocos) {
            //console.log("this.NextFocus -> Employer_Componentes_UI_Web_AutoTabIndex_NextFocus");
            Employer_Componentes_UI_Web_AutoTabIndex.NextFocus(null, null, shit);
        }
        else
            Employer_Componentes_UI_Web_AutoTabIndex.BlurTab();
    }
}

function Employer_Componentes_UI_Web_AutoTabIndex_EndRequestHandler() {
    window.clearTimeout(Employer_Componentes_UI_Web_AutoTabIndex_NextFocus_Timeout);
    Employer_Componentes_UI_Web_AutoTabIndex_NextFocus_Timeout = 0;
    Employer_Componentes_UI_Web_AutoTabIndex._canFocos = true;

    if (!AjaxClientControlBase.IsNullOrEmpty(Employer_Componentes_UI_Web_AutoTabIndex._campoFocoFoco)) {
        //console.log("Focus -> EndRequestHandler" );
        Employer_Componentes_UI_Web_AutoTabIndex.Focus(
            $("#" + Employer_Componentes_UI_Web_AutoTabIndex._campoFocoFoco));
    }
    else if (Employer_Componentes_UI_Web_AutoTabIndex._canFocosNextFocusHandler) {
        //Employer_Componentes_UI_Web_AutoTabIndex._canFocosNextFocusHandler = false;
        //console.log("this.NextFocus -> EndRequestHandler");
        Employer_Componentes_UI_Web_AutoTabIndex_NextFocus();
    }
}

BNE.Componentes.AutoTabIndex = function (element) {
    BNE.Componentes.AutoTabIndex.initializeBase(this, [element]);

    this._id = element.id;
    this._campoFoco = null;
    this._campoFocoFoco = null;

    this._canFocos = true;
    this._canFocosNextFocusHandler = false;

    this._pressedShit = false;
    this._FocoServer = null;

    this.get_FocoServer = function () {
        return this._FocoServer;
    },
    this.set_FocoServer = function (value) {
        this._FocoServer = value;
    },

    this.get_CampoFoco = function () {
        return this._campoFoco;
    },
    this.set_CampoFoco = function (value) {
        this._campoFoco = value;
    },

    this.set_ValorCampoFoco = function (value) {
        $("#" + this.get_CampoFoco()).val(value);
    },
    this.get_ValorCampoFoco = function (value) {
        return AjaxClientControlBase.IsNullOrEmpty(this.get_CampoFoco()) ? null : $("#" + this.get_CampoFoco()).val();
    },

    this.Clear = function () {
        window.clearTimeout(Employer_Componentes_UI_Web_AutoTabIndex_NextFocus_Timeout);
        Employer_Componentes_UI_Web_AutoTabIndex_NextFocus_Timeout = 0;
        this.set_ValorCampoFoco("");
        this._campoFocoFoco = null;
        this._canFocosNextFocusHandler = false;
    },

    this.Set_Focus_Server = function (campoId) {
        this._canFocos = false;
        this.Clear();
        this.set_ValorCampoFoco(campoId);
    },

    this.NextCanFocus = function (el, paraTraz) {
        if (el == null || el.length == 0)
            return null;

        var tag = el.prop("tagName");

        if (tag != "INPUT" && tag != "TEXTAREA") {
            el = $("#" + el.attr("id") + " :input:visible:first");

            if (el == null || el.length == 0)
                return null;
        }

        var tb = el.attr("tabIndex");
        var allinputsIndex = null;
        var index = 0;
        if (tb) {
            allinputsIndex = $(":input:visible[tabindex]:not(input[type='checkbox'])");

            allinputsIndex = jQuery.grep(allinputsIndex, function (value) {
                return $(value).attr("tabIndex") > -1;
            });

            allinputsIndex = allinputsIndex.sort(
                    function (elA, elB) {
                        return $(elA).attr("tabIndex") - $(elB).attr("tabIndex");
                    }
                );
        }
        else {
            allinputsIndex = $(":input:visible:not([tabindex]):not(input[type='checkbox'])");
        }

        for (; allinputsIndex.length > index; index++) {
            if ($(el).is($(allinputsIndex[index])))
                break;
        }

        if (paraTraz) {
            for (var ix = index - 1; ix >= 0; ix--) {
                var elf = $(allinputsIndex[ix]);
                if (AjaxClientControlBase.IsNullOrEmpty(elf.attr("disabled")) &&
                   !AjaxClientControlBase.IsNullOrEmpty(elf.attr("id"))) {
                    return elf;
                }
            }

            return null;
        }
        else {
            for (var ix = index + 1; allinputsIndex.length > ix; ix++) {
                var elf = $(allinputsIndex[ix]);
                if (AjaxClientControlBase.IsNullOrEmpty(elf.attr("disabled")) &&
                    !AjaxClientControlBase.IsNullOrEmpty(elf.attr("id"))) {
                    return elf;
                }
            }

            if (allinputsIndex.length > 0 &&
                allinputsIndex.length == index + 1 && el.attr("tabIndex")) {
                return $(allinputsIndex[0]);
            }

            //primeiro
            var primeiro = $("input[autotab='primeiro']");
            if (primeiro && primeiro.length > 0)
                primeiro.focus();
            else
                return $(":input:visible:not([tabindex]):not([disabled]):not(input[type='checkbox']):first");
        }
    },

    this.Focus = function (canFocus) {
        if (this._canFocos && !AjaxClientControlBase.IsNullOrEmpty(canFocus.attr("id"))) {
            var id = canFocus.attr("id");

            if (!AjaxClientControlBase.IsNullOrEmpty(canFocus.attr("disabled"))) {
                var canFocus = this.NextCanFocus(canFocus);
            }

            this.Clear();
            canFocus.focus();
            this.set_ValorCampoFoco(id);
        }
    },

    this.NextFocus = function (control, ev, shit) {
        window.clearTimeout(Employer_Componentes_UI_Web_AutoTabIndex_NextFocus_Timeout);
        Employer_Componentes_UI_Web_AutoTabIndex_NextFocus_Timeout = 0;

        //console.log("this.NextFocus");

        if (ev) {
            ev.preventDefault();
            ev.stopPropagation();
        }

        if (!control) {
            if (AjaxClientControlBase.IsNullOrEmpty(this.get_ValorCampoFoco()))
                return;
            else
                control = $("#" + this.get_ValorCampoFoco());
        }

        var id = control.attr("id");

        var canFocus = this.NextCanFocus(control, shit);

        var temblur = false;
        var temchange = false;

        if (canFocus) {
            this.Focus(canFocus);
        }
    },

    this.BlurTab = function (ev) {
        if (!AjaxClientControlBase.IsNullOrEmpty(this.get_ValorCampoFoco()))
            $("#" + this.get_ValorCampoFoco()).blur();
    },

    this.TabControl = function (ev) {
        var key = (ev.keyCode ? ev.keyCode : ev.which);

        this.set_ValorCampoFoco(ev.currentTarget.id);

        var ev = ev || window.event;
        if (ev.keyCode == 16) {
            this._pressedShit = true;
        }

        if (key == AjaxClientControlBase.Key.Commands.TAB) {
            ev.preventDefault();
            ev.stopPropagation();

            var shift = this._pressedShit;

            //this._pressedShit = false;

            Employer_Componentes_UI_Web_AutoTabIndex_NextFocus_Timeout =
            window.setTimeout(function () {
                //console.log("this.NextFocus -> tab");
                Employer_Componentes_UI_Web_AutoTabIndex_NextFocus(shift);
            }, 300);
        }
    },

    this.Keyup = function (ev) {
        var ev = ev || window.event;
        if (ev.keyCode == 16) {
            this._pressedShit = false;
        }

        var campo = $(":focus");
        if (!AjaxClientControlBase.IsNullOrEmpty(campo.attr("maxlength")) &&
           campo.attr("maxlength") == campo.val().length) {
            //console.log("this.NextFocus -> maxlength");
            this.NextFocus(campo);
        }
    },

    this.FocusControl = function (el) {
        if (el == null || el.length == 0)
            return null;

        this.set_ValorCampoFoco(el.attr("id"));
        el.attr("textoAnterior", el.val());
    },

    this.RegisterEvents = function () {
        $(function () {
            var query = ":input:visible";

            $(query).unbind("keydown").keydown(function (ev) {
                Employer_Componentes_UI_Web_AutoTabIndex.TabControl(ev);
            });

            $(query).unbind("keyup").keyup(function (ev) {
                Employer_Componentes_UI_Web_AutoTabIndex.Keyup(ev);
            });

            if (Employer_Componentes_UI_Web_AutoTabIndex_Submit == null) {
                Employer_Componentes_UI_Web_AutoTabIndex_Submit = Employer_Componentes_UI_Web_AutoTabIndex_BeginRequestHandlerPost;
                Employer_Componentes_UI_Web_AutoTabIndex_Submit_Base = WebForm_OnSubmit;
                WebForm_OnSubmit = Employer_Componentes_UI_Web_AutoTabIndex_BeginRequestHandlerPost;
            }

            //$("form").attr("onsubmit",
            //    "javascript:return Employer_Componentes_UI_Web_AutoTabIndex_BeginRequestHandlerPost()");

            $(query)
                .unbind("focus").focus(function () {
                    var campo = $(this);
                    var type = campo.attr("type");
                    var NaoignoraFoco = type != "checkbox" && type != "submit" && type != "button" && type != "image";

                    if (NaoignoraFoco) {
                        window.setTimeout(
                            function () {
                                var center = $(window).height() / 2;
                                var top = campo.offset().top;
                                if (top > center) {
                                    $(window).scrollTop(top - center);
                                }
                            },
                            200
                        );
                    }

                    var tab = campo.attr("tabindex");
                    if (AjaxClientControlBase.IsNullOrEmpty(tab) || tab > 0) {
                        if (NaoignoraFoco)
                            Employer_Componentes_UI_Web_AutoTabIndex.FocusControl(campo);
                        else
                            Employer_Componentes_UI_Web_AutoTabIndex.set_ValorCampoFoco("");
                    }
                });
        });
    },

    this.initialize = function () {
        BNE.Componentes.AutoTabIndex.callBaseMethod(this, 'initialize');

        Employer_Componentes_UI_Web_AutoTabIndex = this;

        var sm = Sys.WebForms.PageRequestManager ? Sys.WebForms.PageRequestManager.getInstance() : null;

        sm.add_beginRequest(Employer_Componentes_UI_Web_AutoTabIndex_BeginRequestHandler);
        sm.add_endRequest(Employer_Componentes_UI_Web_AutoTabIndex_EndRequestHandler);

        var nCampo = this.get_ValorCampoFoco();
        if (!AjaxClientControlBase.IsNullOrEmpty(nCampo)) {
            //Definido por name
            if (nCampo.indexOf("$") > -1)
                nCampo = $("input[name='" + nCampo + "']").attr("id");

            var campo = $("#" + nCampo);
            //this.Clear();

            if (sm) {
                //Foi dado foco em serverside por post ajax
                if (!AjaxClientControlBase.IsNullOrEmpty(sm._controlIDToFocus)) {
                    var focusCampoId = sm._controlIDToFocus;

                    //Cancelar foco do scriptManager
                    sm._controlIDToFocus = null;

                    this.Clear();
                    this._campoFocoFoco = focusCampoId;
                }
                //post em ajax
                else if (sm._request) {
                    this.Clear();
                    this.set_ValorCampoFoco(nCampo);
                    //Employer_Componentes_UI_Web_AutoTabIndex.NextFocus();
                    this._canFocosNextFocusHandler = true;
                }
                else { //Post normal
                    //Employer_Componentes_UI_Web_AutoTabIndex_NextFocus_Timeout = window.setTimeout(function () {
                    //Employer_Componentes_UI_Web_AutoTabIndex.Clear();
                    //console.log("this.NextFocus -> Post normal");
                    Employer_Componentes_UI_Web_AutoTabIndex.NextFocus(campo);
                    // }, 200);
                }
            }
        }

        this.RegisterEvents();

        var fc = $(':focus');

        if (fc.length > 0)
            this.set_ValorCampoFoco(fc.attr("id"));

        if (!AjaxClientControlBase.IsNullOrEmpty(this.get_FocoServer())) {
            this.Clear();
            var idF = this.get_FocoServer();

            window.setTimeout(function () {
                var cp = $get(idF);
                if (cp != null)
                    cp.focus();
            }, 500);

            this.set_FocoServer("");
        }
    },

    this.dispose = function () {

        if (Employer_Componentes_UI_Web_AutoTabIndex_Submit != null) {
            WebForm_OnSubmit = Employer_Componentes_UI_Web_AutoTabIndex_Submit_Base;
            Employer_Componentes_UI_Web_AutoTabIndex_Submit = null;
        }

        var sm = Sys.WebForms.PageRequestManager ? Sys.WebForms.PageRequestManager.getInstance() : null;

        sm.remove_beginRequest(Employer_Componentes_UI_Web_AutoTabIndex_BeginRequestHandler);
        sm.remove_endRequest(Employer_Componentes_UI_Web_AutoTabIndex_EndRequestHandler);

        BNE.Componentes.AutoTabIndex.callBaseMethod(this, 'dispose');
    }
}



BNE.Componentes.AutoTabIndex.registerClass('BNE.Componentes.AutoTabIndex', Sys.UI.Behavior);