<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucLoginModal.ascx.cs" Inherits="BNE.Web.UserControls.Mobile.ucLoginModal" %>
<!-- Modal Login-->
<div class="modal fade" id="loginModal" tabindex="-1" role="dialog" aria-labelledby="loginModalLabel">
    <div class="vertical-alignment-helper">
        <div class="modal-dialog vertical-align-center" role="document">

            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                </div>
                <div class="modal-body row ">
                    <div class="col-xs-12 " id="loginModal-logo">
                        <img src="img/logo-bne-login-pf.png" />
                    </div>
                    <div class="col-xs-12 form-group">
                        <h6 class="informe">Informe seus dados de acesso</h6>
                    </div>
                    <div class="col-xs-12 form-group" id="erroCpf">
                        <input id="ModalLoginCPF" type="tel" class="form-control input-lg" placeholder="CPF" onblur="valida_cpf_login(this.value);" />
                        <span class="help-block" id="msgErroCpf"></span>
                    </div>
                    <div class="col-xs-12 form-group" id="erroDataNascimento">
                        <input id="ModalLoginDataDeNascimento" type="tel" placeholder="Data de Nascimento" class="form-control input-lg" onblur="valida_dta_nascimento_login(this.value);" />
                        <span class="help-block" id="msgErroDtaNasc"></span>
                    </div>
                    <div class="col-xs-12 form-group" id="erroLogin">
                        <span class="help-block" id="msgErroLogin"></span>
                        <button type="button" class="btn btn-lg btn-primary btn-block" onclick="ValidarLogin();">ENTRAR</button>
                    </div>
                    <div class="col-xs-12 form-group">
                        <a href="javascript:cadastroNovo();">
                            <h5 class="link">Cadastre-se já!</h5>
                        </a>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="center">
                        <h6 class="informe">ou conecte-se com</h6>
                    </div>
                    <div class="center">
                        <a href="javascript:BNEFacebook.EfetuarLogin();">
                            <img src="img/facebook-login-icon.png" /></a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
