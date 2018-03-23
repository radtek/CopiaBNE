using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Sample.Models.Empresa;
using AdminLTE_Application;
using System.Globalization;
using Sample.DTO;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Sample.Controllers
{
    [Authorize]
    public class EmpresaProspeccaoController : Controller
    {
        // GET: EmpresaProspeccao
        public ActionResult Index(GridProspect model)
        {
            Model db = new Model();
            var empresas = from e in db.VWTanqueEmpresas select e;
           // model.lista = empresas.ToList();
            return View("~/Views/Prospeccao/Index.cshtml", model);
        }

        public ActionResult Empresas(string button, GridProspect model)
        {
            try
            {
                if (button != null)
                    model.pag = 1;

                using (var context = new Model())
                {
                    model.pag = model.pag <= 0 ? 1 : model.pag;
                    model.rowsPag = model.rowsPag <= 1 ? 100 : model.rowsPag;
                    //var RowspPage = new SqlParameter("@RowspPage", model.rowsPag);
                    //var PageNumber = new SqlParameter("@PageNumber", model.pag);
                    ////Filtros
                    //var SigEstado = new SqlParameter("@Sig_Estado", SqlDbType.VarChar, 2) { Value = model.FlEstado};
                    //var NmeCidade = new SqlParameter("@Nme_Cidade", SqlDbType.VarChar, 80) { Value = model.FlCidade };
                    //var Area = new SqlParameter("@Des_Area_BNE", SqlDbType.VarChar, 200) { Value = model.FlArea };
                    //var NumCnpj = new SqlParameter("@Num_CNPJ", null);
                    //var RazSocial = new SqlParameter("Raz_Social", SqlDbType.VarChar, 200);
                    //try
                    //{
                    //    NumCnpj.Value = null;
                    //    RazSocial.Value = null;
                    //}
                    //catch (Exception)
                    //{
                    //    RazSocial.Value = model.FlEmpresa;
                    //}

                    //var QtdTotal = new SqlParameter("@Qtd_Total", SqlDbType.Int) { Direction = ParameterDirection.Output };

                    //var result = context.Database.SqlQuery<VW_Tanque_Empresa>("dbo.SP_Banco_Empresas @PageNumber, @RowspPage, @Num_CNPJ, @Raz_Social, @Des_Area_BNE, @Nme_Cidade, @Sig_Estado, @Qtd_Total out", PageNumber, RowspPage, NumCnpj, RazSocial, Area, NmeCidade, SigEstado, QtdTotal).ToList();
                    //model.lista = result;
                    //model.Qtd_Total = Convert.ToInt32(QtdTotal.Value);
                    //model.TotalPag = (double)model.Qtd_Total / (double)model.rowsPag;
                    //model.TotalPag = Math.Ceiling(model.TotalPag);
                    var baseEmpresas = from s in context.VWbancoEmpresas select s;
                    if (model.FlSituacaoEmpresa.HasValue && model.FlSituacaoEmpresa.Value > 0)
                        baseEmpresas = baseEmpresas.Where(s => s.Idf_Situacao_Filial.Equals(model.FlSituacaoEmpresa.Value));
                    if (!string.IsNullOrEmpty(model.FlPlano))
                        baseEmpresas = baseEmpresas.Where(s => s.Ultimo_Plano.Contains(model.FlPlano));
                    if (!string.IsNullOrEmpty(model.FlArea))
                        baseEmpresas = baseEmpresas.Where(s => s.Des_Area_BNE.Contains(model.FlArea));
                    if (!string.IsNullOrEmpty(model.FlCidade))
                        baseEmpresas = baseEmpresas.Where(s => s.Nme_Cidade.Contains(model.FlCidade));
                    if (!string.IsNullOrEmpty(model.FlEstado))
                        baseEmpresas = baseEmpresas.Where(s => s.Sig_Estado.Contains(model.FlEstado));
                    if (!string.IsNullOrEmpty(model.FlEmpresa))
                    {
                        model.FlEmpresa = model.FlEmpresa.Replace(".", "").Replace("/", "").Replace("-", "");
                        baseEmpresas = baseEmpresas.Where(s => s.Raz_Social.Contains(model.FlEmpresa.ToUpper()) || s.Num_CNPJ.ToString().Equals(model.FlEmpresa.ToUpper()));
                    }
                    if (!string.IsNullOrEmpty(model.FlDataUltimoPlano))
                    {
                        try
                        {
                            DateTime datainicio;
                            DateTime datafim;
                            datainicio = DateTime.ParseExact(model.FlDataUltimoPlano.Substring(0, 10), "dd/MM/yyyy", new CultureInfo("pt-BR"));
                            datafim = DateTime.ParseExact(model.FlDataUltimoPlano.Substring(13, 10), "dd/MM/yyyy", new CultureInfo("pt-BR"));
                            datafim = datafim.AddDays(1);
                            baseEmpresas = baseEmpresas.Where(s => s.Dta_Fim_Plano >= datainicio);
                            baseEmpresas = baseEmpresas.Where(s => s.Dta_Fim_Plano <= datafim);
                        }
                        catch (Exception)
                        {
                            ViewBag.erro = "Data do Fim Plano em formato errado";
                        }
                       
                    }

                    if (!string.IsNullOrEmpty(model.FlDataCadastro))
                    {
                        try
                        {
                            DateTime datainicio;
                            DateTime datafim;
                            datainicio = DateTime.ParseExact(model.FlDataCadastro.Substring(0, 10), "dd/MM/yyyy", new CultureInfo("pt-BR"));
                            datafim = DateTime.ParseExact(model.FlDataCadastro.Substring(13, 10), "dd/MM/yyyy", new CultureInfo("pt-BR"));
                            datafim = datafim.AddDays(1);
                            baseEmpresas = baseEmpresas.Where(s => s.Dta_Cadastro >= datainicio);
                            baseEmpresas = baseEmpresas.Where(s => s.Dta_Cadastro <= datafim);
                        }
                        catch (Exception)
                        {
                            ViewBag.erro = "Data de cadastro em formato errado.";
                        }
                   
                    }

                    if (model.Ordenacao == null && Request.QueryString["ultimatentativacompra"] != null && Request.QueryString["ultimatentativacompra"] == "1")
                        model.Ordenacao = "Data_Ultima_Tentativa_Compra desc";

                    switch (model.Ordenacao)
                    {

                        case "Data_Ultima_acao desc":
                            baseEmpresas = baseEmpresas.OrderByDescending(s => s.Data_Ultima_Acao_Site);
                            break;
                        case "Data_Ultima_acao asc":
                            baseEmpresas = baseEmpresas.OrderBy(s => s.Data_Ultima_Acao_Site);
                            break;
                        case "QtdV_vagas_Ativas desc":
                            baseEmpresas = baseEmpresas.OrderByDescending(s => s.Qtd_Vaga_Ativa);
                            break;
                        case "QtdV_vagas_Ativas asc":
                            baseEmpresas = baseEmpresas.OrderBy(s => s.Qtd_Vaga_Ativa);
                            break;
                        case "Data_Ultima_Tentativa_Compra desc":
                            baseEmpresas = baseEmpresas.OrderByDescending(s => s.Data_Ultima_Tentativa_Compra);
                            break;
                        case "Data_Ultima_Tentativa_Compra asc":
                            baseEmpresas = baseEmpresas.OrderBy(s => s.Data_Ultima_Tentativa_Compra);
                            break;
                        case "Data_Cadastro desc":
                            baseEmpresas = baseEmpresas.OrderByDescending(s => s.Dta_Cadastro);
                            break;
                        case "Data_Cadastro asc":
                            baseEmpresas = baseEmpresas.OrderBy(s => s.Dta_Cadastro);
                            break;
                        case "num_cnpj desc":
                            baseEmpresas = baseEmpresas.OrderByDescending(s => s.Num_CNPJ);
                            break;
                        case "num_cnpj asc":
                            baseEmpresas = baseEmpresas.OrderBy(s => s.Num_CNPJ);
                            break;
                        case "empresa desc":
                            baseEmpresas = baseEmpresas.OrderByDescending(s => s.Raz_Social);
                            break;
                        case "empresa asc":
                            baseEmpresas = baseEmpresas.OrderBy(s => s.Raz_Social);
                            break;
                        case "area desc":
                            baseEmpresas = baseEmpresas.OrderByDescending(s => s.Des_Area_BNE);
                            break;
                        case "area asc":
                            baseEmpresas = baseEmpresas.OrderBy(s => s.Des_Area_BNE);
                            break;
                        case "cidade desc":
                            baseEmpresas = baseEmpresas.OrderByDescending(s => s.Nme_Cidade);
                            break;
                        case "cidade asc":
                            baseEmpresas = baseEmpresas.OrderBy(s => s.Nme_Cidade);
                            break;
                        case "uf desc":
                            baseEmpresas = baseEmpresas.OrderByDescending(s => s.Sig_Estado);
                            break;
                        case "uf asc":
                            baseEmpresas = baseEmpresas.OrderBy(s => s.Sig_Estado);
                            break;
                        case "Dta_Fim_Plano desc":
                            baseEmpresas = baseEmpresas.OrderByDescending(s => s.Dta_Fim_Plano);
                            break;
                        case "Dta_Fim_Plano asc":
                            baseEmpresas = baseEmpresas.OrderBy(s => s.Dta_Fim_Plano);
                            break;
                        case "Ultimo_Plano desc":
                            baseEmpresas = baseEmpresas.OrderByDescending(s => s.Ultimo_Plano);
                            break;
                        case "Ultimo_Plano asc":
                            baseEmpresas = baseEmpresas.OrderBy(s => s.Ultimo_Plano);
                            break;
                        case "total_acao desc":
                            baseEmpresas = baseEmpresas.OrderByDescending(s => s.Total_Acao);
                            break;
                        case "total_acao asc":
                            baseEmpresas = baseEmpresas.OrderBy(s => s.Total_Acao);
                            break;
                        case "Dta_Vencimento desc":
                            baseEmpresas = baseEmpresas.OrderByDescending(s => s.dta_Vencimento);
                            break;
                        case "Dta_Vencimento asc":
                            baseEmpresas = baseEmpresas.OrderBy(s => s.dta_Vencimento);
                            break;
                        case "qtd_funcionario desc":
                            baseEmpresas = baseEmpresas.OrderByDescending(s => s.qtd_Funcionarios);
                            break;
                        case "qtd_funcionario asc":
                            baseEmpresas = baseEmpresas.OrderBy(s => s.qtd_Funcionarios);
                            break;
                        
                        default:
                            baseEmpresas = baseEmpresas.OrderByDescending(s => s.Data_Ultima_Acao_Site);
                            model.Ordenacao = "Data_Ultima_acao desc";
                            break;
                    }
                    
                    model.lista = baseEmpresas.ToPagedList(model.pag,model.rowsPag);
                    List<CobrancaRecorrencia> lsCobranca = new List<CobrancaRecorrencia>();
                    using (var bd = new Model())
                    {

                        foreach (var item in model.lista.Where(x => !x.qtd_prazo_boleto.HasValue))//tipo boleto bancaario
                        {
                            var NumCnpj = new SqlParameter("@Num_CNPJ", item.Num_CNPJ);
                            SqlParameter idfPa;
                            if (item.Idf_Plano_Adquirido.HasValue)
                                idfPa = new SqlParameter("@Idf_Plano_Adquirido", item.Idf_Plano_Adquirido.Value);
                            else
                                idfPa = new SqlParameter("@Idf_Plano_Adquirido", DBNull.Value);

                            var Recorrencia = bd.Database.SqlQuery<CobrancaRecorrencia>("[dbo].[getUltimaCobranca] @num_cnpj, @Idf_Plano_Adquirido", NumCnpj, idfPa);
                            foreach (var rec in Recorrencia.OrderByDescending(x => x.dta_cadastro))
                                lsCobranca.Add(rec);
                        }
                    }
                    model.Recorrencia = lsCobranca;
                }
            }
            catch (Exception ex)
            {
                model.lista = null;// new List<VW_Banco_Empresas>();
                ViewBag.Error = ex.ToString();
            }
          
           
            return View("~/Views/Prospeccao/BaseEmpresas.cshtml",model);
        }
    }
}