using System;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Resources;

namespace BNE.Web.UserControls
{
    public partial class ucObservacaoFilial : BaseUserControl
    {

        #region Propriedades

        #region IdFilialObservacao - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Filial Observacao
        /// </summary>
        public int? IdFilialObservacao
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

        #region IdFilial - Variável 2
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Filial
        /// </summary>
        public int IdFilial
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
                CarregarGrid();
                IdFilialObservacao = null;
                txtObservacoes.Valor = string.Empty;
                upTxtObservacoes.Update();
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
                IdFilialObservacao = Convert.ToInt32(gvObservacoes.DataKeys[gvr.RowIndex].Values["Idf_Filial_Observacao"]);

                txtObservacoes.Valor = new FilialObservacao((int)IdFilialObservacao).RecuperarDescricao();
                //CarregarGrid();
                upTxtObservacoes.Update();
            }
            if (e.CommandName.Equals("Inativar"))
            {
                var gvr = (GridViewRow)(((WebControl)e.CommandSource).NamingContainer);
                int idFilialObservacao = Convert.ToInt32(gvObservacoes.DataKeys[gvr.RowIndex].Values["Idf_Filial_Observacao"]);

                FilialObservacao.Inativar(idFilialObservacao);

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
            gvObservacoes.MockPageIndex = 0;
            gvObservacoes.PageIndex = 0;
            CarregarGrid();
        }
        #endregion

        #region Salvar
        private void Salvar()
        {
            FilialObservacao objFilialObservacao = IdFilialObservacao.HasValue ? FilialObservacao.LoadObject((int)IdFilialObservacao) : new FilialObservacao();

            objFilialObservacao.DescricaoObservacao = txtObservacoes.Valor.Trim();
            if (txtDtaProximoPasso.ValorDatetime.HasValue)
                objFilialObservacao.DataProximaAcao = txtDtaProximoPasso.ValorDatetime.Value;
            else
                objFilialObservacao.DataProximaAcao = null;
            if (!string.IsNullOrWhiteSpace(txtDescricaoProximoPasso.Text))
                objFilialObservacao.DescricaoProximaAcao = txtDescricaoProximoPasso.Text;
            else
                objFilialObservacao.DescricaoProximaAcao = string.Empty;
            objFilialObservacao.FlagInativo = false;
            objFilialObservacao.Filial = new Filial((int)IdFilial);
            objFilialObservacao.UsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value);
            objFilialObservacao.Save();

            txtDescricaoProximoPasso.Text = " ";
            txtDtaProximoPasso.Valor = " ";
        }
        #endregion

        #region CarregarGrid
        private void CarregarGrid()
        {
            int totalRegistros;
            UIHelper.CarregarGridViewEmployer(gvObservacoes, FilialObservacao.ListarObservacoes(new Filial((int)IdFilial), gvObservacoes.PageIndex, gvObservacoes.PageSize, out totalRegistros), totalRegistros);
            upGvObservacoes.Update();
        }
        #endregion

        #region BotaoVisivel
        protected bool BotaoVisivel(int idUsuarioFilialPerfil)
        {
            return base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value.Equals(idUsuarioFilialPerfil);
        }
        #endregion

        #endregion

    }
}