// JavaScript Document
$(document).ready(function () {
    //Ajustando o scroll
    $(window).scroll(function () {
        var vTop = ($(window).height() / 2) - $(".divSlowDown").height() / 2 + $(top.window).scrollTop();

        //Ajustando a altura para não estourar o layout
        if (vTop < 40)
            vTop = 40;
        /*else if (vTop > 830)
            vTop = 825;*/

        $(".divSlowDown").stop().animate({ "top": vTop + "px" }, { duration: 700, queue: false });
    });
});

var showConvite = false;
var showConvocacao = false;
var showUltima = false;

$(document).ready(function () {
    pageLoad();

    $("#lblConvite").css('display', 'block');
    $("#edicao-convite").css('display', 'none');
    
    $("#lblConvocacao").css('display', 'block');
    $("#edicao-convocacao").css('display', 'none');

    $("#lblUltima").css('display', 'block');
    $("#edicao-ultima").css('display', 'none');
});

function ToggleConvite() {
    $("#lblConvite").css('display', showConvite ? 'none' : 'block');
    $("#edicao-convite").css('display', showConvite ? 'block' : 'none');
}
function ToggleConvocacao() {
    $("#lblConvocacao").css('display', showConvocacao ? 'none' : 'block');
    $("#edicao-convocacao").css('display', showConvocacao ? 'block' : 'none');
}
function ToggleUltima() {
    $("#lblUltima").css('display', showUltima ? 'none' : 'block');
    $("#edicao-ultima").css('display', showUltima ? 'block' : 'none');
}

function pageLoad() {
    ToggleConvite();
    ToggleConvocacao();
    ToggleUltima();

    $("#btConvite, #btConvocacao, #btUltima").unbind("click");

    $("#btConvite").on('click', function (e) {
        e.preventDefault();
        showConvite = !showConvite;
        ToggleConvite();
    });

    $("#btConvocacao").on('click', function (e) {
        e.preventDefault();
        showConvocacao = !showConvocacao;
        ToggleConvocacao();
    });

    $("#btUltima").on('click', function (e) {
        e.preventDefault();
        showUltima = !showUltima;
        ToggleUltima();
    });

    $("#txtConvite").keyup(function () {
        $("#lblConvite").text($(this).val());
    });
    $("#txtConvocacao").keyup(function () {
        $("#lblConvocacao").text($(this).val());
    });
    $("#txtUltima").keyup(function () {
        $("#lblUltima").text($(this).val());
    });
}