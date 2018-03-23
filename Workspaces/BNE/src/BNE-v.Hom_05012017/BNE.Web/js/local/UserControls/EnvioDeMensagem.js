var bName = navigator.appName;
var emailChanged = false;
var checkBoxAgendar;
$(document).ready(function () {

    $('.fa-times').click(function () {
        $find('mpeEnvioMensagem').hide();
    });

    $("#cphRodape_ucEnvioMensagem_txtHoraEnvio").text = '';
    $("#ucEnvioMensagem_txtHoraEnvio").text = '';
    $("#cphRodape_ucEnvioMensagem_txtAgendamento").text = '';
    $("#ucEnvioMensagem_txtAgendamento").text = '';
    //$('.date').mask('00/00/0000');
    $.mask.definitions['H'] = "[0-2]";
    $.mask.definitions['h'] = "[0-9]";
    $.mask.definitions['M'] = "[0-5]";
    $.mask.definitions['m'] = "[0-9]";
    var txtHora = $("#cphRodape_ucEnvioMensagem_txtHoraEnvio");
    if (txtHora.length > 0){
        $("#cphRodape_ucEnvioMensagem_txtHoraEnvio").mask("Hh:Mm", { placeholder: " " });
        $("#cphRodape_ucEnvioMensagem_txtAgendamento").mask("99/99/9999");
    }
    else {
        $("#ucEnvioMensagem_txtHoraEnvio").mask("Hh:Mm", { placeholder: " " });
        $("#ucEnvioMensagem_txtAgendamento").mask("99/99/9999");
    }
       

  

    var checkboxsms = $("[id*='ckbSMS']");
    var checkboxemail = $("[id*='ckbEmail']");
    checkBoxAgendar = $("#cphRodape_ucEnvioMensagem_cbAgendar");
    if (checkBoxAgendar.length == 0)
        checkBoxAgendar = $("#ucEnvioMensagem_cbAgendar");

    checkBoxAgendar[0].checked = false;
    checkBoxAgendar.parents('#envio-email-sms').find('#container-agenda').hide();

    $(checkBoxAgendar).on('click', function () {
        if (checkBoxAgendar.is(':checked')) {
            checkBoxAgendar.parents('#envio-email-sms').find('#container-agenda').show();
        } else {
            checkBoxAgendar.parents('#envio-email-sms').find('#container-agenda').hide();
        }
    });

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
    checkBoxAgendar[0].checked = false;
    checkBoxAgendar.parents('#envio-email-sms').find('#container-agenda').hide();
    emailChanged = false;

    $('.contagem-caracteres').css('display', 'none');
    AjustarCheckboxSMS();
    AjustarCheckboxEmail();
}

function pageLoad() {
    var mpe = $find('mpeEnvioMensagem');

    if (mpe != null) {
        mpe.add_shown(AjustarEnvioMensagem);
    }
}