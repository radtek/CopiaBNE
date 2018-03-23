var possuiMediaSalarial = false;
var campoNaoInformado = 'Não informado';
var campoObrigatorio = 'Obrigatório';
var campoEnderecoObrigatorio = 'CEP Obrigatório';

var Parametro = {
    ObrigatorioInstituicao: 0,
    VisivelInstituicao: 0,
    ObrigatorioCurso: 0,
    VisivelCurso: 0,
    ObrigatorioLocal: 0,
    VisivelLocal: 0,
    ObrigatorioCidadeEstado: 0,
    VisivelCidadeEstado: 0,
    ObrigatorioSituacao: 0,
    VisivelSituacao: 0,
    ObrigatorioPeriodo: 0,
    VisivelPeriodo: 0,
    ObrigatorioAnoConclusao: 0,
    VisivelAnoConclusao: 0
}

var CamposFormacao = {
    rfvInstituicao: '',
    rfvTituloCurso: '',
    cvTituloCurso: '',
    cvSituacao: '',
    txtPeriodo: '',
    txtAnoConclusao: '',
    rfvCidade: ''
}

var NivelCurso = {
    Tecnico: '',
    Tecnologo: '',
    Graduacao: '',
    PosGraduacao: '',
    Mestrado: '',
    Doutorado: '',
    PosDoutorado: '',
    Aperfeicoamento: ''
}

function InicializarCamposFormacao(parametros) {
    CamposFormacao.rfvCidade = parametros.rfvCidade,
    CamposFormacao.rfvTituloCurso = parametros.rfvTituloCurso,
    CamposFormacao.cvTituloCurso = parametros.cvTituloCurso,
    CamposFormacao.cvSituacao = parametros.cvSituacao,
    CamposFormacao.txtPeriodo = parametros.txtPeriodo,
    CamposFormacao.txtAnoConclusao = parametros.txtAnoConclusao,
    CamposFormacao.rfvInstituicao = parametros.rfvInstituicao
}

var AutoCompleteExtenders = {
    MsgFaixaSalarial: ''
}

function InicializarAutoCompleteMiniCurriculo(parametros) {
    AutoCompleteExtenders.MsgFaixaSalarial = parametros.msgFaixaSalarial;
}

function EditarCampo(args, idTextBox) {
    var divControle = employer.util.findControl(args);
    var divControleEditar = employer.util.findControl(args.replace('Label', 'TextBox'));

    var controle;

    if (typeof (idTextBox) != 'undefined') {
        controle = idTextBox;
    }
    else {
        controle = divControleEditar[0].childNodes[0].id;
    }

    employer.controles.enableControle(idTextBox, true);

    divControle.css('display', 'none');
    divControleEditar.css('display', 'block');

    if (controle != null) {
        employer.controles.setFocusControle(controle);
        employer.controles.setFocus(controle);
    }
}

function cvFuncaoPretendida1_Validate(sender, args) {

    res = ConferirDados.ValidarFuncao(args.Value);
    args.IsValid = (res.error == null && res.value);

    if (!args.IsValid) {
        sender.innerHTML = "Função Inválida";
        return;
    }

    //var res = ValidarFuncaoLimitacaoPorContrato(args.Value);
    //args.IsValid = (res.error == null && res.value);

    //if (!args.IsValid) {
    //    sender.innerHTML = "Função indisponível";
    //}
}

function ValidarFuncaoLimitacaoPorContrato(value) {
    if (value == null)
        return { 'IsValid': false, 'value': false, 'error': null};

    var limitacaoContrato = replaceDiacritics(value).toString().trim().toLowerCase();
    if (limitacaoContrato == "aprendiz"
        || limitacaoContrato == "estagiario"
        || limitacaoContrato == "estagiaria"
        || limitacaoContrato == "estagio")
        return { 'IsValid': false, 'value': false, 'error': null };

    return { 'IsValid': true, 'value': true, 'error': null };
}

function replaceDiacritics(s) {
    var s;
    var diacritics = [
        /[\300-\306]/g, /[\340-\346]/g,  // A, a
        /[\310-\313]/g, /[\350-\353]/g,  // E, e
        /[\314-\317]/g, /[\354-\357]/g,  // I, i
        /[\322-\330]/g, /[\362-\370]/g,  // O, o
        /[\331-\334]/g, /[\371-\374]/g,  // U, u
    ];

    var chars = ['A', 'a', 'E', 'e', 'I', 'i', 'O', 'o', 'U', 'u'];

    for (var i = 0; i < diacritics.length; i++) {
        s = s.replace(diacritics[i], chars[i]);
    }

    return s;
}

function ControlarTipoContrato(label, checkboxListContainer) {
    checkboxListContainer = $(checkboxListContainer);
    if (checkboxListContainer == null)
        return;

    checkboxListContainer.focusout(function () {
        var result = CalcularTipoContrato(checkboxListContainer);
        if (result == campoNaoInformado) {
            label.className = "label_nao_informado";
        }
        else if (result == campoObrigatorio) {
            label.className = "label_obrigatorio";
        }
        else if (typeof (result) != 'undefined')
            label.className = "";

        if (label == null)
            return;

        employer.util.findControl(label.id).html(result.replace(/(\r\n|\n|\r)/gm, "<br/>"));
    });
}

