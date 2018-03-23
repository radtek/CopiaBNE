using System;
using System.Web.Security;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.BLL.Custom.Email;

namespace SistMars
{
    /// <summary>
    /// Funções gerais
    /// </summary>
    public class util
    {
        #region Construtor

        public util()
        {

        }

        #endregion

        #region EnviarEmailErro
        /// <summary>
        /// Envia email de erro para o administrador do sistema
        /// </summary>
        /// <param name="htmlBody">corpo da mensagem</param>
        public void EnviarEmailErro(string htmlBody)
        {
            string from = Convert.ToString(BNE.BLL.Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens, null));
            string to = "gieyson@bne.com.br; tortato@bne.com.br";
            string body = htmlBody;
            string subject = "ERRO do Sistema";

            EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(subject, body, null, from, to);
        }
        #endregion

        #region RetornarTrimestre

        public byte RetornarTrimestre(DateTime data)
        {
            int dia = data.Day;
            int mes = data.Month;
            byte tri = 0;

            if (mes > 3 && mes < 6)
                tri = 1;
            else if (mes > 6 && mes < 9)
                tri = 2;
            else if (mes > 9 && mes < 12)
                tri = 3;
            else if (mes >= 1 && mes < 3)
                tri = 4;
            else if (mes == 3)
                if (dia < 22)
                    tri = 4;
                else
                    tri = 1;
            else if (mes == 6)
                if (dia < 22)
                    tri = 1;
                else
                    tri = 2;
            else if (mes == 9)
                if (dia < 24)
                    tri = 2;
                else
                    tri = 3;
            else if (mes == 12)
                if (dia < 22)
                    tri = 3;
                else
                    tri = 4;

            return tri;
        }

        #endregion

        #region RetornarLetraNumero

        public string RetornarLetraNumero(byte numero)
        {
            string letra = "";
            switch (numero)
            {
                case 1: letra = "A"; break;
                case 2: letra = "B"; break;
                case 3: letra = "C"; break;
                case 4: letra = "D"; break;
                case 5: letra = "E"; break;
                case 6: letra = "F"; break;
                case 7: letra = "G"; break;
                case 8: letra = "H"; break;
            }
            return letra;
        }

        #endregion

        #region EncriptarSenha
        /// <summary>
        /// Encripta senha
        /// </summary>
        /// <param name="senha">Senha a ser encriptada.</param>
        /// <returns>Retorna a senha encriptada.</returns>
        public string EncriptarSenha(string Senha)
        {
            string strHash, strFormat;

            strFormat = "SHA1"; // "SHA1" ou "MD5"
            strHash = FormsAuthentication.HashPasswordForStoringInConfigFile(Senha, strFormat);

            return strHash;
        }
        #endregion

        #region ValidarCPF
        /// <summary>
        /// Valida o CPF
        /// </summary>
        /// <param name="sCPF">CPF a ser validado</param>
        /// <param name="sDescricaoErro">Descrição do erro</param>
        /// <returns>Retorna TRUE se o CPF estiver OK e FALSE se tiver algum erro</returns>
        public bool ValidarCPF(string sCPF, out string sDescricaoErro)
        {
            try
            {
                sDescricaoErro = string.Empty;
                string sCPF1;
                string sCPF2;
                string sControle = string.Empty;
                int iSoma = 0;
                int iDigito = 0;

                //   'Valores-limite de i para o cálculo do primeiro iDigito

                int iContIni = 2;
                int iContFim = 10;

                if (sCPF == "11111111111" || sCPF == "22222222222" || sCPF == "33333333333" || sCPF == "44444444444" || sCPF == "55555555555" || sCPF == "66666666666" || sCPF == "77777777777" || sCPF == "88888888888" || sCPF == "99999999999" || sCPF == "00000000000")
                {
                    sDescricaoErro = "Err1";
                    return false;
                }

                if (string.IsNullOrEmpty(sCPF))
                {
                    sDescricaoErro = "Err2";
                    return false;
                }

                if (sCPF.Length != 11)
                {
                    sDescricaoErro = "Err3";
                    return false;
                }

                //   'Separa em 9 + 2 algarismos

                sCPF1 = sCPF.Substring(0, 9);
                sCPF2 = sCPF.Substring(9);

                for (int j = 1; j <= 2; j++)
                {
                    iSoma = 0;
                    for (int i = iContIni; i <= iContFim; i++)
                    {
                        iSoma += Convert.ToInt16(sCPF1.Substring(i - j - 1, 1)) * (iContFim + 1 + j - i);
                    }

                    if (j == 2)
                        iSoma += (2 * iDigito);

                    iDigito = (iSoma * 10) % 11;

                    if (iDigito == 10)
                        iDigito = 0;

                    sControle += Convert.ToString(iDigito);

                    //Valores-limite de i para o cálculo do segundo iDigito
                    iContIni = 3;
                    iContFim = 11;
                }

                //   Compara iDigitos calculados (sControle)
                //   com iDigitos informados (sCPF2) e
                //   informa o veredito da função

                if (sControle != sCPF2)
                {
                    sDescricaoErro = "Err1";
                    return false;
                }
                else
                    return true;
            }
            catch (Exception)
            {
                sDescricaoErro = "Err1";
                return false;
            }
        }

        #endregion
    }
}
