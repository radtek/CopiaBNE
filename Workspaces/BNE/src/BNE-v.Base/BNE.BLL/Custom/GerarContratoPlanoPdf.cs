using System.Collections.Specialized;
using System.Globalization;

namespace BNE.BLL.Custom
{
    public class GerarContratoPlano
    {
        public static byte[] ContratoPadraoPdf(string razaoSocial, string numCNPJ, string descRua, string numeroRua, string estado, string nomeCidade, string numCEP, string nomePessoa, string numRG, decimal numCPF, decimal valorPlano, int numeroParcelas, int tempoPlano, int quantidadeSms, int quantidadeUsuarios)
        {

            string urlAmbiente = BLL.Custom.Helper.RecuperarURLAmbiente();
            string urlRequisicao = string.Format("http://{0}/ContratoCia.aspx", urlAmbiente);

            var postData = new NameValueCollection();

            postData.Add("RazaoSocial", razaoSocial);
            postData.Add("NumeroCNPJ", numCNPJ);
            postData.Add("DescricaoRua", descRua);
            postData.Add("NumeroRua", numeroRua);
            postData.Add("Estado", estado);
            postData.Add("CEP", numCEP);
            postData.Add("NomePessoa", nomePessoa);
            postData.Add("NumeroRG", numRG);
            postData.Add("NumeroCPF", numCPF.ToString());
            postData.Add("ValorPlano", valorPlano.ToString("0,0.00", CultureInfo.CurrentCulture));
            postData.Add("NumeroParcelas", numeroParcelas.ToString(CultureInfo.InvariantCulture));
            postData.Add("NomeCidade", nomeCidade);
            postData.Add("TempoPlano", tempoPlano.ToString(CultureInfo.InvariantCulture));
            postData.Add("QuantidadeSms", quantidadeSms.ToString());
            postData.Add("QuantidadeUsuarios", quantidadeUsuarios.ToString());

            const string contentType = "application/x-www-form-urlencoded";

            string resultado = BNE.BLL.Custom.Helper.EfetuarRequisicao(urlRequisicao, postData, contentType);

            resultado = BLL.Custom.PDF.RetornarApenasConteudoHtml(resultado, null);

            var pdf = BLL.Custom.PDF.RecuperarPDFUsandoTextSharp(resultado);

            return pdf;
        }

    }
}
