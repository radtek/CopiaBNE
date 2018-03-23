using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web.UI;
using Ajax;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Maps;
using BNE.BLL.Enumeradores;
using BNE.Common.Session;
using BNE.EL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Code.ViewStateObjects;
using BNE.Web.Master;
using BNE.Web.Resources;
using BNE.Web.UserControls;
using Employer.Componentes.UI.Web;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json.Linq;
using Resources;
using Telerik.Web.UI;
using CategoriaPermissao = BNE.BLL.Enumeradores.CategoriaPermissao;
using CurriculoClassificacao = BNE.BLL.Enumeradores.CurriculoClassificacao;
using Deficiencia = BNE.BLL.Deficiencia;
using Disponibilidade = BNE.BLL.Disponibilidade;
using Escolaridade = BNE.BLL.Escolaridade;
using EstadoCivil = BNE.BLL.EstadoCivil;
using Flag = BNE.BLL.Flag;
using Funcao = BNE.BLL.Funcao;
using Origem = BNE.BLL.Enumeradores.Origem;
using Page = Microsoft.Office.Interop.Excel.Page;
using Parametro = BNE.BLL.Parametro;
using Sexo = BNE.BLL.Sexo;
using TipoMensagem = BNE.Web.Code.Enumeradores.TipoMensagem;

namespace BNE.Web
{
    public partial class PesquisaCurriculoAvancada : BasePage
    {
        protected SessionVariable<PesquisaPadrao> PesquisCurriculoPadrao = new SessionVariable<PesquisaPadrao>(Chave.Temporaria.PesquisaPadrao.ToString());

        #region Propriedades

