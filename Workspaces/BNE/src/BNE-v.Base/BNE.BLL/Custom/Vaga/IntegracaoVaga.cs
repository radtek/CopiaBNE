using BNE.BLL.AsyncServices;
using BNE.Services.Base.ProcessosAssincronos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using BNE.BLL;
using Parametro = BNE.BLL.Parametro;
using TipoAtividade = BNE.BLL.AsyncServices.Enumeradores.TipoAtividade;
using System.Text.RegularExpressions;


namespace BNE.BLL.Custom.Vaga
{
    public class IntegracaoVaga
    {
        #region Globais

        List<SubstituicaoIntegracao> _lstSubstituicoes;

        List<VagaDisponibilidade> _lstDisponibilidadesNovas;
        List<VagaDisponibilidade> _lstDisponibilidadesExcluidas;
        List<VagaTipoVinculo> _lstTipoVinculoNovos;
        List<VagaTipoVinculo> _lstTipoVinculoExcluidos;

        VagaIntegracao _VagaIntegracaoBD;
        VagaIntegracao _VagaIntegracao;
        Integrador _Integrador;
        Origem _Origem;

        string _MensagemSucessoImportacao;
        string _MensagemErroImportacao;
        string _UrlIntegracao;
        string _DescErroPublicacaoAutomatica;

        bool _FlagRecebeCadaCV;
        bool _FlagRecebeTodosCV;
        bool _FlagInicializaPublicadorDeVagas;
        bool _GravaHistorico;

        #endregion

        #region Metodos Publicos

        #region ProcessaVaga
        /// <summary>
        /// Recebe a vaga importada e retorna um novo objeto de vaga, caso tenha sido inserida ou alterada
        /// </summary>
        /// <param name="objVagaImportacao"></param>
        /// <param name="objOrigemImportacao"></param>
        /// <returns></returns>
        public bool ProcessaVaga(BLL.VagaIntegracao objVagaImportacao, Origem origem, Integrador objIntegrador)
        {
            try
            {
                _Integrador = objIntegrador;
                _VagaIntegracao = objVagaImportacao;
                _Origem = origem;

                InicializaVariaveisGlobais();

                return Processa();
            }
            catch (Exception ex)
            {
                GravaLogErro(ex, "ProcessaVaga()");
                throw;
            }
            finally
            {
                LimpaVariaveisGlobais();
            }
        }
        #endregion ProcessaVaga

        #region ConverteVagaToVagaIntegracao
        public BLL.VagaIntegracao ConverteVagaToVagaIntegracao(Integrador objIntegrador, BLL.DTO.wsIntegracao.InVaga vaga)
        {

            //Converte o objeto de vaga DTO (vindo do SINE) e transforma em objIntegrador. Preencher apenas a descrição dos objetos para que na Qeue os obj sejam preenchidos

            try
            {
                _lstSubstituicoes = SubstituicaoIntegracao.ListarSubstituicoesDeIntegrador(objIntegrador);

                VagaIntegracao objVagaIntegracao = new VagaIntegracao();
                objVagaIntegracao.Vaga = new BLL.Vaga();


                objVagaIntegracao.CodigoVagaIntegrador = vaga.CodigoVaga;
                objVagaIntegracao.Vaga.CodigoVaga = vaga.CodigoVaga;

                string valor = String.Empty;

                #region Trata Funcao
                objVagaIntegracao.FuncaoImportada = vaga.DescricaoFuncao;
                #endregion Trata Funcao

                #region Trata Cidade
                objVagaIntegracao.CidadeImportada = vaga.DescricaoCidade;
                #endregion Trata Cidade

                #region Trata Escolaridade
                objVagaIntegracao.EscolaridadeImportada = AplicarSubstituicoes(vaga.DescricaoEscolaridade, BLL.Enumeradores.CampoIntegrador.Escolaridade);
                #endregion Trata Escolaridade

                #region Trata Salario
                objVagaIntegracao.Vaga.ValorSalarioDe = Convert.ToDecimal(vaga.ValorSalarioInicial);
                #endregion Trata Salario

                #region Trata Beneficios
                valor = vaga.DescricaoBeneficio;
                valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.Benefícios);
                objVagaIntegracao.Vaga.DescricaoBeneficio = valor;
                #endregion Trata Beneficios

                #region Trata Idade
                objVagaIntegracao.Vaga.NumeroIdadeMinima = Convert.ToInt16(vaga.NumeroIdadeMinima);
                objVagaIntegracao.Vaga.NumeroIdadeMaxima = Convert.ToInt16(vaga.NumeroIdadeMaxima);
                #endregion Trata Beneficios

                #region Trata Qtde Vagas
                if (vaga.QuantidadeVaga > 0)
                {
                    objVagaIntegracao.Vaga.QuantidadeVaga = short.Parse(vaga.QuantidadeVaga.ToString());
                }
                else
                {
                    objVagaIntegracao.Vaga.QuantidadeVaga = 1;
                }
                #endregion Trata Qtde Vagas

                #region Trata Disponibilidades
                if (!String.IsNullOrEmpty(vaga.DescricaoDisponibilidade))
                {
                    valor = vaga.DescricaoDisponibilidade;
                    valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.Disponibilidade);
                    String[] disponibilidades = valor.Split(';');
                    objVagaIntegracao.Disponibilidades = new List<VagaDisponibilidade>();

                    foreach (string disponibilidade in disponibilidades)
                    {
                        objVagaIntegracao.Disponibilidades.Add(new VagaDisponibilidade { Disponibilidade = Disponibilidade.CarregarPorDescricao(disponibilidade) });
                    }
                }
                #endregion Trata Disponibilidades

