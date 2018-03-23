function Trim(valor)
{
    /// <summary>
    /// Remove espaços a direita e a esquerda da string.
    /// </summary>
    return valor.replace(/^\s+|\s+$/g, '');
}
function RemoverMascaraControle(controle, regex)
{
    /// <summary>
    /// Remove uma regex de um componente.
    /// </summary>
    if(regex)
        controle.value  = RemoverMascaraValor(controle.value, regex);
    controle.select();
}
function ValidarAutomatico(args)
{
    /// <summary>
    /// Dispara validadores do componente.
    /// </summary>
    if(args.Validadores != undefined) {
        window.setTimeout(function () {         
            for (var i = 0; i < args.Validadores.length; i++) {
                var validator = $get(args.Validadores[i]);
                if (validator != null) {
                    ValidatorValidate(validator);
                }
            }
        }, 275, args);               
    }
}

function DoPostBack(controle, args)
{
    /// <summary>
    /// Dispara validadores da pagina antes de um postback.
    /// </summary>
    for (i = 0; i < args.Validadores.length; i++) {

        if ($get(args.Validadores[i]) == null)
            continue;

        if(!$get(args.Validadores[i]).isvalid)
            return;
    }
    __doPostBack(controle.id,controle.id);
}
function ClientFunction(controle, func, args)
{
    /// <summary>
    /// Dispara validadores e chama função de Callback.
    /// </summary>
    for(i=0; i < args.Validadores.length; i++)
    {
        if(!$get(args.Validadores[i]).isvalid)
            return;
    }
    
    if (func)
        func(args);
}
function RemoverMascaraValor(valor, regex)
{
    /// <summary>
    /// Remove mascara de valores.
    /// </summary>
    if(valor.match(regex)) 
        return valor.replace(/[\. -\/]/g, '');
    else
        return valor;  
}

function ExibirDescricao(controle, idDescricao, idGrid){
    /// <summary>
    /// Atualiza valores da descrição da lista de seleção.
    /// </summary>
    var odesc = $get(idDescricao);
    var table = $get(idGrid);
    
    // remove o texto antigo
    if (odesc && odesc.childNodes[0]) {
        odesc.removeChild(odesc.childNodes[0]);
    }

    if(table != null && table.childNodes.length > 0) { 
        var tbody;
        for (var i = 0; i < table.childNodes.length; i++) {
            // desconsidera nodes q não sao elementos
            if (table.childNodes[i].nodeType == 1) {
                tbody = table.childNodes[i];
                break;
            }
        }        
        
        for (var j = 0; j < tbody.childNodes.length; j++) {
            var tr = tbody.childNodes[j];
            if (tr != null && tr.childNodes.length > 0) {
                if (tr.cells.length >= 2) { 
                    var codigo = tr.cells[0].childNodes[0].nodeValue;
                    var descricao = tr.cells[1].childNodes[0].nodeValue;
                    if (codigo == controle.value) {
                        odesc.appendChild(document.createTextNode(descricao));
                        break;
                    }
                }
            }
        } 
    }
}

