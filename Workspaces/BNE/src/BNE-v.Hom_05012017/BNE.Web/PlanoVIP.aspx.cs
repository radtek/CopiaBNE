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
using System.Web.UI.WebControls;
using BNE.BLL.Custom;
using BNE.Web.Master;
using System.Text.RegularExpressions;

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
                        Enumeradores.Parametro.ValorSemDescontoVIPGestaoTrimestral,
                        Enumeradores.Parametro.PlanoRecorrenteVIP
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

        #region IdPlanoRecorrenteVip
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano
        /// </summary>
        public int? IdPlanoRecorrenteVip
        {
            get
            {
                if (ViewState[Chave.Temporaria.IdPlanoRecorrenteVip.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.IdPlanoRecorrenteVip.ToString()].ToString());

                if (Session[Chave.Temporaria.IdPlanoRecorrenteVip.ToString()] != null)
                    return Int32.Parse(Session[Chave.Temporaria.IdPlanoRecorrenteVip.ToString()].ToString());

                return null;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.IdPlanoRecorrenteVip.ToString(), value);
                Session.Add(Chave.Temporaria.IdPlanoRecorrenteVip.ToString(), value);
            }
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

            var master = (Principal)Page.Master;

            if (master != null)
                master.LoginEfetuadoSucesso += master_LoginEfetuadoSucesso;

            CancelarEvento = false;

            if (!IsPostBack)
            {
                string url = AcessoMobile();

                if (!string.IsNullOrEmpty(url))
                {
#if DEBUG
                    Redirect(String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), url));
#else
                    Redirect(String.Format("https://{0}/{1}", UIHelper.RecuperarURLAmbiente(), url));
