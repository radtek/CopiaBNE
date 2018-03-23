using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BNE.Services.Base.ProcessosAssincronos
{
    /// <summary>
    ///     Coleção dos parâmetros de execução
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "Parametros")]
    public class ParametroExecucaoCollection : Collection<ParametroExecucao>
    {
        /// <summary>
        ///     Retorna a instância do parâmetro a partir do valor da sua propriedade "parâmetro".
        ///     Caso o parâmetro não exista, cria uma nova instância e a retorna
        /// </summary>
        /// <param name="parametro">O valor da propriedade "parâmetro"</param>
        /// <returns>A instância do parâmetro</returns>
        public ParametroExecucao this[string parametro]
        {
            get
            {
                foreach (var parm in this)
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

        #region Métodos

        #region ToXML
        /// <summary>
        ///     Convert a coleção para XML
        /// </summary>
        /// <returns>A string xml que representa a coleção</returns>
        public string ToXML()
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
                using (var xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, this);
                }

                var res = textWriter.ToString();
                if (string.IsNullOrEmpty(res))
                    return string.Empty;

                return res.Substring(res.IndexOf("?>", StringComparison.Ordinal) + 2);
            }
        }
        #endregion

        #region FromXML
        /// <summary>
        ///     Carrega a coleção a partir de uma string XML
        /// </summary>
        /// <param name="xml">A string XML</param>
        /// <returns>A coleção carregada</returns>
        public static ParametroExecucaoCollection FromXML(string xml)
        {
            var serializer = new XmlSerializer(typeof(ParametroExecucaoCollection));

            var settings = new XmlReaderSettings();

            using (var textReader = new StringReader(xml))
            {
                using (var xmlReader = XmlReader.Create(textReader, settings))
                {
                    return (ParametroExecucaoCollection) serializer.Deserialize(xmlReader);
                }
            }
        }
        #endregion

        #region Add
        /// <summary>
        ///     Adiciona um ítem a coleção
        /// </summary>
        /// <param name="parametro">O parametro</param>
        /// <param name="desParametro">A descrição do parâmetro</param>
        /// <param name="valor">O valor do parâmetro</param>
        /// <param name="desValor">A descrição do valor do parâmetro</param>
        public void Add(string parametro, string desParametro, string valor, string desValor)
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
        #endregion

        #endregion
    }
}