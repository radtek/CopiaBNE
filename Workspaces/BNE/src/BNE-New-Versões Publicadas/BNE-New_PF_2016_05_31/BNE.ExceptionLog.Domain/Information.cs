using System;
using System.Collections.Generic;
using BNE.ExceptionLog.Data.Repositories;
using BNE.ExceptionLog.Model;

namespace BNE.ExceptionLog.Domain
{
    public class Information
    {
        private readonly IInformationRepository _informationRepository;

        public Information(IInformationRepository informationRepository)
        {
            _informationRepository = informationRepository;
        }

        public Model.Information Logar(string aplicacao, string usuario, string message, string payload)
        {
            var log = Get(aplicacao, message);

            if (log == null)
            {
                log = new Model.Information
                {
                    Aplicacao = aplicacao,
                    Message = message,
                    TipoMensagem = new TipoMensagem(Tipo.Informacao)
                    //Payload = payload
                };
                _informationRepository.Add(log);
            }

            log.Ocorrencias.Add(new MessageBase.Ocorrencia
            {
                DataCadastro = DateTime.Now,
                Payload = payload,
                Usuario = usuario
            });

            _informationRepository.Update(log);

            return log;
        }

        public Model.Information Get(string aplicacao, string message)
        {
            return _informationRepository.Get(x => x.Aplicacao == aplicacao && x.Message == message);
        }

        public IEnumerable<Model.Information> GetAll()
        {
            return _informationRepository.GetAll();
        }
    }
}
