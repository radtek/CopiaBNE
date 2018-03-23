using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO
{
    /// <summary>
    /// Representa o resumo do currículo do candidato.
    /// </summary>
    public class CurriculoCurtoDTO
    {
        /// <summary>
        /// Indica se o candidato é VIP.
        /// </summary>
        public bool Vip { get; set; }

        /// <summary>
        /// Nome do candidato.
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Sexo do candidato
        /// </summary>
        public string Sexo { get; set; }

        /// <summary>
        /// Estado civil do candidato
        /// </summary>
        public string EstadoCivil { get; set; }

        /// <summary>
        /// Idade do candidato
        /// </summary>
        public int Idade { get; set; }

        /// <summary>
        /// Escolaridade do candidato.
        /// </summary>
        public string Escolaridade { get; set; }

        /// <summary>
        /// Pretensão salarial do candidato.
        /// </summary>
        public decimal Pretensao { get; set; }

        /// <summary>
        /// Bairro do candidato.
        /// </summary>
        public string Bairro { get; set; }

        /// <summary>
        /// Cidade do candidato.
        /// </summary>
        public string Cidade { get; set; }

        /// <summary>
        /// Estado do candidato.
        /// </summary>
        public string Estado { get; set; }

        /// <summary>
        /// Lista de funções pretendidas pelo candidato.
        /// </summary>
        public string Funcoes { get; set; }

        /// <summary>
        /// Experiências profissionais do candidato.
        /// </summary>
        public string Experiencia { get; set; }

        /// <summary>
        /// Categoria da carteira de Habilitação do candidato.
        /// </summary>
        public string Carteira { get; set; }

        /// <summary>
        /// Código identificador do candidato.
        /// </summary>
        public int IDCurriculo { get; set; }
    }
}