using System;
using BNE.Data.Infrastructure;
using BNE.Logger.Interface;

namespace BNE.Mensagem.Domain
{
    public class SMS
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _unitOfWork;

        public SMS(ILogger logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public bool EnviarSMS(Command.EnviarSMS objEnviarSMS)
        {
            try
            {

                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return false;
        }

    }
}
