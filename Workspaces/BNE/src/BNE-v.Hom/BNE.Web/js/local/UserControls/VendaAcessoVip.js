//$(document).ready(function() {  
//    window.onunload = GravarUnloadAbandono;
//});



function GravarUnloadAbandono() {
//Comentado por Gieyson
//    var rbCartaoCredito = employer.util.findControl('rbCartaoCredito');
//    
//    if (rbCartaoCredito[0].checked == true)
//        VendaAcessoVip.AbandonoAjax(); 
}

function Imprimir(id, pg) {
    var oPrint, oJan;
    oPrint = window.document.getElementById(id).innerHTML;
    oJan = window.open(pg);
    oJan.document.write(oPrint);
    oJan.history.go();
    oJan.window.print();
}