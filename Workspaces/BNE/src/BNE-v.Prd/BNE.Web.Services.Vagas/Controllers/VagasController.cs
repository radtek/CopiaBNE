using BNE.BLL;
using BNE.BLL.AsyncServices.Enumeradores;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Web.Services.Vagas.DTO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using bs = BNE.Web.Services.Vagas.Business;

namespace BNE.Web.Services.Vagas.Controllers
{
    public class VagasController : ApiController
    {

        public HttpResponseMessage Salvar([FromBody] VagaDTO vaga)
        {
            decimal num_cpf  = decimal.Parse(Request.Headers.GetValues("Num_CPF").First());
            int idf_filial   = int.Parse(Request.Headers.GetValues("Idf_Filial").First());

            #region Verifica o estado das informações enviadas
            if (vaga == null) 
                return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = "Nenhum objeto VagaDTO foi informado! Verfique se o json enviado é válido." });

            string error_message = "";
            if (!ModelState.IsValid) 
            {
                foreach (var ms in ModelState.Values)
                {
                    foreach (var error in ms.Errors)
                    {
                        if(error.Exception != null)
                            error_message += error.Exception.Message + " ";
                        else if(!string.IsNullOrEmpty(error.ErrorMessage))
                            error_message += error.ErrorMessage + " ";
                    }
                }
                return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = error_message });
            }
	        #endregion
           
            #region Carregar pessoa física e filial
            UsuarioFilialPerfil filial_perfil = null;
            Filial filial = null;

            filial = Filial.LoadObject(idf_filial);

            if (filial == null)
            {
                return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = "Impossível carregar filial." });
            }

            if(!UsuarioFilialPerfil.CarregarPorPessoaFisicaFilial(PessoaFisica.CarregarIdPorCPF(num_cpf), idf_filial, out filial_perfil))
            {
                return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = "Impossível carregar perfil de usuário." });
            }
            #endregion

            #region Testa a faixa salarial
            if (vaga.SalarioMin.HasValue && vaga.SalarioMax.HasValue) 
            {
                var diferenca = vaga.SalarioMax - vaga.SalarioMin;
                if (diferenca <= 100m)
                {
                    string msg = "Quando informada uma faixa salarial, os valores informados devem ser iguais ou a diferença deve ser no mínimo de R$ 100,00.";
                    return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = msg });
                }
            }
            #endregion

            #region Parametros
            var parms = new List<BNE.BLL.Enumeradores.Parametro>
            {
                BNE.BLL.Enumeradores.Parametro.QuantidadeDiasPrazoVaga,
                BNE.BLL.Enumeradores.Parametro.IdadeMinima,
                BNE.BLL.Enumeradores.Parametro.IdadeMaxima,
                BNE.BLL.Enumeradores.Parametro.SalarioMinimoNacional
            };
            var valores = BNE.BLL.Parametro.ListarParametros(parms);
            #endregion

            #region Validação de idades
            var idadeMinima = Convert.ToInt32(valores[BNE.BLL.Enumeradores.Parametro.IdadeMinima]);
            var idadeMaxima = Convert.ToInt32(valores[BNE.BLL.Enumeradores.Parametro.IdadeMaxima]);
            string msg_idade = string.Format("A data deve estar entre {0} e {1} anos, verifique o ano de nascimento informado..", idadeMinima, idadeMaxima);

            if (vaga.IdadeMin.HasValue) 
            {
                if (vaga.IdadeMin < idadeMinima || vaga.IdadeMin > idadeMaxima)
                {
                    return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = msg_idade });
                }
            }

            if (vaga.IdadeMax.HasValue)
            {
                if (vaga.IdadeMax < idadeMinima || vaga.IdadeMax > idadeMaxima)
                {
                    return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = msg_idade });
                }
            }

            if (vaga.IdadeMax.HasValue && vaga.IdadeMin.HasValue) 
            {
                if ((vaga.IdadeMax - vaga.IdadeMin) < 0) 
                {
                    return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = "A diferenca de idade deve ser maior do que zero." });
                }  
            }
            #endregion

            #region Validação de salário mínimo
            var salarioMinimo = Convert.ToDecimal(valores[BNE.BLL.Enumeradores.Parametro.SalarioMinimoNacional]);
            if (vaga.SalarioMin.HasValue)
            {
                if (vaga.SalarioMin < salarioMinimo)
                {
                    return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { 
                        Mensagem = string.Format("Faixa salarial mínima deve ser maior que o Salário Mínimo Nacional R$ {0}", salarioMinimo) }); 
                }
            }

            if (vaga.SalarioMax.HasValue)
            {
                if (vaga.SalarioMax < salarioMinimo)
                {
                    return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.BadRequest, new ResultadoVagaDTO() {
                        Mensagem = string.Format("Faixa salarial máxima deve ser maior que o Salário Mínimo Nacional R$ {0}", salarioMinimo)
                    });
                }
            }
            #endregion

            #region Validação da Função
            Funcao objFuncao;
            Funcao.CarregarPorDescricao(vaga.Funcao, out objFuncao);
            string msg_funcao = "Função Inválida. Só será possível salvar após sua correção";

            if (objFuncao == null)
            {
                return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = msg_funcao }); 
            }

            if (!Funcao.ValidarFuncaoLimitacaoIntegracaoWebEstagios(objFuncao.DescricaoFuncao))
            {
                return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = msg_funcao }); 
            }
            #endregion

            #region Instancia a vaga
            Vaga objVaga;
            try 
            {
                objVaga = (vaga.Idf_Vaga.HasValue) ? Vaga.LoadObject(vaga.Idf_Vaga.Value) : new Vaga
                {
                    FlagVagaRapida = false,
                    FlagInativo = false,
                    FlagAuditada = false,
                    FlagEmpresaEmAuditoria = filial.EmpresaEmAuditoria()
                };
            }
            catch 
            {
                return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.InternalServerError, new ResultadoVagaDTO() { Mensagem = "Não foi possível instanciar a vaga informada." });
            }
            #endregion
            
            #region Cidade
            Cidade objCidade = null;
            if (!string.IsNullOrEmpty(vaga.Cidade))
            {
                if (Cidade.CarregarPorNome(vaga.Cidade, out objCidade))
                    objVaga.Cidade = objCidade;
            }

            if (objCidade == null)
                return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = "Cidade inválida." });
            #endregion

            #region Escolaridade
            if (!string.IsNullOrEmpty(vaga.Escolaridade))
            {
                Escolaridade escolaridade = null;
                Escolaridade.CarregarPorNome(vaga.Escolaridade, out escolaridade);
                objVaga.Escolaridade = escolaridade;
            }
            #endregion

            #region Sexo
            List<string> sexos = new List<string>() { "Masculino", "Feminino" };
            if (sexos.IndexOf(vaga.Sexo) >= 0) 
            {
                try
                {
                    objVaga.Sexo = Sexo.LoadObject(sexos.IndexOf(vaga.Sexo) + 1);
                }
                catch
                {
                    return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = "Impossível carregar o sexo informado." });
                }
            }
            else 
            {
                return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = "Impossível carregar o sexo informado." });
            }
            #endregion

            #region Deficiência
            try 
            {
                if (!string.IsNullOrEmpty(vaga.Deficiencia))
                {
                    objVaga.FlagDeficiencia = true;
                    objVaga.Deficiencia = Deficiencia.CarregarPorDescricao(vaga.Deficiencia);
                }
                else
                {
                    objVaga.FlagDeficiencia = false;
                    objVaga.Deficiencia = null;
                }
            }
            catch 
            {
                return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = "Não foi possível carregar a deficiência informada." });
            }
            #endregion

            #region Carrega os valores para dentro do objeto de vaga
            objVaga.ValorSalarioDe = vaga.SalarioMin;
            objVaga.ValorSalarioPara = vaga.SalarioMax;
            objVaga.NumeroIdadeMinima = (vaga.IdadeMin.HasValue) ? vaga.IdadeMin.Value : vaga.IdadeMin;
            objVaga.NumeroIdadeMaxima = (vaga.IdadeMax.HasValue) ? vaga.IdadeMax.Value : vaga.IdadeMax;
            objVaga.DescricaoFuncao = objFuncao.DescricaoFuncao;
            objVaga.Funcao = objFuncao;
            objVaga.DescricaoBeneficio = vaga.Beneficios;
            objVaga.QuantidadeVaga = vaga.Quantidade;
            objVaga.DescricaoRequisito = vaga.Requisitos;
            objVaga.DescricaoAtribuicoes = vaga.Atribuicoes;
            objVaga.NomeEmpresa = vaga.NomeFantasia;
            objVaga.NumeroDDD = vaga.NumDDD;
            objVaga.NumeroTelefone = vaga.Telefone;
            objVaga.EmailVaga = vaga.Email;
            objVaga.FlagConfidencial = vaga.Confidencial.Value;
            objVaga.FlagReceberCadaCV = vaga.ReceberCadaCV;
            objVaga.FlagReceberTodosCV = vaga.ReceberTodosCV;
            objVaga.FlagLiberada = false;
            objVaga.FlagAuditada = false;
            objVaga.DataAbertura = DateTime.Now;
            objVaga.DataPrazo = DateTime.Now.AddDays(Convert.ToInt32(valores[BNE.BLL.Enumeradores.Parametro.QuantidadeDiasPrazoVaga]));
            #endregion

            #region Carrega a origem
            objVaga.UsuarioFilialPerfil = filial_perfil;
            objVaga.Filial = filial;

            OrigemFilial objOrigem = null;
            if (OrigemFilial.CarregarPorFilial(idf_filial, out objOrigem))
            {
                objOrigem.Origem.CompleteObject();
                objVaga.Origem = objOrigem.Origem;
            }
            else
            {
                objVaga.Origem = new Origem(1);
            }
            #endregion

            #region Vaga auditada e liberada se for Admin ou Revisor
            bool auditada = false;
            if ((filial_perfil.Perfil.IdPerfil == (int)BNE.BLL.Enumeradores.Perfil.AdministradorSistema) || (filial_perfil.Perfil.IdPerfil == (int)BNE.BLL.Enumeradores.Perfil.Revisor))
            {
                    if (!objVaga.FlagAuditada.GetValueOrDefault())
                    {
                        auditada = true;
                        objVaga.DataAuditoria = DateTime.Now;
                    }

                    objVaga.FlagLiberada = true;
                    objVaga.FlagAuditada = true;
            }               
            #endregion

            #region Disponibilidade
            List<int> aux_disponibilidades = new List<int>();
            List<VagaDisponibilidade> listVagaDisponibilidade = new List<VagaDisponibilidade>();
            if (vaga.Disponibilidade != null)
            {
                foreach (var dis in vaga.Disponibilidade)
                {
                    try
                    {
                        var disp = Disponibilidade.CarregarPorDescricao(dis);
                        if (!aux_disponibilidades.Contains(disp.IdDisponibilidade))
                        {
                            aux_disponibilidades.Add(disp.IdDisponibilidade);
                            listVagaDisponibilidade.Add(new VagaDisponibilidade()
                            {
                                Disponibilidade = disp,
                                Vaga = objVaga
                            });
                        }
                    }
                    catch 
                    {
                        return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.BadRequest, new ResultadoVagaDTO() { Mensagem = string.Format("\"{0}\" não é uma disponibilidade válida.", dis) }); 
                    }
                }
            }
            #endregion

            #region Tipo de vínculo
            List<int> aux_tipo_vinculo = new List<int>();
            List<VagaTipoVinculo> listVagaTipoVinculo = new List<VagaTipoVinculo>();
            if (vaga.TipoVinculo != null)
            {
                foreach (var vin in vaga.TipoVinculo)
                {
                    var vinc = TipoVinculo.CarregarPorDescricao(vin);
                    if (!aux_tipo_vinculo.Contains(vinc.IdTipoVinculo))
                    {
                        aux_tipo_vinculo.Add(vinc.IdTipoVinculo);
                        listVagaTipoVinculo.Add(new VagaTipoVinculo()
                        {
                            TipoVinculo = vinc,
                            Vaga = objVaga
                        });
                    }
                }
            }
            #endregion

            #region Perguntas
            var listVagaPergunta = new List<VagaPergunta>();
            if (vaga.Perguntas != null)
            {
                foreach (var perg in vaga.Perguntas)
                {
                    listVagaPergunta.Add(new VagaPergunta
                    {
                        DescricaoVagaPergunta = perg.Texto,
                        FlagResposta = (perg.Resposta == "Sim"),
                        Vaga = objVaga,
                        TipoResposta = new TipoResposta(1)
                    });
                }
            }
            #endregion

            #region Palavras-Chave
            List<string> listPalavrasChave = string.IsNullOrEmpty(vaga.PalavrasChave) ? new List<string>() : vaga.PalavrasChave.Split(',').ToList();
            #endregion

            #region Perssite a vaga no banco
            try
            {
                objVaga.SalvarVaga(listVagaDisponibilidade, listVagaTipoVinculo, listVagaPergunta, listPalavrasChave, auditada);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.InternalServerError, new ResultadoVagaDTO() { Mensagem = string.Format("Ocorreu um erro e não foi possível publicar a vaga. {0}", ex.Message) });
            }	 
	        #endregion
            
            #region Se a vaga não está publicada, manda para publicação automática
            try 
            {
                if (!filial.EmpresaEmAuditoria() && !auditada)
                {
                    var parametros = new ParametroExecucaoCollection
                {
                    {"idVaga", "Vaga", objVaga.IdVaga.ToString(CultureInfo.InvariantCulture), objVaga.CodigoVaga},
                    {"EnfileraRastreador", "Deve enfileirar rastreador", "true", "Verdadeiro"}
                };

                    ProcessoAssincrono.IniciarAtividade(
                        TipoAtividade.PublicacaoVaga,
                        BNE.BLL.AsyncServices.PluginsCompatibilidade.CarregarPorMetadata("PublicacaoVaga", "PublicacaoVagaRastreador"),
                        parametros,
                        null,
                        null,
                        null,
                        null,
                        DateTime.Now);
                }
            }
            catch 
            {
                return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.InternalServerError, new ResultadoVagaDTO() { Mensagem = string.Format("Não foi possível enviar a vaga para a fila de publicação/auditoria.") });
            }
            #endregion

            #region Se foi auditada agora, por falha no processo automático, envia a vaga para o precesso do rastreador assincrono
            try
            {
                if (auditada && (objVaga.Deficiencia == null || (objVaga.Deficiencia != null && objVaga.Deficiencia.IdDeficiencia.Equals(0))) && !objVaga.Funcao.IdFuncao.Equals((int)BNE.BLL.Enumeradores.Funcao.Estagiario))
                {
                    var parametros = new ParametroExecucaoCollection
                    {
                        {"idVaga", "Vaga", objVaga.IdVaga.ToString(CultureInfo.InvariantCulture), objVaga.CodigoVaga}
                    };

                    ProcessoAssincrono.IniciarAtividade(
                        TipoAtividade.RastreadorVagas,
                        BNE.BLL.AsyncServices.PluginsCompatibilidade.CarregarPorMetadata("RastreadorVagas", "PluginSaidaEmailSMS"),
                        parametros,
                        null,
                        null,
                        null,
                        null,
                        DateTime.Now);
                }
            }
            catch 
            {
                return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.InternalServerError, new ResultadoVagaDTO() { Mensagem = string.Format("Não foi possível enviar a vaga para a fila do rastreador de vaga.") });
            }
            #endregion

            return Request.CreateResponse<ResultadoVagaDTO>(HttpStatusCode.OK, new ResultadoVagaDTO() { CodigoVaga = objVaga.IdVaga, Mensagem = "Vaga publicada com sucesso!" });    
        }

        [HttpGet]
        public HttpResponseMessage ObterCandidatos([FromUri] int CodigoVaga, int pagina = 1)
        {
            decimal num_cpf = decimal.Parse(Request.Headers.GetValues("Num_CPF").First());
            int idf_filial = int.Parse(Request.Headers.GetValues("Idf_Filial").First());
            HttpResponseMessage response;

            int total_de_registros = 0;
            int total_de_paginas = 0;

            try 
            {
               ResultadoCandidatosDTO result = new ResultadoCandidatosDTO();
               result.Curriculos = bs.Candidatos.ObterCandidatos(CodigoVaga, idf_filial, pagina, out total_de_registros, out total_de_paginas);
               result.TotalPaginas = total_de_paginas;
               result.TotalRegistros = total_de_registros;
               result.Pagina = pagina;
               response = Request.CreateResponse<ResultadoCandidatosDTO>(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message); 
            }

            return response;
        }
    }
}
