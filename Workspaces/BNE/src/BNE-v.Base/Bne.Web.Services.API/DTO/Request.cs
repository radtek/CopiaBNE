using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO
{
    public class Request
    {
        /// <summary>
        /// CPF do usuário que está efetuando a requisição
        /// </summary>
        public String CPF { get; set; }
        /// <summary>
        /// Data de nascimento do usuário que está efetuando a requisição
        /// </summary>
        public DateTime DataNascimento { get; set; }
    }
}