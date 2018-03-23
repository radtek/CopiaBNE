
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

});