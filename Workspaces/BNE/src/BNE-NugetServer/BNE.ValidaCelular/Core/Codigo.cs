using System;
using System.Linq;

namespace BNE.ValidaCelular.Core
{
    internal class Codigo
    {

        public string GerarCodigo()
        {
            return CriarCodigo(6);
        }

        private string CriarCodigo(int tamanho)
        {
            const string chars = "0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, tamanho).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string AplicarMascaraCodigo(string codigo)
        {
            return codigo.Substring(0, 3) + " " + codigo.Substring(3, 3);
        }
    }
}
