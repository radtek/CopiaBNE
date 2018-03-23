var BNEFacebook = {};

BNEFacebook.EfetuarLogin = function () {
    FB.login(
        BNEFacebook.LoginFacebookCallback,
        { scope: 'email,user_about_me,user_birthday,user_location,user_work_history,user_education_history,user_relationships,user_likes,read_friendlists' }
    );
};

BNEFacebook.AssignEvent = function (callback, event) {
    FB.getLoginStatus(callback);
    FB.Event.subscribe(event, callback);
};

BNEFacebook.LoginFacebookCallback = function (response) {
    if (response.authResponse) {
        var objectMe;
        var objectFriends;
        var objectPicture;

        //Recuperando os dados do usuário
        FB.api('/me?locale=pt_BR', function (responseMe) {
            objectMe = responseMe;

            //Recuperando a lista de amigos
            FB.api('/me/friends', function (responseFriends) {
                objectFriends = responseFriends;

                //Recuperando a foto
                FB.api('/me/picture?width=180&height=180', function (responsePicture) {
                    objectPicture = responsePicture;

                    //Depois de pegar todas as informações necessárias, valida se o usuário existe.
                    if (!BNEFacebook.UsuarioExiste(objectMe.id)) {
                        BNEFacebook.ArmazenarDadosFacebook(objectMe, objectPicture);

                        if (typeof (PreencherCamposFacebook) === 'function') {
                            PreencherCamposFacebook(objectMe);
                        }
                        /*if (BNEFacebook.ArmazenarDadosFacebook(objectMe, objectPicture)) {
                            BNEFacebook.UsuarioExiste(objectMe.id);
                        }*/
                    }
                });
            });
        });
    }
};

BNEFacebook.UsuarioExiste = function (id) {
    var existe = false;

    $.ajax({
        type: 'post',
        url: '/ajax/signinf',
        data: {
            l: id
        },
        dataType: "json",
        success: function (d, s, x) {
            if (d) {
                existe = true;
                if (d.t) {
                    modal.fecharModal('div2');
                    cacheToken(d.t);
                    clean();
                    loadMoreEmpresas(0);
                }
            }
        },
        error: function (x, s, e) { mostrarErro(x.statusMessage); }
    });

    return existe;
};

BNEFacebook.ArmazenarDadosFacebook = function (objectMe, objectPicture) {
    var ok;

    $.post('/ajax/salvarfb',
        { m: JSON.stringify(objectMe), p: JSON.stringify(objectPicture) },
        function (d, s, x) {
            ok = d;
        },
        'json');

    return ok;
};



