// Declare a namespace.
Type.registerNamespace("Employer.Componentes.UI.Web");

Employer.Componentes.UI.Web.DataTextBox = function (element) {

    Employer.Componentes.UI.Web.DataTextBox.initializeBase(this, [element]);

    this._id = element.id;

    this._DataMinima = null;
    this._CampoDataMinima = null;
    this._DataMaxima = null;
    this._CampoDataMaxima = null;
    // Summary
    this._MensagemErroObrigatorioSummary = null;
    this._MensagemErroFormatoSummary = null;
    this._MensagemErroValorMinimoSummary = null;
    this._MensagemErroValorMaximoSummary = null;


    //Controles internos  
    this._TextoAntesKeyPress = null;
    this._TextoAntesFocus = null;

    //Globalização
    this._DateSeparator = null;
    this._Mascara = null;
    this._Cultura = null;
    this._MostrarCalendario = null;
    this._ImagemBotaoUrl = null;
    this._Feriados = [];
    this._CssClassFeriado = null;
    //    this._CssClassFeriadosNacionais = null;
    //    this._CssClassFeriadosEstaduais = null;
    //    this._CssClassFeriadosMunicipais = null;
    //    this._CssClassFeriadosOutros = null;


    this._SelecionarFeriados = null;
    this._UrlUpdateFeriados = null;

    // Handlers for the events
    this._onKeyPress = null;
    this._onBlur = null;
    this._onFocus = null;
    this._onChange = null;

    var espGrid = RegExp("(_[0-9]+)$");
    var m = espGrid.exec(this._id);


    this.get_TextBox = function () {
        var espGrid = RegExp("(_[0-9]+)$");
        var m = espGrid.exec(this._id);

        if (m != null)
            return $get(this._id + "_txtValor" + m[0]);
        else
            return $get(this._id + "_txtValor");
    },

    this.configurarDias = function (date, instance) {
        var ano = date.getFullYear();
        if (instance._Feriados != null && instance._Feriados[ano] != null) {
            var tipoCSS = '';
            var concat = '';
            var descricaoFeriado = '';
            for (i = 0; i < instance._Feriados[ano].length; i++) {
                if (date.getDate() == instance._Feriados[ano][i][0]
                 && date.getMonth() == (instance._Feriados[ano][i][1] - 1)
                 && ano == instance._Feriados[ano][i][2]) {
                    descricaoFeriado += concat + instance._Feriados[ano][i][4] + ' - ' + instance._Feriados[ano][i][5];
                    concat = '\n';
                    if (instance._Feriados[ano][i][3] == 1 ||
                    instance._Feriados[ano][i][3] == 2 ||
                    instance._Feriados[ano][i][3] == 3 ||
                    instance._Feriados[ano][i][3] == 4) {
                        tipoCSS = instance._CssClassFeriado;
                    }
                }
            }
            if (instance._SelecionarFeriados)
                return [true, tipoCSS, descricaoFeriado];
            else
                return [false, tipoCSS, descricaoFeriado];
        }
        return [true];
    },


     this.initialize = function () {
         Employer.Componentes.UI.Web.DataTextBox.callBaseMethod(this, 'initialize');

         // Wireup the event handlers
         var element = this.get_TextBox();
         if (element) {

             this._onKeyPress = Function.createDelegate(this, this.KeyPress);
             $addHandler(element, 'keypress', this._onKeyPress);

             this._onBlur = Function.createDelegate(this, this.Blur);
             $addHandler(element, 'blur', this._onBlur);

             this._onFocus = Function.createDelegate(this, this.RemoverMascara);
             $addHandler(element, 'focus', this._onFocus);
         }
         //dtAtual;
         var anoTemp;
         var mesTemp;
         if (isNaN(this.GetDataValue())) {
             this.BuscaDataAtual();
             anoTemp = dtAtual.getFullYear();
             mesTemp = dtAtual.getMonth();
         } else {
             anoTemp = this.GetDataValue().getFullYear();
             mesTemp = this.GetDataValue().getMonth();
         }

         this.AtualizarFeriados(anoTemp);
         if (mesTemp <= 1) {
             this.AtualizarFeriados(anoTemp - 1);
         }
         if (mesTemp >= 10) {
             this.AtualizarFeriados(anoTemp + 1);
         }
         this.assign(this.configurarDias);
     },

     this.NextFocus = function (time) {
         if (typeof (Employer_Componentes_UI_Web_AutoTabIndex_NextFocus) != "undefined") {
             if (time)
                 Employer_Componentes_UI_Web_AutoTabIndex_NextFocusTimeout(time);
             else
                 Employer_Componentes_UI_Web_AutoTabIndex_NextFocus();
         }
         else {
             var inst = this;
             window.setTimeout(function () { inst.Blur(); }, time);
         }

     },

    this.assign = function (callback) {

        // Inicia calendario
        if (this._MostrarCalendario == true) {

            if ($.datepicker == undefined || $.datepicker == null) {
                alert("É necessário ter o js do jQuery UI com a Widget Datepicker habilitada para que o componente DataTextBox funcione corretamente. Você pode obtê-lo em: http://jqueryui.com/download");
                return;
            }

            var idjquery = null;
            if (m != null)
                idjquery = this._id + "_txtValor" + m[0];
            else
                idjquery = this._id + "_txtValor";

            var instance = this;

            $("input[id$='" + idjquery + "']").datepicker({
                showOn: "button",
                buttomImageOnly: true,
                dateFormat: "dd/mm/yy",
                dayNamesMin: ["D", "S", "T", "Q", "Q", "S", "S"],
                monthNames: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"],
                prevText: "Anterior",
                nextText: "Próximo",
                buttonImage: this._ImagemBotaoUrl,
                buttonText: "",
                onSelect: function (dateText, inst) {
                    //$(inst).blur();
                    instance.Blur();
                    //instance.NextFocus();
                },
                onChangeMonthYear: function (year, month, inst) {
                    if (instance._UrlUpdateFeriados != null || instance._UrlUpdateFeriados != "") {
                        if (month <= 2 && instance._Feriados[year - 1] == null) {
                            instance.AtualizarFeriados(year - 1);
                        } else if (month >= 11 && instance._Feriados[year + 1] == null) {
                            instance.AtualizarFeriados(year + 1);
                        }
                    }
                },
                beforeShowDay: function (date) { return callback(date, instance); }
            });

            var txt = $(this.get_TextBox());
            var el = $(this._element);

            if (el.is(":disabled") || txt.is(":disabled")) {
                $("input[id$='" + idjquery + "']").datepicker('disable');
            } else {
                $("input[id$='" + idjquery + "']").datepicker('enable');
            }
        }
    },

    this.dispose = function () {
        $clearHandlers(this.get_TextBox());

        Employer.Componentes.UI.Web.DataTextBox.callBaseMethod(this, 'dispose');
    },

    this.get_Feriados = function () {
        return this._Feriados;
    },
    this.set_Feriados = function (value) {
        this._Feriados = value;
    },

    this.get_CssClassFeriado = function () {
        return this._CssClassFeriado;
    },
    this.set_CssClassFeriado = function (value) {
        this._CssClassFeriado = value;
    },

    this.get_SelecionarFeriados = function () {
        return this._SelecionarFeriados;
    },
    this.set_SelecionarFeriados = function (value) {
        this._SelecionarFeriados = value;
    },
    this.get_UrlUpdateFeriados = function () {
        return this._UrlUpdateFeriados;
    },
    this.set_UrlUpdateFeriados = function (value) {
        this._UrlUpdateFeriados = value;
    },
    this.get_ImagemBotaoUrl = function () {
        return this._ImagemBotaoUrl;
    },
    this.set_ImagemBotaoUrl = function (value) {
        this._ImagemBotaoUrl = value;
    },
    this.get_MostrarCalendario = function () {
        return this._MostrarCalendario;
    },
    this.set_MostrarCalendario = function (value) {
        this._MostrarCalendario = value;
    },

    this.get_TextoAntesFocus = function () {
        return this._TextoAntesFocus;
    },
    this.get_CampoDataMinima = function () {
        return this._CampoDataMinimo;
    },
    this.get_CampoDataMaxima = function () {
        return this._CampoDataMaximo;
    },

    this.get_DateSeparator = function () {
        return this._DateSeparator;
    },
    this.get_Mascara = function () {
        return this._Mascara;
    },
    //Summary
    this.get_MensagemErroObrigatorioSummary = function () {
        if (AjaxClientControlBase.IsNullOrEmpty(this._MensagemErroObrigatorioSummary) == false)
            return this._MensagemErroObrigatorioSummary;

        return this.get_MensagemErroObrigatorio();
    },
    this.set_MensagemErroObrigatorioSummary = function (value) {
        this._MensagemErroObrigatorioSummary = value;
    },
    this.get_MensagemErroFormatoSummary = function () {
        if (AjaxClientControlBase.IsNullOrEmpty(this._MensagemErroFormatoSummary) == false)
            return this._MensagemErroFormatoSummary;

        return this.get_MensagemErroFormato();
    },
    this.set_MensagemErroFormatoSummary = function (value) {
        this._MensagemErroFormatoSummary = value;
    },
    this.get_MensagemErroValorMinimoSummary = function () {
        if (AjaxClientControlBase.IsNullOrEmpty(this._MensagemErroValorMinimoSummary) == false)
            return this._MensagemErroValorMinimoSummary;

        return this.get_MensagemErroValorMinimo();
    },
    this.set_MensagemErroValorMinimoSummary = function (value) {
        this._MensagemErroValorMinimoSummary = value;
    },
    this.get_MensagemErroValorMaximoSummary = function () {
        if (AjaxClientControlBase.IsNullOrEmpty(this._MensagemErroValorMaximoSummary) == false)
            return this._MensagemErroValorMaximoSummary;

        return this.get_MensagemErroValorMaximo();
    },
    this.set_MensagemErroValorMaximoSummary = function (value) {
        this._MensagemErroValorMaximoSummary = value;
    },
    this._IndeceOrdem = null;
    this.get_OrdemMascara = function () {
        if (this._IndeceOrdem == null) {
            var m = this.get_Mascara();
            var Odem = [];

            Odem[0] = m.indexOf("d");
            Odem[1] = m.indexOf("MM");
            Odem[2] = m.indexOf("y");

            //Odem = Odem.sort();

            this._IndeceOrdem = {
                Dia: (Odem[0] > Odem[1] && Odem[0] > Odem[2]) ? 2 : (Odem[0] > Odem[1] || Odem[0] > Odem[2]) ? 1 : 0,
                Mes: (Odem[1] > Odem[0] && Odem[1] > Odem[2]) ? 2 : (Odem[1] > Odem[0] || Odem[1] > Odem[2]) ? 1 : 0,
                Ano: (Odem[2] > Odem[0] && Odem[2] > Odem[1]) ? 2 : (Odem[2] > Odem[0] || Odem[2] > Odem[1]) ? 1 : 0
            };
        }
        return this._IndeceOrdem;
    },
    this.get_RegexData = function () {
        var sRx = this.get_Mascara().replace("dd", "d([0-9]+)");
        sRx = sRx.replace("MM", "M([0-9]+)");
        sRx = sRx.replace("yyyy", "y([0-9]+)");
        return sRx;
    },
    this.get_Cultura = function () {
        return this._Cultura;
    },
     this.get_Validador = function () {
         var espGrid = RegExp("(_[0-9]+)$");
         var m = espGrid.exec(this._id);

         if (m != null) {
             return $get(this._id + "_cvValor" + m[0]);
         }
         else {
             return $get(this._id + "_cvValor");
         }
     },

    this.set_CampoDataMinima = function (value) {
        this._CampoDataMinimo = value;
    },
    this.set_CampoDataMaxima = function (value) {
        this._CampoDataMaximo = value;
    },

    this.set_DataMaxima = function (value) {
        this._DataMaxima = value;
    },
    this.set_DataMinima = function (value) {
        this._DataMinima = value;
    },
    this.set_DateSeparator = function (value) {
        this._DateSeparator = value;
    },
    this.set_Mascara = function (value) {
        this._Mascara = value
    },
    this.set_Cultura = function (value) {
        this._Cultura = value;
    }
}

