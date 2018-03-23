using BNE.BLL;
using BNE.Web.Promocoes.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Data.SqlClient;

namespace BNE.Web.Promocoes.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Redirect()
        
        {
            return Redirect("BH");
        }

        public JsonResult cad(String json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var oCad = js.Deserialize<Cadastro>(json);
           
            try
            {
                if (ModelState.IsValid)
                {
                    PreCadastro oPre = new PreCadastro();
                    oPre.nome = oCad.nome; // model.nome;
                    oPre.email = oCad.email; //model.email;

                    Funcao oFuncao;
                    Funcao.CarregarPorDescricao(oCad.funcao, out oFuncao);
                    oPre.idFuncao = oFuncao.IdFuncao;
                    Cidade oCidade;
                    Cidade.CarregarPorNome(oCad.cidade, out oCidade);
                    oPre.idCidade = oCidade.IdCidade;
                    oPre.idOrigemPreCadastro = (int)BNE.BLL.Enumeradores.OrigemPreCadastro.CamanhaBH;

                    oPre.Save();

                    //Criar o Mini Curriculo do candidato no BNE
                    //checar se a pessoa fisica já existe
                    int idPessoaFisica;
                    if(PessoaFisica.ExistePessoaFisica(oCad.cpf.Replace(".","").Replace("-",""), out idPessoaFisica))
                    {
                        return Json("True", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        using (SqlConnection conn = new SqlConnection(BLL.DataAccessLayer.CONN_STRING))
                        {
                            conn.Open();

                            using (SqlTransaction trans = conn.BeginTransaction())
                            {
                                var objPessoaFisica = new PessoaFisica();
                                var objCurriculo = new Curriculo();

                                var objUsuarioFilialPerfil = new UsuarioFilialPerfil
                                {
                                    Perfil = new Perfil((int)BNE.BLL.Enumeradores.Perfil.AcessoNaoVIP)
                                };

                                objPessoaFisica.NomePessoa = oCad.nome;
                                objPessoaFisica.NomePessoaPesquisa = oCad.nome;
                                objPessoaFisica.NumeroCPF = oCad.cpf;
                                objPessoaFisica.DataNascimento = oCad.datanascimento;
                                objPessoaFisica.EmailPessoa = oCad.email;
                                objPessoaFisica.FlagInativo = false;
                                objPessoaFisica.FlagCelularConfirmado = false;
                                objPessoaFisica.DescricaoIP = "Indicação BH";
                                objPessoaFisica.Cidade = oCidade;
                                objPessoaFisica.Sexo = BLL.Sexo.LoadObject(oCad.sexo);
                                objPessoaFisica.NumeroCelular = oCad.celular.Replace("(","").Replace(")","").Replace("-","").Trim().Substring(2);
                                objPessoaFisica.NumeroDDDCelular = oCad.celular.Substring(1,2);

                                objPessoaFisica.Save(trans);

                                //funcao pretendida
                                //var listaFuncoes = new List<FuncaoPretendida>();

                                

                                //listaFuncoes.Add(objFuncaoPrentedida);

                                //Usuario Origem CV
                                objUsuarioFilialPerfil = new UsuarioFilialPerfil
                                {
                                    Perfil = new Perfil((int)BLL.Enumeradores.Perfil.AcessoNaoVIP)
                                };

                                Origem objOrigem = new Origem(1);

                                objUsuarioFilialPerfil.PessoaFisica = objPessoaFisica;
                                objUsuarioFilialPerfil.FlagInativo = false;
                                objUsuarioFilialPerfil.SenhaUsuarioFilialPerfil = objPessoaFisica.DataNascimento.ToString("dd/MM/yyyy");
                                objUsuarioFilialPerfil.FlagUsuarioResponsavel = true;

                                objUsuarioFilialPerfil.Save(trans);

                                objCurriculo.PessoaFisica = objPessoaFisica;
                                objCurriculo.TipoCurriculo = TipoCurriculo.LoadObject((int)BLL.Enumeradores.TipoCurriculo.Mini,trans);
                                objCurriculo.SituacaoCurriculo = SituacaoCurriculo.LoadObject((int)BLL.Enumeradores.SituacaoCurriculo.AguardandoPublicacao,trans);
                                objCurriculo.FlagInativo = false;
                                objCurriculo.FlagVIP = false;
                                objCurriculo.FlagMSN = false;
                                objCurriculo.DescricaoIP = objPessoaFisica.DescricaoIP;
                                objCurriculo.DataCadastro = DateTime.Now;
                                objCurriculo.DataAtualizacao = DateTime.Now;
                                objCurriculo.ValorPretensaoSalarial = Convert.ToDecimal(oCad.pretensao);
                                
                                objCurriculo.Save(trans);

                                var objFuncaoPrentedida = new FuncaoPretendida();

                                objFuncaoPrentedida.Funcao = oFuncao;
                                objFuncaoPrentedida.Curriculo = objCurriculo;
                                objFuncaoPrentedida.QuantidadeExperiencia = 0;
                                objFuncaoPrentedida.Save(trans);

                                trans.Commit();
                                trans.Dispose();
                            }
                        }
                    }

                    return Json("True", JsonRequestBehavior.AllowGet);
                }
                else
                    return Json("false", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
