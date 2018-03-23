"use strict";

$(document).ready(function () {


    jQuery.validator.addMethod("nomeCompleto", function (value, element) {
        value = value.trim();
        var patt = /[^a-zàáâäãèéêëėìíîïńòóôöõùúûüñç' ]+/i
        return !patt.test(value);
    }, "O nome informado é inválido.");

    
    function Check9DigitsI3(ddd)
    {
        var dddNoveDigitos = ['11', '12', '13', '14', '15', '16', '17', '18', '19', '21', '22',
            '24', '27', '28', '91', '92', '93', '94', '95', '96', '97', '98', '99'];
        return (dddNoveDigitos.indexOf(ddd) > -1);
    }

    jQuery.validator.addMethod("celular", function (value, element) {
        var patt = /[6-9][0-9]{7}/;
        var patt9 = /[6-9][0-9]{8}/;
        var rawFone = $(element).cleanVal() || "";

        if (rawFone.length > 9) {
            var cel_ddd = rawFone.substring(0, 2);
            var cel_num = rawFone.substring(2);

            if (Check9DigitsI3(cel_ddd)) {
                if (cel_num.length != 9)
                    return false;
                else
                    return patt9.test(cel_num);
            }
            else {
                if (cel_num.length != 8)
                    return false;
                else
                    return patt.test(cel_num);
            }
        }
        else
            return false;
    }, "O celular informado é inválido.");



    $('#cadastroForm').validate({
        rules: {
            NmePessoa: { required: true, nomeCompleto: true },
            Celular: { required: true, celular: true },
            Profissao: { required: true },
            Email: {email: true}
        },
        errorElement: 'span',
        errorClass: 'error',
        errorPlacement: function (error, element) {
            var placement = element.data('error');
            if (placement)
                element.parent().append(error);
            else
                error.insertAfter(element.parent());
        }
    });


    $('.btn-salvar').click(function () {
        $('#cadastroForm').validate();
        if ($('#cadastroForm').valid())
        {
            $.blockUI({ message: '<div style="padding: 10px;"><img src="/Content/img/preloader.gif" /> Salvado informações...</div>' });
            $('#cadastroForm').submit();
        }
    });


    $("#profissao").autocomplete({ source: function (request, response) { $.post("/Funcao/Pesquisar", request, response); }, minLength: 3 });
    $('#celular').mask('(00) 0000-0000Z', { clearIfNotMatch: true, translation: { 'Z': { pattern: /[0-9]/, optional: true } } });
});