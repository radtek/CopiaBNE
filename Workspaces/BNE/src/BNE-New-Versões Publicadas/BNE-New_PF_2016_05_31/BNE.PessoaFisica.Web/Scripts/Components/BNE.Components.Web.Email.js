bne.components.web.email = {
    configure: function (control, url, data, selectCallback, type) {
        $(document.body).on('focus', control, function () {
            $(this).autocomplete({
                autoFocus: true,
                source: function (request, response) {

                   var data = ['@hotmail.com','@gmail.com','@yahoo.com.br']
                   return data;
                },
                cache: false,
                select: function (event, ui) {                  

                	this.value = ui.item.label;

                	if (selectCallback != null) {
                        selectCallback();
                    }
                    return false;
                },
                delay: 0,
                minLength: 0,
                focus: function (event, ui) {
                    if (type === bne.components.web.email.type.bairro) {
                        return false;
                    }
                    var menu = $(this).data("ui-autocomplete").menu.element;
                    menu.find("li").removeClass("ui-state-hover");
                    menu.find("li:has(a.ui-state-focus)").addClass("ui-state-hover");
                },
                close: function (event, ui) {
                    var menu = $(this).data("ui-autocomplete").menu.element;
                    menu.find("li").removeClass("ui-state-hover");
                }
            });
        });
    },
    validateRequired: function validateRequired(control) {
        $(control).rules("add", { required: true, messages: { required: 'Obrigatório' } });
    },
    email: function(control,required,validateInput,callback){
        control = 'input:text[id*=' + control + ']';
        var data = { limit: bne.components.web.email.limit.LimiteCompleteEmail };
        bne.components.web.email.configure(control, bne.components.web.email.URL.URLCompleteEmail, data, callback, bne.components.web.email.type.email);

        if(required != null && required) {
            bne.components.web.email.validateRequired(control);
        }
        if(validateInput !=null && validateInput)
        {
            bne.components.web.email.validacao.email(control);
        }
    },
    split: function (val) {
        return val.split(/,\s*/);
    },
    extractLast: function (term) {
        return bne.components.web.email.split(term).pop();
    },
    type: {
        email: 'Email'
    },
    URL: {
        URLCompleteEmail: 'http://localhost:51218/api/pessoafisica/RankingEmail?nome='
    },
    limit: {
        LimiteCompleteEmail: '10',

    },
    validacao: {
        email: function (control) {
            $.validator.addMethod("validateFormatEmail", function (email, element) {
                if (email == null || email == '')
                    return false;

                return (/^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$/.test(email))
            }, "Email inválido!");

            $(control).rules("add", { validateFormatEmail: true });
        }
    }
};