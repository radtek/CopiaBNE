using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AutoMapper;
using BNE.Core.Common;
using BNE.Data.Infrastructure;
using BNE.Logger.Interface;
using BNE.Global.Domain;
using BNE.Global.Model;
using BNE.PessoaFisica.Data.Repositories;
using BNE.PessoaFisica.Domain.Command;
using BNE.PessoaFisica.Model;
using Newtonsoft.Json;

namespace BNE.PessoaFisica.Domain
{
    public class PessoaFisica
    {
        private readonly Cidade _cidade;
        private readonly CodigoConfirmacaoEmail _codigoConfirmacaoEmail;
        private readonly Curriculo _curriculo;
        private readonly CurriculoOrigem _curriculoOrigem;
        private readonly CurriculoParametro _curriculoParametro;
        private readonly Curso _curso;
        private readonly Deficiencia _deficiencia;
        private readonly Email _email;
        private readonly Mensagem.Email _emailService;
        private readonly Escolaridade _escolaridade;
        private readonly ExperienciaProfissional _experienciaProfissional;
        private readonly Formacao _formacao;
        private readonly FuncaoSinonimo _funcao;
        private readonly FuncaoPretendida _funcaoPretendida;
        private readonly InstituicaoEnsino _instituicaoEnsino;
        private readonly ILogger _logger;
        private readonly Origem _origem;
        private readonly Parametro _parametro;
        private readonly IPessoaFisicaRepository _pessoaFisicaRepository;

        private readonly RamoAtividade _ramoAtividade;
        private readonly Sexo _sexo;
        private readonly SituacaoCurriculo _situacaoCurriculo;
        private readonly TelefoneCelular _telefoneCelular;

        private readonly TipoCurriculo _tipoCurriculo;

        private readonly IUnitOfWork _unitOfWork;
        public PreCurriculo PreCurriculo { get; set; }

        public PessoaFisica(IPessoaFisicaRepository pessoaFisicaRepository, IUnitOfWork unitOfWork, TipoCurriculo tipoCurriculo, SituacaoCurriculo situacaoCurriculo, ILogger logger,
            Curriculo curriculo, Email email, FuncaoPretendida funcaoPretendida, CurriculoOrigem curriculoOrigem, Origem origem, TelefoneCelular telefoneCelular,
            CurriculoParametro curriculoParametro, Mensagem.Email emailService, CodigoConfirmacaoEmail codigoConfirmacaoEmail, ExperienciaProfissional experienciaProfissional,
            RamoAtividade ramoAtividade, Formacao formacao, Escolaridade escolaridade, Cidade cidade, InstituicaoEnsino instituicaoEnsino,
            Curso curso, Parametro parametro, FuncaoSinonimo funcao, Sexo sexo, Deficiencia deficiencia)
        {
            _pessoaFisicaRepository = pessoaFisicaRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;

            _tipoCurriculo = tipoCurriculo;
            _situacaoCurriculo = situacaoCurriculo;
            _curriculo = curriculo;
            _email = email;
            _funcaoPretendida = funcaoPretendida;
            _curriculoOrigem = curriculoOrigem;
            _origem = origem;
            _telefoneCelular = telefoneCelular;
            _curriculoParametro = curriculoParametro;
            _emailService = emailService;
            _codigoConfirmacaoEmail = codigoConfirmacaoEmail;
            _experienciaProfissional = experienciaProfissional;
            _ramoAtividade = ramoAtividade;
            _formacao = formacao;
            _escolaridade = escolaridade;
            _cidade = cidade;
            _instituicaoEnsino = instituicaoEnsino;
            _curso = curso;
            _parametro = parametro;
            _funcao = funcao;
            _sexo = sexo;
            _deficiencia = deficiencia;
        }

        public Model.PessoaFisica GetById(int id)
        {
            return _pessoaFisicaRepository.GetById(id);
        }

        /// <summary>
        ///     Validar se o CPF e Data Nascimento já existem
        /// </summary>
        /// <param name="cpf"></param>
        /// <param name="dataNascimento"></param>
        /// <returns></returns>
        public string CarregarNomePessoaFisicaJaExiste(decimal cpf, DateTime dataNascimento)
        {
            var nomePessoa = string.Empty;

            try
            {
                //Chcecar PF no Banco novo
                nomePessoa = _pessoaFisicaRepository.GetMany(p => p.CPF == cpf && p.DataNascimento == dataNascimento.Date).Select(x => x.Nome).FirstOrDefault();

                if (string.IsNullOrWhiteSpace(nomePessoa))
                {
                    nomePessoa = new Mapper.ToOld.PessoaFisica().RecuperarNomePessoaPorCPFDataNascimento(cpf, dataNascimento);
                }

                return nomePessoa;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return "";
            }
        }

