// ATENÇÃO!
// Em caso de Atualização/Modificação:
// Utilizar http://jscompress.com/ com o método 'Packer' para comprimir e atualizar o arquivo geral-pkd.js!

$(function () {
    positionFooter();

    function positionFooter() {
        if ($("#conteudo").length == 0)
            return;

        var bodyHeight = $('body').height();
        if (bodyHeight.length == 0)
            return;

        var windowHeight = $(window).height();

        var range = 82;
        if ((bodyHeight - range) > windowHeight) {
            $(".barra_rodape").css({
                margin: "0px 0 -39px 0"
            });
            return;
        }

        var contentInitial = 145;
        var contentHeight = $("#conteudo").height();

        if (contentHeight == 0
            || contentHeight == typeof (undefined)) {
            return;
        }

        if ($("#conteudo").position().top == typeof (undefined))
            return;

        var heightBottom = 96;
        var headerTop = 55;

        var optionA = (Math.max(contentInitial, $("#conteudo").position().top) + contentHeight + heightBottom);
        var optionB = headerTop + $("#conteudo_interna").outerHeight(true) + heightBottom;
        var sumOfPage = Math.max(optionA, optionB);
        if (windowHeight > sumOfPage) {
            var difference = windowHeight - sumOfPage;
            if (difference < 0)
                difference = 0;

            $(".barra_rodape").css({
                margin: difference + "px 0 -39px 0"
            });
        }
    }

    $(window)
        .resize(function () {
            positionFooter();
        });

    $('form,input,select,textarea').attr("autocomplete", "off");
});



//Handler - Executa quando a modal de login é mostrada
function mpeLogin_Shown() {
    //Coloca o foco no campo de CPF
    $("*[id$='txtCPF_txtValor']").focus();
}

/*
*FIM Controle do tracker do chat comercial
*/

function setCookie(c_name, value, exdays) {
    var exdate = new Date();
    exdate.setDate(exdate.getDate() + exdays);
    var c_value = escape(value) + ((exdays == null) ? "" : "; expires=" + exdate.toUTCString());
    document.cookie = c_name + "=" + c_value;
}

function getCookie(c_name) {
    var c_value = document.cookie;
    var c_start = c_value.indexOf(" " + c_name + "=");
    if (c_start == -1) {
        c_start = c_value.indexOf(c_name + "=");
    }
    if (c_start == -1) {
        c_value = null;
    }
    else {
        c_start = c_value.indexOf("=", c_start) + 1;
        var c_end = c_value.indexOf(";", c_start);
        if (c_end == -1) {
            c_end = c_value.length;
        }
        c_value = unescape(c_value.substring(c_start, c_end));
    }
    return c_value;
}

//Atualiza as informações de data para a impressão
function AtualizarInfosImpressao() {
    var dia;
    var mes;
    var ano;
    var hora;
    var dataAtual;

    dataAtual = new Date();

    dia = String(dataAtual.getDate());
    //Coloca o dígito 0 à frente caso o dia seja menor do que 10
    if (dia.length < 2) {
        dia = "0" + dia;
    }

    mes = String(dataAtual.getMonth() + 1);

    if (mes.length < 2) {
        mes = "0" + mes;
    }

    ano = String(dataAtual.getFullYear());
    hora = String(dataAtual.getHours() + ":" + dataAtual.getMinutes());

    $(".infos_impressao").text("Impresso em: " + dia + "/" + mes + "/" + ano + " " + hora + " (data e hora de Brasília) - Banco Nacional de Empregos - www.bne.com.br");
}

function ValidarPagina(validationGroup) {
    for (vpi = 0; vpi < Page_Validators.length; vpi++) {
        if (Page_Validators[vpi].validationGroup == validationGroup) {
            ValidatorValidate(Page_Validators[vpi]);
        }
    }
}

///* Painel de Avisos */
//var to;
//function ExibirAviso() {
//    clearTimeout(to);
//    $(".painel_avisos").animate({height: "40px", opacity: 1}, 1000);
//    to = setTimeout("OcultarAviso()",30000);
//}
//function OcultarAviso() {
//    //NÃO UTILIZAR, POIS NO CASO DE 2 MSG CONSECUTIVAS NÃO EXIBE (JUCA)
//    //$(".painel_avisos").animate({ opacity: 0 }, 500);
//    //to = setTimeout($(".painel_avisos").css("display", "none"), 500);
//    $(".painel_avisos").css("display", "none");
//    clearTimeout(to);
//}
//$(".painel_avisos").click(function() {
//    OcultarAviso();
//});
///* FIM: Painel de Avisos */