        #region IdPesquisaCurriculoAvancado - Variável 10
        /// <summary>
        ///     Propriedade que armazena e recupera o IdPesquisaCurriculo
        /// </summary>
        protected int? IdPesquisaCurriculoAvancado
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel10.ToString()] != null)
                    return int.Parse(ViewState[Chave.Temporaria.Variavel10.ToString()].ToString());
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
        ///     Propriedade que armazena e recupera o IdPesquisaCurriculo
        /// </summary>
        protected List<int> Permissoes
        {
            get { return (List<int>)ViewState[Chave.Temporaria.Permissoes.ToString()]; }
            set { ViewState.Add(Chave.Temporaria.Permissoes.ToString(), value); }
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

        #region Buscar
        private BLL.PesquisaCurriculo Buscar()
        {
            Page.Validate("PesquisaAvancada");

            if (!Page.IsValid)
                return null;

            //Identifica os itens selecionado na dropdown de idioma.
            var listaIdioma =  new List<PesquisaCurriculoIdioma>();
            foreach (DataRow row in ucPesquisaIdioma.IdiomasSelecionados.Rows)
            {
                PesquisaCurriculoIdioma obj = new PesquisaCurriculoIdioma
                {
                    Idioma = new Idioma(Convert.ToInt32(row["idIdioma"])),
                    NivelIdioma = row["idNivel"] != DBNull.Value ? new NivelIdioma(Convert.ToInt32(row["idNivel"])) : null
                };
                listaIdioma.Add(obj);
            }


            //Identifica os itens selecionado na dropdown de disponibilidade.
            var listaDisponibilidade = rcbDisponibilidade.GetCheckedItems().Select(item => new PesquisaCurriculoDisponibilidade
            {
                Disponibilidade = new Disponibilidade(Convert.ToInt32(item.Value))
            }).ToList();

            var listaFuncoes = ucEstagiarioFuncao.Funcoes.AsEnumerable().Select(item => new PesquisaCurriculoFuncao
            {
                Funcao = new Funcao(Convert.ToInt32(item["IdFuncao"]))
            }).ToList();

            //Identifica os itens selecionado na dropdown de busca de palavra chave.
            var listaCampoBuscaPalavraChave = rcbBuscaPalavraChave.GetCheckedItems().Select(item => new CampoPalavraChavePesquisaCurriculo
            {
                CampoPalavraChave = new CampoPalavraChave(Convert.ToInt32(item.Value))
            }).ToList();

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

            if (CheckAvaliacaoPositiva.Checked)
            {
                objPesquisaCurriculo.InserirAvaliacao(CurriculoClassificacao.AvaliacaoPositiva);
            }

            if (CheckAvaliacaoNegativa.Checked)
            {
                objPesquisaCurriculo.InserirAvaliacao(CurriculoClassificacao.AvaliacaoNegativa);
            }

            if (CheckSemAvaliacao.Checked)
            {
                objPesquisaCurriculo.InserirAvaliacao(CurriculoClassificacao.AvaliacaoNeutra);
            }


            if (pnlBairroZona.Visible)
            {
                var listaBairros = new List<string>();
                var listaZona = new List<string>();

                BuscarBairro(rcbBairroZonaCentral, ref listaBairros, ref listaZona, "Centro");
                BuscarBairro(rcbBairroZonaLeste, ref listaBairros, ref listaZona, "Zona Leste");
                BuscarBairro(rcbBairroZonaSul, ref listaBairros, ref listaZona, "Zona Sul");
                BuscarBairro(rcbBairroZonaOeste, ref listaBairros, ref listaZona, "Zona Oeste");
                BuscarBairro(rcbBairroZonaNorte, ref listaBairros, ref listaZona, "Zona Norte");

                var limite = Convert.ToInt32(Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ValorMaximoBairrosPesquisaCurriculo));
                if (listaBairros.Count > limite)
                {
                    ExibirMensagem(string.Format("Selecione uma zona inteira ou no máximo {0} bairros.", limite), TipoMensagem.Erro);
                    return null;
                }

                if (listaZona.Count > 0)
                    objPesquisaCurriculo.DescricaoZona = listaZona.Aggregate((current, next) => current + "," + next);

                if (listaBairros.Count > 0)
                    objPesquisaCurriculo.DescricaoBairro = listaBairros.Aggregate((current, next) => current + "," + next);
            }
            else
                objPesquisaCurriculo.DescricaoBairro = txtBairro.Valor;

            if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                objPesquisaCurriculo.UsuarioFilialPerfil = new UsuarioFilialPerfil(IdUsuarioFilialPerfilLogadoEmpresa.Value);

            if (IdCurriculo.HasValue)
                objPesquisaCurriculo.Curriculo = new Curriculo(IdCurriculo.Value);

            Cidade objCidade;
            if (Cidade.CarregarPorNome(txtCidadePesquisa.Text, out objCidade))
            {
                objPesquisaCurriculo.Cidade = objCidade;
                if (objCidade.GeoLocalizacao != null && objCidade.GeoLocalizacao != SqlGeography.Null)
                    objPesquisaCurriculo.GeoBuscaCidade = objCidade.GeoLocalizacao;
                else
                {
                    var resultado = GeocodeService.RecuperarCoordenada(string.Empty, string.Empty, string.Empty, string.Empty, objCidade.NomeCidade, objCidade.Estado.SiglaEstado, GeocodeService.Provider.Google);
                    if (resultado != null)
                    {
                        objCidade.GeoLocalizacao = SqlGeography.Point(resultado.Latitude, resultado.Longitude, 4326);
                        objPesquisaCurriculo.GeoBuscaCidade = objCidade.GeoLocalizacao;
                        objCidade.SalvarGeolocalizacao();
                    }
                }

                if (!string.IsNullOrWhiteSpace(txtBairro.Valor))
                {
                    var lista = txtBairro.Valor.Split(',').Where(b => !string.IsNullOrWhiteSpace(b)).ToList();
                    if (lista.Count == 1)
                    {
                        Bairro objBairro;
                        if (Bairro.CarregarPorNomeCidade(lista[0], objCidade, out objBairro))
                        {
                            if (objBairro.GeoLocalizacaoBairro != null && objBairro.GeoLocalizacaoBairro != SqlGeography.Null)
                                objPesquisaCurriculo.GeoBuscaBairro = objBairro.GeoLocalizacaoBairro;
                            else
                            {
                                var resultado = GeocodeService.RecuperarCoordenada(string.Empty, string.Empty, string.Empty, lista[0], objCidade.NomeCidade, objCidade.Estado.SiglaEstado, GeocodeService.Provider.Google);
                                if (resultado != null)
                                {
                                    objBairro.GeoLocalizacaoBairro = SqlGeography.Point(resultado.Latitude, resultado.Longitude, 4326);
                                    objPesquisaCurriculo.GeoBuscaBairro = objBairro.GeoLocalizacaoBairro;
                                    objBairro.Save();
                                }
                            }
                        }

                        objPesquisaCurriculo.DescricaoBairro = txtBairro.Valor;
                    }
                }
            }

            objPesquisaCurriculo.DescricaoPalavraChave = txtPalavraChave.Valor;
            objPesquisaCurriculo.DescricaoFiltroExcludente = txtExcluirPalavraChave.Valor;

            if (!rcbEstado.SelectedValue.Equals("0"))
                objPesquisaCurriculo.Estado = new Estado(rcbEstado.SelectedValue);

            if (!rcbNivelEscolaridade.SelectedValue.Equals("0"))
            {
                var escolaridade = Escolaridade.LoadObject(Convert.ToInt32(rcbNivelEscolaridade.SelectedValue));
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

            if (!string.IsNullOrEmpty(txtIdadeDe.Valor))
                objPesquisaCurriculo.NumeroIdadeMin = Convert.ToInt16(txtIdadeDe.Valor);

            if (!string.IsNullOrEmpty(txtIdadeAte.Valor))
                objPesquisaCurriculo.NumeroIdadeMax = Convert.ToInt16(txtIdadeAte.Valor);

            objPesquisaCurriculo.NumeroSalarioMin = txtSalario.Valor;
            objPesquisaCurriculo.NumeroSalarioMax = txtSalarioAte.Valor;

            //if (!String.IsNullOrEmpty(txtTempoExperiencia.Valor))
            //    objPesquisaCurriculo.QuantidadeExperiencia = Convert.ToInt64(txtTempoExperiencia.Valor);

            if (txtNomeCpfCodigo.Valor.Contains(";"))
                objPesquisaCurriculo.DescricaoCodCPFNome = txtNomeCpfCodigo.Valor.Replace(";", ",");
            else
                objPesquisaCurriculo.DescricaoCodCPFNome = txtNomeCpfCodigo.Valor;

            if (!rcbEstadoCivil.SelectedValue.Equals("0"))
                objPesquisaCurriculo.EstadoCivil = EstadoCivil.LoadObject(Convert.ToInt32(rcbEstadoCivil.SelectedValue));

            objPesquisaCurriculo.DescricaoLogradouro = txtLogradouro.Valor;
            objPesquisaCurriculo.NumeroCEPMin = txtFaixaCep.Valor;
            objPesquisaCurriculo.NumeroCEPMax = txtFaixaCepAte.Valor;

            //Fonte e curso
            if (!string.IsNullOrEmpty(txtInstituicaoTecnicoGraduacao.Text))
            {
                Fonte objFonte;
                if (Fonte.CarregarPorSiglaNome(txtInstituicaoTecnicoGraduacao.Text, out objFonte))
                    objPesquisaCurriculo.FonteTecnicoGraduacao = objFonte;
            }

            if (!string.IsNullOrEmpty(txtTituloTecnicoGraduacao.Text))
            {
                Curso objCurso;
                if (Curso.CarregarPorNome(txtTituloTecnicoGraduacao.Text, out objCurso))
                    objPesquisaCurriculo.CursoTecnicoGraduacao = objCurso;
                else
                {
                    objPesquisaCurriculo.DescricaoCursoTecnicoGraduacao = txtTituloTecnicoGraduacao.Text;
                }
            }

            if (!string.IsNullOrEmpty(txtInstituicao.Text))
            {
                Fonte objFonte;
                if (Fonte.CarregarPorSiglaNome(txtInstituicao.Text, out objFonte))
                    objPesquisaCurriculo.FonteOutrosCursos = objFonte;
            }

            if (!string.IsNullOrEmpty(txtTituloCurso.Text))
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

            //Verifica a Fonte de dados em caso de STC
            objPesquisaCurriculo.Origem = null;
            if (STC.HasValue)
            {
                if (STC.Value)
                {
                    if (chk_stc_fonte_bne.Checked && !chk_stc_fonte_meus_cv.Checked)
                        objPesquisaCurriculo.Origem = new BLL.Origem((int)Origem.BNE);
                    else if (!chk_stc_fonte_bne.Checked && chk_stc_fonte_meus_cv.Checked)
                        objPesquisaCurriculo.Origem = new BLL.Origem(IdOrigem.Value);
                }
            }
            //-----------------

            objPesquisaCurriculo.DescricaoAvaliacao = TxtComentario.Text.Trim();

            objPesquisaCurriculo.Salvar(listaFuncoes, listaIdioma, listaDisponibilidade, listaCampoBuscaPalavraChave, ucEstagiarioFuncao.CheckBoxEstagiarioSelecionado, ucEstagiarioFuncao.CheckBoxAprendizSelecionado);

            objPesquisaCurriculo.SalvarAvaliacoes();

            NoficacaoDePesquisa(objPesquisaCurriculo, listaIdioma, listaDisponibilidade, ucEstagiarioFuncao.CheckBoxEstagiarioSelecionado);

            return objPesquisaCurriculo;
        }

        private void BuscarBairro(ComboCheckbox comboBox, ref List<string> listaBairros, ref List<string> listaZona, string zona)
        {
            if (comboBox.GetCheckedItems().Count == comboBox.Items.Count)
                listaZona.Add(zona);
            else
                listaBairros.AddRange(comboBox.GetCheckedItems().Select(ci => ci.Text));
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

            ucEstagiarioFuncao.AtualizarValidationGroup(btnBuscar.ValidationGroup);
            // upTxtFuncao.Update();

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
            txtCidadePesquisa.Attributes["onBlur"] += "PesquisaCurriculoAvancadaCidadeOnBlur(this)";

            txtSalarioAte.MensagemErroIntervalo = string.Format(MensagemAviso._304501, "Máximo", "maior", "Mínimo");
            cvFuncaoFaixaCepAte.ErrorMessage = string.Format(MensagemAviso._304501, "Máximo", "maior", "Mínimo");

            CarregarParametros();

            InicializarBarraBusca(TipoBuscaMaster.Curriculo, false);

            LimparCampos();

            if (IdPesquisaCurriculoAvancado.HasValue)
                PreencherCampos();
            else
            {
                LimparFuncaoGrid();
                VerificaRedirecionamento();
            }

            //Mosta o painel de fontes apenas na versão STC
            pnl_stc_fonte.Visible = STC.HasValue && STC.Value;
        }

        private void VerificaRedirecionamento()
        {
            if (!PesquisCurriculoPadrao.HasValue || PesquisCurriculoPadrao.Value == null)
                return;

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
                    txtPalavraChave.Valor = pesquisaCurriculo.PalavraChave;
                }
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
                var parametros = new List<BLL.Enumeradores.Parametro>
                {
                    BLL.Enumeradores.Parametro.IntervaloTempoAutoComplete,
                    BLL.Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteFuncao,
                    BLL.Enumeradores.Parametro.NumeroResultadosAutoCompleteFuncao,
                    BLL.Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteCidade,
                    BLL.Enumeradores.Parametro.NumeroResultadosAutoCompleteCidade,
                    BLL.Enumeradores.Parametro.IdadeMinima,
                    BLL.Enumeradores.Parametro.IdadeMaxima,
                    BLL.Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteEstado,
                    BLL.Enumeradores.Parametro.NumeroResultadosAutoCompleteEstado,
                    BLL.Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteNomeCurso,
                    BLL.Enumeradores.Parametro.NumeroResultadosAutoCompleteNomeCurso,
                    BLL.Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteNomeInstituicao,
                    BLL.Enumeradores.Parametro.NumeroResultadosAutoCompleteNomeInstituicao
                };

                var valoresParametros = Parametro.ListarParametros(parametros);

                //aceCidade.CompletionInterval = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                //aceCidade.CompletionSetCount = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroResultadosAutoCompleteCidade]);
                //aceCidade.MinimumPrefixLength = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteCidade]);

                txtIdadeDe.ValorMinimo = txtIdadeAte.ValorMinimo = valoresParametros[BLL.Enumeradores.Parametro.IdadeMinima];
                txtIdadeDe.ValorMaximo = txtIdadeAte.ValorMaximo = valoresParametros[BLL.Enumeradores.Parametro.IdadeMaxima];
                txtIdadeDe.MensagemErroIntervalo = txtIdadeAte.MensagemErroIntervalo = string.Format(MensagemAviso._304502, valoresParametros[BLL.Enumeradores.Parametro.IdadeMinima], valoresParametros[BLL.Enumeradores.Parametro.IdadeMaxima]);

                aceInstituicaoTecnicoGraduacao.CompletionInterval = Convert.ToInt32(valoresParametros[BLL.Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceInstituicaoTecnicoGraduacao.CompletionSetCount = Convert.ToInt32(valoresParametros[BLL.Enumeradores.Parametro.NumeroResultadosAutoCompleteNomeInstituicao]);
                aceInstituicaoTecnicoGraduacao.MinimumPrefixLength = Convert.ToInt32(valoresParametros[BLL.Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteNomeInstituicao]);

                aceTituloTecnicoGraduacao.CompletionInterval = Convert.ToInt32(valoresParametros[BLL.Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceTituloTecnicoGraduacao.CompletionSetCount = Convert.ToInt32(valoresParametros[BLL.Enumeradores.Parametro.NumeroResultadosAutoCompleteNomeCurso]);
                aceTituloTecnicoGraduacao.MinimumPrefixLength = Convert.ToInt32(valoresParametros[BLL.Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteNomeCurso]);

                aceInstituicaoOutrosCursos.CompletionInterval = Convert.ToInt32(valoresParametros[BLL.Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceInstituicaoOutrosCursos.CompletionSetCount = Convert.ToInt32(valoresParametros[BLL.Enumeradores.Parametro.NumeroResultadosAutoCompleteNomeInstituicao]);
                aceInstituicaoOutrosCursos.MinimumPrefixLength = Convert.ToInt32(valoresParametros[BLL.Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteNomeInstituicao]);

                aceInstituicaoOutrosCursos.CompletionInterval = Convert.ToInt32(valoresParametros[BLL.Enumeradores.Parametro.IntervaloTempoAutoComplete]);
                aceInstituicaoOutrosCursos.CompletionSetCount = Convert.ToInt32(valoresParametros[BLL.Enumeradores.Parametro.NumeroResultadosAutoCompleteNomeCurso]);
                aceInstituicaoOutrosCursos.MinimumPrefixLength = Convert.ToInt32(valoresParametros[BLL.Enumeradores.Parametro.NumeroLetrasInicioAutoCompleteNomeCurso]);
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region AjustarPermissoes
        /// <summary>
        ///     Método responsável por ajustar as permissões da tela de acordo com o susuário filial perfil logado.
        /// </summary>
        private void AjustarPermissoes()
        {
            int? idUsuarioFilialPerfil = null;

            if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                idUsuarioFilialPerfil = IdUsuarioFilialPerfilLogadoEmpresa.Value;
            else if (IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
                idUsuarioFilialPerfil = IdUsuarioFilialPerfilLogadoUsuarioInterno.Value;

            if (idUsuarioFilialPerfil.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(idUsuarioFilialPerfil.Value, CategoriaPermissao.PesquisaAvancadaCurriculo);

                if (!Permissoes.Contains((int)BLL.Enumeradores.Permissoes.PesquisaAvancadaCurriculo.AcessarTelaPesquisaAvancadaCurriculo))
                {
                    Session.Add(Chave.Temporaria.MensagemPermissao.ToString(), MensagemAviso._300034);
                    Redirect(Configuracao.UrlAvisoAcessoNegado);
                }
            }
            else
            {

                divFiltrosPessoais.Visible = false;
                divFiltrosPessoaisAviso.Visible = true;
                divFiltrosCandidato.Visible = false;
                divFiltrosCandidatoAviso.Visible = true;
                divAvaliacao.Visible = false;
                divAvaliacaoAviso.Visible = true;
            }
        }
        #endregion

        #region LimparCampos
        private void LimparCampos()
        {
            ucPesquisaIdioma.LimparCampos();
            ucEstagiarioFuncao.LimparCampos();
            txtCidadePesquisa.Text =
                txtPalavraChave.Valor =
                    txtIdadeDe.Valor =
                        txtIdadeAte.Valor =
                //txtTempoExperiencia.Valor =
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
                                                                                txtEmail.Text =
                                                                                    TxtComentario.Text = string.Empty;

            rcbBairroZonaCentral.ClearCheckeds();
            rcbBairroZonaSul.ClearCheckeds();
            rcbBairroZonaOeste.ClearCheckeds();
            rcbBairroZonaLeste.ClearCheckeds();
            rcbBairroZonaNorte.ClearCheckeds();
            rcbBuscaPalavraChave.ClearCheckeds();

            CheckAvaliacaoPositiva.Checked =
                CheckAvaliacaoNegativa.Checked =
                    CheckSemAvaliacao.Checked = false;

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

            upTxtEmpresa.Update();
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos()
        {
            try
            {
                if (IdPesquisaCurriculoAvancado != null)
                {
                    var objPesquisaCurriculo = BLL.PesquisaCurriculo.LoadObject(IdPesquisaCurriculoAvancado.Value);

                    if (objPesquisaCurriculo == null)
                        return;

                    #region Região e Localidade
                    if (objPesquisaCurriculo.Cidade != null)
                    {
                        objPesquisaCurriculo.Cidade.CompleteObject();
                        txtCidadePesquisa.Text = string.Format("{0}/{1}", objPesquisaCurriculo.Cidade.NomeCidade, objPesquisaCurriculo.Cidade.Estado.SiglaEstado);
                        RecuperarBairros(objPesquisaCurriculo.Cidade.NomeCidade, objPesquisaCurriculo.Cidade.Estado.SiglaEstado);
                    }
                    if (objPesquisaCurriculo.Estado != null)
                        rcbEstado.SelectedValue = objPesquisaCurriculo.Estado.SiglaEstado;

                    txtBairro.Valor = objPesquisaCurriculo.DescricaoBairro;
                    txtLogradouro.Valor = objPesquisaCurriculo.DescricaoLogradouro;
                    txtFaixaCep.Valor = objPesquisaCurriculo.NumeroCEPMin;
                    txtFaixaCepAte.Valor = objPesquisaCurriculo.NumeroCEPMax;
                    #endregion

                    #region Definir Palavra Chave
                    txtPalavraChave.Valor = objPesquisaCurriculo.DescricaoPalavraChave;
                    txtExcluirPalavraChave.Valor = objPesquisaCurriculo.DescricaoFiltroExcludente;
                    var listaPalavaraChave = CampoPalavraChavePesquisaCurriculo.ListarPalavraChavePorPesquisa(objPesquisaCurriculo);
                    foreach (var item in listaPalavaraChave)
                        rcbBuscaPalavraChave.SetItemChecked(item.ToString(CultureInfo.CurrentCulture), true);
                    #endregion

                    #region Formação e Escolaridade
                    if (objPesquisaCurriculo.Escolaridade != null)
                        rcbNivelEscolaridadeEstagiario.SelectedValue = rcbNivelEscolaridade.SelectedValue = objPesquisaCurriculo.Escolaridade.IdEscolaridade.ToString();

                    if (objPesquisaCurriculo.CursoTecnicoGraduacao != null)
                    {
                        objPesquisaCurriculo.CursoTecnicoGraduacao.CompleteObject();
                        txtTituloTecnicoGraduacaoEstag.Text = txtTituloTecnicoGraduacao.Text = objPesquisaCurriculo.CursoTecnicoGraduacao.DescricaoCurso;
                    }
                    else if (!string.IsNullOrEmpty(objPesquisaCurriculo.DescricaoCursoTecnicoGraduacao))
                        txtTituloTecnicoGraduacao.Text = objPesquisaCurriculo.DescricaoCursoTecnicoGraduacao;

                    if (objPesquisaCurriculo.CursoOutrosCursos != null)
                    {
                        objPesquisaCurriculo.CursoOutrosCursos.CompleteObject();
                        txtTituloCurso.Text = objPesquisaCurriculo.CursoOutrosCursos.DescricaoCurso;
                    }
                    else if (!string.IsNullOrEmpty(objPesquisaCurriculo.DescricaoCursoOutrosCursos))
                        txtTituloCurso.Text = objPesquisaCurriculo.DescricaoCursoOutrosCursos;

                    //Identifica os itens selecionados na dropdown de idioma.
                    var listaIdioma = PesquisaCurriculoIdioma.ListarIdiomaPorPesquisa(objPesquisaCurriculo);
                    //Carregar idiomas
                    ucPesquisaIdioma.SetIdiomas(listaIdioma);

                    if (objPesquisaCurriculo.FonteTecnicoGraduacao != null)
                    {
                        objPesquisaCurriculo.FonteTecnicoGraduacao.CompleteObject();
                        txtInstituicaoTecnicoGraduacaoEstag.Text = txtInstituicaoTecnicoGraduacao.Text = string.Format("{0} - {1}", objPesquisaCurriculo.FonteTecnicoGraduacao.SiglaFonte, objPesquisaCurriculo.FonteTecnicoGraduacao.NomeFonte);
                    }

                    if (objPesquisaCurriculo.FonteOutrosCursos != null)
                    {
                        objPesquisaCurriculo.FonteOutrosCursos.CompleteObject();
                        txtInstituicao.Text = string.Format("{0} - {1}", objPesquisaCurriculo.FonteOutrosCursos.SiglaFonte, objPesquisaCurriculo.FonteOutrosCursos.NomeFonte);
                    }
                    #endregion

                    #region Experiência do Candidato
                    txtEmpresa.Valor = objPesquisaCurriculo.RazaoSocial;

                    if (objPesquisaCurriculo.AreaBNE != null)
                        rcbAtividadeEmpresa.SelectedValue = objPesquisaCurriculo.AreaBNE.IdAreaBNE.ToString(CultureInfo.CurrentCulture);
                    #endregion

                    #region Características
                    if (objPesquisaCurriculo.NumeroIdadeMin.HasValue)
                        txtIdadeDe.Valor = objPesquisaCurriculo.NumeroIdadeMin.Value.ToString(CultureInfo.CurrentCulture);

                    if (objPesquisaCurriculo.NumeroIdadeMax.HasValue)
                        txtIdadeAte.Valor = objPesquisaCurriculo.NumeroIdadeMax.Value.ToString(CultureInfo.CurrentCulture);

                    if (objPesquisaCurriculo.Raca != null)
                        rcbRaca.SelectedValue = objPesquisaCurriculo.Raca.IdRaca.ToString(CultureInfo.CurrentCulture);

                    if (objPesquisaCurriculo.CategoriaHabilitacao != null)
                        rcbHabilitacao.SelectedValue = objPesquisaCurriculo.CategoriaHabilitacao.IdCategoriaHabilitacao.ToString(CultureInfo.CurrentCulture);

                    if (objPesquisaCurriculo.Sexo != null)
                        rcbSexo.SelectedValue = objPesquisaCurriculo.Sexo.IdSexo.ToString(CultureInfo.CurrentCulture);

                    if (objPesquisaCurriculo.FlagFilhos.HasValue)
                        rcbFilhos.SelectedValue = objPesquisaCurriculo.FlagFilhos.Value ? "1" : "0";

                    if (objPesquisaCurriculo.TipoVeiculo != null)
                        rcbVeiculo.SelectedValue = objPesquisaCurriculo.TipoVeiculo.IdTipoVeiculo.ToString(CultureInfo.CurrentCulture);

                    if (objPesquisaCurriculo.EstadoCivil != null)
                        rcbEstadoCivil.SelectedValue = objPesquisaCurriculo.EstadoCivil.IdEstadoCivil.ToString(CultureInfo.CurrentCulture);

                    if (objPesquisaCurriculo.Deficiencia != null)
                        rcbPCD.SelectedValue = objPesquisaCurriculo.Deficiencia.IdDeficiencia.ToString(CultureInfo.CurrentCulture);
                    #endregion

                    #region Candidato Específico
                    txtNomeCpfCodigo.Valor = objPesquisaCurriculo.DescricaoCodCPFNome;
                    txtTelefone.DDD = objPesquisaCurriculo.NumeroDDDTelefone;
                    txtTelefone.Fone = objPesquisaCurriculo.NumeroTelefone;
                    txtEmail.Text = objPesquisaCurriculo.EmailPessoa;
                    #endregion

                    #region Avaliação do Candidato
                    TxtComentario.Text = objPesquisaCurriculo.DescricaoAvaliacao;

                    foreach (var item in objPesquisaCurriculo.Avaliacoes)
                    {
                        switch (item.Avaliacao.IdAvaliacao)
                        {
                            case ((int)CurriculoClassificacao.AvaliacaoPositiva):
                                CheckAvaliacaoPositiva.Checked = true;
                                break;
                            case ((int)CurriculoClassificacao.AvaliacaoNeutra):
                                CheckSemAvaliacao.Checked = true;
                                break;
                            case ((int)CurriculoClassificacao.AvaliacaoNegativa):
                                CheckAvaliacaoNegativa.Checked = true;
                                break;
                        }
                    }


                    //  CheckAvaliacaoPositiva.Checked = objPesquisaCurriculo.Avaliacoes
                    #endregion

                    #region Pretensão do candidato
                    ucEstagiarioFuncao.SetFuncoes(PesquisaCurriculoFuncao.ListarIdentificadoresFuncaoPorPesquisa(objPesquisaCurriculo).Select(Funcao.LoadObject).ToList());

                    //Identifica os itens selecionado na dropdown de disponibilidade.
                    var listaDisponibilidade = PesquisaCurriculoDisponibilidade.ListarIdentificadoresDisponibilidadePorPesquisa(objPesquisaCurriculo);
                    foreach (var item in listaDisponibilidade)
                        rcbDisponibilidade.SetItemChecked(item.ToString(CultureInfo.CurrentCulture), true);


                    var paramResult = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.IdfsEscolaridadeWebEstagiosQueroEstagiario);

                    if (string.Equals(paramResult, objPesquisaCurriculo.IdEscolaridadeWebStagio, StringComparison.OrdinalIgnoreCase))
                        ucEstagiarioFuncao.SetEstagiario(true);


                    if (objPesquisaCurriculo.Escolaridade != null)
                        rcbNivelEscolaridadeEstagiario.SelectedValue = objPesquisaCurriculo.Escolaridade.IdEscolaridade.ToString(CultureInfo.CurrentCulture);

                    txtSalario.Valor = objPesquisaCurriculo.NumeroSalarioMin;
                    txtSalarioAte.Valor = objPesquisaCurriculo.NumeroSalarioMax;
                    #endregion

                    txtEmpresa.Valor = objPesquisaCurriculo.RazaoSocial;

                    //if (objPesquisaCurriculo.QuantidadeExperiencia.HasValue)
                    //    txtTempoExperiencia.Valor = objPesquisaCurriculo.QuantidadeExperiencia.ToString();

                    // [Obsolete("Função inativada")]
                    // Código comentado é relacionado  a função TipoVínculo que existia
                    //Identifica os itens, selecionando na checkboxlist de tipo vinculo.
                    //List<PesquisaCurriculoTipoVinculo> listPesquisaCurriculoTipoVinculo = PesquisaCurriculoTipoVinculo.ListarPorPesquisaList(objPesquisaCurriculo.IdPesquisaCurriculo);
                    //foreach (var pesquisaCurriculoTipoVinculo in listPesquisaCurriculoTipoVinculo)
                    //{
                    //    ucContratoFuncao.SetTipoVinculoChecked(
                    //        pesquisaCurriculoTipoVinculo.TipoVinculo.IdTipoVinculo.ToString(CultureInfo.CurrentCulture), true);
                    //}

                    if (pnlBairroZona.Visible)
                    {
                        if (!string.IsNullOrWhiteSpace(objPesquisaCurriculo.DescricaoBairro))
                        {
                            var bairros = objPesquisaCurriculo.DescricaoBairro.Split(',');
                            PreencherCamposCheckbox(rcbBairroZonaCentral, bairros);
                            PreencherCamposCheckbox(rcbBairroZonaLeste, bairros);
                            PreencherCamposCheckbox(rcbBairroZonaOeste, bairros);
                            PreencherCamposCheckbox(rcbBairroZonaNorte, bairros);
                            PreencherCamposCheckbox(rcbBairroZonaSul, bairros);
                        }
                        if (!string.IsNullOrWhiteSpace(objPesquisaCurriculo.DescricaoZona))
                        {
                            var zonas = objPesquisaCurriculo.DescricaoZona.Split(',');
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
                        txtBairro.Valor = objPesquisaCurriculo.DescricaoBairro;

                    if (STC.HasValue && STC.Value)
                    {
                        if (objPesquisaCurriculo.Origem != null)
                        {
                            if (objPesquisaCurriculo.Origem.IdOrigem == (int)Origem.BNE)
                            {
                                chk_stc_fonte_bne.Checked = true;
                                chk_stc_fonte_meus_cv.Checked = false;
                            }
                            else if (objPesquisaCurriculo.Origem.IdOrigem == IdOrigem.Value)
                            {
                                chk_stc_fonte_bne.Checked = false;
                                chk_stc_fonte_meus_cv.Checked = true;
                            }
                        }
                    }
                }
                upTxtEmpresa.Update();
            }
            catch (Exception ex)
            {
                GerenciadorException.GravarExcecao(ex, IdPesquisaCurriculoAvancado.Value.ToString());
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

        #region LimparFuncaoGrid
        private void LimparFuncaoGrid()
        {
            ucEstagiarioFuncao.InicializarDataTableFuncoes();
        }
        #endregion

        #region RecuperarBairros
        private void RecuperarBairros(string cidade, string estado)
        {
            var url = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.UrlAPICEP) + "/api/bairro/getbycidade/{cidade}/{estado}".Replace("{cidade}", cidade).Replace("{estado}", estado);
            // Create an HttpClient instance 
            using (var client = new HttpClient())
            {
                // Send a request asynchronously continue when complete 
                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseData = response.Content.ReadAsAsync<dynamic>().Result;

                    var jsonObject = responseData as JObject;

                    if (jsonObject != null)
                    {
                        var centro = jsonObject["Centro"] as JArray;
                        if (centro != null)
                            UIHelper.CarregarRadComboBox(rcbBairroZonaCentral, centro.ToDictionary(js => js["ID"].ToString(), js => js["Nome"].ToString()));

                        var norte = jsonObject["Zona Norte"] as JArray;
                        if (norte != null)
                            UIHelper.CarregarRadComboBox(rcbBairroZonaNorte, norte.ToDictionary(js => js["ID"].ToString(), js => js["Nome"].ToString()));

                        var sul = jsonObject["Zona Sul"] as JArray;
                        if (sul != null)
                            UIHelper.CarregarRadComboBox(rcbBairroZonaSul, sul.ToDictionary(js => js["ID"].ToString(), js => js["Nome"].ToString()));

                        var leste = jsonObject["Zona Leste"] as JArray;
                        if (leste != null)
                            UIHelper.CarregarRadComboBox(rcbBairroZonaLeste, leste.ToDictionary(js => js["ID"].ToString(), js => js["Nome"].ToString()));

                        var oeste = jsonObject["Zona Oeste"] as JArray;
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
                    upBairroTexto.Update();
                    upBairroZona.Update();
                }
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

            Utility.RegisterTypeForAjax(typeof(PesquisaCurriculoAvancada));
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
            if (!string.IsNullOrEmpty(txtCidadePesquisa.Text))
            {
                var valor = txtCidadePesquisa.Text;
                var valorTeste = txtBairro.Valor;
                Cidade objCidade;

                if (!string.IsNullOrEmpty(valorTeste))
                {
                    if (Cidade.CarregarPorNome(valor, out objCidade))
                    {
                        var strNomeCidade = UIHelper.RemoverAcentos(valorTeste) + " " + UIHelper.RemoverAcentos(objCidade.NomeCidade) + " " + objCidade.Estado.SiglaEstado;
                        var url = "BuscarBairro.aspx?pCidade=" + strNomeCidade;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "bairro", "window.open('" + url + "', 'PopUpWindow', 'toolbar=0,scrollbars=0,location=0,statusbar=0,menubar=0,resizable=0,width=510,height=450');", true);
                    }
                }

                if (Cidade.CarregarPorNome(valor, out objCidade))
                {
                    var strNomeCidade = UIHelper.RemoverAcentos(objCidade.NomeCidade) + " " + objCidade.Estado.SiglaEstado;
                    var url = "BuscarBairro.aspx?pCidade=" + strNomeCidade;
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
                var cidadeEstado = txtCidadePesquisa.Text.Split('/');
                if (cidadeEstado.Length == 2)
                {
                    var objEstado = Estado.CarregarPorSiglaEstado(cidadeEstado[1]);
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

            upPnlCamposPesquisa.Update();
            txtLogradouro.Focus();
        }
        #endregion

        #region btiLimparFlutuante_Click
        protected void btiLimparFlutuante_Click(object sender, EventArgs e)
        {
            LimparFuncaoGrid();
            LimparCampos();
            upPnlCamposPesquisa.Update();
        }
        #endregion

        #region btnBuscar_Click
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                var cookie = RecuperarValorCookie(Cookie.AnunciarVagaPesquisaCurriculo);
                if (string.IsNullOrWhiteSpace(cookie) && ucEstagiarioFuncao.Funcoes.Rows.Count == 1 && Cidade.Existe(txtCidadePesquisa.Text.Trim()))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AbrirModalAnuncioVaga", "AbrirModalAnuncioVaga();", true);
                }
                else
                {
                    var objPesquisaCurriculo = Buscar();

                    if (objPesquisaCurriculo != null)
                    {
                        Session.Add(Chave.Temporaria.ViewStateObject_ResultadoPesquisaCurriculo.ToString(), new ResultadoPesquisaCurriculo(new ResultadoPesquisaCurriculoCurriculo { IdPesquisaCurriculo = objPesquisaCurriculo.IdPesquisaCurriculo }));
                        Redirect("/lista-de-curriculos/" + objPesquisaCurriculo.IdPesquisaCurriculo);
                        //Redirect(GetRouteUrl(RouteCollection.PesquisaCurriculoFiltro.ToString(), new { IdPesquisaCurriculoAvancado = objPesquisaCurriculo.IdPesquisaCurriculo }));
                    }
                }
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
            LimparFuncaoGrid();
            LimparCampos();
            upPnlCamposPesquisa.Update();
        }
        #endregion

        #region btlAnunciarVagaNao_Click
        protected void btlAnunciarVagaNao_Click(object sender, EventArgs e)
        {
            try
            {
                GravarValorCookie(Cookie.AnunciarVagaPesquisaCurriculo, string.Empty, ckbAnunciarVagaNaoPerguntarNovamente.Checked ? "não" : string.Empty);
                var objPesquisaCurriculo = Buscar();

                if (objPesquisaCurriculo != null)
                {
                    Session.Add(Chave.Temporaria.ViewStateObject_ResultadoPesquisaCurriculo.ToString(), new ResultadoPesquisaCurriculo(new ResultadoPesquisaCurriculoCurriculo { IdPesquisaCurriculo = objPesquisaCurriculo.IdPesquisaCurriculo }));
                    Redirect("/lista-de-curriculos/" + objPesquisaCurriculo.IdPesquisaCurriculo);
                }
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btlAnunciarVagaSim_Click
        protected void btlAnunciarVagaSim_Click(object sender, EventArgs e)
        {
            try
            {
                var objPesquisaCurriculo = Buscar();

                if (objPesquisaCurriculo != null)
                {
                    try
                    {
                        GravarValorCookie(Cookie.AnunciarVagaPesquisaCurriculo, string.Empty, string.Empty);

                        var objVaga = new Vaga { FlagAuditada = false, QuantidadeVaga = 1 };

                        //Funcao e atribuição
                        objVaga.Funcao = ucEstagiarioFuncao.Funcoes.AsEnumerable().Select(item => Funcao.LoadObject(Convert.ToInt32(item["IdFuncao"]))).First();
                        objVaga.DescricaoAtribuicoes = objVaga.Funcao.DescricaoJob;

                        //Cidade
                        Cidade objCidade;
                        if (Cidade.CarregarPorNome(txtCidadePesquisa.Text, out objCidade))
                        {
                            objVaga.Cidade = objCidade;
                        }

                        //Escolaridade
                        if (!rcbNivelEscolaridade.SelectedValue.Equals("0"))
                        {
                            var escolaridade = Escolaridade.LoadObject(Convert.ToInt32(rcbNivelEscolaridade.SelectedValue));
                            if (escolaridade != null)
                                objVaga.Escolaridade = new Escolaridade(escolaridade.IdEscolaridade) { DescricaoBNE = escolaridade.DescricaoBNE };
                        }

                        //Salario
                        objVaga.ValorSalarioDe = txtSalario.Valor;
                        objVaga.ValorSalarioPara = txtSalarioAte.Valor;

                        //Idade
                        if (!string.IsNullOrEmpty(txtIdadeDe.Valor))
                            objVaga.NumeroIdadeMinima = Convert.ToInt16(txtIdadeDe.Valor);

                        if (!string.IsNullOrEmpty(txtIdadeAte.Valor))
                            objVaga.NumeroIdadeMinima = Convert.ToInt16(txtIdadeAte.Valor);

                        //Sexo
                        if (!rcbSexo.SelectedValue.Equals("0"))
                        {
                            var sexo = Sexo.LoadObject(Convert.ToInt32(rcbSexo.SelectedValue));
                            if (sexo != null)
                                objVaga.Sexo = new Sexo(sexo.IdSexo);
                        }

                        //Deficiencia
                        if (!rcbPCD.SelectedValue.Equals("-1"))
                            objVaga.Deficiencia = new Deficiencia(Convert.ToInt32(rcbPCD.SelectedValue));

                        //Disponibilidades
                        //Identifica os itens selecionado na dropdown de disponibilidade.
                        var listaDisponibilidade = rcbDisponibilidade.GetCheckedItems().Select(item => new VagaDisponibilidade
                        {
                            Disponibilidade = new Disponibilidade(Convert.ToInt32(item.Value)),
                            Vaga = objVaga
                        }).ToList();

                        //Bairro
                        if (pnlBairroZona.Visible)
                        {
                            var listaBairros = new List<string>();
                            var listaZona = new List<string>();

                            BuscarBairro(rcbBairroZonaCentral, ref listaBairros, ref listaZona, "Centro");
                            BuscarBairro(rcbBairroZonaLeste, ref listaBairros, ref listaZona, "Zona Leste");
                            BuscarBairro(rcbBairroZonaSul, ref listaBairros, ref listaZona, "Zona Sul");
                            BuscarBairro(rcbBairroZonaOeste, ref listaBairros, ref listaZona, "Zona Oeste");
                            BuscarBairro(rcbBairroZonaNorte, ref listaBairros, ref listaZona, "Zona Norte");

                            if (listaBairros.Count > 0)
                                objVaga.NomeBairro = listaBairros.Aggregate((current, next) => current + "," + next);
                        }
                        else
                            objVaga.NomeBairro = txtBairro.Valor;

                        //Dados anunciante
                        objVaga.Filial = new Filial(base.IdFilial.Value);
                        objVaga.UsuarioFilialPerfil = new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
                        UsuarioFilial objUsuarioFilial;
                        if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value, out objUsuarioFilial))
                            objVaga.EmailVaga = objUsuarioFilial.EmailComercial;

                        objVaga.Origem = new BLL.Origem((int)Origem.BNE);
                        objVaga.FlagReceberTodosCV = true;

                        //Anunciada com a origem da pesquisa
                        objVaga.OrigemAnuncioVaga = new BLL.OrigemAnuncioVaga((int)BLL.Enumeradores.OrigemAnuncioVaga.PesquisaCurriculo);

                        //Calculando a data de abertura da vaga
                        objVaga.CalcularAberturaEncerramento();

                        var listaVagaTipoVinculo = new List<VagaTipoVinculo> { new VagaTipoVinculo { Vaga = objVaga, TipoVinculo = new BLL.TipoVinculo((int)BLL.Enumeradores.TipoVinculo.Efetivo) } };

                        objVaga.SalvarVaga(listaDisponibilidade, listaVagaTipoVinculo, null, false, 
                            base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue ? base.IdUsuarioFilialPerfilLogadoEmpresa.Value : base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value,null);
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                    }

                    Session.Add(Chave.Temporaria.ViewStateObject_ResultadoPesquisaCurriculo.ToString(), new ResultadoPesquisaCurriculo(new ResultadoPesquisaCurriculoCurriculo { IdPesquisaCurriculo = objPesquisaCurriculo.IdPesquisaCurriculo }));
                    Redirect(GetRouteUrl(RouteCollection.PesquisaCurriculo.ToString(), null));
                }
            }
            catch (Exception ex)
            {
                ExibirMensagemErro(ex);
            }
        }
        #endregion

        #endregion

        #region AjaxMethods

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

        #region RecuperarCidade
        /// <summary>
        ///     Recuperar cidade
        /// </summary>
        [AjaxMethod(HttpSessionStateRequirement.Read)]
        public static string RecuperarCidade(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
                return string.Empty;

            Cidade objCidade;
            if (valor.LastIndexOf('/').Equals(-1))
                if (Cidade.CarregarPorNome(valor, out objCidade))
                    return objCidade.NomeCidade + "/" + objCidade.Estado.SiglaEstado;

            return string.Empty;
        }
        #endregion

        #region ValidarFuncao
        /// <summary>
        ///     Validar Funcao
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        [AjaxMethod(HttpSessionStateRequirement.Read)]
        public bool ValidarFuncao(string valor)
        {
            valor = valor.Trim();

            if (string.IsNullOrEmpty(valor))
                return true;

            int? idOrigem = null;
            if (IdOrigem.HasValue)
                idOrigem = IdOrigem.Value;

            return Funcao.ValidarFuncaoPorOrigem(idOrigem, valor);
        }
        #endregion

        #endregion

    }
}