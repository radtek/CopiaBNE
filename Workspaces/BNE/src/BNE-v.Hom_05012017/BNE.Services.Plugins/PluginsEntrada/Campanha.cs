using BNE.BLL;
using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using BNE.BLL.Custom;
using BNE.BLL.Common;

namespace BNE.Services.Plugins.PluginsEntrada
{
    [Export(typeof(IInputPlugin))]
    [ExportMetadata("Type", "Campanha")]
    public class Campanha : InputPlugin
    {

        #region DoExecuteTask
        protected override IPluginResult DoExecuteTask(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            var parametroIdCampanha = objParametros["idCampanha"];

            if (parametroIdCampanha == null || !parametroIdCampanha.ValorInt.HasValue)
                throw new ArgumentNullException("parametroIdCampanha");

            var idCampanha = parametroIdCampanha.ValorInt.Value;

            if (idCampanha <= 0)
                throw new ArgumentOutOfRangeException("parametroIdCampanha");

            var listaParametros = new List<BLL.AsyncServices.Enumeradores.Parametro>
            {
                BLL.AsyncServices.Enumeradores.Parametro.CampanhaRecrutamentoLotePadraoParaEnvio,
                BLL.AsyncServices.Enumeradores.Parametro.CampanhaRecrutamentoTetoParaEnvio,
                BLL.AsyncServices.Enumeradores.Parametro.CampanhaRecrutamentoMultiplicadorLoteInicial,
                BLL.AsyncServices.Enumeradores.Parametro.CampanhaPesquisaCurriculoLotePadraoParaEnvio,
                BLL.AsyncServices.Enumeradores.Parametro.TentativasRequisicaoBuscaCVCampanha,
                BLL.AsyncServices.Enumeradores.Parametro.EmailDestinoAlertaProblemaCampanha,
                BLL.AsyncServices.Enumeradores.Parametro.EmailRemetenteVagaPerfil,
                BLL.AsyncServices.Enumeradores.Parametro.IdfUsuarioEnvioCampanha,
                BLL.AsyncServices.Enumeradores.Parametro.IdfUsuarioEnvioCampanhaPesquisaCV
            };
            var dicionarioParametros = BLL.AsyncServices.Parametro.ListarParametros(listaParametros);

            try
            {
                var objCampanha = CampanhaRecrutamento.LoadObject(idCampanha);

                if (objCampanha.EnvioCampanhaRecrutamento())
                    return DeRecrutamento(objCampanha, dicionarioParametros);

                return DePesquisaCurriculo(objCampanha, dicionarioParametros);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro na execução da fila de campanha.");
                Core.LogError(ex);
                throw;
            }
        }
        #endregion

        #region Metodos

