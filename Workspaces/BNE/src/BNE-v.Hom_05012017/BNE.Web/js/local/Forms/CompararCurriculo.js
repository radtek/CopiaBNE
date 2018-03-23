function pageLoad() {
    MontarImpressaoCompararCv();
}

function OnClientCheckItem() {
    employer.util.findControl('btnAplicarColunas')[0].click();
}

function ImprimirComparacao(idDiv) {
    if (idDiv) {
        //build html for print page
        var html = "<html><head><title>Comparar Currículos - BNE</title><link href=\"/css/local/CompararCurriculoImprimir.css\" rel=\"stylesheet\" type=\"text/css\" /></head><body>" + $("#" + idDiv).html() + "</body></html>";
        //open new window
        var printWP = window.open("", "printWebPart");
        printWP.document.open();
        //insert content
        printWP.document.write(html);
        printWP.document.close();
        //open print dialog
        printWP.print();
    }
}

//Prepara o conteúdo para impressão com um container que é mostrado
//somente através da @media print do CSS
function MontarImpressaoCompararCv() {
    var conteudoImpressao;
    
    //Limpa o conteúdo da impressão
    jQuery(".corpo_impressao").empty();

    //Define o conteúdo da impressão
    jQuery("div#divImpressaoCompararCurriculo div table").clone().appendTo(".corpo_impressao");

    //Remove o id dos elementos da impressão para evitar conflito com os JS do Asp.Net
    jQuery(".corpo_impressao *").removeAttr("id");

    //Atualiza as informações de data para impressão
    //[Este método está em /js/global/geral.js]
    AtualizarInfosImpressao();
}

//Comanda a impressão (gerenciador do navegador)
function ImprimirCompararCv() {
    MontarImpressaoCompararCv();

    window.print();
}