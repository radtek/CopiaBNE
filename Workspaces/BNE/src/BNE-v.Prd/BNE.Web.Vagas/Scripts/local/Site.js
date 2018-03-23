//Banner posso ajudar
function banner_posso_ajudar_build()
{

    function alo_web_vip_verificar_disponibilidade(jalo)
    {
        var disponivel = false;
        if (jalo.disponibilidade[alo_web_token_vip_id] != undefined && jalo.disponibilidade[alo_web_token_vip_id] != null) {
            disponivel = jalo.disponibilidade[alo_web_token_vip_id] == true;
        }

        if (!disponivel)
        {
            $("#banner-posso-ajudar").hide();
        }
        else
        {
            $("#banner-posso-ajudar").click(function ()
            {
                //--- Abrir atendimento online
                var url_chat = 'http://' + document.domain + '/WebServices/wsUtils.asmx/MontarURLAtendimento';
                var xhr2 = new XMLHttpRequest();
                xhr2.onreadystatechange = function () {
                    if (xhr2.readyState === 4) {
                        if (xhr2.status === 200) {

                            var urlAtendimento = xhr2.responseXML.getElementsByTagName("string")[0].innerHTML;


                            console.log(urlAtendimento);

                            alochat = window.open(urlAtendimento, 'alo49939200', 'scrollbars=no ,width=522, height=533 , top=50 , left=50');
                        }
                    }
                };

                xhr2.open('POST', url_chat);
                xhr2.send(null);
                //---------------------------
            });
        }
    }


    var alo_web_token_vip = '6OucyZIfHCFg93mDQOQZkTlxOMNYaPByKXpOgoT8';
    var alo_web_token_vip_id = 89;

    var xhr = new XMLHttpRequest();

    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) {
            if (xhr.status === 200) {
                alo_web_vip_verificar_disponibilidade(JSON.parse(xhr.responseText));
            }
        }
    };

    xhr.open('GET', 'https://v4.aloweb.com.br/storage/alobot_personalizacao-' + alo_web_token_vip + '.json');
    xhr.send(null);

    return;
}
//---- Fim do banner posso ajudar





$(function () {
    positionFooter();

    function positionFooter() {
        if ($("#conteudo").length == 0)
            return;

        var bodyHeight = $(document).height();
        var windowHeight = $(window).height();

        if (bodyHeight > windowHeight) {
            $("#barra_rodape").css({
                margin: "0 0 0 0"
            });
            return;
        }
        var contentInitial = $("#header").height();
        var contentHeight = $("#conteudo").height();

        if (contentHeight == 0
            || contentHeight == typeof (undefined)) {
            return;
        }

        var heightBottom = 96;

        var sumOfPage = (Math.max(contentInitial, $("#conteudo").position().top) + contentHeight + heightBottom) + 35;
        if (windowHeight > sumOfPage) {
            var difference = windowHeight - sumOfPage;
            if (difference < 0)
                difference = 0;

            $("#barra_rodape").css({
                margin: difference + "px 0 0 0"
            });
        }
    }

    $(window)
        .resize(function () {
            positionFooter();
        });
});

$(document).ready(
    function () {
        autocomplete.funcao('txtFuncao');
        autocomplete.cidade('txtCidade');
        banner_posso_ajudar_build();
    }
);