        #region Recrutamento
        private MensagemPlugin DeRecrutamento(CampanhaRecrutamento objCampanha, Dictionary<BLL.AsyncServices.Enumeradores.Parametro, string> dicionarioParametros)
        {
            var listaEmail = new List<MensagemPlugin.MensagemEmail>();
            var loteSMS = new MensagemPlugin.MensagemSMSTanque();

            int campanhaRecrutamentoLotePadraoParaEnvio = Convert.ToInt16(dicionarioParametros[BLL.AsyncServices.Enumeradores.Parametro.CampanhaRecrutamentoLotePadraoParaEnvio]);
            int campanhaRecrutamentoTetoParaEnvio = Convert.ToInt16(dicionarioParametros[BLL.AsyncServices.Enumeradores.Parametro.CampanhaRecrutamentoTetoParaEnvio]);
            int campanhaRecrutamentoMultiplicadorLoteInicial = Convert.ToInt16(dicionarioParametros[BLL.AsyncServices.Enumeradores.Parametro.CampanhaRecrutamentoMultiplicadorLoteInicial]);
            int tentativasRequisicaoBuscaCVCampanha = Convert.ToInt16(dicionarioParametros[BLL.AsyncServices.Enumeradores.Parametro.TentativasRequisicaoBuscaCVCampanha]);
            string emailDestinoAlertaProblemaCampanha = dicionarioParametros[BLL.AsyncServices.Enumeradores.Parametro.EmailDestinoAlertaProblemaCampanha];
            string emailRemetente = dicionarioParametros[BLL.AsyncServices.Enumeradores.Parametro.EmailRemetenteVagaPerfil];
            string idUsuarioEnvioCampanha = dicionarioParametros[BLL.AsyncServices.Enumeradores.Parametro.IdfUsuarioEnvioCampanha];

            objCampanha.Vaga.CompleteObject();
            objCampanha.Vaga.Cidade.CompleteObject();
            objCampanha.Vaga.Filial.CompleteObject();
            objCampanha.Vaga.UsuarioFilialPerfil.CompleteObject();
            objCampanha.TipoRetornoCampanhaRecrutamento.CompleteObject();
            loteSMS.IdUsuarioTanque = idUsuarioEnvioCampanha;
            loteSMS.IdUsuarioOrigem = objCampanha.Vaga.UsuarioFilialPerfil.IdUsuarioFilialPerfil.ToString();
            loteSMS.idVaga = objCampanha.Vaga.IdVaga;
            loteSMS.desFuncao = objCampanha.Vaga.DescricaoFuncao;
            loteSMS.desCidade = objCampanha.Vaga.Cidade.NomeCidade + "/" + objCampanha.Vaga.Cidade.Estado.SiglaEstado;
            loteSMS.idCampanha = (int)BLL.Enumeradores.CampanhaTanque.CampanhaRecrutamento;

            var templateSMSCampanha = new CampanhaTanque().GetTextoCampanha(BLL.Enumeradores.CampanhaTanque.CampanhaRecrutamento);

            int quantidadeCurriculosEnviados = objCampanha.QuantidadeCurriculosEnviados();

            #region Regra para finalização da campanha
            if (quantidadeCurriculosEnviados > campanhaRecrutamentoTetoParaEnvio)
            {
                UsuarioFilial objSUsuarioFilial;
                UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objCampanha.Vaga.UsuarioFilialPerfil.IdUsuarioFilialPerfil, out objSUsuarioFilial);

                int quantidadeCandidaturas = objCampanha.Vaga.QuantidadeCandidaturas();
                if (quantidadeCandidaturas < objCampanha.QuantidadeRetorno)
                {
                    objCampanha.MotivoCampanhaFinalizada = new MotivoCampanhaFinalizada((int)BLL.Enumeradores.MotivoCampanhaFinalizada.ExcedeuTetoEnvio);
                    if (quantidadeCandidaturas >= 5)
                    {
                        string linkCurriculo = Curriculo.RetornaLinkVisualizacaoCurriculo(objCampanha.Vaga.ListaIdsCurriculosCandidatados());

                        string consultor = "EquipeBNE";
                        var objVendedor = objCampanha.Vaga.Filial.Vendedor();
                        if (objVendedor != null)
                            consultor = objVendedor.NomeVendedor;

                        var parametros = new
                        {
                            LinkCurriculos = linkCurriculo,
                            Usuario = objCampanha.Vaga.UsuarioFilialPerfil.PessoaFisica.PrimeiroNome,
                            QuantidadeCandidatura = quantidadeCandidaturas,
                            Consultor = consultor
                        };
                        string assunto;
                        string mensagem = parametros.ToString(CartaEmail.RetornarConteudoBNE(BLL.Enumeradores.CartaEmail.CampanhaInsucessoMais5Inscritos, out assunto));

                        listaEmail.Add(MontarEmail(assunto, mensagem, emailRemetente, objSUsuarioFilial.EmailComercial));
                    }
                    else
                    {
                        string consultor = "EquipeBNE";
                        var objVendedor = objCampanha.Vaga.Filial.Vendedor();
                        if (objVendedor != null)
                            consultor = objVendedor.NomeVendedor;

                        var parametros = new
                        {
                            Usuario = objCampanha.Vaga.UsuarioFilialPerfil.PessoaFisica.PrimeiroNome,
                            QuantidadeCandidatura = quantidadeCandidaturas,
                            Consultor = consultor
                        };
                        string assunto;
                        string mensagem = parametros.ToString(CartaEmail.RetornarConteudoBNE(BLL.Enumeradores.CartaEmail.CampanhaInsucessoMenos5Inscritos, out assunto));

                        listaEmail.Add(MontarEmail(assunto, mensagem, emailRemetente, objSUsuarioFilial.EmailComercial));

                    }

                    var parametrosAtingiuLimite = new
                    {
                        DataCadastro = objCampanha.DataCadastro.ToShortDateString(),
                        RazaoSocial = objCampanha.Vaga.Filial.ToString(),
                        Usuario = string.Format("{0} - {1}", objCampanha.Vaga.UsuarioFilialPerfil.PessoaFisica.PrimeiroNome, Helper.FormatarTelefone(objSUsuarioFilial.NumeroDDDComercial, objSUsuarioFilial.NumeroComercial)),
                        TipoRetorno = objCampanha.TipoRetornoCampanhaRecrutamento.DescricaoTipoRetornoCampanhaRecrutamento,
                        Funcao = objCampanha.Vaga.DescricaoFuncao,
                        Cidade = Helper.FormatarCidade(objCampanha.Vaga.Cidade.NomeCidade, objCampanha.Vaga.Cidade.Estado.SiglaEstado),
                        Salario = objCampanha.Vaga.ValorSalarioPara,
                        CodigoVaga = objCampanha.Vaga.CodigoVaga,
                        QuantidadeSMS = objCampanha.QuantidadeSMSEnviados(),
                        QuantidadeEmail = objCampanha.QuantidadeEmailEnviados(),
                        QuantidadeCandidatura = quantidadeCandidaturas,
                        QuantidadeRequisitada = objCampanha.QuantidadeRetorno
                    };
                    string assuntoAtingiuLimite;
                    string mensagemAtingiuLimite = parametrosAtingiuLimite.ToString(CartaEmail.RetornarConteudoBNE(BLL.Enumeradores.CartaEmail.CampanhaAtintiuTetoDisparo, out assuntoAtingiuLimite));

                    listaEmail.Add(MontarEmail(assuntoAtingiuLimite, mensagemAtingiuLimite, emailRemetente, emailDestinoAlertaProblemaCampanha));
                }
                else
                {
                    objCampanha.MotivoCampanhaFinalizada = new MotivoCampanhaFinalizada((int)BLL.Enumeradores.MotivoCampanhaFinalizada.QuantidadeCandidaturasAtingida);

                    string linkCurriculo = Curriculo.RetornaLinkVisualizacaoCurriculo(objCampanha.Vaga.ListaIdsCurriculosCandidatados());

                    string consultor = "EquipeBNE";
                    var objVendedor = objCampanha.Vaga.Filial.Vendedor();
                    if (objVendedor != null)
                        consultor = objVendedor.NomeVendedor;

                    var parametros = new
                    {
                        Funcao = objCampanha.Vaga.DescricaoFuncao,
                        Cidade = Helper.FormatarCidade(objCampanha.Vaga.Cidade.NomeCidade, objCampanha.Vaga.Cidade.Estado.SiglaEstado),
                        LinkCurriculos = linkCurriculo,
                        Usuario = objCampanha.Vaga.UsuarioFilialPerfil.PessoaFisica.PrimeiroNome,
                        QuantidadeCandidatura = quantidadeCandidaturas,
                        Consultor = consultor
                    };
                    string assunto;
                    string mensagem = parametros.ToString(CartaEmail.RetornarConteudoBNE(BLL.Enumeradores.CartaEmail.CampanhaSucessoAtingiuQuantidadeRetorno, out assunto));

                    listaEmail.Add(MontarEmail(parametros.ToString(assunto), mensagem, emailRemetente, objCampanha.Vaga.UsuarioFilialPerfil.Email()));

                    string mensagemSMS = parametros.ToString(CartaSMS.RecuperaValorConteudo(BLL.Enumeradores.CartaSMS.CampanhaSucessoParaCliente));
                    loteSMS.mensagens.Add(MontarObjetoSMS(0, objCampanha.Vaga.UsuarioFilialPerfil.PessoaFisica.PrimeiroNome, objCampanha.Vaga.UsuarioFilialPerfil.PessoaFisica.NumeroDDDCelular, objCampanha.Vaga.UsuarioFilialPerfil.PessoaFisica.NumeroCelular, mensagemSMS));
                }
                objCampanha.Save();
            }
            #endregion

