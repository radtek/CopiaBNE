using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace API.Gateway.Auth
{
    public static class CustomClaim
    {
        public static Dictionary<string, object> Build(ClaimsIdentity ClaimsIdentity)
        {
            Dictionary<string, object> _dic = new Dictionary<string, object>();
            foreach (Claim cl in ClaimsIdentity.Claims)
            {
                switch (cl.Type)
                {
                    case "Num_CPF":
                        _dic.Add(cl.Type, Convert.ToDecimal(cl.Value));
                        break;
                    case "Dta_Nascimento":
                        _dic.Add(cl.Type, Convert.ToDateTime(cl.Value));
                        break;
                    case "Idf_Filial":
                        _dic.Add(cl.Type, (cl.Value != "") ?  Convert.ToInt32(cl.Value) : 0 );
                        break;
                    case "Idf_Perfil":
                        _dic.Add(cl.Type, Convert.ToInt32(cl.Value));
                        break;
                    case "Idf_Perfil_Usuario":
                        _dic.Add(cl.Type, Convert.ToInt32(cl.Value));
                        break;
                    case "Dta_Inicio_Plano":
                        _dic.Add(cl.Type, Convert.ToDateTime(cl.Value));
                        break;
                    case "Idf_Cliente":
                        _dic.Add(cl.Type, cl.Value);
                        break;
                }
            }
            return _dic;
        }


    }
}