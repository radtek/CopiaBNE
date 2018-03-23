function cvCidade_Validate(sender, args) {
    if (args.Value == "") {
        args.IsValid = false;
        return;
    }
    var res = EditarAgradecimento.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function cvCidade_Validate2(sender, args) {
    if (args.Value == "") {
        args.IsValid = false;
        return;
    }
    var res = NovoAgradecimento.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function selectItem(item) {
    item.clasName = 'selecionado'; 
    return true;
} 
