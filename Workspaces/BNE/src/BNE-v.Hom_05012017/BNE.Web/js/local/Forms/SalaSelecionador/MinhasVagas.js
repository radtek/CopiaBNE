
function ImprimirVisualizacaoCurriculo(divImprimir) {
    alert('minhas vagas imprimir');
    var wrapperDiv = employer.util.findControl(divImprimir)[0];
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