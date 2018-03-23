using System;
using System.Collections.Generic;
using BNE.ExceptionLog.Data.Repositories;
using BNE.ExceptionLog.Model;

namespace BNE.ExceptionLog.Domain
{
    public class Warning
    {
        private readonly IWarningRepository _warningRepository;

        public Warning(IWarningRepository warningRepository)
        {
            _warningRepository = warningRepository;
        }

        public Model.Warning Logar(string aplicacao, string usuario, string message, string payload)
        {
            var log = Get(aplicacao, message);

            if (log == null)
            {
                log = new Model.Warning
                {
                    Aplicacao = aplicacao,
                    Message = message,
                    TipoMensagem = new TipoMensagem(Tipo.Aviso)
                    //Payload = payload
                };
                _warningRepository.Add(log);
            }

            log.Ocorrencias.Add(new MessageBase.Ocorrencia
            {
                DataCadastro = DateTime.Now,
                Payload = payload,
                Usuario = usuario
            });

            _warningRepository.Update(log);

            return log;
        }

        public Model.Warning Get(string aplicacao, string message)
        {
            return _warningRepository.Get(x => x.Aplicacao == aplicacao && x.Message == message);
        }

        public IEnumerable<Model.Warning> GetAll()
        {
            return _warningRepository.GetAll();
        }
    }
}
