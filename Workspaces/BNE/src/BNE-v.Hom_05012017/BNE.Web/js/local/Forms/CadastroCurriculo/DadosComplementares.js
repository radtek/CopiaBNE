function ddlTipoVeiculo_SelectedIndexChanged(sender, args) {
    var value = sender._value;
    if (value != '0') {
        employer.controles.enableValidatorControleVal('txtModelo', 'rfValor', true, false);
        employer.controles.setAttr('txtModelo', 'Obrigatorio', true);
        employer.controles.enableValidatorControleVal('txtAnoVeiculo', 'rfValor', true, false);
        employer.controles.setAttr('txtAnoVeiculo', 'Obrigatorio', true);

    }
    else {
        employer.controles.enableValidatorControleVal('txtModelo', 'rfValor', false, true);
        employer.controles.setAttr('txtModelo', 'Obrigatorio', false);
        employer.controles.enableValidatorControleVal('txtAnoVeiculo', 'rfValor', false, true);
        employer.controles.setAttr('txtAnoVeiculo', 'Obrigatorio', false);
    }
}    
    
function cvTipoVeiculo_Validate(sender, args) {
    var value = employer.util.findControl('ddlTipoVeiculo')[0].value;
    if (value == '0')
        args.IsValid = false;
    else
        args.IsValid = true;
}

function cvCidadeDisponivel_Validate(sender, args) {
    var res = DadosComplementares.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function cvDisponibilidade_Validate(sender, args) {
    var ckList = employer.util.findControl('ckblDisponibilidade');
    var checkeds = ckList.find('input:checked');
    if (!checkeds.length) {
        args.IsValid = false;
    }
    else {
        args.IsValid = true;
    }
}

function auUpload_ClientFileUploaded(sender, args) {
    var ajaxManager = employer.util.findControl('ramUpload');
    //$find(ajaxManager[0].id).ajaxRequest();
    //sender.remove_fileUploaded(); // = remove_fileUploaded(h)(); //.deleteFileInputAt(0);
}

function auUpload_ClientFileUploadFailed(sender, args) {
    var ajaxManager = employer.util.findControl('ramUpload');
    //$find(ajaxManager[0].id).ajaxRequest();
    //sender.remove_fileUploaded(); // = remove_fileUploaded(h)(); //.deleteFileInputAt(0);
}

function auUpload_ClientFileValidateFailed(sender, args) {
    var ajaxManager = employer.util.findControl('ramUpload');
    //$find(ajaxManager[0].id).ajaxRequest();
    //sender.remove_fileUploaded(); // = remove_fileUploaded(h)(); //.deleteFileInputAt(0);
}

function ckbDisponibilidadeMudarCidade_CheckedChanged(args) {
    employer.util.findControl('lblCidadeDisponivel').css('display', args.checked ? 'block' : 'none');
    employer.util.findControl('txtCidadeDisponivel').css('display', args.checked ? 'block' : 'none');
    employer.util.findControl('rfvCidadeDisponivel').css('display', args.checked ? 'block' : 'none');

    employer.controles.enableValidatorVal('rfvCidadeDisponivel', args.checked, true);
}

$(document).ready(function () {
    autocomplete.cidade("txtCidadeDisponivel");
});
