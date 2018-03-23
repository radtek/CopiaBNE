
jQuery(document).ready(function () {

    // android
    var ua = navigator.userAgent.toLowerCase();
    var isAndroid = ua.indexOf("android") > -1; //&& ua.indexOf("mobile");
    if (isAndroid) {
        $('.modalMobile').show();
    } else {
        $('.modalMobile').hide();
    }

    function carrousel_servicos_itemVisibleInCallbackAfterAnimation(carousel, item, idx, state) {
        if ($("#b1").attr('src').indexOf('bolinha-') <= 0
            && $("#b2").attr('src').indexOf('bolinha-') <= 0)
            // && $("#b3").attr('src').indexOf('bolinha-') <= 0 
        {
            $("#b1").attr('src', $("#b1").attr('src').replace('bolinha.', 'bolinha-azul.'));
            return false;
        }
        else {
            if (item.id == "Item1") {
                $("#b1").attr('src', $("#b1").attr('src').replace('bolinha.', 'bolinha-azul.'));
                $("#b2").attr('src', $("#b2").attr('src').replace('bolinha-azul.', 'bolinha.'));
                // $("#b3").attr('src', $("#b3").attr('src').replace('bolinha-azul.', 'bolinha.'));
                return false;
            }
            else if (item.id == "Item2") {
                $("#b2").attr('src', $("#b2").attr('src').replace('bolinha.', 'bolinha-azul.'));
                $("#b1").attr('src', $("#b1").attr('src').replace('bolinha-azul.', 'bolinha.'));
                // $("#b3").attr('src', $("#b3").attr('src').replace('bolinha-azul.', 'bolinha.'));
                return false;
            }
            //else if (item.id == "Item3") {
            // $("#b3").attr('src', $("#b3").attr('src').replace('bolinha.', 'bolinha-azul.'));
            // $("#b1").attr('src', $("#b1").attr('src').replace('bolinha-azul.', 'bolinha.'));
            //$("#b2").attr('src', $("#b2").attr('src').replace('bolinha-azul.', 'bolinha.'));
            // return false;
            // }
        }
    };
    function carrousel_servicos_initCallback(carousel) {
        $("#b1").click(function () {
            carousel.scroll(1);
            return false;
        });
        $("#b2").click(function () {
            carousel.scroll(2);
            return false;
        });

        function carrousel_empresas_itemVisibleInCallbackAfterAnimation(carousel, item, idx, state) {
            if ($("#Img1").attr('src').indexOf('bolinha-') <= 0
            && $("#Img2").attr('src').indexOf('bolinha-') <= 0
            && $("#Img3").attr('src').indexOf('bolinha-') <= 0) {
                $("#Img1").attr('src', $("#Img1").attr('src').replace('bolinha.', 'bolinha-azul.'));
                return false;
            }
            else {
                if (item.id == "ItemEmpresa1") {
                    $("#Img1").attr('src', $("#Img1").attr('src').replace('bolinha.', 'bolinha-azul.'));
                    $("#Img2").attr('src', $("#Img2").attr('src').replace('bolinha-azul.', 'bolinha.'));
                    $("#Img3").attr('src', $("#Img3").attr('src').replace('bolinha-azul.', 'bolinha.'));
                    return false;
                }
                else if (item.id == "ItemEmpresa5") {
                    $("#Img2").attr('src', $("#Img2").attr('src').replace('bolinha.', 'bolinha-azul.'));
                    $("#Img1").attr('src', $("#Img1").attr('src').replace('bolinha-azul.', 'bolinha.'));
                    $("#Img3").attr('src', $("#Img3").attr('src').replace('bolinha-azul.', 'bolinha.'));
                    return false;
                }
                else if (item.id == "ItemEmpresa9") {
                    $("#Img3").attr('src', $("#Img3").attr('src').replace('bolinha.', 'bolinha-azul.'));
                    $("#Img1").attr('src', $("#Img1").attr('src').replace('bolinha-azul.', 'bolinha.'));
                    $("#Img2").attr('src', $("#Img2").attr('src').replace('bolinha-azul.', 'bolinha.'));
                    return false;
                }
            }
        };
        function carrousel_empresas_initCallback(carousel) {
            $("#Img1").click(function () {
                carousel.scroll(1);
                return false;
            });
            $("#Img2").click(function () {
                carousel.scroll(5);
                return false;
            });
            $("#Img3").click(function () {
                carousel.scroll(9);
                return false;
            });

        };
        jQuery('#carrousel_empresas').jcarousel({
            // Configuration goes here
            scroll: 4,
            auto: 5,
            wrap: "circular",
            itemFirstInCallback: {
                onAfterAnimation: carrousel_empresas_itemVisibleInCallbackAfterAnimation
            },
            initCallback: carrousel_empresas_initCallback
        });
    };
    $('#carrousel_servicos').jcarousel({
        // Configuration goes here
        scroll: 1,
        auto: 4,
        wrap: "circular",
        itemFirstInCallback: {
            onAfterAnimation: carrousel_servicos_itemVisibleInCallbackAfterAnimation
        },
        initCallback: carrousel_servicos_initCallback
    });
    $('.flexslider').flexslider({
        controlNav: true,
        slideshow: true
    });
});