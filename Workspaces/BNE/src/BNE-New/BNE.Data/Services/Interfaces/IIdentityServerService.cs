using System;
using System.Threading.Tasks;

namespace BNE.Data.Services.Interfaces
{
    public interface IIdentityServerService
    {
        /// <summary>
        /// Create Identity Server UserAccount
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="cpf"></param>
        /// <param name="dataNascimento"></param>
        /// <returns>User Account ID</returns>
        Task<Guid> CreateUserAccount(string nome, decimal cpf, DateTime dataNascimento);
    }
}
