$(document).ready(function () {

  
    //   //$('.date').mask('00/00/0000');
    $.mask.definitions['H'] = "[0-2]";
    $.mask.definitions['h'] = "[0-9]";
    $.mask.definitions['M'] = "[0-5]";
    $.mask.definitions['m'] = "[0-9]";
  
    var txtHora = $("#cphConteudo_ucPlanoFidelidade_ucObservacaoFilial_txtHora");
    if (txtHora.length > 0) {
        $("#cphConteudo_ucPlanoFidelidade_ucObservacaoFilial_txtHora").mask("Hh:Mm", { placeholder: " " });
    }
    else {
        $("#cphConteudo_ucObservacaoFilial_txtHora").mask("Hh:Mm", { placeholder: " " });
        
    }
 
});