        public Model.PessoaFisica CarregarPessoaFisicaJaExiste(decimal cpf, DateTime dataNascimento)
        {
            try
            {
                //Chcecar PF no Banco novo
                var pessoaFisica = _pessoaFisicaRepository.GetMany(p => p.CPF == cpf && p.DataNascimento == dataNascimento.Date).FirstOrDefault();

                if (pessoaFisica == null)
                {
                    //Checar PF no banco velho, mas o ID é diferente
                    //se existir mapear para o novo
                }

                return pessoaFisica;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return null;
            }
        }

        public int PessoaFisicaJaExiste(decimal cpf, DateTime dataNascimento)
        {
            try
            {
                //Chcecar PF no Banco novo
                var pessoaFisica = _pessoaFisicaRepository.GetMany(p => p.CPF == cpf && p.DataNascimento == dataNascimento.Date).FirstOrDefault();

                if (pessoaFisica == null)
                {
                    //Checar PF no banco velho, mas o ID é diferente
                    var idPessoaFisica = new Mapper.ToOld.PessoaFisica().CarregarIdSeExistePessoaFisica(cpf);

                    return idPessoaFisica;
                }

                return pessoaFisica.Id;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return -1;
            }
        }

        #region CurriculoExiste
        public bool CurriculoExiste(int idPessoaFisica)
        {
            try
            {
                if (_curriculo.CurriculoAtivo(idPessoaFisica))
                {
                    return true;
                }

                return new Mapper.ToOld.PessoaFisica().ExisteCurriculo(idPessoaFisica);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return false;
            }
        }
        #endregion

        #region GetLinksPaginasSemelhantes
        public string[] GetLinksPaginasSemelhantes(string funcao, string cidade, string siglaEstado, string areaBNE)
        {
            return new Mapper.ToOld.PessoaFisica().GetLinksPaginasSemelhantes(funcao, cidade, siglaEstado, areaBNE);
        }
        #endregion

        #region GetPlano
        public Plano GetPlano(decimal cpf)
        {
            var plano = new Mapper.ToOld.PessoaFisica().CarregarPlanoPremium(cpf);
            var planoR = new Plano { PrecoCandidatura = plano.PrecoCandidatura, PrecoVip = plano.PrecoVip };

            return planoR;
        }
        #endregion

        public string GetCadidaturasDegustacao(decimal cpf, DateTime dataNascimento)
        {
            var saldo = "0";

            var objPessoa = RetornarFisicaJaExiste(cpf, dataNascimento);

            if (objPessoa != null)
            {
                var result = new Mapper.ToOld.PessoaFisica().ECurriculoVIP(cpf);

                if (result.Item1)
                    saldo = "VIP";
                else
                    saldo = new Mapper.ToOld.PessoaFisica().GetParametroCurriculo(result.Item2);
            }

            return saldo;
        }

        /// <summary>
        ///     Validar se o CPF e Data Nascimento já existem
        /// </summary>
        /// <param name="cpf"></param>
        /// <param name="dataNascimento"></param>
        /// <returns></returns>
        public Model.PessoaFisica RetornarFisicaJaExiste(decimal cpf, DateTime dataNascimento)
        {
            try
            {
                //Chcecar PF no Banco novo
                var pessoaFisica = _pessoaFisicaRepository.GetMany(p => p.CPF == cpf && p.DataNascimento == dataNascimento.Date).FirstOrDefault();

                if (pessoaFisica == null)
                {
                    pessoaFisica = new Model.PessoaFisica { Id = 0, CPF = cpf, DataNascimento = dataNascimento };
                }

                return pessoaFisica;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return null;
            }
        }

        #region IndicarAmigos
        public bool IndicarAmigos(Indicacao objIndicacao)
        {
            var commandObjects = MapearIndicacaoAmigo(objIndicacao);
            var retorno = new BNE.Mapper.ToOld.PessoaFisica().IndicarAmigos(commandObjects);

            return retorno;
        }
        #endregion

