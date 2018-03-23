$(document).ready(function () {
    var visibilidadeListaDemaisPessoasChat = function (e) {
        var elem = $(e.target).closest('#box_excesso_pessoas'),
           box = $(e.target).closest('#container_excesso_pessoas');

        if (elem.length) {
            e.preventDefault();
            $('#container_excesso_pessoas').toggle();
        } else if (!box.length) {
            $('#container_excesso_pessoas').hide();
        }
    };

    $(document).bind('click', function (e) {
        visibilidadeListaDemaisPessoasChat(e);
    });
});