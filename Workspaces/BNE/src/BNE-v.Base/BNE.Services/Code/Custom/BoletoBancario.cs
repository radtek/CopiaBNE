using System;
using System.Collections.Generic;
using System.Globalization;
using BNE.BLL;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.BLL.Security;
using System.Data.SqlClient;

namespace BNE.Services.Code.Custom
{
    public class BoletoBancario
    {

        public static byte[] GerarBoleto(Pagamento objPagamento)
        {
            return GerarBoleto(objPagamento, null);
        }

        public static byte[] GerarBoleto(Pagamento objPagamento, SqlTransaction trans)
        {


            #region Variaveis

            string strErro;
            byte[] byteImagem;

            String email = String.Empty;
            String descricaoLogradouro = String.Empty;
            String numeroEndereco = String.Empty;
            String descricaoComplemento = String.Empty;
            String numeroCEP = String.Empty;
            String descricaoBairro = String.Empty;
            String nossoNumero = String.Empty;
            String codigoBarras;
            decimal? cpf= null;
            decimal? cnpj = null;
            String strNomeRazao;
            int idCidade = 0;

            Boolean registraBoleto = false;

            PlanoAdquirido pa = PlanoAdquirido.CarregarPlanoAdquiridopDePagamento(objPagamento.IdPagamento);
            //Se o plano adquirido estiver configurado para registro de boleto e não é a primeira parcela do plano,
            //o boleto será registrado
            if (pa.FlagBoletoRegistrado && objPagamento.PlanoParcela.NumeroParcela() > 1)
            {
                registraBoleto = true;
                nossoNumero = CobrancaBoleto.CarregarProximoNossoNumeroParaRegistrado();
            }

            #endregion

            #region Informações do boleto
            if (objPagamento.Filial != null)
            {
                objPagamento.Filial.CompleteObject(trans);
                if (objPagamento.Filial.NumeroCNPJ != null) cnpj = objPagamento.Filial.NumeroCNPJ.Value;
                strNomeRazao = objPagamento.Filial.RazaoSocial;

                UsuarioFilial objUsuarioFilial;
                if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objPagamento.UsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial))
                {
                    if (!String.IsNullOrEmpty(objUsuarioFilial.EmailComercial))
                        email = objUsuarioFilial.EmailComercial;
                }

                if (objPagamento.Filial.Endereco != null)
                {
                    objPagamento.Filial.Endereco.CompleteObject(trans);

                    idCidade = objPagamento.Filial.Endereco.Cidade.IdCidade;

                    if (!String.IsNullOrEmpty(objPagamento.Filial.Endereco.DescricaoLogradouro))
                        descricaoLogradouro = objPagamento.Filial.Endereco.DescricaoLogradouro;
                    if (!String.IsNullOrEmpty(objPagamento.Filial.Endereco.NumeroEndereco))
                        numeroEndereco = objPagamento.Filial.Endereco.NumeroEndereco;
                    if (!String.IsNullOrEmpty(objPagamento.Filial.Endereco.DescricaoComplemento))
                        descricaoComplemento = objPagamento.Filial.Endereco.DescricaoComplemento;
                    if (!String.IsNullOrEmpty(objPagamento.Filial.Endereco.NumeroCEP))
                        numeroCEP = objPagamento.Filial.Endereco.NumeroCEP;
                    if (!String.IsNullOrEmpty(objPagamento.Filial.Endereco.DescricaoBairro))
                        descricaoBairro = objPagamento.Filial.Endereco.DescricaoBairro;
                }
            }
            else
            {
                objPagamento.UsuarioFilialPerfil.CompleteObject(trans);
                objPagamento.UsuarioFilialPerfil.PessoaFisica.CompleteObject(trans);

                cpf = objPagamento.UsuarioFilialPerfil.PessoaFisica.CPF;
                strNomeRazao = objPagamento.UsuarioFilialPerfil.PessoaFisica.NomePessoa;

                if (!String.IsNullOrEmpty(objPagamento.UsuarioFilialPerfil.PessoaFisica.EmailPessoa))
                    email = objPagamento.UsuarioFilialPerfil.PessoaFisica.EmailPessoa;

                if (objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco != null)
                {
                    objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.CompleteObject(trans);

                    idCidade = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.Cidade.IdCidade;

                    if (!String.IsNullOrEmpty(objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoLogradouro))
                        descricaoLogradouro = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoLogradouro;
                    if (!String.IsNullOrEmpty(objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.NumeroEndereco))
                        numeroEndereco = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.NumeroEndereco;
                    if (!String.IsNullOrEmpty(objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoComplemento))
                        descricaoComplemento = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoComplemento;
                    if (!String.IsNullOrEmpty(objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.NumeroCEP))
                        numeroCEP = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.NumeroCEP;
                    if (!String.IsNullOrEmpty(objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoBairro))
                        descricaoBairro = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoBairro;
                }
            }
            #endregion

