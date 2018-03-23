$(document).ready(function () {
    //Ajustando o scroll
    $(window).scroll(function () {
        var vTop = ($(window).height() / 2) - $("#divSlowDown").height() / 2 + $(top.window).scrollTop();

        //Ajustando a altura para não estourar o layout
        if (vTop < 650)
            vTop = 650;
        else if (vTop > 1930)
            vTop = 1925;

        //if(vTop >850
        $("#divSlowDown").stop().animate({ "top": vTop + "px" }, { duration: 700, queue: false });
    });

    //Ajustando o background
    $('body').addClass('bg_fundo_empresa');

    //ocultando a div de aviso sobre a data de demissão
    $('.tooltips_aviso').hide();
    //apager este comentario.
});