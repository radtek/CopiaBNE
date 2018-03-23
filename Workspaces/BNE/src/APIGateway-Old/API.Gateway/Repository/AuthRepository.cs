using API.Gateway.Auth;
using API.Gateway.DTO;
using API.Gateway.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace API.Gateway.Repository
{
    public class AuthRepository : IDisposable
    {

        private AuthContext _ctx;

        private UserManager<IdentityUser> _userManager;

        public AuthRepository()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
        }

        public async Task<IdentityResult> RegisterUser(Usuario perfil_usuario)
        {
            IdentityUser user = new IdentityUser() { UserName = perfil_usuario.Idf_Usuario.ToString() };
            var result = await _userManager.CreateAsync(user, perfil_usuario.Password_Key.ToString());
            return result;
        }

        public async Task<IdentityResult> UnregisterUser(Usuario perfil_usuario)
        {
            IdentityUser user = await FindUser(perfil_usuario.Idf_Usuario.ToString(), perfil_usuario.Password_Key.ToString());
            var result = _userManager.Delete(user);
            return result;
        }


        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);
            return user;
        }

        public IdentityUser FindUserByName(string userName)
        {
            IdentityUser user = _userManager.FindByName(userName);
            return user;
        }


        public IList<string> GetRoles(string userId)
        {
            return _userManager.GetRoles<IdentityUser, string>(userId);
        }


        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();
        }
    }
}