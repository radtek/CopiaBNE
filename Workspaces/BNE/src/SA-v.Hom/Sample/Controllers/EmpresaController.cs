using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using AdminLTE_Application.Models;
using System.Data.SqlClient;
using System.Security.Claims;
using Sample.DTO;
using Sample.Models.Empresa;

namespace AdminLTE_Application.Controllers
{
    [Authorize]
    public class EmpresaController : Controller
    {
        private Model db = new Model();

        // GET: Empresa
        public async Task<ActionResult> Index()
        {
            //string url = HttpContext.Current.Request.Url.AbsoluteUri;

            return View(await db.VWEmpresas.ToListAsync());
        }
        
        [HttpPost]
        public async Task<ActionResult> AddAtendimento(AddAtendimentoDTO dto)
        {
            var userWithClaims = (ClaimsPrincipal)User;
            var cpf = userWithClaims.Claims.First(c => c.Type == "cpf");

            using (var context = new Model())
            {
                var pNum_CNPJ = new SqlParameter("@num_cnpj", dto.Num_CNPJ);
                var pcpf = new SqlParameter("@cpf", cpf.Value.ToString());


                if (!(dto.Msg==null))
                {
                    var pDataRetorno = new SqlParameter("@DataRetorno", dto.DataAtendimento);
                    if (pDataRetorno.SqlValue==null)
                    {
                        dto.DataAtendimento = "";
                         pDataRetorno = new SqlParameter("@DataRetorno", dto.DataAtendimento);

                    }
                    var pMsg = new SqlParameter("@msg", dto.Msg);
                    var result = context.Database
                    .SqlQuery<Visualizacao>("dbo.incluiObservacao @num_cnpj,@cpf,@msg,@DataRetorno", pNum_CNPJ, pcpf, pMsg, pDataRetorno)
                    .ToList();
                }
                
            }

            return Redirect("../Empresa/Details/" + dto.Num_CNPJ);
        }

       
       [HttpPost]
        public async Task<ActionResult> pesquisar(string Palavra, string Num_CNPJ)
        {
            var userWithClaims = (ClaimsPrincipal)User;
            var cpf = userWithClaims.Claims.First(c => c.Type == "cpf");

            

            return Redirect("../Empresa/Details/" + Num_CNPJ);
        }    
           
       
        // GET: Empresa/Details/5
        public async Task<ActionResult> Details(decimal id, string msg)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            decimal Num_CNPJ = Convert.ToDecimal(id);

            VWEmpresa vWEmpresa = await db.VWEmpresas.FindAsync(id);

            if (vWEmpresa == null)
            {
                return HttpNotFound();
            }
            ViewBag.Msg = msg;
            return View(vWEmpresa);
        }      
       

        // GET: Empresa/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Empresa/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Num_CNPJ,Raz_Social,End_Site,Num_DDD_Comercial,Num_Comercial,Des_Natureza_Juridica,Des_Plano,Dta_Inicio_Plano,Dta_Fim_plano,Sig_Estado,Nme_Cidade,Des_Situacao_Filial,Dta_Cadastro,Des_URL")] VWEmpresa vWEmpresa)
        {
            if (ModelState.IsValid)
            {
                db.VWEmpresas.Add(vWEmpresa);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(vWEmpresa);
        }

        // GET: Empresa/Edit/5
        public async Task<ActionResult> Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VWEmpresa vWEmpresa = await db.VWEmpresas.FindAsync(id);
            if (vWEmpresa == null)
            {
                return HttpNotFound();
            }
          
            return View(vWEmpresa);
        }

        // POST: Empresa/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Num_CNPJ,Raz_Social,End_Site,Num_DDD_Comercial,Num_Comercial,Des_Natureza_Juridica,Des_Plano,Dta_Inicio_Plano,Dta_Fim_plano,Sig_Estado,Nme_Cidade,Des_Situacao_Filial,Dta_Cadastro,Des_URL")] VWEmpresa vWEmpresa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vWEmpresa).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(vWEmpresa);
        }

        // GET: Empresa/Delete/5
        public async Task<ActionResult> Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VWEmpresa vWEmpresa = await db.VWEmpresas.FindAsync(id);
            if (vWEmpresa == null)
            {
                return HttpNotFound();
            }
            return View(vWEmpresa);
        }

        // POST: Empresa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(decimal id)
        {
            VWEmpresa vWEmpresa = await db.VWEmpresas.FindAsync(id);
            db.VWEmpresas.Remove(vWEmpresa);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public async Task<ActionResult> AddProspeccao(AddProspeccaoDTO dto)
        {
            var userWithClaims = (ClaimsPrincipal)User;
            var cpf = userWithClaims.Claims.First(c => c.Type == "cpf");

            using (var context = new Model())
            {
                var Num_CNPJ = new SqlParameter("@num_cnpj", dto.Num_CNPJ);
                var ncpf = new SqlParameter("@cpf", cpf.Value.ToString());
                var pMsg = new SqlParameter("@msg", SqlDbType.VarChar, 200) { Direction = ParameterDirection.Output };
                var result = context.Database
                    .SqlQuery<vendedorProspect>("dbo.SP_Inclui_Propeccao @num_cnpj, @cpf, @MSG out", Num_CNPJ, ncpf, pMsg)
                    .ToList();
                dto.msg = pMsg.Value.ToString();
            }

            return Redirect("../Empresa/Details/" + dto.Num_CNPJ + "/" + dto.msg);
        }

    }
}