            #region Se a campanha não está finalizada
            if (objCampanha.MotivoCampanhaFinalizada == null)
            {
                int lote = campanhaRecrutamentoLotePadraoParaEnvio;
                if (quantidadeCurriculosEnviados == 0) //Se não foi enviado nenhum o lote padrão será manda Multiplicador padrão x Quantidade de curriculo para retorno;
                    lote = campanhaRecrutamentoMultiplicadorLoteInicial * objCampanha.QuantidadeRetorno;

                var curriculos = RecuperarLoteCurriculos(objCampanha, lote, tentativasRequisicaoBuscaCVCampanha);

                if (curriculos.Count == 0 || curriculos.Count < lote)
                {
                    int quantidadeCandidaturas = objCampanha.Vaga.QuantidadeCandidaturas();
                    var parametros = new
                    {
                        DataCadastro = objCampanha.DataCadastro.ToString("dd/MM/yyyy"),
                        RazaoSocial = objCampanha.Vaga.Filial.ToString(),
                        Usuario = objCampanha.Vaga.UsuarioFilialPerfil.PessoaFisica.PrimeiroNome,
                        TipoRetorno = objCampanha.TipoRetornoCampanhaRecrutamento.DescricaoTipoRetornoCampanhaRecrutamento,
                        Funcao = objCampanha.Vaga.DescricaoFuncao,
                        Cidade = Helper.FormatarCidade(objCampanha.Vaga.Cidade.NomeCidade, objCampanha.Vaga.Cidade.Estado.SiglaEstado),
                        Salario = objCampanha.Vaga.ValorSalarioPara.HasValue ? objCampanha.Vaga.ValorSalarioPara.Value.ToString("N") : string.Empty,
                        CodigoVaga = objCampanha.Vaga.CodigoVaga,
                        QuantidadeSMS = objCampanha.QuantidadeSMSEnviados(),
                        QuantidadeEmail = objCampanha.QuantidadeEmailEnviados(),
                        QuantidadeCandidatura = quantidadeCandidaturas,
                        QuantidadeRequisitada = objCampanha.QuantidadeRetorno
                    };
                    string assunto;
                    string mensagem = parametros.ToString(CartaEmail.RetornarConteudoBNE(BLL.Enumeradores.CartaEmail.CampanhaSemBase, out assunto));

                    listaEmail.Add(MontarEmail(assunto, mensagem, emailRemetente, emailDestinoAlertaProblemaCampanha));

                    //Finaliza pois não tem a quantidade necessária de currículo.
                    objCampanha.MotivoCampanhaFinalizada = new MotivoCampanhaFinalizada((int)BLL.Enumeradores.MotivoCampanhaFinalizada.QuantidadeInsuficienteCurriculo);
                    objCampanha.Save();
                }

                DataTable dt = null;

                foreach (var objCurriculo in curriculos)
                {
                    String assunto;

                    var textoSMS = Cartas.SMS.MensagemSMSCvPerfilVaga(objCampanha.Vaga.DescricaoFuncao, objCampanha.Vaga.IdVaga, templateSMSCampanha.mensagem);

                    string listaVagas, salaVip, vip, cadastroCurriculo, pesquisaVagas, loginCandidato, quemMeViu, cadastroExperiencias;
                    Curriculo.RetornarHashLogarCurriculo(objCurriculo.NumeroCPF, objCurriculo.DataNascimento, out listaVagas, out salaVip, out vip, out quemMeViu, out cadastroCurriculo, out pesquisaVagas, out loginCandidato, out cadastroExperiencias);

                    string linkVaga = objCampanha.Vaga.URL();
                    var email = Cartas.Email.MensagemEmailCvPerfilVaga(objCampanha.Vaga.DescricaoFuncao, linkVaga, objCampanha.Vaga.IdVaga, objCampanha.Vaga.QuantidadeVaga,objCampanha.Vaga.ValorSalarioDe, objCampanha.Vaga.ValorSalarioPara, objCampanha.Vaga.Cidade.NomeCidade, objCampanha.Vaga.Cidade.Estado.SiglaEstado, objCampanha.Vaga.DescricaoAtribuicoes, objCampanha.Vaga.DescricaoRequisito, objCurriculo.NomeCompleto, listaVagas, salaVip, vip, quemMeViu, cadastroCurriculo, pesquisaVagas, loginCandidato, out assunto);
                    var objCampanhaEmail = new CampanhaRecrutamentoCurriculo
                    {
                        Curriculo = new Curriculo(objCurriculo.IdCurriculo),
                        TipoMensagemCS = new TipoMensagemCS((int)BLL.Enumeradores.TipoMensagem.Email),
                        CampanhaRecrutamento = objCampanha
                    };
                    objCampanhaEmail.AddBulkTable(ref dt);

                    listaEmail.Add(MontarEmail(assunto, email, emailRemetente, objCurriculo.Email));

                    var objCampanhaSMS = new CampanhaRecrutamentoCurriculo
                    {
                        Curriculo = new Curriculo(objCurriculo.IdCurriculo),
                        TipoMensagemCS = new TipoMensagemCS((int)BLL.Enumeradores.TipoMensagem.SMS),
                        CampanhaRecrutamento = objCampanha
                    };
                    objCampanhaSMS.AddBulkTable(ref dt);

                    loteSMS.mensagens.Add(MontarObjetoSMS(objCurriculo.IdCurriculo, objCurriculo.PrimeiroNome, objCurriculo.NumeroDDDCelular, objCurriculo.NumeroCelular, textoSMS, templateSMSCampanha.id));
                }

                CampanhaRecrutamentoCurriculo.SaveBulkTable(dt, null);
            }
            #endregion

