Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(
    function pageLoad() {

        var element = $("[id='ucVisualizacaoCurriculo_ucModalQueroContratarEstagiario_txtValorBolsa_txtValor']");
        if (element.length != 1)
            return;

        $(element[0]).keydown(function (e) {
            if (jQuery.isNumeric(e.char)) {
                return;
            }

            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 190]) !== -1 ||
                // Allow: Ctrl+A
          (e.keyCode == 65 && e.ctrlKey === true) ||
                // Allow: home, end, left, right
          (e.keyCode >= 35 && e.keyCode <= 39)) {
                // let it happen, don't do anything
                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                if (e.char == ',')
                    return;
                e.preventDefault();
            }
        });
    });

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