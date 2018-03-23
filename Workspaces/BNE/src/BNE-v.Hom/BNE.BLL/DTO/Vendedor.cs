using BNE.BLL.Custom;
using System.Text;

namespace BNE.BLL.DTO
{
    public class Vendedor
    {
        public string NomeVendedor { get; set; }
        public string EmailVendedor { get; set; }
        public string NumeroDDD { get; set; }
        public string NumeroTelefone { get; set; }

        public string ToMailSignature()
        {
            var sb = new StringBuilder();

            sb.AppendFormat("{0}<br>", NomeVendedor);

            if (!string.IsNullOrEmpty(EmailVendedor))
                sb.AppendFormat("{0}<br>", EmailVendedor);

            if (!string.IsNullOrEmpty(NumeroTelefone))
                sb.AppendFormat("{0}<br>", Helper.FormatarTelefone(NumeroDDD, NumeroTelefone));

            return sb.ToString();
        }
    }
}
