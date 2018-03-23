using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web.UI;
using BNE.BLL;
using BNE.BLL.Custom.Maps;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Resources;
using Employer.Componentes.UI.Web;
using Microsoft.SqlServer.Types;
using Resources;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class SalaSelecionadorRastreadorCurriculos : BasePage
    {

        #region Propriedades

        #region IdRastreadorCurriculo - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID do rastreador
        /// </summary>
        public int? IdRastreadorCurriculo
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

        #region UrlOrigem - Variável 2
        /// <summary>
        /// </summary>
        public string UrlOrigem
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel2.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel2.ToString()]).ToString();
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

        #region Permissoes - Variável Permissoes
        /// <summary>
        /// Propriedade que armazena e recupera as permissoes do usuario
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

        #region Comportamento dos checkbox de fonte de dados
        protected void chk_stc_fonte_bne_CheckedChanged(object sender, EventArgs e)
        {
            if (!chk_stc_fonte_bne.Checked && !chk_stc_fonte_meus_cv.Checked)
            {
                chk_stc_fonte_bne.Checked = true;
                pnl_stc_fonte_alert.Visible = true;
            }
            else
            {
                pnl_stc_fonte_alert.Visible = false;
            }
        }
        protected void chk_stc_fonte_meus_cv_CheckedChanged(object sender, EventArgs e)
        {
            if (!chk_stc_fonte_bne.Checked && !chk_stc_fonte_meus_cv.Checked)
            {
                chk_stc_fonte_meus_cv.Checked = true;
                pnl_stc_fonte_alert.Visible = true;
            }
            else
            {
                pnl_stc_fonte_alert.Visible = false;
            }
        }
        #endregion

        #region BuscarBairro
        private void BuscarBairro(ComboCheckbox comboBox, ref List<string> listaBairros, ref List<string> listaZona, string zona)
        {
            if (comboBox.GetCheckedItems().Count == comboBox.Items.Count)
                listaZona.Add(zona);
            else
                listaBairros.AddRange(comboBox.GetCheckedItems().Select(ci => ci.Text));
        }
        #endregion

        #region Inicializar
        private void Inicializar()
        {
            AjustarPermissoes();

            ucEstagiarioFuncao.AtualizarValidationGroup(this.btnSalvar.ValidationGroup);

            //Carrando as drop-down's
            UIHelper.CarregarRadComboBox(rcbNivelEscolaridade, Escolaridade.Listar(), new RadComboBoxItem("Qualquer", "0"));
            UIHelper.CarregarRadComboBox(rcbBuscaPalavraChave, CampoPalavraChave.BuscaCampos());
            UIHelper.CarregarRadComboBox(rcbNivelEscolaridadeEstagiario, Escolaridade.Listar(), new RadComboBoxItem("Qualquer", "0"));
            UIHelper.CarregarRadComboBox(rcbSexo, Sexo.Listar(), new RadComboBoxItem("Qualquer", "0"));
            
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
            txtCidadePesquisa.Attributes["onBlur"] += "RastreadorCurriculoAvancadaCidadeOnBlur(this)";

            txtSalarioAte.MensagemErroIntervalo = String.Format(MensagemAviso._304501, "Máximo", "maior", "Mínimo");
            cvFuncaoFaixaCepAte.ErrorMessage = String.Format(MensagemAviso._304501, "Máximo", "maior", "Mínimo");

            CarregarParametros();

            LimparCampos();
            CarregarGrid();

            if (IdRastreadorCurriculo.HasValue)
                PreencherCampos();

            //Tanto para o limpar campos quanto para o preencher campos
            AtualizarUpdatePanelCampos();
            HabilitaCheckNotificarBoxHora();

            //Mosta o painel de fontes apenas na versão STC
            pnl_stc_fonte.Visible = STC.HasValue && STC.Value;
            InicializarBarraBusca(TipoBuscaMaster.Curriculo, false);
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

                //aceCidade.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                //aceCidade.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteCidade]);
                //aceCidade.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteCidade]);

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
                //txtTempoExperiencia.Valor =
                //txtNomeCpfCodigo.Valor =
            txtBairro.Valor =
            txtLogradouro.Valor =
                txtFaixaCep.Valor =
                txtFaixaCepAte.Valor =
            txtTituloCurso.Text =
            txtInstituicao.Text =
            txtTituloTecnicoGraduacao.Text =
            txtInstituicaoTecnicoGraduacao.Text =
                //txtEmpresa.Valor =
                //txtTelefone.DDD =
                //txtTelefone.Fone =
            txtExcluirPalavraChave.Valor =
                //txtEmail.Text = 
            String.Empty;

            rcbBairroZonaCentral.ClearCheckeds();
            rcbBairroZonaSul.ClearCheckeds();
            rcbBairroZonaOeste.ClearCheckeds();
            rcbBairroZonaLeste.ClearCheckeds();
            rcbBairroZonaNorte.ClearCheckeds();

            pnlBairroZona.Visible = false;
            pnlBairroTexto.Visible = true;

            txtSalarioAte.Valor =
            txtSalario.Valor = null;

        
            rcbDisponibilidade.ClearCheckeds();

        
            rcbDisponibilidade.Text = rcbDisponibilidade.EmptyMessage;

            rcbNivelEscolaridade.SelectedValue =
            rcbNivelEscolaridadeEstagiario.SelectedValue =
            rcbSexo.SelectedValue =
            rcbEstado.SelectedValue =
            rcbEstadoCivil.SelectedValue =
            rcbAtividadeEmpresa.SelectedValue =
            rcbHabilitacao.SelectedValue =
            rcbVeiculo.SelectedValue =
            rcbRaca.SelectedValue = "0";

            rcbPCD.SelectedValue =
            rcbFilhos.SelectedValue = "-1";

            ckbNotificarUmaVezPorDia.Checked = true;
            ckbNotificarUmaVezPorHora.Checked = false;

            ucPesquisaIdioma.LimparCampos();
            //upTxtEmpresa.Update();
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos()
        {
            try
            {
                if (IdRastreadorCurriculo.HasValue)
                {
                    BLL.RastreadorCurriculo objRastreadorCurriculo = BLL.RastreadorCurriculo.LoadObject(IdRastreadorCurriculo.Value);

                    if (objRastreadorCurriculo == null)
                        return;

                    var listaFuncao = BLL.RastreadorCurriculoFuncao.ListarIdentificadoresFuncaoPorRastreador(objRastreadorCurriculo);
                    ucEstagiarioFuncao.SetFuncoes(listaFuncao.Select(Funcao.LoadObject).ToList());

                    if (objRastreadorCurriculo.Cidade != null)
                    {
                        objRastreadorCurriculo.Cidade.CompleteObject();
                        txtCidadePesquisa.Text = String.Format("{0}/{1}", objRastreadorCurriculo.Cidade.NomeCidade, objRastreadorCurriculo.Cidade.Estado.SiglaEstado);
                        RecuperarBairros(objRastreadorCurriculo.Cidade.NomeCidade, objRastreadorCurriculo.Cidade.Estado.SiglaEstado);
                    }

                    txtPalavraChave.Valor = objRastreadorCurriculo.DescricaoPalavraChave;
                    txtExcluirPalavraChave.Valor = objRastreadorCurriculo.DescricaoFiltroExcludente;

                    if (objRastreadorCurriculo.Estado != null)
                        rcbEstado.SelectedValue = objRastreadorCurriculo.Estado.SiglaEstado;

                    if (objRastreadorCurriculo.Escolaridade != null)
                        rcbNivelEscolaridade.SelectedValue = rcbNivelEscolaridadeEstagiario.SelectedValue = objRastreadorCurriculo.Escolaridade.IdEscolaridade.ToString(CultureInfo.CurrentCulture);

                    if (objRastreadorCurriculo.Sexo != null)
                        rcbSexo.SelectedValue = objRastreadorCurriculo.Sexo.IdSexo.ToString(CultureInfo.CurrentCulture);

                    if (objRastreadorCurriculo.NumeroIdadeMin.HasValue)
                        txtIdadeDe.Valor = objRastreadorCurriculo.NumeroIdadeMin.Value.ToString(CultureInfo.CurrentCulture);

                    if (objRastreadorCurriculo.NumeroIdadeMax.HasValue)
                        txtIdadeAte.Valor = objRastreadorCurriculo.NumeroIdadeMax.Value.ToString(CultureInfo.CurrentCulture);

                    txtSalario.Valor = objRastreadorCurriculo.NumeroSalarioMin;
                    txtSalarioAte.Valor = objRastreadorCurriculo.NumeroSalarioMax;

                    //Identifica os itens selecionados na dropdown de idioma.
                    var listaIdioma = RastreadorCurriculoIdioma.ListarIdiomaPorPesquisa(objRastreadorCurriculo);
                    ucPesquisaIdioma.SetIdiomas(listaIdioma);

                    //Identifica os itens selecionado na dropdown de disponibilidade.
                    var listaDisponibilidade = RastreadorCurriculoDisponibilidade.ListarIdentificadoresDisponibilidadePorRastreador(objRastreadorCurriculo);
                    foreach (var item in listaDisponibilidade)
                        rcbDisponibilidade.SetItemChecked(item.ToString(CultureInfo.CurrentCulture), true);

                    if (objRastreadorCurriculo.EstadoCivil != null)
                        rcbEstadoCivil.SelectedValue = objRastreadorCurriculo.EstadoCivil.IdEstadoCivil.ToString(CultureInfo.CurrentCulture);

                    if (pnlBairroZona.Visible)
                    {
                        if (!string.IsNullOrWhiteSpace(objRastreadorCurriculo.DescricaoBairro))
                        {
                            var bairros = objRastreadorCurriculo.DescricaoBairro.Split(',');
                            PreencherCamposCheckbox(rcbBairroZonaCentral, bairros);
                            PreencherCamposCheckbox(rcbBairroZonaLeste, bairros);
                            PreencherCamposCheckbox(rcbBairroZonaOeste, bairros);
                            PreencherCamposCheckbox(rcbBairroZonaNorte, bairros);
                            PreencherCamposCheckbox(rcbBairroZonaSul, bairros);
                        }
                        if (!string.IsNullOrWhiteSpace(objRastreadorCurriculo.DescricaoZona))
                        {
                            var zonas = objRastreadorCurriculo.DescricaoZona.Split(',');
                            if (zonas.Contains("Centro"))
                                rcbBairroZonaCentral.CheckAllItems();
                            if (zonas.Contains("Zona Sul"))
                                rcbBairroZonaSul.CheckAllItems();
                            if (zonas.Contains("Zona Norte"))
                                rcbBairroZonaNorte.CheckAllItems();
                            if (zonas.Contains("Zona Leste"))
                                rcbBairroZonaLeste.CheckAllItems();
                            if (zonas.Contains("Zona Oeste"))
                                rcbBairroZonaOeste.CheckAllItems();
                        }
                    }
                    else
                        txtBairro.Valor = objRastreadorCurriculo.DescricaoBairro;

                    txtLogradouro.Valor = objRastreadorCurriculo.DescricaoLogradouro;
                    txtFaixaCep.Valor = objRastreadorCurriculo.NumeroCEPMin;
                    txtFaixaCepAte.Valor = objRastreadorCurriculo.NumeroCEPMax;

                    //Fonte e curso
                    if (objRastreadorCurriculo.FonteTecnicoGraduacao != null)
                    {
                        objRastreadorCurriculo.FonteTecnicoGraduacao.CompleteObject();
                        txtInstituicaoTecnicoGraduacao.Text = String.Format("{0} - {1}", objRastreadorCurriculo.FonteTecnicoGraduacao.SiglaFonte, objRastreadorCurriculo.FonteTecnicoGraduacao.NomeFonte);
                    }

                    if (objRastreadorCurriculo.FonteOutrosCursos != null)
                    {
                        objRastreadorCurriculo.FonteOutrosCursos.CompleteObject();
                        txtInstituicao.Text = String.Format("{0} - {1}", objRastreadorCurriculo.FonteOutrosCursos.SiglaFonte, objRastreadorCurriculo.FonteOutrosCursos.NomeFonte);
                    }

                    if (objRastreadorCurriculo.CursoOutrosCursos != null)
                    {
                        objRastreadorCurriculo.CursoOutrosCursos.CompleteObject();
                        txtTituloCurso.Text = objRastreadorCurriculo.CursoOutrosCursos.DescricaoCurso;
                    }
                    else if (!String.IsNullOrEmpty(objRastreadorCurriculo.DescricaoCursoOutrosCursos))
                        txtTituloCurso.Text = objRastreadorCurriculo.DescricaoCursoOutrosCursos;

                    if (objRastreadorCurriculo.CursoTecnicoGraduacao != null)
                    {
                        objRastreadorCurriculo.CursoTecnicoGraduacao.CompleteObject();
                        txtTituloTecnicoGraduacao.Text = objRastreadorCurriculo.CursoTecnicoGraduacao.DescricaoCurso;
                    }
                    else if (!String.IsNullOrEmpty(objRastreadorCurriculo.DescricaoCursoTecnicoGraduacao))
                        txtTituloTecnicoGraduacao.Text = objRastreadorCurriculo.DescricaoCursoTecnicoGraduacao;

                    //txtEmpresa.Valor = objRastreadorCurriculo.RazaoSocial;

                    if (objRastreadorCurriculo.AreaBNE != null)
                        rcbAtividadeEmpresa.SelectedValue = objRastreadorCurriculo.AreaBNE.IdAreaBNE.ToString(CultureInfo.CurrentCulture);

                    if (objRastreadorCurriculo.CategoriaHabilitacao != null)
                        rcbHabilitacao.SelectedValue = objRastreadorCurriculo.CategoriaHabilitacao.IdCategoriaHabilitacao.ToString(CultureInfo.CurrentCulture);

                    if (objRastreadorCurriculo.TipoVeiculo != null)
                        rcbVeiculo.SelectedValue = objRastreadorCurriculo.TipoVeiculo.IdTipoVeiculo.ToString(CultureInfo.CurrentCulture);

                    if (objRastreadorCurriculo.Deficiencia != null)
                        rcbPCD.SelectedValue = objRastreadorCurriculo.Deficiencia.IdDeficiencia.ToString(CultureInfo.CurrentCulture);

                    if (objRastreadorCurriculo.Raca != null)
                        rcbRaca.SelectedValue = objRastreadorCurriculo.Raca.IdRaca.ToString(CultureInfo.CurrentCulture);

                    if (objRastreadorCurriculo.FlagFilhos.HasValue)
                        rcbFilhos.SelectedValue = objRastreadorCurriculo.FlagFilhos.Value ? "1" : "0";

                    var paramResult = Parametro.RecuperaValorParametro(Enumeradores.Parametro.IdfsEscolaridadeWebEstagiosQueroEstagiario);

                    if (String.Equals(paramResult, objRastreadorCurriculo.IdEscolaridadeWebStagio, StringComparison.OrdinalIgnoreCase))
                        this.ucEstagiarioFuncao.SetEstagiario(true);

                    ckbNotificarUmaVezPorDia.Checked = objRastreadorCurriculo.FlagNotificaDia.Value;
                    ckbNotificarUmaVezPorHora.Checked = objRastreadorCurriculo.FlagNotificaHora.Value;

                    if (STC.HasValue && STC.Value)
                    {
                        if (objRastreadorCurriculo.Origem != null)
                        {
                            if (objRastreadorCurriculo.Origem.IdOrigem == (int)Enumeradores.Origem.BNE)
                            {
                                chk_stc_fonte_bne.Checked = true;
                                chk_stc_fonte_meus_cv.Checked = false;
                            }
                            else if (objRastreadorCurriculo.Origem.IdOrigem == IdOrigem.Value)
                            {
                                chk_stc_fonte_bne.Checked = false;
                                chk_stc_fonte_meus_cv.Checked = true;
                            }
                        }
                    }
                }
                HabilitaCheckNotificarBoxHora();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, IdRastreadorCurriculo.Value.ToString());
            }
        }
        #endregion

        #region PreencherCamposCheckbox
        private void PreencherCamposCheckbox(ComboCheckbox comboCheckbox, string[] bairros)
        {
            if (comboCheckbox != null && bairros != null)
            {
                var combos = comboCheckbox.Items.Where(c => bairros.Any(b => b.Trim() == c.Text.Trim()));

                foreach (var comboCheckItem in combos)
                    comboCheckbox.SetItemChecked(comboCheckItem, true);
            }
        }
        #endregion

        #region RecuperarBairros
        private void RecuperarBairros(string cidade, string estado)
        {
            var url = Parametro.RecuperaValorParametro(Enumeradores.Parametro.UrlAPICEP) + "/api/bairro/getbycidade/{cidade}/{estado}".Replace("{cidade}", cidade).Replace("{estado}", estado);
            // Create an HttpClient instance 
            using (var client = new HttpClient())
            {

                // Send a request asynchronously continue when complete 
                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseData = response.Content.ReadAsAsync<dynamic>().Result;

                    var jsonObject = responseData as Newtonsoft.Json.Linq.JObject;

                    if (jsonObject != null)
                    {
                        var centro = jsonObject["Centro"] as Newtonsoft.Json.Linq.JArray;
                        if (centro != null)
                            UIHelper.CarregarRadComboBox(rcbBairroZonaCentral, centro.ToDictionary(js => js["ID"].ToString(), js => js["Nome"].ToString()));

                        var norte = jsonObject["Zona Norte"] as Newtonsoft.Json.Linq.JArray;
                        if (norte != null)
                            UIHelper.CarregarRadComboBox(rcbBairroZonaNorte, norte.ToDictionary(js => js["ID"].ToString(), js => js["Nome"].ToString()));

                        var sul = jsonObject["Zona Sul"] as Newtonsoft.Json.Linq.JArray;
                        if (sul != null)
                            UIHelper.CarregarRadComboBox(rcbBairroZonaSul, sul.ToDictionary(js => js["ID"].ToString(), js => js["Nome"].ToString()));

                        var leste = jsonObject["Zona Leste"] as Newtonsoft.Json.Linq.JArray;
                        if (leste != null)
                            UIHelper.CarregarRadComboBox(rcbBairroZonaLeste, leste.ToDictionary(js => js["ID"].ToString(), js => js["Nome"].ToString()));

                        var oeste = jsonObject["Zona Oeste"] as Newtonsoft.Json.Linq.JArray;
                        if (oeste != null)
                            UIHelper.CarregarRadComboBox(rcbBairroZonaOeste, oeste.ToDictionary(js => js["ID"].ToString(), js => js["Nome"].ToString()));

                        pnlBairroZona.Visible = true;
                        pnlBairroTexto.Visible = false;
                    }
                    else
                    {
                        pnlBairroZona.Visible = false;
                        pnlBairroTexto.Visible = true;
                    }
                    upBairroZona.Update();
                    upBairroTexto.Update();
                    upBairroZonaCentral.Update();
                    upBairroZonaNorte.Update();
                    upBairroZonaSul.Update();
                    upBairroZonaLeste.Update();
                    upBairroZonaOeste.Update();
                }
            }
        }
        #endregion

        #region ExisteCampoPreenchido
        private bool ExisteCampoPreenchido()
        {
            bool retorno =

                (
                 ucEstagiarioFuncao.Funcoes.Rows.Count > 0 ||
                 !String.IsNullOrEmpty(txtCidadePesquisa.Text) ||
                 !String.IsNullOrEmpty(txtPalavraChave.Valor) ||
                 !String.IsNullOrEmpty(txtIdadeDe.Valor) ||
                 !String.IsNullOrEmpty(txtIdadeAte.Valor) ||
                /*!String.IsNullOrEmpty(txtExperiencia.Valor) ||*/
                 !String.IsNullOrEmpty(txtBairro.Valor) ||
                 !String.IsNullOrEmpty(txtFaixaCep.Valor) ||
                 !String.IsNullOrEmpty(txtFaixaCepAte.Valor) ||
                 !String.IsNullOrEmpty(txtTituloCurso.Text) ||
                 !String.IsNullOrEmpty(txtInstituicao.Text) ||
                 !String.IsNullOrEmpty(txtTituloTecnicoGraduacao.Text) ||
                 !String.IsNullOrEmpty(txtInstituicaoTecnicoGraduacao.Text) ||
                //!String.IsNullOrEmpty(txtEmpresa.Valor) ||
                 txtSalarioAte.Valor.HasValue ||
                 txtSalario.Valor.HasValue ||
                 !rcbEstado.SelectedValue.Equals("0") ||
                /*!ddlFormacao.SelectedValue.Equals("0") ||*/
                 !rcbSexo.SelectedValue.Equals("0") ||
                 !rcbEstadoCivil.SelectedValue.Equals("0") ||
                 !rcbAtividadeEmpresa.SelectedValue.Equals("0") ||
                 !rcbHabilitacao.SelectedValue.Equals("0") ||
                 !rcbVeiculo.SelectedValue.Equals("0") ||
                 !rcbPCD.SelectedValue.Equals("0") ||
                 !rcbRaca.SelectedValue.Equals("0") ||
                 !rcbFilhos.SelectedValue.Equals("0") ||
                 !rcbDisponibilidade.SelectedValue.Equals(String.Empty));

            return retorno;
        }
        #endregion

        #region Salvar
        private bool Salvar()
        {
            if (!ExisteCampoPreenchido())
            {
                ExibirMensagem(MensagemAviso._304505, TipoMensagem.Erro);
                return false;
            }

            if (!ckbNotificarUmaVezPorDia.Checked && !ckbNotificarUmaVezPorHora.Checked)
            {
                ExibirMensagem("Selecione pelo menos uma forma de notificação", TipoMensagem.Erro);
                return false;
            }

            var objRastreadorCurriculo = new RastreadorCurriculo
                {
                    Filial = new Filial(base.IdFilial.Value),
                    UsuarioFilialPerfil = new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value)
                };

            RastreadorCurriculo objRastreadorCurriculoAntigo = null;
            if (IdRastreadorCurriculo.HasValue)
                objRastreadorCurriculoAntigo = RastreadorCurriculo.LoadObject(IdRastreadorCurriculo.Value);

            //Lista de Idiomas com os niveis.
            var listaIdioma = new List<RastreadorCurriculoIdioma>();
            foreach (DataRow row in ucPesquisaIdioma.IdiomasSelecionados.Rows)
            {
                RastreadorCurriculoIdioma obj = new RastreadorCurriculoIdioma
                {
                    Idioma = new Idioma(Convert.ToInt32(row["idIdioma"])),
                    NivelIdioma = row["idNivel"] != DBNull.Value ? new NivelIdioma(Convert.ToInt32(row["idNivel"])) : null
                };
                listaIdioma.Add(obj);
            }

            //Identifica os itens selecionado na dropdown de disponibilidade.
            var listaDisponibilidade = rcbDisponibilidade.GetCheckedItems().Select(item => new RastreadorCurriculoDisponibilidade
            {
                Disponibilidade = new Disponibilidade(Convert.ToInt32(item.Value))
            }).ToList();

            var listaFuncoes = ucEstagiarioFuncao.Funcoes.AsEnumerable().Select(item => new RastreadorCurriculoFuncao
            {
                Funcao = new Funcao(Convert.ToInt32(item["IdFuncao"]))
            }).ToList();

            //Identifica os itens selecionado na dropdown de busca de palavra chave.
            var listaCampoBuscaPalavraChave = rcbBuscaPalavraChave.GetCheckedItems().Select(item => new CampoPalavraChaveRastreadorCurriculo
            {
                CampoPalavraChave = new CampoPalavraChave(Convert.ToInt32(item.Value))
            }).ToList();

            if (pnlBairroZona.Visible)
            {
                var listaBairros = new List<string>();
                var listaZona = new List<string>();

                BuscarBairro(rcbBairroZonaCentral, ref listaBairros, ref listaZona, "Centro");
                BuscarBairro(rcbBairroZonaLeste, ref listaBairros, ref listaZona, "Zona Leste");
                BuscarBairro(rcbBairroZonaSul, ref listaBairros, ref listaZona, "Zona Sul");
                BuscarBairro(rcbBairroZonaOeste, ref listaBairros, ref listaZona, "Zona Oeste");
                BuscarBairro(rcbBairroZonaNorte, ref listaBairros, ref listaZona, "Zona Norte");

                var limite = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.ValorMaximoBairrosPesquisaCurriculo));
                if (listaBairros.Count > limite)
                {
                    base.ExibirMensagem(string.Format("Selecione uma zona inteira ou no máximo {0} bairros.", limite), TipoMensagem.Erro);
                    return false;
                }

                if (listaZona.Count > 0)
                    objRastreadorCurriculo.DescricaoZona = listaZona.Aggregate((current, next) => current + "," + next);

                if (listaBairros.Count > 0)
                    objRastreadorCurriculo.DescricaoBairro = listaBairros.Aggregate((current, next) => current + "," + next);
            }
            else
                objRastreadorCurriculo.DescricaoBairro = txtBairro.Valor;

            Cidade objCidade;
            if (Cidade.CarregarPorNome(txtCidadePesquisa.Text, out objCidade))
            {
                objRastreadorCurriculo.Cidade = objCidade;
                if (objCidade.GeoLocalizacao != null && objCidade.GeoLocalizacao != SqlGeography.Null)
                    objRastreadorCurriculo.GeoBuscaCidade = objCidade.GeoLocalizacao;
                else
                {
                    var resultado = GeocodeService.RecuperarCoordenada(string.Empty, string.Empty, string.Empty, string.Empty, objCidade.NomeCidade, objCidade.Estado.SiglaEstado, GeocodeService.Provider.Google);
                    if (resultado != null)
                    {
                        objCidade.GeoLocalizacao = SqlGeography.Point(resultado.Latitude, resultado.Longitude, 4326);
                        objRastreadorCurriculo.GeoBuscaCidade = objCidade.GeoLocalizacao;
                        objCidade.Save();
                    }
                }

                if (!string.IsNullOrWhiteSpace(txtBairro.Valor))
                {
                    var lista = txtBairro.Valor.Split(',').Where(b => !string.IsNullOrWhiteSpace(b)).ToList();
                    if (lista.Count() == 1)
                    {
                        Bairro objBairro;
                        if (Bairro.CarregarPorNomeCidade(lista[0], objCidade, out objBairro))
                        {
                            if (objBairro.GeoLocalizacaoBairro != null && objBairro.GeoLocalizacaoBairro != SqlGeography.Null)
                                objRastreadorCurriculo.GeoBuscaBairro = objBairro.GeoLocalizacaoBairro;
                            else
                            {
                                var resultado = GeocodeService.RecuperarCoordenada(string.Empty, string.Empty, string.Empty, lista[0], objCidade.NomeCidade, objCidade.Estado.SiglaEstado, GeocodeService.Provider.Google);
                                if (resultado != null)
                                {
                                    objBairro.GeoLocalizacaoBairro = SqlGeography.Point(resultado.Latitude, resultado.Longitude, 4326);
                                    objRastreadorCurriculo.GeoBuscaBairro = objBairro.GeoLocalizacaoBairro;
                                    objBairro.Save();
                                }
                            }
                        }

                        objRastreadorCurriculo.DescricaoBairro = txtBairro.Valor.ToString();
                    }
                }
            }

            objRastreadorCurriculo.DescricaoPalavraChave = txtPalavraChave.Valor;
            objRastreadorCurriculo.DescricaoFiltroExcludente = txtExcluirPalavraChave.Valor;

            if (!rcbEstado.SelectedValue.Equals("0"))
                objRastreadorCurriculo.Estado = new Estado(rcbEstado.SelectedValue);

            if (!rcbNivelEscolaridade.SelectedValue.Equals("0"))
                objRastreadorCurriculo.Escolaridade = new Escolaridade(Convert.ToInt32(rcbNivelEscolaridade.SelectedValue));

            if (!rcbSexo.SelectedValue.Equals("0"))
                objRastreadorCurriculo.Sexo = new Sexo(Convert.ToInt32(rcbSexo.SelectedValue));

            if (!String.IsNullOrEmpty(txtIdadeDe.Valor))
                objRastreadorCurriculo.NumeroIdadeMin = Convert.ToInt16(txtIdadeDe.Valor);

            if (!String.IsNullOrEmpty(txtIdadeAte.Valor))
                objRastreadorCurriculo.NumeroIdadeMax = Convert.ToInt16(txtIdadeAte.Valor);

            objRastreadorCurriculo.NumeroSalarioMin = txtSalario.Valor;
            objRastreadorCurriculo.NumeroSalarioMax = txtSalarioAte.Valor;

            if (!rcbEstadoCivil.SelectedValue.Equals("0"))
                objRastreadorCurriculo.EstadoCivil = new EstadoCivil(Convert.ToInt32(rcbEstadoCivil.SelectedValue));

            objRastreadorCurriculo.DescricaoLogradouro = txtLogradouro.Valor;
            objRastreadorCurriculo.NumeroCEPMin = txtFaixaCep.Valor;
            objRastreadorCurriculo.NumeroCEPMax = txtFaixaCepAte.Valor;

            //Fonte e curso
            if (!String.IsNullOrEmpty(txtInstituicaoTecnicoGraduacao.Text))
            {
                Fonte objFonte;
                if (Fonte.CarregarPorSiglaNome(txtInstituicaoTecnicoGraduacao.Text, out objFonte))
                    objRastreadorCurriculo.FonteTecnicoGraduacao = objFonte;
            }

            if (!String.IsNullOrEmpty(txtTituloTecnicoGraduacao.Text))
            {
                Curso objCurso;
                if (Curso.CarregarPorNome(txtTituloTecnicoGraduacao.Text, out objCurso))
                    objRastreadorCurriculo.CursoTecnicoGraduacao = objCurso;
                else
                {
                    objRastreadorCurriculo.DescricaoCursoTecnicoGraduacao = txtTituloTecnicoGraduacao.Text;
                }
            }

            if (!String.IsNullOrEmpty(txtInstituicao.Text))
            {
                Fonte objFonte;
                if (Fonte.CarregarPorSiglaNome(txtInstituicao.Text, out objFonte))
                    objRastreadorCurriculo.FonteOutrosCursos = objFonte;
            }

            if (!String.IsNullOrEmpty(txtTituloCurso.Text))
            {
                Curso objCurso;
                if (Curso.CarregarPorNome(txtTituloCurso.Text, out objCurso))
                    objRastreadorCurriculo.CursoOutrosCursos = objCurso;
                else
                {
                    objRastreadorCurriculo.DescricaoCursoOutrosCursos = txtTituloCurso.Text;
                }
            }

            if (!rcbAtividadeEmpresa.SelectedValue.Equals("0"))
                objRastreadorCurriculo.AreaBNE = new AreaBNE(Convert.ToInt32(rcbAtividadeEmpresa.SelectedValue));

            if (!rcbHabilitacao.SelectedValue.Equals("0"))
                objRastreadorCurriculo.CategoriaHabilitacao = new CategoriaHabilitacao(Convert.ToInt32(rcbHabilitacao.SelectedValue));

            if (!rcbVeiculo.SelectedValue.Equals("0"))
                objRastreadorCurriculo.TipoVeiculo = new TipoVeiculo(Convert.ToInt32(rcbVeiculo.SelectedValue));

            if (!rcbPCD.SelectedValue.Equals("-1"))
                objRastreadorCurriculo.Deficiencia = new Deficiencia(Convert.ToInt32(rcbPCD.SelectedValue));

            if (!rcbRaca.SelectedValue.Equals("0"))
                objRastreadorCurriculo.Raca = new Raca(Convert.ToInt32(rcbRaca.SelectedValue));

            if (!rcbFilhos.SelectedValue.Equals("-1"))
                objRastreadorCurriculo.FlagFilhos = rcbFilhos.SelectedValue.Equals("1");

            //Verifica a Fonte de dados em caso de STC
            objRastreadorCurriculo.Origem = null;
            if (STC.HasValue)
            {
                if (STC.Value)
                {
                    if (chk_stc_fonte_bne.Checked && !chk_stc_fonte_meus_cv.Checked)
                        objRastreadorCurriculo.Origem = new Origem((int)Enumeradores.Origem.BNE);
                    else if (!chk_stc_fonte_bne.Checked && chk_stc_fonte_meus_cv.Checked)
                        objRastreadorCurriculo.Origem = new Origem(IdOrigem.Value);
                }
            }

            //Notificacao
            objRastreadorCurriculo.FlagNotificaDia = ckbNotificarUmaVezPorDia.Checked;
            objRastreadorCurriculo.FlagNotificaHora = ckbNotificarUmaVezPorHora.Checked;

            objRastreadorCurriculo.Salvar(listaFuncoes, listaIdioma, listaDisponibilidade, listaCampoBuscaPalavraChave, objRastreadorCurriculoAntigo, ucEstagiarioFuncao.CheckBoxEstagiarioSelecionado, ucEstagiarioFuncao.CheckBoxAprendizSelecionado);

            return true;
        }
        #endregion

        #region CarregarGrid
        private void CarregarGrid()
        {
            int totalRegistros;
            UIHelper.CarregarRadGrid(gvRastreadorCurriculos, RastreadorCurriculo.ListarRastreadores(base.IdFilial.Value, base.IdUsuarioFilialPerfilLogadoEmpresa.Value, gvRastreadorCurriculos.CurrentPageIndex, gvRastreadorCurriculos.PageSize, out totalRegistros), totalRegistros);

            upGvRastreadorCurriculos.Update();
        }
        #endregion

        #region AtualizarUpdatePanelCampos
        private void AtualizarUpdatePanelCampos()
        {
            upDisponibilidade.Update();
            upNivelEscolaridadeEstagiario.Update();
            upTituloTecnicoGraduacaoEstag.Update();
            upInstituicaoTecnicoGraduacaoEstag.Update();
            upSalarioDe.Update();
            upSalarioAte.Update();
            upCidade.Update();
            upEstado.Update();
            upLogradouro.Update();
            upFaixaCepDe.Update();
            upFaixaCepAte.Update();
            upPalavraChave.Update();
            upBuscaPalavraChave.Update();
            upExcluirPalavraChave.Update();
            upNivelEscolaridade.Update();
          
            upTituloTecnicoGraduacao.Update();
            upInstituicaoTecnicoGraduacao.Update();
            upTituloCurso.Update();
            upInstituicao.Update();
            upAtividadeEmpresa.Update();
            upIdadeDe.Update();
            upIdadeAte.Update();
            upSexo.Update();
            upEstadoCivil.Update();
            upPCD.Update();
            upFilhos.Update();
            upRaca.Update();
            upNotificarUmaVezPorDia.Update();
            upNotificarUmaVezPorHora.Update();
            upEstagiarioFuncao.Update();

           
            /*
            upBairroZonaCentral
            upBairroZonaNorte
            upBairroZonaSul
            upBairroZonaLeste
            upBairroZonaOeste
             */
        }
        #endregion

        #region RecuperarQuantidadeNaoVisualizado
        protected string RecuperarQuantidadeNaoVisualizado(int idRastreadorCurriculo)
        {
            return new RastreadorCurriculo(idRastreadorCurriculo).QuantidadeCurriculoNaoVisualizado().ToString();
        }
        #endregion

        #region HabilitaCheckNotificarBoxHora
        private void HabilitaCheckNotificarBoxHora()
        {
            if (ucEstagiarioFuncao.Funcoes.Rows.Count > 0 && !string.IsNullOrWhiteSpace(txtCidadePesquisa.Text))
            {
                ckbNotificarUmaVezPorHora.Enabled = true;
            }
            else
            {
                ckbNotificarUmaVezPorHora.Checked = false;
                ckbNotificarUmaVezPorHora.Enabled = false;
            }
            
            upNotificarUmaVezPorHora.Update();
        }
        #endregion

        #endregion

        #region Eventos

        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            ucEstagiarioFuncao.FuncoesAlteradas += ucEstagiarioFuncao_FuncoesAlteradas;
            ucConfirmacaoExclusao.Confirmar += ucConfirmacaoExclusao_Confirmar;
            Ajax.Utility.RegisterTypeForAjax(typeof(SalaSelecionadorRastreadorCurriculos));
        }
        #endregion
       
        #region Page_PreRender
        protected void Page_PreRender(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "AjustarCampos", "javaScript:AjustarCampos();", true);
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
                        ScriptManager.RegisterStartupScript(this, typeof(Microsoft.Office.Interop.Excel.Page), "bairro", "window.open('" + url + "', 'PopUpWindow', 'toolbar=0,scrollbars=0,location=0,statusbar=0,menubar=0,resizable=0,width=510,height=450');", true);
                    }
                }

                if (Cidade.CarregarPorNome(valor, out objCidade))
                {
                    string strNomeCidade = UIHelper.RemoverAcentos(objCidade.NomeCidade) + " " + objCidade.Estado.SiglaEstado;
                    string url = "BuscarBairro.aspx?pCidade=" + strNomeCidade;
                    ScriptManager.RegisterStartupScript(this, typeof(Microsoft.Office.Interop.Excel.Page), "bairro", "window.open('" + url + "', 'PopUpWindow', 'toolbar=0,scrollbars=0,location=0,statusbar=0,menubar=0,resizable=0,width=510,height=450');", true);
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

                    RecuperarBairros(cidadeEstado[0], cidadeEstado[1]);
                }
                else
                    txtCidadePesquisa.Text = string.Empty;
            }
            else
            {
                rcbEstado.ClearSelection();
            }

            HabilitaCheckNotificarBoxHora();
            txtLogradouro.Focus();
            upEstado.Update();
        }
        #endregion

        #region txtCidadePesquisa_TextChanged
        void ucEstagiarioFuncao_FuncoesAlteradas(object sender, EventArgs e)
        {
            HabilitaCheckNotificarBoxHora();
        }
        #endregion

        #region btnSalvar_Click
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Salvar())
                {
                    LimparCampos();
                    CarregarGrid();
                    ucConfirmacao.PreencherCampos("Confirmação", MensagemAviso._24025, false);
                    ucConfirmacao.MostrarModal();
                }
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            /*
             * if (base.STC.Value)
                Redirect("SiteTrabalheConoscoMenu.aspx");
            else
                Redirect("SalaSelecionador.aspx");
             */
            if (!string.IsNullOrEmpty(UrlOrigem))
                Redirect(UrlOrigem);
            else
                Redirect("Default.aspx");
        }
        #endregion

        #region Grid

        #region gvRastreadorCurriculos_ItemCommand
        protected void gvRastreadorCurriculos_ItemCommand(object source, GridCommandEventArgs e)
        {
            try
            {
                int idRastreador = Convert.ToInt32(gvRastreadorCurriculos.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Rastreador_Curriculo"]);

                if (e.CommandName.Equals("VisualizarCurriculo"))
                {
                    base.UrlDestino.Value = "SalaSelecionadorRastreadorCurriculos.aspx";

                    var objResultadoPesquisaCurriculo = new Code.ViewStateObjects.ResultadoPesquisaCurriculo(new Code.ViewStateObjects.ResultadoPesquisaCurriculoRastreador
                    {
                        IdRastreadorCurriculo = idRastreador
                    });

                    Session.Add(Chave.Temporaria.ViewStateObject_ResultadoPesquisaCurriculo.ToString(), objResultadoPesquisaCurriculo);

                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculo.ToString(), null));
                }
                if (e.CommandName.Equals("VisualizarCurriculoNaoVisualizado"))
                {
                    base.UrlDestino.Value = "SalaSelecionadorRastreadorCurriculos.aspx";

                    var objRastreadorCurriculo = new RastreadorCurriculo(idRastreador);

                    var objResultadoPesquisaCurriculo = new Code.ViewStateObjects.ResultadoPesquisaCurriculo(new Code.ViewStateObjects.ResultadoPesquisaCurriculoRastreador
                    {
                        IdRastreadorCurriculo = objRastreadorCurriculo.IdRastreadorCurriculo,
                        DataVisualizacao = objRastreadorCurriculo.RecuperarDataUltimaVisualizacao()
                    });

                    Session.Add(Chave.Temporaria.ViewStateObject_ResultadoPesquisaCurriculo.ToString(), objResultadoPesquisaCurriculo);

                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculo.ToString(), null));
                }
                else if (e.CommandName.Equals("EditarRastreador"))
                {
                    IdRastreadorCurriculo = idRastreador;

                    LimparCampos();
                    PreencherCampos();

                    AtualizarUpdatePanelCampos();
                }
                else if (e.CommandName.Equals("ExcluirRastreador"))
                {
                    IdRastreadorCurriculo = idRastreador;

                    ucConfirmacaoExclusao.Inicializar("Atenção!", "Tem certeza que deseja excluir este registro?!");
                    ucConfirmacaoExclusao.MostrarModal();
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region gvRastreadorCurriculos_PageIndexChanged
        protected void gvRastreadorCurriculos_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {
            gvRastreadorCurriculos.CurrentPageIndex++;
            CarregarGrid();
        }
        #endregion

        #endregion

        #region ucConfirmacaoExclusao_Confirmar
        private void ucConfirmacaoExclusao_Confirmar()
        {
            if (IdRastreadorCurriculo.HasValue)
            {
                try
                {
                    new RastreadorCurriculo((int)IdRastreadorCurriculo).Inativar();

                    CarregarGrid();
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                    throw;
                }
            }
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