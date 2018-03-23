using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO.Empresas
{
    /// <summary>
    /// Dados da empresa
    /// </summary>
    public class DadosEmpresa
    {
        /// <summary>
        /// Nome da empresa
        /// </summary>
        public string NomeEmpresa { get; set; }

        /// <summary>
        /// Quantidade de funcionários
        /// </summary>
        public int? QuantidadeFuncionarios { get; set; }

        /// <summary>
        /// Número de telefone da empresa
        /// </summary>
        public string NumeroTelefone { get; set; }

        /// <summary>
        /// Cidade da empresa
        /// </summary>
        public string Cidade { get; set; }

        /// <summary>
        /// Bairro da empresa
        /// </summary>
        public string Bairro { get; set; }

        /// <summary>
        /// Data de cadastro da empresa
        /// </summary>
        public DateTime? DataCadastro { get; set; }

        /// <summary>
        /// Quantidade de vagas divulgadas pela empresa
        /// </summary>
        public int? QuantidadeVagasDivulgadas { get; set; }

        /// <summary>
        /// CNPJ da empresa
        /// </summary>
        public decimal NumeroCNPJ { get; set; }

        /// <summary>
        /// Área de atuação da empresa
        /// </summary>
        public string DesAreaBne { get; set; }
    }
}