                #region Trata Vinculos
                if (!String.IsNullOrEmpty(vaga.DescricaoTipoVinculo))
                {
                    valor = vaga.DescricaoTipoVinculo;
                    valor = AplicarSubstituicoes(valor, BLL.Enumeradores.CampoIntegrador.Contrato);
                    String[] contratos = valor.Split(';');
                    objVagaIntegracao.TiposVinculo = new List<VagaTipoVinculo>();

                    foreach (string contrato in contratos)
                    {
                        objVagaIntegracao.TiposVinculo.Add(new VagaTipoVinculo { TipoVinculo = TipoVinculo.CarregarPorDescricao(contrato) });
                    }
                }
                #endregion Trata Vinculos

                #region Trata Requisitos
                if (!String.IsNullOrEmpty(vaga.DescricaoRequisito))
                {
                    valor = AplicarSubstituicoes(vaga.DescricaoRequisito, BLL.Enumeradores.CampoIntegrador.Requisitos);
                    objVagaIntegracao.Vaga.DescricaoRequisito = valor.Trim();
                }
                #endregion Trata Requisitos

                #region Trata Atribuicoes
                if (!String.IsNullOrEmpty(vaga.DescricaoAtribuicoes))
                {
                    valor = AplicarSubstituicoes(vaga.DescricaoAtribuicoes, BLL.Enumeradores.CampoIntegrador.Atribuicoes);
                    objVagaIntegracao.Vaga.DescricaoAtribuicoes = valor.Trim();
                }
                #endregion Trata Atribuicoes

                #region Trata Deficiencia
                if (vaga.DescricaoDeficiencia != null)
                {
                    valor = AplicarSubstituicoes(vaga.DescricaoDeficiencia, BLL.Enumeradores.CampoIntegrador.Deficiencia).Trim();
                    objVagaIntegracao.DeficienciaImportada = valor;
                    objVagaIntegracao.VagaParaDeficiente = true;
                    objVagaIntegracao.Vaga.FlagDeficiencia = true;
                }
                #endregion Trata Deficiencia

                #region Trata Empresa
                if (!String.IsNullOrEmpty(vaga.NomeEmpresa))
                {
                    valor = AplicarSubstituicoes(vaga.NomeEmpresa, BLL.Enumeradores.CampoIntegrador.NomeFantasia);
                    objVagaIntegracao.Vaga.NomeEmpresa = valor.Trim();
                }
                #endregion Trata Empresa

                #region Trata Telefone
                if (!string.IsNullOrEmpty(vaga.NumeroTelefone) && !string.IsNullOrEmpty(vaga.NumeroDDD))
                {
                    valor = AplicarSubstituicoes(vaga.NumeroTelefone, BLL.Enumeradores.CampoIntegrador.Telefone);
                    objVagaIntegracao.Vaga.NumeroTelefone = valor.Trim();

                    valor = AplicarSubstituicoes(vaga.NumeroDDD, BLL.Enumeradores.CampoIntegrador.DDDTelefone);
                    objVagaIntegracao.Vaga.NumeroDDD = valor.Trim();
                }
                #endregion Trata Telefone

                objVagaIntegracao.Vaga.FlagConfidencial = vaga.FlagConfidencial;

                #region Trata Email
                if (!String.IsNullOrEmpty(vaga.EmailVaga))
                {
                    valor = AplicarSubstituicoes(vaga.EmailVaga, BLL.Enumeradores.CampoIntegrador.EmailRetorno);
                    objVagaIntegracao.Vaga.EmailVaga = valor.Trim().ToLower();
                }
                #endregion Trata Email

                if (!string.IsNullOrEmpty(vaga.DataAbertura))
                    objVagaIntegracao.Vaga.DataAbertura = Convert.ToDateTime(vaga.DataAbertura.ToString());

                if (!string.IsNullOrEmpty(vaga.DataPrazo))
                    objVagaIntegracao.Vaga.DataPrazo = Convert.ToDateTime(vaga.DataPrazo.ToString());

