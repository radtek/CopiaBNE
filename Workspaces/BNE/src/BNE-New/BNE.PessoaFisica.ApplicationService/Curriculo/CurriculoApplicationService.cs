using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AutoMapper;
using SharedKernel.Repositories.Contracts;
using BNE.Data.Services.Interfaces;
using BNE.Global.Data.Repositories;
using BNE.PessoaFisica.ApplicationService.CodigoConfirmacaoEmail.Interface;
using BNE.PessoaFisica.ApplicationService.Curriculo.Command;
using BNE.PessoaFisica.ApplicationService.Curriculo.Interface;
using BNE.PessoaFisica.ApplicationService.Curriculo.Model;
using BNE.PessoaFisica.Domain.Command;
using BNE.PessoaFisica.Domain.Enumeradores;
using BNE.PessoaFisica.Domain.Model;
using BNE.PessoaFisica.Domain.Repositories;
using SharedKernel.DomainEvents.Assertation;
using SharedKernel.DomainEvents.Core;
using SharedKernel.Repositories.Contracts;
using ExperienciaProfissionalResponse = BNE.PessoaFisica.ApplicationService.Curriculo.Model.ExperienciaProfissionalResponse;
using FormacaoResponse = BNE.PessoaFisica.ApplicationService.Curriculo.Model.FormacaoResponse;
using SalvarExperienciaProfissionalCommand = BNE.PessoaFisica.ApplicationService.Curriculo.Command.SalvarExperienciaProfissionalCommand;
using SalvarFormacaoCommand = BNE.PessoaFisica.ApplicationService.Curriculo.Command.SalvarFormacaoCommand;
using SituacaoCurriculo = BNE.PessoaFisica.Domain.Enumeradores.SituacaoCurriculo;
using TipoCurriculo = BNE.PessoaFisica.Domain.Enumeradores.TipoCurriculo;

namespace BNE.PessoaFisica.ApplicationService.Curriculo
{
    public class CurriculoApplicationService : SharedKernel.ApplicationService.ApplicationService, ICurriculoApplicationService
    {
        private readonly ICidadeRepository _cidadeRepository;
        private readonly ICurriculoOrigemRepository _curriculoOrigemRepository;
        private readonly ICurriculoParametroRepository _curriculoParametroRepository;

        private readonly ICurriculoRepository _curriculoRepository;
        private readonly ICursoRepository _cursoRepository;
        private readonly IDeficienciaRepository _deficienciaRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly IEscolaridadeRepository _escolaridadeRepository;
        private readonly IExperienciaProfissionalRepository _experienciaProfissionalRepository;
        private readonly IFormacaoRepository _formacaoRepository;
        private readonly IFuncaoPretendidaRepository _funcaoPretendidaRepository;
        private readonly IFuncaoSinonimoRepository _funcaoSinonimoRepository;
        private readonly IInstituicaoEnsinoRepository _instituicaoEnsinoRepository;

        private readonly IMapper _mapper;
        private readonly IOrigemRepository _origemRepository;
        private readonly IParametroRepository _parametroRepository;
        private readonly IPessoaFisicaRepository _pessoaFisicaRepository;
        private readonly IPreCurriculoRepository _preCurriculoRepository;
        private readonly IRamoAtividadeRepository _ramoAtividadeRepository;
        private readonly ISexoRepository _sexoRepository;
        private readonly ISituacaoCurriculoRepository _situacaoCurriculoRepository;

        private readonly TelefoneCelular _telefoneCelular;
        private readonly ITipoCurriculoRepository _tipoCurriculoRepository;

        private readonly IIdentityServerService _identityServerService;