            //Carregando os parametros para o boleto
            var parametros = new List<Enumeradores.Parametro>
                {
                    Enumeradores.Parametro.CobreBemXCodigoAgencia, 
                    Enumeradores.Parametro.CobreBemXNumeroContaCorrente, 
                    Enumeradores.Parametro.CobreBemXCodigoCedente, 
                    Enumeradores.Parametro.CobreBemXCodBanco, 
                    Enumeradores.Parametro.NumCnpjBNE
                };

            Dictionary<Enumeradores.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

            String codigoAgencia = valoresParametros[Enumeradores.Parametro.CobreBemXCodigoAgencia];
            String[] conta = valoresParametros[Enumeradores.Parametro.CobreBemXNumeroContaCorrente].Split('-');
            String codigoCedente = valoresParametros[Enumeradores.Parametro.CobreBemXCodigoCedente];
            String codigoBanco = valoresParametros[Enumeradores.Parametro.CobreBemXCodBanco];
            String cnpjBNE = valoresParametros[Enumeradores.Parametro.NumCnpjBNE];

            var objTransacao = new wsTransacao.wsTRANSACAO();
            ServiceAuth.GerarHashAcessoWS(objTransacao);

            string strResult = objTransacao.CobrancaBoleto_Novo("bne",
                (int)Enumeradores.Sistema.BNE,
                "Local de pagamento: Até o vencimento preferencialmente no HSBC. Após o vencimento, Somente no HSBC. Todas as informações deste boleto são de exclusiva responsabilidade do cedente. Um dia útil após a confirmação do pagamento, você terá acesso ao serviço do BNE.",
                Convert.ToDecimal(cnpjBNE),
                codigoAgencia,
                codigoCedente,//convenio bancario
                conta[0],//numero conta
                conta[1],//digito verificador
                Convert.ToInt32(codigoBanco),
                "Bne - Banco Nacional de Empregos Ltda",
                registraBoleto,
                (DateTime)objPagamento.DataVencimento,
                objPagamento.ValorPagamento,
                strNomeRazao,
                email,
                "Boleto BNE",
                "",
                descricaoLogradouro,
                numeroEndereco,
                descricaoComplemento,
                idCidade,
                numeroCEP,
                descricaoBairro,
                "Todas as informações deste boleto são de exclusiva responsabilidade do cedente. Um dia útil após a confirmação do pagamento, você terá acesso ao serviço do BNE.",
                false,
                objPagamento.IdPagamento.ToString(CultureInfo.CurrentCulture),
                cpf,
                cnpj,
                nossoNumero,
                out strErro,
                out byteImagem,
				out codigoBarras,
				out nossoNumero);

            objTransacao.RetornoTipoBoleto("bne", strResult, out nossoNumero, out codigoBarras);

            objPagamento.CodigoGuid = strResult;
            objPagamento.DescricaoIdentificador = nossoNumero;
            objPagamento.DescricaoDescricao = codigoBarras;
            objPagamento.TipoPagamento = new TipoPagamento((int)Enumeradores.TipoPagamento.BoletoBancario);

            if (trans != null)
                objPagamento.Save(trans);
            else
                objPagamento.Save();

            return byteImagem;
        }

        public static byte[] RetornarBoleto(Pagamento objPagamento)
        {
            var objTransacao = new wsTransacao.wsTRANSACAO();
            ServiceAuth.GerarHashAcessoWS(objTransacao);

            return objTransacao.DevolveBoleto("bne", objPagamento.CodigoGuid, true);
        }

    }
}