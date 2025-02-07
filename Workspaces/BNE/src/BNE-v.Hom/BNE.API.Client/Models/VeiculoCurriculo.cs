// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace BNE.WebServices.API.Client.Models
{
    using System.Linq;

    /// <summary>
    /// Classe com informações sobre veículos
    /// </summary>
    public partial class VeiculoCurriculo
    {
        /// <summary>
        /// Initializes a new instance of the VeiculoCurriculo class.
        /// </summary>
        public VeiculoCurriculo() { }

        /// <summary>
        /// Initializes a new instance of the VeiculoCurriculo class.
        /// </summary>
        /// <param name="tipoVeiculo">Tipo do veículo.
        /// Deve conter um dos valores presentes na tabela
        /// TiposVeiculos</param>
        /// <param name="descricaoModelo">Descrição do modelo do
        /// veículo</param>
        /// <param name="ano">Ano de fabricação do veículo</param>
        public VeiculoCurriculo(string tipoVeiculo, string descricaoModelo = default(string), int? ano = default(int?))
        {
            TipoVeiculo = tipoVeiculo;
            DescricaoModelo = descricaoModelo;
            Ano = ano;
        }

        /// <summary>
        /// Gets or sets tipo do veículo.
        /// Deve conter um dos valores presentes na tabela TiposVeiculos
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "TipoVeiculo")]
        public string TipoVeiculo { get; set; }

        /// <summary>
        /// Gets or sets descrição do modelo do veículo
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "DescricaoModelo")]
        public string DescricaoModelo { get; set; }

        /// <summary>
        /// Gets or sets ano de fabricação do veículo
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "Ano")]
        public int? Ano { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (TipoVeiculo == null)
            {
                throw new Microsoft.Rest.ValidationException(Microsoft.Rest.ValidationRules.CannotBeNull, "TipoVeiculo");
            }
        }
    }
}
