using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace BNE.Web.LanHouse.Code
{
    public static class Helper
    {
        /// <summary>
        /// Recupera o endereço de IP do usuário
        /// </summary>
        /// <param name="controller">Controller atual</param>
        /// <returns>string com o endereço de IP do usuário</returns>
        public static string RecuperarIP(Controller controller)
        {
            var httpContext = controller.HttpContext;

            string ip = string.Empty;

            if (httpContext != null)
            {
                ip = httpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (string.IsNullOrEmpty(ip))
                    ip = httpContext.Request.ServerVariables["REMOTE_ADDR"];
            }

            return ip;
        }

        /// <summary>
        /// Método que formata a string para apenas a primeira letra maiúscula.
        /// </summary>
        /// <param name="entrada">Texto de entrada.</param>
        /// <returns>Texto formatado.</returns>
        private static string FormatarPrimeiraMaiscula(string entrada)
        {
            string[] palavras = entrada.Split(new[] { ' ' });

            entrada = palavras.Where(palavra => !palavra.Length.Equals(0)).Aggregate(string.Empty, (current, palavra) => current + (" " + palavra.Substring(0, 1).ToUpper() + palavra.Substring(1).ToLower()));

            if (entrada.Length.Equals(0))
                return string.Empty;

            return entrada.Substring(1);
        }

        /// <summary>
        /// Ajusta nomes e outras entradas
        /// </summary>
        /// <param name="entrada">Entrada do usuário</param>
        /// <returns>string ajustada</returns>
        public static string AjustarString(string entrada)
        {
            //Remove espaços iniciais e finais.
            entrada = entrada.Trim();

            while (entrada.Contains("  "))
            {
                entrada = entrada.Replace("  ", " ");
            }

            //Coloca primeira letra para maiúscula.
            entrada = FormatarPrimeiraMaiscula(entrada);

            //removendo palavras auxiliares
            entrada = entrada.Replace(" De ", " de ");
            entrada = entrada.Replace(" Do ", " do ");
            entrada = entrada.Replace(" Dos ", " dos ");
            entrada = entrada.Replace(" Da ", " da ");
            entrada = entrada.Replace(" Para ", " para ");

            return entrada;
        }

        /// <summary>
        /// Transforma um CPF formatado em tipo Decimal
        /// </summary>
        /// <param name="cpf">CPF formatado</param>
        /// <returns>tipo decimal do CPF informado</returns>
        public static Decimal ConverterCpfParaDecimal(string cpf)
        {
            cpf = cpf.Replace(".", String.Empty).Replace("-", String.Empty);

            if (cpf.Length != 11)
                throw new ArgumentException("CPF informado não é válido, deve conter 11 dígitos", "cpf");

            return Decimal.Parse(cpf);
        }

        /// <summary>
        /// Gerar número de CPF a partir de uma string de semente com 9 dígitos
        /// </summary>
        /// <param name="semente">string com 9 dígitos</param>
        /// <returns>valor decimal do CPF, com a semente e os dois dígitos verificadores</returns>
        public static decimal GerarCpf(string semente)
        {
            semente = semente.Replace(".", String.Empty).Replace("-", String.Empty);

            if (semente.Length != 9)
                throw new ArgumentException("Semente deve ter 9 dígitos", "semente");

            int soma1 = 0;
            int soma2 = 0;

            for (int i = 0; i < 9; i++)
            {
                string dig_cpf = semente[i].ToString();
                soma1 += Convert.ToInt32(dig_cpf) * (10 - i);
                soma2 += Convert.ToInt32(dig_cpf) * (11 - i);
            }

            int dv1 = (11 - (soma1 % 11));
            dv1 = dv1 >= 10 ? 0 : dv1;
            soma2 = soma2 + (dv1 * 2);
            int dv2 = (11 - (soma2 % 11));
            dv2 = dv2 >= 10 ? 0 : dv2;

            decimal cpf = Convert.ToDecimal(semente) * 100m + dv1 * 10m + dv2;

            return cpf;
        }

        /// <summary>
        /// Verifica digito verificador de CPFs
        /// </summary>
        /// <param name="cpf">string cpf formatado ou não</param>
        /// <returns>true se for valido, false caso contrario</returns>
        public static bool VerificarDigitoVerificadorCpf(string cpf)
        {
            cpf = cpf.Replace(".", String.Empty).Replace("-", String.Empty);

            if (cpf.Length != 11)
                throw new ArgumentException("CPF informado não tem 11 dígitos", "cpf");

            int soma1 = 0;
            int soma2 = 0;
            
            for (int i = 0; i < 9; i++)
            {
                string dig_cpf = cpf[i].ToString();
                soma1 += Convert.ToInt32(dig_cpf) * (10 - i);
                soma2 += Convert.ToInt32(dig_cpf) * (11 - i);
            }

            int dv1 = (11 - (soma1 % 11));
            dv1 = dv1 >= 10 ? 0 : dv1;
            soma2 = soma2 + (dv1 * 2);
            int dv2 = (11 - (soma2 % 11));
            dv2 = dv2 >= 10 ? 0 : dv2;
            string dv = cpf.Substring(9, 2);

            return dv.Equals(dv1.ToString() + dv2.ToString());
        }

        /// <summary>
        /// Retira formatação do telefone
        /// </summary>
        /// <param name="telefone">telefone com formatação</param>
        /// <returns>telefone sem formatação</returns>
        public static string RetirarFormatacaoTelefone(string telefone)
        {
            return telefone.Replace("-", String.Empty);
        }

        #region FormatarCidadeUF
        public static string FormatarCidadeUF(string cidade, string uf)
        {
            return String.Format(@"{0}/{1}", cidade, uf);
        }
        #endregion

        /// <summary>
        /// Formata telefone com -
        /// </summary>
        /// <param name="p">telefone sem formatação</param>
        /// <returns>telefone formatado com "-"</returns>
        public static string FormatarTelefone(string telefone)
        {
            telefone = telefone.Trim();

            if (Regex.IsMatch(telefone, @"^(\d{5})(\d{4})$"))
                return Regex.Replace(telefone, @"^(\d{5})(\d{4})$", "$1-$2");

            if (Regex.IsMatch(telefone, @"^(\d{4})(\d{4})$"))
                return Regex.Replace(telefone, @"^(\d{4})(\d{4})$", "$1-$2");

            throw new ArgumentException("Telefone inválido", "telefone");
        }

        /// <summary>
        /// Formata CPF com ponto e traço
        /// </summary>
        /// <param name="cpf">CPF a ser formatado</param>
        /// <returns>CPF no formato 000.000.000-00</returns>
        public static string FormatarCPF(decimal cpf)
        {
            string cpfString = cpf.ToString("00000000000");

            if (cpfString.Length != 11)
                throw new ArgumentException("O CPF deve conter 11 dígitos", "cpf");

            return String.Format("{0}.{1}.{2}-{3}", 
                cpfString.Substring(0, 3), cpfString.Substring(3, 3), cpfString.Substring(6, 3), cpfString.Substring(9)); 
        }

        /// <summary>
        /// Formatar salário com duas casas decimais e separador de milhar
        /// </summary>
        /// <param name="salario">Salário a ser formatado</param>
        /// <returns>Salário formatado</returns>
        public static string FormatarSalario(decimal? salario)
        {
            string salarioString = salario.HasValue ? salario.Value.ToString("N2", CultureInfo.GetCultureInfo("pt-br")) : String.Empty;

            return salarioString;
        }
    }
}