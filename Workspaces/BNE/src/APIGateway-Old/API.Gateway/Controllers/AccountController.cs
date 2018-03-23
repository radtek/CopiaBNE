using API.Gateway.Auth;
using API.Gateway.DTO;
using API.Gateway.Models;
using API.Gateway.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebApiThrottle;

namespace API.Gateway.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : BaseApiController
    {

        
        private AuthRepository _repo = null;

        public AccountController()
        {
            _repo = new AuthRepository();
        }

        [AllowAnonymous]
        [Route("Unregister")]
        public async Task<HttpResponseMessage> Unregister(RemoverModel reg)
        {
            Usuario perfil_usuario;

            if (!ModelState.IsValid)
            {
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "Modelo de dados incorreto.");
            }


            #region Verifica as permissões de acesso
            using (var dbo = new APIGatewayContext())
            {
                int count_keys = (from k in dbo.KeyToken.AsNoTracking() where k.KeyValue == reg.keytoken && k.Flg_Ativo == true select k).Count();
                if (count_keys <= 0)
                {
                    return Request.CreateResponse<string>(HttpStatusCode.Forbidden, "Chave de acesso inválida.");
                }
            }
            #endregion


            using (var dbo = new APIGatewayContext())
            {
                perfil_usuario = (from pu in dbo.Usuario where pu.Num_CPF == reg.Num_CPF && pu.Idf_Filial == reg.Idf_Filial select pu).FirstOrDefault();
                try 
                {
                    
                    var policyRepository = new PolicyMemoryCacheRepository();
                    var policy = policyRepository.FirstOrDefault(ThrottleManager.GetPolicyKey());

                    List<Quota> velhas_quotas = dbo.Quota.Include("Endpoint").Where(q => q.Idf_Perfil == perfil_usuario.Idf_Perfil).ToList();
                    foreach (var q in velhas_quotas)
                    {
                        string velha_rota_perfil = string.Format("{0}/api/{1}/{2}/{3}/{4}", perfil_usuario.Idf_Usuario, q.Endpoint.VersionAPI,
                            q.Endpoint.Nme_Api, q.Endpoint.Controller, q.Endpoint.Action);
                        policy.ClientRules.Remove(velha_rota_perfil);
                    }

                    ThrottleManager.UpdatePolicy(policy, policyRepository);

                    //IdentityResult result = await _repo.UnregisterUser(perfil_usuario);
                    perfil_usuario.Flg_Inativo = true;
                    dbo.SaveChanges();
                }
                catch(Exception ex)
                {
                    return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
                }
                return Request.CreateResponse<string>(HttpStatusCode.OK, "Usuário desativado com sucesso!");
            }
        }



        [AllowAnonymous]
        [Route("Register")]
        public async Task<HttpResponseMessage> Register(RegistroModel reg) 
        {
            Usuario perfil_usuario;

            #region Verifica o estado das informações enviadas
            if (reg == null)
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "Nenhum parâmetro informado. Verifique se o objeto JSON está montado corretamente.");

            string error_message = "";
            if (!ModelState.IsValid)
            {
                foreach (var ms in ModelState.Values)
                {
                    foreach (var error in ms.Errors)
                    {
                        if (error.Exception != null)
                            error_message += error.Exception.Message + " ";
                        else if (!string.IsNullOrEmpty(error.ErrorMessage))
                            error_message += error.ErrorMessage + " ";
                    }
                }
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, error_message);
            }
            #endregion

            #region Verifica as permissões de acesso
            using (var dbo = new APIGatewayContext()) 
            {
                int count_keys = (from k in dbo.KeyToken.AsNoTracking() where k.KeyValue == reg.keytoken && k.Flg_Ativo == true select k).Count();
                if (count_keys <= 0) 
                {
                    return Request.CreateResponse<string>(HttpStatusCode.Forbidden, "Chave de acesso inválida.");
                }
            }
            #endregion

            try
            {
                using (var dbo = new APIGatewayContext())
                {
                    using (var dbContextTransaction = dbo.Database.BeginTransaction()) 
                    {
                        perfil_usuario = (from pu in dbo.Usuario where pu.Num_CPF == reg.Num_CPF && pu.Idf_Filial == reg.Idf_Filial select pu).FirstOrDefault();

                        DateTime cr_date = DateTime.Now;
                        DateTime clean_date = new DateTime(cr_date.Year, cr_date.Month, cr_date.Day);

                        if (perfil_usuario == null)
                        {
                            perfil_usuario = new Usuario()
                            {
                                Num_CPF = Decimal.Round(Convert.ToDecimal(reg.Num_CPF), 0),
                                Idf_Filial = reg.Idf_Filial,
                                Senha = reg.Senha,
                                Dta_Nascimento = (DateTime)reg.Dta_Nascimento,
                                Idf_Perfil = (int)reg.Idf_Perfil,
                                Dta_Cadastro = cr_date,
                                Dta_Alteracao = cr_date,
                                Password_Key = Guid.NewGuid(),
                                Dta_Inicio_Plano = clean_date,
                                Flg_Inativo = false
                            };
                            dbo.Usuario.Add(perfil_usuario);
                        }
                        else
                        {
                            perfil_usuario.Senha = reg.Senha;
                            perfil_usuario.Dta_Nascimento = (DateTime)reg.Dta_Nascimento;
                            int perfil_velho = perfil_usuario.Idf_Perfil;
                            perfil_usuario.Flg_Inativo = false;
                            #region Pefil de usuário
                            if (perfil_usuario.Idf_Perfil != (int)reg.Idf_Perfil)
                            {
                                perfil_usuario.Idf_Perfil = (int)reg.Idf_Perfil;
                                perfil_usuario.Dta_Inicio_Plano = clean_date;

                                var policyRepository = new PolicyMemoryCacheRepository();
                                var policy = policyRepository.FirstOrDefault(ThrottleManager.GetPolicyKey());

                                List<Quota> velhas_quotas = dbo.Quota.Include("Endpoint").Where(q => q.Idf_Perfil == perfil_velho).ToList();
                                foreach (var q in velhas_quotas)
                                {
                                    string velha_rota_perfil = string.Format("{0}/api/{1}/{2}/{3}/{4}", perfil_usuario.Idf_Usuario, q.Endpoint.VersionAPI,
                                        q.Endpoint.Nme_Api, q.Endpoint.Controller, q.Endpoint.Action);
                                    policy.ClientRules.Remove(velha_rota_perfil);
                                }

                                ThrottleManager.UpdatePolicy(policy, policyRepository);

                                List<Quota> novas_quotas = dbo.Quota.Include("Endpoint").Where(q => q.Idf_Perfil == (int)reg.Idf_Perfil).ToList();
                                foreach (var q in novas_quotas)
                                {
                                    string nova_rota_perfil = string.Format("{0}/api/{1}/{2}/{3}/{4}", perfil_usuario.Idf_Usuario, q.Endpoint.VersionAPI,
                                        q.Endpoint.Nme_Api, q.Endpoint.Controller, q.Endpoint.Action);

                                    var rateLimits = new RateLimits()
                                    {
                                        PerSecond = q.Per_Second,
                                        PerMinute = q.Per_Minute,
                                        PerHour = q.Per_Hour,  PerDay = q.Per_Day, 
                                        PerWeek = 0 //Controlado pelo proxy
                                    };
                                    policy.ClientRules[nova_rota_perfil] = rateLimits;
                                    ThrottleManager.UpdatePolicy(policy, policyRepository);
                                }
                            }
                            perfil_usuario.Dta_Alteracao = cr_date;
                            #endregion
                        }

                        dbo.SaveChanges();

                        if (_repo.FindUserByName(perfil_usuario.Idf_Usuario.ToString()) == null) 
                        {
                            IdentityResult result = await _repo.RegisterUser(perfil_usuario);
                            IHttpActionResult errorResult = GetErrorResult(result);
                            if (errorResult != null)
                            {
                                dbContextTransaction.Rollback();
                                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, errorResult.ToString());
                            };
                        }

                        dbContextTransaction.Commit();
                    };
                }
            }
            catch (DbEntityValidationException ex)
            {
                string log = "";
                foreach (var eve in ex.EntityValidationErrors)
                {
                    log += string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        log += string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, log);
            }
            catch (Exception ex) 
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
            }
            return Request.CreateResponse<string>(HttpStatusCode.OK, "Usuário registrado com sucesso!");
        }


        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Token")]
        public async Task<HttpResponseMessage> Token(TokenModel model)
        {

            bool is_ok_super_user = false;
            bool is_ok_normal_user = false;

            #region Verifica o estado das informações enviadas
            if (model == null)
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, "Nenhum parâmetro informado. Verifique se o objeto JSON está montado corretamente." );

            string error_message = "";
            if (!ModelState.IsValid)
            {
                foreach (var ms in ModelState.Values)
                {
                    foreach (var error in ms.Errors)
                    {
                        if (error.Exception != null)
                            error_message += error.Exception.Message + " ";
                        else if (!string.IsNullOrEmpty(error.ErrorMessage))
                            error_message += error.ErrorMessage + " ";
                    }
                }
                return Request.CreateResponse<string>(HttpStatusCode.BadRequest, error_message );
            }
            #endregion


            Usuario perfil_usuario;
            using (var dbo = new APIGatewayContext())
            {
                perfil_usuario = (from pu in dbo.Usuario where pu.Num_CPF == model.Num_CPF && pu.Idf_Filial == model.Idf_Filial && pu.Flg_Inativo == false select pu).FirstOrDefault();
            }

            if (perfil_usuario == null)
                return Request.CreateResponse<string>(HttpStatusCode.Forbidden, "Nenhum usuário encontrado.");

            Cliente obj_cliente = null;
            Guid guid = new Guid(model.Idf_Cliente);
            using (var dbo = new APIGatewayContext()) 
            {
                obj_cliente = (from c in dbo.Cliente where c.Idf_Cliente == guid select c).FirstOrDefault();
            }

            if (obj_cliente == null)
                return Request.CreateResponse<string>(HttpStatusCode.Forbidden, "Nenhum cliente encontrado.");

            is_ok_super_user  = (perfil_usuario.Idf_Perfil == 1 && perfil_usuario.Senha == model.Senha);
            is_ok_normal_user = (perfil_usuario.Idf_Perfil != 1 && perfil_usuario.Dta_Nascimento == model.Dta_Nascimento);

            if (is_ok_normal_user || is_ok_super_user) 
            {

                AccessToken currentToken = null;
                using (var dbo = new APIGatewayContext())
                {
                     currentToken =  dbo.AccessToken.Where(a => a.Idf_Usuario == perfil_usuario.Idf_Usuario && a.Dta_Validade > DateTime.Now).OrderByDescending(a => a.Dta_Validade).FirstOrDefault();
                }

                if (currentToken != null) 
                {
                    var dt_diff = currentToken.Dta_Validade.Subtract(DateTime.Now);
                    if (dt_diff.TotalSeconds > 0) 
                    {
                        Dictionary<string, object> result = new Dictionary<string, object>();
                        result.Add("access_token", currentToken.Token);
                        result.Add("token_type", "bearer");
                        result.Add("expires_in", (int)dt_diff.TotalSeconds);
                        return Request.CreateResponse<object>(HttpStatusCode.OK, result);
                    }
                }

                var pairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>( "grant_type", "password" ), 
                    new KeyValuePair<string, string>( "username", perfil_usuario.Idf_Usuario.ToString() ), 
                    new KeyValuePair<string, string> ( "Password", perfil_usuario.Password_Key.ToString() ),
                    new KeyValuePair<string, string> ( "Idf_Cliente", model.Idf_Cliente )
                };

                var content = new FormUrlEncodedContent(pairs);
                using (var client = new HttpClient())
                {
                    var dt_cri = DateTime.Now;
                    var response = await client.PostAsync(string.Format("http://{0}/token", Request.Headers.Host), content);

                    if (response.IsSuccessStatusCode)
                    {
                        JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                        Dictionary<string, object> objResult = (Dictionary<string, object>)json_serializer.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                        using (var dbo = new APIGatewayContext())
                        {
                            dbo.AccessToken.Add(new AccessToken
                            {
                                Idf_Usuario = perfil_usuario.Idf_Usuario,
                                Dta_Validade = dt_cri.AddSeconds((int) objResult["expires_in"]),
                                Token = objResult["access_token"].ToString(),
                                ExpiresIn = (int)objResult["expires_in"]
                            });
                            dbo.SaveChanges();
                        }
                        return Request.CreateResponse<object>(HttpStatusCode.OK, objResult);
                    }
                    else
                    {
                        return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, response.ReasonPhrase);
                    }
                }
            }
            else
                return Request.CreateResponse<string>(HttpStatusCode.Forbidden, "Dados de usuário inválidos! Não foi possível gerar a chave de acesso.");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {

            var appUser = await this._userManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await this._userManager.GetRolesAsync(appUser.Id);

            var rolesNotExists = rolesToAssign.Except(this.AppRoleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExists.Count() > 0)
            {

                ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            IdentityResult removeResult = await this._userManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await this._userManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();
        }


    }
}
