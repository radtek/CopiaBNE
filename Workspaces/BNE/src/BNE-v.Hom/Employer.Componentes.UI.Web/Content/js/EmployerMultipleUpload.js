Type.registerNamespace("Employer.Componentes.UI.Web");

// Define a simplified component.
Employer.Componentes.UI.Web.EmployerMultipleUpload = function (element) {
    Employer.Componentes.UI.Web.EmployerMultipleUpload.initializeBase(this, [element]);

    this._id = element.id;
    this._RegexValidacao = null;

    // Compatibilidade com ControlBaseValidator
    this._mensagemErroValorMinimo = null;
    this._mensagemErroValorMaximo = null;

    this._caminhoSWF = null;
    this._caminhoUpload = null;
    this._IdArquivos = null;
    this._ExtensoesArquivos = null;
    this._DesExtensoesArquivos = null;
    this._ImgBotao = null;
    this._ImgBotaoCancelar = null;
    this._CssBotao = null;
    this._TextoBotao = null;
    this._TamanhoLimiteArquivo = null;
    this._MensagemErroTamanhoLimiteArquivo = null;
    this._MensagemErroArquivoEmBranco = null;
    this._TamanhoQtdLimiteArquivos = null;
    this._MensagemErroQtdLimiteArquivos = null;

    this._ImplementaEventoDeErro = null

    // Handlers for the events    
    this._onChange = null;

    this.get_TamanhoLimiteArquivo = function () {
        return this._TamanhoLimiteArquivo;
    },
    this.set_TamanhoLimiteArquivo = function (value) {
        this._TamanhoLimiteArquivo = value;
    },

    this.get_ImplementaEventoDeErro = function () {
        return this._ImplementaEventoDeErro;
    },
    this.set_ImplementaEventoDeErro = function (value) {
        this._ImplementaEventoDeErro = value;
    },

    this.get_MensagemErroQtdLimiteArquivos = function () {
        return this._MensagemErroQtdLimiteArquivos;
    },
    this.set_MensagemErroQtdLimiteArquivos = function (value) {
        this._MensagemErroQtdLimiteArquivos = value;
    },

    this.get_MensagemErroTamanhoLimiteArquivo = function () {
        return this._MensagemErroTamanhoLimiteArquivo;
    },
    this.set_MensagemErroTamanhoLimiteArquivo = function (value) {
        this._MensagemErroTamanhoLimiteArquivo = value;
    },

    this.get_MensagemErroArquivoEmBranco = function () {
        return this._MensagemErroArquivoEmBranco;
    },
    this.set_MensagemErroArquivoEmBranco = function (value) {
        this._MensagemErroArquivoEmBranco = value;
    },

    this.get_TamanhoQtdLimiteArquivos = function () {
        return this._TamanhoQtdLimiteArquivos;
    },
    this.set_TamanhoQtdLimiteArquivos = function (value) {
        this._TamanhoQtdLimiteArquivos = value;
    },

    this.get_CaminhoSWF = function () {
        return this._caminhoSWF;
    },
    this.set_CaminhoSWF = function (value) {
        this._caminhoSWF = value;
    },

    this.get_CaminhoUpload = function () {
        return this._caminhoUpload;
    },
    this.set_CaminhoUpload = function (value) {
        this._caminhoUpload = value;
    },

    this.get_IdArquivos = function () {
        return this._IdArquivos;
    },
    this.set_IdArquivos = function (value) {
        this._IdArquivos = value;
    },

    this.get_ExtensoesArquivos = function () {
        return this._ExtensoesArquivos;
    },
    this.set_ExtensoesArquivos = function (value) {
        this._ExtensoesArquivos = value;
    },

    this.get_DesExtensoesArquivos = function () {
        return this._DesExtensoesArquivos;
    },
    this.set_DesExtensoesArquivos = function (value) {
        this._DesExtensoesArquivos = value;
    },

    this.get_ImgBotao = function () {
        return this._ImgBotao;
    },
    this.set_ImgBotao = function (value) {
        this._ImgBotao = value;
    },

    this.get_ImgBotaoCancelar = function () {
        return this._ImgBotaoCancelar;
    },
    this.set_ImgBotaoCancelar = function (value) {
        this._ImgBotaoCancelar = value;
    },

    this.get_CssBotao = function () {
        return this._CssBotao;
    },
    this.set_CssBotao = function (value) {
        this._CssBotao = value;
    },

    this.get_TextoBotao = function () {
        return this._TextoBotao;
    },
    this.set_TextoBotao = function (value) {
        this._TextoBotao = value;
    },

    this.initialize = function () {
        Employer.Componentes.UI.Web.EmployerMultipleUpload.callBaseMethod(this, 'initialize');

        // Wireup the event handlers
        var element = this.get_CampoArquivo();
        if (element) {
            this._onChange = Function.createDelegate(this, this.Change);
            $addHandler(element, 'change', this._onChange);

            var instace = this;

            //$(function () {
            var uploadComp = $(element).uploadify({
                'swf': this.get_CaminhoSWF(),
                'uploader': this.get_CaminhoUpload() +
                    (this.get_CaminhoUpload().indexOf("?") > -1 ? "&" : "?") + "IdArquivos=" + this.get_IdArquivos(),
                'fileTypeExts': this.get_ExtensoesArquivos(),
                'fileTypeDesc': this.get_DesExtensoesArquivos(),
                'buttonImage': this.get_ImgBotao(),
                'itemTemplate': '<div id="${fileID}" class="uploadify-queue-item">' +
					'<div class="cancel">' +
						'<a style="background:url(\'' + this.get_ImgBotaoCancelar() + '\')" ' +
                       ' href="javascript:$(\'#${instanceID}\').uploadify(\'cancel\', \'${fileID}\')">X</a>' +
					'</div>' +
					'<span class="fileName">${fileName} (${fileSize})</span><span class="data"></span>' +
					'<div class="uploadify-progress">' +
						'<div class="uploadify-progress-bar"><!--Progress Bar--></div>' +
					'</div>' +
				'</div>',
                'onSelectError': function (file, errorCode, errorMsg) {
                    var mensagem = this.queueData.errorMsg;

                    if (SWFUpload.QUEUE_ERROR.QUEUE_LIMIT_EXCEEDED == errorCode) {
                        if (!AjaxClientControlBase.IsNullOrEmpty(instace.get_MensagemErroQtdLimiteArquivos())) {
                            mensagem = String.format(instace.get_MensagemErroQtdLimiteArquivos(), instace.get_TamanhoQtdLimiteArquivos());
                        }
                    }
                    else if (SWFUpload.QUEUE_ERROR.FILE_EXCEEDS_SIZE_LIMIT == errorCode) {
                        if (!AjaxClientControlBase.IsNullOrEmpty(instace.get_MensagemErroTamanhoLimiteArquivo())) {
                            mensagem = String.format(instace.get_MensagemErroTamanhoLimiteArquivo(), file.name, instace.get_TamanhoLimiteArquivo());
                        }
                    }
                    else if (SWFUpload.QUEUE_ERROR.ZERO_BYTE_FILE == errorCode) {

                        if (!AjaxClientControlBase.IsNullOrEmpty(instace.get_MensagemErroArquivoEmBranco())) {
                            mensagem = String.format(instace.get_MensagemErroArquivoEmBranco(), file.name);
                        }
                    }

                    this.queueData.errorMsg = "";
                    instace.add_ErrosNoUpload(mensagem, errorCode);
                },
                'onDialogClose': function () {
                    var erros = instace.get_ErrosNoUpload();
                    if (this.queueData.filesErrored > 0 && erros.length > 0) {
                        if (!instace.get_ImplementaEventoDeErro()) {
                            var msgs = "Erro(s):";

                            for (var i = 0; erros.length > i; i++)
                                msgs += "\n" + erros[i].Mensagem;

                            alert(msgs);
                        }
                        else if (this.queueData.filesQueued == 0) {
                            //dispara evento de erro
                            instace.get_Campo2("_btnErroUpload").click();
                        }
                    }
                },
                'onDialogOpen': function () {
                    instace.clear_ErrosNoUpload();
                },
                'buttonClass': this.get_CssBotao(),
                'buttonText': this.get_TextoBotao(),
                'fileSizeLimit': this.get_TamanhoLimiteArquivo(),
                'multi': this.get_TamanhoQtdLimiteArquivos() > 1,
                'onSWFReady': function () {
                    window.setTimeout(function () {
                        if (instace.get_Campo2("_txtCampoVal").getAttribute("disabled")) {
                            uploadComp.uploadify('settings', 'buttonClass', instace.get_CssBotao() + " " + instace.get_CssBotao() + "_disable");
                            uploadComp.uploadify('disable', true);
                        }
                    }, 400);
                },
                'onUploadSuccess':
                function (file, data, response) {
                    var campo = $(instace.get_TextBox());
                    campo.val(campo.val() + ";" + file.name);
                },
                'queueSizeLimit': this.get_TamanhoQtdLimiteArquivos(),
                'onUploadComplete':
                function (file) {
                    var campo = $(instace.get_TextBox());
                    var args = { Value: campo.val(), IsValid: true }
                    instace.Validar(args);
                },
                'onQueueComplete':
                function (queueData) {
                    var btnAtualizarGrid = instace.get_BtnAtualizarGrid();
                    btnAtualizarGrid.click();
                }
            });

            
        }
    },
    this.dispose = function () {
        var element = this.get_CampoArquivo();
        if (element) {
            $(element).uploadify('destroy');
            $clearHandlers(element);
        }

        Employer.Componentes.UI.Web.EmployerMultipleUpload.callBaseMethod(this, 'dispose');
    },
    this.get_Campo2 = function (nome) {
        var espGrid = RegExp("(_[0-9]+)$");
        var m = espGrid.exec(this._id);

        if (m != null)
            return $get(this._id + nome + m[0]);
        else
            return $get(this._id + nome);
    },
    this.get_BtnAtualizarGrid = function () {
        return this.get_Campo2("_btnAtualizarGrid");
    },

    this.get_TextBox = function () {
        return this.get_Campo2("_txtCampoVal");
    },
    this.set_TextBox = function (value) {
        this.get_TextBox().value = value;
    },

    this.get_CampoArquivo = function () {
        return this.get_Campo2("_CampoArquivo");
    },

    this.get_ErrosNoUpload = function () {
        var val = this.get_Campo2("_hdnErros").value;

        if (AjaxClientControlBase.IsNullOrEmpty(val))
            val = "[]";

        return Sys.Serialization.JavaScriptSerializer.deserialize(val);
    },
    this.clear_ErrosNoUpload = function () {
        this.get_Campo2("_hdnErros").value = "";
    },

    this.add_ErrosNoUpload = function (msg, codErro) {
        var obj = this.get_ErrosNoUpload();

        obj[obj.length] = { Mensagem: msg, Codigo: codErro };

        var val = Sys.Serialization.JavaScriptSerializer.serialize(obj);

        this.get_Campo2("_hdnErros").value = val;
    },

    this.get_PnlValidador = function () {
        return this.get_Campo2("_pnlValidador");
    },

    this.get_RegexValidacao = function () {
        return this._RegexValidacao;
    },
    this.set_RegexValidacao = function (valor) {
        this._RegexValidacao = valor;
    },

    //Compatibilidade com ControlBaseValidator
    this.get_MensagemErroValorMinimo = function () {
        return this._mensagemErroValorMinimo;
    },
    this.set_MensagemErroValorMinimo = function (valor) {
        this._mensagemErroValorMinimo = valor;
    },
    this.get_MensagemErroValorMaximo = function () {
        return this._mensagemErroValorMaximo;
    },
    this.set_MensagemErroValorMaximo = function (valor) {
        this._mensagemErroValorMaximo = valor;
    }
    //Fim compatibilidade com ControlBaseValidator
}

