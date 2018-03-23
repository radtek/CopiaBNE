using API.Gateway.Console.APIModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Net.Http;
using System.Web.Configuration;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;

namespace API.Gateway.Console.Business
{
    public static class UsuarioBusiness
    {

        public static IPagedList<Usuario> Carregar(int Pagina, int PorPagina = 50)
        {
            using (var dbo = new APIGatewayContext()) 
            {
               return (from us in dbo.Usuario select us).OrderByDescending(us => us.Dta_Cadastro).ToPagedList(Pagina, PorPagina);
            } 
        }

        public static Usuario CarregarPorId(int Id)
        {
            using (var dbo = new APIGatewayContext())
            {
                return dbo.Usuario.Include("Perfil").Where(u => u.Idf_Usuario == Id).FirstOrDefault();
            }
        }


        public static bool Desativar(int Id, out string Message)
        {

            Usuario us = UsuarioBusiness.CarregarPorId(Id);
            string uri = WebConfigurationManager.AppSettings["APIGatewayURI"];
            string key_token = WebConfigurationManager.AppSettings["KeyToken"];

            var httpHandler = new HttpClientHandler() { UseDefaultCredentials = true, UseProxy = true };

            using (var client = new HttpClient(httpHandler))
            {

                client.BaseAddress = new Uri(string.Format("http://{0}/", uri));
                client.DefaultRequestHeaders.Accept.Clear();

                HttpResponseMessage response;
                try
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    response = client.PostAsJsonAsync<object>("api/Account/Unregister", new { Num_CPF = us.Num_CPF, Idf_Filial = us.Idf_Filial, keytoken = key_token }).Result;
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                    return false;
                }

                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                object objResult = (object)json_serializer.DeserializeObject(response.Content.ReadAsStringAsync().Result);

                Message = objResult.ToString();

                return (response.StatusCode == System.Net.HttpStatusCode.OK);
            }
        }


        public static bool Registar(decimal Num_CPF, int? Idf_Filial, string Senha, DateTime Dta_Nascimento, int Idf_Perfil, out string Message) 
        {
            string uri = WebConfigurationManager.AppSettings["APIGatewayURI"];
            string key_token = WebConfigurationManager.AppSettings["KeyToken"];

            var httpHandler = new HttpClientHandler() { UseDefaultCredentials = true, UseProxy = true };

            using (var client = new HttpClient(httpHandler))
            {

                client.BaseAddress = new Uri(string.Format("http://{0}/", uri));
                client.DefaultRequestHeaders.Accept.Clear();

                HttpResponseMessage response;
                try
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    response = client.PostAsJsonAsync<object>("api/Account/Register", 
                        new { Num_CPF = Num_CPF, Idf_Filial = Idf_Filial, Senha = Senha, Dta_Nascimento = Dta_Nascimento, Idf_Perfil = Idf_Perfil, keytoken = key_token }).Result;
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                    return false;
                }

                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                object objResult = (object)json_serializer.DeserializeObject(response.Content.ReadAsStringAsync().Result);

                Message = objResult.ToString();

                return (response.StatusCode == System.Net.HttpStatusCode.OK);
            }
        }

        public static List<Perfil> CarregarPerfis() 
        {
            using (var dbo = new APIGatewayContext())
            {
                return (from per in dbo.Perfil select per).OrderBy(per => per.Des_Perfil).ToList();
            }    
        }

    }
}