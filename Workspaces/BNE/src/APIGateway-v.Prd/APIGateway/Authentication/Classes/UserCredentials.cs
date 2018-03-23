using System;

namespace APIGateway.Authentication.Classes
{
    internal class UserCredentials
    {
        public decimal CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public decimal? CNPJ { get; set; }
        public Guid Sistema { get; set; }
    }
}