//Simula herança
Employer.Componentes.UI.Web.EmployerMultipleUpload.prototype = new Employer.Componentes.UI.Web.ControlBaseValidator;

Employer.Componentes.UI.Web.EmployerMultipleUpload.prototype.Change = function () {
    this.set_TextBox(this.get_CampoArquivo().value);

    var val = this.get_Validator();
    ValidatorEnable(val);
}

Employer.Componentes.UI.Web.EmployerMultipleUpload.prototype.ValidarRegex = function () {
    if (this.get_RegexValidacao() == null)
        return true;

    var valor = AjaxClientControlBase.Trim(this.get_TextBox().value);

    var patt = new RegExp(this.get_RegexValidacao(), "g");

    return patt.test(valor);
}

Employer.Componentes.UI.Web.EmployerMultipleUpload.prototype.MostrarErro = function (msg, msgSummary) {
    var validator = this.get_Validator();
    var div = this.get_PnlValidador();

    validator.innerHTML = msg;
    validator.errormessage = msgSummary == null || msgSummary == undefined ? msg : msgSummary;
    validator.style.display = "block"
    div.style.display = "block";
}

Employer.Componentes.UI.Web.EmployerMultipleUpload.prototype.Validar = function (arg) {
    arg.Value = this.get_TextBox().value;

    var validator = this.get_Validator();
    var div = this.get_PnlValidador();

    arg.IsValid = true;
    if (arg.Value == "" || arg.Value == null) {
        if (this.get_Obrigatorio()) {
            validator.innerHTML = this.get_MensagemErroObrigatorio();
            validator.errormessage = this.get_MensagemErroObrigatorioSummary();
            arg.IsValid = false;
        }
    }
    
    if (arg.IsValid) {
        div.style.display = "none";
        this.get_Validator().style.display = "none"
    }
    else {
        this.get_Validator().style.display = "block"
        div.style.display = "block";
    }
}

Employer.Componentes.UI.Web.EmployerMultipleUpload.registerClass('Employer.Componentes.UI.Web.EmployerMultipleUpload', Sys.UI.Behavior);


Employer.Componentes.UI.Web.EmployerMultipleUpload.ValidarTextBox = function (sender, args) {
    var espGrid = RegExp("(_[0-9]+)$");
    var m = espGrid.exec(sender.id);

    var controle = null;

    if (m != null) {
        controle = $find(sender.id.replace("_cvValor_" + m[0], ""));
    }
    else {
        controle = $find(sender.id.replace("_cvValor", ""));
    }

    if (controle != null)
        controle.Validar(args);
}
