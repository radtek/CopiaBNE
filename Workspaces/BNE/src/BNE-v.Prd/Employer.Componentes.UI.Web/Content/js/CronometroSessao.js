
Type.registerNamespace("Employer.Componentes.UI.Web");

var Employer_Componentes_UI_Web_CronometroSessao_Timer = 0;
var Employer_Componentes_UI_Web_CronometroSessao_Client = 0;
var Employer_Componentes_UI_Web_CronometroSessao_Client_Timer = 0;

Employer.Componentes.UI.Web.CronometroSessao = function (element) {
    Employer.Componentes.UI.Web.CronometroSessao.initializeBase(this, [element]);

    this._id = element.id;
    this._campoId = null;
    this._tempo_Aviso = null;
    this._tempo_Sessao = null;
    this._mostrouAviso = false;
    this._IdModal = null;
    this._IdBtnFechar = null;
    this._UrlsIgnorar = null;
    this._FechouSessao = false;
    this._TextoModal = null;
    this._IdLabelModal = null;

    this.get_IdLabelModal = function () {
        return this._IdLabelModal;
    },
    this.set_IdLabelModal = function (value) {
        this._IdLabelModal = value;
    },

    this.get_TextoModal = function () {
        return this._TextoModal;
    },
    this.set_TextoModal = function (value) {
        this._TextoModal = value;
    },

    this.get_Tempo_Aviso = function () {
        return this._tempo_Aviso;
    },
    this.set_Tempo_Aviso = function (value) {
        this._tempo_Aviso = value;
    },

    this.get_UrlsIgnorar = function () {
        return this._UrlsIgnorar;
    },
    this.set_UrlsIgnorar = function (value) {
        this._UrlsIgnorar = eval(value);
    },

    this.get_IdBtnFechar = function () {
        return this._IdBtnFechar;
    },
    this.set_IdBtnFechar = function (value) {
        this._IdBtnFechar = value;
    },

    this.get_IdModal = function () {
        return this._IdModal;
    },
    this.set_IdModal = function (value) {
        this._IdModal = value;
    },

    this.get_Tempo_Sessao = function () {
        return this._tempo_Sessao;
    },
    this.set_Tempo_Sessao = function (value) {
        this._tempo_Sessao = value;
    },

    this.get_CampoId = function () {
        return this._campoId;
    },
    this.set_CampoId = function (value) {
        this._campoId = value;
    },

    this.set_Campo = function (v) {
        $get(this.get_CampoId()).value = v;
    },

     this.get_Modal = function () {
         return $find(this.get_IdModal());
     },

    this.On_WebRequestCompleted = function (executor, eventArgs) {

        var achou = false;
        for (var i = 0; i < this.get_UrlsIgnorar().length; i++) {
            var url = this.get_UrlsIgnorar()[i];
            if (executor._webRequest._url.indexOf(url) > -1) {
                achou = true;
                break;
            }
        }

        if (!achou)
            Employer_Componentes_UI_Web_CronometroSessao_Client = 0;
    },

    this.FecharSessao = function () {
        try {
            $get(this.get_IdBtnFechar()).click();
        }
        catch (e) {
            var comp = this;
            window.setTimeout(function () {
                comp.FecharSessao();
            }, 500);
        }
    },

    this.timerIncrement = function () {
        Employer_Componentes_UI_Web_CronometroSessao_Client = Employer_Componentes_UI_Web_CronometroSessao_Client + 1;

        var idleTime = Math.floor(Employer_Componentes_UI_Web_CronometroSessao_Client / 60.0);

        var restaMin = this.get_Tempo_Sessao() - idleTime;
        var restaSec = ((idleTime) * 60) - Employer_Componentes_UI_Web_CronometroSessao_Client;

        if (restaSec < 0) {
            restaMin = restaMin - 1;
            restaSec = 60 + restaSec;
        }

        var tempo = null;
        if (restaSec == 60)
            tempo = AjaxClientControlBase.padLeft((restaMin + 1).toString(), "0", 2) + ":00";
        else
            tempo = AjaxClientControlBase.padLeft(restaMin.toString(), "0", 2) + ":" +
                AjaxClientControlBase.padLeft(restaSec.toString(), "0", 2);

        $get(this.get_CampoId()).value = tempo;

        tempo = this.get_TextoModal().replace("{0}", tempo);

        $("#" + this.get_IdLabelModal()).html(tempo);

        if (restaMin == this.get_Tempo_Aviso() && restaSec == 0 && this._mostrouAviso == false) {
            this._mostrouAviso = true;
            this.get_Modal().show();
        }

        if (restaMin < 0 && this._FechouSessao == false) {
            this._FechouSessao = true;

            window.clearTimeout(Employer_Componentes_UI_Web_CronometroSessao_Client_Timer);
            Employer_Componentes_UI_Web_CronometroSessao_Client_Timer = 0;

            var comp = this;
            window.setTimeout(function () {
                comp.FecharSessao();
            }, 500);
        }
    },

    this.initialize = function () {
        Employer.Componentes.UI.Web.CronometroSessao.callBaseMethod(this, 'initialize');

        var comp = this;

        Sys.Net.WebRequestManager.add_completedRequest(function (executor, eventArgs) {
            comp.On_WebRequestCompleted(executor, eventArgs);
        });



        $(function () {
            //Increment the idle time counter every minute.
            Employer_Componentes_UI_Web_CronometroSessao_Client_Timer = setInterval(function () {
                comp.timerIncrement();
            }, 1000); // 1 minute



            //            //Zero the idle timer on mouse movement.
            //            $(this).mousemove(function (e) {
            //                Employer_Componentes_UI_Web_CronometroSessao_Client = 0;
            //            });
            //            $(this).keypress(function (e) {
            //                Employer_Componentes_UI_Web_CronometroSessao_Client = 0;
            //            });
        });

    },

    this.dispose = function () {

        //$clearHandlers(this.get_TextBox());

        //        window.clearTimeout(Employer_Componentes_UI_Web_CronometroSessao_Timer);
        //        Employer_Componentes_UI_Web_CronometroSessao_Timer = 0;

        Sys.Net.WebRequestManager.remove_completedRequest(this.On_WebRequestCompleted);

        window.clearTimeout(Employer_Componentes_UI_Web_CronometroSessao_Client_Timer);
        Employer_Componentes_UI_Web_CronometroSessao_Client_Timer = 0;

        Employer.Componentes.UI.Web.CronometroSessao.callBaseMethod(this, 'dispose');
    }


}

Employer.Componentes.UI.Web.CronometroSessao.registerClass('Employer.Componentes.UI.Web.CronometroSessao', Sys.UI.Behavior);