using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using AdminLTE_Application;
using System;
using System.Globalization;

namespace Sample.Controllers
{
    public class VagasEmpresaController : Controller
    {
        private Model db = new Model();

        // GET: VagasEmpresa
        public async Task<ActionResult> Index()
        {
            var cRM_Empresa = db.CRM_Empresa.Include(c => c.CRM_Situacao_Atendimento);
            return View(await cRM_Empresa.ToListAsync());
        }

        // GET: VagasEmpresa/Details/5
        public async Task<ActionResult> Details(decimal id, string FlPeriodo, bool? Ativo, bool? Inativa, bool? Oportunidade, bool? Confidencial)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var vagas = from s in db.VWVaga.Where(x => x.num_cnpj.Equals(id))
                        select s;

             ViewBag.Confidencial = Confidencial.HasValue ? Confidencial.Value : false;


            if (!Ativo.HasValue && !Inativa.HasValue && !Oportunidade.HasValue && string.IsNullOrEmpty(FlPeriodo))
            {
                ViewBag.Ativo = Ativo = ViewBag.Inativa = Inativa = ViewBag.Oportunidade = Oportunidade = true;
            }
            else{
                ViewBag.Ativo = Ativo.HasValue ? Ativo.Value : false;
                ViewBag.Inativa = Inativa.HasValue ? Inativa.Value : false;
                ViewBag.Oportunidade = Oportunidade.HasValue ? Oportunidade.Value : false;
            }
           
            if (Ativo.HasValue && Ativo.Value && Inativa.HasValue && Inativa.Value && Oportunidade.HasValue && Oportunidade.Value)
            {
                //Faz nda todas as situações da vaga
            }
            else if (Inativa.HasValue && Inativa.Value && Ativo.HasValue && Ativo.Value)
                vagas = vagas.Where(x => x.Status_vaga.Equals("Ativa") || x.Status_vaga.Equals("Inativa"));
            else if (Ativo.HasValue && Oportunidade.HasValue && Oportunidade.Value && Ativo.Value)
                vagas = vagas.Where(x => x.Status_vaga.Equals("Ativa") || x.Status_vaga.Equals("Oportunidade"));
            else if (Inativa.HasValue && Oportunidade.HasValue && Inativa.Value && Oportunidade.Value)
                vagas = vagas.Where(x => x.Status_vaga.Equals("Inativa") || x.Status_vaga.Equals("Oportunidade"));
            else if (Ativo.HasValue && Ativo.Value)
                vagas = vagas = vagas.Where(x => x.Status_vaga.Equals("Ativa"));
            else if (Inativa.HasValue && Inativa.Value)
                vagas = vagas = vagas.Where(x => x.Status_vaga.Equals("Inativa"));
            else if (Oportunidade.HasValue && Oportunidade.Value)
                vagas = vagas = vagas.Where(x => x.Status_vaga.Equals("Oportunidade"));

            if (Confidencial.HasValue && Confidencial.Value)
                vagas = vagas.Where(x => x.Confidencial);


            if (!string.IsNullOrEmpty(FlPeriodo))
            {
                try
                {
                    DateTime datainicio;
                    DateTime datafim;
                    datainicio = DateTime.ParseExact(FlPeriodo.Substring(0, 10), "dd/MM/yyyy", new CultureInfo("pt-BR"));
                    datafim = DateTime.ParseExact(FlPeriodo.Substring(13, 10), "dd/MM/yyyy", new CultureInfo("pt-BR"));
                    datafim = datafim.AddDays(1);
                    vagas = vagas.Where(s => s.Dta_Cadastro >= datainicio);
                    vagas = vagas.Where(s => s.Dta_Cadastro <= datafim);
                }
                catch (Exception)
                {
                    ViewBag.erro = "Data de Fim Plano em formato errado.";
                }

            }

            if (vagas == null)
            {
                return HttpNotFound();
            }

              vagas = vagas.OrderBy(x => x.Status_vaga);
           // return View(await vagas.ToListAsync());
            return View(vagas.ToList());

        }
        public void VagaObservacao(int idVaga)
        {
            Response.Redirect("~/VagaObservacao?idVaga=" + idVaga);
        }

        // GET: VagasEmpresa/Create
        public ActionResult Create()
        {
            ViewBag.Idf_Situacao_Atendimento = new SelectList(db.CRM_Situacao_Atendimento, "Idf_Situacao_Atendimento", "Des_Situacao_atendimento");
            return View();
        }

        // POST: VagasEmpresa/Create
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

        // GET: VagasEmpresa/Edit/5
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

        // POST: VagasEmpresa/Edit/5
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

        // GET: VagasEmpresa/Delete/5
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

        // POST: VagasEmpresa/Delete/5
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
