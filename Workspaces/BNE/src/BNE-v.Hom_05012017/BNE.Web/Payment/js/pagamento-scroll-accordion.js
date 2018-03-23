
$(document).ready(function () { 

    $('#updCartaoDeCredito, #updDebitoOnline, #updBoleto, #updDebitoEmConta, #updPagseguro, #updPaypal').addClass('payment-method panel').css('display', 'block');

    var hasClicked = false;
    $('.payment-method-type').click(function (e)
    {
       
        if (!hasClicked) {
            $('html, body').animate({
                scrollTop: $(this).offset().top - 8
            }, 'slow');
            hasClicked = !hasClicked;
        }
    });

    $('#accordion').on('hidden.bs.collapse', function ()
    {
        $('html, body').animate({
            scrollTop: $("div[aria-expanded='true']").offset().top - 8
        }, 'slow');
    });
    $("body.mobile-payment").css("overflow-y", "hidden");
});


function scroll_to_anchor(anchor_id) {

    if (exibirLogin(true)) return;

    var section = $("#" + anchor_id + "");
    section.addClass("active");
    $("body.mobile-payment").css("background-color", section.attr("next-color"));
    $("body.mobile-payment").css("overflow-y", "hidden");
    setTimeout(function () {

        $('html, body').animate({ scrollTop: section.offset().top }, 750, function () {

            section.prev().removeClass("active");
            $("body.mobile-payment").css("overflow-y", "auto");
        });
    }, 50);


    if ("payment-stage" == anchor_id) {

        $("#welcome-stage").hide();
        $("#advantages01-stage").hide();
        $("#advantages02-stage").hide();
        $("#summary-stage").hide();
        $("body.mobile-payment").css("overflow-y", "auto");
    }
    return false;
}


$('#accordion').on('hidden.bs.collapse', function () {
    $('html, body').animate({
        scrollTop: $("div[aria-expanded='true']").offset().top - 8
    }, 'slow');
});

