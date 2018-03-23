using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using AdminLTE_Application;
using Sample.Models;
using System.Data.SqlClient;

namespace Sample.Controllers
{
    public class VisualizacaoEmpresaController : Controller
    {
        private Model db = new Model();

        // GET: VisualizacaoEmpresa
        public async Task<ActionResult> Index()
        {
            var cRM_Empresa = db.CRM_Empresa.Include(c => c.CRM_Situacao_Atendimento);
            return View(await cRM_Empresa.ToListAsync());
        }

        // GET: VisualizacaoEmpresa/Details/5
        public async Task<ActionResult> Details(decimal id, VisualizacaoEmpresa model)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var context = new Model())
            {
                model.pag = model.pag <= 0 ? 1 : model.pag;
                model.rowsPag = model.rowsPag <= 1 ? 100 : model.rowsPag;
                var Num_CNPJ = new SqlParameter("@num_cnpj ", Convert.ToDecimal(id));
                var pag = new SqlParameter("@PageNumber ", model.pag);
                var rowPag = new SqlParameter("@RowspPage ", model.rowsPag);
                var Qtd_Total = new SqlParameter("@Qtd_Total", SqlDbType.Int) { Direction = ParameterDirection.Output };
                var Nme_Empresa = new SqlParameter("@Nme_Empresa", SqlDbType.VarChar, 120) { Direction = ParameterDirection.Output };
                var result = context.Database
                    .SqlQuery<Visualizacao>("dbo.SP_Visualizacao_Curriculos @PageNumber, @RowspPage, @num_cnpj, @Qtd_Total out, @Nme_Empresa out ", pag, rowPag, Num_CNPJ, Qtd_Total, Nme_Empresa)
                    .ToList();
                model.listaVisualizacao = result;
                model.Qtd_Total = Convert.ToInt32(Qtd_Total.Value);
                model.TotalPag = (double)model.Qtd_Total/(double)model.rowsPag;
                model.TotalPag = Math.Ceiling(model.TotalPag);
                model.RazSocial = Nme_Empresa.Value.ToString();
            }
          
            return View(model);
        }

        // GET: VisualizacaoEmpresa/Create
        public ActionResult Create()
        {
            ViewBag.Idf_Situacao_Atendimento = new SelectList(db.CRM_Situacao_Atendimento, "Idf_Situacao_Atendimento", "Des_Situacao_atendimento");
            return View();
        }

        // POST: VisualizacaoEmpresa/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Num_CNPJ,Idf_Filial,Raz_Social,Dta_Cadastro,Dta_Ultimo_Acesso,Idf_Plano,Des_Plano,Dta_Inicio,Dta_Fim,Qtd_Dias_Plano_Ativo,Qtd_Acabar_Plano,Idf_Situacao_Atendimento,Dta_Ultimo_Atendimento,Nme_Cidade,Sig_Estado,Idf_Cidade,Dta_Retorno,Idf_Area_BNE")] CRM_Empresa cRM_Empresa)
        {
            if (ModelState.IsValid)
            {
                db.CRM_Empresa.Add(cRM_Empresa);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Idf_Situacao_Atendimento = new SelectList(db.CRM_Situacao_Atendimento, "Idf_Situacao_Atendimento", "Des_Situacao_atendimento", cRM_Empresa.Idf_Situacao_Atendimento);
            return View(cRM_Empresa);
        }

        // GET: VisualizacaoEmpresa/Edit/5
        public async Task<ActionResult> Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRM_Empresa cRM_Empresa = await db.CRM_Empresa.FindAsync(id);
            if (cRM_Empresa == null)
            {
                return HttpNotFound();
            }
            ViewBag.Idf_Situacao_Atendimento = new SelectList(db.CRM_Situacao_Atendimento, "Idf_Situacao_Atendimento", "Des_Situacao_atendimento", cRM_Empresa.Idf_Situacao_Atendimento);
            return View(cRM_Empresa);
        }

        // POST: VisualizacaoEmpresa/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Num_CNPJ,Idf_Filial,Raz_Social,Dta_Cadastro,Dta_Ultimo_Acesso,Idf_Plano,Des_Plano,Dta_Inicio,Dta_Fim,Qtd_Dias_Plano_Ativo,Qtd_Acabar_Plano,Idf_Situacao_Atendimento,Dta_Ultimo_Atendimento,Nme_Cidade,Sig_Estado,Idf_Cidade,Dta_Retorno,Idf_Area_BNE")] CRM_Empresa cRM_Empresa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cRM_Empresa).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Idf_Situacao_Atendimento = new SelectList(db.CRM_Situacao_Atendimento, "Idf_Situacao_Atendimento", "Des_Situacao_atendimento", cRM_Empresa.Idf_Situacao_Atendimento);
            return View(cRM_Empresa);
        }

        // GET: VisualizacaoEmpresa/Delete/5
        public async Task<ActionResult> Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CRM_Empresa cRM_Empresa = await db.CRM_Empresa.FindAsync(id);
            if (cRM_Empresa == null)
            {
                return HttpNotFound();
            }
            return View(cRM_Empresa);
        }

        // POST: VisualizacaoEmpresa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(decimal id)
        {
            CRM_Empresa cRM_Empresa = await db.CRM_Empresa.FindAsync(id);
            db.CRM_Empresa.Remove(cRM_Empresa);
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
    }
}
