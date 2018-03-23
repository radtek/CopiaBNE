using System;
using BNE.Core.ExtensionsMethods;
using log4net;
using Newtonsoft.Json;

namespace BNE.PessoaJuridica.Domain.Services
{
    public class NCallService
    {

        private readonly ParametroService _parametro;
        private readonly ILog _logger;

        public NCallService(ParametroService parametro, ILog logger)
        {
            _parametro = parametro;

            _logger = logger;
        }

        public bool FilaBoasVindasDisponivel()
        {
            return FilaDisponivel(_parametro.RecuperarValor(Model.Enumeradores.Parametro.NCallFilaBoasVindas));
        }
        public bool FilaCIADisponivel()
        {
            return FilaDisponivel(_parametro.RecuperarValor(Model.Enumeradores.Parametro.NCallFilaCia));
        }

        public void LigarBoasVindas(string numero, string nome)
        {
            Ligar(_parametro.RecuperarValor(Model.Enumeradores.Parametro.NCallFilaBoasVindas), numero, nome);
        }
        public void LigarCIA(string numero, string nome)
        {
            Ligar(_parametro.RecuperarValor(Model.Enumeradores.Parametro.NCallFilaCia), numero, nome);
        }

        private bool FilaDisponivel(string numeroFila)
        {
            var retorno = StatusFila(numeroFila);

            if (retorno != null && retorno.disponivel > 0)
                return true;

            return false;
        }

        private RetornoStatusFila StatusFila(string numeroFila)
        {
            try
            {
                var parametro = new
                {
                    endereco = _parametro.RecuperarValor(Model.Enumeradores.Parametro.EnderecoNCall),
                    fila = numeroFila
                };
                var urlStatusFila = _parametro.RecuperarValor(Model.Enumeradores.Parametro.NCallURLVerificacaoStatusFila);

                //Gerando o link para consulta
                var link = parametro.ToString(urlStatusFila);
                //Criando um servico que implementa get/post
                var httpService = new Core.Helpers.HttpService();
                //Retornando o resultado do link
                var url = new Uri(link);
                //Efetuando requisição
                var retorno = httpService.Get(url, url.PathAndQuery);

                return JsonConvert.DeserializeObject<RetornoStatusFila>(retorno);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return new RetornoStatusFila();
        }

        private void Ligar(string numeroFila, string numeroTelefone, string nome)
        {
            try
            {
                var parametro = new
                {
                    endereco = _parametro.RecuperarValor(Model.Enumeradores.Parametro.EnderecoNCall),
                    fila = numeroFila,
                    numero = numeroTelefone,
                    nome = nome,
                    telefone = BNE.Core.Common.Utils.FormataTelefone(numeroTelefone)
                };
                var urlStatusFila = _parametro.RecuperarValor(Model.Enumeradores.Parametro.NCallURLClick2Call);

                //Gerando o link para consulta
                var link = parametro.ToString(urlStatusFila);
                //Criando um servico que implementa get/post
                var httpService = new Core.Helpers.HttpService();
                var url = new Uri(link);
                //Efetuando requisição
                httpService.Get(url, url.PathAndQuery);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        public class RetornoStatusFila
        {
            public int ret { get; set; }
            public int idfila { get; set; }
            public string nomefila { get; set; }
            public int deslogado { get; set; }
            public int disponivel { get; set; }
            public int ocupado { get; set; }
        }
    }
}
