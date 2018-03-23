$(document).ready(function () {
    //Define o click dos ícones conforme links do rodapé
    //Atualizar Currículo
    $("div[id$='pnlServicoAtualizarCurriculo'] a").attr("href", $("a[id$='btlAtualizarCurriculo']").attr("href"));

    //Quem me Viu?
    $("div[id$='pnlServicoQuemMeViu'] a").attr("href", $("a[id$='btlSalaVipQuemMeViu']").attr("href"));

    //Sala VIP
    $("div[id$='pnlServicoSalaVIP'] a").attr("href", $("a[id$='btlSalaVIP']").attr("href"));

    //VIP
    $("div[id$='pnlServicoVIP'] a").attr("href", $("a[id$='btlCompreVip']").attr("href"));
});