using System;
using System.Web.Security;
using BNE.Auth.Core.ClaimTypes;
using BNE.Auth.Core.Helper;

namespace BNE.Auth.Core
{
    public class User
    {
        public System.Web.HttpContextBase Context;
        public decimal CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Nome { get; set; }

        public static User GetUser(System.Web.HttpContextBase context)
        {
            var authUser = new User();

            if (context != null)
            {
                var user = context.User;
                if (user == null || !user.Identity.IsAuthenticated)
                    return null;

                //IPrincipal currentPrincipal;
                var formsIdentity = user.Identity as FormsIdentity;
                if (formsIdentity != null)
                {
                    if (!String.IsNullOrEmpty(formsIdentity.Ticket.UserData))
                    {
                        foreach (var sc in ClaimsDefaultSerializerHelper.DeserializeClaims(formsIdentity.Ticket.UserData))
                        {
                            if (sc.ClaimType == BNEClaimTypes.CPF)
                                authUser.CPF = Convert.ToDecimal(sc.Value);
                            else if (sc.ClaimType == BNEClaimTypes.DataNascimento)
                                authUser.DataNascimento = Convert.ToDateTime(sc.Value);
                            else if (sc.ClaimType == Microsoft.IdentityModel.Claims.ClaimsIdentity.DefaultNameClaimType)
                                authUser.Nome = sc.Value;
                        }
                    }
                }
                else
                {
                    //var securityIdentity = user.Identity as System.Security.Claims.ClaimsIdentity;
                    //if (securityIdentity != null)
                    //{
                    //    foreach (var sc in securityIdentity.Claims)
                    //    {
                    //        if (sc.Type == BNEClaimTypes.CPF)
                    //            authUser.CPF = Convert.ToDecimal(sc.Value);
                    //        else if (sc.Type == BNEClaimTypes.DataNascimento)
                    //            authUser.DataNascimento = Convert.ToDateTime(sc.Value);
                    //    }
                    //}
                    //else
                    //{
                        var microsoftIdentity = user.Identity as Microsoft.IdentityModel.Claims.ClaimsIdentity;
                        if (microsoftIdentity != null)
                        {
                            foreach (var sc in microsoftIdentity.Claims)
                            {
                                if (sc.ClaimType == BNEClaimTypes.CPF)
                                    authUser.CPF = Convert.ToDecimal(sc.Value);
                                else if (sc.ClaimType == BNEClaimTypes.DataNascimento)
                                    authUser.DataNascimento = Convert.ToDateTime(sc.Value);
                                else if (sc.ClaimType == Microsoft.IdentityModel.Claims.ClaimsIdentity.DefaultNameClaimType)
                                    authUser.Nome = sc.Value;
                            }
                        }
                        else
                        {
                            authUser = null;
                        }
                    //}
                }
            }

            return authUser;
        }

    }
}