function ExibirValorCampo(idDiv, idLabel, idTextBox) {
    var divControle = employer.util.findControl(idDiv);
    var divControleEditar = employer.util.findControl(idDiv.replace('TextBox', 'Label'));
    var controle = employer.util.findControl(idLabel);
    var controleEditar = employer.util.findControl(idTextBox);

    if (!ValidadorValido('CadastroRevisao', idTextBox)) {
        employer.controles.setFocusControle(controle.id);
    }
    else {
        employer.controles.enableControle(idTextBox, false);

        divControle.css('display', 'none');
        divControleEditar.css('display', 'block');
        //Componente Employer
        var value = employer.controles.recuperarValorControle(idTextBox);

        //Radio Button List        
        if (typeof (value) == 'undefined') {
            //Sexo
            var rblSexo = employer.util.findControl('rblSexo');
            if (idTextBox === rblSexo[0].id) {
                value = rblSexo.find('input:radio:checked').next().text();
            }
            //Estado Civil
            var ddlEstadoCivil = employer.util.findControl('ddlEstadoCivil');
            if (idTextBox === ddlEstadoCivil[0].id) {
                value = ddlEstadoCivil.find('option:selected').text();
                if (value == "Selecione")
                    value = campoObrigatorio;
            }
            //Filhos
            var ddlFilhos = employer.util.findControl('ddlFilhos');
            if (idTextBox === ddlFilhos[0].id) {
                value = ddlFilhos.find('option:selected').text();
                if (value == "Selecione")
                    value = campoNaoInformado;
            }
            //Habilitação
            var ddlHabilitacao = employer.util.findControl('ddlHabilitacao');
            if (idTextBox === ddlHabilitacao[0].id) {
                value = ddlHabilitacao.find('option:selected').text();
                if (value == "Selecione")
                    value = campoNaoInformado;
            }
            //Raca
            var ddlRaca = employer.util.findControl('ddlRaca');
            if (idTextBox === ddlRaca[0].id) {
                value = ddlRaca.find('option:selected').text();
                if (value == "Selecione")
                    value = campoNaoInformado;
            }

            //Deficiencia
            var ddlDeficiencia = employer.util.findControl('ddlDeficiencia');
            if (idTextBox === ddlDeficiencia[0].id) {
                value = ddlDeficiencia.find('option:selected').text();
                if (value == "Selecione")
                    value = campoNaoInformado;
            }

            //Tipo Contrato
            var boxList = employer.util.findControl('chblTipoContrato');
            if (boxList != null && boxList.length == 1) {
                if (idTextBox == boxList[0].id) {

                    value = CalcularTipoContrato(boxList[0]);

                    if (value == null || value == '') {
                        value = campoObrigatorio;
                    }
                }
            }

            //Disponibilidade para Viagem
            var ckbDisponibilidadeViagem = employer.util.findControl('ckbDisponibilidadeViagem');
            if (idTextBox === ckbDisponibilidadeViagem[0].id) {
                value = ckbDisponibilidadeViagem[0].checked;
                value = value ? 'Sim' : 'Não';
            }

            //Disponibilidade para Trabalho
            var ckbDisponibilidade = employer.util.findControl('ckblDisponibilidade');
            if (idTextBox === ckbDisponibilidade[0].id) {
                value = '';
                ckbDisponibilidade.each(function () {
                    this.disabled = false;
                });

                ckbDisponibilidade.find("input[type=checkbox]").each(function () {
                    if (this.checked) {
                        if (value.length > 0)
                            value += ', ' + this.parentElement.innerText;
                        else
                            value += this.parentElement.innerText;
                    }
                });


                if (value === '')
                    value = campoObrigatorio;
            }

            if (typeof (value) == 'undefined') {
                //Componente ASPNET
                var ddd, fone;
                ddd = employer.controles.recuperarValor(idTextBox + '_txtDDD');
                fone = employer.controles.recuperarValor(idTextBox + '_txtFone');
                if (typeof (ddd) != 'undefined' && typeof (fone) != 'undefined')
                    if (fone.trim() == "") {
                        value = campoNaoInformado;
                    }
                    else {
                        value = '(' + ddd + ') ' + fone;
                    }
            }

            if (typeof (value) == 'undefined') {
                //Componente ASPNET
                value = employer.controles.recuperarValor(idTextBox);
            }
        }

        if (value == '') {
            if (idTextBox.toString().indexOf('FuncaoPretendida1') > -1) {
                value = campoObrigatorio;
            } else {
                value = campoNaoInformado;
            }
        }

        employer.util.findControl(idLabel).html(value.replace(/(\r\n|\n|\r)/gm, "<br/>"));

        //Campos de Caracteristicas fisicas
        nome = employer.util.findControl(idLabel)[0].id;
        if (nome.indexOf("Peso") != -1)
            employer.util.findControl(idLabel).text('Peso: ' + value + "Kg ");
        else if (nome.indexOf("Altura") != -1)
            employer.util.findControl(idLabel).text('Altura: ' + value + "m ");
        else if (nome.indexOf("Raca") != -1)
            employer.util.findControl(idLabel).text('Raça: ' + value + " ");
    }

    if (value == campoNaoInformado) {
        document.getElementById(idLabel).className = "label_nao_informado";
    }
    else if (value == campoObrigatorio) {
        document.getElementById(idLabel).className = "label_obrigatorio";
    }
    else if (typeof (value) != 'undefined')
        document.getElementById(idLabel).className = "";
}

