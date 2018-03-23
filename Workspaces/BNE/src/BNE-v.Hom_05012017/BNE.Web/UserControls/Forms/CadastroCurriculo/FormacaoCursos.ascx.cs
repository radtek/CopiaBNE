using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using JSONSharp;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;
using System.Linq;

namespace BNE.Web.UserControls.Forms.CadastroCurriculo
{
    public partial class FormacaoCursos : BaseUserControl
    {

        #region Propriedades

        #region IdPessoaFisica - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
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

        #region IdComplementar - Variável 2
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int? IdComplementar
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel2.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel2.ToString()].ToString());
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

        #region IdFormacao - Variável 3
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int? IdFormacao
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

        #region IdEspecializacao - Variável 4
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int? IdEspecializacao
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel4.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel4.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel4.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel4.ToString());
            }
        }
        #endregion

        #region EstadoManutencao - Variável 5
        /// <summary>
        /// Propriedade que armazena e recupera um boolean indicando se o user controls está em estado de manutenção
        /// </summary>
        public bool EstadoManutencao
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel5.ToString()] != null)
                    return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel5.ToString()]);
                return false;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel5.ToString(), value);
            }
        }
        #endregion

        #region EnumSituacaoCurriculo - Variável 6
        /// <summary>
        /// Propriedade que armazena e recupera um enum situação curriculo setado pelo usuário administrador no curriculo completo
        /// </summary>
        public Enumeradores.SituacaoCurriculo? EnumSituacaoCurriculo
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel6.ToString()] != null)
                    return (Enumeradores.SituacaoCurriculo)(ViewState[Chave.Temporaria.Variavel6.ToString()]);
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

        #region DicionarioParametros - Variável 7
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public Dictionary<string, bool> DicionarioParametros
        {
            get
            {
                return (Dictionary<string, bool>)ViewState[Chave.Temporaria.Variavel7.ToString()];
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel7.ToString()] = value;
            }
        }
        #endregion

        #region IdEspecializacao - Variável 8
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int? IdFonteFormacao
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel8.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel8.ToString()].ToString());
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

        #region IdFonteEspecializacao - Variável 9
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int? IdFonteEspecializacao
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
        /// <summary>
        /// Método executado quando a página é carregada
        /// </summary>
        /// <param name="sender">Objeto Correspondente</param>
        /// <param name="e">Argumento do Evento</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                Inicializar();

            if (!base.STC.Value || (base.STC.Value && !base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue))
                InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "FormacaoCursos");
            else
                InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "FormacaoCursos");

            Ajax.Utility.RegisterTypeForAjax(typeof(FormacaoCursos));
        }
        #endregion

        #region btnSalvarCursoComplementar_Click
        /// <summary>
        /// Evento disparado no click do btnSalvar
        /// </summary>
        /// <param name="sender">Objeto Correspondente</param>
        /// <param name="e">Argumento do Evento</param>
        protected void btnSalvarCursoComplementar_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate("Complementar");

                if (Page.IsValid)
                {
                    SalvarCursoComplementar();
                    LimparCamposCursos();
                    CarregarGridComplementar();
                    txtInstituicaoComplementar.Focus();
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnSalvarFormacao_Click
        /// <summary>
        /// Evento disparado quando o botão avançar é clicado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSalvarFormacao_Click(object sender, EventArgs e)
        {
            try
            {
                SalvarFormacao();
                LimparCamposFormacao();
                IdFormacao = null;
                CarregarGridFormacao();
                AjustarListaNivelGraducao();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnSalvarEspecializacao_Click
        /// <summary>
        /// Evento disparado quando o botão avançar é clicado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSalvarEspecializacao_Click(object sender, EventArgs e)
        {
            try
            {
                SalvarEspecializacao();
                LimparCamposEspecializacao();
                IdEspecializacao = null;
                CarregarGridEspecializacao();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region gvComplementar

        protected void gvComplementar_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int idFormacao = Convert.ToInt32(gvComplementar.DataKeys[e.RowIndex]["Idf_Formacao"]);
            IdComplementar = idFormacao;
            PreencherCamposComplementar();
        }

        protected void gvComplementar_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int idFormacao = Convert.ToInt32(gvComplementar.DataKeys[e.RowIndex]["Idf_Formacao"]);
            Formacao objFormacao = Formacao.LoadObject(idFormacao);
            objFormacao.FlagInativo = true;
            objFormacao.Save();
            IdComplementar = null;
            CarregarGridComplementar();
        }

        #endregion

        #region gvFormacao

        #region gvFormacao_RowDeleting
        protected void gvFormacao_RowDeleting(object source, GridViewDeleteEventArgs e)
        {
            int idFormacao = Convert.ToInt32(gvFormacao.DataKeys[e.RowIndex]["Idf_Formacao"]);
            Formacao objFormacao = Formacao.LoadObject(idFormacao);
            objFormacao.FlagInativo = true;
            objFormacao.Save();
            IdFormacao = null;
            CarregarGridFormacao();
            CarregarNivelEscolaridade();
        }
        #endregion

        protected void gvFormacao_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int idForm = Convert.ToInt32(gvFormacao.DataKeys[e.RowIndex]["Idf_Formacao"]);
            IdFormacao = idForm;
            LimparCamposFormacao();
            PreencherCamposFormacao();
        }
        #endregion

        #region gvEspecializacao

        #region gvEspecializacao_RowUpdating
        protected void gvEspecializacao_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int idForm = Convert.ToInt32(gvEspecializacao.DataKeys[e.RowIndex]["Idf_Formacao"]);
            IdEspecializacao = idForm;
            LimparCamposEspecializacao();
            PreencherCamposEspecializacao();
        }
        #endregion

        protected void gvEspecializacao_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int idFormacao = Convert.ToInt32(gvEspecializacao.DataKeys[e.RowIndex]["Idf_Formacao"]);
            Formacao objFormacao = Formacao.LoadObject(idFormacao);
            objFormacao.FlagInativo = true;
            objFormacao.Save();
            IdEspecializacao = null;
            CarregarGridEspecializacao();
            CarregarNivelEscolaridade();
        }

        #endregion

        #region gvIdiomas

        #region gvIdioma_RowDeleting
        protected void gvIdioma_RowDeleting(object source, GridViewDeleteEventArgs e)
        {
            int idIdioma = Convert.ToInt32(gvIdioma.DataKeys[e.RowIndex]["Idf_Pessoa_Fisica_Idioma"]);
            PessoafisicaIdioma objPessoaFisicaIdioma = PessoafisicaIdioma.LoadObject(idIdioma);
            objPessoaFisicaIdioma.FlagInativo = true;
            objPessoaFisicaIdioma.Save();
            CarregarGridIdioma();
        }
        #endregion

        #endregion

        #region btnSalvarIdioma_Click
        protected void btnSalvarIdioma_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate("CadastroIdioma");
                if (Page.IsValid)
                {
                    SalvarIdioma();
                    LimparCamposIdiomas();
                    CarregarGridIdioma();
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlMiniCurriculo
        /// <summary>
        /// habilitar aba Dados Pessoais
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlMiniCurriculo_Click(object sender, EventArgs e)
        {
            try
            {
                if (SalvarInformacoesSemConfirmacao())
                {
                    Salvar();

                    ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);

                    if (IdPessoaFisica.HasValue)
                        Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoMini.ToString(), null));
                }
                else
                    ExibirMensagem(MensagemAviso._24034, TipoMensagem.Aviso);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlDadosPessoais
        /// <summary>
        /// habilitar aba Dados Pessoais
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlDadosPessoais_Click(object sender, EventArgs e)
        {
            try
            {
                if (SalvarInformacoesSemConfirmacao())
                {
                    Salvar();

                    ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);

                    if (IdPessoaFisica.HasValue)
                        Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoDados.ToString(), null));
                }
                else
                    ExibirMensagem(MensagemAviso._24034, TipoMensagem.Aviso);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlDadosComplementares_Click
        /// <summary>
        /// habilitar aba Dados Pessoais
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlDadosComplementares_Click(object sender, EventArgs e)
        {
            try
            {
                if (SalvarInformacoesSemConfirmacao())
                {
                    Salvar();
                    ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);

                    if (IdPessoaFisica.HasValue)
                        Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoComplementar.ToString(), null));
                }
                else
                    ExibirMensagem(MensagemAviso._24034, TipoMensagem.Aviso);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlRevisaoDados_Click
        /// <summary>
        /// habilitar aba Dados Pessoais
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlRevisaoDados_Click(object sender, EventArgs e)
        {
            try
            {
                if (SalvarInformacoesSemConfirmacao())
                {
                    Salvar();
                    ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);

                    if (IdPessoaFisica.HasValue)
                        Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoRevisao.ToString(), null));
                }
                else
                    ExibirMensagem(MensagemAviso._24034, TipoMensagem.Aviso);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlGestao_Click
        /// <summary>
        /// habilitar aba Gestao
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btlGestao_Click(object sender, EventArgs e)
        {
            try
            {
                if (SalvarInformacoesSemConfirmacao())
                {
                    Salvar();

                    ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);

                    if (IdPessoaFisica.HasValue)
                        Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                    Redirect("~/CadastroCurriculoGestao.aspx");
                }
                else
                    ExibirMensagem(MensagemAviso._24034, TipoMensagem.Aviso);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnSalvar_Click
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            if (SalvarInformacoesSemConfirmacao())
            {
                Salvar();

                if (IdPessoaFisica.HasValue)
                    Session[Chave.Temporaria.Variavel1.ToString()] = IdPessoaFisica.Value;

                Redirect(GetRouteUrl(Enumeradores.RouteCollection.CadastroCurriculoComplementar.ToString(), null));
            }
            else
                ExibirMensagem(MensagemAviso._24034, TipoMensagem.Aviso);
        }
        #endregion

        #region cvNivelIdioma_ServerValidate
        protected void cvNivelIdioma_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (Convert.ToInt32(ddlIdioma.SelectedValue) >= 1 && (String.IsNullOrEmpty(rblNivelIdioma.SelectedValue) || Convert.ToInt32(rblNivelIdioma.SelectedValue) < 1))
                args.IsValid = false;
            else
                args.IsValid = true;
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

        #region ddlNivelEspecializacao_SelectedIndexChanged
        protected void ddlNivelEspecializacao_SelectedIndexChanged(object sender, EventArgs e)
        {
            AjustarParametrosCampos(ddlNivelEspecializacao.SelectedValue);
            AjustarAutoCompleteNivelCurso(ddlNivelEspecializacao.SelectedValue);
            AjustarCamposEspecializacao();
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

        #endregion

        #region Métodos

        #region Inicializar
        /// <summary>
        /// Método utilizado para  preenchimento de componentes, funções de foco e navegação
        /// </summary>
        private void Inicializar()
        {
            if (EstadoManutencao)
            {
                pnlBotoes.Visible = false;
                pnlAbas.Visible = false;
                litTitulo.Text = "Formação e Cursos";
            }

            CarregarNivelEscolaridade();

            UIHelper.CarregarDropDownList(ddlIdioma, Idioma.Listar(), new ListItem("Selecione", "0"));
            UIHelper.CarregarDropDownList(ddlSituacao, SituacaoFormacao.Listar(), new ListItem("Selecione", "0"));
            UIHelper.CarregarDropDownList(ddlSituacaoEspecializacao, SituacaoFormacao.Listar(), new ListItem("Selecione", "0"));

            //Carregar RadioButtonList
            UIHelper.CarregarRadioButtonList(rblNivelIdioma, NivelIdioma.Listar(), "Idf_Nivel_Idioma", "Des_Nivel_Idioma");

            CarregarParametros();

            //Carrega todas as grids
            this.PreencherCampos();

            //focus inicial
            ddlNivel.Focus();

            btlGestao.Visible = base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue;

            AjustarParametrosCampos(ddlNivel.SelectedValue);
            AjustarCamposFormacao();
            AjustarCamposEspecializacao();

            AjustarAutoCompleteInstituicaoComplementar();

            UIHelper.ValidateFocus(btnSalvar);
            UIHelper.ValidateFocus(btlDadosComplementares);
            UIHelper.ValidateFocus(btlDadosPessoais);
            UIHelper.ValidateFocus(btlMiniCurriculo);
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

            upFormacao.Update();
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

            upFormacaoEspecializacao.Update();
        }
        #endregion

        #region AjustarAutoCompleteInstituicaoComplementar
        /// <summary>
        /// Este auto complete é setado server side pois o nivel do curso sempre será o mesmo
        /// Já os outros são setados pelo java script dinamicamente, de acordo com a mudança de nivel dos cursos nas combos
        /// </summary>
        private void AjustarAutoCompleteInstituicaoComplementar()
        {
            aceInstituicaoComplementar.ContextKey = ((int)Enumeradores.NivelCurso.Aperfeicoamento).ToString(CultureInfo.CurrentCulture);
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
                    //Setando o contextKey do auto complete de instituicao com o nivel do curso             
                    aceInstituicaoEspecializacao.ContextKey = Convert.ToString((int)nivelCurso); ;

                    //Setando o contextKey com  o nivel do curso e id do curso existente 
                    if (IdFonteEspecializacao.HasValue)
                        aceTituloCursoEspecializacao.ContextKey = (int)nivelCurso + "," + (int)IdFonteEspecializacao;
                    else
                        aceTituloCursoEspecializacao.ContextKey = Convert.ToString((int)nivelCurso);
                    //aceTituloCursoEspecializacao.ContextKey = string.Empty + "," + Convert.ToString((int)nivelCurso);
                    break;
            }
        }
        #endregion

        #region CarregarParametros
        /// <summary>
        /// Carrega os parâmetros iniciais da aba de dados gerais.
        /// </summary>
        private void CarregarParametros()
        {
            try
            {
                var parametros = new List<Enumeradores.Parametro>
                    {
                        Enumeradores.Parametro.IntervaloTempoAutoComplete,
                        Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteSigla,
                        Enumeradores.Parametro.NumeroResultadosAutoCompleteSigla,
                        Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteNomeInstituicao,
                        Enumeradores.Parametro.NumeroResultadosAutoCompleteNomeInstituicao,
                        Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteNomeCurso,
                        Enumeradores.Parametro.NumeroResultadosAutoCompleteNomeCurso
                    };

                Dictionary<Enumeradores.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

                aceTituloCursoEspecializacao.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceTituloCursoEspecializacao.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteNomeInstituicao]);
                aceTituloCursoEspecializacao.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteNomeInstituicao]);

                aceInstituicao.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceInstituicao.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteNomeInstituicao]);
                aceInstituicao.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteNomeInstituicao]);

                aceTituloCurso.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceTituloCurso.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteNomeCurso]);
                aceTituloCurso.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteNomeCurso]);

            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region CarregarGridFormacao
        /// <summary>
        /// Carrega a Grid de formação
        /// </summary>
        public void CarregarGridFormacao()
        {
            UIHelper.CarregarGridView(gvFormacao, Formacao.ListarFormacao(IdPessoaFisica.Value, true, false, false));

            AjustarVisibilidadeEspecializacao();
        }
        #endregion

        #region CarregarGridEspecializacao
        /// <summary>
        /// Carrega a Grid de formação
        /// </summary>
        public void CarregarGridEspecializacao()
        {
            UIHelper.CarregarGridView(gvEspecializacao, Formacao.ListarFormacao(IdPessoaFisica.Value, false, true, false));

            AjustarVisibilidadeEspecializacao();
        }
        #endregion

        #region AjustarVisibilidadeEspecializacao
        private void AjustarVisibilidadeEspecializacao()
        {
            if (ExisteGraduacao() || !gvEspecializacao.Rows.Count.Equals(0))
                pnlFormacaoEspecializacao.Style["display"] = "";
            else
                pnlFormacaoEspecializacao.Style["display"] = "none";

            upFormacaoEspecializacao.Update();
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
            upNivelEspecializacao.Update();
        }
        #endregion

        #region CarregarGridFormacao
        /// <summary>
        /// Carrega a Grid de cursos
        /// </summary>
        public void CarregarGridComplementar()
        {
            UIHelper.CarregarGridView(gvComplementar, Formacao.ListarFormacao(IdPessoaFisica.Value, false, false, true));
        }
        #endregion

        #region CarregarGridIdioma
        /// <summary>
        /// Carrega a Grid de Idiomas
        /// </summary>
        public void CarregarGridIdioma()
        {
            UIHelper.CarregarGridView(gvIdioma, PessoafisicaIdioma.ListarPorPessoaFisicaDT(IdPessoaFisica.Value));
        }
        #endregion

        #region PreencherCampos
        /// <summary>
        /// Preenche os campos do formulário
        /// </summary>
        private void PreencherCampos()
        {
            if (IdPessoaFisica.HasValue)
            {
                CarregarGridFormacao();
                CarregarGridEspecializacao();
                CarregarGridComplementar();
                CarregarGridIdioma();
                //this.ExibirMensagemSMS();
            }
        }
        #endregion

        #region PreencherCamposComplementar
        /// <summary>
        /// Preenche os campos do formulário
        /// </summary>
        private void PreencherCamposComplementar()
        {
            if (IdComplementar.HasValue)
            {
                Formacao objFormacao = Formacao.LoadObject(IdComplementar.Value);

                if (objFormacao.Fonte != null)
                {
                    objFormacao.Fonte.CompleteObject();
                    txtInstituicaoComplementar.Text = string.Format("{0} - {1}", objFormacao.Fonte.SiglaFonte, objFormacao.Fonte.NomeFonte);
                }
                else
                    txtInstituicaoComplementar.Text = objFormacao.DescricaoFonte;

                if (objFormacao.Curso != null)
                {
                    objFormacao.Curso.CompleteObject();
                    txtTituloCursoComplementar.Text = objFormacao.Curso.DescricaoCurso;
                }
                else
                    txtTituloCursoComplementar.Text = objFormacao.DescricaoCurso;

                if (objFormacao.Cidade != null)
                {
                    objFormacao.Cidade.CompleteObject();
                    txtCidadeComplementar.Text = String.Format("{0}/{1}", objFormacao.Cidade.NomeCidade, objFormacao.Cidade.Estado.SiglaEstado);
                }
                else
                    txtCidadeComplementar.Text = String.Empty;

                txtAnoConclusaoComplementar.Valor = objFormacao.AnoConclusao.HasValue ? objFormacao.AnoConclusao.ToString() : string.Empty;
                txtCargaHorariaComplementar.Valor = objFormacao.QuantidadeCargaHoraria.HasValue ? objFormacao.QuantidadeCargaHoraria.Value.ToString(CultureInfo.CurrentCulture) : string.Empty;

                upComplementar.Update();
            }
        }
        #endregion

        #region PreencherCamposFormacao
        /// <summary>
        /// Preenche os campos do formulário
        /// </summary>
        private void PreencherCamposFormacao()
        {
            if (IdFormacao.HasValue)
            {
                Formacao objFormacao = Formacao.LoadObject(IdFormacao.Value);

                if (objFormacao.Fonte != null)
                {
                    objFormacao.Fonte.CompleteObject();
                    IdFonteFormacao = objFormacao.Fonte.IdFonte;
                    txtInstituicao.Text = string.Format("{0} - {1}", objFormacao.Fonte.SiglaFonte, objFormacao.Fonte.NomeFonte);
                }
                else
                    txtInstituicao.Text = objFormacao.DescricaoFonte;

                if (objFormacao.Curso != null)
                {
                    objFormacao.Curso.CompleteObject();
                    txtTituloCurso.Text = objFormacao.Curso.DescricaoCurso;
                }
                else
                    txtTituloCurso.Text = objFormacao.DescricaoCurso;

                if (objFormacao.Cidade != null)
                {
                    objFormacao.Cidade.CompleteObject();
                    txtCidade.Text = String.Format("{0}/{1}", objFormacao.Cidade.NomeCidade, objFormacao.Cidade.Estado.SiglaEstado);
                }

                if (objFormacao.AnoConclusao.HasValue)
                    txtAnoConclusao.Valor = objFormacao.AnoConclusao.Value.ToString(CultureInfo.CurrentCulture);
                else
                    txtAnoConclusao.Valor = string.Empty;

                // Escolaridade objEscolaridade;
                objFormacao.Escolaridade.CompleteObject();

                if (objFormacao.SituacaoFormacao != null)
                    ddlSituacao.SelectedValue = objFormacao.SituacaoFormacao.IdSituacaoFormacao.ToString(CultureInfo.CurrentCulture);

                if (objFormacao.NumeroPeriodo != null)
                    txtPeriodo.Valor = objFormacao.NumeroPeriodo.Value.ToString(CultureInfo.CurrentCulture);

                ListItem li = ddlNivel.Items.FindByValue(objFormacao.Escolaridade.IdEscolaridade.ToString(CultureInfo.CurrentCulture));

                if (li == null)
                    ddlNivel.Items.Add(new ListItem(objFormacao.Escolaridade.DescricaoBNE, objFormacao.Escolaridade.IdEscolaridade.ToString(CultureInfo.CurrentCulture)));

                ddlNivel.SelectedValue = objFormacao.Escolaridade.IdEscolaridade.ToString(CultureInfo.CurrentCulture);
                ddlNivel.Enabled = true;

                AjustarParametrosCampos(ddlNivel.SelectedValue);
                AjustarAutoCompleteNivelCurso(ddlNivel.SelectedValue);
                AjustarCamposFormacao();
            }
        }
        #endregion

        #region LimparCamposFormacao
        /// <summary>
        /// Método utilizado para limpar todos os campos  ou colocar na opção default relacionados ao Formação 
        /// </summary>
        private void LimparCamposFormacao()
        {
            ddlNivel.SelectedIndex = 0;
            txtInstituicao.Text = String.Empty;
            txtTituloCurso.Text = String.Empty;
            ddlSituacao.SelectedIndex = 0;
            txtPeriodo.Valor = String.Empty;
            txtAnoConclusao.Valor = String.Empty;
            txtCidade.Text = String.Empty;

            AjustarParametrosCampos(ddlNivel.SelectedValue);
            AjustarAutoCompleteNivelCurso(ddlNivel.SelectedValue);
            AjustarCamposFormacao();
        }
        #endregion

        #region PreencherCamposEspecializacao
        /// <summary>
        /// Preenche os campos do formulário
        /// </summary>
        private void PreencherCamposEspecializacao()
        {
            if (IdEspecializacao.HasValue)
            {
                Formacao objFormacao = Formacao.LoadObject(IdEspecializacao.Value);

                if (objFormacao.Fonte != null)
                {
                    objFormacao.Fonte.CompleteObject();
                    IdFonteEspecializacao = objFormacao.Fonte.IdFonte;
                    txtInstituicaoEspecializacao.Text = string.Format("{0} - {1}", objFormacao.Fonte.SiglaFonte, objFormacao.Fonte.NomeFonte);
                }
                else
                    txtInstituicaoEspecializacao.Text = objFormacao.DescricaoFonte;

                if (objFormacao.Curso != null)
                {
                    objFormacao.Curso.CompleteObject();
                    txtTituloCursoEspecializacao.Text = objFormacao.Curso.DescricaoCurso;
                }
                else
                    txtTituloCursoEspecializacao.Text = objFormacao.DescricaoCurso;

                if (objFormacao.Cidade != null)
                {
                    objFormacao.Cidade.CompleteObject();
                    txtCidadeEspecializacao.Text = String.Format("{0}/{1}", objFormacao.Cidade.NomeCidade, objFormacao.Cidade.Estado.SiglaEstado);
                }

                if (objFormacao.AnoConclusao.HasValue)
                    txtAnoConclusaoEspecializacao.Valor = objFormacao.AnoConclusao.Value.ToString(CultureInfo.CurrentCulture);
                else
                    txtAnoConclusaoEspecializacao.Valor = string.Empty;

                // Escolaridade objEscolaridade;
                objFormacao.Escolaridade.CompleteObject();

                if (objFormacao.SituacaoFormacao != null)
                    ddlSituacaoEspecializacao.SelectedValue = objFormacao.SituacaoFormacao.IdSituacaoFormacao.ToString(CultureInfo.CurrentCulture);

                if (objFormacao.NumeroPeriodo != null)
                    txtPeriodoEspecializacao.Valor = objFormacao.NumeroPeriodo.Value.ToString(CultureInfo.CurrentCulture);

                upFormacaoEspecializacao.Update();

                ddlNivelEspecializacao.Items.Add(new ListItem(objFormacao.Escolaridade.DescricaoBNE, objFormacao.Escolaridade.IdEscolaridade.ToString(CultureInfo.CurrentCulture)));
                ddlNivelEspecializacao.SelectedValue = objFormacao.Escolaridade.IdEscolaridade.ToString(CultureInfo.CurrentCulture);

                AjustarParametrosCampos(ddlNivelEspecializacao.SelectedValue);
                AjustarAutoCompleteNivelCurso(ddlNivelEspecializacao.SelectedValue);
                AjustarCamposEspecializacao();
            }
        }
        #endregion

        #region LimparCamposEspecializacao
        /// <summary>
        /// Método utilizado para limpar todos os campos  ou colocar na opção default relacionados ao Formação 
        /// </summary>
        private void LimparCamposEspecializacao()
        {
            ddlNivelEspecializacao.SelectedIndex = 0;
            txtInstituicaoEspecializacao.Text = String.Empty;
            txtTituloCursoEspecializacao.Text = String.Empty;
            ddlSituacaoEspecializacao.SelectedIndex = 0;
            txtPeriodoEspecializacao.Valor = String.Empty;
            txtAnoConclusaoEspecializacao.Valor = String.Empty;
            txtCidadeEspecializacao.Text = String.Empty;

            AjustarParametrosCampos(ddlNivelEspecializacao.SelectedValue);
            AjustarAutoCompleteNivelCurso(ddlNivelEspecializacao.SelectedValue);
            AjustarCamposEspecializacao();
        }
        #endregion

        #region LimparCamposCursos
        /// <summary>
        /// Método utilizado para limpar todos os campos  ou colocar na opção default relacionados ao Formação 
        /// </summary>
        private void LimparCamposCursos()
        {
            txtInstituicaoComplementar.Text = String.Empty;
            txtTituloCursoComplementar.Text = String.Empty;
            txtCidadeComplementar.Text = String.Empty;
            txtCargaHorariaComplementar.Valor = String.Empty;
            txtAnoConclusaoComplementar.Valor = String.Empty;
        }
        #endregion

        #region LimparCamposIdiomas
        /// <summary>
        /// Método utilizado para limpar ou colocar na opção default todos os campos relacionados ao Idioma
        /// </summary>
        private void LimparCamposIdiomas()
        {
            ddlIdioma.SelectedIndex = 0;
            rblNivelIdioma.SelectedIndex = -1;
        }
        #endregion

        #region Salvar
        /// <summary>
        /// Método utilizado para salvar a grid formação
        /// </summary>
        public void Salvar()
        {
            if (IdPessoaFisica.HasValue)
            {
                Curriculo objCurriculo;
                if (Curriculo.CarregarPorPessoaFisica(IdPessoaFisica.Value, out objCurriculo))
                    objCurriculo.SalvarFormacaoCursos(EnumSituacaoCurriculo);
            }
        }
        #endregion

        #region SalvarInformacoesSemConfirmacao
        /// <summary>
        /// Se o usuário não confirmou por exemplo a formação através do botão de salvar e inserir na grid, 
        /// Se os campos obrigatorios estiverem preenchidos, este metodo fará a inserção.
        /// </summary>
        private bool SalvarInformacoesSemConfirmacao()
        {
            bool mgsCampoObrigatorioFormacao;
            if (ValidarFormacaoNaoConfirmado(out mgsCampoObrigatorioFormacao))
            {
                SalvarFormacao();
                CarregarGridFormacao();
                LimparCamposFormacao();
            }

            bool mgsCampoObrigatorioEspecializacao;
            if (ValidarEspecializacaoNaoConfirmado(out mgsCampoObrigatorioEspecializacao))
            {
                SalvarEspecializacao();
                CarregarGridEspecializacao();
                LimparCamposEspecializacao();
            }

            bool mgsCampoObrigatorioCursosComplemetares;
            if (ValidarCursosComplementaresNaoConfirmado(out mgsCampoObrigatorioCursosComplemetares))
            {
                SalvarCursoComplementar();
                CarregarGridComplementar();
                LimparCamposCursos();
            }

            bool mgsCampoObrigatorioIdiomas;
            if (ValidarIdiomasNaoConfirmado(out mgsCampoObrigatorioIdiomas))
            {
                SalvarIdioma();
                CarregarGridIdioma();
                LimparCamposIdiomas();
            }

            if (mgsCampoObrigatorioFormacao || mgsCampoObrigatorioEspecializacao || mgsCampoObrigatorioCursosComplemetares || mgsCampoObrigatorioIdiomas)
                return false;
            
            return true;
        }
        #endregion

        #region ValidarFormacaoNaoConfirmado
        /// <summary>
        /// Metodo responsável por validar os campos da formação quando não forem confirmados em btnSalvarFormacao
        /// </summary>
        /// <returns></returns>
        private bool ValidarFormacaoNaoConfirmado(out bool mostrarMensagemCampoObrigatorio)
        {
            mostrarMensagemCampoObrigatorio = false;

            //Se for diferente de selecione
            if (!ddlNivel.SelectedValue.Equals("0"))
            {
                if (ddlNivel.SelectedValue.Equals(((int)Enumeradores.Escolaridade.EnsinoFundamentalIncompleto).ToString(CultureInfo.CurrentCulture)))
                    return true;
                if (ddlNivel.SelectedValue.Equals(((int)Enumeradores.Escolaridade.EnsinoFundamentalCompleto).ToString(CultureInfo.CurrentCulture)))
                    return true;
                if (ddlNivel.SelectedValue.Equals(((int)Enumeradores.Escolaridade.EnsinoMedioIncompleto).ToString(CultureInfo.CurrentCulture)))
                {
                    if (!ddlSituacao.SelectedValue.Equals("0"))
                        return true;
                    mostrarMensagemCampoObrigatorio = true;
                    return false;
                }
                if (ddlNivel.SelectedValue.Equals(((int)Enumeradores.Escolaridade.EnsinoMedioCompleto).ToString(CultureInfo.CurrentCulture)))
                {
                    if (!string.IsNullOrEmpty(txtAnoConclusao.Valor))
                        return true;
                    mostrarMensagemCampoObrigatorio = true;
                    return false;
                }
                if (ddlNivel.SelectedValue.Equals(((int)Enumeradores.Escolaridade.TecnicoPosMedioIncompleto).ToString(CultureInfo.CurrentCulture)))
                {
                    bool retorno = true;
                    if (string.IsNullOrEmpty(txtInstituicao.Text))
                        retorno = false;
                    if (string.IsNullOrEmpty(txtTituloCurso.Text))
                        retorno = false;
                    if (ddlSituacao.SelectedValue.Equals("0"))
                        retorno = false;

                    if (!retorno)
                        mostrarMensagemCampoObrigatorio = true;

                    return retorno;
                }
                if (ddlNivel.SelectedValue.Equals(((int)Enumeradores.Escolaridade.TecnicoPosMedioCompleto).ToString(CultureInfo.CurrentCulture))
                    || ddlNivel.SelectedValue.Equals(((int)Enumeradores.Escolaridade.TecnologoCompleto).ToString(CultureInfo.CurrentCulture))
                    || ddlNivel.SelectedValue.Equals(((int)Enumeradores.Escolaridade.SuperiorCompleto).ToString(CultureInfo.CurrentCulture)))
                {
                    bool retorno = true;
                    if (string.IsNullOrEmpty(txtInstituicao.Text))
                        retorno = false;
                    if (string.IsNullOrEmpty(txtTituloCurso.Text))
                        retorno = false;

                    if (!retorno)
                        mostrarMensagemCampoObrigatorio = true;

                    return retorno;
                }
                if (ddlNivel.SelectedValue.Equals(((int)Enumeradores.Escolaridade.TecnologoIncompleto).ToString(CultureInfo.CurrentCulture))
                    || ddlNivel.SelectedValue.Equals(((int)Enumeradores.Escolaridade.SuperiorIncompleto).ToString(CultureInfo.CurrentCulture)))
                {
                    bool retorno = true;
                    if (string.IsNullOrEmpty(txtInstituicao.Text))
                        retorno = false;
                    if (string.IsNullOrEmpty(txtTituloCurso.Text))
                        retorno = false;
                    if (ddlSituacao.SelectedValue.Equals("0"))
                        retorno = false;
                    if (string.IsNullOrEmpty(txtPeriodo.Valor))
                        retorno = false;

                    if (!retorno)
                        mostrarMensagemCampoObrigatorio = true;

                    return retorno;
                }
            }
            return false;
        }
        #endregion

        #region ValidarEspecializacaoNaoConfirmado
        /// <summary>
        /// Metodo responsável por validar os campos da especializacao quando não forem confirmados em btnSalvarFormacao
        /// </summary>
        /// <returns></returns>
        private bool ValidarEspecializacaoNaoConfirmado(out bool mostrarMensagemCampoObrigatorio)
        {
            mostrarMensagemCampoObrigatorio = false;
            //Se for diferente de selecione
            if (!ddlNivelEspecializacao.SelectedValue.Equals("0"))
            {
                bool retorno = true;
                if (string.IsNullOrEmpty(txtInstituicaoEspecializacao.Text))
                    retorno = false;
                if (string.IsNullOrEmpty(txtTituloCursoEspecializacao.Text))
                    retorno = false;

                if (!retorno)
                    mostrarMensagemCampoObrigatorio = true;

                return retorno;
            }
            return false;
        }

        #endregion

        #region ValidarCursosComplementaresNaoConfirmado
        /// <summary>
        /// Metodo responsável por validar os campos da especializacao quando não forem confirmados em btnSalvarFormacao
        /// </summary>
        /// <returns></returns>
        private bool ValidarCursosComplementaresNaoConfirmado(out bool mostrarMensagemCampoObrigatorio)
        {
            mostrarMensagemCampoObrigatorio = false;

            //Se qualquer campo obrigatorio for preenchido, cada campo obrigatorio será verificado
            if (!string.IsNullOrEmpty(txtInstituicaoComplementar.Text) || !string.IsNullOrEmpty(txtTituloCursoComplementar.Text)
                || !string.IsNullOrEmpty(txtAnoConclusaoComplementar.Valor))
            {

                bool retorno = true;
                if (string.IsNullOrEmpty(txtInstituicaoComplementar.Text))
                    retorno = false;
                if (string.IsNullOrEmpty(txtTituloCursoComplementar.Text))
                    retorno = false;

                if (!retorno)
                    mostrarMensagemCampoObrigatorio = true;

                return retorno;
            }
            else //Como não foi preenchido nenhum campo não exibirá nenhuma mensagem pois não é obrigatório preencher os cursos complementares.
                return false;
        }
        #endregion

        #region ValidarIdiomasNaoConfirmado
        /// <summary>
        /// Metodo responsável por validar os campos da idiomas quando não forem confirmados em btnSalvarFormacao
        /// </summary>
        /// <returns></returns>
        private bool ValidarIdiomasNaoConfirmado(out bool mostrarMensagemCampoObrigatorio)
        {
            mostrarMensagemCampoObrigatorio = false;
            bool nivelIdiomaChecado = false;

            foreach (ListItem lst in rblNivelIdioma.Items)
            {
                if (lst.Selected)
                    nivelIdiomaChecado = true;
            }

            if (nivelIdiomaChecado && !ddlIdioma.SelectedValue.Equals("0"))
                return true;
            
            return false;
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
            upNivel.Update();
        }
        #endregion

        #region SalvarFormacao
        private void SalvarFormacao()
        {
            if (!ddlNivel.SelectedValue.Equals("0"))
            {
                PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);
                Formacao objFormacao = IdFormacao.HasValue ? Formacao.LoadObject(IdFormacao.Value) : new Formacao();

                Curriculo objCurriculo;
                Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo);

                objFormacao.PessoaFisica = objPessoaFisica;
                objFormacao.Escolaridade = new BNE.BLL.Escolaridade(Convert.ToInt32(ddlNivel.SelectedValue));

                string[] instituicaoSigla = txtInstituicao.Text.Split('-');

                string nomeInstituicao;
                if (instituicaoSigla.Length.Equals(1))
                    nomeInstituicao = instituicaoSigla[0].Trim();
                else
                    nomeInstituicao = instituicaoSigla[1].Trim();

                if (!String.IsNullOrEmpty(nomeInstituicao))
                {
                    Fonte objFonte;
                    if (Fonte.CarregarPorNome(nomeInstituicao, out objFonte))
                    {
                        objFormacao.Fonte = objFonte;
                        objFormacao.DescricaoFonte = String.Empty;
                    }
                    else
                    {
                        objFormacao.Fonte = null;
                        objFormacao.DescricaoFonte = nomeInstituicao;
                    }
                }
                else
                    objFormacao.Fonte = null;



                //Quando não existe curso na formação define-a como null
                string[] semCurso = { "4", "5", "6", "7" };
                if (semCurso.Contains(ddlNivel.SelectedValue)) 
                {
                    objFormacao.Curso = null;
                    objFormacao.DescricaoCurso = String.Empty;
                }
                else 
                {
                    if (!String.IsNullOrEmpty(txtTituloCurso.Text))
                    {
                        Curso objCurso;
                        if (Curso.CarregarPorNome(txtTituloCurso.Text, out objCurso))
                        {
                            objFormacao.Curso = objCurso;
                            objFormacao.DescricaoCurso = String.Empty;
                        }
                        else
                        {
                            objFormacao.Curso = null;
                            objFormacao.DescricaoCurso = txtTituloCurso.Text.Trim();
                        }
                    }
                    else
                        objFormacao.Curso = null;
                }


                //Lista de formações incompletas.
                string[] incompletas = {"4", "6", "8", "10", "11"};

                //Caso a formação seja incompleta o campo de ano de conclusão deve receber o valor NULO
                //caso contrário busca o valor informado no campo.
                if (incompletas.Contains(ddlNivel.SelectedValue))
                {
                    objFormacao.AnoConclusao = null;
                }
                else 
                {
                    short anoconclusao;
                    if (!string.IsNullOrEmpty(txtAnoConclusao.Valor))
                    {
                        if (short.TryParse(txtAnoConclusao.Valor, out anoconclusao))
                            objFormacao.AnoConclusao = anoconclusao;
                        else
                            objFormacao.AnoConclusao = null;
                    }
                }

                //Se a formação é completa a situação e período deve receber valores nulos.
                if (!incompletas.Contains(ddlNivel.SelectedValue))
                {
                    objFormacao.SituacaoFormacao = null;
                    objFormacao.NumeroPeriodo = null;

                    //mudar parametro currículo Flag_Estagio
                    ParametroCurriculo objAceitaEstagio;

                    if (ParametroCurriculo.CarregarParametroPorCurriculo(Enumeradores.Parametro.CurriculoAceitaEstagio, objCurriculo, out objAceitaEstagio, null))
                    {
                        if (objAceitaEstagio.ValorParametro.ToLower() == "true")
                        {
                            objAceitaEstagio.ValorParametro = "false";
                            objAceitaEstagio.Save();
                        }
                    }

                }
                else 
                {
                    if (!ddlSituacao.SelectedValue.Equals("0"))
                        objFormacao.SituacaoFormacao = new SituacaoFormacao(Convert.ToInt16(ddlSituacao.SelectedValue));
                    else
                        objFormacao.SituacaoFormacao = null;

                    short periodo;
                    if (!string.IsNullOrEmpty(txtPeriodo.Valor))
                    {
                        if (short.TryParse(txtPeriodo.Valor, out periodo))
                            objFormacao.NumeroPeriodo = periodo;
                        else
                            objFormacao.NumeroPeriodo = null;
                    }
                }
               

                Cidade objCidade;
                if (Cidade.CarregarPorNome(txtCidade.Text, out objCidade))
                    objFormacao.Cidade = objCidade;
                else
                    objFormacao.Cidade = null;

                objFormacao.Save();
            }
        }
        #endregion

        #region SalvarEspecializacao
        private void SalvarEspecializacao()
        {
            if (!ddlNivelEspecializacao.SelectedValue.Equals("0"))
            {
                PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);
                Formacao objFormacao = IdEspecializacao.HasValue ? Formacao.LoadObject(IdEspecializacao.Value) : new Formacao();

                objFormacao.PessoaFisica = objPessoaFisica;
                objFormacao.Escolaridade = new BNE.BLL.Escolaridade(Convert.ToInt32(ddlNivelEspecializacao.SelectedValue));

                string[] instituicaoSigla = txtInstituicaoEspecializacao.Text.Split('-');
                string nomeInstituicao;
                if (instituicaoSigla.Length.Equals(1))
                    nomeInstituicao = instituicaoSigla[0].Trim();
                else
                    nomeInstituicao = instituicaoSigla[1].Trim();

                if (!String.IsNullOrEmpty(nomeInstituicao))
                {
                    Fonte objFonte;
                    if (Fonte.CarregarPorNome(nomeInstituicao, out objFonte))
                    {
                        objFormacao.Fonte = objFonte;
                        objFormacao.DescricaoFonte = String.Empty;
                    }
                    else
                    {
                        objFormacao.Fonte = null;
                        objFormacao.DescricaoFonte = nomeInstituicao;
                    }
                }
                else
                    objFormacao.Fonte = null;

                if (!String.IsNullOrEmpty(txtTituloCursoEspecializacao.Text))
                {
                    Curso objCurso;
                    if (Curso.CarregarPorNome(txtTituloCursoEspecializacao.Text, out objCurso))
                    {
                        objFormacao.Curso = objCurso;
                        objFormacao.DescricaoCurso = String.Empty;
                    }
                    else
                    {
                        objFormacao.Curso = null;
                        objFormacao.DescricaoCurso = txtTituloCursoEspecializacao.Text;
                    }
                }
                else
                    objFormacao.Curso = null;

                if (!ddlSituacaoEspecializacao.SelectedValue.Equals("0"))
                    objFormacao.SituacaoFormacao = new SituacaoFormacao(Convert.ToInt16(ddlSituacaoEspecializacao.SelectedValue));
                else
                    objFormacao.SituacaoFormacao = null;

                short periodo;
                if (short.TryParse(txtPeriodoEspecializacao.Valor, out periodo))
                    objFormacao.NumeroPeriodo = periodo;
                else
                    objFormacao.NumeroPeriodo = null;

                short anoconclusao;
                if (short.TryParse(txtAnoConclusaoEspecializacao.Valor, out anoconclusao))
                    objFormacao.AnoConclusao = anoconclusao;
                else
                    objFormacao.AnoConclusao = null;

                Cidade objCidade;
                if (Cidade.CarregarPorNome(txtCidadeEspecializacao.Text, out objCidade))
                    objFormacao.Cidade = objCidade;
                else
                    objFormacao.Cidade = null;

                objFormacao.Save();
            }
        }
        #endregion

        #region SalvarCursoComplementar
        private void SalvarCursoComplementar()
        {
            PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);
            Formacao objFormacao = IdComplementar.HasValue ? Formacao.LoadObject(IdComplementar.Value) : new Formacao();

            objFormacao.PessoaFisica = objPessoaFisica;
            objFormacao.Escolaridade = new Escolaridade((int)Enumeradores.Escolaridade.OutrosCursos);

            string[] instituicaoSigla = txtInstituicaoComplementar.Text.Split('-');
            string nomeInstituicao;
            if (instituicaoSigla.Length.Equals(1))
                nomeInstituicao = instituicaoSigla[0].Trim();
            else
                nomeInstituicao = instituicaoSigla[1].Trim();

            Fonte objFonte;
            if (Fonte.CarregarPorNome(nomeInstituicao, out objFonte))
            {
                objFormacao.Fonte = objFonte;
                objFormacao.DescricaoFonte = String.Empty;
            }
            else
            {
                objFormacao.Fonte = null;
                objFormacao.DescricaoFonte = nomeInstituicao;
            }

            Curso objCurso;
            if (Curso.CarregarPorNome(txtTituloCursoComplementar.Text, out objCurso))
            {
                objFormacao.Curso = objCurso;
                objFormacao.DescricaoCurso = String.Empty;
            }
            else
            {
                objFormacao.Curso = null;
                objFormacao.DescricaoCurso = txtTituloCursoComplementar.Text;
            }

            Cidade objCidade;
            if (Cidade.CarregarPorNome(txtCidadeComplementar.Text, out objCidade))
                objFormacao.Cidade = objCidade;
            else
                objFormacao.Cidade = null;

            short cargahoraria;
            if (short.TryParse(txtCargaHorariaComplementar.Valor, out cargahoraria))
                objFormacao.QuantidadeCargaHoraria = cargahoraria;
            else
                objFormacao.QuantidadeCargaHoraria = null;

            short anoconclusao;
            if (short.TryParse(txtAnoConclusaoComplementar.Valor, out anoconclusao))
                objFormacao.AnoConclusao = anoconclusao;
            else
                objFormacao.AnoConclusao = null;

            objFormacao.FlagInativo = false;

            objFormacao.Save();

            IdComplementar = null;
        }
        #endregion

        #region SalvarIdioma
        private void SalvarIdioma()
        {
            if (!ddlIdioma.SelectedValue.Equals("0"))
            {
                PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);
                PessoafisicaIdioma objPessoaFisicaIdioma;

                if (!PessoafisicaIdioma.CarregarPorPessoaFisicaIdioma(IdPessoaFisica.Value, Convert.ToInt32(ddlIdioma.SelectedValue), out objPessoaFisicaIdioma))
                    objPessoaFisicaIdioma = new PessoafisicaIdioma();

                objPessoaFisicaIdioma.PessoaFisica = objPessoaFisica;
                objPessoaFisicaIdioma.Idioma = new Idioma(Convert.ToInt32(ddlIdioma.SelectedValue));
                objPessoaFisicaIdioma.NivelIdioma = new NivelIdioma(Convert.ToInt32(rblNivelIdioma.SelectedValue));
                objPessoaFisicaIdioma.FlagInativo = false;
                objPessoaFisicaIdioma.Save();
            }
        }
        #endregion

        #region ExisteGraduacao
        /// <summary>
        /// Validar graduacao
        /// </summary>
        /// <returns></returns>
        public bool ExisteGraduacao()
        {
            const int idGraduacaoSuperior = 13; //Código identificador de Formacao Graduacao
            const int idGraduacaoTecnologo = 12; //Código identificador de Formacao Graduacao

            Formacao objFormacao;
            if (Formacao.CarregarPorPessoaFisicaEscolaridade((int)IdPessoaFisica, idGraduacaoSuperior, out objFormacao) ||
                Formacao.CarregarPorPessoaFisicaEscolaridade((int)IdPessoaFisica, idGraduacaoTecnologo, out objFormacao))
                return true;

            return false;
        }
        #endregion

        #endregion

        #region AjaxMehods

        #region ValidarCidade
        /// <summary>
        /// Validar cidade
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static bool ValidarCidade(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
                return true;

            Cidade objCidade;
            return Cidade.CarregarPorNome(valor, out objCidade);
        }
        #endregion

        #region RecuperarValorDropDownIdioma
        /// <summary>
        /// Validar cidade
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static string RecuperarValorDropDownIdioma(string valor)
        {
            valor = valor.Trim();
            if (string.IsNullOrEmpty(valor))
                return "0";

            Idioma objIdioma;
            if (Idioma.CarregarPorNome(valor, out objIdioma))
                return objIdioma.IdIdioma.ToString(CultureInfo.CurrentCulture);
            return "0";
        }
        #endregion

        #region RecuperarValorDropDownNivel
        /// <summary>
        /// Validar Nivel
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static string RecuperarValorDropDownNivel(string valor)
        {
            valor = valor.Trim();
            if (string.IsNullOrEmpty(valor))
                return "0";
            Escolaridade objEscolaridade;

            if (Escolaridade.CarregarPorNome(valor, out objEscolaridade))
                return objEscolaridade.IdEscolaridade.ToString(CultureInfo.CurrentCulture);
            return "0";
        }
        #endregion

        #region RecuperarInstituicao
        /// <summary>
        /// Validar cidade
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static string RecuperarInstituicao(string valor)
        {
            valor = valor.Trim();
            if (string.IsNullOrEmpty(valor))
                return String.Empty;

            string[] instituicao = valor.Split('-');
            if (instituicao.Length > 1)
                valor = instituicao[1].Trim();

            Fonte objFonte;
            if (Fonte.CarregarPorNome(valor, out objFonte))
            {
                if (objFonte.FlagAuditada)
                {
                    var parametros = new
                    {
                        IdFonte = objFonte.IdFonte.ToString(CultureInfo.CurrentCulture),
                        SiglaFonte = objFonte.SiglaFonte,
                        FlagAuditada = objFonte.FlagAuditada

                    };
                    return new JSONReflector(parametros).ToString();
                }
                return String.Empty;
            }
            return String.Empty;
        }
        #endregion

        #region RecuperarCurso
        /// <summary>
        /// Validar cidade
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static string RecuperarCurso(string valor)
        {
            valor = valor.Trim();
            if (string.IsNullOrEmpty(valor))
                return String.Empty;

            Idioma objIdioma;
            if (Idioma.CarregarPorNome(valor, out objIdioma))
                return objIdioma.IdIdioma.ToString(CultureInfo.CurrentCulture);
            return "0";
        }
        #endregion

        #region ValidarFormacao
        /// <summary>
        /// Validar formacao
        /// </summary>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static bool ValidarFormacao()
        {
            int idPessoaFisica = Convert.ToInt32(HttpContext.Current.Session[Chave.Temporaria.Variavel1.ToString()]);
            return Formacao.ExisteFormacaoInformada(idPessoaFisica);
        }
        #endregion

        #endregion

    }
}