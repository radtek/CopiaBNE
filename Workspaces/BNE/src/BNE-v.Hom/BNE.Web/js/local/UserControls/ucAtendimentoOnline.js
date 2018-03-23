function AbrirAtendimentoOnline() {
    var urlAtendimento = "";

    $.ajax({
        type: "POST",
        url: "/WebServices/wsUtils.asmx/MontarURLAtendimento",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            urlAtendimento = data.d;
        }
    });

    alochat = window.open(urlAtendimento, 'alo49939200', 'scrollbars=no ,width=522, height=533 , top=50 , left=50');
    return false;
}