        #region Efetuar a candidatura
        /// <summary>
        ///     Efetuar a candidatura e retorna o nome do candidato
        /// </summary>
        /// <param name="preCurriculo"></param>
        /// <returns></returns>
        public Aggregates.Candidatura Candidatar(Command.PreCurriculo preCurriculo, bool jaExiste = false)
        {
            var cpf = preCurriculo.CPF;
            var dataNascimento = preCurriculo.DataNascimento;

            try
            {
                var nome = string.Empty;

                if (!jaExiste)
                {
                    nome = CarregarNomePessoaFisicaJaExiste(cpf, dataNascimento);

                    if (new Mapper.ToOld.PessoaFisica().PessoaFisicaInativa(cpf) || string.IsNullOrWhiteSpace(nome))
                    {
                        return new Aggregates.Candidatura(string.Empty, cpf, dataNascimento, preCurriculo.UrlPesquisa, false);
                    }
                }

                var list = new List<Tuple<int, bool?, string>>();

                if (preCurriculo.Perguntas != null)
                {
                    list.AddRange(preCurriculo.Perguntas.Select(pergunta => new Tuple<int, bool?, string>(pergunta.idVagaPergunta, pergunta.flgRespostaPergunta, pergunta.resposta)));
                }

                var result = new Mapper.ToOld.PessoaFisica().SalvarCandidatura(cpf, preCurriculo.IdVaga, false, list);

                if (result)
                {
                    //candidatura salva
                    var pessoa = RetornarFisicaJaExiste(cpf, dataNascimento);
                    var curriculo = _curriculo.GetByIdPessoaFisica(pessoa.Id);
                    if (curriculo != null)
                    {
                        DescontarCandidatura(curriculo.Id);
                    }
                    _unitOfWork.Commit();
                }

                return new Aggregates.Candidatura(nome, cpf, dataNascimento, preCurriculo.UrlPesquisa, result);
            }
            catch (Exception ex)
            {
                return new Aggregates.Candidatura(string.Empty, cpf, dataNascimento, preCurriculo.UrlPesquisa, false);
            }
        }
        #endregion

        #region CadastrarMiniCV
        public bool CadastrarMiniCV(Command.PessoaFisica pessoaFisica, out bool candidatura)
        {
            var objPessoaFisica = CarregarPessoaFisicaJaExiste(pessoaFisica.CPF, pessoaFisica.DataNascimento);

            var baseData = DateTime.Now;
            var tipoCurriculo = _tipoCurriculo.GetById(1);
            var situacaoCurriculo = _situacaoCurriculo.GetById(1);

            //TODO: Charan => pegar origem conforme o STC ou parceiro BNE
            var origem = _origem.GetById(1); // Origem 1 é o BNE.

            #region PessoaFisica
            if (objPessoaFisica == null)
            {
                objPessoaFisica = new Model.PessoaFisica
                {
                    DataCadastro = baseData
                };
            }

            var objSexo = _sexo.GetByChar(pessoaFisica.Sexo);


            objPessoaFisica.Nome = pessoaFisica.Nome;
            objPessoaFisica.CPF = Convert.ToDecimal(Utils.LimparMascaraCPFCNPJCEP(pessoaFisica.CPF.ToString()));
            objPessoaFisica.DataNascimento = pessoaFisica.DataNascimento;
            objPessoaFisica.Sexo = objSexo;
            objPessoaFisica.DeficienciaGlobal = pessoaFisica.IdDeficiencia != null ? _deficiencia.GetById(pessoaFisica.IdDeficiencia.Value) : null;
            objPessoaFisica.DataAlteracao = baseData;
            objPessoaFisica.EscolaridadeGlobal = pessoaFisica.IdEscolaridade > 0 ? _escolaridade.GetById(pessoaFisica.IdEscolaridade) : null;

            #region Cidade
            var objCidade = _cidade.GetByNomeUF(pessoaFisica.Cidade);
            objPessoaFisica.Cidade = objCidade;
            #endregion

            var objEmailPessoaFisica = new Model.Email();

            if (pessoaFisica.Email != null)
            {
                objEmailPessoaFisica.Endereco = pessoaFisica.Email;
                objEmailPessoaFisica.PessoaFisica = objPessoaFisica;
                objEmailPessoaFisica.DataCadastro = baseData;

                _pessoaFisicaRepository.Add(objPessoaFisica);
                _email.SalvarEmail(objEmailPessoaFisica);
            }

            #region Telefone
            var dddCelular = byte.Parse(pessoaFisica.Celular.Replace("(", "").Replace(")", "").Replace("-", "").Substring(0, 2));
            var celular = decimal.Parse(pessoaFisica.Celular.Replace("(", "").Replace(")", "").Replace("-", "").Substring(3));

            _telefoneCelular.SalvarTelefone(objPessoaFisica, dddCelular, celular);
            #endregion

            #endregion

            #region Curriculo
            var objCurriculo = new Model.Curriculo
            {
                PessoaFisica = objPessoaFisica,
                FlgVIP = false,
                Ativo = false,
                FlgDisponivelViagem = false,
                TipoCurriculo = tipoCurriculo,
                SituacaoCurriculo = situacaoCurriculo,
                PretensaoSalarial = pessoaFisica.PretensaoSalarial.Value,
                DataCadastro = baseData
            };

            var cvSalvo = _curriculo.SalvarMiniCurriculo(objCurriculo);
            #endregion

            #region Funcao Pretendida
            short tempoExperiencia = 0;

            if (pessoaFisica.TempoExperienciaAnos != null && pessoaFisica.TempoExperienciaMeses != null)
                tempoExperiencia = short.Parse(((pessoaFisica.TempoExperienciaAnos * 12) + pessoaFisica.TempoExperienciaMeses).ToString());

            //TODO Charan => verificar as funções, por ex.: Desenvolvedor PHP na retorna nessa consulta
            var objFuncao = _funcao.GetByNome(pessoaFisica.Funcao);

            var objFuncaoPretendida = new Model.FuncaoPretendida
            {
                DataCadastro = baseData,
                IdFuncao = objFuncao.Id,
                Curriculo = objCurriculo,
                Descricao = pessoaFisica.Funcao,
                TempoExperiencia = tempoExperiencia
            };

            _funcaoPretendida.Salvar(objFuncaoPretendida);
            #endregion

            #region CurriculoOrigem
            var objCurriculoOrigem = new Model.CurriculoOrigem
            {
                DataCadastro = baseData,
                Curriculo = objCurriculo,
                OrigemGlobal = origem
            };

            _curriculoOrigem.Salvar(objCurriculoOrigem);
            #endregion

            #region CurriculoParametro
            var objCurriculoParametro = new Model.CurriculoParametro
            {
                Curriculo = objCurriculo,
                DataCadastro = baseData,
                Ativo = true,
                Valor = Model.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao.ToString(),
                Parametro = _parametro.GetById(Model.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao)
            };

            _curriculoParametro.Salvar(objCurriculoParametro);
            #endregion

            //Chamar o mapeamento
            var commandObject = MapearNovoToAntigo(objCurriculo, baseData, objCurriculoOrigem, objFuncaoPretendida, objPessoaFisica, objCurriculoParametro, objEmailPessoaFisica.Endereco, 0, dddCelular, celular);
            new Mapper.ToOld.PessoaFisica().Map(commandObject, out candidatura, true);

            _unitOfWork.Commit();

            #region Enviar Carta de Confirmação de e-mail.
            if (pessoaFisica.Email != null)
            {
                EnviarCartaConfirmacaoDeEmail(objPessoaFisica.Nome, objPessoaFisica.CPF, objPessoaFisica.DataNascimento, objCurriculo.Id, objPessoaFisica.Id, objEmailPessoaFisica.Endereco);
            }
            #endregion

            return true;
        }
        #endregion

