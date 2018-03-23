using System;
using System.Web.UI.WebControls;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class SalaVipCursos : BasePage
    {

        #region Propriedades

        #region Funcao - Variavel 1
        /// <summary>
        /// Propriedade que armazena e recupera a Funcao
        /// </summary>
        protected string Funcao
        {
            get
            {
                if (RouteData.Values.Count > 0)
                {
                    if (RouteData.Values["Funcao"] != null)
                        return RouteData.Values["Funcao"].ToString();
                }

                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return ViewState[Chave.Temporaria.Variavel1.ToString()].ToString();
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

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            InicializarBarraBusca(TipoBuscaMaster.Vaga, true, GetType().ToString());
        }
        #endregion

        #region Inicializar
        private void Inicializar()
        {
            //Recuperando a função da rota
            Funcao = Funcao;
            //Ajustando a tela
            AjustarTituloTela("Cursos");
            //Carregando os cursos
            CarregarCursos(Funcao);
        }
        #endregion

        #region CarregarCursos
        private void CarregarCursos(string descricaoFuncao)
        {
            if (string.IsNullOrEmpty(descricaoFuncao))
            {
                UIHelper.CarregarRepeater(rptCursosEspecificos, CursoParceiroTecla.CarregarCursos());
                litCursos.Text = "Todos os cursos";
            }
            else
            {
                var objFuncao = BLL.Funcao.CarregarPorDescricao(Funcao);
                if (objFuncao != null)
                {
                    UIHelper.CarregarRepeater(rptCursosEspecificos, CursoParceiroTecla.CarregarCursosPorFuncao(objFuncao.IdFuncao));
                    if (rptCursosEspecificos.Items.Count.Equals(0))
                        pnlCursosEspecificos.Visible = false;
                    litCursos.Text = "Cursos no seu perfil";

                    UIHelper.CarregarRepeater(rptOutrosCursos, CursoParceiroTecla.CarregarOutrosCursos(objFuncao.IdFuncao));
                    pnlOutrosCursos.Visible = true;
                }
                else
                    UIHelper.CarregarRepeater(rptCursosEspecificos, CursoParceiroTecla.CarregarCursos());
            }
        }
        #endregion

        #region rptCursos_ItemCommand
        protected void rptCursos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("detalhes"))
            {
                var idCurso = Convert.ToInt32(e.CommandArgument);

                Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.Curso.ToString(), new { NomeCurso = CursoParceiroTecla.RecuperarNomeCursoURL(idCurso) }));
            }
        }
        #endregion

    }
}