$(document).ready(function () {
    $("#cphConteudo_ucDeficiencia_cbAll").change(function (event) {
        var checkbox = $('#cphConteudo_ucDeficiencia_cblFisica:checked').event.target;
        if (checkbox.checked) {

            
            $(':checkbox').each(function () {
                this.checked = true;
            });
        } else {
            $(':checkbox').each(function () {
                this.checked = false;
            });
        }
    });
    function checkAll() {
        console.log("lsh");
        check($('#cphConteudo_ucDeficiencia_cblFisica').find('input'));
        check($('#cphConteudo_ucDeficiencia_cblAuditiva').find('input'));
        check($('#cphConteudo_ucDeficiencia_cblVisual').find('input'));
        check($('#cphConteudo_ucDeficiencia_cblMental').find('input'));
    }

    function check(cbl) {
        for (var i = 0; i < cbl.length; i++) {
            if ($("#cphConteudo_ucDeficiencia_cbAll")[0].checked)
                cbl[i].checked = true;
            else
                cbl[i].checked = false;
        }
    }
});