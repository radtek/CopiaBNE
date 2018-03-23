using Bne.Web.Services.API.DTO.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace Bne.Web.Services.API.DTO.CadastroCV
{
    /// <summary>
    /// Classe com informações complementares
    /// </summary>
    [DataContract]
    public class DadosComplementares
    {
        /// <summary>
        /// Veículos do candidato
        /// </summary>
        [DataMember]
        public VeiculoCurriculo[] Veiculos { get; set; }

        /// <summary>
        /// Categoria da CNH (Carteira Nacional de Habilitação)
        /// </summary>
        [DataMember]
        [JsonConverter(typeof(StringEnumConverter))]
        public CategoriaCNH? CategoriaCNH { get; set; }

        /// <summary>
        /// Número da CNH (Carteira Nacional de Habilitação)
        /// </summary>
        [DataMember]
        public decimal? NumeroCnh { get; set; }

        /// <summary>
        /// Descrição de outros conhecimentos e habilidades
        /// </summary>
        [DataMember]
        public string OutrosConhecimentos { get; set; }

        /// <summary>
        /// Outras observações pertinentes
        /// </summary>
        [DataMember]
        public string Observacoes { get; set; }

        /// <summary>
        /// Lista com as disponibilidades de horário de trabalho e viagens
        /// </summary>
        [DataMember]
        public Disponibilidade[] Disponibilidades { get; set; }

        /// <summary>
        /// Lista com as cidades para disponibilidade de trabalho.
        /// Nome da cidade no formato "NomeCidade/SiglaEstado" (Ex.: São Paulo/SP)
        /// </summary>
        [DataMember]
        public string[] DisponibilidadeOutrasCidades { get; set; }

        /// <summary>
        /// Raça do candidato de acordo com a tabela do governo
        /// </summary>
        [DataMember]
        [JsonConverter(typeof(StringEnumConverter))]
        public Raca? Raca { get; set; }

        /// <summary>
        /// Altura em metros
        /// </summary>
        [DataMember]
        public decimal? Altura { get; set; }

        /// <summary>
        /// Altura em metros
        /// </summary>
        [DataMember]
        public decimal? Peso { get; set; }

        /// <summary>
        /// Indica se o candidato possui filhos
        /// </summary>
        [DataMember]
        public bool? PossuiFilhos { get; set; }

        /// <summary>
        /// Deficiência do candidato
        /// Deve estar presente na tabela Deficiencias
        /// </summary>
        [DataMember]
        public string Deficiencia { get; set; }

        /// <summary>
        /// Complemento da deficiência
        /// </summary>
        [DataMember]
        public string ComplementoDeficiencia { get; set; }
    }
}