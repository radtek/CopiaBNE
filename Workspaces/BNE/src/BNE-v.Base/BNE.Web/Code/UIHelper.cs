using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.BLL.Custom;
using BNE.BLL.Security;
using BNE.Web.Properties;
using Employer.Componentes.UI.Web;
using Telerik.Web.UI;
using System.Web;

namespace BNE.Web.Code
{
    public static class UIHelper
    {

        #region LimparSession
        /// <summary>
        /// Método que remove da sessão os valores temporários e adiciona no ViewState.
        /// </summary>
        public static void LimparSession(HttpSessionState session, StateBag viewState, Type enumType)
        {
            foreach (string chave in Enum.GetNames(enumType))
            {
                if (session[chave] != null)
                {
                    viewState.Add(chave, session[chave]);
                    session.Remove(chave);
                }
            }
        }
        #endregion

        #region CarregarListaSugestao
        /// <summary>
        /// Método para preencher a Lista de sugestão
        /// </summary>
        /// <param name="lista">Componente lista de sugestão</param>
        /// <param name="dataSource"></param>
        public static void CarregarListaSugestao(Employer.Plataforma.Web.Componentes.ListaSugestoes lista, Dictionary<string, string> dataSource)
        {
            lista.Dicionario = dataSource;
            lista.DataBind();
        }
        #endregion

        #region CarregarRepeater
        /// <summary>
        /// Método para encapsular a ação de carregar um Repeater, permitindo o Dispose do IDataReader.
        /// </summary>
        /// <param name="rpt">Componente que será preenchido.</param>
        /// <param name="dataSource">IDataReader contendo os dados.</param>
        public static void CarregarRepeater(Repeater rpt, IDataReader dataSource)
        {
            rpt.DataSource = dataSource;
            rpt.DataBind();
            if (dataSource != null)
                dataSource.Dispose();
        }

        public static void CarregarRepeater(Repeater rpt, IEnumerable dataSource)
        {
            rpt.DataSource = dataSource;
            rpt.DataBind();
        }

        public static void CarregarRepeater(Repeater rpt, DataTable dataSource)
        {
            rpt.DataSource = dataSource;
            rpt.DataBind();
            if (dataSource != null)
                dataSource.Dispose();
        }
        #endregion

        #region CarregarGridView
        /// <summary>
        /// Método para encapsular a ação de carregar um GridView, permitindo o Dispose do IDataReader.
        /// </summary>
        /// <param name="gv">Componente que será preenchido.</param>
        /// <param name="dataSource">DataTable contendo os dados.</param>
        public static void CarregarGridView(GridView gv, DataTable dataSource)
        {
            gv.DataSource = dataSource;
            gv.DataBind();
            if (dataSource != null)
                dataSource.Dispose();
        }
        #endregion

        #region CarregarRadGrid
        /// <summary>
        /// Método para encapsular a ação de carregar um GridView, permitindo o Dispose do IDataReader.
        /// </summary>
        /// <param name="gv">Componente que será preenchido.</param>
        /// <param name="dataSource">IDataReader contendo os dados.</param>
        public static void CarregarRadGrid(RadGrid gv, DataTable dataSource)
        {
            gv.DataSource = dataSource;
            gv.DataBind();
            if (dataSource != null)
                dataSource.Dispose();
        }
        public static void CarregarRadGrid(RadGrid gv, DataTable dataSource, int totalRegistros)
        {
            gv.DataSource = dataSource;
            gv.VirtualItemCount = totalRegistros;
            gv.MasterTableView.VirtualItemCount = totalRegistros;
            gv.DataBind();

            if (dataSource != null)
                dataSource.Dispose();
        }
        #endregion

        #region CarregarGridViewEmployer
        /// <summary>
        /// Método para encapsular a ação de carregar um GridView, permitindo o Dispose do IDataReader.
        /// </summary>
        /// <param name="gv">Componente que será preenchido.</param>
        /// <param name="dataSource">IDataReader contendo os dados.</param>
        /// <param name="totalRegistros"> </param>
        public static void CarregarGridViewEmployer(EmployerGrid gv, DataTable dataSource, int totalRegistros)
        {
            gv.DataSource = dataSource;
            gv.MockItemCount = totalRegistros;
            gv.DataBind();
            if (dataSource != null)
                dataSource.Dispose();
        }
        #endregion

        #region CarregarRadListView
        public static void CarregarRadListView(RadListView rlv, DataTable dataSource)
        {
            rlv.DataSource = dataSource;
            rlv.DataBind();
            if (dataSource != null)
                dataSource.Dispose();
        }
        #endregion

