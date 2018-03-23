using System;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using BNE.BLL.Custom;
using System.Text.RegularExpressions;

namespace BNE.BLL.Integracoes.Pagamento.CobreBem
{
    public class CartaoCredito : Pagamento.CartaoCredito
    {

        #region Construtor
        public CartaoCredito()
        {
            ParcelamentoAdministradora = 'N';
            Adquirente = "CIELO";
            PreAutorizacao = 'N';
        }
        #endregion

        #region ValidarTransacao
        public RetornoCartaoCredito ValidarTransacao(Transacao objTransacao, out string erro)
        {
            erro = string.Empty;
            try
            {
                var respostaAprovaFacil = AprovaFacil.Requisitar(this.MontarDadosPost());
                var retorno = ResultadoAPC.LerRetorno(respostaAprovaFacil);

                TransacaoResposta.SalvarResposta(objTransacao, retorno.Aprovada, retorno.ResultadoSolicitacaoAprovacao, retorno.CodigoAutorizacao, retorno.Transacao, retorno.CartaoMascarado, retorno.NumeroSequencial, retorno.ComprovanteAdministradora, retorno.NacionalidadeEmissor);

                TratarRetorno(retorno.CodigoResultadoAprovacao, retorno.ResultadoSolicitacaoAprovacao, out erro);

                if (!retorno.Aprovada && String.IsNullOrEmpty(erro))
                {
                    erro = Regex.Replace(retorno.ResultadoSolicitacaoAprovacao, @"\[[0-9]*\]","");
                }

                return new RetornoCartaoCredito
                {
                    Aprovado = retorno.Aprovada,
                    DescricaoTransacao = retorno.Transacao,
                    DescricaoAutorizacao = retorno.ResultadoSolicitacaoAprovacao,
                    CodigoAutorizacao = retorno.CodigoAutorizacao
                };
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
            return null;
        }
        #endregion

        #region CapturarTransacao
        public static RetornoCaptura CapturarTransacao(Transacao objTransacao, out string erro)
        {
            erro = string.Empty;
            var respostaAprovaFacil = AprovaFacil.Capturar(objTransacao.DescricaoTransacao);
            var retornoCaptura = ResultadoCAP.LerRetorno(respostaAprovaFacil);

            if (!retornoCaptura.CapturadaComSucesso)
            {
                erro = retornoCaptura.ResultadoSolicitacaoConfirmacao;
            }

            return new RetornoCaptura
            {
                Capturado = retornoCaptura.CapturadaComSucesso,
                DescricaoMensagemCaptura = retornoCaptura.ResultadoSolicitacaoConfirmacao
            };  
        }
        #endregion

        #region TratarRetorno
        private void TratarRetorno(string codigoResultadoAprovacao, string descricaoResultadoAprovacao, out string erro)
        {
            erro = string.Empty;

            if (!string.IsNullOrEmpty(codigoResultadoAprovacao))
                erro = TransacaoMensagemErro.RecuperarMensagemPorCodigoErro(codigoResultadoAprovacao);
            else if (!string.IsNullOrEmpty(descricaoResultadoAprovacao))
                erro = TransacaoMensagemErro.RecuperarMensagemPorDescricaoErro(descricaoResultadoAprovacao);
        }
        #endregion

        #region MontarDadosPost
        public string MontarDadosPost()
        {
            return this.ToString("NumeroDocumento={NumeroDocumento}&" +
                                 "ValorDocumento={ValorDocumento:0.00}&" +
                                 "QuantidadeParcelas={QuantidadeParcelas}&" +
                                 "NumeroCartao={NumeroCartao}&" +
                                 "MesValidade={MesValidade}&" +
                                 "AnoValidade={AnoValidade}&" +
                                 "CodigoSeguranca={CodigoSeguranca}&" +
                                 "EnderecoIPComprador={EnderecoIPComprador}&" +
                                 "Bandeira={Bandeira}&" +
                                 "Adquirente={Adquirente}&" +
                                 "ParcelamentoAdministradora={ParcelamentoAdministradora}", CultureInfo.InvariantCulture);
        }
        #endregion

        public class ResultadoAPC
        {
            //[XmlIgnore]
            public string TransacaoAprovada { get; set; }
            public string ResultadoSolicitacaoAprovacao { get; set; }
            public string CodigoAutorizacao { get; set; }
            public string Transacao { get; set; }
            public string CartaoMascarado { get; set; }
            public string CodigoResultadoAprovacao { get; set; }
            public string NumeroSequencialUnico { get; set; }
            public string ComprovanteAdministradora { get; set; }
            public string NacionalidadeEmissor { get; set; }

            public decimal? NumeroSequencial
            {
                get
                {
                    decimal aux;
                    if (Decimal.TryParse(NumeroSequencialUnico, out aux))
                        return aux;
                    return null;
                }
            }
            public bool Aprovada { get { return TransacaoAprovada.Equals("True"); } }

            public static ResultadoAPC LerRetorno(string retorno)
            {
                ResultadoAPC objRetornoTransacao = null;
                using (var sr = new StringReader(retorno))
                {
                    var serializer = new XmlSerializer(typeof(ResultadoAPC));
                    objRetornoTransacao = (ResultadoAPC)serializer.Deserialize(sr);

                    sr.Close();
                    sr.Dispose();
                }

                return objRetornoTransacao;
            }
        }

        public class ResultadoCAP
        {
            //[XmlIgnore]
            public string ResultadoSolicitacaoConfirmacao { get; set; }
            public string ComprovanteAdministradora { get; set; }

            public bool CapturadaComSucesso { get { return this.ResultadoSolicitacaoConfirmacao.Contains("Confirmado"); } }

            public static ResultadoCAP LerRetorno(string retorno)
            {
                ResultadoCAP objRetornoTransacao = null;
                using (var sr = new StringReader(retorno))
                {
                    var serializer = new XmlSerializer(typeof(ResultadoCAP));
                    objRetornoTransacao = (ResultadoCAP)serializer.Deserialize(sr);
                    
                    objRetornoTransacao.ResultadoSolicitacaoConfirmacao = objRetornoTransacao.ResultadoSolicitacaoConfirmacao.Replace("\n","");

                    sr.Close();
                    sr.Dispose();
                }

                return objRetornoTransacao;
            }
        }
    }
}
