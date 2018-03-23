var modal = {
    abrirModal: function (control) {

        var id = control;
        //var id = control + $.now();
        //$('#' + control).attr('id', id);

        /*if (latestOpenedModal !== '' && latestOpenedModal !== id) {
            if ($('#' + latestOpenedModal).length > 0) //Verifica se o elemento existe
            {
                $('#' + latestOpenedModal).bPopup().close();
                $('#' + latestOpenedModal).remove();
            }
        }*/

        latestOpenedModal = id;

        $('#' + id).bPopup({
            modalClose: false,
            /*modalColor: '#cde1e8',*/
            speed: 300,
            zIndex: 2,
            opacity: 0.4,
            easing: 'slideIn',
            positionStyle: 'fixed',
            onClose: function () {
                //alert($(this)[0].id);
                //$(this).remove();
            }
        });
    },
    fecharModal: function (control) {
        $('#' + control).bPopup().close();
    },
    fecharModais: function () {
        $('.modal').each(function () { $(this).bPopup().close(); });
    },
    ultimaModalAberta: function () {
        return latestOpenedModal;
    }
};

var latestOpenedModal = '';