//Simula herança
Employer.Componentes.UI.Web.DataTextBox.prototype = new Employer.Componentes.UI.Web.ControlBaseValidator;

Employer.Componentes.UI.Web.DataTextBox.prototype.GetData = function (sDate) {
    var rxData = new RegExp(this.get_RegexData());

    //Converte p/ pt-br
    sDate = sDate.replace(rxData, "$1/$2/$3");
    var sPlit = sDate.split("/");

    return new Date(sPlit[this.get_OrdemMascara().Ano], sPlit[this.get_OrdemMascara().Mes] - 1, sPlit[this.get_OrdemMascara().Dia], 12, 0, 0, 0);
}

Employer.Componentes.UI.Web.DataTextBox.prototype.DataFormatar = function (dtVal) {
    var m = this.get_Mascara();

    m = m.replace("dd", AjaxClientControlBase.padLeft(dtVal.getDate(), '0', 2));
    m = m.replace("MM", AjaxClientControlBase.padLeft(dtVal.getMonth() + 1, '0', 2));
    m = m.replace("yyyy", dtVal.getFullYear());

    return m;
}

var dtAtual = null;
var dtAtualFormatada = null;
Employer.Componentes.UI.Web.DataTextBox.prototype.BuscaDataAtual = function () {
    if (dtAtual == null) {
        dtAtualFormatada = DataTextBox.DataAtual(this.get_Cultura()).value;
        dtAtual = this.GetData(dtAtualFormatada);
    }
}
Employer.Componentes.UI.Web.DataTextBox.prototype.GetDataAtual = function () {
    this.BuscaDataAtual();
    return dtAtual;
}
Employer.Componentes.UI.Web.DataTextBox.prototype.GetDataAtualFormatada = function () {
    this.BuscaDataAtual();
    return dtAtualFormatada;
}
Employer.Componentes.UI.Web.DataTextBox.prototype.GetDataValue = function () {
    return this.GetData(this.get_TextBox().value);
}
Employer.Componentes.UI.Web.DataTextBox.prototype.FormatarData = function (sData) {
    sData = AjaxClientControlBase.Trim(sData);
    if (sData == "")
        return sData;

    var rxBr = new RegExp("\\" + this.get_DateSeparator() + "+", "g");
    var nsData = sData.replace(rxBr, "");
    var nMask = this.get_Mascara().replace(rxBr, "");

    nsData = AjaxClientControlBase.padRight(nsData, " ", nMask.length);

    var dia = null;
    var mes = null;
    var ano = null;
    if (sData.indexOf(this.get_DateSeparator()) > -1) {
        var sPlit = sData.split(this.get_DateSeparator());

        dia = sPlit[this.get_OrdemMascara().Dia];
        mes = sPlit[this.get_OrdemMascara().Mes];
        ano = sPlit[this.get_OrdemMascara().Ano];
    }

    if (dia == null)
        dia = nsData.substring(nMask.indexOf("d"), nMask.indexOf("d") + 2);
    if (mes == null)
        mes = nsData.substring(nMask.indexOf("M"), nMask.indexOf("M") + 2);
    if (ano == null)
        ano = nsData.substring(nMask.indexOf("y"), nMask.indexOf("y") + 4);

    var rxEsp = new RegExp(" ", "g");
    dia = dia.replace(rxEsp, "");
    mes = mes.replace(rxEsp, "");
    ano = ano.replace(rxEsp, "");

    var dt = this.GetDataAtual();

    dia = dia == "" ? "" + dt.getDate() : dia;
    mes = mes == "" ? "" + (dt.getMonth() + 1) : mes;
    ano = ano == "" ? "" + dt.getFullYear() : ano;

    if (dia.length < 2)
        dia = '0' + dia;
    else
        dia = dia.substring(0, 2);

    if (mes.length < 2)
        mes = '0' + mes;
    else
        mes = mes.substring(0, 2);

    if (ano != null) {
        if (ano.length < 4) {
            if (ano > 30 && ano < 99)
                ano = "1900".substring(0, 4 - ano.length) + ano;
            else
                ano = "2000".substring(0, 4 - ano.length) + ano;
        }
        else
            ano = ano.substring(0, 4);
    }

    //Precisa completar data
    if (dia != null && mes != null && ano != null) {
        return this.get_Mascara().replace("dd", dia).replace("MM", mes).replace("yyyy", ano);
    }
    return sData;
}

