using BNE.PessoaFisica.Data.Repositories;
using System.Linq;

namespace BNE.PessoaFisica.Domain
{
    public class Email
    {
        private readonly IEmailRepository _emailRepository;

        public Email(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }

        public Model.Email GetById(int id)
        {
            return _emailRepository.GetById(id);
        }

        public Model.Email GetByIdPessoaFisica(int idPessoaFisica)
        {
            return _emailRepository.GetMany(p => p.IdPessoaFisica == idPessoaFisica).FirstOrDefault();
        }

        public bool SalvarEmail(Model.Email email)
        {
            try
            {
                _emailRepository.Add(email);

                return true;
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}