function CalcularTipoContrato(componente) {
    var value = '';
    if (componente == null)
        return value;

    componente.find(':checkbox')
        .each(function (index, checkItem) {

            if (!checkItem.checked) {
                return;
            }

            if (value != '') {
                value += ", ";
            }
            value += checkItem.nextSibling.firstChild.data;
        });

    if (value == '')
        value = campoObrigatorio;

    return value;
}

//Função TRIM
String.prototype.trim = function () {
    return this.replace(/^\s*/, "").replace(/\s*$/, "");
}

function ValidarNome(source, args) {
    var w, z, y, x;
    var isValid = true;
    for (x = 0; x < args.Value.length; x++) {
        z = args.Value.substring(x, x + 1);
        if ((x >= 2 && z == y && z == w)) {
            isValid = false;
        }
        else {
            y = w;
            w = z;
            z = '-';
        }
    }

    if (!args.Value.match("^[A-Z,a-z,ÁáÂâÃãÄäÉéÊêËëÍíÏïÓóÔôÕõÖöÚúÛûÜüÇç']{1,}( ){1,}[A-Z,a-z,ÁáÂâÃãÄäÉéÊêËëÍíÏïÓóÔôÕõÖöÚúÛûÜüÇç']{2,}(( ){1,}[A-Z,a-z,ÁáÂâÃãÄäÉéÊêËëÍíÏïÓóÔôÕõÖöÚúÛûÜüÇç']{1,})*$"))
        isValid = false;

    args.IsValid = isValid;
}

function cvEstadoCivil_Validate(sender, args) {
    var res = ConferirDados.ValidarEstadoCivil(args.Value);
    args.IsValid = (res.error == null && res.value != '');
}