//Valida a data com ano bisexto e formato.
//Ele não pode mudar o formato
Employer.Componentes.UI.Web.DataTextBox.prototype.ValidarDataClient = function (sDate) {
    try {
        var dtVal = this.GetData(sDate);

        var m = this.get_Mascara();

        m = m.replace("dd", AjaxClientControlBase.padLeft(dtVal.getDate(), '0', 2));
        m = m.replace("MM", AjaxClientControlBase.padLeft(dtVal.getMonth() + 1, '0', 2));
        m = m.replace("yyyy", dtVal.getFullYear());

        //O javascript troca de mes caso passe os dis válidos de mes.
        return sDate == m;
    }
    catch (e) {
        return false;
    }
}
Employer.Componentes.UI.Web.DataTextBox.prototype.get_DataMinima = function () {
    if (this.get_CampoDataMinima() != null) {
        var sValue = $get(this.get_CampoDataMinima()).value;
        if (this.ValidarDataClient(sValue))
            return this.GetData(sValue);
    }
    return this._DataMinima;
}
Employer.Componentes.UI.Web.DataTextBox.prototype.ValidarIntervaloMinimo = function () {
    if (this.get_DataMinima() == null)
        return true;
    var dt = this.get_DataMinima();
    return this.GetDataValue() >= new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 12, 0, 0, 0);
}
Employer.Componentes.UI.Web.DataTextBox.prototype.get_DataMaxima = function () {
    if (this.get_CampoDataMaxima() != null) {
        var sValue = $get(this.get_CampoDataMaxima()).value;
        if (this.ValidarDataClient(sValue))
            return this.GetData(sValue);
    }
    return this._DataMaxima;
}
Employer.Componentes.UI.Web.DataTextBox.prototype.ValidarIntervaloMaximo = function () {
    if (this.get_DataMaxima() == null)
        return true;
    var dt = this.get_DataMaxima();
    return this.GetDataValue() <= new Date(dt.getFullYear(), dt.getMonth(), dt.getDate(), 12, 0, 0, 0);
}
Employer.Componentes.UI.Web.DataTextBox.prototype.get_MensagemErroDataMinima = function () {
    if (this.get_DataMinima() != null && this._MensagemErroValorMinimo != null) {
        return this._MensagemErroValorMinimo.replace("{0}", this.DataFormatar(this.get_DataMinima())
        );
    }
    return this._MensagemErroValorMinimo;
}
//Compatibilidade com ControlBaseValidator
Employer.Componentes.UI.Web.DataTextBox.prototype.get_MensagemErroValorMinimo = function () {
    return this.get_MensagemErroDataMinima();
}
Employer.Componentes.UI.Web.DataTextBox.prototype.get_MensagemErroDataMaxima = function () {
    if (this.get_DataMaxima() != null && this._MensagemErroValorMaximo != null) {
        return this._MensagemErroValorMaximo.replace("{0}", this.DataFormatar(this.get_DataMaxima())
        );
    }
    return this._MensagemErroValorMaximo;
}
//Compatibilidade com ControlBaseValidator
Employer.Componentes.UI.Web.DataTextBox.prototype.get_MensagemErroValorMaximo = function () {
    return this.get_MensagemErroDataMaxima();
}
Employer.Componentes.UI.Web.DataTextBox.prototype.Validar = function (arg) {
    this.get_TextBox().value = this.FormatarData(this.get_TextBox().value);
    arg.Value = this.get_TextBox().value;

    var validator = $get(this._id + "_cvValor");
    var div = $get(this._id + "_pnlValidador")

    arg.IsValid = false;
    if (AjaxClientControlBase.IsNullOrEmpty(arg.Value)) {
        if (this.get_Obrigatorio()) {
            validator.innerHTML = this.get_MensagemErroObrigatorio();
            validator.errormessage = this.get_MensagemErroObrigatorioSummary();
        }
        else
            arg.IsValid = true;
    }
    else if (this.ValidarDataClient(arg.Value)) {
        if (this.ValidarIntervaloMinimo() == false) {
            validator.innerHTML = this.get_MensagemErroDataMinima();
            validator.errormessage = this.get_MensagemErroValorMinimoSummary();
        }
        else if (this.ValidarIntervaloMaximo() == false) {
            validator.innerHTML = this.get_MensagemErroDataMaxima();
            validator.errormessage = this.get_MensagemErroValorMaximoSummary();
        }
        else
            arg.IsValid = true;
    }
    else {
        validator.innerHTML = this.get_MensagemErroFormato();
        validator.errormessage = this.get_MensagemErroFormatoSummary();
    }

    if (arg.IsValid) {
        div.style.display = "none";
        this.get_Validador().style.visibility = "hidden"
    }
    else {
        this.get_Validador().style.visibility = "visible"
        div.style.display = "block";
    }
}
Employer.Componentes.UI.Web.DataTextBox.prototype.MostrarErros = function () {
    this.get_TextBox().value = this.FormatarData(this.get_TextBox().value);
    var Value = this.get_TextBox().value;

    var validator = $get(this._id + "_cvValor");
    //var div = $get(this._id + "_pnlValidador")
    var div = document.getElementById(this._id + "_pnlValidador");
    var IsValid = false;

    if (Value == "" || Value == null) {
        IsValid = true;
        validator.innerHTML = "";
        validator.errormessage = "";
    }
    else {
        if (this.ValidarDataClient(Value)) {
            if (this.ValidarIntervaloMinimo() == false) {
                validator.innerHTML = this.get_MensagemErroDataMinima();
                validator.errormessage = this.get_MensagemErroValorMinimoSummary();
            }
            else if (this.ValidarIntervaloMaximo() == false) {
                validator.innerHTML = this.get_MensagemErroDataMaxima();
                validator.errormessage = this.get_MensagemErroValorMaximoSummary();
            }
            else {
                IsValid = true;
                validator.innerHTML = "";
                validator.errormessage = "";
            }
        }
        else {
            validator.innerHTML = this.get_MensagemErroFormato();
            validator.errormessage = this.get_MensagemErroFormatoSummary();
        }
    }

    if (IsValid) {
        div.style.display = "none";
        //this.get_Validador().style.visibility = "hidden";
        this.get_Validador().style.display = "none";
    }
    else {
        //this.get_Validador().style.visibility = "visible";
        this.get_Validador().style.display = "block";
        div.style.display = "block";
    }

    return IsValid;
}
Employer.Componentes.UI.Web.DataTextBox.prototype.AtualizarFeriados = function (ano, inst) {
    if (this.get_UrlUpdateFeriados() != null && this.get_UrlUpdateFeriados() != "") {
        var instance = this;
        $.ajax({
            type: 'POST',
            url: this.get_UrlUpdateFeriados(),
            data: '{ ano: ' + ano + ' }',
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            success: function (msg) {
                var feriados = instance._Feriados || instance.get_Feriados();
                feriados[ano] = msg.d;
            }
        });
    }
}
Employer.Componentes.UI.Web.DataTextBox.prototype.ValidarBlur = function () {
    var val = $get(this._id + "_cvValor");

    val.isvalid = this.MostrarErros();

    return true;
}

