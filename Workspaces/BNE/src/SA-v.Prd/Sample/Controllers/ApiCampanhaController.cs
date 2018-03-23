using AdminLTE_Application;
using Newtonsoft.Json;
using Sample.BLL;
using Sample.DTO;
using Sample.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;

namespace Sample.Controllers
{
    public class ApiCampanhaController : ApiController
    {
        private Model db = new Model();


        // GET: ApiCampanha
        [System.Web.Http.HttpGet]
        [ResponseType(typeof(string))]
        [System.Web.Http.Route("consulta/apicampanha/{html:string}")]
        public async Task<IHttpActionResult> VisualizarEmail(decimal cnpj)
        {
            var userWithClaims = (ClaimsPrincipal)User;
            var cpf = userWithClaims.Claims.First(c => c.Type == "cpf");
            var tipoVendedor = userWithClaims.Claims.First(c => c.Type == "tipoVendedor");
            decimal Num_CPF = Convert.ToDecimal(cpf.Value.ToString());

            var result = RetornaHtmlMontado(cnpj, Num_CPF);

            return this.Ok(result);
        }

        private static ParametrosCampanhaDTO RetornaHtmlMontado(decimal cnpj, decimal Num_CPF)
        {
            using (var context = new Model())
            {
                var numcpf = new SqlParameter("@NUM_CPF", Num_CPF);
                var numcnpj = new SqlParameter("@NUM_CNPJ", cnpj);

                var retorno = context.Database
                    .SqlQuery<ParametrosCampanhaDTO>("dbo.SP_Recuperar_Info_Email_Campanha @NUM_CPF, @NUM_CNPJ", numcpf, numcnpj).FirstOrDefault();
                retorno.Nme_Usuario_Primeiro = Helper.RetornarPrimeiroNome(retorno.Nme_Usuario);

                return retorno;
            }


        }

