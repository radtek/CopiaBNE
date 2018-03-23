
using BNE.BLL.Common;

namespace BNE.BLL.Custom
{
    public class GerarContratoPlano
    {

        public static byte[] ContratoPadraoPdf(string razaoSocial, decimal numeroCNPJ, string lougradouro, string numeroRua, string estado, string cidade, string cep, string nomePessoaResponsavel, string numRG, decimal numeroCPFResponsavel, decimal valorPlano, int numeroParcelas, int tempoPlano, int quantidadeSms, int quantidadeSmsTanque, int quantidadeUsuarios)
        {
            string descricaoQuantidadaSMS = string.Empty;
            if (quantidadeSms > 0)
                descricaoQuantidadaSMS += quantidadeSms + " sms por mês;";

            if (quantidadeSmsTanque > 0)
                descricaoQuantidadaSMS += quantidadeSmsTanque + " sms por dia;";

            var parametros = new
            {
                NumeroCNPJ = numeroCNPJ,
                RazaoSocial = razaoSocial,
                Logradouro = lougradouro,
                NumeroRua = numeroRua,
                Cidade = Helper.FormatarCidade(cidade, estado),
                NumeroCEP = cep,
                NomeResponsavelEmpresa = nomePessoaResponsavel,
                CPFResponsavelEmpresa = Helper.FormatarCPF(numeroCPFResponsavel),
                ValorPlano = string.Format("{0:n2}",numeroParcelas * valorPlano),
                NumeroParcelas = numeroParcelas,
                ValorParcelas = string.Format("{0:n2}",valorPlano),
                QuantidadeUsuarios = quantidadeUsuarios,
                QuantidadeSMS = descricaoQuantidadaSMS,
                ValorTempoPlano = tempoPlano,
            };

            var template = TemplateContrato.RecuperarContratoVigente(Enumeradores.TemplateContrato.TemplateGeral).DescricaoTemplateContrato;
            return ContratoPadraoPdf(parametros.ToString(template));
        }

        public static byte[] ContratoPadraoPdf_PlanoRecorrenteCia(string razaoSocial, decimal numeroCNPJ, string lougradouro, string numeroRua, string estado, string cidade, string cep, string nomePessoaResponsavel, string numRG, decimal numeroCPFResponsavel, decimal valorPlano, int quantidadeVisualizacoes, int quantidadeSms, int quantidadeUsuarios, Enumeradores.TemplateContrato templateContrato)
        {
            string descricaoQuantidadeSMS = string.Empty;
            if (quantidadeSms > 0)
                descricaoQuantidadeSMS += quantidadeSms + " sms por mês;";

            string descricaoQuantidadeVisualizacoes = "Visualização de " + quantidadeVisualizacoes + " currículos de forma limitada e anúncio de vagas ilimitado;";
            string usuariosAceite = string.Empty;

            var parametros = new
            {
                NumeroCNPJ = numeroCNPJ,
                RazaoSocial = razaoSocial,
                Logradouro = lougradouro,
                NumeroRua = numeroRua,
                Cidade = Helper.FormatarCidade(cidade, estado),
                NumeroCEP = cep,
                NomeResponsavelEmpresa = nomePessoaResponsavel,
                CPFResponsavelEmpresa = Helper.FormatarCPF(numeroCPFResponsavel),
                ValorPlano = string.Format("{0:n2}",valorPlano),
                QuantidadeUsuarios = quantidadeUsuarios,
                TextQuantidadeVisualizacoes = descricaoQuantidadeVisualizacoes,
                QuantidadeSMS = descricaoQuantidadeSMS,
                UsuariosAceite = usuariosAceite
            };

            var template = TemplateContrato.RecuperarContratoVigente(templateContrato).DescricaoTemplateContrato;
            return ContratoPadraoPdf(parametros.ToString(template)); 
        }

        public static byte[] ContratoPadraoPdf(string contrato)
        {
            string resultado = contrato;

            resultado = PDF.RetornarApenasConteudoHtml(resultado, null);

            var pdf = PDF.RecuperarPDFUsandoTextSharp(resultado);

            return pdf;
        }
    }
}