        public CurriculoApplicationService(ICurriculoRepository curriculoRepository,
            IMapper mapper, IPessoaFisicaRepository pessoaFisicaRepository,
            ICurriculoParametroRepository curriculoParametroRepository, IEscolaridadeRepository escolaridadeRepository,
            IRamoAtividadeRepository ramoAtividadeRepository, IInstituicaoEnsinoRepository instituicaoEnsinoRepository,
            ICidadeRepository cidadeRepository, ICursoRepository cursoRepository, IFormacaoRepository formacaoRepository,
            IExperienciaProfissionalRepository experienciaProfissionalRepository, ISituacaoCurriculoRepository situacaoCurriculoRepository,
            ITipoCurriculoRepository tipoCurriculoRepository, IOrigemRepository origemRepository, ISexoRepository sexoRepository,
            IDeficienciaRepository deficienciaRepository, IEmailRepository emailRepository, TelefoneCelular telefoneCelular,
            IFuncaoPretendidaRepository funcaoPretendidaRepository, IFuncaoSinonimoRepository funcaoSinonimoRepository,
            ICurriculoOrigemRepository curriculoOrigemRepository, IParametroRepository parametroRepository,
            ICodigoConfirmacaoEmailApplicationService codigoConfirmacaoEmailApplicationService, IPreCurriculoRepository preCurriculoRepository,
            IIdentityServerService identityServerService, IUnitOfWork unitOfWork,
            EventPoolHandler<AssertError> assertEventPool,
            IBus bus) : base(unitOfWork, assertEventPool, bus)
        {
            _curriculoRepository = curriculoRepository;
            _mapper = mapper;
            _pessoaFisicaRepository = pessoaFisicaRepository;
            _curriculoParametroRepository = curriculoParametroRepository;
            _escolaridadeRepository = escolaridadeRepository;
            _ramoAtividadeRepository = ramoAtividadeRepository;
            _instituicaoEnsinoRepository = instituicaoEnsinoRepository;
            _cidadeRepository = cidadeRepository;
            _cursoRepository = cursoRepository;
            _formacaoRepository = formacaoRepository;
            _experienciaProfissionalRepository = experienciaProfissionalRepository;
            _situacaoCurriculoRepository = situacaoCurriculoRepository;
            _tipoCurriculoRepository = tipoCurriculoRepository;
            _origemRepository = origemRepository;
            _sexoRepository = sexoRepository;
            _deficienciaRepository = deficienciaRepository;
            _emailRepository = emailRepository;
            _funcaoPretendidaRepository = funcaoPretendidaRepository;
            _funcaoSinonimoRepository = funcaoSinonimoRepository;
            _curriculoOrigemRepository = curriculoOrigemRepository;
            _parametroRepository = parametroRepository;

            _preCurriculoRepository = preCurriculoRepository;
            _identityServerService = identityServerService;

            _telefoneCelular = telefoneCelular;
        }

        public InformacaoCurriculoResponse CarregarInformacoesCurriculo(RecuperarInformacaoCurriculoCommand command)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var objInformacaoCurriculo = _curriculoRepository.GetInformacaoCurriculo(command.CPF, command.IdVaga);

            var stopWatchMapeamento = new Stopwatch();
            var map = _mapper.Map<Domain.Aggregates.InformacaoCurriculo, InformacaoCurriculoResponse>(objInformacaoCurriculo);
            stopWatchMapeamento.Stop();

            stopWatch.Stop();

            return map;
        }

        public CandidaturaDegustacao CarregarCadidaturasDegustacao(RecuperarCandidaturaDegustacaoCommand command)
        {
            var objCandidaturaDegustacao = new CandidaturaDegustacao();

            var objPessoa = _pessoaFisicaRepository.Get(command.CPF, command.Data);

            if (objPessoa != null)
            {
                var idCurriculo = _curriculoRepository.GetIdCurriculo(command.CPF);
                var result = _curriculoRepository.VIP(idCurriculo);

                if (result)
                    objCandidaturaDegustacao.CurriculoVIP = true;
                else
                    objCandidaturaDegustacao.QuantidadeCandidatura = _curriculoRepository.SaldoCandidatura(idCurriculo);
            }

            return objCandidaturaDegustacao;
        }

