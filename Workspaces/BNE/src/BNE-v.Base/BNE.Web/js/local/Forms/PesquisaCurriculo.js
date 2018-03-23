function ImprimirVisualizacaoCurriculo(divImprimir) {
    var wrapperDiv = employer.util.findControl(divImprimir)[0];
    wrapperDiv.jqprint();

    var printIframe = document.createElement("IFRAME");
    document.body.appendChild(printIframe);
    var printDocument = printIframe.contentWindow.document;
    printDocument.designMode = "on";
    printDocument.open();
    printDocument.write("<html><head></head><body>" + wrapperDiv.innerHTML + "</body></html>");
    printIframe.style.position = "absolute";
    printIframe.style.top = "-1000px";
    printIframe.style.left = "-1000px";


    if (document.all) {
        printDocument.execCommand("Print", null, false);
    }
    else {
        printDocument.contentWindow.print();
    }
}

function AbrirAnexo(controle) {
    setTimeout(function () { employer.util.findControl(controle)[0].click(); }, 500);
    return false;
}

function txtFuncaoExercida1_TextChanged(args) {
    employer.util.findControl('btnAplicarFiltro')[0].click();
}

function DispararFiltro(args) {
    employer.util.findControl('btnAplicarFiltro')[0].click();
}

function DispararFiltroAutoComplete(args) {
    employer.util.findControl('btnAplicarFiltroRefazerBusca')[0].click();
    employer.form.util.autoCompleteClientSelected(args);
}

function AjustarMiniCurriculo() {

}

function DispararOrdenacao(btlID, sender, evt) {
    var evtSource;

    evt = (evt) ? evt : window.event;
    evtSource = (evt.srcElement) ? evt.srcElement : evt.target;

    if (evt.target) {
        if (evt.target.nodeType == 3) {
            evtSource = evtSource.parentNode;
        }
    }

    //Se o evento foi disparado de outro controle, que não seja o linkbutton
    if ((evtSource.getAttribute("id") != btlID) && (evt.type == "click")) {    
        var linkCollection = sender.getElementsByTagName("a");

        for (var i = 0; i < linkCollection.length; i++) {
            var onClickAttribute = linkCollection[i].getAttribute("onclick");
            if (onClickAttribute != null) {
                linkCollection[i].onclick();
                break;
            }

            var hrefAttribute = linkCollection[i].getAttribute("href");
            this.location.href = hrefAttribute;
            break;
        }
    }
}

function tryToDownload(url) {
    open(url);
}

function AbrirPopup(url) {
    open(url);
}

function AbrirCurriculo(url) {
    window.open(url, '_new');
}

function AjustarRolagemParaTopo() {
    $('html,body').animate({ scrollTop: 0 });
}