function ExibirComponente(controle, id, show, posicao)
{
    /// <summary>
    /// Mostra a lista de sugestão ancorada a baixo do elemento em foco.
    /// </summary>
    var elem = document.getElementById(id);

    if (elem) 
    {
      if (show) 
      { 
        elem.style.display = 'block';
        if (posicao)
        {
            //var sPath = window.location.pathname;
            //var sPage = sPath.substring(sPath.lastIndexOf('/') + 1);

            //if (sPage != "FuncaoContratoCadastro.aspx")
            if (controle.id.toLowerCase().indexOf("ucsolicitacaocadastrofuncao") < 0)
                elem.style.position = 'absolute';
            
            pos = getPosition(controle);    
            x = pos.x;
            y = pos.y;

            switch(posicao)
            {
                case 1:           
                    y += GetSize(controle).height;
                    break;
                case 2:             
                    y -= GetSize(elem).height;
                    break;
                case 3:
                    x += GetSize(controle).width;
                    break;
                case 4:
                    x -= GetSize(elem).width;
                    break;
            }
            
            elem.style.left = x + 'px';
            elem.style.top = y + 'px';

            pos = getPosition(elem);
            if(pos.x != x)
            {
                x -= (pos.x - x);             
                elem.style.left = x + 'px';   
            } 
            if(pos.y != y)
            {  
                y -= (pos.y - y);
                elem.style.top = y + 'px'; 
            }
        }
      } 
      else
      {
        elem.style.display = 'none';
      }
    }
}
function ExibirComponenteInLine(controle, id, show, posicao)
{
    /// <summary>
    /// Mostra a lista de sugestão (inline) ancorada ao lado do elemento em foco.
    /// </summary>
    var elem = document.getElementById(id);

    if (elem) 
    {
      if (show) 
      { 
        elem.style.display = 'inline';
        if (posicao)
        {
            //var sPath = window.location.pathname;
            //var sPage = sPath.substring(sPath.lastIndexOf('/') + 1);

            //if (sPage != "FuncaoContratoCadastro.aspx")
            if (controle.id.toLowerCase().indexOf("ucsolicitacaocadastrofuncao") < 0)
                elem.style.position = 'absolute';

            pos = getPosition(controle);    
            x = pos.x;
            y = pos.y;
            
            switch(posicao)
            {
                case 1:           
                    y += GetSize(controle).height;
                    break;
                case 2:             
                    y -= GetSize(elem).height;
                    break;
                case 3:
                    x += GetSize(controle).width;
                    break;
                case 4:
                    x -= GetSize(elem).width;
                    break;
            }

            elem.style.left = x + 'px';
            elem.style.top = y + 'px';
            
            pos = getPosition(elem);
            if(pos.x != x)
            {
                x -= (pos.x - x);
                elem.style.left = x + 'px';
            } 
            if(pos.y != y)
            {
                y -= (pos.y - y);
                elem.style.top = y + 'px';
            }
        }
      } 
      else
      {
        elem.style.display = 'none';
      }
    }
}

// POSICAO
function getPosition(e){
    /// <summary>
    /// Retorna a posição do elemento.
    /// </summary>
	var left = 0;
	var top  = 0;
	while (e.offsetParent){
		left += e.offsetLeft;
		top  += e.offsetTop;
		e     = e.offsetParent;
	}
	left += e.offsetLeft;
	top  += e.offsetTop;

	return {x:left, y:top};
}
function GetSize(e)
{
    /// <summary>
    /// Retorna a largura e altura do elemento.
    /// </summary>
    return {width:e.offsetWidth, height:e.offsetHeight};
}
function getScroll(e) 
{
    var left = 0;
	var top  = 0;
	while (e.offsetParent){
		left += e.scrollLeft;
		top  += e.scrollTop;
		e     = e.offsetParent;
	}

	left += e.scrollLeft;
	top  += e.scrollTop;
	
	return {x:left, y:top};
}
function findPaste(controle)
{
    /// <summary>
    /// Fitral o CTRL PASTE e força um blur no elemento.
    /// </summary>
    if ("which" in event) // NN4 & FF &amp; Opera 
        tecla=event.which; 
    else if ("keyCode" in event) // Safari & IE4+ 
        tecla=event.keyCode;
    else if ("keyCode" in window.event) // IE4+ 
        tecla=window.event.keyCode; 
    else if ("which" in window.event)
        tecla=event.which;
    
    // intercepta o V + ctrl
    if (tecla == 86 && event.ctrlKey && controle.value.length == 11)
        controle.blur();
    else if (tecla == 17 && controle.value.length == 11) // intercepta apenas o CTRL caso a tecla tenha sido solta antes do V
        controle.blur();     
}

function ApenasNumeros(controle, event, tam){    
    /// <summary>
    /// Retorna somente nros ou backspace/delete/return.
    /// </summary>
    var tecla = employer.event.getKey(event);
    if (employer.key.isCtrlV(event)) {
        return true;
    }
    
    //verifica se é uma tecla especial
    if (employer.key.isSpecial(tecla[0])) return true;
    
    var selecionado = false;
    var userSelection;
    if (controle.value.length == tam && component_isTextNotSelected(controle)) { return false; }
    // 48='0', 57='9'
    if (employer.key.isSpecial(tecla[0])) return true; // firefox bug backspace        
    if (tecla[1] >= 48 && tecla[1] <= 57) {
        return true;    
    } else {
        return false;
    }
}