function cvCidade_Validate(sender, args) {
    var res = ConferirDados.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function cvCidadeDisponivel_Validate(sender, args) {
    var res = ConferirDados.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function cvDisponibilidade_Validate(sender, args) {
    var ckList = employer.util.findControl('ckblDisponibilidade');
    var checkeds = ckList.find('input:checked');
    if (!checkeds.length) {
        args.IsValid = false;
    }
    else {
        args.IsValid = true;
    }
}

function moveToNextElement(element, event, size) {
    /// <summary>
    /// Força foco para o proximo campo caso ultrapasse o tamanho do campo.
    /// </summary>
    var tecla = employer.event.getKey(event);
    // alterado validação para todas as teclas especiais
    if (element.value.length >= size && !employer.key.isSpecial(tecla[0]) && component_isTextNotSelected(element)) {
        // caso seja o único elemento ativo da pagina: tira o foco dele (para executar comportamentos do onblur) e retorna o foco ao elemento
        element.blur();
    } else {
        return true;
    }
}

function FuncaoPretendida_OnChange(args) {
    var mediaSalarial = ConferirDados.PesquisarMediaSalarial(employer.controles.recuperarValor('txtFuncaoPretendida1'), employer.controles.recuperarValor('txtCidade'));
    if (mediaSalarial.value != null && mediaSalarial.value != 0) {
        var valorMinimo = mediaSalarial.value.split(';')[0];
        var valorMaximo = mediaSalarial.value.split(';')[1];
        var mensagem = AutoCompleteExtenders.MsgFaixaSalarial.replace('{0}', valorMinimo).replace('{1}', valorMaximo);
        employer.controles.setAttr('faixa_salarial', 'innerHTML', mensagem);
        possuiMediaSalarial = true;
    }
    else
        possuiMediaSalarial = false;

    FuncaoPretendida_OnBlur();
}

function FuncaoPretendida_OnBlur() {
    var funcaoPretendida = employer.controles.recuperarValor('txtFuncaoPretendida1');

    if (funcaoPretendida != "") {
        if (possuiMediaSalarial)
            employer.util.findControl('faixa_salarial').css('display', 'block');
        else
            employer.util.findControl('faixa_salarial').css('display', 'none');
    }
    else
        employer.util.findControl('faixa_salarial').css('display', 'none');
}

function ddlTipoVeiculo_SelectedIndexChanged(sender, args) {
    var value = sender._value;
    if (value != '0') {
        employer.controles.enableValidatorControleVal('txtModelo', 'rfValor', true, false);
        employer.controles.setAttr('txtModelo', 'Obrigatorio', true);
        employer.controles.enableValidatorControleVal('txtAnoVeiculo', 'rfValor', true, false);
        employer.controles.setAttr('txtAnoVeiculo', 'Obrigatorio', true);

    }
    else {
        employer.controles.enableValidatorControleVal('txtModelo', 'rfValor', false, true);
        employer.controles.setAttr('txtModelo', 'Obrigatorio', false);
        employer.controles.enableValidatorControleVal('txtAnoVeiculo', 'rfValor', false, true);
        employer.controles.setAttr('txtAnoVeiculo', 'Obrigatorio', false);
    }
}

function cvTipoVeiculo_Validate(sender, args) {
    var value = employer.util.findControl('ddlTipoVeiculo')[0].value;
    if (value == '0')
        args.IsValid = false;
    else
        args.IsValid = true;
}

function cvAtividadeExercida_Validate(sender, args) {
    args.IsValid = args.Value > 0 ? true : false;
}

function txtAtividadeExercida_OnBlur() {
    employer.controles.setFocusControle('txtUltimoSalario');
}

function ddlNivel_SelectedIndexChanged(args) {
    AjustarParametros(employer.util.findControl('ddlNivel')[0].value);
    AjustarAutoCompleteNivelCurso(employer.util.findControl('ddlNivel')[0].value);
    AjustarCampos();
}

function ddlNivelEspecializacao_SelectedIndexChanged(args) {
    AjustarParametros(employer.util.findControl('ddlNivelEspecializacao')[0].value);
    AjustarAutoCompleteNivelCurso(employer.util.findControl('ddlNivelEspecializacao')[0].value);
    AjustarCamposEspecializacao();
}

function AjustarCampos() {
    var obrigatorioInstituicao = !!parseInt(Parametro.ObrigatorioInstituicao);
    var visivelInstituicao = !!parseInt(Parametro.VisivelInstituicao);
    var obrigatorioCurso = !!parseInt(Parametro.ObrigatorioCurso);
    var visivelCurso = !!parseInt(Parametro.VisivelCurso);
    var obrigatorioLocal = !!parseInt(Parametro.ObrigatorioLocal);
    var obrigatorioCidadeEstado = !!parseInt(Parametro.ObrigatorioCidadeEstado);
    var visivelCidadeEstado = !!parseInt(Parametro.VisivelCidadeEstado);
    var obrigatorioSituacao = !!parseInt(Parametro.ObrigatorioSituacao);
    var visivelSituacao = !!parseInt(Parametro.VisivelSituacao);
    var obrigatorioPeriodo = !!parseInt(Parametro.ObrigatorioPeriodo);
    var visivelPeriodo = !!parseInt(Parametro.VisivelPeriodo);
    var visivelSituacaoPeriodo = visivelSituacao || visivelPeriodo;
    var obrigatorioAnoConclusao = !!parseInt(Parametro.ObrigatorioAnoConclusao);
    var visivelAnoConclusao = !!parseInt(Parametro.VisivelAnoConclusao);

    //Habilitando/Desabilitando os componentes
    var arrControles = new Array(
                                    { id: 'txtInstituicao', tipo: ControlType.Aspnet, enable: visivelInstituicao },
                                    { id: 'txtTituloCurso', tipo: ControlType.Aspnet, enable: visivelCurso },
                                    { id: 'ddlSituacao', tipo: ControlType.Component, enable: visivelSituacao },
                                    { id: 'txtPeriodo', tipo: ControlType.Component, enable: visivelPeriodo },
                                    { id: 'txtAnoConclusao', tipo: ControlType.Component, enable: visivelAnoConclusao }
                                );

    employer.controles.enableControlesArr(arrControles);

    //Mostrando/Escondendo as DIV's
    var arrDivs = new Array(
                                { id: 'divLinhaInstituicao', enable: visivelInstituicao },
                                { id: 'divLinhaTituloCurso', enable: visivelCurso },
                                { id: 'divCidade', enable: visivelCidadeEstado },
                                { id: 'divLinhaSituacao', enable: visivelSituacaoPeriodo },
                                { id: 'divLinhaConclusao', enable: visivelAnoConclusao }
                           );

    var arrComponentes = new Array(
                                { id: 'lblPeriodo', enable: visivelPeriodo },
                                { id: 'txtPeriodo', enable: visivelPeriodo }
                            );

    Funcoes.MostrarEsconderDivs(arrDivs);
    Funcoes.MostrarEsconderControles(arrComponentes);

    //Habilitando/Desabilitando os validadores
    employer.controles.enableValidatorVal(CamposFormacao.rfvInstituicao, obrigatorioInstituicao, true);
    employer.controles.enableValidatorVal(CamposFormacao.rfvTituloCurso, obrigatorioCurso, true);
    employer.controles.enableValidatorVal(CamposFormacao.cvSituacao, obrigatorioSituacao, true);
    employer.controles.enableValidatorControleVal(CamposFormacao.txtPeriodo, 'rfValor', obrigatorioPeriodo, true);
    employer.controles.setAttr(CamposFormacao.txtPeriodo, 'Obrigatorio', obrigatorioPeriodo);
    employer.controles.enableValidatorControleVal(CamposFormacao.txtAnoConclusao, 'rfValor', obrigatorioAnoConclusao, true);
    employer.controles.setAttr(CamposFormacao.txtAnoConclusao, 'Obrigatorio', obrigatorioAnoConclusao);
    employer.controles.enableValidatorVal(CamposFormacao.rfvCidade, obrigatorioCidadeEstado, true);
}

function AjustarCamposEspecializacao() {
    var obrigatorioInstituicao = !!parseInt(Parametro.ObrigatorioInstituicao);
    var visivelInstituicao = !!parseInt(Parametro.VisivelInstituicao);
    var obrigatorioCurso = !!parseInt(Parametro.ObrigatorioCurso);
    var visivelCurso = !!parseInt(Parametro.VisivelCurso);
    var obrigatorioLocal = !!parseInt(Parametro.ObrigatorioLocal);
    //var visivelLocal = !!parseInt(Parametro.VisivelLocal);
    var obrigatorioCidadeEstado = !!parseInt(Parametro.ObrigatorioCidadeEstado);
    var visivelCidadeEstado = !!parseInt(Parametro.VisivelCidadeEstado);
    var obrigatorioSituacao = !!parseInt(Parametro.ObrigatorioSituacao);
    var visivelSituacao = !!parseInt(Parametro.VisivelSituacao);
    var obrigatorioPeriodo = !!parseInt(Parametro.ObrigatorioPeriodo);
    var visivelPeriodo = !!parseInt(Parametro.VisivelPeriodo);
    var visivelSituacaoPeriodo = visivelSituacao || visivelPeriodo;
    var obrigatorioAnoConclusao = !!parseInt(Parametro.ObrigatorioAnoConclusao);
    var visivelAnoConclusao = !!parseInt(Parametro.VisivelAnoConclusao);

    //Habilitando/Desabilitando os componentes
    var arrControlesEspecializacao = new Array(
                                    { id: 'txtInstituicaoEspecializacao', tipo: ControlType.Aspnet, enable: visivelInstituicao },
                                    { id: 'txtTituloCursoEspecializacao', tipo: ControlType.Aspnet, enable: visivelCurso },
                                    { id: 'ddlSituacaoEspecializacao', tipo: ControlType.Component, enable: visivelSituacao },
                                    { id: 'txtPeriodoEspecializacao', tipo: ControlType.Component, enable: visivelPeriodo },
                                    { id: 'txtAnoConclusaoEspecializacao', tipo: ControlType.Component, enable: visivelAnoConclusao }
                                );

    employer.controles.enableControlesArr(arrControlesEspecializacao);

    //Mostrando/Escondendo as DIV's
    var arrDivsEspecializacao = new Array(
                                { id: 'divLinhaInstituicaoEspecializacao', enable: visivelInstituicao },
                                { id: 'divLinhaTituloCursoEspecializacao', enable: visivelCurso },
    //{ id: 'divLinhaLocalEspecializacao', enable: visivelLocal },
                                { id: 'divCidadeEspecializacao', enable: visivelCidadeEstado },
                                { id: 'divLinhaSituacaoEspecializacao', enable: visivelSituacaoPeriodo },
                                { id: 'divLinhaConclusaoEspecializacao', enable: visivelAnoConclusao }
                           );

    var arrComponentesEspecializacao = new Array(
                                { id: 'lblPeriodoEspecializacao', enable: visivelPeriodo },
                                { id: 'txtPeriodoEspecializacao', enable: visivelPeriodo }
                            );

    Funcoes.MostrarEsconderDivs(arrDivsEspecializacao);
    Funcoes.MostrarEsconderControles(arrComponentesEspecializacao);

    //Habilitando/Desabilitando os validadores

    employer.controles.enableValidatorVal('rfvInstituicaoEspecializacao', obrigatorioInstituicao, true);

    employer.controles.enableValidatorVal('rfvTituloCursoEspecializacao', obrigatorioCurso, true);
    employer.controles.enableValidatorVal('cvTituloCursoEspecializacao', obrigatorioCurso, true);

    //employer.controles.setAttr('rblLocal', 'Obrigatorio', obrigatorioInstituicao);
    employer.controles.enableValidatorVal('cvSituacaoEspecializacao', obrigatorioSituacao, true);
    //employer.controles.setAttr('cvSituacao', 'Obrigatorio', obrigatorioSituacao);

    employer.controles.enableValidatorControleVal('txtPeriodoEspecializacao', 'rfValor', obrigatorioPeriodo, true);
    employer.controles.setAttr('txtPeriodoEspecializacao', 'Obrigatorio', obrigatorioPeriodo);
    employer.controles.enableValidatorControleVal('txtAnoConclusaoEspecializacao', 'rfValor', obrigatorioAnoConclusao, true);
    employer.controles.setAttr('txtAnoConclusaoEspecializacao', 'Obrigatorio', obrigatorioAnoConclusao);
    employer.controles.enableValidatorVal('rfvCidadeEspecializacao', obrigatorioCidadeEstado, true);
    //employer.controles.enableValidatorVal('cvCidadeEspecializacao', obrigatorioCidadeEstado, true);
}

function AjustarParametros(value) {
    value = value.toString();
    switch (value) {
        case '0': //Default
        case '1': //Não é do bne
        case '2': //Não é do bne
        case '3': //Não é do bne
        case '4': //Ensino fundamental Incompleto
        case '5': //Ensino fundamental Completo
            Parametro.ObrigatorioInstituicao = Parametro.VisivelInstituicao = 0;
            Parametro.ObrigatorioCurso = Parametro.VisivelCurso = 0;
            Parametro.ObrigatorioLocal = Parametro.VisivelLocal = 0;
            Parametro.ObrigatorioCidadeEstado = Parametro.VisivelCidadeEstado = 0;
            Parametro.ObrigatorioSituacao = Parametro.VisivelSituacao = 0;
            Parametro.ObrigatorioPeriodo = Parametro.VisivelPeriodo = 0;
            Parametro.ObrigatorioAnoConclusao = Parametro.VisivelAnoConclusao = 0;
            break;
        case '6': // Ensino Médio Incompleto
            Parametro.ObrigatorioInstituicao = 0;
            Parametro.VisivelInstituicao = 0;
            Parametro.ObrigatorioCurso = 0;
            Parametro.VisivelCurso = 0;
            Parametro.ObrigatorioLocal = 0;
            Parametro.VisivelLocal = 0;
            Parametro.ObrigatorioCidadeEstado = 0;
            Parametro.VisivelCidadeEstado = 0;
            Parametro.ObrigatorioSituacao = 1;
            Parametro.VisivelSituacao = 1;
            Parametro.ObrigatorioPeriodo = 0;
            Parametro.VisivelPeriodo = 0;
            Parametro.ObrigatorioAnoConclusao = 0;
            Parametro.VisivelAnoConclusao = 0;
            break;
        case '7': // Ensino Médio Completo
            Parametro.ObrigatorioInstituicao = 0;
            Parametro.VisivelInstituicao = 0;
            Parametro.ObrigatorioCurso = 0;
            Parametro.VisivelCurso = 0;
            Parametro.ObrigatorioLocal = 0;
            Parametro.VisivelLocal = 0;
            Parametro.ObrigatorioCidadeEstado = 0;
            Parametro.VisivelCidadeEstado = 0;
            Parametro.ObrigatorioSituacao = 0;
            Parametro.VisivelSituacao = 0;
            Parametro.ObrigatorioPeriodo = 0;
            Parametro.VisivelPeriodo = 0;
            Parametro.ObrigatorioAnoConclusao = 0;
            Parametro.VisivelAnoConclusao = 1;
            break;
        case '8': // Técnico / Pós-Médio Incompleto
            Parametro.ObrigatorioInstituicao = 1;
            Parametro.VisivelInstituicao = 1;
            Parametro.ObrigatorioCurso = 1;
            Parametro.VisivelCurso = 1;
            Parametro.ObrigatorioLocal = 0;
            Parametro.VisivelLocal = 0;
            Parametro.ObrigatorioCidadeEstado = 0;
            Parametro.VisivelCidadeEstado = 1;
            Parametro.ObrigatorioSituacao = 1;
            Parametro.VisivelSituacao = 1;
            Parametro.ObrigatorioPeriodo = 0;
            Parametro.VisivelPeriodo = 0;
            Parametro.ObrigatorioAnoConclusao = 0;
            Parametro.VisivelAnoConclusao = 0;
            break;
        case '9': // Técnico / Pós-Médio Completo
        case '12': // Tecnólogo Completo
        case '13': // Superior Completo
        case '14': // Pós-Graduação / Especialização
        case '15': // Mestrado
        case '16': // Doutorado
        case '17': // Pós-Doutorado
            Parametro.ObrigatorioInstituicao = 1;
            Parametro.VisivelInstituicao = 1;
            Parametro.ObrigatorioCurso = 1;
            Parametro.VisivelCurso = 1;
            Parametro.ObrigatorioLocal = 0;
            Parametro.VisivelLocal = 0;
            Parametro.ObrigatorioCidadeEstado = 0;
            Parametro.VisivelCidadeEstado = 1;
            Parametro.ObrigatorioSituacao = 0;
            Parametro.VisivelSituacao = 0;
            Parametro.ObrigatorioPeriodo = 0;
            Parametro.VisivelPeriodo = 0;
            Parametro.ObrigatorioAnoConclusao = 0;
            Parametro.VisivelAnoConclusao = 1;
            break;
        case '10': //Tecnólgo Incompleto
        case '11': //Superior Incompleto
            Parametro.ObrigatorioInstituicao = 1;
            Parametro.VisivelInstituicao = 1;
            Parametro.ObrigatorioCurso = 1;
            Parametro.VisivelCurso = 1;
            Parametro.ObrigatorioLocal = 0;
            Parametro.VisivelLocal = 0;
            Parametro.ObrigatorioCidadeEstado = 0;
            Parametro.VisivelCidadeEstado = 1;
            Parametro.ObrigatorioSituacao = 1;
            Parametro.VisivelSituacao = 1;
            Parametro.ObrigatorioPeriodo = 1;
            Parametro.VisivelPeriodo = 1;
            Parametro.ObrigatorioAnoConclusao = 0;
            Parametro.VisivelAnoConclusao = 0;
            break;
    }
}

function InicializarStatus(parametros) {
    Parametro.ObrigatorioInstituicao = parametros.ObrigatorioInstituicao;
    Parametro.VisivelInstituicao = parametros.VisivelInstituicao;
    Parametro.ObrigatorioCurso = parametros.ObrigatorioCurso;
    Parametro.VisivelCurso = parametros.VisivelCurso;
    Parametro.ObrigatorioLocal = parametros.ObrigatorioLocal;
    Parametro.VisivelLocal = parametros.VisivelLocal;
    Parametro.ObrigatorioCidadeEstado = parametros.ObrigatorioCidadeEstado;
    Parametro.VisivelCidadeEstado = parametros.VisivelCidadeEstado;
    Parametro.ObrigatorioSituacao = parametros.ObrigatorioSituacao;
    Parametro.VisivelSituacao = parametros.VisivelSituacao;
    Parametro.ObrigatorioPeriodo = parametros.ObrigatorioPeriodo;
    Parametro.VisivelPeriodo = parametros.VisivelPeriodo;
    Parametro.ObrigatorioAnoConclusao = parametros.ObrigatorioAnoConclusao;
    Parametro.VisivelAnoConclusao = parametros.VisivelAnoConclusao;
}

function AjustarAutoCompleteNivelCurso(value) {
    value = value.toString();
    switch (value) {
        case '8': // Técnico / Pós-Médio Incompleto
        case '9': // Técnico / Pós-Médio Completo
            ContextKeyNivelCurso(NivelCurso.Tecnico);
            break;
        case '10': //Tecnólgo Incompleto
        case '12': // Tecnólogo Completo
            ContextKeyNivelCurso(NivelCurso.Tecnologo);
            break;
        case '13': // Superior Completo
        case '11': //Superior Incompleto
            ContextKeyNivelCurso(NivelCurso.Graduacao);
            break;
        case '14': // Pós-Graduação / Especialização
        case '15': // Mestrado        
            ContextKeyNivelCurso(NivelCurso.PosGraduacao);
            break;
        case '16': // Doutorado
        case '17': // Pós-Doutorado
            ContextKeyNivelCurso(NivelCurso.Doutorado);
            break;
    }
}

function ContextKeyNivelCurso(nivelCurso) {
    /* Somente os cursos de graduação e especialização estão sendo tratados, 
    os complementares sempre serão os mesmos nivel pois não existe uma combo box para mudar o nivel(setados server side)  */

    /* Somente trazer a instituicao de acordo com o nivel escolhido */

    nivelCurso = nivelCurso.toString();
    switch (nivelCurso) {
        case '1': //Técnico
        case '2': //Tecnólogo
        case '3': //Graduação
            AutoCompleteContextKey.IdNivelCurso = nivelCurso;

            //Setando o contextKey do auto complete de instituicao com o nivel do curso 
            $find(AutoCompleteExtenders.instituicao).set_contextKey(AutoCompleteContextKey.IdNivelCurso);

            //Setando o contextKey com  o nivel do curso e id do curso existente 
            if (AutoCompleteContextKey.IdFonte != '')
                $find(AutoCompleteExtenders.curso).set_contextKey(AutoCompleteContextKey.IdNivelCurso + "," + AutoCompleteContextKey.IdFonte);
            else
                $find(AutoCompleteExtenders.curso).set_contextKey(AutoCompleteContextKey.IdNivelCurso);
            break;

        case '4': // Pós graduação
        case '5': // Mestrado        
        case '6': // Doutorado
        case '7': // Pós-Doutorado
            AutoCompleteContextKey.IdNivelCursoEspecializacao = nivelCurso;

            //Setando o contextKey do auto complete de instituicao com o nivel do curso             
            $find(AutoCompleteExtenders.instituicaoEspecializacao).set_contextKey(AutoCompleteContextKey.IdNivelCurso);

            //Setando o contextKey com  o nivel do curso e id do curso existente 
            if (AutoCompleteContextKey.IdFonteCursoEspecializacao != '')
                $find(AutoCompleteExtenders.cursoEspecializacao).set_contextKey(AutoCompleteContextKey.IdNivelCursoEspecializacao + "," + AutoCompleteContextKey.IdFonteCursoEspecializacao);
            else
                $find(AutoCompleteExtenders.cursoEspecializacao).set_contextKey(AutoCompleteContextKey.IdNivelCursoEspecializacao);
            break;
    }
}

function cvTituloCurso_Validate(sender, args) {
    args.IsValid = true;
}

function cvTituloCursoComplementar_Validate(sender, args) {
    args.IsValid = true;
}

function cvCidadeFormacao_Validate(sender, args) {
    var res = ConferirDados.ValidarCidade(args.Value);
    args.IsValid = (res.error == null && res.value);
}

function cvSituacao_Validate(sender, args) {
    var value = employer.util.findControl('ddlSituacao')[0].value;
    if (value == '0')
        args.IsValid = false;
    else
        args.IsValid = true;
}

var Funcoes = {
    MostrarEsconderDivs: function (controles, status) {
        /// <summary>
        /// Habilita/Desabilita uma lista de controles sob-demanda
        /// </summary>
        for (i = 0; i < controles.length; i++) {
            if (controles[i].enable)
                $('#' + controles[i].id).show();
            else
                $('#' + controles[i].id).hide();
        }

        controles.length = 0; // Remove o Array da Memória
    },
    MostrarEsconderControles: function (controles, status) {
        /// <summary>
        /// Habilita/Desabilita uma lista de controles
        /// </summary>
        for (i = 0; i < controles.length; i++) {
            if (controles[i].enable)
                employer.util.findControl(controles[i].id)[0].style.display = '';
            else
                employer.util.findControl(controles[i].id)[0].style.display = 'none';
        }

        controles.length = 0; // Remove o Array da Memória
    }
};

function ValidadorValido(validationGroup, idControle) {
    ValidarPagina(validationGroup);
    for (var siv = 0; siv < Page_Validators.length; siv++) {
        if (typeof Page_Validators[siv].isvalid == 'boolean' && !Page_Validators[siv].isvalid
            && (Page_Validators[siv].id.indexOf(idControle) != -1
            || (typeof (employer.util.findControl(Page_Validators[siv].id).attr('controltovalidate')) != 'undefined'
                    && employer.util.findControl(Page_Validators[siv].id).attr('controltovalidate').indexOf(idControle) != -1))) {
            return false;
        }
    }
    return true;
}

function AbrirCurriculo(url) {
    open(url, '_newtab');
}

Array.prototype.contains = function (k) {
    for (p in this)
        if (this[p] === k)
            return true;
    return false;
}

function searchInvalidValidator(element, validationGroup) {
    /// <summary>
    /// Método que procura o primeiro validador inválido da página e set o focus para o controle validado.
    /// DEPRICATED -> Usuar funções do namespace de cada app.
    /// </summary>
    window.setTimeout(function () {
        ValidarPagina(validationGroup);
        for (var siv = 0; siv < Page_Validators.length; siv++) {
            if (typeof Page_Validators[siv].isvalid == 'boolean' && !Page_Validators[siv].isvalid) {
                var validador = $('#' + Page_Validators[siv].id)

                var controle = null;
                if ($get(Page_Validators[siv].controltovalidate) != null)
                    controle = $get(Page_Validators[siv].controltovalidate).id;

                var divTextBox = validador.parents(".container_textbox");
                var divLabel = divTextBox.prev(".container_label");

                divLabel.css('display', 'none');
                divTextBox.css('display', 'block');

                if (controle != null) {
                    employer.controles.enableControle(controle, true);
                    $get(Page_Validators[siv].controltovalidate).focus();
                }
            }
        }
    }, 75);
}

$('body').click(function (e) {
    var target = $(e.target);
    if (target.hasClass('conteudo') || target.hasClass('div_curriculo_conteudo')) {
        $('.container_textbox').hide();
        $('.container_label').show();
    }
});


function CentralizarObjeto(IdObjeto) {

    $(IdObjeto).css("left", ($(window).width() / 2 - $(IdObjeto).width() / 2));

    $(IdObjeto).css("top", ($(window).height() / 2 - $(IdObjeto).height() / 2));

}

//Formata a modal de CEP
setInterval(
            function () {
                //$("*[id*='ucBuscarCEP_pnlModal']").attr("class", "modal_conteudo");
                //$("*[id*='ucBuscarCEP_pnlModal'] h1").attr("class", "titulo_modal");
                $("*[id*='ucBuscarCEP_MPE_backgroundElement']").attr("class", "modal_fundo");
                $("*[id*='pnlBotoesBuscaCEP']").attr("class", "painel_botoes");
                $("*[id*='bntLocalizar']").attr("class", "botao_padrao");
                $("*[id*='bntCancelar']").attr("class", "botao_padrao");

                $(".painel_gv_resultado_cep").attr("class", "painel_botoes");

                $(".label_campo span").each(
                    function () {
                        var c = $(this).html();
                        $(this).replaceWith("<label class=\"label_principal\">" + c + "</label>");
                    }
                );
                $(".label_campo").css("text-align", "right");
                $(".label_campo").width($(".label_principal").width());

                CentralizarObjeto("#" + $("*[id*='ucBuscarCEP_pnlModal']").attr("id"));
            }
, 500);

function txtAtividadeExercida_KeyUp() {
    contarCaracteres(document.getElementById('cphConteudo_ucConferirDados_txtAtividadeExercida_txtValor').value, 'GraficoQualidade');
}

function contarCaracteres(box, campoRetorno) {
    var conta = box.length;

    if (conta <= 70) {
        document.getElementById(campoRetorno).innerHTML = "<div class='icon icon-fraco icon-size'></br><span class='iconLabel'>FRACO</span></div>";
    }
    else if (conta > 70 && conta <= 140) {
        document.getElementById(campoRetorno).innerHTML = "<div class='icon icon-regular icon-size'></br><span class='iconLabel'>REGULAR</span></div>";
    }
    else {
        document.getElementById(campoRetorno).innerHTML = "<div class='icon icon-otimo icon-size'></br><span class='iconLabel'>ÓTIMO</span></div>";
    }
}

//Validar data de demissão para que não seja menor que a data de Admissão
function ValidarDataDemissao(campoDtaAdmissao, campoDtaDemissao, divAviso) {

    var txtDataAdmissao = document.getElementById(campoDtaAdmissao).value;
    var txtDataDemissao = document.getElementById(campoDtaDemissao).value;

    var dataAdmissao = parseInt(txtDataAdmissao.split("/")[2].toString() + txtDataAdmissao.split("/")[1].toString() + txtDataAdmissao.split("/")[0].toString());
    var dataDemissao = parseInt(txtDataDemissao.split("/")[2].toString() + txtDataDemissao.split("/")[1].toString() + txtDataDemissao.split("/")[0].toString());

    if (dataAdmissao > dataDemissao) {

        $('#' + campoDtaDemissao).addClass('inputError');
        $('#' + divAviso).show();
        $('#cphConteudo_ucConferirDados_btnSalvarExperiencia').attr('disabled', 'disabled');
    }
    else {
        $('#' + campoDtaDemissao).removeClass('inputError');
        $('#' + divAviso).hide();
        $('#cphConteudo_ucConferirDados_btnSalvarExperiencia').removeAttr('disabled');

    }
}