        public CandidaturaResponse CandidatarCurriculo(Command.SalvarCurriculoCommand command)
        {
            var candidatarPreCurriculoCommand = _mapper.Map<Command.SalvarCurriculoCommand, SalvarCandidatarCurriculoCommand>(command);

            #region perguntas
            var lista = new List<SalvarCandidatarPreCurriculoSalvarPerguntaCommand>();

            if (command.Vaga.Perguntas != null)
            {
                lista.AddRange(command.Vaga.Perguntas.Select(pergunta => _mapper.Map<Pergunta, SalvarCandidatarPreCurriculoSalvarPerguntaCommand>(pergunta)));
            }
            candidatarPreCurriculoCommand.Perguntas = lista;
            #endregion

            return _mapper.Map<Domain.Aggregates.Candidatura, CandidaturaResponse>(Candidatar(candidatarPreCurriculoCommand));
        }

        public ExperienciaProfissionalResponse SalvarExperienciaProfissional(SalvarExperienciaProfissionalCommand command)
        {
            var experienciaProfissionalCommand = _mapper.Map<SalvarExperienciaProfissionalCommand, Domain.Command.SalvarExperienciaProfissionalCommand>(command);

            var objPessoa = _pessoaFisicaRepository.Get(experienciaProfissionalCommand.CPF, experienciaProfissionalCommand.DataNascimento);
            var nome = _pessoaFisicaRepository.GetNomeJaExiste(experienciaProfissionalCommand.CPF, experienciaProfissionalCommand.DataNascimento);

            if (objPessoa != null || !string.IsNullOrEmpty(nome))
            {
                var baseData = DateTime.Now;
                experienciaProfissionalCommand.DataCadastro = baseData;
                experienciaProfissionalCommand.Ativo = true;

                var salvarExperiencia = !string.IsNullOrWhiteSpace(experienciaProfissionalCommand.NomeEmpresa) && !string.IsNullOrWhiteSpace(experienciaProfissionalCommand.FuncaoExercida);

                if (salvarExperiencia)
                {
                    if (objPessoa != null && objPessoa.Id > 0) //Se tem pessoa física no novo
                    {
                        var objExperiencia = new Domain.Model.ExperienciaProfissional
                        {
                            PessoaFisica = objPessoa,
                            AtividadesExercidas = experienciaProfissionalCommand.AtividadesExercidas,
                            DataEntrada = experienciaProfissionalCommand.DataEntrada,
                            DataSaida = experienciaProfissionalCommand.DataSaida,
                            NomeEmpresa = experienciaProfissionalCommand.NomeEmpresa,
                            Ativo = true,
                            FlgImportado = false,
                            UltimoSalario = experienciaProfissionalCommand.UltimoSalario,
                            DataCadastro = baseData,
                            FuncaoExercida = experienciaProfissionalCommand.FuncaoExercida,
                            RamoAtividadeGlobal = _ramoAtividadeRepository.GetById(experienciaProfissionalCommand.IdRamoEmpresa)
                        };

                        _experienciaProfissionalRepository.Add(objExperiencia);

                        //Descontar candidatura
                        if (experienciaProfissionalCommand.Candidatar)
                        {
                            var curriculo = _curriculoRepository.GetIdCurriculo(experienciaProfissionalCommand.CPF);
                            if (curriculo > 0 && !_curriculoRepository.VIP(curriculo))
                            {
                                DescontarCandidatura(curriculo);
                            }
                        }
                    }

                    _curriculoRepository.MapExperienciaProfissional(experienciaProfissionalCommand, salvarExperiencia);
                }
                else
                {
                    _curriculoRepository.MapCurriculoParametro(experienciaProfissionalCommand);
                }

                if (Commit())
                {
                    return new ExperienciaProfissionalResponse(nome, experienciaProfissionalCommand.CPF, experienciaProfissionalCommand.DataNascimento.Date, experienciaProfissionalCommand.UrlPesquisa);
                }
            }
            return null;
        }

