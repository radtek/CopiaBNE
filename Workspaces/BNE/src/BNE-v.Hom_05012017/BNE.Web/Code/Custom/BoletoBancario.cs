using System;
using System.Collections.Generic;
using System.Drawing;
using BNE.BLL;
using EnumeradoresBNE = BNE.BLL.Enumeradores;
using BNE.BLL.Security;
using System.IO;
using System.Data.SqlClient;

namespace BNE.Web.Code.Custom
{
    public class BoletoBancario
    {

        #region GerarBoleto
        /// <summary>
        /// Metodo responsável por gerar o boleto e  criar o pagamento
        /// </summary>
        /// <param name="objPagamento">Object</param>
        /// <param name="cancelarBoletoGerado">Se o usuário tiver um boleto gerado, mas se esse boleto já estiver vencido, o pagamento desse boleto é cancelado e é criado um novo pagamento com um novo boleto</param>
        /// <param name="idPlano">id do Plano</param>
        /// <param name="dtaVencimento">Se este metodo for chamado da sala do administrador,o parametro dtaVencimento deve ser informado e assim a geração do boleto perde a regra de vir a quantidade de dias por parametro</param>
        /// <returns></returns>
        /// 
        public static string GerarBoleto(SqlTransaction trans, BLL.Pagamento objPagamento, DateTime? dtaVencimento, Plano objPlano = null)
        {
            try
            {

                #region Variaveis

                PlanoAdquirido pa = PlanoAdquirido.CarregarPlanoAdquiridopDePagamento(objPagamento.IdPagamento);

                string strErro = null;
                byte[] byteImagem = null;

                String email = String.Empty;
                String descricaoLogradouro = String.Empty;
                String numeroEndereco = String.Empty;
                String descricaoComplemento = String.Empty;
                String numeroCEP = String.Empty;
                String descricaoBairro = String.Empty;
                String nossoNumero = String.Empty;
                String codigoBarras = String.Empty;
                decimal? CNPJ = null;
                decimal? CPF = null;
                String strNomeRazao = String.Empty;
                int idCidade = 0;


                //Se o plano adquirido estiver configurado para registro de boleto e não é a primeira parcela do plano,
                //o boleto será registrado
                bool registraBoleto = pa.FlagBoletoRegistrado && (objPagamento.PlanoParcela != null && objPagamento.PlanoParcela.NumeroParcela() > 1);

                #endregion

                #region Informações do boleto
                if (objPagamento.Filial != null)
                {
                    objPagamento.Filial.CompleteObject(trans);
                    CNPJ = objPagamento.Filial.NumeroCNPJ.Value;
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

                    CPF = objPagamento.UsuarioFilialPerfil.PessoaFisica.CPF;
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

                objPagamento.TipoPagamento = new TipoPagamento((int)EnumeradoresBNE.TipoPagamento.BoletoBancario);
                objPagamento.Save(trans);
                #endregion

                //Carregando os parametros para o boleto
                List<EnumeradoresBNE.Parametro> parametros = new List<EnumeradoresBNE.Parametro>();
                parametros.Add(EnumeradoresBNE.Parametro.CobreBemXCodigoAgencia);
                parametros.Add(EnumeradoresBNE.Parametro.CobreBemXNumeroContaCorrente);
                parametros.Add(EnumeradoresBNE.Parametro.CobreBemXCodigoCedente);
                parametros.Add(EnumeradoresBNE.Parametro.CobreBemXCodBanco);
                parametros.Add(EnumeradoresBNE.Parametro.NumCnpjBNE);

                Dictionary<EnumeradoresBNE.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

                String codigoAgencia = valoresParametros[EnumeradoresBNE.Parametro.CobreBemXCodigoAgencia];
                String[] conta = valoresParametros[EnumeradoresBNE.Parametro.CobreBemXNumeroContaCorrente].Split('-');
                String codigoCedente = valoresParametros[EnumeradoresBNE.Parametro.CobreBemXCodigoCedente];
                String codigoBanco = valoresParametros[EnumeradoresBNE.Parametro.CobreBemXCodBanco];
                String cnpjBNE = valoresParametros[EnumeradoresBNE.Parametro.NumCnpjBNE];

                using (var objTransacao = new wsTransacao.wsTRANSACAO())
                {

                    ServiceAuth.GerarHashAcessoWS(objTransacao);


                    var textoRodapeBoleto = Parametro.RecuperaValorParametro(EnumeradoresBNE.Parametro.TextoRodapeBoleto);

                    string strResult = objTransacao.CobrancaBoleto_Novo("bne",
                        (int)EnumeradoresBNE.Sistema.BNE,
                        textoRodapeBoleto,
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
                        objPagamento.IdPagamento.ToString(),
                        CPF,
                        CNPJ,
                        string.Empty,
                        out strErro,
                        out byteImagem,
                        out codigoBarras,
                        out nossoNumero);

                    objTransacao.RetornoTipoBoleto("bne", strResult, out nossoNumero, out codigoBarras);

                    objPagamento.CodigoGuid = strResult;
                    objPagamento.DescricaoIdentificador = nossoNumero;
                    objPagamento.DescricaoDescricao = codigoBarras;
                    objPagamento.Save(trans);
                }

                ImageConverter ic = new ImageConverter();
                Bitmap bmp = new Bitmap((System.Drawing.Image)ic.ConvertFrom(byteImagem));
                string strNomeArquivo = string.Format("boleto_{0}.jpg", DateTime.Now.Ticks);
                bmp.Save(string.Format("{0}\\{1}", System.Web.HttpContext.Current.Server.MapPath("ArquivosTemporarios"), strNomeArquivo), System.Drawing.Imaging.ImageFormat.Jpeg);

                return string.Format("ArquivosTemporarios/{0}", strNomeArquivo);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw ex;
            }
        }

        public static string GerarBoleto(BLL.Pagamento objPagamento, DateTime? dtaVencimento, Plano objPlano = null)
        {
            //TODO: Que merda de código
            using (SqlConnection conn = new SqlConnection(BLL.DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {

                        #region Variaveis

                        if (objPlano == null)
                        {
                            objPlano = Plano.CarregarPlanoDePagamento(objPagamento.IdPagamento);
                        }

                        PlanoAdquirido pa = PlanoAdquirido.CarregarPlanoAdquiridopDePagamento(objPagamento.IdPagamento);

                        string strErro = null;
                        byte[] byteImagem = null;

                        String email = String.Empty;
                        String descricaoLogradouro = String.Empty;
                        String numeroEndereco = String.Empty;
                        String descricaoComplemento = String.Empty;
                        String numeroCEP = String.Empty;
                        String descricaoBairro = String.Empty;
                        String nossoNumero = String.Empty;
                        String codigoBarras = String.Empty;
                        decimal? CNPJ = null;
                        decimal? CPF = null;
                        String strNomeRazao = String.Empty;
                        int idCidade = 0;


                        //Se o plano adquirido estiver configurado para registro de boleto e não é a primeira parcela do plano,
                        //o boleto será registrado
                        bool registraBoleto = pa.FlagBoletoRegistrado && (objPagamento.PlanoParcela != null && objPagamento.PlanoParcela.NumeroParcela() > 1);

                        #endregion

                        #region Informações do boleto
                        if (objPagamento.Filial != null)
                        {
                            objPagamento.Filial.CompleteObject(trans);
                            CNPJ = objPagamento.Filial.NumeroCNPJ.Value;
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

                            CPF = objPagamento.UsuarioFilialPerfil.PessoaFisica.CPF;
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

                        objPagamento.TipoPagamento = new TipoPagamento((int)EnumeradoresBNE.TipoPagamento.BoletoBancario);
                        objPagamento.Save(trans);
                        #endregion

                        //Carregando os parametros para o boleto
                        List<EnumeradoresBNE.Parametro> parametros = new List<EnumeradoresBNE.Parametro>();
                        parametros.Add(EnumeradoresBNE.Parametro.CobreBemXCodigoAgencia);
                        parametros.Add(EnumeradoresBNE.Parametro.CobreBemXNumeroContaCorrente);
                        parametros.Add(EnumeradoresBNE.Parametro.CobreBemXCodigoCedente);
                        parametros.Add(EnumeradoresBNE.Parametro.CobreBemXCodBanco);
                        parametros.Add(EnumeradoresBNE.Parametro.NumCnpjBNE);

                        Dictionary<EnumeradoresBNE.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

                        String codigoAgencia = valoresParametros[EnumeradoresBNE.Parametro.CobreBemXCodigoAgencia];
                        String[] conta = valoresParametros[EnumeradoresBNE.Parametro.CobreBemXNumeroContaCorrente].Split('-');
                        String codigoCedente = valoresParametros[EnumeradoresBNE.Parametro.CobreBemXCodigoCedente];
                        String codigoBanco = valoresParametros[EnumeradoresBNE.Parametro.CobreBemXCodBanco];
                        String cnpjBNE = valoresParametros[EnumeradoresBNE.Parametro.NumCnpjBNE];

                        using (var objTransacao = new wsTransacao.wsTRANSACAO())
                        {

                            ServiceAuth.GerarHashAcessoWS(objTransacao);


                            var textoRodapeBoleto = Parametro.RecuperaValorParametro(EnumeradoresBNE.Parametro.TextoRodapeBoleto);

                            string strResult = objTransacao.CobrancaBoleto_Novo("bne",
                                (int)EnumeradoresBNE.Sistema.BNE,
                                textoRodapeBoleto,
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
                                objPagamento.IdPagamento.ToString(),
                                CPF,
                                CNPJ,
                                string.Empty,
                                out strErro,
                                out byteImagem,
                                out codigoBarras,
                                out nossoNumero);

                            objTransacao.RetornoTipoBoleto("bne", strResult, out nossoNumero, out codigoBarras);

                            objPagamento.CodigoGuid = strResult;
                            objPagamento.DescricaoIdentificador = nossoNumero;
                            objPagamento.DescricaoDescricao = codigoBarras;
                            objPagamento.Save(trans);
                        }

                        ImageConverter ic = new ImageConverter();
                        Bitmap bmp = new Bitmap((System.Drawing.Image)ic.ConvertFrom(byteImagem));
                        string strNomeArquivo = string.Format("boleto_{0}.jpg", DateTime.Now.Ticks);
                        bmp.Save(string.Format("{0}\\{1}", System.Web.HttpContext.Current.Server.MapPath("ArquivosTemporarios"), strNomeArquivo), System.Drawing.Imaging.ImageFormat.Jpeg);

                        trans.Commit();

                        return string.Format("ArquivosTemporarios/{0}", strNomeArquivo);
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        EL.GerenciadorException.GravarExcecao(ex);
                        throw ex;
                    }
                }
            }
        }
        #endregion

        #region AjustarVencimentoBoleto
        /// <summary>
        /// Ajusta o vencimento do boleto para os planos ilimitador 
        /// </summary>
        private static void AjustarVencimentoBoleto(BLL.Pagamento objPagamento, int? idPlano, DateTime? dtaVencimento)
        {
            int diasAdicionais;
            if (dtaVencimento.HasValue)
            {
                //Tira as horas da data para funcionar a subtração
                string dataAtualSemHora = DateTime.Now.ToShortDateString();
                DateTime diaAtual = Convert.ToDateTime(dataAtualSemHora);
                DateTime diaInformado = Convert.ToDateTime(dtaVencimento);
                TimeSpan dias = diaInformado.Subtract(diaAtual);
                diasAdicionais = Feriado.RetornarDiaUtilVencimento(DateTime.Now.AddDays(dias.TotalDays));
                objPagamento.DataVencimento = diaInformado.AddDays(diasAdicionais);
            }
            else
            {
                if (idPlano.HasValue)
                {
                    int diasVencimentoParametro;
                    if (objPagamento.Filial != null)
                    {
                        diasVencimentoParametro = Convert.ToInt32(Parametro.RecuperaValorParametro(EnumeradoresBNE.Parametro.QuantidadeDiasVencimentoAposEmissaoBoletoPJ));
                        diasAdicionais = Feriado.RetornarDiaUtilVencimento(DateTime.Now.AddDays(diasVencimentoParametro));
                        objPagamento.DataVencimento = DateTime.Today.AddDays(diasVencimentoParametro + diasAdicionais);
                    }
                    else
                    {
                        diasVencimentoParametro = Convert.ToInt32(Parametro.RecuperaValorParametro(EnumeradoresBNE.Parametro.QuantidadeDiasVencimentoAposEmissaoBoletoPF));
                        diasAdicionais = Feriado.RetornarDiaUtilVencimento(DateTime.Now.AddDays(diasVencimentoParametro));
                        objPagamento.DataVencimento = DateTime.Today.AddDays(diasVencimentoParametro + diasAdicionais);
                    }
                }
            }
        }

        #endregion

        #region RetornarBoleto
        public static byte[] RetornarBoleto(SqlTransaction trans, BLL.Pagamento objPagamento, bool gerarPDF)
        {
            wsTransacao.wsTRANSACAO objTransacao = new wsTransacao.wsTRANSACAO();
            ServiceAuth.GerarHashAcessoWS(objTransacao);

            #region GeraGuidPagamento
            if (objPagamento.CodigoGuid == null)
                GerarBoleto(trans, objPagamento, objPagamento.DataVencimento);
            #endregion

            return objTransacao.DevolveBoleto("bne", objPagamento.CodigoGuid, gerarPDF);
        }

        public static byte[] RetornarBoleto(BLL.Pagamento objPagamento, bool gerarPDF)
        {
            wsTransacao.wsTRANSACAO objTransacao = new wsTransacao.wsTRANSACAO();
            ServiceAuth.GerarHashAcessoWS(objTransacao);

            #region GeraGuidPagamento
            if (objPagamento.CodigoGuid == null)
                GerarBoleto(objPagamento, objPagamento.DataVencimento);
            #endregion

            return objTransacao.DevolveBoleto("bne", objPagamento.CodigoGuid, gerarPDF);
        }
        #endregion

        #region RetornarBoletoImagem
        public static string RetornarBoletoImagem(SqlTransaction trans, BLL.Pagamento objPagamento)
        {
            byte[] byteImagem = RetornarBoleto(trans, objPagamento, false);

            string strNomeArquivo = string.Format("boleto_{0}.jpg", DateTime.Now.Ticks);
            return GerarLinkDownload(byteImagem, strNomeArquivo);
        }

        public static string RetornarBoletoImagem(BLL.Pagamento objPagamento)
        {
            byte[] byteImagem = RetornarBoleto(objPagamento, false);

            string strNomeArquivo = string.Format("boleto_{0}.jpg", DateTime.Now.Ticks);
            return GerarLinkDownload(byteImagem, strNomeArquivo);
        }
        #endregion

        #region RetornarBoletoPDF

        public static string RetornarBoletoPDF(SqlTransaction trans, BLL.Pagamento objPagamento)
        {
            byte[] byteArray = RetornarBoleto(trans, objPagamento, true);

            string strNomeArquivo = string.Format("boleto_{0}.pdf", DateTime.Now.Ticks);

            return GerarLinkDownload(byteArray, strNomeArquivo);
        }

        public static string RetornarBoletoPDF(BLL.Pagamento objPagamento)
        {
            byte[] byteArray = RetornarBoleto(objPagamento, true);

            string strNomeArquivo = string.Format("boleto_{0}.pdf", DateTime.Now.Ticks);

            return GerarLinkDownload(byteArray, strNomeArquivo);
        }

        #endregion

        #region GerarLinkDownload
        /// <summary>
        /// Passa a string para gerar o arquivo para a tela de geração de arquivo (DownloadArquivo.aspx)
        /// </summary>
        /// <param name="strFinal">String de dados que devem estar no arquivo</param>
        /// <param name="nomeArquivo">Nome do arquivo a ser gerado</param>
        private static string GerarLinkDownload(byte[] arrayFinal, string nomeArquivo)
        {
            string caminhoArquivo = String.Format("{0}{1}", Resources.Configuracao.PastaArquivoTemporario, nomeArquivo);
            string caminhoNomeArquivo = System.Web.HttpContext.Current.Server.MapPath(caminhoArquivo);

            FileStream fs = null;
            try
            {
                fs = new FileStream(caminhoNomeArquivo, FileMode.CreateNew, FileAccess.Write);
                fs.Write(arrayFinal, 0, arrayFinal.Length);
                fs.Close();
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }

            return caminhoArquivo;
        }
        #endregion

    }
}