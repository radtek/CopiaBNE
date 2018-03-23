using BNE.BLL.Custom;
using BNE.Web.LanHouse.BLL;
using BNE.Web.LanHouse.Code;
using BNE.Web.LanHouse.Code.Enumeradores;
using BNE.Web.LanHouse.EntityFramework;
using BNE.Web.LanHouse.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Helper = BNE.BLL.Custom.Helper;

namespace BNE.Web.LanHouse.Controllers
{
    public partial class AjaxController : AbstractController
    {

        #region BuscarQuantidadeCandidaturas
        [AutorizadoQC]
        [ActionName("qc")]
        public ActionResult BuscarQuantidadeCandidaturas()
        {
            int idPessoaFisica = IdPessoaFisica(); // Convert.ToInt32(HttpContext.User.Identity.Name);//(int)Session[Chave.IdPessoaFisica.ToString()];
            return Json(BLL.OportunidadeCurriculo.QuantidadeCandidaturas(idPessoaFisica));
        }
        #endregion BuscarQuantidadeCandidaturas

        #region CurriculoVip
        [AutorizadoQC]
        [ActionName("vip")]
        public ActionResult CurriculoVip()
        {
            int idPessoaFisica = IdPessoaFisica(); // Convert.ToInt32(HttpContext.User.Identity.Name);//(int)Session[Chave.IdPessoaFisica.ToString()];
            BNE_Curriculo objCurriculo;
            if (!BLL.Curriculo.CarregarPorPessoaFisica(idPessoaFisica, out objCurriculo))
                throw new InvalidOperationException("Pessoa física não tem currículo!");

            return Json(objCurriculo.Flg_VIP);
        }
        #endregion BuscarQuantidadeCandidaturas

        #region VerificarSeUsuarioEstaLogado
        [AutorizadoLogado]
        [ActionName("logado")]
        public ActionResult VerificarSeUsuarioEstaLogado()
        {
            return Json(IdPessoaFisica() != 0);
        }
        #endregion VerificarSeUsuarioEstaLogado

        #region BuscarCartaApresentacao
        [ActionName("ca")]
        public ActionResult BuscarCartaApresentacao(string c)
        {
            decimal cnpjDecimal = Convert.ToDecimal(c);

            LAN_Companhia companhia;
            if (BLL.Companhia.CarregarPorCnpj(cnpjDecimal, out companhia))
            {
                string carta;
                Companhia.RecuperarCartaApresentacao(cnpjDecimal, out carta);
                return Json(new { c = carta, n = companhia.TAB_Filial != null ? companhia.TAB_Filial.Nme_Fantasia : companhia.Nme_Companhia });
            }

            return MensagemErro(String.Format("Não foi possível recuperar a carta de apresentação da empresa de CNPJ: {0}", c));
        }
        #endregion BuscarCartaApresentacao

        #region BuscarFilial
        [ActionName("f")]
        public ActionResult BuscarFilial(ModelAjaxBuscarFilial model)
        {
            int idCidade = IdCidadeLAN();
            int idPessoaFisica = IdPessoaFisica();

            if (idCidade == 0)
                throw new InvalidOperationException("A cidade da LAN House não está definida; isso pode ter ocorrido porque o site ficou muito tempo aberto e sem utilização. Favor tentar novamente!");

            TAB_Cidade objCidade;
            Cidade.CarregarPorId(idCidade, out objCidade);

            string busca = string.Empty;

            if (!String.IsNullOrWhiteSpace(model.TermoBusca))
                busca = Helper.RemoverAcentos(model.TermoBusca.Trim());

            int? idCurriculo = null;
            BNE_Curriculo objCurriculo;
            if (BLL.Curriculo.CarregarPorPessoaFisica(idPessoaFisica, out objCurriculo))
                idCurriculo = objCurriculo.Idf_Curriculo;

            var oportunidades = BLL.Oportunidade.CarregarOportunidades(objCidade, busca, idCurriculo, model.Index, model.Count);

            if (oportunidades != null)
            {
                var retorno = oportunidades.Select(delegate(BUSCA_EMPRESA_RETORNO o)
                    {
                        string carta;
                        Companhia.RecuperarCartaApresentacao(o.CNPJ.Value, out carta);

                        return new
                        {
                            cnpj = o.CNPJ,
                            filial = o.Filial,
                            nomeFantasia = o.NomeFantasia,
                            candidatado = o.Candidatado,
                            destaqueCss = o.Prioridade.Equals(10) ? "destaque" : o.Prioridade.Equals(5) ? "destaque-small" : "normal",
                            apresentacao = o.Prioridade.Equals(10) ? carta.Truncate(200) : carta.Truncate(100),
                            p = idPessoaFisica != 0,
                            o = o.RecrutamentoTerceirizado ?? string.Empty
                        };
                    });
                //.ToArray();

                return Json(retorno);
            }

            return Json(new object[0]);
        }
        #endregion BuscarFilial

