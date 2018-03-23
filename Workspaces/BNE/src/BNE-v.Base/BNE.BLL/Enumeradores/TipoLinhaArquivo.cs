namespace BNE.BLL.Enumeradores
{
    enum TipoLinhaArquivo
    {
        /// <summary>
        /// Primeira linha dos arquivos
        /// </summary>
        Header = 0,
        /// <summary>
        /// Última linha dos arquivos
        /// </summary>
        Footer = 1,
        /// <summary>
        /// Linha de confirmação de pagamentos de boletos NÃO registrados
        /// </summary>
        BoletoCNR = 2,
        /// <summary>
        /// Linha de inclusão de débito em conta do HSBC
        /// </summary>
        RegistroDebitoHSBC = 3,
        /// <summary>
        /// Linha de Retorno de débito em conta do HSBC
        /// </summary>
        RetornoDebitoHSBC = 4,
        /// <summary>
        /// Linha de confirmação de processamento de arquivo de débito do HSBC
        /// </summary>
        ProcessamentoDebitoHSBC = 5,
        /// <summary>
        /// Linha de envio de boleto para registro
        /// </summary>
        RemessaRegistroBoletoHSBC = 6,
        /// <summary>
        /// Linha de retorno de registro de boleto
        /// </summary>
        RetornoRegistroBoletoHSBC = 7,
        /// <summary>
        /// Linha de retorno de registro de boleto
        /// </summary>
        RemessaRegistroDebitoBB = 8,
    }
}
