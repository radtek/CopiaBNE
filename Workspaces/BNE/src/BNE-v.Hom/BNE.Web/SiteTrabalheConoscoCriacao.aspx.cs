using System;
using System.Collections.Generic;
using System.Web;
using BNE.BLL;
using BNE.Componentes.EL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Handlers;
using BNE.Web.Resources;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class SiteTrabalheConoscoCriacao : BasePage
    {

        #region Propriedades

        #region Permissoes - Variável Permissoes
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaCurriculo
        /// </summary>
        private List<int> Permissoes
        {
            get
            {
                return (List<int>)ViewState[Chave.Temporaria.Permissoes.ToString()];
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Permissoes.ToString(), value);
            }
        }
        #endregion

        #region LogoWSBytes - Variavel10
        private byte[] LogoWSBytes
        {
            get
            {
                return (byte[])(ViewState[Chave.Temporaria.Variavel10.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel10.ToString(), value);
            }
        }
        #endregion

        #region LogoWSURL - Variavel11
        private string LogoWSURL
        {
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel11.ToString(), value);
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

            txtEnderecoSite.MensagemErroValor = MensagemAviso._300039;

            InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "SiteTrabalheConosco");
            Ajax.Utility.RegisterTypeForAjax(typeof(SiteTrabalheConoscoCriacao));
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Redirect("SalaSelecionador.aspx");
        }
        #endregion

        #region btnSalvarAvancar_Click
        /// <summary>
        /// Evento disparado quando o botão Salvar Avançar é clicado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSalvarAvancar_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar();

                Redirect("SiteTrabalheConoscoConfirmacao.aspx");
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region ucFoto_Error
        protected void ucFoto_Error(Exception ex)
        {
            if (ex is ImageSlicerException)
            {
                ExibirMensagem(ex.Message, TipoMensagem.Erro);
            }
            else
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlExisteLogoWS_Click
        protected void btlExisteLogoWS_Click(object sender, EventArgs e)
        {
            ucFoto.ImageData = LogoWSBytes;
            LogoWSBytes = null;
            LogoWSURL = string.Empty;
            btlExisteLogoWS.Visible = false;

            upFoto.Update();
        }
        #endregion btlExisteLogoWS_Click

        #endregion

        #region Métodos

        #region Inicializar
        private void Inicializar()
        {
            AjustarPermissoes();
            PreencherCampos();
        }
        #endregion

        #region AjustarPermissoes
        /// <summary>
        /// Método responsável por ajustar as permissões da tela de acordo com o susuário filial perfil logado.
        /// </summary>
        private void AjustarPermissoes()
        {
            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, Enumeradores.CategoriaPermissao.SalaSelecionadora);

                if (!Permissoes.Contains((int)Enumeradores.Permissoes.SalaSelecionadora.AcessarTelaSalaSelecionadora))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, Enumeradores.CategoriaPermissao.SalaSelecionadora);

                if (!Permissoes.Contains((int)Enumeradores.Permissoes.SalaSelecionadora.AcessarTelaSalaSelecionadora))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), new { Destino = Enumeradores.LoginEmpresaDestino.STC }));
        }
        #endregion

        #region PreencherCampos
        /// <summary>
        /// Método responsável por preecher os componentes em tela.
        /// </summary>
        private void PreencherCampos()
        {
            if (base.IdFilial.HasValue)
            {
                OrigemFilial objOrigemFilial;
                if (OrigemFilial.CarregarPorFilial(base.IdFilial.Value, out objOrigemFilial))
                {
                    Session["IdOrigemFilial"] = objOrigemFilial.IdOrigemFilial;

                    txtEnderecoSite.Valor = objOrigemFilial.DescricaoDiretorio;
                    objOrigemFilial.Template.CompleteObject();
                    hfTemplate.Value = objOrigemFilial.Template.NomeTemplate;
                    objOrigemFilial.Filial.CompleteObject();

                    CarregarLogo(objOrigemFilial.Filial);
                }
                else
                {
                    Session["IdOrigemFilial"] = null;

                    Filial objFilial = Filial.LoadObject(base.IdFilial.Value);
                    CarregarLogo(objFilial);
                    txtEnderecoSite.Valor = objFilial.NomeFantasia.Replace(" ", "");
                }
            }
        }
        #endregion

        #region CarregarLogo
        /// <summary>
        /// Rotina para carregar a logo da empresa
        /// </summary>
        private void CarregarLogo(Filial objFilial)
        {
            try
            {
                LogoWSBytes = null;
                LogoWSURL = null;
                btlExisteLogoWS.Visible = false;

                byte[] byteArray = FilialLogo.RecuperarArquivo((decimal)objFilial.NumeroCNPJ);

                if (byteArray != null)
                {
                    ucFoto.LimparFoto();
                    ucFoto.ImageData = byteArray;
                }
                else
                {

                    #region Plataforma
                    try
                    {
                        using (var wsPJ = new WSPessoaJuridica.WSPessoaJuridica())
                        {
                            byteArray = wsPJ.CarregarPessoaJuridicaLogoPrincipalBinario((decimal)objFilial.NumeroCNPJ);
                        }
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                    }
                    #endregion

                    if (byteArray != null)
                    {
                        LogoWSBytes = byteArray;
                        LogoWSURL = UIHelper.RetornarUrlLogo(objFilial.CNPJ, Handlers.PessoaJuridicaLogo.OrigemLogo.Plataforma);
                        btlExisteLogoWS.Visible = true;
                    }
                }
                upFoto.Update();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region Salvar
        private void Salvar()
        {
            Origem objOrigem;
            OrigemFilial objOrigemFilial;
            FilialLogo objFilialLogo;
            if (!OrigemFilial.CarregarPorFilial(base.IdFilial.Value, out objOrigemFilial))
            {
                objOrigemFilial = new OrigemFilial
                    {
                        Filial = new Filial(base.IdFilial.Value)
                    };

                var valorConteudo = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.MensagemPadraoHomeSTC);
                objOrigemFilial.DescricaoPaginaInicial = string.Format(valorConteudo, objOrigemFilial.Filial.RazaoSocial);

                objOrigem = new Origem();
                if (!FilialLogo.CarregarLogo(objOrigemFilial.Filial.IdFilial, out objFilialLogo))
                    objFilialLogo = new FilialLogo();
            }
            else
            {
                objOrigem = objOrigemFilial.Origem;
                objOrigem.CompleteObject();

                if (!FilialLogo.CarregarLogo(objOrigemFilial.Filial.IdFilial, out objFilialLogo))
                    objFilialLogo = new FilialLogo();
            }

            objOrigemFilial.Filial.CompleteObject();

            string urlAmbiente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLAmbiente);

            objOrigem.DescricaoURL = string.Format("http://{0}/{1}", urlAmbiente, txtEnderecoSite.Valor);
            objOrigem.DescricaoOrigem = txtEnderecoSite.Valor;
            objOrigem.TipoOrigem = new TipoOrigem((int)Enumeradores.TipoOrigem.Publico);
            objOrigem.FlagInativo = false;

            objOrigemFilial.DescricaoDiretorio = txtEnderecoSite.Valor;
            objOrigemFilial.Template = Template.CarregarPorNome(hfTemplate.Value);
            objOrigemFilial.DescricaoIP = Request.ServerVariables["REMOTE_HOST"];
            objOrigemFilial.FlagInativo = false;
            objOrigemFilial.FlagTodasFuncoes = true;

            #region Logo
            if (ucFoto.ImageData != null && !ucFoto.ImageData.Length.Equals(0))
            {
                objFilialLogo.ImagemLogo = ucFoto.ImageData;
                objFilialLogo.FlagInativo = false;
            }
            else
            {
                objFilialLogo.ImagemLogo = null;
                objFilialLogo.FlagInativo = true;
            }
            #endregion Logo

            objOrigemFilial.Salvar(objOrigem, objFilialLogo);

            BuscaIntencoes(base.IdFilial.Value, objOrigemFilial);
        }
        #endregion

        #region BuscaIntencoes
        /// <summary>
        /// Método responsável por Incluir as Intenções de vagas desta empresa para a Origem Curriculo.
        /// </summary>
        private void BuscaIntencoes(int idFilial, OrigemFilial objOrigemFilial)
        {
            //Busca todas as intenções
            IntencaoFilial liIntencao;
            System.Data.DataTable dt = IntencaoFilial.CarregarPorFilial(idFilial, out liIntencao);

            if (dt.Rows.Count > 0)
            {
                //Verifica se tem alguma intenção que já tenha sido cadastrada na Origem
                foreach (System.Data.DataRow item in dt.Rows)
                    if (CurriculoOrigem.ExisteCurriculoNaOrigem(new Curriculo(Convert.ToInt32(item["Idf_Curriculo"].ToString())), objOrigemFilial.Origem))
                        item.Delete();
                dt.AcceptChanges();

                //cria um novo DataTable com as colunas necessárias para o BulkInsert
                var dtInsert = new System.Data.DataTable();
                dtInsert.Columns.Add("Idf_Origem", typeof(int));
                dtInsert.Columns.Add("Idf_Curriculo_Origem", typeof(int));
                dtInsert.Columns.Add("Idf_Curriculo", typeof(int));
                dtInsert.Columns.Add("Dta_Cadastro", typeof(DateTime));
                dtInsert.Columns.Add("Dta_Alteracao", typeof(DateTime));
                dtInsert.Columns.Add("Des_IP", typeof(string));
                //Insere no novo DataTable os valores que iram para o banco
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    System.Data.DataRow dr = dtInsert.NewRow();
                    dr["Idf_Origem"] = objOrigemFilial.Origem.IdOrigem;
                    dr["Idf_Curriculo"] = Convert.ToInt32(dt.Rows[i].ItemArray[1].ToString());
                    dr["Dta_Cadastro"] = Convert.ToDateTime(dt.Rows[i].ItemArray[3].ToString());
                    dr["Dta_Alteracao"] = Convert.ToDateTime(dt.Rows[i].ItemArray[3].ToString());
                    dr["Des_IP"] = 0;

                    dtInsert.Rows.Add(dr);
                }

                try
                {
                    IntencaoFilial.InsereIntencaoOrigem(dtInsert);
                }
                catch (Exception ex)
                {
                    ExibirMensagemErro(ex);
                }
            }
        }
        #endregion

        #endregion

        #region AjaxMehods

        #region ValidarEnderecoSTC
        /// <summary>
        /// Validar o endereço digitado como endereço do STC
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static bool ValidarEnderecoSTC(string valor)
        {
            valor = valor.Trim();
            if (string.IsNullOrEmpty(valor))
                return true;

            OrigemFilial objOrigemFilial;
            if (OrigemFilial.CarregarPorDiretorio(valor, out objOrigemFilial)) //Caso encontre um filial com este diretorio
            {
                //Valida se está editando a filial que possue este diretório
                if (HttpContext.Current.Session["IdOrigemFilial"] != null && objOrigemFilial.IdOrigemFilial.Equals((int)HttpContext.Current.Session["IdOrigemFilial"]))
                    return true;

                return false;
            }
            return true;
        }
        #endregion

        #endregion

    }
}