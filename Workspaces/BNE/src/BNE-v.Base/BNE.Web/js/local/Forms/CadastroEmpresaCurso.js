function cvNivel_Validate(sender, args) {
    var value = CadastroEmpresaCurso.ValidaDropDown(employer.util.findControl('ddlNivel')[0].value).value;
    if (value == '0')
        args.IsValid = false;
    else
        args.IsValid = true;
}

function TravaTexbox() {

    tecla = event.keyCode;
    if (tecla >= 48 && tecla <= 57) {
        return true;

    }
    else {
        return false;
    }

}

function OnBlurDuracaoAno() {

    var duracaoAno = employer.controles.recuperarValor('txtDuracaoAnos');
    if (duracaoAno == "") {
        employer.controles.setValor('txtDuracaoAnos', 0);
    }

}


function OnBlurDuracaoMes() {

    var duracaoMes = employer.controles.recuperarValor('txtDuracaoMeses');
    if (duracaoMes == "") {
        employer.controles.setValor('txtDuracaoMeses', 0);
    }

}

function DuracaoMes_Validate(sender, args) {
    var duracaoMes = employer.controles.recuperarValor('txtDuracaoMeses');
    var duracaoAno = employer.controles.recuperarValor('txtDuracaoAnos');
    
    if (duracaoAno == '') {
        employer.controles.setValor('txtDuracaoAnos', 0);
        duracaoAno = 0;    
    }

    var resultado = CadastroEmpresaCurso.ValidaDuracao(duracaoAno, duracaoMes).value;

    if (resultado == 0)
        args.IsValid = false;

}

function ImprimirVisualizacao(divImprimir) {
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