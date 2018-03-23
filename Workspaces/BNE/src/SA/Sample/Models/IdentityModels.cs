using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;
using AdminLTE_Application.Models;
using System;

namespace Sample.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here

            string nameApi = userIdentity.Name;
            nameApi = nameApi.Replace("@","arroba");
            nameApi = nameApi.Replace(".", "dott");

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://sa.bne.com.br/api");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = client.GetAsync("api/ApiGetClaims/" + nameApi).Result;
            if (response.IsSuccessStatusCode)
            {
                var yourcustomobjects = response.Content.ReadAsAsync<IEnumerable<UserClaim>>().Result;
                foreach (var x in yourcustomobjects)
                {
                    userIdentity.AddClaim(new Claim("cpf", x.cpf.ToString()));
                    userIdentity.AddClaim(new Claim("nome",x.nome.ToString()));
                    userIdentity.AddClaim(new Claim("tipoVendedor", x.tipoVendedor.ToString()));
                }
            }           

            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DW_CRM2012", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}