using BNE.BLL;
using BNE.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using IMG = System.Drawing;
using System.Web.UI.WebControls;
using System.Drawing.Imaging;

namespace Bne.Web.Services.API.Controllers
{
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
        private DTO.ResultadoPesquisaCurriculoCompleto VerDadosCompleto(Curriculo objCurriculo, Filial objFilial, bool flgDadosPessoais = false)
        {
            DTO.ResultadoPesquisaCurriculoCompleto rdcc = new DTO.ResultadoPesquisaCurriculoCompleto();
            CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, objCurriculo, true, Helper.RecuperarIP());
            PessoaFisicaComplemento pfc = null;
            PessoaFisicaComplemento.CarregarPorPessoaFisica(objCurriculo.PessoaFisica.IdPessoaFisica, out pfc);

            #region Dados Pessoais

            rdcc.vip = objCurriculo.VIP();

            if (objCurriculo.PessoaFisica != null)
                objCurriculo.PessoaFisica.CompleteObject();

            if (flgDadosPessoais)
            {
                rdcc.cpf = objCurriculo.PessoaFisica.CPF;
                rdcc.dtaNascimento = objCurriculo.PessoaFisica.DataNascimento.ToString("yyyy-MM-dd");
                rdcc.dddCelular = objCurriculo.PessoaFisica.NumeroDDDCelular;
                rdcc.numCelular = objCurriculo.PessoaFisica.NumeroCelular;
                rdcc.email = objCurriculo.PessoaFisica.EmailPessoa;
                rdcc.nomeCompleto = objCurriculo.PessoaFisica.NomeCompleto;
                rdcc.idade = objCurriculo.PessoaFisica.RetornarIdade();
                rdcc.orgaoExpeditor = objCurriculo.PessoaFisica.NomeOrgaoEmissor + "/" + objCurriculo.PessoaFisica.SiglaUFEmissaoRG;
                rdcc.numeroRG = objCurriculo.PessoaFisica.NumeroRG;

                if (objCurriculo.PessoaFisica.Sexo != null)
                {
                    objCurriculo.PessoaFisica.Sexo.CompleteObject();
                    rdcc.sexo = objCurriculo.PessoaFisica.Sexo.SiglaSexo;
                }


                #region CategoriaHabilitacao


                if (pfc != null && pfc.CategoriaHabilitacao != null)
                {
                    pfc.CategoriaHabilitacao.CompleteObject();
                    rdcc.carteira = pfc.CategoriaHabilitacao.DescricaoCategoriaHabilitacao;
                }
                #endregion

                #region Deficiencia
                if (objCurriculo.PessoaFisica.Deficiencia != null)
                {
                    objCurriculo.PessoaFisica.Deficiencia.CompleteObject();
                    rdcc.deficiente = objCurriculo.PessoaFisica.Deficiencia.DescricaoDeficiencia;
                }
                #endregion

                #region EstadoCivil
                if (objCurriculo.PessoaFisica.EstadoCivil != null)
                {
                    objCurriculo.PessoaFisica.EstadoCivil.CompleteObject();
                    rdcc.estadoCivil = objCurriculo.PessoaFisica.EstadoCivil.DescricaoEstadoCivil;
                }
                #endregion

                #region Endereco,CEP,Cidade

                if (objCurriculo.PessoaFisica.Endereco != null)
                {
                    objCurriculo.PessoaFisica.Endereco.CompleteObject();
                    rdcc.cepEndereco = objCurriculo.PessoaFisica.Endereco.NumeroCEP;
                    rdcc.logradouroEndereco = objCurriculo.PessoaFisica.Endereco.DescricaoLogradouro;
                    rdcc.numeroEndereco = objCurriculo.PessoaFisica.Endereco.NumeroEndereco;
                    rdcc.complementoEndereco = objCurriculo.PessoaFisica.Endereco.DescricaoComplemento;
                    rdcc.bairroEndereco = objCurriculo.PessoaFisica.Endereco.DescricaoBairro;

                    if (objCurriculo.PessoaFisica.Endereco.Cidade != null)
                    {
                        objCurriculo.PessoaFisica.Endereco.Cidade.CompleteObject();
                        rdcc.cidade = objCurriculo.PessoaFisica.Endereco.Cidade.NomeCidade;
                    }

                    if (objCurriculo.PessoaFisica.Endereco.Cidade.Estado != null)
                    {
                        objCurriculo.PessoaFisica.Endereco.Cidade.Estado.CompleteObject();
                        rdcc.estado = objCurriculo.PessoaFisica.Endereco.Cidade.Estado.NomeEstado;
                    }
                }
                //Carregar imagem
                rdcc.foto = CarregarFoto(objCurriculo.IdCurriculo, true);

                #endregion
            }



