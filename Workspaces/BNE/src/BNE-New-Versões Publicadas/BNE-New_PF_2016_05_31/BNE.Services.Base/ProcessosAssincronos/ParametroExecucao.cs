using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Collections.ObjectModel;
using System.Xml;

namespace BNE.Services.Base.ProcessosAssincronos
{
    [Serializable]
    public class ParametroExecucaoValor
    {
        [XmlAttribute]
        public String Valor { get; set; }
        [XmlAttribute]
        public String DesValor { get; set; }
    }

    /// <summary>
    /// Os parâmetros de execução
    /// </summary>
    [Serializable]
    public class ParametroExecucao
    {
        public ParametroExecucao()
        {
            Valores = new List<ParametroExecucaoValor>();
            ExibirParametro = true;
        }

        #region Parametro
        /// <summary>
        /// Parametro
        /// </summary>
        public String Parametro { get; set; }
        #endregion

        #region DesParametro
        /// <summary>
        /// Descrição do parâmetro - Usado em tela
        /// </summary>
        public String DesParametro { get; set; }
        #endregion

        #region Valores
        public List<ParametroExecucaoValor> Valores { get; set; }
        #endregion

        #region Valor
        /// <summary>
        /// Valor do primeiro parâmetro
        /// </summary>
        [XmlIgnore]
        public String Valor
        {
            get
            {
                if (Valores.Count == 0)
                {
                    Valores.Add(new ParametroExecucaoValor());
                }
                return Valores[0].Valor;
            }
            set
            {
                if (Valores.Count == 0)
                {
                    Valores.Add(new ParametroExecucaoValor());
                }
                Valores[0].Valor = value;
            }
        }
        #endregion

        #region DesValor
        /// <summary>
        /// Descrição do valor primeiro do parâmetro - Usado em tela
        /// </summary>
        [XmlIgnore]
        public String DesValor
        {
            get
            {
                if (Valores.Count == 0)
                {
                    Valores.Add(new ParametroExecucaoValor());
                }

                var vals = new String[Valores.Count];
                for (int i = 0; i < Valores.Count; i++)
                {
                    vals[i] = Valores[i].DesValor;
                }

                return String.Join(", ", vals);
            }
            set
            {
                if (Valores.Count == 0)
                {
                    Valores.Add(new ParametroExecucaoValor());
                }
                Valores[0].DesValor = value;
            }

        }
        #endregion

        #region ValorInt
        /// <summary>
        /// Retorna o valor do parâmetro em int
        /// </summary>
        [XmlIgnoreAttribute]
        public int? ValorInt
        {
            get
            {
                int dummy;
                if (int.TryParse(Valor, out dummy))
                    return dummy;

                return null;
            }
            set
            {
                Valor = Convert.ToString(value);
            }
        }
        #endregion

        #region ValorDecimal
        /// <summary>
        /// Retorna o valor do parâmetro em decimal
        /// </summary>
        [XmlIgnoreAttribute]
        public Decimal? ValorDecimal
        {
            get
            {
                Decimal dummy;
                if (Decimal.TryParse(Valor, out dummy))
                    return dummy;

                return null;
            }
            set
            {
                Valor = Convert.ToString(value);
            }
        }
        #endregion

        #region ValorBool
        /// <summary>
        /// Retorna o valor do parâmetro em booleano
        /// </summary>
        [XmlIgnoreAttribute]
        public bool ValorBool
        {
            get
            {
                Boolean dummy;
                if (Boolean.TryParse(Valor, out dummy))
                    return dummy;

                return "1".Equals(Valor);
            }
            set
            {
                Valor = Convert.ToString(value);
            }
        }
        #endregion

        #region ExibirParametro
        public Boolean ExibirParametro { get; set; }
        #endregion

        #region Adicionar
        public void Adicionar(String valor, String desValor)
        {
            Valores.Add(new ParametroExecucaoValor { Valor = valor, DesValor = desValor });
        }
        #endregion
    }

    #region ParametroExecucaoCollection
    /// <summary>
    /// Coleção dos parâmetros de execução
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "Parametros")]
    public class ParametroExecucaoCollection : Collection<ParametroExecucao>
    {
        #region Métodos

        #region ToXML
        /// <summary>
        /// Convert a coleção para XML
        /// </summary>
        /// <returns>A string xml que representa a coleção</returns>
        public String ToXML()
        {
            var serializer = new XmlSerializer(GetType());

            var settings = new XmlWriterSettings
                {
                    Encoding = new UnicodeEncoding(false, false),
                    Indent = false,
                    OmitXmlDeclaration = false
                };

            using (var textWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, this);
                }

                String res = textWriter.ToString();
                if (String.IsNullOrEmpty(res))
                    return String.Empty;

                return res.Substring(res.IndexOf("?>", StringComparison.Ordinal) + 2);
            }
        }
        #endregion

        #region FromXML
        /// <summary>
        /// Carrega a coleção a partir de uma string XML
        /// </summary>
        /// <param name="xml">A string XML</param>
        /// <returns>A coleção carregada</returns>
        public static ParametroExecucaoCollection FromXML(String xml)
        {
            var serializer = new XmlSerializer(typeof(ParametroExecucaoCollection));

            var settings = new XmlReaderSettings();
            // No settings need modifying here

            using (var textReader = new StringReader(xml))
            {
                using (XmlReader xmlReader = XmlReader.Create(textReader, settings))
                {
                    return (ParametroExecucaoCollection)serializer.Deserialize(xmlReader);
                }
            }
        }
        #endregion

        #region Add
        /// <summary>
        /// Adiciona um ítem a coleção
        /// </summary>
        /// <param name="parametro">O parametro</param>
        /// <param name="desParametro">A descrição do parâmetro</param>
        /// <param name="valor">O valor do parâmetro</param>
        /// <param name="desValor">A descrição do valor do parâmetro</param>
        public void Add(String parametro, String desParametro, String valor, String desValor)
        {
            var objParametro = new ParametroExecucao
                {
                    Parametro = parametro,
                    DesParametro = desParametro,
                    Valor = valor,
                    DesValor = desValor
                };

            Add(objParametro);
        }
        public void Add(String parametro, String desParametro, String valor, String desValor, bool exibirParametro)
        {
            var objParametro = new ParametroExecucao
                {
                    Parametro = parametro,
                    DesParametro = desParametro,
                    Valor = valor,
                    DesValor = desValor,
                    ExibirParametro = exibirParametro
                };

            Add(objParametro);
        }
        #endregion

        #endregion

        #region Propriedades

        #region this
        /// <summary>
        /// Retorna a instância do parâmetro a partir do valor da sua propriedade "parâmetro".
        /// Caso o parâmetro não exista, cria uma nova instância e a retorna
        /// </summary>
        /// <param name="parametro">O valor da propriedade "parâmetro"</param>
        /// <returns>A instância do parâmetro</returns>
        public ParametroExecucao this[String parametro]
        {
            get
            {
                foreach (ParametroExecucao parm in this)
                {
                    if (parm.Parametro == parametro)
                        return parm;
                }

                var p = new ParametroExecucao
                    {
                        Parametro = parametro,
                        DesParametro = parametro
                    };
                Add(p);

                return p;
            }
        }
        #endregion

        #endregion
    }
    #endregion

}
