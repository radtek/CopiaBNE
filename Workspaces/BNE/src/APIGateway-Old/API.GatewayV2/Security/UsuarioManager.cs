using API.GatewayV2.APIModel;
using API.GatewayV2.BNEModel;
using API.GatewayV2.Throttle;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace API.GatewayV2.Security
{
    public class UsuarioManager
    {
        private static ObjectCache cache = MemoryCache.Default;
        private static CacheItemPolicy policy = null;
        private CacheEntryRemovedCallback callback = null;

        public enum UsuarioCachePriority
        {
            Default,
            NotRemovable
        }

        public static UserCredentials DecodeCredentials(string base64) 
        {
            UserCredentials credentials = null;
            try 
            {
                var json = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                credentials = json_serializer.Deserialize<UserCredentials>(json);
            }
            catch{}
            return credentials;
        }

        public Usuario ObtainAccess(decimal cpf, DateTime dta_nascimento, decimal? num_cnpj) 
        {
            List<Usuario> usuarios;
            Usuario api_user;
            

            using (var dbo = new BNEContext())
            {

                usuarios = (from pf in dbo.TAB_Pessoa_Fisica
                            join ufp in dbo.TAB_Usuario_Filial_Perfil on pf.Idf_Pessoa_Fisica equals ufp.Idf_Pessoa_Fisica
                            join fi in dbo.TAB_Filial on ufp.Idf_Filial equals fi.Idf_Filial
                            where pf.Num_CPF == cpf && pf.Dta_Nascimento == dta_nascimento && pf.Flg_Inativo == false && ufp.Idf_Filial != null && ufp.Flg_Inativo == false
                            select new Usuario { Dta_Nascimento = (DateTime)pf.Dta_Nascimento, Num_CPF = pf.Num_CPF, Idf_Perfil = (int)ufp.Idf_Perfil, Num_CNPJ = (decimal)fi.Num_CNPJ }).ToList();
            }

            var usuario = (usuarios.Count > 1 && num_cnpj.HasValue) ? usuarios.Where(u => u.Num_CNPJ == num_cnpj).FirstOrDefault() : usuarios.FirstOrDefault();

            if (usuario != null) 
            {
                using (var dbo = new APIGatewayContext())
                {
                    api_user = dbo.Usuario.Where(u => (u.Num_CPF == usuario.Num_CPF && u.Num_CNPJ == usuario.Num_CNPJ )).FirstOrDefault();
                    if (api_user == null) 
                    {
                        DateTime crt_date =  DateTime.Now;
                        DateTime clean_date = new DateTime(crt_date.Year, crt_date.Month, crt_date.Day);
                        DateTime clean_nasc = new DateTime(usuario.Dta_Nascimento.Year, usuario.Dta_Nascimento.Month, usuario.Dta_Nascimento.Day);
                        
                        var novo =  new Usuario()
                        {
                            Num_CPF = usuario.Num_CPF,
                            Num_CNPJ = usuario.Num_CNPJ,
                            Dta_Nascimento = clean_nasc,
                            Idf_Perfil = usuario.Idf_Perfil,
                            Dta_Cadastro = crt_date,
                            Dta_Alteracao = crt_date,
                            Dta_Inicio_Plano = clean_date,
                            Flg_Inativo = false
                        };

                        dbo.Usuario.Add(novo);
                        dbo.SaveChanges();
                        QuotaManager.Update(novo);
                        return novo;
                    }
                    else 
                    {
                        var do_update = false;

                        if (api_user.Dta_Nascimento != usuario.Dta_Nascimento) 
                        {
                            api_user.Dta_Nascimento = usuario.Dta_Nascimento;
                            do_update = true;
                        }


                        if(api_user.Idf_Perfil != usuario.Idf_Perfil)
                        {
                            QuotaManager.Clean(api_user);
                            api_user.Idf_Perfil = usuario.Idf_Perfil;
                            api_user.Dta_Inicio_Plano = DateTime.Now;
                            do_update = true;
                        }

                        if (do_update) 
                        {
                            api_user.Dta_Alteracao = DateTime.Now;
                            dbo.SaveChanges();
                        }
                            
                        QuotaManager.Update(api_user);
                        return api_user;
                    }
                }
            }
            return null;
        }

        public void Cache(string CacheKeyName, Usuario usuario)
        {
            callback = new CacheEntryRemovedCallback(this.UserCachedRemovedCallback);
            policy = new CacheItemPolicy();
            policy.Priority = CacheItemPriority.Default;
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddHours(24);
            policy.RemovedCallback = callback;
            //policy.ChangeMonitors.Add(new HostFileChangeMonitor(FilePath));

            cache.Set(CacheKeyName, usuario, policy);
        }

        public void Uncache(string CacheKeyName)
        {
            if (cache.Contains(CacheKeyName))
            {
                cache.Remove(CacheKeyName);
            }
        }

        public Usuario GetCached(string CacheKeyName)
        {
            return cache[CacheKeyName] as Usuario;
        }

        private void UserCachedRemovedCallback(CacheEntryRemovedArguments arguments) 
        { 
            QuotaManager.Clean((Usuario) arguments.CacheItem.Value);
        }

        public static string CreateUsuarioKey(decimal cpf, decimal? cnpj) 
        {
            SHA1 hasher = SHA1.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();

            string str_filial = (cnpj.HasValue) ? cnpj.Value.ToString() : "";
            string assinatura = string.Format("{0}{1}", cpf.ToString(), str_filial);

            byte[] array = encoding.GetBytes(assinatura);
            array = hasher.ComputeHash(array);

            StringBuilder strHexa = new StringBuilder();

            foreach (byte item in array)
            {
                strHexa.Append(item.ToString("x2"));
            }

            return strHexa.ToString();    
        }

    }
}