//Configurações do Swfobject
var flashvars = {};

var params = {};
params.wmode = "transparent";

var attributes = {};
attributes.id = "MapaBrasil";

swfobject.embedSWF("/swf/MapaBrasil02_CS4_bne.swf", "object_flash", "306", "320", "10.0.0", "swf/expressInstall.swf", flashvars, params, attributes, SwfobjectCallback);

//Função de Callback do Swfobject
function SwfobjectCallback(e) {
    //Caso o flash não seja carregado troca o texto para indicar a seleção pela tabela
    if (e.success == false) {
        $(".texto_selecione").html("Selecione um estado na tabela ao lado");
    }
}

//Mostra as informações das filiais
function MostrarFiliais(ufSelecionada) {
    //Esconde todas as informações das filiais
    $(".texto_filiais div").css("display", "none");

    //Mostra somente as informações das filiais para o parâmetro informado
    $("#uf" + ufSelecionada).css("display", "block");

    //Mostra as outras colunas do PR
    if (ufSelecionada == "MT") {
        $("#uf" + ufSelecionada + "2").css("display", "block");
    }

    //Mostra as outras colunas do PR
    if (ufSelecionada == "PR") {
        $("#uf" + ufSelecionada + "2").css("display", "block");
        $("#uf" + ufSelecionada + "3").css("display", "block");
    }

    //Mostra as outras colunas de SC
    if (ufSelecionada == "SC") {
        $("#uf" + ufSelecionada + "2").css("display", "block");
    }
}

$(function ($) {
    $next = $('.next');
    $list = $('ul.menu_onde');

    var aux = {
        clone_item: function (list) {
            this.list = list
            $(list).find('li:first').appendTo(list);
        }
    }

    $next.click(function (event) {
        aux.clone_item($list);
        event.preventDefault();
    });
});