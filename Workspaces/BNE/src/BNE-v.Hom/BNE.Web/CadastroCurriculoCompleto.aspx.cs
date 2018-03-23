using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Enumeradores = BNE.BLL.Enumeradores;
using System.Net;
using BNE.BLL.Custom.Solr.Buffer;

namespace BNE.Web
{
    public partial class CadastroCurriculoCompleto : BasePage
    {

        #region Propriedades

        #region IdCurriculo - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID do Currículo.
        /// </summary>
        public int IdCurriculo
        {
            get
            {
                return Int32.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion

        #region UrlOrigem - Variável 3
        /// <summary>
        /// </summary>
        public string UrlOrigem
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel3.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel3.ToString()]).ToString();
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

        #region IdSituacaoCurriculoAntesDeSalvar - Variável 4
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Situação do Currículo antes de Salvá-lo no Banco
        /// </summary>
        public int IdSituacaoCurriculoAntesDeSalvar
        {
            get
            {
                return Int32.Parse(ViewState[Chave.Temporaria.Variavel4.ToString()].ToString());
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel4.ToString(), value);
            }
        }
        #endregion

        #region EnumSituacaoCurriculo - Variavel5
        /// <summary>
        /// Propriedade que armazena e recupera um enum situação curriculo setado pelo usuário administrador no curriculo completo
        /// </summary>
        public Enumeradores.SituacaoCurriculo? EnumSituacaoCurriculo
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel5.ToString()] != null)
                    return (Enumeradores.SituacaoCurriculo)(ViewState[Chave.Temporaria.Variavel5.ToString()]);

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

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.UrlReferrer != null && !Request.UrlReferrer.AbsoluteUri.Contains("CadastroCurriculoCompleto"))
                    UrlOrigem = Request.UrlReferrer.AbsoluteUri;

                gvCritica.PageSize = 100;

                Curriculo objCurriculo = Curriculo.LoadObject(IdCurriculo);

                ucObservacaoCurriculo.IdCurriculo = objCurriculo.IdCurriculo;

                AjustarBotoes(objCurriculo);
                PreencherCampos(objCurriculo);
                CarregarGridCritica();
            }

            InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "CadastroCurriculoCompleto");
        }
        #endregion

        #region btnSalvarCurriculo_Click
        protected void btnSalvarCurriculo_Click(object sender, EventArgs e)
        {
            //TODO: Merda de código, da tela inteira
            try
            {
                if (pnlSituacaoCurriculo.Visible)
                {
                    if (Convert.ToInt32(rcbSituacaoCurriculo.SelectedValue).Equals(Enumeradores.SituacaoCurriculo.Invisivel.GetHashCode()))
                        EnumSituacaoCurriculo = Enumeradores.SituacaoCurriculo.Invisivel;
                    else if (Convert.ToInt32(rcbSituacaoCurriculo.SelectedValue).Equals(Enumeradores.SituacaoCurriculo.Bloqueado.GetHashCode()))
                        EnumSituacaoCurriculo = Enumeradores.SituacaoCurriculo.Bloqueado;
                    else if (Convert.ToInt32(rcbSituacaoCurriculo.SelectedValue).Equals(Enumeradores.SituacaoCurriculo.Cancelado.GetHashCode()))
                        EnumSituacaoCurriculo = Enumeradores.SituacaoCurriculo.Cancelado;
                }

                Curriculo objCurriculo = Curriculo.LoadObject(IdCurriculo);
                SituacaoCurriculo objSituacaoCurriculoAntiga = null;
                if (base.IdPerfil.Value == (int)Enumeradores.Perfil.AdministradorSistema) //TODO: Recupera o perfil do usuário filial perfil, além disso ver qual é o Tipo do Perfil
                {
                    objCurriculo.SituacaoCurriculo.CompleteObject();
                    objSituacaoCurriculoAntiga = (SituacaoCurriculo)objCurriculo.SituacaoCurriculo.Clone();
                }

                string mensagemErro;
                if (Salvar(out mensagemErro))
                {

                    if (EnumSituacaoCurriculo.HasValue)
                    {
                        if (base.IdPerfil.Value == (int)Enumeradores.Perfil.AdministradorSistema)
                        {
                            //TODO: Performance, analizar se é possível chamar um método e atualizar apenas a Situação
                            objCurriculo.AlterarSituacao(new SituacaoCurriculo(Convert.ToInt32(rcbSituacaoCurriculo.SelectedValue)));
                            NotificiarAlteracaoSituacao(objSituacaoCurriculoAntiga, SituacaoCurriculo.LoadObject(Convert.ToInt32(rcbSituacaoCurriculo.SelectedValue)), objCurriculo);
                        }

                        Redirect("SalaAdministradorEdicaoCV.aspx");
                    }
                    else
                    {
                        CarregarGridCritica();

                        //Caso ainda tenha críticas ao currículo
                        if (gvCritica.Items.Count > 0)
                            ExibirMensagem("Para finalizar a edição nenhum campo pode ter crítica.", TipoMensagem.Erro);
                        else
                        {
                            //Se for o administrador irá alterar a situação do curriculo
                            if (base.IdPerfil.Value == (int)Enumeradores.Perfil.AdministradorSistema)
                            {
                                //TODO: Performance, analizar se é possível chamar um método e atualizar apenas a Situação
                                objCurriculo.AlterarSituacao(new SituacaoCurriculo(Convert.ToInt32(rcbSituacaoCurriculo.SelectedValue)));
                                NotificiarAlteracaoSituacao(objSituacaoCurriculoAntiga, SituacaoCurriculo.LoadObject(Convert.ToInt32(rcbSituacaoCurriculo.SelectedValue)), objCurriculo);
                            }

                        }

                        Redirect("SalaAdministradorEdicaoCV.aspx");
                    }
                }
                else
                {
                    ExibirMensagem(mensagemErro, TipoMensagem.Erro);
                }
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }

        private void NotificiarAlteracaoSituacao(SituacaoCurriculo objSituacaoCurriculoAntiga, SituacaoCurriculo objSituacaoCurriculoNova, Curriculo objCurriculo)
        {
            if (objSituacaoCurriculoAntiga != null && objSituacaoCurriculoNova != null)
            {
                List<CompareObject.CompareResult> listaAlteracoes = CompareObject.CompareList(objSituacaoCurriculoAntiga, objSituacaoCurriculoNova, new[] { "IdSituacaoCurriculo", "FlagInativo", "DataCadastro" });

                if (listaAlteracoes.Any()) //Se possui alteração
                {
                    string alteracoes = "O status do currículo foi alterado: " + listaAlteracoes.FirstOrDefault().Value1 + " para " + listaAlteracoes.FirstOrDefault().Value2 + " <br/> ";

                    var objCurriculoObservacao = new CurriculoObservacao
                    {
                        DescricaoObservacao = alteracoes,
                        Curriculo = objCurriculo,
                        FlagSistema = true,
                        UsuarioFilialPerfil = new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value)
                    };
                    objCurriculoObservacao.Save();
                }
            }
        }

        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Redirect(UrlOrigem);
        }
        #endregion

        #endregion

        #region Métodos

        #region CarregarGridCritica
        private void CarregarGridCritica()
        {
            UIHelper.CarregarRadGrid(gvCritica, Curriculo.RecuperarCriticas(IdCurriculo, Resources.Configuracao.MensagemPadraoCriticaValidacao));
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos(Curriculo objCurriculo)
        {
            litCodigo.Text = objCurriculo.IdCurriculo.ToString(CultureInfo.CurrentCulture);
            litCadastradoEm.Text = objCurriculo.DataCadastro.ToShortDateString();

            ucMiniCurriculo.EstadoManutencao = true;
            ucMiniCurriculo.IdPessoaFisica = objCurriculo.PessoaFisica.IdPessoaFisica;
            ucDadosPessoais.EstadoManutencao = true;
            ucDadosPessoais.IdPessoaFisica = objCurriculo.PessoaFisica.IdPessoaFisica;
            ucFormacaoCursos.EstadoManutencao = true;
            ucFormacaoCursos.IdPessoaFisica = objCurriculo.PessoaFisica.IdPessoaFisica;
            ucDadosComplementares.EstadoManutencao = true;
            ucDadosComplementares.IdPessoaFisica = objCurriculo.PessoaFisica.IdPessoaFisica;
        }
        #endregion

        #region Salvar
        private bool Salvar(out string mensagemErro)
        {
            Curriculo objCurriculo = Curriculo.LoadObject(IdCurriculo);
            IdSituacaoCurriculoAntesDeSalvar = objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo;

            if (EnumSituacaoCurriculo.HasValue)
            {
                ucMiniCurriculo.EnumSituacaoCurriculo = EnumSituacaoCurriculo;
                ucFormacaoCursos.EnumSituacaoCurriculo = EnumSituacaoCurriculo;
                ucDadosPessoais.EnumSituacaoCurriculo = EnumSituacaoCurriculo;
                ucDadosComplementares.EnumSituacaoCurriculo = EnumSituacaoCurriculo;
            }

            if (!ucMiniCurriculo.Salvar(out mensagemErro))
                return false;

            if (!ucDadosPessoais.Salvar(out mensagemErro))
                return false;

            ucFormacaoCursos.Salvar();
            ucDadosComplementares.Salvar();
            objCurriculo = Curriculo.LoadObject(IdCurriculo);

            
            if (objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo != (int)BLL.Enumeradores.SituacaoCurriculo.Bloqueado
                && objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo != (int)BLL.Enumeradores.SituacaoCurriculo.Cancelado)
                BufferAtualizacaoCurriculo.Update(objCurriculo);

            return true;
        }
        #endregion



        #region AjustarBotoes
        private void AjustarBotoes(Curriculo objCurriculo)
        {
            if (base.IdPerfil.HasValue)
            {
                UIHelper.CarregarRadComboBox(rcbSituacaoCurriculo, SituacaoCurriculo.Listar(), "Idf_Situacao_Curriculo", "Des_Situacao_Curriculo");
                rcbSituacaoCurriculo.SelectedValue = objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo.ToString(CultureInfo.CurrentCulture);
                pnlSituacaoCurriculo.Visible = true;
                rcbSituacaoCurriculo.Enabled = false;
            }
        }
        #endregion

        #endregion

    }
}