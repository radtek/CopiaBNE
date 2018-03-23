using System;
using System.Globalization;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls
{
    public partial class ucAvaliacaoCurriculo : BaseUserControl
    {

        #region Propriedades

        #region IdCurriculoAvaliacaoCurriculo - Variavel1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        private int IdCurriculoAvaliacaoCurriculo
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

        #region IdFilialAvaliacaoCurriculo - Variavel2
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        private int IdFilialAvaliacaoCurriculo
        {
            get
            {
                return Int32.Parse(ViewState[Chave.Temporaria.Variavel2.ToString()].ToString());
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel2.ToString(), value);
            }
        }
        #endregion

        #region IdfUsuarioFilialPerfil - Variavel 3
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        private int IdfUsuarioFilialPerfil
        {
            get
            {
                return Int32.Parse(ViewState[Chave.Temporaria.Variavel3.ToString()].ToString());
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel3.ToString(), value);
            }
        }
        #endregion

        #region IdClassificacaoCurriculo - Variavel 4
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        private int? IdClassificacaoCurriculo
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

        #endregion

        #region Delegate
        public delegate void DelegateCurriculoAvaliado();
        public event DelegateCurriculoAvaliado CurriculoAvaliado;
        #endregion

        #region Eventos

        #region btnClassificar_Click
        protected void btnClassificar_Click(object sender, EventArgs e)
        {
            bool avaliou = false;
            if (rbPositiva.Checked)
                avaliou = AvaliarAtendimento(Enumeradores.CurriculoClassificacao.AvaliacaoPositiva);
            else if (rbNegativa.Checked)
                avaliou = AvaliarAtendimento(Enumeradores.CurriculoClassificacao.AvaliacaoNegativa);
            else if (rbSemClassificacao.Checked)
                avaliou = AvaliarAtendimento(Enumeradores.CurriculoClassificacao.AvaliacaoNeutra);

            if (CurriculoAvaliado != null && avaliou)
                CurriculoAvaliado();
        }
        #endregion

        #region cvAvaliacao_ServerValidate
        protected void cvAvaliacao_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            if (!rbPositiva.Checked && !rbSemClassificacao.Checked && !rbNegativa.Checked)
                args.IsValid = false;
        }
        #endregion

        #region gvClassificacoes_ItemCommand
        protected void gvClassificacoes_ItemCommand(object source, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Atualizar"))
            {
                var gvr = (GridViewRow)(((WebControl)e.CommandSource).NamingContainer);
                IdClassificacaoCurriculo = Convert.ToInt32(gvClassificacoes.DataKeys[gvr.RowIndex].Values["Idf_Curriculo_Classificacao"]);

                var objCurriculoClassificacao = CurriculoClassificacao.LoadObject(IdClassificacaoCurriculo.Value);

                txtObservacoes.Text = objCurriculoClassificacao.DescricaoObservacao.Trim();
                switch ((Enumeradores.CurriculoClassificacao)Enum.Parse(typeof(Enumeradores.CurriculoClassificacao), objCurriculoClassificacao.Avaliacao.IdAvaliacao.ToString(CultureInfo.CurrentCulture)))
                {
                    case Enumeradores.CurriculoClassificacao.AvaliacaoPositiva:
                        rbPositiva.Checked = true;
                        break;
                    case Enumeradores.CurriculoClassificacao.AvaliacaoNegativa:
                        rbNegativa.Checked = true;
                        break;
                    case Enumeradores.CurriculoClassificacao.AvaliacaoNeutra:
                        rbSemClassificacao.Checked = true;
                        break;
                }

                upAvaliacaoCurriculo.Update();
            }
            if (e.CommandName.Equals("Inativar"))
            {
                var gvr = (GridViewRow)(((WebControl)e.CommandSource).NamingContainer);
                var idCurriculoAvaliacaoCurriculo = Convert.ToInt32(gvClassificacoes.DataKeys[gvr.RowIndex].Values["Idf_Curriculo_Classificacao"]);

                CurriculoClassificacao.Inativar(idCurriculoAvaliacaoCurriculo);

                ExibirMensagem(MensagemAviso._100015, TipoMensagem.Aviso);

                LimparCampos();

                CarregarGrid();
            }
        }
        #endregion

        #region gvClassificacoes_PageIndexChanging
        protected void gvClassificacoes_PageIndexChanging(object source, GridViewPageEventArgs e)
        {
            gvClassificacoes.MockPageIndex = e.NewPageIndex;
            gvClassificacoes.PageIndex = e.NewPageIndex;
            CarregarGrid();
        }
        #endregion

        #region gvClassificacoes_RowDataBound
        protected void gvClassificacoes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var imgAvaliacao = (LinkButton)e.Row.FindControl("imgAvaliacao");

                //Ajusando imagem de avaliação
                if (!String.IsNullOrEmpty(imgAvaliacao.CommandArgument))
                {
                    if (Convert.ToInt32(imgAvaliacao.CommandArgument) == (int)Enumeradores.CurriculoClassificacao.AvaliacaoNegativa)
                        imgAvaliacao.CssClass = "fa fa-frown-o fa-2x";
                    else if (Convert.ToInt32(imgAvaliacao.CommandArgument) == (int)Enumeradores.CurriculoClassificacao.AvaliacaoPositiva)
                        imgAvaliacao.CssClass = "fa fa-smile-o fa-2x";
                    else if (Convert.ToInt32(imgAvaliacao.CommandArgument) == (int)Enumeradores.CurriculoClassificacao.AvaliacaoNeutra)
                        imgAvaliacao.CssClass = "fa fa-meh-o fa-2x";
                }
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region CarregarGrid
        private void CarregarGrid()
        {
            int totalRegistros;
            var objFilial = new Filial(IdFilialAvaliacaoCurriculo);
            var objUsuarioFilialPerfil = objFilial.EmpresaAssociacao() ? new UsuarioFilialPerfil(IdfUsuarioFilialPerfil) : null;

            UIHelper.CarregarGridViewEmployer(gvClassificacoes, CurriculoClassificacao.ListarObservacoes(objFilial, objUsuarioFilialPerfil, new Curriculo(IdCurriculoAvaliacaoCurriculo), gvClassificacoes.PageIndex, gvClassificacoes.PageSize, out totalRegistros), totalRegistros);
            upGvClassificacoes.Update();
        }
        #endregion

        #region Inicializar
        public void Inicializar(Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, Curriculo objCurriculo)
        {
            IdCurriculoAvaliacaoCurriculo = objCurriculo.IdCurriculo;
            IdFilialAvaliacaoCurriculo = objFilial.IdFilial;
            IdfUsuarioFilialPerfil = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;

            LimparCampos();

            CarregarGrid();
        }
        #endregion

        #region AvaliarAtendimento
        private bool AvaliarAtendimento(BLL.Enumeradores.CurriculoClassificacao classificacao)
        {
            CurriculoClassificacao.SalvarAvaliacao(new Filial(IdFilialAvaliacaoCurriculo), new UsuarioFilialPerfil(IdfUsuarioFilialPerfil), new Curriculo(IdCurriculoAvaliacaoCurriculo), IdClassificacaoCurriculo, txtObservacoes.Text.Trim(), classificacao);
            CurriculoClassificacao.EnviarAvaliacaoSolr(new Curriculo(IdCurriculoAvaliacaoCurriculo));
            CarregarGrid();
            LimparCampos();

            return true;
        }
        #endregion

        #region LimparCampos
        private void LimparCampos()
        {
            IdClassificacaoCurriculo = null;
            txtObservacoes.Text = String.Empty;
            rbPositiva.Checked = false;
            rbNegativa.Checked = false;
            rbSemClassificacao.Checked = false;

            upAvaliacaoCurriculo.Update();
        }
        #endregion

        #region BotaoVisivel
        protected bool BotaoVisivel(int idUsuarioFilialPerfil)
        {
            return base.IdUsuarioFilialPerfilLogadoEmpresa.Value.Equals(idUsuarioFilialPerfil);
        }
        #endregion

        #endregion

    }
}