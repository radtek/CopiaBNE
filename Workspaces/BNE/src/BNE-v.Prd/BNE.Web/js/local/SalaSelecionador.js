$(document).ready(
    function () {
        //Ajustando o background
        $("body").addClass("bg_fundo_empresa");
    }
);

	$(document).ready(function () {
	    console.log(getCookie("pesquisafolha"));
	
	    if (getCookieValue("pesquisafolha") != $("#hdfUfP").val()) {
	        var dados = "{'Filial':" + $("#hdfFilial").val() + "}";
	        $.ajax({
	            type: "Post",
	            url: "/ajax.aspx/PesquisaFolha",
	            data: dados,
	            contentType: "application/json; charset=utf-8",
	            dataType: "json",
	            async: true,
	            success: function (retorno) {
	                if (retorno.d.MostraModal) {
	                    for (var i = 0; i < retorno.d.lista.length; i++) {
	                        if (retorno.d.lista[i].IdSistemaFolhaPgto == 6) {
	                            $("#divOutroEsc").append('<div class="col-md-3 step2 folha_option" ><input type="radio" name="folha" value="' + retorno.d.lista[i].IdSistemaFolhaPgto + '"> <h6>' + retorno.d.lista[i].DescricaoSistemaFolhaPgto + '</h6></div>');
	                        }
	                        else {
	                            $("#divOpcoes").append('<div class="col-md-3 step2 folha_option" ><input type="radio" name="folha" value="' + retorno.d.lista[i].IdSistemaFolhaPgto + '"> <h6>' + retorno.d.lista[i].DescricaoSistemaFolhaPgto + '</h6></div>');
	                        }
	                    }
	                    $('#modal_melhoria_footer').hide();
	                    $('.step2').hide();
	                    $('.folha_option_outro').hide();
	                    $('input[type="radio"]').click(function () {
	                        if ($(this).attr('name') == 'folha') {
	                            $('#modal_melhoria_footer').show();
	                        }
	                        else {
	                            $('#modal_melhoria_footer').hide();
	                        }
	                    });

	                    $('input[type="radio"]').click(function () {
	                        if ($(this).attr('value') == '6') {
	                            $('.folha_option_outro').show();
	                        }
	                        else {
	                            $('.folha_option_outro').hide();
	                        }
	                    });

	                    $(".gostep2").click(function () {
	                        $(".step1").hide();
	                        $(".step2").show();
	                        document.getElementById("mdlTitle").innerHTML = "Ok, qual folha de pagamento sua empresa utiliza?"
	                    });

	                    mostrarModal();

	                }

	            }
	        });

	    }
	});

	function validateFolha() {
	    var checkboxs = document.getElementsByName("folha")
	    for (var i = 0; i < checkboxs.length; i++) {
	        if (checkboxs[i].checked) {//6 Outros
	            if (checkboxs[i].value == "6" && $("#outro_txt").val().length <= 0){
	                //exibir aviso
	                document.getElementById("divOutro").className += " has-error";
	                document.getElementsByClassName("validationmsg")[0].style.display = "block";
                    return false
                }
	            salvar(checkboxs[i].value);
	        }
	    }
	    return false;
	}
	function NaoUtiliza() {
	    salvar(0);
	}

	function salvar(checkTrue) {
	    var dados = "{'ufp':" + $("#hdfUfP").val() + ",'folhaPgto':" + checkTrue + ",'outro':'" + $("#outro_txt").val() + "'}";
	    $.ajax({
	        type: "Post",
	        url: "/ajax.aspx/SavePesquisaFolha",
	        data: dados,
	        contentType: "application/json; charset=utf-8",
	        dataType: "json",
	        async: true,
	        success: function (retorno) {
	            if (retorno.d) {
	                fecharModal();
	            }

	        }
	    });



	}

	function mostrarModal() {
	    $('#modal_melhoria').modal({
	        backdrop: 'static',
	        keyboard: true
	    }).show();
	}

	function fecharModal() {
	    $('#modal_melhoria').modal('hide');
	    setCookie('pesquisafolha', $("#hdfUfP").val(), 2020);
	}