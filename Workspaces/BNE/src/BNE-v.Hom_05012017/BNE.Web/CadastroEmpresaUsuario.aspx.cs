using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Ajax;
using BNE.BLL;
using BNE.BLL.Enumeradores;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Resources;
using RestSharp.Extensions;
using Telerik.Web.UI;
using CategoriaPermissao = BNE.BLL.Enumeradores.CategoriaPermissao;
using Funcao = BNE.BLL.Funcao;
using Parametro = BNE.BLL.Parametro;
using Perfil = BNE.BLL.Enumeradores.Perfil;
using Sexo = BNE.BLL.Sexo;
using TipoMensagem = BNE.Web.Code.Enumeradores.TipoMensagem;
using BNE.BLL.Assincronos;

namespace BNE.Web
{
    public partial class CadastroEmpresaUsuario : BasePage
    {
        #region Propriedades

        #region IdFilial - Variável 1
        /// <summary>
        ///     Propriedade que armazena e recupera o ID da Filial.
        /// </summary>
        public int? IdFilial
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return int.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());
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
        ///     Propriedade que armazena e recupera o ID da Pessoa Fisica.
        /// </summary>
        public int? IdPessoaFisica
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel2.ToString()] != null)
                    return int.Parse(ViewState[Chave.Temporaria.Variavel2.ToString()].ToString());
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
        ///     Propriedade que armazena e recupera o ID do UsuarioFilialPerfil.
        /// </summary>
        public int? IdUsuarioFilialPerfil
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel3.ToString()] != null)
                    return int.Parse(ViewState[Chave.Temporaria.Variavel3.ToString()].ToString());
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

        #region IdUsuarioFilialContato - Variável 4
        /// <summary>
        ///     Identificador de contato
        /// </summary>
        public int? IdUsuarioFilialContato
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel4.ToString()] != null)
                    return int.Parse(ViewState[Chave.Temporaria.Variavel4.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel4.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel4.ToString());
            }
        }
        #endregion

        #region IdPerfilUsuarioLogado - Variável 5
        /// <summary>
        ///     Verifica se existe O CPF informado na base de dados
        /// </summary>
        public int IdPerfilUsuarioLogado
        {
            get { return Convert.ToInt32(ViewState[Chave.Temporaria.Variavel5.ToString()]); }
            set { ViewState.Add(Chave.Temporaria.Variavel5.ToString(), value); }
        }
        #endregion

        #region IdPerfilUsuarioLogado - Variável 6
        /// <summary>
        ///     Verifica se existe O CPF informado na base de dados
        /// </summary>
        public int idNovoResponsavel
        {
            get { return Convert.ToInt32(ViewState[Chave.Temporaria.Variavel6.ToString()]); }
            set { ViewState.Add(Chave.Temporaria.Variavel6.ToString(), value); }
        }
        #endregion

        #region Permissoes - Variável Permissoes
        /// <summary>
        ///     Propriedade que armazena e recupera o IdPesquisaCurriculo
        /// </summary>
        protected List<int> Permissoes
        {
            get { return (List<int>)ViewState[Chave.Temporaria.Permissoes.ToString()]; }
            set { ViewState.Add(Chave.Temporaria.Permissoes.ToString(), value); }
        }
        #endregion

        #region UrlOrigem - Variável 6
        /// <summary>
        ///     Propriedade que armazena e recupera o enumerador do login origem
        /// </summary>
        public string UrlOrigem
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel6.ToString()] != null)
                    return ViewState[Chave.Temporaria.Variavel6.ToString()].ToString();
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
        ///     Responsável por configurar a tela em seu modo inicial, configurando os controles necessários.
        /// </summary>
        private void Inicializar()
        {
            AjustarPermissoes();

            if (Request.UrlReferrer != null)
                UrlOrigem = Request.UrlReferrer.AbsoluteUri.ToString(CultureInfo.CurrentCulture);

            lblMensagemGridUsuarios.Text = "Responsável pelo Cadastro - Poderá realizar alterações nos Dados da Empresa e Cadastros de Usuários";

            //TODO: Rever esta parte, possibilidade de um usuário interno cadastrar usuários
            if (!IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue) //Se não for um usuário interno
            {
                var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(IdUsuarioFilialPerfilLogadoEmpresa.Value);
                IdPerfilUsuarioLogado = objUsuarioFilialPerfil.Perfil.IdPerfil;

                btnSalvar.Enabled = !IdPerfilUsuarioLogado.Equals((int)Perfil.AcessoEmpresaMaster);

                gvUsuarios.Columns.FindByUniqueName("Status").Visible = false;
                pnlFiltroAtivosInativos.Visible = false;
            }
            else
            {
                var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(IdUsuarioFilialPerfilLogadoUsuarioInterno.Value);
                IdPerfilUsuarioLogado = objUsuarioFilialPerfil.Perfil.IdPerfil;

                pnlFiltroAtivosInativos.Visible = true;
                gvUsuarios.Columns.FindByUniqueName("Status").Visible = true;
            }

            //Carregar Listas de Sugestões.
            UIHelper.CarregarRadioButtonList(rblSexo, Sexo.Listar());
            var dicionario = new Dictionary<string, string>
            {
                {"1", "Usuários Ativos"},
                {"2", "Usuários Inativos"},
                {"3", "Usuários Ativos e Inativos"}
            };

            UIHelper.CarregarRadComboBox(rcbStatus, dicionario);

            rcbStatus.SelectedValue = "3";

            gvUsuarios.PageSize = Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.QuantidadePaginacaoCadastroUsuario));

            CarregarParametros();

            #region Data de Nascimento

            //Aplicação de Regra: 
            //- Intervalo da Data de Nascimento. ( 14 anos >= idade <= 80 anos )

            var idadeMinima = Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.IdadeMinima));
            var idadeMaxima = Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.IdadeMaxima));
            var dataAtual = DateTime.Now;
            try
            {
                var dataMinima = Convert.ToDateTime(dataAtual.Day.ToString(CultureInfo.CurrentCulture) + "/" + dataAtual.Month.ToString(CultureInfo.CurrentCulture) + "/" + (dataAtual.Year - idadeMaxima).ToString(CultureInfo.CurrentCulture));
                var dataMaxima = Convert.ToDateTime(dataAtual.Day.ToString(CultureInfo.CurrentCulture) + "/" + dataAtual.Month.ToString(CultureInfo.CurrentCulture) + "/" + (dataAtual.Year - idadeMinima).ToString(CultureInfo.CurrentCulture));
                txtDataNascimento.DataMinima = dataMinima;
                txtDataNascimento.DataMaxima = dataMaxima;
            }
            catch
            {
                txtDataNascimento.DataMinima = DateTime.MinValue;
                txtDataNascimento.DataMaxima = DateTime.MaxValue;
            }

            txtDataNascimento.MensagemErroIntervalo = string.Format(MensagemAviso._100005, idadeMinima, idadeMaxima);
            #endregion

            CarregarGridUsuarios();
            CarregarDadosFilial();
            CarregarGridContatos();

            //Task: 41751
            if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
            {
                AjustarObrigatoriedadeNosCamposUsuario(false);
                AjustarObrigatoriedadeNosCamposContato(true);
            }
            else
                AjustarObrigatoriedadeNosCamposUsuario(true);

            //Foco inicial
            if (string.IsNullOrEmpty(txtCPF.Valor))
                txtCPF.Focus();
            else
                txtNome.Focus();

            Utility.RegisterTypeForAjax(typeof(CadastroEmpresaUsuario));

        }
        #endregion

        
        private bool UsuarioInterno()
        {
            return base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue;
        }

        #region CarregarParametros
        /// <summary>
        ///     Inicializa os controles com os valores pré-definidos em parâmetro.
        /// </summary>
        private void CarregarParametros()
        {
            try
            {
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
        ///     Carrega a Grid de Usuários
        /// </summary>
        public void CarregarGridUsuarios()
        {
            int totalRegistros;
            if (UsuarioInterno())
            {
                bool? flagAtivosInativos = null; //NULL busca todas os usuários
                if (rcbStatus.SelectedValue.Equals("1")) //Se for Usuários ativos
                    flagAtivosInativos = true;
                else if (rcbStatus.SelectedValue.Equals("2")) //Se for usuários inativos
                    flagAtivosInativos = false;
                UIHelper.CarregarRadGrid(gvUsuarios, UsuarioFilialPerfil.CarregarUsuariosCadastradosPorFilial(gvUsuarios.CurrentPageIndex, gvUsuarios.PageSize, IdFilial.Value, (int)Perfil.AcessoEmpresaMaster, IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, flagAtivosInativos, out totalRegistros), totalRegistros);
            }
            else
            {
                UIHelper.CarregarRadGrid(gvUsuarios, UsuarioFilialPerfil.CarregarUsuariosCadastradosPorFilial(gvUsuarios.CurrentPageIndex, gvUsuarios.PageSize, base.IdFilial.Value, IdPerfilUsuarioLogado, IdUsuarioFilialPerfilLogadoEmpresa.Value, true, out totalRegistros), totalRegistros);
            }

            upGvUsuarios.Update();
        }
        #endregion

        #region CarregarGridContatos
        /// <summary>
        ///     Carrega a Grid de Contatos
        /// </summary>
        public void CarregarGridContatos()
        {
            if (UsuarioInterno())
            {
                int totalRegistros;
                bool? flagAtivosInativos = null; //NULL busca todas os usuários
                if (rcbStatus.SelectedValue.Equals("1")) //Se for Usuários ativos
                    flagAtivosInativos = true;
                else if (rcbStatus.SelectedValue.Equals("2")) //Se for usuários inativos
                    flagAtivosInativos = false;
                UIHelper.CarregarRadGrid(gvContatos, UsuarioFilialContato.CarregarContatosCadastradosPorFilial(gvContatos.CurrentPageIndex, gvUsuarios.PageSize, IdFilial.Value, (int)Perfil.AcessoEmpresaMaster, IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, flagAtivosInativos, out totalRegistros), totalRegistros);

                gvContatos.Visible = true;
            }
            else
            {
                gvContatos.Visible = false;
            }

            upGvContatos.Update();
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
        ///     Preenche quando a mesma estiver em modo de edição.
        /// </summary>
        private void PreencherCampos()
        {
            #region Dados Pessoais
            if (IdUsuarioFilialContato.HasValue)
            {
                var objUsuarioFilialContato = UsuarioFilialContato.LoadObject((int)IdUsuarioFilialContato);

                if (objUsuarioFilialContato.NumeroCPF.HasValue)
                {
                    txtCPF.Valor = objUsuarioFilialContato.CPF;
                }
                txtNome.Valor = objUsuarioFilialContato.NomeContato;
                txtEmail.Text = objUsuarioFilialContato.EmailContato;
                if (objUsuarioFilialContato.DataNascimento.HasValue)
                {
                    txtDataNascimento.Valor = objUsuarioFilialContato.DataNascimento.Value.ToShortDateString();
                }
                if (objUsuarioFilialContato.Sexo != null)
                {
                    rblSexo.SelectedValue = objUsuarioFilialContato.Sexo.IdSexo.ToString(CultureInfo.CurrentCulture);
                }
                if (!string.IsNullOrEmpty(objUsuarioFilialContato.NumeroDDDCelular) && !string.IsNullOrEmpty(objUsuarioFilialContato.NumeroCelular))
                {
                    txtTelefoneCelular.DDD = objUsuarioFilialContato.NumeroDDDCelular;
                    txtTelefoneCelular.Fone = objUsuarioFilialContato.NumeroCelular;
                }
                else
                {
                    ObrigatoriedadeCelular(false);
                }
                if (objUsuarioFilialContato.DDDTelefone != null && objUsuarioFilialContato.NumeroTelefone != null)
                {
                    txtTelefone.DDD = objUsuarioFilialContato.DDDTelefone;
                    txtTelefone.Fone = objUsuarioFilialContato.NumeroTelefone;

                    if (objUsuarioFilialContato.NumeroRamal != null)
                        txtRamal.Valor = objUsuarioFilialContato.NumeroRamal;
                }
                if (objUsuarioFilialContato.Funcao != null)
                {
                    objUsuarioFilialContato.Funcao.CompleteObject();
                    txtFuncaoExercida.Text = objUsuarioFilialContato.Funcao.DescricaoFuncao;
                }
                else
                    txtFuncaoExercida.Text = objUsuarioFilialContato.DescricaoFuncao;
            }

            if (IdPessoaFisica.HasValue || IdUsuarioFilialPerfil.HasValue)
            {

                #region Pessoa Física
                if (IdPessoaFisica.HasValue)
                {
                    var objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);
                    if (objPessoaFisica != null)
                    {
                        txtCPF.Valor = objPessoaFisica.NumeroCPF;
                        txtDataNascimento.Valor = objPessoaFisica.DataNascimento.ToShortDateString();
                        txtNome.Valor = objPessoaFisica.NomePessoa;
                        rblSexo.SelectedValue = objPessoaFisica.Sexo.IdSexo.ToString(CultureInfo.CurrentCulture);

                        if (!string.IsNullOrEmpty(objPessoaFisica.NumeroDDDCelular) && !string.IsNullOrEmpty(objPessoaFisica.NumeroCelular))
                        {
                            txtTelefoneCelular.DDD = objPessoaFisica.NumeroDDDCelular;
                            txtTelefoneCelular.Fone = objPessoaFisica.NumeroCelular;
                        }
                        else
                        {
                            ObrigatoriedadeCelular(false);
                        }
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

                    var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(IdUsuarioFilialPerfil.Value);
                    ckbMaster.Checked = objUsuarioFilialPerfil.Perfil.IdPerfil.Equals((int)Perfil.AcessoEmpresaMaster);
                    ckbResponsavel.Checked = objUsuarioFilialPerfil.FlagUsuarioResponsavel;
                }
                #endregion

            }
            #endregion

            upDadosBasicos.Update();
        }
        #endregion

        #region ObrigatoriedadeCelular
        protected void ObrigatoriedadeCelular(bool obrigatorio)
        {
            txtTelefoneCelular.Obrigatorio = obrigatorio;

            if (!obrigatorio)
            {
                txtTelefoneCelular.CssClassTextBoxDDD = txtTelefoneCelular.CssClassTextBoxDDD.Replace(" campo_obrigatorio", "");
                txtTelefoneCelular.CssClassTextBoxDDI = txtTelefoneCelular.CssClassTextBoxDDI.Replace(" campo_obrigatorio", "");
                txtTelefoneCelular.CssClassTextBoxFone = txtTelefoneCelular.CssClassTextBoxFone.Replace(" campo_obrigatorio", "");
            }
            else
            {
                txtTelefoneCelular.CssClassTextBoxDDD = txtTelefoneCelular.CssClassTextBoxDDD + " campo_obrigatorio";
                txtTelefoneCelular.CssClassTextBoxDDI = txtTelefoneCelular.CssClassTextBoxDDI + " campo_obrigatorio";
                txtTelefoneCelular.CssClassTextBoxFone = txtTelefoneCelular.CssClassTextBoxFone + " campo_obrigatorio";
            }
        }
        #endregion

        #region Salvar
        /// <summary>
        ///     Salva as informações da tela
        /// </summary>
        /// <returns></returns>
        private bool Salvar(out string erro)
        {
            erro = string.Empty;
            string descricao = string.Empty;

            PessoaFisica objPessoaFisica = null;
            PessoaFisica objPessoaFisicaOld = null;

            if (IdPessoaFisica.HasValue)
            {
                objPessoaFisica = PessoaFisica.LoadObject(IdPessoaFisica.Value);
                objPessoaFisicaOld = objPessoaFisica.Clone() as PessoaFisica;
            }
            else
            {
                if (TodosOsCamposEstaoPreenchidos()) //Se todos os campos estão preenchidos
                {
                    objPessoaFisica = new PessoaFisica { FlagInativo = false }; //Então cria uma pessoa física
                }
            }

            //Insntanciando um contato, se houver.
            UsuarioFilialContato objUsuarioFilialContato = IdUsuarioFilialContato.HasValue ? UsuarioFilialContato.LoadObject((int)IdUsuarioFilialContato) : null;
            UsuarioFilialContato objUsuarioFilialContatoOld = null;

            if (IdUsuarioFilialContato.HasValue)
                objUsuarioFilialContatoOld = objUsuarioFilialContato.Clone() as UsuarioFilialContato;

            var objFilial = new Filial(base.IdFilial.HasValue ? base.IdFilial.Value : IdFilial.Value);
            var funcao = txtFuncaoExercida.Text;
            Funcao objFuncao;
            if (Funcao.CarregarPorDescricao(txtFuncaoExercida.Text, out objFuncao))
            {
                funcao = null;
            }

            bool salvar;
            //Ajusta valores de pessoa fisica, se for edição de uma já existente ou ter um cpf informado
            if (objPessoaFisica != null)
            {
                objPessoaFisica.NomePessoa = txtNome.Valor;
                objPessoaFisica.NumeroCPF = txtCPF.Valor;
                objPessoaFisica.NumeroCelular = txtTelefoneCelular.Fone;
                objPessoaFisica.NumeroDDDCelular = txtTelefoneCelular.DDD;
                objPessoaFisica.Sexo = new Sexo(Convert.ToInt32(rblSexo.SelectedValue));
                objPessoaFisica.DataNascimento = DateTime.Parse(txtDataNascimento.Valor);
                objPessoaFisica.NomePessoaPesquisa = txtNome.Valor;

                Usuario objUsuario;
                if (!Usuario.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objUsuario))
                {
                    objUsuario = new Usuario
                    {
                        PessoaFisica = objPessoaFisica,
                        SenhaUsuario = objPessoaFisica.DataNascimento.ToString("ddMMyyyy")
                    };
                }

                #region BNE_Usuario_Filial, Usuario_Filial_Perfil

                UsuarioFilial objUsuarioFilial;
                UsuarioFilialPerfil objUsuarioFilialPerfil;

                UsuarioFilial objUsuarioFilialOld = null;
                UsuarioFilialPerfil objUsuarioFilialPerfilOld = null;

                if (IdUsuarioFilialPerfil.HasValue)
                {
                    objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(IdUsuarioFilialPerfil.Value);
                    objUsuarioFilialPerfilOld = objUsuarioFilialPerfil.Clone() as UsuarioFilialPerfil;

                    UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(IdUsuarioFilialPerfil.Value, out objUsuarioFilial);

                    if (objUsuarioFilial == null)
                        objUsuarioFilial = new UsuarioFilial();
                    else
                        objUsuarioFilialOld = objUsuarioFilial.Clone() as UsuarioFilial;
                }
                else
                {
                    objUsuarioFilialPerfil = new UsuarioFilialPerfil();
                    objUsuarioFilial = new UsuarioFilial();
                }

                objUsuarioFilialPerfil.Filial = objFilial;
                objUsuarioFilialPerfil.DescricaoIP = Request.ServerVariables["REMOTE_ADDR"];
                objUsuarioFilialPerfil.PessoaFisica = objPessoaFisica;
                objUsuarioFilialPerfil.Perfil = new BLL.Perfil(ckbMaster.Checked ? (int)Perfil.AcessoEmpresaMaster : (int)Perfil.AcessoEmpresa);
                objUsuarioFilialPerfil.SenhaUsuarioFilialPerfil = objPessoaFisica.DataNascimento.ToString("ddMMyyyy");
                objUsuarioFilialPerfil.FlagUsuarioResponsavel = ckbResponsavel.Checked;

                objUsuarioFilial.UsuarioFilialPerfil = objUsuarioFilialPerfil;
                objUsuarioFilial.Funcao = objFuncao;
                objUsuarioFilial.DescricaoFuncao = funcao;
                objUsuarioFilial.NumeroRamal = txtRamal.Valor;
                objUsuarioFilial.NumeroComercial = txtTelefone.Fone;
                objUsuarioFilial.NumeroDDDComercial = txtTelefone.DDD;
                objUsuarioFilial.EmailComercial = txtEmail.Text;
                #endregion

                if (IdUsuarioFilialPerfil.HasValue && IdPessoaFisica.HasValue && IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                {
                    //Verifica se o usuario logado esta tentando se inativar
                    if (objUsuarioFilialPerfil.FlagInativo &&
                        IdUsuarioFilialPerfil.Value.Equals(IdUsuarioFilialPerfilLogadoEmpresa.Value) &&
                        IdPessoaFisica.Value.Equals(IdPessoaFisicaLogada.Value))
                    {
                        ExibirMensagem("Não é possível desativar o usuário atual", TipoMensagem.Aviso);
                        return false;
                    }
                }

                salvar = objUsuarioFilial.SalvarUsuarioFilial(objPessoaFisica, objUsuarioFilialPerfil, objUsuario, objUsuarioFilialContato, out erro);

                descricao += objUsuarioFilialOld != null ? MensagemAssincronoLogCRM.StringListaDeItensModificados(MensagemAssincronoLogCRM.DiffProperties(objUsuarioFilialOld, objUsuarioFilial, objUsuarioFilial.GetType())) : "Novo Cadastro de Usuario Filial. \n";
                descricao += objUsuarioFilialPerfilOld != null ? MensagemAssincronoLogCRM.StringListaDeItensModificados(MensagemAssincronoLogCRM.DiffProperties(objUsuarioFilialPerfilOld, objUsuarioFilialPerfil, objUsuarioFilialPerfilOld.GetType())) : "Novo Cadastro de Usuario Filial Perfil. \n";

            }
            else //Se não existe pessoa física, então é apenas um contato
            {
                if (objUsuarioFilialContato == null)
                {
                    objUsuarioFilialContato = new UsuarioFilialContato();
                }
                objUsuarioFilialContato.CPF = txtCPF.Valor;
                objUsuarioFilialContato.DataNascimento = txtDataNascimento.ValorDatetime;
                objUsuarioFilialContato.NomeContato = txtNome.Valor;
                objUsuarioFilialContato.EmailContato = txtEmail.Text;
                objUsuarioFilialContato.Funcao = objFuncao;
                objUsuarioFilialContato.DescricaoFuncao = funcao;
                objUsuarioFilialContato.DDDTelefone = txtTelefone.DDD;
                objUsuarioFilialContato.NumeroTelefone = txtTelefone.Fone;
                objUsuarioFilialContato.NumeroDDDCelular = txtTelefoneCelular.DDD;
                objUsuarioFilialContato.NumeroCelular = txtTelefoneCelular.Fone;
                objUsuarioFilialContato.Filial = objFilial;

                if (!string.IsNullOrWhiteSpace(rblSexo.SelectedValue) && rblSexo.SelectedValue != "0")
                {
                    objUsuarioFilialContato.Sexo = new Sexo(Convert.ToInt32(rblSexo.SelectedValue));
                }
                else
                {
                    objUsuarioFilialContato.Sexo = null;
                }

                objUsuarioFilialContato.Save();
                salvar = true;
            }

            if (!salvar)
            {
                if (!string.IsNullOrEmpty(descricao))
                    MensagemAssincronoLogCRM.SalvarModificacoesFilialCRM(descricao, objFilial, new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value), null);
                else
                    MensagemAssincronoLogCRM.SalvarModificacoesFilialCRM("Cadastro de um Usuário Novo \n Nome: " + objPessoaFisica.NomeCompleto + "\n CPF: " + objPessoaFisica.CPF, objFilial, new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value), null);

                return false;
            }


            //Mostra mensagem de sucesso
            ucModalConfirmacao.PreencherCampos("Confirmação", MensagemAviso._100001, false);
            ucModalConfirmacao.MostrarModal();



            descricao += objPessoaFisicaOld != null ? MensagemAssincronoLogCRM.StringListaDeItensModificados(MensagemAssincronoLogCRM.DiffProperties(objPessoaFisicaOld, objPessoaFisica, objPessoaFisica.GetType())) : string.Empty;
            descricao += objUsuarioFilialContatoOld != null ? MensagemAssincronoLogCRM.StringListaDeItensModificados(MensagemAssincronoLogCRM.DiffProperties(objUsuarioFilialContatoOld, objUsuarioFilialContato, objUsuarioFilialContatoOld.GetType())) : string.Empty;

            if (!string.IsNullOrEmpty(descricao))
                MensagemAssincronoLogCRM.SalvarModificacoesFilialCRM(descricao, objFilial, new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value), null);
            else
            {
                if (objPessoaFisica != null)
                    MensagemAssincronoLogCRM.SalvarModificacoesFilialCRM("Cadastro de um Usuário Novo \n Nome: " + objPessoaFisica.NomeCompleto + "\n CPF: " + objPessoaFisica.CPF, objFilial, new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value), null);
                else
                    MensagemAssincronoLogCRM.SalvarModificacoesFilialCRM("Cadastro de um Contato Novo \n Nome" + objUsuarioFilialContato.NomeContato + "\n Email: " + objUsuarioFilialContato.EmailContato, objFilial, new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value), null);
            }

            return true;
        }
        #endregion

        #region Excluir
        /// <summary>
        ///     Exclui um usuário
        /// </summary>
        /// <returns></returns>
        private bool Excluir(bool confirmacao)
        {
            try
            {
                if (IdUsuarioFilialPerfil.HasValue)
                {
                    string descricao = "Exclusão de Usuário! ";

                    var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(IdUsuarioFilialPerfil.Value);
                    var objUsuarioFilialPerfilOld = objUsuarioFilialPerfil.Clone() as UsuarioFilialPerfil;

                    if (!confirmacao && objUsuarioFilialPerfil.FlagUsuarioResponsavel)
                    {
                        UIHelper.CarregarRadGrid(gvResponsavel, UsuarioFilialPerfil.CarregarUsuariosMasterPorFilial(IdFilial.Value));
                        gvResponsavel.DataBind();
                        upResponsavel.Update();
                        mpeResponsavel.Show();
                        return false;

                    }
                    else if (confirmacao)
                    {
                        descricao = "Exclusão de Usuário responsável";
                        UsuarioFilialPerfil.AtualizaNovoResponsavel(Convert.ToInt32(hfIdResponsavel.Value), IdUsuarioFilialPerfil.Value);
                        hfIdResponsavel.Value = string.Empty;
                        mpeResponsavel.Hide();
                    }
                    else
                    {
                        objUsuarioFilialPerfil.FlagInativo = true;
                        objUsuarioFilialPerfil.Save();
                    }

                    descricao += objUsuarioFilialPerfilOld != null ? MensagemAssincronoLogCRM.StringListaDeItensModificados(MensagemAssincronoLogCRM.DiffProperties(objUsuarioFilialPerfilOld, objUsuarioFilialPerfil, objUsuarioFilialPerfilOld.GetType())) : string.Empty;

                    if (!string.IsNullOrEmpty(descricao))
                        MensagemAssincronoLogCRM.SalvarModificacoesFilialCRM(descricao, objUsuarioFilialPerfil.Filial, new UsuarioFilialPerfil(IdUsuarioFilialPerfil.Value), null);

                    Task.Factory.StartNew(() => CelularSelecionador.HabilitarDesabilitarUsuarios(objUsuarioFilialPerfil.Filial));
                }
                else if (IdUsuarioFilialContato.HasValue)
                {
                    new UsuarioFilialContato(IdUsuarioFilialContato.Value).Inativar();
                }

                return true;
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
            return false;
        }
        #endregion

        #region Ativar
        private bool Ativar()
        {
            try
            {

                if (IdUsuarioFilialPerfil.HasValue)
                {
                    string descricao = "Ativar Usuário! ";

                    var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(IdUsuarioFilialPerfil.Value);
                    var objUsuarioFilialPerfilOld = objUsuarioFilialPerfil.Clone() as UsuarioFilialPerfil;

                    objUsuarioFilialPerfil.FlagInativo = false;
                    objUsuarioFilialPerfil.Save();

                    descricao += objUsuarioFilialPerfilOld != null ? MensagemAssincronoLogCRM.StringListaDeItensModificados(MensagemAssincronoLogCRM.DiffProperties(objUsuarioFilialPerfilOld, objUsuarioFilialPerfil, objUsuarioFilialPerfilOld.GetType())) : string.Empty;

                    if (!string.IsNullOrEmpty(descricao))
                        MensagemAssincronoLogCRM.SalvarModificacoesFilialCRM(descricao, objUsuarioFilialPerfil.Filial, new UsuarioFilialPerfil(IdUsuarioFilialPerfil.Value), null);

                    Task.Factory.StartNew(() => CelularSelecionador.HabilitarDesabilitarUsuarios(objUsuarioFilialPerfil.Filial));
                }
                else if (IdUsuarioFilialContato.HasValue)
                {
                    new UsuarioFilialContato(IdUsuarioFilialContato.Value).Ativar();
                }

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
        ///     Limpa os valores de cada campo na tela.
        /// </summary>
        private void LimparCampos(bool limparCampoCpf)
        {
            IdPessoaFisica = null;
            IdUsuarioFilialPerfil = null;
            IdUsuarioFilialContato = null;

            if (limparCampoCpf)
            {
                txtCPF.Valor = string.Empty;
                lbltxtCPF.Visible = !(txtDataNascimento.Visible = txtCPF.Visible = limparCampoCpf);
                lbltxtDataNascimento.Visible = !(txtDataNascimento.Visible = limparCampoCpf);

            }
                

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
            ckbResponsavel.Checked = false;

            //Reabilitando CPF e DtaNasc
            txtCPF.Enabled = true;
            txtDataNascimento.Enabled = true;
            ckbMaster.Checked = false;

            upDadosBasicos.Update();
        }
        #endregion

        #region VerificaNumeroCpf
        /// <summary>
        ///     Verifica se o número do CPF que foi informado na tela já existe para aquela empresa,ou se a pessoa fisica não
        /// </summary>
        /// <returns>Existe - true| Não Existe - False</returns>
        public bool VerificaNumeroCpf()
        {
            if (string.IsNullOrWhiteSpace(txtCPF.Valor))
                return false;

            PessoaFisica objPessoaFisica;
            if (PessoaFisica.CarregarPorCPF(Convert.ToDecimal(txtCPF.Valor), out objPessoaFisica))
            {
                UsuarioFilialPerfil objUsuarioFilialPerfil;
                if (UsuarioFilialPerfil.CarregarPorPessoaFisicaFilial(objPessoaFisica.IdPessoaFisica, base.IdFilial.HasValue ? base.IdFilial.Value : IdFilial.Value, out objUsuarioFilialPerfil))
                    IdUsuarioFilialPerfil = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;

                HabilitarCamposCpfExistenteBaseDados(false);
                AjustarObrigatoriedadeNosCamposUsuario(true);

                IdPessoaFisica = objPessoaFisica.IdPessoaFisica;
                return true;
            }
            return true;
        }
        #endregion

        #region HabilitarCamposCpfExistenteEmpresa
        /// <summary>
        ///     Caso o CPF se encontre cadastrado como usuario na empresa os campos serão desabilitados
        /// </summary>
        /// <param name="enable"></param>
        public void HabilitarCamposCpfExistenteEmpresa(bool enable)
        {
            //Reset obrigatoriedade do campo celular
            ObrigatoriedadeCelular(true);

            txtDataNascimento.Enabled = enable;
            txtNome.Enabled = enable;
            rblSexo.Enabled = enable;
            txtTelefone.Enabled = enable;
            txtRamal.Enabled = enable;
            txtTelefoneCelular.Enabled = enable;
            txtEmail.Enabled = enable;
            ckbMaster.Enabled = enable;
            ckbResponsavel.Enabled = enable;
            txtFuncaoExercida.Enabled = enable;

            if (!IdPerfilUsuarioLogado.Equals(Perfil.AcessoEmpresaMaster.GetHashCode()) && !IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue) //Se não for um usuário interno)
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
        ///     Caso o CPF se encontre cadastrado na base de dados, não poderão ser alterados
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
            ckbResponsavel.Enabled = enable;
        }
        #endregion

        #region AjustarObrigatoriedadeNosCamposUsuario
        public void AjustarObrigatoriedadeNosCamposUsuario(bool obrigatorio)
        {
            txtCPF.Obrigatorio = obrigatorio;
            txtDataNascimento.Obrigatorio = obrigatorio;
            txtNome.Obrigatorio = obrigatorio;
            rfvSexo.Enabled = obrigatorio;
            rfvFuncaoExercida.Enabled = obrigatorio;
            txtTelefone.Obrigatorio = obrigatorio;
            txtTelefoneCelular.Obrigatorio = obrigatorio;
            rfvEmail.Enabled = obrigatorio;
        }
        #endregion

        #region AjustarObrigatoriedadeNosCamposContato
        public void AjustarObrigatoriedadeNosCamposContato(bool obrigatorio)
        {
            txtNome.Obrigatorio = obrigatorio;
            rfvEmail.Enabled = obrigatorio;

            txtCPF.Obrigatorio = false;
            txtDataNascimento.Obrigatorio = false;
            rfvSexo.Enabled = false;
            rfvFuncaoExercida.Enabled = false;
            txtTelefone.Obrigatorio = false;
            txtTelefoneCelular.Obrigatorio = false;
        }
        #endregion

        #region TodosOsCamposEstaoPreenchidos
        public bool TodosOsCamposEstaoPreenchidos()
        {
            if (string.IsNullOrWhiteSpace(txtCPF.Valor))
                return false;

            if (string.IsNullOrWhiteSpace(txtDataNascimento.Valor))
                return false;

            if (string.IsNullOrWhiteSpace(txtNome.Valor))
                return false;

            if (rblSexo.SelectedValue == "0")
                return false;

            if (string.IsNullOrWhiteSpace(txtFuncaoExercida.Text))
                return false;

            if (string.IsNullOrWhiteSpace(txtTelefoneCelular.DDD))
                return false;

            if (string.IsNullOrWhiteSpace(txtTelefoneCelular.Fone))
                return false;

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
                return false;

            if (string.IsNullOrWhiteSpace(txtTelefone.DDD))
                return false;

            if (string.IsNullOrWhiteSpace(txtTelefone.Fone))
                return false;

            return true;
        }
        #endregion

        #region AjustarPermissoes
        /// <summary>
        ///     Método responsável por ajustar as permissões da tela de acordo com o susuário filial perfil logado.
        /// </summary>
        private void AjustarPermissoes()
        {
            if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(IdUsuarioFilialPerfilLogadoEmpresa.Value, CategoriaPermissao.TelaCadastroEmpresaUsuario);

                if (!Permissoes.Contains((int)BLL.Enumeradores.Permissoes.TelaCadastroEmpresaUsuario.AcessarTelaCadastroUsuários))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else if (IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, CategoriaPermissao.TelaCadastroEmpresaUsuario);

                if (!Permissoes.Contains((int)BLL.Enumeradores.Permissoes.TelaCadastroEmpresaUsuario.AcessarTelaCadastroUsuários))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else
                Redirect(GetRouteUrl(RouteCollection.LoginComercialEmpresa.ToString(), null));
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
        ///     Metodo resposável por Ajustas os campos para a atualização
        /// </summary>
        /// <param name="enable"></param>
        private void AjustarCamposEdicao(bool enable)
        {
            txtCPF.Enabled = enable;
            txtDataNascimento.Enabled = enable;
            if (enable.Equals(false)){
                lbltxtCPF.Text = UIHelper.FormatarCPF(txtCPF.Valor);
                txtCPF.Visible = !(lbltxtCPF.Visible = true); 
                lbltxtDataNascimento.Text = txtDataNascimento.Valor;
                if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                    lbltxtDataNascimento.Visible = !(txtDataNascimento.Enabled = txtDataNascimento.Visible = !enable);
                else
                    lbltxtDataNascimento.Visible = !(txtDataNascimento.Enabled = txtDataNascimento.Visible = enable);
            }
            
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
        ///     Método executado toda a vez que a tela for carregada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            revEmail.ValidationExpression = Configuracao.regexEmail;

            InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "CadastroEmpresaUsuario");
        }
        #endregion

        #region txtCPF_ValorAlterado
        /// <summary>
        ///     Responsável por executar os procedimentos necessários para validar e carregar os dados da tela conforme
        ///     o cpf e data de nascimento informados.
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

        #region btnSalvar
        /// <summary>
        ///     Executa os procedimentos necessários para salvar as informações da tela.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                string erro;
                var retorno = Salvar(out erro);

                if (retorno)
                {
                    LimparCampos(true);
                    CarregarGridUsuarios();
                    CarregarGridContatos();
                    HabilitarTodosOsCampos(true);

                    AjustarObrigatoriedadeNosCamposUsuario(false);
                    AjustarObrigatoriedadeNosCamposContato(true);

                    upDadosBasicos.Update();

                    if (!IdPerfilUsuarioLogado.Equals((int)Perfil.AcessoEmpresaMaster) &&
                        !IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                    {
                        btnSalvar.Enabled = false;
                        AjustarUsuarioNaoMasterModoEdicao(true);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(erro))
                    ExibirMensagem(erro, TipoMensagem.Aviso);
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
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void gvUsuarios_ItemCommand(object source, GridCommandEventArgs e)
        {
            HabilitarCamposCpfExistenteBaseDados(true);
            HabilitarCamposCpfExistenteEmpresa(true);

            if (e.CommandName.Equals("Atualizar"))
            {
                var datakeyArray = gvUsuarios.MasterTableView.DataKeyValues[e.Item.ItemIndex];
                if (datakeyArray.ContainsKey("Idf_Usuario_Filial_Perfil"))
                {
                    IdUsuarioFilialPerfil = Convert.ToInt32(gvUsuarios.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Usuario_Filial_Perfil"]);
                    IdPessoaFisica = Convert.ToInt32(gvUsuarios.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Pessoa_Fisica"]);

                    if (!IdPerfilUsuarioLogado.Equals(Perfil.AcessoEmpresaMaster.GetHashCode()) && !IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                        ckbMaster.Enabled = ckbResponsavel.Enabled = false;
                    else
                        ckbMaster.Enabled = ckbResponsavel.Enabled = true;
                }

                btnSalvar.Enabled = true;
                PreencherCampos();
                //Se for edição de um Usuário que já possua uma PF, todos os campos são obrigatórios
                AjustarObrigatoriedadeNosCamposUsuario(true);
                btnCancelar.Visible = true;

                AjustarCamposEdicao(false);

                upDadosBasicos.Update();
                upBotoesCadastro.Update();
            }
            else if (e.CommandName.Equals("Deletar"))
            {
                var idUsuarioFilialPerfil = Convert.ToInt32(gvUsuarios.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Usuario_Filial_Perfil"]);
                IdUsuarioFilialPerfil = idUsuarioFilialPerfil;
                mpeConfirmacaoExclusao.Show();
            }
            else if (e.CommandName.Equals("Ativar"))
            {
                IdUsuarioFilialPerfil = Convert.ToInt32(gvUsuarios.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Usuario_Filial_Perfil"]);
                if (Ativar())
                {
                    CarregarGridUsuarios();
                    CarregarGridContatos();
                    LimparCampos(true);
                    btnSalvar.Enabled = true;
                    upDadosBasicos.Update();
                    upGvUsuarios.Update();
                }


            }
        }
        #endregion

        #region gvUsuarios_PageIndexChanged
        protected void gvUsuarios_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            gvUsuarios.CurrentPageIndex = e.NewPageIndex;

            if (IdUsuarioFilialPerfil.HasValue)
                AjustarCamposEdicao(false);
            else
                AjustarCamposEdicao(true);

            CarregarGridUsuarios();
            upGvUsuarios.Update();
        }
        #endregion

        #region gvUsuarios_OnItemDataBound
        protected void gvUsuarios_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                var lnkButton = (LinkButton)e.Item.FindControl("btiAtivar");
                lnkButton.Visible = (
                    IdPerfilUsuarioLogado.Equals((int)Perfil.AdministradorSistema) ||
                    IdPerfilUsuarioLogado.Equals((int)Perfil.Financeiro) ||
                    IdPerfilUsuarioLogado.Equals((int)Perfil.Vendedor)
                    );

                var imageButton = (LinkButton)e.Item.FindControl("btiExcluir");
                imageButton.Visible = (IdPerfilUsuarioLogado.Equals(
                    (int)Perfil.AcessoEmpresaMaster) ||
                    IdPerfilUsuarioLogado.Equals((int)Perfil.AdministradorSistema) ||
                    IdPerfilUsuarioLogado.Equals((int)Perfil.Financeiro) ||
                    IdPerfilUsuarioLogado.Equals((int)Perfil.Vendedor)
                );
            }
        }
        #endregion

        #endregion

        #region gvContatos

        #region gvContatos_ItemCommand
        /// <summary>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void gvContatos_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("Atualizar"))
            {
                LimparCampos(true);

                var datakeyArray = gvContatos.MasterTableView.DataKeyValues[e.Item.ItemIndex];
                if (datakeyArray.ContainsKey("Idf_Usuario_Filial_Contato"))
                {
                    IdUsuarioFilialContato = Convert.ToInt32(gvContatos.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Usuario_Filial_Contato"]);

                    if (!IdPerfilUsuarioLogado.Equals(Perfil.AcessoEmpresaMaster.GetHashCode()) && !IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                        ckbMaster.Enabled = ckbResponsavel.Enabled = false;
                    else
                        ckbMaster.Enabled = ckbResponsavel.Enabled = true;
                }

                btnSalvar.Enabled = true;
                PreencherCampos();
                //Se for uma edição de contato, não são todos os campos que são obrigatórios
                AjustarObrigatoriedadeNosCamposUsuario(false);
                AjustarObrigatoriedadeNosCamposContato(true);

                btnCancelar.Visible = true;

                upDadosBasicos.Update();
                upBotoesCadastro.Update();
            }
            else if (e.CommandName.Equals("Deletar"))
            {
                IdUsuarioFilialContato = Convert.ToInt32(gvContatos.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Usuario_Filial_Contato"]);
                mpeConfirmacaoExclusao.Show();
            }
        }
        #endregion

        #region gvContatos_PageIndexChanged
        protected void gvContatos_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            gvContatos.CurrentPageIndex = e.NewPageIndex;

            if (IdUsuarioFilialContato.HasValue)
                AjustarCamposEdicao(false);
            else
                AjustarCamposEdicao(true);

            CarregarGridContatos();
            upGvContatos.Update();
        }
        #endregion

        #endregion

        #region btnCancelar_Click
        /// <summary>
        ///     Somente no Modo de edição
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimparCampos(true);
            AjustarCamposEdicao(true);
            AjustarUsuarioNaoMasterModoEdicao(true);

            //Task: 41751
            if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
            {
                AjustarObrigatoriedadeNosCamposUsuario(false);
                AjustarObrigatoriedadeNosCamposContato(true);
            }
            else
                AjustarObrigatoriedadeNosCamposUsuario(true);

            ckbMaster.Enabled = true;
            ckbResponsavel.Enabled = true;
            btnCancelar.Visible = false;

            if (!IdPerfilUsuarioLogado.Equals((int)Perfil.AcessoEmpresaMaster) && !IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                btnSalvar.Enabled = false;
            else
                btnSalvar.Enabled = true;

            upDadosBasicos.Update();
            upBotoesCadastro.Update();
        }
        #endregion

        #region Modal

        #region btnExcluirModalExclusao_Click
        /// <summary>
        ///     Executa os procedimentos de exclusão do usuário selecionado e fecha a modal de confirmação de exclusão.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExcluirModalExclusao_Click(object sender, EventArgs e)
        {
            if (Excluir(false))
            {
                CarregarGridUsuarios();
                CarregarGridContatos();
                LimparCampos(true);
                btnSalvar.Enabled = true;
                upDadosBasicos.Update();
                upGvUsuarios.Update();
            }

            mpeConfirmacaoExclusao.Hide();
        }
        #endregion

        #region btnCancelarModalExclusao
        /// <summary>
        ///     Cancela a excluão do usuário e fecha a modal.
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

        #region rcbStatus_SelectedIndexChanged
        protected void rcbStatus_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            CarregarGridUsuarios();
            CarregarGridContatos();
        }
        #endregion

        #endregion

        protected void btnInativar_Click(object sender, EventArgs e)
        {
            if(hfIdResponsavel.Value.Equals(string.Empty))
            {
                ExibirMensagem(MensagemAviso._505708, TipoMensagem.Aviso);
                return;
            }
            if (Excluir(true))
            {
                CarregarGridUsuarios();
                CarregarGridContatos();
                LimparCampos(true);
                btnSalvar.Enabled = true;
                upDadosBasicos.Update();
                upGvUsuarios.Update();
            }
        }

       
    }
}