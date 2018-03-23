using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using BNE.BLL.Custom.Email;

namespace BNE.BLL.Custom
{
    public class QueroContratarCurriculoEstudante
    {
        public enum BolsaTipo
        {
            ValorFixo,
            ValorPorHora
        }

        public BolsaTipo TipoBolsa { get; set; }
        public string NomeDaMae { get; set; }
        public decimal ValorBolsa { get; set; }
        public string Beneficios { get; set; }

        public bool EnviarEmailQueroContratar(int filialId, int curriculoId, out string errorMessage)
        {
            if (!ValidarDadosAtuais(out errorMessage))
            {
                return false;
            }
            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    return ProcessarEnvioDeEmailQueroContratar(filialId, curriculoId, out errorMessage, trans);
                }
            }
        }

        private bool ValidarDadosAtuais(out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(NomeDaMae))
            {
                errorMessage = "O nome da mãe é inválido.";
                return false;
            }

            if (ValorBolsa <= 0)
            {
                errorMessage = "O valor da bolsa é inválido.";
                return false;
            }
            errorMessage = string.Empty;
            return true;
        }

        private bool ProcessarEnvioDeEmailQueroContratar(int filialId, int curriculoId, out string errorMessage,
                                                                SqlTransaction trans)
        {
            Filial filial;
            Curriculo curriculo;
            if (!CarregarParametrosParaEnvioEmail(filialId, curriculoId, trans, out filial, out curriculo, out errorMessage))
                return false;

            ParametroFilial usaWebEstagiosParam;
            if (!ParametroFilial.CarregarParametroPorFilial(Enumeradores.Parametro.FilialParceiraWebEstagios, filial,
                                                            out usaWebEstagiosParam, trans))
                usaWebEstagiosParam = new ParametroFilial
                    {
                        IdFilial = filialId,
                        IdParametro = (int)Enumeradores.Parametro.FilialParceiraWebEstagios,
                        ValorParametro = "false"
                    };

            bool clienteOuParceiroWebEstagios;
            if (!bool.TryParse(usaWebEstagiosParam.ValorParametro, out clienteOuParceiroWebEstagios))
            {
                clienteOuParceiroWebEstagios = false;
            }

            var dadosConteudo = GerarConteudoDadosCurriculo(filial, curriculo, clienteOuParceiroWebEstagios);
            if (!clienteOuParceiroWebEstagios)
            {
                if (!DispararEmailEmpresaSemWebEstagios(dadosConteudo))
                {
                    errorMessage = "Falha ao enviar os dados (Code: DEQCESWE).";
                    return false;
                }
            }
            errorMessage = string.Empty;
            return DispararEmailEmpresaQueroContratar(dadosConteudo);
        }

        private bool CarregarParametrosParaEnvioEmail(int filialId, int curriculoId, SqlTransaction trans, out Filial filial, out Curriculo curriculo, out string errorMessage)
        {
            filial = new Filial(filialId);
            if (!filial.CompleteObject(trans))
            {
                errorMessage = "Falha ao consultar os dados atuais (empresa/filial).";
                curriculo = null;
                return false;
            }

            if (filial.Endereco != null)
                filial.Endereco.CompleteObject(trans);

            curriculo = new Curriculo(curriculoId);
            if (!curriculo.CompleteObject(trans))
            {
                errorMessage = "Falha ao consultar os dados atuais (currículo).";
                return false;
            }

            if (curriculo.PessoaFisica == null)
            {
                errorMessage = "Falha ao acessar os dados atuais (pessoa física).";
                return false;
            }

            if (!curriculo.PessoaFisica.CompleteObject(trans))
            {
                errorMessage = "Falha ao consultar os dados atuais (pessoa física).";
                return false;
            }

            if (curriculo.PessoaFisica.Endereco != null)
                curriculo.PessoaFisica.Endereco.CompleteObject(trans);

            errorMessage = string.Empty;
            return true;
        }

        private static bool DispararEmailEmpresaSemWebEstagios(ExpandoObject parametrosDinamicos)
        {
            const Enumeradores.Parametro cadastroWebEstagiosParam = Enumeradores.Parametro.EmailWebEstagiosIntegracaoCadastroEmpresa;
            var parms = new List<Enumeradores.Parametro>
             {
                 cadastroWebEstagiosParam
             };

            var valores = Parametro.ListarParametros(parms);
            var emailLst = valores[cadastroWebEstagiosParam];

            if (string.IsNullOrEmpty(emailLst))
                return true;

            const Enumeradores.CartaEmail cartaParam = Enumeradores.CartaEmail.ConteudoWebEstagiosIntegracaoQueroContratar;

            string assunto;
            string conteudo = CartaEmail.RetornarConteudoBNE(cartaParam, out assunto);

            if (string.IsNullOrEmpty(conteudo))
                return false;

            string mensagemEmail = parametrosDinamicos.ToString(conteudo);

            string emailRemetenteSistema = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);

            foreach (var item in emailLst.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                          .Enviar(assunto, mensagemEmail, emailRemetenteSistema, item);
            }

            return true;
        }

        private static bool DispararEmailEmpresaQueroContratar(ExpandoObject parametrosDinamicos)
        {
            const Enumeradores.Parametro queroContratarWebEstagios = Enumeradores.Parametro.EmailWebEstagiosQueroContratar;
            var parms = new List<Enumeradores.Parametro>
             {
                 queroContratarWebEstagios
             };

            var valores = Parametro.ListarParametros(parms);
            var emailLst = valores[queroContratarWebEstagios];

            if (string.IsNullOrEmpty(emailLst))
                return true;

            const Enumeradores.CartaEmail cartaParam = Enumeradores.CartaEmail.ConteudoWebEstagiosIntegracaoQueroContratar;

            string assunto;
            string conteudo = CartaEmail.RetornarConteudoBNE(cartaParam, out assunto);

            if (string.IsNullOrEmpty(conteudo))
                return false;

            string mensagemEmail = parametrosDinamicos.ToString(conteudo);

            string emailRemetenteSistema = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);

            foreach (var item in emailLst.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                          .Enviar(assunto, mensagemEmail, emailRemetenteSistema, item);
            }

            return true;
        }

        private ExpandoObject GerarConteudoDadosCurriculo(Filial filial, Curriculo curriculo, bool utilizaWebEstagios)
        {
            var expando = new ExpandoObject();
            ((IDictionary<string, object>)expando)["NomeEmpresa"] = filial.NomeFantasia;

            if (utilizaWebEstagios)
            {
                ((IDictionary<string, object>) expando)["FlagCliente"] = "SIM";
            }
            else
            {
                ((IDictionary<string, object>) expando)["FlagCliente"] = "NÃO";
            }
        

            var actualType = this.GetType();
            foreach (var ap in actualType.GetProperties())
            {
                ((IDictionary<string, object>)expando)[ap.Name] = ap.GetValue(this, null);
            }

            var typeFilial = filial.GetType();
            foreach (var fp in typeFilial.GetProperties())
            {
                ((IDictionary<string, object>)expando)[fp.Name] = fp.GetValue(filial, null);

                if (fp.PropertyType != typeof (Endereco)) 
                    continue;

                var enderecoObj = ((IDictionary<string, object>)expando)[fp.Name];
                if (enderecoObj == null)
                    continue;

                foreach (var ep in typeof(Endereco).GetProperties())
                {
                    ((IDictionary<string, object>)expando)[typeof(Filial).Name + ep.Name] = ep.GetValue(((IDictionary<string, object>)expando)[fp.Name], null);
                }
            }

            var typeCurriculo = curriculo.GetType();
            foreach (var cp in typeCurriculo.GetProperties())
            {
                ((IDictionary<string, object>)expando)[cp.Name] = cp.GetValue(curriculo, null);

                if (cp.PropertyType != typeof (PessoaFisica))
                    continue;

                var pessoaFisicaObj = ((IDictionary<string, object>)expando)[cp.Name];
                if (pessoaFisicaObj == null)
                    continue;

                foreach (var pp in typeof(PessoaFisica).GetProperties())
                {
                    ((IDictionary<string, object>)expando)[pp.Name] = pp.GetValue(pessoaFisicaObj, null);

                    if (pp.PropertyType != typeof (Endereco)) 
                        continue;

                    var enderecoObj = ((IDictionary<string, object>)expando)[pp.Name];
                    if (enderecoObj == null)
                        continue;

                    foreach (var ep in typeof(Endereco).GetProperties())
                    {
                        ((IDictionary<string, object>)expando)[typeof(PessoaFisica).Name + ep.Name] =
                            ep.GetValue(enderecoObj, null);
                    }
                }
            }

            return expando;
        }
    }
}
