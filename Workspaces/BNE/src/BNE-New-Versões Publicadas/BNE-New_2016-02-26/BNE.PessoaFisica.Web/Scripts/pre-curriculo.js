function Inicializar() {
    //$('.linkLogin').hide();
    //$('.linkCadastro').hide();
}

//Conta quantos acessos houveram em uma determinada página
function trackEvent(category, action, label) {
    try {
        ga('send', 'event', category, action, label);
    } catch (err) { }
}

function ConfigurarPasso1() {

    $(".linkLogin").show();
    $('.linkCadastro').hide();

    $('.divTempoExp').hide();

    $('#chkSemExperiencia').click(function () {
        if (this.checked) {
            $('.divTempoExp').show();
        }
        else {
            $('.divTempoExp').hide();
        }
    });

    $('#PreCadastroLoginForm').validate();

    bne.components.web.textbox('txtNome', true, bne.components.web.textbox.type.nome);
    bne.components.web.autocomplete.email('txtEmail', false, true);
    bne.components.web.decimal('txtPretensaoSalarial', true, bne.components.web.textbox.type.numero, true);
    bne.components.web.telefone('txtCelular', true, bne.components.web.telefone.type.celular);
    bne.components.web.textbox('txtTempoExperienciaAnos', false, bne.components.web.textbox.type.numero);
    bne.components.web.textbox('txtTempoExperienciaMeses', false, bne.components.web.textbox.type.meses);

    componentHandler.upgradeDom();
}

function ConfigurarPasso2() {
    $(".linkCadastro").show();
    $(".linkLogin").hide();

    $('#PreCadastroLoginForm').validate();
    
    bne.components.web.CPF('txtCPF', true);
    bne.components.web.data('txtDataNascimento', true, bne.components.web.data.type.dataNascimento);

    componentHandler.upgradeDom();
}

function ConfigurarExperiencia() {
    $(".linkCadastro").hide();
    $(".linkLogin").hide();
    $('#PreCadastroLoginForm').validate();

    bne.components.web.textbox('txtNomeEmpresa', true, bne.components.web.textbox.type.normal);
    bne.components.web.autocomplete.funcao('txtFuncaoExercida', true, false);

    bne.components.web.data('txtDataSaida', true, bne.components.web.data.type.dataSaida,true);
    bne.components.web.data('txtDataEntrada', true, bne.components.web.data.type.dataEntrada);
    bne.components.web.decimal('txtUltimoSalario', true, bne.components.web.textbox.type.numero, true);

    $('#chkEmpregoAtual').click(function () {
        if (this.checked) {
            bne.components.web.data('txtDataSaida', false, bne.components.web.data.type.dataSaida, false);
            $('#txtDataSaida-error').hide();
            $('#txtDataSaida').val('');
            $('#txtDataSaida').attr('disabled','disable');

        } else {
            bne.components.web.data('txtDataSaida', true, bne.components.web.data.type.dataSaida, true);
            $('#txtDataSaida-error').html('Obrigatório');
            $('#txtDataSaida-error').show();
            $('#txtDataSaida').removeAttr('disabled');
        }
    });

    $('#chkSemExperiencia').click(function () {
        if (this.checked)
            $('.experiencia').hide();
        else
            $('.experiencia').show();
    });
   
    componentHandler.upgradeDom();
}

function ConfigurarFormacao() {
    $(".linkCadastro").hide();
    $(".linkLogin").hide();

    $('#PreCadastroLoginForm').validate();

    //bne.components.web.dropdownlist('ddlEscolaridade', true);

    $('#ddlEscolaridade').change(function () {
        if($(this).val() > 0 && $(this).val() < 8){
            $('.Instituicao').hide();
            $('.Curso').hide();
            $('.Cidade').hide();

            bne.components.web.autocomplete.cidade('txtNomeCidade', false, false);
            bne.components.web.autocomplete.curso('txtNomeCurso', false, false);
            bne.components.web.autocomplete.instituicaoEnsino('txtNomeInstituicao', false, false);
            bne.components.web.textbox('txtAnoConclusao', false, bne.components.web.textbox.type.ano);

        } else {
            $('.Instituicao').show();
            $('.Curso').show();
            $('.Cidade').show();

            bne.components.web.autocomplete.cidade('txtNomeCidade', false, false);
            bne.components.web.autocomplete.curso('txtNomeCurso', true, false);
            bne.components.web.autocomplete.instituicaoEnsino('txtNomeInstituicao', true, false);
            bne.components.web.textbox('txtAnoConclusao', false, bne.components.web.textbox.type.ano);
        }

        if ($("#ddlEscolaridade option[value='" + $(this).val() + "']").text().indexOf('Completo') > 0)
        {
            $('#lblCursoAtual').hide();
            console.info('Esconder');
        }
        else {
            $('#lblCursoAtual').show();
            console.info('Mostrar');
        }

        componentHandler.upgradeDom();
    });

    componentHandler.upgradeDom();
}
function ConfigurarJaEnviei() {
    $(".linkCadastro").hide();
    $(".linkLogin").hide();
}

