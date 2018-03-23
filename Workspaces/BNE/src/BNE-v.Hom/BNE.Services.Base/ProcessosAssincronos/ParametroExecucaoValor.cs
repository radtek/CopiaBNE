using System;
using System.Xml.Serialization;

namespace BNE.Services.Base.ProcessosAssincronos
{
    [Serializable]
    public class ParametroExecucaoValor
    {
        [XmlAttribute]
        public string Valor { get; set; }

        [XmlAttribute]
        public string DescricaoValor { get; set; }
    }
}