        #region BuscarCurriculo
        [AutorizadoLogado]
        [ActionName("cv")]
        public ActionResult BuscarCurriculo()
        {
            BNE_Curriculo objCurriculo;
            if (BLL.Curriculo.CarregarPorPessoaFisica(IdPessoaFisica(), out objCurriculo))
            {
                var data = DateTime.Now;
                var cidade = BNE.BLL.Cidade.LoadObject(IdCidadeLAN()).NomeCidade;
                var miniCv = new BLL.Entity.MiniCurriculo(objCurriculo);

                var destino = string.Concat("/", Session[Chave.DiretorioSTCLan.ToString()], "/cadastro-de-curriculo-gratis");
                var link = string.Concat("http://www.bne.com.br/logar/", LoginAutomatico.GerarHashAcessoLoginAutomatico(miniCv.Cpf, miniCv.DataNasc, destino)); //  string.Empty), "?", destino);

                return Json(new
                {
                    c = Code.Helper.FormatarCPF(miniCv.Cpf),
                    d = miniCv.DataNasc.ToString("dd/MM/yyyy"),
                    e = miniCv.Email,
                    f = miniCv.Cargo,
                    s = Code.Helper.FormatarSalario(miniCv.Salario),
                    nc = miniCv.NomeCompleto,
                    n = FormatarNomePessoa(miniCv.NomeCompleto),
                    ddd = miniCv.DDD,
                    l = miniCv.NumCelular,
                    x = (int)miniCv.Sexo,
                    i = miniCv.IdadePessoa,
                    dc = string.Format("{0}, {1} de {2} de {3}", cidade, data.Day, DateTimeFormatInfo.CurrentInfo.GetMonthName(data.Month), data.Year),
                    link = link
                });
            }

            return Json(false);
        }
        #endregion BuscarCurriculo

        #region SeInteressar
        public ActionResult SeInteressar(string c, string f)
        {
            decimal cnpj;
            int filial;
            if (String.IsNullOrEmpty(c) || !Decimal.TryParse(c, NumberStyles.Number, CultureInfo.GetCultureInfo("pt-br"), out cnpj) ||
                String.IsNullOrEmpty(f) || !Int32.TryParse(f, out filial))
                return Json(false);

            Session[Chave.CnpjOportunidade.ToString()] = cnpj;
            Session[Chave.IdFilialOportunidade.ToString()] = filial;

            // verifica se o usuario logado ja se candidatou para essa vaga
            int idPessoaFisica = IdPessoaFisica();
            if (idPessoaFisica != 0)
            {
                bool jaSeCandidatou = BLL.Oportunidade.JaSeCandidatou(cnpj, filial, idPessoaFisica);
                return Json(!jaSeCandidatou);
            }

            return Json(true);
        }
        #endregion

        #region FormatarCpf
        private string FormatarCpf(decimal cpf)
        {
            return cpf.ToString("00000000000");
        }
        #endregion FormatarCpf

        #region FormatarNomePessoa
        private string FormatarNomePessoa(string nome)
        {
            return BNE.BLL.PessoaFisica.RetornarPrimeiroNome(nome);
        }
        #endregion FormatarNomePessoa

