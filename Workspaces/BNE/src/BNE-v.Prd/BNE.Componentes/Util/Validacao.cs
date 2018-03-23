using System;

namespace BNE.Componentes.Util
{
    public class Validacao
    {
        /*
        #region ValidarCPF
        public static bool ValidarCPF(string cpf)
        {
            cpf = Formatadores.SomenteNumeros(cpf);

            // Caso coloque todos os numeros iguais
            switch (cpf)
            {
                case "11111111111":
                    return false;
                case "00000000000":
                    return false;
                case "2222222222":
                    return false;
                case "33333333333":
                    return false;
                case "44444444444":
                    return false;
                case "55555555555":
                    return false;
                case "66666666666":
                    return false;
                case "77777777777":
                    return false;
                case "88888888888":
                    return false;
                case "99999999999":
                    return false;
            }

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }
        #endregion
        */
        #region ValidarCNPJ
        public static bool ValidarCNPJ(String cnpj)
        {
            string CNPJ = Formatadores.SomenteNumeros(cnpj);

            int[] digitos, soma, resultado;
            int nrDig;
            string ftmt;
            bool[] CNPJOk;

            ftmt = "6543298765432";
            digitos = new int[14];
            soma = new int[2];
            soma[0] = 0;
            soma[1] = 0;
            resultado = new int[2];
            resultado[0] = 0;
            resultado[1] = 0;
            CNPJOk = new bool[2];
            CNPJOk[0] = false;
            CNPJOk[1] = false;

            try
            {
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = int.Parse(
                        CNPJ.Substring(nrDig, 1));
                    if (nrDig <= 11)
                        soma[0] += (digitos[nrDig] *
                          int.Parse(ftmt.Substring(
                          nrDig + 1, 1)));
                    if (nrDig <= 12)
                        soma[1] += (digitos[nrDig] *
                          int.Parse(ftmt.Substring(
                          nrDig, 1)));
                }

                for (nrDig = 0; nrDig < 2; nrDig++)
                {
                    resultado[nrDig] = (soma[nrDig] % 11);
                    if ((resultado[nrDig] == 0) || (
                         resultado[nrDig] == 1))
                        CNPJOk[nrDig] = (
                        digitos[12 + nrDig] == 0);
                    else
                        CNPJOk[nrDig] = (
                        digitos[12 + nrDig] == (
                        11 - resultado[nrDig]));
                }
                return (CNPJOk[0] && CNPJOk[1]);
            }
            catch
            {
                return false;
            }


        }
        #endregion
        /*
        #region ValidarCEI
        public static bool ValidarCEI(string valor)
        {
            if (string.IsNullOrEmpty(valor))
                return false;

            Regex rx = new Regex(@"([^\.//]+)[\.//]*");
            if (rx.IsMatch(valor))
                valor = rx.Replace(valor, "$1");

            Int64 vlr;
            if (valor.Length != 12 || !Int64.TryParse(valor, out vlr))
                return false;

            if (!ValidarCalculoCEI(valor))
                return false;

            return true;
        }

        /// <summary>
        /// Calcula o digito verificador de um número
        /// de CEI.
        /// </summary>
        /// <param name="cei">string com o número do cei sem formatação</param>
        /// <returns>Digito Verificador</returns>
        private static bool ValidarCalculoCEI(string cei)
        {
            //variáveis

            //Contantes para efeito de cálculo
            int[] ceiConstante = { 7, 4, 1, 8, 5, 2, 1, 6, 3, 7, 4 };

            //Calcula a soma dos digitos multiplicados pelo seu correspondente
            //no vetor de constantes para o calculo.
            int soma = 0;
            for (int i = 0; i < 11; i++)
                soma += Convert.ToInt32(cei[i].ToString()) * ceiConstante[i];

            //Com a variável soma calculada, é possível calcular os valores das
            //variáveis unidades e dezena.
            int unidade = soma % 10;
            int dezena = (soma % 100) - unidade;

            //Calculo do digito verificador.
            int digitoVerificador = 10 - (((unidade + (dezena / 10)) % 10));

            //Caso o digito calculado for maior que 9, o valor do dígito é 0. 
            if (digitoVerificador > 9)
                digitoVerificador = 0;

            return cei[11].Equals(Convert.ToChar(digitoVerificador.ToString()));
        }
        #endregion
         * */
    }
}
