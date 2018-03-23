using System;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class SalaVipCurso : BasePage
    {

        #region IdCurso - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaVaga
        /// </summary>
        protected int IdCurso
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

        #region NomeCurso - Variavel 1
        /// <summary>
        /// Propriedade que armazena e recupera a Funcao
        /// </summary>
        protected string NomeCurso
        {
            get
            {
                if (RouteData.Values.Count > 0)
                {
                    if (RouteData.Values["NomeCurso"] != null)
                        return RouteData.Values["NomeCurso"].ToString();
                }

                if (ViewState[Chave.Temporaria.Variavel2.ToString()] != null)
                    return ViewState[Chave.Temporaria.Variavel2.ToString()].ToString();

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

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            ucModalLogin.Logar += ucModalLogin_Logar;
            InicializarBarraBusca(TipoBuscaMaster.Vaga, true, GetType().ToString());
        }
        #endregion

        #region ucModalLogin_Logar
        void ucModalLogin_Logar(string urlDestino)
        {
            Matricular();
        }
        #endregion

        #region Inicializar
        private void Inicializar()
        {
            //Recuperando o nome curso da rota
            NomeCurso = NomeCurso;
            IdCurso = CursoParceiroTecla.RecuperarCursoPorNomeURL(NomeCurso);
            //AjustarTituloTela(CursoParceiroTecla.RecuperarNomeCurso(IdCurso));

            PreencherCampos();

            var parametros = new
                {
                    URL = string.Format("http://{0}{1}", Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLAmbiente), Page.GetRouteUrl(Enumeradores.RouteCollection.Curso.ToString(), new { NomeCurso = NomeCurso }))
                };

            litLikeButtonFacebook.Text = parametros.ToString("<fb:like href='{URL}' send='false' layout='button_count' width='450' show_faces='false'></fb:like>");
            litComentariosFacebook.Text = parametros.ToString("<fb:comments href='{URL}' width='470' num_posts='10'></fb:comments>");
            litLikeButtonFacesFacebook.Text = parametros.ToString("<fb:like-box href='http://www.facebook.com/banconacionaldeempregos' width='292' show_faces='true' stream='false' header='false'></fb:like-box>");
            litTweetButton.Text = parametros.ToString("<a href='{URL}' class='twitter-share-button' data-lang='en' data-size='medium' data-count='horizontal'>Tweet</a>");
        }
        #endregion

        #region btnQueroCursar_Click
        protected void btnQueroCursar_Click(object sender, EventArgs e)
        {
            Matricular();
        }
        #endregion

        #region btlMatriculese_OnClick
        protected void btlMatriculese_OnClick(object sender, EventArgs e)
        {
            Matricular();
        }
        #endregion

        #region Métodos

        #region Matricular
        private void Matricular()
        {
            if (base.IdPessoaFisicaLogada.HasValue)
            {
                var objPessoaFisica = new PessoaFisica(base.IdPessoaFisicaLogada.Value);
                if (string.IsNullOrEmpty(objPessoaFisica.EmailPessoa))
                    ExibirMensagem("Informe um e-mail no Cadastro de Currículo.", TipoMensagem.Erro);
                else
                {
                    //Recupera o curso em questão
                    var objCursoParceiro = CursoParceiroTecla.LoadObject(IdCurso);

                    string login, senha;
                    if (InscricaoCurso.Matricular(new Curriculo(base.IdCurriculo.Value), objCursoParceiro, out login, out senha))
                    {
                        var parametros = new
                            {
                                login = login,
                                senha = senha,
                                url = objCursoParceiro.DescricaoURLCursoTecla
                            };
                        string json = Helper.ToJSON(parametros);
                        string url = string.Format(objCursoParceiro.ParceiroTecla.DescricaoURLAutenticacao, Helper.ToBase64(json));

                        hlSiteParceiro.NavigateUrl = url;
                        upSiteParceiro.Update();

                        mpeRedirecionamento.Show();
                    }
                    else
                        ExibirMensagem("Houve um erro em sua matrícula, tente novamente!", TipoMensagem.Erro);
                }
            }
            else
            {
                ucModalLogin.Inicializar();
                ucModalLogin.Mostrar();
            }
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos()
        {
            var objCursoParceiro = CursoParceiroTecla.LoadObject(IdCurso);
            litDescricao.Text = objCursoParceiro.DescricaoCurso;
            litConteudo.Text = objCursoParceiro.DescricaoConteudo;
            litInstrutor.Text = objCursoParceiro.DescricaoInstrutorCurso;
            litInstrutorAssinatura.Text = objCursoParceiro.DescricaoAssinaturaInstrutorCurso;
            
            objCursoParceiro.CursoModalidadeTecla.CompleteObject();
            litModalidade.Text = objCursoParceiro.CursoModalidadeTecla.DescricaoCursoModalidadeTecla;
            litPublicoAlvo.Text = objCursoParceiro.DescricaoPublicoAlvo;
            litDuracao.Text = objCursoParceiro.QuantidadeCargaHoraria.ToString();
            litCertificado.Text = objCursoParceiro.FlagCertificado ? "Sim" : "Não";
            //litValorSemDesconto.Text = objCursoParceiro.ValorCursoSemDesconto.ToString();
            litValor.Text = objCursoParceiro.ValorCurso.ToString();

            pnlParcela.Visible = objCursoParceiro.QuantidadeParcela.HasValue;

            if (objCursoParceiro.QuantidadeParcela.HasValue)
                litQuantidadeParcela.Text = objCursoParceiro.QuantidadeParcela.Value.ToString();
            
            if (objCursoParceiro.ValorCursoParcela.HasValue)
                litValorParcela.Text = objCursoParceiro.ValorCursoParcela.Value.ToString();

            litNomeCurso.Text = objCursoParceiro.DescricaoTituloCurso;
            pnlBanner.Style["background-image"] = objCursoParceiro.DescricaoCaminhoImagemBanner;
        }
        #endregion

        #endregion

    }
}