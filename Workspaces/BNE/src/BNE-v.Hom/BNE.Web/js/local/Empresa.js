$(function () {
    $('a[href*="#"]:not([href="#"])').click(function () {
        if (location.pathname.replace(/^\//, '') === this.pathname.replace(/^\//, '') && location.hostname === this.hostname) {
            var target = $(this.hash);
            target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
            if (target.length) {
                $('html, body').animate({
                    scrollTop: target.offset().top
                }, 500);
                return false;
            }
        }
        return false;
    });

    autocomplete.cidade("txtCidadeVaga");
    autocomplete.funcao("txtFuncaoVaga");
});
$('#Banner-button').click(function () {
    $("#homeVideo").addClass("active");
});
$(document).ready(function () {
    $('#Banner-button').on('click', function (ev) {
        $("#BNEEmpresa")[0].src += "&autoplay=1";
        ev.preventDefault();
    });
});
$('#playVideo').click(function () {
    $("#homeVideo").addClass("active");
});
$(document).ready(function () {
    $('#playVideo').on('click', function (ev) {
        $("#BNEEmpresa")[0].src += "&autoplay=1";
        ev.preventDefault();
    });
});
$('#playVideofooter').click(function () {
    $("#homeVideo").addClass("active");
});
$(document).ready(function () {
    $('#playVideofooter').on('click', function (ev) {
        $("#BNEEmpresa")[0].src += "&autoplay=1";
        ev.preventDefault();
    });

    $('#btnPesquisarCurriculo').click(function () {
        document.location.href = '/pesquisa-de-curriculo';
    });
});
function cvCidadeVaga_Validate(sender, args) {
    var res = Empresa.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function cvFuncaoVaga_Validate(sender, args) {
    var res = Empresa.ValidarFuncao(args.Value);
    args.IsValid = (res.error == null && res.value);
}

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

    if (!args.Value.match("^[A-Z,a-z,ÁáÂâÃãÄäÉéÊêËëÍíÏïÓóÔôÕõÖöÚúÛûÜüÇç']{1,}( ){1,}[A-Z,a-z,ÁáÂâÃãÄäÉéÊêËëÍíÏïÓóÔôÕõÖöÚúÛûÜüÇç']{2,}(( ){1,}[A-Z,a-z,ÁáÂâÃãÄäÉéÊêËëÍíÏïÓóÔôÕõÖöÚúÛûÜüÇç']{1,})*$"))
        isValid = false;

    args.IsValid = isValid;
}
