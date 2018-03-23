using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Resources;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;
using PessoaFisicaFoto = BNE.Web.Handlers.PessoaFisicaFoto;

namespace BNE.Web
{
    public partial class CompararCurriculo : BasePage
    {

        #region Propriedades

        #region ListCurriculos - Variável 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public List<int> ListCurriculos
        {
            get
            {
                return (List<int>)(ViewState[Chave.Temporaria.Variavel1.ToString()]);
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel1.ToString()] = value;
            }
        }
        #endregion

        #region DicionarioColunas - Variável 2
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public Dictionary<string, string> DicionarioColunas
        {
            get
            {
                return (Dictionary<string, string>)(ViewState[Chave.Temporaria.Variavel2.ToString()]);
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel2.ToString()] = value;
            }
        }
        #endregion

        #region IdPesquisaCurriculo - Variável 3
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaCurriculo
        /// </summary>
        protected int? IdPesquisaCurriculo
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

        #region CampoOrdernacao - Variável 5
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaCurriculo
        /// </summary>
        protected string CampoOrdernacao
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel5.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel5.ToString()]).ToString();
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel5.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel5.ToString());
            }
        }
        #endregion

        #region TipoOrdernacao - Variável 6
        /// <summary>
        /// Propriedade que armazena e recupera o IdPesquisaCurriculo
        /// </summary>
        protected string TipoOrdernacao
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

        #region IdVagaCompararCurriculo - Variável 7
        /// <summary>
        /// Propriedade que armazena e recupera o IdVaga
        /// </summary>
        protected int? IdVagaCompararCurriculo
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel7.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel7.ToString()].ToString());
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel7.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel7.ToString());
            }
        }
        #endregion

        #region BoolCandidatosNaoVisualizados - Variável 8
        public bool BoolCandidatosNaoVisualizados
        {
            get
            {
                return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel8.ToString()]);
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel8.ToString()] = value;
            }
        }
        #endregion

        #region BoolCandidatosNoPerfil - Variável 9
        public bool BoolCandidatosNoPerfil
        {
            get
            {
                return Convert.ToBoolean(ViewState[Chave.Temporaria.Variavel9.ToString()]);
            }
            set
            {
                ViewState[Chave.Temporaria.Variavel9.ToString()] = value;
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (IdPesquisaCurriculo.HasValue)
                Session.Add(Chave.Temporaria.Variavel6.ToString(), IdPesquisaCurriculo);

            if (IdVagaCompararCurriculo.HasValue)
            {
                Session.Add(Chave.Temporaria.Variavel7.ToString(), IdVagaCompararCurriculo);
                Session.Add(Chave.Temporaria.Variavel8.ToString(), BoolCandidatosNaoVisualizados);
                Session.Add(Chave.Temporaria.Variavel11.ToString(), BoolCandidatosNoPerfil);
            }

            Session.Add(Chave.Temporaria.Variavel1.ToString(), ListCurriculos);

            Redirect(GetRouteUrl(Enumeradores.RouteCollection.PesquisaCurriculo.ToString(), null));
        }
        #endregion

        #region gvCompararCurriculo_ItemCommand
        protected void gvCompararCurriculo_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("Deletar"))
            {
                int idCurriculo = Convert.ToInt32(gvCompararCurriculo.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Curriculo"]);
                ListCurriculos.Remove(idCurriculo);
                CarregarGrid();
            }
        }
        #endregion

        #region gvCompararCurriculo_SortCommand
        protected void gvCompararCurriculo_SortCommand(object source, GridSortCommandEventArgs e)
        {
            CampoOrdernacao = e.SortExpression;

            switch (e.OldSortOrder)
            {
                case GridSortOrder.Ascending: TipoOrdernacao = "ASC";
                    break;
                case GridSortOrder.Descending: TipoOrdernacao = "DESC";
                    break;
                case GridSortOrder.None: TipoOrdernacao = "ASC";
                    break;
            }

            CarregarGrid();
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        private void Inicializar()
        {
            CarregarDicionariodeColunas();
            //Carregando a combo box
            UIHelper.CarregarRadComboBox(ccbColunas, DicionarioColunas);
            AjustarHeader();
            AjustarColunasIniciaisComboBox();
            AjustarColunas();
            AjustarColunasIniciaisComboBox();
            CarregarGrid();

            base.InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "CompararCurriculo");
        }
        #endregion

        #region RetornarUrlFoto
        protected string RetornarUrlFoto(string strCpf)
        {
            return UIHelper.RetornarUrlFoto(strCpf.Trim(), PessoaFisicaFoto.OrigemFoto.Local);
        }
        #endregion

        #region CarregarGrid
        private void CarregarGrid()
        {
            UIHelper.CarregarRadGrid(gvCompararCurriculo, Curriculo.RetornarInformacoesCurriculos(ListCurriculos, CampoOrdernacao, TipoOrdernacao));
        }
        #endregion

        #region AjustarColunas
        private void AjustarColunas()
        {
            foreach (KeyValuePair<string, string> kvp in DicionarioColunas)
            {
                GridColumn gc = gvCompararCurriculo.MasterTableView.GetColumn(kvp.Key);
                gc.Display = false;
            }

            foreach (RadComboBoxItem rcbi in ccbColunas.GetCheckedItems())
            {
                GridColumn gc = gvCompararCurriculo.MasterTableView.GetColumn(rcbi.Value);
                gc.Display = true;
            }
        }
        #endregion

        #region ContarColunasSelecionadas
        private int ContarColunasSelecionadas()
        {
            return ccbColunas.GetCheckedItems().Count;
        }
        #endregion

        #region AjustarColunasIniciaisComboBox
        private void AjustarColunasIniciaisComboBox()
        {
            foreach (RadComboBoxItem rcbi in ccbColunas.Items)
            {
                if (rcbi.Value.Equals("Coluna_nome"))
                    rcbi.Enabled = false;
            }

            ccbColunas.SetItemChecked("Coluna_nome", true);
            ccbColunas.SetItemChecked("Coluna_idade", true);
            ccbColunas.SetItemChecked("Coluna_escolaridade", true);
            ccbColunas.SetItemChecked("Coluna_cidade", true);
            ccbColunas.SetItemChecked("Coluna_pretensao_salarial", true);
            ccbColunas.SetItemChecked("Coluna_ultimo_salario", true);
            ccbColunas.SetItemChecked("Coluna_funcao_pretendida", true);
        }
        #endregion

        #region AjustarHeader
        private void AjustarHeader()
        {
            foreach (KeyValuePair<string, string> kvp in DicionarioColunas)
            {
                gvCompararCurriculo.MasterTableView.GetColumn(kvp.Key).HeaderText = kvp.Value;
            }
        }
        #endregion

        #region CarregarDicionariodeColunas
        private void CarregarDicionariodeColunas()
        {
            DicionarioColunas = new Dictionary<string, string>();
            DicionarioColunas.Add("Coluna_nome", "Nome");
            DicionarioColunas.Add("Coluna_idade", "Idade");
            DicionarioColunas.Add("Coluna_estadocivil", "Estado Civil");
            DicionarioColunas.Add("Coluna_escolaridade", "Escolaridade");
            DicionarioColunas.Add("Coluna_cidade", "Cidade");
            DicionarioColunas.Add("Coluna_bairro", "Bairro");
            DicionarioColunas.Add("Coluna_cnh", "CNH");
            DicionarioColunas.Add("Coluna_veiculo", "Tipo de Veículo");
            DicionarioColunas.Add("Coluna_filho", "Filhos");
            DicionarioColunas.Add("Coluna_deficiencia", "Deficiência");
            DicionarioColunas.Add("Coluna_pretensao_salarial", "Pretensão");
            DicionarioColunas.Add("Coluna_ultimo_salario", "Último Salário");
            DicionarioColunas.Add("Coluna_funcao_pretendida", "Função Pretendida");
            DicionarioColunas.Add("Coluna_ultima_experiencia", "Última Empresa/Função/Tempo");
            DicionarioColunas.Add("Coluna_penultima_experiencia", "Penúltima Empresa/Função/Tempo");
            DicionarioColunas.Add("Coluna_antepenultima_experiencia", "Antepenúltima Empresa/Função/Tempo");
        }
        #endregion

        #region btnAplicarColunas_Click
        protected void btnAplicarColunas_Click(object sender, EventArgs e)
        {
            int countColunasSelecionadas = ContarColunasSelecionadas();

            if (countColunasSelecionadas < 6)
                base.ExibirMensagem(MensagemAviso._206501, TipoMensagem.Erro);
            else if (countColunasSelecionadas > 7)
                base.ExibirMensagem(MensagemAviso._206502, TipoMensagem.Erro);
            else
            {
                AjustarColunas();
                upGvCompararCurriculo.Update();
            }
        }
        #endregion

        #region FormatarFuncaoPretendida
        /// <summary>
        /// Metodo responsavel por formatar a função pretendida corretamente na grid
        /// </summary>
        /// <param name="funcoesPretendidas"></param>
        /// <returns></returns>
        public string FormatarFuncaoPretendida(object funcoesPretendidas)
        {
            string funcoes = funcoesPretendidas.ToString();
            string[] funcoesSplit = funcoes.Split(';');

            var sb = new StringBuilder();
            for (int i = 0; i < funcoesSplit.Length - 1; i++)
            {
                if ((funcoesSplit.Length - 1).Equals(1))
                    sb.Append(funcoesSplit[i]);
                else
                {
                    if (i.Equals(funcoesSplit.Length - 2))
                        sb.Append(funcoesSplit[i]);
                    else
                        sb.Append(funcoesSplit[i] + " ; ");
                }
            }

            return sb.ToString();
        }

        #endregion

        #region RetornarURL
        protected string RetornarURL(string funcao, string nomeCidade, string siglaEstado, int identificadorCurriculo)
        {
            string nomeFuncao = string.Empty;

            if (!string.IsNullOrEmpty(funcao))
            {
                var funcoes = funcao.Split(';').Where(f => !string.IsNullOrEmpty(f));
                nomeFuncao = funcoes.ElementAt(0);
            }

            return SitemapHelper.MontarUrlVisualizacaoCurriculo(nomeFuncao, nomeCidade, siglaEstado, identificadorCurriculo);
        }
        #endregion

        #endregion
    }
}