        public FormacaoResponse SalvarFormacao(SalvarFormacaoCommand command)
        {
            var formacaoCommand = _mapper.Map<SalvarFormacaoCommand, Domain.Command.SalvarFormacaoCommand>(command);

            var objPessoa = _pessoaFisicaRepository.Get(formacaoCommand.CPF, formacaoCommand.DataNascimento);
            var nome = _pessoaFisicaRepository.GetNomeJaExiste(formacaoCommand.CPF, formacaoCommand.DataNascimento);

            if (objPessoa != null || !string.IsNullOrEmpty(nome))
            {
                var baseData = DateTime.Now;
                formacaoCommand.DataCadastro = baseData;
                formacaoCommand.DataAlteracao = baseData;
                formacaoCommand.Ativo = true;

                if (objPessoa != null && objPessoa.Id > 0) //Se tem pessoa física no novo
                {
                    var objFormacao = new Domain.Model.Formacao
                    {
                        PessoaFisica = objPessoa,
                        AnoConclusao = formacaoCommand.AnoConclusao,
                        CargaHoraria = formacaoCommand.CargaHoraria,
                        NomeInstituicao = formacaoCommand.NomeInstituicao,
                        NomeCurso = formacaoCommand.NomeCurso,
                        DataCadastro = baseData,
                        DataAlteracao = baseData,
                        Ativo = true,
                        EscolaridadeGlobal = _escolaridadeRepository.GetById(formacaoCommand.IdEscolaridade),
                        Cidade = _cidadeRepository.GetByNomeUF(formacaoCommand.NomeCidade),
                        InstituicaoEnsino = _instituicaoEnsinoRepository.GetByNome(formacaoCommand.NomeInstituicao),
                        Curso = _cursoRepository.GetByDescricao(formacaoCommand.NomeCurso)
                    };

                    _formacaoRepository.Add(objFormacao);
                }
                //Mapear para banco antigo
                _curriculoRepository.MapFormacao(formacaoCommand);

                //Descontar candidatura
                if (command.Candidatar)
                {
                    var curriculo = _curriculoRepository.GetIdCurriculo(formacaoCommand.CPF);
                    if (curriculo > 0 && !_curriculoRepository.VIP(curriculo))
                    {
                        DescontarCandidatura(curriculo);
                    }
                }

                if (Commit())
                {
                    return new FormacaoResponse(nome, formacaoCommand.CPF, formacaoCommand.DataNascimento.Date, formacaoCommand.UrlPesquisa);
                }
            }
            return null;
        }

