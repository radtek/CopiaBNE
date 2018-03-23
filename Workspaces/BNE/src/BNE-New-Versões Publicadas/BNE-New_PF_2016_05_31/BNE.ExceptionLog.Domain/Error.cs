using System;
using System.Collections.Generic;
using BNE.ExceptionLog.Data.Repositories;
using BNE.ExceptionLog.Model;

namespace BNE.ExceptionLog.Domain
{
    public class Error
    {
        private readonly IErrorRepository _errorRepository;

        public Error(IErrorRepository errorRepository)
        {
            _errorRepository = errorRepository;
        }

        public Model.Error Logar(string aplicacao, string usuario, string payload, TraceLog ex, string message, string customMessage)
        {
            var objLog = Get(aplicacao, message, customMessage);

            if (objLog == null)
            {
                objLog = CriarErro(aplicacao, message, ex, customMessage);
                _errorRepository.Add(objLog);
            }

            objLog.Ocorrencias.Add(new MessageBase.Ocorrencia
            {
                DataCadastro = DateTime.Now,
                Payload = payload,
                Usuario = usuario
            });

            _errorRepository.Update(objLog);

            return objLog;
        }

        public Model.Error CriarErro(string aplicacao, string message, TraceLog ex, string customMessage = null)
        {
            return new Model.Error
            {
                Aplicacao = aplicacao,
                CustomMessage = customMessage,
                Message = message,
                TraceLog = ex,
                Ocorrencias = new List<MessageBase.Ocorrencia>()
            };
        }

        public Model.Error Get(string aplicacao, string message, string customMessage)
        {
            return _errorRepository.Get(x => x.Aplicacao == aplicacao && x.Message == message && x.CustomMessage == customMessage);
        }

        public IEnumerable<Model.Error> GetAll()
        {
            return _errorRepository.GetAll();
        }
    }
}