/* Painel de Avisos */
var to;
function ExibirAviso(parametros) {
    clearTimeout(to);
    $(".painel_avisos").css("display", "block");
    $(".painel_avisos").css("opacity", "0.9");

    if (parametros.aumentarPainelMensagem)
        $(".painel_avisos").css("height", "60px");

    //$(".painel_avisos").animate({ opacity: 1 }, 1);
    to = setTimeout("OcultarAviso()", 20000);
}
function OcultarAviso() {
    //NÃO UTILIZAR, POIS NO CASO DE 2 MSG CONSECUTIVAS NÃO EXIBE (JUCA)
    //$(".painel_avisos").animate({ opacity: 0 }, 500);
    //to = setTimeout($(".painel_avisos").css("display", "none"), 500);
    $(".painel_avisos").css("display", "none");
    clearTimeout(to);
}
$(".painel_avisos").click(function () {
    OcultarAviso();
});
/* FIM: Painel de Avisos */

/**** MANIPULATE ELEMENTS FUNCTIONS ****/

function getEvent(event) {
    /// <summary>
    /// Retorna o evento disparador. X-browser compliance.
    /// DEPRICATED -> Usuar funções do namespace de cada app.
    /// </summary>
    var e = (!event) ? window.event : event;
    return e;
}

function getEventKey(event) {
    /// <summary>
    /// Retorna o keycode/charcode da tecla digitada no evento. X-browser compliance.
    /// DEPRICATED -> Usuar funções do namespace de cada app.
    /// </summary>
    var e = getEvent(event);
    var keycode = (e.keyCode) ? e.keyCode : e.which;
    return keycode;
}

function getEventTarget(event) {
    /// <summary>
    /// Retorna o elemento que iniciou o evento. X-browser compliance.
    /// DEPRICATED -> Usuar funções do namespace de cada app.
    /// </summary>
    var e = getEvent(event);
    var t = (e.target) ? e.target : e.srcElement;
    if (t.nodeType == 3) {
        // Safari bug
        t = t.parentNode;
    }
    return t;
}

function searchInvalidValidator(element, validationGroup) {
    /// <summary>
    /// Método que procura o primeiro validador inválido da página e set o focus para o controle validado.
    /// DEPRICATED -> Usuar funções do namespace de cada app.
    /// </summary>
    window.setTimeout(function () {
        if (!element.isDisabled) {
            //ValidarPagina(validationGroup);
            for (var siv = 0; siv < Page_Validators.length; siv++) {
                if (typeof Page_Validators[siv].isvalid == 'boolean' && !Page_Validators[siv].isvalid) {
                    $get(Page_Validators[siv].controltovalidate).focus();
                    return false;
                }
            }
            return true;
        }
        else
            return false;
    }, 75);
}


/**** MANIPULATE DOM ELEMENTS FUNCTIONS ****/

function setElementText(element, text) {
    /// <summary>
    /// atualiza o texto de um element DOM (ex: label)
    /// DEPRICATED -> Usuar funções do namespace de cada app.
    /// </summary>
    var olabel;
    if (typeof element == "string") {
        olabel = $get(element);
    } else { olabel = element; }
    // remove o texto antigo
    if (olabel) {
        if (olabel.childNodes[0]) {
            olabel.removeChild(olabel.childNodes[0]);
        }
        var newtext = document.createTextNode(text);
        olabel.appendChild(newtext);
    }
}

function getElementText(element) {
    /// <summary>
    /// retorna o texto de um element DOM (ex: label)
    /// TODO: criar recursividade com filhos (nodetype == 1)
    /// DEPRICATED -> Usuar funções do namespace de cada app.
    /// </summary>
    var olabel;
    var text;
    if (typeof element == "string") {
        olabel = $get(element);
    } else { olabel = element; }
    if (!olabel) {
        return;
    } else {
        for (var i = 0; i < olabel.childNodes.length; i++) {
            if (olabel.childNodes[i].nodeType == 3) {
                text = olabel.childNodes[i].nodeValue;
                break;
            }
        }
        return text;
    }
}

