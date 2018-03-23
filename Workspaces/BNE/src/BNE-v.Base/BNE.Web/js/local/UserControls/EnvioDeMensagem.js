var bName = navigator.appName;
var emailChanged = false;

$(document).ready(function () {

    $('.fa-times').click(function () {
        $find('mpeEnvioMensagem').hide();
    });


    var checkboxsms = $("[id*='ckbSMS']");
    var checkboxemail = $("[id*='ckbEmail']");
    checkboxsms.on('click', function () {
        AjustarCheckboxSMS();
    });

    checkboxemail.on('click', function () {
        AjustarCheckboxEmail();
    });

    $("body").on("click", ".suggest-text", function () {
        $("[id*='txtSMS']").val($(this).text().trim());
        $("[id*='txtEmail']").val($(this).text().trim());
    });

    $("body").on("keyup", "[id*='txtSMS']", function () {
        $('.contagem-caracteres').css('display', 'block');
        $('.suggest').css('display', 'none');

        if (!emailChanged) {
            $("[id*='txtEmail']").val($(this).val());
        }
    });

    $("body").on("keyup", "[id*='txtEmail']", function () {
        emailChanged = true;
    });

});

function AjustarCheckboxSMS() {
    var checkboxsms = $("[id*='ckbSMS']");
    var checkboxemail = $("[id*='ckbEmail']");

    if (checkboxsms.is(':checked')) {
        checkboxsms.parents('#envio-email-sms').find('#container-sms').show();
        checkboxemail.removeAttr('disabled');
    } else {
        checkboxsms.parents('#envio-email-sms').find('#container-sms').hide();
        if (!checkboxsms.is(':checked')) {
            checkboxemail.attr('disabled', 'disabled');
        }
    }
}

function AjustarCheckboxEmail() {
    var checkboxsms = $("[id*='ckbSMS']");
    var checkboxemail = $("[id*='ckbEmail']");

    if (checkboxemail.is(':checked')) {
        checkboxemail.parents('#envio-email-sms').find('#container-email').show();
        checkboxsms.removeAttr('disabled');
    } else {
        checkboxemail.parents('#envio-email-sms').find('#container-email').hide();
        if (!$(checkboxemail).is(':checked')) {
            checkboxsms.attr('disabled', 'disabled');
        }

    }
}

function createObject(objId) {
    if (document.getElementById) return document.getElementById(objId);
    else if (document.layers) return eval("document." + objId);
    else if (document.all) return eval("document.all." + objId);
    else return eval("document." + objId);
}

function AjustarEnvioMensagem() {
    $("[id*='ckbSMS']").attr('checked', 'true');
    $("[id*='ckbEmail']").attr('checked', 'true');

    emailChanged = false;

    $('.contagem-caracteres').css('display', 'none');

    AjustarCheckboxSMS();
    AjustarCheckboxEmail();
}

function pageLoad() {
    $find('mpeEnvioMensagem').add_shown(AjustarEnvioMensagem);
}