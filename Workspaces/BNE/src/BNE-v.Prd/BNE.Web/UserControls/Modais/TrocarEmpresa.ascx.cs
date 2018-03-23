using System;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Handlers;

namespace BNE.Web.UserControls.Modais
{
    public partial class TrocarEmpresa : BaseUserControl
    {
        #region Propriedades

        #region PageIndex
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int PageIndex
        {
            get
            {
                if (ViewState[Chave.Temporaria.PageIndex.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.PageIndex.ToString()].ToString());
                else
                    return 1;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.PageIndex.ToString(), value);
            }
        }
        #endregion

        #region DesPesquisa - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public string DesPesquisa
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return ViewState[Chave.Temporaria.Variavel1.ToString()].ToString();
                return string.Empty;
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

        public delegate void delegateEmpresaSelecionada(string urlRedirectEmpresa);
        public event delegateEmpresaSelecionada EmpresaSelecionada;

        #region Eventos

        #region Page_Load

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #endregion

        #region btnCancelarTrocaEmpresa_Click

        protected void btnCancelarTrocaEmpresa_Click(object sender, EventArgs e)
        {
            FecharModal();
            upTrocarEmpresa.Update();
        }

        #endregion

        #region gvEmpresas_ItemCommand

        protected void gvEmpresas_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("SelecionarEmpresa"))
            {
                int idFilial = Convert.ToInt32(gvEmpresas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Filial"]);
                int idUsuarioFilialPerfil = Convert.ToInt32(gvEmpresas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Usuario_Filial_Perfil"]);

                VerificarLoginEmpresa(base.IdPessoaFisicaLogada.Value, idFilial);

                base.IdFilial.Value = idFilial;
                base.IdUsuarioFilialPerfilLogadoEmpresa.Value = idUsuarioFilialPerfil;

                Usuario objUsuario;
                if (!Usuario.CarregarPorPessoaFisica(base.IdPessoaFisicaLogada.Value, out objUsuario))
                {
                    objUsuario = new Usuario();
                    objUsuario.PessoaFisica = PessoaFisica.LoadObject(base.IdPessoaFisicaLogada.Value);
                    objUsuario.SenhaUsuario = objUsuario.PessoaFisica.DataNascimento.ToString("ddMMyyyy");
                }

                objUsuario.UltimaFilialLogada = new Filial(base.IdFilial.Value);
                objUsuario.Save();
                
                if (EmpresaSelecionada != null)
                    EmpresaSelecionada("SalaSelecionador.aspx");
                else
                    Redirect("SalaSelecionador.aspx");
            }
        }
        #endregion

        #region gvEmpresas_PageIndexChanged

        protected void gvEmpresas_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            PageIndex = e.NewPageIndex + 1;
            CarregarGridEmpresa();
            upTrocarEmpresa.Update();
        }

        #endregion

        #region btiFechar_Click

        protected void btiFechar_Click(object sender, EventArgs e)
        {
            FecharModal();
        }

        #endregion

        #region btnPesquisar_Click

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxFiltroBusca.Text))
            {
                DesPesquisa = tbxFiltroBusca.Text;
                CarregarGridEmpresa();
            }
            else
            {
                DesPesquisa = string.Empty;
                CarregarGridEmpresa();
            }
        }

        #endregion

        #endregion

        #region Metodos

        #region Inicializar

        public void Inicializar()
        {
            //Setando propriedades da radgrid
            gvEmpresas.PageSize = 6;

            CarregarGridEmpresa();
        }

        #endregion

        #region MostrarModal

        public void MostrarModal()
        {
            mpeTrocarEmpresa.Show();
        }

        #endregion

        #region FecharModal

        public void FecharModal()
        {
            mpeTrocarEmpresa.Hide();
        }

        #endregion

        #region CarregarGridEmpresa

        public void CarregarGridEmpresa()
        {
            int totalRegistros;
            UIHelper.CarregarRadGrid(gvEmpresas, UsuarioFilialPerfil.RetornarUsuarioFilialPerfil(base.IdPessoaFisicaLogada.Value, PageIndex, gvEmpresas.PageSize, DesPesquisa, out totalRegistros), totalRegistros);
            upTrocarEmpresa.Update();
        }

        #endregion

        #region VerificaFilialLogado
        /// <summary>
        /// Verifica se o IdFilialSession Informado é igual ao IdFilialSession Logado
        /// </summary>
        /// <param name="idfFilial"></param>
        /// <returns></returns>
        public bool VerificaFilialLogado(object idfFilialParm)
        {
            int idfFilial = Convert.ToInt32(idfFilialParm);

            if (base.IdFilial.HasValue)
            {
                if (base.IdFilial.Value.Equals(idfFilial))
                    return true;
                return false;
            }
            return false;
        }

        #endregion

        #region VerificaLogoEmpresa
        /// <summary>
        /// Verifica se a empresa tem logomarca
        /// </summary>
        /// <param name="numCNPJ"></param>
        /// <returns></returns>
        public bool VerificaLogoEmpresa(object numCNPJ)
        {
             return PessoaJuridicaLogo.ExisteLogo(Convert.ToDecimal(numCNPJ));
        }
        #endregion

        #endregion
    }
}