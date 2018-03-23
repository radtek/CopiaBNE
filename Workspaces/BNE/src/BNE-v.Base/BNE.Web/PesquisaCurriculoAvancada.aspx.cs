using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Common.Session;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Master;
using BNE.Web.Resources;
using BNE.Web.UserControls;
using Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class PesquisaCurriculoAvancada : BasePage
    {
        protected SessionVariable<PesquisaPadrao> PesquisCurriculoPadrao = new SessionVariable<PesquisaPadrao>(Chave.Temporaria.PesquisaPadrao.ToString());

        #region Propriedades

        #region IdPesquisaCurriculoAvancado - Variável 10
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaCurriculo
        /// </summary>
        protected int? IdPesquisaCurriculoAvancado
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel10.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel10.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel10.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel10.ToString());
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

        #endregion

        #region Métodos

        #region Buscar
        private BLL.PesquisaCurriculo Buscar()
        {
            Page.Validate();

            if (!Page.IsValid)
                return null;

            //Identifica os itens selecionado na dropdown de idioma.
            var listPesquisaCurriculoIdioma = new List<PesquisaCurriculoIdioma>();
            foreach (RadComboBoxItem item in rcbIdioma.GetCheckedItems())
            {
                var objPesquisaCurriculoIdioma = new PesquisaCurriculoIdioma
                    {
                        Idioma = new Idioma(Convert.ToInt32(item.Value))
                    };
                listPesquisaCurriculoIdioma.Add(objPesquisaCurriculoIdioma);
            }

            //Identifica os itens selecionado na dropdown de disponibilidade.
            var listPesquisaCurriculoDisponibilidade = new List<PesquisaCurriculoDisponibilidade>();
            foreach (RadComboBoxItem item in rcbDisponibilidade.GetCheckedItems())
            {
                var objPesquisaCurriculoDisponibilidade = new PesquisaCurriculoDisponibilidade
                    {
                        Disponibilidade = new Disponibilidade(Convert.ToInt32(item.Value))
                    };
                listPesquisaCurriculoDisponibilidade.Add(objPesquisaCurriculoDisponibilidade);
            }

            //[Obsoleto("Função inativada.")]
            //Identifica os itens selecionados no checkbox de tipovinculo.
            //var listPesquisaCurriculoTipoVinculo = new List<PesquisaCurriculoTipoVinculo>();
            //foreach (var item in ucContratoFuncao.TipoContratoItens.Where(obj => obj.Selected))
            //{
            //    var tipoVinculo = new PesquisaCurriculoTipoVinculo();
            //    tipoVinculo.TipoVinculo = new TipoVinculo(Convert.ToInt32(item.Value));
            //    listPesquisaCurriculoTipoVinculo.Add(tipoVinculo);
            //}

            var objPesquisaCurriculo = new BLL.PesquisaCurriculo();

            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                objPesquisaCurriculo.UsuarioFilialPerfil = new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);

            if (base.IdCurriculo.HasValue)
                objPesquisaCurriculo.Curriculo = new Curriculo(base.IdCurriculo.Value);

            if (!String.IsNullOrEmpty(ucEstagiarioFuncao.FuncaoDesc))
            {
                var funcao = Funcao.CarregarPorDescricao(ucEstagiarioFuncao.FuncaoDesc);
                if (funcao != null)
                    objPesquisaCurriculo.Funcao = new Funcao(funcao.IdFuncao) { DescricaoFuncao = funcao.DescricaoFuncao };
            }

            Cidade objCidade;
            if (Cidade.CarregarPorNome(txtCidadePesquisa.Text, out objCidade))
            {
                objPesquisaCurriculo.Cidade = new Cidade(objCidade.IdCidade) { NomeCidade = objCidade.NomeCidade };
            }

            objPesquisaCurriculo.DescricaoPalavraChave = txtPalavraChave.Valor;
            objPesquisaCurriculo.DescricaoFiltroExcludente = txtExcluirPalavraChave.Valor;

            if (!rcbEstado.SelectedValue.Equals("0"))
                objPesquisaCurriculo.Estado = new Estado(rcbEstado.SelectedValue);

            if (!rcbNivel.SelectedValue.Equals("0"))
            {
                var escolaridade = Escolaridade.LoadObject(Convert.ToInt32(rcbNivel.SelectedValue));
                if (escolaridade != null)
                    objPesquisaCurriculo.Escolaridade = new Escolaridade(escolaridade.IdEscolaridade) { DescricaoBNE = escolaridade.DescricaoBNE };
            }

            if (!rcbSexo.SelectedValue.Equals("0"))
            {
                var sexo = Sexo.LoadObject(Convert.ToInt32(rcbSexo.SelectedValue));
                if (sexo != null)
                    objPesquisaCurriculo.Sexo = new Sexo(sexo.IdSexo)
                        {
                            SiglaSexo =
                                default(char) == sexo.SiglaSexo ? sexo.DescricaoSexo.FirstOrDefault() : sexo.SiglaSexo
                        };
            }

            if (!String.IsNullOrEmpty(txtIdadeDe.Valor))
                objPesquisaCurriculo.DataIdadeMin = DateTime.Today.AddYears(-(Convert.ToInt32(txtIdadeDe.Valor)));

            if (!String.IsNullOrEmpty(txtIdadeAte.Valor))
                objPesquisaCurriculo.DataIdadeMax = DateTime.Today.AddYears(-(Convert.ToInt32(txtIdadeAte.Valor)));

            objPesquisaCurriculo.NumeroSalarioMin = txtSalario.Valor;
            objPesquisaCurriculo.NumeroSalarioMax = txtSalarioAte.Valor;

            if (!String.IsNullOrEmpty(txtTempoExperiencia.Valor))
                objPesquisaCurriculo.QuantidadeExperiencia = Convert.ToInt64(txtTempoExperiencia.Valor);

            if (txtNomeCpfCodigo.Valor.Contains(";"))
                objPesquisaCurriculo.DescricaoCodCPFNome = txtNomeCpfCodigo.Valor.Replace(";", ",");
            else
                objPesquisaCurriculo.DescricaoCodCPFNome = txtNomeCpfCodigo.Valor;

            if (!rcbEstadoCivil.SelectedValue.Equals("0"))
                objPesquisaCurriculo.EstadoCivil = EstadoCivil.LoadObject(Convert.ToInt32(rcbEstadoCivil.SelectedValue));

            objPesquisaCurriculo.DescricaoBairro = txtBairro.Valor;
            objPesquisaCurriculo.DescricaoLogradouro = txtLogradouro.Valor;
            objPesquisaCurriculo.NumeroCEPMin = txtFaixaCep.Valor;
            objPesquisaCurriculo.NumeroCEPMax = txtFaixaCepAte.Valor;

            //Fonte e curso
            if (!String.IsNullOrEmpty(txtInstituicaoTecnicoGraduacao.Text))
            {
                Fonte objFonte;
                if (Fonte.CarregarPorSiglaNome(txtInstituicaoTecnicoGraduacao.Text, out objFonte))
                    objPesquisaCurriculo.FonteTecnicoGraduacao = objFonte;
            }

            if (!String.IsNullOrEmpty(txtTituloTecnicoGraduacao.Text))
            {
                Curso objCurso;
                if (Curso.CarregarPorNome(txtTituloTecnicoGraduacao.Text, out objCurso))
                    objPesquisaCurriculo.CursoTecnicoGraduacao = objCurso;
                else
                {
                    objPesquisaCurriculo.DescricaoCursoTecnicoGraduacao = txtTituloTecnicoGraduacao.Text;
                }
            }

            if (!String.IsNullOrEmpty(txtInstituicao.Text))
            {
                Fonte objFonte;
                if (Fonte.CarregarPorSiglaNome(txtInstituicao.Text, out objFonte))
                    objPesquisaCurriculo.FonteOutrosCursos = objFonte;
            }

            if (!String.IsNullOrEmpty(txtTituloCurso.Text))
            {
                Curso objCurso;
                if (Curso.CarregarPorNome(txtTituloCurso.Text, out objCurso))
                    objPesquisaCurriculo.CursoOutrosCursos = objCurso;
                else
                {
                    objPesquisaCurriculo.DescricaoCursoOutrosCursos = txtTituloCurso.Text;
                }
            }

            objPesquisaCurriculo.RazaoSocial = txtEmpresa.Valor;

            if (!rcbAtividadeEmpresa.SelectedValue.Equals("0"))
                objPesquisaCurriculo.AreaBNE = AreaBNE.LoadObject(Convert.ToInt32(rcbAtividadeEmpresa.SelectedValue));

            if (!rcbHabilitacao.SelectedValue.Equals("0"))
                objPesquisaCurriculo.CategoriaHabilitacao = CategoriaHabilitacao.LoadObject(Convert.ToInt32(rcbHabilitacao.SelectedValue));

            if (!rcbVeiculo.SelectedValue.Equals("0"))
                objPesquisaCurriculo.TipoVeiculo = new TipoVeiculo(Convert.ToInt32(rcbVeiculo.SelectedValue));

            if (!rcbPCD.SelectedValue.Equals("-1"))
                objPesquisaCurriculo.Deficiencia = new Deficiencia(Convert.ToInt32(rcbPCD.SelectedValue));

            objPesquisaCurriculo.NumeroDDDTelefone = txtTelefone.DDD;
            objPesquisaCurriculo.NumeroTelefone = txtTelefone.Fone;
            objPesquisaCurriculo.EmailPessoa = txtEmail.Text;

            if (!rcbRaca.SelectedValue.Equals("0"))
                objPesquisaCurriculo.Raca = new Raca(Convert.ToInt32(rcbRaca.SelectedValue));

            if (!rcbFilhos.SelectedValue.Equals("-1"))
                objPesquisaCurriculo.FlagFilhos = rcbFilhos.SelectedValue.Equals("1");

            objPesquisaCurriculo.FlagPesquisaAvancada = true;

            objPesquisaCurriculo.Salvar(listPesquisaCurriculoIdioma, listPesquisaCurriculoDisponibilidade, ucEstagiarioFuncao.CheckBoxEstagiarioSelecionado, ucEstagiarioFuncao.CheckBoxAprendizSelecionado);

            NoficacaoDePesquisa(objPesquisaCurriculo, listPesquisaCurriculoIdioma, listPesquisaCurriculoDisponibilidade, ucEstagiarioFuncao.CheckBoxEstagiarioSelecionado);

            Session.Add(Chave.Temporaria.Variavel6.ToString(), objPesquisaCurriculo.IdPesquisaCurriculo);

            return objPesquisaCurriculo;
        }

        private void NoficacaoDePesquisa(BLL.PesquisaCurriculo objPesquisaCurriculo,
                                         List<PesquisaCurriculoIdioma> listPesquisaCurriculoIdioma,
                                         List<PesquisaCurriculoDisponibilidade> listPesquisaCurriculoDisponibilidade,
                                         bool checkBoxEstagiarioSelecionado)
        {
            if (!checkBoxEstagiarioSelecionado)
                return;

            var filialId = IdFilial.ValueOrDefault;
            if (filialId <= 0)
                return;

            var noficacao = new NotificarPesquisaDeEstagiario(filialId, objPesquisaCurriculo,
                                                              listPesquisaCurriculoIdioma,
                                                              listPesquisaCurriculoDisponibilidade);

            noficacao.EnviarSeAplicavel();
        }
        #endregion

        #region Inicializar
        private void Inicializar()
        {
            AjustarPermissoes();

            ucEstagiarioFuncao.CarregarParametros();
            ucEstagiarioFuncao.AtualizarValidationGroup(this.btnBuscar.ValidationGroup);
            upEstagiarioFuncaoArea.Update();

            //Carrando as drop-down's
            UIHelper.CarregarRadComboBox(rcbNivel, Escolaridade.Listar(), new RadComboBoxItem("Qualquer", "0"));
            UIHelper.CarregarRadComboBox(rcbSexo, Sexo.Listar(), new RadComboBoxItem("Qualquer", "0"));
            UIHelper.CarregarRadComboBox(rcbIdioma, Idioma.Listar());
            UIHelper.CarregarRadComboBox(rcbEstado, Estado.Listar(), new RadComboBoxItem("Qualquer", "0"));
            UIHelper.CarregarRadComboBox(rcbEstadoCivil, EstadoCivil.Listar(), new RadComboBoxItem("Qualquer", "0"));
            UIHelper.CarregarRadComboBox(rcbAtividadeEmpresa, AreaBNE.Listar(), new RadComboBoxItem("Qualquer", "0"));
            UIHelper.CarregarRadComboBox(rcbVeiculo, TipoVeiculo.Listar(), "Idf_Tipo_Veiculo", "Des_tipo_Veiculo", new RadComboBoxItem("Selecione", "0"));
            UIHelper.CarregarRadComboBox(rcbDisponibilidade, Disponibilidade.Listar());
            UIHelper.CarregarRadComboBox(rcbHabilitacao, CategoriaHabilitacao.Listar(), "Idf_Categoria_Habilitacao", "Des_Categoria_Habilitacao", new RadComboBoxItem("Selecione", "0"));
            UIHelper.CarregarRadComboBox(rcbPCD, Deficiencia.Listar(), new RadComboBoxItem("Indiferente", "-1"));
            UIHelper.CarregarRadComboBox(rcbRaca, Raca.Listar(), "Idf_Raca", "Des_Raca_BNE", new RadComboBoxItem("Qualquer", "0"));
            UIHelper.CarregarRadComboBox(rcbFilhos, Flag.Listar(), "Idf_Flag", "Des_Flag", new RadComboBoxItem("Indiferente", "-1"));

            txtCidadePesquisa.Attributes["onChange"] += "txtCidade_onChange(this);";
            txtCidadePesquisa.Attributes["onBlur"] += "PesquisaCurriculoAvancadaCidadeOnBlur(this)";

            txtSalarioAte.MensagemErroIntervalo = String.Format(MensagemAviso._304501, "Máximo", "maior", "Mínimo");
            cvFuncaoFaixaCepAte.ErrorMessage = String.Format(MensagemAviso._304501, "Máximo", "maior", "Mínimo");

            CarregarParametros();

            InicializarBarraBusca(TipoBuscaMaster.Curriculo, true, "PesquisaCurriculoAvancada");

            LimparCampos();

            if (IdPesquisaCurriculoAvancado.HasValue)
                PreencherCampos();
            else
                VerificaRedirecionamento();
        }

        private bool VerificaRedirecionamento()
        {
            if (!PesquisCurriculoPadrao.HasValue || PesquisCurriculoPadrao.Value == null)
                return false;

            try
            {
                var funcao = Helper.RemoverAcentos((PesquisCurriculoPadrao.Value.Funcao ?? string.Empty).Trim());

                if (funcao.Equals("Estagio", StringComparison.OrdinalIgnoreCase)
                    || funcao.IndexOf("Estagiari", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    ucEstagiarioFuncao.SetEstagiario(true);
                    ucEstagiarioFuncao.Focus();
                    ucEstagiarioFuncao.SetFocus(FuncaoEmPesquisaCurriculo.TipoFoco.Funcao);
                }

                if (funcao.Equals("Aprendiz", StringComparison.OrdinalIgnoreCase))
                {
                    ucEstagiarioFuncao.SetAprendiz(true);
                    ucEstagiarioFuncao.Focus();
                    ucEstagiarioFuncao.SetFocus(FuncaoEmPesquisaCurriculo.TipoFoco.Funcao);
                }

                var pesquisaCurriculo = PesquisCurriculoPadrao.Value as PesquisaCurriculoPadrao;
                if (pesquisaCurriculo != null)
                {
                    this.txtPalavraChave.Valor = pesquisaCurriculo.PalavraChave;
                }

                return true;
            }
            finally
            {
                txtCidadePesquisa.Text = PesquisCurriculoPadrao.Value.Cidade;
                txtCidadePesquisa_TextChanged(txtCidadePesquisa, EventArgs.Empty);
                PesquisCurriculoPadrao.Clear();
            }
        }
        #endregion

        #region CarregarParametros
        private void CarregarParametros()
        {
            try
            {
                var parametros = new List<Enumeradores.Parametro>
                    {
                        Enumeradores.Parametro.IntervaloTempoAutoComplete, 
                        Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteFuncao,
                        Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao, 
                        Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteCidade, 
                        Enumeradores.Parametro.NumeroResultadosAutoCompleteCidade, 
                        Enumeradores.Parametro.IdadeMinima, 
                        Enumeradores.Parametro.IdadeMaxima,
                        Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteEstado, 
                        Enumeradores.Parametro.NumeroResultadosAutoCompleteEstado,
                        Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteNomeCurso,
                        Enumeradores.Parametro.NumeroResultadosAutoCompleteNomeCurso,
                        Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteNomeInstituicao,
                        Enumeradores.Parametro.NumeroResultadosAutoCompleteNomeInstituicao
                    };

                Dictionary<Enumeradores.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

                aceCidade.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceCidade.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteCidade]);
                aceCidade.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteCidade]);

                txtIdadeDe.ValorMinimo = txtIdadeAte.ValorMinimo = valoresParametros[Enumeradores.Parametro.IdadeMinima];
                txtIdadeDe.ValorMaximo = txtIdadeAte.ValorMaximo = valoresParametros[Enumeradores.Parametro.IdadeMaxima];
                txtIdadeDe.MensagemErroIntervalo = txtIdadeAte.MensagemErroIntervalo = String.Format(MensagemAviso._304502, valoresParametros[Enumeradores.Parametro.IdadeMinima], valoresParametros[Enumeradores.Parametro.IdadeMaxima]);

                aceInstituicaoTecnicoGraduacao.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceInstituicaoTecnicoGraduacao.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteNomeInstituicao]);
                aceInstituicaoTecnicoGraduacao.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteNomeInstituicao]);

                aceTituloTecnicoGraduacao.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceTituloTecnicoGraduacao.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteNomeCurso]);
                aceTituloTecnicoGraduacao.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteNomeCurso]);

                aceInstituicaoOutrosCursos.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceInstituicaoOutrosCursos.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteNomeInstituicao]);
                aceInstituicaoOutrosCursos.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteNomeInstituicao]);

                aceInstituicaoOutrosCursos.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceInstituicaoOutrosCursos.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteNomeCurso]);
                aceInstituicaoOutrosCursos.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteNomeCurso]);
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region AjustarPermissoes
        /// <summary>
        /// Método responsável por ajustar as permissões da tela de acordo com o susuário filial perfil logado.
        /// </summary>
        private void AjustarPermissoes()
        {

            int? idUsuarioFilialPerfil = null;

            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                idUsuarioFilialPerfil = base.IdUsuarioFilialPerfilLogadoEmpresa.Value;
            else if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                idUsuarioFilialPerfil = base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value;

            if (idUsuarioFilialPerfil.HasValue)
            {

                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(idUsuarioFilialPerfil.Value, Enumeradores.CategoriaPermissao.PesquisaAvancadaCurriculo);

                if (!Permissoes.Contains((int)Enumeradores.Permissoes.PesquisaAvancadaCurriculo.AcessarTelaPesquisaAvancadaCurriculo))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }

            }
            else
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialEmpresa.ToString(), new { Destino = Enumeradores.LoginEmpresaDestino.PesquisaCurriculo }));
        }
        #endregion

        #region LimparCampos
        private void LimparCampos()
        {
            ucEstagiarioFuncao.LimparCampos();

            txtCidadePesquisa.Text =
            txtPalavraChave.Valor =
            txtIdadeDe.Valor =
            txtIdadeAte.Valor =
            txtTempoExperiencia.Valor =
            txtNomeCpfCodigo.Valor =
            txtBairro.Valor =
            txtLogradouro.Valor =
                txtFaixaCep.Valor =
                txtFaixaCepAte.Valor =
            txtTituloCurso.Text =
            txtInstituicao.Text =
            txtTituloTecnicoGraduacao.Text =
            txtInstituicaoTecnicoGraduacao.Text =
            txtEmpresa.Valor =
            txtTelefone.DDD =
            txtTelefone.Fone =
            txtExcluirPalavraChave.Valor =
            txtEmail.Text = String.Empty;

            txtSalarioAte.Valor =
            txtSalario.Valor = null;

            rcbIdioma.ClearCheckeds();
            rcbDisponibilidade.ClearCheckeds();

            rcbIdioma.Text = rcbIdioma.EmptyMessage;
            rcbDisponibilidade.Text = rcbDisponibilidade.EmptyMessage;

            rcbNivel.SelectedValue =
            rcbSexo.SelectedValue =
            rcbEstado.SelectedValue =
            rcbEstadoCivil.SelectedValue =
            rcbAtividadeEmpresa.SelectedValue =
            rcbHabilitacao.SelectedValue =
            rcbVeiculo.SelectedValue =
            rcbRaca.SelectedValue = "0";

            rcbPCD.SelectedValue =
            rcbFilhos.SelectedValue = "-1";
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos()
        {
            BLL.PesquisaCurriculo objPesquisaCurriculo = BLL.PesquisaCurriculo.LoadObject(IdPesquisaCurriculoAvancado.Value);

            if (objPesquisaCurriculo.Funcao != null)
            {
                objPesquisaCurriculo.Funcao.CompleteObject();
                ucEstagiarioFuncao.SetFuncao(objPesquisaCurriculo.Funcao.DescricaoFuncao);
            }

            if (objPesquisaCurriculo.Cidade != null)
            {
                objPesquisaCurriculo.Cidade.CompleteObject();
                txtCidadePesquisa.Text = String.Format("{0}/{1}", objPesquisaCurriculo.Cidade.NomeCidade, objPesquisaCurriculo.Cidade.Estado.SiglaEstado);
            }

            txtPalavraChave.Valor = objPesquisaCurriculo.DescricaoPalavraChave;
            txtExcluirPalavraChave.Valor = objPesquisaCurriculo.DescricaoFiltroExcludente;

            if (objPesquisaCurriculo.Estado != null)
                rcbEstado.SelectedValue = objPesquisaCurriculo.Estado.SiglaEstado;

            if (objPesquisaCurriculo.Escolaridade != null)
                rcbNivel.SelectedValue = objPesquisaCurriculo.Escolaridade.IdEscolaridade.ToString(CultureInfo.CurrentCulture);

            if (objPesquisaCurriculo.Sexo != null)
                rcbSexo.SelectedValue = objPesquisaCurriculo.Sexo.IdSexo.ToString(CultureInfo.CurrentCulture);

            if (objPesquisaCurriculo.DataIdadeMin.HasValue)
                txtIdadeDe.Valor = (DateTime.Today.Year - objPesquisaCurriculo.DataIdadeMin.Value.Year).ToString(CultureInfo.CurrentCulture);

            if (objPesquisaCurriculo.DataIdadeMax.HasValue)
                txtIdadeAte.Valor = (DateTime.Today.Year - objPesquisaCurriculo.DataIdadeMax.Value.Year).ToString(CultureInfo.CurrentCulture);

            txtSalario.Valor = objPesquisaCurriculo.NumeroSalarioMin;
            txtSalarioAte.Valor = objPesquisaCurriculo.NumeroSalarioMax;

            if (objPesquisaCurriculo.QuantidadeExperiencia.HasValue)
                txtTempoExperiencia.Valor = objPesquisaCurriculo.QuantidadeExperiencia.ToString();

            //Identifica os itens selecionados na dropdown de idioma.
            List<PesquisaCurriculoIdioma> listPesquisaCurriculoIdioma = PesquisaCurriculoIdioma.ListarPorPesquisaList(objPesquisaCurriculo.IdPesquisaCurriculo);
            foreach (PesquisaCurriculoIdioma objPesquisaCurriculoIdioma in listPesquisaCurriculoIdioma)
                rcbIdioma.SetItemChecked(objPesquisaCurriculoIdioma.Idioma.IdIdioma.ToString(CultureInfo.CurrentCulture), true);

            //Identifica os itens selecionado na dropdown de disponibilidade.
            List<PesquisaCurriculoDisponibilidade> listPesquisaCurriculoDisponibilidade = PesquisaCurriculoDisponibilidade.ListarPorPesquisa(objPesquisaCurriculo);
            foreach (PesquisaCurriculoDisponibilidade objPesquisaCurriculoDisponibilidade in listPesquisaCurriculoDisponibilidade)
                rcbDisponibilidade.SetItemChecked(objPesquisaCurriculoDisponibilidade.Disponibilidade.IdDisponibilidade.ToString(CultureInfo.CurrentCulture), true);

            // [Obsolete("Função inativada")]
            // Código comentado é relacionado  a função TipoVínculo que existia
            //Identifica os itens, selecionando na checkboxlist de tipo vinculo.
            //List<PesquisaCurriculoTipoVinculo> listPesquisaCurriculoTipoVinculo = PesquisaCurriculoTipoVinculo.ListarPorPesquisaList(objPesquisaCurriculo.IdPesquisaCurriculo);
            //foreach (var pesquisaCurriculoTipoVinculo in listPesquisaCurriculoTipoVinculo)
            //{
            //    ucContratoFuncao.SetTipoVinculoChecked(
            //        pesquisaCurriculoTipoVinculo.TipoVinculo.IdTipoVinculo.ToString(CultureInfo.CurrentCulture), true);
            //}

            txtNomeCpfCodigo.Valor = objPesquisaCurriculo.DescricaoCodCPFNome;

            if (objPesquisaCurriculo.EstadoCivil != null)
                rcbEstadoCivil.SelectedValue = objPesquisaCurriculo.EstadoCivil.IdEstadoCivil.ToString(CultureInfo.CurrentCulture);

            txtBairro.Valor = objPesquisaCurriculo.DescricaoBairro;
            txtLogradouro.Valor = objPesquisaCurriculo.DescricaoLogradouro;
            txtFaixaCep.Valor = objPesquisaCurriculo.NumeroCEPMin;
            txtFaixaCepAte.Valor = objPesquisaCurriculo.NumeroCEPMax;

            //Fonte e curso
            if (objPesquisaCurriculo.FonteTecnicoGraduacao != null)
            {
                objPesquisaCurriculo.FonteTecnicoGraduacao.CompleteObject();
                txtInstituicaoTecnicoGraduacao.Text = String.Format("{0} - {1}", objPesquisaCurriculo.FonteTecnicoGraduacao.SiglaFonte, objPesquisaCurriculo.FonteTecnicoGraduacao.NomeFonte);
            }

            if (objPesquisaCurriculo.FonteOutrosCursos != null)
            {
                objPesquisaCurriculo.FonteOutrosCursos.CompleteObject();
                txtInstituicao.Text = String.Format("{0} - {1}", objPesquisaCurriculo.FonteOutrosCursos.SiglaFonte, objPesquisaCurriculo.FonteOutrosCursos.NomeFonte);
            }

            if (objPesquisaCurriculo.CursoOutrosCursos != null)
            {
                objPesquisaCurriculo.CursoOutrosCursos.CompleteObject();
                txtTituloCurso.Text = objPesquisaCurriculo.CursoOutrosCursos.DescricaoCurso;
            }
            else if (!String.IsNullOrEmpty(objPesquisaCurriculo.DescricaoCursoOutrosCursos))
                txtTituloCurso.Text = objPesquisaCurriculo.DescricaoCursoOutrosCursos;

            if (objPesquisaCurriculo.CursoTecnicoGraduacao != null)
            {
                objPesquisaCurriculo.CursoTecnicoGraduacao.CompleteObject();
                txtTituloTecnicoGraduacao.Text = objPesquisaCurriculo.CursoTecnicoGraduacao.DescricaoCurso;
            }
            else if (!String.IsNullOrEmpty(objPesquisaCurriculo.DescricaoCursoTecnicoGraduacao))
                txtTituloTecnicoGraduacao.Text = objPesquisaCurriculo.DescricaoCursoTecnicoGraduacao;

            txtEmpresa.Valor = objPesquisaCurriculo.RazaoSocial;

            if (objPesquisaCurriculo.AreaBNE != null)
                rcbAtividadeEmpresa.SelectedValue = objPesquisaCurriculo.AreaBNE.IdAreaBNE.ToString(CultureInfo.CurrentCulture);

            if (objPesquisaCurriculo.CategoriaHabilitacao != null)
                rcbHabilitacao.SelectedValue = objPesquisaCurriculo.CategoriaHabilitacao.IdCategoriaHabilitacao.ToString(CultureInfo.CurrentCulture);

            if (objPesquisaCurriculo.TipoVeiculo != null)
                rcbVeiculo.SelectedValue = objPesquisaCurriculo.TipoVeiculo.IdTipoVeiculo.ToString(CultureInfo.CurrentCulture);

            if (objPesquisaCurriculo.Deficiencia != null)
                rcbPCD.SelectedValue = objPesquisaCurriculo.Deficiencia.IdDeficiencia.ToString(CultureInfo.CurrentCulture);

            if (objPesquisaCurriculo.Raca != null)
                rcbRaca.SelectedValue = objPesquisaCurriculo.Raca.IdRaca.ToString(CultureInfo.CurrentCulture);

            if (objPesquisaCurriculo.FlagFilhos.HasValue)
                rcbFilhos.SelectedValue = objPesquisaCurriculo.FlagFilhos.Value ? "1" : "0";

            var paramResult = Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdfsEscolaridadeWebEstagiosQueroEstagiario);

            if (String.Equals(paramResult, objPesquisaCurriculo.IdEscolaridadeWebStagio, StringComparison.OrdinalIgnoreCase))
                this.ucEstagiarioFuncao.SetEstagiario(true);

            txtTelefone.DDD = objPesquisaCurriculo.NumeroDDDTelefone;
            txtTelefone.Fone = objPesquisaCurriculo.NumeroTelefone;
            txtEmail.Text = objPesquisaCurriculo.EmailPessoa;
        }
        #endregion

        #region AnunciarVaga
        private void AnunciarVaga()
        {
            try
            {
                base.UrlDestino.Value = "PesquisaCurriculoAvancada.aspx";

                var objPesquisaCurriculo = Buscar();

                if (objPesquisaCurriculo != null)
                {
                    Session.Add(Chave.Temporaria.Variavel7.ToString(), objPesquisaCurriculo.IdPesquisaCurriculo);

                    Redirect(Page.GetRouteUrl(Enumeradores.RouteCollection.AnunciarVaga.ToString(), null));
                }
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            Ajax.Utility.RegisterTypeForAjax(typeof(PesquisaCurriculoAvancada));
        }
        #endregion

        #region Page_PreRender
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "AjustarCampos", "javaScript:AjustarCampos();", true);
        }
        #endregion

        #region btiMapaBairro_Click
        protected void btiMapaBairro_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtCidadePesquisa.Text))
            {
                string valor = txtCidadePesquisa.Text;
                string valorTeste = txtBairro.Valor;
                Cidade objCidade;

                if (!String.IsNullOrEmpty(valorTeste))
                {
                    if (Cidade.CarregarPorNome(valor, out objCidade))
                    {
                        string strNomeCidade = UIHelper.RemoverAcentos(valorTeste) + " " + UIHelper.RemoverAcentos(objCidade.NomeCidade) + " " + objCidade.Estado.SiglaEstado;
                        string url = "BuscarBairro.aspx?pCidade=" + strNomeCidade;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "bairro", "window.open('" + url + "', 'PopUpWindow', 'toolbar=0,scrollbars=0,location=0,statusbar=0,menubar=0,resizable=0,width=510,height=450');", true);
                    }
                }

                if (Cidade.CarregarPorNome(valor, out objCidade))
                {
                    string strNomeCidade = UIHelper.RemoverAcentos(objCidade.NomeCidade) + " " + objCidade.Estado.SiglaEstado;
                    string url = "BuscarBairro.aspx?pCidade=" + strNomeCidade;
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "bairro", "window.open('" + url + "', 'PopUpWindow', 'toolbar=0,scrollbars=0,location=0,statusbar=0,menubar=0,resizable=0,width=510,height=450');", true);
                }
            }
            else
                ExibirMensagem("É necessário informar uma cidade para utilizar este campo.", TipoMensagem.Erro);
        }
        #endregion

        #region txtCidadePesquisa_TextChanged

        protected void txtCidadePesquisa_TextChanged(object serder, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCidadePesquisa.Text))
            {
                string[] cidadeEstado = txtCidadePesquisa.Text.Split('/');
                if (cidadeEstado.Length == 2)
                {
                    Estado objEstado = Estado.CarregarPorSiglaEstado(cidadeEstado[1]);
                    rcbEstado.SelectedValue = objEstado.SiglaEstado;
                    rcbEstado.Enabled = false;
                }
                else
                    txtCidadePesquisa.Text = string.Empty;
            }
            else
            {
                rcbEstado.ClearSelection();
                rcbEstado.Enabled = true;
            }

            upPnlCamposPesquisa.Update();
            txtPalavraChave.Focus();
        }

        #endregion

        #region btiBuscarFlutuante_Click
        protected void btiBuscarFlutuante_Click(object sender, EventArgs e)
        {
            try
            {
                Buscar();

                Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculo.ToString(), null));
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btiLimparFlutuante_Click
        protected void btiLimparFlutuante_Click(object sender, EventArgs e)
        {
            LimparCampos();
            upPnlCamposPesquisa.Update();
        }
        #endregion

        #region btnAnunciarVagas_Click
        protected void btnAnunciarVagas_Click(object sender, EventArgs e)
        {
            AnunciarVaga();
        }
        #endregion

        #region btiAnunciarVagas_Click
        protected void btiAnunciarVagas_Click(object sender, EventArgs e)
        {
            AnunciarVaga();
        }
        #endregion

        #region btnBuscar_Click
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                Buscar();

                Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculo.ToString(), null));
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnLimpar_Click
        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
            upPnlCamposPesquisa.Update();
        }
        #endregion

        #endregion

        #region AjaxMethods

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

        #region RecuperarCidade
        /// <summary>
        /// Recuperar cidade
        /// </summary>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public static string RecuperarCidade(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
                return String.Empty;

            Cidade objCidade;
            if (valor.LastIndexOf('/').Equals(-1))
                if (Cidade.CarregarPorNome(valor, out objCidade))
                    return objCidade.NomeCidade + "/" + objCidade.Estado.SiglaEstado;

            return String.Empty;
        }
        #endregion

        #region ValidarFuncao
        /// <summary>
        /// Validar Funcao
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public bool ValidarFuncao(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
                return true;

            int? idOrigem = null;
            if (base.IdOrigem.HasValue)
                idOrigem = base.IdOrigem.Value;

            return Funcao.ValidarFuncaoPorOrigem(idOrigem, valor);
        }
        #endregion

        #endregion

    }
}