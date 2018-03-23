var BNEFacebook = {};

BNEFacebook.EfetuarLogin = function () {
    FB.login(
        BNEFacebook.LoginFacebookCallback,
        { scope: 'email,user_about_me,user_birthday,user_location,user_work_history,user_education_history,user_relationships,user_likes,read_friendlists' }
    );
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
                    var userExist = Login.ValidarFacebook(objectMe.id);
                    if (userExist.value) {
                        var botao = employer.util.findControl('btnEntrarFacebook');
                        __doPostBack(botao[0].id, 'facebook');
                        //window.location.reload();
                    } else {
                        var jsonMe = JSON.stringify(objectMe);
                        var jsonFriends = JSON.stringify(objectFriends);
                        var jsonMePicture = JSON.stringify(objectPicture);
                        Login.ArmazenarDadosFacebook(jsonMe, jsonFriends, jsonMePicture);
                        window.location = "/CadastroCurriculoMini.aspx";
                    }
                });
            });
        });
    }
};