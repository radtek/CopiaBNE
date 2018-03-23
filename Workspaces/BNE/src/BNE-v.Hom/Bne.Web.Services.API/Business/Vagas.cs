using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using Bne.Web.Services.API.DTO;
using BNE.BLL;
using Parametro = BNE.BLL.Enumeradores.Parametro;
using Perfil = BNE.BLL.Enumeradores.Perfil;
using VagaEmpresa = Bne.Web.Services.API.DTO.VagaEmpresa;
using Bne.Web.Services.API.DTO.Enum;

namespace Bne.Web.Services.API.Business
{
    public class Vagas
    {
        /// <summary>
        /// Dictionary usado no mapeamento dos facets para os nomes dos campos do Solr
        /// </summary>
        private static Dictionary<VagaFacetField, string> mapedFacetFields = new Dictionary<VagaFacetField, string>()
        {
            {VagaFacetField.SiglaEstado, "Sig_Estado" },
            {VagaFacetField.NomeCidade, "Nme_Cidade" },
            {VagaFacetField.NomeEmpresa, "Nme_Empresa" },
            {VagaFacetField.Area, "Des_Area_BNE" },
            {VagaFacetField.Escolaridade, "Des_BNE" },
            {VagaFacetField.Curso, "Des_Curso" },
            {VagaFacetField.Deficiencia, "Des_Deficiencia" },
            {VagaFacetField.Funcao, "Des_Funcao" },
            {VagaFacetField.TipoVinculo, "Des_Tipo_Vinculo" },
        };

        public static HttpResponseMessage Salvar(HttpRequestMessage Request, VagaEmpresa vaga, int? idVaga = null)
        {
#if DEBUG
            var num_cpf = 81531382193;
            var num_cnpj = 82344425000182;
#else
            var num_cpf = decimal.Parse(Request.Headers.GetValues("Num_CPF").First());
            var num_cnpj = decimal.Parse(Request.Headers.GetValues("Num_CNPJ").First());
#endif
            #region Carregar pessoa física e filial
            UsuarioFilialPerfil filial_perfil = null;
            Filial filial = null;

            if (!Filial.CarregarPorCnpj(num_cnpj, out filial))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ResultadoVagaDTO { Mensagem = "Impossível carregar filial." });
            }

