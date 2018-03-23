var textoWM = 'Aqui você pode descrever a mensagem de retorno para os candidatos reprovados em seu processo seletivo.<br>O envio ocorre após a exclusão do candidato na listagem de inscritos na vaga.';

function reAgradecimentoCandidatura_ClientLoad(editor, args) {
    var texto = '<div class=\'texto_watermark\'>' + textoWM + '</div>';

    if (editor.get_html().length === 0 || editor.get_html() === '<p>&nbsp;</p>' || editor.get_html() === '<br>') {
        employer.util.findControl('editor').css('display', 'block');
    }
    $telerik.addExternalHandler(editor.get_document(), "mouseout", function (e) {
        if (editor.get_html().length === 0 || editor.get_html() === '<p>&nbsp;</p>' || editor.get_html() === '<br>') {
            employer.util.findControl('editor').html(texto);
            employer.util.findControl('editor').css('display', 'block');

        }
    });
    $telerik.addExternalHandler(editor.get_document(), "mouseover", function (e) {
        employer.util.findControl('editor').css('display', 'none');
    });
    employer.util.findControl('editor').mouseover(function () {
        $(this).css('display', 'none');
    });
}