                return objVagaIntegracao;
            }
            catch (Exception ex)
            {
                GravaLogErro(ex, "ConverteVagaToVagaIntegracao()");
                throw;
                return null;
            }
        }
        #endregion ConverteVagaToVagaIntegracao

        #region InativaVaga
        public void InativaVaga(string idVagaSine, Integrador oIntegrador)
        {
            try
            {
                VagaIntegracao objVagaIntegracaoBD;
                VagaIntegracao.CarregarPorCodigoIntegrador(idVagaSine, oIntegrador, out objVagaIntegracaoBD);


                if (objVagaIntegracaoBD != null && objVagaIntegracaoBD.Vaga != null)
                {
                    objVagaIntegracaoBD.Vaga = BLL.Vaga.LoadObject(objVagaIntegracaoBD.Vaga.IdVaga);

                    if (!objVagaIntegracaoBD.Vaga.FlagInativo)
                    {
                        objVagaIntegracaoBD.Vaga.FlagInativo = true;
                        objVagaIntegracaoBD.Vaga.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                GravaLogErro(ex, "InativaVaga()");
                throw;
            }
        }
        #endregion InativaVaga

        #endregion Metodos Publicos


        #region Metodos Privados

        #region InicializaVariaveisGlobais
        protected void InicializaVariaveisGlobais()
        {
            try
            {
                _lstDisponibilidadesNovas = new List<VagaDisponibilidade>();
                _lstDisponibilidadesExcluidas = new List<VagaDisponibilidade>();
                _lstTipoVinculoNovos = new List<VagaTipoVinculo>();
                _lstTipoVinculoExcluidos = new List<VagaTipoVinculo>();

                _MensagemSucessoImportacao = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.MensagemSucessoImportacao);
                _MensagemErroImportacao = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.MensagemErroImportacao);
                _UrlIntegracao = _Integrador.GetValorParametro(BLL.Enumeradores.Parametro.Integracao_Url_Integracao);

                VerificaFlagsCV();
            }
            catch (Exception ex)
            {
                GravaLogErro(ex, "InicializaVariaveisGlobais()");
                throw;
            }
        }
        #endregion InicializaVariaveisGlobais

        #region LimpaVariaveisGlobais
        protected void LimpaVariaveisGlobais()
        {
            try
            {
                _lstDisponibilidadesNovas = null;
                _lstDisponibilidadesExcluidas = null;
                _lstTipoVinculoNovos = null;
                _lstTipoVinculoExcluidos = null;

                _MensagemSucessoImportacao = string.Empty;
                _MensagemErroImportacao = string.Empty;
                _UrlIntegracao = string.Empty;
                _DescErroPublicacaoAutomatica = string.Empty;

                _Integrador = null;

                _VagaIntegracaoBD = null;
                _VagaIntegracao = null;

                _Origem = null;

            }
            catch (Exception ex)
            {
                GravaLogErro(ex, "LimpaVariaveisGlobais()");
                throw;
            }
        }
        #endregion LimpaVariaveisGlobais


        #region VerificaFlagsCV
        protected void VerificaFlagsCV()
        {
            try
            {
                string sFlag;
                sFlag = _Integrador.GetValorParametro(BLL.Enumeradores.Parametro.Integracao_Flg_Receber_Cada_CV);
                _FlagRecebeCadaCV = sFlag == "1" || sFlag == "true";
                sFlag = _Integrador.GetValorParametro(BLL.Enumeradores.Parametro.Integracao_Flg_Receber_Todos_CV);
                _FlagRecebeTodosCV = sFlag == "1" || sFlag == "true";
            }
            catch (Exception ex)
            {
                GravaLogErro(ex, "VerificaFlags()");
                throw;
            }
        }
        #endregion VerificaFlagsCV

        #region Processa
        /// <summary>
        /// Processa novas vagas ou as atualizas, o processo de inativação de vaga será feito por outro método específico para isto
        /// </summary>
        protected bool Processa()
        {
            try
            {
                //Se a cidade não foi definida, então não realiza importação
                if (_VagaIntegracao.Vaga.Cidade == null)
                    return false;

                //Verifica se a vaga pode ser publicada automaticamente
                _DescErroPublicacaoAutomatica = VerificarPublicacaoAutomatica();

                //Variável que define se o processo de e-mail do publicador de vaga tem que ser executado
                _FlagInicializaPublicadorDeVagas = false;

                _GravaHistorico = true;

                //Se funcao não foi identificada tenta detectar funcao através da string recebida.
                if (_VagaIntegracao.Vaga.Funcao == null)
                    _VagaIntegracao.Vaga.Funcao = Funcao.DetectarFuncao(_VagaIntegracao.FuncaoImportada);

                //Verifica Escolaridade
                if (_VagaIntegracao.Vaga.Escolaridade == null && !string.IsNullOrEmpty(_VagaIntegracao.EscolaridadeImportada))
                {
                    Escolaridade oEscolaridade;
                    Escolaridade.CarregarPorNome(_VagaIntegracao.EscolaridadeImportada, out oEscolaridade);
                    _VagaIntegracao.Vaga.Escolaridade = oEscolaridade;
                }

                //Verifica Deficiencia
                if (_VagaIntegracao.Vaga.Deficiencia == null && !string.IsNullOrEmpty(_VagaIntegracao.DeficienciaImportada))
                    _VagaIntegracao.Vaga.Deficiencia = Deficiencia.CarregarPorDescricao(_VagaIntegracao.DeficienciaImportada);

                //Verificando a existencia da vaga
                bool VagaProcessada;
                if (!VagaIntegracao.CarregarPorCodigoIntegrador(_VagaIntegracao.CodigoVagaIntegrador, _Integrador, out _VagaIntegracaoBD))
                    VagaProcessada = ProcessaNovaVaga();
                else
                    VagaProcessada = ProcessaVagaExistente();

                if (!VagaProcessada)
                    return false;

                AtualizaInformacoesVaga();

                SalvaVaga();

                SalvaDisponibilidadesVinculos();

                SalvaHistoricoPublicacaoVaga();

                ExecutaPublicacaoVaga();

                return true;
            }
            catch (Exception ex)
            {
                GravaLogErro(ex, "Processa()");
                return false;
            }
            finally
            {
            }
        }
        #endregion ProcessaVaga

        #region ProcessaNovaVaga
        protected bool ProcessaNovaVaga()
        {
            try
            {
                //Cria uma nova vaga
                _VagaIntegracaoBD = new VagaIntegracao
                {
                    CodigoVagaIntegrador = _VagaIntegracao.CodigoVagaIntegrador,
                    Integrador = _Integrador,
                    Vaga = _VagaIntegracao.Vaga,
                    FlagEnviadaParaAuditoria = false
                };

                //Inicializando flags
                _VagaIntegracaoBD.Vaga.FlagAuditada = false;
                _VagaIntegracaoBD.Vaga.FlagBNERecomenda = false;
                _VagaIntegracaoBD.Vaga.FlagVagaArquivada = false;
                _VagaIntegracaoBD.Vaga.FlagVagaMassa = false;
                _VagaIntegracaoBD.Vaga.FlagLiberada = false;

                //Configurando envio de e-mails de candidatos
                _VagaIntegracao.Vaga.FlagReceberCadaCV = _FlagRecebeCadaCV;
                _VagaIntegracao.Vaga.FlagReceberTodosCV = _FlagRecebeTodosCV;

                //Setando Filial e UsuarioFilialPerfil
                _VagaIntegracaoBD.Vaga.Filial = _Integrador.Filial;
                _VagaIntegracaoBD.Vaga.UsuarioFilialPerfil = _Integrador.UsuarioFilialPerfil;

                //Setando todas as disponibilidades como novas
                if (_VagaIntegracao.Disponibilidades != null)
                    _lstDisponibilidadesNovas.AddRange(_VagaIntegracao.Disponibilidades);

                if (_VagaIntegracao.TiposVinculo != null)
                    _lstTipoVinculoNovos = _VagaIntegracao.TiposVinculo;


                //Verifica se a vaga pode ser publicada automaticamente
                if (String.IsNullOrEmpty(_DescErroPublicacaoAutomatica))
                {
                    if (_VagaIntegracaoBD.Vaga.DataAbertura == null || _VagaIntegracaoBD.Vaga.DataAbertura == DateTime.MinValue)
                        _VagaIntegracaoBD.Vaga.DataAbertura = DateTime.Now;

                    _VagaIntegracaoBD.Vaga.DataPrazo = _VagaIntegracaoBD.Vaga.DataAbertura.Value.AddDays(Convert.ToInt32(Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.QuantidadeDiasPrazoVaga)));

                    //Vaga importada pela primeira vez publicada automaticamente. Inicializa Publicador de vagas
                    _FlagInicializaPublicadorDeVagas = true;
                }
                else
                {
                    _VagaIntegracaoBD.Vaga.DataAbertura = null;
                    _VagaIntegracaoBD.Vaga.DataPrazo = null;
                }
                return true;
            }
            catch (Exception ex)
            {
                GravaLogErro(ex, "ProcessaNovaVaga()");
                throw;
            }
        }
        #endregion ProcessaNovaVaga

        #region ProcessaVagaExistente
        protected bool ProcessaVagaExistente()
        {
            try
            {
                _VagaIntegracaoBD.Vaga.CompleteObject();

                //Se a vaga foi excluída ou auditada no BNE, não realiza nenhuma alteração.
                if ((_VagaIntegracaoBD.Vaga.FlagAuditada.HasValue && _VagaIntegracaoBD.Vaga.FlagAuditada.Value)
                    || _VagaIntegracaoBD.Vaga.FlagInativo || _VagaIntegracaoBD.Vaga.FlagVagaArquivada)
                {
                    return false;
                }

                //Comparando conteúdo dos objetos. Se forem iguais, não faz alteração.
                if (_VagaIntegracaoBD.Vaga.Equals(_VagaIntegracao.Vaga))
                    return false;

                //Se funcao não foi identificada tenta detectar funcao através da string recebida.
                if (_VagaIntegracao.Vaga.Funcao == null)
                    _VagaIntegracao.Vaga.Funcao = Funcao.DetectarFuncao(_VagaIntegracao.Vaga.DescricaoFuncao);

                //Verificando se o histórico deve ser gravado
                //Não grava histórico se a vaga já foi auditada ou se não foi auditada e continua com erros para a publicação automática
                if (_VagaIntegracaoBD.Vaga.FlagAuditada.Value ||
                    (!_VagaIntegracaoBD.Vaga.FlagAuditada.Value && !String.IsNullOrEmpty(_DescErroPublicacaoAutomatica)))
                {
                    _GravaHistorico = false;
                }

                //Verifica se a vaga pode ser publicada automaticamente e publica automaticamente caso esta já não esteja auditada.
                if (String.IsNullOrEmpty(_DescErroPublicacaoAutomatica) &&
                    !_VagaIntegracaoBD.FlagEnviadaParaAuditoria &&
                    !_VagaIntegracaoBD.Vaga.FlagAuditada.Value)
                {
                    //Se vaga ainda não foi auditada, vai ser auditada agora
                    //Processo Publicador de vagas será iniciado
                    _FlagInicializaPublicadorDeVagas = true;
                    _VagaIntegracaoBD.Vaga.DataAbertura = DateTime.Now;
                    _VagaIntegracaoBD.Vaga.DataPrazo = DateTime.Now.AddDays(Convert.ToInt32(Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.QuantidadeDiasPrazoVaga)));
                }

                _VagaIntegracao.Vaga.Filial = _VagaIntegracaoBD.Vaga.Filial;
                _VagaIntegracao.Vaga.UsuarioFilialPerfil = _VagaIntegracaoBD.Vaga.UsuarioFilialPerfil;

                #region Verificando a inserção e exclusão de disponibilidades
                List<VagaDisponibilidade> disponibilidadesSalvas = VagaDisponibilidade.ListarDisponibilidadesPorVaga(_VagaIntegracaoBD.Vaga.IdVaga);
                disponibilidadesSalvas.ForEach(d => d.Disponibilidade.CompleteObject());
                if (_VagaIntegracao.Disponibilidades != null)
                {
                    foreach (var objVagaDisponibilidade in _VagaIntegracao.Disponibilidades)
                    {
                        if (!disponibilidadesSalvas.Exists(d => d.Disponibilidade.IdDisponibilidade == objVagaDisponibilidade.Disponibilidade.IdDisponibilidade))
                        {
                            _lstDisponibilidadesNovas.Add(objVagaDisponibilidade);
                        }
                    }
                }
                if (disponibilidadesSalvas != null && _VagaIntegracao.Disponibilidades != null)
                {
                    foreach (var objVagaDisponibilidade in disponibilidadesSalvas)
                    {
                        if (!_VagaIntegracao.Disponibilidades.Exists(d => d.Disponibilidade.IdDisponibilidade == objVagaDisponibilidade.Disponibilidade.IdDisponibilidade))
                        {
                            _lstDisponibilidadesExcluidas.Add(objVagaDisponibilidade);
                        }
                    }
                }
                #endregion Verificando a inserção e exclusão de disponibilidades

                #region Verificando a inserção e exclusão de tipo vinculo
                List<VagaTipoVinculo> tipoVinculoSalvos = VagaTipoVinculo.ListarTipoVinculoPorVaga(_VagaIntegracaoBD.Vaga.IdVaga);
                tipoVinculoSalvos.ForEach(d => d.TipoVinculo.CompleteObject());
                if (_VagaIntegracao.TiposVinculo != null)
                {
                    foreach (var objVagaTipoVinculo in _VagaIntegracao.TiposVinculo)
                    {
                        if (!tipoVinculoSalvos.Exists(d => d.TipoVinculo.IdTipoVinculo == objVagaTipoVinculo.TipoVinculo.IdTipoVinculo))
                        {
                            _lstTipoVinculoNovos.Add(objVagaTipoVinculo);
                        }
                    }
                }
                if (tipoVinculoSalvos != null && _VagaIntegracao.TiposVinculo != null)
                {
                    foreach (var objVagaTipoVinculo in tipoVinculoSalvos)
                    {
                        if (!_VagaIntegracao.TiposVinculo.Exists(d => d.TipoVinculo.IdTipoVinculo == objVagaTipoVinculo.TipoVinculo.IdTipoVinculo))
                        {
                            _lstTipoVinculoExcluidos.Add(objVagaTipoVinculo);
                        }
                    }
                }
                #endregion Verificando a inserção e exclusão de tipo vinculo

                return true;
            }
            catch (Exception ex)
            {
                GravaLogErro(ex, "ProcessaVagaExistente()");
                throw;
            }
        }
        #endregion ProcessaNovaVaga

        #region AtualizaInformacoesVaga
        protected void AtualizaInformacoesVaga()
        {
            try
            {
                //Se nesse ponto a funcao a ser importada foi reconhecida, redefine função da vaga.
                if (_VagaIntegracao.Vaga.Funcao != null)
                {
                    _VagaIntegracaoBD.Vaga.Funcao = _VagaIntegracao.Vaga.Funcao;
                    _VagaIntegracaoBD.Vaga.DescricaoFuncao = _VagaIntegracao.Vaga.DescricaoFuncao;
                }

                _VagaIntegracaoBD.Vaga.Cidade = _VagaIntegracao.Vaga.Cidade;
                _VagaIntegracaoBD.Vaga.Deficiencia = _VagaIntegracao.Vaga.Deficiencia;
                _VagaIntegracaoBD.Vaga.FlagDeficiencia = _VagaIntegracao.Vaga.Deficiencia != null || (_VagaIntegracao.Vaga.FlagDeficiencia.HasValue && _VagaIntegracao.Vaga.FlagDeficiencia.Value);
                _VagaIntegracaoBD.Vaga.DescricaoAtribuicoes = _VagaIntegracao.Vaga.DescricaoAtribuicoes;
                _VagaIntegracaoBD.Vaga.DescricaoBeneficio = _VagaIntegracao.Vaga.DescricaoBeneficio;
                _VagaIntegracaoBD.Vaga.DescricaoRequisito = _VagaIntegracao.Vaga.DescricaoRequisito;
                _VagaIntegracaoBD.Vaga.EmailVaga = _VagaIntegracao.Vaga.EmailVaga;
                _VagaIntegracaoBD.Vaga.Escolaridade = _VagaIntegracao.Vaga.Escolaridade;
                _VagaIntegracaoBD.Vaga.Filial = _VagaIntegracao.Vaga.Filial;
                _VagaIntegracaoBD.Vaga.FlagBNERecomenda = _VagaIntegracao.Vaga.FlagBNERecomenda;
                _VagaIntegracaoBD.Vaga.FlagConfidencial = _VagaIntegracao.Vaga.FlagConfidencial;
                _VagaIntegracaoBD.Vaga.FlagInativo = _VagaIntegracao.Vaga.FlagInativo;
                _VagaIntegracaoBD.Vaga.FlagLiberada = _VagaIntegracao.Vaga.FlagLiberada;
                _VagaIntegracaoBD.Vaga.FlagReceberCadaCV = _VagaIntegracao.Vaga.FlagReceberCadaCV;
                _VagaIntegracaoBD.Vaga.FlagReceberTodosCV = _VagaIntegracao.Vaga.FlagReceberTodosCV;
                _VagaIntegracaoBD.Vaga.FlagVagaArquivada = _VagaIntegracao.Vaga.FlagVagaArquivada;
                _VagaIntegracaoBD.Vaga.FlagVagaMassa = _VagaIntegracao.Vaga.FlagVagaMassa;
                _VagaIntegracaoBD.Vaga.FlagVagaRapida = _VagaIntegracao.Vaga.FlagVagaRapida;
                _VagaIntegracaoBD.Vaga.NomeEmpresa = _VagaIntegracao.Vaga.NomeEmpresa;
                _VagaIntegracaoBD.Vaga.NumeroIdadeMaxima = _VagaIntegracao.Vaga.NumeroIdadeMaxima;
                _VagaIntegracaoBD.Vaga.NumeroIdadeMinima = _VagaIntegracao.Vaga.NumeroIdadeMinima;
                _VagaIntegracaoBD.Vaga.NumeroDDD = _VagaIntegracao.Vaga.NumeroDDD;
                _VagaIntegracaoBD.Vaga.NumeroTelefone = _VagaIntegracao.Vaga.NumeroTelefone;
                _VagaIntegracaoBD.Vaga.QuantidadeVaga = _VagaIntegracao.Vaga.QuantidadeVaga;
                _VagaIntegracaoBD.Vaga.Sexo = _VagaIntegracao.Vaga.Sexo;
                _VagaIntegracaoBD.Vaga.UsuarioFilialPerfil = _VagaIntegracao.Vaga.UsuarioFilialPerfil;
                _VagaIntegracaoBD.Vaga.ValorSalarioDe = _VagaIntegracao.Vaga.ValorSalarioDe;
                _VagaIntegracaoBD.Vaga.ValorSalarioPara = _VagaIntegracao.Vaga.ValorSalarioPara;
                _VagaIntegracaoBD.Vaga.DescricaoFuncao = _VagaIntegracao.Vaga.DescricaoFuncao;
                
                //Todas as vagas presentes no arquivo estarão ativas
                _VagaIntegracaoBD.FlagInativo = false;
                _VagaIntegracaoBD.Vaga.FlagInativo = false;
                _VagaIntegracaoBD.Vaga.Origem = _Origem;
            }
            catch (Exception ex)
            {
                GravaLogErro(ex, "AtualizaInformaçõesVaga()");
                throw;
            }
        }
        #endregion

        #region VerificarPublicacaoAutomatica
        /// <summary>
        /// Verifica se erros ocorreram na importação da vaga.
        /// </summary>
        /// <returns>String com os erros. Se o retorno for vazio, nenhum erro foi encontrado.</returns>
        protected string VerificarPublicacaoAutomatica()
        {
            try
            {
                StringBuilder retorno = new StringBuilder();
                String msg;
                if (_VagaIntegracao.Vaga.Funcao == null || _VagaIntegracao.Vaga.Funcao.IdFuncao <= 0)
                {
                    msg = BNE.BLL.Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.MensagemPublicacaoAutomaticaErroFuncao);
                    msg = msg.Replace("{funcao}", _VagaIntegracao.FuncaoImportada);
                    retorno.AppendLine(msg);
                }
                if (_VagaIntegracao.Vaga.Cidade == null || _VagaIntegracao.Vaga.Cidade.IdCidade <= 0)
                {
                    msg = BNE.BLL.Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.MensagemPublicacaoAutomaticaErroCidade);
                    msg = msg.Replace("{cidade}", _VagaIntegracao.CidadeImportada);
                    retorno.AppendLine(msg);
                }

                return retorno.ToString();
            }
            catch (Exception ex)
            {
                GravaLogErro(ex, "VerificaPublicacaoAutomatica()");
                throw;
                return "";
            }
        }
        #endregion

        #region SalvaVaga
        protected void SalvaVaga()
        {
            var conn = new SqlConnection(BNE.BLL.DataAccessLayer.CONN_STRING);
            conn.Open();
            try
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        #region transacao

                        //Salvando todos os dados
                        _VagaIntegracaoBD.Vaga.Save(trans);
                        _VagaIntegracaoBD.Save(trans);

                        //Definindo as palavras chave da vaga
                        //vaga, emprego, trabalho, <descição da função>, <Nome da Cidade>

                        List<PalavraChave> palavrasChaves = new List<PalavraChave>();

                        //Palavra Vaga, Emprego e Trabalho
                        palavrasChaves.Add(new PalavraChave { DescricaoPalavraChave = "vaga" });
                        palavrasChaves.Add(new PalavraChave { DescricaoPalavraChave = "emprego" });
                        palavrasChaves.Add(new PalavraChave { DescricaoPalavraChave = "trabalho" });
                        palavrasChaves.Add(new PalavraChave { DescricaoPalavraChave = "oportunidade" });

                        //Funcao
                        if (_VagaIntegracaoBD.Vaga.Funcao != null)
                        {
                            foreach (
                                string palavra in _VagaIntegracaoBD.Vaga.Funcao.DescricaoFuncao.Split(' '))
                            {
                                if (palavra.Length > 2)
                                {
                                    palavrasChaves.Add(new PalavraChave { DescricaoPalavraChave = palavra });
                                }
                            }

                            palavrasChaves.Add(new PalavraChave
                            {
                                DescricaoPalavraChave = _VagaIntegracaoBD.Vaga.Funcao.DescricaoFuncao
                            });
                        }

                        //Cidade
                        if (_VagaIntegracaoBD.Vaga.Cidade != null)
                        {
                            palavrasChaves.Add(new PalavraChave
                            {
                                DescricaoPalavraChave =
                                    String.Format("{0} {1}", _VagaIntegracaoBD.Vaga.Cidade.NomeCidade,
                                        _VagaIntegracaoBD.Vaga.Cidade.Estado.SiglaEstado)
                            });
                        }

                        VagaPalavraChave.AtualizaPalavrasChaveDaVaga(_VagaIntegracaoBD.Vaga, palavrasChaves, trans);

                        trans.Commit();

                        #endregion transacao
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion SalvaVaga

        #region SalvaDisponibilidadesVinculos
        protected void SalvaDisponibilidadesVinculos()
        {
            try
            {
                if (_lstDisponibilidadesNovas != null)
                {
                    foreach (var objVagaDisponibilidade in _lstDisponibilidadesNovas)
                    {
                        try
                        {
                            objVagaDisponibilidade.Vaga = _VagaIntegracaoBD.Vaga;
                            objVagaDisponibilidade.Save();
                        }
                        catch (Exception ex)
                        {
                            GravaLogErro(ex, "SalvaDisponibilidadesVinculos() - gravar disponibilidade");
                        }
                    }
                }
                if (_lstDisponibilidadesExcluidas != null)
                {
                    foreach (var objVagaDisponibilidade in _lstDisponibilidadesExcluidas)
                    {
                        try
                        {
                            VagaDisponibilidade.Delete(objVagaDisponibilidade.IdVagaDisponibilidade);
                        }
                        catch (Exception ex)
                        {
                            GravaLogErro(ex, "SalvaDisponibilidadesVinculos() - excluir disponibilidade");
                        }
                    }
                }
                if (_lstTipoVinculoNovos != null)
                {
                    foreach (var objTipoVinculo in _lstTipoVinculoNovos)
                    {
                        try
                        {
                            objTipoVinculo.Vaga = _VagaIntegracaoBD.Vaga;
                            objTipoVinculo.Save();
                        }
                        catch (Exception ex)
                        {
                            GravaLogErro(ex, "SalvaDisponibilidadesVinculos() - gravar tipo vinculo");
                        }
                    }
                }
                if (_lstTipoVinculoExcluidos != null)
                {
                    foreach (var objTipoVinculo in _lstTipoVinculoExcluidos)
                    {
                        try
                        {
                            VagaTipoVinculo.Delete(objTipoVinculo.IdVagaTipoVinculo);
                        }
                        catch (Exception ex)
                        {
                            GravaLogErro(ex, "SalvaDisponibilidadesVinculos() - excluir tipo vinculo");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GravaLogErro(ex, "SalvaDisponibilidadesVinculos()");
                throw;
            }
        }
        #endregion SalvaDisponibilidadesVinculos

        #region SalvaHistoricoPublicacaoVaga
        protected void SalvaHistoricoPublicacaoVaga()
        {
            try
            {
                if (_GravaHistorico)
                {
                    if (String.IsNullOrEmpty(_DescErroPublicacaoAutomatica))
                    {
                        HistoricoPublicacaoVaga.RegistrarHistorico(_VagaIntegracaoBD.Vaga.IdVaga, _MensagemSucessoImportacao.Replace("{url}", _UrlIntegracao).Replace("{data}", DateTime.Now.ToString("dd/MM/yy H:mm:ss")));
                    }
                    else
                    {
                        HistoricoPublicacaoVaga.RegistrarHistorico(_VagaIntegracaoBD.Vaga.IdVaga, _MensagemErroImportacao.Replace("{erros}", _DescErroPublicacaoAutomatica));
                    }
                }
            }
            catch (Exception ex)
            {
                GravaLogErro(ex, "SalvaHistoricoPublicacaoVaga()");
                throw;
            }
        }
        #endregion SalvaHistoricoPublicacaoVaga

        #region ExecutaPublicacaoVaga
        protected void ExecutaPublicacaoVaga()
        {
            try
            {
                //Se foi está pronta para a auditoria e o processo de publicação não foi iniciado, envia a vaga para o precesso do Publicador assincrono
                if (_FlagInicializaPublicadorDeVagas && !_VagaIntegracaoBD.FlagEnviadaParaAuditoria)
                {
                    var parametros = new ParametroExecucaoCollection
                        {
                            {"idVaga", "Vaga", _VagaIntegracaoBD.Vaga.IdVaga.ToString(CultureInfo.InvariantCulture), _VagaIntegracaoBD.Vaga.CodigoVaga} ,
                            {"EnfileraRastreador", "Deve enfileirar rastreador", "true", "Verdadeiro"}
                        };

                    ProcessoAssincrono.IniciarAtividade(
                        TipoAtividade.PublicacaoVaga,
                        PluginsCompatibilidade.CarregarPorMetadata("PublicacaoVaga", "PublicacaoVagaRastreador"),
                        parametros,
                        null,
                        null,
                        null,
                        null,
                        DateTime.Now);

                    //Sinalizando que a vaga foi enviada para o processo de auditoria
                    _VagaIntegracaoBD.FlagEnviadaParaAuditoria = true;
                    _VagaIntegracaoBD.Save();
                }
            }
            catch (Exception ex)
            {
                GravaLogErro(ex, "ExecutaPublicacaoVaga()");
                throw;
            }
        }
        #endregion ExecucaPublicacaoVaga

        #region AplicarSubstituicoes
        /// <summary>
        /// Método para aplicar as regras de substituição nos campos da vaga importada.
        /// </summary>
        /// <param name="lstSubstituicoes">Lista com as substituições a serem efetuadas</param>
        /// <param name="objVagaIntegracao">Objeto vaga onde as substituições serão aplicadas</param>
        protected String AplicarSubstituicoes(String descricao, BNE.BLL.Enumeradores.CampoIntegrador campo)
        {
            //Lista de substituições para todos os campos
            List<SubstituicaoIntegracao> lstSubstituicoesTodos = _lstSubstituicoes.Where(s => s.RegraSubstituicaoIntegracao == null ||
                                                                                             s.RegraSubstituicaoIntegracao.CampoIntegrador == null).ToList();
            foreach (var objSubstituicao in lstSubstituicoesTodos)
            {
                descricao = objSubstituicao.AplicarSubstituicao(descricao);
            }

            //Lista de substituições para o campo
            List<SubstituicaoIntegracao> lstSubstituicoesCampo = _lstSubstituicoes.Where(s => s.RegraSubstituicaoIntegracao != null &&
                                                                                             s.RegraSubstituicaoIntegracao.CampoIntegrador == campo).ToList();
            foreach (var objSubstituicao in lstSubstituicoesCampo)
            {
                descricao = objSubstituicao.AplicarSubstituicao(descricao);
            }

            return descricao;
        }
        #endregion
        #endregion Metodos Privados

        #region GravaLogErro
        protected static void GravaLogErro(Exception ex, string metodo)
        {
            try
            {
                string sCustomMessage = "appBNE - Integracao Vagas. Erro na classe BLL.Custom.Vaga.IntegrarVaga. Método: " + metodo;
                EL.GerenciadorException.GravarExcecao(ex, sCustomMessage);
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}
