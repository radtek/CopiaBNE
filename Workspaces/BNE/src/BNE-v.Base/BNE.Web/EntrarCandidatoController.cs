using BNE.Common.Session;
using BNE.Web.Code;
using BNE.Web.Master;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BNE.Web
{
    public class EntrarCandidatoController : Controller
    {
        #region [ Fields / Attributes ]
        public SessionVariable<int> IdCurriculo = new SessionVariable<int>(BNE.Web.Code.Enumeradores.Chave.Permanente.IdCurriculo.ToString());
        public SessionVariable<int> IdPessoaFisicaLogada = new SessionVariable<int>(BNE.Web.Code.Enumeradores.Chave.Permanente.IdPessoaFisicaLogada.ToString());
        public SessionVariable<int> IdUsuarioFilialPerfilLogadoCandidato = new SessionVariable<int>(BNE.Web.Code.Enumeradores.Chave.Permanente.IdUsuarioFilialPerfilLogadoCandidato.ToString());
        public SessionVariable<int> IdOrigem = new SessionVariable<int>(BNE.Web.Code.Enumeradores.Chave.Permanente.IdOrigem.ToString());
        #endregion

        #region [ Public ]
        public ActionResult Index()
        {
            return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), null);
        }

        public ActionResult Home()
        {
            return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), null);
        }

        public ActionResult Com(string ret, string inParam, string inHash)
        {
            if (string.IsNullOrWhiteSpace(inHash))
            {
                if (string.IsNullOrWhiteSpace(inParam))
                    //return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                    return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), new System.Web.Routing.RouteValueDictionary { { RedirectParamArg, ret } });

                try
                {
                    var cpfRegex = Regex.Match(inParam, "([0-9]{3}.[0-9]{3}.[0-9]{3}-[0-9]{2})");
                    if (!cpfRegex.Success)
                    {
                        //return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                        if (string.IsNullOrEmpty(ret))
                            return Com(string.Empty, string.Empty, inParam);

                        return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), new System.Web.Routing.RouteValueDictionary { { RedirectParamArg, ret } });
                    }

                    var dateRegex = Regex.Match(inParam, "([0-9]{2}/[0-9]{2}/[0-9]{2,4})");
                    if (!dateRegex.Success)
                    {
                        //return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                        if (string.IsNullOrEmpty(ret))
                            return Com(string.Empty, string.Empty, inParam);

                        return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), new System.Web.Routing.RouteValueDictionary { { RedirectParamArg, ret } });
                    }

                    var culture = System.Globalization.CultureInfo.GetCultureInfo("pt-BR");
                    DateTime validDate;
                    if (!DateTime.TryParseExact(dateRegex.Value, "dd/MM/yyyy", culture.DateTimeFormat, DateTimeStyles.None, out validDate)
                        && !DateTime.TryParseExact(dateRegex.Value, "dd/MM/yy", culture.DateTimeFormat, DateTimeStyles.None, out validDate))
                    {
                        if (string.IsNullOrEmpty(ret))
                            return Com(string.Empty, string.Empty, inParam);

                        //return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                        return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), new System.Web.Routing.RouteValueDictionary { { RedirectParamArg, ret } });
                    }
                    var withoutCpf = inParam.Replace(cpfRegex.Value, string.Empty);
                    var withoutCpfAndDate = withoutCpf.Replace(dateRegex.Value, string.Empty);

                    var onlyNumbers = new string(withoutCpfAndDate.TakeWhile(a => char.IsNumber(a)).ToArray());
                    if (onlyNumbers.Length <= 0)
                    {
                        onlyNumbers = new string(withoutCpfAndDate.Reverse().TakeWhile(a => char.IsNumber(a)).Reverse().ToArray());
                        if (onlyNumbers.Length <= 0)
                        {
                            onlyNumbers = new string(withoutCpfAndDate.Where(a => char.IsNumber(a)).ToArray());
                        }
                    }

                    int id;
                    if (!int.TryParse(onlyNumbers, out id))
                        //return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                        return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), new System.Web.Routing.RouteValueDictionary { { RedirectParamArg, ret } });

                    return ProcessarLogin(ret, id, cpfRegex.Value, validDate, null, true);
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                    return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), new System.Web.Routing.RouteValueDictionary { { RedirectParamArg, ret } });
                }
            }

            return ProcessarLogin(ret, inHash, true);
        }

        public ActionResult Usando(string ret, int i, string c, DateTime d)
        {
            return ProcessarLogin(ret, i, c, d, null, true);
        }

        public ActionResult Redirec(string ret, int i, string c, DateTime d)
        {
            return ProcessarLogin(ret, i, c, d, null, false);
        }

        public ActionResult Hash(string ret, string inHash)
        {
            return ProcessarLogin(ret, inHash, true);
        }
        public ActionResult ReTentar(string ret, string inHash)
        {
            return ProcessarLogin(ret, inHash, false);
        }
        #endregion

        #region [ Private ]

        private string AjustarRetorno(string ret)
        {
            if (ret == null)
            {
                ret = "~/";
            }
            else if (ret.IndexOf(".bne.com.br", StringComparison.OrdinalIgnoreCase) == -1)
            {
                if (!ret.StartsWith("~/"))
                {
                    if (ret.StartsWith("/"))
                        ret = "~" + ret;
                    else
                        ret = "~/" + ret;
                }
            }
            return ret;
        }

        private ActionResult ProcessarLogin(string ret, string hash, bool acceptRedirect)
        {
            if (string.IsNullOrWhiteSpace(hash))
            {
                //return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), new System.Web.Routing.RouteValueDictionary { { RedirectParamArg, ret } });
            }

            BLL.Custom.LoginAutomatico user;
            try
            {
                user = BNE.BLL.Custom.LoginAutomatico.RecuperarInformacaoHash(hash);
            }
            catch
            {
                //return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), new System.Web.Routing.RouteValueDictionary { { RedirectParamArg, ret } });
            }

            int curriculoId;
            if (!BLL.Curriculo.CarregarIdPorCpf(user.NumeroCPF, out curriculoId))
            {
                //return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), new System.Web.Routing.RouteValueDictionary { { RedirectParamArg, ret } });
            }

            return ProcessarLogin(ret, curriculoId, user.NumeroCPF.ToString(), user.DataNascimento, user.IdPessoFisica, acceptRedirect);
        }


        public string RedirectParamArg
        {
            get
            {
                return ConfigurationManager.AppSettings["aspnet:FormsAuthReturnUrlVar"] ?? "ReturnUrl";
            }
        }

        private ActionResult ProcessarLogin(string ret, int curriculoId, string cpfDesc, DateTime dtNasc, int? pfId = null, bool acceptRedirect = false)
        {
            ret = AjustarRetorno(ret);

            HttpStatusCodeResult res;
            if (!IsValidParams(curriculoId, cpfDesc, dtNasc, out res))
                //return res;
                return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), new System.Web.Routing.RouteValueDictionary { { RedirectParamArg, ret } });

            try
            {
                var cpf = Convert.ToDecimal(new string(cpfDesc.Where(char.IsNumber).ToArray()));
                BNE.BLL.PessoaFisica objPessoaFisica;
                if (!BNE.BLL.PessoaFisica.CarregarPorCPF(cpf, out objPessoaFisica))
                    //return new HttpStatusCodeResult(System.Net.HttpStatusCode.PreconditionFailed);
                    return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), new System.Web.Routing.RouteValueDictionary { { RedirectParamArg, ret } });

                if (pfId.HasValue && pfId.Value != objPessoaFisica.IdPessoaFisica)
                    //return new HttpStatusCodeResult(System.Net.HttpStatusCode.PreconditionFailed);
                    return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), new System.Web.Routing.RouteValueDictionary { { RedirectParamArg, ret } });

                if (dtNasc.Date != objPessoaFisica.DataNascimento.Date)
                    //return new HttpStatusCodeResult(System.Net.HttpStatusCode.PreconditionFailed);
                    return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), new System.Web.Routing.RouteValueDictionary { { RedirectParamArg, ret } });

                if (objPessoaFisica.FlagInativo.HasValue && objPessoaFisica.FlagInativo.Value)
                    //return new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized);
                    return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), new System.Web.Routing.RouteValueDictionary { { RedirectParamArg, ret } });

                BNE.BLL.UsuarioFilialPerfil objPerfisPessoa;
                if (!BNE.BLL.UsuarioFilialPerfil.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objPerfisPessoa))
                    //return new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized);
                    return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), new System.Web.Routing.RouteValueDictionary { { RedirectParamArg, ret } });

                int idUsuarioFilialPerfil, idPerfil;
                if (BNE.BLL.PessoaFisica.VerificaPessoaFisicaUsuarioInterno(objPessoaFisica.IdPessoaFisica, out idUsuarioFilialPerfil, out idPerfil))
                    //return new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized);
                    return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), new System.Web.Routing.RouteValueDictionary { { RedirectParamArg, ret } });

                BNE.BLL.Curriculo objCurriculo;
                if (!BNE.BLL.Curriculo.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objCurriculo))
                    //return new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized);
                    return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), new System.Web.Routing.RouteValueDictionary { { RedirectParamArg, ret } });

                if (objCurriculo.FlagInativo)
                    //return new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized);
                    return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), new System.Web.Routing.RouteValueDictionary { { RedirectParamArg, ret } });

                if (objCurriculo.SituacaoCurriculo.IdSituacaoCurriculo.Equals((int)BNE.BLL.Enumeradores.SituacaoCurriculo.Bloqueado))
                    //    return new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized);
                    return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), new System.Web.Routing.RouteValueDictionary { { RedirectParamArg, ret } });

                if (objCurriculo.IdCurriculo != curriculoId)
                    //     return new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized);
                    return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), new System.Web.Routing.RouteValueDictionary { { RedirectParamArg, ret } });

                if (IdCurriculo.HasValue)
                {
                    if (IdCurriculo.ValueOrDefault == objCurriculo.IdCurriculo)
                    {
                        return RedirectPermanent(ret);
                    }
                    else
                    {
                        BNE.Auth.BNEAutenticacao.DeslogarPadrao();
                        return RedirectToAction("Redirec", "EntrarCandidato", new { ret = ret, i = curriculoId, c = cpfDesc, d = dtNasc.ToString("dd/MM/yyyy") });

                        //using (var master = new Principal())
                        //{
                        //    using (master.Page = new BasePage())
                        //    {
                        //        master.SairGlobal();

                        //        if (acceptRedirect)
                        //        {
                        //            return RedirectToAction("Forcar", "EntrarCandidato", new { ret = ret, i = curriculoId, c = cpf, d = dtNasc.ToString("dd/MM/yyyy") });
                        //        }
                        //    }
                        //}
                    }
                }

                IdCurriculo.Value = objCurriculo.IdCurriculo;
                IdPessoaFisicaLogada.Value = objPessoaFisica.IdPessoaFisica;
                PageHelper.AtualizarSessaoUsuario(objPessoaFisica.IdPessoaFisica, Session.SessionID);
                using (var pag = new BasePage())
                {
                    pag.GravarCookieAcesso(objPessoaFisica);
                }

                BNE.Auth.BNEAutenticacao.LogarCandidato(objPessoaFisica.NomePessoa, objPessoaFisica.IdPessoaFisica, objPessoaFisica.CPF, objCurriculo.IdCurriculo, false);

                if (ret.IndexOf("|hashAcc|", StringComparison.OrdinalIgnoreCase) > -1
                    || ret.IndexOf("%7ChashAcc%7C", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    try
                    {
                        var hash = BLL.Custom.LoginAutomatico.GerarHashAcessoLogin(objPessoaFisica);
                        ret = ReplaceStringEx(ret, "|hashAcc|", hash, StringComparison.OrdinalIgnoreCase);
                        ret = ReplaceStringEx(ret, "%7ChashAcc%7C", hash, StringComparison.OrdinalIgnoreCase);
                    }
                    catch
                    {

                    }
                }

                BNE.BLL.UsuarioFilialPerfil objUsuarioFilialPerfil;
                if (BNE.BLL.UsuarioFilialPerfil.CarregarPorPessoaFisica(objPessoaFisica.IdPessoaFisica, out objUsuarioFilialPerfil))
                {
                    IdUsuarioFilialPerfilLogadoCandidato.Value = objUsuarioFilialPerfil.IdUsuarioFilialPerfil;
                }

                int diaslimite = Convert.ToInt32(BNE.BLL.Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.DiasLimiteAtualizacaoCurriculo));
                if (objCurriculo.DataAtualizacao >= DateTime.Now.AddDays(-1 * diaslimite))
                {
                    objCurriculo.Salvar();
                }

                if (!BNE.BLL.CurriculoOrigem.ExisteCurriculoNaOrigem(objCurriculo, new BNE.BLL.Origem(IdOrigem.ValueOrDefault)))
                {
                    Session.Add(BNE.Web.Code.Enumeradores.Chave.Temporaria.Variavel1.ToString(), objPessoaFisica.IdPessoaFisica);
                }

                return RedirectPermanent(ret);
            }
            catch (Exception ex)
            {
                BNE.Auth.BNEAutenticacao.DeslogarPadrao();

                EL.GerenciadorException.GravarExcecao(ex);
                return new RedirectToRouteResult(BNE.BLL.Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), new System.Web.Routing.RouteValueDictionary { { RedirectParamArg, ret } });
            }
        }

        public static string ReplaceStringEx(string str, string oldValue, string newValue, StringComparison comparison)
        {
            var sb = new StringBuilder();

            int previousIndex = 0;
            int index = str.IndexOf(oldValue, comparison);
            while (index != -1)
            {
                sb.Append(str.Substring(previousIndex, index - previousIndex));
                sb.Append(newValue);
                index += oldValue.Length;

                previousIndex = index;
                index = str.IndexOf(oldValue, index, comparison);
            }
            sb.Append(str.Substring(previousIndex));

            return sb.ToString();
        }

        private bool IsValidParams(int i, string c, DateTime d, out HttpStatusCodeResult res)
        {
            if (i <= 0)
            {
                res = new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                return false;
            }

            if (string.IsNullOrWhiteSpace(c))
            {
                res = new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                return false;
            }

            c = new string(c.Where(a => char.IsNumber(a)).ToArray());
            if (c.Length != 11)
            {
                res = new HttpStatusCodeResult(System.Net.HttpStatusCode.PreconditionFailed);
                return false;
            }

            if (d == default(DateTime))
            {
                res = new HttpStatusCodeResult(System.Net.HttpStatusCode.PreconditionFailed);
                return false;
            }

            res = new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
            return true;
        }
        #endregion
    }
}