            if (loteSMS.mensagens.Count > 0 || listaEmail.Count > 0)
            {
                var retorno = new MensagemPlugin(this, false)
                {
                    ListaSMSTanque = new List<MensagemPlugin.MensagemSMSTanque> { loteSMS },
                    ListaEmail = listaEmail
                };
                return retorno;
            }

            return new MensagemPlugin(this, false);
        }
        #endregion

        #region PesquisaCurriculo
        private MensagemPlugin DePesquisaCurriculo(CampanhaRecrutamento objCampanha, Dictionary<BLL.AsyncServices.Enumeradores.Parametro, string> dicionarioParametros)
        {
            var lotesSMS = new List<MensagemPlugin.MensagemSMSTanque>();

            int campanhaPesquisaCurriculoLotePadraoParaEnvio = Convert.ToInt16(dicionarioParametros[BLL.AsyncServices.Enumeradores.Parametro.CampanhaPesquisaCurriculoLotePadraoParaEnvio]);
            int tentativasRequisicaoBuscaCVCampanha = Convert.ToInt16(dicionarioParametros[BLL.AsyncServices.Enumeradores.Parametro.TentativasRequisicaoBuscaCVCampanha]);
            string idUsuarioEnvioCampanhaPesquisaCV = dicionarioParametros[BLL.AsyncServices.Enumeradores.Parametro.IdfUsuarioEnvioCampanhaPesquisaCV];

            objCampanha.PesquisaCurriculo.CompleteObject();
            objCampanha.PesquisaCurriculo.Cidade.CompleteObject();

            var funcoesPesquisa = PesquisaCurriculoFuncao.ListarIdentificadoresFuncaoPorPesquisa(objCampanha.PesquisaCurriculo).Select(Funcao.LoadObject).ToList();

            foreach (var objFuncao in funcoesPesquisa)
            {
                var loteSMS = new MensagemPlugin.MensagemSMSTanque
                {
                    IdUsuarioTanque = idUsuarioEnvioCampanhaPesquisaCV,
                    IdUsuarioOrigem = objCampanha.PesquisaCurriculo.UsuarioFilialPerfil.IdUsuarioFilialPerfil.ToString(),
                    desCidade = objCampanha.PesquisaCurriculo.Cidade.NomeCidade + "/" + objCampanha.PesquisaCurriculo.Cidade.Estado.SiglaEstado,
                    desFuncao = objFuncao.DescricaoFuncao,
                    idCampanha = (int)BLL.Enumeradores.CampanhaTanque.PesquisaCV
                };
                var templateSMSCampanha = new CampanhaTanque().GetTextoCampanha(BLL.Enumeradores.CampanhaTanque.PesquisaCV);

                #region Se a campanha não está finalizada
                if (objCampanha.MotivoCampanhaFinalizada == null)
                {
                    int lote = campanhaPesquisaCurriculoLotePadraoParaEnvio;

                    var curriculos = RecuperarLoteCurriculos(objCampanha, lote, tentativasRequisicaoBuscaCVCampanha, objFuncao);
                    DataTable dt = null;

                    foreach (var objCurriculo in curriculos)
                    {
                        var textoSMS = Cartas.SMS.MensagemPesquisaCurriculoEmpresa(objFuncao.DescricaoFuncao, objCampanha.PesquisaCurriculo.Cidade.NomeCidade + "/" + objCampanha.PesquisaCurriculo.Cidade.Estado.SiglaEstado, objCurriculo.PrimeiroNome, templateSMSCampanha.mensagem);

                        var objCampanhaSMS = new CampanhaRecrutamentoCurriculo
                        {
                            Curriculo = new BLL.Curriculo(objCurriculo.IdCurriculo),
                            TipoMensagemCS = new TipoMensagemCS((int)BLL.Enumeradores.TipoMensagem.SMS),
                            CampanhaRecrutamento = objCampanha
                        };
                        objCampanhaSMS.AddBulkTable(ref dt);

                        loteSMS.mensagens.Add(MontarObjetoSMS(objCurriculo.IdCurriculo, objCurriculo.PrimeiroNome, objCurriculo.NumeroDDDCelular, objCurriculo.NumeroCelular, textoSMS, templateSMSCampanha.id));
                    }

                    CampanhaRecrutamentoCurriculo.SaveBulkTable(dt, null);
                }
                #endregion

                lotesSMS.Add(loteSMS);
            }
            if (lotesSMS.Count > 0)
            {
                var retorno = new MensagemPlugin(this, false)
                {
                    ListaSMSTanque = lotesSMS.Where(x => x.mensagens.Count > 0).ToList()
                };
                return retorno;
            }

            return new MensagemPlugin(this, false);
        }
        #endregion

