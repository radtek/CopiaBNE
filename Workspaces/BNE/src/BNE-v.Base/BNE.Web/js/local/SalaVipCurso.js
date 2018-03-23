function Visualizar(tipo) {
    var conteudo = employer.util.findControl('conteudo_curso');
    var descricao = employer.util.findControl('descricao_curso');
    var divVerDescricao = employer.util.findControl('divVerDescricao');
    var divVerConteudo = employer.util.findControl('divVerConteudo');

    divVerDescricao.removeClass('descricao_curso_azul');
    divVerDescricao.removeClass('descricao_curso_cinza');
    divVerConteudo.removeClass('conteudo_curso_azul');
    divVerConteudo.removeClass('conteudo_curso_cinza');

    switch (tipo) {
        case 'D':
            descricao.css('display', 'block');
            conteudo.css('display', 'none');
            divVerDescricao.addClass('descricao_curso_azul');
            divVerConteudo.addClass('conteudo_curso_cinza');
            break;
        case 'C':
            descricao.css('display', 'none');
            conteudo.css('display', 'block');
            divVerDescricao.addClass('descricao_curso_cinza');
            divVerConteudo.addClass('conteudo_curso_azul');
            break;
    }
}