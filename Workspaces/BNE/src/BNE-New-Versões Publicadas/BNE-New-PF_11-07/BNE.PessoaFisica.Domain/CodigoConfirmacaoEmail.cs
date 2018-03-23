using BNE.Core.Common;
using BNE.Data.Infrastructure;
using BNE.Logger.Interface;
using BNE.PessoaFisica.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.PessoaFisica.Domain
{
    public class CodigoConfirmacaoEmail
    {

        private readonly ICodigoConfirmacaoEmailRepository _codigoConfirmacaoEmailRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public CodigoConfirmacaoEmail(ICodigoConfirmacaoEmailRepository codigoConfirmacaoEmail, ILogger logger,IUnitOfWork unitOfWork)
        {
            _codigoConfirmacaoEmailRepository = codigoConfirmacaoEmail;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public Model.CodigoConfirmacaoEmail GetById(int id)
        {
            return _codigoConfirmacaoEmailRepository.GetById(id);
        }

        public Model.CodigoConfirmacaoEmail GetByCodigo(string codigo)
        {
            return _codigoConfirmacaoEmailRepository.GetMany(p => p.Codigo == codigo).SingleOrDefault();
        }

        #region GerarCodigoValidacaoEmail
        /// <summary>
        /// Gerar Codigo de validação do e-mail
        /// </summary>
        /// <param name="idPessoaFisica"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public string GerarCodigoValidacaoEmail(int idPessoaFisica, string email)
        {
            string token = Utils.ToBase64(Guid.NewGuid().ToString());

            var objCodigoConfirmaEmail = new Model.CodigoConfirmacaoEmail
            {
                Codigo = token,
                DataCriacao = DateTime.Now,
                Email = email
            };

            _codigoConfirmacaoEmailRepository.Add(objCodigoConfirmaEmail);

            //TODO: validar se vai dar B.O sem o Commit aqui.
            //_unitOfWork.Commit();

            return token;

        }
        #endregion

        public bool Salvar(Model.CodigoConfirmacaoEmail codigoConfirmacaoEmail)
        {
            try
            {
                _codigoConfirmacaoEmailRepository.Update(codigoConfirmacaoEmail);

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao salvar codigoConfirmacaoEmail parametro");
                throw;
            }
        }
    }
}
