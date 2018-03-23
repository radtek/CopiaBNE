function ModalVendaInformacaoEmpresa() { };
ModalVendaInformacaoEmpresa.Show = function () {
    $('#modalVendaInformacaoEmpresa').modal({
        backdrop: "static"
    });
};
ModalVendaInformacaoEmpresa.Hide = function () {
    $('#modalVendaInformacaoEmpresa').modal('hide');
};

$(document).ready(function () {
    $(document.body).on('click', '.fa-times', function () {
        ModalVendaInformacaoEmpresa.Hide();
    });
    $(document.body).on('click', '.btn_cancel', function () {
        ModalVendaInformacaoEmpresa.Hide();
    });
    autocomplete.funcao('txtFuncao');
});