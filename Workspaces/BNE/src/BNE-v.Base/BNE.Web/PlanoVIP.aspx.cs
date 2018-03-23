using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class PlanoVIP : BasePagePagamento
    {

        #region Propriedades

        #region Parametros
        public Dictionary<Enumeradores.Parametro, string> Parametros
        {
            get
            {
                if (ViewState["Parametros"] == null)
                {
                    List<Enumeradores.Parametro> listaParametros =
                        new List<Enumeradores.Parametro>()
                    {
                        Enumeradores.Parametro.PlanoVIPUniversitarioMensal,
                        Enumeradores.Parametro.PlanoVIPUniversitarioTrimestral,
                        Enumeradores.Parametro.ValorSemDescontoVIPOperacaoMensal,
                        Enumeradores.Parametro.ValorSemDescontoVIPOperacaoTrimestral,
                        Enumeradores.Parametro.ValorSemDescontoVIPApoioMensal,
                        Enumeradores.Parametro.ValorSemDescontoVIPApoioTrimestral,
                        Enumeradores.Parametro.ValorSemDescontoVIPEspecialistaMensal,
                        Enumeradores.Parametro.ValorSemDescontoVIPEspecialistaTrimestral,
                        Enumeradores.Parametro.ValorSemDescontoVIPGestaoMensal,
                        Enumeradores.Parametro.ValorSemDescontoVIPGestaoTrimestral
                    };

                    ViewState["Parametros"] =
                        Parametro.ListarParametros(listaParametros);
                }
                return (Dictionary<Enumeradores.Parametro, string>)ViewState["Parametros"];
            }
        }
        #endregion

        #region ContadorTentativas
        private byte ContadorTentativas
        {
            get
            {
                if (ViewState["ContadorTentativas"] == null)
                    ViewState["ContadorTentativas"] = 0;

                return Convert.ToByte(ViewState["ContadorTentativas"]);
            }
            set
            {
                ViewState["ContadorTentativas"] = value;
            }
        }
        #endregion

        #region CancelarEvento
        public bool CancelarEvento
        {
            get;
            set;
        }
        #endregion

        #region IdPlano
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano
        /// </summary>
        public int IdPlano
        {
            get
            {
                return Int32.Parse(ViewState[Chave.Temporaria.IdPlano.ToString()].ToString());
            }
            set
            {
                ViewState.Add(Chave.Temporaria.IdPlano.ToString(), value);
            }
        }
        #endregion

        #region IdPlanoMensal
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano
        /// </summary>
        public int? IdPlanoMensal
        {
            get
            {
                if (ViewState[Chave.Temporaria.IdPlanoNormal.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.IdPlanoNormal.ToString()].ToString());

                if (Session[Chave.Temporaria.IdPlanoNormal.ToString()] != null)
                    return Int32.Parse(Session[Chave.Temporaria.IdPlanoNormal.ToString()].ToString());

                return null;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.IdPlanoNormal.ToString(), value);
                Session.Add(Chave.Temporaria.IdPlanoNormal.ToString(), value);
            }
        }
        #endregion

        #region IdPlanoTrimestral
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano Extendido
        /// </summary>
        public int? IdPlanoTrimestral
        {
            get
            {
                if (ViewState[Chave.Temporaria.IdPlanoExtendido.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.IdPlanoExtendido.ToString()].ToString());

                if (Session[Chave.Temporaria.IdPlanoExtendido.ToString()] != null)
                    return Int32.Parse(Session[Chave.Temporaria.IdPlanoExtendido.ToString()].ToString());

                return null;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.IdPlanoExtendido.ToString(), value);
                Session.Add(Chave.Temporaria.IdPlanoExtendido.ToString(), value);
            }
        }
        #endregion

        #region IdPlanoPromocional
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano
        /// </summary>
        public int? IdPlanoPromocional
        {
            get
            {
                if (ViewState[Chave.Temporaria.IdPlanoPromocional.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.IdPlanoPromocional.ToString()].ToString());

                if (Session[Chave.Temporaria.IdPlanoPromocional.ToString()] != null)
                    return Int32.Parse(Session[Chave.Temporaria.IdPlanoPromocional.ToString()].ToString());

                return null;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.IdPlanoPromocional.ToString(), value);
                Session.Add(Chave.Temporaria.IdPlanoPromocional.ToString(), value);
            }
        }
        #endregion

        #region CodigoDesconto
        /// <summary>
        /// Propriedade que armazena e recupera o Codigo de Desconto
        /// </summary>
        public string CodigoDesconto
        {
            get
            {
                if (base.PagamentoIdCodigoDesconto.HasValue)
                {
                    CodigoDesconto codigoDesconto = BLL.CodigoDesconto.LoadObject(base.PagamentoIdCodigoDesconto.Value);

                    return codigoDesconto.DescricaoCodigoDesconto;
                }

                return null;
            }
        }
        #endregion

        #region PercentualDesconto
        /// <summary>
        /// Propriedade que armazena e recupera o percentual de desconto
        /// </summary>
        public decimal PercentualDesconto
        {
            get
            {
                if (base.PagamentoIdCodigoDesconto.HasValue)
                {
                    CodigoDesconto codigoDesconto = BLL.CodigoDesconto.LoadObject(base.PagamentoIdCodigoDesconto.Value);

                    if (codigoDesconto.TipoCodigoDesconto != null)
                    {
                        codigoDesconto.TipoCodigoDesconto.CompleteObject();
                        return codigoDesconto.TipoCodigoDesconto.NumeroPercentualDesconto;
                    }
                }

                return Decimal.Zero;
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            CancelarEvento = false;

            if (!IsPostBack)
            {
                LimparSessionPagamento();

                ContadorTentativas = 0;

                LimparSessionDesconto();
                InicializarBarraBusca(TipoBuscaMaster.Vaga, false, GetType().ToString());
                PreencherCampos();
            }
        }
        #endregion

        #region txtCodigoCredito_TextChanged
        protected void txtCodigoCredito_TextChanged(object sender, EventArgs e)
        {
            ValidarCodigoDesconto();
        }
        #endregion

        #region btnValidarCodigoCredito_Click
        protected void btnValidarCodigoCredito_Click(object sender, EventArgs e)
        {
            ValidarCodigoDesconto();
        }
        #endregion

        #region btnContinuar_Click
        protected void btnContinuar_Click(object sender, ImageClickEventArgs e)
        {
            if (rbPlanoTri.Checked || rbPlanoMensal.Checked || rbPlanoPromocional.Checked)
            {
                if (rbPlanoMensal.Checked)
                    IdPlano = IdPlanoMensal.Value;

                if (rbPlanoTri.Checked)
                    IdPlano = IdPlanoTrimestral.Value;

                if (rbPlanoPromocional.Checked)
                    IdPlano = IdPlanoPromocional.Value;

                if (PercentualDesconto >= 99.9m)    // desconto é maior que 99,9%?
                    ConcederDescontoIntegral();
                else
                    ComprarPlano();
            }
            else
                ExibirMensagem("Selecione um plano para continuar!", TipoMensagem.Erro);
        }
        #endregion

        #endregion

        #region Métodos

        #region AtivarTodosPaineis
        private void AtivarTodosPaineis()
        {
            panelOu.Visible = panelPlanoMensal.Visible = panelPlanoTri.Visible = true;
        }
        #endregion

        #region LimparSessionDesconto
        private void LimparSessionDesconto()
        {
            base.PagamentoIdCodigoDesconto.Clear();
        }
        #endregion

        #region ValidarCodigoDesconto
        private void ValidarCodigoDesconto()
        {
            if (CancelarEvento)
                return;

            CancelarEvento = true; // para evitar que o manipulador de evento seja disparado duas vezes

            panelPlanos.Visible = true;
            panelPlanoPromocional.Visible = false;

            LimparSessionDesconto();
            AtivarTodosPaineis();
            PreencherCampos();

            if (++ContadorTentativas > 100)     // impedir que bots descubram codigos
            {
                DeslogarUsuario();
                return;
            }

            if (string.IsNullOrEmpty(txtCodigoCredito.Text))
                return;

            CodigoDesconto codigo;
            if (!BLL.CodigoDesconto.CarregarPorCodigo(txtCodigoCredito.Text, out codigo))
            {
                ExibirMensagem("Código promocional inválido", TipoMensagem.Erro);
                return;
            }

            if (codigo.JaUtilizado())
            {
                ExibirMensagem("Código promocional já utilizado", TipoMensagem.Erro);
                txtCodigoCredito.Text = String.Empty;
                return;
            }

            if (!codigo.DentroValidade())
            {
                ExibirMensagem("Código promocional fora da validade", TipoMensagem.Erro);
                txtCodigoCredito.Text = String.Empty;
                return;
            }

            TipoCodigoDesconto tipoCodigoDesconto;
            if (!codigo.TipoDescontoDefinido(out tipoCodigoDesconto))
            {
                ExibirMensagem("Código promocional inválido", TipoMensagem.Erro);
                txtCodigoCredito.Text = String.Empty;
                return;
            }

            List<Plano> planosVinculados;
            if (codigo.HaPlanosVinculados(out planosVinculados))
            {
                if (System.IO.File.Exists(Server.MapPath(String.Format("img/CodigoPromocional/{0}.jpg", tipoCodigoDesconto.IdTipoCodigoDesconto))))
                {
                    imgCodigoPromocional.ImageUrl = String.Format("~/img/CodigoPromocional/{0}.jpg", tipoCodigoDesconto.IdTipoCodigoDesconto);
                    panelPlanos.Visible = false;
                    panelPlanoPromocional.Visible = true;
                    IdPlanoPromocional = planosVinculados.First().IdPlano;
                    rbPlanoPromocional.Checked = true;
                }
                else
                {
                    // esconde do usuario, na tela, o plano que nao esta vinculado
                    Boolean mostraMensal = planosVinculados.Any(plano => plano.IdPlano == IdPlanoMensal.Value);
                    Boolean mostraTrimestral = planosVinculados.Any(plano => plano.IdPlano == IdPlanoTrimestral.Value);
                    rbPlanoMensal.Checked = rbPlanoTri.Checked = false;
                    panelPlanoMensal.Visible = panelPlanoTri.Visible = panelOu.Visible = true;

                    if (mostraMensal && mostraTrimestral)
                    {
                        panelPlanoMensal.Visible = panelPlanoTri.Visible = panelOu.Visible = true;
                    }
                    else if (mostraMensal)
                    {
                        panelPlanoMensal.Visible = true;
                        panelOu.Visible = panelPlanoTri.Visible = rbPlanoMensal.Checked = rbPlanoTri.Checked = false;
                    }
                    else if (mostraTrimestral)
                    {
                        panelPlanoTri.Visible = true;
                        panelOu.Visible = panelPlanoMensal.Visible = rbPlanoMensal.Checked = rbPlanoTri.Checked = false;
                    }
                    else if (planosVinculados.Count > 0)
                    {
                        // existem planos vinculados, mas nenhum corresponde aos planos mostrados/selecionado na tela
                        ExibirMensagem("Esse código promocional não serve para o plano escolhido! Favor escolher outro plano ou trocar o código promocional", TipoMensagem.Erro);
                        return;

                    }
                }
            }

            // seta sessao com as informacoes sobre o desconto
            base.PagamentoIdCodigoDesconto.Value = codigo.IdCodigoDesconto;
            ExibirMensagem(null, TipoMensagem.Erro);

            if (!rbPlanoPromocional.Checked)
            {
                // mostra na tela os valores atualizados, ja com o desconto
                PreencherCampos();
            }
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos()
        {
            FuncaoCategoria objFuncaoCategoria = FuncaoCategoria.RecuperarCategoriaPorCurriculo(new Curriculo(base.IdCurriculo.Value));

            OrigemFilial objOrigemFilial = null;
            if (base.STC.Value && OrigemFilial.CarregarPorOrigem(base.IdOrigem.Value, out objOrigemFilial) && (objOrigemFilial != null && objOrigemFilial.Filial.PossuiSTCUniversitario()))
            {
                IdPlanoMensal = Convert.ToInt32(Parametros[Enumeradores.Parametro.PlanoVIPUniversitarioMensal]);
                IdPlanoTrimestral = Convert.ToInt32(Parametros[Enumeradores.Parametro.PlanoVIPUniversitarioTrimestral]);
            }
            else
            {
                IdPlanoMensal = Plano.RecuperarCodigoPlanoMensalPorFuncaoCategoria(objFuncaoCategoria);
                IdPlanoTrimestral = Plano.RecuperarCodigoPlanoTrimestralPorFuncaoCategoria(objFuncaoCategoria);
            }

            decimal mensalSemDesconto = Decimal.Zero;
            decimal trimestralSemDesconto = Decimal.Zero;
            switch (objFuncaoCategoria.IdFuncaoCategoria)
            {
                case (int)Enumeradores.FuncaoCategoria.Operacao:
                    mensalSemDesconto = Convert.ToDecimal(Parametros[Enumeradores.Parametro.ValorSemDescontoVIPOperacaoMensal]);
                    trimestralSemDesconto = Convert.ToDecimal(Parametros[Enumeradores.Parametro.ValorSemDescontoVIPOperacaoTrimestral]);
                    break;
                case (int)Enumeradores.FuncaoCategoria.Apoio:
                    mensalSemDesconto = Convert.ToDecimal(Parametros[Enumeradores.Parametro.ValorSemDescontoVIPApoioMensal]);
                    trimestralSemDesconto = Convert.ToDecimal(Parametros[Enumeradores.Parametro.ValorSemDescontoVIPApoioTrimestral]);
                    break;
                case (int)Enumeradores.FuncaoCategoria.Especialista:
                    mensalSemDesconto = Convert.ToDecimal(Parametros[Enumeradores.Parametro.ValorSemDescontoVIPEspecialistaMensal]);
                    trimestralSemDesconto = Convert.ToDecimal(Parametros[Enumeradores.Parametro.ValorSemDescontoVIPEspecialistaTrimestral]);
                    break;
                case (int)Enumeradores.FuncaoCategoria.Gestao:
                    mensalSemDesconto = Convert.ToDecimal(Parametros[Enumeradores.Parametro.ValorSemDescontoVIPGestaoMensal]);
                    trimestralSemDesconto = Convert.ToDecimal(Parametros[Enumeradores.Parametro.ValorSemDescontoVIPGestaoTrimestral]);
                    break;
            }

            // calcula desconto, caso haja cupom de desconto
            decimal precoDescontoMensal = new Plano(IdPlanoMensal.Value).RecuperarValor();
            decimal precoDescontoTri = new Plano(IdPlanoTrimestral.Value).RecuperarValor();
            if (base.PagamentoIdCodigoDesconto.HasValue)
            {
                BLL.CodigoDesconto objCodigoDesconto = new BLL.CodigoDesconto(base.PagamentoIdCodigoDesconto.Value);
                objCodigoDesconto.CalcularDesconto(ref precoDescontoMensal);
                objCodigoDesconto.CalcularDesconto(ref precoDescontoTri);
            }

            // mostra na tela o valor apos os calculos
            litPrecoSemDescontoMensal.Text = mensalSemDesconto.ToString(CultureInfo.CurrentCulture);
            litPrecoSemDescontoTri.Text = trimestralSemDesconto.ToString(CultureInfo.CurrentCulture);
            litPrecoDescontoMensal.Text = precoDescontoMensal.ToString("0.00", CultureInfo.CurrentCulture);
            litPrecoDescontoTri.Text = precoDescontoTri.ToString("0.00", CultureInfo.CurrentCulture);

            base.PrazoBoleto.Value = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeDiasVencimentoAposEmissaoBoletoPF));
        }
        #endregion

        #region ConcederDescontoIntegral
        /// <summary>
        /// Metodo responsavel por processar o objeto pagamento quando o plano tem desconto de 100%
        /// </summary>
        private void ConcederDescontoIntegral()
        {
            if (CancelarEvento)    // se passou pela validação do codigo de credito, nao entra aqui
                return;

            RegistraSession();

            string erro = null;
            try
            {
                if (BLL.Pagamento.ConcederDescontoIntegral(
                        new Curriculo(base.IdCurriculo.Value),
                        new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoCandidato.Value),
                        new Plano(base.PagamentoIdentificadorPlano.Value),
                        new CodigoDesconto(base.PagamentoIdCodigoDesconto.Value),
                        out erro))
                {
                    base.StatusTransacaoCartao = true;  // para mostrar a modal

                    string urlRedirect = base.PagamentoUrlRetorno.Value;
                    base.LimparSessionPagamento();
                    Redirect(urlRedirect);

                    return;
                }
            }
            catch (Exception ex)
            {
                if (!(ex is ThreadAbortException))
                    EL.GerenciadorException.GravarExcecao(ex);

                erro = ex.Message;
            }

            ExibirMensagem("Erro durante a concessão do crédito: " + erro, TipoMensagem.Erro);
        }
        #endregion

        #region ComprarPlano
        /// <summary>
        /// Valida o tipo de transação do usuário e executa o processo especifico.
        /// Método responsável por validar o tipo de pessoa logada, o plano adquirido e o tipo de pagamento onde redireciona para a pagina ou método especifico relacionado ao tipo.
        /// </summary>
        private void ComprarPlano()
        {
            if (CancelarEvento)    // se passou pela validação do codigo de credito, nao entra aqui
                return;

            try
            {
                RegistraSession();

                CalcularBeneficio();

                CriarPlanoAdquirido();
               
                var r = Rota.RecuperarURLRota(Enumeradores.RouteCollection.PagamentoPlano);

#if DEBUG
                Redirect(String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), r));
