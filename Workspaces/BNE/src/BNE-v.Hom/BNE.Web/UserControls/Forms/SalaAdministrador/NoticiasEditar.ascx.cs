using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Code;
using BNE.BLL;
using Resources;
using BNE.EL;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class NoticiasEditar : BaseUserControl
    {

        #region Propriedades

        #region IdNoticia
        public int? IdNoticia
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return Convert.ToInt32(ViewState[Chave.Temporaria.Variavel1.ToString()]);
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {}

        public void Inicializar(int? idNoticia)
        {
            IdNoticia = idNoticia;
            if (IdNoticia.HasValue)
            {
                litModoTela.Text = "Editar Notícia";
                Noticia objNoticia = Noticia.LoadObject(IdNoticia.Value);
                txtTitulo.Valor = objNoticia.NomeTituloNoticia;
                reDescricao.Content = objNoticia.DescricaoNoticia;
                txtDataPublicacao.ValorDatetime = objNoticia.DataPublicacao;
                rdbSim.Checked = objNoticia.FlagExibicao;
                rdbNao.Checked = !objNoticia.FlagExibicao;
            }
            else
            {
                litModoTela.Text = "Nova Notícia";
                txtTitulo.Valor = String.Empty;
                reDescricao.Content = String.Empty;
                txtDataPublicacao.Valor = null;
                rdbSim.Checked = true;
            }
        }

        #region Salvar
        private void Salvar()
        {
            Noticia objNoticia = IdNoticia.HasValue ? Noticia.LoadObject(IdNoticia.Value) : new Noticia();

            objNoticia.NomeTituloNoticia = txtTitulo.Valor;
            objNoticia.DescricaoNoticia = reDescricao.Content;
            objNoticia.DataPublicacao = txtDataPublicacao.ValorDatetime;
            objNoticia.FlagExibicao = rdbSim.Checked;

            objNoticia.Save();
        }
        #endregion

        #region btnSalvar_Click
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar();
                ExibirMensagemConfirmacao("Confirmação de Cadastro", "Cadastro efetuado com sucesso!", false);
                Redirect("SalaAdministradorConfiguracoes.aspx");
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

    }
}