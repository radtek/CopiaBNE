using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Modais
{
    public partial class EnvioCurriculo : BaseUserControl
    {

        #region Propriedades

        #region ListIdCurriculos - Variável 1
        /// <summary>que armazena e recup
        /// /// Propriedade era Currículos
        /// </summary>         
        public List<int> ListIdCurriculos
        {
            get
            {
                return (List<int>)ViewState[Chave.Temporaria.Variavel1.ToString()];
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion

        #region EnumTipoEnvioCurriculo - Variável 2
        /// <summary>
        /// Propriedade para setar se o envio de curriculo será  Vip ou será Empresa
        /// </summary>         
        public TipoEnvioCurriculo TipoEnvioCurriculo
        {
            get
            {
                return (TipoEnvioCurriculo)ViewState[Chave.Temporaria.Variavel2.ToString()];
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
        /// <summary>
        /// Método executado quando a página é carregada
        /// </summary>
        /// <param name="sender">Objeto Correspondente</param>
        /// <param name="e">Argumento do Evento</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();
            Ajax.Utility.RegisterTypeForAjax(typeof(EnvioCurriculo));
        }
        #endregion

        #region btnEnviar
        /// <summary>
        /// Evento  disparado no click do botão enviar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEnviarCurriculo_Click(object sender, EventArgs e)
        {
            try
            {
                switch (TipoEnvioCurriculo)
                {
                    case TipoEnvioCurriculo.VIP:
                        ValidarEEfetuarEnvioVIP();
                        break;
                    case TipoEnvioCurriculo.Empresa:
                        ValidarEEfetuarEnvioEmpresa();
                        break;
                }
                upEnvioCurriculo.Update();
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btiFechar_Click
        protected void btiFechar_Click(object sender, EventArgs e)
        {
            LimparCampos();
            FecharModal();
            upEnvioCurriculo.Update();
        }
        #endregion

        #region Delegates
        public delegate void DelegateEnviarConfirmacao();
        public event DelegateEnviarConfirmacao EnviarConfirmacao;
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        /// <summary>
        /// Método utilizado para para preenchimento de componentes, funções de foco e navegação
        /// </summary>
        public void Inicializar()
        {
            txtAssunto.ValidationGroup = btnEnviarCurriculo.ClientID;
            rfvEnvioPara.ValidationGroup = btnEnviarCurriculo.ClientID;
            cvEmailEnviarPara.ValidationGroup = btnEnviarCurriculo.ClientID;
            btnEnviarCurriculo.ValidationGroup = btnEnviarCurriculo.ClientID;

            ListIdCurriculos = new List<int>();
        }
        #endregion

        #region CarregarAssunto
        /// <summary>
        /// Método que carrega o campo Assunto de acordo com a lista de curriculos selecionados
        /// </summary>
        public void CarregarAssunto()
        {
            switch (TipoEnvioCurriculo)
            {
                //A propriedade EnumTipoEnvioCurriculo tem que ser setado na sala Vip como Vip, para que a modal funcione corretamente.
                case TipoEnvioCurriculo.VIP:
                    CarregarAssuntoVip();
                    break;
                case TipoEnvioCurriculo.Empresa:
                    CarregarAssuntoEmpresa();
                    break;
            }
        }
        #endregion

        #region CarregarAssuntoVip
        /// <summary>
        /// Carrega o assunto do usuário Vip....Lembrando que a modal Envio de Curriculo está considerando que o currículo pertence ao candidato Vip
        /// </summary>
        private void CarregarAssuntoVip()
        {
            Curriculo objCurriculo = Curriculo.LoadObject(ListIdCurriculos[0]);

            //Pega o nome do usuário Vip que está no currículo para montar o assunto.
            string nomeUsuarioVip = objCurriculo.PessoaFisica.PrimeiroNome;
            txtAssunto.Valor = string.Format("Currículo de {0}", nomeUsuarioVip);
            upEnvioCurriculo.Update();
        }
        #endregion

        #region CarregarAssuntoEmpresa
        /// <summary>
        /// Método responsavel por carregar o assunto referente a empresa.
        /// </summary>
        private void CarregarAssuntoEmpresa()
        {
            Filial objFilial = Filial.LoadObject(base.IdFilial.Value);
            string nomeEmpresa = objFilial.NomeFantasia;

            //Se for mais de um currículo selecionado.
            if (ListIdCurriculos.Count > 1)
                txtAssunto.Valor = string.Format("Currículos enviados por {0}", nomeEmpresa);
            else
            {
                Curriculo objCurriculo = Curriculo.LoadObject(ListIdCurriculos[0]);
                objCurriculo.PessoaFisica.CompleteObject();
                txtAssunto.Valor = string.Format("Currículo de {0} - {1}  enviado por {2}", objCurriculo.PessoaFisica.PrimeiroNome, objCurriculo.IdCurriculo, nomeEmpresa);
            }
            upEnvioCurriculo.Update();
        }
        #endregion

        #region ListEmailDestino
        /// <summary>
        /// Retorna os emails em uma List de string
        /// </summary>
        /// <returns></returns>
        private List<string> ListEmailDestino()
        {
            string[] emailPara = txtEnvioPara.Text.Split(';');
            var listEmail = new List<string>();
            var objRegex = new Regex("\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
            int cont = 0;

            foreach (string email in emailPara)
            {
                if (objRegex.IsMatch(email))//Valida o email
                {
                    foreach (string t in listEmail)//Verifica se há um email igual VSEmail
                    {
                        if (email == t)
                            cont = +1;
                    }

                    //Se não tiver nenhum email igual na VS ele adiciona 
                    if (cont.Equals(0))
                    {
                        listEmail.Add(email);
                        cont = 0;
                    }
                }
            }
            return listEmail;
        }
        #endregion

        #region EmailRemetente
        private string EmailRemetente()
        {
            string emailRemetente = string.Empty;

            switch (TipoEnvioCurriculo)
            {
                case TipoEnvioCurriculo.VIP:
                    {
                        var objPessoaFisica = new PessoaFisica(base.IdPessoaFisicaLogada.Value);
                        if (!string.IsNullOrWhiteSpace(objPessoaFisica.EmailPessoa))
                            emailRemetente = objPessoaFisica.EmailPessoa;
                        else
                            emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);
                        break;
                    }
                case TipoEnvioCurriculo.Empresa:
                    {
                        UsuarioFilial objUsuarioFilial;
                        if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, out objUsuarioFilial))
                            emailRemetente = objUsuarioFilial.EmailComercial;

                        break;
                    }
            }
            return emailRemetente;
        }
        #endregion

        #region Salvar
        public void Salvar()
        {
            Salvar(null);
        }
        public void Salvar(Filial objFilial)
        {
            var dicCurriculo = new Dictionary<int, string>();
            int idUsuarioFilialPerfil = objFilial != null ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : base.IdUsuarioFilialPerfilLogadoCandidato.Value;

            foreach (int idCurriculo in ListIdCurriculos)
            {
                dicCurriculo.Add(idCurriculo, new BLL.Curriculo(idCurriculo).RecuperarHTMLCurriculo(base.IdFilial.HasValue ? base.IdFilial.Value : (int?)null, idUsuarioFilialPerfil, !cbOcultarFoto.Checked, !cbOcultarDados.Checked, base.STC.Value, !cbOcultarObservacoes.Checked, Enumeradores.FormatoVisualizacaoImpressao.Empresa));
            }

            string emailRemetente = EmailRemetente();
            List<string> listEmailDestino = ListEmailDestino();

            bool descontarVisualizacao = false;
            if (TipoEnvioCurriculo.Equals(TipoEnvioCurriculo.VIP))
                descontarVisualizacao = false;
            else if (TipoEnvioCurriculo.Equals(TipoEnvioCurriculo.Empresa))
                descontarVisualizacao = !cbOcultarDados.Checked;

            MensagemCS.EnviarCurriculo(dicCurriculo, objFilial, idUsuarioFilialPerfil, emailRemetente, txtMensagem.Text, listEmailDestino, txtAssunto.Valor, cbAbertoEmail.Checked, cbAnexoPdf.Checked, descontarVisualizacao, base.IdOrigem.Value);
        }
        #endregion

        #region FecharModal
        public void FecharModal()
        {
            mpeEnvioCurriculo.Hide();
        }
        #endregion

        #region MostrarModal
        public void MostrarModal()
        {
            mpeEnvioCurriculo.Show();
            upEnvioCurriculo.Update();
            upEnvioPara.Update();
        }
        #endregion

        #region EsconderCamposEnviarComoOcultarDados
        /// <summary>
        /// Esconde a check box para ocultar dados de contato e os controles enviar como.
        /// </summary>
        public void EsconderCamposEnviarComoOcultarDados()
        {
            containerFormaEnvio.Visible = false;
            cbOcultarDados.Visible = false;
        }
        #endregion

        #region LimparCampos
        public void LimparCampos()
        {
            txtAssunto.Valor = string.Empty;
            txtEnvioPara.Text = string.Empty;
            txtMensagem.Text = string.Empty;
            cbOcultarDados.Checked = cbOcultarFoto.Checked = false;
            ListIdCurriculos.Clear();
        }
        #endregion

        #region ValidarEEfetuarEnvioEmpresa
        private void ValidarEEfetuarEnvioEmpresa()
        {
            //TODO: Revisar envio com desconto de saldo
            //Boolean que indica se pode ser enviado os curriculos ou não. E 
            bool possuiSaldoVIPJaVisualizado;
            var objFilial = new Filial(base.IdFilial.Value);

            if (!cbOcultarDados.Checked) //Só faz a validação caso necessite que os dados da pessoa fisica deva ser mostrado.
            {
                int countCurriculoParaDescontarVisualizacao = 0; //Propriedade que vai armazenar os currículos que devem ser descontada visualizacao.

                int saldoVisualizacaoFilial = objFilial.RecuperarSaldoVisualizacao();
                int diasRevisualizacao = Int32.Parse(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PeriodoReVisualizacaoCurriculo));

                foreach (int idCurriculo in ListIdCurriculos)
                {
                    var objCurriculo = new Curriculo(idCurriculo);
                    //Verifica se currículo não é VIP.
                    if (!objCurriculo.VIP())
                    {
                        //Verifica se a filial não tem visualização
                        CurriculoVisualizacao objCurriculoVisualizacao;
                        if (!CurriculoVisualizacao.CarregarPorCurriculoFilial(objCurriculo, objFilial, diasRevisualizacao, out objCurriculoVisualizacao))
                            countCurriculoParaDescontarVisualizacao++; //Se o curriculo não é vip e a filial não possui visualizacao deste currciulo no prazo correto, então deve descontar visualizacao deste curriculo

                        //Se existe o curriculo na origem STC, então não deve descontar visualização
                        if (!base.IdOrigem.Value.Equals((int)Enumeradores.Origem.BNE))
                        {
                            if (CurriculoOrigem.ExisteCurriculoNaOrigem(objCurriculo, new Origem(base.IdOrigem.Value)))
                                countCurriculoParaDescontarVisualizacao--;
                        }
                    }

                    //Se já passou o saldo da filial, então pode sair do laço e estourar erro.
                    if (countCurriculoParaDescontarVisualizacao > saldoVisualizacaoFilial)
                        break;
                }

                if (countCurriculoParaDescontarVisualizacao > saldoVisualizacaoFilial)
                {
                    ExibirMensagem("Você não tem saldo de visualização de currículo.", TipoMensagem.Aviso);
                    possuiSaldoVIPJaVisualizado = false;
                }
                else
                    possuiSaldoVIPJaVisualizado = true;
            }
            else
                possuiSaldoVIPJaVisualizado = true;

            if (possuiSaldoVIPJaVisualizado)
            {
                Salvar(objFilial);
                LimparCampos();
                FecharModal();

                if (EnviarConfirmacao != null)
                    EnviarConfirmacao();
            }
        }
        #endregion

        #region ValidarEEfetuarEnvioVIP
        private void ValidarEEfetuarEnvioVIP()
        {
            Salvar();
            LimparCampos();
            FecharModal();

            if (EnviarConfirmacao != null)
                EnviarConfirmacao();
        }
        #endregion

        #endregion

        #region Ajax
        /// <summary>
        /// Ajax Method para validar Email
        /// </summary>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public string ValidarEmail(string emailPara)
        {
            string[] email = emailPara.Split(';');
            string emailErro = string.Empty;
            int cont = 0;
            //var objRegex = new Regex("\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*");
            var objRegex = new Regex("^[\\w-]+(\\.[\\w-]+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$");


            foreach (string t in email)//Valida o email
            {
                if (!objRegex.IsMatch(t))
                {
                    emailErro += " " + t;
                    cont += 1;
                }
            }

            if (cont.Equals(1))
                return string.Format(MensagemAviso._104001, emailErro);

            if (cont > 1)
                return string.Format(MensagemAviso._104002, emailErro);

            return null;
        }
        #endregion

    }
}