        public CadastroCurriculoResponse Cadastrar(Command.SalvarCurriculoCommand command)
        {
            var domainCommand = _mapper.Map<Command.SalvarCurriculoCommand, Domain.Command.SalvarCurriculoCommand>(command);

            var objPessoaFisica = _pessoaFisicaRepository.Get(domainCommand.CPF, domainCommand.DataNascimento);
            if (objPessoaFisica != null)
            {
                var candidaturaCommand = _mapper.Map<Command.SalvarCurriculoCommand, SalvarCandidatarCurriculoCommand>(command);
                var candidaturaResponse = Candidatar(candidaturaCommand);

                return new CadastroCurriculoResponse { Candidatou = candidaturaResponse.Candidatou, CPF = objPessoaFisica.CPF, DataNascimento = objPessoaFisica.DataNascimento, Nome = objPessoaFisica.Nome };
            }
            var baseData = DateTime.Now;

            var tipoCurriculo = _tipoCurriculoRepository.GetById((int)TipoCurriculo.Mini);
            var situacaoCurriculo = _situacaoCurriculoRepository.GetById((int)SituacaoCurriculo.Publicado);
            //TODO: Charan => pegar origem conforme o STC ou parceiro BNE
            var origem = _origemRepository.GetById((int)Origem.BNE);

            #region PessoaFisica
            if (objPessoaFisica == null)
            {
                objPessoaFisica = new Domain.Model.PessoaFisica
                {
                    DataCadastro = baseData
                };
            }

            objPessoaFisica.Nome = domainCommand.Nome;
            objPessoaFisica.CPF = domainCommand.CPF;
            objPessoaFisica.DataNascimento = domainCommand.DataNascimento;
            objPessoaFisica.Sexo = _sexoRepository.GetByChar(domainCommand.Sexo);
            objPessoaFisica.DeficienciaGlobal = domainCommand.IdDeficiencia != null ? _deficienciaRepository.GetById(domainCommand.IdDeficiencia.Value) : null;
            objPessoaFisica.DataAlteracao = baseData;
            objPessoaFisica.EscolaridadeGlobal = domainCommand.IdEscolaridade > 0 ? _escolaridadeRepository.GetById(domainCommand.IdEscolaridade) : null;
            objPessoaFisica.FlgWhatsApp = domainCommand.FlgWhatsApp;
            #region Cidade
            var objCidade = _cidadeRepository.GetByNomeUF(domainCommand.Cidade);
            objPessoaFisica.Cidade = objCidade;
            #endregion

            var objEmailPessoaFisica = new Domain.Model.Email();

            if (!string.IsNullOrWhiteSpace(domainCommand.Email))
            {
                objEmailPessoaFisica.Endereco = domainCommand.Email;
                objEmailPessoaFisica.PessoaFisica = objPessoaFisica;
                objEmailPessoaFisica.DataCadastro = baseData;

                _pessoaFisicaRepository.Add(objPessoaFisica);
                _emailRepository.Add(objEmailPessoaFisica);
            }

            #region Telefone
            //var dddCelular = command.Celular; byte.Parse(pessoaFisica.Celular.Replace("(", "").Replace(")", "").Replace("-", "").Substring(0, 2));
            //var celular = decimal.Parse(pessoaFisica.Celular.Replace("(", "").Replace(")", "").Replace("-", "").Substring(3));

            _telefoneCelular.Salvar(objPessoaFisica, domainCommand.DDDCelular, domainCommand.NumeroCelular);
            #endregion

            #endregion

            #region Curriculo
            var objCurriculo = new Domain.Model.Curriculo
            {
                PessoaFisica = objPessoaFisica,
                FlgVIP = false,
                Ativo = false,
                FlgDisponivelViagem = false,
                TipoCurriculo = tipoCurriculo,
                SituacaoCurriculo = situacaoCurriculo,
                PretensaoSalarial = domainCommand.PretensaoSalarial,
                DataCadastro = baseData
            };

            _curriculoRepository.Add(objCurriculo);
            #endregion

            #region Funcao Pretendida
            short tempoExperiencia = 0;

            if (domainCommand.TempoExperienciaAnos != null && domainCommand.TempoExperienciaMeses != null)
                tempoExperiencia = short.Parse(((domainCommand.TempoExperienciaAnos * 12) + domainCommand.TempoExperienciaMeses).ToString());

            //TODO Charan => verificar as funções, por ex.: Desenvolvedor PHP na retorna nessa consulta
            var objFuncao = _funcaoSinonimoRepository.GetByNome(domainCommand.DescricaoFuncao);

            var objFuncaoPretendida = new FuncaoPretendida
            {
                DataCadastro = baseData,
                IdFuncao = objFuncao.Id,
                Curriculo = objCurriculo,
                Descricao = domainCommand.DescricaoFuncao,
                TempoExperiencia = tempoExperiencia
            };

            _funcaoPretendidaRepository.Add(objFuncaoPretendida);
            #endregion

            #region CurriculoOrigem
            var objCurriculoOrigem = new CurriculoOrigem
            {
                DataCadastro = baseData,
                Curriculo = objCurriculo,
                OrigemGlobal = origem
            };

            _curriculoOrigemRepository.Add(objCurriculoOrigem);
            #endregion

            #region CurriculoParametro
            var objParametro = _parametroRepository.GetById((int)Domain.Enumeradores.Parametro.QuantidadeCandidaturaDegustacao);
            var objCurriculoParametro = new CurriculoParametro
            {
                Curriculo = objCurriculo,
                DataCadastro = baseData,
                Ativo = true,
                Valor = objParametro.Valor,
                Parametro = objParametro
            };

            _curriculoParametroRepository.Add(objCurriculoParametro);
            #endregion

            //Ajustando commando para mapear para o velho
            domainCommand.TempoExperiencia = tempoExperiencia;
            domainCommand.IdSituacaoCurriculo = situacaoCurriculo.Id;
            domainCommand.IdTipoCurriculo = tipoCurriculo.Id;
            domainCommand.IdOrigem = origem.Id;
            domainCommand.DataCadastro = baseData;
            domainCommand.DataAlteracao = baseData;
            bool candidatura;

            _curriculoRepository.Map(domainCommand, out candidatura);

            if (Commit())
            {
                var _objPrecurriculo = _preCurriculoRepository.GetById(domainCommand.Id);
                if (_objPrecurriculo != null)
                {
                    _objPrecurriculo.IdCurriculo = objCurriculo.Id;
                    _preCurriculoRepository.Update(_objPrecurriculo);
                }

                _identityServerService.CreateUserAccount(objPessoaFisica.Nome, objPessoaFisica.CPF, objPessoaFisica.DataNascimento);

                return new CadastroCurriculoResponse { Candidatou = candidatura, CPF = objPessoaFisica.CPF, DataNascimento = objPessoaFisica.DataNascimento, Nome = objPessoaFisica.Nome };
            }

            return null;
        }

