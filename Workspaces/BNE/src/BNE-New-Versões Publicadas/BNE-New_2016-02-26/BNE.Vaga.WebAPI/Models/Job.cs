using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.Vaga.WebAPI.Models
{
    public class Job
    {
        /// <summary>
        /// Identificador da vaga
        /// </summary>
        public int Id;

        /// <summary>
        /// Tipo do Contrato (Efetivo, Temporário, Estágio, Autonomo, Freelancer, Aprendiz)
        /// </summary>
        public String Contract;
        /// <summary>
        /// Função da vaga
        /// </summary>
        public String Title;
        /// <summary>
        /// Nível da formação. Indicado para tipo de contrato Estágio.
        /// </summary>
        public String EducationLevel;
        /// <summary>
        /// Curso da formação. Indicado para tipo de contrato Estágio.
        /// </summary>
        public String Course;
        /// <summary>
        /// Cidade da Vaga
        /// </summary>
        public String City;
        /// <summary>
        /// Bairro
        /// </summary>
        public String Neighborhood;
        /// <summary>
        /// Valor mínimo do salário
        /// </summary>
        public Decimal MinSalary;
        /// <summary>
        /// Valor máximo do salário
        /// </summary>
        public Decimal MaxSalary;
        /// <summary>
        /// Quantidade de vagas
        /// </summary>
        public int Quantity;

        /// <summary>
        /// E-mail de contato para a vaga
        /// </summary>
        public string Email;
        /// <summary>
        /// Telefone de contato para a vaga
        /// </summary>
        public string PhoneNumber;
        /// <summary>
        /// Flag indicando se a vaga é confidencial
        /// </summary>
        public bool Confidential;

        /// <summary>
        /// Flag indicando se o anunciante deseja receber um e-mail a cada candidatura
        /// </summary>
        public bool ReceiveOneEmailPerApply;
        /// <summary>
        /// Flag indicando se o anunciante deseja receber um e-mail diário com o resumo das candidaturas
        /// </summary>
        public bool ReceiveApplicationsDailyEmail;
        
        /// <summary>
        /// Benefícios da Vaga
        /// </summary>
        public List<String> Benefits;

        /// <summary>
        /// Descrição Geral da Vaga
        /// </summary>
        public String Description;

        /// <summary>
        /// Grupo da Vaga
        /// </summary>
        public String Group;

        /// <summary>
        /// Sistema que incluiu a vaga
        /// </summary>
        public String System;

        /// <summary>
        /// Origem de vaga importada
        /// </summary>
        public String ImportedFrom;

    }
}