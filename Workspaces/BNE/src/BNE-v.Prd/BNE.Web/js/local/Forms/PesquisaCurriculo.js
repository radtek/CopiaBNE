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

function AbrirBoxLateral() {

    $('#DivBotoesLateraisAberto').show('slide', { direction: 'right' }, 1400);
}

function FecharBoxLateral() {

    $('#DivBotoesLateraisAberto').hide('slide', { direction: 'right' }, 1400);
}


function SalarioBR() {
  
    if (document.getElementById('cphConteudo_IdPF').value.length > 0 &&
        document.getElementById('cphConteudo_IdFi').value.length > 0) {
        var dados = "{'IdPF':'" + document.getElementById('cphConteudo_IdPF').value + "', 'Idf':'" + document.getElementById('cphConteudo_IdFi').value + "'}";
            $.ajax({
            type: "POST",
            url: "/ajax.aspx/SalarioBR",
            data: dados,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (retorno) {

            }
        });
    }
}

function AbrirModalFiltros(){
    $("#filtrar_busca_cv-modal").show();
    document.getElementById('spModal').style.display = 'block';
}

function FecharModalFiltros() {
    $("#filtrar_busca_cv-modal").hide();
    document.getElementById('spModal').style.display = 'none';
}

$("#filtrar_busca_cv-filtros_utilizados").hide();
$('#filtrar_busca_cv-modal').on('hidden.bs.modal', function (e) {
    $("#filtrar_busca_cv-filtros_utilizados").show();
})

function MostrarCarregando() {
    $("#upgGlobalCarregandoInformacoes").show();
}

function AnexoSine(curriculo) {

    var dados = "{'curriculo':" + curriculo + "}";
    $.ajax({
        type: "POST",
        url: "/ajax.aspx/AnexoSine",
        data: dados,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: true,
        success: function (retorno) {
            console.log(retorno);
            if (retorno.d) {
                ExibirDownloadSineCV(curriculo);
            }
        }
    });
}