        #region RecuperarLoteCurriculos

        /// <summary>
        /// Recupera os currículos necessários para enviar a campanha
        /// </summary>
        /// <param name="objCampanhaRecrutamento"></param>
        /// <param name="quantidadeCurriculos"></param>
        /// <param name="objFuncao">Usado no fluxo de campanha de pesquisa</param>
        /// <returns></returns>
        public List<BLL.DTO.Curriculo> RecuperarLoteCurriculos(CampanhaRecrutamento objCampanhaRecrutamento, int quantidadeCurriculos, int retries, Funcao objFuncao = null)
        {
            try
            {
                var pagina = 0;
                List<int> listaCurriculosEnviado;

                if (objCampanhaRecrutamento.EnvioCampanhaRecrutamento())
                    listaCurriculosEnviado = CampanhaRecrutamentoCurriculo.ListaIdsCurriculosEnviados(objCampanhaRecrutamento);
                else
                    listaCurriculosEnviado = CampanhaRecrutamentoCurriculo.ListaIdsCurriculosEnviadosCampanhaPesquisaCV(objCampanhaRecrutamento);

                var urlBuscaCVsCampanha = PesquisaCurriculo.MontaURLSolrBuscaCVsCampanha(objCampanhaRecrutamento, objFuncao);

                var lista = new List<BLL.DTO.Curriculo>();
                int idFilial;
                if (objCampanhaRecrutamento.PesquisaCurriculo != null)
                {
                    objCampanhaRecrutamento.PesquisaCurriculo.UsuarioFilialPerfil.CompleteObject();
                    idFilial = objCampanhaRecrutamento.PesquisaCurriculo.UsuarioFilialPerfil.Filial.IdFilial;
                }
                else
                    idFilial = objCampanhaRecrutamento.Vaga.Filial.IdFilial;
                        
                    
                while (lista.Count < quantidadeCurriculos)
                {
                    var cvs = PesquisaCurriculo.BuscaCurriculosCampanha(urlBuscaCVsCampanha, pagina * 50, 50);

                    if (cvs.Count > 0)
                    {
                        cvs = cvs.Where(
                                x => !String.IsNullOrEmpty(x.NumeroDDDCelular)
                                  && !String.IsNullOrEmpty(x.NumeroCelular)
                             ).ToList();
                        var aux = cvs;
                        foreach(var cv in aux)
                        {
                            if(BLL.CurriculoNaoVisivelFilial.CurriculoVisivelParaEmpresa(cv.IdCurriculo, idFilial))
                                cvs = cvs.Where(x => (x.IdCurriculo != cv.IdCurriculo)).ToList();
                        }
                        lista.AddRange(cvs.Except(cvs.Where(c => listaCurriculosEnviado.Contains(c.IdCurriculo))));
                    }

                    pagina++;

                    if (pagina >= retries)
                        break;
                }

                return lista.Take(quantidadeCurriculos).ToList();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro ao recuperar lote de CVs");
                throw;
            }
        }
        #endregion

        #region MontarSMS
        private MensagemPlugin.MensagemSMS MontarObjetoSMS(int idCurriculo, string nomePessoa, string numeroDDD, string numeroCelular, string descricao, int? IdMensagemCampanha = null)
        {
            try
            {
                return new MensagemPlugin.MensagemSMS
                {
                    IdCurriculo = idCurriculo,
                    NomePessoa = nomePessoa,
                    DDDCelular = numeroDDD,
                    NumeroCelular = numeroCelular,
                    Descricao = descricao,
                    idMensagemCampanha = IdMensagemCampanha
                };
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro ao montar mensagens SMS.");
                throw;
            }
        }
        #endregion

        #region MontarEmail
        private MensagemPlugin.MensagemEmail MontarEmail(string descricaoAssunto, string descricaoMensagem, string emailRemetente, string emailDestinatario)
        {
            return new MensagemPlugin.MensagemEmail
            {
                Assunto = descricaoAssunto,
                Descricao = descricaoMensagem,
                From = emailRemetente,
                To = emailDestinatario
            };
        }
        #endregion

        #endregion

    }

}
