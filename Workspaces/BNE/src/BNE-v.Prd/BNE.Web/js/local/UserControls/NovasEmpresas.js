function txtDt_ValorAlterado(sender)
{
    var txtDataInicial = employer.controles.recuperarValorControle('txtDataInicial').replace('-', '');
    var txtDataFinal = employer.controles.recuperarValorControle('txtDataFinal').replace('-', '');
    var btGerar = document.getElementById("cphConteudo_ucNvsEmp_btnGerar");
    if (txtDataInicial.length > 0 && txtDataFinal.length > 0) {

        var dataIni = txtDataInicial.split("/");
        var dataFim = txtDataFinal.split("/");

        var dtIn = new Date();
        dtIn.setDate(dataIni[0]);
        dtIn.setMonth(parseInt(dataIni[1].substring(1)));
        dtIn.setYear(dataIni[2]);
        dtIn.setHours(00);
        dtIn.setMinutes(00);
        dtIn.setSeconds(00);
        dtIn.setMilliseconds(000);

        var dtFn = new Date();
        dtFn.setDate(dataFim[0]);
        dtFn.setMonth(parseInt(dataFim[1].substring(1)));
        dtFn.setYear(dataFim[2]);
        dtFn.setHours(00);
        dtFn.setMinutes(00);
        dtFn.setSeconds(00);
        dtFn.setMilliseconds(000);

        if (dtIn > dtFn) {

            alert('A Data Inicial nao pode ser maior que a Data Final!');
            return;
        }
    }
}