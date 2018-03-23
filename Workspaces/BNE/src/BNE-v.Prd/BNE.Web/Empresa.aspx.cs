using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ajax;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Enumeradores;
using BNE.EL;
using BNE.Web.Resources;
using JSONSharp;
using Funcao = BNE.BLL.Funcao;
using Parametro = BNE.BLL.Enumeradores.Parametro;
using TipoVinculo = BNE.BLL.Enumeradores.TipoVinculo;
using BNE.Web.Code;
using TipoMensagem = BNE.Web.Code.Enumeradores.TipoMensagem;

namespace BNE.Web
{
    public partial class Empresa : BasePage
    {
        #region Métodos

        #region CarregarParametros
        /// <summary>
        ///     Carrega os parâmetros iniciais da aba de dados gerais.
        /// </summary>
        private void CarregarParametros()
        {
            try
            {
                var parametros = new List<Parametro>
                {
                    Parametro.NumeroResultadosAutoCompleteFuncao,
                    Parametro.NumeroResultadosAutoCompleteCidade,
                    Parametro.UrlAutoCompleteCidade,
                    Parametro.UrlAutoCompleteFuncao,
                    Parametro.UrlAutoCompleteBairro
                };

                var valoresParametros = BLL.Parametro.ListarParametros(parametros);

                var parametrosJSON = new
                {
                    URLCompleteCidade = valoresParametros[Parametro.UrlAutoCompleteCidade],
                    LimiteCompleteCidade = valoresParametros[Parametro.NumeroResultadosAutoCompleteCidade],
                    URLCompleteFuncao = valoresParametros[Parametro.UrlAutoCompleteFuncao],
                    LimiteCompleteFuncao = valoresParametros[Parametro.NumeroResultadosAutoCompleteFuncao],
                    URLCompleteBairro = valoresParametros[Parametro.UrlAutoCompleteBairro],
                    LimiteCompleteBairro = valoresParametros[Parametro.NumeroResultadosAutoCompleteCidade]
                };
                ScriptManager.RegisterStartupScript(this, GetType(), DateTime.Now.Ticks.ToString(), "javaScript:InicializarAutoCompletes(" + new JSONReflector(parametrosJSON) + ");", true);
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region Redirect
        private void Redirect(string url)
        {
            try
            {
                Response.Redirect(url);
            }
            catch
            {
                //Suprimindo a exception de thread aborted do webforms.
            }
        }
        #endregion

        #region LimparCampos
        private void LimparCampos()
        {
            txtFuncaoVaga.Text = string.Empty;
            txtCidadeVaga.Text = string.Empty;
            txtSalarioDe.Valor = null;
            txtSalarioAte.Valor = null;
            txtEmailVaga.Text = string.Empty;
            txtAtribuicoesVaga.Text = string.Empty;
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarParametros();
                a1.HRef = anchorPlanos1.HRef = anchorPlanos2.HRef = GetRouteUrl(RouteCollection.EscolhaPlanoCIA.ToString(), null);

                revEmailVaga.ValidationExpression = revTxtEmail.ValidationExpression = Configuracao.regexEmail;

            }
            Utility.RegisterTypeForAjax(typeof(Empresa));
        }
        #endregion

        #region btnSalvarVaga_OnClick
        protected void btnSalvarVaga_OnClick(object sender, EventArgs e)
        {
            var email = txtEmailVaga.Text.ToLower();

            UsuarioFilialPerfil objUsuarioFilialPerfil = null;
            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
            {
                objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
            }
            else
            {
                //Se o email já existe para um usuário
                if (UsuarioFilial.ExisteUsuarioComEmail(email))
                {
                    base.ExibirMensagem("Para anunciar uma vaga com o e-mail informado é necessário estar logado.", TipoMensagem.Erro);
                    return;
                }
            }

            var objVaga = new Vaga { FlagAuditada = false, QuantidadeVaga = 1 };

            Funcao objFuncao;
            if (Funcao.CarregarPorDescricao(txtFuncaoVaga.Text, out objFuncao))
            {
                objVaga.Funcao = objFuncao;
            }

            Cidade objCidade;
            if (Cidade.CarregarPorNome(txtCidadeVaga.Text, out objCidade))
            {
                objVaga.Cidade = objCidade;
            }

            objVaga.ValorSalarioDe = txtSalarioDe.Valor;
            objVaga.ValorSalarioPara = txtSalarioAte.Valor;
            objVaga.EmailVaga = email;
            objVaga.DescricaoAtribuicoes = txtAtribuicoesVaga.Text;
            objVaga.Origem = new BLL.Origem((int)BLL.Enumeradores.Origem.BNE);

            if (objUsuarioFilialPerfil == null)
            {
                objVaga.Filial = new Filial(Convert.ToInt32(BLL.Parametro.RecuperaValorParametro(Parametro.VagaRapida_Filial)));
                objVaga.UsuarioFilialPerfil = new UsuarioFilialPerfil(Convert.ToInt32(BLL.Parametro.RecuperaValorParametro(Parametro.VagaRapida_UsuarioFilialPerfil)));
                objVaga.FlagVagaRapida = true; //Flag para identificar vaga rápida
                objVaga.FlagConfidencial = true; // não mostrar os dados da empresa 'fake' Task: 41553
            }
            else
            {
                objVaga.Filial = objUsuarioFilialPerfil.Filial;
                objVaga.UsuarioFilialPerfil = objUsuarioFilialPerfil;
                objVaga.FlagVagaRapida = false; //Se já possui usuário não salva como vaga rápida
            }

            objVaga.FlagReceberTodosCV = true;

            var vinculos = new List<VagaTipoVinculo> { new VagaTipoVinculo { Vaga = objVaga, TipoVinculo = new BLL.TipoVinculo((int)TipoVinculo.Efetivo) } };

            //Calculando a data de abertura da vaga
            objVaga.CalcularAberturaEncerramento();

            objVaga.SalvarVagaRapida(vinculos, txtNomeVaga.Valor, txtTelefoneVaga.DDD, txtTelefoneVaga.Fone, VagaLog.VagaRapida);

            litCodigoVaga.Text = objVaga.CodigoVaga;

            modal_sucesso_vaga_check.Checked = true;

            LimparCampos();
        }
        #endregion

        #region btnCadastrar_OnClick
        protected void btnCadastrar_OnClick(object sender, EventArgs e)
        {
            var rotaCadastro = "/cadastro-de-empresa-gratis";
            var nome = HttpUtility.UrlPathEncode(txtNome.Text.Trim());
            var email = txtEmail.Text.Trim();
            var cnpj = Helper.LimparMascaraCPFCNPJ(txtCNPJ.Text);

            var url = $"{rotaCadastro}/{nome}/{email}/{cnpj}";

            Redirect(url);
        }
        #endregion

        #region btlEntrar_OnClick
        protected void btlEntrar_OnClick(object sender, EventArgs e)
        {
            Redirect(GetRouteUrl(RouteCollection.LoginComercialEmpresa.ToString(), new { Destino = LoginEmpresaDestino.PesquisaCurriculo }));
        }
        #endregion

        #region btnConfirmarVaga_OnClick
        protected void btnConfirmarVaga_OnClick(object sender, EventArgs e)
        {
            var email = txtEmailVaga.Text.ToLower();
            if (UsuarioFilial.ExisteUsuarioComEmail(email)) //Se existe usuário
            {
                Redirect(GetRouteUrl(RouteCollection.LoginComercialEmpresa.ToString(), new { Destino = LoginEmpresaDestino.VagasAnunciadas }));
            }
            else
            {
                modal_sucesso_vaga_check.Checked = false;
            }
        }
        #endregion

        #region cvFuncaoVaga_OnServerValidate
        protected void cvFuncaoVaga_OnServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!ValidarFuncao(txtFuncaoVaga.Text))
            {
                args.IsValid = false;
                cvFuncaoVaga.ErrorMessage = @"Função Inválida";
            }
        }
        #endregion

        #region ValidarFuncao
        /// <summary>
        ///     Validar funcao
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [AjaxMethod(HttpSessionStateRequirement.Read)]
        public bool ValidarFuncao(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
                return true;

            return Funcao.ValidarFuncaoPorOrigem(null, valor);
        }
        #endregion

        #region ValidarCidade
        /// <summary>
        ///     Validar cidade
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [AjaxMethod(HttpSessionStateRequirement.Read)]
        public static bool ValidarCidade(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
                return true;

            Cidade objCidade;
            return Cidade.CarregarPorNome(valor, out objCidade);
        }
        #endregion

        #endregion

    }
}