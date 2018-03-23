using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using JSONSharp;
using Resources;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class CadastroEmpresaUsuario : BasePage
    {

        #region Propriedades

        #region IdFilial - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Filial.
        /// </summary>
        public int? IdFilial
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());
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

        #region IdPessoaFisica - Variável 2
        /// <summary>
        /// Propriedade que armazena e recupera o ID da Pessoa Fisica.
        /// </summary>
        public int? IdPessoaFisica
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel2.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel2.ToString()].ToString());
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

        #region IdUsuarioFilialPerfil - Variável 3
        /// <summary>
        /// Propriedade que armazena e recupera o ID do UsuarioFilialPerfil.
        /// </summary>
        public int? IdUsuarioFilialPerfil
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel3.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel3.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel3.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel3.ToString());
            }
        }
        #endregion

        #region IdPerfilUsuarioLogado - Variável 5
        /// <summary>
        /// Verifica se existe O CPF informado na base de dados
        /// </summary>
        public int IdPerfilUsuarioLogado
        {
            get
            {
                return Convert.ToInt32((ViewState[Chave.Temporaria.Variavel5.ToString()]));
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel5.ToString(), value);
            }
        }
        #endregion

        #region Permissoes - Variável Permissoes
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaCurriculo
        /// </summary>
        protected List<int> Permissoes
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
                return 1;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.PageIndex.ToString(), value);
            }
        }
        #endregion

        #region UrlOrigem - Variável 6
        /// <summary>
        /// Propriedade que armazena e recupera o enumerador do login origem
        /// </summary>
        public string UrlOrigem
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel6.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel6.ToString()]).ToString();
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel6.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel6.ToString());
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        /// <summary>
        /// Responsável por configurar a tela em seu modo inicial, configurando os controles necessários.
        /// </summary>
        private void Inicializar()
        {
            AjustarPermissoes();

            if (Request.UrlReferrer != null)
                UrlOrigem = Request.UrlReferrer.AbsoluteUri.ToString(CultureInfo.CurrentCulture);

            lblMensagemGridUsuarios.Text = "Responsável pelo Cadastro - Poderá realizar alterações nos Dados da Empresa e Cadastros de Usuários";

            //TODO: Rever esta parte, possibilidade de um usuário interno cadastrar usuários
            if (!base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue) //Se não for um usuário interno
            {
                UsuarioFilialPerfil objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
                IdPerfilUsuarioLogado = objUsuarioFilialPerfil.Perfil.IdPerfil;

                if (!IdPerfilUsuarioLogado.Equals((int)Enumeradores.Perfil.AcessoEmpresaMaster))
                    btnSalvar.Enabled = false;

                gvUsuarios.Columns.FindByUniqueName("Status").Visible = false;
            }
            else
                gvUsuarios.Columns.FindByUniqueName("Status").Visible = true;

            //Carregar Listas de Sugestões.
            UIHelper.CarregarRadioButtonList(rblSexo, Sexo.Listar());

            gvUsuarios.PageSize = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadePaginacaoCadastroUsuario));

            CarregarParametros();

            #region Data de Nascimento
            //Aplicação de Regra: 
            //- Intervalo da Data de Nascimento. ( 14 anos >= idade <= 80 anos )

            int idadeMinima = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdadeMinima));
            int idadeMaxima = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdadeMaxima));
            DateTime dataAtual = DateTime.Now;
            try
            {
                DateTime dataMinima = Convert.ToDateTime(dataAtual.Day.ToString(CultureInfo.CurrentCulture) + "/" + dataAtual.Month.ToString(CultureInfo.CurrentCulture) + "/" + (dataAtual.Year - idadeMaxima).ToString(CultureInfo.CurrentCulture));
                DateTime dataMaxima = Convert.ToDateTime(dataAtual.Day.ToString(CultureInfo.CurrentCulture) + "/" + dataAtual.Month.ToString(CultureInfo.CurrentCulture) + "/" + (dataAtual.Year - idadeMinima).ToString(CultureInfo.CurrentCulture));
                txtDataNascimento.DataMinima = dataMinima;
                txtDataNascimento.DataMaxima = dataMaxima;
            }
            catch
            {
                txtDataNascimento.DataMinima = DateTime.MinValue;
                txtDataNascimento.DataMaxima = DateTime.MaxValue;
            }

            txtDataNascimento.MensagemErroIntervalo = String.Format(MensagemAviso._100005, idadeMinima, idadeMaxima);

            #endregion

            CarregarGridUsuarios();
            CarregarDadosFilial();

            //Foco inicial
            if (string.IsNullOrEmpty(txtCPF.Valor))
                txtCPF.Focus();
            else
                txtNome.Focus();

            var parametros = new
            {
                aceFuncaoExercida = aceFuncaoExercida.ClientID
            };
            ScriptManager.RegisterStartupScript(this, GetType(), "InicializarAutoComplete", "javaScript:InicializarAutoComplete(" + new JSONReflector(parametros) + ");", true);

            Ajax.Utility.RegisterTypeForAjax(typeof(CadastroEmpresaUsuario));
        }
        #endregion

        #region CarregarParametros
        /// <summary>
        /// Inicializa os controles com os valores pré-definidos em parâmetro.
        /// </summary>
        private void CarregarParametros()
        {

            try
            {
                var parametros = new List<Enumeradores.Parametro>
                                     {
                                         Enumeradores.Parametro.IntervaloTempoAutoComplete,
                                         Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao,
                                         Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteFuncao
                                     };
                Dictionary<Enumeradores.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

                aceFuncaoExercida.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceFuncaoExercida.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao]);
                aceFuncaoExercida.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteFuncao]);

                if (Request.QueryString["i"] != null)
                    IdFilial = Convert.ToInt32(Request.QueryString["i"].ToString(CultureInfo.CurrentCulture));
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region CarregarGridUsuarios
        /// <summary>
        /// Carrega a Grid de Usuários
        /// </summary>
        public void CarregarGridUsuarios()
        {
            int totalRegistros;

            //TODO: Rever esta parte, possibilidade de um usuário interno cadastrar usuários Gieyson em 29/09
            if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                UIHelper.CarregarRadGrid(gvUsuarios, UsuarioFilialPerfil.CarregarUsuariosCadastradosPorFilial(PageIndex, gvUsuarios.PageSize, IdFilial.Value, (int)Enumeradores.Perfil.AcessoEmpresaMaster, base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, true, out totalRegistros), totalRegistros);
            else
                UIHelper.CarregarRadGrid(gvUsuarios, UsuarioFilialPerfil.CarregarUsuariosCadastradosPorFilial(PageIndex, gvUsuarios.PageSize, base.IdFilial.Value, IdPerfilUsuarioLogado, base.IdUsuarioFilialPerfilLogadoEmpresa.Value, false, out totalRegistros), totalRegistros);

            upGvUsuarios.Update();
        }
        #endregion

        #region CarregarDadosFilial
        public void CarregarDadosFilial()
        {
            var objFilial = Filial.LoadObject(base.IdFilial.HasValue ? base.IdFilial.Value : IdFilial.Value);
            lblUsuariosEmpresa.Text = string.Format("Usuários da Empresa {0} - CNPJ: {1}", objFilial.RazaoSocial, objFilial.CNPJ);
        }
        #endregion

        #region PreencherCampos
        /// <summary>
        /// Preenche quando a mesma estiver em modo de edição.
        /// </summary>
        private void PreencherCampos()
        {
            #region Dados Pessoais

            if (IdPessoaFisica.HasValue)
            {
                PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);
                if (objPessoaFisica != null)
                {
                    txtCPF.Valor = objPessoaFisica.NumeroCPF;
                    txtDataNascimento.Valor = objPessoaFisica.DataNascimento.ToShortDateString();
                    txtNome.Valor = objPessoaFisica.NomePessoa;
                    rblSexo.SelectedValue = objPessoaFisica.Sexo.IdSexo.ToString(CultureInfo.CurrentCulture);
                    txtTelefoneCelular.DDD = objPessoaFisica.NumeroDDDCelular;
                    txtTelefoneCelular.Fone = objPessoaFisica.NumeroCelular;
                }
            }

            #endregion

            #region Dados Profissionais

            if (IdUsuarioFilialPerfil.HasValue)
            {
                UsuarioFilial objUsuarioFilial;
                if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(IdUsuarioFilialPerfil.Value, out objUsuarioFilial))
                {
                    if (objUsuarioFilial.Funcao != null)
                    {
                        objUsuarioFilial.Funcao.CompleteObject();
                        txtFuncaoExercida.Text = objUsuarioFilial.Funcao.DescricaoFuncao;
                    }
                    else
                        txtFuncaoExercida.Text = objUsuarioFilial.DescricaoFuncao;

                    if (objUsuarioFilial.NumeroDDDComercial != null && objUsuarioFilial.NumeroComercial != null)
                    {
                        txtTelefone.DDD = objUsuarioFilial.NumeroDDDComercial;
                        txtTelefone.Fone = objUsuarioFilial.NumeroComercial;
                    }

                    if (objUsuarioFilial.NumeroRamal != null)
                        txtRamal.Valor = objUsuarioFilial.NumeroRamal;

                    if (objUsuarioFilial.EmailComercial != null)
                        txtEmail.Text = objUsuarioFilial.EmailComercial;
                }

                UsuarioFilialPerfil objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(IdUsuarioFilialPerfil.Value);
                ckbMaster.Checked = objUsuarioFilialPerfil.Perfil.IdPerfil.Equals((int)Enumeradores.Perfil.AcessoEmpresaMaster);
            }

            #endregion

            upDadosBasicos.Update();
        }
        #endregion

        #region Salvar
        /// <summary>
        /// Salva as informações da tela
        /// </summary>
        /// <returns></returns>
        private void Salvar()
        {
            #region Variaveis

            UsuarioFilial objUsuarioFilial;
            UsuarioFilialPerfil objUsuarioFilialPerfil;
            Usuario objUsuario;

            #endregion

            #region PessoaFisica

            PessoaFisica objPessoaFisica = IdPessoaFisica.HasValue ? PessoaFisica.LoadObject(IdPessoaFisica.Value) : new PessoaFisica();
            objPessoaFisica.NomePessoa = txtNome.Valor;
            objPessoaFisica.NumeroCPF = txtCPF.Valor;
            objPessoaFisica.NumeroCelular = txtTelefoneCelular.Fone;
            objPessoaFisica.NumeroDDDCelular = txtTelefoneCelular.DDD;
            objPessoaFisica.Sexo = new Sexo(Convert.ToInt32(rblSexo.SelectedValue));
            objPessoaFisica.DataNascimento = DateTime.Parse(txtDataNascimento.Valor);
            objPessoaFisica.FlagInativo = false;
            objPessoaFisica.NomePessoaPesquisa = txtNome.Valor;

            #endregion

            if (!Usuario.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objUsuario))
            {
                objUsuario = new Usuario
                {
                    PessoaFisica = objPessoaFisica,
                    SenhaUsuario = objPessoaFisica.DataNascimento.ToString("ddMMyyyy")
                };
            }

            #region BNE_Usuario_Filial, Usuario_Filial_Perfil

            if (IdUsuarioFilialPerfil.HasValue)
            {
                objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(IdUsuarioFilialPerfil.Value);

                UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(IdUsuarioFilialPerfil.Value, out objUsuarioFilial);

                if (objUsuarioFilial == null)
                    objUsuarioFilial = new UsuarioFilial();
            }
            else
            {
                objUsuarioFilialPerfil = new UsuarioFilialPerfil();
                objUsuarioFilial = new UsuarioFilial();
            }

            objUsuarioFilialPerfil.Filial = new Filial(base.IdFilial.HasValue ? base.IdFilial.Value : IdFilial.Value);
            objUsuarioFilialPerfil.DescricaoIP = Request.ServerVariables["REMOTE_ADDR"];
            objUsuarioFilialPerfil.FlagInativo = false;
            objUsuarioFilialPerfil.PessoaFisica = objPessoaFisica;
            objUsuarioFilialPerfil.Perfil = new Perfil(ckbMaster.Checked ? (int)Enumeradores.Perfil.AcessoEmpresaMaster : (int)Enumeradores.Perfil.AcessoEmpresa);
            objUsuarioFilialPerfil.SenhaUsuarioFilialPerfil = objPessoaFisica.DataNascimento.ToString("ddMMyyyy");

            objUsuarioFilial.UsuarioFilialPerfil = objUsuarioFilialPerfil;

            Funcao objFuncao;
            if (Funcao.CarregarPorDescricao(txtFuncaoExercida.Text, out objFuncao))
            {
                objUsuarioFilial.Funcao = objFuncao;
                objUsuarioFilial.DescricaoFuncao = null;
            }
            else
            {
                objUsuarioFilial.Funcao = null;
                objUsuarioFilial.DescricaoFuncao = txtFuncaoExercida.Text;
            }

            objUsuarioFilial.NumeroRamal = txtRamal.Valor;
            objUsuarioFilial.NumeroComercial = txtTelefone.Fone;
            objUsuarioFilial.NumeroDDDComercial = txtTelefone.DDD;
            objUsuarioFilial.EmailComercial = txtEmail.Text;

            #endregion

            if (IdUsuarioFilialPerfil.HasValue && IdPessoaFisica.HasValue)
            {
                //Verifica se o usuario logado esta tentando se inativar
                if (objUsuarioFilialPerfil.FlagInativo &&
                    IdUsuarioFilialPerfil.Value.Equals(base.IdUsuarioFilialPerfilLogadoEmpresa.Value) &&
                     IdPessoaFisica.Value.Equals(base.IdPessoaFisicaLogada.Value))
                {
                    ExibirMensagem("Não é possível desativar o usuário atual", TipoMensagem.Aviso);
                    return;
                }
            }

            bool salvar = objUsuarioFilial.SalvarUsuarioFilial(objPessoaFisica, objUsuarioFilialPerfil, objUsuario);

            if (salvar)
            {
                #region EnvioSMS
                
                string idUFPRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdfUfpEnvioSMSTanqueCadastroEmpresa);

                string smsUm = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.SMSBoasVindasUm);
                smsUm = smsUm.Replace("{Nome_Usuario}", objPessoaFisica.PrimeiroNome);

                string smsDois = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.SMSBoasVindasDois);
                smsDois = smsDois.Replace("{Funcao_Usuario}", String.IsNullOrEmpty(objUsuarioFilial.DescricaoFuncao) ? objFuncao.DescricaoFuncao : objUsuarioFilial.DescricaoFuncao);

                List<BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque> listaUsuarios = new List<BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque>();

                var objUsuarioEnvioSMS = new BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque
                {
                    dddCelular = objPessoaFisica.NumeroDDDCelular,
                    numeroCelular = objPessoaFisica.NumeroCelular,
                    nomePessoa = objPessoaFisica.NomePessoa,
                    mensagem = smsUm,
                    idDestinatario = objUsuarioFilialPerfil.IdUsuarioFilialPerfil
                };

                listaUsuarios.Add(objUsuarioEnvioSMS);

                var objUsuarioEnvioSMSDois = new BNE.BLL.DTO.PessoaFisicaEnvioSMSTanque
                {
                    dddCelular = objPessoaFisica.NumeroDDDCelular,
                    numeroCelular = objPessoaFisica.NumeroCelular,
                    nomePessoa = objPessoaFisica.NomePessoa,
                    mensagem = smsDois,
                    idDestinatario = objUsuarioFilialPerfil.IdUsuarioFilialPerfil
                };

                listaUsuarios.Add(objUsuarioEnvioSMSDois);

                Mensagem.EnvioSMSTanque(idUFPRemetente, listaUsuarios);

                #endregion

                ucModalConfirmacao.PreencherCampos("Confirmação", MensagemAviso._100001, false);
                ucModalConfirmacao.MostrarModal();
            }
        }
        #endregion

        #region Excluir
        /// <summary>
        /// Exclui um usuário
        /// </summary>
        /// <returns></returns>
        private bool Excluir()
        {
            try
            {
                UsuarioFilialPerfil objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(IdUsuarioFilialPerfil.Value);
                objUsuarioFilialPerfil.FlagInativo = true;
                objUsuarioFilialPerfil.Save();

                IdUsuarioFilialPerfil = null;

                return true;

            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
            return false;
        }
        #endregion

        #region LimparCampos
        /// <summary>
        /// Limpa os valores de cada campo na tela.
        /// </summary>
        private void LimparCampos(bool limparCampoCpf)
        {
            IdPessoaFisica = null;
            IdUsuarioFilialPerfil = null;

            if (limparCampoCpf)
                txtCPF.Valor = string.Empty;

            rblSexo.ClearSelection();
            txtDataNascimento.Valor = string.Empty;
            txtNome.Valor = string.Empty;
            txtFuncaoExercida.Text = string.Empty;
            txtTelefone.DDD = string.Empty;
            txtTelefone.Fone = string.Empty;
            txtRamal.Valor = string.Empty;
            txtTelefoneCelular.DDD = string.Empty;
            txtTelefoneCelular.Fone = string.Empty;
            txtEmail.Text = string.Empty;
            ckbMaster.Checked = false;

            //Reabilitando CPF e DtaNasc
            txtCPF.Enabled = true;
            txtDataNascimento.Enabled = true;
            ckbMaster.Checked = false;

            upDadosBasicos.Update();
        }
        #endregion

        #region VerificaNumeroCpf
        /// <summary>
        /// Verifica se o número do CPF que foi informado na tela já existe para aquela empresa,ou se a pessoa fisica não 
        /// </summary>
        /// <returns>Existe - true| Não Existe - False</returns>
        public bool VerificaNumeroCpf()
        {
            PessoaFisica objPessoaFisica;
            if (PessoaFisica.CarregarPorCPF(Convert.ToDecimal(txtCPF.Valor), out objPessoaFisica))
            {
                UsuarioFilialPerfil objUsuarioFilialPerfil;
                if (UsuarioFilialPerfil.CarregarPorPessoaFisicaFilial(objPessoaFisica.IdPessoaFisica, base.IdFilial.HasValue ? base.IdFilial.Value : IdFilial.Value, out objUsuarioFilialPerfil))
                    IdUsuarioFilialPerfil = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;

                HabilitarCamposCpfExistenteBaseDados(false);

                IdPessoaFisica = objPessoaFisica.IdPessoaFisica;
                return true;
            }
            return true;
        }
        #endregion

        #region HabilitarCamposCpfExistenteEmpresa
        /// <summary>
        /// Caso o CPF se encontre cadastrado como usuario na empresa os campos serão desabilitados
        /// </summary>
        /// <param name="enable"></param>
        public void HabilitarCamposCpfExistenteEmpresa(bool enable)
        {
            txtDataNascimento.Enabled = enable;
            txtNome.Enabled = enable;
            rblSexo.Enabled = enable;
            txtTelefone.Enabled = enable;
            txtRamal.Enabled = enable;
            txtTelefoneCelular.Enabled = enable;
            txtEmail.Enabled = enable;
            ckbMaster.Enabled = enable;
            txtFuncaoExercida.Enabled = enable;

            if (!IdPerfilUsuarioLogado.Equals(Enumeradores.Perfil.AcessoEmpresaMaster.GetHashCode()) && !base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue) //Se não for um usuário interno)
            {
                btnSalvar.Enabled = false;
                upBotoesCadastro.Update();
                upDadosBasicos.Update();
                return;
            }

            btnSalvar.Enabled = enable;

            upBotoesCadastro.Update();
            upDadosBasicos.Update();
        }

        #endregion

        #region HabilitarCamposCpfExistenteBaseDados
        /// <summary>
        /// Caso o CPF se encontre cadastrado na base de dados, não poderão ser alterados
        /// </summary>
        /// <param name="enable"></param>
        public void HabilitarCamposCpfExistenteBaseDados(bool enable)
        {
            txtDataNascimento.Enabled = enable;
            txtNome.Enabled = enable;
            rblSexo.Enabled = enable;
            txtTelefoneCelular.Enabled = enable;

            upDadosBasicos.Update();
            upBotoesCadastro.Update();
        }

        #endregion

        #region HabilitarTodosOsCampos

        public void HabilitarTodosOsCampos(bool enable)
        {
            txtCPF.Enabled = enable;
            txtDataNascimento.Enabled = enable;
            txtNome.Enabled = enable;
            rblSexo.Enabled = enable;
            txtFuncaoExercida.Enabled = enable;
            txtTelefoneCelular.Enabled = enable;
            txtTelefoneCelular.Enabled = enable;
            txtEmail.Enabled = enable;
            ckbMaster.Enabled = enable;
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
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, Enumeradores.CategoriaPermissao.TelaCadastroEmpresaUsuario);

                if (!Permissoes.Contains((int)Enumeradores.Permissoes.TelaCadastroEmpresaUsuario.AcessarTelaCadastroUsuários))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, Enumeradores.CategoriaPermissao.TelaCadastroEmpresaUsuario);

                if (!Permissoes.Contains((int)Enumeradores.Permissoes.TelaCadastroEmpresaUsuario.AcessarTelaCadastroUsuários))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), null));
        }
        #endregion

        #region AjustarUsuarioNaoMasterModoEdicao
        public void AjustarUsuarioNaoMasterModoEdicao(bool enable)
        {
            txtCPF.Enabled = enable;
            txtNome.Enabled = enable;
            txtTelefoneCelular.Enabled = enable;
            rblSexo.Enabled = enable;
            txtDataNascimento.Enabled = enable;
        }
        #endregion

        #region AjustarCamposEdicao
        /// <summary>
        /// Metodo resposável por Ajustas os campos para a atualização
        /// </summary>
        /// <param name="enable"></param>
        private void AjustarCamposEdicao(bool enable)
        {
            txtCPF.Enabled = enable;
            txtDataNascimento.Enabled = enable;
        }

        #endregion

        #region FecharModal
        public void FecharModal()
        {
            mpeConfirmacaoExclusao.Hide();
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        /// <summary>
        /// Método executado toda a vez que a tela for carregada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            //Ajustando a expressao de validacao.
            revEmail.ValidationExpression = Configuracao.regexEmail;

            InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "CadastroEmpresaUsuario");
        }
        #endregion

        #region txtCPF_ValorAlterado
        /// <summary>
        /// Responsável por executar os procedimentos necessários para validar e carregar os dados da tela conforme 
        /// o cpf e data de nascimento informados.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtCPF_ValorAlterado(object sender, EventArgs e)
        {
            HabilitarCamposCpfExistenteEmpresa(true);

            if (VerificaNumeroCpf())
                PreencherCampos();
            else
            {
                HabilitarCamposCpfExistenteEmpresa(false);
                ExibirMensagem(MensagemAviso._24018, TipoMensagem.Aviso);
                LimparCampos(false);
            }
        }
        #endregion

        #region gvUsuarios_PageIndexChanged
        protected void gvUsuarios_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            PageIndex = e.NewPageIndex + 1;

            if (IdUsuarioFilialPerfil.HasValue)
                AjustarCamposEdicao(false);
            else
                AjustarCamposEdicao(true);

            CarregarGridUsuarios();
            upGvUsuarios.Update();
        }
        #endregion

        #region btnSalvar
        /// <summary>
        /// Executa os procedimentos necessários para salvar as informações da tela.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar();
                LimparCampos(true);
                CarregarGridUsuarios();
                HabilitarTodosOsCampos(true);
                upDadosBasicos.Update();

                if (!IdPerfilUsuarioLogado.Equals((int)Enumeradores.Perfil.AcessoEmpresaMaster) && !base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                {
                    btnSalvar.Enabled = false;
                    AjustarUsuarioNaoMasterModoEdicao(true);
                }
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region gvUsuarios

        #region gvUsuarios_ItemCommand
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void gvUsuarios_ItemCommand(object source, GridCommandEventArgs e)
        {
            HabilitarCamposCpfExistenteBaseDados(true);
            HabilitarCamposCpfExistenteEmpresa(true);

            if (e.CommandName.Equals("Atualizar"))
            {
                int idUsuarioFilialPerfil = Convert.ToInt32(gvUsuarios.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Usuario_Filial_Perfil"]);
                UsuarioFilialPerfil objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(idUsuarioFilialPerfil);

                IdUsuarioFilialPerfil = idUsuarioFilialPerfil;
                IdPessoaFisica = objUsuarioFilialPerfil.PessoaFisica.IdPessoaFisica;

                if (!IdPerfilUsuarioLogado.Equals(Enumeradores.Perfil.AcessoEmpresaMaster.GetHashCode()) && !base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                    ckbMaster.Enabled = false;
                else
                    ckbMaster.Enabled = true;

                btnSalvar.Enabled = true;
                PreencherCampos();
                CarregarGridUsuarios();
                btnCancelar.Visible = true;

                AjustarCamposEdicao(false);

                upDadosBasicos.Update();
                upBotoesCadastro.Update();
            }
            else if (e.CommandName.Equals("Deletar"))
            {
                int idUsuarioFilialPerfil = Convert.ToInt32(gvUsuarios.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Usuario_Filial_Perfil"]);
                IdUsuarioFilialPerfil = idUsuarioFilialPerfil;
                mpeConfirmacaoExclusao.Show();
            }
        }
        #endregion

        #endregion

        #region btnCancelar_Click
        /// <summary>
        /// Somente no Modo de edição
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimparCampos(true);
            AjustarCamposEdicao(true);
            ckbMaster.Enabled = true;
            btnCancelar.Visible = false;

            if (!IdPerfilUsuarioLogado.Equals((int)Enumeradores.Perfil.AcessoEmpresaMaster) && !base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                btnSalvar.Enabled = false;
            else
                btnSalvar.Enabled = true;

            upBotoesCadastro.Update();
        }

        #endregion

        #region Modal

        #region btnExcluirModalExclusao_Click
        /// <summary>
        /// Executa os procedimentos de exclusão do usuário selecionado e fecha a modal de confirmação de exclusão.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcluirModalExclusao_Click(object sender, EventArgs e)
        {
            if (Excluir())
            {
                CarregarGridUsuarios();
                btnSalvar.Enabled = true;
                upDadosBasicos.Update();
                upGvUsuarios.Update();
            }

            mpeConfirmacaoExclusao.Hide();
        }
        #endregion

        #region btnCancelarModalExclusao
        /// <summary>
        /// Cancela a excluão do usuário e fecha a modal.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelarModalExclusao_Click(object sender, EventArgs e)
        {
            mpeConfirmacaoExclusao.Hide();
            btnSalvar.Enabled = true;
        }
        #endregion

        #region btnFinalizar_Click
        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            if (UrlOrigem != null)
                Redirect(UrlOrigem);
            else
                Redirect("Default.aspx");
        }
        #endregion

        #endregion

        #region btiFechar_Click
        protected void btiFechar_Click(object sender, EventArgs e)
        {
            FecharModal();
        }
        #endregion

        #endregion

    }
}
