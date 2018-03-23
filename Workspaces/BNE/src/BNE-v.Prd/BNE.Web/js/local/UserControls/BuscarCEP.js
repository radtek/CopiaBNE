function BuscaCEP_lsUF_ValorAlterado(args)
{
    var campo = $get(args.NomeCampoValor);
    $find(campo.parentNode.getAttribute('MeuMunicipio')).set_contextKey(campo.value);
}

$(document).ready(function() {
    autocomplete.cidade("txtMunicipio");
});