function moveToNextElement(element, event, size) {
    /// <summary>
    /// Força foco para o proximo campo caso ultrapasse o tamanho do campo.
    /// </summary>
    var tecla = employer.event.getKey(event);
    // alterado validação para todas as teclas especiais
    if (element.value.length >= size && !employer.key.isSpecial(tecla[0]) && component_isTextNotSelected(element)) {
        // caso seja o único elemento ativo da pagina: tira o foco dele (para executar comportamentos do onblur) e retorna o foco ao elemento
        element.blur();
        // caso preencheu o tamanho segue para prox elemento
        window.setTimeout(function () { 
            employer.form.next(element);
        }, 200);
    } else {
        return true;
    }
}

function ProxControle(element){
    /// <summary>
    /// metodo traz foco para o proximo elemento ativo do formulario
    /// </param>
    employer.form.next(element);
}

function checkComponentValidatorFocusOnError(element) {
    /// <summary>
    /// verifica se os validadores do componente permite a ida para o proximo campo
    /// retorna TRUE caso possa ir
    /// retorna FALSE caso não possa ir
    /// </summary>
    // validadores do elemento
    var validator = element.getAttribute("Validators");
    var pattern = /(\d|[a-z]|[A-Z]|[_])+(([_][r][e])|([_][c][v])|([_][r][v]))(\d|[A-Z]|[a-z])+$/;           
    if (typeof(validator) == 'object' && validator) {
        for (var i = 0; i < validator.length; i++) {
            if (pattern.exec(validator[i].id)) {
                var focusOnError = validator[i].getAttribute("focusOnError");
                var isValid = validator[i].getAttribute("isValid");            
                if ((typeof(isValid) == "boolean" && isValid == false) && (typeof(focusOnError) == "string" && focusOnError === "t")) {
                    return false;
                }
            }     
        }        
    }
    return true;    
}

function component_getEventKey(event) {
    /// <summary>
    /// Retorna a tecla digitada do evento. X-browser compliance.
    /// </summary>
	var e = (!event) ? window.event : event;	
	var keycode = (e.keyCode) ? e.keyCode : e.which;
	return keycode;
}

function component_isEnabled(element) {
    /// <summary>
    /// metodo verifica se o campo é ativo
    /// 1st param : elemento DOM atual
    /// </summary>
    /// <param name="element" type="object" DomElement="true">
    /// elemento de referencia
    /// </param>
    if (!element.disabled && 
         element.type != "hidden" &&
         element.style.display != "none") {
        // navega os pais para saber se ele esta dentro de um modal escondida.
        var eparent = element.parentNode;
        //eparent && eparent.id && (eparent.id != "conteudo" || eparent.id != "topo") && 
        while (eparent.style.display != "none" && (eparent.id != "conteudo" && eparent.id != "topo")) {
            eparent = eparent.parentNode;
        }
        if ( eparent.style.display != "none" ) { return true; } 
        else { return false; }
    } else {
        return false;
    }
}

function component_isTextNotSelected(element) {
    /// <summary>
    /// Verifica se usuário não selecionou parte do text ou textarea. X-browser compliance.
    /// </summary>
    var value = false;
    if (document.selection) {
        //IE support
        element.focus();
        sel = document.selection.createRange();
        if (sel.text == "") { value = true; }
    } else if (typeof element.selectionStart == 'number') {
        // selectionStart == 0 é false então é melhor verificar a tipagem da propriedade
        //MOZILLA/NETSCAPE support
        var startPos = element.selectionStart;
        var endPos = element.selectionEnd;
        if (startPos == endPos) { value = true; }
    }
    return value;
}

function component_getSelectionStart(element) {
    /// <summary>
    /// Retorna a posição inicial de seleção do texto em um DOM element.
    /// </summary>
    if (document.selection) {
        var textRange = document.selection.createRange();
        var isCollapsed = textRange.compareEndPoints("StartToEnd", textRange) == 0;
        if (!isCollapsed) { textRange.collapse(true); }
        var bookmark = textRange.getBookmark();
        var ret = bookmark.charCodeAt(2) - 2;
        if (navigator.userAgent.toLowerCase().indexOf('nt 5') > -1) {
            ret--;
        }
        return ret;
    } else if (typeof element.selectionStart == 'number') {
        // selectionStart == 0 é false então é melhor verificar a tipagem da propriedade
        return element.selectionStart;    
    } 
}