            #endregion

            #region Funções Pretendidas
            rdcc.funcoes = FuncaoPretendida.CarregarNomeDeFuncoesPretendidasPorCurriculo(objCurriculo);
            rdcc.pretensao = objCurriculo.ValorPretensaoSalarial;
            rdcc.idCurriculo = objCurriculo.IdCurriculo;
            #endregion

            #region Escolaridade
            if (objCurriculo.PessoaFisica.Escolaridade != null)
            {
                objCurriculo.PessoaFisica.Escolaridade.CompleteObject();
                rdcc.escolaridade = objCurriculo.PessoaFisica.Escolaridade.DescricaoBNE;
            };

            #endregion

            #region Experiencias Profissionais


            using (IDataReader dr = BNE.BLL.ExperienciaProfissional.ListarExperienciaPorPessoaFisica(objCurriculo.PessoaFisica.IdPessoaFisica))
            {
                rdcc.listExperienciaProfissional = new List<DTO.ResumoExperienciaProfissional>();
                DTO.ResumoExperienciaProfissional resExpProf = null;
                while (dr.Read())
                {
                    resExpProf = new DTO.ResumoExperienciaProfissional();
                    resExpProf.razaoSocial = Convert.ToString(dr["Raz_Social"]);
                    resExpProf.descAreaBNE = Convert.ToString(dr["Des_Area_BNE"]);
                    resExpProf.dtaAdmissao = Convert.ToString(dr["Dta_Admissao"]);
                    resExpProf.dtaDemissao = Convert.ToString(dr["Dta_Demissao"]);
                    resExpProf.descFuncao = Convert.ToString(dr["Des_Funcao"]);
                    resExpProf.descAtividade = Convert.ToString(dr["Des_Atividade"]);
                    if (dr["Vlr_Ultimo_Salario"] != DBNull.Value)
                        resExpProf.vlrUltimoSalario = Convert.ToDecimal(dr["Vlr_Ultimo_Salario"]);
                    if (dr["Vlr_Salario"] != DBNull.Value)
                        resExpProf.vlrSalario = Convert.ToDecimal(dr["Vlr_Salario"]);
                    resExpProf.ordem = Convert.ToInt32(dr["Ordem"]);
                    rdcc.listExperienciaProfissional.Add(resExpProf);
                }
            }

            #endregion

            #region Observacoes
            rdcc.caracteristicasPessoais = string.Format("{0} - {1} - {2}", objCurriculo.PessoaFisica.Raca, pfc.NumeroAltura, pfc.NumeroPeso);
            rdcc.outrosConhecimento = pfc.DescricaoConhecimento;
            rdcc.FlagViagem = pfc.FlagViagem;
            #endregion