function setMensagemAviso(msg, type) {
    /// <summary>
    /// atualiza o texto de um element DOM (ex: label)
    /// type: warning = darkblue | error = red
    /// DEPRICATED -> Usuar funções do namespace de cada app.
    /// </summary>
    var label = $get('ctl00_lblAviso');
    var color = 'black';
    if (type == 'warning') {
        color = 'darkblue';
    } else if (type == 'error') {
        color = 'red';
    }
    if (label) {
        // remove outras msgs
        if (label && label.childNodes[0]) { label.removeChild(label.childNodes[0]); }

        if (msg.length > 0) {
            var style = label.getAttribute("style");
            if (!style) {
                label.setAttribute("style", "color: " + color);
            } else {
                style.color = color;
            }
            var newbold = document.createElement("b");
            var newtext = document.createTextNode(msg);

            newbold.appendChild(newtext);
            label.appendChild(newbold);
            ExibirAviso();
        }
    }
}

function stringToNumber(string) {
    /// <summary>
    /// Remove letras e caracteres, remove '.', troca ',' por '.' para casas decimais.
    /// DEPRICATED -> Usuar funções do namespace de cada app.
    /// </summary>
    var i = 0;
    var negativo = false;
    while (string.charAt(i) == ' ') {
        i++;
    }
    if (string.charAt(i) == '-') {
        negativo = true;
    }
    var temp = string.replace(/\./g, '');
    temp = temp.replace(/,/g, '.');
    temp = temp.replace(/[^0-9.]/g, '');
    if (negativo) {
        return -(+temp);
    } else {
        return (+temp);
    }
}

function formataMoeda(num) {
    x = 0;
    if (num < 0) {
        num = Math.abs(num);
        x = 1;
    }
    if (isNaN(num)) num = "0";
    cents = Math.floor((num * 100 + 0.5) % 100);
    num = Math.floor((num * 100 + 0.5) / 100).toString();
    if (cents < 10) cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3) ; i++)
        num = num.substring(0, num.length - (4 * i + 3)) + '.' + num.substring(num.length - (4 * i + 3));
    ret = num + ',' + cents;
    if (x == 1) ret = ' - ' + ret; return ret;
}

function maskNumber(val, mask) {
    /// <summary>
    /// R$ ###.###.###.##0,00
    /// #-##.#-##.#-##.##000
    /// 12
    /// 12.3
    /// 12.33
    /// 12.33555678
    /// DEPRICATED -> Usuar funções do namespace de cada app.
    /// </summary>
    var prefix;
    var rad;
    var sufix;
    var negative = false;
    if (val < 0) {
        negative = true;
    }
    if (typeof val == 'number') {
        val = String(val);
        val = val.replace(/\./g, ',');
        prefix = mask.substring(0, mask.indexOf('#'));
        sufix = mask.substring(mask.lastIndexOf('#') + 1, mask.length);
        mask = mask.substring(mask.indexOf('#'), mask.length);
        //mask = mask.substring(mask.indexOf('#'), mask.lastIndexOf('#')+1);
        // normaliza o value caso haja 
        if (sufix) {
            if (sufix.indexOf(',') > -1) {
                ts = sufix.split(',');
                tv = val.split(',');
                // ambos possuem decimal
                if (ts.length == tv.length) {
                    // decimal do sufixo maior q decimal do valor
                    if (tv[1].length < ts[1].length) {
                        for (var i = 0; i < ts[1].length; i++) {
                            if (tv[1].charAt(i) == '') {
                                tv[1] += ts[i].charAt(i);
                            }
                        }
                        // remove caracteres qndo o decimal do valor é maior q o decimal do sufixo
                    } else if (tv[1].length > ts[1].length) {
                        val = val.substring(0, val.length - (tv[1].length - ts[1].length));
                    }
                    // adiciona o decima do sufixo ao valor
                } else if (tv.length < ts.length) {
                    val += "," + ts[1];
                }
            }
        }
        var im = 0;
        if (val.length - sufix.length > 0) {
            for (var j = val.length - sufix.length - 1; j > -1; j--) {
                if (val.charAt(j) != '') {
                    if (mask.charAt(mask.length - sufix.length - 1 - im) != '#') {
                        val = val.substring(0, j + 1) + mask.charAt(mask.length - sufix.length - 1 - im) + val.substring(j + 1, val.length);
                    }
                }
                im++;
            }
        }
    }
    if (typeof val == 'string') {
        if (negative) {
            return "( " + prefix + val + " )";
        } else {
            return prefix + val;
        }
    } else {
        return null;
    }
}


