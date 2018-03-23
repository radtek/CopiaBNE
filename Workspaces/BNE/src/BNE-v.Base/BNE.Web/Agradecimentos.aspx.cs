using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using BNE.BLL;
using BNE.BLL.Custom.Email;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using JSONSharp;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class Agradecimentos : BasePage
    {

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                Inicializar();

            ucModalConfirmacao.ModalConfirmada += ucModalConfirmacaoModalConfirmada;
            Ajax.Utility.RegisterTypeForAjax(typeof(Agradecimentos));
        }
        #endregion

        #region ucModalConfirmacaoModalConfirmada
        void ucModalConfirmacaoModalConfirmada()
        {
            LimparCampos();
            upAgradecimentos.Update();
        }
        #endregion

        #region btnEnviar_Click
        protected void btnEnviar_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar();
                ucModalConfirmacao.PreencherCampos(MensagemAviso._23012, MensagemAviso._24017, false);
                ucModalConfirmacao.MostrarModal();
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btiVip_Click
        protected void btiVip_Click(object sender, ImageClickEventArgs e)
        {
            Redirect("SalaVipMeuPlano.aspx");
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Redirect("Default.aspx");
        }
        #endregion

        #endregion

        #region Metodos

        #region Inicializar

        private void Inicializar()
        {
            //Validação Email
            revEmail.ValidationExpression = Configuracao.regexEmail;
            CarregarParametros();

            PreencherCampos();

            InicializarBarraBusca(TipoBuscaMaster.Vaga, true, "PesquisaCurriculoAvancada");

            imgDireita.Attributes.Add("onclick", "javascript:SlideCarrousel('Direita');");
            imgEsquerda.Attributes.Add("onclick", "javascript:SlideCarrousel('Esquerda');");

            ScriptManager.RegisterStartupScript(this, GetType(), "InicializarAgradecimentos", "javaScript:InicializarAgradecimentos();", true);
        }
        #endregion

        #region CarregarParametros
        /// <summary>
        /// Carrega os parâmetros iniciais da aba de dados gerais.
        /// </summary>
        private void CarregarParametros()
        {
            try
            {
                var parametros = new List<Enumeradores.Parametro>
                    {
                        Enumeradores.Parametro.IntervaloTempoAutoComplete,
                        Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteCidade,
                        Enumeradores.Parametro.NumeroResultadosAutoCompleteCidade
                    };

                Dictionary<Enumeradores.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

                aceCidadeAgradecimento.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceCidadeAgradecimento.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteCidade]);
                aceCidadeAgradecimento.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteCidade]);
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos()
        {
            if (base.IdPessoaFisicaLogada.HasValue)
            {
                PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(base.IdPessoaFisicaLogada.Value);

                Curriculo objCurriculo;
                if (Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo))
                {
                    if (objCurriculo.CidadeEndereco != null)
                    {
                        objCurriculo.CidadeEndereco.CompleteObject();
                        txtCidadeAgradecimento.Text = objCurriculo.CidadeEndereco.NomeCidade;
                    }
                }
                txtNome.Text = objPessoaFisica.NomePessoa;
                txtEmail.Text = objPessoaFisica.EmailPessoa;
            }
        }
        #endregion

        #region Salvar
        private void Salvar()
        {
            Cidade objCidade;
            Cidade.CarregarPorNome(txtCidadeAgradecimento.Text, out objCidade);

            if (Agradecimento.SalvarAgradecimento(null, txtNome.Text, txtEmail.Text, objCidade, txtMensagem.Text))
            {
                int? idUsuarioFilialPerfil = null;

                if (base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                    idUsuarioFilialPerfil = base.IdUsuarioFilialPerfilLogadoCandidato.Value;

                if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                    idUsuarioFilialPerfil = base.IdUsuarioFilialPerfilLogadoEmpresa.Value;

                EmailSenderFactory
                    .Create(TipoEnviadorEmail.Fila)
                    .Enviar(null, idUsuarioFilialPerfil.HasValue ? new UsuarioFilialPerfil(idUsuarioFilialPerfil.Value) : null, null, String.Empty, txtMensagem.Text, txtEmail.Text, Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailAgradecimento));
            }
        }
        #endregion

        #region LimparCampos
        private void LimparCampos()
        {
            txtNome.Text = String.Empty;
            txtEmail.Text = String.Empty;
            txtCidadeAgradecimento.Text = String.Empty;
            txtMensagem.Text = String.Empty;
        }
        #endregion

        #endregion

        #region AjaxMethod

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static string CarregarAgradecimento(int paginaAtual, int quantidadeItens)
        {
            string agradecimento = Agradecimento.CarregarAgradecimento(paginaAtual, quantidadeItens);
            return agradecimento;
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static string ListarAgradecimentos(int agradecimentoAtual)
        {

            Dictionary<int, MensagemAgradecimento> dicionario = Agradecimento.ListarAgradecimentos(agradecimentoAtual);

            MensagemAgradecimento objMensagemAgradecimentoAnterior = dicionario.ElementAt(0).Value;
            MensagemAgradecimento objMensagemAgradecimentoAtual = dicionario.ElementAt(1).Value;
            MensagemAgradecimento objMensagemAgradecimentoProximo = dicionario.ElementAt(2).Value;

            var parametros = new
            {
                //Agradecimento anterior
                IdAgradecimentoAnterior = objMensagemAgradecimentoAnterior.Idf_Agradecimento,
                MensagemAgradecimentoAnterior = objMensagemAgradecimentoAnterior.Des_Agradecimento,
                UsuarioAgradecimentoAnterior = objMensagemAgradecimentoAnterior.Des_Usuario_Agradecimento,
                CidadeAgradecimentoAnterior = objMensagemAgradecimentoAnterior.Des_Cidade_Agradecimento,
                //Agradecimento atual
                IdAgradecimentoAtual = objMensagemAgradecimentoAtual.Idf_Agradecimento,
                MensagemAgradecimentoAtual = objMensagemAgradecimentoAtual.Des_Agradecimento,
                UsuarioAgradecimentoAtual = objMensagemAgradecimentoAtual.Des_Usuario_Agradecimento,
                CidadeAgradecimentoAtual = objMensagemAgradecimentoAtual.Des_Cidade_Agradecimento,
                //Agradecimento próximo
                IdAgradecimentoProximo = objMensagemAgradecimentoProximo.Idf_Agradecimento,
                MensagemAgradecimentoProximo = objMensagemAgradecimentoProximo.Des_Agradecimento,
                UsuarioAgradecimentoProximo = objMensagemAgradecimentoProximo.Des_Usuario_Agradecimento,
                CidadeAgradecimentoProximo = objMensagemAgradecimentoProximo.Des_Cidade_Agradecimento
            };

            return new JSONReflector(parametros).ToString();
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static string RetornarUltimoRowId()
        {
            return Agradecimento.RetornarUltimoRowId();
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static string RetornarMaiorCodigoAgradecimento()
        {
            return Agradecimento.RetornarMaiorCodigoAgradecimento().ToString(CultureInfo.CurrentCulture);
        }

        #region ValidarCidade
        /// <summary>
        /// Validar cidade
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
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