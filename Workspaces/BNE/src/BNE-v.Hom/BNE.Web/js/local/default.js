
$(document).ready(function () {

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
        $(document).on('click', '#a1', function () {
            carousel.scroll(1);
            return false;
        });
        $(document).on('click', '#a2', function () {
            carousel.scroll(2);
            return false;
        });
        $(document).on('click', '#a3', function () {
            carousel.scroll(3);
            return false;
        });
    };
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
        $(document).on('click', '#aImg1', function () {
            carousel.scroll(1);
            return false;
        });
        $(document).on('click', '#aImg2', function () {
            carousel.scroll(5);
            return false;
        });
        $(document).on('click', '#aImg3', function () {
            carousel.scroll(9);
            return false;
        });
    };

    function carrousel_depoimento_itemVisibleInCallbackAfterAnimation(carousel, item, idx, state) {
        if ($("#dep1").attr('src').indexOf('bolinha-') <= 0
            && $("#dep2").attr('src').indexOf('bolinha-') <= 0
             && $("#dep3").attr('src').indexOf('bolinha-') <= 0
            && $("#dep4").attr('src').indexOf('bolinha-') <= 0)
        {
            $("#dep1").attr('src', $("#dep1").attr('src').replace('bolinha.', 'bolinha-azul.'));
            return false;
        }
        else {
            if (item.id == "depoimento1") {
                $("#dep1").attr('src', $("#dep1").attr('src').replace('bolinha.', 'bolinha-azul.'));
                $("#dep2").attr('src', $("#dep2").attr('src').replace('bolinha-azul.', 'bolinha.'));
                // $("#b3").attr('src', $("#b3").attr('src').replace('bolinha-azul.', 'bolinha.'));
                return false;
            }
            else if (item.id == "depoimento2") {
                $("#dep2").attr('src', $("#dep2").attr('src').replace('bolinha.', 'bolinha-azul.'));
                $("#dep3").attr('src', $("#dep3").attr('src').replace('bolinha-azul.', 'bolinha.'));
                // $("#b3").attr('src', $("#b3").attr('src').replace('bolinha-azul.', 'bolinha.'));
                return false;
            }
            else if (item.id == "depoimento3") {
                $("#dep3").attr('src', $("#dep3").attr('src').replace('bolinha.', 'bolinha-azul.'));
                $("#dep4").attr('src', $("#dep4").attr('src').replace('bolinha-azul.', 'bolinha.'));
            //$("#b2").attr('src', $("#b2").attr('src').replace('bolinha-azul.', 'bolinha.'));
             return false;
            }
            else if (item.id == "depoimento4") {
                $("#dep4").attr('src', $("#dep4").attr('src').replace('bolinha.', 'bolinha-azul.'));
                $("#dep1").attr('src', $("#dep1").attr('src').replace('bolinha-azul.', 'bolinha.'));
                //$("#b2").attr('src', $("#b2").attr('src').replace('bolinha-azul.', 'bolinha.'));
                return false;
            }
        }
    };
    function carrousel_depoimento_initCallback(carousel) {
        $(document).on('click', '#d1', function () {
            carousel.scroll(1);
            return false;
        });
        $(document).on('click', '#d2', function () {
            carousel.scroll(2);
            return false;
        });
        $(document).on('click', '#d3', function () {
            carousel.scroll(3);
            return false;
        });
        $(document).on('click', '#d4', function () {
            carousel.scroll(4);
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

    $('#carrousel_depoimento').jcarousel({
        // Configuration goes here
        scroll: 1,
        auto: 5,
        wrap: "circular",
        itemFirstInCallback: {
            onAfterAnimation: carrousel_depoimento_itemVisibleInCallbackAfterAnimation
        },
        initCallback: carrousel_depoimento_initCallback
    });
    //$('.flexslider').flexslider({
    //    controlNav: true,
    //    slideshow: true
    //});
});

function AbrirBoxLateral() {

    $('#divOpen').show('slide', { direction: 'down' }, 1400);
}

function FecharBoxLateral() {

    $('#divOpen').hide('slide', { direction: 'down' }, 1400);
    if (getCookie("PreCadastro") != null)
        document.getElementById('PreCadastroClose').style.display = 'none';

}

function FuncaoOnChangePre() {
    var valor = employer.controles.recuperarValor('txtFuncaoPre');
    if (valor == "")
        employer.util.findControl('divFuncaoInexistentePre').css('display', 'none');
    employer.util.findControl('cvFuncaoPre').css('display', 'none');
}

function cvFuncaoPre_Validate(sender, args) {
    var res = Principal.ValidarFuncao(args.Value);
    args.IsValid = (res.error == null && res.value);
    employer.util.findControl('divFuncaoInexistentePre').css('display', args.IsValid ? 'none' : 'block');
}

function CidadeOnChangePre(sender) {
    var valor = employer.controles.recuperarValor('txtCidadeMaster');


    if (valor == "")
        employer.util.findControl('divCidadeInexistentePre').css('display', 'none');
   // employer.util.findControl('cvCidadePre').css('display', 'none');
}
function cvCidadePre_Validate(sender, args) {
    var res = Principal.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
    employer.util.findControl('divCidadeInexistentePre').css('display', args.IsValid ? 'none' : 'block');
}
