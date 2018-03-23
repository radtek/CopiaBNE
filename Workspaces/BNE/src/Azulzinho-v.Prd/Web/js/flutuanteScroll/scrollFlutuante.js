// JavaScript Document
$(document).ready(function () {
    //Ajustando o scroll
    $(window).scroll(function () {
        //$("#divSlowDown").stop().animate({ "top": $(window).scrollTop() + ($(screen)[0].height / 4) + "px" }, { duration: 700, queue: false });
        var vTop = ($(window).height() / 2) - $("#divSlowDown").height() / 2 + $(top.window).scrollTop();

        //Ajustando a altura para não estourar o layout
        if (vTop < 80)
            vTop = 80;
        else if (vTop > 830)
            vTop = 825;

        $("#divSlowDown").stop().animate({ "top": vTop + "px" }, { duration: 700, queue: false });
    });

    //Ajustando o background
    //$('body').addClass('bg_fundo_empresa');
});