function pageLoad(sender, args) {
    /// <summary>
    /// DOTNET infra
    /// executa apos carregar a tela (DOM READY)
    /// </summary>

    // checa versao do flash player para definicao do conteudo
    // do progress template

    if (typeof swfobject != 'undefined') {
        //      swfobject.embedSWF( "swf/progress_template.swf" , "progress_img_container", "130", "40", "5.0.0", false, false, {wmode:"transparent"});
        swfobject.embedSWF("swf/ico_avisos.swf", "icoavisos_img_container", "44", "44", "7.0.0", false, false, { wmode: "transparent" });
        swfobject.embedSWF("swf/progress_template.swf", "progress_img_container", "214", "40", "7.0.0", false, false, { wmode: "transparent" });
    }

    // seta classe css FOCUS no event focus dos elementos
    $(":input:not(:image):not(:submit):not(:reset):not(:button)").bind("focus", function () {
        Sys.UI.DomElement.addCssClass(this, 'focus');
    });
    // remove classe css FOCUS no event blur dos elementos
    $(":input:not(:image):not(:submit):not(:reset):not(:button)").bind("blur", function () {
        Sys.UI.DomElement.removeCssClass(this, 'focus');
    });

    // guarda o ultimo campo que obteve foco
    //$(":input:not(:hidden):not(:image):not(:submit):not(:reset):not(:button)").bind("blur", employer.form.setLastFocus);

    // caso haja o plugin SCROLLTO do JQUERY traz campo em foco para o topo da pagina
    $(":input:not(:image):not(:submit):not(:reset):not(:button)").bind("focus", function () {
        if ($.scrollTo) { $.scrollTo(this, { offset: -150, axis: 'y', easing: 'linear' }); }
    });

    // tratamento ENTER BACKSPACE
    document.onkeydown = employer.key.disableKeys; // IE, Firefox, Safari
    document.onkeypress = employer.key.disableKeys; // only Opera needs the backspace nullifying in onkeypress
    //document.onkeypress = employer.key.disableCapsLock; 

    // atalhos de teclado
    //$("#conteudo").bind("keydown", employer.key.shortcuts);
    // atalhos de teclado para o campo de busca (TOPO)
    $("#ctl00_txtFiltroPesquisa_txtValor").bind("keydown", employer.key.shortcuts);
    $("#ctl00_cphConteudo_txtFiltroPesquisa_txtValor").bind("keydown", employer.key.shortcuts);

    // vincula a navegação de teclado ao keydown
    //$(":input:not(:image)").bind("keydown", employer.form.nav);

    // remove caracteres maiores que a propriedade MaxLength ou cols * rows
    $(":input:not(:image):not(:submit):not(:reset):not(:button)").bind("paste", employer.form.util.truncate);

    // testa e chama um pageload local caso exista
    if (typeof LocalPageLoad == 'function') {
        LocalPageLoad();
    }

    // correção setar foco no primeiro
    /* Testes
    var firstElement = employer.form.getFirstElement();
    if (firstElement != undefined) {
    if (employer.form.util.canHaveFocus(firstElement))
    employer.controles.setFocus(employer.form.getFirstElement().id);
    else
    employer.form.next(firstElement);
    }
    */
    if (employer.form.util.focusid != false) {
        var e = $get(employer.form.util.focusid);
        if (employer.form.util.canHaveFocus(e)) {
            e.focus();
        } else {
            employer.form.next(e);
        }
        employer.form.util.focusid = false;
    }

    //Testa se a tela de login existe e adiciona o evento para quando a modal de login é mostrada
    if ($find('mpeLoginBehavior') != null) {
        $find('mpeLoginBehavior').add_shown(mpeLogin_Shown);
    }
    AddRequestHandler();
}

