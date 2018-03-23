using BNE.BLL;
using BNE.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using IMG = System.Drawing;
using System.Drawing.Imaging;
using System.Net.Http.Headers;

namespace Bne.Web.Services.API.Controllers
{
    /// <summary>
    /// API para consulta dos currículos presentes na base do BNE.
    /// </summary>
    public class CurriculoController : BNEApiController
    {

        #region CarregarFoto
        /// <summary>
        /// Metodo responsável por carregar a foto 
        /// </summary>
        private string CarregarFoto(int IdCurriculo, bool mostrarDadosCompletos)
        {
            if (mostrarDadosCompletos)
            {
                byte[] byteArray = BNE.BLL.PessoaFisicaFoto.RecuperarFotoPorCurriculoId(IdCurriculo);
                if (byteArray != null)
                {
                    Stream streamIn = new MemoryStream(byteArray);
                    IMG.Image img = IMG.Image.FromStream(streamIn);

                    //Proporção de imagem
                    decimal width = img.Width;
                    decimal heigth = img.Height;

                    while (width > 200 || heigth > 200)
                    {
                        width = Math.Truncate(width * Convert.ToDecimal(0.9));
                        heigth = Math.Truncate(heigth * Convert.ToDecimal(0.9));
                    }

                    IMG.Image thumbNail = img.GetThumbnailImage((int)width, (int)heigth, null, new IntPtr());
                    Stream streamOut = new MemoryStream();
                    thumbNail.Save(streamOut, ImageFormat.Jpeg);

                    return Convert.ToBase64String(((MemoryStream)streamOut).ToArray());
                }
                else
                    return null;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region VerDadosCompleto
        private DTO.ResultadoPesquisaCurriculoCompleto VerDadosCompleto(int idCurriculo, Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, bool flgDadosPessoais = false)
        {
            DTO.ResultadoPesquisaCurriculoCompleto rdcc = new DTO.ResultadoPesquisaCurriculoCompleto();
            CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, objUsuarioFilialPerfil, new Curriculo(idCurriculo), flgDadosPessoais, Helper.RecuperarIP());

            //Tenta carregar o cv do solr, se não busca do sql
            BNE.BLL.DTO.Curriculo objCurriculo = Curriculo.CarregarCurriculoSolr(idCurriculo, string.Empty, true) ?? Curriculo.CarregarCurriculoDTO(idCurriculo, Curriculo.DadosCurriculo.Tudo);

            #region Dados Pessoais

            rdcc.vip = objCurriculo.VIP;
            rdcc.idCurriculo = objCurriculo.IdCurriculo;
            rdcc.dtaAtualizacao = objCurriculo.DataAtualizacaoCurriculo.ToString("yyyy-MM-dd");
            rdcc.cidade = objCurriculo.NomeCidade;
            rdcc.estado = objCurriculo.SiglaEstado;

            if (flgDadosPessoais)
            {
                rdcc.cpf = objCurriculo.NumeroCPF;
                rdcc.dtaNascimento = objCurriculo.DataNascimento.ToString("yyyy-MM-dd");
                rdcc.dddCelular = objCurriculo.NumeroDDDCelular;
                rdcc.numCelular = objCurriculo.NumeroCelular;
                rdcc.dddTelefone = objCurriculo.NumeroDDDTelefone;
                rdcc.numTelefone = objCurriculo.NumeroTelefone;
                rdcc.email = objCurriculo.Email;
                rdcc.nomeCompleto = objCurriculo.NomeCompleto;
                rdcc.idade = objCurriculo.Idade;
                //rdcc.orgaoExpeditor = objCurriculo.rg + "/" + objCurriculo.PessoaFisica.SiglaUFEmissaoRG;
                //rdcc.numeroRG = objCurriculo.PessoaFisica.NumeroRG;
                if (objCurriculo.Sexo != null)
                    rdcc.sexo = objCurriculo.Sexo[0];

                rdcc.carteira = objCurriculo.CategoriaHabilitacao;

                rdcc.deficiente = objCurriculo.Deficiencia;

                rdcc.estadoCivil = objCurriculo.EstadoCivil;

                #region Endereco,CEP,Cidade
                rdcc.cepEndereco = objCurriculo.NumeroCEP;
                rdcc.logradouroEndereco = objCurriculo.Logradouro;
                rdcc.numeroEndereco = objCurriculo.NumeroEndereco;
                rdcc.complementoEndereco = objCurriculo.Complemento;
                rdcc.bairroEndereco = objCurriculo.Bairro;

                //Carregar imagem
                rdcc.foto = CarregarFoto(objCurriculo.IdCurriculo, true);

                #endregion
            }

            #endregion

            #region Funções Pretendidas
            foreach (var funcao in objCurriculo.FuncoesPretendidas)
            {
                if (!String.IsNullOrEmpty(rdcc.funcoes))
                    rdcc.funcoes += ";";
                rdcc.funcoes += funcao.NomeFuncaoPretendida;
            }
            rdcc.pretensao = objCurriculo.ValorPretensaoSalarial;
            #endregion

            #region Escolaridade
            rdcc.escolaridade = objCurriculo.UltimaFormacaoCompleta;
            rdcc.formacoes = objCurriculo.Formacoes;
            #endregion

            rdcc.listExperienciaProfissional = objCurriculo.Experiencias;
            foreach (var exp in rdcc.listExperienciaProfissional)
            {
                exp.DataAdmissao = DateTime.Parse(exp.DataAdmissao.ToString("yyyy-MM-dd"));
                if (exp.DataDemissao.HasValue)
                    exp.DataDemissao = DateTime.Parse(exp.DataDemissao.Value.ToString("yyyy-MM-dd"));
            }

            rdcc.idiomas = objCurriculo.Idiomas;

            #region Observacoes
            rdcc.caracteristicasPessoais = string.Format("{0} - {1} - {2}", objCurriculo.Raca, objCurriculo.Altura, objCurriculo.Peso);
            rdcc.outrosConhecimento = objCurriculo.OutrosConhecimentos;
            rdcc.FlagViagem = objCurriculo.DisponibilidadeViajar;
            #endregion

            return rdcc;
        }
        #endregion

        #region RetornoAuditoriaEmpresa
        private HttpResponseMessage RetornoAuditoriaEmpresa(Filial objFilial, UsuarioFilialPerfil objUsuarioFilialPerfil, Curriculo objCurriculo)
        {
            int QuantidadeCurriculosVIPVisualizadosPelaEmpresa = BNE.BLL.CurriculoVisualizacaoHistorico.RecuperarQuantidadeVisualizacaoDadosCompletosCurriculosVIP(objFilial);

            if (objFilial.DataCadastro.DayOfYear == DateTime.Now.DayOfYear && QuantidadeCurriculosVIPVisualizadosPelaEmpresa > 20)
            {
                CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, objUsuarioFilialPerfil, objCurriculo, false, Helper.RecuperarIP(), "O limite de visualizações diário foi atingido, para mais informações ligue 0800 41 2400.");
                return errorRequestPost(HttpStatusCode.Unauthorized, "O limite de visualizações diário foi atingido, para mais informações ligue 0800 41 2400.");
            }
            else if (objFilial.DataCadastro.DayOfYear < DateTime.Now.DayOfYear && QuantidadeCurriculosVIPVisualizadosPelaEmpresa > 5)
            {
                CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, objUsuarioFilialPerfil, objCurriculo, false, Helper.RecuperarIP(), "O limite de visualizações diário foi atingido, para mais informações ligue 0800 41 2400.");
                return errorRequestPost(HttpStatusCode.Unauthorized, "O limite de visualizações diário foi atingido, para mais informações ligue 0800 41 2400.");
            }
            else
            {
                CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, objUsuarioFilialPerfil, objCurriculo, false, Helper.RecuperarIP(), "O limite de visualizações diário foi atingido, para mais informações ligue 0800 41 2400.");
                return errorRequestPost(HttpStatusCode.Unauthorized, "Por motivo de segurança, solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!");
            }
        }
        #endregion

        #region Metodos Da API

        #region ObterCV
        /// <summary>
        /// Retorna o curriculo selecionado pelo usuario a partir do IdCurriculo.
        /// </summary>
        /// <param name="IdCurriculo">Id do currículo a ser retornado.</param>
        /// <param name="FlgDadosdeContato">Indica se deseja que os dados de contato devem ser retornados. Se true, a consulta será contabilizada como visualização do currículo e será descontada do plano do cliente.</param>
        /// <returns>Objeto com dados completos do curriculo com ou sem contato</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Por motivo de segurança a requisição foi rejeitada. Solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!</response>
        /// <response code="403">Somente usuários de empresas cadastrados no BNE tem acesso à API</response>
        /// <response code="404">Currículo inexistente</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [ResponseType(typeof(DTO.ResultadoPesquisaCurriculoCompleto))]
        public HttpResponseMessage ObterCV(int IdCurriculo, Boolean? FlgDadosdeContato)
        {
            try
            {
                BNE.BLL.UsuarioFilialPerfil objUsuarioFilialPerfil;
                objUsuarioFilialPerfil = Login(Request);

                if (objUsuarioFilialPerfil != null)
                {
                    if (objUsuarioFilialPerfil.Filial != null)
                    {
                        //Se setado o valor ele o pega,senão seta false
                        bool flgDadosPessoais = FlgDadosdeContato.HasValue ? FlgDadosdeContato.Value : false;
                        Filial objFilial = Filial.LoadObject(objUsuarioFilialPerfil.Filial.IdFilial);

                        var objCurriculo = new Curriculo(IdCurriculo);

                        if (!objCurriculo.CompleteObject())
                            return errorRequestPost(HttpStatusCode.NotFound, "Currículo inexistente em nossa base de dados!");
                        else
                        {
                            if (flgDadosPessoais)
                            {
                                if (!objFilial.EmpresaEmAuditoria())
                                {
                                    var curriculoEstagio = objCurriculo.CurriculoCompativelComEstagio();
                                    var autorizacaoPelaWebEstagios = objFilial.AvalWebEstagios() && curriculoEstagio;
                                    if (objFilial.PossuiPlanoAtivo() //Se a empresa possui plano ativo
                                                || autorizacaoPelaWebEstagios) // Se tem o parâmetro especifico da webestagios) 
                                    {
                                        if (objCurriculo.VIP())
                                            return Request.CreateResponse(HttpStatusCode.OK, VerDadosCompleto(IdCurriculo, objFilial, objUsuarioFilialPerfil, flgDadosPessoais));
                                        else
                                        {
                                            if (CurriculoVisualizacao.FilialPodeVerDadosCurriculo(objFilial, objCurriculo, autorizacaoPelaWebEstagios))
                                                return Request.CreateResponse(HttpStatusCode.OK, VerDadosCompleto(IdCurriculo, objFilial, objUsuarioFilialPerfil, flgDadosPessoais));
                                            else
                                            {
                                                CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, objUsuarioFilialPerfil, objCurriculo, false, Helper.RecuperarIP(), "Por motivo de segurança, solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!");
                                                return errorRequestPost(HttpStatusCode.Unauthorized, "Por motivo de segurança, solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (objCurriculo.VIP() && objFilial.EmpresaSemPlanoPodeVisualizarCurriculo(1))
                                            return Request.CreateResponse(HttpStatusCode.OK, VerDadosCompleto(objCurriculo.IdCurriculo, objFilial, objUsuarioFilialPerfil, flgDadosPessoais));
                                        else
                                            return RetornoAuditoriaEmpresa(objFilial, objUsuarioFilialPerfil, objCurriculo);

                                    }
                                }
                            }
                            return Request.CreateResponse(HttpStatusCode.OK, VerDadosCompleto(IdCurriculo, objFilial, objUsuarioFilialPerfil, false));
                        }
                    }
                    else
                        return errorRequestPost(HttpStatusCode.Forbidden, "Somente empresas cadastradas tem acesso a currículos!");
                }
                else
                    return errorRequestPost(HttpStatusCode.Forbidden, "Não Autorizado!");
            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Retorna o curriculo selecionado pelo usuario a partir do IdCurriculo.
        /// </summary>
        /// <param name="objParamentros"></param>
        /// <returns>Objeto com dados completos do curriculo com ou sem contato</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Por motivo de segurança a requisição foi rejeitada. Solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!</response>
        /// <response code="403">Somente usuários de empresas cadastrados no BNE tem acesso à API</response>
        /// <response code="404">Currículo inexistente</response>
        /// <response code="500">Internal Server Error</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        [ResponseType(typeof(DTO.ResultadoPesquisaCurriculoCompleto))]
        public HttpResponseMessage ObterCV([FromBody]DTO.PesquisaCurriculoCompleto objParametros)
        {
            try
            {
                BNE.BLL.UsuarioFilialPerfil objUsuarioFilialPerfil;
                if (Request.Headers.Contains("Num_CPF"))
                    objUsuarioFilialPerfil = Login(Request);
                else
                    objUsuarioFilialPerfil = Login(Convert.ToDecimal(objParametros.CPF), objParametros.DataNascimento);

                if (objUsuarioFilialPerfil != null)
                {
                    if (objUsuarioFilialPerfil.Filial != null)
                    {
                        //Se setado o valor ele o pega,senão seta false
                        bool flgDadosPessoais = objParametros.FlgDadosdeContato.HasValue ? objParametros.FlgDadosdeContato.Value : false;
                        Filial objFilial = Filial.LoadObject(objUsuarioFilialPerfil.Filial.IdFilial);

                        var objCurriculo = new Curriculo(objParametros.IdCurriculo);

                        if (!objCurriculo.CompleteObject())
                            return errorRequestPost(HttpStatusCode.NotFound, "Currículo inexistente em nossa base de dados!");
                        else
                        {
                            if (flgDadosPessoais)
                            {
                                if (!objFilial.EmpresaEmAuditoria())
                                {
                                    var curriculoEstagio = objCurriculo.CurriculoCompativelComEstagio();
                                    var autorizacaoPelaWebEstagios = objFilial.AvalWebEstagios() && curriculoEstagio;
                                    if (objFilial.PossuiPlanoAtivo() //Se a empresa possui plano ativo
                                                || autorizacaoPelaWebEstagios) // Se tem o parâmetro especifico da webestagios) 
                                    {
                                        if (objCurriculo.VIP())
                                            return Request.CreateResponse(HttpStatusCode.OK, VerDadosCompleto(objParametros.IdCurriculo, objFilial, objUsuarioFilialPerfil, flgDadosPessoais));
                                        else
                                        {
                                            if (CurriculoVisualizacao.FilialPodeVerDadosCurriculo(objFilial, objCurriculo, autorizacaoPelaWebEstagios))
                                                return Request.CreateResponse(HttpStatusCode.OK, VerDadosCompleto(objParametros.IdCurriculo, objFilial, objUsuarioFilialPerfil, flgDadosPessoais));
                                            else
                                            {
                                                CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, objUsuarioFilialPerfil, objCurriculo, false, Helper.RecuperarIP(), "Por motivo de segurança, solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!");
                                                return errorRequestPost(HttpStatusCode.Unauthorized, "Por motivo de segurança, solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (objCurriculo.VIP() && objFilial.EmpresaSemPlanoPodeVisualizarCurriculo(1))
                                            return Request.CreateResponse(HttpStatusCode.OK, VerDadosCompleto(objCurriculo.IdCurriculo, objFilial, objUsuarioFilialPerfil, flgDadosPessoais));
                                        else
                                            return RetornoAuditoriaEmpresa(objFilial, objUsuarioFilialPerfil, objCurriculo);

                                    }
                                }
                            }
                            return Request.CreateResponse(HttpStatusCode.OK, VerDadosCompleto(objParametros.IdCurriculo, objFilial, objUsuarioFilialPerfil, false));
                        }
                    }
                    else
                        return errorRequestPost(HttpStatusCode.Forbidden, "Somente empresas cadastradas tem acesso a currículos!");
                }
                else
                    return errorRequestPost(HttpStatusCode.Forbidden, "Não Autorizado!");
            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        #region DownloadArquivo
        [HttpGet]
        public HttpResponseMessage DownloadArquivo(decimal cpf)
        {
            try
            {
                string sNomeArquivo;
                var bytes = PessoaFisicaComplemento.CarregarAnexoPorCPFDoStorage(cpf, out sNomeArquivo);

                if (String.IsNullOrEmpty(sNomeArquivo) || bytes == null || bytes.Length <= 0)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Arquivo inexistente");
                
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                var stream = new MemoryStream(bytes);
                result.Content = new StreamContent(stream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue(BNE.BLL.Custom.Helper.GetMIMEType(sNomeArquivo));
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = sNomeArquivo
                };
                return result;

            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
        #endregion DownloadArquivo

        #endregion

        #region PesquisaAvancada
        /// <summary>
        /// Retorna uma lista de currículos baseada nos parâmetros informados.
        /// </summary>
        /// <param name="filtros">Objeto com os filtros a serem aplicados na pesquisa de curriculos</param>
        /// <returns>Objeto contendo a lista de mini currículos filtrada, juntamente com os registros de totalização para paginação</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="401">Usuário não informado ou não cadastrados no BNE tem acesso à API</response>
        /// <response code="500">Internal Server Error</response>
        [ResponseType(typeof(DTO.ResultadoPesquisaCurriculo))]
        public HttpResponseMessage PesquisaAvancada([FromBody]DTO.PesquisaCurriculo filtros)
        {
            try
            {
                BNE.BLL.UsuarioFilialPerfil objUsuarioFilialPerfil;
#if DEBUG
                objUsuarioFilialPerfil = Login(Request);
#else
                if (Request.Headers.Contains("Num_CPF"))
                    objUsuarioFilialPerfil = Login(Request);
                else
                    objUsuarioFilialPerfil = Login(Convert.ToDecimal(filtros.CPF), filtros.DataNascimento);
#endif

                if (objUsuarioFilialPerfil == null)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "Usuário não encontrado");
                }

                BNE.BLL.PesquisaCurriculo objPesquisaCurriculo = new BNE.BLL.PesquisaCurriculo();
                objPesquisaCurriculo.UsuarioFilialPerfil = objUsuarioFilialPerfil;
                objPesquisaCurriculo.FlagPesquisaAvancada = true;
                objPesquisaCurriculo.DescricaoIP = HttpContext.Current.Request.UserHostAddress;

                #region criando objeto de consulta

                Cidade objCidade = BNE.BLL.Custom.Helper.DetectaCidade(filtros.Cidade);
                if (objCidade != null)
                {
                    objPesquisaCurriculo.Cidade = new Cidade(objCidade.IdCidade) { NomeCidade = objCidade.NomeCidade };
                }

                var listaFuncao = new List<PesquisaCurriculoFuncao>();
                if (!String.IsNullOrEmpty(filtros.Funcao))
                {
                    var funcao = Funcao.CarregarPorDescricao(filtros.Funcao);
                    if (funcao != null)
                        listaFuncao.Add(new PesquisaCurriculoFuncao { Funcao = funcao });
                }

                objPesquisaCurriculo.DescricaoPalavraChave = filtros.PalavraChave;
                objPesquisaCurriculo.DescricaoExperiencia = filtros.PalavraChaveExperiencia;

                if (!String.IsNullOrEmpty(filtros.FuncaoExperiencia))
                {
                    var funcao = Funcao.CarregarPorDescricao(filtros.FuncaoExperiencia);
                    if (funcao != null)
                        objPesquisaCurriculo.FuncaoExercida = new Funcao(funcao.IdFuncao) { DescricaoFuncao = funcao.DescricaoFuncao };
                    else
                        objPesquisaCurriculo.DescricaoFuncaoExercida = filtros.FuncaoExperiencia;
                }

                if (!String.IsNullOrEmpty(filtros.Estado))
                    objPesquisaCurriculo.Estado = new Estado(filtros.Estado);

                if (!String.IsNullOrEmpty(filtros.Escolaridade))
                {
                    Escolaridade escolaridade;
                    if (Escolaridade.CarregarPorNome(filtros.Escolaridade, out escolaridade))
                        objPesquisaCurriculo.Escolaridade = escolaridade;
                }

                if (filtros.Sexo.HasValue)
                {
                    objPesquisaCurriculo.Sexo = new Sexo((int)filtros.Sexo);
                    objPesquisaCurriculo.Sexo.SiglaSexo = filtros.Sexo.Value.ToString()[0];
                }

                if (filtros.IdadeMinima.HasValue)
                    objPesquisaCurriculo.NumeroIdadeMin = (short)filtros.IdadeMinima.Value;

                if (filtros.IdadeMaxima.HasValue)
                    objPesquisaCurriculo.NumeroIdadeMax = (short)filtros.IdadeMaxima.Value;

                objPesquisaCurriculo.NumeroSalarioMin = filtros.SalarioMinimo;
                objPesquisaCurriculo.NumeroSalarioMax = filtros.SalarioMaximo;
                objPesquisaCurriculo.QuantidadeExperiencia = (short?)filtros.QuantidadeExperiencia;

                if (!string.IsNullOrEmpty(filtros.CodCPFNome))
                    objPesquisaCurriculo.DescricaoCodCPFNome = filtros.CodCPFNome.Replace(";", ",");
                else
                    objPesquisaCurriculo.DescricaoCodCPFNome = filtros.CodCPFNome;

                if (!string.IsNullOrEmpty(filtros.EstadoCivil))
                {
                    //@todo Carregar estado civil por descricao
                    //objPesquisaCurriculo.EstadoCivil = EstadoCivil.LoadObject(Convert.ToInt32(rcbEstadoCivil.SelectedValue));
                }

                objPesquisaCurriculo.DescricaoBairro = filtros.Bairro;
                objPesquisaCurriculo.DescricaoLogradouro = filtros.Logradouro;
                objPesquisaCurriculo.NumeroCEPMin = filtros.CEPMinimo;
                objPesquisaCurriculo.NumeroCEPMax = filtros.CEPMaximo;

                //Fonte e curso
                //@todo Table para pesquisa
                if (!String.IsNullOrEmpty(filtros.InstituicaoTecnicoGraduacao))
                {
                    Fonte objFonte;
                    if (Fonte.BuscarPorDescricao(filtros.InstituicaoTecnicoGraduacao, out objFonte))
                        objPesquisaCurriculo.FonteTecnicoGraduacao = objFonte;
                }

                //@todo Table para pesquisa
                if (!String.IsNullOrEmpty(filtros.CursoTecnicoGraduacao))
                {
                    Curso objCurso;
                    if (Curso.BuscaCurso(filtros.CursoTecnicoGraduacao, out objCurso))
                        objPesquisaCurriculo.CursoTecnicoGraduacao = objCurso;
                    else
                    {
                        objPesquisaCurriculo.DescricaoCursoTecnicoGraduacao = filtros.CursoTecnicoGraduacao;
                    }
                }

                if (!String.IsNullOrEmpty(filtros.InstituicaoOutrosCursos))
                {
                    Fonte objFonte;
                    if (Fonte.BuscarPorDescricao(filtros.InstituicaoOutrosCursos, out objFonte))
                        objPesquisaCurriculo.FonteOutrosCursos = objFonte;
                }

                if (!String.IsNullOrEmpty(filtros.CursoOutrosCursos))
                {
                    Curso objCurso;
                    if (Curso.BuscaCurso(filtros.CursoOutrosCursos, out objCurso))
                        objPesquisaCurriculo.CursoOutrosCursos = objCurso;
                    else
                    {
                        objPesquisaCurriculo.DescricaoCursoOutrosCursos = filtros.CursoOutrosCursos;
                    }
                }

                objPesquisaCurriculo.RazaoSocial = filtros.EmpresaQueJaTrabalhou;

                //@todo table
                if (!String.IsNullOrEmpty(filtros.AreaEmpresaQueJaTrabalhou))
                {
                    AreaBNE objAreaBNE;
                    if (AreaBNE.CarregarPorDescricao(filtros.AreaEmpresaQueJaTrabalhou, out objAreaBNE))
                    {
                        objPesquisaCurriculo.AreaBNE = objAreaBNE;
                    }
                }

                if (!String.IsNullOrEmpty(filtros.CategoriaHabilitacao))
                {
                    CategoriaHabilitacao objCategoriaHabilitacao;
                    if (CategoriaHabilitacao.CarregarPorDescricao(filtros.CategoriaHabilitacao, out objCategoriaHabilitacao))
                    {
                        objPesquisaCurriculo.CategoriaHabilitacao = objCategoriaHabilitacao;
                    }
                }

                if (!String.IsNullOrEmpty(filtros.TipoVeiculo))
                {
                    TipoVeiculo objTipoVeiculo = TipoVeiculo.CarregarPorDescricao(filtros.TipoVeiculo);
                    if (objTipoVeiculo != null)
                    {
                        objPesquisaCurriculo.TipoVeiculo = objTipoVeiculo;
                    }
                }

                if (!String.IsNullOrEmpty(filtros.Deficiencia))
                {
                    Deficiencia objDeficiencia = Deficiencia.CarregarPorDescricao(filtros.Deficiencia);
                    if (objDeficiencia != null)
                    {
                        objPesquisaCurriculo.Deficiencia = objDeficiencia;
                    }
                }

                objPesquisaCurriculo.NumeroDDDTelefone = filtros.DDDTelefone;
                objPesquisaCurriculo.NumeroTelefone = filtros.NumeroTelefone;
                objPesquisaCurriculo.EmailPessoa = filtros.Email;

                if (!String.IsNullOrEmpty(filtros.Raca))
                {
                    Raca objRaca = Raca.CarregarPorDescricao(filtros.Raca);
                    objPesquisaCurriculo.Raca = objRaca;
                }

                if (filtros.PossuiFilhos.HasValue)
                    objPesquisaCurriculo.FlagFilhos = filtros.PossuiFilhos.Value;

                var listPesquisaCurriculoIdioma = new List<PesquisaCurriculoIdioma>();
                var listPesquisaCurriculoDisponibilidade = new List<PesquisaCurriculoDisponibilidade>();
                var listaCampoBuscaPalavraChave = new List<CampoPalavraChavePesquisaCurriculo>();
                #endregion criando objeto de consulta

                objPesquisaCurriculo.Salvar(listaFuncao, listPesquisaCurriculoIdioma, listPesquisaCurriculoDisponibilidade, listaCampoBuscaPalavraChave, filtros.QueroContratarEstagiarios);

                DTO.ResultadoPesquisaCurriculo resultadoPesquisa = new DTO.ResultadoPesquisaCurriculo();
                int numeroRegistros;
                decimal mediaSalarial;
                int PageSize = filtros.RegistrosPorPagina > 0 ? filtros.RegistrosPorPagina : Convert.ToInt32(Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.QuantidadeItensPorPaginaPesquisaCurriculo));
                int? filial = objUsuarioFilialPerfil.Filial == null ? null : (int?)objUsuarioFilialPerfil.Filial.IdFilial;
                DataTable resultado = BNE.BLL.PesquisaCurriculo.BuscaCurriculo(PageSize, filtros.Pagina, 1, filial, null, objPesquisaCurriculo, out numeroRegistros, out mediaSalarial);

                resultadoPesquisa.TotalDeRegistros = numeroRegistros;
                resultadoPesquisa.RegistrosPorPagina = PageSize;
                List<DTO.MiniCurriculo> lstRetorno = new List<DTO.MiniCurriculo>();
                foreach (DataRow row in resultado.Rows)
                {
                    lstRetorno.Add(new DTO.MiniCurriculo(row));
                }
                resultadoPesquisa.Curriculos = lstRetorno;
                return Request.CreateResponse(HttpStatusCode.OK, resultadoPesquisa);
            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        #endregion

        #endregion
    }
}