        #region PostMiniCurriculo
        /// <summary>
        ///     Criar o mini curriculo a partir do pré-cadastro
        /// </summary>
        /// <param name="preCurriculo"></param>
        /// <returns></returns>
        public bool PostMiniCurriculo(Command.PreCurriculo preCurriculo, out bool candidatura)
        {
            candidatura = false;
            try
            {
                var idPessoaFisica = PessoaFisicaJaExiste(preCurriculo.CPF, preCurriculo.DataNascimento);

                if (idPessoaFisica > 0)
                {
                    if (CurriculoExiste(idPessoaFisica))
                    {
                        return Candidatar(preCurriculo, true).Candidatou;
                    }
                    //Todo: Retornar mensagem de CV Bloqueado
                    return false;
                }

                var baseData = DateTime.Now; //Data base para todas as datas de cadastros

                var tipoCurriculo = _tipoCurriculo.GetById(1);
                var situacaoCurriculo = _situacaoCurriculo.GetById(1);

                // TODO: pegar origem conforme o STC ou parceiro BNE
                var origem = _origem.GetById(1); // Origem 1 é o BNE.

                Model.PessoaFisica objPessoaFisica;
                byte dddCelular;
                decimal celular;
                #region PessoaFisica
                try
                {
                    var objSexo = _sexo.GetByChar(preCurriculo.Sexo);
                    objPessoaFisica = new Model.PessoaFisica
                    {
                        Nome = preCurriculo.Nome,
                        CPF = preCurriculo.CPF,
                        DataCadastro = baseData,
                        DataNascimento = preCurriculo.DataNascimento,
                        Sexo = objSexo
                    };

                    if (preCurriculo.Email != null)
                    {
                        var objEmailPessoaFisica = new Model.Email
                        {
                            Endereco = preCurriculo.Email,
                            PessoaFisica = objPessoaFisica,
                            DataCadastro = baseData
                        };

                        _pessoaFisicaRepository.Add(objPessoaFisica);
                        _email.SalvarEmail(objEmailPessoaFisica);
                    }

                    dddCelular = byte.Parse(preCurriculo.Celular.Replace("(", "").Replace(")", "").Replace("-", "").Substring(0, 2));
                    celular = decimal.Parse(preCurriculo.Celular.Replace("(", "").Replace(")", "").Replace("-", "").Substring(3));

                    _telefoneCelular.SalvarTelefone(objPessoaFisica, dddCelular, celular);

                    objPessoaFisica.Cidade = _cidade.GetById(preCurriculo.IdCidade);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Erro ao criar Pessoa física");
                    throw ex;
                }
                #endregion

                #region Curriculo
                Model.Curriculo objCurriculo;
                try
                {
                    objCurriculo = new Model.Curriculo
                    {
                        PessoaFisica = objPessoaFisica,
                        FlgVIP = false,
                        Ativo = false,
                        FlgDisponivelViagem = false,
                        TipoCurriculo = tipoCurriculo,
                        SituacaoCurriculo = situacaoCurriculo,
                        PretensaoSalarial = preCurriculo.PretensaoSalarial,
                        DataCadastro = baseData
                    };

                    _curriculo.SalvarMiniCurriculo(objCurriculo);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Erro ao criar Currículo");
                    throw ex;
                }
                #endregion

                #region Funcao Pretendida
                Model.FuncaoPretendida objFuncaoPretendida;
                try
                {
                    short tempoExperiencia = 0;

                    if (preCurriculo.TempoExperienciaAnos != null && preCurriculo.TempoExperienciaMeses != null)
                        tempoExperiencia = short.Parse(((preCurriculo.TempoExperienciaAnos * 12) + preCurriculo.TempoExperienciaMeses).ToString());

                    objFuncaoPretendida = new Model.FuncaoPretendida
                    {
                        DataCadastro = baseData,
                        IdFuncao = preCurriculo.IdFuncao,
                        Curriculo = objCurriculo,
                        Descricao = preCurriculo.DescricaoFuncao,
                        TempoExperiencia = tempoExperiencia
                    };

                    _funcaoPretendida.Salvar(objFuncaoPretendida);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Erro ao criar Função Pretendida");
                    throw ex;
                }
                #endregion

                #region CurriculoOrigem
                Model.CurriculoOrigem objCurriculoOrigem;
                try
                {
                    objCurriculoOrigem = new Model.CurriculoOrigem
                    {
                        DataCadastro = baseData,
                        Curriculo = objCurriculo,
                        OrigemGlobal = origem
                    };

                    _curriculoOrigem.Salvar(objCurriculoOrigem);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Erro ao criar CurriculoOrigem");
                    throw ex;
                }
                #endregion

                #region CurriculoParametro
                Model.CurriculoParametro objCurriculoParametro;
                try
                {
                    objCurriculoParametro = new Model.CurriculoParametro
                    {
                        Curriculo = objCurriculo,
                        DataCadastro = baseData,
                        Ativo = true,
                        Valor = _curriculo.EstaNaRegiaoDeCampanhaBH(objPessoaFisica.Cidade.Id) ? _parametro.GetById(Model.Enumeradores.Parametro.QuantidadeCandidaturaDegustacaoBH).Valor : ((int)Model.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao).ToString(),
                        Parametro = _parametro.GetById(Model.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao)
                    };

                    _curriculoParametro.Salvar(objCurriculoParametro);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Erro ao criar CurriculoParametro");
                    throw ex;
                }
                #endregion

                //Chamar o mapeamento
                var commandObject = MapearNovoToAntigo(objCurriculo, baseData, objCurriculoOrigem, objFuncaoPretendida, objPessoaFisica, objCurriculoParametro, preCurriculo.Email, preCurriculo.IdVaga, dddCelular, celular);


                new Mapper.ToOld.PessoaFisica().Map(commandObject, out candidatura, preCurriculo.Candidatar);


                _unitOfWork.Commit();

                #region Atualizar PreCurriculo com o Id do Currículo
                try
                {
                    var preCV = PreCurriculo.Get(preCurriculo.Id);
                    preCV.IdCurriculo = objCurriculo.Id;

                    PreCurriculo.SetarCurriculonoPreCurriculo(preCV);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Erro ao criar Atualizar PreCurriculo com o Id do Currículo");
                    throw ex;
                }
                #endregion

                //atualizar número de candidaturas
                try
                {
                    if (preCurriculo.Candidatar)
                        DescontarCandidatura(objCurriculoParametro.Curriculo.Id);
                    //objCurriculoParametro.Valor = "2"; //descontar uma candidatura
                    //_curriculoParametro.Atualizar(objCurriculoParametro);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Erro ao criar atualizar número de candidaturas");
                    throw ex;
                }

                #region Enviar Carta de Confirmação de e-mail.
                try
                {
                    if (preCurriculo.Email != null)
                    {
                        EnviarCartaConfirmacaoDeEmail(objPessoaFisica.Nome, objPessoaFisica.CPF, objPessoaFisica.DataNascimento, objCurriculo.Id, objPessoaFisica.Id, preCurriculo.Email);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Erro ao criar Enviar Carta de Confirmação de e-mail");
                    throw ex;
                }
                #endregion

                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao criar mini CV");
                throw ex;
            }
        }
        #endregion

        #region Enviar Carta de Confirmação de e-mail.
        private void EnviarCartaConfirmacaoDeEmail(string nome, decimal cpf, DateTime dataNascimento, int idCurriculo, int idPessoaFisica, string email)
        {
            var confirmacao = new
            {
                NomeCandidato = nome,
                cpf,
                dataNascimento = dataNascimento.ToString("dd/MM/yyyy"),
                Link = ConfigurationManager.AppSettings["EnderecoBNE"] +
                       "/confirmar-email?IdCV=" + idCurriculo + "&codigo=" +
                       _codigoConfirmacaoEmail.GerarCodigoValidacaoEmail(idPessoaFisica, email)
            };

            _emailService.EnviarEmail(Model.Enumeradores.Email.TemplateConfirmacaoEmailCandidato, "atendimento@bne.com.br", email, confirmacao);
        }
        #endregion

        #region PostFormacao
        public bool PostFormacao(Command.Formacao formacao)
        {
            var baseData = DateTime.Now;

            try
            {
                var pessoa = RetornarFisicaJaExiste(formacao.Cpf, formacao.DataNascimento.Date);

                var objFormacao = new Model.Formacao
                {
                    AnoConclusao = formacao.AnoConclusao,
                    CargaHoraria = formacao.CargaHoraria,
                    NomeInstituicao = formacao.NomeInstituicao,
                    NomeCurso = formacao.NomeCurso,
                    DataCadastro = baseData,
                    DataAlteracao = baseData,
                    Ativo = true,
                    PessoaFisica = pessoa,
                    EscolaridadeGlobal = _escolaridade.GetById(formacao.IdEscolaridade),
                    Cidade = _cidade.GetByNomeUF(formacao.NomeCidade),
                    InstituicaoEnsino = _instituicaoEnsino.GetByName(formacao.NomeInstituicao),
                    Curso = _curso.GetByName(formacao.NomeCurso)
                };

                _formacao.Salvar(objFormacao);

                //Mapear para banco antigo
                var commandObject = MapearFormacao(objFormacao);
                new Mapper.ToOld.PessoaFisica().MapFormacao(commandObject, formacao.IdVaga, formacao.Candidatar);

                _unitOfWork.Commit();

                //Descontar candidatura
                if (formacao.Candidatar)
                {
                    var result = GetCadidaturasDegustacao(formacao.Cpf, formacao.DataNascimento);

                    if (result != "0" && result != "VIP")
                    {
                        var curriculo = _curriculo.GetByIdPessoaFisica(pessoa.Id);
                        if (curriculo != null)
                        {
                            DescontarCandidatura(curriculo.Id);
                        }
                        _unitOfWork.Commit();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao inserir Formação candidato", JsonConvert.SerializeObject(formacao));
                return false;
            }
        }
        #endregion

        #region DescontarCandidatura
        private void DescontarCandidatura(int idCurriculo)
        {
            var objCurriculoParametro = _curriculoParametro.GetByIdCurriculo(idCurriculo);

            if (objCurriculoParametro != null)
            {
                objCurriculoParametro.Valor = (Convert.ToInt32(objCurriculoParametro.Valor) - 1).ToString();
                _curriculoParametro.Atualizar(objCurriculoParametro);
            }
        }
        #endregion

        #region PostExperienciaProfissional
        public bool PostExperienciaProfissional(Command.ExperienciaProfissional experienciaProfissional)
        {
            var baseData = DateTime.Now;
            var salvarExperiencia = experienciaProfissional.NomeEmpresa != null && experienciaProfissional.FuncaoExercida != null;

            try
            {
                var pessoa = RetornarFisicaJaExiste(experienciaProfissional.Cpf, experienciaProfissional.DataNascimento.Date);

                var objExperiencia = new Model.ExperienciaProfissional
                {
                    AtividadesExercidas = experienciaProfissional.AtividadesExercidas,
                    DataEntrada = experienciaProfissional.DataEntrada,
                    DataSaida = experienciaProfissional.DataSaida,
                    NomeEmpresa = experienciaProfissional.NomeEmpresa,
                    Ativo = true,
                    FlgImportado = false,
                    UltimoSalario = experienciaProfissional.UltimoSalario,
                    DataCadastro = baseData,
                    FuncaoExercida = experienciaProfissional.FuncaoExercida,
                    RamoAtividadeGlobal = _ramoAtividade.GetById(experienciaProfissional.IdRamoEmpresa),
                    PessoaFisica = pessoa == null ? new Model.PessoaFisica { CPF = experienciaProfissional.Cpf } : pessoa
                };

                if (pessoa.Id > 0 && salvarExperiencia)
                {
                    _experienciaProfissional.Salvar(objExperiencia);

                    //Descontar candidatura
                    if (experienciaProfissional.Candidatar)
                    {
                        var curriculo = _curriculo.GetByIdPessoaFisica(pessoa.Id);
                        if (curriculo != null)
                        {
                            DescontarCandidatura(curriculo.Id);
                        }
                    }

                    //Mapear para Banco antigo
                    var commandObject = MapearExperienciaProfissional(objExperiencia);

                    try
                    {
                        new Mapper.ToOld.PessoaFisica().MapExperienciaProfissional(commandObject, experienciaProfissional.IdVaga, salvarExperiencia, experienciaProfissional.Candidatar);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, "commandObject: ", JsonConvert.SerializeObject(commandObject.IdRamoEmpresa));
                        throw;
                    }
                }
                else
                {
                    //salvar parametro Curriculo sem experiencia
                    var commandObject = MapearExperienciaProfissional(objExperiencia);

                    try
                    {
                        new Mapper.ToOld.PessoaFisica().MapCurriculoParametro(commandObject, experienciaProfissional.IdVaga, experienciaProfissional.Candidatar);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex, "erro ao salvar curriculo parametro");
                        throw;
                    }
                }

                _unitOfWork.Commit();

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao persistir Experiencia profissional", JsonConvert.SerializeObject(experienciaProfissional));
                throw;
            }
        }
        #endregion

        #region MapearIndicacaoAmigo
        private Mapper.Models.Indicacao.Indicacao MapearIndicacaoAmigo(Indicacao objIndicacao)
        {
            AutoMapper.Mapper.CreateMap<Indicacao, Mapper.Models.Indicacao.Indicacao>();
            AutoMapper.Mapper.CreateMap<AmigoIndicado, Mapper.Models.Indicacao.AmigoIndicado>();

            var commandObjcts = AutoMapper.Mapper.Map<Indicacao, Mapper.Models.Indicacao.Indicacao>(objIndicacao);
            return commandObjcts;
        }
        #endregion

        #region MapearFormacao
        private Mapper.Models.PessoaFisica.Formacao MapearFormacao(Model.Formacao objFormacao)
        {
            try
            {
                var command = new Command.Formacao
                {
                    DataCadastro = objFormacao.DataCadastro,
                    DataAlteracao = objFormacao.DataAlteracao,
                    NomeCurso = objFormacao.NomeCurso,
                    NomeInstituicao = objFormacao.NomeInstituicao,
                    AnoConclusao = objFormacao.AnoConclusao,
                    Ativo = objFormacao.Ativo,
                    IdCidade = objFormacao.Cidade == null ? 0 : objFormacao.Cidade.Id,
                    IdInstituicaoEnsino = objFormacao.InstituicaoEnsino == null ? 0 : objFormacao.InstituicaoEnsino.Id,
                    IdCurso = objFormacao.Curso == null ? 0 : objFormacao.Curso.Id,
                    IdEscolaridade = objFormacao.EscolaridadeGlobal.Id,
                    IdPessoa = objFormacao.PessoaFisica.Id,
                    Cpf = objFormacao.PessoaFisica.CPF
                };

                AutoMapper.Mapper.CreateMap<Command.Formacao, Mapper.Models.PessoaFisica.Formacao>();

                var commandObject = AutoMapper.Mapper.Map<Command.Formacao, Mapper.Models.PessoaFisica.Formacao>(command);

                return commandObject;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro no mapeamento de Experiência profissional NEWBNE to OLD");
                throw;
            }
        }
        #endregion

        #region MapearExperienciaProfissional
        private Mapper.Models.PessoaFisica.ExperienciaProfissional MapearExperienciaProfissional(Model.ExperienciaProfissional objExperiencia)
        {
            try
            {
                var command = new Command.ExperienciaProfissional
                {
                    DataEntrada = objExperiencia.DataEntrada != null ? objExperiencia.DataEntrada.Value : DateTime.Now,
                    DataSaida = objExperiencia.DataSaida,
                    DataCadastro = objExperiencia.DataCadastro,
                    AtividadesExercidas = objExperiencia.AtividadesExercidas,
                    FlgImportado = false,
                    UltimoSalario = objExperiencia.UltimoSalario,
                    NomeEmpresa = objExperiencia.NomeEmpresa,
                    FuncaoExercida = objExperiencia.FuncaoExercida,
                    Ativo = objExperiencia.Ativo,
                    Cpf = objExperiencia.PessoaFisica.CPF,
                    //IdPessoa = objExperiencia.PessoaFisica.Id,
                    IdRamoEmpresa = objExperiencia.RamoAtividadeGlobal != null ? objExperiencia.RamoAtividadeGlobal.Id : 0
                };

                AutoMapper.Mapper.CreateMap<Command.ExperienciaProfissional, Mapper.Models.PessoaFisica.ExperienciaProfissional>();

                var commandObject = AutoMapper.Mapper.Map<Command.ExperienciaProfissional, Mapper.Models.PessoaFisica.ExperienciaProfissional>(command);

                return commandObject;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro no mapeamento de Experiência profissional NEWBNE to OLD");
                throw;
            }
        }
        #endregion

        #region MapearNovoToAntigo
        private Mapper.Models.PessoaFisica.PessoaFisica MapearNovoToAntigo(Model.Curriculo objCurriculo, DateTime baseData,
            Model.CurriculoOrigem objCurriculoOrigem,
            Model.FuncaoPretendida objFuncaoPretendida,
            Model.PessoaFisica objPessoaFisica,
            Model.CurriculoParametro objCurriculoParametro,
            string email, int idVaga, byte dddCelular, decimal celular)
        {
            try
            {
                var commandCV = new Command.Curriculo
                {
                    FlgVIP = objCurriculo.FlgVIP,
                    FlgInativo = objCurriculo.Ativo,
                    FlgDisponivelViagem = objCurriculo.FlgDisponivelViagem,
                    PretensaoSalarial = objCurriculo.PretensaoSalarial,
                    Observacao = objCurriculo.Observacao,
                    Conhecimento = objCurriculo.Conhecimento,
                    IdTipoCurriculo = objCurriculo.TipoCurriculo.Id,
                    IdSituacaoCurriculo = objCurriculo.SituacaoCurriculo.Id,
                    DataCadastro = baseData,
                    DataAtualizacao = objCurriculo.DataAtualizacao,
                    DataModificacao = objCurriculo.DataModificacao,
                    IdOrigem = objCurriculoOrigem.OrigemGlobal.Id
                };

                var cmdFuncaoPretendida = new Command.FuncaoPretendida
                {
                    TempoExperiencia = objFuncaoPretendida.TempoExperiencia,
                    Descricao = objFuncaoPretendida.Descricao,
                    idFuncao = objFuncaoPretendida.IdFuncao
                };

                var command = new CriarAtualizarPessoaFisica
                {
                    CPF = objPessoaFisica.CPF,
                    Nome = objPessoaFisica.Nome,
                    Email = email,
                    DataNascimento = objPessoaFisica.DataNascimento,
                    DataCadastro = baseData,
                    DDDCelular = dddCelular,
                    Celular = celular,
                    IdVaga = idVaga,
                    IdSexo = objPessoaFisica.Sexo != null ? objPessoaFisica.Sexo.Sigla == "M" ? 1 : 2 : 0,
                    IdCidade = objPessoaFisica.Cidade.Id,
                    IdEscolaridade = objPessoaFisica.EscolaridadeGlobal != null ? objPessoaFisica.EscolaridadeGlobal.Id : 0,
                    IdDeficiencia = objPessoaFisica.DeficienciaGlobal != null ? objPessoaFisica.DeficienciaGlobal.Id : 0
                };

                var cmdCurriculoParamtro = new Command.CurriculoParametro
                {
                    idParametro = objCurriculoParametro.Parametro.Id,
                    Valor = objCurriculoParametro.Valor
                };

                //Criar o Map
                AutoMapper.Mapper.CreateMap<CriarAtualizarPessoaFisica, Mapper.Models.PessoaFisica.PessoaFisica>();
                AutoMapper.Mapper.CreateMap<Command.Curriculo, Mapper.Models.PessoaFisica.Curriculo>();
                AutoMapper.Mapper.CreateMap<Command.FuncaoPretendida, Mapper.Models.PessoaFisica.FuncaoPretendida>();
                AutoMapper.Mapper.CreateMap<Command.CurriculoParametro, Mapper.Models.PessoaFisica.CurriculoParametro>();


                //inserir objeto Curriculo na PessoaFisica
                command.Curriculo = commandCV;
                command.Curriculo.FuncaoPretendida = cmdFuncaoPretendida;
                command.Curriculo.CurriculoParametro = cmdCurriculoParamtro;


                //Consumir o Map
                var commandObject = AutoMapper.Mapper.Map<CriarAtualizarPessoaFisica, Mapper.Models.PessoaFisica.PessoaFisica>(command);

                return commandObject;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro no mapeamento NEWBNE to OLD");
                throw;
            }
        }
        #endregion

        #region GerarHashAcesso
        public string GerarHashAcessoLoginAutomatico(decimal cpf, DateTime dataNascimento, string urlDestino)
        {
            if (urlDestino == null)
            {
                urlDestino = string.Empty;
            }

            var parametros = new
            {
                NumeroCPF = cpf,
                DataNascimento = dataNascimento,
                Url = urlDestino
            };

            var json = Utils.ToJSON(parametros);
            return Utils.ToBase64(json);
        }
        #endregion
    }
}