        #region CarregarDropDownList
        public static void CarregarDropDownList(DropDownList ddl, IDataReader dataSource, string dataValueField, string dataTextField)
        {
            CarregarDropDownList(ddl, dataSource, dataValueField, dataTextField, null);
        }
        public static void CarregarDropDownList(DropDownList ddl, IDataReader dataSource, string dataValueField, string dataTextField, ListItem listItemIndexZero)
        {
            ddl.DataValueField = dataValueField;
            ddl.DataTextField = dataTextField;
            ddl.DataSource = dataSource;
            ddl.DataBind();

            if (dataSource != null)
                dataSource.Dispose();

            if (listItemIndexZero != null)
                ddl.Items.Insert(0, listItemIndexZero);
        }
        public static void CarregarDropDownList(DropDownList ddl, Dictionary<string, string> dataSource)
        {
            CarregarDropDownList(ddl, dataSource, null);
        }
        public static void CarregarDropDownList(DropDownList ddl, Dictionary<string, string> dataSource, ListItem listItemIndexZero)
        {
            ddl.DataValueField = "Key";
            ddl.DataTextField = "Value";
            ddl.DataSource = dataSource;
            ddl.DataBind();

            if (listItemIndexZero != null)
                ddl.Items.Insert(0, listItemIndexZero);
        }
        #endregion

        #region CarregarRadComboBox
        /// <summary>
        /// Método para carregar uma dropdownlist
        /// </summary>
        /// <param name="rcb">Componente RadComboBox</param>
        /// <param name="dataTextField">DataTextField da lista</param>
        /// <param name="dataSource"></param>
        /// <param name="dataValueField">DataValueField da lista</param>
        public static void CarregarRadComboBox(RadComboBox rcb, IDataReader dataSource, string dataValueField, string dataTextField)
        {
            CarregarRadComboBox(rcb, dataSource, dataValueField, dataTextField, null);
        }
        public static void CarregarRadComboBox(RadComboBox rcb, IDataReader dataSource, string dataValueField, string dataTextField, RadComboBoxItem rcbiIndexZero)
        {
            rcb.DataValueField = dataValueField;
            rcb.DataTextField = dataTextField;
            rcb.DataSource = dataSource;
            rcb.DataBind();

            if (dataSource != null)
                dataSource.Dispose();

            if (rcbiIndexZero != null)
                rcb.Items.Insert(0, rcbiIndexZero);
        }
        public static void CarregarRadComboBox(RadComboBox rcb, Dictionary<string, string> dataSource)
        {
            CarregarRadComboBox(rcb, dataSource, null);
        }
        public static void CarregarRadComboBox(RadComboBox rcb, Dictionary<string, string> dataSource, RadComboBoxItem rcbiIndexZero)
        {
            rcb.DataValueField = "Key";
            rcb.DataTextField = "Value";
            rcb.DataSource = dataSource;
            rcb.DataBind();

            if (rcbiIndexZero != null)
                rcb.Items.Insert(0, rcbiIndexZero);
        }
        #endregion

        #region CarregarRadioButtonList
        /// <summary>
        /// Método para carregar uma RadioButtonList
        /// </summary>
        /// <param name="rbl">Componente RadioButtonList</param>
        /// <param name="dataTextField">DataTextField da lista</param>
        /// <param name="dataSource"></param>
        /// <param name="dataValueField">DataValueField da lista</param>
        public static void CarregarRadioButtonList(RadioButtonList rbl, IDataReader dataSource, string dataValueField, string dataTextField)
        {
            rbl.DataValueField = dataValueField;
            rbl.DataTextField = dataTextField;
            rbl.DataSource = dataSource;
            rbl.DataBind();

            if (dataSource != null)
                dataSource.Dispose();
        }
        public static void CarregarRadioButtonList(RadioButtonList rbl, Dictionary<string, string> dataSource)
        {
            rbl.DataValueField = "Key";
            rbl.DataTextField = "Value";
            rbl.DataSource = dataSource;
            rbl.DataBind();
        }
        #endregion

        #region AjustarString
        public static string AjustarString(string entrada)
        {
            //Remove espaços iniciais e finais.
            entrada = entrada.Trim();

            while (entrada.Contains("  "))
            {
                entrada = entrada.Replace("  ", " ");
            }

            //Coloca primeira letra para maiúscula.
            entrada = FormatarPrimeiraMaiscula(entrada);
            //removendo palavras auxiliares
            entrada = entrada.Replace(" De ", " de ");
            entrada = entrada.Replace(" Do ", " do ");
            entrada = entrada.Replace(" Dos ", " dos ");
            entrada = entrada.Replace(" Da ", " da ");
            entrada = entrada.Replace(" Para ", " para ");

            return entrada;
        }

