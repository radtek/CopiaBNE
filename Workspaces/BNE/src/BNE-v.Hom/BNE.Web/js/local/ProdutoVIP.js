$(document).ready(function () {
    assinarClickVantagens();
});

function assinarClickVantagens() {
    $("#listaVantagens li a").live('click',
       function () {
           var li = $(this).closest('li');
           var px = li.offset().top - $("#container_lista").offset().top - 4;
           $('.seta_laranja').css('top', px + 'px');

           var id = $(this).attr("data-vantagem");
           EfetuarRequisicaoVantagem(id);
       }
   );
}

function associarSeta(vantagem) {
    $("#listaVantagens li a").each(function () {
        var id = $(this).attr("data-vantagem");
        if (id === vantagem) {
            var li = $(this).closest('li');
            var px = li.offset().top - 335;
            $('.seta_laranja').css('top', px + 'px');
        }
    });
}

function EfetuarRequisicaoVantagem(id) {
    var dados = "{'idVantagem':'" + id + "'}";
    $.ajax({
        type: "POST",
        url: "/produtovip.aspx/GetVantagens",
        data: dados,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (retorno) {
            employer.util.findControl('container_vantagem').html(retorno.d.html);
        }
    });
}