            if (!UsuarioFilialPerfil.CarregarPorPessoaFisicaFilial(PessoaFisica.CarregarIdPorCPF(num_cpf), filial.IdFilial, out filial_perfil))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ResultadoVagaDTO { Mensagem = "Impossível carregar perfil de usuário." });
            }
            #endregion

            #region Testa a faixa salarial
            if (vaga.SalarioMin.HasValue && vaga.SalarioMax.HasValue)
            {
                var diferenca = vaga.SalarioMax - vaga.SalarioMin;
                if (vaga.SalarioMax != vaga.SalarioMin 
                    && diferenca < 100m)
                {
                    var msg = "Quando informada uma faixa salarial, os valores informados devem ser iguais ou a diferença deve ser no mínimo de R$ 100,00.";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ResultadoVagaDTO { Mensagem = msg });
                }
            }
            #endregion

            #region Parametros
            var parms = new List<Parametro>
            {
                Parametro.QuantidadeDiasPrazoVaga,
                Parametro.IdadeMinima,
                Parametro.IdadeMaxima,
                Parametro.SalarioMinimoNacional,
                Parametro.BolsaMinimaEstagio,
                Parametro.BolsaMinimaAprendiz
            };
            var valores = BNE.BLL.Parametro.ListarParametros(parms);
            #endregion

            #region Validação de idades
            var idadeMinima = Convert.ToInt32(valores[Parametro.IdadeMinima]);
            var idadeMaxima = Convert.ToInt32(valores[Parametro.IdadeMaxima]);
            var msg_idade = string.Format("A data deve estar entre {0} e {1} anos, verifique o ano de nascimento informado..", idadeMinima, idadeMaxima);

            if (vaga.IdadeMin.HasValue)
            {
                if (vaga.IdadeMin < idadeMinima || vaga.IdadeMin > idadeMaxima)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ResultadoVagaDTO { Mensagem = msg_idade });
                }
            }

            if (vaga.IdadeMax.HasValue)
            {
                if (vaga.IdadeMax < idadeMinima || vaga.IdadeMax > idadeMaxima)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ResultadoVagaDTO { Mensagem = msg_idade });
                }
            }

            if (vaga.IdadeMax.HasValue && vaga.IdadeMin.HasValue)
            {
                if ((vaga.IdadeMax - vaga.IdadeMin) < 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ResultadoVagaDTO { Mensagem = "A diferenca de idade deve ser maior do que zero." });
                }
            }
            #endregion

            #region Validação da Função
            Funcao objFuncao;
            Funcao.CarregarPorDescricao(vaga.Funcao, out objFuncao);
            var msg_funcao = "Função Inválida. Só será possível salvar após sua correção";

            if (objFuncao == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ResultadoVagaDTO { Mensagem = msg_funcao });
            }
            #endregion

            #region Instancia a vaga
            BNE.BLL.Vaga objVaga;
            try
            {
                objVaga = (idVaga.HasValue) ? BNE.BLL.Vaga.LoadObject(idVaga.Value) : new BNE.BLL.Vaga
                {
                    FlagInativo = false,
                    FlagAuditada = false,
                    FlagEmpresaEmAuditoria = filial.EmpresaEmAuditoria()
                };
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new ResultadoVagaDTO { Mensagem = "Não foi possível instanciar a vaga informada." });
            }
            #endregion

            #region Tipo de vínculo
            var aux_tipo_vinculo = new List<int>();
            var listVagaTipoVinculo = new List<VagaTipoVinculo>();
            if (vaga.TipoVinculo != null)
            {
                foreach (var vin in vaga.TipoVinculo)
                {
                    var vinc = BNE.BLL.TipoVinculo.CarregarPorDescricao(vin);
                    if (!aux_tipo_vinculo.Contains(vinc.IdTipoVinculo))
                    {
                        aux_tipo_vinculo.Add(vinc.IdTipoVinculo);
                        listVagaTipoVinculo.Add(new VagaTipoVinculo
                        {
                            TipoVinculo = vinc,
                            Vaga = objVaga
                        });
                    }
                }
            }

            if (listVagaTipoVinculo.Count == 0)
            {
                var vinc = BNE.BLL.TipoVinculo.CarregarPorDescricao("Efetivo");
                listVagaTipoVinculo.Add(new VagaTipoVinculo
                {
                    TipoVinculo = vinc,
                    Vaga = objVaga
                });
            }
            #endregion

            #region Validação de salário mínimo
            //Estagio 
            decimal salarioMinimo;
            #region [Salario minimo de acordo com o vinculo]
            bool estagioAprendiz = false;
            if (listVagaTipoVinculo.Count().Equals(1) && (from item in listVagaTipoVinculo
                                                  where item.TipoVinculo.IdTipoVinculo.Equals((int)BNE.BLL.Enumeradores.TipoVinculo.Estágio)
                                                  select item.TipoVinculo.IdTipoVinculo).Count() > 0)
            {
                salarioMinimo = Convert.ToDecimal(valores[Parametro.BolsaMinimaEstagio], new System.Globalization.CultureInfo("pt-BR"));
                estagioAprendiz = true;
            }
            else if (listVagaTipoVinculo.Count().Equals(1) && (from item in listVagaTipoVinculo
                                                       where item.TipoVinculo.IdTipoVinculo.Equals((int)BNE.BLL.Enumeradores.TipoVinculo.Aprendiz)
                                                       select item.TipoVinculo.IdTipoVinculo).Count() > 0)
            {
                salarioMinimo = Convert.ToDecimal(valores[Parametro.BolsaMinimaAprendiz], new System.Globalization.CultureInfo("pt-BR"));
                estagioAprendiz = true;
            }
            else
                salarioMinimo = Convert.ToDecimal(valores[Parametro.SalarioMinimoNacional], new System.Globalization.CultureInfo("pt-BR"));
            #endregion

            if (vaga.SalarioMin.HasValue)
            {
                if (vaga.SalarioMin < salarioMinimo)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ResultadoVagaDTO
                    {
                        Mensagem = $"Faixa salarial mínima deve ser maior que {(estagioAprendiz ? string.Empty: "o Salário Mínimo Nacional")} R$ {salarioMinimo}"
                    });
                }
            }

            if (vaga.SalarioMax.HasValue)
            {
                if (vaga.SalarioMax < salarioMinimo)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ResultadoVagaDTO
                    {
                        Mensagem = $"Faixa salarial máxima deve ser maior que {(estagioAprendiz ? string.Empty: "o Salário Mínimo Nacional")} R$ {salarioMinimo}"
                    });
                }
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ResultadoVagaDTO { Mensagem = "Cidade inválida." });
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
            var sexos = new List<string> { "Masculino", "Feminino" };
            if (vaga.Sexo == "Qualquer" || string.IsNullOrEmpty(vaga.Sexo))
            {
                objVaga.Sexo = null;
            }
            else if (sexos.IndexOf(vaga.Sexo) >= 0)
            {
                try
                {
                    objVaga.Sexo = BNE.BLL.Sexo.LoadObject(sexos.IndexOf(vaga.Sexo) + 1);
                }
                catch
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new ResultadoVagaDTO { Mensagem = "Impossível carregar o sexo informado." });
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ResultadoVagaDTO { Mensagem = "Impossível carregar o sexo informado." });
            }
            #endregion

            #region Deficiência
            try
            {
                if (!string.IsNullOrEmpty(vaga.Deficiencia))
                {
                    objVaga.FlagDeficiencia = true;
                    objVaga.Deficiencia = BNE.BLL.Deficiencia.CarregarPorDescricao(vaga.Deficiencia);
                }
                else
                {
                    objVaga.FlagDeficiencia = false;
                    objVaga.Deficiencia = null;
                }
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ResultadoVagaDTO { Mensagem = "Não foi possível carregar a deficiência informada." });
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
            objVaga.QuantidadeVaga = (vaga.Quantidade.HasValue) ? vaga.Quantidade.Value : (short)1;
            objVaga.DescricaoRequisito = vaga.Requisitos;
            objVaga.DescricaoAtribuicoes = vaga.Atribuicoes;


            objVaga.NomeEmpresa = (vaga.NomeFantasia == null) ? filial.NomeFantasia : vaga.NomeFantasia;


            objVaga.NumeroDDD = (vaga.NumDDD == null) ? filial.NumeroDDDComercial : vaga.NumDDD;
            objVaga.NumeroTelefone = (vaga.Telefone == null) ? filial.NumeroComercial : vaga.Telefone;

            if (vaga.Email == null)
            {
                UsuarioFilial objUsuarioFilial;
                try
                {
                    if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(filial_perfil.IdUsuarioFilialPerfil, out objUsuarioFilial))
                    {
                        objVaga.EmailVaga = objUsuarioFilial.EmailComercial;
                    }
                }
                catch
                {
                    objVaga.EmailVaga = null;
                }
            }
            else
            {
                objVaga.EmailVaga = vaga.Email;
            }


            objVaga.FlagConfidencial = vaga.Confidencial;
            objVaga.FlagReceberCadaCV = vaga.ReceberCadaCV;
            objVaga.FlagReceberTodosCV = vaga.ReceberTodosCV;
            objVaga.FlagLiberada = false;
            objVaga.FlagAuditada = false;
            objVaga.DataAbertura = DateTime.Now;
            objVaga.DataPrazo = DateTime.Now.AddDays(Convert.ToInt32(valores[Parametro.QuantidadeDiasPrazoVaga]));
            #endregion

            #region Carrega a origem
            objVaga.UsuarioFilialPerfil = filial_perfil;
            objVaga.Filial = filial;

            OrigemFilial objOrigem = null;
            if (OrigemFilial.CarregarPorFilial(filial.IdFilial, out objOrigem))
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
            var auditada = false;
            if ((filial_perfil.Perfil.IdPerfil == (int)Perfil.AdministradorSistema) || (filial_perfil.Perfil.IdPerfil == (int)Perfil.Revisor))
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
            var aux_disponibilidades = new List<int>();
            var listVagaDisponibilidade = new List<VagaDisponibilidade>();
            if (vaga.Disponibilidade != null)
            {
                foreach (var dis in vaga.Disponibilidade)
                {
                    try
                    {
                        var disp = BNE.BLL.Disponibilidade.CarregarPorDescricao(dis);
                        if (!aux_disponibilidades.Contains(disp.IdDisponibilidade))
                        {
                            aux_disponibilidades.Add(disp.IdDisponibilidade);
                            listVagaDisponibilidade.Add(new VagaDisponibilidade
                            {
                                Disponibilidade = disp,
                                Vaga = objVaga
                            });
                        }
                    }
                    catch
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new ResultadoVagaDTO { Mensagem = string.Format("\"{0}\" não é uma disponibilidade válida.", dis) });
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


            #region VagaCurso
            var listVagaCurso = new List<VagaCurso>();

            // Somente grava o curso se a vaga é de estágio
            if (listVagaTipoVinculo.Any(tv => tv.TipoVinculo.IdTipoVinculo == 1 || tv.TipoVinculo.IdTipoVinculo == 4))
                listVagaCurso = VagaCurso.ResolverVagaCurso(vaga.Cursos, objVaga);

            #endregion VagaCurso

            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    #region Persiste a vaga no banco
                    try
                    {
                        objVaga.SalvarVaga(listVagaDisponibilidade, listVagaTipoVinculo, listVagaPergunta, listVagaCurso, auditada, null, BNE.BLL.Enumeradores.VagaLog.ServicesAPIBusiness, trans);
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new ResultadoVagaDTO { Mensagem = string.Format("Ocorreu um erro e não foi possível publicar a vaga. {0}", ex.Message) });
                    }
                    #endregion

                    #region Se a vaga não está publicada, manda para publicação automática
                    try
                    {
                        if (!filial.EmpresaEmAuditoria() && !auditada)
                            objVaga.Publicar();
                    }
                    catch
                    {
                        trans.Rollback();
                        return Request.CreateResponse(HttpStatusCode.InternalServerError, new ResultadoVagaDTO { Mensagem = "Não foi possível enviar a vaga para a fila de publicação/auditoria." });
                    }
                    #endregion

                    trans.Commit();
                    return Request.CreateResponse(HttpStatusCode.OK, new ResultadoVagaDTO { CodigoVaga = objVaga.IdVaga, Mensagem = "Vaga publicada com sucesso!" });
                }
            }
        }
        
        /// <summary>
        /// Mapeia os facets solicitados para os respectivos campos do Solr
        /// </summary>
        /// <param name="facetFields"></param>
        /// <returns></returns>
        public static string[] MapFacetFields(VagaFacetField[] facetFields)
        {
            if (facetFields == null) return null;

            var retorno = new List<string>();

            foreach (var facetField in facetFields)
                if (mapedFacetFields.ContainsKey(facetField))
                    retorno.Add(mapedFacetFields[facetField]);

            return retorno.ToArray();
        }

        /// <summary>
        /// Mapeia o enum Facet baseado no nome do campo Solr
        /// </summary>
        /// <param name="solrFieldName"></param>
        /// <returns></returns>
        public static VagaFacetField GetFacetField(string solrFieldName)
        {
            return mapedFacetFields.FirstOrDefault(x => x.Value == solrFieldName).Key;
        }
    }
}