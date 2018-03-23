function ValidarNome(source, args) {
    var w, z, y, x;
    var isValid = true;
    for (x = 0; x < args.Value.length; x++) {
        z = args.Value.substring(x, x + 1);
        if ((x >= 2 && z == y && z == w)) {
            isValid = false;
        }
        else {
            y = w;
            w = z;
            z = '-';
        }
    }

    if (!args.Value.match("^[A-Z,a-z,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç']{1,}( ){1,}[A-Z,a-z,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç']{2,}(( ){1,}[A-Z,a-z,ÁáÂâÃãÉéÊêÍíÓóÔôÕõÚúÛûÇç']{1,})*$"))
        isValid = false;

    args.IsValid = isValid;
}

function cvCidade_Validate(sender, args) {
    var res = ContatoSite.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}