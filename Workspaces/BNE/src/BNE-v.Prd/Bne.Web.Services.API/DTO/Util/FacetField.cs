using Bne.Web.Services.API.DTO.Enum;
using BNE.Solr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO.Util
{
    public class FacetField
    {
        public FacetField(VagaFacetField field, List<object> facetValues)
        {
            FieldName = field;
            Facets = new List<Facet>();
            for (int i = 0; i < facetValues.Count; i = i + 2)
            {
                Facets.Add(new Facet(Convert.ToString(facetValues[i]), 
                    Convert.ToInt32(facetValues[i + 1])));
            }
        }

        /// <summary>
        /// Campo solicitado para sumarização
        /// </summary>
        public VagaFacetField FieldName { get; set; }
        /// <summary>
        /// Lista de valores e quantidades de ocorrência
        /// </summary>
        public List<Facet> Facets { get; set; }
    }

    public class Facet
    {
        public Facet(string valor, int quantidade)
        {
            Valor = valor;
            Quantidade = quantidade;
        }

        /// <summary>
        /// Valor do campo solicitado
        /// </summary>
        public string Valor { get; set; }

        /// <summary>
        /// Quantidade de vezes em que o Valor aparece para o campo solicitado
        /// </summary>
        public int Quantidade { get; set; }
    }
}