            return rdcc;
        }
        #endregion

        #region RetornoAuditoriaEmpresa
        private HttpResponseMessage RetornoAuditoriaEmpresa(Filial objFilial, Curriculo objCurriculo)
        {
            int QuantidadeCurriculosVIPVisualizadosPelaEmpresa = BNE.BLL.CurriculoVisualizacaoHistorico.RecuperarQuantidadeVisualizacaoDadosCompletosCurriculosVIP(objFilial);

            if (objFilial.DataCadastro.DayOfYear == DateTime.Now.DayOfYear && QuantidadeCurriculosVIPVisualizadosPelaEmpresa > 20)
            {
                CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, objCurriculo, false, Helper.RecuperarIP(), "O limite de visualizações diário foi atingido, para mais informações ligue 0800 41 2400.");
                return errorRequestPost(HttpStatusCode.Unauthorized, "O limite de visualizações diário foi atingido, para mais informações ligue 0800 41 2400.");
            }
            else if (objFilial.DataCadastro.DayOfYear < DateTime.Now.DayOfYear && QuantidadeCurriculosVIPVisualizadosPelaEmpresa > 5)
            {
                CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, objCurriculo, false, Helper.RecuperarIP(), "O limite de visualizações diário foi atingido, para mais informações ligue 0800 41 2400.");
                return errorRequestPost(HttpStatusCode.Unauthorized, "O limite de visualizações diário foi atingido, para mais informações ligue 0800 41 2400.");
            }
            else
            {
                CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, objCurriculo, false, Helper.RecuperarIP(), "O limite de visualizações diário foi atingido, para mais informações ligue 0800 41 2400.");
                return errorRequestPost(HttpStatusCode.Unauthorized, "Por motivo de segurança, solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!");
            }
        }
        #endregion

        #region Metodos Da API

        #region PesquisaCVCompleto
        /// <summary>
        /// Retorna o curriculo selecionado pelo usuario a partir do IdCurriculo ou CPF do Cliente
        /// </summary>
        /// <param name="objParamentros"></param>
        /// <returns>Objeto com dados completos do curriculo com ou sem contato</returns>
        public HttpResponseMessage ObterCV([FromBody]DTO.PesquisaCurriculoCompleto objParametros)
        {
            try
            {
                BNE.BLL.UsuarioFilialPerfil objUsuarioFilialPerfil = Login(Convert.ToDecimal(objParametros.CPF), objParametros.DataNascimento);

                if (objUsuarioFilialPerfil != null)
                {
                    if (objUsuarioFilialPerfil.Filial != null)
                    {
                        //Se setado o valor ele o pega,senão seta false
                        bool flgDadosPessoais = objParametros.FlgDadosdeContato.HasValue ? objParametros.FlgDadosdeContato.Value : false;
                        Filial objFilial = Filial.LoadObject(objUsuarioFilialPerfil.Filial.IdFilial);

                        var objCurriculo = new Curriculo(objParametros.IdCurriculo);

                        if (!objCurriculo.CompleteObject())
                        return errorRequestPost(HttpStatusCode.BadRequest, "Currículo inexistente em nossa base de dados!");
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
                                            return Request.CreateResponse(HttpStatusCode.OK,VerDadosCompleto(objCurriculo, objFilial, flgDadosPessoais));
                                        else
                                        {
                                            if (CurriculoVisualizacao.FilialPodeVerDadosCurriculo(objFilial, objCurriculo, autorizacaoPelaWebEstagios))
                                                return Request.CreateResponse(HttpStatusCode.OK,VerDadosCompleto(objCurriculo, objFilial, flgDadosPessoais));
                                            else
                                            {
                                                CurriculoVisualizacaoHistorico.SalvarHistoricoVisualizacao(objFilial, objCurriculo, false, Helper.RecuperarIP(), "Por motivo de segurança, solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!");
                                                return errorRequestPost(HttpStatusCode.Unauthorized, "Por motivo de segurança, solicitamos que entre em contato com o 0800 41 2400 e valide seu acesso!");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (objCurriculo.VIP() && objFilial.EmpresaSemPlanoPodeVisualizarCurriculo(1))
                                            return Request.CreateResponse(HttpStatusCode.OK, VerDadosCompleto(objCurriculo, objFilial, flgDadosPessoais));
                                        else
                                            return RetornoAuditoriaEmpresa(objFilial, objCurriculo);

                                    }
                                }
                            }
                            return Request.CreateResponse(HttpStatusCode.OK, VerDadosCompleto(objCurriculo, objFilial, false));
                        }
                    }
                    else
                    return errorRequestPost(HttpStatusCode.BadRequest, "Somente empresas cadastradas tem acesso a currículos!");
                }
                else
                    return errorRequestPost(HttpStatusCode.NotFound, "Não Autorizado!");
            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        #endregion

        #region PesquisaAvancada
        /// <summary>
        /// Retorna uma lista de currículos baseada nos parâmetros informados.
        /// </summary>
        /// <param name="objPesquisaCurriculo">Objeto com os parâmetros</param>
        /// <returns>Objeto contendo a lista de currículos filtrada, juntamente com os registros de totalização para paginação</returns>
        public HttpResponseMessage PesquisaAvancada([FromBody]DTO.PesquisaCurriculo objParametros)
        {
            try
            {
                BNE.BLL.UsuarioFilialPerfil objUsuarioFilialPerfil = Login(Convert.ToDecimal(objParametros.CPF), objParametros.DataNascimento);

                if (objUsuarioFilialPerfil == null)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "Usuário não encontrado");
                }

                BNE.BLL.PesquisaCurriculo objPesquisaCurriculo = new BNE.BLL.PesquisaCurriculo();
                objPesquisaCurriculo.UsuarioFilialPerfil = objUsuarioFilialPerfil;
                objPesquisaCurriculo.FlagPesquisaAvancada = true;
                objPesquisaCurriculo.DescricaoIP = HttpContext.Current.Request.UserHostAddress;

                #region criando objeto de consulta

                Cidade objCidade;
                if (Cidade.CarregarPorNome(objParametros.Cidade, out objCidade))
                {
                    objPesquisaCurriculo.Cidade = new Cidade(objCidade.IdCidade) { NomeCidade = objCidade.NomeCidade };
                }

                if (!String.IsNullOrEmpty(objParametros.Funcao))
                {
                    var funcao = Funcao.CarregarPorDescricao(objParametros.Funcao);
                    if (funcao != null)
                        objPesquisaCurriculo.Funcao = new Funcao(funcao.IdFuncao) { DescricaoFuncao = funcao.DescricaoFuncao };
                }

                objPesquisaCurriculo.DescricaoPalavraChave = objParametros.PalavraChave;

                if (!String.IsNullOrEmpty(objParametros.Estado))
                    objPesquisaCurriculo.Estado = new Estado(objParametros.Estado);

                if (!String.IsNullOrEmpty(objParametros.Escolaridade))
                {
                    Escolaridade escolaridade;
                    if (Escolaridade.CarregarPorNome(objParametros.Escolaridade, out escolaridade))
                        objPesquisaCurriculo.Escolaridade = escolaridade;
                }

                if (!string.IsNullOrEmpty(objParametros.Sexo))
                {
                    Sexo sexo = null;
                    if (objParametros.Sexo.ToLower().Equals("masculino"))
                    {
                        sexo = new Sexo((int)BNE.BLL.Enumeradores.Sexo.Masculino)
                        {
                            SiglaSexo = 'M'
                        };
                    }
                    if (objParametros.Sexo.ToLower().Equals("feminino"))
                    {
                        sexo = new Sexo((int)BNE.BLL.Enumeradores.Sexo.Masculino)
                        {
                            SiglaSexo = 'F'
                        };
                    }
                    if (sexo != null)
                        objPesquisaCurriculo.Sexo = sexo;
                }

                if (objParametros.IdadeMinima.HasValue)
                    objPesquisaCurriculo.DataIdadeMin = DateTime.Today.AddYears(-(objParametros.IdadeMinima.Value));

                if (objParametros.IdadeMaxima.HasValue)
                    objPesquisaCurriculo.DataIdadeMax = DateTime.Today.AddYears(-(objParametros.IdadeMaxima.Value));

                objPesquisaCurriculo.NumeroSalarioMin = objParametros.SalarioMinimo;
                objPesquisaCurriculo.NumeroSalarioMax = objParametros.SalarioMaximo;
                objPesquisaCurriculo.QuantidadeExperiencia = objParametros.QuantidadeExperiencia;

                if (!string.IsNullOrEmpty(objParametros.CodCPFNome))
                    objPesquisaCurriculo.DescricaoCodCPFNome = objParametros.CodCPFNome.Replace(";", ",");
                else
                    objPesquisaCurriculo.DescricaoCodCPFNome = objParametros.CodCPFNome;

                if (!string.IsNullOrEmpty(objParametros.EstadoCivil))
                {
                    //@todo Carregar estado civil por descricao
                    //objPesquisaCurriculo.EstadoCivil = EstadoCivil.LoadObject(Convert.ToInt32(rcbEstadoCivil.SelectedValue));
                }

                objPesquisaCurriculo.DescricaoBairro = objParametros.Bairro;
                objPesquisaCurriculo.DescricaoLogradouro = objParametros.Logradouro;
                objPesquisaCurriculo.NumeroCEPMin = objParametros.CEPMinimo;
                objPesquisaCurriculo.NumeroCEPMax = objParametros.CEPMaximo;

                //Fonte e curso
                //@todo Table para pesquisa
                if (!String.IsNullOrEmpty(objParametros.InstituicaoTecnicoGraduacao))
                {
                    Fonte objFonte;
                    if (Fonte.BuscarPorDescricao(objParametros.InstituicaoTecnicoGraduacao, out objFonte))
                        objPesquisaCurriculo.FonteTecnicoGraduacao = objFonte;
                }

                //@todo Table para pesquisa
                if (!String.IsNullOrEmpty(objParametros.CursoTecnicoGraduacao))
                {
                    Curso objCurso;
                    if (Curso.BuscaCurso(objParametros.CursoTecnicoGraduacao, out objCurso))
                        objPesquisaCurriculo.CursoTecnicoGraduacao = objCurso;
                    else
                    {
                        objPesquisaCurriculo.DescricaoCursoTecnicoGraduacao = objParametros.CursoTecnicoGraduacao;
                    }
                }

                if (!String.IsNullOrEmpty(objParametros.InstituicaoOutrosCursos))
                {
                    Fonte objFonte;
                    if (Fonte.BuscarPorDescricao(objParametros.InstituicaoOutrosCursos, out objFonte))
                        objPesquisaCurriculo.FonteOutrosCursos = objFonte;
                }

                if (!String.IsNullOrEmpty(objParametros.CursoOutrosCursos))
                {
                    Curso objCurso;
                    if (Curso.BuscaCurso(objParametros.CursoOutrosCursos, out objCurso))
                        objPesquisaCurriculo.CursoOutrosCursos = objCurso;
                    else
                    {
                        objPesquisaCurriculo.DescricaoCursoOutrosCursos = objParametros.CursoOutrosCursos;
                    }
                }

                objPesquisaCurriculo.RazaoSocial = objParametros.EmpresaQueJaTrabalhou;

                //@todo table
                if (!String.IsNullOrEmpty(objParametros.AreaEmpresaQueJaTrabalhou))
                {
                    AreaBNE objAreaBNE;
                    if (AreaBNE.CarregarPorDescricao(objParametros.AreaEmpresaQueJaTrabalhou, out objAreaBNE))
                    {
                        objPesquisaCurriculo.AreaBNE = objAreaBNE;
                    }
                }

                if (!String.IsNullOrEmpty(objParametros.CategoriaHabilitacao))
                {
                    CategoriaHabilitacao objCategoriaHabilitacao;
                    if (CategoriaHabilitacao.CarregarPorDescricao(objParametros.CategoriaHabilitacao, out objCategoriaHabilitacao))
                    {
                        objPesquisaCurriculo.CategoriaHabilitacao = objCategoriaHabilitacao;
                    }
                }

                if (!String.IsNullOrEmpty(objParametros.TipoVeiculo))
                {
                    TipoVeiculo objTipoVeiculo = TipoVeiculo.CarregarPorDescricao(objParametros.TipoVeiculo);
                    if (objTipoVeiculo != null)
                    {
                        objPesquisaCurriculo.TipoVeiculo = objTipoVeiculo;
                    }
                }

                if (!String.IsNullOrEmpty(objParametros.Deficiencia))
                {
                    Deficiencia objDeficiencia = Deficiencia.CarregarPorDescricao(objParametros.Deficiencia);
                    if (objDeficiencia != null)
                    {
                        objPesquisaCurriculo.Deficiencia = objDeficiencia;
                    }
                }

                objPesquisaCurriculo.NumeroDDDTelefone = objParametros.DDDTelefone;
                objPesquisaCurriculo.NumeroTelefone = objParametros.NumeroTelefone;
                objPesquisaCurriculo.EmailPessoa = objParametros.Email;

                if (!String.IsNullOrEmpty(objParametros.Raca))
                {
                    Raca objRaca = Raca.CarregarPorDescricao(objParametros.Raca);
                    objPesquisaCurriculo.Raca = objRaca;
                }

                if (objParametros.PossuiFilhos.HasValue)
                    objPesquisaCurriculo.FlagFilhos = objParametros.PossuiFilhos.Value;

                var listPesquisaCurriculoIdioma = new List<PesquisaCurriculoIdioma>();
                var listPesquisaCurriculoDisponibilidade = new List<PesquisaCurriculoDisponibilidade>();

                #endregion criando objeto de consulta

                objPesquisaCurriculo.Salvar(listPesquisaCurriculoIdioma, listPesquisaCurriculoDisponibilidade, objParametros.QueroContratarEstagiarios);

                DTO.ResultadoPesquisaCurriculo resultadoPesquisa = new DTO.ResultadoPesquisaCurriculo();
                int numeroRegistros;
                decimal mediaSalarial;
                int PageSize = Convert.ToInt32(Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.QuantidadeItensPorPaginaPesquisaCurriculo));
                int? filial = objUsuarioFilialPerfil.Filial == null ? null : (int?)objUsuarioFilialPerfil.Filial.IdFilial;
                DataTable resultado = BNE.BLL.PesquisaCurriculo.BuscaCurriculo(PageSize, objParametros.Pagina, 1, filial, null, objPesquisaCurriculo, out numeroRegistros, out mediaSalarial);

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
