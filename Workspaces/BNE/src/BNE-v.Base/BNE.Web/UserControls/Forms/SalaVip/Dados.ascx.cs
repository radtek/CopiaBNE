using System;
using BNE.BLL;
using BNE.Web.Code;
using PessoaFisicaFoto = BNE.Web.Handlers.PessoaFisicaFoto;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using System.Collections.Generic;
using System.Data.SqlClient;
using Enumeradores = BNE.BLL.Enumeradores;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;


namespace BNE.Web.UserControls.Forms.SalaVip
{
    public partial class Dados : BaseUserControl
    {
        #region Propriedades

        #region IdPessoaFisica - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa.
        /// </summary>
        public int? IdPessoaFisica
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());

                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel1.ToString());
            }
        }
        #endregion

        #region dataAlteracaoCurriculo - Variável 2
        /// <summary>
        /// Propriedade que armazena e recupera a data de alteração do curriculo.
        /// </summary>
        public DateTime? dataAlteracaoCurriculo
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel2.ToString()] != null)
                    return DateTime.Parse(ViewState[Chave.Temporaria.Variavel2.ToString()].ToString());

                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel2.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel2.ToString());
            }
        }
        #endregion

        #region IdExperienciaProfissional - Variável 3
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Experiencia Profissional.
        /// </summary>
        public int? IdExperienciaProfissional
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel3.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel3.ToString()].ToString());

                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel3.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel3.ToString());
            }
        }
        #endregion

        #region DicionarioParametros - Variável 4
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public Dictionary<string, bool> DicionarioParametros
        {
            get
            {
                return (Dictionary<string, bool>)ViewState[Chave.Temporaria.Variavel4.ToString()];
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel4.ToString()] = value;
            }
        }
        #endregion

        #region IdFormacao - Variável 5
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int? IdFormacao
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel5.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel5.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel5.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel5.ToString());
            }
        }
        #endregion

        #region IdEspecializacao - Variável 6
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int? IdFonteFormacao
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel6.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel6.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel6.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel6.ToString());
            }
        }
        #endregion

        #region IdFonteEspecializacao - Variável 7
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int? IdFonteEspecializacao
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel7.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel7.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel7.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel7.ToString());
            }
        }
        #endregion

        #region IdFonteEspecializacao - Variável 8
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public ExperienciaProfissional objExerienciaProfissionalTemp
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel8.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel8.ToString()]) as ExperienciaProfissional;
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel8.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel8.ToString());
            }
        }
        #endregion

        #region IdPerguntaSalaVipHistorico - Variável 9
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pergunta Sala Vip Hsitorico
        /// </summary>
        public int? IdPerguntaSalaVipHistorico
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel9.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel9.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel9.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel9.ToString());
            }
        }
        #endregion
        #endregion

        #region Eventos

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        #endregion

        #region txtTelefoneCelular_OnValorAlteradoFone
        protected void txtTelefoneCelular_OnValorAlteradoFone(object sender, EventArgs e)
        {
            EL.GerenciadorException.GravarExcecao(new Exception("Sala VIP >> txtTelefoneCelular_OnValorAlteradoFone PF: " + base.IdPessoaFisicaLogada.Value));
            ValidacaoCelular();
        }
        #endregion txtTelefoneCelular_OnValorAlteradoFone

        #region UcValidacaoCelular_CodigoValido
        void UcValidacaoCelular_CodigoValido(object sender, EventArgs e)
        {
            base.ExibirMensagem("Código válido.", TipoMensagem.Aviso);
        }
        #endregion UcValidacaoCelular_CodigoValido

        #region UcValidacaoCelular_BeforeVerificarEnviarCodigoCelular
        void UcValidacaoCelular_BeforeVerificarEnviarCodigoCelular(object sender, EventArgs e)
        {
            UcValidacaoCelular.NumeroDDDCelular = txtTelefoneCelular.DDD;
            UcValidacaoCelular.NumeroCelular = txtTelefoneCelular.Fone;
        }
        #endregion UcValidacaoCelular_BeforeVerificarEnviarCodigoCelular

        #region UcValidacaoCelular_CodigoEnviado
        void UcValidacaoCelular_CodigoEnviado(object sender, EventArgs e)
        {
            base.ExibirMensagem("Foi enviado um código para este número. Insira o código no campo código de validação para validar seu celular.", TipoMensagem.Aviso);
        }
        #endregion UcValidacaoCelular_CodigoEnviado

        #region btnSalvarTelefone
        protected void btnSalvarTelefone_Click(object sender, EventArgs e)
        { 
            SalvarCelular(true);
        }
        #endregion

        #region btnSalvarSalario
        protected void btnSalvarSalario_Click(object sender, EventArgs e)
        {
            SalvarSalario(true);
        }
        #endregion

        #region btnCelularSim
        protected void btnCelularSim_Click(object sender, EventArgs e)
        {
            SalvarCelular(false);
        }
        #endregion

        #region btnCelularNao
        protected void btnCelularNao_Click(object sender, EventArgs e)
        {
            pnAtualizarCelular.Visible = true;
            pnPerguntaCelular.Visible = false;
        }
        #endregion

        #region btnEmailSim
        protected void btnEmailSim_Click(object sender, EventArgs e)
        {
            SalvarEmail(false);
        }
        #endregion

        #region btnEmailNao
        protected void btnEmailNao_Click(object sender, EventArgs e)
        {
            pnAtualizarEmail.Visible = true;
            pnPerguntaEmail.Visible = false;
        }
        #endregion

        #region btnSalarioSim
        protected void btnSalarioSim_Click(object sender, EventArgs e)
        {
            SalvarSalario(false);
        }
        #endregion

        #region btnSalarioNao
        protected void btnSalarioNao_Click(object sender, EventArgs e)
        {
            pnAtualizarSalario.Visible = true;
            pnPerguntaSalario.Visible = false;
        }
        #endregion

        #region btnFormacaoSim
        protected void btnFormacaoSim_Click(object sender, EventArgs e)
        {
            SalvarEducacao(false);
        }
        #endregion

        #region btnFormacaoNao
        protected void btnFormacaoNao_Click(object sender, EventArgs e)
        {
            pnPerguntaFormacao.Visible = false;
            pnAtualizarFormacao.Visible = true;

            CarregarFormacao();
        }
        #endregion

        #region btnSalvarExperiencia
        protected void btnSalvarExperiencia_Click(object sender, EventArgs e)
        {
            SalvarExperienciaProfissional(true);
        }
        #endregion

        #region btnSalvarUltimaEmpresa_Fase1

        protected void btnSalvarUltimaEmpresa_Fase1_Click(object sender, EventArgs e)
        {
            pnUltimaEmpresa_Fase1.Visible = false;
            pnUltimaEmpresa_Fase2.Visible = true;

            ExperienciaProfissional expro = ExperienciaProfissional.LoadObject(IdExperienciaProfissional.Value);

            if (expro != null)
            {
                //txtFuncaoExercida1.Text = expro.DescricaoFuncaoExercida;
                txtAtividadeExercida.Valor = expro.DescricaoAtividade;
                pnAviso.Visible = false;

                //salvar cmpos fase 1 na session
                Session["RazaoSocial"] = txtEmpresa1.Text;
                Session["DataAdmissao"] = txtDataAdmissao1.Valor;
                Session["DataDemissao"] = txtDataDemissao1.Valor;
            }
        }

        #endregion

        #region btnPularExperiencia_Click
        protected void btnPularExperiencia_Click(object sender, EventArgs e)
        {
            SalvarExperienciaProfissional(false);
        }
        #endregion

        #region btnSalvarEmail
        protected void btnSalvarEmail_Click(object sender, EventArgs e)
        {
            SalvarEmail(true);
        }

        #endregion

        #region btnSalvarEducacao
        protected void btnSalvarEducacao_Click(object sender, EventArgs e)
        {
            SalvarEducacao(true);
        }
        #endregion

        #region ddlNivel_SelectedIndexChanged
        protected void ddlNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            AjustarParametrosCampos(ddlNivel.SelectedValue);
            AjustarAutoCompleteNivelCurso(ddlNivel.SelectedValue);
            AjustarCamposFormacao();
        }
        #endregion

        #region txtInstituicao_TextChanged
        protected void txtInstituicao_TextChanged(object sender, EventArgs e)
        {
            IdFonteFormacao = null;
            Fonte objFonte;
            if (Fonte.CarregarPorSiglaNome(txtInstituicao.Text, out objFonte))
            {
                if (objFonte.FlagAuditada)
                    IdFonteFormacao = objFonte.IdFonte;
            }
            AjustarAutoCompleteNivelCurso(ddlNivel.SelectedValue);
        }
        #endregion 

        #region ddlNivelEspecializacao_SelectedIndexChanged
        protected void ddlNivelEspecializacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            AjustarParametrosCampos(ddlNivelEspecializacao.SelectedValue);
            AjustarAutoCompleteNivelCurso(ddlNivelEspecializacao.SelectedValue);
            AjustarCamposEspecializacao();
        }
        #endregion

        #region txtInstituicaoEspecializacao_TextChanged
        protected void txtInstituicaoEspecializacao_TextChanged(object sender, EventArgs e)
        {
            IdFonteEspecializacao = null;
            Fonte objFonte;
            if (Fonte.CarregarPorSiglaNome(txtInstituicaoEspecializacao.Text, out objFonte))
            {
                if (objFonte.FlagAuditada)
                    IdFonteEspecializacao = objFonte.IdFonte;
            }
            AjustarAutoCompleteNivelCurso(ddlNivelEspecializacao.SelectedValue);
        }
        #endregion

        #region txtFuncaoExercida1_TextChanged
        protected void txtFuncaoExercida1_TextChanged(object sender, EventArgs e)
        {
            txtSugestaoTarefas1.Text = RecuperarJobFuncao(txtFuncaoExercida1.Text);
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #region txtEmpresa1_TextChanged
        protected void txtEmpresa1_TextChanged(object sender, EventArgs e)
        {
            VerificarObrigatoriedadeCamposExperiencia();
        }
        #endregion

        #endregion

        #region Metodos

        #region Inicializar

        public void Inicializar()
        {
            PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(base.IdPessoaFisicaLogada.Value);
            if (objPessoaFisica != null)
            {
                IdPessoaFisica = objPessoaFisica.IdPessoaFisica;

                CarregarSaudacao();
                lblNomePessoa.Text = objPessoaFisica.PrimeiroNome;
                imgFotoUsuario.ImageUrl = RetornarUrlFoto(objPessoaFisica.NumeroCPF);

                dataAlteracaoCurriculo = objPessoaFisica.DataAlteracao;

                DateTime dtaRespondeuCelular = PerguntaSalaVipHistorico.CarregarPerguntaHistoricoUltimaResposta(objPessoaFisica.IdPessoaFisica, BLL.Enumeradores.PerguntaSalaVip.Celular);
                DateTime dtaRespondeuEmail = PerguntaSalaVipHistorico.CarregarPerguntaHistoricoUltimaResposta(objPessoaFisica.IdPessoaFisica, BLL.Enumeradores.PerguntaSalaVip.Email);
                DateTime dtaRespondeuExp = PerguntaSalaVipHistorico.CarregarPerguntaHistoricoUltimaResposta(objPessoaFisica.IdPessoaFisica, BLL.Enumeradores.PerguntaSalaVip.Experiencia);
                DateTime dtaRespondeuSalario = PerguntaSalaVipHistorico.CarregarPerguntaHistoricoUltimaResposta(objPessoaFisica.IdPessoaFisica, BLL.Enumeradores.PerguntaSalaVip.Salario);
                DateTime dtaRespondeuEdu = PerguntaSalaVipHistorico.CarregarPerguntaHistoricoUltimaResposta(objPessoaFisica.IdPessoaFisica, BLL.Enumeradores.PerguntaSalaVip.Educacao);

                ExibirPerguntaAleatoria(objPessoaFisica, (dtaRespondeuCelular > DateTime.MinValue), (dtaRespondeuEmail > DateTime.MinValue), 
                    (dtaRespondeuExp > DateTime.MinValue), (dtaRespondeuSalario > DateTime.MinValue), (dtaRespondeuEdu > DateTime.MinValue));
            }
            upCabecalho.Update();
        }

        #endregion

        #region RetornarUrlFoto
        protected string RetornarUrlFoto(string strCpf)
        {
            return UIHelper.RetornarUrlFoto(strCpf, PessoaFisicaFoto.OrigemFoto.Local);
        }
        #endregion

        #region CarregarSaudacao

        public void CarregarSaudacao()
        {
            if (DateTime.Now.Hour < 12)
                lblSaudacao.Text = "Bom Dia, ";
            else if (DateTime.Now.Hour < 18)
                lblSaudacao.Text = "Boa Tarde, ";
            else
                lblSaudacao.Text = "Boa Noite, ";
        }

        #endregion

        #region Salvar SalvarEducacao

        private void SalvarEducacao(bool EducacaoAlterada)
        {
            PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);
            Formacao objFormacao = IdFormacao.HasValue ? Formacao.LoadObject(IdFormacao.Value) : new Formacao();

            objFormacao.PessoaFisica = objPessoaFisica;
            objFormacao.Escolaridade = new BNE.BLL.Escolaridade(objFormacao.Escolaridade.IdEscolaridade);

            if (objFormacao != null)
            {
                if (!EducacaoAlterada)
                {
                    switch (objFormacao.Escolaridade.IdEscolaridade)
                    {
                        case 6:
                            objFormacao.Escolaridade.IdEscolaridade = Convert.ToInt32(Enumeradores.Escolaridade.EnsinoMedioCompleto);
                            break;
                        case 8:
                            objFormacao.Escolaridade.IdEscolaridade = Convert.ToInt32(Enumeradores.Escolaridade.TecnicoPosMedioCompleto);
                            break;
                        case 10:
                            objFormacao.Escolaridade.IdEscolaridade = Convert.ToInt32(Enumeradores.Escolaridade.TecnologoCompleto);
                            break;
                        case 11:
                            objFormacao.Escolaridade.IdEscolaridade = Convert.ToInt32(Enumeradores.Escolaridade.SuperiorCompleto);
                            break;
                    }

                    //Salvar o hsitorico de alteração do candidato.
                    PerguntaSalaVipHistorico.SalvarHistoricoPerguntaResposta(IdPerguntaSalaVipHistorico, "", !EducacaoAlterada);
                }
                else
                {
                    //Salvar o hsitorico de alteração do candidato.
                    string strAlteracao = string.Format("idFormacao:{0},Situacao:{1},DescricaoFonte:{2},DescricaoCurso:{3},AnoConclusao:{4}", objFormacao.IdFormacao.ToString(),
                        objFormacao.SituacaoFormacao, objFormacao.DescricaoFonte, objFormacao.DescricaoCurso, objFormacao.AnoConclusao);

                    PerguntaSalaVipHistorico.SalvarHistoricoPerguntaResposta(IdPerguntaSalaVipHistorico, strAlteracao, !EducacaoAlterada);

                    objFormacao.Escolaridade.IdEscolaridade = Convert.ToInt32(ddlNivel.SelectedValue);
                    objFormacao.SituacaoFormacao.IdSituacaoFormacao = Convert.ToInt16(ddlSituacao.SelectedValue);
                    objFormacao.DescricaoFonte = txtInstituicao.Text;
                    objFormacao.DescricaoCurso = txtTituloCurso.Text;
                    objFormacao.AnoConclusao = Convert.ToInt16(txtAnoConclusao.Valor);
                }
               
                objFormacao.SalvarFormacao(objFormacao);

                pnEducacao.Visible = false;
                ltAvisoSucesso.Text = (EducacaoAlterada ? "Formação atualizada com sucesso!" : "Obrigado por confirmar sua formação!");
                pnAviso.Visible = true;

                System.Web.UI.ScriptManager.RegisterStartupScript(pnPerguntaAleatoria, pnPerguntaAleatoria.GetType(),
                    "OcultarPanelPerguntaFormacao", "setTimeout(function() {$('#cphConteudo_ucDados_pnAviso').fadeOut('slow');}, 3000);", true);

                ExibirPerguntaAleatoria(objPessoaFisica, true, true, true, true,true);
            }
        }
        #endregion

        #region Salvar Salario

        private void SalvarSalario(bool salarioAlterado)
        {
            PessoaFisica objPessoaFisica = null;
            Curriculo objCurriculo = null;

            if (IdPessoaFisica.HasValue)
            {
                objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);
                
                Curriculo.CarregarPorPessoaFisica(IdPessoaFisica.Value,out objCurriculo);

                if (salarioAlterado)
                {
                    #region Parametros
                    var parms = new List<Enumeradores.Parametro>
                    {
                        Enumeradores.Parametro.SalarioMinimoNacional
                    };
                    var valores = Parametro.ListarParametros(parms);
                    #endregion

                    decimal salarioMinimo = Convert.ToDecimal(valores[Enumeradores.Parametro.SalarioMinimoNacional]);

                    if (txtPretensaoSalarial.Valor >= salarioMinimo)
                    {
                        //Salvar o hsitorico de alteração do candidato.
                        PerguntaSalaVipHistorico.SalvarHistoricoPerguntaResposta(IdPerguntaSalaVipHistorico, objCurriculo.ValorPretensaoSalarial.ToString(), !salarioAlterado);

                        objCurriculo.ValorPretensaoSalarial = txtPretensaoSalarial.Valor;
                    }
                    else
                    {
                        faixa_salarial.InnerHtml = string.Format("A sua pretenção deve ser maior que R$ {0}", salarioMinimo);
                        faixa_salarial.Visible = true;
                        return;
                    }
                }
                else
                {
                    //Salvar o hsitorico de alteração do candidato.
                    PerguntaSalaVipHistorico.SalvarHistoricoPerguntaResposta(IdPerguntaSalaVipHistorico, "", !salarioAlterado);
                    objCurriculo.ValorPretensaoSalarial = Convert.ToDecimal(hdfValorSalario.Value);
                }

                objCurriculo.DataAtualizacao = DateTime.Now;

                objCurriculo.SalvarCurriculoPretencaoSalarial(objCurriculo);
            }

            pnSalario.Visible = false;
            ltAvisoSucesso.Text = (salarioAlterado ? "Pretenção atualizada com sucesso!" : "Obrigado por confirmar sua pretenção!");
            pnAviso.Visible = true;

            System.Web.UI.ScriptManager.RegisterStartupScript(pnPerguntaAleatoria, pnPerguntaAleatoria.GetType(),
                "OcultarPanelPerguntaSalario", "setTimeout(function() {$('#cphConteudo_ucDados_pnAviso').fadeOut('slow');}, 3000);", true);

            ExibirPerguntaAleatoria(objPessoaFisica, true, true, true, true, false);
        }

        #endregion

        #region Salvar Email

        private void SalvarEmail(bool emailAlterado)
        {
            PessoaFisica objPessoaFisica = null;

            if (IdPessoaFisica.HasValue)
            {
                objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);

                if (emailAlterado)
                {
                    //Salvar o hsitorico de alteração do candidato.
                    PerguntaSalaVipHistorico.SalvarHistoricoPerguntaResposta(IdPerguntaSalaVipHistorico, objPessoaFisica.EmailPessoa, !emailAlterado);

                    objPessoaFisica.EmailPessoa = txtEmail.Text;
                    objPessoaFisica.FlagEmailConfirmado = false;
                    objPessoaFisica.EmailSituacaoConfirmacao = BLL.EmailSituacao.LoadObject(Convert.ToInt32(Enumeradores.EmailSituacao.NaoConfirmado));
                    objPessoaFisica.EmailSituacaoValidacao = BLL.EmailSituacao.LoadObject(Convert.ToInt32(Enumeradores.EmailSituacao.NaoValidado));

                    //Enviar Carta de Validação de E-mail
                    EnviarCartaValidacaoEmail(objPessoaFisica);
                }
                else
                {
                    //Salvar o hsitorico de alteração do candidato.
                    PerguntaSalaVipHistorico.SalvarHistoricoPerguntaResposta(IdPerguntaSalaVipHistorico, "", !emailAlterado);

                    objPessoaFisica.EmailPessoa = hdfEmail.Value;

                    if(!objPessoaFisica.FlagEmailConfirmado)
                        EnviarCartaValidacaoEmail(objPessoaFisica);
                }

                objPessoaFisica.SalvarPessoaFisica(objPessoaFisica);
            }

            pnEmail.Visible = false;
            ltAvisoSucesso.Text = (emailAlterado?"E-mail atualizado com sucesso!":"obrigado por confirmar o e-mail!");
            pnAviso.Visible = true;

            System.Web.UI.ScriptManager.RegisterStartupScript(pnPerguntaAleatoria, pnPerguntaAleatoria.GetType(),
                "OcultarPanelPerguntaEmail", "setTimeout(function() {$('#cphConteudo_ucDados_pnAviso').fadeOut('slow');}, 3000);", true);

            ExibirPerguntaAleatoria(objPessoaFisica, true, true, false, false, false);
        }

        #endregion

        #region SalvarExperienciaProfissional

        public void SalvarExperienciaProfissional(bool empresaAlterada)
        {
            PessoaFisica objPessoaFisica = null;

            if (IdPessoaFisica.HasValue)
            {
                objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);

                if (empresaAlterada)
                {
                    ExperienciaProfissional objExperienciaProfissional = ExperienciaProfissional.LoadObject(IdExperienciaProfissional.Value);

                    //Salvar o hsitorico de alteração do candidato.
                    string strAlteracao = string.Format("RazaoSocial:{0},DataAdmissao:{1},DataDemissao:{2},Funcao:{3},DescricaoAtividades:{4}", objExperienciaProfissional.RazaoSocial,
                        objExperienciaProfissional.DataAdmissao, objExperienciaProfissional.DataDemissao, txtFuncaoExercida1.Text, objExperienciaProfissional.DescricaoAtividade);

                    PerguntaSalaVipHistorico.SalvarHistoricoPerguntaResposta(IdPerguntaSalaVipHistorico, strAlteracao, !empresaAlterada);

                    objExperienciaProfissional.RazaoSocial = Session["RazaoSocial"].ToString();
                    objExperienciaProfissional.DataAdmissao = Convert.ToDateTime(Session["DataAdmissao"].ToString());
                    objExperienciaProfissional.DataDemissao = (Session["DataDemissao"].ToString() != "" ? Convert.ToDateTime(Session["DataDemissao"].ToString()) : (DateTime?)null);

                    objExperienciaProfissional.DescricaoAtividade = txtAtividadeExercida.Valor;

                    Funcao objFuncao;
                    if (Funcao.CarregarPorDescricao(txtFuncaoExercida1.Text, out objFuncao))
                    {
                        objExperienciaProfissional.Funcao = objFuncao;
                        objExperienciaProfissional.DescricaoFuncaoExercida = String.Empty;
                    }

                    objExperienciaProfissional.SalvarExperienciaProfissional(objExperienciaProfissional);

                    ltAvisoSucesso.Text = "Atividades atualizadas com sucesso!";
                    pnAviso.Visible = true;

                    System.Web.UI.ScriptManager.RegisterStartupScript(pnPerguntaAleatoria, pnPerguntaAleatoria.GetType(),
                        "OcultarPanelPerguntaEx", "setTimeout(function() {$('#cphConteudo_ucDados_pnAviso').fadeOut('slow');}, 3000);", true);
                }
                else
                {
                    //Salvar o hsitorico de alteração do candidato.
                    PerguntaSalaVipHistorico.SalvarHistoricoPerguntaResposta(IdPerguntaSalaVipHistorico, "", !empresaAlterada);
                }

                pnUltimaEmpresa.Visible = false;
                ExibirPerguntaAleatoria(objPessoaFisica, true, true, true, false, false);
            }
        }
        #endregion

        #region Salvar Celular

        private void SalvarCelular(bool numeroAlterado) 
        {
            PessoaFisica objPessoaFisica =null;

            if (IdPessoaFisica.HasValue)
            {
                objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);

                if (numeroAlterado)
                {
                    //Salvar o hsitorico de alteração do candidato.
                    PerguntaSalaVipHistorico.SalvarHistoricoPerguntaResposta(IdPerguntaSalaVipHistorico, objPessoaFisica.NumeroDDDCelular + " " + objPessoaFisica.NumeroCelular, !numeroAlterado);
               
                    objPessoaFisica.NumeroDDDCelular = txtTelefoneCelular.DDD;
                    objPessoaFisica.NumeroCelular = txtTelefoneCelular.Fone;
                }
                else
                {
                    //Salvar o hsitorico de alteração do candidato.
                    PerguntaSalaVipHistorico.SalvarHistoricoPerguntaResposta(IdPerguntaSalaVipHistorico, "", !numeroAlterado);

                    objPessoaFisica.NumeroDDDCelular = hdfTelefoneDDD.Value;
                    objPessoaFisica.NumeroCelular = hdfTelefoneNumero.Value;
                }

                objPessoaFisica.SalvarPessoaFisica(objPessoaFisica);
            }

            ltAvisoSucesso.Text = (numeroAlterado ? "Número de celular atualizado com sucesso!" : "Obrigado por confirmaro celular!");
            pnAviso.Visible = true;

            pnCelular.Visible = false;
            
            System.Web.UI.ScriptManager.RegisterStartupScript(pnPerguntaAleatoria, pnPerguntaAleatoria.GetType(),
                "OcultarPanelPergunta", "setTimeout(function() {$('#cphConteudo_ucDados_pnAviso').fadeOut('slow');}, 3000);", true);

            ExibirPerguntaAleatoria(objPessoaFisica, true, false, false, false, false);
        }

        #endregion

        #region Enviar Carta de validação de E-mail

        private void EnviarCartaValidacaoEmail(PessoaFisica objPessoaFisica)
        {
            BLL.UsuarioFilialPerfil objUsuarioFilialPerfil;
            UsuarioFilialPerfil.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objUsuarioFilialPerfil);
            BLL.Curriculo objCurriculo;
            Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo);

            #region Carta de Validação de E-mail

            string emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);
            string urlSite = string.Concat("http://", Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLAmbiente));

            var carta = CartaEmail.RecuperarCarta(Enumeradores.CartaEmail.ValidacaoEmail);

            string assuntoValidacaoEmail = carta.Assunto;
            string templateValidacaoEmail = carta.Conteudo;

            var parametrosValidacaoEmail = new
            {
                Link = string.Format("{0}/{1}?codigo={2}", urlSite, Rota.RecuperarURLRota(Enumeradores.RouteCollection.ConfirmacaoEmail), objPessoaFisica.ValidacaoEmailGerarCodigo(null)),
                NomeCandidato = objPessoaFisica.PrimeiroNome
            };
            string mensagemValidacaoEmail = parametrosValidacaoEmail.ToString(templateValidacaoEmail);

            MensagemCS.SalvarEmail(objCurriculo, null, objUsuarioFilialPerfil, null, assuntoValidacaoEmail, mensagemValidacaoEmail, emailRemetente, objPessoaFisica.EmailPessoa, null, null, null);

            #endregion
        }

        #endregion

        #region Exibir pergunta aleatoria

        /// <summary>
        /// Checar qual pergunta deve ser exibida
        /// </summary>
        private void ExibirPerguntaAleatoria(PessoaFisica objPessoaFisica, bool TelefoneAlterado, bool emailAlterado, bool empresaAlterada, bool salarioAlterado, bool educacaiAlterado)
        {
            int totalDias = (DateTime.Now - dataAlteracaoCurriculo.Value).Days;
            int NumeroDiasUltimaResposta = 0;
            DateTime dtaUltimaAlteracao = DateTime.MinValue;

            if (totalDias >= 30 && !TelefoneAlterado)
            {
                PreencherCamposTelefone(objPessoaFisica);
            }
            else if (totalDias >= 30 && !emailAlterado)
            {
                PreencherCamposEmail(objPessoaFisica);
            }
            else if (totalDias >= 60 && !empresaAlterada)
            {
                //carregar ultima empresa do candidato
                CarregarUltimaExperiencialProfissional(objPessoaFisica);
            }
            else if (totalDias >= 60 && !salarioAlterado)
            {
                //exibir panel Salário
                PreencherCamposSalario(objPessoaFisica);
            }
            else if (totalDias >= 90 && !educacaiAlterado)
            {
                //exibir panel educação
                CarregarCamposEducacao(objPessoaFisica);
            }
            else
            {
                pnPerguntaAleatoria.Visible = false;
            }
        }
        #endregion

        #region CarregarFormacao()
        public void CarregarFormacao()
        {
            pnDadosFormacao.Visible = true;

            Formacao objFormacao = Formacao.LoadObject(IdFormacao.Value);

            if (objFormacao != null)
            {
                CarregarNivelEscolaridade();
                UIHelper.CarregarDropDownList(ddlSituacao, SituacaoFormacao.Listar(), new ListItem("Selecione", "0"));
                UIHelper.CarregarDropDownList(ddlSituacaoEspecializacao, SituacaoFormacao.Listar(), new ListItem("Selecione", "0"));

                //preencher nivel
                ddlNivel.SelectedValue = objFormacao.Escolaridade.IdEscolaridade.ToString();
                ddlSituacao.SelectedValue = objFormacao.SituacaoFormacao.IdSituacaoFormacao.ToString();
                txtAnoConclusao.Valor = objFormacao.AnoConclusao.ToString();
                txtInstituicao.Text = objFormacao.DescricaoFonte;
                txtCidade.Text = objFormacao.Cidade.NomeCidade;
                txtTituloCurso.Text = objFormacao.DescricaoCurso;
                txtPeriodo.Valor = objFormacao.NumeroPeriodo.ToString();
                txtCidade.Text = objFormacao.Cidade.NomeCidade;
            }
        }
        #endregion

        #region CarregarCamposEducacao
        public void CarregarCamposEducacao(PessoaFisica objPessoaFisica)
        {
            IDataReader reader = null;
            
            reader = Formacao.RetornarUltimaFormacaoCandidato(objPessoaFisica.IdPessoaFisica);

            if (reader.Read())
            {
                if (reader[2].ToString().Length > 0 && reader[3].ToString().Length > 0)
                {
                    IdFormacao = int.Parse(reader[0].ToString());
                    ltDescricaoPergunta.Text = string.Format("<p class=\"pNomeEmpresa\">Sua formação na {0} em {1} foi concluída?</p>", reader[2], reader[3]);
                    pnEducacao.Visible = true;
                    pnFormacao.Visible = true;
                }
                else
                {
                    ExibirPerguntaAleatoria(objPessoaFisica, true, true, true, true, true);
                }

                IdPerguntaSalaVipHistorico = PerguntaSalaVipHistorico.SalvarHistoricoPerguntaExibicao(objPessoaFisica.IdPessoaFisica, BLL.Enumeradores.PerguntaSalaVip.Educacao);
            }
            else
            {
                ExibirPerguntaAleatoria(objPessoaFisica, true, true, true, true, true);
            }
        }

        #endregion

        #region Preencer campos salario
        public void PreencherCamposSalario(PessoaFisica objPessoaFisica)
        {
            //exibir panel
            pnSalario.Visible = true;

            #region Parametros
            var parms = new List<Enumeradores.Parametro>
                {
                    Enumeradores.Parametro.SalarioMinimoNacional
                };
            var valores = Parametro.ListarParametros(parms);
            #endregion

            Curriculo objCurriculo;
            Curriculo.CarregarPorPessoaFisica(null,objPessoaFisica.IdPessoaFisica, out objCurriculo);

            //checar se o valor é nulo.
            if (!string.IsNullOrEmpty(objCurriculo.ValorPretensaoSalarial.ToString()))
            {
                hdfValorSalario.Value = objCurriculo.ValorPretensaoSalarial.ToString();

                lblSalario.Text = objCurriculo.ValorPretensaoSalarial.Value.ToString();
            }
            else
            {
                pnPerguntaSalario.Visible = false;
                pnAtualizarSalario.Visible = true;
            }

            IdPerguntaSalaVipHistorico = PerguntaSalaVipHistorico.SalvarHistoricoPerguntaExibicao(objPessoaFisica.IdPessoaFisica, BLL.Enumeradores.PerguntaSalaVip.Salario);

        }
        #endregion

        #region Preencer campos telefone
        public void PreencherCamposTelefone(PessoaFisica objPessoaFisica)
        {
            UcValidacaoCelular.CodigoValido += UcValidacaoCelular_CodigoValido;
            UcValidacaoCelular.BeforeVerificarEnviarCodigoCelular += UcValidacaoCelular_BeforeVerificarEnviarCodigoCelular;
            UcValidacaoCelular.CodigoEnviado += UcValidacaoCelular_CodigoEnviado;

            //exibir panel celular
            pnCelular.Visible = true;
            pnEmail.Visible = false;

            //checar se o telefone é nulo.
            if (!string.IsNullOrEmpty(objPessoaFisica.NumeroCelular))
            {
                hdfTelefoneDDD.Value = objPessoaFisica.NumeroDDDCelular.ToString();
                hdfTelefoneNumero.Value = objPessoaFisica.NumeroCelular.ToString();

                lbCelular.Text = string.Format("({0}) {1}", objPessoaFisica.NumeroDDDCelular, objPessoaFisica.NumeroCelular);
            }
            else
            {
                pnPerguntaCelular.Visible = false;
                pnAtualizarCelular.Visible = true;
            }

            //gravar historico de exibição da pergunta
            IdPerguntaSalaVipHistorico =  PerguntaSalaVipHistorico.SalvarHistoricoPerguntaExibicao(objPessoaFisica.IdPessoaFisica, BLL.Enumeradores.PerguntaSalaVip.Celular);
        }
        #endregion

        #region Preencher Campos Email
        private void PreencherCamposEmail(PessoaFisica objPessoaFisica)
        {
            //exibir panel email
                pnEmail.Visible = true;
                pnCelular.Visible = false;

                pnPerguntaEmail.Visible = true;
                pnAtualizarEmail.Visible = false;

                hdfEmail.Value = objPessoaFisica.EmailPessoa;
                lblEmail.Text = objPessoaFisica.EmailPessoa;

                revEmail.ValidationExpression = Configuracao.regexEmail;

                //gravar historico de exibição da pergunta
                IdPerguntaSalaVipHistorico = PerguntaSalaVipHistorico.SalvarHistoricoPerguntaExibicao(objPessoaFisica.IdPessoaFisica, BLL.Enumeradores.PerguntaSalaVip.Email);
                }

        #endregion

        #region Carregar última experiência profissional

        public void CarregarUltimaExperiencialProfissional(PessoaFisica objPessoaFisica)
        {
            List<ExperienciaProfissional> lstExperiencias = objPessoaFisica.RecuperarUltimaExperienciaProfissional(null, 1);

            if (lstExperiencias.Count > 0)
            {
                foreach (var objExperiencia in lstExperiencias)
                {
                    IdExperienciaProfissional = objExperiencia.IdExperienciaProfissional;
                    txtEmpresa1.Text = objExperiencia.RazaoSocial;
                    ltNomeEmpresa.Text = objExperiencia.RazaoSocial;
                    txtDataAdmissao1.Valor = objExperiencia.DataAdmissao.ToString("dd/MM/yyyy");
                    txtDataDemissao1.Valor = (objExperiencia.DataDemissao != null ? objExperiencia.DataDemissao.Value.ToString("dd/MM/yyyy") : "");
                    txtAtividadeExercida.Valor = objExperiencia.DescricaoAtividade;

                    if (objExperiencia.Funcao != null)
                    {
                        objExperiencia.Funcao.CompleteObject();
                        txtFuncaoExercida1.Text = objExperiencia.Funcao.DescricaoFuncao;
                        txtSugestaoTarefas1.Text = objExperiencia.Funcao.DescricaoJob;
                    }
                    else
                        txtFuncaoExercida1.Text = objExperiencia.DescricaoFuncaoExercida;

                    //Session.Add("Experiencia", objExperiencia);
                }

                var dataMinima = new DateTime(1900, 01, 01);
                var dataMaxima = new DateTime(3000, 12, 31);

                txtDataAdmissao1.DataMinima = dataMinima;
                txtDataDemissao1.DataMinima = dataMinima;
                txtDataAdmissao1.DataMaxima = dataMaxima;
                txtDataDemissao1.DataMaxima = dataMaxima;

                //exibir panel ultima empresas
                pnUltimaEmpresa.Visible = true;
                pnUltimaEmpresa_Fase1.Visible = true;

                IdPerguntaSalaVipHistorico = PerguntaSalaVipHistorico.SalvarHistoricoPerguntaExibicao(objPessoaFisica.IdPessoaFisica, BLL.Enumeradores.PerguntaSalaVip.Experiencia);
            }
            else 
            {
                //vai para a proxima pergunta
                ExibirPerguntaAleatoria(objPessoaFisica, true, true, true, false, false);
            }
        }
        #endregion

        #region ValidacaoCelular
        public void ValidacaoCelular()
        {
            /*
            if (!string.IsNullOrWhiteSpace(txtTelefoneCelular.DDD) && !string.IsNullOrWhiteSpace(txtTelefoneCelular.Fone))
            {
                DateTime? dataEnvio;
                PessoaFisica.ValidacaoCelularCodigoEnviado(txtTelefoneCelular.DDD, txtTelefoneCelular.Fone, out dataEnvio);
                if (dataEnvio.HasValue)
                {
                    base.ExibirMensagem(string.Format("Foi enviado um código para este número no dia {0}. Insira o código no campo código de validação para validar seu celular.", dataEnvio), TipoMensagem.Aviso);
                }
                else
                {
                    PessoaFisica.ValidacaoCelularEnviarCodigo(txtTelefoneCelular.DDD, txtTelefoneCelular.Fone);
                    base.ExibirMensagem("Foi enviado um código para este número. Insira o código no campo código de validação para validar seu celular.", TipoMensagem.Aviso);
                }
            }

            pnlValidacaoCelular.Visible = (!string.IsNullOrWhiteSpace(txtTelefoneCelular.DDD) && !string.IsNullOrWhiteSpace(txtTelefoneCelular.Fone));

            upValidacaoCelular.Update();
             * */
        }
        #endregion

        #region CarregarNivelEscolaridade
        /// <summary>
        /// Carrega a CarregarNivelEscolaridade
        /// </summary>
        public void CarregarNivelEscolaridade()
        {
            AjustarListaNivelGraducao();

            UIHelper.CarregarDropDownList(ddlNivelEspecializacao, Escolaridade.ListaNivelEducacao(IdPessoaFisica.Value, false), "Idf_Escolaridade", "Des_BNE", new ListItem("Selecione", "0"));
            //upNivelEspecializacao.Update();
        }
        #endregion

        #region CarregarGridFormacao
        /// <summary>
        /// Carrega a Grid de cursos
        /// </summary>
        public void CarregarGridComplementar()
        {
            //UIHelper.CarregarGridView(gvComplementar, Formacao.ListarFormacao(IdPessoaFisica.Value, false, false, true));
        }
        #endregion

        #region AjustarListaNivelGraducao
        private void AjustarListaNivelGraducao()
        {
            DataTable dt = Formacao.ListarFormacaoMenosCursosComplementares(IdPessoaFisica.Value);
            bool ensinoFundamentalIncompleto, outrasEscolaridades;
            bool alfabetizado = ensinoFundamentalIncompleto = outrasEscolaridades = false;
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToInt32(dr["Idf_Escolaridade"]).Equals((int)Enumeradores.Escolaridade.EnsinoFundamentalIncompleto))
                    ensinoFundamentalIncompleto = true;
                else
                    outrasEscolaridades = true;
            }

            if ((alfabetizado || ensinoFundamentalIncompleto) && !outrasEscolaridades)
                ddlNivel.Enabled = false;
            else
                ddlNivel.Enabled = true;

            UIHelper.CarregarDropDownList(ddlNivel, Escolaridade.ListaNivelEducacao(IdPessoaFisica.Value, true), "Idf_Escolaridade", "Des_BNE", new ListItem("Selecione", "0"));
            ddlNivel.SelectedIndex = 0;
            //upNivel.Update();
        }
        #endregion

        #region AjustarParametrosCampos
        private void AjustarParametrosCampos(string nivel)
        {
            DicionarioParametros = new Dictionary<string, bool>();

            switch (nivel)
            {
                case "0": //Default
                case "1": //Não é do bne
                case "2": //Não é do bne
                case "3": //Não é do bne
                case "4": //Ensino fundamental Incompleto
                case "5": //Ensino fundamental Completo
                    DicionarioParametros.Add("ObrigatorioInstituicao", false);
                    DicionarioParametros.Add("VisivelInstituicao", false);
                    DicionarioParametros.Add("ObrigatorioCurso", false);
                    DicionarioParametros.Add("VisivelCurso", false);
                    DicionarioParametros.Add("ObrigatorioLocal", false);
                    DicionarioParametros.Add("VisivelLocal", false);
                    DicionarioParametros.Add("ObrigatorioCidadeEstado", false);
                    DicionarioParametros.Add("VisivelCidadeEstado", false);
                    DicionarioParametros.Add("ObrigatorioSituacao", false);
                    DicionarioParametros.Add("VisivelSituacao", false);
                    DicionarioParametros.Add("ObrigatorioPeriodo", false);
                    DicionarioParametros.Add("VisivelPeriodo", false);
                    DicionarioParametros.Add("ObrigatorioAnoConclusao", false);
                    DicionarioParametros.Add("VisivelAnoConclusao", false);
                    break;
                case "6": // Ensino Médio Incompleto
                    DicionarioParametros.Add("ObrigatorioInstituicao", false);
                    DicionarioParametros.Add("VisivelInstituicao", false);
                    DicionarioParametros.Add("ObrigatorioCurso", false);
                    DicionarioParametros.Add("VisivelCurso", false);
                    DicionarioParametros.Add("ObrigatorioLocal", false);
                    DicionarioParametros.Add("VisivelLocal", false);
                    DicionarioParametros.Add("ObrigatorioCidadeEstado", false);
                    DicionarioParametros.Add("VisivelCidadeEstado", false);
                    DicionarioParametros.Add("ObrigatorioSituacao", true);
                    DicionarioParametros.Add("VisivelSituacao", true);
                    DicionarioParametros.Add("ObrigatorioPeriodo", false);
                    DicionarioParametros.Add("VisivelPeriodo", false);
                    DicionarioParametros.Add("ObrigatorioAnoConclusao", false);
                    DicionarioParametros.Add("VisivelAnoConclusao", false);
                    break;
                case "7": // Ensino Médio Completo
                    DicionarioParametros.Add("ObrigatorioInstituicao", false);
                    DicionarioParametros.Add("VisivelInstituicao", false);
                    DicionarioParametros.Add("ObrigatorioCurso", false);
                    DicionarioParametros.Add("VisivelCurso", false);
                    DicionarioParametros.Add("ObrigatorioLocal", false);
                    DicionarioParametros.Add("VisivelLocal", false);
                    DicionarioParametros.Add("ObrigatorioCidadeEstado", false);
                    DicionarioParametros.Add("VisivelCidadeEstado", false);
                    DicionarioParametros.Add("ObrigatorioSituacao", false);
                    DicionarioParametros.Add("VisivelSituacao", false);
                    DicionarioParametros.Add("ObrigatorioPeriodo", false);
                    DicionarioParametros.Add("VisivelPeriodo", false);
                    DicionarioParametros.Add("ObrigatorioAnoConclusao", true);
                    DicionarioParametros.Add("VisivelAnoConclusao", true);
                    break;
                case "8": // Técnico / Pós-Médio Incompleto
                    DicionarioParametros.Add("ObrigatorioInstituicao", true);
                    DicionarioParametros.Add("VisivelInstituicao", true);
                    DicionarioParametros.Add("ObrigatorioCurso", true);
                    DicionarioParametros.Add("VisivelCurso", true);
                    DicionarioParametros.Add("ObrigatorioLocal", false);
                    DicionarioParametros.Add("VisivelLocal", false);
                    DicionarioParametros.Add("ObrigatorioCidadeEstado", false);
                    DicionarioParametros.Add("VisivelCidadeEstado", true);
                    DicionarioParametros.Add("ObrigatorioSituacao", true);
                    DicionarioParametros.Add("VisivelSituacao", true);
                    DicionarioParametros.Add("ObrigatorioPeriodo", false);
                    DicionarioParametros.Add("VisivelPeriodo", false);
                    DicionarioParametros.Add("ObrigatorioAnoConclusao", false);
                    DicionarioParametros.Add("VisivelAnoConclusao", false);
                    break;
                case "9": // Técnico / Pós-Médio Completo
                case "12": // Tecnólogo Completo
                case "13": // Superior Completo
                    DicionarioParametros.Add("ObrigatorioInstituicao", true);
                    DicionarioParametros.Add("VisivelInstituicao", true);
                    DicionarioParametros.Add("ObrigatorioCurso", true);
                    DicionarioParametros.Add("VisivelCurso", true);
                    DicionarioParametros.Add("ObrigatorioLocal", false);
                    DicionarioParametros.Add("VisivelLocal", false);
                    DicionarioParametros.Add("ObrigatorioCidadeEstado", false);
                    DicionarioParametros.Add("VisivelCidadeEstado", true);
                    DicionarioParametros.Add("ObrigatorioSituacao", false);
                    DicionarioParametros.Add("VisivelSituacao", false);
                    DicionarioParametros.Add("ObrigatorioPeriodo", false);
                    DicionarioParametros.Add("VisivelPeriodo", false);
                    DicionarioParametros.Add("ObrigatorioAnoConclusao", true);
                    DicionarioParametros.Add("VisivelAnoConclusao", true);
                    break;
                case "14": // Pós-Graduação / Especialização
                case "15": // Mestrado
                case "16": // Doutorado
                case "17": // Pós-Doutorado
                    DicionarioParametros.Add("ObrigatorioInstituicao", true);
                    DicionarioParametros.Add("VisivelInstituicao", true);
                    DicionarioParametros.Add("ObrigatorioCurso", true);
                    DicionarioParametros.Add("VisivelCurso", true);
                    DicionarioParametros.Add("ObrigatorioLocal", false);
                    DicionarioParametros.Add("VisivelLocal", false);
                    DicionarioParametros.Add("ObrigatorioCidadeEstado", false);
                    DicionarioParametros.Add("VisivelCidadeEstado", true);
                    DicionarioParametros.Add("ObrigatorioSituacao", false);
                    DicionarioParametros.Add("VisivelSituacao", false);
                    DicionarioParametros.Add("ObrigatorioPeriodo", false);
                    DicionarioParametros.Add("VisivelPeriodo", false);
                    DicionarioParametros.Add("ObrigatorioAnoConclusao", false);
                    DicionarioParametros.Add("VisivelAnoConclusao", true);
                    break;
                case "10": //Tecnólgo Incompleto
                case "11": //Superior Incompleto
                    DicionarioParametros.Add("ObrigatorioInstituicao", true);
                    DicionarioParametros.Add("VisivelInstituicao", true);
                    DicionarioParametros.Add("ObrigatorioCurso", true);
                    DicionarioParametros.Add("VisivelCurso", true);
                    DicionarioParametros.Add("ObrigatorioLocal", false);
                    DicionarioParametros.Add("VisivelLocal", false);
                    DicionarioParametros.Add("ObrigatorioCidadeEstado", false);
                    DicionarioParametros.Add("VisivelCidadeEstado", true);
                    DicionarioParametros.Add("ObrigatorioSituacao", true);
                    DicionarioParametros.Add("VisivelSituacao", true);
                    DicionarioParametros.Add("ObrigatorioPeriodo", true);
                    DicionarioParametros.Add("VisivelPeriodo", true);
                    DicionarioParametros.Add("ObrigatorioAnoConclusao", false);
                    DicionarioParametros.Add("VisivelAnoConclusao", false);
                    break;
            }
        }
        #endregion

        #region AjustarCamposFormacao
        private void AjustarCamposFormacao()
        {
            //Habilitando/Desabilitando controles
            txtInstituicao.Enabled = DicionarioParametros["VisivelInstituicao"];
            txtTituloCurso.Enabled = DicionarioParametros["VisivelCurso"];
            ddlSituacao.Enabled = DicionarioParametros["VisivelSituacao"];
            txtPeriodo.Enabled = DicionarioParametros["VisivelPeriodo"];
            txtAnoConclusao.Enabled = DicionarioParametros["VisivelAnoConclusao"];

            //Mostrando/Escondendo div's
            divLinhaInstituicao.Visible = DicionarioParametros["VisivelInstituicao"];
            divLinhaTituloCurso.Visible = DicionarioParametros["VisivelCurso"];
            divCidade.Visible = DicionarioParametros["VisivelCidadeEstado"];
            divLinhaSituacao.Visible = DicionarioParametros["VisivelSituacao"] || DicionarioParametros["VisivelPeriodo"];
            divLinhaConclusao.Visible = DicionarioParametros["VisivelAnoConclusao"];

            //Mostrando/Escondendo controles
            lblPeriodo.Visible = DicionarioParametros["VisivelPeriodo"];
            txtPeriodo.Visible = DicionarioParametros["VisivelPeriodo"];

            //Habilitando/Desabilitando validadores
            rfvInstituicao.Enabled = DicionarioParametros["ObrigatorioInstituicao"];
            rfvTituloCurso.Enabled = DicionarioParametros["ObrigatorioCurso"];
            cvTituloCurso.Enabled = DicionarioParametros["ObrigatorioCurso"];
            cvSituacao.Enabled = DicionarioParametros["ObrigatorioSituacao"];

            txtPeriodo.Obrigatorio = DicionarioParametros["ObrigatorioPeriodo"];
            txtAnoConclusao.Obrigatorio = DicionarioParametros["ObrigatorioAnoConclusao"];
            rfvCidade.Enabled = DicionarioParametros["ObrigatorioCidadeEstado"];

            upCabecalho.Update();
        }
        #endregion

        #region AjustarAutoCompleteNivelCurso
        private void AjustarAutoCompleteNivelCurso(string valor)
        {
            switch (valor)
            {
                case "8": // Técnico / Pós-Médio Incompleto
                case "9": // Técnico / Pós-Médio Completo
                    ContextKeyNivelCurso(Enumeradores.NivelCurso.Tecnico);
                    break;
                case "10": //Tecnólgo Incompleto
                case "12": // Tecnólogo Completo
                    ContextKeyNivelCurso(Enumeradores.NivelCurso.Tecnologo);
                    break;
                case "13": // Superior Completo
                case "11": //Superior Incompleto
                    ContextKeyNivelCurso(Enumeradores.NivelCurso.Graduacao);
                    break;
                case "14": // Pós-Graduação / Especialização
                case "15": // Mestrado        
                    ContextKeyNivelCurso(Enumeradores.NivelCurso.PosGraduacao);
                    break;
                case "16": // Doutorado
                case "17": // Pós-Doutorado
                    ContextKeyNivelCurso(Enumeradores.NivelCurso.Doutorado);
                    break;
            }
        }
        #endregion

        #region AjustarCamposEspecializacao
        private void AjustarCamposEspecializacao()
        {
            //Habilitando/Desabilitando controles
            txtInstituicaoEspecializacao.Enabled = DicionarioParametros["VisivelInstituicao"];
            txtTituloCursoEspecializacao.Enabled = DicionarioParametros["VisivelCurso"];
            ddlSituacaoEspecializacao.Enabled = DicionarioParametros["VisivelSituacao"];
            txtPeriodoEspecializacao.Enabled = DicionarioParametros["VisivelPeriodo"];
            txtAnoConclusaoEspecializacao.Enabled = DicionarioParametros["VisivelAnoConclusao"];

            //Mostrando/Escondendo div's
            divLinhaInstituicaoEspecializacao.Visible = DicionarioParametros["VisivelInstituicao"];
            divLinhaTituloCursoEspecializacao.Visible = DicionarioParametros["VisivelCurso"];
            divCidadeEspecializacao.Visible = DicionarioParametros["VisivelCidadeEstado"];
            divLinhaSituacaoEspecializacao.Visible = DicionarioParametros["VisivelSituacao"] || DicionarioParametros["VisivelPeriodo"];
            divLinhaConclusaoEspecializacao.Visible = DicionarioParametros["VisivelAnoConclusao"];

            //Mostrando/Escondendo controles
            lblPeriodoEspecializacao.Visible = DicionarioParametros["VisivelPeriodo"];
            txtPeriodoEspecializacao.Visible = DicionarioParametros["VisivelPeriodo"];

            //Habilitando/Desabilitando validadores
            rfvInstituicaoEspecializacao.Enabled = DicionarioParametros["ObrigatorioInstituicao"];
            rfvTituloCursoEspecializacao.Enabled = DicionarioParametros["ObrigatorioCurso"];
            cvSituacaoEspecializacao.Enabled = DicionarioParametros["ObrigatorioSituacao"];

            txtPeriodoEspecializacao.Obrigatorio = DicionarioParametros["ObrigatorioPeriodo"];
            txtAnoConclusaoEspecializacao.Obrigatorio = DicionarioParametros["ObrigatorioAnoConclusao"];
            rfvCidadeEspecializacao.Enabled = DicionarioParametros["ObrigatorioCidadeEstado"];

            //upFormacaoEspecializacao.Update();
        }
        #endregion

        #region ContextKeyNivelCurso
        private void ContextKeyNivelCurso(Enumeradores.NivelCurso nivelCurso)
        {
            switch (nivelCurso)
            {
                case Enumeradores.NivelCurso.Tecnico: //Técnico
                case Enumeradores.NivelCurso.Tecnologo: //Tecnólogo
                case Enumeradores.NivelCurso.Graduacao: //Graduação
                    //Setando o contextKey do auto complete de instituicao com o nivel do curso 
                    aceInstituicao.ContextKey = Convert.ToString((int)nivelCurso);

                    //Setando o contextKey com  o nivel do curso e id do curso existente 
                    if (IdFonteFormacao.HasValue)
                        aceTituloCurso.ContextKey = (int)nivelCurso + "," + (int)IdFonteFormacao;
                    else
                        aceTituloCurso.ContextKey = Convert.ToString((int)nivelCurso);
                    //aceTituloCurso.ContextKey = string.Empty + "," + Convert.ToString((int)nivelCurso);
                    break;

                case Enumeradores.NivelCurso.PosGraduacao: // Pós graduação
                case Enumeradores.NivelCurso.Mestrado: // Mestrado        
                case Enumeradores.NivelCurso.Doutorado: // Doutorado
                case Enumeradores.NivelCurso.PosDoutorado: // Pós-Doutorado
                    
                    ////Setando o contextKey do auto complete de instituicao com o nivel do curso             
                    //aceInstituicaoEspecializacao.ContextKey = Convert.ToString((int)nivelCurso); ;

                    ////Setando o contextKey com  o nivel do curso e id do curso existente 
                    //if (IdFonteEspecializacao.HasValue)
                    //    aceTituloCursoEspecializacao.ContextKey = (int)nivelCurso + "," + (int)IdFonteEspecializacao;
                    //else
                    //    aceTituloCursoEspecializacao.ContextKey = Convert.ToString((int)nivelCurso);
                    ////aceTituloCursoEspecializacao.ContextKey = string.Empty + "," + Convert.ToString((int)nivelCurso);
                   break;
            }
        }
        #endregion

        #region RecuperarJobFuncao
        /// <summary>
        /// Validar Funcao
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public string RecuperarJobFuncao(string valor)
        {
            Funcao objFuncao;
            if (Funcao.CarregarPorDescricao(valor, out objFuncao))
            {
                if (!string.IsNullOrEmpty(objFuncao.DescricaoJob))
                    return objFuncao.DescricaoJob;
                return String.Empty;
            }
            return String.Empty;
        }
        #endregion

        #region VerificarObrigatoriedadeCamposExperiencia
        private void VerificarObrigatoriedadeCamposExperiencia()
        {
            bool empresa1 = false;

            if (!String.IsNullOrEmpty(txtDataDemissao1.Valor) || !String.IsNullOrEmpty(txtFuncaoExercida1.Text))
                empresa1 = true;

            txtDataAdmissao1.Obrigatorio = empresa1;
            rfvFuncaoExercida1.Enabled = empresa1;
            txtAtividadeExercida.Obrigatorio = empresa1;
            //txtUltimoSalario.Obrigatorio = empresa1;

            ScriptManager.RegisterStartupScript(upCabecalho, upCabecalho.GetType(), "txtAtividadeExercida_KeyUp", "javaScript:ChecarGraficoQualidadeDasAtividadesExercidas();", true);

        }
        #endregion

        #endregion
    }
}