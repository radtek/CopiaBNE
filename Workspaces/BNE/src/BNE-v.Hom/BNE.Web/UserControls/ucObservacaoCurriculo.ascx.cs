using System;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Resources;

namespace BNE.Web.UserControls
{
    public partial class ucObservacaoCurriculo : BaseUserControl
    {
        #region Propriedades

        #region IdCurriculoObservacao - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Filial Observacao
        /// </summary>
        public int? IdCurriculoObservacao
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

        #region IdCurriculo - Variável 2
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Curriculo
        /// </summary>
        public int IdCurriculo
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

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();
        }
        #endregion

        #region btnSalvar_Click
        /// <summary>
        /// habilitar aba Dados Pessoais
        /// </summary>
        /// <param name="sender">Objeto correspondente</param>
        /// <param name="e">Argumento do evento</param>        
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar();
                LimparCampos();
                CarregarGrid();
                
                ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region gvObservacoes_ItemCommand
        protected void gvObservacoes_ItemCommand(object source, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Atualizar"))
            {
                var gvr = (GridViewRow)(((WebControl)e.CommandSource).NamingContainer);
                IdCurriculoObservacao = Convert.ToInt32(gvObservacoes.DataKeys[gvr.RowIndex].Values["Idf_Curriculo_Observacao"]);

                txtObservacoes.Valor = new CurriculoObservacao((int)IdCurriculoObservacao).RecuperarDescricao();
                
                upTxtObservacoes.Update();
            }
            if (e.CommandName.Equals("Inativar"))
            {
                var gvr = (GridViewRow)(((WebControl)e.CommandSource).NamingContainer);
                int idCurriculoObservacao = Convert.ToInt32(gvObservacoes.DataKeys[gvr.RowIndex].Values["Idf_Curriculo_Observacao"]);

                CurriculoObservacao.Inativar(idCurriculoObservacao);

                ExibirMensagem(MensagemAviso._100015, TipoMensagem.Aviso);

                CarregarGrid();
            }
        }
        #endregion

        #region gvObservacoes_PageIndexChanging
        protected void gvObservacoes_PageIndexChanging(object source, GridViewPageEventArgs e)
        {
            gvObservacoes.MockPageIndex = e.NewPageIndex;
            gvObservacoes.PageIndex = e.NewPageIndex;
            CarregarGrid();
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        public void Inicializar()
        {
            //gvObservacoes.MockPageIndex = 0;
            gvObservacoes.PageIndex = 0;
            CarregarGrid();
            CarregarMotivos(IdCurriculo);
        }
        #endregion

        #region Salvar
        private void Salvar()
        {
            CurriculoObservacao objCurriculoObservacao = IdCurriculoObservacao.HasValue ? CurriculoObservacao.LoadObject((int)IdCurriculoObservacao) : new CurriculoObservacao();

            objCurriculoObservacao.DescricaoObservacao = txtObservacoes.Valor.Trim();
            objCurriculoObservacao.FlagInativo = false;
            objCurriculoObservacao.Curriculo = new Curriculo(IdCurriculo);
            objCurriculoObservacao.UsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value);
            objCurriculoObservacao.Save();
        }
        #endregion

        #region CarregarGrid
        private void CarregarGrid()
        {
            int totalRegistros;
            UIHelper.CarregarGridViewEmployer(gvObservacoes, CurriculoObservacao.ListarObservacoes(new Curriculo((int)IdCurriculo), gvObservacoes.PageIndex, gvObservacoes.PageSize, out totalRegistros), totalRegistros);
            upGvObservacoes.Update();
        }
        #endregion

        #region CarregarMotivos
        public void CarregarMotivos(int IDCurriculo)
        {
            UIHelper.CarregarGridViewEmployer(gvBronquinha, Curriculo.LsitarMotivosBronquinha(IdCurriculo), gvBronquinha.PageSize);
        }
        #endregion

        #region BotaoVisivel
        protected bool BotaoVisivel(int idUsuarioFilialPerfil)
        {
            return base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value.Equals(idUsuarioFilialPerfil);
        }
        #endregion

        #region LimparCampos
        private void LimparCampos()
        {
            IdCurriculoObservacao = null;
            txtObservacoes.Valor = string.Empty;
            upTxtObservacoes.Update();
        }
        #endregion

        #endregion
    }
}