#endif
                }

                LimparSessionPagamento();

                ContadorTentativas = 0;

                LimparSessionDesconto();
                PreencherCampos();

                if (!base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                    base.ExibirLogin();
            }
            else
            {

            }
        }

        private string AcessoMobile()
        {
            string url = string.Empty;

            //Tipo de acesso
            string strUserAgent = Request.UserAgent.ToString().ToLower();

            string u = Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
                url = "Payment/PaymentMobileFluxoVip.aspx";
            return url;

        }
        #endregion

        #region txtCodigoCredito_TextChanged
        protected void txtCodigoCredito_TextChanged(object sender, EventArgs e)
        {
            ValidarCodigoDesconto();
        }
        #endregion

        #region btnAproveitarDescontoCodigoBNE10_Click
        protected void btnAproveitarDescontoCodigoBNE10_Click(object sender, EventArgs e)
        {
            System.Web.UI.HtmlControls.HtmlInputHidden hddJaClicou = (System.Web.UI.HtmlControls.HtmlInputHidden)Page.Master.FindControl("hddJaClicou");

            hddJaClicou.Value = "1";
            txtCodigoCredito.Text = "BNE10";
            ValidarCodigoDesconto();
        }
        #endregion

        #region btnValidarCodigoCredito_Click
        protected void btnValidarCodigoCredito_Click(object sender, EventArgs e)
        {
            ValidarCodigoDesconto();
        }
        #endregion

        #region btnPlanoMensal_Click
        protected void btnPlanoMensal_Click(object sender, EventArgs e)
        {
            if (base.IdCurriculo.HasValue)
            {
                IdPlano = IdPlanoMensal.Value;

                if (PercentualDesconto >= 99.9m) // desconto é maior que 99,9%?
                    ConcederDescontoIntegral();
                else
                    ComprarPlano();
            }
            else
                base.ExibirLogin();
        }
        #endregion

        #region btnPlanoTrimestral_Click
        protected void btnPlanoTrimestral_Click(object sender, EventArgs e)
        {
            if (base.IdCurriculo.HasValue)
            {
                IdPlano = IdPlanoTrimestral.Value;

                if (PercentualDesconto >= 99.9m)    // desconto é maior que 99,9%?
                    ConcederDescontoIntegral();
                else
                    ComprarPlano();
            }
            else
                base.ExibirLogin();
        }
        #endregion

        #region master_LoginEfetuadoSucesso
        private void master_LoginEfetuadoSucesso()
        {
            if (!base.IdCurriculo.HasValue)
                base.ExibirLogin();
            else
                PreencherCampos();
        }
        #endregion

        #region btnPlanoRecorrente_Click
        protected void btnPlanoRecorrente_Click(object sender, EventArgs e)
        {
            if (base.IdCurriculo.HasValue)
            {
                IdPlano = IdPlanoRecorrenteVip.Value;

                if (PercentualDesconto >= 99.9m) // desconto é maior que 99,9%?
                    ConcederDescontoIntegral();
                else
                    ComprarPlano();
            }
            else
                base.ExibirLogin();
        }
        #endregion

        #endregion

        #region Métodos

        #region btiSair_Click
        protected void btiSair_Click(object sender, EventArgs e)
        {
            ((BNE.Web.Master.Principal)this.Master).SairModal();
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

            LimparSessionDesconto();
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
                    //panelPlanos.Visible = false;
                    panelPlanoPromocional.Visible = true;
                    IdPlanoPromocional = planosVinculados.First().IdPlano;
                    rbPlanoPromocional.Checked = true;
                }
                else
                {
                    // esconde do usuario, na tela, o plano que nao esta vinculado
                    Boolean mostraMensal = planosVinculados.Any(plano => plano.IdPlano == IdPlanoMensal.Value);
                    Boolean mostraTrimestral = planosVinculados.Any(plano => plano.IdPlano == IdPlanoTrimestral.Value);
                    Boolean mostraRecorrente = planosVinculados.Any(plano => plano.IdPlano == IdPlanoRecorrenteVip.Value);
                    //rbPlanoMensal.Checked = rbPlanoTri.Checked = false;
                    //panelPlanoMensal.Visible = panelPlanoTri.Visible = panelOu.Visible = true;

                    if (mostraMensal && mostraTrimestral && mostraRecorrente)
                    {
                        btnPlanoMensal.Visible = btnPlanoTrimestral.Visible = btnPlanoRecorrente.Visible = true;
                    }
                    else if (mostraMensal)
                    {
                        btnPlanoMensal.Visible = true;
                        btnPlanoRecorrente.Visible = btnPlanoTrimestral.Visible = false;
                        //divPlanoMensal.Attributes.Add("class", "plano_selecionado");
                    }
                    else if (mostraTrimestral)
                    {
                        btnPlanoTrimestral.Visible = true;
                        //divPlanoTri.Attributes.Add("class", "plano_selecionado");
                        btnPlanoRecorrente.Visible = btnPlanoMensal.Visible = false;
                    }
                    else if (planosVinculados.Count > 0)
                    {
                        // existem planos vinculados, mas nenhum corresponde aos planos mostrados/selecionado na tela
                        ExibirMensagem("Esse código promocional não serve para o plano escolhido! Favor escolher outro plano ou trocar o código promocional", TipoMensagem.Erro);
                        return;

                    }
                }
            }
            else
            {
                btnPlanoMensal.Visible = btnPlanoTrimestral.Visible = btnPlanoRecorrente.Visible = true;
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
            if (base.IdCurriculo.HasValue)
            {
                FuncaoCategoria objFuncaoCategoria =
                    FuncaoCategoria.RecuperarCategoriaPorCurriculo(new Curriculo(base.IdCurriculo.Value));

                OrigemFilial objOrigemFilial = null;
                if (base.STC.Value && OrigemFilial.CarregarPorOrigem(base.IdOrigem.Value, out objOrigemFilial) &&
                    (objOrigemFilial != null && objOrigemFilial.Filial.PossuiSTCUniversitario()))
                {
                    IdPlanoMensal = Convert.ToInt32(Parametros[Enumeradores.Parametro.PlanoVIPUniversitarioMensal]);
                    IdPlanoTrimestral =
                        Convert.ToInt32(Parametros[Enumeradores.Parametro.PlanoVIPUniversitarioTrimestral]);
                }
                else
                {
                    IdPlanoMensal = Plano.RecuperarCodigoPlanoMensalPorFuncaoCategoria(objFuncaoCategoria);
                    IdPlanoTrimestral = Plano.RecuperarCodigoPlanoTrimestralPorFuncaoCategoria(objFuncaoCategoria);
                }

                IdPlanoRecorrenteVip = Convert.ToInt32(Parametros[Enumeradores.Parametro.PlanoRecorrenteVIP]);

                decimal mensalSemDesconto = Decimal.Zero;
                decimal trimestralSemDesconto = Decimal.Zero;
                switch (objFuncaoCategoria.IdFuncaoCategoria)
                {
                    case (int)Enumeradores.FuncaoCategoria.Operacao:
                        mensalSemDesconto =
                            Convert.ToDecimal(Parametros[Enumeradores.Parametro.ValorSemDescontoVIPOperacaoMensal]);
                        trimestralSemDesconto =
                            Convert.ToDecimal(Parametros[Enumeradores.Parametro.ValorSemDescontoVIPOperacaoTrimestral]);
                        break;
                    case (int)Enumeradores.FuncaoCategoria.Apoio:
                        mensalSemDesconto =
                            Convert.ToDecimal(Parametros[Enumeradores.Parametro.ValorSemDescontoVIPApoioMensal]);
                        trimestralSemDesconto =
                            Convert.ToDecimal(Parametros[Enumeradores.Parametro.ValorSemDescontoVIPApoioTrimestral]);
                        break;
                    case (int)Enumeradores.FuncaoCategoria.Especialista:
                        mensalSemDesconto =
                            Convert.ToDecimal(Parametros[Enumeradores.Parametro.ValorSemDescontoVIPEspecialistaMensal]);
                        trimestralSemDesconto =
                            Convert.ToDecimal(
                                Parametros[Enumeradores.Parametro.ValorSemDescontoVIPEspecialistaTrimestral]);
                        break;
                    case (int)Enumeradores.FuncaoCategoria.Gestao:
                        mensalSemDesconto =
                            Convert.ToDecimal(Parametros[Enumeradores.Parametro.ValorSemDescontoVIPGestaoMensal]);
                        trimestralSemDesconto =
                            Convert.ToDecimal(Parametros[Enumeradores.Parametro.ValorSemDescontoVIPGestaoTrimestral]);
                        break;
                }

                // calcula desconto, caso haja cupom de desconto
                decimal precoDescontoMensal = new Plano(IdPlanoMensal.Value).RecuperarValor();
                decimal precoDescontoTri = new Plano(IdPlanoTrimestral.Value).RecuperarValor();
                decimal precoDescontoRecorrente = new Plano(IdPlanoRecorrenteVip.Value).RecuperarValor();

                if (base.PagamentoIdCodigoDesconto.HasValue)
                {
                    BLL.CodigoDesconto objCodigoDesconto = new BLL.CodigoDesconto(base.PagamentoIdCodigoDesconto.Value);
                    objCodigoDesconto.CalcularDesconto(ref precoDescontoMensal);
                    objCodigoDesconto.CalcularDesconto(ref precoDescontoTri);
                    //objCodigoDesconto.CalcularDesconto(ref precoDescontoRecorrente);
                }

                // mostra na tela o valor apos os calculos
                decimal intPrecoDescontoMensal = 0;
                if (decimal.TryParse(precoDescontoMensal.ToString(), out intPrecoDescontoMensal))
                {
                    lblPrecoDescontoMensal.Text = intPrecoDescontoMensal.ToString("N2");
                }
                else
                {
                    lblPrecoDescontoMensal.Text = precoDescontoMensal.ToString("N2");
                }

                decimal intPrecoDescontoTri = 0;
                if (decimal.TryParse(precoDescontoTri.ToString(), out intPrecoDescontoTri))
                {
                    lblPrecoDescontoTri.Text = intPrecoDescontoTri.ToString("N2");
                }
                else
                {
                    lblPrecoDescontoTri.Text = precoDescontoTri.ToString("N2");
                }


                decimal intPrecoDescontoRecorrente = 0;
                if (decimal.TryParse(precoDescontoRecorrente.ToString(), out intPrecoDescontoRecorrente))
                {
                    lblPrecoDescontoRecorrente.Text = intPrecoDescontoRecorrente.ToString("N2");
                }
                else
                {
                    lblPrecoDescontoRecorrente.Text = precoDescontoRecorrente.ToString("N2");
                }

                base.PrazoBoleto.Value = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeDiasVencimentoAposEmissaoBoletoPF));
                lblPrecoDescontoMensal.CssClass = lblPrecoDescontoTri.CssClass = lblPrecoDescontoRecorrente.CssClass = string.Empty;
            }
            else
            {
                lblPrecoDescontoMensal.Text = lblPrecoDescontoTri.Text = lblPrecoDescontoRecorrente.Text = "9,99";
                lblPrecoDescontoMensal.CssClass = lblPrecoDescontoTri.CssClass = lblPrecoDescontoRecorrente.CssClass = "blurry";
            }

            upPrecoDescontoTri.Update();
            upPrecoDescontoMensal.Update();
            upPrecoDescontoRecorrente.Update();
        }
        #endregion

        #region CalcularTotalComprasPorFuncaoCategoria
        /// <summary>
        /// Mostrar o total de pessoas que compraram o plano VIP de acordo com a função categoria
        /// </summary>
        /// <param name="idFuncaoCategoria"></param>
        private string CalcularTotalComprasPorFuncaoCategoria(int idParametro)
        {
            return EstatisticaPlano.RecuperarEstatisticaPlano(idParametro).ToString("N0");
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

                //Enviar atualizações do plano para o Allin.
                BufferAtualizacaoCurriculoAllin.Add(IdCurriculo.Value);

#if DEBUG
                Redirect(String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), Rota.RecuperarURLRota(Enumeradores.RouteCollection.PagamentoPlano)));
#else
                Redirect(String.Format("https://{0}/{1}", UIHelper.RecuperarURLAmbiente(), Rota.RecuperarURLRota(Enumeradores.RouteCollection.PagamentoPlano)));
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
                decimal valorPlano = objPlano.ValorBase;

                if (base.PagamentoIdCodigoDesconto.HasValue)
                {
                    CodigoDesconto objCodigoDesconto = new CodigoDesconto(base.PagamentoIdCodigoDesconto.Value);
                    objCodigoDesconto.CalcularDesconto(ref valorPlano, objPlano);
                }
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