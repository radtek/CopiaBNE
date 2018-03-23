"use strict";

$(document).ready(function ()
{

    jQuery.validator.addMethod("bneDateValidator", function (value, element) {
        return new Date().validateBR(value);
    }, 'A data informada não é válida.');

    jQuery.validator.addMethod("bneCPFValidator", function (value, element) {
        return Util.TestCPF($(element).cleanVal());
    }, 'O CPF informado é inválido.');


    $('#loginForm').validate({
        rules: { cpf: { required: true, bneCPFValidator: true }, nascimento: { required: true, bneDateValidator: true } },
        errorElement: 'span',
        errorClass: 'error',
        errorPlacement: function (error, element)
        {
            var placement = element.data('error');
            if (placement) 
                element.parent().append(error);
            else 
                error.insertAfter(element.parent());
        }
    });


    $('#cpf').mask('000.000.000-00', { clearIfNotMatch: true });
    $('#nascimento').mask('00/00/0000', { clearIfNotMatch: true });

    $('#cpf, #nascimento').focusin(function ()  { $("#ErrorMessage").fadeOut(); });
    //$('#cpf, #nascimento').focusout(function () {
    //    $('#btnEntrar').prop('disabled', !$('#loginForm').valid());
    //    if($('#loginForm').valid())
    //    {
    //        $('#btnEntrar').removeClass('bloqueado');
    //    }
    //    else
    //    {
    //        $('#btnEntrar').addClass('bloqueado');
    //    }
    //});

});