var postbackElement;//= null;
function RestoreFocus(source, args) {

    if (postbackElement == null)
        return;

    var element = document.getElementById(postbackElement.id);

    if (element == null)
        return;

    element.disabled = false;

    var $all = $('form :input:visible');
    for (var i = 0; i < $all.length - 1; ++i) {
        if ($all[i] != element)
            continue;

        if ($all.length == i + 1)
            return;


        var $focused = $(':focus');
        if ($focused == $all[i + 1])
            return;

        if (!(typeof ($.browser) !== 'undefined' && $.browser.webkit)) {
            $all[i + 1].focus();
        }
        return;
    }
}
function SavePostbackElement(source, args) {
    postbackElement = args.get_postBackElement();

    postbackElement.disabled = true;
}
function AddRequestHandler() {
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(RestoreFocus);
    prm.add_beginRequest(SavePostbackElement);
}

//Função de redirecionamento de página no google analytics
function trackOutboundLink(link, category, action, label) {
    try {
        ga('send', 'event', category, action, label);
    } catch (err) { }

    setTimeout(function () {
        document.location.href = link.href;
    }, 100);
}

//Conta quantos acessos houveram em uma determinada página
function trackEvent(category, action, label) {
    try {
        ga('send', 'event', category, action, label);
    } catch (err) { }
}

var URLCompleteCidade = '';
var LimiteCompleteCidade = '';
var URLCompleteFuncao = '';
var LimiteCompleteFuncao = '';
var URLCompleteBairro = '';
var LimiteCompleteBairro = '';
var URLCompleteCurso = '';
var LimiteCompleteCurso = '';

function InicializarAutoCompletes(parametros) {
    if (typeof (parametros) != 'undefined' && parametros != 'undefined') {
        autocomplete.URL.URLCompleteCidade = parametros.URLCompleteCidade;
        autocomplete.limit.LimiteCompleteCidade = parametros.LimiteCompleteCidade;
        autocomplete.URL.URLCompleteFuncao = parametros.URLCompleteFuncao;
        autocomplete.limit.LimiteCompleteFuncao = parametros.LimiteCompleteFuncao;
        autocomplete.URL.URLCompleteBairro = parametros.URLCompleteBairro;
        autocomplete.limit.LimiteCompleteBairro = parametros.LimiteCompleteBairro;
        autocomplete.URL.URLCompleteCurso = parametros.URLCompleteCurso;
        autocomplete.limit.LimiteCompleteCurso = parametros.LimiteCompleteCurso;
    }
}