Employer.Componentes.UI.Web.DataTextBox.prototype.Blur = function () {
    this.get_TextBox().value = this.FormatarData(this.get_TextBox().value);

    this.PostPadrao();

    //Atualiza Feriados
    var anoTemp;
    var mesTemp;
    if (this.GetDataValue() == null) {
        this.BuscaDataAtual();
        anoTemp = dtAtual.getFullYear();
        mesTemp = dtAtual.getMonth();
    } else {
        anoTemp = this.GetDataValue().getFullYear();
        mesTemp = this.GetDataValue().getMonth();
    }
    if (this._Feriados[anoTemp] == null) {
        this.AtualizarFeriados(anoTemp);
    }
    if (mesTemp <= 1 && this._Feriados[anoTemp - 1] == null) {
        this.AtualizarFeriados(anoTemp - 1);
    }
    if (mesTemp >= 10 && this._Feriados[anoTemp + 1] == null) {
        this.AtualizarFeriados(anoTemp + 1);
    }
}
Employer.Componentes.UI.Web.DataTextBox.prototype.KeyPress = function (e) {
    this._TextoAntesKeyPress = this.get_TextBox().value;

    var key = AjaxClientControlBase.Key.GetKeyCode(e.rawEvent);

    if (e.rawEvent.keyCode && AjaxClientControlBase.Key.isCommand(key))
        return true;

    if (key == Sys.UI.Key.space) {
        if (this.get_TextBox().value.length == 0) {
            this.get_TextBox().value = this.GetDataAtualFormatada();
            AjaxClientControlBase.CancelEvent(e);

            this.NextFocus(200);
        }
        else
            e.preventDefault();
        return false;
    }

    var caracter = String.fromCharCode(key);

    if (caracter == this.get_DateSeparator()) {
        if (this.ValidarSeparador() == false) { //Não deixa colocar duas mais q duas /
            return false; AjaxClientControlBase.CancelEvent(e);
        }
    } else if (isNaN(caracter))  //É letra    
        return AjaxClientControlBase.CancelEvent(e);

    if (this.get_TextBox().value.length > (7 + this.ContarSeparadores()) && !this.has_selected_text(this.get_TextBox())) {
        this.get_TextBox().value = this.get_TextBox().value.substring(0, (8 + this.ContarSeparadores()));
        return AjaxClientControlBase.CancelEvent(e);
    }
    else if (this.get_TextBox().value.length == (7 + this.ContarSeparadores()) && !this.has_selected_text(this.get_TextBox())) {
        this.NextFocus(200);
    }
}
Employer.Componentes.UI.Web.DataTextBox.prototype.ValidarSeparador = function () {
    var data = this.get_TextBox().value;
    var i = data.indexOf(this.get_DateSeparator());
    if (i >= 0)
        if (data.substring(i + 1).indexOf(this.get_DateSeparator()) >= 0)
            return false;
    return true;
}
Employer.Componentes.UI.Web.DataTextBox.prototype.ContarSeparadores = function () {
    var count = 0;
    for (i = 0; i < this.get_TextBox().value.length; i++) {
        if (this.get_TextBox().value[i] == this.get_DateSeparator()) {
            count = count + 1;
        }
    }
    return count;
}

