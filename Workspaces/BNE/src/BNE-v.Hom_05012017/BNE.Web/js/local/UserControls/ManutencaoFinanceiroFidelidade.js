function txtFidelidadeDetalhesValorParcela_ValorAlterado(sender, args) {
    var vlrParcela = employer.controles.recuperarValorControle('txtFidelidadeDetalhesValorParcela').replace('.','').replace(',','.');
    var qtdParcela = employer.controles.recuperarValorControle('txtFidelidadeDetalhesParcelas');
    
    if (qtdParcela != '')
        employer.controles.setValorControle('txtFidelidadeDetalhesValor', (vlrParcela * +qtdParcela).toFixed(2).replace('.',','));
}

function txtFidelidadeDetalhesValor_ValorAlterado(sender, args) {
    var vlrPlano = employer.controles.recuperarValorControle('txtFidelidadeDetalhesValor').replace('.','').replace(',','.');
    var qtdParcela = employer.controles.recuperarValorControle('txtFidelidadeDetalhesParcelas');
    
    if (qtdParcela != '')
        employer.controles.setValorControle('txtFidelidadeDetalhesValorParcela', (vlrPlano / +qtdParcela).toFixed(2).replace('.',','));
}

function txtFidelidadeDetalhesParcelas_ValorAlterado(sender, args) {
    var vlrPlano = employer.controles.recuperarValorControle('txtFidelidadeDetalhesValor').replace('.','').replace(',','.');
    var vlrParcela = employer.controles.recuperarValorControle('txtFidelidadeDetalhesValorParcela').replace('.','').replace(',','.');
    var qtdParcela = employer.controles.recuperarValorControle('txtFidelidadeDetalhesParcelas');

    if (vlrPlano != '') {
        employer.controles.setValorControle('txtFidelidadeDetalhesValorParcela', (vlrPlano / +qtdParcela).toFixed(2).replace('.',','));
    }
    else if (vlrParcela != '') {
        employer.controles.setValorControle('txtFidelidadeDetalhesValor', (vlrParcela * +qtdParcela).toFixed(2).replace('.',','));
    }
}