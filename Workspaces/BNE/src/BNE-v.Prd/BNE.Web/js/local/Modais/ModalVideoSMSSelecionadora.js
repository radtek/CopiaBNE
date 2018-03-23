function AbrirModalVideoSMSSelecionadora(autoplay) {
    var iframe = '<iframe id="video_player" width="560" height="315" src="https://www.youtube.com/embed/bDc4uaQQHHY" frameborder="0" allowfullscreen></iframe>';
    if (autoplay) {
        var iframe = '<iframe id="video_player" width="560" height="315" src="https://www.youtube.com/embed/bDc4uaQQHHY?autoplay=1" frameborder="0" allowfullscreen></iframe>';
        trackEvent("SalaSelecionadora", "play", "video sms da selecionadora");
    }
    if ($('#modalVideoSMSSelecionadora iframe').length <= 0) {
        $('#modalVideoSMSSelecionadora .modal-body').append(iframe);
    }

    $('#modalVideoSMSSelecionadora').modal('show');
}

$(document).ready(function () {
    $('#modalVideoSMSSelecionadora').on('hide', function () {
        $('#modalVideoSMSSelecionadora iframe').remove();
    });
});