        public HttpResponseMessage DispararCampanha([FromBody] dispararModel obj)
        {
            try
            {
                string pastaCampanha = ConfigurationManager.AppSettings["DiretorioCampanha"] + obj.campanha.Replace(" ", "_");

                if (!Directory.Exists(pastaCampanha))
                    Directory.CreateDirectory(pastaCampanha);
                else if (!obj.envioTeste)
                {
                    int? teste = 0;
                    var campanha = new SqlParameter("@Nme_Campanha", obj.campanha);
                    using (var context = new Model())
                    {

                        teste = context.Database.SqlQuery<int>(@"select top 1 idf_campanha_vendas from dbo.CRM_Campanha_Vendas with(nolock)
                                        where Nme_Campanha = @Nme_Campanha", campanha).FirstOrDefault();
                    }
                    if (teste > 0)
                    {
                        var response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                        response.Content = new StringContent("Já existe uma campanha com esse nome");
                        return response;
                    }
                }
                var listEmpresasEnvio = JsonConvert.DeserializeObject<List<CampanhaEmpresaModel>>(obj.listCnpj);
                var userWithClaims = (ClaimsPrincipal)User;
                var cpf = userWithClaims.Claims.First(c => c.Type == "cpf");
                var tipoVendedor = userWithClaims.Claims.First(c => c.Type == "tipoVendedor");
                decimal Num_CPF = Convert.ToDecimal(cpf.Value.ToString());

                var emailRemetente = (from q in db.CRM_Vendedor where q.Num_CPF.Equals(Num_CPF) select q.Eml_Vendedor).FirstOrDefault();
                var htmlComImagens = Mail.RetornarHtmlcamanhaComImagens(obj.html, pastaCampanha, obj.campanha.Replace(" ", "_"));

                if (obj.envioTeste)
                {
                    htmlComImagens = htmlComImagens.Replace("{Raz_Social}", listEmpresasEnvio.FirstOrDefault().Raz_Social)
                        .Replace("{Nome_Completo}", listEmpresasEnvio.FirstOrDefault().Nme_Usuario)
                        .Replace("{Primeiro_Nome}", listEmpresasEnvio.FirstOrDefault().Nme_Usuario_Primeiro);
                    Mail.Send(emailRemetente, emailRemetente, obj.assunto, htmlComImagens, null);
                }
                else//envio em massa
                {
                    var IdCampanha = 0;
                    using (var context = new Model())
                    {
                        #region [Salva Campanha]
                        var Num_CNPJ = new SqlParameter("@num_cpf", Num_CPF);
                        var campanha = new SqlParameter("@Campanha", obj.campanha);
                        IdCampanha = context.Database.SqlQuery<int>(@"dbo.SetCampanha  @num_cpf, @Campanha", Num_CNPJ, campanha).FirstOrDefault();

                        #endregion

                    }

                    if (IdCampanha > 0)
                    {

                        #region [Enviar Em massa]

                        int countEnviados = 0;
                        foreach (var item in listEmpresasEnvio)
                        {
                            try
                            {
                                //salvar no banco depois enviar
                                using (var context = new Model())
                                {
                                    var numCnpjPar = new SqlParameter("@Num_CNPJ", item.num_cnpj);
                                    var listaUsuarioEmpresa = context.Database.SqlQuery<UsuarioEmpresa>(@"dbo.SP_Recuperar_Usuario_Ativos_Filial @NUM_CNPJ", numCnpjPar).ToList();

                                    #region [Envia para cada usuario ativo da empresa]
                                    foreach (var usuEmpresa in listaUsuarioEmpresa)
                                    {
                                        var htmlFinal = htmlComImagens.Replace("{Raz_Social}", item.Raz_Social)
                                        .Replace("{Nome_Completo}", usuEmpresa.Nme_Pessoa)
                                             .Replace("{Primeiro_Nome}", Helper.RetornarPrimeiroNome(usuEmpresa.Nme_Pessoa));

                                        var idCampanhaPar = new SqlParameter("@Idf_Campanha_Vendas", IdCampanha);
                                        var htlmlFinalPar = new SqlParameter("@Html", htmlFinal);
                                        var emlDestinoPar = new SqlParameter("@Eml_Destino", usuEmpresa.Eml_Comercial);
                                        var numCnpjParLog = new SqlParameter("@Num_CNPJ", item.num_cnpj);
                                        var emlRemetentePar = new SqlParameter("@Eml_Remetente", emailRemetente);
                                        context.Database.ExecuteSqlCommand(@"insert into CRM_Log_Envio_Campanha(Eml_Remetente,Eml_Destino,Num_CNPJ,Dta_Envio,Idf_Campanha_Vendas,Html_Campanha)
                                                values( @Eml_Remetente, @Eml_Destino, @Num_CNPJ, getdate(), @Idf_Campanha_Vendas, @Html)", emlRemetentePar, emlDestinoPar, numCnpjParLog, idCampanhaPar, htlmlFinalPar);

                                        Mail.Send(usuEmpresa.Eml_Comercial, emailRemetente, obj.assunto, htmlFinal, null);
                                        countEnviados++;
                                        if (countEnviados.Equals(1))//enviar uma copia pro vendedor
                                            Mail.Send(emailRemetente, emailRemetente, obj.assunto, htmlFinal, null);

                                    }
                                    #endregion

                                    #region [Salvar Atendimento]
                                    var pNum_CNPJ = new SqlParameter("@num_cnpj", item.num_cnpj);
                                    var pcpf = new SqlParameter("@cpf", cpf.Value);
                                    var pMsg = new SqlParameter("@msg", $"#Atendimento Enviado Campanha: {obj.campanha} para {listaUsuarioEmpresa.Count()} usuarios ativo");
                                    var pDataRetorno = new SqlParameter("@DataRetorno", string.Empty);
                                    var result = context.Database
                                    .SqlQuery<Visualizacao>("dbo.incluiObservacao @num_cnpj,@cpf,@msg,@DataRetorno", pNum_CNPJ, pcpf, pMsg, pDataRetorno)
                                    .ToList();
                                    #endregion
                                }

                            
                            }
                            catch (Exception ex)
                            {

                            }

                        }
                        #endregion


                    }
                    else
                    {
                        var response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                        response.Content = new StringContent("Erro Ao Criar a Campanha.");
                        return response;
                    }
                }


                return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            }
            catch (Exception ex)
            {

                var response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                response.Content = new StringContent(ex.ToString());
                return response;
            }

        }


        public class dispararModel
        {
            public string assunto { get; set; }
            public string campanha { get; set; }
            public string html { get; set; }
            public bool envioTeste { get; set; }
            public string listCnpj { get; set; }
        }

        public class UsuarioEmpresa
        {
            public string Eml_Comercial { get; set; }
            public string Nme_Pessoa { get; set; }
        }
    }
}