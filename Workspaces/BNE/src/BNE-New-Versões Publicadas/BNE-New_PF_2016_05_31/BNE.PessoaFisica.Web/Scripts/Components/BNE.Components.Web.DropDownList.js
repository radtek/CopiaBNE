//validar campo DropDown
bne.components.web.dropdownlist = function (control,required) {
    control = 'input:text[id*=' + control + ']';
    validateRequired: function validateRequired(control,required) {
        $(control).rules("add", { required: required, min:1, messages: { required: 'Obrigatório', min:'Selecione uma opção.' } });
    }

    validateRequired(required);
}