function component_getSelectionEnd(element) {
    /// <summary>
    /// Retorna a posição final de seleção do texto em um DOM element.
    /// </summary>
    if (document.selection) {
        var textRange = document.selection.createRange();
        var isCollapsed = textRange.compareEndPoints("StartToEnd", textRange) == 0;
        if (!isCollapsed) { textRange.collapse(false); }
        var ret = bookmark.charCodeAt(2) - 2;
        if (navigator.userAgent.toLowerCase().indexOf('nt 5') > -1) {
            ret--;
        }
        return ret;
    } else if (typeof element.selectionEnd == 'number') {
        // selectionStart == 0 é false então é melhor verificar a tipagem da propriedade
        return element.selectionEnd;
    }
}

function component_getSizeTextSelected(element) {
    /// <summary>
    /// Retorna o tamanho da seleção do texto em um DOM element.
    /// </summary>
    if (document.selection) {
        var textRange = document.selection.createRange();
        return textRange.text.length;        
    } else if (typeof element.selectionStart == 'number') {
        // selectionStart == 0 é false então é melhor verificar a tipagem da propriedade
        return element.selectionEnd - element.selectionStart;
    }
}

function component_isSpecialKey(event) {
    /// <summary>
    /// Verifica se uma tecla é especial. X-browser compliance.
    /// 0 keycode = non char
    /// 1 charcode = chars
    /// </summary>
    //var key = component_getEventKey(event);
    var key = employer.event.getKey(event);
    if (event.type == 'keypress') {
        switch (key[0]) {
            case 8 : // BACKSPACE
            case 9 : // TAB
            case 13 : // ENTER
            case 16 : // SHIFT CTRL ALT 16-18
            case 17 : 
            case 18 : 
            case 20 : // CAPS LOCK
            case 27 : // ESCAPE
            case 33 : // PAGE UP DOWN
            case 34 : 
            case 35 : // HOME END
            case 36 : 
            case 37 : // ARROWS
            case 38 : 
            case 39 : 
            case 40 : 
            case 45 : // INSERT DELETE
            case 46 : 
            case 112 : // F1
            case 113 : // F2
            case 114 : // F3
            case 115 : // F4
            case 116 : // F5
            case 117 : // F6
            case 118 : // F7
            case 119 : // F8
            case 120 : // F9
            case 121 : // F10
            case 122 : // F11
            case 123 : // F12
              return true;
                break;
            default :
                return false;
                break;
        }    
    }   
}
       
function component_getFormElements(element){
    /// <summary>
    /// Retorna umas lista com todos os elementos intut select textarea.
    /// Dependencia: JQuery.
    /// </summary>
    var parent;
    // verifica se o elemento não é container máximo do form
    if (element.id == "conteudo" || element.id == "topo") { tabParent = element } 
    else { parent = element.parentNode; }

    if (parent && typeof parent == 'object') {
        while (parent.id != "conteudo" && parent.id != "topo") {
            var pclassName = parent.className.toLowerCase();
            var ptagName = parent.tagName.toLowerCase();
            // caso seja uma modal
            if (ptagName == "div" && (pclassName.match("painel_resultado_padrao") || pclassName == "uc_cep")) { break; }
            parent = parent.parentNode;
        }
    }
    var container;
    if (typeof parent.id == 'string' && parent.id ) {
        container = "#"+parent.id;
    } else {
        container = "#conteudo";
    }
    return $(container).find(":input:visible:enabled:not(:image)");
}

function component_isFocusable(element) {
    /// <summary>
    /// verifica se é um elemento válido para foco
    /// </summary>
    var tagname = element.tagName.toLowerCase();
    var type = element.type.toLowerCase();
    if (typeof tagname != "undefined") {
         if (component_isValidFormElement(element)) { return true; }
         else { return false; }
    } else { return false; }
}

function component_isValidFormElement(element) {
    // nodeName (input textarea select)
    // type (button text textarea select-one)
    // input, textarea, select and button
    var node = element.nodeName.toLowerCase();
    if (element && (node == 'input' || node == 'textarea' || node == 'select')) { 
        return true;
    } else {
        return false;
    }   
}

