//validar campo DropDown
bne.components.web.dropdownlist = function (control, required) {
    var currentControl = '[id*=' + control + ']';
    validateRequired: function validateRequired(controle, required) {
        $(controle).rules("add", { required: required, min: 1, messages: { required: 'Obrigatório', min: 'Selecione uma opção.' } });
    }

    validateRequired(currentControl, required);
}