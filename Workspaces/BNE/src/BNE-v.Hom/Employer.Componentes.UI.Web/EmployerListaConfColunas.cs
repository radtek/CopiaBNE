using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.Script.Serialization;

[assembly: WebResource("Employer.Componentes.UI.Web.Content.js.EmployerListaConfColunas.js", "text/javascript")]

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Painel com itens que podem ordenados usando clicar e arrastar.<br/>
    /// Os itens são configurações de colunas usadas para a importação de arquivos.
    /// </summary>
    public class EmployerListaConfColunas : AjaxClientControlBase
    {
        HiddenField _hdnMetaDados = new HiddenField();
        Employer.Componentes.UI.Web.Panel _pnlTabela = new Componentes.UI.Web.Panel();
        CustomValidator _ctvColunas = new CustomValidator();

        /// <summary>
        /// Classe css do painel principal
        /// </summary>
        public override string CssClass
        {
            get
            {
                EnsureChildControls();
                return _pnlTabela.CssClass;
            }
            set
            {
                EnsureChildControls();
                _pnlTabela.CssClass = value;
            }
        }

        private Model _dados;

        /// <summary>
        /// Dados 
        /// </summary>
        public Model Dados
        {
            get
            {
                EnsureChildControls();
                if (_dados == null)
                {
                    var js = new JavaScriptSerializer();
                    _dados = js.Deserialize<Model>(_hdnMetaDados.Value);
                }
                if (_dados == null)
                    _dados = new Model();

                return _dados;
            }
        }

        /// <inheritdoc/>
        public string ValidationGroup
        {
            get { EnsureChildControls(); return _ctvColunas.ValidationGroup; }
            set { EnsureChildControls(); _ctvColunas.ValidationGroup = value; }
        }

        /// <inheritdoc/>
        protected override void CreateChildControls()
        {
            _hdnMetaDados.ID = "hdnMetaDados";
            _pnlTabela.Controls.Add(_hdnMetaDados);

            _pnlTabela.ID = "pnlTabela";
            this.Controls.Add(_pnlTabela);

            _ctvColunas.ID = "ctvColunas";
            _ctvColunas.EnableClientScript = true;
            _ctvColunas.ClientValidationFunction = "Employer.Componentes.UI.Web.EmployerListaConfColunas.ValidarColunas";
            this.Controls.Add(_ctvColunas);

            base.CreateChildControls();
        }

        /// <inheritdoc/>
        public override void DataBind()
        {
            for (short i = 0; this.Dados.Colunas.Count > i; i++)
            {
                this.Dados.Colunas[i].Ordem = i;
            }

            var js = new JavaScriptSerializer();
            _hdnMetaDados.Value = js.Serialize(this.Dados);

            base.DataBind();
        }

        /// <inheritdoc/>
        protected override void Render(HtmlTextWriter writer)
        {
            DataBind();

            base.Render(writer);
        }

        /// <inheritdoc/>
        public override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            ScriptControlDescriptor descriptor = new ScriptControlDescriptor("Employer.Componentes.UI.Web.EmployerListaConfColunas", this.ClientID);

            this.SetScriptDescriptors(descriptor);

            return new ScriptControlDescriptor[] { descriptor };
        }

        /// <inheritdoc/>
        public override IEnumerable<ScriptReference> GetScriptReferences()
        {
            IList<ScriptReference> references = new List<ScriptReference>(20);
            base.SetScriptReferences(references);

            ScriptReference reference = new ScriptReference();
            reference.Assembly = "Employer.Componentes.UI.Web";
            reference.Name = "Employer.Componentes.UI.Web.Content.js.EmployerListaConfColunas.js";
            references.Add(reference);

            return references;
        }

        /// <summary>
        /// Dados de configuração de importação externo
        /// </summary>
        [Serializable]
        public class ModelExterno
        {
            /// <inheritdoc/>
            public int? Id { get; set; }

            /// <inheritdoc/>
            public int IdArquivo { get; set; }

            /// <inheritdoc/>
            public short FormatoArquivo { get; set; }

            /// <inheritdoc/>
            public string Nome { get; set; }

            /// <summary>
            /// Configuração do arquivo
            /// </summary>
            public List<Coluna> ColunasArquivo { get; set; }

            /// <summary>
            /// Configuração padrão do arquivo
            /// </summary>
            public List<Coluna> ColunasConfiguracaoArquivo { get; set; }

            /// <inheritdoc/>
            public ModelExterno()
            {
                ColunasArquivo = new List<Coluna>(30);
                ColunasConfiguracaoArquivo = new List<Coluna>(30);
            }
        }

        /// <summary>
        /// Configuração de importação. Dados interno do componente
        /// </summary>
        [Serializable]
        public class Model
        {
            /// <summary>
            /// Excel: 3, Csv: 2, Posicional: 1 
            /// </summary>
            public int TipoArquivo { get; set; }

            /// <summary>
            /// Arquivo: 1, Configuracao: 2
            /// </summary>
            public short TipoLista { get; set; }
            /// <summary>
            /// Somente leitura
            /// </summary>
            public bool PermiteEdicao { get; set; }

            /// <summary>
            /// Colunas da configuração
            /// </summary>
            public List<Coluna> Colunas { get; set; }

            /// <summary>
            /// Id do objeto registrado em javascript
            /// </summary>
            public string IdListaConf { get; set; }

            /// <inheritdoc/>
            public Model()
            {
                Colunas = new List<Coluna>(30);
                PermiteEdicao = true;
            }            
        }

        /// <summary>
        /// Configuração da coluna de importação
        /// </summary>
        [Serializable]
        public class Coluna
        {
            /// <summary>
            /// Id da couna
            /// </summary>
            public int Id { get; set; }

            /// <inheritdoc/>
            public string Titulo { get; set; }

            /// <inheritdoc/>
            public string TituloSistema { get; set; }

            /// <summary>
            /// Ordem do campo
            /// </summary>
            public short Ordem { get; set; }
            /// <summary>
            /// Tamanho do campo. Usado em arquivo posicional.
            /// </summary>
            public int Tamanho { get; set; }
            /// <summary>
            /// Tipo de coluna: Texto: nulo, Numérico: 1, Data: 2
            /// </summary>
            public int? Tipo { get; set; }
            /// <summary>
            /// Campo obrigatório
            /// </summary>
            public bool Obrigatorio { get; set; }

            /// <inheritdoc/>
            public string Formato { get; set; }

            /// <summary>
            /// Direação de leitura do campo. Esquerda ou Direita
            /// </summary>
            public string Alinhamento { get; set; }

            /// <summary>
            /// Tipo de separador decimal Pode ser "Caracter" ou "Posição" 
            /// </summary>
            public string TipoSeparador { get; set; }

            /// <summary>
            /// Caráter separador de decimal  
            /// </summary>
            public string Separador { get; set; }
        }
    }
}