Employer.Componentes.UI.Web.DataTextBox.prototype.RemoverMascara = function () {
    this._TextoAntesFocus = this.get_TextBox().value;

    var valor = AjaxClientControlBase.Trim(this.get_TextBox().value);

    var rx = new RegExp("\\" + this.get_DateSeparator() + "+", "g");
    if (valor.match(rx)) {
        valor = valor.replace(rx, '');
    }
    this.get_TextBox().value = valor;
    this.get_TextBox().select();
}

Employer.Componentes.UI.Web.DataTextBox.descriptor = {
    properties: [
                { name: 'Obrigatorio', type: Boolean },
                { name: 'DataMinima', type: Date },
                { name: 'DataMaxima', type: Date },
                { name: 'MensagemErroObrigatorio', type: String },
                { name: 'MensagemErroFormato', type: String },
                { name: 'MensagemErroValorMinimo', type: String },
                { name: 'MensagemErroValorMaximo', type: String }
                ]
}

Employer.Componentes.UI.Web.DataTextBox.registerClass('Employer.Componentes.UI.Web.DataTextBox', Sys.UI.Behavior);



/*if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();*/


Employer.Componentes.UI.Web.DataTextBox.ValidarTextBox = function (sender, args) {
    var controle = $find(sender.id.replace("_cvValor", ""));
    
    if (controle == null)
        return;

    controle.Validar(args);
}