#else
                Redirect(String.Format("https://{0}/{1}", UIHelper.RecuperarURLAmbiente(), r));
#endif
            }
            catch (Exception ex)
            {
                if (!(ex is ThreadAbortException))
                    EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region CriarPlanoAdquirido
        public void CriarPlanoAdquirido()
        {
            UsuarioFilialPerfil objUsuarioFilialPerfil = null;
            Plano objPlano = null;

            // Carrega Plano de acordo com plano escolhido na session
            if (base.PagamentoIdentificadorPlano.HasValue)
                objPlano = Plano.LoadObject(base.PagamentoIdentificadorPlano.Value);

            // Carrega Usuario Filial Perfil session
            if (base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoCandidato.Value);

            // Atualiza Valor Base Plano de acordo valor gravado na session
            if (base.ValorBasePlano.HasValue)
                objPlano.ValorBase = base.ValorBasePlano.Value;

            var objPlanoAdquirido = PlanoAdquirido.CriarPlanoAdquiridoPF(objUsuarioFilialPerfil, objPlano, base.PrazoBoleto.Value);

            base.PagamentoIdentificadorPlanoAdquirido.Value = objPlanoAdquirido.IdPlanoAdquirido;
        }
        #endregion

        #region RegistraSession
        private void RegistraSession()
        {
            String url;
            // Seta url para montar retorno da pagina a ser redirecionada a aplicacao após finalizar operações na cielo.
            if (String.IsNullOrEmpty(Request.Url.Query))
                url = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "");
            else
                url = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "").Replace(Request.Url.Query, "");

            string urlRetorno = string.Empty;
            if (base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                urlRetorno = url + "/SalaVipMeuPlano.aspx";

            base.PagamentoIdentificadorPlano.Value = IdPlano;
            base.PagamentoUrlRetorno.Value = urlRetorno;
        }
        #endregion

        #region CalcularBeneficio
        private bool CalcularBeneficio()
        {
            try
            {
                Plano objPlano = Plano.LoadObject(base.PagamentoIdentificadorPlano.Value);
                CodigoDesconto objCodigoDesconto = new CodigoDesconto(base.PagamentoIdCodigoDesconto.Value);

                decimal valorPlano = objPlano.ValorBase;

                objCodigoDesconto.CalcularDesconto(ref valorPlano, objPlano);

                base.ValorBasePlano.Value = valorPlano;

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region DeslogarUsuario
        private void DeslogarUsuario()
        {
            if (base.IdPessoaFisicaLogada.HasValue)
                new PessoaFisica(IdPessoaFisicaLogada.Value).ZerarDataInteracaoUsuario();

            //base.LimparSessionPagamento();
            BNE.Auth.BNEAutenticacao.DeslogarPagamento();
        }
        #endregion

        #endregion

    }
}