        #region Efetuar a candidatura
        /// <summary>
        ///     Efetuar a candidatura e retorna o nome do candidato
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public Domain.Aggregates.Candidatura Candidatar(SalvarCandidatarCurriculoCommand command)
        {
            var cpf = command.CPF;
            var dataNascimento = command.DataNascimento;

            var nome = _pessoaFisicaRepository.GetNomeJaExiste(cpf, dataNascimento);

            if (string.IsNullOrWhiteSpace(nome) || _pessoaFisicaRepository.Inativa(cpf))
            {
                return new Domain.Aggregates.Candidatura(string.Empty, cpf, dataNascimento, command.UrlPesquisa, false, true);
            }

            var list = new List<Tuple<int, bool?, string>>();

            if (command.Perguntas != null)
            {
                list.AddRange(command.Perguntas.Select(pergunta => new Tuple<int, bool?, string>(pergunta.IdVagaPergunta, pergunta.FlgRespostaPergunta, pergunta.Resposta)));
            }

            var result = _curriculoRepository.SalvarCandidatura(cpf, command.IdVaga, false, list);

            if (result)
            {
                var curriculo = _curriculoRepository.GetIdCurriculo(cpf);
                if (curriculo > 0 && !_curriculoRepository.VIP(curriculo)) //Só desconta se não for vip
                {
                    DescontarCandidatura(curriculo);
                }
                if (!Commit())
                {
                    return null;
                }
            }

            return new Domain.Aggregates.Candidatura(nome, cpf, dataNascimento, command.UrlPesquisa, result, false);
        }
        #endregion

        #region DescontarCandidatura
        private void DescontarCandidatura(int idCurriculo)
        {
            var objCurriculoParametro = _curriculoParametroRepository.GetByIdCurriculo(idCurriculo);

            if (objCurriculoParametro != null)
            {
                objCurriculoParametro.Descontar();
                _curriculoParametroRepository.Update(objCurriculoParametro);
            }
        }
        #endregion
    }
}