function ArmazenarUltimoFocoComponente(campo)
{
    /// <summary>
    /// Grava o ultimo foco em um hidden field.
    /// </summary>
    if(campo != null)
    {
        var campoUltimoFoco = $get('ctl00_hfUltimoFoco');
        if(campoUltimoFoco != undefined)
            campoUltimoFoco.value = campo.id;
    }
}

// TODO: acertar para funcionar no navegador chrome. Usar DOM3
// http://www.w3.org/TR/DOM-Level-3-Events/events.html#Events-KeyboardEvent-initKeyboardEvent        
// http://developer.mozilla.org/en/DOM/document.createEvent

function component_keyEvtToCase(event, c) { 
    /// <summary>
    /// Sobreescreve o evento KEYPRESS forçando um toCase(upper/lower).
    /// event: o evento keypress que está sendo verificado
    /// toCase: upper - força toUpperCase no evento keypress
    ///         lower - força toLowerCase no evento keypress
    ///         normal - qualquer outra
    /// </summary>
    var charcode = event.which ? event.which : event.keyCode;
    var toCase = typeof c != "undefined" ? c : "normal";
    var type = event.type;
    if (type == 'keypress') {
        if ( toCase == "upper" && (charcode >= 97 && charcode <= 122) || (charcode >= 224 && charcode <= 255) ) { 
             // converte de acordo com o valor decimal da tecla na tabela ascii    
             charcode = charcode - 32;
             component_createNewEventKeypress(event, charcode); 
        } else if ( toCase == "lower" && (charcode >= 65 && charcode <= 90) || (charcode >= 192 && charcode <= 223) ) { 
             // converte de acordo com o valor decimal da tecla na tabela ascii    
             charcode = charcode + 32; 
             component_createNewEventKeypress(event, charcode);
        }
    }
}

function component_createNewEventKeypress(oldEvent, newkey) {
    // firefox e outros que usam o Gecko 
    if (oldEvent.which) { 
        var newEvent = document.createEvent("KeyboardEvent"); 
        newEvent.initKeyEvent("keypress", true, true, document.defaultView, 
                 oldEvent.ctrlKey, oldEvent.altKey, oldEvent.shiftKey, 
                 oldEvent.metaKey, 0, newkey); 
        oldEvent.preventDefault(); 
        oldEvent.target.dispatchEvent(newEvent);
        
    // IE 
    } else { 
        oldEvent.keyCode = newkey;
    }    
}

function component_getEventTarget(event) {
    /// <summary>
    /// Retorna o elemento que iniciou o evento. X-browser compliance.
    /// </summary>
    var e = (!event) ? window.event : event;
    var target = (e.target) ? e.target : e.srcElement;
	if (target.nodeType == 3) {
	    // Safari bug
		target = targ.parentNode;
    }
    return target;
}


function componentIsValid(id) {
    var vals = [];

    var rfFone = employer.util.findControl(id + "_rfFone")[0];
    var reFone = employer.util.findControl(id + "_reFone")[0];

    var rfDDD = employer.util.findControl(id + "_rfDDD")[0];
    var reDDD = employer.util.findControl(id + "_reDDD")[0];

    var rfDDI = employer.util.findControl(id + "_rfDDI")[0];
    var reDDI = employer.util.findControl(id + "_reDDI")[0];

    var rfValor = employer.util.findControl(id + "_rfValor")[0];
    var reValor = employer.util.findControl(id + "_reValor")[0];
    var cvValor = employer.util.findControl(id + "_cvValor")[0];
    
    if (rfFone != null)
        vals.push(rfFone);

    if (reFone != null)
        vals.push(reFone);

    if (rfDDD != null)
        vals.push(rfDDD);

    if (reDDD != null)
        vals.push(reDDD);

    if (rfDDI != null)
        vals.push(rfDDI);

    if (reDDI != null)
        vals.push(rfDDI);

    if (rfValor != null)
        vals.push(rfValor);

    if (reValor != null)
        vals.push(rfValor);

    if (cvValor != null)
        vals.push(cvValor);
        
    for (var i = 0; i < vals.length; i++) {
        ValidatorValidate(vals[i]);
        if (!vals[i].isvalid)
            return false;
    }

    return true;
}