var autocomplete = {
    configure: function (control, url, data, selectCallback, type) {
        $(document.body).on('focus', 'input:text[name*=' + control + ']', function () {
            $(this).autocomplete({
                autoFocus: true,
                source: function (request, response) {

                    if ($(this).data('xhr')) {
                        $(this).data('xhr').abort();
                    }

                    var query = request.term;
                    if (type === autocomplete.type.bairro) {
                        query = autocomplete.extractLast(request.term);
                        data.cidade = data.cidade;
                        data.funcao = data.funcao;
                    }else if (type === autocomplete.type.curso) {
                        data.nomeParcial = query;
                    } else {
                        data.query = query;
                    }

                    /*remove caracteres especiais*/
                    data.query = query.replace(/[^a-zA-Z0-9_# ÁáÂâÃãÄäÉéÊêËëÍíÏïÓóÔôÕõÖöÚúÛûÜüÇç]/, ' ');


                    $(this).data('xhr',
                        $.ajax({
                            url: url,
                            dataType: "json",
                            data: data,
                            success: function (data) {
                                response($.map(data, function (item) {
                                    return { label: item };
                                }));
                            }
                        }));
                },
                cache: false,
                select: function (event, ui) {
                    if (type === autocomplete.type.bairro) {
                        var terms = autocomplete.split(this.value);
                        // remove the current input
                        terms.pop();
                        // add the selected item
                        terms.push(ui.item.value);
                        // add placeholder to get the comma-and-space at the end
                        terms.push("");
                        this.value = terms.join(", ");
                        return false;
                    } else {
                        this.value = ui.item.label;
                    }
                    if (selectCallback != null) {
                        selectCallback();
                    }
                    return false;
                },
                delay: 0,
                minLength: 0,
                focus: function (event, ui) {
                    if (type === autocomplete.type.bairro) {
                        return false;
                    }
                    var menu = $(this).data("ui-autocomplete").menu.element;
                    menu.find("li").removeClass("ui-state-hover");
                    menu.find("li:has(a.ui-state-focus)").addClass("ui-state-hover");
                },
                close: function (event, ui) {
                    var menu = $(this).data("ui-autocomplete").menu.element;
                    menu.find("li").removeClass("ui-state-hover");
                }
            });
        });
    },
    funcao: function (control, callback) {
        var data = { limit: autocomplete.limit.LimiteCompleteFuncao };
        autocomplete.configure(control, autocomplete.URL.URLCompleteFuncao, data, callback, autocomplete.type.funcao);
    },
    cidade: function (control, callback) {
        var data = { limit: autocomplete.limit.LimiteCompleteCidade };
        autocomplete.configure(control, autocomplete.URL.URLCompleteCidade, data, callback, autocomplete.type.cidade);
    },
    bairro: function (control, cidade, estado, callback, unicoBairro) {
        var data = { limit: autocomplete.limit.LimiteCompleteBairro, cidade: cidade, estado: estado };
        autocomplete.configure(control, autocomplete.URL.URLCompleteBairro, data, callback, unicoBairro == 'true' ? autocomplete.type.unicoBairro : autocomplete.type.bairro);
    },
    curso: function (control, callback) {
        var data = { numeroRegistros: autocomplete.limit.LimiteCompleteCurso };
        autocomplete.configure(control, autocomplete.URL.URLCompleteCurso, data, callback, autocomplete.type.curso);
    },
    split: function (val) {
        return val.split(/,\s*/);
    },
    extractLast: function (term) {
        return autocomplete.split(term).pop();
    },
    type: {
        cidade: 'Cidade', bairro: 'Bairro', unicoBairro: 'unicoBairro', funcao: 'Funcao', curso: 'Curso'
    },
    URL: {
        URLCompleteCidade: '',
        URLCompleteFuncao: '',
        URLCompleteBairro: '',
        URLCompleteCurso: ''
    },
    limit: {
        LimiteCompleteCidade: '',
        LimiteCompleteFuncao: '',
        LimiteCompleteBairro: '',
        LimiteCompleteCurso: ''
    },
    validacao: {
        cidade: function (sender, args) {
            var compare = args.Value;
            var data = { query: args.Value.split('/')[0], limit: 1 };
            $.ajax({
                url: autocomplete.URL.URLCompleteCidade,
                dataType: "json",
                data: data,
                async: false,
                success: function (result) {
                    args.IsValid = false;
                    if (result != null) {
                        args.IsValid = result.contains(compare);
                    }
                }
            });
        },
        funcao: function (sender, args) {
            var data = { query: args.Value, limit: 1 };
            $.ajax({
                url: autocomplete.URL.URLCompleteFuncao,
                dataType: "json",
                data: data,
                async: false,
                success: function (result) {
                    args.IsValid = false;
                    if (result != null) {
                       // args.IsValid = result.contains(data.query);
                        args.IsValid = result[0].indexOf(data.query) >= 0;
                    }
                }
            });
            //args.IsValid = response;
        }
    }
};

function setCookie(name, value) {
    var cookie = name + "=" + escape(value) + ";expires=Thu, 13 Mai 2214 12:00:00 GMT";
    document.cookie = cookie;
    getCookie(name);
}

function getCookie(name) {
    var cookies = document.cookie;
    var prefix = name + "=";
    var begin = cookies.indexOf("; " + prefix);

    if (begin == -1) {

        begin = cookies.indexOf(prefix);

        if (begin != 0) {
            return false;
        }

    } else {
        begin += 2;
    }

    var end = cookies.indexOf(";", begin);

    if (end == -1) {
        end = cookies.length;
    }
    //console.log(unescape(cookies.substring(begin + prefix.length, end)));
    //return unescape(cookies.substring(begin + prefix.length, end));
    return true;
}


function cvCidadeGeral_Validate(sender, args) {
    var res = Principal.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function getCookieValue(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