        /// <summary>
        /// Método que formata a string para apenas a primeira letra maiúscula.
        /// </summary>
        /// <param name="entrada">Texto de entrada.</param>
        /// <returns>Texto formatado.</returns>
        private static string FormatarPrimeiraMaiscula(string entrada)
        {
            string[] palavras = entrada.Split(new[] { ' ' });

            entrada = palavras.Where(palavra => !palavra.Length.Equals(0)).Aggregate(string.Empty, (current, palavra) => current + (" " + palavra.Substring(0, 1).ToUpper() + palavra.Substring(1).ToLower()));

            if (entrada.Length.Equals(0))
                return string.Empty;

            return entrada.Substring(1);
        }
        #endregion

        #region RemoverAcentos
        /// <summary>
        /// Método que remove os acentos de uma string
        /// </summary>
        /// <param name="texto">Texto a ser removido a acentuação</param>
        /// <returns>String sem acentos</returns>
        public static string RemoverAcentos(string texto)
        {
            string s = texto.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char t in s)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(t);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(t);
                }
            }
            return sb.ToString();
        }
        #endregion

        #region CarregarCheckBoxList
        /// <summary>
        /// Método para carregar uma CheckBoxList
        /// </summary>
        /// <param name="rbl">Componente CheckBoxList</param>
        /// <param name="dataTextField">DataTextField da lista</param>
        /// <param name="dataSource"></param>
        /// <param name="dataValueField">DataValueField da lista</param>
        /// 
        public static void CarregarCheckBoxList(CheckBoxList rbl, Dictionary<string, string> dataSource)
        {
            rbl.DataValueField = "Key";
            rbl.DataTextField = "Value";
            rbl.DataSource = dataSource;
            rbl.DataBind();
        }
        #endregion

        #region ValidateFocus
        /// <summary>
        /// Método que registra um script para adicionar um EventHandler a um controle.
        /// Este event handler atribuirá o foco para o primeiro campo inválido da página.
        /// </summary>
        /// <param name="controle">Controle que será adicionado o manipulador de evento.</param>
        public static void ValidateFocus(WebControl controle)
        {
            string validationGroup = string.Empty;
            PropertyInfo prop = controle.GetType().GetProperty("ValidationGroup");
            if (prop != null)
                validationGroup = prop.GetValue(controle, null).ToString();
            controle.Attributes["OnClick"] += "searchInvalidValidator(this, '" + validationGroup + "');";
        }
        #endregion

        #region RecuperarItensAutoComplete
        /// <summary>
        /// Método responsável por montar os itens de um auto complete utilizando um dictionary com a Key (Idf) e Value (Texto a ser mostrado)
        /// </summary>
        /// <param name="dicItens"></param>
        /// <returns></returns>
        public static string[] RecuperarItensAutoComplete(Dictionary<int, string> dicItens)
        {
            return dicItens.Select(kvp => AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(kvp.Value, kvp.Key.ToString(CultureInfo.CurrentCulture))).ToArray();
        }
        #endregion

        #region FormatarCidade
        public static string FormatarCidade(string nomeCidade, string siglaEstado)
        {
            return BLL.Custom.Helper.FormatarCidade(nomeCidade, siglaEstado);
        }
        #endregion

        #region RetornarDesricaoSalario
        public static string RetornarDesricaoSalario(decimal? valorSalarioDe, decimal? valorSalarioAte)
        {
            if ((valorSalarioDe == null && valorSalarioAte == null) || (valorSalarioDe == null || valorSalarioAte == null))
                return "Salário a combinar";

            if (valorSalarioDe.Equals(valorSalarioAte))
                return string.Format("Salário de R$ {0}", ((decimal)valorSalarioDe).ToString("N2"));

            return string.Format(CultureInfo.CurrentCulture, "Salário de R$ {0} a R$ {1}", ((decimal)valorSalarioDe).ToString("N2"), ((decimal)valorSalarioAte).ToString("N2"));
        }
        #endregion

        #region FormatarTelefone
        public static String FormatarTelefone(string ddd, string telefone)
        {
            return BLL.Custom.Helper.FormatarTelefone(ddd, telefone);
        }
        #endregion

        #region FormatarCPF
        public static string FormatarCPF(string cpf)
        {
            return BLL.Custom.Helper.FormatarCPF(cpf);
        }
        #endregion

        #region RecuperarCaminhoProdutoVIP
        /// <summary>
        /// Método que retorna o caminho para as telas de venda de produto VIP
        /// </summary>
        /// <param name="urlProdutoVIP">URL produto vip</param>
        /// <param name="enumVantagensVIP">Enumerador da vantagem</param>
        /// <returns></returns>
        public static string RecuperarCaminhoProdutoVIP(string urlProdutoVIP, BLL.Enumeradores.VantagensVIP? enumVantagensVIP)
        {
            if (enumVantagensVIP == null)
            {
                return urlProdutoVIP;
            }
            return string.Format("{1}?vantagem={0}", enumVantagensVIP, urlProdutoVIP);
        }
        #endregion

        #region RecuperarEnderecoCompleto
        public static bool RecuperarEnderecoCompleto(string numCEP, out string descricaoLogradouro, out string descricaoBairro, out string nomeCidade, out string siglaEstado)
        {
            descricaoLogradouro = descricaoBairro = nomeCidade = siglaEstado = string.Empty;

            var servico = new wsCEP.cepws();
            ServiceAuth.GerarHashAcessoWS(servico);

            var objCep = new wsCEP.CEP
            {
                Cep = numCEP.Replace("-", string.Empty).Replace(".", string.Empty).Trim()
            };

            int qtdeCepEncontrados = 0;
            try
            {
                if (servico.RecuperarQuantidadeEnderecosPorCEP(objCep, ref qtdeCepEncontrados) && !qtdeCepEncontrados.Equals(0))
                {
                    servico.CompletarCEP(ref objCep);

                    descricaoLogradouro = objCep.Logradouro;
                    descricaoBairro = objCep.Bairro;
                    nomeCidade = objCep.Cidade;
                    siglaEstado = objCep.Estado;

                    return true;
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
            return false;
        }
        #endregion

        #region RecuperarURLAmbiente
        /// <summary>
        /// Recupera a url do ambiente
        /// </summary>
        public static string RecuperarURLAmbiente()
        {
            return BLL.Custom.Helper.RecuperarURLAmbiente();
        }
        #endregion

        #region RetornarUrlFoto
        /// <summary>
        /// Recupera a URL da Foto do candidato
        /// </summary>
        /// <param name="numeroCPF"></param>
        /// <param name="origemFoto"></param>
        /// <returns></returns>
        public static string RetornarUrlFoto(string numeroCPF, Handlers.PessoaFisicaFoto.OrigemFoto origemFoto)
        {
            return string.Format("http://{0}{1}", RecuperarURLAmbiente(), string.Format(Settings.Default.PessoaFisicaFotoHandler, numeroCPF, origemFoto));
        }
        #endregion RetornarUrlFoto

        #region RetornarUrlLogo
        /// <summary>
        /// Recupera a URL da Logo da empresa
        /// </summary>
        /// <param name="numeroCNPJ"></param>
        /// <param name="origemLogo"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static string RetornarUrlLogo(string numeroCNPJ, Handlers.PessoaJuridicaLogo.OrigemLogo origemLogo, int? width = null, int? height = null)
        {
            var url = string.Format("http://{0}{1}", RecuperarURLAmbiente(), string.Format(Settings.Default.PessoaJuridicaLogoHandler, numeroCNPJ, origemLogo));

            if (width.HasValue && height.HasValue)
                url += string.Format("&width={0}&height={1}", width, height);

            return url;
        }
        #endregion RetornarUrlLogo

        /// <summary>
        /// Converts the provided app-relative path into an absolute Url containing the 
        /// full host name
        /// </summary>
        /// <param name="relativeUrl">App-Relative path</param>
        /// <returns>Provided relativeUrl parameter as fully qualified Url</returns>
        /// <example>~/path/to/foo to http://www.web.com/path/to/foo</example>
        public static string GetAbsoluteUrl(this string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (HttpContext.Current == null)
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return String.Format("{0}://{1}{2}{3}",
                url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }

        #region ExtrairTemplateRadEditor
        public static string ExtrairTemplateRadEditor(string conteudo)
        {
            conteudo = conteudo.Replace("\r", string.Empty).Replace("\n", string.Empty);

            var regex = new Regex(".*<td style=\"line-height: 150%;\">(.*)</td>.*");

            Match match = regex.Match(conteudo);
            if (match.Success)
                return match.Groups[1].Value;

            regex = new Regex(".*<td style=\\\"line-height: 150%;\\\">(.*)</td>.*");
            match = regex.Match(conteudo);
            if (match.Success)
                return match.Groups[1].Value;

            return string.Empty;
        }
        #endregion ExtrairTemplateRadEditor

        #region IncluirTemplateRadEditor
        public static string IncluirTemplateRadEditor(string conteudo)
        {
            const string template = "<table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" width=\"540\"> <tr> <td style=\"line-height: 150%;\"> {Conteudo} </td> </tr> </table>";
            var parametro = new
            {
                Conteudo = conteudo
            };

            return parametro.ToString(template);
        }
        #endregion IncluirTemplateRadEditor        

    }
}