        #region BuscarDadosPessoa
        [AutorizadoLogado]
        [ActionName("dp")]
        public ActionResult BuscarDadosPessoa(string dado)
        {
            if (String.IsNullOrEmpty(dado))
                throw new ArgumentNullException("Não é possível obter dados de um campo que não foi informado", "dado");

            int idPessoaFisica = IdPessoaFisica();

            if (idPessoaFisica == 0)
                throw new InvalidOperationException("O usuário atual não está logado. Isso pode ocorrer quando o site fica muito tempo aberto sem utilização. Favor tentar novamente!");

            TAB_Pessoa_Fisica objPessoaFisica;
            if (!BLL.PessoaFisica.CarregarPorId(idPessoaFisica, out objPessoaFisica))
                throw new InvalidOperationException("O usuário atual não existe na base de dados! Favor contactar o administrador do site");

            switch (dado)
            {
                case "cpf":
                    return Json(new
                    {
                        cpf = FormatarCpf(objPessoaFisica.Num_CPF)
                    });

                case "nome":
                    return Json(new
                    {
                        nome = FormatarNomePessoa(objPessoaFisica.Nme_Pessoa)
                    });

                default:
                    return Json(null);
            }
        }
        #endregion BuscarDadosPessoa

        #region ValidaCelular
        [ActionName("vc")]
        public ActionResult ValidaCelular(string ddd, string numero, bool enviarNovamente)
        {
            numero = numero.Replace("-", string.Empty);

            DateTime? dataEnvio;
            BNE.BLL.PessoaFisica.ValidacaoCelularCodigoEnviado(ddd, numero, out dataEnvio);
            if (dataEnvio.HasValue && !enviarNovamente)
            {
                return Json(new { message = string.Format("Foi enviado um código para este número no dia {0}. Insira o código no campo código de validação para validar seu celular.", dataEnvio) });
            }
            else
            {
                BNE.BLL.PessoaFisica.ValidacaoCelularEnviarCodigo(ddd, numero);
                return Json(new { message = "Foi enviado um código para este número. Insira o código no campo código de validação para validar seu celular." });
            }

            return Json(true);
        }
        #endregion BuscarFilial

        #region AutoCompleteCidade
        [HttpPost]
        public JsonResult AutoCompleteCidade(string term)
        {
            IEnumerable<TAB_Cidade> objCidades;
            BLL.Cidade.CarregarPorDescricao(term, out objCidades);

            int limite = BLL.Parametro.NumeroResultadosAutoCompleteCidade();

            return Json(objCidades
                .Take(limite)
                .Select(c =>
                    new
                    {
                        id = c.Idf_Cidade,
                        label = Code.Helper.FormatarCidadeUF(c.Nme_Cidade, c.Sig_Estado),
                        value = Code.Helper.FormatarCidadeUF(c.Nme_Cidade, c.Sig_Estado)
                    })
                .ToArray(), JsonRequestBehavior.AllowGet);
        }
        #endregion AutoCompleteCidade

        #region AutoCompleteFonte
        [HttpPost]
        public JsonResult AutoCompleteFonte(string term)
        {
            IEnumerable<TAB_Fonte> objFontes;
            BLL.Fonte.CarregarPorDescricaoOuSigla(term, out objFontes);

            return Json(objFontes.Select(f =>
                new
                {
                    id = f.Idf_Fonte,
                    label = String.Format("{0} {1}", f.Sig_Fonte, f.Nme_Fonte),
                    value = String.Format("{0} {1}", f.Sig_Fonte, f.Nme_Fonte)
                })
                .ToArray(), JsonRequestBehavior.AllowGet);
        }
        #endregion AutoCompleteFonte

        #region AutoCompleteFuncao
        [HttpPost]
        public JsonResult AutoCompleteFuncao(string term)
        {
            IEnumerable<TAB_Funcao> objFuncoes;
            BLL.Funcao.CarregarPorDescricao(term, out objFuncoes);

            int limite = BLL.Parametro.NumeroResultadosAutoCompleteFuncao();

            return Json(objFuncoes
                .Take(limite)
                .Select(f =>
                    new
                    {
                        id = f.Idf_Funcao,
                        label = f.Des_Funcao,
                        value = f.Des_Funcao
                    })
                .ToArray(), JsonRequestBehavior.AllowGet);
        }
        #endregion AutoCompleteFuncao

    }
}
